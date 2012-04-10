'   Project browser form
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
Public Class frmPBList
    Public Function FillView() As Boolean
        Try
            lvProjects.Items.Clear()
            For xInt As Int16 = 1 To ProjectInfo.Projects.ProjectCount
                Dim Project As clsProjectInfo.sProject.clsProject = ProjectInfo.Projects.Project(ProjectInfo.Projects.ProjectNumber(xInt))
                Dim nItem As New ListViewItem
                nItem.Text = Project.ProjectNumber
                nItem.SubItems.Add(Project.ServerIP)
                nItem.SubItems.Add(Project.WUName)
                nItem.SubItems.Add(Project.NumberOfAtoms)
                nItem.SubItems.Add(Project.PreferredDays)
                nItem.SubItems.Add(Project.FinalDeadline)
                nItem.SubItems.Add(Project.Credit)
                nItem.SubItems.Add(Project.Frames)
                nItem.SubItems.Add(Project.Code)
                nItem.SubItems.Add("-link-")
                nItem.SubItems.Add(Project.Contact)
                nItem.SubItems.Add(Project.kFactor)
                lvProjects.Items.Add(nItem)
                nItem = Nothing
                Project = Nothing
            Next
            lvProjects.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            Dim iWidth(0 To lvProjects.Columns.Count - 1) As Int16, iTotal As Int16 = 0
            For xInt As Int16 = 0 To lvProjects.Columns.Count - 1
                iWidth(xInt) = lvProjects.Columns(xInt).Width
            Next
            lvProjects.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            For xint As Int16 = 0 To lvProjects.Columns.Count - 1
                If lvProjects.Columns(xint).Width < iWidth(xint) Then lvProjects.Columns(xint).Width = iWidth(xint)
            Next
            For xInt As Int16 = 0 To lvProjects.Columns.Count - 1
                iTotal += iWidth(xInt) + 3 'lines
            Next
            iTotal += Me.Width - Me.ClientSize.Width
            Me.Width = iTotal
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
    Private Sub lvProjects_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvProjects.MouseDoubleClick
        Try
            Dim lItem As ListViewItem = lvProjects.GetItemAt(e.X, e.Y)
            Try
                If lItem.Text = "" Then Exit Sub
            Catch ex As Exception : End Try
            If Not ProjectInfo.Projects.KnownProject(lItem.Text) Then Exit Sub
            Dim eProject As clsProjectInfo.sProject.clsProject = ProjectInfo.Projects.Project(lItem.Text)
            Dim fEdit As New frmEditProject
            fEdit.Text = "Edit project - " & eProject.ProjectNumber
            fEdit.Project = eProject
            AddHandler fEdit.FormClosed, AddressOf EditClosed
            fEdit.ShowDialog(Me)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub EditClosed()
        FillView()
    End Sub
    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            If lvProjects.SelectedItems.Count = 0 Then Exit Sub
            For Each sItem As ListViewItem In lvProjects.SelectedItems
                Dim nProject As clsProjectInfo.sProject.clsProject = ProjectInfo.Projects.Project(sItem.Text)
                Dim rVal As MsgBoxResult = MsgBox("Confirm removal of project - " & nProject.ProjectNumber, MsgBoxStyle.OkCancel, "Remove")
                If Not rVal = MsgBoxResult.Cancel Then
                    If Not ProjectInfo.RemoveProject(nProject.ProjectNumber) Then
                        MsgBox("Removal of project failed for some reason")
                    End If
                End If
            Next
            FillView()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Try
            Dim fAdd As New frmAddProject
            AddHandler fAdd.FormClosed, AddressOf Padded
            fAdd.ShowDialog(Me)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Padded()
        FillView()
    End Sub
    Private Sub cmdPurge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPurge.Click
        Try
            If Not My.Computer.Network.IsAvailable Then
                MsgBox("You do not seem to have an active network connection, a purge would delete all local info without being able to update.", vbApplicationModal And vbOKOnly And vbInformation, "Purge aborted")
                Exit Sub
            End If

            Dim rVal As MsgBoxResult = MsgBox("Purge will delete all local projectinfo and download new information from the project summary, are you sure you want to purge?", MsgBoxStyle.OkCancel, "Confirm purge")
            If rVal = MsgBoxResult.Cancel Then Exit Sub
            If Not ProjectInfo.Purge Then MsgBox("Purge failed for some reason, and the backup has been restored.")
            FillView()
        Catch ex As Exception

        End Try
    End Sub
    Private SelectedProject As clsProjectInfo.sProject.clsProject
    Private Sub ViewProjectDescriptionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewProjectDescriptionToolStripMenuItem.Click
        Try
            If SelectedProject.Description <> "" Then
                Process.Start(SelectedProject.Description)
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmPBList_ViewProjectDescription", Err)
        End Try
    End Sub
    Private Sub lvProjects_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvProjects.MouseDown
        Try
            Dim lItem As ListViewItem = lvProjects.GetItemAt(e.X, e.Y)
            Try
                If lItem.Text = "" Then Exit Sub
            Catch ex As Exception : End Try
            If Not ProjectInfo.Projects.KnownProject(lItem.Text) Then Exit Sub
            SelectedProject = ProjectInfo.Projects.Project(lItem.Text)
        Catch ex As Exception
            LogWindow.WriteError("frmPBList_lvProjects_MouseDown", Err)
        End Try
    End Sub
End Class