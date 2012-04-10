'   finfo Log window class
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
Imports System.IO
Public Class clsLogwindow
    Private WithEvents _Form As New frmLogWindow
    Private WithEvents Nicon As New NotifyIcon
    Private WithEvents Menu As New ContextMenuStrip
    Public fileLOG As String = ""
    Public Enum TrayIcon
        Log
        Warning
    End Enum
    Public Function CreateLog(Optional ByVal FileName As String = "", Optional ByVal DateTrim As DateTime = #1/1/2000#, Optional ByVal ShowIcon As Boolean = False) As clsLogwindow
        Try
            Nicon = _Form.nIcon
            Nicon.ContextMenuStrip = Menu
            _Form.rtLog.Clear()
            _Form.rtLog.MaxLength = Int32.MaxValue
            _Form.Icon = My.Resources.fTray_Log
            _Form.nIcon.Text = Application.ProductName & Environment.NewLine & Application.ProductVersion.ToString
            Dim _dtCutOff As DateTime = DateTrim
            If FileName = "" Then
                FileName = Application.StartupPath & "\" & Application.ProductName & ".log"
            Else
                FileName = FileName
            End If
            fileLOG = FileName
            If DateTrim = #1/1/2000# Then
                _dtCutOff = DateTime.Now.Subtract(TimeSpan.FromDays(2))
            Else
                _dtCutOff = DateTrim
            End If

            If My.Computer.FileSystem.FileExists(FileName) Then
                Dim fStream As FileStream = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                Dim fRead As StreamReader = New StreamReader(fStream)
                Dim nText As String = fRead.ReadToEnd
                fStream.Close()
                fRead.Close()
                fStream = Nothing
                fRead = Nothing
                'Get last line with datecutoff
                If nText.Contains("[" & DateTrim.ToShortDateString & "] - " & DateTrim.ToLongTimeString & " - ") Then
                    Dim iTrim As Int32 = InStr(nText.LastIndexOf("[" & DateTrim.ToShortDateString & "] - " & DateTrim.ToLongTimeString & " - "), nText, Environment.NewLine, CompareMethod.Text)
                    nText = Mid(nText, iTrim + 1)
                End If
                _Form.rtLog.Text = nText
            End If
            EmptyLine()
            WriteLog(Application.ProductName & " " & Application.ProductVersion & " logging started")
            EmptyLine()
            Return Me

        Catch ex As Exception
            Return Me
        End Try
    End Function
    Delegate Sub Addoutput(ByVal [text] As String)
    Delegate Sub AddEmptyLine()
    Public Sub EmptyLine()
        Try
            If _Form.rtLog.InvokeRequired Then
                Dim dE As New AddEmptyLine(AddressOf DoEmpty)
                _Form.rtLog.Invoke(dE)
            Else
                _Form.rtLog.Text &= environment.newline
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DoEmpty()
        Try
            _Form.rtLog.Text &= environment.newline
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WriteLog(ByVal Message As String)
        Try
            If _Form.rtLog.InvokeRequired Then
                Dim nInvoke As New Addoutput(AddressOf wInv)
                _Form.rtLog.Invoke(nInvoke, New Object() {[Message]})
            Else
                _Form.rtLog.Text &= "[" & DateTime.Now.ToShortDateString & "] - " & DateTime.Now.ToLongTimeString & " - " & Message & environment.newline
                With _Form.rtLog
                    .SelectionLength = 0
                    .SelectionStart = .TextLength
                    .ScrollToCaret()
                End With
                _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub wInv(ByVal Message As String)
        _Form.rtLog.Text &= "[" & DateTime.Now.ToShortDateString & "] - " & DateTime.Now.ToLongTimeString & " - " & Message & environment.newline
        With _Form.rtLog
            .SelectionLength = 0
            .SelectionStart = .TextLength
            .ScrollToCaret()
        End With
        _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
    End Sub
    Public Overloads Sub WriteError(ByVal Location As String, ByVal ErrObject As ErrObject, Optional ByVal ExtraInfo As String = "")
        Try
            WriteLog("Error at " & Location)
            WriteLog(" - Err.source: " & ErrObject.Source & " Line: " & ErrObject.Erl & " - Err.number : " & ErrObject.Number)
            WriteLog(" - Err.description: " & ErrObject.Description)
            If ExtraInfo.Length <> 0 Then WriteLog(" - Extra info: " & ExtraInfo)
            WriteLog(" - stacktrace: " & ErrObject.GetException.StackTrace.ToString)
            WriteLog(" - " & Err.GetException.StackTrace.ToString)
        Catch ex As Exception

        End Try
    End Sub
    Public Overloads Sub WriteError(ByVal Source As Object, ByVal ErrObj As ErrObject, Optional ByVal ExtraInfo As String = "")
        Try
            WriteLog("Error at " & Source.ToString)
            WriteLog(" - Err.source: " & ErrObj.Source & " Line: " & ErrObj.Erl & " - Err.number : " & ErrObj.Number)
            WriteLog(" - Err.description: " & ErrObj.Description)
            If ExtraInfo.Length <> 0 Then WriteLog(" - Extra info: " & ExtraInfo)
            WriteLog(" - " & Err.GetException.StackTrace.ToString)
        Catch ex As Exception

        End Try
    End Sub
    Private _Warning As Boolean = False
    Public Sub ShowIcon(ByVal tIcon As TrayIcon)
        If tIcon = TrayIcon.Log Then
            _Form.nIcon.Icon = My.Resources.fTray_Log
            Nicon.Icon = My.Resources.fTray_Log
            _Warning = False
        Else
            _Form.nIcon.Icon = My.Resources.fTray_Warning
            Nicon.Icon = My.Resources.fTray_Warning
            _Warning = True
        End If
        _Form.nIcon.Visible = True
    End Sub
    Public Sub ShowDebugWindow(Optional ByVal tIcon As TrayIcon = TrayIcon.Log)
        Try
            If tIcon = TrayIcon.Log Then
                _Form.nIcon.Icon = My.Resources.fTray_Log
                Nicon.Icon = My.Resources.fTray_Log
            Else
                _Form.nIcon.Icon = My.Resources.fTray_Warning
                Nicon.Icon = My.Resources.fTray_Warning
            End If
            _Form.Show()
            _Form.Focus()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub HideDebugWindow()
        _Form.Visible = False
    End Sub
    Public ReadOnly Property IsWindowActive() As Boolean
        Get
            Return _Form.Visible
        End Get
    End Property
    Public Sub HideIcon()
        frmLogWindow.nIcon.Visible = False
    End Sub
    Public ReadOnly Property ActiveWarning As Boolean
        Get
            Return _Warning
        End Get
    End Property
    Public bAllowClose As Boolean = False
    Public Sub CloseLog()
        WriteLog("Closing logging class")
        _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
        _Form.CloseForm()
    End Sub
    Public ReadOnly Property Form As Form
        Get
            Return _Form
        End Get
    End Property
    Private Sub _Form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Form.Resize
        If _Form.WindowState = FormWindowState.Minimized Then
            _Form.Hide()
        End If
    End Sub
    Private Sub Nicon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Nicon.MouseClick
        
        '_Form.nIcon.Visible = False
        '_Form.Show()
       


    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Nicon_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Nicon.MouseDown
        Try
            If e.Button = MouseButtons.Right Then
                ' POPUP Menu
                Menu.Items.Clear()
                If LogWindow.IsWindowActive Then
                    Menu.Items.Add("Hide log messages")
                Else
                    Menu.Items.Add("Show log messages")
                End If
                If Control.IsFormActive Then
                    Menu.Items.Add("Hide control form")
                Else
                    Menu.Items.Add("Show control form")
                End If
                If ExtClient.ProjectInfo.IsFormActive Then
                    Menu.Items.Add("Hide ProjectBrowser")
                Else
                    Menu.Items.Add("Show ProjectBrowser")
                End If
                Menu.Items.Add("-")
                If Settings.IsFormActive Then
                    Menu.Items.Add("Hide settings")
                Else
                    Menu.Items.Add("Show settings")
                End If
                If About.IsFormActive Then
                    Menu.Items.Add("Hide 'About'")
                Else
                    Menu.Items.Add("About")
                End If
                Menu.Items.Add("Exit")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Menu_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles Menu.ItemClicked
        Try
            Select Case e.ClickedItem.Text
                Case "Hide log messages"
                    LogWindow.HideDebugWindow()
                Case "Show log messages"
                    LogWindow.ShowDebugWindow(TrayIcon.Log)
                Case "Hide control form"
                    Control.HideForm()
                Case "Show control form"
                    Control.ShowForm()
                Case "Show ProjectBrowser"
                    ExtClient.ProjectInfo.ShowBrowser()
                Case "Hide ProjectBrowser"
                    ExtClient.ProjectInfo.HideBrowser()
                Case "Hide settings"
                    Settings.HideSettings()
                Case "Show settings"
                    Settings.ShowSettings()
                Case "About"
                    About.showForm()
                Case "Hide 'About'"
                    About.hideForm()
                Case "Exit"
                    ExitApplication()
            End Select
        Catch ex As Exception

        End Try

    End Sub
End Class
