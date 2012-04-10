Imports System.Windows.Forms
Public Class diagClearSettings
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Select Case MsgBox("Are you sure?", CType(MsgBoxStyle.Question Or MsgBoxStyle.OkCancel, MsgBoxStyle), "Confirm clearing settings")
            Case MsgBoxResult.Ok
                If chkAffinity.Checked Then
                    sqdata.ClearAffinitySettings()
                ElseIf chkColumn.Checked Then
                    sqdata.ClearColumnSettings()
                ElseIf chkEmail.Checked Then
                    sqdata.ClearMailSettings()
                ElseIf chkExceptions.Checked Then
                    sqdata.ClearExceptions()
                ElseIf chkNonFatal.Checked Then
                    sqdata.ClearNonFatal()
                ElseIf chkRemoteClients.Checked Then
                    sqdata.ClearRemoteClients()
                ElseIf chkGraphSettings.Checked Then
                    modMySettings.clsGraphSettings.ResetDefaults()
                ElseIf chkGeneral.Checked Then
                    sqdata.ClearSettings()
                End If
                modMySettings.SaveSettings()
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub chkGeneral_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkGeneral.CheckedChanged
        If chkGeneral.Checked Then
            chkGraphSettings.Checked = True
            chkGraphSettings.Enabled = False
        Else
            chkGraphSettings.Enabled = True
        End If
    End Sub
End Class
