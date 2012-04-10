'/*
' * fInfo frmTest
' *
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
Imports OpenHardwareMonitor
Imports OpenHardwareMonitor.Hardware
Imports FAHInterface
Imports FAHInterface.Client
Imports HWInfo

Public Class frmTEST
    '    'All text output disabeld for cpu usage trail :D 
    '    'Looking good so far, will run overnight ( offcourse need to store info in local db, will add overhead but not that much I hope )

    '    Public Delegate Sub AppendRT(Sensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)
    '    Public Sub InvOhmAppend(Sensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)
    '        Try
    '            If ReferenceEquals(Form.ActiveForm, Me) Then
    '                rtf.AppendText(Sensor.EventTime.ToLongTimeString & " " & Sensor.Identifier.ToString & "-" & Sensor.Name & ":" & CShort(Sensor.CurrentValue) & Environment.NewLine)
    '                If Not chkDRTF.Checked Then
    '                    rtf.SelectionStart = rtf.TextLength
    '                    rtf.ScrollToCaret()
    '                End If
    '            End If
    '        Catch ex As Exception

    '        End Try
    '    End Sub
    '    Public Sub OhmAppend(sensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)
    '        Try
    '            If rtf.InvokeRequired Then
    '                Dim nInv As New AppendRT(AddressOf InvOhmAppend)
    '                rtf.Invoke(nInv, {sensor})
    '            Else
    '                If ReferenceEquals(Form.ActiveForm, Me) Then
    '                    rtf.AppendText(sensor.EventTime.ToLongTimeString & " " & sensor.Identifier.ToString & "-" & sensor.Name & ":" & CShort(sensor.CurrentValue) & Environment.NewLine)
    '                    If Not chkDRTF.Checked Then
    '                        rtf.SelectionStart = rtf.TextLength
    '                        rtf.ScrollToCaret()
    '                    End If
    '                End If
    '            End If
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub
    '    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
    '        If Button1.Text = "Disable updates" Then
    '            nudInt.Value = 0
    '            ohmIntInterval(CInt(nudInt.Value))
    '            Button1.Text = "Enable updates"
    '            Button1.Enabled = False
    '        Else
    '            'OUTPUT INTIAL VALUES, REPLACE WITH STORE INITIAL VALUES IN DB
    '            For xInt As Short = 0 To hwInf.ohmInterface.CpuCount - 1
    '                With hwInf.ohmInterface.CPU(xInt)
    '                    .Hardware.Update()
    '                    For Each Sens As ISensor In .Hardware.Sensors
    '                        If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                        'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                    Next
    '                End With
    '            Next
    '            For xInt As Short = 0 To hwInf.ohmInterface.ATICount - 1
    '                With hwInf.ohmInterface.Ati(xInt)
    '                    .Hardware.Update()
    '                    For Each Sens As ISensor In .Hardware.Sensors
    '                        If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                        'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                    Next
    '                End With
    '            Next
    '            For xInt As Short = 0 To hwInf.ohmInterface.NVIDIACount - 1
    '                With hwInf.ohmInterface.Nvidia(xInt)
    '                    .Hardware.Update()
    '                    For Each Sens As ISensor In .Hardware.Sensors
    '                        If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                        'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                    Next
    '                End With
    '            Next
    '            ohmIntInterval(CInt(nudInt.Value))
    '            Button1.Text = "Disable updates"
    '        End If
    '    End Sub
    '    Public Sub SensorHandler(Sensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)
    '        Try
    '            OhmAppend(Sensor)
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub

    '    Private Sub frmTEST_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated

    '    End Sub
    '    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '        hwInf.ohmInterface.Close()
    '        Application.DoEvents()
    '        Application.Exit()
    '    End Sub
    '    Private WithEvents tWait As New Timer

    '    Private Sub cmdConnect_Click(sender As System.Object, e As System.EventArgs) Handles cmdConnect.Click
    '        If cmdConnect.Text = "Connect" Then
    '            cmdClientUpdates.Text = "Disable updates"
    '            cmdClientUpdates.Enabled = False
    '            Client = New FAHInterface.Client.ClientAccess
    '            Client.Connect(txtHost.Text.Substring(0, txtHost.Text.IndexOf(":")), CInt(txtHost.Text.Substring(txtHost.Text.IndexOf(":") + 1)))
    '            rtfC.AppendText(Client.ReadBuffer)
    '            Client.SendCommand("auth " & TextBox3.Text)
    '            Client.Update()
    '            rtfC.AppendText(Client.ReadBuffer)
    '            'Call Timer2_Tick(Me, Nothing)
    '            Timer2.Interval = nudClient.Value
    '            Timer3.Interval = nudOpt.Value
    '            If Client.ValidPassword Then
    '                cmdConnect.Text = "Disconnect"
    '                Client.SendCommand("info")
    '                Client.Update()
    '                Client.SendCommand("options -d")
    '                Client.Update()
    '                Client.SendCommand("queue-info")
    '                Client.Update()
    '                Client.SendCommand("slot-info")
    '                Client.Update()
    '                Button1.Text = "Updating sensor readings"
    '                For xInt As Short = 0 To hwInf.ohmInterface.CpuCount - 1
    '                    With hwInf.ohmInterface.CPU(xInt)
    '                        .Hardware.Update()
    '                        For Each Sens As ISensor In .Hardware.Sensors
    '                            If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                            'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                        Next
    '                    End With
    '                Next
    '                For xInt As Short = 0 To hwInf.ohmInterface.ATICount - 1
    '                    With hwInf.ohmInterface.Ati(xInt)
    '                        .Hardware.Update()
    '                        For Each Sens As ISensor In .Hardware.Sensors
    '                            If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                            'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                        Next
    '                    End With
    '                Next
    '                For xInt As Short = 0 To hwInf.ohmInterface.NVIDIACount - 1
    '                    With hwInf.ohmInterface.Nvidia(xInt)
    '                        .Hardware.Update()
    '                        For Each Sens As ISensor In .Hardware.Sensors
    '                            If Not chkCPU.Checked Then rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & Environment.NewLine)
    '                            'If Not chkCPU.Checked Then rtf.AppendText(Environment.NewLine & Environment.NewLine & Environment.NewLine)
    '                        Next
    '                    End With
    '                Next
    '                If chkDRTF.Checked = False Then
    '                    rtf.AppendText("Updated: " & DateTime.Now.ToLongTimeString & vbNewLine)
    '                    rtf.SelectionStart = rtf.TextLength
    '                    rtf.ScrollToCaret()
    '                    Application.DoEvents()
    '                End If
    '                Call Timer2_Tick(Me, Nothing)
    '                Call Timer3_Tick(Me, Nothing)
    '                Timer2.Enabled = True
    '                Timer3.Enabled = True
    '                ' TODO handle creating tables with initial values 
    '                Data = New Data.Data
    '                Select Case Data.Init(Application.StartupPath & "\Data", hwInf, Client)
    '                    Case Is = Global.Data.Data.eInitResult.Fatal

    '                    Case Is = Global.Data.Data.eInitResult.NonFatal

    '                End Select


    '            Else
    '                Client = Nothing
    '            End If
    '            cmdClientUpdates.Enabled = True
    '        Else
    '            Client = Nothing
    '            Timer2.Enabled = False
    '            Timer3.Enabled = False
    '            cmdConnect.Text = "Connect"
    '            cmdClientUpdates.Text = "Enable updates"
    '        End If
    '    End Sub
    '    Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
    '        Try
    '            ' Don't send command first as async op will cause problems
    '            ' txtOUT.AppendText(txtCMD.Text & environment.newline)
    '            If Not IsNothing(Client) Then
    '                If Not Client.Connected Then Exit Sub
    '                Client.SendCommand(txtCmd.Text)
    '                Client.Update()
    '                rtfC.AppendText(txtCmd.Text & Environment.NewLine)
    '                rtfC.AppendText(Client.ReadBuffer)
    '                rtfC.SelectionStart = rtfC.TextLength
    '                rtfC.ScrollToCaret()
    '                txtCmd.Clear()
    '                txtCmd.Focus()
    '            End If
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub

    '    Private Sub nudInt_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudInt.ValueChanged
    '        If nudInt.Value <> 0 Then Button1.Enabled = True
    '    End Sub

    '    Private Sub SplitContainer1_Resize(sender As Object, e As System.EventArgs) Handles SplitContainer1.Resize
    '        Try
    '            Panel3.Dock = DockStyle.Top
    '            Panel2.Dock = DockStyle.Bottom
    '            rtfC.Top = Panel3.Top + Panel3.Height + 2
    '            rtfC.Height = Panel2.Top - (Panel3.Top + Panel3.Height + 2)
    '            rtfC.Width = Panel2.Width
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub

    '    Private Sub tWait_Tick(sender As Object, e As System.EventArgs) Handles tWait.Tick
    '        tWait.Enabled = False
    '    End Sub

    '    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
    '        If Not Client.Connected Then
    '            Client.Connect("192.168.1.201", 36330)
    '            Client.SendCommand("auth " & TextBox3.Text)
    '            Client.Update()
    '        End If
    '        Client.SendCommand("queue-info")
    '        Client.Update()
    '        Client.SendCommand("slot-info")
    '        Client.Update()
    '        With Client
    '            If Not chkCPU.Checked Then rtfC.AppendText("Connected: " & .Connected.ToString & Environment.NewLine)
    '            If Not chkCPU.Checked Then rtfC.AppendText("Hostname: " & .HostName & " (" & .Port.ToString & ")" & Environment.NewLine)
    '            If Not IsNothing(.Info) Then
    '                If ReferenceEquals(Form.ActiveForm, Me) Then
    '                    If Not chkCPU.Checked Then rtfC.AppendText("Info values:- (" & .Info.Values.Count.ToString & ")" & Environment.NewLine)
    '                    For xInt As Int16 = 0 To .Info.Values.Count
    '                        If Not chkCPU.Checked Then rtfC.AppendText(.Info.Keys(xInt) & ": " & .Info.Values(xInt) & Environment.NewLine)
    '                    Next
    '                End If
    '            End If
    '            If Not IsNothing(.Options) Then
    '                If ReferenceEquals(Me, Form.ActiveForm) Then
    '                    If Not chkCPU.Checked Then rtfC.AppendText("Options values:- (" & .Options.Values.Count.ToString & ")" & Environment.NewLine)
    '                    For xInt As Int16 = 0 To .Options.Values.Count
    '                        If Not chkCPU.Checked Then rtfC.AppendText(.Options.Keys(xInt) & ": " & .Options.Values(xInt) & Environment.NewLine)
    '                    Next
    '                End If
    '            End If
    '            If Not IsNothing(.Slots(0)) Then
    '                If Not chkCPU.Checked Then rtfC.AppendText("Slot values:- (" & .SlotCount & ")" & Environment.NewLine)
    '                If .SlotCount > 0 Then
    '                    If ReferenceEquals(Me, Form.ActiveForm) Then
    '                        For xInt As Int16 = 0 To .SlotCount - 1
    '                            If Not chkCPU.Checked Then rtfC.AppendText("Slot: " & xInt.ToString & "(" & .Slots(xInt).Values.Count & ")" & Environment.NewLine)
    '                            For yInt As Int16 = 0 To .Slots(xInt).Values.Count
    '                                If Not chkCPU.Checked Then rtfC.AppendText(.Slots(xInt).Keys(yInt) & ": " & .Slots(xInt).Values(yInt) & Environment.NewLine)
    '                            Next
    '                        Next
    '                    End If
    '                End If
    '            End If
    '            If .QueueCount > 0 Then
    '                If Not chkCPU.Checked Then rtfC.AppendText("Queue values:- (" & .QueueCount & ")" & Environment.NewLine)
    '                For xInt As Int16 = 0 To .QueueCount - 1
    '                    If ReferenceEquals(Me, Form.ActiveForm) Then
    '                        If Not chkCPU.Checked Then rtfC.AppendText("Queue: " & xInt.ToString & "(" & .Queue(xInt).Values.Count & ")" & Environment.NewLine)
    '                        For yInt As Int16 = 0 To .Queue(xInt).Values.Count
    '                            If Not chkCPU.Checked Then rtfC.AppendText(.Queue(xInt).Keys(yInt) & ": " & .Queue(xInt).Values(yInt) & Environment.NewLine)
    '                        Next
    '                    End If
    '                Next
    '            End If
    '            If chkDRTFC.Checked = False Then
    '                If ReferenceEquals(Me, Form.ActiveForm) Then
    '                    rtfC.AppendText(DateTime.Now.ToLongTimeString & " : Queue/Slots updated." & Environment.NewLine)
    '                    rtfC.SelectionStart = rtfC.TextLength
    '                    rtfC.ScrollToCaret()
    '                End If
    '            End If

    '        End With
    '        ' Client is strongtyped as well, breakpoint at with and check in debugger :)
    '        ' info.gpu(index) is zero based
    '    End Sub
    '    Private Sub Timer3_Tick(sender As System.Object, e As System.EventArgs) Handles Timer3.Tick
    '        Try
    '            If Not IsNothing(Client) Then
    '                If Client.Connected Then
    '                    Client.SendCommand("options -d")
    '                    Client.Update()
    '                    Dim bOnce As Boolean = False
    '                    If chkDRTFC.Checked = False Then
    '                        If ReferenceEquals(Form.ActiveForm, Me) Then rtfC.AppendText(DateTime.Now.ToLongTimeString & " : Options updated." & Environment.NewLine)
    'Scroll:
    '                        If ReferenceEquals(Form.ActiveForm, Me) Then
    '                            rtfC.SelectionStart = rtfC.TextLength
    '                            rtfC.ScrollToCaret()
    '                        End If
    '                    End If
    '                    If chkCPU.Checked And chkDRTFC.Checked = False Then
    '                        If Not bOnce Then
    '                            bOnce = True
    '                            GoTo Scroll
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                Timer3.Enabled = False
    '                Timer2.Enabled = False
    '                cmdClientUpdates.Text = "Enable updates"
    '            End If
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub

    '    Private Sub nudClient_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudClient.ValueChanged
    '        Timer2.Interval = nudClient.Value
    '    End Sub

    '    Private Sub cmdClientUpdates_Click(sender As System.Object, e As System.EventArgs) Handles cmdClientUpdates.Click
    '        Try
    '            If IsNothing(Client) Then Exit Sub
    '            If Not Client.Connected Then
    '                cmdClientUpdates.Text = "Enable updates"
    '                Timer2.Enabled = False
    '                Timer3.Enabled = False
    '                Exit Sub
    '            End If
    '            If cmdClientUpdates.Text = "Enable updates" Then
    '                Timer2.Enabled = True
    '                Timer3.Enabled = True
    '                cmdClientUpdates.Text = "Disable updates"
    '            Else
    '                Timer2.Enabled = False
    '                Timer3.Enabled = False
    '                cmdClientUpdates.Text = "Enable updates"
    '            End If
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try
    '    End Sub

    '    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    '        Try
    '            rtf.Top = 1 : rtf.Left = 1
    '            rtf.Width = SplitContainer1.Panel1.Width - 2
    '            rtf.Height = SplitContainer1.Panel1.Height - 2 - Panel1.Height
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try

    '    End Sub

    '    Private Sub txtCmd_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtCmd.KeyDown
    '        Try
    '            If e.KeyCode = Keys.Enter Then Call cmdSend_Click(Me, Nothing)
    '        Catch ex As Exception
    '            LogWindow.WriteError(Me, Err)
    '        End Try

    '    End Sub

    '    Private Sub nudOpt_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudOpt.ValueChanged
    '        Timer3.Interval = nudOpt.Value
    '    End Sub


    '    Private Sub frmTEST_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    '        While Not Me.Created
    '            Application.DoEvents()
    '        End While
    '        If TextBox3.Text <> "" Then cmdConnect_Click(Me, Nothing)

    '    End Sub
End Class
