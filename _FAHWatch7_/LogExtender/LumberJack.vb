Public Class LumberJack
    Public Event LogWritten(sender As Object, message As String)
    Public Event ErrorWritten(sender As Object, message As String, ErrObj As ErrObject)
    Public Sub WriteLog(sender As Object, Message As String)
        RaiseEvent LogWritten(sender, Message)
    End Sub
    Public Sub WriteError(sender As Object, message As String, ErrObj As ErrObject)
        RaiseEvent ErrorWritten(sender, message, ErrObj)
    End Sub
End Class