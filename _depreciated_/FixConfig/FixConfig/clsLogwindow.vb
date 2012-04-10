'/*
' * FAHWatch7  Copyright Marvin Westmaas ( mtm )
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
'/*	
Imports System.IO
Imports System.Text

Public Class clsLogwindow
    Private WithEvents _Form As New frmLogWindow
    Private WithEvents Nicon As New NotifyIcon
    Private WithEvents Menu As New ContextMenuStrip
    Public fileLOG As String = ""
    Public Enum eMenuOptions
        eDefault
        eClose
    End Enum
    Public MenuOptions As eMenuOptions = eMenuOptions.eDefault
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
            '_Form.Icon = My.Resources.fTray_Log
            _Form.nIcon.Text = Application.ProductName & Environment.NewLine & Application.ProductVersion.ToString
            Dim _dtCutOff As DateTime = DateTrim
            If FileName = "" Then
                FileName = Application.StartupPath & "\" & Application.ProductName & ".log"
            Else
                FileName = FileName
            End If
            fileLOG = FileName
            If DateTrim = #1/1/2000# Then
                _dtCutOff = DateTime.Now.Subtract(TimeSpan.FromDays(7))
            Else
                _dtCutOff = DateTrim
            End If

            If My.Computer.FileSystem.FileExists(FileName) Then
                ' Don't store history from parsing?

                My.Computer.FileSystem.DeleteFile(FileName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)


                'Dim fStream As FileStream = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                'Dim fRead As StreamReader = New StreamReader(fStream)
                'Dim nText As String = fRead.ReadToEnd
                'fStream.Close()
                'fRead.Close()
                'fStream = Nothing
                'fRead = Nothing
                ''Get last line with datecutoff ( doesn't work, convert each line to dtobj 
                'Dim Lines() As String = nText.Split(Environment.NewLine)
                'Dim lCutOff As Long = -1
                'For xInt As Int32 = Lines.GetLowerBound(0) To Lines.GetUpperBound(0) Step 1
                '    Lines(xInt).Replace(Environment.NewLine, "")
                '    If Lines(xInt).Contains(" * ") AndAlso Lines(xInt).Contains(" - ") AndAlso Lines(xInt).Contains("[") AndAlso Lines(xInt).Contains("]") Then
                '        While Not Lines(xInt).Substring(0, 1) = "["
                '            Lines(xInt) = Lines(xInt).Substring(1)
                '        End While
                '        Dim lDT As DateTime = DateTime.Parse(Lines(xInt).Substring(1, Lines(xInt).IndexOf(" - ")).Replace("]", "").Replace("*", "").Replace("[", ""))
                '        If lDT <= _dtCutOff Then
                '            lCutOff = CLng(xInt)
                '        Else
                '            Exit For
                '        End If
                '    End If
                'Next
                'If lCutOff <> -1 Then
                '    Dim sb As New StringBuilder
                '    For xInt As Int32 = CInt(lCutOff) To Lines.Count - 1
                '        If Lines(xInt).Contains(" * ") AndAlso Lines(xInt).Contains(" - ") AndAlso Lines(xInt).Contains("[") AndAlso Lines(xInt).Contains("]") Then
                '            If Lines(xInt).Contains(Environment.NewLine) Then Lines(xInt) = Lines(xInt).Replace(Environment.NewLine, "")
                '            If Lines(xInt).Contains(Chr(10)) Then Lines(xInt) = Lines(xInt).Replace(Chr(10), "")
                '            _Form.rtLog.Text &= Lines(xInt) & Environment.NewLine
                '        End If
                '    Next
                '    'Dim nF As New Form
                '    'nF.Controls.Add(New RichTextBox)
                '    'With CType(nF.Controls(0), RichTextBox)
                '    '    .Multiline = True
                '    '    .Text = _Form.rtLog.Text
                '    '    .Dock = DockStyle.Fill
                '    'End With
                '    'Application.Run(nF)
                'Else
                '    _Form.rtLog.Text = nText
                'End If


            End If
            ' TODO remove after diagnostics
            WriteLog("****** " & Application.ProductName & " " & My.Application.Info.Version.ToString & " logging started ******")
            Return Me
        Catch ex As Exception
            Return Me
        End Try
    End Function
    Delegate Sub Addoutput(ByVal [text] As String)
    Delegate Sub AddEmptyLine()
    Private Sub DoEmpty()
        Try
            _Form.rtLog.Text &= Environment.NewLine
        Catch ex As Exception

        End Try
    End Sub
    Public Sub WriteLog(ByVal Message As String)
        Try
            If _Form.rtLog.InvokeRequired Then
                Dim nInvoke As New Addoutput(AddressOf wInv)
                _Form.rtLog.Invoke(nInvoke, New Object() {[Message]})
            Else
                _Form.rtLog.Text &= ("[" & DateTime.Now.ToShortDateString & "] * " & DateTime.Now.ToLongTimeString & " - " & Message & Environment.NewLine)
                Application.DoEvents()
                _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
                If Not _Form.Focused AndAlso Not _Form.Visible Then Exit Sub
                With _Form.rtLog
                    .SelectionLength = 0
                    .SelectionStart = .TextLength
                    .ScrollToCaret()
                End With
                If Not fileLOG.ToUpper.Contains("DEBUG") Then _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
                Application.DoEvents()
            End If
        Catch ex As Exception
            End
        End Try
    End Sub
    Public Sub wInv(ByVal Message As String)
        _Form.rtLog.Text &= ("[" & DateTime.Now.ToShortDateString & "] * " & DateTime.Now.ToLongTimeString & " - " & Message & Environment.NewLine)
        Application.DoEvents()
        _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
        If Not _Form.Focused AndAlso Not _Form.Visible Then Exit Sub
        With _Form.rtLog
            .SelectionLength = 0
            .SelectionStart = .TextLength
            .ScrollToCaret()
        End With
        If Not fileLOG.ToUpper.Contains("DEBUG") Then _Form.rtLog.SaveFile(fileLOG, RichTextBoxStreamType.PlainText)
        Application.DoEvents()
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
    Public Overloads Sub ShowDebugWindow(ByVal TheIcon As Icon, Optional ByVal ExitApp As Boolean = False)
        Try
            Nicon.Icon = TheIcon
            _Form.Icon = Nicon.Icon
            _Form.ScrollToEnd()
            _Form.Show()
            _Form.Focus()
            _Form.ExitApp = ExitApp
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
    Public ReadOnly Property Form As frmLogWindow
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




End Class
