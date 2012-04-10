Public Class frmAddNF
    Public Event NonFatal(ByVal Value As String)
    Public Enum eNFmode
        AddNew
        DoEdit
    End Enum
    Public NFmode As eNFmode
    Public Index As Int16
    Private Sub txtHex_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHex.KeyPress
        Try
            If e.KeyChar >= "a" And e.KeyChar <= "f" Then
                e.KeyChar = e.KeyChar.ToString.ToUpper
            End If
            If (Not (e.KeyChar >= "A" And e.KeyChar <= "F") And (Not (e.KeyChar >= "0" And e.KeyChar <= "9"))) And Not Char.IsControl(e.KeyChar) And Not e.KeyChar = ("-") Then
                e.Handled = True
            ElseIf e.KeyChar = "-" And txtHex.TextLength > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub txtHex_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHex.TextChanged
        Try
            Dim dValue As Int64 = Convert.ToInt64(txtHex.Text, 16)
            lblDecimal.Text = "(" & dValue & ")"
            cmdAccept.Enabled = True
        Catch ex As Exception
            cmdAccept.Enabled = False
        End Try
    End Sub

    Private Sub cmdAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccept.Click
        Try
            RaiseEvent NonFatal(txtHex.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class