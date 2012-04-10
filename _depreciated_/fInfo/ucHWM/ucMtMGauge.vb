'/*

'  mtmGauge class
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
Imports System.Drawing.Drawing2D
Imports OpenHardwareMonitor.Hardware
Imports OpenHardwareMonitor
Imports cftUnity.Settings
Imports cftUnity.Settings.cftunity

Public Class mtmGauge
#Region "Sensor"
    Public mySensor As ISensor
    Public Sub close()
        Try
            MySettings = Nothing
            mySensor = Nothing
        Catch ex As Exception

        End Try
    End Sub
    Public Function AttachSensor(ByVal Sensor) As Boolean
        Try
            If IsNothing(Sensor) Then
                'break here!
                MsgBox("bleh!")
            End If
            mySensor = Sensor
            'try getting values
            With mySensor
                Debug.Print(.Name)
                Debug.Print(.Min & "-" & .Value & "-" & .Max)
                Debug.Print(.SensorType.ToString)
                Debug.Print(.Values.Count)
                For xInt As Short = 0 To .Values.Count
                    Debug.Print(.Values(xInt).Time.ToLongTimeString & " - " & .Values(xInt).Value.ToString)
                Next
            End With
            Return True
        Catch ex As Exception
            LogWindow.WriteError("mtmGauge_AttachSensor", Err)
            Return False
        End Try
    End Function
#End Region
#Region "Colors"
    Private cBack As Color = Color.yellow, cRange As Color = Color.Yellow, cCurrent As Color = Color.Blue, cMinMax As Color = Color.DarkGray, cWarning As Color = Color.Red
    Public Property colorBack As Color
        Get
            Return cBack
        End Get
        Set(ByVal value As Color)
            cBack = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property colorCurrent As Color
        Get
            Return cCurrent
        End Get
        Set(ByVal value As Color)
            cCurrent = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property colorRange As Color
        Get
            Return cRange
        End Get
        Set(ByVal value As Color)
            cRange = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property colorWarning As Color
        Get
            Return cWarning
        End Get
        Set(ByVal value As Color)
            cWarning = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property colorMinMax As Color
        Get
            Return cMinMax
        End Get
        Set(ByVal value As Color)
            cMinMax = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
#End Region
#Region "Range settings"
    Public Class sValueRange
        Public Max As Integer = 100
        Public Min As Integer = 0
        Public normalMin As Integer = 30
        Public WarningStart As Integer = 50
        Public StepIntervalLarge As Integer = 10
        Public StepIntervalSmall As Integer = 5
        Public vSize As Int32 = 3
        Public iCorner As Int32 = 20
        Public iBorder As Int32 = 3
        Public borderColor As Color = Color.Black
    End Class
    Private valueRange As New sValueRange
    Public Property Border As Int32
        Get
            Return valueRange.iBorder
        End Get
        Set(ByVal value As Int32)
            valueRange.iBorder = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property borderColor As Color
        Get
            Return valueRange.borderColor
        End Get
        Set(ByVal value As Color)
            valueRange.borderColor = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property iCorner As Int32
        Set(ByVal value As Int32)
            valueRange.iCorner = value
            bDoGradients = True
            Me.Invalidate()
        End Set
        Get
            Return valueRange.iCorner
        End Get
    End Property
    Public Property vSize As Int32
        Get
            Return valueRange.vSize
        End Get
        Set(ByVal value As Int32)
            valueRange.vSize = value
        End Set
    End Property
    Public Property maxValue As Integer
        Get
            Return valueRange.Max
        End Get
        Set(ByVal value As Integer)
            valueRange.Max = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property minValue As Integer
        Get
            Return valueRange.Min
        End Get
        Set(ByVal value As Integer)
            valueRange.Min = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property normalMin As Integer
        Get
            Return valueRange.normalMin
        End Get
        Set(ByVal value As Integer)
            valueRange.normalMin = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property warningStart As Integer
        Get
            Return valueRange.WarningStart
        End Get
        Set(ByVal value As Integer)
            valueRange.WarningStart = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property StepIntervalSmall As Integer
        Get
            Return valueRange.StepIntervalSmall
        End Get
        Set(ByVal value As Integer)
            valueRange.StepIntervalSmall = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
    Public Property StepIntervalLarge As Integer
        Get
            Return valueRange.StepIntervalLarge
        End Get
        Set(ByVal value As Integer)
            valueRange.StepIntervalLarge = value
            bDoGradients = True
            Me.Invalidate()
        End Set
    End Property
#End Region
#Region "Values"
    Public Structure sValues
        Public Current As Integer
        Public Min As Integer
        Public Max As Integer
    End Structure
    Private Values As New sValues
    Public WriteOnly Property Value() As Integer
        Set(ByVal value As Integer)
            Try
                If value < valueMin Or (value <> 0 And valueMin = 0) Then
                    Values.Min = value
                End If
                If value > valueMax Then
                    Values.Max = value
                End If
                Values.Current = value
                Me.Invalidate()
            Catch ex As Exception

            End Try
        End Set
    End Property
    Public Sub ResetMaxMin()
        Values.Min = 0
        Values.Max = 0
        bDoGradients = True
        Me.Invalidate()
    End Sub
    Public ReadOnly Property CurrentValue As Integer
        Get
            Return Values.Current
        End Get
    End Property
    Public ReadOnly Property valueMax As Integer
        Get
            Return Values.Max
        End Get
    End Property
    Public ReadOnly Property valueMin As Integer
        Get
            Return Values.Min
        End Get
    End Property
#End Region
#Region "Orientation"
    Public Enum eOrientation
        Horizontal
        Vertical
    End Enum
    Public Orientation As eOrientation = eOrientation.Vertical
#End Region
#Region "Init"
    Sub Init(Optional ByVal vRange As sValueRange = Nothing)
        Try
            'Me.SetStyle(ControlStyles.UserPaint, True)
            Me.Refresh()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub mtmGauge_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.ToolTip.SetToolTip(Me, "empty")

    End Sub
    Public bDoGradients As Boolean = True
    Private bmpGradient As Bitmap, gradientGraphics As Graphics
    Private valueGraphics As Graphics, bmpValue As Bitmap, arlRectInvalidate As New ArrayList
#End Region
#Region "Painting"
    Public Enum eIconSize
        Size48
        Size32
        Size16
    End Enum
    Public IconSize As eIconSize = eIconSize.Size16

    Public ReadOnly Property GradientBitmap As Bitmap 'don't use outside of testform 
        Get
            Return bmpGradient
        End Get
    End Property
    Public ReadOnly Property valueBitmap As Bitmap ''don't use outside of testform 
        Get
            Return bmpValue
        End Get
    End Property
    Private gpBorder As New GraphicsPath 'Seperate path to paint border
    Private gpControl As New GraphicsPath 'Sperate path for possible region
    Private pBorder As Pen, rInside As Region
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Try


            Debug.Print(mySensor.Identifier.ToString)
            If IsNothing(valueRange) Then
                valueRange = New sValueRange
            End If
            If IsNothing(bmpGradient) Or IsNothing(gradientGraphics) Then
                bmpGradient = New Bitmap(Me.DisplayRectangle.Width, Me.DisplayRectangle.Height, Me.CreateGraphics)
                gradientGraphics = Graphics.FromImage(bmpGradient)
                gradientGraphics.SmoothingMode = SmoothingMode.HighQuality
                'gradientGraphics.FillRectangle(New SolidBrush(Color.Transparent), Me.ClientRectangle)
            End If
            Static ucRect As Rectangle = New Rectangle(Me.ClientRectangle.Location, Me.ClientRectangle.Size)
            ucRect = New Rectangle(Me.DisplayRectangle.Location, Me.DisplayRectangle.Size)
            If bDoGradients Then
                bDoGradients = False


                gpControl.Reset()
                gpBorder.Reset()
                rInside = New Region


                If valueRange.iCorner > 0 Then
                    With gpControl
                        Dim Bottom As Integer = ucRect.Y + ucRect.Height - valueRange.iCorner
                        Dim Right As Integer = ucRect.X + ucRect.Width - valueRange.iCorner
                        ' Top Left
                        .AddArc(New Rectangle(ucRect.X, ucRect.Y, valueRange.iCorner, valueRange.iCorner), 180, 90)
                        ' Top Right
                        .AddArc(New Rectangle(Right, ucRect.Y, valueRange.iCorner, valueRange.iCorner), 270, 90)
                        ' Bottom Right
                        .AddArc(New Rectangle(Right, Bottom, valueRange.iCorner, valueRange.iCorner), 0, 90)
                        'Bottom Left
                        .AddArc(New Rectangle(ucRect.X, Bottom, valueRange.iCorner, valueRange.iCorner), 90, 90)
                        .CloseFigure()
                        If valueRange.iBorder > 0 Then
                            With gpBorder
                                ' Top Left
                                .AddArc(New Rectangle(ucRect.X + valueRange.iBorder, ucRect.Y + valueRange.iBorder, valueRange.iCorner, valueRange.iCorner), 180, 90)
                                ' Top Right
                                .AddArc(New Rectangle(Right - valueRange.iBorder, ucRect.Y + valueRange.iBorder, valueRange.iCorner, valueRange.iCorner), 270, 90)
                                ' Bottom Right
                                .AddArc(New Rectangle(Right - valueRange.iBorder, Bottom - valueRange.iBorder, valueRange.iCorner, valueRange.iCorner), 0, 90)
                                'Bottom Left
                                .AddArc(New Rectangle(ucRect.X + valueRange.iBorder, Bottom - valueRange.iBorder, valueRange.iCorner, valueRange.iCorner), 90, 90)
                                .CloseFigure()
                            End With 'border is 1 pixels smaller then control
                        Else
                            gpBorder = gpControl.Clone
                        End If
                    End With
                Else
                    gpControl.AddLine(New Point(ucRect.X, ucRect.Y), New Point(ucRect.X + ucRect.Width, ucRect.Y))
                    gpControl.AddLine(New Point(ucRect.X + ucRect.Width, ucRect.Y), New Point(ucRect.X + ucRect.Width, ucRect.Y + ucRect.Height))
                    gpControl.AddLine(New Point(ucRect.X + ucRect.Width, ucRect.Y + ucRect.Height), New Point(ucRect.X, ucRect.Y + ucRect.Height))
                    gpControl.CloseFigure()
                    If valueRange.iBorder > 0 Then
                        Dim pR As Point = New Point(ucRect.Location.X + valueRange.iBorder, ucRect.Location.Y + valueRange.iBorder)
                        gpBorder.AddRectangle(New Rectangle(pR, New Size(Size.Subtract(ucRect.Size, New Size(valueRange.iBorder * 2, valueRange.iBorder * 2)))))
                    Else
                        gpBorder = gpControl.Clone
                    End If
                End If
                rInside = New Region(gpBorder)
                Me.Region = New Region(gpControl)
                If valueRange.iBorder > 0 Then
                    'gpBorder.Widen(pBorder)
                    If valueRange.iCorner = 0 Then
                        'Dim rFill As Rectangle = New Rectangle(New Point(ucRect.Location.X + iBorder, ucRect.Location.Y + iBorder), Size.Subtract(Panel1.DisplayRectangle.Size, New Size(iBorder * 2, iBorder * 2)))
                        'controlGraphics.FillPath(New SolidBrush(colorFill), gpBorder)
                        gradientGraphics.FillRectangle(New SolidBrush(valueRange.borderColor), Me.DisplayRectangle)
                    Else
                        Dim pBorder As New Pen(valueRange.borderColor, valueRange.iBorder)
                        pBorder.DashStyle = DashStyle.Solid
                        pBorder.MiterLimit = valueRange.iBorder
                        gradientGraphics.DrawPath(pBorder, gpBorder)
                    End If
                End If
                gradientGraphics.FillRegion(New SolidBrush(cBack), rInside) 'fill back color
                ' Dim ucRect As Rectangle = New Rectangle(New Point(Me.ClientRectangle.Location.X + 5, Me.ClientRectangle.Y + 5), Size.Subtract(Me.ClientRectangle.Size, New Size(10, 10)))
                If iCorner > (Me.DisplayRectangle.Width / 2) - 2 Then
                    'Reset iCorner 
                    iCorner = Me.DisplayRectangle.Width / 2 - 2
                End If
                ucRect = New Rectangle(Me.DisplayRectangle.X, Me.DisplayRectangle.Y, Me.DisplayRectangle.Width, Me.DisplayRectangle.Height)
                Dim stepValue As Double ' size of single value step
                If Orientation = eOrientation.Vertical Then
                    stepValue = (ucRect.Height / maxValue)
                Else
                    stepValue = (ucRect.Width / maxValue)
                End If
                Dim rectWarningHeight As Double = ucRect.Height - (stepValue * warningStart)
                Dim rectNormalMinHeight As Double = ucRect.Height - (stepValue * normalMin) - rectWarningHeight
                Dim rectWarning As Rectangle = New Rectangle
                Dim sPoint As Point = New Point(ucRect.Location)
                If Orientation = eOrientation.Vertical Then
                    rectWarning.Width = ucRect.Width
                    rectWarning.Height = rectWarningHeight
                    rectWarning.Y = 0
                    Dim gBrush As New LinearGradientBrush(rectWarning, cWarning, cRange, 90)
                    gradientGraphics.FillRectangle(gBrush, rectWarning)
                    gBrush = Nothing
                Else
                    Throw New NotImplementedException
                End If
                Dim rectNormal As Rectangle = New Rectangle
                'Get start point
                If Orientation = eOrientation.Vertical Then
                    'spoint.y = 
                    sPoint.Y = rectWarning.Height
                    rectNormal.Location = sPoint
                    rectNormal.Width = ucRect.Width
                    rectNormal.Height = rectNormalMinHeight
                    Dim gBrush As New LinearGradientBrush(rectNormal, cRange, cBack, 90)
                    'gradientGraphics.FillRectangle(gBrush, rectNormal)
                    'g.FillRectangle(New SolidBrush(cRange), rectNormal)
                Else
                    Throw New NotImplementedException
                End If
                'Icon
                Dim mPoint As Point
                If MySettings.MySettings.HasGaugeSettings(mySensor.Identifier.ToString) Then
                    If Not IsNothing(MySettings.MySettings.GaugeSettings(mySensor.Identifier.ToString).AlternateImage) And Not MySettings.MySettings.GaugeSettings(mySensor.Identifier.ToString).AlternateImage = "" Then
                        Static myImage As Image = Image.FromFile(MySettings.MySettings.GaugeSettings(mySensor.Identifier.ToString).AlternateImage)
                        mPoint = New Point(bmpValue.Width / 2 - myImage.Width - 2, bmpValue.Height - myImage.Height - 10)
                        gradientGraphics.DrawImage(myImage, mPoint)
                    Else
                        GoTo Normal
                    End If
                Else
Normal:
                    Select Case IconSize
                        Case Is = eIconSize.Size16
                            If bmpGradient.Width >= 20 And bmpGradient.Height >= 50 And Not IsNothing(mySensor) Then
                                mPoint = New Point((bmpGradient.Width / 2) - 8, ucRect.Height - 18)
                                Dim rectSize As Rectangle = New Rectangle(mPoint, New Size(16, 16))
                                If mySensor.SensorType = SensorType.Load Then
                                    gradientGraphics.DrawImage(My.Resources.Load48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Temperature Then
                                    gradientGraphics.DrawImage(My.Resources.Temp48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Fan Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Control And mySensor.Hardware.HardwareType = HardwareType.GpuNvidia Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, rectSize)
                                End If
                            Else
                                If IsNothing(mySensor) Then
                                    LogWindow.WriteLog("DrawIcon for pbar failed, no sensortype!")
                                Else
                                    LogWindow.WriteLog("DrawIcon for pbar skipped, size to small!")
                                End If
                            End If
                        Case Is = eIconSize.Size32
                            If bmpGradient.Width >= 34 And bmpGradient.Height >= 50 And Not IsNothing(mySensor) Then
                                mPoint = New Point((bmpGradient.Width / 2) - 16, ucRect.Height - 37)
                                Dim rectSize As Rectangle = New Rectangle(mPoint, New Size(32, 32))
                                If mySensor.SensorType = SensorType.Load Then
                                    gradientGraphics.DrawImage(My.Resources.Load48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Temperature Then
                                    gradientGraphics.DrawImage(My.Resources.Temp48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Fan Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Control And mySensor.Hardware.HardwareType = HardwareType.GpuNvidia Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, rectSize)
                                ElseIf mySensor.SensorType = SensorType.Clock Then
                                    gradientGraphics.DrawImage(My.Resources.Load48x48, rectSize)
                                End If
                            Else
                                If IsNothing(mySensor) Then
                                    LogWindow.WriteLog("DrawIcon for pbar failed, no sensortype!")
                                Else
                                    LogWindow.WriteLog("DrawIcon for pbar skipped, size to small!")
                                End If
                            End If
                        Case Is = eIconSize.Size48
                            If bmpGradient.Width >= 37 And bmpGradient.Height >= 50 And Not IsNothing(mySensor) Then
                                mPoint = New Point((bmpValue.Width / 2) - 24, ucRect.Height - 52)
                                If mySensor.SensorType = SensorType.Load Then
                                    gradientGraphics.DrawImage(My.Resources.Load48x48, mPoint)
                                ElseIf mySensor.SensorType = SensorType.Temperature Then
                                    gradientGraphics.DrawImage(My.Resources.Temp48x48, mPoint)
                                ElseIf mySensor.SensorType = SensorType.Fan Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, mPoint)
                                ElseIf mySensor.SensorType = SensorType.Control And mySensor.Hardware.HardwareType = HardwareType.GpuNvidia Then
                                    gradientGraphics.DrawImage(My.Resources.Fan48x48, mPoint)
                                End If
                            Else
                                If IsNothing(mySensor) Then
                                    LogWindow.WriteLog("DrawIcon for pbar failed, no sensortype!")
                                Else
                                    LogWindow.WriteLog("DrawIcon for pbar skipped, size to small!")
                                End If
                            End If
                    End Select
                End If

                'Create visual steps
                If Orientation = eOrientation.Vertical Then
                    Dim dWidth As Int32 = ucRect.Width / 10
                    For xInt As Int32 = maxValue To 0 Step -StepIntervalSmall
                        Dim rectVisual As Rectangle = New Rectangle
                        rectVisual.Width = dWidth
                        Dim sV As Int32 = 1
                        If CInt(stepValue) > 0 Then
                            sV = CInt(stepValue)
                        End If
                        rectVisual.Height = sV
                        rectVisual.Y = xInt * stepValue
                        rectVisual.X = -2
                        gradientGraphics.FillRectangle(New SolidBrush(Color.Black), rectVisual)
                        rectVisual.X = ucRect.Width - dWidth
                        gradientGraphics.FillRectangle(New SolidBrush(Color.Black), rectVisual)
                    Next
                    dWidth = ucRect.Width / 5
                    For xInt As Double = maxValue To 0 Step -StepIntervalLarge
                        Dim rectVisual As Rectangle = New Rectangle
                        rectVisual.Width = dWidth
                        Dim sV As Int32 = 1
                        If CInt(stepValue) > 0 Then
                            sV = CInt(stepValue)
                        End If
                        rectVisual.Height = sV
                        rectVisual.Y = xInt * stepValue
                        rectVisual.X = -2
                        gradientGraphics.FillRectangle(New SolidBrush(Color.Black), rectVisual)
                        rectVisual.X = ucRect.Width - dWidth
                        gradientGraphics.FillRectangle(New SolidBrush(Color.Black), rectVisual)
                    Next
                Else
                    Throw New NotImplementedException
                End If

                Me.CreateGraphics.DrawImage(bmpGradient, 0, 0)
                GoTo DrawValues
            Else
DrawValues:

                bmpValue = New Bitmap(bmpGradient)
                valueGraphics = Graphics.FromImage(bmpValue)
                'Draw icon
                Dim rectValue As New Rectangle, rectMin As New Rectangle, rectMax As New Rectangle
                Dim stepValue As Double ' size of single value step
                If Orientation = eOrientation.Vertical Then
                    stepValue = (ucRect.Height / maxValue)
                Else
                    stepValue = (ucRect.Width / maxValue)
                End If
                If Orientation = eOrientation.Vertical Then
                    Dim dWidth As Int32 = ucRect.Width / 10
                    Dim vWidth As Int32 = dWidth * 8
                    If CurrentValue <> 0 Then
                        rectValue.Width = vWidth
                        rectValue.Height = stepValue * valueRange.vSize
                        rectValue.X = dWidth * 2
                        rectValue.Y = ucRect.Height - (stepValue * (CurrentValue + 3))
                        valueGraphics.FillRectangle(New SolidBrush(cCurrent), rectValue)
                    End If
                    If Not IsNothing(Values.Max) Then
                        If Values.Max <> Values.Current Then
                            rectMax.Width = ucRect.Width
                            rectMax.Height = stepValue
                            rectMax.Y = ucRect.Height - (stepValue * Values.Max)
                            valueGraphics.FillRectangle(New SolidBrush(cMinMax), rectMax)
                        End If
                    End If
                    If Not IsNothing(Values.Min) Then
                        If Values.Min <> Values.Current AndAlso Values.Min <> 0 Then
                            rectMin.Width = ucRect.Width
                            rectMin.Height = stepValue
                            rectMin.Y = ucRect.Height - (stepValue * Values.Min)
                            valueGraphics.FillRectangle(New SolidBrush(cMinMax), rectMin)
                        End If
                    End If
                Else
                    Throw New NotImplementedException
                End If

                Me.CreateGraphics.DrawImage(bmpValue, 0, 0)
            End If







        Catch ex As Exception
            ' Debug.Print(mySensor.Identifier.ToString & " " & ex.Message)

        End Try

    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub

    Private Sub mtmGauge_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseHover
        Try
            Dim tString As String = mySensor.Identifier.ToString & " " & mySensor.Name.ToString & vbNewLine & Me.maxValue & vbNewLine & valueMax & " - " & CurrentValue & " - " & valueMin
            ToolTip.Show(tString, Me)
        Catch ex As Exception


        End Try
    End Sub

    Private Sub ucGBAR_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        bDoGradients = True
    End Sub
#End Region
#Region "Gauge settings"
    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Try
            Dim dSettings As New DiagGaugeSettings
            dSettings.Assign_Settings(MySettings.File, False, MySettings.MySettings)
            dSettings.SetGauge = Me
            If dSettings.ShowDialog Then
                If dSettings.DialogResult = DialogResult.OK Then
                    'get saved settings
                    Dim sSettings As clsSettings.sSettings.sGaugeSettings = MySettings.MySettings.GaugeSettings(mySensor.Identifier.ToString)
                    With sSettings
                        Me.colorBack = .backColor
                        Me.colorMinMax = .minmaxColor
                        Me.normalMin = .normalMIN
                        Me.colorRange = .rangeColor
                        Me.StepIntervalLarge = .stepLarge
                        Me.StepIntervalSmall = .stepSmall
                        Me.colorCurrent = .valueColor
                        Me.maxValue = .valueMAX
                        Me.minValue = .valueMIN
                        Me.colorWarning = .warningColor
                        Me.warningStart = .warningStart
                        Me.vSize = .vSize
                        Me.Border = .iBorder
                        Me.iCorner = .iCorner
                        Me.borderColor = .borderColor
                        'Alternate image is handeld in paint event
                        Me.Invalidate()
                    End With
                    sSettings = Nothing
                End If
            End If
            dSettings = Nothing
        Catch ex As Exception
            LogWindow.WriteError("mtmGauge_SettingsToolStripItem", Err)
        End Try
    End Sub
    Private Sub ResetMaxMinToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResetMaxMinToolStripMenuItem.Click
        Me.ResetMaxMin()
    End Sub
    Private Sub cMenu_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cMenu.Opening
        Try
            If TypeOf Me.ParentForm Is DiagGaugeSettings Then e.Cancel = True
        Catch ex As Exception

        End Try
        If ToolTip.Active Then ToolTip.Hide(Me)
    End Sub
#End Region

#Region "Log extender"
    Private MySettings As clsSettings
    Public Function Assign_Settings(ByVal Location As String, Optional ByVal SetDefaults As Boolean = False, Optional ByVal Settings As clsSettings.sSettings = Nothing) As Boolean
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
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public ReadOnly Property IsSettingsEmpty As Boolean
        Get
            Return MySettings.IsEmpty
        End Get
    End Property
    Public ReadOnly Property CheckSettings As Boolean
        Get
            Return Not IsNothing(MySettings)
        End Get
    End Property
    Public Class cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public WithEvents LogWindow As New cLW
    Public Event Log(ByVal Message As String)
    Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
    Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub
#End Region
End Class
