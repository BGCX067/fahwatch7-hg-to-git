'/*

'  ucHWM
'  Version: MPL 1.1/GPL 2.0/LGPL 2.1

'  The contents of this file are subject to the Mozilla Public License Version
'  1.1 (the "License"); you may not use this file except in compliance with
'  the License. You may obtain a copy of the License at

'  http://www.mozilla.org/MPL/

'  Software distributed under the License is distributed on an "AS IS" basis,
'  WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
'  for the specific language governing rights and limitations under the License.

'  The Original Code is the cftUnity.nl source code.

'  The Initial Developer of the Original Code is 
'   Marvin Westmaas/ Marvin_The_Martian / MtM ( webmaster@cftunity.nl )
'  Portions created by the Initial Developer are Copyright (C) 2010-2011
'  the Initial Developer. All Rights Reserved.

'  The repository for cftUnity.nl is hosted at
'  http://code.google.com/p/cftunity/

'  Contributor(s):

'  Alternatively, the contents of this file may be used under the terms of
'  either the GNU General Public License Version 2 or later (the "GPL"), or
'  the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
'  in which case the provisions of the GPL or the LGPL are applicable instead
'  of those above. If you wish to allow use of your version of this file only
'  under the terms of either the GPL or the LGPL, and not to allow others to
'  use your version of this file under the terms of the MPL, indicate your
'  decision by deleting the provisions above and replace them with the notice
'  and other provisions required by the GPL or the LGPL. If you do not delete
'  the provisions above, a recipient may use your version of this file under
'  the terms of any one of the MPL, the GPL or the LGPL.

'*/
Imports OpenHardwareMonitor.Hardware
Imports OpenHardwareMonitor
Imports cftUnity.Settings
Imports System.Drawing.Drawing2D
Imports cftUnity.Settings.cftunity

Public Class ucHWM
    Private myHW As OpenHardwareMonitor.Hardware.IHardware
    Public ReadOnly Property Identifier As String
        Get
            Return myHW.Identifier.ToString
        End Get
    End Property
    Private WithEvents lblName As New Label
#Region "AutoUpdate"
    Private alGauges As New ArrayList, alClocks As New ArrayList
    Private sEventHandler As SensorEventHandler
    Private Delegate Sub delEventHandler()
    Private iVisitor As OpenHardwareMonitor.Hardware.IVisitor
    Private iHandler As New SensorEventHandler(AddressOf SensorEvenentHandler)
    Private WithEvents autoTimer As System.Timers.Timer
    Public Function AutoUpdate(ByVal Interval As Integer) As Boolean
        Try
            If IsNothing(autoTimer) Then autoTimer = New System.Timers.Timer
            If Interval = 0 Then
                autoTimer.Enabled = False
                autoTimer.Dispose()
                autoTimer = Nothing
                Return True
            End If
            For Each sGauge In alGauges
                With CType(sGauge, mtmGauge)
                    .ResetMaxMin()
                    .Value = .mySensor.Value
                    .bDoGradients = True
                    .Invalidate()
                End With
            Next
            autoTimer.AutoReset = True
            autoTimer.Interval = Interval
            autoTimer.Enabled = True
            Return True
        Catch ex As Exception
            LogWindow.WriteError("ucHWM_Autoupdate", Err)
            Return False
        End Try
    End Function
    Private Sub autoTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles autoTimer.Elapsed
        Try
            If Not Me.Created OrElse bClosing Then Exit Sub
            Dim nInv As New delUpdate(AddressOf doUpdate)
            Me.Invoke(nInv)
            nInv = Nothing
        Catch ex As Exception
            LogWindow.WriteError("ucHWM_AutotimerElapsed", Err)
        End Try
    End Sub
    Private Delegate Sub delUpdate()
    Private Sub doUpdate()
        Try
            If bClosing Then Exit Sub
            myHW.Accept(iVisitor)
            'myHW.Update()
            iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
            myHW.Accept(iVisitor)
        Catch ex As Exception
            LogWindow.WriteError("ucHWM_doUpdate", Err)
        End Try

    End Sub
    Public Sub SensorEvenentHandler(ByVal Sensor As ISensor)
        Try
            If bClosing Then Exit Sub
            For Each mGauge In alGauges
                With CType(mGauge, mtmGauge)
                    If ReferenceEquals(Sensor, .mySensor) Then
                        ' If Sensor.Values.Count >= 3 Then
                        'do average of last 2 readings IF timedifference is less then 1s
                        'Dim iTotal As Int32, iValues As Int32 = 0
                        'For xInt As Int32 = Sensor.Values.Count To Sensor.Values.Count - 3 Step -1
                        '    If DateTime.Now.Subtract(Sensor.Values(xInt).Time).TotalSeconds < 1 Then
                        '        iTotal += Sensor.Values(xInt).Value
                        '        iValues += 1
                        '    End If
                        'Next
                        'If iTotal = 0 And iValues = 0 Then
                        '    .Value = 0
                        'Else
                        '    .Value = iTotal / iValues
                        'End If
                        'Else
                        .Value = Sensor.Value
                        'End If
                        Return
                    End If
                End With
            Next
            If myHWSettings.ClocksAsGauges OrElse myHWSettings.HideClocks Then Return
            For Each lblClock In Me.Controls
                If TypeOf lblClock Is Label Then
                    If Not IsNothing(lblClock.tag) AndAlso Not IsNothing(Sensor.Value) And lblClock.Tag = Sensor.Identifier.ToString Then
                        With CType(lblClock, Label)
                            If .Text.Contains(Sensor.Name.Replace("CPU ", "").Replace("GPU ", "").Replace("#", "")) Then
                                If lblClock.Text <> Sensor.Name.Replace("CPU ", "").Replace("GPU ", "").Replace("#", "") & " " & CInt(Sensor.Value).ToString & " Mhz" Then
                                    lblClock.Text = Sensor.Name.Replace("CPU ", "").Replace("GPU ", "").Replace("#", "") & " " & CInt(Sensor.Value).ToString & " Mhz"
                                End If
                                Return
                            Else
                                If .Text <> CInt(Sensor.Value.ToString).ToString & "Mhz" Then
                                    .Text = CInt(Sensor.Value.ToString).ToString & "Mhz"
                                End If
                            End If
                        End With
                    End If
                End If
            Next
        Catch ex As Exception
            LogWindow.WriteError("ucHWM_SensorEventHandler", Err)
        End Try
    End Sub
#End Region
    Private myHWSettings As clsSettings.sSettings.sHWsettings
    Private Enum eLayout
        A
        B
    End Enum
    Private myLayout As eLayout = eLayout.A
    Private myGPath As New GraphicsPath
    Private WithEvents lblOptions As New Label
    Public Function AttachHW(ByVal Hardware As OpenHardwareMonitor.Hardware.IHardware) As Boolean
        Try
            Dim intSummary As Int32 = 0, intMywidth As Int16 = 0
            Dim lT As New Label, s As New Size, g As Graphics
            g = lT.CreateGraphics
            intSummary = g.MeasureString(Hardware.Name, lT.Font).ToSize.Height
            myHW = Hardware
            Me.Controls.Clear()
            alGauges.Clear()
            alClocks.Clear()
            If MySettings.MySettings.HasHWSettings(myHW.Identifier.ToString) Then
                myHWSettings = MySettings.MySettings.HWSettings(myHW.Identifier.ToString)
            Else
                myHWSettings = New clsSettings.sSettings.sHWsettings
                myHWSettings.ClocksAsGauges = False
                myHWSettings.HideClocks = False
                myHWSettings.Identifier = myHW.Identifier.ToString
            End If

            With myHW
                If .HardwareType = HardwareType.CPU Then
                    Dim alLoad As New ArrayList
                    Dim alTemp As New ArrayList
                    Dim alClock As New ArrayList
                    With Hardware
                        Dim txtReport As String = .GetReport
                        Dim CoreCount As Int32
                        Dim txtTmp As String = txtReport.Substring(txtReport.IndexOf("Number of Cores: "), txtReport.IndexOf(vbNewLine, txtReport.IndexOf("Number of Cores: ")) - txtReport.IndexOf("Number of Cores: ")).Replace("Number of Cores: ", "")
                        CoreCount = CInt(txtTmp)
                        txtTmp = Nothing
                        If CoreCount > 4 Then
                            myLayout = eLayout.A
                        Else
                            myLayout = eLayout.B
                        End If

                        Dim mWidth As Int32 = (CoreCount) * 27
                        Dim lblName As New Label
                        AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
                        AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
                        AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
                        AddHandler lblName.Paint, AddressOf lblName_Paint
                        With lblName
                            .Text = myHW.Name
                            .Left = 0
                            Dim nF As New Font("Cambria", 8, FontStyle.Bold)
                            .Font = nF
                            .AutoSize = False
                            .Width = mWidth - 10
                            .TextAlign = ContentAlignment.MiddleCenter
                            .AutoEllipsis = True
                            .BackColor = Color.Transparent
                            .ForeColor = Color.White
                            g = lblName.CreateGraphics
                            s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
                            If s.Width <= 0 Or s.Height <= 0 Then
                                myLayout = eLayout.B
                            End If
                            .Refresh()
                            .Top = 0
                        End With
                        Me.Controls.Add(lblName)
                        intSummary = lblName.Top + lblName.Height + 5
                        'group sensors
                        For Each sSensor As ISensor In myHW.Sensors
                            If sSensor.SensorType = SensorType.Clock Then
                                alClocks.Add(sSensor)
                            ElseIf sSensor.SensorType = SensorType.Load Then
                                alLoad.Add(sSensor)
                            ElseIf sSensor.SensorType = SensorType.Temperature Then
                                alTemp.Add(sSensor)
                            ElseIf sSensor.SensorType = SensorType.Voltage Then
                                'Dosomething

                            End If
                        Next
                        Dim cpuloadGauge(0 To CoreCount) As mtmGauge
                        Dim cpuTempGauge(0 To CoreCount) As mtmGauge
                        Dim cpuClockGauge(0 To CoreCount) As mtmGauge
                        For xInt As Integer = 0 To CoreCount - 1 'Last in array = average 
                            'Group sensors for each core
                            'load/fan/temp
                            If myHWSettings.ClocksAsGauges Then
                                cpuClockGauge(xInt) = DefaultGaugeSettings(CType(alClocks(xInt), ISensor))
                                AddHandler cpuClockGauge(xInt).Log, AddressOf LogWindow.WriteLog
                                AddHandler cpuClockGauge(xInt).LogError, AddressOf LogWindow_LogError
                                cpuClockGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
                                cpuClockGauge(xInt).AttachSensor(alClocks(xInt))
                            End If
                            cpuloadGauge(xInt) = DefaultGaugeSettings(CType(alLoad(xInt), ISensor))
                            AddHandler cpuloadGauge(xInt).Log, AddressOf LogWindow.WriteLog
                            AddHandler cpuloadGauge(xInt).LogError, AddressOf LogWindow_LogError
                            cpuloadGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
                            cpuloadGauge(xInt).AttachSensor(alLoad(xInt))
                            cpuTempGauge(xInt) = DefaultGaugeSettings(CType(alTemp(xInt), ISensor))
                            AddHandler cpuTempGauge(xInt).Log, AddressOf LogWindow.WriteLog
                            AddHandler cpuTempGauge(xInt).LogError, AddressOf LogWindow_LogError
                            cpuTempGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
                            cpuTempGauge(xInt).AttachSensor(alTemp(xInt))
                            If myHWSettings.ClocksAsGauges Then
                                Me.Controls.Add(cpuClockGauge(xInt))
                                cpuClockGauge(xInt).Left = xInt * (cpuClockGauge(xInt).Width + 5)
                                cpuClockGauge(xInt).Top = intSummary
                                Me.Controls.Add(cpuloadGauge(xInt))
                                cpuloadGauge(xInt).Left = xInt * (cpuloadGauge(xInt).Width + 5)
                                cpuloadGauge(xInt).Top = (cpuClockGauge(xInt).Top + cpuClockGauge(xInt).Height + 5)
                                Me.Controls.Add(cpuTempGauge(xInt))
                                cpuTempGauge(xInt).Left = xInt * (cpuloadGauge(xInt).Width + 5)
                                cpuTempGauge(xInt).Top = (cpuloadGauge(xInt).Top + cpuloadGauge(xInt).Height + 5)
                            Else
                                Me.Controls.Add(cpuloadGauge(xInt))
                                cpuloadGauge(xInt).Left = xInt * (cpuloadGauge(xInt).Width + 5)
                                cpuloadGauge(xInt).Top = intSummary
                                Me.Controls.Add(cpuTempGauge(xInt))
                                cpuTempGauge(xInt).Left = xInt * (cpuloadGauge(xInt).Width + 5)
                                cpuTempGauge(xInt).Top = (cpuloadGauge(xInt).Top + cpuloadGauge(xInt).Height + 5)
                            End If
                            If myHWSettings.ClocksAsGauges Then alGauges.Add(cpuClockGauge(xInt))
                            alGauges.Add(cpuloadGauge(xInt))
                            alGauges.Add(cpuTempGauge(xInt))
                        Next
                        intSummary = cpuTempGauge(cpuTempGauge.Count - 2).Top + cpuTempGauge(cpuTempGauge.Count - 2).Height + 5
                        intMywidth = (cpuloadGauge(0).Width + 5) * CoreCount - 1
                    End With
                ElseIf Hardware.HardwareType = HardwareType.GpuNvidia Then
                    Dim lblName As New Label
                    AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
                    AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
                    AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
                    AddHandler lblName.Paint, AddressOf lblName_Paint
                    With lblName
                        .Text = myHW.Name
                        .Left = 0
                        .AutoSize = False
                        Dim nF As New Font("Cambria", 8, FontStyle.Bold)
                        .Font = nF
                        .TextAlign = ContentAlignment.MiddleCenter
                        .AutoEllipsis = True
                        .BackColor = Color.Transparent
                        .ForeColor = Color.White
                        g = lblName.CreateGraphics
                        s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
                        If s.Width <= 0 Or s.Height <= 0 Then
                            myLayout = eLayout.B
                        End If
                        .Refresh()
                        .Top = 0
                    End With

                    intSummary = lblName.Top + lblName.Height + 5
                    With Hardware
                        For Each nvSensor As ISensor In .Sensors
                            .Update()
                        Next
                        For Each nvSensor As ISensor In .Sensors
                            If Not IsNothing(nvSensor) And Not (nvSensor.SensorType = SensorType.Load AndAlso nvSensor.Name.ToUpper.Contains("VIDEO ENGINE")) Then
                                If nvSensor.SensorType = SensorType.Clock AndAlso Not myHWSettings.ClocksAsGauges Then
                                    alClocks.Add(nvSensor)
                                Else
                                    Dim nvGauge As mtmGauge = DefaultGaugeSettings(nvSensor)
                                    nvGauge.Assign_Settings(MySettings.File, False, MySettings.MySettings)
                                    nvGauge.AttachSensor(nvSensor)
                                    nvGauge.Left = alGauges.Count * (nvGauge.Width + 5)
                                    nvGauge.Top = intSummary
                                    Me.Controls.Add(nvGauge)
                                    alGauges.Add(nvGauge)
                                End If
                            End If
                        Next
                        intSummary += CType(alGauges(0), mtmGauge).Height + 5
                        intMywidth = (CType(alGauges(0), mtmGauge).Width + 5) * alGauges.Count
                        lblName.Width = intMywidth - 10
                        Me.Controls.Add(lblName)
                    End With
                ElseIf Hardware.HardwareType = HardwareType.GpuAti Then
                   Dim lblName As New Label
                    AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
                    AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
                    AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
                    AddHandler lblName.Paint, AddressOf lblName_Paint
                    With lblName
                        .Text = myHW.Name
                        .Left = 0
                        .AutoSize = False
                        Dim nF As New Font("Cambria", 8, FontStyle.Bold)
                        .Font = nF
                        .TextAlign = ContentAlignment.MiddleCenter
                        .AutoEllipsis = True
                        .BackColor = Color.Transparent
                        .ForeColor = Color.White
                        g = lblName.CreateGraphics
                        s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
                        If s.Width <= 0 Or s.Height <= 0 Then
                            myLayout = eLayout.B
                        End If
                        .Refresh()
                        .Top = 0
                    End With
                    Me.Controls.Add(lblName)
                    With Hardware
                        For Each nvSensor As ISensor In .Sensors
                            .Update()
                        Next
                        For Each nvSensor As ISensor In .Sensors
                            If Not IsNothing(nvSensor) AndAlso Not nvSensor.SensorType = SensorType.Voltage Then
                                If nvSensor.SensorType = SensorType.Clock AndAlso Not myHWSettings.ClocksAsGauges Then
                                    alClocks.Add(nvSensor)
                                Else
                                    Dim nvGauge As mtmGauge = DefaultGaugeSettings(nvSensor)
                                    nvGauge.Assign_Settings(MySettings.File, False, MySettings.MySettings)
                                    nvGauge.AttachSensor(nvSensor)
                                    Me.Controls.Add(nvGauge)
                                    nvGauge.Left = alGauges.Count * (nvGauge.Width + 5)
                                    nvGauge.Top = intSummary
                                    alGauges.Add(nvGauge)
                                End If
                            End If
                        Next
                        intSummary += CType(alGauges(0), mtmGauge).Height + 5
                        intMywidth = (CType(alGauges(0), mtmGauge).Width + 5) * alGauges.Count
                    End With
                End If
            End With
            If Not myHWSettings.ClocksAsGauges AndAlso Not myHWSettings.HideClocks Then
                For xInt As Int16 = 0 To alClocks.Count - 1
                    Dim lblClock As New Label
                    lblClock.AutoSize = False
                    lblClock.TextAlign = ContentAlignment.BottomCenter
                    lblClock.Left = 3
                    lblClock.AutoEllipsis = True
                    lblClock.Width = intMywidth - 6
                    lblClock.Text = CType(alClocks(xInt), ISensor).Name.Replace("CPU ", "").Replace("GPU ", "").Replace("#", "") & " " & CInt(alClocks(xInt).Value).ToString & " Mhz"
                    g = lblClock.CreateGraphics
                    s = Size.Subtract(lblClock.ClientSize, g.MeasureString(lblClock.Text, lblClock.Font).ToSize)
                    g = Nothing
                    If s.Width <= 0 Or s.Height <= 0 Then
                        Dim t As New ToolTip
                        t.SetToolTip(lblClock, lblClock.Text)
                        lblClock.Text = alClocks(xInt).Value.ToString & "Mhz"
                    End If
                    s = Nothing
                    lblClock.Top = intSummary
                    intSummary = lblClock.Top + lblClock.Height + 5
                    lblClock.Tag = CType(alClocks(xInt), ISensor).Identifier.ToString
                    Dim gPath As New GraphicsPath()
                    Dim iCorner As Int32 = 10
                    With lblClock
                        Dim ucRect As Rectangle = New Rectangle(.ClientRectangle.Location, .ClientRectangle.Size)
                        With gPath
                            Dim Bottom As Integer = ucRect.Y + ucRect.Height - iCorner
                            Dim Right As Integer = ucRect.X + ucRect.Width - iCorner
                            ' Top Left
                            '.AddArc(New Rectangle(ucRect.X, ucRect.Y, iCorner, iCorner), 180, 90)
                            .AddLine(New Point(Me.Width / 2 - 1, 0), New Point(Me.Width / 2 + 1, 0))
                            ' Top Right
                            .AddArc(New Rectangle(Right, ucRect.Y, iCorner, iCorner), 270, 90)
                            ' Bottom Right
                            .AddArc(New Rectangle(Right, Bottom, iCorner, iCorner), 0, 90)
                            'Bottom Left
                            .AddLine(New Point(ucRect.Width / 2 - 1, ucRect.Height), New Point(0, ucRect.Height))
                            '.AddArc(New Rectangle(ucRect.X, Bottom, iCorner, iCorner), 90, 90)
                            .CloseFigure()
                        End With
                        .BackColor = Color.White
                        .ForeColor = Color.Black
                        .Region = New Region(gPath)
                    End With
                    Me.Controls.Add(lblClock)
                Next
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function DefaultGaugeSettings(ByVal Sensor As ISensor) As mtmGauge
        Try
            Dim Gauge As mtmGauge = New mtmGauge
            If MySettings.MySettings.HasGaugeSettings(Sensor.Identifier.ToString) Then
                ' do from settings
                Dim gSettings As clsSettings.sSettings.sGaugeSettings = MySettings.MySettings.GaugeSettings(Sensor.Identifier.ToString)
                With gSettings
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.Border = .iBorder
                    If .borderColor = Color.Empty Then
                        Gauge.borderColor = Color.Black
                        .borderColor = Color.Black
                    Else
                        Gauge.borderColor = .borderColor
                    End If
                    Gauge.iCorner = .iCorner
                    Gauge.colorBack = .backColor
                    Gauge.colorWarning = .warningColor
                    Gauge.colorRange = .rangeColor
                    Gauge.colorCurrent = .valueColor
                    Gauge.colorMinMax = .minmaxColor
                    Gauge.StepIntervalSmall = .stepSmall
                    Gauge.StepIntervalLarge = .stepLarge
                    Gauge.normalMin = .normalMIN
                    Gauge.warningStart = .warningStart
                    Gauge.maxValue = .valueMAX
                    Gauge.minValue = .valueMIN
                    Gauge.vSize = .vSize
                    Gauge.Invalidate()
                End With
                gSettings = Nothing
            Else
                Gauge.Border = 0
                Gauge.borderColor = Color.Black
                If Sensor.SensorType = SensorType.Fan Then
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.warningStart = 55
                    Gauge.normalMin = -30
                    Gauge.BackColor = Color.Yellow
                    Gauge.colorCurrent = Color.Blue
                    Gauge.colorWarning = Color.Red
                    Gauge.colorRange = Color.Yellow
                    Gauge.colorMinMax = Color.DarkGray
                    Gauge.maxValue = 5000
                    Gauge.StepIntervalSmall = 50
                ElseIf Sensor.SensorType = SensorType.Load Then
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.warningStart = 55
                    Gauge.normalMin = 0
                    Gauge.BackColor = Color.White
                    Gauge.colorCurrent = Color.Blue
                    Gauge.colorWarning = Color.Red
                    Gauge.colorRange = Color.Yellow
                    Gauge.colorMinMax = Color.DarkGray
                    Gauge.maxValue = 100
                    Gauge.StepIntervalLarge = 10
                    Gauge.StepIntervalSmall = 5
                ElseIf Sensor.SensorType = SensorType.Temperature Then
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.warningStart = 55
                    Gauge.normalMin = -30
                    Gauge.BackColor = Color.Yellow
                    Gauge.colorCurrent = Color.Blue
                    Gauge.colorWarning = Color.Red
                    Gauge.colorRange = Color.Yellow
                    Gauge.colorMinMax = Color.DarkGray
                    Gauge.maxValue = 130
                    Gauge.StepIntervalLarge = 10
                    Gauge.StepIntervalSmall = 5
                ElseIf Sensor.SensorType = SensorType.Control Then
                    'control seems to be fan for nvidia
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.warningStart = 90
                    Gauge.normalMin = -30
                    Gauge.BackColor = Color.Yellow
                    Gauge.colorCurrent = Color.Blue
                    Gauge.colorWarning = Color.Red
                    Gauge.colorRange = Color.Yellow
                    Gauge.colorMinMax = Color.DarkGray
                    Gauge.maxValue = 100
                    Gauge.StepIntervalLarge = 10
                    Gauge.StepIntervalSmall = 5
                ElseIf Sensor.SensorType = SensorType.Clock Then
                    If Sensor.Hardware.HardwareType = HardwareType.CPU Then
                        Gauge.maxValue = 5000
                        Gauge.StepIntervalLarge = 500
                        Gauge.StepIntervalSmall = 100
                        Gauge.warningStart = 4000
                        Gauge.normalMin = -30
                    ElseIf Sensor.Hardware.HardwareType = HardwareType.GpuAti Then
                        Gauge.maxValue = 2000
                        Gauge.StepIntervalLarge = 200
                        Gauge.StepIntervalSmall = 40
                        Gauge.warningStart = 1500
                        Gauge.normalMin = -30
                    ElseIf Sensor.Hardware.HardwareType = HardwareType.GpuNvidia Then
                        Gauge.maxValue = 2600
                        Gauge.StepIntervalLarge = 260
                        Gauge.StepIntervalSmall = 65
                        Gauge.warningStart = 1750
                        Gauge.normalMin = -30
                    End If
                    Gauge.Height = 75
                    Gauge.Width = 24
                    Gauge.BackColor = Color.Yellow
                    Gauge.colorCurrent = Color.Blue
                    Gauge.colorWarning = Color.Red
                    Gauge.colorRange = Color.Yellow
                    Gauge.colorMinMax = Color.DarkGray
                End If
            End If
            Return Gauge
        Catch ex As Exception
            LogWindow.WriteError("ucHWM_DefaultGaugeSettings", Err)
            Return Nothing
        End Try
    End Function
    Public _floatForm As frmFloat
    Private Sub cMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cMenu.Opening
        Try
            Dim iTop As Int32 = CType(alGauges(0), mtmGauge).Top

            HideClocksToolStripMenuItem.Checked = myHWSettings.HideClocks
            ClocksGaugesToolStripMenuItem.Checked = myHWSettings.ClocksAsGauges
            If TypeOf Me.ParentForm Is frmFloat Then
                If Not FloatToolStripMenuItem.Checked Then FloatToolStripMenuItem.Checked = True
            Else
                Dim bCheck As Boolean = (Not IsNothing(_floatForm) AndAlso _floatForm.Visible)
                If bCheck <> FloatToolStripMenuItem.Checked Then FloatToolStripMenuItem.Checked = bCheck
            End If
            'If TypeOf Me.ParentForm Is frmFloat Then FloatToolStripMenuItem.Enabled = False
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ClocksGaugesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClocksGaugesToolStripMenuItem.CheckedChanged
        myHWSettings.ClocksAsGauges = ClocksGaugesToolStripMenuItem.Checked
        MySettings.MySettings.SaveHWSettings(myHW.Identifier.ToString, myHWSettings)
        'can be done on application exit when finished debugging
        MySettings.SaveSettings()
    End Sub
    Private Sub HideClocksToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideClocksToolStripMenuItem.CheckedChanged
        myHWSettings.HideClocks = HideClocksToolStripMenuItem.Checked
        MySettings.MySettings.SaveHWSettings(myHW.Identifier.ToString, myHWSettings)
        MySettings.SaveSettings()
        'can be done on application exit when finished debugging
    End Sub
    Private Manager As Manager
    Private Sub FloatToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FloatToolStripMenuItem.CheckedChanged
        Try
            If FloatToolStripMenuItem.Checked Then
                If TypeOf Me.ParentForm Is frmFloat Then
                    FloatToolStripMenuItem.Enabled = True
                    Exit Sub
                End If
                _floatForm = New frmFloat
                Dim nMe As ucHWM = Manager.userControl(Identifier, _floatForm)
                nMe._floatForm = _floatForm
                nMe.AllowDrag = True
                AddHandler nMe.Log, AddressOf LogWindow_Log
                AddHandler nMe.LogError, AddressOf LogWindow_LogError
                'nMe.Init(MySettings.File, False, MySettings.MySettings)
                'nMe.AttachHW(myHW)
                _floatForm.Controls.Add(nMe)
                _floatForm.Show()
                nMe.AutoUpdate(2000)
                AddHandler _floatForm.FormClosed, AddressOf _floatForm_FormClosed
            Else
                If IsNothing(_floatForm) Then Return
                If _floatForm.IsHandleCreated Then _floatForm.Close()
                _floatForm = Nothing
                AutoUpdate(2000)
            End If
        Catch ex As Exception
            LogWindow.WriteError("ucHWM__floattoolstripmenuitem_click", Err)
        End Try
    End Sub
    Private Sub _floatForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        Try
            'Start updating own sensor data
            AutoUpdate(2000)
        Catch ex As Exception
            LogWindow.WriteError("ucHWM__floatform_formClosed", Err)
        End Try
    End Sub
    Private MySettings As clsSettings
    Public Event ParentClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
    Private WithEvents pForm As Form
    Public Function Init(ByVal Location As String, Optional ByVal SetDefaults As Boolean = False, Optional ByVal Settings As clsSettings.sSettings = Nothing, Optional ByVal Hardware As OpenHardwareMonitor.Hardware.IHardware = Nothing, Optional ByVal TheManager As Manager = Nothing) As Boolean
        '(location, false,nothing)
        Try
            If Location.Contains("\config.dat") Then
                Location = Location.Replace("\config.dat", "")
            End If
            MySettings = New clsSettings(Location)
            AddHandler MySettings.Log, AddressOf LogWindow_Log
            AddHandler MySettings.LogError, AddressOf LogWindow_LogError
            If Not IsNothing(Settings) Then
                MySettings.MySettings = Settings
            ElseIf SetDefaults Then
                MySettings.SetDefaults()
            End If
            If Not IsNothing(TheManager) Then Manager = TheManager
            myHW = Hardware
            alGauges.Clear()
            alClocks.Clear()
            If MySettings.MySettings.HasHWSettings(myHW.Identifier.ToString) Then
                myHWSettings = MySettings.MySettings.HWSettings(myHW.Identifier.ToString)
            Else
                myHWSettings = New clsSettings.sSettings.sHWsettings
                myHWSettings.ClocksAsGauges = False
                myHWSettings.HideClocks = False
                myHWSettings.Identifier = myHW.Identifier.ToString
            End If

            Dim intSummary As Int32 = 0, intMywidth As Int16 = 0
            Dim lT As New Label, s As New Size, g As Graphics
            g = lT.CreateGraphics
            intSummary = g.MeasureString(Hardware.Name, lT.Font).ToSize.Height
            myHW = Hardware
            Me.Controls.Clear()
            alGauges.Clear()
            alClocks.Clear()
            If MySettings.MySettings.HasHWSettings(myHW.Identifier.ToString) Then
                myHWSettings = MySettings.MySettings.HWSettings(myHW.Identifier.ToString)
            Else
                myHWSettings = New clsSettings.sSettings.sHWsettings
                myHWSettings.ClocksAsGauges = False
                myHWSettings.HideClocks = False
                myHWSettings.Identifier = myHW.Identifier.ToString
            End If
            AttachHW(myHW)
            'Dim alLoad As New ArrayList
            'Dim alTemp As New ArrayList
            'Dim alClock As New ArrayList
            'With CType(Hardware, OpenHardwareMonitor.Hardware.CPU.GenericCPU)
            '    If .coreCount > 2 Then
            '        myLayout = eLayout.A
            '    Else
            '        myLayout = eLayout.B
            '    End If

            '    Dim mWidth As Int32 = (.coreCount) * 41
            '    Dim lblName As New Label
            '    AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
            '    AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
            '    AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
            '    AddHandler lblName.Paint, AddressOf lblName_Paint
            '    With lblName
            '        .Text = myHW.Name
            '        .Left = 0
            '        .AutoSize = False
            '        .Width = mWidth - 10
            '        .TextAlign = ContentAlignment.BottomCenter
            '        .AutoEllipsis = True
            '        .BackColor = Color.White
            '        .ForeColor = Color.Black
            '        g = lblName.CreateGraphics
            '        s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
            '        If s.Width <= 0 Or s.Height <= 0 Then
            '            myLayout = eLayout.B
            '        End If
            '        .Refresh()
            '        .Top = 0
            '    End With
            '    Me.Controls.Add(lblName)

            '    'group sensors
            '    For Each sSensor As Sensor In myHW.Sensors
            '        If sSensor.SensorType = SensorType.Clock Then
            '            alClocks.Add(sSensor)
            '        ElseIf sSensor.SensorType = SensorType.Load Then
            '            alLoad.Add(sSensor)
            '        ElseIf sSensor.SensorType = SensorType.Temperature Then
            '            alTemp.Add(sSensor)
            '        ElseIf sSensor.SensorType = SensorType.Voltage Then
            '            'Dosomething

            '        End If
            '    Next



            'With myHW
            '    If .HardwareType = HardwareType.CPU Then
            '        Dim alLoad As New ArrayList
            '        Dim alTemp As New ArrayList
            '        Dim alClock As New ArrayList
            '        With CType(Hardware, OpenHardwareMonitor.Hardware.CPU.GenericCPU)
            '            If .coreCount > 2 Then
            '                myLayout = eLayout.A
            '            Else
            '                myLayout = eLayout.B
            '            End If

            '            Dim mWidth As Int32 = ((.Sensors.Count - 2) / 3) * 41 '-2 for bus speed and total load
            '            Dim lblName As New Label
            '            AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
            '            AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
            '            AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
            '            AddHandler lblName.Paint, AddressOf lblName_Paint
            '            With lblName
            '                .Text = myHW.Name
            '                .Left = 2
            '                .AutoSize = False
            '                .Width = mWidth - 10
            '                .TextAlign = ContentAlignment.BottomCenter
            '                .AutoEllipsis = True
            '                .BackColor = Color.White
            '                .ForeColor = Color.Black
            '                g = lblName.CreateGraphics
            '                s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
            '                If s.Width <= 2 Or s.Height <= 0 Then
            '                    myLayout = eLayout.B
            '                End If
            '                .Refresh()
            '                .Top = 0
            '            End With
            '            Me.Controls.Add(lblName)

            '            'group sensors
            '            For Each sSensor As Sensor In myHW.Sensors
            '                If sSensor.SensorType = SensorType.Clock Then
            '                    alClocks.Add(sSensor)
            '                ElseIf sSensor.SensorType = SensorType.Load Then
            '                    alLoad.Add(sSensor)
            '                ElseIf sSensor.SensorType = SensorType.Temperature Then
            '                    alTemp.Add(sSensor)
            '                End If
            '            Next
            '            Dim cpuloadGauge(0 To .coreCount) As mtmGauge
            '            Dim cpuTempGauge(0 To .coreCount) As mtmGauge
            '            Dim cpuClockGauge(0 To .coreCount) As mtmGauge
            '            For xInt As Integer = 0 To .coreCount - 1 'Last in array = average 
            '                'Group sensors for each core
            '                'load/fan/temp
            '                If myHWSettings.ClocksAsGauges Then
            '                    cpuClockGauge(xInt) = DefaultGaugeSettings(CType(alClocks(xInt), Sensor))
            '                    AddHandler cpuClockGauge(xInt).Log, AddressOf LogWindow.WriteLog
            '                    AddHandler cpuClockGauge(xInt).LogError, AddressOf LogWindow_LogError
            '                    cpuClockGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
            '                    cpuClockGauge(xInt).AttachSensor(alClocks(xInt))
            '                End If
            '                cpuloadGauge(xInt) = DefaultGaugeSettings(CType(alLoad(xInt), Sensor))
            '                AddHandler cpuloadGauge(xInt).Log, AddressOf LogWindow.WriteLog
            '                AddHandler cpuloadGauge(xInt).LogError, AddressOf LogWindow_LogError
            '                cpuloadGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
            '                cpuloadGauge(xInt).AttachSensor(alLoad(xInt))
            '                cpuTempGauge(xInt) = DefaultGaugeSettings(CType(alTemp(xInt), Sensor))
            '                AddHandler cpuTempGauge(xInt).Log, AddressOf LogWindow.WriteLog
            '                AddHandler cpuTempGauge(xInt).LogError, AddressOf LogWindow_LogError
            '                cpuTempGauge(xInt).Assign_Settings(MySettings.File, False, MySettings.MySettings)
            '                cpuTempGauge(xInt).AttachSensor(alTemp(xInt))
            '                If myHWSettings.ClocksAsGauges Then
            '                    Me.Controls.Add(cpuClockGauge(xInt))
            '                    cpuClockGauge(xInt).Left = xInt * cpuClockGauge(xInt).Width
            '                    cpuClockGauge(xInt).Top = intSummary
            '                    Me.Controls.Add(cpuloadGauge(xInt))
            '                    cpuloadGauge(xInt).Left = xInt * cpuloadGauge(xInt).Width
            '                    cpuloadGauge(xInt).Top = (cpuClockGauge(xInt).Top + cpuClockGauge(xInt).Height + 5)
            '                    Me.Controls.Add(cpuTempGauge(xInt))
            '                    cpuTempGauge(xInt).Left = xInt * cpuloadGauge(xInt).Width
            '                    cpuTempGauge(xInt).Top = (cpuloadGauge(xInt).Top + cpuloadGauge(xInt).Height + 5)
            '                Else
            '                    Me.Controls.Add(cpuloadGauge(xInt))
            '                    cpuloadGauge(xInt).Left = xInt * cpuloadGauge(xInt).Width
            '                    cpuloadGauge(xInt).Top = intSummary
            '                    Me.Controls.Add(cpuTempGauge(xInt))
            '                    cpuTempGauge(xInt).Left = xInt * cpuloadGauge(xInt).Width
            '                    cpuTempGauge(xInt).Top = (cpuloadGauge(xInt).Top + cpuloadGauge(xInt).Height + 5)
            '                End If
            '                If myHWSettings.ClocksAsGauges Then alGauges.Add(cpuClockGauge(xInt))
            '                alGauges.Add(cpuloadGauge(xInt))
            '                alGauges.Add(cpuTempGauge(xInt))
            '            Next
            '            intSummary = cpuTempGauge(cpuTempGauge.Count - 2).Top + cpuTempGauge(cpuTempGauge.Count - 2).Height + 5
            '            intMywidth = cpuloadGauge(0).Width * .coreCount - 1
            '        End With
            '    ElseIf Hardware.HardwareType = HardwareType.GpuNvidia Then

            '        Dim mWidth As Int32 = .Sensors.Count * 41
            '        Dim lblName As New Label
            '        AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
            '        AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
            '        AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
            '        AddHandler lblName.Paint, AddressOf lblName_Paint
            '        With lblName
            '            .Text = myHW.Name
            '            .Left = 2
            '            .AutoSize = False
            '            .Width = mWidth - 10
            '            .TextAlign = ContentAlignment.BottomCenter
            '            .AutoEllipsis = True
            '            .BackColor = Color.White
            '            .ForeColor = Color.Black
            '            g = lblName.CreateGraphics
            '            s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
            '            If s.Width <= 2 Or s.Height <= 0 Then
            '                myLayout = eLayout.B
            '            End If
            '            .Refresh()
            '            .Top = 0
            '        End With
            '        Me.Controls.Add(lblName)
            '        With CType(Hardware, OpenHardwareMonitor.Hardware.Nvidia.NvidiaGPU)
            '            For Each nvSensor As Sensor In .Sensors
            '                .ActivateSensor(nvSensor)
            '            Next
            '            For Each nvSensor As Sensor In .Sensors
            '                If Not IsNothing(nvSensor) And Not (nvSensor.SensorType = SensorType.Load AndAlso nvSensor.Name.ToUpper.Contains("VIDEO ENGINE")) Then
            '                    If nvSensor.SensorType = SensorType.Clock AndAlso Not myHWSettings.ClocksAsGauges Then
            '                        alClocks.Add(nvSensor)
            '                    Else
            '                        Dim nvGauge As mtmGauge = DefaultGaugeSettings(nvSensor)
            '                        nvGauge.Assign_Settings(MySettings.File, False, MySettings.MySettings)
            '                        nvGauge.AttachSensor(nvSensor)
            '                        Me.Controls.Add(nvGauge)
            '                        nvGauge.Left = alGauges.Count * nvGauge.Width
            '                        nvGauge.Top = intSummary
            '                        alGauges.Add(nvGauge)
            '                    End If
            '                End If
            '            Next
            '            intSummary += CType(alGauges(0), mtmGauge).Height + 5
            '            intMywidth = CType(alGauges(0), mtmGauge).Width * alGauges.Count
            '        End With
            '    ElseIf Hardware.HardwareType = HardwareType.GpuAti Then
            '        Dim mWidth As Int32 = .Sensors.Count * 41
            '        Dim lblName As New Label
            '        AddHandler lblName.MouseDown, AddressOf lblName_MouseDown
            '        AddHandler lblName.MouseMove, AddressOf lblName_MouseMove
            '        AddHandler lblName.MouseUp, AddressOf lblName_MouseUp
            '        AddHandler lblName.Paint, AddressOf lblName_Paint
            '        With lblName
            '            .Text = myHW.Name
            '            .Left = 2
            '            .AutoSize = False
            '            .Width = mWidth - 10
            '            .TextAlign = ContentAlignment.BottomCenter
            '            .AutoEllipsis = True
            '            .BackColor = Color.White
            '            .ForeColor = Color.Black
            '            g = lblName.CreateGraphics
            '            s = Size.Subtract(g.MeasureString(lblName.Text, lblName.Font).ToSize, lblName.ClientSize)
            '            If s.Width <= 2 Or s.Height <= 0 Then
            '                myLayout = eLayout.B
            '            End If
            '            .Refresh()
            '            .Top = 0
            '        End With
            '        Me.Controls.Add(lblName)
            '        With CType(Hardware, OpenHardwareMonitor.Hardware.ATI.ATIGPU)
            '            For Each nvSensor As Sensor In .Sensors
            '                .ActivateSensor(nvSensor)
            '            Next
            '            For Each nvSensor As Sensor In .Sensors
            '                If Not IsNothing(nvSensor) AndAlso Not nvSensor.SensorType = SensorType.Voltage Then
            '                    If nvSensor.SensorType = SensorType.Clock AndAlso Not myHWSettings.ClocksAsGauges Then
            '                        alClocks.Add(nvSensor)
            '                    Else
            '                        Dim nvGauge As mtmGauge = DefaultGaugeSettings(nvSensor)
            '                        nvGauge.Assign_Settings(MySettings.File, False, MySettings.MySettings)
            '                        nvGauge.AttachSensor(nvSensor)
            '                        Me.Controls.Add(nvGauge)
            '                        nvGauge.Left = alGauges.Count * nvGauge.Width
            '                        nvGauge.Top = intSummary
            '                        alGauges.Add(nvGauge)
            '                    End If
            '                End If
            '            Next
            '            intSummary += CType(alGauges(0), mtmGauge).Height + 5
            '            intMywidth = CType(alGauges(0), mtmGauge).Width * alGauges.Count
            '        End With
            '    End If
            'End With
            'If Not myHWSettings.ClocksAsGauges AndAlso Not myHWSettings.HideClocks Then
            '    For xInt As Int16 = 0 To alClocks.Count - 1
            '        Dim lblClock As New Label
            '        lblClock.AutoSize = False
            '        lblClock.TextAlign = ContentAlignment.BottomCenter
            '        lblClock.Left = 3
            '        lblClock.AutoEllipsis = True
            '        lblClock.Width = intMywidth - 6
            '        lblClock.Text = CType(alClocks(xInt), Sensor).Name.Replace("CPU ", "").Replace("GPU ", "").Replace("#", "") & " " & CInt(alClocks(xInt).Value).ToString & " Mhz"
            '        g = lblClock.CreateGraphics
            '        s = Size.Subtract(lblClock.ClientSize, g.MeasureString(lblClock.Text, lblClock.Font).ToSize)
            '        g = Nothing
            '        If s.Width <= 0 Or s.Height <= 0 Then
            '            Dim t As New ToolTip
            '            t.SetToolTip(lblClock, lblClock.Text)
            '            lblClock.Text = alClocks(xInt).Value.ToString & "Mhz"
            '        End If
            '        s = Nothing
            '        lblClock.Top = intSummary
            '        intSummary = lblClock.Top + lblClock.Height + 5
            '        lblClock.Tag = CType(alClocks(xInt), Sensor).Identifier.ToString
            '        Dim gPath As New GraphicsPath()
            '        Dim iCorner As Int32 = 10
            '        With lblClock
            '            Dim ucRect As Rectangle = New Rectangle(.ClientRectangle.Location, .ClientRectangle.Size)
            '            With gPath
            '                Dim Bottom As Integer = ucRect.Y + ucRect.Height - iCorner
            '                Dim Right As Integer = ucRect.X + ucRect.Width - iCorner
            '                ' Top Left
            '                '.AddArc(New Rectangle(ucRect.X, ucRect.Y, iCorner, iCorner), 180, 90)
            '                .AddLine(New Point(Me.Width / 2 - 1, 0), New Point(Me.Width / 2 + 1, 0))
            '                ' Top Right
            '                .AddArc(New Rectangle(Right, ucRect.Y, iCorner, iCorner), 270, 90)
            '                ' Bottom Right
            '                .AddArc(New Rectangle(Right, Bottom, iCorner, iCorner), 0, 90)
            '                'Bottom Left
            '                .AddLine(New Point(ucRect.Width / 2 - 1, ucRect.Height), New Point(0, ucRect.Height))
            '                '.AddArc(New Rectangle(ucRect.X, Bottom, iCorner, iCorner), 90, 90)
            '                .CloseFigure()
            '            End With
            '            .BackColor = Color.White
            '            .ForeColor = Color.Black
            '            .Region = New Region(gPath)
            '        End With
            '        Me.Controls.Add(lblClock)
            '    Next
            'End If

            'Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#Region "Log extender"
    Public Class clsLogWindow
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public WithEvents LogWindow As New clsLogWindow
    Public Event Log(ByVal Message As String)
    Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
    Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub
#End Region
    Private bClosing As Boolean = False
    Public Sub Close()
        Try
            bClosing = True
            autoTimer.Enabled = False
            Threading.Thread.Sleep(500)
            myHW = Nothing
            MySettings = Nothing
            myHWSettings = Nothing
            For Each c As Control In Me.Controls
                If TypeOf c Is mtmGauge Then CType(c, mtmGauge).close()
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private bDown As Boolean = False
    Private bDrag As Boolean = False
    Public Property AllowDrag As Boolean
        Get
            Return bDrag
        End Get
        Set(ByVal value As Boolean)
            bDrag = value
        End Set
    End Property
    Private oldX As Int32, oldY As Int32
    Private Sub lblName_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblName.MouseDown
        Try
            If Not bDrag Then Exit Sub
            bDown = True
            oldX = e.X
            oldY = e.Y
            Debug.Print("oldx/y")
            Debug.Print(oldX & "," & oldY)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub lblName_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblName.MouseMove
        Try
            If bDown Then
                Dim startpoint As Point = New Point(oldX, oldY)
                Dim p1 As Point = New Point(e.X, e.Y)
                Dim p2 As Point = _floatForm.PointToScreen(p1)
                Dim p3 As Point = New Point(p2.X - startpoint.X, p2.Y - startpoint.Y)
                _floatForm.Location = p3
                Debug.Print("mousemove e")
                Debug.Print(e.X & "," & e.Y)
                _floatForm.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub lblName_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblName.MouseUp
        Try
            bDown = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lblName_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblName.Paint
        Try
            'Dim g As Graphics = Me.CreateGraphics
            'g.DrawLine(New Pen(Brushes.Black), New Point(0, 5), New Point(lblName.Width, 5))
            'g.DrawLine(New Pen(Brushes.Black), New Point(0, 0), New Point(0, lblName.Height))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ucHWM_Invalidated(ByVal sender As Object, ByVal e As System.Windows.Forms.InvalidateEventArgs) Handles Me.Invalidated
        Try
            If bClosing Then autoTimer.Enabled = False
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ucHWM_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Try
            ''custom region declaration here
            'Dim myRegion As New Region
            'Dim myPath As New GraphicsPath



        Catch ex As Exception

        End Try
    End Sub

    Private Sub ucHWM_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ParentChanged
        Try
            AddHandler CType(sender, ucHWM).ParentForm.FormClosing, AddressOf uchwm_closing
        Catch ex As Exception

        End Try
    End Sub
    Private Sub uchwm_closing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
        RaiseEvent ParentClosed(sender, e)
    End Sub

    Private Sub FloatToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FloatToolStripMenuItem.Click

    End Sub
End Class
