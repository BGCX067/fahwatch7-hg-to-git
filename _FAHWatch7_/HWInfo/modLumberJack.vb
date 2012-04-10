Module modLumberJack
    Friend LumberJack As LumberJack.LumberJack
    Friend Sub WriteError(message As String, ErrObj As ErrObject, Optional sender As Object = Nothing)
        LumberJack.WriteError(sender, message, ErrObj)
    End Sub
    Friend Sub WriteLog(message As String, Optional sender As Object = Nothing)
        LumberJack.WriteLog(sender, message)
    End Sub
End Module
