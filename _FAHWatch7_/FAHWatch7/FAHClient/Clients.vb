Imports System.Globalization
Imports System.Threading

Friend Class Clients
    Inherits Dictionary(Of String, Client)
    Private Shared m As Clients
    Sub New()
        Try
            m = Me
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#Region "Client collection"
    Friend Shared ReadOnly Property AllRunning As Boolean
        Get
            Try
                Dim rVal As Boolean = True
                For Each Client As Client In Clients
                    If Not Client.AllRunning Then
                        rVal = False
                        Exit For
                    End If
                Next
                Return rVal
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Shared ReadOnly Property AllStopped As Boolean
        Get
            Try
                Dim rVal As Boolean = True
                For Each Client As Client In Clients
                    If Not Client.AllStopped Then
                        rVal = False
                        Exit For
                    End If
                Next
                Return rVal
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Shared ReadOnly Property QueuedWorkUnits As List(Of clsWU)
        Get
            Dim rVal As New List(Of clsWU)
            For Each Client As Client In Clients
                rVal.AddRange(Client.Queue)
            Next
            Return rVal
        End Get
    End Property
    Friend Shared ReadOnly Property Clients As List(Of Client)
        Get
            Return m.Values.ToList
        End Get
    End Property
    Friend Shared Property Client(Name As String) As Client
        Get
            Try
                Return m(Name)
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(value As Client)
            m(Name) = value
        End Set
    End Property
    Friend Shared ReadOnly Property LocalClient As Client
        Get
            Return Clients(0)
        End Get
    End Property
    Friend Shared ReadOnly Property initialized As Boolean
        Get
            Return Clients.Count > 0
        End Get
    End Property
    Friend Shared Sub Init(Optional ByVal Name As String = "local")
        Try
            'Add local name support
            If m.ContainsKey(Name) Then
                WriteLog("Attempt at creating a second client with the same name", eSeverity.Important)
            Else
                m.Add(Name, New Client(Name, ClientConfig.Configuration.DataLocation))
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub AddClient(ByVal Name As String, ByVal Location As String, FCPort As String, PWD As String, FWPort As String, Optional ByVal Enabled As Boolean = True)
        Try
            If Not m.ContainsKey(Name) Then
                m.Add(Name, New Client(Name, Location))
                m(Name).Enabled = Enabled
                m(Name).PWD = PWD
                m(Name).FWPort = FWPort
                m(Name).FCPort = FCPort
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub SetLocation(ClientName As String, Location As String)
        'Might use this when 'migrating' clients
        m(ClientName).ClientLocation = Location
        'Throw New NotImplementedException
    End Sub
    Friend Shared Sub setClientFCPort(Name As String, FCPort As String)
        m(Name).FCPort = FCPort
    End Sub
    Friend Shared Sub setClientFWPort(Name As String, FWPort As String)
        m(Name).FWPort = FWPort
    End Sub
    Friend Shared Sub setClientPWD(Name As String, PWD As String)
        m(Name).PWD = PWD
    End Sub
    Friend Shared Sub SetClientState(ByVal Name As String, ByVal State As Boolean)
        Try
            If m.ContainsKey(Name) Then
                m(Name).Enabled = State
                WriteLog("Changed client '" & Name & "' enabled state to: " & State.ToString)
            Else
                WriteLog("Attempt to change client '" & Name & "' state but client is not in collection", eSeverity.Critical)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub RemoveClient(ByVal Name As String)
        Try
            If m.ContainsKey(Name) Then
                m.Remove(Name)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Functions"
    Private Shared bRunning As Boolean = False
    Public Shared ReadOnly Property IsParsing As Boolean
        Get
            Return bRunning
        End Get
    End Property
    Friend Shared startLog As New LogStartObject
    'Friend Shared Sub ParseWithCallback()
    '    clsParser.AsyncMain.ParseCallback(False)
    'End Sub
    'Friend Shared Function UpdateLogs(Optional sBar As StatusBar = Nothing, Optional AsThreads As Boolean = True) As Boolean
    '    Try
    '        clsParser.AsyncMain.ParseCallback(False)


    '        For Each Client As client In Clients
    '            startLog.clear()
    '            'Check if 
    '            Dim nFiles As New List(Of String)
    '            Try
    '                If Not Client.ClientLocation = LocalClient.ClientLocation Then
    '                    If Not My.Computer.Network.IsAvailable Then
    '                        WriteLog("Network not available, skipping parse of " & Client.ClientName, eSeverity.Important)
    '                    Else
    '                        Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Client.ClientLocation & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
    '                        For Each File As String In lFiles
    '                            If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
    '                                nFiles.Add(File)
    '                            End If
    '                        Next
    '                    End If
    '                Else
    '                    Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Client.ClientLocation & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
    '                    For Each File As String In lFiles
    '                        If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
    '                            nFiles.Add(File)
    '                        End If
    '                    Next
    '                End If
    '            Catch ex As Exception
    '                WriteError(ex.Message, Err)
    '                Return False
    '            End Try
    '            nFiles.Add(Client.ClientLocation & "\log.txt")
    '            With startLog
    '                .ClientName = Client.ClientName

    '                .Files = nFiles

    '            End With
    '            If AsThreads Then
    '                'Dim pThread As New Threading.Thread(AddressOf ParseLog)
    '                'pThread.Start({startLog})
    '                'While pThread.IsAlive
    '                '    Application.DoEvents()
    '                '    'Threading.Thread.Yield()
    '                'End While
    '                ''Store/Update/Remove
    '                'WriteDebug("Join finished")
    '            Else
    '                'ParseLog(startLog)
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        WriteError(ex.Message, Err)
    '        Return False
    '    Finally
    '        GC.Collect()
    '    End Try
    'End Function
    Private Shared pContext As parseContext
    Private Class parseContext
        Inherits ApplicationContext
        Private Structure parseStartOptions
            Property ShowBussy As Boolean
            Property ParentForm As Form
            Property DisableForms As Boolean
        End Structure
        Private tExit As Threading.Timer
        Private mInterlock As ManualResetEvent
        Sub New(ShowBussy As Boolean, Optional parentForm As Form = Nothing, Optional DisableForms As Boolean = False)
            mInterlock = New ManualResetEvent(False)
            Dim t As Thread = New Thread(AddressOf parseStart)
            t.SetApartmentState(ApartmentState.STA)
            Dim pStart As New parseStartOptions
            pStart.ShowBussy = ShowBussy
            pStart.ParentForm = parentForm
            pStart.DisableForms = DisableForms
            t.Start(pStart)
            mInterlock.WaitOne()
            tExit = New Threading.Timer(New Threading.TimerCallback(AddressOf tExitThread), Nothing, 0, 500)
        End Sub
        Private Sub parseStart(pStartObj As Object)
            mInterlock.Set()
            Dim pStart As parseStartOptions = DirectCast(pStartObj, parseStartOptions)
            ParseLogs(pStart.ShowBussy, pStart.parentForm, pStart.DisableForms)
        End Sub
        Private Sub tExitThread(state As Object)
            tExit.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
            tExit.Dispose()
            pContext.ExitThreadCore()
        End Sub
        Private Sub ParseLogs(ShowBussy As Boolean, Optional parentForm As Form = Nothing, Optional DisableForms As Boolean = False)
            If clsParser.AsyncMain.ParseWait(ShowBussy, parentForm, DisableForms) Then
                Timers.setLastParse(DateTime.Now)
                Dim lUP As List(Of String) = sqdata.UnknownProjects
                If lUP.Count > 0 Then
                    If DateTime.Now.Subtract(ProjectInfo.LastAttempt).TotalMinutes < 15 Then
                        WriteLog("Unknown projects " & lUP.Count.ToString & ", but last attempt less then 15m ago. Aborting psummary parse", eSeverity.Important)
                    Else
                        WriteLog("Unknown projects " & lUP.Count.ToString & ", updating psummary information from " & modMySettings.DefaultSummary & ": " & ProjectInfo.GetProjects(Nothing, False).ToString, eSeverity.Important)
                        WriteLog("Unknown projects remaining: " & sqdata.UnknownProjects.Count, eSeverity.Important)
                    End If
                End If
                delegateFactory.RaiseParserCompleted(Me, New MyEventArgs.ParserCompletedEventArgs)
            Else
                WriteLog("The logparser indicated a failure, there should be more imformation in the log", eSeverity.Important)
                delegateFactory.RaiseParserFailed(Me, New MyEventArgs.ParserFailedEventArgs)
            End If
        End Sub
    End Class
    Friend Shared Function ParseLogs(Optional ShowBussy As Boolean = False, Optional ParentForm As Form = Nothing, Optional DisableForms As Boolean = True) As Boolean
        Try
            If bRunning Then
                WriteLog("Clients.ParseLogs called while parser is running, cancelled")
                Return True
            Else
                bRunning = True
                If clsParser.AsyncMain.ParseWait(ShowBussy, ParentForm, DisableForms) Then
                    Timers.setLastParse(DateTime.Now)
                    Dim lUP As List(Of String) = sqdata.UnknownProjects
                    If lUP.Count > 0 Then
                        If DateTime.Now.Subtract(ProjectInfo.LastAttempt).TotalMinutes < 15 Then
                            WriteLog("Unknown projects " & lUP.Count.ToString & ", but last attempt less then 15m ago. Aborting psummary parse", eSeverity.Important)
                        Else
                            WriteLog("Unknown projects " & lUP.Count.ToString & ", updating psummary information from " & modMySettings.DefaultSummary & ": " & ProjectInfo.GetProjects(Nothing, False).ToString, eSeverity.Important)
                            WriteLog("Unknown projects remaining: " & sqdata.UnknownProjects.Count, eSeverity.Important)
                        End If
                    End If
                    delegateFactory.RaiseParserCompleted(Nothing, New MyEventArgs.ParserCompletedEventArgs)
                    Return True
                    'RaiseEvent Completed(Me, New MyEventArgs.BooleanArgs(True))
                    'mResult = True
                Else
                    WriteLog("The logparser indicated a failure, there should be more imformation in the log", eSeverity.Important)
                    Return False
                    'RaiseEvent Completed(Me, New MyEventArgs.BooleanArgs(False))
                    'mResult = False
                End If
                'pContext = New parseContext(ShowBussy, ParentForm, DisableForms)
                'Application.Run(pContext)
                'Return pContext.result
                'Return clsParser.AsyncMain.ParseWait(ShowBussy, ParentForm, DisableForms)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            bRunning = False
        End Try
    End Function
    Friend Shared Sub UpdateLogs(ParentForm As Form)
        pContext = New parseContext(False, ParentForm, False)
        'Application.Run(pContext)
    End Sub
#End Region
End Class