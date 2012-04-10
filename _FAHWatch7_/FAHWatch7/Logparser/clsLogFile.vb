Imports System.Globalization

Friend Class clsLogFile
    Implements IDisposable
    Private _ClientName As String = ""
    Friend ReadOnly Property ClientName As String
        Get
            Return _ClientName
        End Get
    End Property
    Friend Sub New(ByVal ClientName As String)
        _ClientName = ClientName
    End Sub
    Private bDone As Boolean = True
    Friend Property AllDone As Boolean
        Get
            Return bDone
            'Check if all wu's have been fully parsed!
            'Dim bRet As Boolean = True
            'Try
            '    For Each wu As clsWU In dWU.Values
            '        If wu.IsActive Then
            '            bRet = False
            '            Exit For
            '        End If
            '    Next
            'Catch ex As Exception
            '    WriteError(ex.Message, Err)
            'End Try
            'Return bRet
        End Get
        Set(value As Boolean)
            bDone = value
        End Set
    End Property
    'Extending logfile structure with log information
    Friend dWU As New Dictionary(Of String, clsWU)
    Friend ReadOnly Property lstWU As List(Of clsWU)
        Get
            Return dWU.Values.ToList
        End Get
    End Property
    Friend Log As String = ""
    Friend File As String = ""
    'FileName is used for db storage!
    Friend ReadOnly Property fileName As String
        Get
            If File.Contains("/") Then
                Return (File.Substring(File.LastIndexOf("/") + 1).Replace(".txt", "").Replace("-", "_"))
            Else
                Return (File.Substring(File.LastIndexOf("\") + 1).Replace(".txt", "").Replace("-", "_"))
            End If
        End Get
    End Property
    Friend LineCount As String = ""
    Friend ReadOnly Property logDate As DateTime
        Get
            Try
                Return DateTime.Parse(Log.Replace("*", "").Replace("Log Started", "").Trim.Replace("-", " "), CultureInfo.InvariantCulture)
            Catch ex As Exception
                Return New DateTime
            End Try
        End Get
    End Property
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                ClientInfo.Dispose()
                ClientConfig.Dispose()
                dWU.Clear()
                dWU = Nothing
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Friend Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class