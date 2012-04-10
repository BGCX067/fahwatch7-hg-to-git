Imports FAHInterface.FAHInterface
Imports FAHInterface.Client

Public Class Form1
    Private WithEvents FAHinterface As New clsFAHInterface

    Public Delegate Sub AppendText(ByVal Text As String)

    Public Sub Append(ByVal text As String)
        If txtOUT.InvokeRequired Then
            Dim nAP As New AppendText(AddressOf Append)
            txtOUT.Invoke(nAP, {text})
        Else
            txtOUT.Clear()
            txtOUT.WordWrap = False
            If text.Contains("{") Then

            End If
            txtOUT.AppendText(text & vbNewLine)
            txtOUT.SelectionStart = txtOUT.TextLength
            txtOUT.ScrollToCaret()
        End If
    End Sub

    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Static bOnce As Boolean = False
        If bOnce Then Exit Sub
        bOnce = True
        Call cmdConnect_Click(Me, Nothing)
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub cmdConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConnect.Click
        Dim strIP As String = Mid(txtHost.Text, 1, txtHost.Text.LastIndexOf(":"))
        Dim strPORT As String = txtHost.Text.Replace(strIP & ":", "")
        If FAHinterface.ConnectToClient(strIP, strPORT) Then
            cmdDisconnect.Enabled = True
            cmdConnect.Enabled = False
        End If

    End Sub

    Private Sub FAHinterface_ResponceRecieved(ByVal Response As String) Handles FAHinterface.ResponseRecieved
        Append(Response)
    End Sub

    Private Sub cmdSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        ' Don't send command first as async op will cause problems
        ' txtOUT.AppendText(txtCMD.Text & vbNewLine)
        FAHinterface.SendCommand(txtCMD.Text, Nothing)
        txtCMD.Clear()
        txtCMD.Focus()
    End Sub

    Private Sub cmdDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDisconnect.Click
        If FAHinterface.IsConnected(Nothing) Then
            FAHinterface.Disconnect(Nothing)
            If FAHinterface.IsConnected(Nothing) Then
                cmdDisconnect.Enabled = True
                cmdConnect.Enabled = False
            Else
                cmdConnect.Enabled = True
                cmdDisconnect.Enabled = False
            End If
        End If
    End Sub

    Private Sub txtCMD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCMD.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call cmdSend_Click(Me, Nothing)
        End If
    End Sub

    Private Sub txtCMD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCMD.KeyPress
        If e.KeyChar = vbNewLine Then

        End If
    End Sub

    Private Sub txtCMD_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCMD.TextChanged

    End Sub

    Private Sub cmNotes_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmNotes.ItemClicked
        Try
            Select Case e.ClickedItem.Text
                Case Is = "Save"
                    If tcNotes.SelectedTab.Text <> "New" Then
                        My.Computer.FileSystem.WriteAllText(Application.StartupPath & tcNotes.SelectedTab.Text, rtNote.Text, False)
                    Else
                        GoTo saveAs
                    End If
                Case Is = "Save as"
saveAs:
                    Using sDia As New SaveFileDialog
                        sDia.FileName = "New.txt"
                        'sDia.InitialDirectory = Application.StartupPath & "\Notes\"
                        sDia.InitialDirectory = Application.StartupPath
                        sDia.DefaultExt = "txt"
                        If sDia.ShowDialog Then
                            My.Computer.FileSystem.WriteAllText(sDia.FileName, rtNote.Text, False)
                            tcNotes.SelectedTab.Text = sDia.FileName.Substring(sDia.FileName.LastIndexOf("\") + 1)
                        End If
                    End Using
                Case Is = "Load"
                    Using lDia As New OpenFileDialog
                        lDia.DefaultExt = "txt"
                        lDia.CheckFileExists = True
                        lDia.CheckPathExists = True
                        lDia.InitialDirectory = Application.StartupPath
                        If lDia.ShowDialog Then
                            rtNote.LoadFile(lDia.FileName)
                        End If
                    End Using
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmNotes_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles cmNotes.Opening

    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Try
            Dim nTab As New TabPage
            nTab.Text = "New"
            nTab.Controls.Add(New RichTextBox)
            With nTab.Controls(0)
                .Dock = DockStyle.Fill
                .ContextMenuStrip = cmNotes
            End With
            tcNotes.TabPages.Add(nTab)
        Catch ex As Exception

        End Try
       
    End Sub

    Private Sub LoadToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        Try
            Using lDia As New OpenFileDialog
                lDia.DefaultExt = "*.txt"
                lDia.InitialDirectory = Application.StartupPath
                If lDia.ShowDialog Then
                    Dim nTab As New TabPage
                    nTab.Text = lDia.FileName.Substring(lDia.FileName.LastIndexOf("\") + 1)
                    Dim nRT As New RichTextBox
                    With nRT
                        .Dock = DockStyle.Fill
                        .ContextMenuStrip = cmNotes
                        .Text = My.Computer.FileSystem.ReadAllText(lDia.FileName)
                    End With
                    nTab.Controls.Add(nRT)
                    tcNotes.TabPages.Add(nTab)
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim c As New FAHInterface.Client.ClientAccess
        c.Connect("192.168.1.200", 36330)
        c.SendCommand("auth marvin")
        c.SendCommand("info")
        c.SendCommand("options -d")
        c.SendCommand("queue-info")
        c.Update()
        MsgBox(Options.Checkpoint)

        txtCMD.Text = "info"
        cmdSend.PerformClick()

    End Sub
End Class
