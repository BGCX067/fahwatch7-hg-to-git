Public Class frmTray
    Private _lHandle As IntPtr = 0
    Private Sub frmTray_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            My.Settings.Save()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmTray_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False
        Startup()
    End Sub
    Private Sub Startup()
        Try
            'check if client is running ->
            
            nIcon.Text = "fTray"



        Catch ex As Exception

        End Try

    End Sub







    Private Sub tIcon_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tIcon.Tick
        Try
            tIcon.Enabled = False
            UpdateIcon()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub nIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try

        Catch ex As Exception

        End Try
    End Sub
End Class