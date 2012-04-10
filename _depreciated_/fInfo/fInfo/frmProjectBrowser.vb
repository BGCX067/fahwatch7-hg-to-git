'   cftUnity project browser
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
Public Class frmProjectBrowser
    Private cProject As clsProjectInfo.sProject.clsProject
    Private _cForm As Form
    Public Function ShowBrowser(ByVal OwnerForm As Form, Optional ByVal Pnumber As String = "0") As Boolean
        _cForm = OwnerForm
        cmbProjects.Items.Clear()
        For x = 1 To ProjectInfo.Projects.ProjectCount
            cmbProjects.Items.Add(ProjectInfo.Projects.ProjectNumber(x))
        Next
        If ProjectInfo.Projects.KnownProject(Pnumber) Then
            cmbProjects.Text = Pnumber
        Else
            cmbProjects.Text = cmbProjects.Items(0)
        End If
        RefreshBrowser()
        If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
        If Me.ShowInTaskbar = False Then Me.ShowInTaskbar = True
        If Not Me.Visible Then Me.ShowDialog(OwnerForm)
    End Function
    Public Function RefreshBrowser()
        Try
            Dim pnumber As String = cmbProjects.Text
            cProject = ProjectInfo.Project(pnumber)
            lblAtoms.Text = cProject.NumberOfAtoms
            lblDeadline.Text = cProject.FinalDeadline
            lblPreffered.Text = cProject.PreferredDays
            lnkCode.Text = cProject.Code
            lblContact.Text = cProject.Contact
            lnkDescription.Text = cProject.Description
            lblServerIP.Text = cProject.ServerIP
            lblCredit.Text = cProject.Credit
            Me.Text = "Project browser - known projects: " & ProjectInfo.Projects.ProjectCount
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub cmbProjects_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbProjects.SelectedIndexChanged
        RefreshBrowser()
    End Sub
    Private Sub lnkDescription_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkDescription.LinkClicked
        Process.Start(cProject.Description)
    End Sub
    Private Sub lnkContact_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Process.Start(cProject.Contact)
    End Sub
    Private Sub lnkServer_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Process.Start("http://" & cProject.ServerIP)
    End Sub
    Private Sub lnkCode_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkCode.LinkClicked
        Process.Start("http://fahwiki.net/index.php/Cores")
    End Sub
    Private Sub GroupBox1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GroupBox1.MouseClick
        If MouseButtons = Windows.Forms.MouseButtons.Right Then cMenu.Show()
    End Sub
    Private Sub FetchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FetchToolStripMenuItem.Click
        ProjectInfo.GetProjects()
        ShowBrowser(_cForm, cmbProjects.Text)
    End Sub
    Private Sub ImportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportToolStripMenuItem.Click
        Dim iFile As String = vbNullString
        Dim fBrowser As New OpenFileDialog
        With fBrowser
            .RestoreDirectory = True
            .ShowHelp = False
            .Title = "Select file to import"
            If .ShowDialog(Me) And .FileName <> "" Then
                Dim AllText As String = My.Computer.FileSystem.ReadAllText(.FileName)
                If Not ProjectInfo.ParseProjects(.FileName) Then MsgBox("Could not parse selected file.")
            End If
        End With
    End Sub

    Private Sub frmProjectBrowser_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If ProjectInfo.Projects.IsEmpty Then ProjectInfo.GetProjects()
        RefreshBrowser()
    End Sub

    Private Sub EditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem.Click
        Try
            Dim fEdit As New frmEditProject
            With fEdit
                .Text &= " - " & cmbProjects.Text
                .txtCredit.Text = lblCredit.Text
                .txtFinalDeadline.Text = lblDeadline.Text
                .txtPrefferdDays.Text = lblPreffered.Text
                .txtNOatoms.Text = lblAtoms.Text
                .txtServerIP.Text = lblServerIP.Text
            End With
            fEdit.Project = ProjectInfo.Project(cmbProjects.Text)
            fEdit.ShowDialog(Me)
        Catch ex As Exception

        End Try
    End Sub
End Class

