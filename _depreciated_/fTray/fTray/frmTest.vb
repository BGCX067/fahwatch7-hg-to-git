Public Class frmTest
    Private WithEvents hwI As New clsHWInfo

    Delegate Sub UpdateLogOut(ByVal Message As String)
    Public Sub InvokeLog(ByVal Message As String)
        rt.AppendText(Message)
        rt.SelectionStart = rt.TextLength
        rt.ScrollToCaret()
    End Sub
    Public Sub DoInvoke(ByVal Message As String)
        Dim nInvoke As New UpdateLogOut(AddressOf InvokeLog)
        rt.Invoke(nInvoke, New Object() {[Message]})
    End Sub
    Private Sub hwI_Outputrecieved(ByVal Message As String) Handles hwI.Outputrecieved
        DoInvoke(Message)
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If hwI.InitOHM() Then
            Button1.Enabled = False
            Button2.Enabled = True
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        hwI.CloseOHM()
        Button1.Enabled = True
    End Sub
End Class