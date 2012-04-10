Public Class frmAddProject
    

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        Try
            If txtProjectNumber.Text = "" Then
                txtProjectNumber.Focus()
                Exit Sub
            ElseIf txtCredit.Text = "" Then
                txtCredit.Focus()
                Exit Sub
            End If
            If ProjectInfo.Projects.KnownProject(txtProjectNumber.Text) Then
                MsgBox("There already is a reference to that project, try editing it instead of creating a new one", MsgBoxStyle.OkOnly, "Duplicate")
                Exit Sub
            Else
                Dim nProject As New clsProjectInfo.sProject.clsProject
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
                If Not ProjectInfo.AddProject(nProject) Then
                    MsgBox("For some reason adding the project has failed")
                End If
            End If
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class