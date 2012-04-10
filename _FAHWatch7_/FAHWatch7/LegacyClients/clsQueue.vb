'   FAHWatch7 LegacyClients
'
'   Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
'   The Queue class is based on documentation on the queue.dat structure as described 
'   on http://fahwiki.net/index.php/Queue.dat 
'
'   This documentation is based on Dick Howels qd ( Queue Dump ) 
'   http://linuxminded.xs4all.nl/?target=software-qd-tools.plc mirror maintained by 
'   Bas Couwenberg aka Smoking2000 http://linuxminded.xs4all.nl/?target=about.plc
'
'   A description of the hexadecimal values in queue.dat is located here
'   http://linuxminded.xs4all.nl/software/qd-tools/documentation/queue.dat-layout/hexdump.html

Imports System.IO
Imports System
Namespace LegacyClients
    Public Class clsQueue
#Region "Watchers"
#Region "Declares"
        'Private declares
        Private _ClientVersion As String = ""
        Private _Arguments As String = ""
        Private _Location As String = ""
        Private _CoreVersion As String = ""
        Private _FahLog As String = ""
        Private _logfile_ As String = ""
        Private _Progress As Int32 = 0
        Private _tsFrame As TimeSpan = TimeSpan.FromSeconds(0)
        Private _tsEffective As TimeSpan = TimeSpan.FromSeconds(0)
        Private _LastFrameCompletionTime As DateTime = #1/1/2000#
        Private _EtaDT As DateTime = DateTime.MinValue
        Private _ppdLastFrame As Double = 0
        Private _ppdEffective As Double = 0
        Private _PointsInUpdate As Double = 0
        Private _EUE As Int32 = 0
        Private _lngLogFile_Size As Long = 0
        Private _b24h_Pauze As Boolean = False
        Private _iFailedDownloads As Int32 = 0
        'FW watchers declaration
        Private WithEvents fwQueue As New FileSystemWatcher
        Private WithEvents fwLog As New System.IO.FileSystemWatcher
        Private WithEvents fWUnitInfo As New System.IO.FileSystemWatcher
        Private bReStart As Boolean = True
        'FahCore process
        Private WithEvents pFahCore As Process
        'Public events declaration
        Public Event NewFrame(ByVal CurrentProgress As Int32, ByVal FrameTime As TimeSpan, ByVal dtFrameEnd As DateTime)
        Public Event ProjectEnd(ByVal Slot As Entry, ByVal FrameTime As TimeSpan, ByVal dtFrameBegin As DateTime, ByVal Percentage As Integer)
        Public Event ProjectStart(ByVal Slot As Entry)
        Public Event FailedUpload(ByVal Slot As Entry)
        Public Event SuccesfullUpload(ByVal Slot As Entry)
        Public Event EUE(ByVal CoreStatus As clsCoreStatus)
#End Region
#Region "Properties"
        'Public readable properties
        Public ReadOnly Property Arguments As String
            Get
                Return _Arguments
            End Get
        End Property
        Public ReadOnly Property PointsInUpdate() As Double
            Get
                Return _PointsInUpdate
            End Get
        End Property
        Public ReadOnly Property EUE_Count() As Int32
            Get
                Return _EUE
            End Get
        End Property
        Public ReadOnly Property Progress() As Int32
            Get
                Return _Progress
            End Get
        End Property
        Public ReadOnly Property dtLastFrame() As DateTime
            Get
                Return _LastFrameCompletionTime
            End Get
        End Property
        Public ReadOnly Property PPD_LastFrame() As Double
            Get
                Return _ppdLastFrame
            End Get
        End Property
        Public ReadOnly Property PPD_Effective() As Double
            Get
                Return _ppdEffective
            End Get
        End Property
        Public ReadOnly Property Eta() As DateTime
            Get
                Return _EtaDT
            End Get
        End Property
        Public ReadOnly Property Eta2 As String
            Get
                Try
                    'String value easier to implement
                    Dim rString As String = ""
                    Dim tsRemain As TimeSpan = _EtaDT.Subtract(DateTime.Now)
                    If tsRemain.Ticks < 0 Then
                        Return "0.00:00:00"
                    End If
                    If tsRemain.Days > 0 Then rString &= tsRemain.Days.ToString & "."
                    If tsRemain.Hours = 0 Then
                        rString &= "00:"
                    ElseIf tsRemain.Hours > 0 And tsRemain.Hours < 10 Then
                        rString &= "0" & tsRemain.Hours.ToString & ":"
                    ElseIf tsRemain.Hours >= 10 Then
                        rString &= tsRemain.Hours.ToString & ":"
                    End If
                    If tsRemain.Minutes = 0 Then
                        rString &= "00:"
                    ElseIf tsRemain.Minutes > 0 And tsRemain.Minutes < 10 Then
                        rString &= "0" & tsRemain.Minutes.ToString & ":"
                    ElseIf tsRemain.Minutes >= 10 Then
                        rString &= tsRemain.Minutes.ToString & ":"
                    End If
                    If tsRemain.Seconds = 0 Then
                        rString &= "00"
                    ElseIf tsRemain.Seconds > 0 And tsRemain.Seconds < 10 Then
                        rString &= "0" & tsRemain.Seconds.ToString
                    ElseIf tsRemain.Seconds >= 10 Then
                        rString &= tsRemain.Seconds.ToString
                    End If
                    Return rString
                Catch ex As Exception
                    Return "0.00:00:00"
                End Try
            End Get
        End Property
        Public ReadOnly Property tsFrame() As TimeSpan
            Get
                Return _tsFrame
            End Get
        End Property
        Public ReadOnly Property tsEffective As TimeSpan
            Get
                Return _tsEffective
            End Get
        End Property
        Public ReadOnly Property IsWatching() As Boolean
            Get
                Try
                    Return fwQueue.EnableRaisingEvents
                Catch ex As Exception
                    Return False
                End Try
            End Get
        End Property
        Public ReadOnly Property CoreVersion() As String
            Get
                Return _CoreVersion
            End Get
        End Property
        Public ReadOnly Property ClientVersion As String
            Get
                Return _ClientVersion
            End Get
        End Property
        Public Property RestartOnError() As Boolean
            Get
                Return bReStart
            End Get
            Set(ByVal value As Boolean)
                bReStart = value
            End Set
        End Property
#End Region
#Region "Files"
#Region "Queue.dat fw"
        Private Sub SetQProperties()
            fwQueue.EnableRaisingEvents = False
            fwQueue = New FileSystemWatcher
            With fwQueue
                .Filter = _Queue.Replace(_Location & "\", "")
                .NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.Attributes Or NotifyFilters.CreationTime
                .Path = _Location & "\"
            End With
            fwQueue.EnableRaisingEvents = True
            AddQHandlers()
        End Sub
        Private Sub AddQHandlers()
            AddHandler fwQueue.Changed, AddressOf QueueEvent
            AddHandler fwQueue.Created, AddressOf QueueEvent
            AddHandler fwQueue.Deleted, AddressOf QueueEvent
            AddHandler fwQueue.Renamed, AddressOf QueueEvent
            AddHandler fwQueue.Error, AddressOf fwError
        End Sub
        Private Sub RemoveQHandlers()
            fwQueue.EnableRaisingEvents = False
            RemoveHandler fwQueue.Changed, AddressOf QueueEvent
            RemoveHandler fwQueue.Created, AddressOf QueueEvent
            RemoveHandler fwQueue.Deleted, AddressOf QueueEvent
            RemoveHandler fwQueue.Renamed, AddressOf QueueEvent
            RemoveHandler fwQueue.Error, AddressOf fwError
        End Sub
#End Region
#Region "Fahlog.txt fw"
        Private Sub SetLProperties()
            With fwLog
                If My.Computer.FileSystem.FileExists(_Location & "\Fahlog.txt") Then
                    .Filter = "Fahlog.txt"
                Else
                    .Filter = "fahlog.txt"
                End If
                .NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.Attributes Or NotifyFilters.CreationTime
                .Path = _Location & "\"
            End With
            AddLHandlers()
            fwLog.EnableRaisingEvents = True
        End Sub
        Private Sub AddLHandlers()
            AddHandler fwLog.Changed, AddressOf QueueEvent
            AddHandler fwLog.Created, AddressOf QueueEvent
            AddHandler fwLog.Deleted, AddressOf QueueEvent
            AddHandler fwLog.Renamed, AddressOf QueueEvent
            AddHandler fwLog.Error, AddressOf fwError
        End Sub
        Private Sub RemoveLHandlers()
            fwLog.EnableRaisingEvents = False
            RemoveHandler fwLog.Changed, AddressOf QueueEvent
            RemoveHandler fwLog.Created, AddressOf QueueEvent
            RemoveHandler fwLog.Deleted, AddressOf QueueEvent
            RemoveHandler fwLog.Renamed, AddressOf QueueEvent
            RemoveHandler fwLog.Error, AddressOf fwError
        End Sub
#End Region
#Region "Unitinfo.txt fw"
        Private Sub SetUProperties()
            With fWUnitInfo
                If My.Computer.FileSystem.FileExists(_Location & "\Unitinfo.txt") Then
                    .Filter = "Unitinfo.txt"
                Else
                    .Filter = "unitinfo.txt"
                End If
                .NotifyFilter = NotifyFilters.LastAccess Or NotifyFilters.Attributes Or NotifyFilters.CreationTime
                .Path = _Location & "\"
            End With
            AddUHandlers()
            fWUnitInfo.EnableRaisingEvents = True
        End Sub
        Private Sub AddUHandlers()
            AddHandler fWUnitInfo.Changed, AddressOf QueueEvent
            AddHandler fWUnitInfo.Created, AddressOf QueueEvent
            AddHandler fWUnitInfo.Deleted, AddressOf QueueEvent
            AddHandler fWUnitInfo.Renamed, AddressOf QueueEvent
            AddHandler fWUnitInfo.Error, AddressOf fwError
        End Sub
        Private Sub RemoveUHandlers()
            fWUnitInfo.EnableRaisingEvents = False
            RemoveHandler fWUnitInfo.Changed, AddressOf QueueEvent
            RemoveHandler fWUnitInfo.Created, AddressOf QueueEvent
            RemoveHandler fWUnitInfo.Deleted, AddressOf QueueEvent
            RemoveHandler fWUnitInfo.Renamed, AddressOf QueueEvent
            RemoveHandler fWUnitInfo.Error, AddressOf fwError
        End Sub
#End Region
#End Region
#Region "Public tracking functions"
        Public Function CheckCoreExit() As Boolean
            Try
                'Get status of last project?!
                Dim cStatus As clsCoreStatus
                cStatus = CoreStatus(Me.ActiveSlot)
                If Not cStatus.IsEmpty Then
                    Dim bNF As Boolean = False
                    For Each nfString As String In modMySettings.NonFatal.Codes
                        If cStatus.CoreStatus.ToUpper.Contains("CORESTATUS =") And cStatus.CoreStatus.Contains(nfString) And cStatus.CoreStatus.Contains("(" & Convert.ToInt64(nfString, 16) & ")") Then
                            bNF = True
                            Exit For
                        End If
                    Next
                    If Not bNF Then RaiseEvent EUE(cStatus)
                End If
                Return True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Function StartWatching() As Boolean
            Try
                If fwLog.EnableRaisingEvents Then Return True
                StopWatching()
                SetQProperties()
                SetLProperties()
                SetUProperties()
                ReadQueue()
                ActiveSlot.Progress = CurrentProgress()
                Return True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Function StopWatching() As Boolean
            Try
                RemoveQHandlers()
                RemoveLHandlers()
                RemoveUHandlers()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
        Public Sub InitQueue()
            Call QueueEvent(Me, Nothing)
        End Sub
#End Region
        Private Sub fwError(ByVal Sender As Object, ByVal e As System.IO.ErrorEventArgs)
            Try
                WriteLog("clsQueue, FWerror : " & e.GetException.Message)
                If RestartOnError Then
                    If ReferenceEquals(Sender, fwQueue) Then
                        RemoveQHandlers()
                        SetQProperties()
                    ElseIf ReferenceEquals(Sender, fwLog) Then
                        RemoveLHandlers()
                        SetLProperties()
                    ElseIf ReferenceEquals(Sender, fWUnitInfo) Then
                        RemoveUHandlers()
                        SetUProperties()
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private _bReportErrors As Boolean = False, iFailureCount As Int32 = 0
        Private Sub QueueEvent(ByVal Sender As Object, ByVal e As System.IO.FileSystemEventArgs)
            Static bIsBussy As Boolean = False
            Try
                Debug.Print(DateTime.Now & ": Queueevent sub started, Bussy = " & bIsBussy)
                'fLog.UpdateIcon()
                'backup slots
                If bIsBussy Then
                    Exit Sub
                End If
                Dim dtStart As DateTime = DateTime.Now
                'LogWindow.WriteLog("Queue event started")
                bIsBussy = True
                Dim bEntry(0 To 9) As Entry
                Array.Copy(Entries, bEntry, 10)
                Dim bActiveSlot As Int32 = _Current
                Dim biCurrent As Int32 = ActiveSlot.Progress
                If ReadQueue() Then
                    'Check for .Status = "4" followed by .Status = "0"
                    If bEntry(bActiveSlot).Status = "0" And Entries(_Current).Status = "0" Then
                        _iFailedDownloads += 1
                    End If
                    'Check for new active slot
                    If bActiveSlot <> _Current Then
                        'Set _progress to 0
                        _Progress = 0
                        _tsFrame = TimeSpan.FromSeconds(0)
                        _EtaDT = #1/1/2000#
                        _LastFrameCompletionTime = #1/1/2000#

                        RaiseEvent ProjectStart(ActiveSlot)
                    Else
                        'Check progress
                        If CurrentProgress() <> biCurrent Then
                            'Get frame time, dt last frame and eta ect and store in private declarations
                            _tsFrame = CurrentFrameTime()
                            _EtaDT = ETA_Date()
                            If _Progress = 100 Then 'Finished project.
                                Dim cStatus As clsCoreStatus = CoreStatus(bEntry(bActiveSlot))
                                Entries(bActiveSlot).CoreStatus = cStatus
                                If Not cStatus.IsEmpty Then
                                    Dim bNF As Boolean = False
                                    For Each nfString As String In modMySettings.NonFatal.Codes
                                        If cStatus.CoreStatus.ToUpper.Contains(nfString.ToUpper) Or nfString.ToUpper.Contains(cStatus.CoreStatus.ToUpper) Then
                                            bNF = True
                                            Exit For
                                        End If
                                    Next
                                    If Not bNF Then RaiseEvent EUE(cStatus)
                                End If
                                RaiseEvent ProjectEnd(bEntry(bActiveSlot), _tsFrame, _LastFrameCompletionTime, _Progress)
                                _Progress = 0
                                _tsFrame = TimeSpan.FromSeconds(0)
                                _EtaDT = #1/1/2000#
                                _LastFrameCompletionTime = #1/1/2000#
                            ElseIf (_Progress >= (biCurrent + 1)) Or (biCurrent = 0 And _Progress > 0) Then 'New Frame
                                If _Progress = 0 Or biCurrent = 0 Then
                                    _CoreVersion = ("FahCore_" & ActiveSlot.CoreNumber & " (" & GetCoreVersion() & ")").Replace("()", "")
                                End If
                                _ppdEffective = PPDEffective()
                                _ppdLastFrame = PPDLastFrame()
                                RaiseEvent NewFrame(_Progress, _tsFrame, _LastFrameCompletionTime)
                            End If
                        End If
                        'Compare entries for succesfull and failed uploads
                        For yInt As Int32 = 0 To 9
                            If bEntry(yInt).Status = "2" And Entries(yInt).Status = "0" Then
                                'Add entry to succesfully uploaded entries
                                RaiseEvent SuccesfullUpload(Entries(yInt))
                            ElseIf CInt(Entries(yInt).UploadFailures) > CInt(bEntry(yInt).UploadFailures) Then
                                'Add upload failure notice
                                RaiseEvent FailedUpload(Entries(yInt))
                            End If
                        Next
                    End If
                    ActiveSlot.Progress = _Progress
                Else
                    WriteLog("QueueEvent could not be procesed due to previous errors ( ReadQueue? )")
                End If
                bIsBussy = False
                WriteLog("-Queue event handeld in " & DateTime.Now.Subtract(dtStart).TotalMilliseconds & "ms")
            Catch ex As Exception
                If _bReportErrors Then
                    WriteError(ex.Message, Err)
                Else
                    iFailureCount += 1
                    If iFailureCount > 2 Then
                        _bReportErrors = True
                    End If
                End If
                bIsBussy = False
            End Try
        End Sub
        Public ReadOnly Property CoreStatus(ByVal Entry As clsQueue.Entry) As clsCoreStatus
            Get
                Try
                    Dim rCoreStatus As New clsCoreStatus(Entry)
                    If My.Computer.FileSystem.FileExists(_Location & "\fahlog.txt") Then
                        _logfile_ = _Location & "\fahlog.txt"
                    ElseIf My.Computer.FileSystem.FileExists(_Location & "\Fahlog.txt") Then
                        _logfile_ = _Location & "\Fahlog.txt"
                    Else
                        GoTo ReturnNothing
                    End If
                    Dim fStream As FileStream = New FileStream(_logfile_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                    Dim fRead As StreamReader = New StreamReader(fStream)
                    Dim Log As String = fRead.ReadToEnd
                    Dim Lines() As String = Nothing
                    If Log.Contains(vbNewLine) Then
                        Lines = Log.Split(CChar(vbNewLine))
                    ElseIf Log.Contains(vbCrLf) Then
                        Lines = Log.Split(CChar(vbCrLf))
                    ElseIf Log.Contains(vbLf) Then
                        Lines = Log.Split(CChar(vbLf))
                    End If
                    Dim iStart As Int32 = -1 : Dim iEnd As Int32 = -1 : Dim iStatus As Int32 = -1 : Dim iProject As Int32 = -1
                    For xInd As Int32 = Lines.GetUpperBound(0) To (Lines.GetUpperBound(0) - 150) Step -1
                        Dim sLine As String = Lines(xInd).ToUpper
                        If sLine.Contains("CORESTATUS =") Then
                            iStatus = xInd
                            rCoreStatus.CoreStatus = Mid(sLine, 9)
                        End If
                        If sLine.Contains("PROJECT: " & Entry.Project.Project) And sLine.Contains("RUN " & Entry.Project.Run) And sLine.Contains("CLONE " & Entry.Project.Clone) And sLine.Contains("GEN " & Entry.Project.Gen) Then
                            iProject = xInd
                            If iStatus <> -1 Then
                                Array.Copy(Lines, iStatus - 10, rCoreStatus.LogSnippet, 0, 20)
                                Return rCoreStatus
                            End If
                        End If
                    Next
ReturnNothing:
                    rCoreStatus.IsEmpty = True
                    Return rCoreStatus
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Dim Cempty As New clsCoreStatus(ActiveSlot)
                    Cempty.IsEmpty = True
                    Return Cempty
                End Try
            End Get
        End Property
        'Private parsing functions
        Private Function PPDLastFrame() As Double
            Try
                If _tsFrame.TotalSeconds = 0 Or Not ProjectInfo.KnownProject(ActiveSlot.Project.Project) Then Return 0
                'get kFactor
                Dim iKfactor As Double = CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).kFactor.Replace(".", ","))
                Dim iPworth As Double = CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).Credit.Replace(".", ","))
                If iKfactor > 0 Then
                    'check if eta is before preferred
                    If _EtaDT < ActiveSlot.Issued.AddDays(CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).PreferredDays.Replace(".", ","))) Then
                        'final_points = base_points * max(1,sqrt(k*deadline_length/elapsed_time))
                        'estimated length of unit
                        Dim uEst As TimeSpan = Eta.Subtract(ActiveSlot.Issued)
                        Dim bMulti As Double = Math.Sqrt((CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) * iKfactor) / uEst.TotalDays)
                        iPworth = Math.Round(iPworth * bMulti)
                    End If
                End If
                iPworth = iPworth / 100.0F
                'How many frames per 24/h
                Dim iPPD As Double = 0
                Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
                Do
                    If tsDay.Subtract(_tsFrame).TotalSeconds >= 0 Then
                        iPPD += iPworth
                        tsDay = tsDay.Subtract(_tsFrame)
                    Else
                        Exit Do
                    End If
                Loop
                'get fraction of _tsFrame to be done in remaining seconds
                Dim iRfraction As Double
                If tsDay.TotalSeconds > 0 Then
                    iRfraction = tsDay.TotalSeconds / _tsFrame.TotalSeconds
                    iPPD += iRfraction * iPworth
                End If

                Return (Math.Round(iPPD, 2))
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return 0
            End Try
        End Function
        Private Function PPDEffective() As Double
            Try
                'Get begin time, current progress
                Dim dtBegin As DateTime = ActiveSlot.TimeData.BeginTime
                If dtBegin = #1/1/2000# Then Return 0
                If Eta = DateTime.MinValue Then Return 0
                If Eta = #1/1/2000# Then Return 0
                If _Progress = 0 Then Return 0
                'When looking back, take average frametime = _progress \ (datetime.now - dtbegin) 
                Dim tsHistoricalAverage As TimeSpan = DateTime.Now.Subtract(dtBegin)
                tsHistoricalAverage = TimeSpan.FromSeconds(tsHistoricalAverage.TotalSeconds / _Progress)
                _tsEffective = tsHistoricalAverage
                Dim dtExpected As DateTime = _LastFrameCompletionTime
                For xInt As Int32 = _Progress To 100
                    dtExpected = dtExpected.Add(tsHistoricalAverage)
                Next
                'Use eta date - begindate
                Dim tsProject As TimeSpan = TimeSpan.FromSeconds(dtExpected.Subtract(dtBegin).TotalSeconds / 100)
                Dim iKfactor As Double = CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).kFactor.Replace(".", ","))
                Dim iPworth As Double = CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).Credit.Replace(".", ","))
                If iKfactor > 0 Then
                    'check if eta is before preferred
                    If _EtaDT < ActiveSlot.Issued.AddDays(CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).PreferredDays.Replace(".", ","))) Then
                        'final_points = base_points * max(1,sqrt(k*deadline_length/elapsed_time))
                        'estimated length of unit
                        Dim uEst As TimeSpan = Eta.Subtract(ActiveSlot.Issued)
                        Dim bMulti As Double = Math.Sqrt((CDbl(ProjectInfo.Project(ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) * iKfactor) / uEst.TotalDays)
                        iPworth = Math.Round(iPworth * bMulti)
                    End If
                End If
                iPworth = iPworth / 100.0F
                'How many frames per 24/h
                Dim iPPD As Double = 0
                Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
                Do
                    If tsDay.Subtract(tsProject).TotalSeconds >= 0 Then
                        iPPD += iPworth
                        tsDay = tsDay.Subtract(tsProject)
                    Else
                        Exit Do
                    End If
                Loop
                'get fraction of _tsFrame to be done in remaining seconds
                Dim iRfraction As Double
                If tsDay.TotalSeconds > 0 Then
                    iRfraction = tsDay.TotalSeconds / tsProject.TotalSeconds
                    iPPD += iRfraction * iPworth
                End If
                Return (Math.Round(iPPD, 2))
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return 0
            End Try
        End Function
        Private Function CurrentProgress() As Int32
            Try
                If Not HasError And FullSlots Then
                    _logfile_ = _Location & "\work\logfile_0" & Current.ToString & ".txt"
                    If Not My.Computer.FileSystem.FileExists(_logfile_) Then Return -2
                    If ActiveSlot.Status = "1" Then
                        Dim fStream As FileStream = New FileStream(_logfile_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                        If fStream.Length = _lngLogFile_Size Then
                            'Log to window
                            Return _Progress
                        End If
                        _lngLogFile_Size = fStream.Length
                        Dim fRead As StreamReader = New StreamReader(fStream)
                        Dim Log As String = fRead.ReadToEnd.ToUpper
                        Dim Lines() As String = Nothing
                        If Log.Contains(vbNewLine) Then
                            Lines = Log.Split(CChar(vbNewLine))
                        ElseIf Log.Contains(vbCr) Then
                            Lines = Log.Split(CChar(vbCr))
                        ElseIf Log.Contains(vbCrLf) Then
                            Lines = Log.Split(CChar(vbCrLf))
                            'ElseIf Log.Contains(CByte(&HA)) Or Log.Contains(ChrW(10)) Then
                            '    Lines = Log.Split(ChrW(&HA))
                        End If
                        For xInd As Int32 = Lines.GetUpperBound(0) To 0 Step -1
                            With Lines(xInd)
                                If .Contains("COMPLETED") And .Contains("%") And Not .Contains("STEPS") Then
                                    Dim lFrame As String = Mid(Lines(xInd), Lines(xInd).LastIndexOf("COMPLETED") + 10)
                                    lFrame = lFrame.Replace("%", "")
                                    lFrame = lFrame.Trim(CChar(" "))
                                    _Progress = CInt(lFrame)
                                    Return CInt(lFrame)
                                ElseIf .Contains("COMPLETED") And .Contains("PERCENT") Then
                                    Dim lFrame As String = Lines(xInd).ToUpper
                                    If lFrame.IndexOf("PERCENT)") <> -1 OrElse lFrame.IndexOf("PERCENT )") <> -1 Then
                                        'If InStr(lFrame, "PERCENT)") Or InStr(lFrame, "PERCENT )") Then
                                        lFrame = Mid(lFrame, lFrame.LastIndexOf("("))
                                        lFrame = lFrame.Replace(")", "")
                                        lFrame = lFrame.Replace("(", "")
                                        lFrame = lFrame.Replace("PERCENT", "")
                                        lFrame = lFrame.Trim(CChar(" "))
                                        _Progress = CInt(lFrame)
                                        Return _Progress
                                    Else
                                        'assume no "("
                                        lFrame = lFrame.Substring(lFrame.IndexOf("PERCENT") - Len("PERCENT") - 3).Trim
                                        'lFrame = Mid(lFrame, "PERCENT" - 3)
                                        lFrame = lFrame.Trim(CChar(" "))
                                        _Progress = CInt(lFrame)
                                        Return _Progress
                                    End If
                                ElseIf .Contains("COMPLETED") And .Contains("STEPS") And .Contains("%") Then
                                    Dim lFrame As String = Mid(Lines(xInd), Lines(xInd).LastIndexOf("(") + 1)
                                    lFrame = lFrame.Replace("(", "")
                                    lFrame = lFrame.Replace(")", "")
                                    lFrame = lFrame.Replace("%", "")
                                    _Progress = CInt(lFrame)
                                    Return _Progress
                                ElseIf .Contains("COMPLETED") And .Contains("STEPS") Then
                                    'get total steps
                                    Dim tTSteps As String = Mid(Lines(xInd), Lines(xInd).LastIndexOf("OUT OF ") + 7)
                                    tTSteps = tTSteps.Replace("STEPS", "")
                                    tTSteps = tTSteps.Trim(CChar(" "))
                                    Dim iTSteps As Integer = CInt(tTSteps)
                                    'Get current step
                                    Dim tCStep As String = Mid(Lines(xInd), Lines(xInd).LastIndexOf("COMPLETED") + 9, Lines(xInd).Length - Lines(xInd).LastIndexOf("OUT OF"))
                                    tCStep = tCStep.Trim(CChar(" "))
                                    Dim iCStep As Integer = CInt(tCStep)
                                    'icstep / (itsteps / 100) = percentage
                                    _Progress = CInt(iCStep / (iTSteps / 100))
                                    Return CInt(iCStep / (iTSteps / 100))
                                ElseIf .Contains("FINISHED A FRAME") Then
                                    'Get total frames
                                    If ProjectInfo.KnownProject(ActiveSlot.Project.Project) Then
                                        Dim iTFrames As Integer = CInt(ProjectInfo.Project(ActiveSlot.Project.Project).Frames)
                                        Dim tCFrame As String = Mid(Lines(xInd), Lines(xInd).LastIndexOf("FINISHED A FRAME") + Len("FINISHED A FRAME") + 1)
                                        tCFrame = tCFrame.Replace("(", "")
                                        tCFrame = tCFrame.Replace(")", "")
                                        tCFrame = tCFrame.Trim(CChar(" "))
                                        Dim iCFrame As Integer = CInt(tCFrame)
                                        'cint(icframe / (itframes / 100))
                                        _Progress = CInt(iCFrame / (iTFrames / 100))
                                        Return CInt(iCFrame / (iTFrames / 100))
                                    Else
                                        _Progress = 0
                                        Return 0
                                    End If
                                Else
                                    _Progress = 0
                                    Return 0
                                End If
                            End With
                        Next
                    ElseIf ActiveSlot.Status = "2" Then
                        _Progress = 0
                        Return 0
                    Else
                        _Progress = 0
                        Return 0
                    End If
                Else
                    _Progress = 0
                    Return 0
                End If
                _Progress = 0
                Return 0
            Catch ex As Exception
                _Progress = 0
                Return 0
            End Try
        End Function
        Private Function CurrentFrameTime() As TimeSpan
            Try
                If Not HasError() And FullSlots Then
                    If ActiveSlot.Status = "1" Then
                        If My.Computer.FileSystem.FileExists(_Location & "\Fahlog.txt") Then
                            _logfile_ = _Location & "\Fahlog.txt"
                        ElseIf My.Computer.FileSystem.FileExists(_Location & "\fahlog.txt") Then
                            _logfile_ = _Location & "\fahlog.txt"
                        End If
                        Dim fStream As FileStream = New FileStream(_logfile_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                        Dim fRead As StreamReader = New StreamReader(fStream)
                        Dim Log As String = fRead.ReadToEnd
                        Dim Lines() As String = Nothing
                        If Log.Contains(vbNewLine) Then
                            Lines = Log.Split(CChar(vbNewLine))
                        ElseIf Log.Contains(vbCrLf) Then
                            Lines = Log.Split(CChar(vbCrLf))
                        ElseIf Log.Contains(vbLf) Then
                            Lines = Log.Split(CChar(vbLf))
                        End If
                        For xInd As Integer = Lines.GetUpperBound(0) To 0 Step -1
                            With Lines(xInd).ToUpper
                                If .Contains("Arguments:".ToUpper) Then
                                    _Arguments = Mid(Lines(xInd).ToUpper, Lines(xInd).ToUpper.LastIndexOf("Arguments:".ToUpper) + Len("Arguments:") + 1)
                                    _Arguments = _Arguments.Trim(CChar(vbCrLf))
                                ElseIf .Contains("Folding@Home Client Version".ToUpper) Then
                                    _ClientVersion = Mid((Lines(xInd).ToUpper), (Lines(xInd).ToUpper.IndexOf("Folding@Home Client Version".ToUpper) + Len("Folding@Home Client Version") + 1))
                                    _ClientVersion = _ClientVersion.TrimEnd(CChar(vbLf)).TrimEnd(CChar(vbNewLine)).TrimEnd(CChar(vbCrLf))
                                    _ClientVersion = _ClientVersion.Replace(" ", "")
                                End If
                                If .Contains("COMPLETED") And Not .Contains("STEPS") And Not .Contains("UNITS") And Not .Contains("UNSENT") And Not .Contains("AUTOSEND") Then
                                    'GET TIME
                                    Dim dL As New DateTime
                                    Dim strT As String = Mid(Lines(xInd), Lines(xInd).IndexOf("[") + 2, 8)
                                    Dim strThours As String = Mid(strT, 1, 2)
                                    Dim strtMinutes As String = Mid(strT, 4, 2)
                                    Dim strtSeconds As String = Mid(strT, 7, 2)
                                    dL = DateTime.UtcNow.Date
                                    dL = dL.AddHours(CDbl(strThours)).AddMinutes(CDbl(strtMinutes)).AddSeconds(CDbl(strtSeconds))
                                    _LastFrameCompletionTime = TimeZoneInfo.ConvertTimeFromUtc(dL, TimeZoneInfo.Local)
                                    For xInd2 As Int32 = xInd - 1 To 0 Step -1
                                        With Lines(xInd2).ToUpper
                                            If .Contains("COMPLETED") And Not .Contains("UNITS") And Not .Contains("UNSENT") And Not .Contains("REMAINING") And Not .Contains("AUTOSEND") Then
                                                Dim dB As New DateTime
                                                Dim strB As String = Mid(Lines(xInd2), Lines(xInd2).IndexOf("[") + 2, 8)
                                                Dim strBHours As String = Mid(strB, 1, 2)
                                                Dim strBMinutes As String = Mid(strB, 4, 2)
                                                Dim strBSeconds As String = Mid(strB, 7, 2)
                                                dB = DateTime.UtcNow.Date
                                                dB = dB.AddHours(CDbl(strBHours)).AddMinutes(CDbl(strBMinutes)).AddSeconds(CDbl(strBSeconds))
                                                If dB = dL Then
                                                    GoTo Skip
                                                End If
                                                Dim tsTest As TimeSpan = dL.Subtract(dB)
                                                If tsTest.TotalSeconds > 0 Then
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                Else
                                                    'Negative timespan can only occus when the dl and db are on diffrent days, substract 1 day from db
                                                    dB = dB.Subtract(TimeSpan.FromDays(1))
                                                    tsTest = dL.Subtract(dB)
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                End If
                                            ElseIf .Contains("PROJECT") Then
                                                _tsFrame = TimeSpan.FromSeconds(0)
                                                Return TimeSpan.FromSeconds(0)
                                            End If
                                        End With
Skip:
                                    Next
                                ElseIf .Contains("COMPLETED") And .Contains("STEPS") Then
                                    'GET TIME
                                    Dim dL As New DateTime
                                    Dim strT As String = Mid(Lines(xInd), Lines(xInd).IndexOf("[") + 2, 8)
                                    Dim strThours As String = Mid(strT, 1, 2)
                                    Dim strtMinutes As String = Mid(strT, 4, 2)
                                    Dim strtSeconds As String = Mid(strT, 7, 2)
                                    dL = DateTime.UtcNow.Date
                                    dL = dL.AddHours(CDbl(strThours)).AddMinutes(CDbl(strtMinutes)).AddSeconds(CDbl(strtSeconds))
                                    _LastFrameCompletionTime = TimeZoneInfo.ConvertTimeFromUtc(dL, TimeZoneInfo.Local)
                                    For xInd2 As Int32 = xInd - 1 To 0 Step -1
                                        With Lines(xInd2).ToUpper
                                            If .Contains("COMPLETED") And .Contains("STEPS") Then
                                                Dim dB As New DateTime
                                                Dim strB As String = Mid(Lines(xInd2), Lines(xInd2).IndexOf("[") + 2, 8)
                                                Dim strBHours As String = Mid(strB, 1, 2)
                                                Dim strBMinutes As String = Mid(strB, 4, 2)
                                                Dim strBSeconds As String = Mid(strB, 7, 2)
                                                dB = DateTime.UtcNow.Date
                                                dB = dB.AddHours(CDbl(strBHours)).AddMinutes(CDbl(strBMinutes)).AddSeconds(CDbl(strBSeconds))
                                                If dB = dL Then
                                                    GoTo Skip2
                                                End If
                                                Dim tsTest As TimeSpan = dL.Subtract(dB)
                                                If tsTest.TotalSeconds > 0 Then
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                Else
                                                    'Negative timespan can only occus when the dl and db are on diffrent days, substract 1 day from db
                                                    dB = dB.Subtract(TimeSpan.FromDays(1))
                                                    tsTest = dL.Subtract(dB)
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                End If
                                            ElseIf .Contains("PROJECT") Then
                                                _tsFrame = TimeSpan.FromSeconds(0)
                                                Return TimeSpan.FromSeconds(0)
                                            End If
                                        End With
Skip2:
                                    Next
                                ElseIf .Contains("FINISHED A FRAME") Then
                                    'GET TIME
                                    Dim dL As New DateTime
                                    Dim strT As String = Mid(Lines(xInd), Lines(xInd).IndexOf("[") + 2, 8)
                                    Dim strThours As String = Mid(strT, 1, 2)
                                    Dim strtMinutes As String = Mid(strT, 4, 2)
                                    Dim strtSeconds As String = Mid(strT, 7, 2)
                                    dL = DateTime.UtcNow.Date
                                    dL = dL.AddHours(CDbl(strThours)).AddMinutes(CDbl(strtMinutes)).AddSeconds(CDbl(strtSeconds))
                                    _LastFrameCompletionTime = TimeZoneInfo.ConvertTimeFromUtc(dL, TimeZoneInfo.Local)
                                    For xInd2 As Int32 = xInd - 1 To 0 Step -1
                                        With Lines(xInd2).ToUpper
                                            If .Contains("FINISHED A FRAME") Then
                                                Dim dB As New DateTime
                                                Dim strB As String = Mid(Lines(xInd2), Lines(xInd2).IndexOf("[") + 2, 8)
                                                Dim strBHours As String = Mid(strB, 1, 2)
                                                Dim strBMinutes As String = Mid(strB, 4, 2)
                                                Dim strBSeconds As String = Mid(strB, 7, 2)
                                                dB = DateTime.UtcNow.Date
                                                dB = dB.AddHours(CDbl(strBHours)).AddMinutes(CDbl(strBMinutes)).AddSeconds(CDbl(strBSeconds))
                                                If dB = dL Then
                                                    GoTo Skip3
                                                End If
                                                Dim tsTest As TimeSpan = dL.Subtract(dB)
                                                If tsTest.TotalSeconds > 0 Then
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                Else
                                                    'Negative timespan can only occus when the dl and db are on diffrent days, substract 1 day from db
                                                    dB = dB.Subtract(TimeSpan.FromDays(1))
                                                    tsTest = dL.Subtract(dB)
                                                    _tsFrame = tsTest
                                                    Return tsTest
                                                End If
                                            ElseIf .Contains("PROJECT") Then
                                                _tsFrame = TimeSpan.FromSeconds(0)
                                                Return TimeSpan.FromSeconds(0)
                                            End If
                                        End With
Skip3:
                                    Next
                                ElseIf .Contains("PROJECT") Then
                                    Return TimeSpan.FromSeconds(0)
                                End If
                            End With
                        Next
                    End If
                End If
            Catch ex As Exception
                _Err.Clear()
                _Err.Description = Err.Description
                _Err.Number = CStr(Err.Number)
                _HasError = True
                _LastFrameCompletionTime = #1/1/2000#
                _tsFrame = TimeSpan.FromSeconds(0)
                Return TimeSpan.FromSeconds(0)
            End Try
        End Function
        Private Function ETA_Date() As DateTime
            Try
                If _tsFrame.TotalSeconds <= 0 Or _Progress = 0 Or _LastFrameCompletionTime = #1/1/2000# Then
                    Return #1/1/2000#
                End If
                'Multiply tsframe with (100 - progress) and add to 
                Dim tRemain As TimeSpan = TimeSpan.FromSeconds(_tsFrame.TotalSeconds * (100 - _Progress))
                Dim dEta As DateTime = _LastFrameCompletionTime.Add(tRemain)
                Return dEta
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return #1/1/2000#
            End Try

        End Function
        Private Function GetCoreVersion() As String
            Try
                _logfile_ = _Location & "\work\logfile_0" & Current.ToString & ".txt"
                If Not My.Computer.FileSystem.FileExists(_logfile_) Then Return ""
                If ActiveSlot.Status = "1" Then
                    Dim fStream As FileStream = New FileStream(_logfile_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                    Dim fRead As StreamReader = New StreamReader(fStream)
                    Dim Log As String = fRead.ReadToEnd
                    fRead.Close()
                    fStream.Close()
                    fRead = Nothing : fStream = Nothing
                    Dim Lines() As String = Nothing
                    If Log.Contains(vbNewLine) Then
                        Lines = Log.Split(CChar(vbNewLine))
                    ElseIf Log.Contains(vbCrLf) Then
                        Lines = Log.Split(CChar(vbCrLf))
                    ElseIf Log.Contains(vbLf) Then
                        Lines = Log.Split(CChar(vbLf))
                    End If
                    Dim bArg As Boolean = False, bCore As Boolean = False, bVersion As Boolean = False
                    For xInd As Integer = 0 To Lines.GetUpperBound(0)
                        With Lines(xInd).ToUpper
                            If .Contains("Arguments:".ToUpper) Then
                                _Arguments = Mid(Lines(xInd).ToUpper, Lines(xInd).ToUpper.LastIndexOf("Arguments:".ToUpper) + Len("Arguments:") + 1)
                                _Arguments = _Arguments.Trim(CChar(vbCrLf))
                                bArg = True
                            ElseIf .Contains("Folding@Home Client Version".ToUpper) Then
                                _ClientVersion = Mid((Lines(xInd).ToUpper), (Lines(xInd).ToUpper.IndexOf("Folding@Home Client Version".ToUpper) + Len("Folding@Home Client Version") + 1))
                                _ClientVersion = _ClientVersion.TrimEnd(CChar(vbLf)).TrimEnd(CChar(vbNewLine)).TrimEnd(CChar(vbCrLf))
                                _ClientVersion = _ClientVersion.Replace(" ", "")
                                bVersion = True
                            End If
                            If .Contains("*------------------------------*") Then
                                'Check 2 down
                                If Lines(xInd + 2).ToUpper.Contains("VERSION") Then
                                    Dim strVer As String = Mid(Lines(xInd + 2), Lines(xInd + 2).IndexOf("]") + 2)
                                    strVer = Mid(strVer, 1, strVer.LastIndexOf("(") - 1).ToUpper.Replace("VERSION", "").Trim(CChar(" "))
                                    _CoreVersion = strVer
                                    bCore = True
                                End If
                            ElseIf .Contains("FOLDING@HOME ") And .Contains(" CORE") Then
                                'Check 1 down
                                If Lines(xInd + 1).ToUpper.Contains("VERSION") Then
                                    Dim strVer As String = Mid(Lines(xInd + 1), Lines(xInd + 2).IndexOf("]") + 2)
                                    strVer = Mid(strVer, 1, strVer.LastIndexOf("(") - 1).ToUpper.Replace("VERSION", "").Trim(CChar(" "))
                                    _CoreVersion = strVer
                                    bCore = True
                                End If
                            ElseIf bCore And bVersion Then
                                Return _CoreVersion
                            End If
                        End With
                    Next
                    Return ""
                Else
                    Return ""
                End If
            Catch ex As Exception
                WriteError("GetCoreVersion", Err)
                Return ""
            End Try
        End Function
        Private Function GetClientVersion() As String
            Try
                _logfile_ = _Location & "\fahlog.txt"
                Dim fStream As FileStream = New FileStream(_logfile_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                Dim fRead As StreamReader = New StreamReader(fStream)
                Dim Log As String = fRead.ReadToEnd
                fRead.Close()
                fStream.Close()
                fRead = Nothing : fStream = Nothing
                Dim Lines() As String = Nothing
                If Log.Contains(vbNewLine) Then
                    Lines = Log.Split(CChar(vbNewLine))
                ElseIf Log.Contains(vbCrLf) Then
                    Lines = Log.Split(CChar(vbCrLf))
                ElseIf Log.Contains(vbLf) Then
                    Lines = Log.Split(CChar(vbLf))
                End If
                For xInd As Integer = 0 To Lines.GetUpperBound(0)
                    With Lines(xInd).ToUpper
                        If .Contains("Folding@Home Client Version".ToUpper) Then
                            _ClientVersion = Mid((Lines(xInd).ToUpper), (Lines(xInd).ToUpper.IndexOf("Folding@Home Client Version".ToUpper) + Len("Folding@Home Client Version")))
                            _ClientVersion = _ClientVersion.TrimEnd(CChar(vbLf)).TrimEnd(CChar(vbNewLine)).TrimEnd(CChar(vbCrLf))
                        End If
                    End With
                Next
                Return _ClientVersion
            Catch ex As Exception
                WriteError("GetClientVersion", Err)
                Return ""
            End Try
        End Function
#End Region
#Region "Core status"
        <Serializable()> _
        Public Class clsCoreStatus
            Public IsEmpty As Boolean = False
            Public cEntry As New clsQueue.Entry
            Public CoreStatus As String = ""
            Public LogSnippet() As String
            Public ClientLocation As String = ""
            Public ReadOnly Property SnippetSize() As Int32
                Get
                    Try
                        Return (LogSnippet.GetUpperBound(0) - LogSnippet.GetLowerBound(0))
                    Catch ex As Exception
                        Return 0
                    End Try
                End Get
            End Property
            Sub New(ByVal Entry As clsQueue.Entry)
                cEntry = Entry
            End Sub
        End Class

#End Region
#Region "Queue.dat"
        Private _Version As String = ""
        Private _Current As Int32 = 0
        Private _PerformanceFraction As String = ""
        Private _PerformanceUnits As String = ""
        Private _DownloadRate As String = ""
        Private _DownloadRateUnits As String = ""
        Private _UploadRate As String = ""
        Private _UploadRateUnits As String = ""
        Private _TotalUploadFailures As Int32 = 0
        Private _WUsReady As Int32 = 0
        Private _HasError As Boolean = False
        Private _FullSlots As Boolean = False
        Public Structure s_Err
            Public Description As String
            Public Number As String
            Public Sub Clear()
                Description = ""
                Number = ""
            End Sub
        End Structure
        Private _Err As s_Err
        Private fBuff() As Byte
        Private Entries(0 To 9) As Entry
        Private _Queue As String
        Public ReadOnly Property StatsFile() As String
            Get
                Try
                    Dim strRet As String = ActiveSlot.Project.Project & "(R" & ActiveSlot.Project.Run & "G" & ActiveSlot.Project.Gen & "C" & ActiveSlot.Project.Clone & ")-" & ActiveSlot.TimeData.strIdentifier & ".bin"
                    Return strRet
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return vbNullString
                End Try
            End Get
        End Property
        Public ReadOnly Property Version() As String
            Get
                Return _Version
            End Get
        End Property
        Public ReadOnly Property Current() As Int32
            Get
                Return _Current
            End Get
        End Property
        Public ReadOnly Property PerformanceFraction() As String
            Get
                Return _PerformanceFraction
            End Get
        End Property
        Public ReadOnly Property PerformanceFractionUnits() As String
            Get
                Return _PerformanceUnits
            End Get
        End Property
        Public ReadOnly Property DownloadRate() As String
            Get
                Return _DownloadRate
            End Get
        End Property
        Public ReadOnly Property DownloadRateUnits() As String
            Get
                Return _DownloadRateUnits
            End Get
        End Property
        Public ReadOnly Property UploadRate() As String
            Get
                Return _UploadRate
            End Get
        End Property
        Public ReadOnly Property UploadRateUnits() As String
            Get
                Return _UploadRateUnits
            End Get
        End Property
        Public ReadOnly Property TotalUploadFailures() As Int32
            Get
                Return _TotalUploadFailures
            End Get
        End Property
        Public ReadOnly Property WUs_Ready() As Int32
            Get
                Return _WUsReady
            End Get
        End Property
        Public ReadOnly Property HasError() As Boolean
            Get
                Return _HasError
            End Get
        End Property
        Public ReadOnly Property LastError() As s_Err
            Get
                Try
                    If Not _HasError Then Return Nothing
                    Return _Err
                Catch ex As Exception
                    _Err.Description = Err.Description
                    _Err.Number = CStr(Err.Number)
                    _HasError = True
                    Return Nothing
                End Try
            End Get
        End Property
        Public ReadOnly Property ActiveSlot() As Entry
            Get
                If _HasError Then Return Nothing
                If Not _FullSlots Then Return Nothing
                Return Entries(Current)
            End Get
        End Property
        Public ReadOnly Property Slot(ByVal Index As Int32) As Entry
            Get
                Try
                    If Not _FullSlots Then Return Nothing
                    If Index > 9 Then Index = 9 : If Index < 0 Then Index = 0
                    Return Entries(Index)
                Catch ex As Exception
                    _Err.Number = CStr(Err.Number)
                    _Err.Description = Err.Description
                    _HasError = True
                    Return Nothing
                End Try
            End Get
        End Property
        Public ReadOnly Property FullSlots() As Boolean
            Get
                Return _FullSlots
            End Get
        End Property
        <Serializable()> _
        Public Class Entry
            Public Status As String = ""
            Private iProgress As Int32
            Public Property Progress() As Int32
                Get
                    Return iProgress
                End Get
                Set(ByVal value As Int32)
                    iProgress = value
                End Set
            End Property
            <Serializable()> _
            Public Structure sTimeData
                Public BeginTime As DateTime
                Public EndTime As DateTime
                Public ReadOnly Property strIdentifier() As String
                    Get
                        Dim strRet As String = BeginTime.Year.ToString
                        If BeginTime.Month < 10 Then
                            strRet &= "0" & BeginTime.Month
                        Else
                            strRet &= BeginTime.Month
                        End If
                        If BeginTime.Day < 10 Then
                            strRet &= "0" & BeginTime.Day
                        Else
                            strRet &= BeginTime.Day
                        End If
                        If BeginTime.Hour < 10 Then
                            strRet &= "0" & BeginTime.Hour
                        Else
                            strRet &= BeginTime.Hour
                        End If
                        If BeginTime.Minute < 10 Then
                            strRet &= "0" & BeginTime.Minute
                        Else
                            strRet &= BeginTime.Minute
                        End If
                        If BeginTime.Second < 10 Then
                            strRet &= "0" & BeginTime.Second
                        Else
                            strRet &= BeginTime.Second
                        End If
                        Return strRet
                    End Get
                End Property
                Sub Init()
                    BeginTime = New DateTime
                    EndTime = New DateTime
                    BeginTime = #1/1/2000#
                    EndTime = #1/1/2000#
                End Sub
                Public Sub New(ByVal Start As String)
                    Init()
                End Sub
            End Structure
            Public TimeData As New sTimeData("")
            Public UploadStatus As String = ""
            Public CoreURL As String = ""
            Public CoreNumber As String = ""
            Public WUSize As Long = 0
            <Serializable()> _
            Public Structure sProject
                Public Project As String
                Public Run As String
                Public Clone As String
                Public Gen As String
            End Structure
            Public ReadOnly Property PRCG As String
                Get
                    Dim rString As String = Project.Project & " (R" & Project.Run & "C" & Project.Clone & "G" & Project.Gen & ")"
                    Return rString
                End Get
            End Property
            Public Project As New sProject
            Public Issued As DateTime = #1/1/1970#
            Public Uploaded As DateTime
            Public MachineID As String = ""
            Public ServerIP As String = ""
            Public ServerPort As String = ""
            Public WorkUnitType As String = "" 'Usually message
            Public UserName As String = ""
            Public TeamNumber As String = ""
            Public PassKey As String = ""
            Public BenchMark As String = ""
            <Serializable()> _
            Public Enum eCpuType
                CPU_UNKNOWN = 0
                CPU_X86 = 1
                CPU_POWERPC = 2
                CPU_MIPS = 3
                CPU_MIPS64 = 4
                CPU_ALPHA = 5
                CPU_PA_RISC = 6
                CPU_68K = 7
                CPU_SPARC = 8
                CPU_SPARC64 = 9
                CPU_POWER = 10
                CPU_VAX = 11
                CPU_ARM = 12
                CPU_88K = 13
                CPU_S390 = 14
                CPU_SH4 = 15
            End Enum
            Public CpuType As eCpuType
            <Serializable()> _
            Public Enum eOsType
                OS_UNKNOWN = 0
                OS_WIN32 = 1
                OS_MACOS = 2
                OS_MACOSX = 3
                OS_LINUX = 4
                OS_BSDI = 5
                OS_NETBSD = 6
                OS_FREEBSD = 7
                OS_OPENBSD = 8
                OS_NEXTSTEP = 9
                OS_BEOS = 10
                OS_IRIX = 11
                OS_IRIX64 = 12
                OS_SUNOS = 13
                OS_SOLARIS = 14
                OS_SCO = 15
                OS_QNX = 16
                OS_TRU64 = 17
                OS_VMS = 18
                OS_OS2 = 19
                OS_UNIXWARE = 20
                OS_HPUX = 21
                OS_MACH = 22
                OS_AIX = 23
                OS_AUX = 24
                OS_AMIGAOS = 25
                OS_NETWARE = 26
                OS_MVS = 27
                OS_ULTRIX = 28
                OS_DGUX = 29
                OS_SINIX = 30
                OS_DYNIX = 31
                OS_OS390 = 32
                OS_RISCOS = 33
                OS_OS9 = 34
            End Enum
            Public CoreStatus As clsCoreStatus
            Public OsType As eOsType
            Public Expires As New DateTime
            Public CollectionServer As String = ""
            Public SmpCores As String = ""
            Public Flops As String = ""
            Public CpuMemory As String = ""
            Public GpuMemory As String = ""
            Public Due As DateTime = New DateTime
            Public PacketSizeLimit As String = ""
            Public UploadFailures As String = ""
        End Class
        Public Function ReadQueue() As Boolean
            Try
                'Reset total count
                _TotalUploadFailures = 0
                _WUsReady = 0
                If My.Computer.FileSystem.FileExists(_Queue) = False Then Return False
                Dim fStream As FileStream = New FileStream(_Queue, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    fBuff.Reverse()
                    _Version = BitConverter.ToInt32(fBuff, 0).ToString.Replace(",", ".")
                Catch ex As Exception : End Try
                'Current
                Try
                    If Not _Version.Contains(".") Then
                        _Version = Mid(_Version, 1, 1) & "." & Mid(_Version, 2)
                    End If
                Catch ex As Exception

                End Try
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                _Current = CInt(fBuff(0))
                ReDim Entries(0 To 9)
                For xInd As Int32 = 0 To 9
                    Entries(xInd) = New Entry
                    Entries(xInd).TimeData.Init()
                    Select Case xInd
                        Case 0
                            fStream.Position = 7 + 1
                        Case 1
                            fStream.Position = 719 + 1
                        Case 2
                            fStream.Position = 1431 + 1
                        Case 3
                            fStream.Position = 2143 + 1
                        Case 4
                            fStream.Position = 2855 + 1
                        Case 5
                            fStream.Position = 3567 + 1
                        Case 6
                            fStream.Position = 4279 + 1
                        Case 7
                            fStream.Position = 4991 + 1
                        Case 8
                            fStream.Position = 5703 + 1
                        Case 9
                            fStream.Position = 6415 + 1
                    End Select
                    With Entries(xInd)
                        'Status
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Status = CStr(CInt(fBuff(0)))
                        Catch ex As Exception
                        End Try
                        'padding / smpcores
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .SmpCores = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'TimeData
                        ReDim fBuff(0 To 31)
                        Try
                            fStream.Read(fBuff, 0, 32) 'First 16 bytes -> start date / last 16 ->end date ( using only first 4 bytes!)???
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Dim dStart As Long, dEnd As Long
                        dStart = BitConverter.ToInt32(fBuff, 0)
                        dEnd = BitConverter.ToInt32(fBuff, 16) ' or 15?
                        Try
                            .TimeData.BeginTime = #1/1/2000#.AddSeconds(dStart)
                            .TimeData.BeginTime = TimeZoneInfo.ConvertTimeFromUtc(.TimeData.BeginTime, TimeZoneInfo.Local)
                        Catch ex As Exception
                            .TimeData.BeginTime = #1/1/2000#
                        End Try
                        Try
                            .TimeData.EndTime = #1/1/2000#.AddSeconds(dEnd)
                            .TimeData.EndTime = TimeZoneInfo.ConvertTimeFromUtc(.TimeData.EndTime, TimeZoneInfo.Local)
                        Catch ex As Exception
                            .TimeData.EndTime = #1/1/2000#
                        Finally
                            If .TimeData.EndTime < .TimeData.BeginTime Then
                                .TimeData.EndTime = #1/1/2000#
                            ElseIf .TimeData.EndTime > DateTime.Now Then
                                .TimeData.EndTime = #1/1/2000#
                            End If
                        End Try
                        'padding
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'UploadStatus
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .UploadStatus = fBuff(0).ToString
                        Catch ex As Exception : End Try
                        'CoreUrl '128 bytes
                        ReDim fBuff(0 To 127)
                        Try
                            fStream.Read(fBuff, 0, 128)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            For xSecond As Int32 = 0 To 127
                                If fBuff(xSecond) = Byte.MinValue Then Exit For
                                .CoreURL &= ChrW(fBuff(xSecond))
                            Next
                        Catch ex As Exception : End Try
                        'padding
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Core number
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .CoreNumber = Hex(fBuff(0))
                        Catch ex As Exception : End Try
                        'padding
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Wu size
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .WUSize = BitConverter.ToInt32(fBuff, 0)
                        Catch ex As Exception : End Try
                        'padding   16 bytes
                        ReDim fBuff(0 To 15)
                        Try
                            fStream.Read(fBuff, 0, 16)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'project number 
                        ReDim fBuff(0 To 1)
                        Try
                            fStream.Read(fBuff, 0, 2)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Project.Project = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Run number 
                        ReDim fBuff(0 To 1)
                        Try
                            fStream.Read(fBuff, 0, 2)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Project.Run = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Clone number 
                        ReDim fBuff(0 To 1)
                        Try
                            fStream.Read(fBuff, 0, 2)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Project.Clone = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Gen number 
                        ReDim fBuff(0 To 1)
                        Try
                            fStream.Read(fBuff, 0, 2)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Project.Gen = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Issued
                        ReDim fBuff(0 To 7)
                        Try
                            fStream.Read(fBuff, 0, 8)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Dim dIssued As Long = BitConverter.ToInt32(fBuff, 0)
                        Try
                            .Issued = #1/1/1970#.AddSeconds(dIssued)
                            .Issued = TimeZoneInfo.ConvertTimeFromUtc(.Issued, TimeZoneInfo.Local)
                        Catch ex As Exception : End Try
                        'padding   36 bytes
                        ReDim fBuff(0 To 35)
                        Try
                            fStream.Read(fBuff, 0, 36)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'padding   4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .MachineID = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'ServerIP 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .ServerIP = fBuff(3) & "." & fBuff(2) & "." & fBuff(1) & "." & fBuff(0)
                        Catch ex As Exception : End Try
                        'ServerIP 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .ServerPort = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Wu type / message!
                        ReDim fBuff(0 To 63)
                        Try
                            fStream.Read(fBuff, 0, 64)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            For xSecond As Int32 = 0 To 63
                                If fBuff(xSecond) = Byte.MinValue Then Exit For
                                .WorkUnitType &= ChrW(fBuff(xSecond))
                            Next
                        Catch ex As Exception : End Try
                        'Username
                        ReDim fBuff(0 To 63)
                        Try
                            fStream.Read(fBuff, 0, 64)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            For xSecond As Int32 = 0 To 63
                                If fBuff(xSecond) = Byte.MinValue Then Exit For
                                .UserName &= ChrW(fBuff(xSecond))
                            Next
                        Catch ex As Exception : End Try
                        'Team number
                        ReDim fBuff(0 To 63)
                        Try
                            fStream.Read(fBuff, 0, 64)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            For xSecond As Int32 = 0 To 63
                                If fBuff(xSecond) = Byte.MinValue Then Exit For
                                .TeamNumber &= ChrW(fBuff(xSecond))
                            Next
                        Catch ex As Exception : End Try
                        'CpuID 'don't parse
                        ReDim fBuff(0 To 7)
                        Try
                            fStream.Read(fBuff, 0, 8)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Benchmark
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .BenchMark = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Benchmark 2
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        If .BenchMark = "0" Then
                            'no benchmark in previous field
                            Try
                                .BenchMark = CStr(BitConverter.ToInt32(fBuff, 0))
                            Catch ex As Exception : End Try
                        End If
                        'CpuType
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .CpuType = CType(BitConverter.ToInt32(fBuff, 0), Entry.eCpuType)
                        Catch ex As Exception : End Try
                        'OsType
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .OsType = CType(BitConverter.ToInt32(fBuff, 0), Entry.eOsType)
                        Catch ex As Exception : End Try
                        'Cpu species
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Os species
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Expires
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            If .TimeData.BeginTime <> #1/1/2000# Then
                                .Expires = .TimeData.BeginTime.AddSeconds(BitConverter.ToInt32(fBuff, 0))
                            Else
                                .Expires = #1/1/2000#
                            End If
                        Catch ex As Exception : End Try
                        'Padding
                        ReDim fBuff(0 To 19)
                        Try
                            fStream.Read(fBuff, 0, 20)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Collection server IP
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .CollectionServer = fBuff(3) & "." & fBuff(2) & "." & fBuff(1) & "." & fBuff(0)
                        Catch ex As Exception : End Try
                        'Download started 4 bytes
                        'z528 16 bytes
                        'Padd those
                        ReDim fBuff(0 To 19)
                        Try
                            fStream.Read(fBuff, 0, 20)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'NoSMP cores 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            If .SmpCores = "" Or CStr(BitConverter.ToInt32(fBuff, 0)) <> "" Then
                                .SmpCores = CStr(BitConverter.ToInt32(fBuff, 0))
                            End If
                        Catch ex As Exception : End Try
                        'WuTag 16 bytes / Discard?
                        ReDim fBuff(0 To 15)
                        Try
                            fStream.Read(fBuff, 0, 16)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Padding 16 bytes
                        ReDim fBuff(0 To 15)
                        Try
                            fStream.Read(fBuff, 0, 16)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Passkey 32 bytes
                        ReDim fBuff(0 To 31)
                        Try
                            fStream.Read(fBuff, 0, 32)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            For xSecond As Int32 = 0 To 31
                                If fBuff(xSecond) = Byte.MinValue Then Exit For
                                .PassKey &= ChrW(fBuff(xSecond))
                            Next
                        Catch ex As Exception : End Try
                        'Flops per core 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            Array.Reverse(fBuff)
                            .Flops = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Memory 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .CpuMemory = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'gpu memory 4 bytes
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .GpuMemory = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'z624 64 bytes
                        ReDim fBuff(0 To 63)
                        Try
                            fStream.Read(fBuff, 0, 64)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        'Due
                        ReDim fBuff(0 To 7)
                        Try
                            fStream.Read(fBuff, 0, 8)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .Due = #1/1/2000#.AddSeconds(BitConverter.ToInt64(fBuff, 0))
                            .Due = .Due.ToLocalTime
                        Catch ex As Exception : End Try
                        fStream.Position += 8
                        'PacketSize limit
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Try
                            .PacketSizeLimit = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        'Failed uploads
                        ReDim fBuff(0 To 3)
                        Try
                            fStream.Read(fBuff, 0, 4)
                        Catch ex As Exception
                            GoTo NoGo
                        End Try
                        Dim fCheck(0 To 3) As Byte
                        Array.Copy(fBuff, fCheck, 4)
                        Try
                            .UploadFailures = CStr(BitConverter.ToInt32(fBuff, 0))
                        Catch ex As Exception : End Try
                        If (.Status = "2" And CInt(.UploadFailures) > 0) Then
                            _TotalUploadFailures += CInt(.UploadFailures)
                        End If
                        If .Status = "2" Then
                            _WUsReady += 1
                        End If
                    End With
                Next
                fStream.Position = 7128
                'Performance fraction
                ReDim fBuff(0 To 3)  'Padd four zero bytes?!
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    _PerformanceFraction = BitConverter.ToSingle(fBuff, 0).ToString
                Catch ex As Exception : End Try
                'Performance fraction units
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    _PerformanceUnits = CStr(BitConverter.ToInt32(fBuff, 0))
                Catch ex As Exception : End Try
                'Download rate
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    'Reverse fbuff
                    'Array.Reverse(fBuff)
                    _DownloadRate = CStr((BitConverter.ToUInt32(fBuff, 0) / 1000.0F))
                Catch ex As Exception : End Try
                'Download rate units
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    _DownloadRateUnits = CStr(BitConverter.ToInt32(fBuff, 0))
                Catch ex As Exception : End Try
                'Upload rate
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    'Reverse fbuff
                    'Array.Reverse(fBuff)
                    _UploadRate = CStr((BitConverter.ToUInt32(fBuff, 0) / 1000.0F))
                Catch ex As Exception : End Try
                'Upload rate units
                ReDim fBuff(0 To 3)
                Try
                    fStream.Read(fBuff, 0, 4)
                Catch ex As Exception
                    GoTo NoGo
                End Try
                Try
                    _UploadRateUnits = CStr(BitConverter.ToInt32(fBuff, 0))
                Catch ex As Exception : End Try
                'PFraction as of queue.dat > 5.00?
                fBuff = Nothing
                fStream.Close()
                fStream = Nothing
                _FullSlots = True
                Return True
            Catch ex As Exception
                WriteError("clsQueue, ReadQueue", Err)
                _Err.Description = Err.Description
                _Err.Number = CStr(Err.Number)
                _HasError = True
                _FullSlots = False
                Return False
            End Try
NoGo:
            WriteLog("clsQueue_ReadQueue_Condition NoGo triggerd, " & _Queue & " could not be succesfully read.", eSeverity.Critical)
            _Err.Description = Err.Description
            _Err.Number = CStr(Err.Number)
            _HasError = True
            _FullSlots = False
            Return False
        End Function
#End Region
#Region "Entry point"
        Sub New(ByVal Location As String)
            Try
                If Location.EndsWith("\") Then Location.TrimEnd(CChar("\"))
                _Location = Location
                If My.Computer.FileSystem.FileExists(Location & "\Queue.dat") Then
                    Location &= "\Queue.dat"
                    _Queue = Location
                ElseIf My.Computer.FileSystem.FileExists(Location & "\queue.dat") Then
                    Location &= "\queue.dat"
                    _Queue = Location
                Else
                    _HasError = True
                    _Err.Description = "no queue file"
                End If
            Catch ex As Exception
                WriteError("clsQueue, New()", Err)
                _HasError = True
                _Err.Description = Err.Description
                _Err.Number = CStr(Err.Number)
            End Try
        End Sub

#End Region
    End Class
End Namespace


