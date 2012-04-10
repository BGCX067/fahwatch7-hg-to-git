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
            If FAHWatch7.ProjectInfo.KnownProject(txtProjectNumber.Text) Then
                MsgBox("There already is a reference to that project, try editing it instead of creating a new one", MsgBoxStyle.OkOnly, "Duplicate")
                Exit Sub
            Else
                Dim nProject As New pSummary
                With nProject
                    .ProjectNumber = txtProjectNumber.Text
                    .Credit = txtCredit.Text
                    .Contact = txtContact.Text
                    .FinalDeadline = txtFinalDeadline.Text
                    .PreferredDays = txtPrefferdDays.Text
                    .NumberOfAtoms = txtNOatoms.Text
                    .ServerIP = txtServerIP.Text
                    .kFactor = txtkFactor.Text
                End With
                If Not FAHWatch7.ProjectInfo.AddProject(nProject) Then MsgBox("For some reason adding the project has failed")
            End If
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub frmAddProject_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub frmAddProject_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
        AddHandler cmdUpdate.Click, AddressOf cmdUpdate_Click
        AddHandler cmdCancel.Click, AddressOf cmdCancel_Click
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size
    End Sub
End Class