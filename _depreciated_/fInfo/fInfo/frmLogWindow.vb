'/*
' * fInfo LogWindow class Copyright Marvin Westmaas ( mtm ) 
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
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.RichTextBox
Public Class frmLogWindow
    Private bAllowClose As Boolean = False
    Public ReadOnly Property AllowClose As Boolean
        Get
            Return bAllowClose
        End Get
    End Property
    Public Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub
    Private Sub frmLogWindow_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            rtLog.SaveFile(LogWindow.fileLOG, RichTextBoxStreamType.PlainText)
            If Not bAllowClose Then
                e.Cancel = True
                Me.WindowState = FormWindowState.Minimized
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub rtLog_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtLog.MouseDoubleClick
        Try

            If Me.TopMost Then
                Me.TopMost = False
                Me.Text = Me.Text.Replace("Topmost", "")
            Else
                Me.Text = Me.Text & " Topmost"
                Me.TopMost = True
            End If

            If LogWindow.ActiveWarning Then
                Dim rVal As MsgBoxResult
                rVal = MsgBox("Do you want to clear the warning icon?", vbYesNo + MsgBoxStyle.Information, "Clear warning notification")
                If rVal = MsgBoxResult.No Then Exit Sub
                LogWindow.ShowIcon(clsLogwindow.TrayIcon.Log)

            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmLogWindow_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Try
            If rtLog.TextLength > 0 Then
                rtLog.SelectionStart = rtLog.TextLength
                rtLog.ScrollToCaret()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rtLog_TextChanged(sender As System.Object, e As System.EventArgs) Handles rtLog.TextChanged

    End Sub
End Class

