Imports System.Drawing.Imaging

Public Class frmAbout
    <Serializable()> Private Class clsFile
        Public FileName As String
        Public MD5Hash As String
        Public Version As String
        Public CreationTimeUTC As String
        Public HasEULA As Boolean
        Public EULA As String
    End Class
    Private Files As New List(Of clsFile)
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmAbout_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Static bOnce As Boolean = False
        If bOnce Then Exit Sub
        bOnce = True

        For Each File As clsFile In Files
            lbFiles.Items.Add(File.FileName.Replace(Application.StartupPath & "\", ""))
        Next
        If lbFiles.Items.Count > 0 Then
            lbFiles.SelectedIndex = 0
        End If
    End Sub

    Private Sub lbFiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFiles.SelectedIndexChanged
        Try
            Dim sFile As String = Application.StartupPath & "\" & lbFiles.Items(lbFiles.SelectedIndex).ToString
            Dim fInfo As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(sFile)
            lblVersion.Text = "Version: "
            lblCopyright.Text = "Copyright: "
            lblCreation.Text = "Creation time: "
            If fInfo.Exists Then
                lblVersion.Text &= FileVersionInfo.GetVersionInfo(sFile).FileVersion
                lblCopyright.Text &= FileVersionInfo.GetVersionInfo(sFile).LegalCopyright
                lblCreation.Text &= fInfo.CreationTime.ToString
            End If
            Dim fName As String = fInfo.Name.Replace(" .", "")
            fName = fName.Replace(fInfo.Extension, "")

            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\EULA\" & fName & ".txt") Then
                rtfEULA.LoadFile(Application.StartupPath & "\EULA\" & fName & ".txt", RichTextBoxStreamType.PlainText)
            Else
                rtfEULA.Clear()
            End If
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\CHANGELOG\" & fName & "\" & Application.ProductVersion.ToString & ".txt") Then
                rtfChangeLog.LoadFile(Application.StartupPath & "\CHANGELOG\" & fName & "\" & Application.ProductVersion.ToString & ".txt", RichTextBoxStreamType.PlainText)
            Else
                rtfChangeLog.Clear()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Enum eWizzard
        ConfirmCancelWizzard
        ConfirmExitForm
        NoConfirmExitForm
        ConfirmExitApplication
        NoConfirmExitApplication
    End Enum
    Public Enum eDisableButtons
        DisableMin
        DisableClose
        DisableHelp
        DisableMin_Close
        DisableMin_Help
        DisableHelp_Close
        DisableAll
        DisableNone
    End Enum
    Public Property ButtonDisabled As eDisableButtons = eDisableButtons.DisableNone
    Public Property CloseOption As eWizzard = eWizzard.NoConfirmExitApplication

    Private Sub frmAbout_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not bAllowClose Then
            e.Cancel = True
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
        End If
    End Sub
    Public Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub
    Private Sub frmUI_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Size = Me.BackgroundImage.Size
            Me.MinimumSize = Me.Size
            Me.MaximumSize = Me.Size
            Me.SetStyle(ControlStyles.UserPaint, True)
            'Handle image loading/placement here
            lblClose.Visible = False : lblHelp.Visible = False : lblMin.Visible = False
            If Not ButtonDisabled = eDisableButtons.DisableMin_Close And Not ButtonDisabled = eDisableButtons.DisableHelp_Close And Not ButtonDisabled = eDisableButtons.DisableClose Then
                ' Enable close button
                lblClose.Size = My.Resources.Close_button.Size : lblClose.Image = My.Resources.Close_button
                lblClose.Location = New Point(735, 8)
                lblClose.Visible = True
            ElseIf Not ButtonDisabled = eDisableButtons.DisableHelp_Close And Not ButtonDisabled = eDisableButtons.DisableMin_Help And Not ButtonDisabled = eDisableButtons.DisableHelp Then
                ' Enable help button
                lblHelp.Size = My.Resources.Help_button.Size : lblHelp.Image = My.Resources.Help_button
                lblHelp.Location = New Point(615, 8)
                lblHelp.Visible = True
            ElseIf Not ButtonDisabled = eDisableButtons.DisableMin And Not ButtonDisabled = eDisableButtons.DisableMin_Close And Not ButtonDisabled = eDisableButtons.DisableMin_Help Then
                ' Enable min button 
                lblMin.Size = My.Resources.Min_button.Size : lblMin.Image = My.Resources.Min_button
                lblMin.Location = New Point(675, 8)
                lblHelp.Visible = True
            End If
            ' Update handling

            Dim iCount As Int32 = 0
            For Each sFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchAllSubDirectories, {"*.exe", "*.dll", "*.sys", "*.config"})
                Dim nF As New clsFile
                nF.FileName = sFile
                nF.MD5Hash = MD5CalcFile(sFile)
                Dim fInfo As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(sFile)
                sFile = sFile.Replace(Application.StartupPath & "\", "")
                'lvfCheck.Items.Add(sFile)
                'lvfCheck.Items(iCount).SubItems.Add(nF.MD5Hash)
                If fInfo.Exists Then
                    'lvfCheck.Items(iCount).SubItems.Add(FileVersionInfo.GetVersionInfo(sFile).FileVersion)
                    'lvfCheck.Items(iCount).SubItems.Add(fInfo.CreationTime.ToString)
                    nF.Version = FileVersionInfo.GetVersionInfo(sFile).FileVersion
                    nF.CreationTimeUTC = (fInfo.CreationTimeUtc.ToString)
                Else
                    'lvfCheck.Items(iCount).SubItems.Add("") : lvfCheck.Items(iCount).SubItems.Add("")
                End If
                Dim eulaFile As String = Application.StartupPath & "\EULA\"
                If sFile.Contains("x64") Then sFile = sFile.Replace("x64", "")
                If sFile.Contains(".dll") Then
                    eulaFile &= sFile.Replace(".dll", "")
                ElseIf sFile.Contains(".DLL") Then
                    eulaFile &= sFile.Replace(".DLL", "")
                ElseIf sFile.Contains(".exe") Then
                    eulaFile &= sFile.Replace(".exe", "")
                ElseIf sFile.Contains(".EXE") Then
                    eulaFile &= sFile.Replace(".EXE", "")
                ElseIf sFile.Contains(".sys") Then
                    eulaFile &= sFile.Replace(".sys", "")
                ElseIf sFile.Contains(".SYS") Then
                    eulaFile &= sFile.Replace(".SYS", "")
                End If
                If My.Computer.FileSystem.FileExists(eulaFile & ".txt") Or My.Computer.FileSystem.FileExists(eulaFile & ".license") Then
                    If My.Computer.FileSystem.FileExists(eulaFile & ".txt") Then
                        eulaFile &= ".txt"
                    Else
                        eulaFile &= ".license"
                    End If
                    'lvfCheck.Items(iCount).SubItems.Add("Yes")
                    nF.HasEULA = True
                    nF.EULA = My.Computer.FileSystem.ReadAllText(eulaFile)
                Else
                    nF.HasEULA = False
                    'lvfCheck.Items(iCount).SubItems.Add("No")
                End If
                'lvfCheck.Items(iCount).Checked = True
                iCount += 1
                Files.Add(nF)
            Next
            iCount = Nothing


        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub

#Region " ClientAreaMove Handling and UI layout "
    Private Sub lblHelp_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblHelp.MouseEnter
        lblHelp.Image = My.Resources.Help_button_mouse_over
    End Sub
    Private Sub lblHelp_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblHelp.MouseLeave
        lblHelp.Image = My.Resources.Help_button
    End Sub
    Private Sub lblHelp_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblHelp.MouseDown
        lblHelp.Image = My.Resources.Help_button_mouse_down
    End Sub
    Private Sub lblHelp_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblHelp.MouseUp
        lblHelp.Image = My.Resources.Help_button_mouse_over
    End Sub

    Private Sub lblMin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.Click
        Me.Visible = False
    End Sub

    Private Sub lblMin_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.MouseEnter
        lblMin.Image = My.Resources.Min_button_mouse_over
    End Sub
    Private Sub lblMin_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.MouseLeave
        lblMin.Image = My.Resources.Min_button
    End Sub
    Private Sub lblMin_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblMin.MouseDown
        lblMin.Image = My.Resources.Min_button_mouse_down
    End Sub
    Private Sub lblMin_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblMin.MouseUp
        lblMin.Image = My.Resources.Min_button_mouse_over
    End Sub
    Private bAllowClose As Boolean = False
    Private Sub lblClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.Click
        Try
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lblClose_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.MouseEnter
        lblClose.Image = My.Resources.Close_button_mouse_over
    End Sub
    Private Sub lblClose_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.MouseLeave
        lblClose.Image = My.Resources.Close_button
    End Sub
    Private Sub lblClose_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblClose.MouseDown
        lblClose.Image = My.Resources.Close_button_mouse_down
    End Sub
    Private Sub lblClose_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblClose.MouseUp
        lblClose.Image = My.Resources.Close_button_mouse_over
    End Sub
    Protected Sub lblMin_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblMin.Paint
        Try
            Dim bmp As Bitmap = lblMin.Image
            Dim g As Graphics = Graphics.FromImage(bmp)
            Dim attr As New ImageAttributes
            g.DrawImage(lblMin.Image, New Point(0, 0))
            attr.SetColorKey(bmp.GetPixel(0, 0), bmp.GetPixel(0, 0))
            Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attr)
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub
    Protected Sub lblHelp_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblHelp.Paint
        Try
            Dim bmp As Bitmap = lblHelp.Image
            Dim g As Graphics = Graphics.FromImage(bmp)
            Dim Attr As New ImageAttributes
            Attr.SetColorKey(Color.White, Color.White)
            Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, Attr)
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub
    Protected Sub lblClose_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblClose.Paint
        Try
            Dim bmp As Bitmap = lblClose.Image
            Dim g As Graphics = Graphics.FromImage(bmp)
            Dim Attr As New ImageAttributes
            Attr.SetColorKey(Color.White, Color.White)
            Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, Attr)
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
                'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
            Case Else
                'Make sure you pass unhandled messages back to the default message handler.
                MyBase.WndProc(m)
        End Select
    End Sub
#End Region
End Class