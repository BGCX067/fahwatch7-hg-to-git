Imports System.Globalization
Imports System.Net.NetworkInformation
Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Friend Event NetworkAvailable(sender As Object, e As EventArgs)
        Friend Event NetworkLost(sender As Object, e As EventArgs)
        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged
            Try
                If e.IsNetworkAvailable Then
                    RaiseEvent NetworkAvailable(Me, Nothing)
                Else
                    RaiseEvent NetworkLost(Me, Nothing)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Sub MyApplication_Shutdown(sender As Object, e As System.EventArgs) Handles Me.Shutdown
            Try
                'Expand to handle active warnings related to errors involving/causing possible database corruptions 
                If My.Computer.FileSystem.FileExists(Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & "_old.db") Then
                    Try
                        My.Computer.FileSystem.DeleteFile(Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & "_old.db")
                    Catch ex As Exception : End Try
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                Dim strTmp As String = modLinkDemand.EnvironmentSetting("TMP") & "\"
                Dim lFiles As List(Of String) = My.Computer.FileSystem.GetFiles(strTmp, FileIO.SearchOption.SearchTopLevelOnly, "fw7_*.*").ToList
                For Each fName In lFiles
                    Try
                        My.Computer.FileSystem.DeleteFile(fName, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                    Catch ex As Exception : End Try
                Next
                If My.Application.CommandLineArgs.Count > 0 Then
                    For Each cArg In My.Application.CommandLineArgs
                        If cArg.ToLowerInvariant.Contains("setdbfolder:") Then
                            dbFolder = (Chr(34) & cArg.ToLowerInvariant.Replace("setdbfolder:", "") & Chr(34)).Replace(Chr(34) & Chr(34), Chr(34))
                            SetFolder()
                            End
                        End If
                        If cArg.ToLowerInvariant = "cleanexceptions" Then
                            ReadFolder()
                            sqdata = New Data
                            sqdata.Init(dbFolder)
                            sqdata.ClearExceptions()
                            ExitApplication()
                            End
                        End If
                        If cArg.ToLowerInvariant = "console" Then
                            bNoConsole = False
                        End If
                        If cArg.ToLowerInvariant = "diagnostic" Or cArg.ToLowerInvariant = "diagnostics" Then
                            Diagnostics()
                            End
                        End If
                        If cArg.ToLowerInvariant = "whitelist" Then
                            WhiteList()
                            ExitApplication(True)
                        End If
                        If cArg.ToLowerInvariant = "debug" Then
                            DebugOutput = True
                        End If
                        If cArg.ToLowerInvariant = "simplebrowser" Then
                            simpleNetworkbrowser = True
                        End If
                        If cArg.ToLowerInvariant = "test" Then
                            bTestForm = True
                        End If
                    Next
                End If
                ApplicationEntryPoint.Main()
            Catch ex As Exception
                MsgBox(Err.Source & "-" & Err.Erl & ":" & Err.Description & Environment.NewLine & Err.GetException.StackTrace.ToString)
                End
            End Try
        End Sub
        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Try
                e.BringToForeground = True
                End
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Sub MyApplication_UnhandledException(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            WriteError(e.Exception.Message, Err)
        End Sub
    End Class
End Namespace

