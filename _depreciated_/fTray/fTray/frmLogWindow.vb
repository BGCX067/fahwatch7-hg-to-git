'   fTray LogWindow class
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
Public Class frmLogWindow

    Private Sub frmLogWindow_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            rtLog.SaveFile(LogWindow.fileLOG, RichTextBoxStreamType.PlainText)
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
End Class