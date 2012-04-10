Imports System.Windows.Forms

Public Class diagRemoveUserData
    Private mDiagRes As DialogResult = Windows.Forms.DialogResult.None
    Public Overloads ReadOnly Property DialogResult As DialogResult
        Get
            Return mDiagRes
        End Get
    End Property
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        mDiagRes = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        mDiagRes = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Friend Sub SetLocation(Location As String)
        Label1.Text &= Location
    End Sub

    Private Sub diagRemoveUserData_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If mDiagRes = Windows.Forms.DialogResult.None Then mDiagRes = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub diagRemoveUserData_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Label1.Text = "If you're installing a newer version, select Cancel to keep your existing data." & Environment.NewLine & Environment.NewLine & "If after updating the application stops working, the database layout between versions has changed. You can still access your old data if you download the version compatible with it so move it to another version and download a zipped copy of the old version." & Environment.NewLine & Environment.NewLine & "Removing the incompatible database will also make sure you can run the new version." & Environment.NewLine & Environment.NewLine & "Data location: " & dbLocation
    End Sub
End Class
