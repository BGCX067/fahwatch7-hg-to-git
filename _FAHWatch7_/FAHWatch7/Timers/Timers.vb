Imports System.Threading
Public Class Timers
    Private Shared eocCallBack As New TimerCallback(AddressOf HandleEOC)
    Private Shared parseCallBack As New TimerCallback(AddressOf HandleParse)
    Private Shared tEOC As Threading.Timer : Private Shared tParse As Threading.Timer

    Sub New()
        AddHandler delegateFactory.ParseOnIntervalChanged, AddressOf HandleParseOnIntervalChanged
        AddHandler delegateFactory.EOC_EnabledChanged, AddressOf HandleEOCEnabledChange
        AddHandler delegateFactory.LiveMinimized, AddressOf LiveMinimized
        AddHandler delegateFactory.LiveRestored, AddressOf LiveRestored
    End Sub

    Private Sub LiveMinimized(sender As Object, e As EventArgs)
        If Not modMySettings.AlwaysTrack Then
            If modMySettings.ParseLogsOnInterval Then
                IntervalBasedParserEnabled = True
            Else
                IntervalBasedParserEnabled = False
            End If
        Else
            If modMySettings.ParseLogsOnInterval Then
                IntervalBasedParserEnabled = True
            End If
        End If
    End Sub
    Private Sub LiveRestored(sender As Object, e As EventArgs)
        If Not modMySettings.AlwaysTrack Then
            'Start update timer
            IntervalBasedParserEnabled = True
        End If
    End Sub

#Region "Interval based timer"
    Private Shared dtLastParse As DateTime = #1/1/2000#
    Friend Shared Sub setLastParse(p1 As Date)
        dtLastParse = p1
    End Sub
    Friend Shared ReadOnly Property lastParse As DateTime
        Get
            Return dtLastParse
        End Get
    End Property
    Friend Shared ReadOnly Property nextParse As DateTime
        Get
            Return dtLastParse.Add(modMySettings.ParserInterval)
        End Get
    End Property
    Friend Overloads Shared Sub StartParseTimer(Interval As Double)
        StartParseTimer(CInt(Interval))
    End Sub
    Friend Overloads Shared Sub StartParseTimer(Interval As Integer)
        tParse = New Threading.Timer(parseCallBack, Nothing, 0, Interval)
        WriteLog("ParseInterval set to " & FormatTimeSpan(TimeSpan.FromMilliseconds(Interval)))
    End Sub
    Friend Shared Property IntervalBasedParserEnabled As Boolean
        Get
            Return Not IsNothing(tParse)
        End Get
        Set(value As Boolean)
            WriteLog("ParseOnInterval changed to " & value)
            If value Then
                If delegateFactory.IsFormVisible(Live) AndAlso delegateFactory.GetFormWindowState(Live) <> FormWindowState.Minimized Then
                    StartParseTimer(modMySettings.LiveParserInterval)
                Else
                    StartParseTimer(modMySettings.ParserInterval.TotalMilliseconds)
                End If
            Else
                If Not IsNothing(tParse) Then
                    tParse.Change(-1, Timeout.Infinite)
                    tParse.Dispose()
                End If
            End If
        End Set
    End Property
    Private Shared Sub HandleParseOnIntervalChanged(sender As Object, e As MyEventArgs.ParseOnIntervalArgs)
        Try
            WriteLog("ParseOnInterval changed to " & e.Enabled)
            IntervalBasedParserEnabled = e.Enabled
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub HandleParse(state As Object)
        Try
            If modMySettings.AlwaysTrack Then
                If DateTime.Now.Subtract(lastParse).TotalMinutes < 1 Then
                    WriteLog("Live monitoring aborts update due to last log parse being less then one minute ago")
                    Exit Try
                Else
                    WriteLog("Live monitoring timer fired, running inefficient update...")
                    Clients.ParseLogs(False, Live, False)
                End If
            Else
                If delegateFactory.IsFormVisible(Live) AndAlso Not delegateFactory.GetFormWindowState(Live) = FormWindowState.Minimized Then
                    If DateTime.Now.Subtract(lastParse).TotalMinutes < 1 Then
                        WriteLog("Live monitoring aborts update due to last log parse being less then one minute ago")
                        Exit Try
                    Else
                        WriteLog("Live monitoring timer fired, running inefficient update...")
                        Clients.ParseLogs(False, Live, False)
                    End If
                ElseIf modMySettings.ParseLogsOnInterval Then
                    If Not modMySettings.ParseOnEOCUpdate Then
                        If DateTime.Now > nextParse Then
                            WriteLog("Parsing logs files due to interval reached")
                            Clients.ParseLogs(False, Nothing, False)
                        Else
                            WriteLog("Aborting timer based interval because last parse was to recent")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "EOC Update timer"
    Friend Overloads Shared Sub StartEOCTimer(Interval As Double)
        StartEOCTimer(CInt(Interval))
    End Sub
    Friend Overloads Shared Sub StartEOCTimer(Interval As Integer)
        tEOC = New Threading.Timer(eocCallBack, Nothing, 0, Interval)
    End Sub
    Private Shared Sub HandleEOC(state As Object)
        Try
            Dim bParse As Boolean = False
            For Each EOCAccount As EOCInfo.sEOCAccount In EOCInfo.eocAccounts
                Dim EOCStat As clsEOC = EOCInfo.EOCStats(EOCAccount.Username, EOCAccount.Teamnumber)
                With EOCStat
                    If .ShouldRefresh Then
                        WriteLog("Attempting to refresh eoc for " & EOCStat.UserName & "(" & EOCStat.TeamNumber & ")")
                        'ReadWebXml should handle all status changes..
                        If .ReadWebXml Then
                            EOCInfo.Status = "Current"
                            bParse = True
                            delegateFactory.RaiseEOCUpdateRecieved(New MyEventArgs.EocUpdateArgs(EOCStat))
                        End If
                    Else
                        EOCInfo.Status = "Current"
                    End If
                End With
                If bParse And modMySettings.EOCNotify Then
                    WriteLog("Settings indicate showing the signature images after a new update, showing sigs")
                    EOCInfo.showSigs()
                End If
                If modMySettings.ParseOnEOCUpdate And bParse Then
                    If Clients.IsParsing Then
                        WriteLog("Eoc update recieved but parser is running, aborting parse")
                    ElseIf DateTime.Now.Subtract(lastParse).TotalMinutes < 10 Then
                        WriteLog("Eoc update recieved but parser ran less then 10 minutes ago, aborting parse")
                    Else
                        WriteLog("Settings indicate to parse the logs after recieving an eoc update, starting parser")
                        Clients.ParseLogs(False, Nothing, False)
                    End If
                End If
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub HandleEOCEnabledChange(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
        Try
            WriteLog("EocEnabled changed, timer active: " & e.Enabled)
            If e.Enabled Then
                StartEOCTimer(TimeSpan.FromMinutes(5).TotalMilliseconds)
            Else
                If Not IsNothing(tEOC) Then
                    tEOC.Change(-1, Timeout.Infinite)
                    tEOC.Dispose()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

    Friend Shared Sub Dispose()
        If Not IsNothing(tEOC) Then
            tEOC.Change(-1, Timeout.Infinite)
            tEOC.Dispose()
        End If
        If Not IsNothing(tParse) Then
            tParse.Change(-1, Timeout.Infinite)
            tParse.Dispose()
        End If
    End Sub
End Class
