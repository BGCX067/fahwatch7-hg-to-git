Imports System.Text
Imports System.Net.Mail

Module ApplicationEntryPoint
    Sub Main()
        Try
            'Try
            '    ReadFolder()
            '    sqdata = New Data
            '    sqdata.Init(dbFolder)
            '    sqdata.ClearExceptions()
            'Catch ex As Exception
            '
            'Finally
            '    ExitApplication()
            '    End
            'End Try

            Application.EnableVisualStyles()
            Logwindow.CreateLog()
            ReadFolder()
            WriteLog("****** " & My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString & " started")
            Dim cArgs As String = ""
            For Each argument As String In My.Application.CommandLineArgs
                cArgs &= Chr(32) & argument
            Next
            WriteLog("****** Arguments: " & cArgs)
            sqdata = New Data
            WriteLog("Initializing database access: " & sqdata.Init(dbFolder).ToString)
            WriteLog("Initializing exception handler")
            Exceptions.Init()

            Dim rVal As Boolean = False
            If Not IsNothing(Splash) Then
                WriteDebug("Splash form being created")
                Splash.Show()
                Splash.TopMost = True
            End If
            Try
                If Not ClientConfig.Configuration.SetLocations() Then
                    WriteLog("Failed to find client folders, running verbose analysis.")
                    ClientConfig.Configuration.SetLocations(True)
                    GoTo Skip
                Else
                    WriteLog("Found FAHClient folders!")
                End If
                'Fill info
                If ClientConfig.ReadFAHClientConfig Then
                    WriteLog("Client config obtained")
                Else
                    WriteLog("Could not obtain client config")
                    GoTo Skip
                End If
                Try
                    'This doesn't actually keep the fahclientinfo, it's reparsed by the logparser so it's ok
                    clsClientInfo.FAHClientInfo.Parse()
                    WriteLog("Client info obtained")
                Catch ex As Exception
                    WriteLog("Could not obtain client info")
                    Exit Try
                End Try
                modMySettings.Init()
                WriteLog("Starting hardware detection")
                Exceptions.SetExceptionIsNonFatal = True
                If Not m_hwInfo.Init(modMAIN.LumberJack, False) Then
                    If Not IsNothing(Splash) Then
                        If Not Splash.IsDisposed Then
                            Splash.Close()
                            Splash.Dispose()
                        End If
                    End If
                    'Check why?
                    WriteLog("Hardware detection failed", eSeverity.Important)
                    'GoTo Skip 'ignore failure's here
                Else
                    'Write a diagnostics to fahclient's shared data folder so other instanes have that information, include the port to use to communicate
                    Dim strLD As New StringBuilder
                    strLD.AppendLine("****** " & Application.ProductName & " " & My.Application.Info.Version.ToString & " diagnostics started ******")
                    If My.Computer.FileSystem.FileExists(ClientConfig.Configuration.DataLocation & "\Diagnostics.txt") Then My.Computer.FileSystem.DeleteFile(ClientConfig.Configuration.DataLocation & "\Diagnostics.txt")
                    If m_hwInfo.Territory.IsX64 Then
                        strLD.AppendLine("Os: " & m_hwInfo.Territory.OsName & " (x64)")
                    Else
                        strLD.AppendLine("Os: " & m_hwInfo.Territory.OsName & " (x86)")
                    End If
                    strLD.AppendLine("Com port: " & modMySettings.NetworkPort)
                    Dim lString As String = Mid(m_hwInfo.ohmInterface.OHMReport, 1, m_hwInfo.ohmInterface.OHMReport.IndexOf("Sensors"))
                    strLD.AppendLine(lString)
                    If Not IsNothing(m_hwInfo.ohmInterface.ADL) Then
                        strLD.AppendLine(m_hwInfo.ohmInterface.ADL.Report)
                    End If
                    If Not IsNothing(m_hwInfo.ohmInterface.NVAPI) Then
                        strLD.AppendLine(m_hwInfo.ohmInterface.NVAPI.Report)
                    End If
                    If Not IsNothing(m_hwInfo.gpuInf) Then
                        strLD.AppendLine(m_hwInfo.gpuInf.TextReport)
                    End If
                    WriteLog("Diagnostics being saved to " & ClientConfig.Configuration.DataLocation)
                    Try
                        My.Computer.FileSystem.WriteAllText(ClientConfig.Configuration.DataLocation & "\Diagnostics.txt", strLD.ToString, False)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                    strLD = Nothing
                End If


                'If Not m_hwInfo.Territory.IsAdmin Then
                '    WriteLog("Application was started without admin rights and will now exit")
                '    MsgBox("This application needs admin rights!", CType(vbCritical + vbOKOnly, MsgBoxStyle))
                '    ExitApplication(True)
                'End If

                'Return nonfatal to false 
                Exceptions.SetExceptionIsNonFatal = False
                Dim bWarnThem As Boolean = False
                If modMySettings.FirstRun Then
                    WriteLog("No settings stored, showing first run wizard.")
                    bWarnThem = True
                    If Not IsNothing(Splash) Then
                        If Not Splash.IsDisposed Then
                            Splash.Close()
                            Splash.Dispose()
                        End If
                    End If
                    If modMySettings.ShowOptionsForm(Nothing) = DialogResult.Cancel Then
                        WriteLog("User canceled setup, exiting")
                        Data.dbPool.close(True)
                        Application.DoEvents()
                        End
                        'mExit = True
                        rVal = False
                        GoTo Skip
                    Else
                        WriteDebug("Continuing from initial options form")
                    End If
                Else
                    WriteLog("Initializing parser, local client: " & modMySettings.LocalClientName)
                    Clients.Init(modMySettings.LocalClientName)
                    WriteLog("Checking for remote clients configuration.." & sqdata.HasRemoteClients.ToString)
                    If sqdata.HasRemoteClients Then
                        If sqdata.ReadRemoteClients Then
                            WriteLog("Remote clients initialized!")
                        Else
                            WriteLog("Could not read the clients configuration!")
                        End If
                    End If
                End If
                'Storing Diagnostics
                '... not implemented
                WriteLog("Initializing project info")
                If sqdata.HasProjectInfo Then
                    sqdata.ReadProjectInfo()
                Else
                    If Not FAHWatch7.ProjectInfo.GetProjects(modMySettings.DefaultSummary, True) Then
                        'http://fah-web.stanford.edu/psummary.html
                        'http://calxalot.net/downloads/psummary.html
                        If Not modMySettings.DefaultSummary = "http://fah-web.stanford.edu/psummary.html" Then
                            WriteDebug("-Retrying Psummary download from http://fah-web.stanford.edu/psummary.html : " & FAHWatch7.ProjectInfo.GetProjects("http://fah-web.stanford.edu/psummary.html", True).ToString)
                        End If
                    End If
                    If Not FAHWatch7.ProjectInfo.pSummaryList.Count = 0 Then
                        sqdata.Update_ProjectInfo()
                    Else
                        MsgBox("Failed to download project information, if at a later time project info is downloaded work units without server reported credit will be credited using the downloaded information", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle))
                    End If
                End If
                WriteLog("-Known projects:" & CStr(FAHWatch7.ProjectInfo.pSummaryList.Count))
                If bWarnThem Then
                    MsgBox("This is the first time you're parsing your history so all of your log files will be checked and depending on the amount of clients or log files this might take some time.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle))
                End If
                If Not IsNothing(Splash) Then
                    If Not Splash.IsDisposed Then
                        Splash.Close()
                        Splash.Dispose()
                    End If
                End If
                Dim dtStart As DateTime = DateTime.Now
                If Not EOCInfo.ReadAccounts() Then
                    WriteLog("Error trying to prepare EOC accounts!", eSeverity.Critical)
                End If
                If Clients.ParseLogs(True) Then
                    If modMySettings.FirstRun Then
                        dtStart = DateTime.Now
                        Dim bSucces As Boolean
                        For Each FClient As Client In Clients.Clients
                            bSucces = sqdata.GetFAHControlProjectDescriptions(FClient.ClientConfig.Configuration.DataLocation)
                        Next
                        WriteLog("Importing project descriptions ( status: " & bSucces.ToString & " took " & FormatTimeSpan(DateTime.Now.Subtract(dtStart), True))
                    End If
                    dtStart = Nothing
                Else
                    WriteLog("Logparser failed!", eSeverity.Critical)
                    GoTo Skip
                End If
                If sqdata.InitEOC Then
                    WriteDebug("Initialized database for ExtremeOverclocking updates")
                Else
                    WriteLog("Initializing database for ExtremeOverclocking updates failed", eSeverity.Critical)
                End If
                If EOCInfo.Prepare Then
                    WriteDebug("Initialized ExtremeOverclocking updates handler")
                Else
                    WriteLog("Initializing ExtremeOverclocking updates handler failed", eSeverity.Critical)
                End If
                'Initialize filters
                If Not sqdata.InitSQLFilters Then
                    WriteLog("Failed to get stored sql filters.", eSeverity.Important)
                Else
                    WriteLog("Initialized sql filters")
                End If
                If Not modMySettings.InitializeFilters Then
                    WriteLog("Failed to get prepare notification filters.", eSeverity.Important)
                Else
                    WriteLog("Initialized notification filters")
                End If
                If modMySettings.FirstRun Then
                    If Not IsNothing(Splash) Then
                        If Not Splash.IsDisposed Then
                            Splash.Close()
                            Splash.Dispose()
                        End If
                    End If
                    modMySettings.ShowNotifyForm()
                End If
                If Not IsNothing(Splash) Then
                    If Not Splash.IsDisposed Then
                        Splash.Close()
                        Splash.Dispose()
                    End If
                End If
                rVal = True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                If Not IsNothing(Splash) Then
                    If Not Splash.IsDisposed Then
                        Splash.Close()
                        Splash.Dispose()
                    End If
                End If
            End Try
Skip:
            'Logwindow.Form.IsInintialized = True
            If Not rVal Then
                Logwindow.bAllowClose = True
                Logwindow.ShowDebugWindow(My.Resources.iWarning, True)
                'Application.Run(Logwindow.Form)
            Else
                If modMySettings.MinimizeToTray Then modIcon.ShowIcon()
                If bTestForm Then
                    'mExit = True
                    Dim Test As New frmTest
                    Application.Run(Test)
                Else
                    If EOCInfo.Init Then
                        WriteLog("Initialized EOC xml updates")
                    Else
                        WriteLog("Failed to initialize EOC xml updates")
                    End If
                    appContext = New clsAppContext
                    Application.Run(appContext)
                End If
            End If
Skip2:
        Catch ex As Exception
            End
        End Try
    End Sub
End Module
