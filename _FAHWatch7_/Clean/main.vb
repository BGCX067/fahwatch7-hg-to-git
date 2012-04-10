Imports System.Text
Imports System.IO

Module main
    Friend dbLocation As String = ""
    Sub Main()
        For Each cArg As String In My.Application.CommandLineArgs
            If cArg.ToLowerInvariant.Contains("installdir:") Then
                Try
                    Dim folder As String = cArg.ToLowerInvariant.Replace("installdir:", "").Trim(Chr(34))
                    'My.Computer.FileSystem.WriteAllText("c:\installdir.txt", folder, False)
                    If My.Computer.FileSystem.DirectoryExists(folder) Then
                        My.Computer.FileSystem.DeleteDirectory(folder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                    MsgBox("Could not remove the application installation folder" & Environment.NewLine & Environment.NewLine & "Exception: " & ex.Message, CType(MsgBoxStyle.Critical Or MsgBoxStyle.OkCancel, MsgBoxStyle), "Error")
                End Try
            ElseIf cArg.ToLowerInvariant.Contains("cleanfolder:") Then
                Try
                    dbLocation = cArg.ToLowerInvariant.Replace("cleanfolder:", "").Trim(Chr(34))
                    'My.Computer.FileSystem.WriteAllText("c:\databasedir.txt", dbLocation, False)
                    If Not My.Computer.FileSystem.DirectoryExists(dbLocation) Then Exit Sub
                    Dim diag As New diagRemoveUserData
                    If diag.ShowDialog = DialogResult.No Then
                        GoTo NextArg
                    Else
                        My.Computer.FileSystem.DeleteDirectory(dbLocation, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                    MsgBox("Could not remove the application data folder" & Environment.NewLine & Environment.NewLine & "Exception: " & ex.Message, CType(MsgBoxStyle.Critical Or MsgBoxStyle.OkCancel, MsgBoxStyle), "Error")
                End Try
            ElseIf cArg.ToLowerInvariant.Contains("cleansilent") Then
                Try
                    Dim folder As String = cArg.ToLowerInvariant.Replace("cleansilent:", "").Trim(Chr(34))
                    My.Computer.FileSystem.DeleteDirectory(folder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                Catch ex As Exception
                    'MsgBox("Could not remove the application data folder", CType(MsgBoxStyle.Critical Or MsgBoxStyle.OkCancel, MsgBoxStyle), "Error")
                End Try
            End If
NextArg:
        Next
    End Sub
End Module
