Imports System.Configuration
Public Class ConnectionStrings
    Public Shared ReadOnly Property GetConnectionString As String
        Get
            Return ConfigurationManager.AppSettings("MySqlConnectionString")
        End Get
    End Property
End Class

