'   Add projects form
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
Public Class frmAddProject
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub
    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If txtProjectNumber.Text = "" Then
                txtProjectNumber.Focus()
                Exit Sub
            ElseIf txtCredit.Text = "" Then
                txtCredit.Focus()
                Exit Sub
            End If
            'If ProjectInfo.Projects.KnownProject(txtProjectNumber.Text) Then
            '    MsgBox("There already is a reference to that project, try editing it instead of creating a new one", MsgBoxStyle.OkOnly, "Duplicate")
            '    Exit Sub
            'Else
            '    Dim nProject As New clsProjectInfo.sProject.clsProject
            '    With nProject
            '        .ProjectNumber = txtProjectNumber.Text
            '        .Credit = txtCredit.Text
            '        .Contact = txtContact.Text
            '        .FinalDeadline = txtFinalDeadline.Text
            '        .PreferredDays = txtPrefferdDays.Text
            '        .NumberOfAtoms = txtNOatoms.Text
            '        .ServerIP = txtServerIP.Text
            '        .kFactor = txtkFactor.Text
            '    End With
            '    'If Not ProjectInfo.AddProject(nProject) Then
            '    '    MsgBox("For some reason adding the project has failed")
            '    'End If
            'End If
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmAddProject_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size
    End Sub
End Class