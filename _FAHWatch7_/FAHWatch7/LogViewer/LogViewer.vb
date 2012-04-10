Imports System.IO
Public Class LogViewer
#Region "Async files retrieval"
    Private Shared Function AsyncGetFiles(ByVal Location As String, ByRef Files As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
        Dim bRes As Boolean = True
        Try
            Files = My.Computer.FileSystem.GetFiles(Location & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
        Catch ioEx As IOException
            bRes = False
            If Err.Number = 57 Then
                WriteLog("Unable to access " & Location, eSeverity.Important)
            Else
                WriteError(ioEx.Message, Err)
            End If
        Catch ex As Exception
            bRes = False
            WriteError(ex.Message, Err)
        End Try
        Return bRes
    End Function
    Private Shared Function AsyncFindInFiles(ByVal Location As String, ByRef Files As System.Collections.ObjectModel.ReadOnlyCollection(Of String), Pattern As String) As Boolean
        Dim bRes As Boolean = True
        Try
            Files = My.Computer.FileSystem.FindInFiles(Location & "\Logs\", Pattern, True, FileIO.SearchOption.SearchTopLevelOnly)
        Catch ioEx As IOException
            bRes = False
            If Err.Number = 57 Then
                WriteLog("Unable to access " & Location, eSeverity.Important)
            Else
                WriteError(ioEx.Message, Err)
            End If
        Catch ex As Exception
            bRes = False
            WriteError(ex.Message, Err)
        End Try
        Return bRes
    End Function
    Private Delegate Function dFindInFiles(Location As String, ByRef Files As System.Collections.ObjectModel.ReadOnlyCollection(Of String), Pattern As String) As Boolean
    Private Delegate Function dAsyncGetFiles(ByVal Location As String, ByRef Files As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
#End Region
    Private Shared ClientName As String
    Private Shared WUToSearch As clsWU
    Friend Shared Function SearchWU(WU As clsWU) As Boolean
        Try
            Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing
            Dim aGetFiles As New dAsyncGetFiles(AddressOf AsyncGetFiles)
            Dim aGetFilesResult As IAsyncResult = aGetFiles.BeginInvoke(Clients.Client(WU.ClientName).ClientLocation, lFiles, Nothing, Nothing)
            While Not aGetFilesResult.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As Boolean = aGetFiles.EndInvoke(lFiles, aGetFilesResult)
            aGetFilesResult.AsyncWaitHandle.Close()
            If Not bRes Then Return False
            Dim fFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing
            Dim aFindInFiles As New dFindInFiles(AddressOf AsyncFindInFiles)
            Dim aFindInFileResult As IAsyncResult = aFindInFiles.BeginInvoke(Clients.Client(WU.ClientName).ClientLocation, fFiles, "", Nothing, Nothing)
            While Not aFindInFileResult.IsCompleted
                Application.DoEvents()
            End While
            bRes = aFindInFiles.EndInvoke(fFiles, aFindInFileResult)
            If Not bRes Then Return False



            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            GC.Collect(GC.MaxGeneration)
        End Try
    End Function
End Class
