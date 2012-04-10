'   fTray Log window class
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
    Public fileLOG As String = ""
    Public Enum TrayIcon
        Log
        Warning
    End Enum
    Public Function CreateLog(Optional ByVal FileName As String = "", Optional ByVal DateTrim As DateTime = #1/1/2000#, Optional ByVal ShowIcon As Boolean = False) As clsLogwindow
        Try
            Nicon = _Form.nIcon
            _Form.rtLog.Clear()
            _Form.rtLog.MaxLength = Int32.MaxValue
            _Form.Icon = My.Resources.fTray_Log
            Dim _dtCutOff As DateTime = DateTrim
            If FileName = "" Then
                FileName = Application.StartupPath & "\fTray.log"
            Else
                FileName = FileName
            End If
            fileLOG = FileName
            If DateTrim = #1/1/2000# Then
                _dtCutOff = DateTime.Now.Subtract(TimeSpan.FromHours(2))
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
                If nText.Contains(DateTrim) Then
                    Dim iTrim As Int32 = InStr(nText.LastIndexOf(DateTrim), nText, vbNewLine, CompareMethod.Text)
                    nText = Mid(nText, iTrim + 1)
                End If
                _Form.rtLog.Text = nText
            End If
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
                _Form.rtLog.Text &= vbNewLine
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DoEmpty()
        Try
            _Form.rtLog.Text &= vbNewLine
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WriteLog(ByVal Message As String)
        Try
            If _Form.rtLog.InvokeRequired Then
                Dim nInvoke As New Addoutput(AddressOf wInv)
                _Form.rtLog.Invoke(nInvoke, New Object() {[Message]})
            Else
                _Form.rtLog.Text &= "[" & DateTime.Now.ToShortDateString & "] - " & DateTime.Now.ToLongTimeString & " - " & Message & vbNewLine
                With _Form.rtLog
                    .SelectionLength = 0
                    .SelectionStart = .TextLength
                    .ScrollToCaret()
                End With
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub wInv(ByVal Message As String)
        _Form.rtLog.Text &= "-[" & DateTime.Now.ToShortDateString & "]-" & " " & DateTime.Now.ToShortTimeString & " : " & Message & vbNewLine
        With _Form.rtLog
            .SelectionLength = 0
            .SelectionStart = .TextLength
            .ScrollToCaret()
        End With
    End Sub
    Public Sub WriteError(ByVal Location As String, ByVal ErrObject As ErrObject, Optional ByVal ExtraInfo As String = "")
        WriteLog("Error at " & Location)
        If ExtraInfo.Length <> 0 Then WriteLog(" - Extra info: " & ExtraInfo)
        WriteLog(" - Err.source: " & ErrObject.Source & " Line: " & ErrObject.Erl & " - Err.number : " & ErrObject.Number)
        WriteLog(" - Err.description: " & ErrObject.Description)
    End Sub
    Public Sub ShowIcon(ByVal tIcon As TrayIcon)
        If tIcon = TrayIcon.Log Then
            _Form.nIcon.Icon = My.Resources.fTray_Log
        Else
            _Form.nIcon.Icon = My.Resources.fTray_Warning
        End If
        _Form.nIcon.Visible = True
    End Sub
    Public Sub ShowDebugWindow(ByVal tIcon As TrayIcon)
        Try
            If tIcon = TrayIcon.Log Then
                _Form.nIcon.Icon = My.Resources.fTray_Log
            Else
                _Form.nIcon.Icon = My.Resources.fTray_Warning
            End If
            _Form.Show()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub HideDebugWindow()
        _Form.Visible = False
        _Form.nIcon.Visible = False
    End Sub
    Public ReadOnly Property IsWindowActive() As Boolean
        Get
            If _Form.Visible Or _Form.nIcon.Visible Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public Sub HideIcon()
        frmLogWindow.nIcon.Visible = False
    End Sub
    Public bAllowClose As Boolean = False
    Private Sub _Form_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles _Form.FormClosing
        If Not bAllowClose Then
            e.Cancel = True
            _Form.Visible = False
            _Form.nIcon.Visible = True
        Else
            _Form.nIcon.Visible = False
        End If
    End Sub
    Private Sub _Form_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles _Form.Resize
        If _Form.WindowState = FormWindowState.Minimized Then
            _Form.Visible = False
            _Form.nIcon.Visible = True
        End If
    End Sub
    Private Sub Nicon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Nicon.MouseClick
        _Form.Visible = True
        _Form.nIcon.Visible = False
        _Form.Show()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
