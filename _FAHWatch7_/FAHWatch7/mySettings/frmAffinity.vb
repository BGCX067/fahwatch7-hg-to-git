Public Class frmAffinity
    Private mDiaglogResult As DialogResult = Windows.Forms.DialogResult.None
    Public Overloads ReadOnly Property DialogResult As DialogResult
        Get
            Return mDiaglogResult
        End Get
    End Property

    Private Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click
        mDiaglogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(sender As Object, e As System.EventArgs) Handles cmdOk.Click
        mDiaglogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmAffinity_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 500, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub

    Private Sub frmAffinity_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 500, NativeMethods.AnimateWindowFlags.AW_CENTER)
    End Sub
End Class