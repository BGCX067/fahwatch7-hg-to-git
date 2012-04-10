'   EUE form
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
Public Class frmEUE
    Private _cStatus As clsQueue.clsCoreStatus
    Private Sub rt_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles rt.LinkClicked
        Process.Start(e.LinkText)
    End Sub
    Public Sub NotifyEUE(ByVal cStatus As clsQueue.clsCoreStatus)
        Try
            Me.Hide()
            If cStatus.IsEmpty Then Exit Sub
            _cStatus = cStatus
            rt.Text &= "Client: " & cStatus.ClientLocation & vbNewLine
            rt.Text &= "Core status: " & vbTab & cStatus.CoreStatus & vbNewLine
            rt.Text &= "Project: " & vbTab & cStatus.cEntry.Project.Project & " (Run " & cStatus.cEntry.Project.Run & " Clone " & cStatus.cEntry.Project.Clone & " Gen " & cStatus.cEntry.Project.Gen & ")" & vbNewLine
            rt.Text &= "Issued: " & vbTab & cStatus.cEntry.Issued.ToShortDateString & " " & cStatus.cEntry.Issued.ToLongTimeString & vbNewLine
            rt.Text &= "End time (eue):" & vbTab & cStatus.cEntry.TimeData.EndTime.ToShortDateString & " " & cStatus.cEntry.TimeData.EndTime.ToLongTimeString & vbNewLine & vbNewLine
            For Each sLine As String In cStatus.LogSnippet
                rt.Text &= sLine & vbNewLine
            Next
            nIcon.Text = cStatus.ClientLocation & vbNewLine & cStatus.CoreStatus
            nIcon.BalloonTipIcon = ToolTipIcon.Warning
            nIcon.BalloonTipTitle = cStatus.CoreStatus
            nIcon.BalloonTipText = cStatus.ClientLocation
            nIcon.ShowBalloonTip(5000)
        Catch ex As Exception
            Me.Finalize()
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If My.Computer.FileSystem.FileExists(_cStatus.ClientLocation & "\fahlog.txt") Then Process.Start(_cStatus.ClientLocation & "\fahlog.txt")
    End Sub
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start(_cStatus.ClientLocation)
    End Sub

    Private Sub nIcon_BalloonTipClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles nIcon.BalloonTipClicked
        Me.Show()
    End Sub
    Private Sub nIcon_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nIcon.DoubleClick
        Me.Show()
    End Sub
    Private Sub nIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseDoubleClick
        Me.Show()
    End Sub
    Private Sub frmEUE_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        rt.Height = Me.ClientRectangle.Height - LinkLabel1.Height - 5
    End Sub
End Class