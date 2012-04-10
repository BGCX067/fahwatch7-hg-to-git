Friend Class LogStartObject
    Implements IDisposable
    Friend Property Failed As Boolean = False
    Friend Property Exception As Exception = Nothing
    Friend Property ClientName As String = String.Empty
    Friend Property Files As New List(Of String)
    Friend Property ShowUI As Boolean = False
    Friend Property ParentForm As Form = Nothing
    Friend Property ClientInfo As New clsClientInfo
    Friend Property ClientConfig As New clsClientConfig
    Friend Property dLogFiles As New Dictionary(Of String, clsLogFile)
    Friend Property dClientInfo As New Dictionary(Of DateTime, clsClientInfo)
    Friend Property dClientConfig As New Dictionary(Of String, clsClientConfig)
    'completed work units
    Friend Property lWU As New List(Of clsWU)
    'active work units 
    Friend Property lActiveWU As New List(Of clsWU)
    'queued work units
    Friend Property lQueued As New List(Of clsWU)
    Friend Property LastLineIndex As Int32 = 0
    Friend Property LastLine As String = ""
    Friend Property LastLineDT As DateTime = #1/1/2000#
    Friend ReadOnly Property lLogFiles As List(Of clsLogFile)
        Get
            Return dLogFiles.Values.ToList
        End Get
    End Property
    Friend ReadOnly Property IsLogActive(Log As String) As Boolean
        Get
            If Not dLogFiles.ContainsKey(Log) Then
                Return False
            Else
                Return dLogFiles(Log).File.Contains("log.txt")
            End If
        End Get
    End Property
    Friend Sub clear()
        Files.Clear()
        ClientInfo = New clsClientInfo
        ClientConfig = New clsClientConfig
        ShowUI = False
        ClientName = String.Empty
        dLogFiles.Clear()
        dClientConfig.Clear()
        dClientInfo.Clear()
        lWU.Clear()
        lActiveWU.Clear()
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        Try
            If Not Me.disposedValue Then
                If disposing Then
                    Files.Clear()
                    ClientInfo.Dispose()
                    ClientConfig.Dispose()
                    ShowUI = Nothing
                    ClientName = Nothing
                    dLogFiles.Clear()
                    dClientConfig.Clear()
                    dClientInfo.Clear()
                    lWU.Clear()
                    lActiveWU.Clear()
                End If
                GC.Collect()
                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
