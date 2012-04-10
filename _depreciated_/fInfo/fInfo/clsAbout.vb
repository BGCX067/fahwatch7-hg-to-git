Public Class clsAbout
    Private _form As New frmAbout
    Public Sub showForm()
        _form.ButtonDisabled = frmAbout.eDisableButtons.DisableNone
        _form.Show()
        _form.Focus()
    End Sub
    Public ReadOnly Property IsFormActive As Boolean
        Get
            Return _form.Visible
        End Get
    End Property
    Public Sub CloseForm()
        _form.CloseForm()
    End Sub
    Public Sub HideForm()
        _form.Visible = False
    End Sub
End Class
