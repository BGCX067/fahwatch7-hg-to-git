 '/* This program is free software. It comes without any warranty, to
' * the extent permitted by applicable law. You can redistribute it
' * and/or modify it under the terms of the Do What The Fuck You Want
' * To Public License, Version 2, as published by Sam Hocevar. See
' * http://sam.zoy.org/wtfpl/COPYING for more details. */
Imports OpenHardwareMonitor
Imports OpenHardwareMonitor.Hardware
Imports FAHInterface
Imports FAHInterface.Client
Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        modMAIN.InitHWINF()
        If Timer1.Enabled Then
            Button1.Text = "Update sensor readings"
            Timer1.Enabled = False
        Else
            Button1.Text = "Updating sensor readings"
            Timer1.Enabled = True
        End If
    End Sub
    ' Calling HW.update is wrong, it will cause any application to freeze eventually. Use .accept(Visitor) and the sensorventhandler for implementation.
    
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Button1.Text = "Updating sensor readings"
        Application.DoEvents()
        For xInt As Short = 0 To hwInf.ohmInterface.CpuCount - 1
            With hwInf.ohmInterface.CPU(xInt)
                .Hardware.Update()
                For Each Sens As ISensor In .Hardware.Sensors
                    rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & vbNewLine)
                    rtf.AppendText(vbNewLine & vbNewLine & vbNewLine & vbNewLine)
                    rtf.SelectionStart = rtf.TextLength
                    rtf.ScrollToCaret()
                    Application.DoEvents()
                Next
            End With
        Next
        For xInt As Short = 0 To hwInf.ohmInterface.ATICount - 1
            With hwInf.ohmInterface.Ati(xInt)
                .Hardware.Update()
                For Each Sens As ISensor In .Hardware.Sensors
                    rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & vbNewLine)
                    rtf.AppendText(vbNewLine & vbNewLine & vbNewLine & vbNewLine)
                    rtf.SelectionStart = rtf.TextLength
                    rtf.ScrollToCaret()
                    Application.DoEvents()
                Next
            End With
        Next
        For xInt As Short = 0 To hwInf.ohmInterface.NVIDIACount - 1
            With hwInf.ohmInterface.Nvidia(xInt)
                .Hardware.Update()
                For Each Sens As ISensor In .Hardware.Sensors
                    rtf.AppendText(DateTime.Now.ToLongTimeString & Sens.Identifier.ToString & "-" & Sens.Name & ":" & Sens.Value & vbNewLine)
                    rtf.AppendText(vbNewLine & vbNewLine & vbNewLine & vbNewLine)
                    rtf.SelectionStart = rtf.TextLength
                    rtf.ScrollToCaret()
                    Application.DoEvents()
                Next
            End With
        Next
        If CheckBox1.Checked Then
            Timer1.Enabled = True
        Else
            Button1.Text = "Update sensor readings"
        End If
    End Sub

    Private Sub Form1_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        Static bOnce As Boolean = False
        If bOnce Then Exit Sub
        bOnce = True
        Call Button1_Click(Me, Nothing)
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private WithEvents tWait As New Timer

    Private Sub cmdConnect_Click(sender As System.Object, e As System.EventArgs) Handles cmdConnect.Click
        Dim c As New FAHInterface.Client.ClientAccess
        c.Connect("192.168.1.201", 36330)
        c.SendCommand("auth " & TextBox3.Text)
        c.Update()
        c.SendCommand("updates add 0 5 $heartbeat")
        c.Update()

        c.SendCommand("info")
        c.Update()
        c.SendCommand("options -d")
        c.Update()
        c.SendCommand("queue-info")
        c.Update()
        c.SendCommand("slot-info")
        c.Update()
        tWait.Interval = 500
        c.SendCommand("log-updates start")
        c.Update()
        Do
            While tWait.Enabled
                Application.DoEvents()
            End While
            c.Update()
            tWait.Enabled = True
        Loop






        If cmdConnect.Text = "Connect" Then
            Dim strIP As String = Mid(txtHost.Text, 1, txtHost.Text.LastIndexOf(":"))
            Dim strPORT As String = txtHost.Text.Replace(strIP & ":", "")
            If FAHinterface.ConnectToClient(strIP, strPORT) Then
                If TextBox3.Text <> "" Then FAHinterface.SendCommand("auth " & TextBox3.Text, Nothing)



                cmdConnect.Text = "Disconnect"
            End If
        Else
            If FAHinterface.IsConnected(Nothing) Then
                FAHinterface.Disconnect(Nothing)
                If FAHinterface.IsConnected(Nothing) Then
                    cmdConnect.Text = "Disconnect"
                Else
                    cmdConnect.Text = "Connect"
                End If
            End If
        End If

    End Sub

     Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        ' Don't send command first as async op will cause problems
        ' txtOUT.AppendText(txtCMD.Text & vbNewLine)
        FAHinterface.SendCommand(txtCMD.Text, Nothing)
        txtCMD.Clear()
        txtCMD.Focus()
    End Sub

    Private Sub nudInt_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudInt.ValueChanged
        Timer1.Interval = nudInt.Value
    End Sub

    Private Sub SplitContainer1_Panel2_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer1.Panel2.Paint

    End Sub

    Private Sub SplitContainer1_Resize(sender As Object, e As System.EventArgs) Handles SplitContainer1.Resize
        Try
            Panel3.Dock = DockStyle.Top
            Panel2.Dock = DockStyle.Bottom
            rtfC.Top = Panel3.Top + Panel3.Height + 2
            rtfC.Height = Panel2.Top - (Panel3.Top + Panel3.Height + 2)
            rtfC.Width = Panel2.Width
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tWait_Tick(sender As Object, e As System.EventArgs) Handles tWait.Tick
        tWait.Enabled = False
    End Sub
End Class
