Public Class frmAbout
    Private Sub frmAbout_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub frmAbout_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
    End Sub
End Class