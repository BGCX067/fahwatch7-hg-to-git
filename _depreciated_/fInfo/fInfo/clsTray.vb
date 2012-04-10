Public Class clsTray
    Private WithEvents iMain As New NotifyIcon
    Private WithEvents iLog As New NotifyIcon
    Private WithEvents iWarning As New NotifyIcon
    Private WithEvents iEOC As New NotifyIcon
    Private WithEvents cmMAIN As New ContextMenuStrip
    Private mainForm As frmWizzard = Nothing
    Public Property mForm As frmWizzard
        Get
            Return mainForm
        End Get
        Set(ByVal value As frmWizzard)
            mainForm = value
        End Set
    End Property
    Public Sub New()
        iMain.Visible = False
        iMain.ContextMenuStrip = cmMAIN
        iLog.Visible = False
        iWarning.Visible = False
        iEOC.Visible = False
        iLog.Icon = My.Resources.cftUnity.fTray_Log
        iWarning.Icon = My.Resources.cftUnity.fTray_Warning
        iEOC.Icon = My.Resources.cftUnity.fTray_EOC
        iMain.Icon = My.Resources.cftUnity.fTray
        iMain.Text = Application.ProductName & " " & Application.ProductVersion.ToString
    End Sub

    Public Sub ShowMainIcon(ByVal tIcon As Icon, ByVal Tooltip As String)
        If iMain.Visible Then iMain.Visible = False
        iMain.Icon = tIcon
        iMain.Text = Tooltip
        iMain.Visible = True
    End Sub
    Public Sub ShowMainForm()
        If Not UI.Visible Then UI.Show()
        UI.Focus()
    End Sub
    Public Sub HideMainForm()
        UI.Hide()
    End Sub
    Public Sub MShowBalloon(ByVal bIcon As ToolTipIcon, ByVal bTitle As String, ByVal bText As String, Optional ByVal Duration As Integer = 5000)
        iMain.ShowBalloonTip(Duration, bTitle, bText, bIcon)
    End Sub
    Public Sub MCloseBalloon()
        iMain.ShowBalloonTip(0)
    End Sub
    Public Sub CloseAll()
        iMain.Visible = False
        iEOC.Visible = False
        iWarning.Visible = False
        iLog.Visible = False
    End Sub

    Private Sub cmMAIN_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmMAIN.ItemClicked

        Select Case e.ClickedItem.Text
            Case Is = "&Close"
                HandleClose(UI)
            Case Is = "&About"
                frmAbout.ButtonDisabled = frmAbout.eDisableButtons.DisableMin_Help
                frmAbout.CloseOption = frmAbout.eWizzard.NoConfirmExitForm
                frmAbout.Show()
                frmAbout.Focus()
            Case Is = "&Show"
                ShowMainForm()
            Case Is = "&Hide"
                HideMainForm()
        End Select

    End Sub
    Public Function HandleClose(ByVal TheForm As frmWizzard) As Boolean

        ' Handle closing decisions here 
        Select Case TheForm.CloseOption
            Case Is = frmWizzard.eWizzard.ConfirmCancelWizzard
                Dim rVal As MsgBoxResult = MsgBox("Cancel the wizzard?", vbOKCancel, "Exit wizzard and quit cftUnity?")
                If rVal = vbOK Then
                    Application.Exit()
                Else
                    Return False
                End If
            Case Is = frmWizzard.eWizzard.ConfirmExitApplication
                Dim rVal As MsgBoxResult = MsgBox("Exit cftUnity?", vbOKCancel, "Confirm exit")
                If rVal = vbOK Then
                    Application.Exit()
                Else
                    Return False
                End If
            Case Is = frmWizzard.eWizzard.NoConfirmExitApplication
                Application.Exit()
            Case Is = frmWizzard.eWizzard.ConfirmExitForm
                Dim rVal As MsgBoxResult = MsgBox("Close ?", vbOKCancel, "Confirm form close")
                Return (rVal = vbOK)
            Case Is = frmWizzard.eWizzard.NoConfirmExitForm
                'Just close 
                Return True
            Case Else
                Return True
        End Select
    End Function

    Private Sub iMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles iMain.MouseDown
        Try
            'Dynamic menu creation
            cmMAIN.Items.Clear()
            With cmMAIN
                ' Add other options here
                If mainForm.Visible Then
                    .Items.Add("&Hide")
                Else
                    .Items.Add("&Show")
                End If
                ' Default items
                If .Items.Count > 0 Then .Items.Add("-")
                .Items.Add("&About")
                .Items.Add("&Close")
            End With
        Catch ex As Exception

        End Try
    End Sub

    Function HandleClose(ByVal TheForm As frmAbout) As Boolean

        ' Handle closing decisions here 
        Select Case TheForm.CloseOption
            Case Is = frmWizzard.eWizzard.ConfirmCancelWizzard
                Dim rVal As MsgBoxResult = MsgBox("Cancel the wizzard?", vbOKCancel, "Exit wizzard and quit cftUnity?")
                If rVal = vbOK Then
                    Application.Exit()
                Else
                    Return False
                End If
            Case Is = frmWizzard.eWizzard.ConfirmExitApplication
                Dim rVal As MsgBoxResult = MsgBox("Exit cftUnity?", vbOKCancel, "Confirm exit")
                If rVal = vbOK Then
                    Application.Exit()
                Else
                    Return False
                End If
            Case Is = frmWizzard.eWizzard.NoConfirmExitApplication
                Application.Exit()
            Case Is = frmWizzard.eWizzard.ConfirmExitForm
                Dim rVal As MsgBoxResult = MsgBox("Close ?", vbOKCancel, "Confirm form close")
                Return (rVal = vbOK)
            Case Is = frmWizzard.eWizzard.NoConfirmExitForm
                'Just close 
                Return True
            Case Else
                Return True
        End Select
    End Function

End Class