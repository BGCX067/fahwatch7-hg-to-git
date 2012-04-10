Imports System.Text.RegularExpressions
Imports System.Text
Friend Class Client
    Implements IDisposable
#Region "Last parsed log line"
    Private mLastLineIndex As Int32 = 0
    Private mLastLine As String = ""
    Private mLastLineDT As DateTime = #1/1/2000#
    Friend Property LastLineIndex As Int32
        Get
            Return mLastLineIndex
        End Get
        Set(value As Int32)
            mLastLineIndex = value
        End Set
    End Property
    Friend Property LastLine As String
        Get
            Return mLastLine
        End Get
        Set(value As String)
            mLastLine = value
        End Set
    End Property
    Friend Property LastLineDT As DateTime
        Get
            Return mLastLineDT
        End Get
        Set(value As DateTime)
            mLastLineDT = value
        End Set
    End Property
#End Region
    Friend Property ClientName As String = "local"
    Friend Property ClientLocation As String = ""
    Friend Property ClientConfig As clsClientConfig
    Friend Property ClientInfo As clsClientInfo
    Friend Property FCPort As String = ""
    Friend Property PWD As String = ""
    Friend Property FWPort As String = "" 'FAHWatch7 port
    Friend Property Enabled As Boolean = True
    Friend Property Reachable As Boolean = True
    Friend Property Running As Boolean = False
#Region "ActiveWU"
    Private Property mActiveWU As New List(Of clsWU)
    Friend ReadOnly Property ActiveWorkUnits As Int32
        Get
            Return ActiveWU.Count
        End Get
    End Property
    Friend ReadOnly Property ActiveWU As List(Of clsWU)
        Get
            Dim rVal As New List(Of clsWU)
            rVal.AddRange(mActiveWU)
            For Each Slot As clsSlot In Slots
                If Slot.WorkUnit IsNot Nothing Then
                    If Not rVal.Contains(Slot.WorkUnit) Then rVal.Add(Slot.WorkUnit)
                End If
            Next
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property IsUnitActive(WU As clsWU) As Boolean
        Get
            Return ActiveWU.Contains(WU)
        End Get
    End Property
    Friend Sub ClearActiveWU(Optional ClearSlotWUs As Boolean = False)
        mActiveWU.Clear()
        If ClearSlotWUs Then
            For Each Slot As clsSlot In Slots
                Slot.WorkUnit = Nothing
            Next
        End If
    End Sub
    Friend Sub AddActiveWU(Workunit As clsWU)
        If IsUnitActive(Workunit) Then
            WriteLog("Attempt to add an active workunit which is already present")
            Exit Sub
        End If
        If HasSlot(Workunit.Slot) Then
            Slot(Workunit.Slot).WorkUnit = Workunit
        Else
            mActiveWU.Add(Workunit)
        End If
    End Sub
    Friend Sub RemoveActiveWorkunit(WorkUnit As clsWU)
        If Not ActiveWU.Contains(WorkUnit) Then Exit Sub
        For Each Slot As clsSlot In Slots
            If ReferenceEquals(WorkUnit, Slot.WorkUnit) Then
                Slot.WorkUnit = Nothing
                Exit Sub
            End If
        Next
        If mActiveWU.Contains(WorkUnit) Then
            mActiveWU.Remove(WorkUnit)
        End If
    End Sub
#End Region
#Region "Queued work units"
    Private mQueue As New List(Of clsWU)
    Friend ReadOnly Property Queued As Int32
        Get
            Dim iRet As Int32 = mQueue.Count
            For Each Slot As clsSlot In Slots
                iRet += Slot.Queue.Count
            Next
            Return iRet
        End Get
    End Property
    Friend ReadOnly Property Queue As List(Of clsWU)
        Get
            Try
                Dim rVal As New List(Of clsWU)
                rVal.AddRange(mQueue)
                For Each Slot As clsSlot In dSlots.Values.ToList
                    rVal.AddRange(Slot.Queue)
                Next
                Return rVal
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New List(Of clsWU)
            End Try
        End Get
    End Property
    Friend Sub ClearQueue()
        mQueue.Clear()
    End Sub
    Friend ReadOnly Property IsUnitQueued(WU As clsWU) As Boolean
        Get
            Return mQueue.Contains(WU)
        End Get
    End Property
    Friend Function RemoveFromQueue(WU As clsWU) As Boolean
        Try
            If Not IsUnitQueued(WU) Then
                Return True
            Else
                mQueue.Remove(WU)
                Return True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Overloads Function AddToQueue(WorkUnits As List(Of clsWU)) As Boolean
        Try
            For Each WU As clsWU In WorkUnits
                If Not AddToQueue(WU) Then Return False
            Next
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Overloads Function AddToQueue(WorkUnit As clsWU) As Boolean
        Try
            If Not mQueue.Contains(WorkUnit) Then mQueue.Add(WorkUnit)
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
    Friend ReadOnly Property AllRunning As Boolean
        Get
            Dim rVal As Boolean = True
            For Each Slot As clsSlot In Slots
                If Slot.Status <> "RUNNING" And Slot.Status <> "READY" Then
                    rVal = False
                    Exit For
                End If
            Next
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property AllStopped As Boolean
        Get
            Dim rVal As Boolean = True
            For Each Slot As clsSlot In Slots
                If Slot.Status = "RUNNING" Then
                    rVal = False
                    Exit For
                End If
            Next
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property CombinedLogUTC(Optional WarningsAndErrors As Boolean = False, Optional IncludeOtherWUs As Boolean = True) As String
        Get
            Try
                Dim sLog As New SortedDictionary(Of DateTime, List(Of String))
                For Each Slot As clsSlot In Slots
                    Dim dSlot As Dictionary(Of String, DateTime) = Slot.WorkUnit.dictLog
                    For Each Entry As KeyValuePair(Of String, DateTime) In dSlot
                        If Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                            If WarningsAndErrors Then
                                If (Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(Entry.Key, "[W][U][" & Slot.Index.Substring(0, 1) & "][" & Slot.Index.Substring(1, 1) & "]") Then
                                    If IncludeOtherWUs Then
                                        'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        Dim dtNow As DateTime = Entry.Value
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                Else
                                    'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    Dim dtNow As DateTime = Entry.Value
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                End If
                            End If
                        Else
                            If Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]") Then
                                If Regex.IsMatch(Entry.Key, "[W][U][" & Slot.Index.Substring(0, 1) & "][" & Slot.Index.Substring(1, 1) & "]") Then
                                    'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    Dim dtNow As DateTime = Entry.Value
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                Else
                                    If IncludeOtherWUs Then
                                        'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        Dim dtNow As DateTime = Entry.Value
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                End If
                            Else
                                'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                Dim dtNow As DateTime = Entry.Value
                                If sLog.ContainsKey(dtNow) Then
                                    If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                Else
                                    Dim nList As New List(Of String)
                                    nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    sLog.Add(dtNow, nList)
                                End If
                            End If
                        End If
                    Next
                Next
                If IncludeOtherWUs AndAlso Queued > 0 Then
                    'Include queue 
                    For Each WU As clsWU In Queue
                        Dim dSlot As Dictionary(Of String, DateTime) = WU.dictLog
                        For Each Entry As KeyValuePair(Of String, DateTime) In dSlot
                            If Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                                If WarningsAndErrors Then
                                    If (Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(Entry.Key, "[W][U][" & WU.Slot.Substring(0, 1) & "][" & WU.Slot.Substring(1, 1) & "]") Then
                                        If IncludeOtherWUs Then
                                            'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                            Dim dtNow As DateTime = Entry.Value
                                            If sLog.ContainsKey(dtNow) Then
                                                If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            Else
                                                Dim nList As New List(Of String)
                                                nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                                sLog.Add(dtNow, nList)
                                            End If
                                        End If
                                    Else
                                        'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        Dim dtNow As DateTime = Entry.Value
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                End If
                            Else
                                If Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]") Then
                                    If Regex.IsMatch(Entry.Key, "[W][U][" & WU.Slot.Substring(0, 1) & "][" & WU.Slot.Substring(1, 1) & "]") Then
                                        'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        Dim dtNow As DateTime = Entry.Value
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    Else
                                        If IncludeOtherWUs Then
                                            'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                            Dim dtNow As DateTime = Entry.Value
                                            If sLog.ContainsKey(dtNow) Then
                                                If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            Else
                                                Dim nList As New List(Of String)
                                                nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                                sLog.Add(dtNow, nList)
                                            End If
                                        End If
                                    End If
                                Else
                                    'Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    Dim dtNow As DateTime = Entry.Value
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If

                Dim strB As New StringBuilder
                For Each Entry As KeyValuePair(Of DateTime, List(Of String)) In sLog
                    For Each line As String In Entry.Value
                        strB.AppendLine(line)
                    Next
                Next
                Return strB.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return String.Empty
            Finally
                GC.Collect(GC.MaxGeneration)
            End Try
        End Get
    End Property
    Friend ReadOnly Property CombinedLog(Optional WarningsAndErrors As Boolean = False, Optional IncludeOtherWUs As Boolean = False) As String
        Get
            Try
                Dim sLog As New SortedDictionary(Of DateTime, List(Of String))
                For Each Slot As clsSlot In Slots
                    Dim dSlot As Dictionary(Of String, DateTime) = Slot.WorkUnit.dictLog
                    For Each Entry As KeyValuePair(Of String, DateTime) In dSlot
                        If Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                            If WarningsAndErrors Then
                                If (Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(Entry.Key, "[W][U][" & Slot.Index.Substring(0, 1) & "][" & Slot.Index.Substring(1, 1) & "]") Then
                                    If IncludeOtherWUs Then
                                        Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                Else
                                    Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                End If
                            End If
                        Else
                            If Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]") Then
                                If Regex.IsMatch(Entry.Key, "[W][U][" & Slot.Index.Substring(0, 1) & "][" & Slot.Index.Substring(1, 1) & "]") Then
                                    Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                Else
                                    If IncludeOtherWUs Then
                                        Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                End If
                            Else
                                Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                If sLog.ContainsKey(dtNow) Then
                                    If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                Else
                                    Dim nList As New List(Of String)
                                    nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    sLog.Add(dtNow, nList)
                                End If
                            End If
                        End If
                    Next
                Next
                If IncludeOtherWUs AndAlso Queued > 0 Then
                    'Include queue 
                    For Each WU As clsWU In Queue
                        If WU.dictLog.Count = 0 Then
                            WriteLog(WorkUnitLogHeader(WU) & "Converting core snippet to dictionary: " & WU.ConvertSnippetToActiveLog().ToString)
                        End If
                        Dim dSlot As Dictionary(Of String, DateTime) = WU.dictLog
                        For Each Entry As KeyValuePair(Of String, DateTime) In dSlot
                            If Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(Entry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                                If WarningsAndErrors Then
                                    If (Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(Entry.Key, "[W][U][" & WU.Slot.Substring(0, 1) & "][" & WU.Slot.Substring(1, 1) & "]") Then
                                        If IncludeOtherWUs Then
                                            Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                            If sLog.ContainsKey(dtNow) Then
                                                If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            Else
                                                Dim nList As New List(Of String)
                                                nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                                sLog.Add(dtNow, nList)
                                            End If
                                        End If
                                    Else
                                        Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    End If
                                End If
                            Else
                                If Regex.IsMatch(Entry.Key, "[W][U][0-9][0-9]") Then
                                    If Regex.IsMatch(Entry.Key, "[W][U][" & WU.Slot.Substring(0, 1) & "][" & WU.Slot.Substring(1, 1) & "]") Then
                                        Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                        If sLog.ContainsKey(dtNow) Then
                                            If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        Else
                                            Dim nList As New List(Of String)
                                            nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            sLog.Add(dtNow, nList)
                                        End If
                                    Else
                                        If IncludeOtherWUs Then
                                            Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                            If sLog.ContainsKey(dtNow) Then
                                                If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                            Else
                                                Dim nList As New List(Of String)
                                                nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                                sLog.Add(dtNow, nList)
                                            End If
                                        End If
                                    End If
                                Else
                                    Dim dtNow As DateTime = TimeZoneInfo.ConvertTimeFromUtc(Entry.Value, TimeZoneInfo.Local)
                                    If sLog.ContainsKey(dtNow) Then
                                        If Not sLog(dtNow).Contains(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8)) Then sLog(dtNow).Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                    Else
                                        Dim nList As New List(Of String)
                                        nList.Add(FormatTimeSpan(dtNow.TimeOfDay) & Entry.Key.Substring(8))
                                        sLog.Add(dtNow, nList)
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If

                Dim strB As New StringBuilder
                For Each Entry As KeyValuePair(Of DateTime, List(Of String)) In sLog
                    For Each line As String In Entry.Value
                        strB.AppendLine(line)
                    Next
                Next
                Return strB.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return String.Empty
            Finally
                GC.Collect(GC.MaxGeneration)
            End Try
        End Get
    End Property
#Region "Slots"
    Private dSlots As New Dictionary(Of String, clsSlot)
    Friend Class clsSlot
        Private m_Index As String = ""
        Private m_Type As String = "" 'gpu/smp/uniprocessor
        Private m_Hardware As String = ""
        Private m_GpuIndex As Int32 = -1
        Private mStatus As String = ""
        Private m_ucHW As HWInfo.ucHardware
        Private m_WU As clsWU
        Private m_Queue As New List(Of clsWU)
        Friend Property Status As String
            Set(value As String)
                If value = "UNPAUSED" Then
                    mStatus = "RUNNING"
                Else
                    mStatus = value
                End If
            End Set
            Get
                Return mStatus
            End Get
        End Property
        Friend Property WorkUnit As clsWU
            Get
                Return m_WU
            End Get
            Set(value As clsWU)
                m_WU = value
            End Set
        End Property
        Friend ReadOnly Property Queue As List(Of clsWU)
            Get
                Return m_Queue
            End Get
        End Property
        Friend Sub ClearQueue()
            m_Queue.Clear()
        End Sub
        Friend Function AddToQueue(WorkUnit As clsWU) As Boolean
            Try
                If Not m_Queue.Contains(WorkUnit) Then
                    m_Queue.Add(WorkUnit)
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Friend ReadOnly Property IsWUQueued(WU As clsWU) As Boolean
            Get
                Return m_Queue.Contains(WU)
            End Get
        End Property
        Friend Sub RemoveFromQueue(WU As clsWU)
            If m_Queue.Contains(WU) Then m_Queue.Remove(WU)
        End Sub
        Friend Property ucHW As HWInfo.ucHardware
            Get
                Return m_ucHW
            End Get
            Set(value As HWInfo.ucHardware)
                m_ucHW = value
            End Set
        End Property
        Friend Property Index As String
            Get
                Return m_Index
            End Get
            Set(value As String)
                m_Index = value
            End Set
        End Property
        Friend Property Type As String
            Get
                Return m_Type
            End Get
            Set(value As String)
                m_Type = value
            End Set
        End Property
        Friend Property Hardware As String
            Get
                Return m_Hardware
            End Get
            Set(value As String)
                m_Hardware = value
            End Set
        End Property
        Friend Property GpuIndex As String
            Get
                Return CStr(m_GpuIndex)
            End Get
            Set(value As String)
                m_GpuIndex = CInt(value)
            End Set
        End Property
    End Class
    Friend ReadOnly Property Slots As List(Of clsSlot)
        Get
            Return dSlots.Values.ToList
        End Get
    End Property
    Friend ReadOnly Property HasSlot(FahSlot As String) As Boolean
        Get
            Return dSlots.ContainsKey(FahSlot)
        End Get
    End Property
    Friend ReadOnly Property Slot(FahSlot As String) As clsSlot
        Get
            Return dSlots(FahSlot)
        End Get
    End Property
    Friend Sub New(mClientName As String, mClientLocation As String)
        ClientName = mClientName
        ClientLocation = mClientLocation
    End Sub
    Friend Sub ClearSlots()
        dSlots.Clear()
    End Sub
    Friend Sub AddSlot(Slot As clsSlot)
        If Not dSlots.ContainsKey(Slot.Index) Then
            dSlots.Add(Slot.Index, Slot)
        Else
            WriteLog("clsClient:Attempt to add a slot which is already present", eSeverity.Critical)
        End If
    End Sub
    'Friend Function ResetSlots() As Boolean
    '    Dim rVal As Boolean = True
    '    Try
    '        Dim dBackup As Dictionary(Of String, clsSlot) = dSlots
    '        dSlots.Clear()
    '        For Index As Int32 = 0 To ClientConfig.Configuration.slots.Count - 1
    '            Dim nSlot As New clsSlot, cSlot As clsClientConfig.clsConfiguration.sSlot = ClientConfig.Configuration.slots(Index)
    '            Dim m_Index As String = "", m_Type As String = "", m_Hardware As String = "", m_GpuIndex As Int32 = -1
    '            If CBool(CInt(cSlot.id) < 10) Then
    '                m_Index = "0" & cSlot.id
    '            Else
    '                m_Index = cSlot.id
    '            End If
    '            If cSlot.type = "GPU" Then
    '                m_Type = "GPU"
    '                If ClientConfig.Configuration.Slot(cSlot.id).HasKey("gpu-index") Then
    '                    m_GpuIndex = CInt(ClientConfig.Configuration.Slot(cSlot.id).GetValue("gpu-index"))
    '                    m_Hardware = ClientInfo.Info.GPU(m_GpuIndex).DeviceName
    '                Else
    '                    'gpu index = 0
    '                    m_Hardware = ClientInfo.Info.GPU(0).DeviceName
    '                End If
    '            Else
    '                m_Hardware = ClientInfo.Info.CPU
    '                m_Type = cSlot.type
    '            End If
    '            If nSlot.Init(m_Index, m_Type, m_Hardware, m_GpuIndex) Then
    '                dSlots.Add(m_Index, nSlot)
    '                WriteDebug("Initialized slot " & m_Index & " for " & ClientName)
    '            Else
    '                WriteLog("Failed to initialize slot " & m_Index & " for " & ClientName, eSeverity.Critical)
    '                rVal = False
    '                Exit For
    '            End If
    '        Next
    '        If Not IsNothing(dBackup) Then
    '            For Each DictionaryEntry In dBackup
    '                If HasSlot(DictionaryEntry.Key) Then
    '                    SetStatus(DictionaryEntry.Key, DictionaryEntry.Value.Status)
    '                End If
    '            Next
    '        End If
    '        dBackup = Nothing
    '    Catch ex As Exception
    '        WriteError(ex.Message, Err)
    '        rVal = False
    '    End Try
    '    Return rVal
    'End Function
    Friend Sub SetStatus(Slot As String, Status As String)
        Try
            If Not dSlots.ContainsKey(Slot) Then
                WriteLog("Trying to set a slot status but slot isn't known, slot: " & Slot)
                Return
            End If
            dSlots(Slot).Status = Status
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    '#Region "HFM.Client"
    '    Friend ReadOnly Property Connected As Boolean
    '        Get
    '            Return mHFMClient.Connected
    '        End Get
    '    End Property
    '    Private mLastUpdate As DateTime = #1/1/2000#
    '    Friend ReadOnly Property LastUpdate As DateTime
    '        Get
    '            If mHFMClient.Connected Then
    '                Return mHFMClient.GetJsonMessage("slots").Received
    '            Else
    '                Return mLastUpdate
    '            End If
    '        End Get
    '    End Property
    '    Private WithEvents mHFMClient As New clsFAHClientWrapper
    '    Friend Sub Connect()
    '        mHFMClient.Connect(ClientName, CInt(ClientConfig.Configuration.RemoteCommandServer.port), PWD)

    '    End Sub
    '#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Friend Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
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