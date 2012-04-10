'/*
' * FAHWatch7  Copyright Marvin Westmaas ( mtm )
' *
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
'/*	
'Imports m_hwinfo.clsHWInfo
'Imports gpuInfo.gpuInfo
Imports System.Text
Imports System.IO
Imports System
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting.Messaging
Imports System.Text.RegularExpressions
Imports System.Globalization

Friend Class clsParser
#Region "Async parser class and delegate"
    Friend Class AsyncParser
        Friend Function ParseLogs(lStartObj As Object) As LogStartObject
            Thread.CurrentThread.Priority = ThreadPriority.Highest
            Dim rVal As Boolean = False, dtFunctionStart As DateTime = DateTime.Now
            Dim lStart As LogStartObject = DirectCast(lStartObj, LogStartObject)
            Try
                Dim comLog As New List(Of String)
                Try
                    Dim nFiles As List(Of String) = lStart.Files
                    Dim iFile As Int32 = 1 'Index of log file 
                    Dim iLog As Int32 = 0 'Index of current line in log
                    Dim iSkipLog As Int32 = -1 'Next line with unit request
                    WriteLog("Logparser:Parsing client " & lStart.ClientName)
                    If lStart.ShowUI Then
                        If Not delegateFactory.BussyBox.IsFormVisible Then
                            delegateFactory.BussyBox.ShowForm("Parsing logs for " & lStart.ClientName, True)
                        Else
                            delegateFactory.BussyBox.SetMessage("Parsing logs for " & lStart.ClientName)
                        End If
                    Else
                        delegateFactory.SetMessage("Parsing logs for " & lStart.ClientName)
                    End If
                    'combine logs
                    Dim Lines As New List(Of String)
                    Try
                        For Each FileName In nFiles
                            WriteLog("Logparser:" & lStart.ClientName & ":Opening " & FileName)
                            Dim lText As String = ""
                            Dim fStream As FileStream = Nothing
                            Try
                                fStream = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                                Using fRead As StreamReader = New StreamReader(fStream)
                                    fStream = Nothing
                                    lText = fRead.ReadToEnd
                                End Using
                            Catch ex As Exception
                                lStart.Failed = True
                                lStart.Exception = ex
                                WriteError(ex.Message, Err)
                                GoTo Skip
                            Finally
                                If Not IsNothing(fStream) Then fStream.Dispose()
                            End Try
                            If lText.IndexOf(ChrW(10)) = lText.IndexOf(GetChar(Environment.NewLine, 1)) + 1 Then
                                WriteLog("Stripping log of chrw(10)", eSeverity.Debug)
                                lText = lText.Replace(ChrW(10), "")
                            End If
                            Dim nLog As New clsLogFile(lStart.ClientName)
                            nLog.File = FileName
                            nLog.Log = lText.Split(GetChar(Environment.NewLine, 1))(0)
                            nLog.LineCount = CStr(lText.Split(GetChar(Environment.NewLine, 1)).Count)
                            lStart.dLogFiles.Add(nLog.Log, nLog)
                            Lines.AddRange(lText.Split(GetChar(Environment.NewLine, 1)))
                            Dim logDate As DateTime = DateTime.Parse(lText.Split(GetChar(Environment.NewLine, 1))(0).Replace("*", "").Replace("Log Started", "").Trim.Replace("-", " "), CultureInfo.InvariantCulture)
                            WriteLog("Logparser:Setting log started to " & logDate.ToString("s"))
                            WriteDebug("-Linecount = " & Lines.Count.ToString)
                            If lStart.LastLineIndex = 0 OrElse Lines(lStart.LastLineIndex) <> lStart.LastLine Then 'orelse check!
                                'Set client info/config
                                If lText.Contains("********************** Folding@home Client *************************") And lText.Contains("********************************************************************") Then
                                    Dim strInfo As String = lText.Substring(lText.IndexOf("********************** Folding@home Client *************************"), lText.LastIndexOf("********************************************************************") - lText.IndexOf("********************** Folding@home Client *************************"))
                                    Dim sb As New StringBuilder
                                    For Each Line As String In strInfo.Split(GetChar(Environment.NewLine, 1))
                                        Try
                                            If Line.Contains(":") Then
                                                sb.AppendLine(Line.Substring(9).Trim)
                                            Else
                                                sb.AppendLine(Line)
                                            End If
                                        Catch ex As Exception
                                            WriteError("line: " & Line, Err)
                                        End Try
                                    Next
                                    strInfo = sb.ToString
                                    lStart.ClientInfo = New clsClientInfo
                                    lStart.ClientInfo.Info = lStart.ClientInfo.Info.ParseString(strInfo)
                                    If Not lStart.dClientInfo.ContainsKey(logDate) Then lStart.dClientInfo.Add(logDate, lStart.ClientInfo)
                                    WriteLog("Added clientinfo for " & lStart.ClientName & " - " & nLog.logDate.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat))
                                End If
                                'Finds original <config> at client startup
                                If lText.ToUpperInvariant.Contains("<CONFIG>") AndAlso lText.ToUpperInvariant.Contains("</CONFIG>") Then
                                    Dim indexStart As Int32 = lText.ToUpperInvariant.IndexOf("<CONFIG>")
                                    Dim indexEnd As Int32 = lText.ToUpperInvariant.IndexOf("</CONFIG>") + 9 'len </config>
                                    Dim iLen As Int32 = indexEnd - indexStart
                                    Dim strConfig As String = lText.Substring(indexStart, iLen)
                                    Dim sb As New StringBuilder
                                    For Each Line As String In strConfig.Split(GetChar(Environment.NewLine, 1))
                                        Try
                                            While Not Line.IndexOf(":") = -1
                                                Line = Line.Substring(Line.IndexOf(":") + 1)
                                            End While
                                            sb.AppendLine(Line)
                                        Catch ex As Exception
                                            lStart.Failed = True
                                            lStart.Exception = ex
                                            WriteError(ex.Message, Err)
                                            GoTo Skip
                                        End Try
                                    Next
                                    strConfig = sb.ToString
                                    lStart.ClientConfig = New clsClientConfig
                                    If Not lStart.dClientConfig.ContainsKey(strConfig & "##" & logDate.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat)) Then
                                        If lStart.ClientConfig.Configuration.ReadString(strConfig) Then
                                            lStart.ClientConfig.Configuration.ConfigurationDT = logDate
                                            lStart.dClientConfig.Add(strConfig & "##" & logDate.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat), lStart.ClientConfig)
                                            WriteLog("Added configuration for " & lStart.ClientName & " - " & nLog.logDate.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat))
                                            If nLog.File.Contains("log.txt") Then 'if Not lText.IndexOf("<config>", indexEnd, StringComparison.InvariantCultureIgnoreCase) > -1 Then
                                                'No more log files, init slots 
                                                With Clients.Client(lStart.ClientName)
                                                    .ClientInfo = lStart.dClientInfo.Values(lStart.dClientInfo.Values.Count - 1)
                                                    .ClientConfig = lStart.dClientConfig.Values(lStart.dClientConfig.Values.Count - 1)
                                                    '.ResetSlots()
                                                End With
                                            End If
                                        Else
                                            WriteLog("Failed to process the client configuration options", eSeverity.Critical)
                                            lStart.Failed = True
                                            lStart.Exception = New Exception("Logparser:Failed to process a config.xml")
                                            GoTo Skip
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try

                    Dim iIndex As Int32 = 0, dtStart As DateTime = #1/1/2000#
                    If lStart.LastLineIndex > 0 AndAlso Lines(lStart.LastLineIndex) = lStart.LastLine Then
                        WriteLog("LogParser:Starting log parse for " & lStart.ClientName & " from last parsed line")
                        iIndex = lStart.LastLineIndex
                        dtStart = lStart.LastLineDT
                    End If

                    Dim iAddDays As Int32, strLog As String = "", IsActiveLog As Boolean = False
                    Do
                        Dim Line As String = Lines(iIndex).ToUpperInvariant
#If CONFIG = "Debug" Then
                        'WriteDebug("-Parselogs-" & Line)
#End If
                        'Check for iAddDays
                        If iIndex - 1 > 0 And Lines(iIndex).Length > 2 AndAlso Lines(iIndex - 1).Length > 2 Then
                            Try
                                If Regex.IsMatch(Lines(iIndex - 1), "^\d{2}[:]\d{2}[:]\d{2}") AndAlso Regex.IsMatch(Lines(iIndex), "^\d{2}[:]\d{2}[:]\d{2}") Then
                                    If TimeSpan.Parse(Lines(iIndex - 1).Substring(0, 8)) > TimeSpan.Parse(Lines(iIndex).Substring(0, 8)) Then
                                        iAddDays += 1
                                        WriteLog("Logparser:Set iAddDays to " & iAddDays)
                                    End If
                                End If
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                lStart.Failed = True
                                lStart.Exception = ex
                                GoTo Skip
                            End Try
                        End If
                        If Line.Contains("******************************** DATE:") Then
                            '******************************** Date: 21/01/12 ********************************
                            Dim strFormat As String = "dd/MM/yy", provider As CultureInfo = CultureInfo.InvariantCulture
                            dtStart = DateTime.ParseExact(Line.Substring(Line.IndexOf("DATE:") + 6, 8), strFormat, provider)
                            iAddDays = 0
                            WriteDebug("Logparser:Startdate set to " & dtStart.ToString("s"))
                        ElseIf Line.Contains("LOG STARTED") Then
                            Try
                                'Set new starting datetime, reset iAddDays
                                strLog = Lines(iIndex)
                                dtStart = DateTime.Parse(Line.Replace("*", "").Replace("LOG STARTED", "").Trim.Replace("-", " "), CultureInfo.InvariantCulture)
                                iAddDays = 0
                                WriteDebug("Logparser:Log started set to " & dtStart.ToString("s"))
                                IsActiveLog = lStart.IsLogActive(strLog)
                            Catch ex As Exception
                                lStart.Failed = True
                                lStart.Exception = ex
                                WriteError(ex.Message, Err)
                                GoTo Skip
                            End Try
                        ElseIf Line.Contains("<CONFIG>") Then 'Check for new <config> 
                            Dim iEnd As Int32 = -1
                            For xInt As Int32 = iIndex To Lines.Count - 1
                                If Lines(xInt).ToUpperInvariant.Contains("</CONFIG>") Then
                                    iEnd = xInt
                                    Exit For
                                End If
                            Next
                            If iEnd <> -1 Then
                                Dim dtNConfig As DateTime = dtStart.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(iEnd).Substring(0, 8)))
                                Dim strB As New StringBuilder
                                For xInt As Int32 = iIndex To iEnd
                                    strB.Append(Lines(xInt).Substring(9))
                                Next
                                If Not lStart.ClientConfig.Configuration.ReadString(strB.ToString) Then
                                    WriteLog("Log parser hit a snare parsing a <config> </config> section", eSeverity.Important)
                                    lStart.Failed = True
                                    lStart.Exception = New Exception("Logparser failed to parse a client config.xml")
                                    GoTo Skip
                                Else
                                    If IsActiveLog Then
                                        Clients.Client(lStart.ClientName).ClientConfig = lStart.ClientConfig
                                        Clients.Client(lStart.ClientName).ClientInfo = lStart.ClientInfo
                                    End If
                                    If Not sqdata.HasClientConfig(lStart.ClientName, strB.ToString, dtNConfig) Then
                                        If Not lStart.dClientConfig.ContainsKey(strB.ToString & "##" & dtNConfig.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat)) Then
                                            lStart.ClientConfig.Configuration.ConfigurationDT = dtNConfig
                                            'WriteDebug("Added configuration for " & lStart.ClientName & " - " & dtNConfig.ToString("s",cultureinfo.invariantculture.datetimeformat))
                                            lStart.dClientConfig.Add(strB.ToString & "##" & dtNConfig.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat), lStart.ClientConfig)
                                        End If
                                    End If
                                End If
                                iSkipLog = iEnd
                                GoTo NewLine
                            Else
                                WriteLog("Parsing loop missed an </config>!", eSeverity.Critical)
                                lStart.Failed = True
                                lStart.Exception = New Exception("Logparser failed to find a </config>")
                                GoTo Skip
                            End If
                        ElseIf Line.Contains("ENABLED FOLDING SLOT ") Then
                            '06:11:26:Enabled folding slot 01: READY smp:4
                            '06:11:26:Enabled folding slot 02: PAUSED gpu:1:"GT200b [GeForce GTX 275]"
                            '06:11:26:Enabled folding slot 00: READY gpu:0:"RV710 [Radeon HD 4350]"
                            '
                            'Enum lines with enabled folding slot
                            If Clients.Client(lStart.ClientName).Queued > 0 Then
                                WriteLog("Logparser:Resetting slots, saving " & CStr(Clients.Client(lStart.ClientName).Queued) & " queued work units")
                                sqdata.SaveQueuedWU(Clients.Client(lStart.ClientName).Queue)
                            End If
                            Dim lActiveWU As List(Of clsWU) = Clients.Client(lStart.ClientName).ActiveWU
                            If lActiveWU.Count > 0 Then WriteLog("Logparser:Resetting slots, safe keeping " & CStr(lActiveWU.Count) & " active work units")
                            Clients.Client(lStart.ClientName).ClearSlots()
                            Clients.Client(lStart.ClientName).ClearQueue()
                            Try
                                For zCheck As Int32 = iIndex To Lines.Count - 1
                                    If Lines(zCheck).ToLowerInvariant.Contains("enabled folding slot") Then
                                        Dim SlotID As String = Lines(zCheck).Substring(Lines(zCheck).ToUpperInvariant.IndexOf("SLOT ") + 5, 2)
                                        Dim Status As String = Lines(zCheck).Substring(Lines(zCheck).ToUpperInvariant.IndexOf("SLOT " & SlotID) + Len("SLOT " + SlotID) + 2).Substring(0, Lines(zCheck).ToUpperInvariant.Substring(Lines(zCheck).ToUpperInvariant.IndexOf("SLOT " & SlotID) + Len("SLOT " + SlotID) + 2).IndexOf(Chr(32)))
                                        'sStatus = sStatus.Substring(0, sStatus.IndexOf(Chr(32)))
                                        Dim Type As String = String.Empty
                                        If Lines(zCheck).IndexOf(":", Lines(zCheck).IndexOf(Status)) = -1 Then
                                            'classic?
                                            Type = Lines(zCheck).Substring(Lines(zCheck).IndexOf(Status) + Len(Status) + 1)
                                        Else
                                            Type = Lines(zCheck).Substring(Lines(zCheck).IndexOf(Status) + Len(Status) + 1).Substring(0, Lines(zCheck).Substring(Lines(zCheck).IndexOf(Status) + Len(Status) + 1).IndexOf(":"))
                                        End If
                                        Dim gpuIndex As String = String.Empty, gpuName As String = String.Empty
                                        If Type = "smp" Then
                                            'append number of cores
                                            Type = Lines(zCheck).Substring(Lines(zCheck).IndexOf(Type))
                                        ElseIf Type = "gpu" Then
                                            gpuIndex = Lines(zCheck).Substring(Lines(zCheck).IndexOf(Type) + Len(Type) + 1, 1)
                                            gpuName = Lines(zCheck).Substring(Lines(zCheck).IndexOf(Chr(34))).Trim(Chr(34))
                                        End If
                                        Dim nSlot As New Client.clsSlot
                                        nSlot.Index = SlotID
                                        nSlot.Status = Status
                                        nSlot.Type = Type
                                        If Type = "gpu" Then
                                            nSlot.Hardware = gpuName
                                            nSlot.GpuIndex = gpuIndex
                                        Else
                                            nSlot.Hardware = lStart.ClientInfo.Info.CPU
                                        End If
                                        Clients.Client(lStart.ClientName).AddSlot(nSlot)
                                    Else
                                        iSkipLog = zCheck
                                        Exit For
                                    End If
                                    For aInt As Int32 = 0 To lActiveWU.Count - 1
                                        Dim aWU As clsWU = lActiveWU(aInt)
                                        If Clients.Client(aWU.ClientName).HasSlot(aWU.Slot) Then
                                            Clients.Client(aWU.ClientName).Slot(aWU.Slot).WorkUnit = aWU
                                            WriteLog("Logparser:" & WorkUnitLogHeader(aWU) & "Set as active work unit")
                                        Else
                                            Clients.Client(aWU.ClientName).ActiveWU.Add(aWU)
                                            WriteLog("Logparser:" & WorkUnitLogHeader(aWU) & "Addded to clients active work unit list, slot " & aWU.Slot & " not found")
                                        End If
                                    Next
                                Next
                            Catch ex As Exception
                                lStart.Failed = True
                                lStart.Exception = ex
                                WriteError(ex.Message, Err)
                                GoTo Skip
                            End Try
                            Try
                                Dim lQueue As List(Of clsWU) = sqdata.QueuedWorkUnits(lStart.ClientName)
                                If lQueue.Count > 0 Then
                                    WriteLog("Logparser:Trying to restore " & CStr(lQueue.Count) & " queued work units")
                                End If
                                For qInt As Int32 = 0 To lQueue.Count - 1
                                    Dim qwu As clsWU = lQueue(qInt)
                                    If qwu.IsActive Then
                                        Dim bAddedToSlot As Boolean = False
                                        For Each Slot As Client.clsSlot In Clients.Client(lStart.ClientName).Slots
                                            If qwu.Slot = Slot.Index And qwu.HW = Slot.Hardware Then
                                                Slot.WorkUnit = qwu
                                                WriteLog("Logparser:" & WorkUnitLogHeader(qwu) & "Set as active work unit")
                                                bAddedToSlot = True
                                                Exit For
                                            End If
                                        Next
                                        If Not bAddedToSlot Then
                                            'Add to client queue
                                            Clients.Client(lStart.ClientName).AddToQueue(qwu)
                                            WriteLog("Logparser:" & WorkUnitLogHeader(qwu) & "Added to client's active work units, slot " & qwu.Slot & " not found")
                                        End If
                                    ElseIf qwu.IsQueue Then
                                        Dim bAddedToSlot As Boolean = False
                                        For Each Slot As Client.clsSlot In Clients.Client(lStart.ClientName).Slots
                                            If qwu.Slot = Slot.Index And qwu.HW = Slot.Hardware Then
                                                Slot.AddToQueue(qwu)
                                                WriteLog("Logparser:" & WorkUnitLogHeader(qwu) & "Restored queued work unit to slot")
                                                bAddedToSlot = True
                                                Exit For
                                            End If
                                        Next
                                        If Not bAddedToSlot Then
                                            'Add to client queue
                                            Clients.Client(lStart.ClientName).AddToQueue(qwu)
                                            WriteLog("Logparser:" & WorkUnitLogHeader(qwu) & "Added queued work unit to client, slot " & qwu.Slot & " not found")
                                        End If
                                    Else
                                        MsgBox("Break")
                                    End If
                                Next
                            Catch ex As Exception
                                lStart.Failed = True
                                lStart.Exception = ex
                                WriteError(ex.Message, Err)
                                GoTo Skip
                            End Try
                            GoTo Newline
                        ElseIf IsActiveLog AndAlso (Regex.IsMatch(Line, "[0-9][0-9]:[0-9][0-9]:[0-9][0-9]:FS[0-9][0-9]:PAUSED") Or Regex.IsMatch(Line, "[0-9][0-9]:[0-9][0-9]:[0-9][0-9]:FS[0-9][0-9]:UNPAUSED")) Then
                            Dim sSlot As String = Line.Substring(Line.IndexOf(":FS") + 3, 2)
                            If Clients.Client(lStart.ClientName).HasSlot(sSlot) Then
                                Clients.Client(lStart.ClientName).Slot(sSlot).Status = Line.Substring(Line.LastIndexOf(":") + 1)
                                WriteLog("Logparser:Set " & lStart.ClientName & " FS" & sSlot & " status to " & Line.Substring(Line.LastIndexOf(":") + 1))
                            Else
                                WriteLog("Logparser:Attempt to set a slot status for a slot which doesn't exist", eSeverity.Important)
                            End If
                        ElseIf IsActiveLog AndAlso (Regex.IsMatch(Line, "SLOT [0-9][0-9] PAUSED") Or Regex.IsMatch(Line, "SLOT [0-9][0-9] UNPAUSED")) Then
                            Dim sSlot As String = Line.Substring(Line.IndexOf("SLOT ") + 6, 2)
                            If Clients.Client(lStart.ClientName).HasSlot(sSlot) Then
                                Clients.Client(lStart.ClientName).Slot(sSlot).Status = Line.Substring(Line.LastIndexOf(":") + 1)
                                WriteLog("Logparser:Set " & lStart.ClientName & " FS" & sSlot & " status to " & Line.Substring(Line.IndexOf("SLOT ") + 6, 2))
                            Else
                                WriteLog("Logparser:Attempt to set a slot status for a slot which doesn't exist", eSeverity.Important)
                            End If
                        ElseIf IsActiveLog AndAlso Line.Contains(":CLEAN EXIT") Then
                            ':Clean exit
                            Clients.Client(lStart.ClientName).Running = False
                        ElseIf Line.Contains("RECEIVED UNIT") Then 'Received Unit
                            '05:17:23:WU01:FS02:Connecting to assign-GPU.stanford.edu:80
                            '05:17:24:WU01:FS02:News: Welcome to Folding@Home
                            '05:17:24:WU01:FS02:Assigned to work server 171.67.108.11
                            '05:17:24:WU01:FS02:Requesting new work unit for slot 02: RUNNING gpu:1:"GT200b [GeForce GTX 275]" from 171.67.108.11
                            '05:17:24:WU01:FS02:Connecting to 171.67.108.11:8080
                            '05:17:24:WU01:FS02:Downloading 44.77KiB
                            '05:17:25:WU01:FS02:Download complete
                            '05:17:25:WU01:FS02:Received Unit: id:01 state:DOWNLOAD error:OK project:5772 run:8 clone:199 gen:828 core:0x11 unit:0x0a00fed44f1a4a66033c00c70008168c
                            Dim nWU As New clsWU
                            Try
                                nWU.lineIndex = iIndex
                                nWU.ClientName = lStart.ClientName
                                nWU.Log = strLog
                                nWU.ID = Lines(iIndex).Substring(Line.IndexOf("ID:") + 3, Line.IndexOf("STATE:") - (Line.IndexOf("ID:") + 3)).Trim
                                nWU.unit = Lines(iIndex).Substring(Line.LastIndexOf("UNIT:") + 5).Trim.ToLower(CultureInfo.InvariantCulture)
                                nWU.PRCG = Lines(iIndex).Substring(Line.IndexOf("PROJECT:"), Line.IndexOf("CORE:") - (Line.IndexOf("PROJECT:"))).Trim
                                nWU.Core = Lines(iIndex).Substring(Line.IndexOf("CORE:") + 5, 4).Trim
                                If Line.Contains(":FS") Then nWU.Slot = Line.Substring(Line.IndexOf(":FS") + 3, 2)
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                lStart.Failed = True
                                lStart.Exception = ex
                                GoTo Skip
                            End Try
                            'fill wu info, scan lines upwards to match slot
                            Dim iRDays As Int32 = 0, bSucces As Boolean = False
                            Dim lTMP As New List(Of String)
                            For xInt As Int32 = iIndex To 0 Step -1
                                Dim xLine As String = Lines(xInt).ToUpperInvariant
                                lTMP.Add(xLine)
#If CONFIG = "Debug" Then
                                'WriteDebug(WorkUnitLogHeader(nWU) & "Parselogs_reverse:" & xLine)
#End If
                                If xInt > 0 AndAlso Lines(xInt).Length > 2 AndAlso Lines(xInt - 1).Length > 2 Then
                                    Try
                                        If Regex.IsMatch(Lines(xInt), "^\d{2}[:]\d{2}[:]\d{2}") AndAlso Regex.IsMatch(Lines(xInt - 1), "^\d{2}[:]\d{2}[:]\d{2}") Then
                                            If TimeSpan.Parse(Lines(xInt).Substring(0, 8)) < TimeSpan.Parse(Lines(xInt - 1).Substring(0, 8)) Then
                                                iRDays += 1
                                                WriteDebug(WorkUnitLogHeader(nWU) & "Parselogs_reverse:-Setting iRDays to " & iRDays)
                                            End If
                                        End If
                                        'If Int32.Parse(Lines(xInt).Replace(ChrW(34), "").Substring(0, 2), NumberStyles.Integer) > Int32.Parse(Lines(xInt - 1).Replace(ChrW(34), "").Substring(0, 2), NumberStyles.Integer) Then

                                        'End If
                                    Catch ex As Exception
                                        WriteError(ex.Message, Err)
                                        lStart.Failed = True
                                        lStart.Exception = ex
                                        GoTo Skip
                                    End Try
                                End If
                                If (xLine.Contains("SLOT ") And nWU.Slot = "") Or (xLine.Contains(strID(nWU)) And xLine.Contains("DOWNLOAD COMPLETE")) Or _
                                    (nWU.Slot = "" And xLine.Contains("DOWNLOAD COMPLETE")) Then
                                    If nWU.Slot = "" Then
                                        nWU.Slot = Lines(xInt).Substring(Lines(xInt).ToUpperInvariant.IndexOf("SLOT ") + Len("SLOT "), 2)
                                        WriteDebug("Logparser:Slot set to FS" & nWU.Slot)
                                    End If
                                    nWU.dtDownloaded = dtStart.Date.AddDays(iAddDays - iRDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                                    WriteDebug("Logparser:Downloaded: " & nWU.dtDownloaded.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat))
                                    'check db for existing unit
                                    If Not sqdata.HasWorkUnit(nWU) Then
                                        WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " found new work unit")
                                    End If
                                ElseIf (xLine.Contains("SLOT " & nWU.Slot) Or xLine.Contains(strID(nWU))) And xLine.Contains("DOWNLOADING") Then
                                    nWU.dtStartDownload = dtStart.Date.AddDays(iAddDays - iRDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                                    WriteDebug("Logparser:StartDownload: " & nWU.dtStartDownload.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat))
                                    nWU.DownloadSize = Lines(xInt).Substring(xLine.IndexOf("DOWNLOADING") + 11).Trim
                                    WriteDebug("Logparser:DownloadSize: " & nWU.DownloadSize)
                                ElseIf xLine.Contains("REQUESTING NEW WORK UNIT FOR SLOT " & nWU.Slot) Then
                                    If Lines(xInt).Contains(Chr(34)) Then
                                        nWU.HW = Lines(xInt).Substring(Lines(xInt).IndexOf(ChrW(34)) + 1, Lines(xInt).LastIndexOf(ChrW(34)) - Lines(xInt).IndexOf(ChrW(34)) - 1).Trim
                                        WriteDebug("Logparser:Hardware for work unit detected: " & nWU.HW)
                                    Else
                                        If xLine.Contains("READY") Then
                                            If IsActiveLog Then
                                                Clients.Client(lStart.ClientName).Slot(nWU.Slot).Status = "READY"
                                                WriteLog("Logparser:" & lStart.ClientName & " set slot " & nWU.Slot & " status to ready")
                                            End If
                                            nWU.HW = Lines(xInt).Substring(xLine.IndexOf("READY") + 6, xLine.IndexOf("FROM") - (xLine.IndexOf("READY") + 7)).Trim
                                            WriteDebug("Logparser:(2)Hardware for work unit detected: " & nWU.HW & Chr(32) & " - READY")
                                        ElseIf xLine.Contains("PAUSED") Then
                                            If IsActiveLog Then
                                                Clients.Client(lStart.ClientName).Slot(nWU.Slot).Status = "PAUSED"
                                                WriteLog("Logparser:" & lStart.ClientName & " set slot " & nWU.Slot & " status to paused")
                                            End If
                                            nWU.HW = Lines(xInt).Substring(xLine.IndexOf("PAUSED") + 7, xLine.ToUpperInvariant.IndexOf("FROM") - (xLine.IndexOf("PAUSED") + 7)).Trim
                                            WriteDebug("Logparser:(2)Hardware for work unit detected: " & nWU.HW & Chr(32) & " - PAUSED")
                                        ElseIf xLine.Contains("RUNNING") Then
                                            If IsActiveLog Then
                                                Clients.Client(lStart.ClientName).Slot(nWU.Slot).Status = "RUNNING"
                                                WriteLog("Logparser:" & lStart.ClientName & " set slot " & nWU.Slot & " status to running")
                                            End If
                                            nWU.HW = Lines(xInt).Substring(xLine.IndexOf("RUNNING") + 8, xLine.IndexOf("FROM") - (xLine.IndexOf("RUNNING") + 8)).Trim
                                            WriteDebug("Logparser:(2)Hardware for work unit detected: " & nWU.HW & Chr(32) & " - RUNNING")
                                        End If
                                    End If
                                    If xLine.Contains(" FROM ") Then
                                        nWU.WS = xLine.Substring(xLine.LastIndexOf("FROM") + 5).Trim
                                        WriteDebug("Logparser:(2)set work server to: " & nWU.WS)
                                    Else
                                        If Lines(xInt + 1).ToUpperInvariant.Contains("CONNECTING TO") Then
                                            nWU.WS = Lines(xInt + 1).Substring(Lines(xInt + 1).ToUpperInvariant.IndexOf("CONNECTING TO") + 14)
                                            If nWU.WS.Contains(":") Then nWU.WS = nWU.WS.Substring(0, nWU.WS.IndexOf(":"))
                                            WriteDebug("Logparser:(3)set work server to: " & nWU.WS)
                                        End If
                                    End If
                                End If
                                'Check if all fields are populated?
                                If nWU.WS <> "" And nWU.Slot <> "" And nWU.HW <> "" And nWU.DownloadSize <> "" Then
                                    bSucces = True
                                    Exit For
                                End If
                            Next
                            If Not bSucces Then
                                WriteLog("Logparser:Failed to find work unit details", eSeverity.Critical)
                                Dim iInt As Int32 = 1, iIntMax As Int32 = lTMP.Count
                                For Each logStr As String In lTMP
                                    Dim lStr As String = CStr(iInt)
                                    While lStr.Length < CStr(iIntMax).Length
                                        lStr = "0" & lStr
                                    End While
                                    WriteLog("Logparser:" & lStr & ":" & logStr)
                                Next
                                WriteLog(strID(nWU) & " - " & nWU.unit & " - " & nWU.dtDownloaded)
                                lTMP.Clear()
                                lTMP = Nothing
                                lStart.Failed = True
                                lStart.Exception = New Exception("Logparser:Failed to find work unit start details")
                                GoTo Skip
                            Else
                                nWU.SlotConfig = lStart.ClientConfig.Configuration.Slot(nWU.Slot)
                                WriteLog("Logparser:" & lStart.ClientName & ":Assigned slot config for slot " & nWU.Slot)
                            End If
                            lTMP.Clear()
                            lTMP = Nothing
                            If Not bSucces Then
                                lStart.Failed = True
                                lStart.Exception = Err.GetException
                                GoTo Skip
                            End If
                            If Not lStart.lQueued.Contains(nWU) AndAlso Not sqdata.HasWorkUnit(nWU) Then
                                WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " starting parse")
                                If ParseWorkUnit(nWU, Lines, iIndex, lStart) Then
                                    If Not nWU.IsActive Then
                                        lStart.lWU.Add(nWU)
                                        WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added to finished work units")
                                    Else
                                        lStart.dLogFiles(nWU.Log).AllDone = False
                                        If nWU.dtStarted = #1/1/2000# Then
                                            'Unit is queued for start
                                            If Clients.Client(lStart.ClientName).HasSlot(nWU.Slot) Then
                                                Clients.Client(lStart.ClientName).Slot(nWU.Slot).AddToQueue(nWU)
                                                WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added to queued work units ( prepared ) for slot")
                                            Else
                                                Clients.Client(lStart.ClientName).AddToQueue(nWU)
                                                WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added to queued work units ( prepared ) for client")
                                            End If
                                            sqdata.SaveQueuedWU(nWU)
                                        Else
                                            If Clients.Client(lStart.ClientName).HasSlot(nWU.Slot) Then
                                                Clients.Client(lStart.ClientName).Slot(nWU.Slot).WorkUnit = nWU
                                                WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " set as active work unit for slot")
                                            Else
                                                Clients.Client(lStart.ClientName).ActiveWU.Add(nWU)
                                                WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added as active work unit for client")
                                            End If
                                        End If
                                    End If
                                Else
                                    If Not nWU.IsActive And Not nWU.IsQueue Then
                                        WriteLog("Logparser failed, work unit has undetermined status", eSeverity.Important)
                                        lStart.Failed = True
                                        lStart.Exception = New Exception("Failed to determine work unit status")
                                        GoTo Skip
                                    ElseIf nWU.IsQueue Then
                                        lStart.dLogFiles(nWU.Log).AllDone = False
                                        If Clients.Client(nWU.ClientName).HasSlot(nWU.Slot) Then
                                            Clients.Client(lStart.ClientName).Slot(nWU.Slot).AddToQueue(nWU)
                                            WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added to queued work units ( finished ) for slot")
                                        Else
                                            Clients.Client(lStart.ClientName).AddToQueue(nWU)
                                            WriteLog("Logparser:" & WorkUnitLogHeader(nWU) & " added to queued work units ( finished ) for client")
                                        End If
                                        sqdata.SaveQueuedWU(nWU)
                                    Else
                                        MsgBox("break")
                                    End If
                                End If
                                If lStart.ShowUI Then
                                    delegateFactory.BussyBox.SetMessage("Parsing logs for " & lStart.ClientName & Environment.NewLine & Environment.NewLine & "Found a new work unit " & strID(nWU) & " - " & nWU.unit & Environment.NewLine & "Finished: " & lStart.lWU.Count.ToString & Environment.NewLine & "Active: " & CStr(Clients.Client(lStart.ClientName).ActiveWorkUnits) & Environment.NewLine & "Queued: " & Clients.Client(lStart.ClientName).Queued.ToString)
                                Else
                                    delegateFactory.SetMessage("Parsing logs for " & lStart.ClientName & " Found a new work unit " & strID(nWU) & " - " & nWU.unit & vbTab & "Finished: " & lStart.lWU.Count.ToString & vbTab & "Active: " & lStart.lActiveWU.Count.ToString & vbTab & "Queued: " & Clients.Client(lStart.ClientName).Queued)
                                End If
                            End If
                        ElseIf lStart.lQueued.Count > 0 Then
                            Try
                                Dim lWUsFinished As New List(Of clsWU)
                                For Each qWU As clsWU In lStart.lQueued
                                    If Line = qWU.Line.ToUpperInvariant AndAlso Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:]") AndAlso dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Line.Substring(0, 8))) = qWU.LineDT Then
                                        If qWU.CoreStatus <> "" Then
                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " looking for upload")
                                            Dim iEnd As Int32
                                            Try
                                                If Not GetIEnd(Lines, iIndex, qWU, iEnd, lStart) Or iEnd = -1 Then
                                                    WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & "Can not find cleanup event, keeping in queue")
                                                    qWU.CoreSnippet = qWU.ActiveLogfileUTC(True, True)
                                                    sqdata.SaveQueuedWU(qWU)
                                                    If qWU.utcStarted = #1/1/2000# Then
                                                        'not started, add to slot queue
                                                        If Not Clients.Client(qWU.ClientName).HasSlot(qWU.Slot) Then
                                                            Clients.Client(qWU.ClientName).AddToQueue(qWU)
                                                        Else
                                                            Clients.Client(qWU.ClientName).Slot(qWU.Slot).AddToQueue(qWU)
                                                        End If
                                                    End If
                                                Else
                                                    lWUsFinished.Add(qWU)
                                                    sqdata.RemoveWorkUnitFromQueue(qWU)
                                                    qWU.CoreSnippet = qWU.ActiveLogfileUTC(True, True)
                                                    'Check wether unit has been submitted or is now started 
                                                    If qWU.utcSubmitted = #1/1/2000# And Not qWU.utcStarted = #1/1/2000# Then
                                                        'Unit is now started, move to active wu
                                                        If Clients.Client(qWU.ClientName).HasSlot(qWU.Slot) Then
                                                            Clients.Client(qWU.ClientName).Slot(qWU.Slot).WorkUnit = qWU
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " set as active work unit for slot")
                                                        Else
                                                            Clients.Client(qWU.ClientName).ActiveWU.Add(qWU)
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " set as active work unit for slot")
                                                        End If
                                                    ElseIf qWU.utcSubmitted <> #1/1/2000# Then
                                                        'unit is now submitted
                                                        WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & "Found cleanup event, removing from queue")
                                                        lStart.lWU.Add(qWU)
                                                    Else
                                                        MsgBox("break")
                                                    End If
                                                End If
                                            Catch ex As Exception
                                                WriteError(ex.Message, Err)
                                            End Try
                                        Else
                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " checking work unit start")
                                            If ParseWorkUnit(qWU, Lines, iIndex, lStart) Then
                                                If Not qWU.IsActive Then
                                                    lStart.lWU.Add(qWU)
                                                    WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added to finished work units and removing from queue")
                                                    lWUsFinished.Add(qWU)
                                                Else
                                                    lStart.dLogFiles(qWU.Log).AllDone = False
                                                    If qWU.dtStarted = #1/1/2000# Then
                                                        'Unit is queued for start
                                                        If Clients.Client(lStart.ClientName).HasSlot(qWU.Slot) Then
                                                            Clients.Client(lStart.ClientName).Slot(qWU.Slot).AddToQueue(qWU)
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added to queued work units ( prepared ) for slot")
                                                        Else
                                                            Clients.Client(lStart.ClientName).AddToQueue(qWU)
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added to queued work units ( prepared ) for client")
                                                        End If
                                                        sqdata.SaveQueuedWU(qWU)
                                                    Else
                                                        If Clients.Client(lStart.ClientName).HasSlot(qWU.Slot) Then
                                                            Clients.Client(lStart.ClientName).Slot(qWU.Slot).WorkUnit = qWU
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " set as active work unit for slot")
                                                        Else
                                                            Clients.Client(lStart.ClientName).ActiveWU.Add(qWU)
                                                            WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added as active work unit for client")
                                                        End If
                                                    End If
                                                End If
                                            Else
                                                If Not qWU.IsActive And Not qWU.IsQueue Then
                                                    WriteLog("Logparser failed, work unit has undetermined status", eSeverity.Important)
                                                    lStart.Failed = True
                                                    lStart.Exception = New Exception("Failed to determine work unit status")
                                                    GoTo Skip
                                                ElseIf qWU.IsQueue Then
                                                    lStart.dLogFiles(qWU.Log).AllDone = False
                                                    If Clients.Client(qWU.ClientName).HasSlot(qWU.Slot) Then
                                                        Clients.Client(lStart.ClientName).Slot(qWU.Slot).AddToQueue(qWU)
                                                        WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added to queued work units ( finished ) for slot")
                                                    Else
                                                        Clients.Client(lStart.ClientName).AddToQueue(qWU)
                                                        WriteLog("Logparser:" & WorkUnitLogHeader(qWU) & " added to queued work units ( finished ) for client")
                                                    End If
                                                    sqdata.SaveQueuedWU(qWU)
                                                Else
                                                    MsgBox("break")
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                For Each WU As clsWU In lWUsFinished
                                    WriteLog("Logparser:" & WorkUnitLogHeader(WU) & " removed from queue list")
                                    lStart.lQueued.Remove(WU)
                                Next
                                'Check queued
                            Catch ex As Exception
                                lStart.Failed = True
                                lStart.Exception = ex
                                WriteError(ex.Message, Err)
                                GoTo Skip
                            End Try
                        End If
Newline:
                        If iSkipLog > -1 Then
                            iIndex = iSkipLog
                            iSkipLog = -1
                        Else
                            iIndex += 1
                            If iIndex = Lines.Count Then Exit Do
                        End If
                        lStart.LastLineIndex = iIndex - 1
                        lStart.LastLine = Lines(iIndex - 1)
                        lStart.LastLineDT = dtStart
                    Loop
                    WriteLog("Logparser:Finished after " & FormatTimeSpan(DateTime.Now.Subtract(dtFunctionStart), True))
                Catch ex As Exception
                    lStart.Failed = True
                    lStart.Exception = ex
                    WriteError(ex.Message, Err)
                    GoTo Skip
                End Try
                rVal = True
            Catch ex As Exception
                lStart.Failed = True
                lStart.Exception = ex
                WriteError(ex.Message, Err)
            End Try
Skip:
            Return lStart
        End Function
        Private Shared Function HasMatch(WorkUnit As clsWU, Line As String) As Boolean
            If Line.Contains(strID(WorkUnit)) OrElse Line.Contains("WU" & WorkUnit.ID) OrElse Line.Contains("UNIT" & WorkUnit.ID) OrElse Line.Contains("UNIT " & WorkUnit.ID) Then
                Return True
            ElseIf Line.Contains("PAUSE") AndAlso (Line.Contains("SLOT " & WorkUnit.Slot) OrElse Line.Contains("FS" & WorkUnit.Slot)) Then ' PAUSE caotures UNPAUSED
                Return True
            Else
                Return False
            End If
        End Function
        'Use haslogmatch to add lines to the activelog
        Private Shared Function HasLogMatch(Workunit As clsWU, Line As String) As Boolean
            If Line.Contains(strID(Workunit)) OrElse Line.Contains("WU" & Workunit.ID) OrElse Line.Contains("UNIT" & Workunit.ID) OrElse Line.Contains("UNIT " & Workunit.ID) Then
                Return True
            ElseIf Line.Contains("PAUSE") AndAlso (Line.Contains("SLOT " & Workunit.Slot) OrElse Line.Contains("FS" & Workunit.Slot)) Then ' PAUSE captures UNPAUSED
                Return True
            ElseIf Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:][W][A][R][N][I][N][G][:][W][U]\d{2}[F][S][" & Workunit.Slot.Substring(0, 1) & "][" & Workunit.Slot.Substring(1, 1) & "]") Then
                Return True
            ElseIf Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:][E][R][R][O][R][:][W][U]\d{2}[F][S][" & Workunit.Slot.Substring(0, 1) & "][" & Workunit.Slot.Substring(1, 1) & "]") Then
                Return True
            ElseIf Line.Contains("FS" & Workunit.Slot) Then ' for a queued work unit this results in VERY long logs!!!
                Return True
            Else
                Return False
            End If
        End Function
        'Use HasIendMatch to avoid including next wu info if the unit is not uploading
        Private Shared Function HasIendMatch(WorkUnit As clsWU, Line As String) As Boolean
            If Line.Contains(strID(WorkUnit)) Then
                Return True
            ElseIf Line.Contains("PAUSE") AndAlso (Line.Contains("SLOT " & WorkUnit.Slot) OrElse Line.Contains("FS" & WorkUnit.Slot)) Then ' PAUSE captures UNPAUSED
                Return True
            ElseIf Line.Contains("FS" & WorkUnit.Slot) AndAlso Line.Contains("WU" & WorkUnit.ID) Then
                Return True
            Else
                Return False
            End If
        End Function
        Private Shared Function ParseWorkUnit(WorkUnit As clsWU, ByRef Lines As List(Of String), xInt As Int32, ByVal lStart As LogStartObject) As Boolean
            Dim dtStart As DateTime = WorkUnit.utcDownloaded, iAddDays As Int32 = 0
            WriteLog("LogParser:ParseWorkUnit:" & WorkUnitLogHeader(WorkUnit) & "Starting parse, setting dtStart to " & dtStart.ToString(CultureInfo.CurrentCulture))
            'Add slot config to workunit log
            WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:---Slot configuration---")
            For Each DictionaryEntry In WorkUnit.SlotConfig.mArguments
                WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:SlotOption:" & DictionaryEntry.Key & " = " & DictionaryEntry.Value)
            Next
            WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:---/Slot configuration---")
            'parse work unit
            Do
                'Declare iSkip for skipping to a line
                Dim iSkip As Int32 = -1
                Dim Line As String = Lines(xInt).ToUpperInvariant
                If HasLogMatch(WorkUnit, Line) Then
                    Try
                        WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), Lines(xInt))
                        WorkUnit.Line = Lines(xInt)
                        WorkUnit.lineIndex = xInt
                        WorkUnit.LineDT = WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                End If
                'Check for iAddDays
                Try
                    If Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}") AndAlso Regex.IsMatch(Lines(xInt - 1), "^\d{2}[:]\d{2}[:]\d{2}") Then
                        If TimeSpan.Parse(Lines(xInt - 1).Substring(0, 8)) > TimeSpan.Parse(Lines(xInt).Substring(0, 8)) Then
                            iAddDays += 1
                            WriteLog("LogParser:ParseWorkUnit:" & WorkUnitLogHeader(WorkUnit) & "Set iAddDays to " & iAddDays)
                            WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:Date changed: " & dtStart.Date.AddDays(iAddDays).ToLongDateString)
                        End If
                    End If
                    'If xInt - 1 > 0 And Lines(xInt).Length > 2 AndAlso Lines(xInt - 1).Length > 2 Then
                    '    Try
                    '        If Int32.Parse(Lines(xInt - 1).Replace(ChrW(34), "").Substring(0, 2), NumberStyles.Integer) > Int32.Parse(Lines(xInt).Replace(ChrW(34), "").Substring(0, 2), NumberStyles.Integer) Then
                    '            iAddDays += 1
                    '            WriteDebug("-Set iAddDays to " & iAddDays)
                    '            WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:Date changed: " & dtStart.Date.AddDays(iAddDays).ToLongDateString)
                    '        End If
                    '    Catch ex As Exception : End Try
                    'End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
                'Check for new " Log Started "
                If Line.Contains("******************************** DATE:") Then
                    '******************************** Date: 21/01/12 ********************************
                    Dim strFormat As String = "dd/MM/yy", provider As CultureInfo = CultureInfo.InvariantCulture
                    dtStart = DateTime.ParseExact(Line.Substring(Line.IndexOf("DATE:") + 6, 8), strFormat, provider)
                    iAddDays = 0
                    WriteLog("LogParser:ParseWorkUnit:" & WorkUnitLogHeader(WorkUnit) & "Startdate set to " & dtStart.ToString("s"))
                ElseIf Line.Contains("LOG STARTED") Then
                    Try
                        'Set new starting datetime, reset iAddDays
                        dtStart = DateTime.Parse(Lines(xInt).Replace("*", "").Replace("Log Started", "").Trim.Replace("-", " "), CultureInfo.InvariantCulture)
                        iAddDays = 0
                        WorkUnit.AddLogLine(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt + 1).Substring(0, 8))), FormatTimeSpan(dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt + 1).Substring(0, 8))).TimeOfDay) & ":FW7:Log started: " & DateTime.Parse(Lines(xInt).Replace("*", "").Replace("Log Started", "").Trim.Replace("-", " "), CultureInfo.InvariantCulture))
                        WriteLog("LogParser:ParseWorkUnit:" & WorkUnitLogHeader(WorkUnit) & "Startdate set to " & dtStart.ToString("s"))
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                ElseIf Line.Contains("<CONFIG>") Then 'Check for new <config> 
                    Dim iEnd As Int32 = -1
                    For yInt As Int32 = xInt To Lines.Count - 1
                        If Lines(yInt).ToUpperInvariant.Contains("</CONFIG>") Then
                            iEnd = yInt
                            Exit For
                        End If
                    Next
                    If iEnd <> -1 Then
                        Dim dtNConfig As DateTime = dtStart.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(iEnd).Substring(0, 8)))
                        Dim strB As New StringBuilder
                        For yInt As Int32 = xInt To iEnd
                            If Regex.IsMatch(Lines(yInt), "^\d{2}[:]\d{2}[:]\d{2}[:]") Then
                                strB.AppendLine(Lines(yInt).Substring(9))
                            Else
                                strB.AppendLine(Lines(yInt))
                            End If
                        Next
                        Dim nClientConfig As New clsClientConfig.clsConfiguration
                        If nClientConfig.ReadString(strB.ToString) Then
                            If Not nClientConfig.Slot(WorkUnit.Slot).Equals(WorkUnit.SlotConfig) Then
                                WorkUnit.SetNewSlotConfig(nClientConfig.Slot(WorkUnit.Slot), dtNConfig)
                            End If
                        Else
                            WriteLog("Log parser hit a snare parsing a <config> </config> section, log: " & WorkUnit.Log, eSeverity.Critical)
                            Return False
                        End If
                    End If
                ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("CLEANING UP") Then
                    'DEBUG:-ParseWorkUnit::23:59:00:WU00:FS02:0X11:COMPLETED 54%
                    'DEBUG:-MARVIN-PC:WU00:FS02:project:5768 run:6 clone:240 gen:247 - 0x4adc926a4f0ccead00f700f000061688::Added a frame, progress: 54% frame datetime: 2012-01-19T23:59:00
                    'DEBUG:-MARVIN-PC:WU00:FS02:project:5768 run:6 clone:240 gen:247 - 0x4adc926a4f0ccead00f700f000061688::Setting progress:54 at: UTC - 1/19/2012 11:59:00 PM local - 1/20/2012 12:59:00 AM
                    'DEBUG:-ParseWorkUnit::23:59:01:WU00:FS02:0X11:MDRUN_GPU RETURNED 
                    'DEBUG:-ParseWorkUnit::23:59:11:WU00:FS02:0X11:GOING TO SEND BACK WHAT HAVE DONE -- STEPSTOTALG=15000000
                    'DEBUG:-ParseWorkUnit::23:59:22:WU00:FS02:0X11:WORK FRACTION=0.5400 STEPS=15000000.
                    'DEBUG:-ParseWorkUnit::23:59:22:WU00:FS02:0X11:LOGFILE SIZE=17033 INFOLENGTH=17033 EDR=0 TRR=25
                    'DEBUG:-ParseWorkUnit::23:59:23:WU00:FS02:0X11:+ OPENED RESULTS FILE
                    'DEBUG:-ParseWorkUnit::23:59:23:WU00:FS02:0X11:- WRITING 17571 BYTES OF CORE DATA TO DISK...
                    'DEBUG:-ParseWorkUnit::23:59:23:WU00:FS02:0X11:DONE: 17059 -> 4840 (COMPRESSED TO 28.3 PERCENT)
                    'DEBUG:-ParseWorkUnit::23:59:23:WU00:FS02:0X11:  ... DONE.
                    'DEBUG:-ParseWorkUnit::00:00:08:WU00:FS02:0X11:DELETEFRAMEFILES: SUCCESSFULLY DELETED FILE=00/WUDATA_01.CKP
                    'DEBUG:-Set iAddDays to 1
                    'DEBUG:-Startdate set to 2012-01-20T00:20:10
                    'DEBUG:-ParseWorkUnit::00:20:11:ENABLED FOLDING SLOT 02: PAUSED GPU:1:"G92 [GEFORCE 9600 GSO]"
                    'DEBUG:-ParseWorkUnit::00:20:11:WARNING:WU00:MISSING DATA FILES, DUMPING
                    'DEBUG:-ParseWorkUnit::00:20:11:WU00:FS02:CLEANING UP

                    'This shouldn't happen?
                    WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Unexpected unit cleanup event detected!", eSeverity.Critical)
                    Dim nArr(200) As String
                    Lines.CopyTo(xInt - 100, nArr, 0, 200)
                    Dim strCheck As String = ""
                    If nArr.Count > 0 Then
                        strCheck = Join(nArr, Environment.NewLine)
                    End If
#If CONFIG = "Debug" Then
                    Console.WriteLine(New String(GetChar("-", 1), 25))
#Else
                    WriteLog(New String(GetChar("-", 1), 25), eSeverity.Critical)
#End If
                    Dim i As Int32 = 0
                    For Each Str As String In nArr
#If CONFIG = "Debug" Then
                        Console.WriteLine(i & " - " & Str)
#Else
                        writelog(i & " - " & Str,eSeverity.Critical)
#End If
                        i += 1
                    Next
#If CONFIG = "Debug" Then
                    Console.WriteLine(New String(GetChar("-", 1), 25))
#Else
                    WriteLog(New String(GetChar("-", 1), 25), eSeverity.Critical)
#End If
                ElseIf (Line.Contains("WARNING: UNIT " & WorkUnit.ID) Or Line.Contains(strID(WorkUnit))) And Line.Contains("MIGRATING TO SLOT") Then
                    'Migrate to other slot - continue parsing 
                    WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "migrated to a new slot, change slotID and continue")
                    WorkUnit.Slot = Lines(xInt).Substring(Lines(xInt).IndexOf("migrating to Slot ") + Len("migrating to Slot "))
                ElseIf HasMatch(WorkUnit, Line) AndAlso WorkUnit.dtStarted = #1/1/2000# AndAlso Line.Contains("STARTING") Then
                    WorkUnit.dtStarted = dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                    WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting dtStarted:" & WorkUnit.utcStarted.ToString(CultureInfo.CurrentCulture))
                ElseIf HasMatch(WorkUnit, Line) AndAlso WorkUnit.CoreVersion = "" AndAlso Line.Contains(":VERSION ") Then
                    WorkUnit.CoreVersion = Lines(xInt).Substring(Line.IndexOf("VERSION ") + 8).Trim
                    WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting core version:" & WorkUnit.CoreVersion)
                ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("RUNNING FAHCORE:") Then
                    If Line.Contains("FAHCOREWRAPPER.exe") Then
                        Try
                            Dim iStart As Int32 = Line.IndexOf("FAHCOREWRAPPER.EXE", 0) + Len("fahcorewrapper.exe   ")
                            Dim tmpStr As String = Line.Substring(iStart)
                            Dim Worker As String = tmpStr.Substring(0, tmpStr.IndexOf(Chr(34)))
                            tmpStr = tmpStr.Replace(Worker, "").TrimStart(Chr(34))
                            Dim args As String = tmpStr
                            WorkUnit.Worker = Worker
                            WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting worker:" & Worker)
                            WorkUnit.WorkerArguments = args
                            WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting worker arguments:" & args)
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                    Else
                        'ignore, all clients use fahcorewrapper
                    End If
                ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains(":COMPILER ") Then
                    '10:22:10:WU03:FS02:0x11:Compiler  : Microsoft (R) 32-bit C/C++ Optimizing Compiler Version 14.00.50727.762 for 80x86 
                    '04:50:08:Unit 01:Compiler  : Microsoft (R) 32-bit C/C++ Optimizing Compiler Version 14.00.50727.762 for 80x86 
                    Dim strTmp As String = Lines(xInt).Substring(Line.IndexOf("COMPILER"))
                    If WorkUnit.CoreCompiler = "" Then
                        WorkUnit.CoreCompiler = strTmp.Substring(strTmp.IndexOf(":") + 1).Trim
                        WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting core compiler:" & WorkUnit.CoreCompiler)
                    Else
                        If Not WorkUnit.CoreCompiler = strTmp.Substring(strTmp.IndexOf(":") + 1).Trim Then
                            WorkUnit.CoreCompiler = strTmp.Substring(strTmp.IndexOf(":") + 1).Trim
                            WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting new core compiler:" & WorkUnit.CoreCompiler)
                        End If
                    End If
                ElseIf HasMatch(WorkUnit, Line) AndAlso WorkUnit.BoardType = "" AndAlso Line.Contains("BOARD TYPE:") Then
                    WorkUnit.BoardType = Lines(xInt).Substring(Line.IndexOf("BOARD TYPE:") + 11).Trim
                    WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting board type:" & WorkUnit.BoardType)
                ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("COMPLETED") AndAlso Line.Contains("%") Then
                    Try
                        For xIP As Int32 = Lines(xInt).IndexOf("%") To 1 Step -1
                            If Lines(xInt).Substring(xIP, 1) = " " Or Lines(xInt).Substring(xIP, 1) = "(" Then
                                Try
                                    If Lines(xInt).Substring(xIP, Lines(xInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim = "0" Then
                                        'ignore 0% untill all core's report it
                                        'If WorkUnit.dtStarted = #1/1/2000# Then 
                                        ' Always set dtStarted, and use frame(0).frameDT.substract(workunit.utcStarted) as timespan for the first frame!
                                        WorkUnit.dtStarted = dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                                        WorkUnit.Percentage = "0"
                                        WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting unit start to " & WorkUnit.utcStarted.ToString(CultureInfo.CurrentCulture))
                                        'End If
                                        Exit For
                                    Else
                                        If WorkUnit.Frames.Count > 0 Then
                                            'TODO Add boolean to track when 'log started' or log date line has been encounterd, discard frames before that happens and parse the rest using
                                            'the latest time index ( prevents all subsequent frames being missed ).
                                            If WorkUnit.Frames(WorkUnit.Frames.Count - 1).utcFrame > dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))) Then
                                                WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Previous and current frame's don't have a valid datetime value", eSeverity.Important)
                                                WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "This can happen if the computer time was adjusted while FAHClient is running", eSeverity.Important)
                                                'Check for active frame restart
                                                If Not WorkUnit.activeFrame.Percentage = CInt(Lines(xInt).Substring(xIP, Lines(xInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim) Then
                                                    'Frame is new, clear previous frame
                                                    WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Clearing last frame", eSeverity.Important)
                                                    WorkUnit.ClearFrames(WorkUnit.activeFrame.Percentage)
                                                End If
                                                GoTo AddFrame
#If CONFIG = "Debug" Then
                                                'Auto correct?
                                                'Console.WriteLine("datetime of current/previous frame and new frame don't match!")
                                                'Console.WriteLine("-Last frame timestamp: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString("s"))
                                                'Console.WriteLine("-Last frame log line:  " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).LogString)
                                                'Dim nArr(200) As String
                                                'Lines.CopyTo(xInt - 100, nArr, 0, 200)
                                                'Dim strCheck As String = ""
                                                'If nArr.Count > 0 Then
                                                '    strCheck = Join(nArr, Environment.NewLine)
                                                'End If
                                                'Console.WriteLine("-------------------------------------------")
                                                'Dim i As Int32 = 0
                                                'For Each Str As String In nArr
                                                '    If Not String.IsNullOrEmpty(Str) Then
                                                '        Console.WriteLine(WorkUnitLogHeader(WorkUnit) & "(" & CStr(i) & "):" & Str)
                                                '        i += 1
                                                '    End If
                                                'Next
                                                'Console.WriteLine("-------------------------------------------")
#Else
                                                WriteDebug("-Failed to parse a frame")
                                                WriteDebug("datetime of current/previous frame and new frame don't match!")
                                                WriteDebug("-Last frame timestamp: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString("s"))
                                                WriteDebug("-Last frame log line:  " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).LogString)
#End If
                                            Else
                                                GoTo AddFrame
                                            End If
                                        Else
AddFrame:
                                            If CShort(Lines(xInt).Substring(xIP, Lines(xInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim) <= WorkUnit.activeFrame.Percentage Then
                                                WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Frame restarted")
                                            Else
                                                If WorkUnit.AddFrame(Lines(xInt), dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))) Then
                                                    If modMySettings.ConvertUTC Then
                                                        WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Added a frame, progress: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).strPercentage & "%" & " frame datetime(local): " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString(CultureInfo.CurrentCulture))
                                                    Else
                                                        WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Added a frame, progress: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).strPercentage & "%" & " frame datetime: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString(CultureInfo.CurrentCulture))
                                                    End If
                                                Else
                                                    WriteLog("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Failed to add a frame to collection: ", eSeverity.Important)
#If CONFIG = "Debug" Then
                                                    'Auto correct?
                                                    Dim nArr(200) As String
                                                    Lines.CopyTo(xInt - 100, nArr, 0, 200)
                                                    Dim strCheck As String = ""
                                                    If nArr.Count > 0 Then
                                                        strCheck = Join(nArr, Environment.NewLine)
                                                    End If
                                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "datetime of current/previous frame and new frame don't match!")
                                                    If WorkUnit.Frames.Count > 0 Then
                                                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Last frame timestamp: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString(CultureInfo.CurrentCulture))
                                                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Last frame log line:  " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).LogString)
                                                    End If
                                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "-------------------------------------------")
                                                    Dim i As Int32 = 0
                                                    For Each Str As String In nArr
                                                        If HasMatch(WorkUnit, Str) Then WriteDebug(WorkUnitLogHeader(WorkUnit) & CStr(i) & " - " & Str)
                                                        i += 1
                                                    Next
                                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "-------------------------------------------")
#Else
                                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Failed to parse a frame")
                                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "datetime of current/previous frame and new frame don't match!")
                                                    If WorkUnit.Frames.Count > 0 Then
                                                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Last frame timestamp: " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).FrameDT.ToString(CultureInfo.CurrentCulture))
                                                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Last frame log line:  " & WorkUnit.Frames(WorkUnit.Frames.Count - 1).LogString)
                                                    End If
#End If
                                                End If
                                            End If

                                        End If
                                        WorkUnit.Percentage = Lines(xInt).Substring(xIP, Lines(xInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim
                                        WriteDebug("Logparser:ParseUnit:" & WorkUnitLogHeader(WorkUnit) & "Setting progress:" & WorkUnit.Percentage & " at: UTC - " & dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).ToString(CultureInfo.CurrentCulture) & " local - " & dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8))).AddHours(lStart.ClientInfo.Info.UTC_Offset).ToString(CultureInfo.CurrentCulture))
                                        Exit For
                                    End If
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                    Return False
                                End Try
                            End If
                            '/xIP index of percentage parsing
                        Next
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                ElseIf (Line.Contains("WARNING: UNIT " & WorkUnit.ID) Or Line.Contains(strID(WorkUnit))) AndAlso Line.Contains("DUMPING") Or (Line.Contains("WARNING:WU" & WorkUnit.ID & ":SLOT ID " & WorkUnit.Slot) And Line.Contains("DUMPING")) Or (Line.Contains("WARNING:WU" & WorkUnit.ID & ":SLOT ID " & WorkUnit.Slot.Substring(1, 1)) And Line.Contains("DUMPING")) Or (HasMatch(WorkUnit, Line) And Line.Contains("DUMPING")) Then
                    '22:06:53:WARNING:WU00:Slot ID 0 no longer exists and there are no other matching slots, dumping
                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Dumping unit detected")
                    WorkUnit.ServerResponce = "dumped" 'lines(xInt).Substring(9)
                    If WorkUnit.CoreStatus = "" Then WorkUnit.CoreStatus = "dumped"
                    WorkUnit.dtCompleted = dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Line.Substring(0, 8)))
                    Dim iStart As Int32
                    If Not GetIStart(Lines, xInt, WorkUnit, iStart) Or iStart = -1 Then
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Failed to set the start of the log snippet", eSeverity.Critical)
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Please report the failure and include the content of log file " & WorkUnit.Log, eSeverity.Critical)
                        Return False
                    End If
                    Dim iEnd As Int32
                    If Not GetIEnd(Lines, xInt, WorkUnit, iEnd, lStart) Or iEnd = -1 Then
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Can not find cleanup event, keeping in queue")
                        WorkUnit.CoreSnippet = WorkUnit.ActiveLogfileUTC(True, True)
                        Return False
                    ElseIf iEnd = Int32.MinValue Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Workunit is restarted, continuing parsing")
                        iSkip = WorkUnit.iSkip
                        GoTo NextLine
                    End If
                    WorkUnit.CoreSnippet = WorkUnit.ActiveLogfileUTC(True, True)
                    Return True
                ElseIf (HasMatch(WorkUnit, Line) AndAlso Line.Contains("FAHCORE RETURNED")) Or (Line.Contains("FAHCORE, RUNNING UNIT " & WorkUnit.ID) Or Line.Contains("FAHCORE RUNNING UNIT " & WorkUnit.ID) AndAlso Line.Contains("RETURNED")) Then
                    Dim strTmp As String = Lines(xInt).Substring(Lines(xInt).ToUpperInvariant.IndexOf("RETURNED:") + 10).Trim
                    If strTmp.Contains("110") Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Skipping core outdated core status")
                        GoTo NextLine
                    ElseIf strTmp.ToUpperInvariant.Contains("INTERRUPTED") Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Skipping INTERRUPTED Core Status")
                        GoTo NextLine
                    ElseIf strTmp.ToUpperInvariant.Contains("TERMINATED") Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Skipping TERMINATED Core Status")
                        GoTo NextLine
                    End If
                    'Unit completion - corestatus - submitted - snippet
                    WorkUnit.CoreStatus = strTmp
                    WorkUnit.dtCompleted = dtStart.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Lines(xInt).Substring(0, 8)))
                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "FahCore returned value: " & WorkUnit.CoreStatus)
                    Dim iStart As Int32
                    If Not GetIStart(Lines, xInt, WorkUnit, iStart) Or iStart = -1 Then
                        '    Return False
                        'ElseIf iStart = -1 Then
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Failed to set the start of the log snippet", eSeverity.Critical)
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Please report the failure and include the content of log file " & WorkUnit.Log, eSeverity.Critical)
                        Return False
                    End If
                    Dim iEnd As Int32
                    If Not GetIEnd(Lines, xInt, WorkUnit, iEnd, lStart) Or iEnd = -1 Then
                        '    Return False
                        'ElseIf iEnd = -1 Then
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "Can not find cleanup event, keeping in queue")
                        WorkUnit.CoreSnippet = WorkUnit.ActiveLogfileUTC(True, True)
                        Return False
                    ElseIf iEnd = Int32.MinValue Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Workunit is restarted, continuing parsing")
                        iSkip = WorkUnit.iSkip
                        GoTo NextLine
                    End If
                    WorkUnit.CoreSnippet = WorkUnit.ActiveLogfileUTC(True, True)
                    Return True
                    '/
                End If
                '/xInt - line index for lines parsing
NextLine:
                Try
                    If xInt = Lines.Count - 1 Then
                        Return True
                    End If
                    If iSkip = -1 Then
                        xInt += 1
                    Else
                        xInt = iSkip
                        iSkip = -1
                    End If
                Catch ex As Exception
                    WriteDebug("Exception in unit parsing loop!")
                    WriteError(ex.Message, Err)
                    Return False
                End Try
                '/xInt increment
            Loop
        End Function
        Private Shared Function GetIStart(comLog As List(Of String), iStart As Int32, WorkUnit As clsWU, ByRef Result As Int32) As Boolean
            Result = -1
            Try
                For zInt As Int32 = iStart To 0 Step -1
                    If comLog(zInt).ToUpperInvariant.Contains("RECEIVED UNIT") AndAlso comLog(zInt).ToUpperInvariant.Contains("UNIT:" & WorkUnit.unit.ToUpperInvariant) Then
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Found unit received event")
                        Result = zInt
                        Exit For
                    End If

                    'If HasMatch(WorkUnit, comLog(zInt).ToUpperInvariant) AndAlso comLog(zInt).ToUpperInvariant.Contains("COMPLETED") AndAlso comLog(zInt).ToUpperInvariant.Contains("%") Then
                    '    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Dump exited due to last frame found")
                    '    Result = zInt
                    '    Exit For
                    'ElseIf HasMatch(WorkUnit, comLog(zInt).ToUpperInvariant) AndAlso comLog(zInt).ToUpperInvariant.Contains("STARTING") Then
                    '    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Dump exited due to unit start detection")
                    '    Result = zInt
                    '    Exit For
                    'End If
                Next
                If Result = -1 Then
                    WriteDebug("-Failed to get the start index of the log snippet")
                    Return False
                End If
                Return True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Private Shared Function GetIEnd(comLog As List(Of String), iStart As Int32, <Out()> WorkUnit As clsWU, ByRef Result As Int32, ByVal lStart As LogStartObject) As Boolean
            Result = -1
            Try
                Dim iAddDays As Int32 = 0
                For xInt As Int32 = iStart To comLog.Count - 1
                    Dim Line As String = comLog(xInt).ToUpperInvariant
                    Try
                        If Regex.IsMatch(comLog(xInt - 1), "^\d{2}[:]\d{2}[:]\d{2}") AndAlso Regex.IsMatch(comLog(xInt), "^\d{2}[:]\d{2}[:]\d{2}") Then
                            If TimeSpan.Parse(comLog(xInt - 1).Substring(0, 8)) > TimeSpan.Parse(comLog(xInt).Substring(0, 8)) Then
                                iAddDays += 1
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))), FormatTimeSpan(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:Date changed: " & WorkUnit.utcCompleted.Date.AddDays(iAddDays).ToLongDateString)
                                WriteDebug("Added a day to iAddDays, current value:" & iAddDays & " - current date:" & WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))))
                            End If
                        End If
                        'If Int32.Parse(comLog(xInt - 1).Substring(0, 2), NumberStyles.Integer) > Int32.Parse(comLog(xInt).Substring(0, 2), NumberStyles.Integer) Then
                        '    iAddDays += 1
                        '    WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))), FormatTimeSpan(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))).TimeOfDay) & ":FW7:Date changed: " & WorkUnit.utcCompleted.Date.AddDays(iAddDays).ToLongDateString)
                        '    WriteDebug("Added a day to iAddDays, current value:" & iAddDays & " - current date:" & WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))))
                        'End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        lStart.Failed = True
                        lStart.Exception = ex
                        Return False
                    End Try
                    If HasIendMatch(WorkUnit, Line) Then
                        WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8))), comLog(xInt))
                        WorkUnit.Line = comLog(xInt)
                        WorkUnit.lineIndex = xInt
                        WorkUnit.LineDT = WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(xInt).Substring(0, 8)))
                    End If
                    If HasMatch(WorkUnit, Line) AndAlso Line.Contains("UPLOADING") Then
                        '23:03:08:Unit 01: Uploading 632B
                        '06:02:28:UNIT 00: UPLOADING 256.55KIB TO 171.64.65.102
                        WorkUnit.dtStartUpload = WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Line.Substring(0, 8)))
                        If WorkUnit.UploadSize = "" Then
                            WorkUnit.UploadSize = comLog(xInt).Substring(Line.IndexOf("UPLOADING ") + Len("Uploading "))
                            If WorkUnit.UploadSize.IndexOf(" ") > 0 Then
                                WorkUnit.UploadSize = WorkUnit.UploadSize.Substring(0, WorkUnit.UploadSize.IndexOf(" "))
                            End If
                            If Line.IndexOf(" TO ") > 0 Then
                                WorkUnit.CS = Line.Substring(Line.IndexOf(" TO ") + 4)
                            End If
                            WriteDebug(WorkUnitLogHeader(WorkUnit) & "Detected upload, upload size:" & WorkUnit.UploadSize & " CS:" & WorkUnit.CS)
                        ElseIf Line.IndexOf(" TO ") > 0 Then
                            If WorkUnit.CS <> Line.Substring(Line.IndexOf(" TO ") + 4) Then
                                WorkUnit.CS = Line.Substring(Line.IndexOf(" TO ") + 4)
                                WriteDebug(WorkUnitLogHeader(WorkUnit) & "Changed submission server to " & WorkUnit.CS)
                            End If
                        End If
                    ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("UPLOAD COMPLETE") Then
                        '11:44:44:Server responded WORK_ACK (400)
                        '11:44:44:Cleaning up Unit 09
                        'OR
                        '01:24:26:Unit 08: Upload complete
                        '01:24:26:Server responded WORK_ACK (400)
                        '01:24:26:Final credit estimate, 2012.00 points
                        '01:24:26:Cleaning up Unit 08
                        'OR
                        '23:07:56:WU03:FS01:Upload complete
                        '23:07:56:WU03:FS01:Server responded WORK_ACK (400)
                        '23:07:56:WU03:FS01:Final credit estimate, 213.00 points
                        WorkUnit.dtSubmitted = WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(Line.Substring(0, 8)))
                        WriteDebug(WorkUnitLogHeader(WorkUnit) & "Found upload completed event at " & WorkUnit.utcSubmitted.ToString(CultureInfo.CurrentCulture))
                        For zInt As Int32 = xInt To comLog.Count - 1
                            Dim zLine As String = comLog(zInt).ToUpperInvariant
                            If HasMatch(WorkUnit, zLine) AndAlso zLine.Contains("SERVER RESPONDED") Then
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(zInt).Substring(0, 8))), comLog(zInt))
                                WorkUnit.ServerResponce = comLog(zInt).Substring(zLine.IndexOf("SERVER RESPONDED ") + Len("SERVER RESPONDED "))
                                WriteDebug(WorkUnitLogHeader(WorkUnit) & "Response found:" & WorkUnit.ServerResponce)
                            ElseIf Regex.IsMatch(zLine.ToUpperInvariant, "^\d{2}[:]\d{2}[:]\d{2}[:][sS][eE][rR][vV][eE][rR]\s[rR][eE][sS][pP][oO][nN][dD][eE][dD]") Then
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(zInt).Substring(0, 8))), comLog(zInt))
                                WorkUnit.ServerResponce = comLog(zInt).Substring(zLine.IndexOf("SERVER RESPONDED ") + Len("SERVER RESPONDED "))
                                WriteDebug(WorkUnitLogHeader(WorkUnit) & "Response found:" & WorkUnit.ServerResponce)
                            ElseIf HasMatch(WorkUnit, zLine) AndAlso WorkUnit.Credit = "" AndAlso zLine.Contains("FINAL CREDIT ESTIMATE") Then
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(zInt).Substring(0, 8))), comLog(zInt))
                                Try
                                    Dim strPoints As String = zLine.Trim(Chr(10)).Substring(zLine.Trim(Chr(10)).IndexOf("ESTIMATE,") + 9).Replace("POINTS", "").Trim
                                    'Dim strPoints As String = zLine.Trim(Chr(10)).Substring(32).Replace("POINTS", "")
                                    If CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator <> "." Then
                                        strPoints = strPoints.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                    End If
                                    WorkUnit.Credit = FormatPPD(strPoints)
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                    Return False
                                Finally
                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Server reported credit estimate: " & WorkUnit.Credit)
                                End Try
                            ElseIf Regex.IsMatch(zLine.ToUpperInvariant, "^\d{2}[:]\d{2}[:]\d{2}[:][fF][iI][nN][aA][lL]\s[cC][rR][eE][dD][iI][tT]\s[eE][sS][tT][iI][mM][aA][tT][eE]") Then
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(zInt).Substring(0, 8))), comLog(zInt))
                                Try
                                    Dim strPoints As String = zLine.Trim(Chr(10)).Substring(zLine.Trim(Chr(10)).IndexOf("ESTIMATE,") + 9).Replace("POINTS", "").Trim
                                    'Dim strPoints As String = zLine.Trim(Chr(10)).Substring(32).Replace("POINTS", "")
                                    If CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator <> "." Then
                                        strPoints = strPoints.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                                    End If
                                    WorkUnit.Credit = FormatPPD(strPoints)
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                    Return False
                                Finally
                                    WriteDebug(WorkUnitLogHeader(WorkUnit) & "Server reported credit estimate: " & WorkUnit.Credit)
                                End Try
                            ElseIf HasMatch(WorkUnit, zLine) AndAlso zLine.Contains("CLEANING UP") Then
                                WorkUnit.AddLogLine(WorkUnit.utcCompleted.Date.AddDays(iAddDays).Add(TimeSpan.Parse(comLog(zInt).Substring(0, 8))), comLog(zInt))
                                'This should not happen before any upload, server responce and credit estimate has been found
                                Result = xInt
                                WriteDebug(WorkUnitLogHeader(WorkUnit) & "Cleaning up detected")
                                'WorkUnit = AccreditWorkunit(WorkUnit)
                                If Not AccreditWorkunit(WorkUnit) Then
                                    Return False
                                Else
                                    Return True
                                End If
                            End If
                        Next
                        WriteLog("Unit: " & WorkUnit.unit & " can't find cleanup event!", eSeverity.Critical)
                        Result = Int32.MinValue
                        Return False
                    ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("STARTING") Then '  Line.Contains("STARTING UNIT " & WorkUnit.ID) Or Line.Contains(strID(WorkUnit) & ":STARTING") 
                        WriteLog(WorkUnitLogHeader(WorkUnit) & "seems to be restarted")
                        WorkUnit.bHasRestarted = True
                        WorkUnit.iSkip = Int32.MinValue
                        If WorkUnit.Frames.Count >= 1 Then
                            WorkUnit.restartInfo.AddRestart(WorkUnit.CoreStatus, WorkUnit.Frames(WorkUnit.Frames.Count - 1))
                        Else
                            'Check!
                            WorkUnit.restartInfo.AddRestart(WorkUnit.CoreStatus, New clsWU.clsFrame(comLog(xInt).Substring(0, 8) & "Unit 18:Completed INVALID out of INVALID steps  (0%)", WorkUnit.dtDownloaded))
                        End If
                        'Check to clear frames
                        For zInt As Int32 = xInt To comLog.Count - 1
                            'FahCore, running Unit 13, returned:
                            '(comLog(zInt).ToUpperInvariant.Contains("FAHCORE, RUNNING UNIT " & WorkUnit.ID & ", RETURNED:") Or comLog(zInt).ToUpperInvariant.Contains("FAHCORE RUNNING UNIT " & WorkUnit.ID & ", RETURNED:")) Or comLog(zInt).ToUpperInvariant.Contains("WU" & WorkUnit.ID & ":FS" & WorkUnit.Slot & ":FAHCORE RETURNED") Then
                            If HasMatch(WorkUnit, comLog(zInt).ToUpperInvariant) AndAlso (comLog(zInt).ToUpperInvariant.Contains("FAHCORE, RUNNING UNIT " & WorkUnit.ID) Or comLog(zInt).ToUpperInvariant.Contains("FAHCORE RUNNING UNIT " & WorkUnit.ID) AndAlso comLog(zInt).ToUpperInvariant.Contains("RETURNED")) Or comLog(zInt).ToUpperInvariant.Contains("FAHCORE RETURNED:") Then
                                'WorkUnit.iSkip = zInt - 1
                                WorkUnit.iSkip = zInt
                                Result = Int32.MinValue
                                Return True
                            ElseIf HasMatch(WorkUnit, comLog(zInt).ToUpperInvariant) AndAlso comLog(zInt).ToUpperInvariant.Contains("COMPLETED") AndAlso comLog(zInt).ToUpperInvariant.Contains("%") Then
                                '(comLog(zInt).ToUpperInvariant.Contains("UNIT " & WorkUnit.ID & ":") And comLog(zInt).Contains("%")) Or (comLog(zInt).ToUpperInvariant.Contains("WU" & WorkUnit.ID & ":FS" & WorkUnit.Slot) And comLog(zInt).ToUpperInvariant.Contains("COMPLETED") And comLog(zInt).ToUpper.Contains("%")) Then
                                For xIP As Int32 = comLog(zInt).IndexOf("%", 0, System.StringComparison.InvariantCulture) To 1 Step -1
                                    If comLog(zInt).Substring(xIP, 1) = " " Or comLog(zInt).Substring(xIP, 1) = "(" Then
                                        If WorkUnit.Percentage = comLog(zInt).Substring(xIP, comLog(zInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim Then
                                            'Don't clear frames, continue parsing unit from xIP
                                            WriteDebug("-Unit has been restarted from the same checkpoint, continue parsing from " & zInt)
                                            WorkUnit.iSkip = zInt
                                            'return minvalue and continue parsing in main function from iSkip
                                            Result = Int32.MinValue
                                            Return True
                                        ElseIf WorkUnit.Percentage <> "" Then
                                            If Int32.Parse(WorkUnit.Percentage, NumberStyles.Integer) > Int32.Parse(comLog(zInt).Substring(xIP, comLog(zInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim, NumberStyles.Integer) Then
                                                'Clear frames before new percentage 
                                                WriteDebug("-Unit has been restarted from " & comLog(zInt).Substring(xIP, comLog(zInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim & ", clearing redundant frame info and continuing to parse unit from " & zInt)
                                                WorkUnit.ClearFrames(CInt(comLog(zInt).Substring(xIP, comLog(zInt).IndexOf("%") - xIP).Trim.Replace("(", "").Trim))
                                                'set iSkip to xInt and continue parsing
                                                WorkUnit.iSkip = zInt
                                                'return minvalue and continue parsing in main function from iSkip
                                                Result = Int32.MinValue
                                                Return True
                                            End If
                                        End If
                                    End If
                                    '/xIP stripping down to progress
                                Next
                                WriteLog("Failed to parse a frame!", eSeverity.Critical)
                                WriteLog("-" & strID(WorkUnit) & ":" & WorkUnit.PRCG & " - " & WorkUnit.unit, eSeverity.Critical)
                                If WorkUnit.Frames.Count > 0 Then WriteLog("--" & WorkUnit.Frames(WorkUnit.Frames.Count - 1).LogString, eSeverity.Critical)
                                WriteLog("--" & comLog(zInt).ToUpperInvariant, eSeverity.Critical)
                            End If
                            '/zInt looking for frame clear
                        Next
                    ElseIf HasMatch(WorkUnit, Line) AndAlso Line.Contains("CLEANING UP") Then
                        ' Line.Contains("CLEANING UP UNIT " & WorkUnit.ID) Or Line.Contains(strID(WorkUnit) & ":CLEANING UP") Or Line.Contains(":WU" & WorkUnit.ID & ":CLEANING UP") Then
                        'This should not happen before any upload, server responce and credit estimate has been found
                        Result = xInt
                        WriteLog("Cleaning up detected - Finalizing " & WorkUnit.unit & " but unexpected things might happen...", eSeverity.Critical)
                        'WorkUnit = AccreditWorkunit(WorkUnit)
                        If Not AccreditWorkunit(WorkUnit) Then
                            WriteDebug("-Failed to accredit work unit: " & WorkUnit.ClientName & "-" & strID(WorkUnit) & ":" & WorkUnit.PRCG)
                            Return False
                        End If
                        WriteDebug("Finalized work unit " & WorkUnit.unit)
                        Return True
                    End If
                    '/xInt looking for upload or dump event 
                Next
                If Result = -1 Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            Finally
                '#If CONFIG = "Debug" Then
                '                DebugOutput = bOld
                '#End If
            End Try
        End Function
    End Class
    'Delegate function 
    Friend Delegate Function AsyncParseLogs(ByVal lStart As LogStartObject) As LogStartObject
#End Region
#Region "AsyncMain entry point"
    Friend Class AsyncMain
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
        Private Delegate Function dAsyncGetFiles(ByVal Location As String, ByRef Files As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
#End Region
#Region "ParseWait"

        Friend Shared Function ParseWait(Optional ShowBussy As Boolean = False, Optional ParentForm As Form = Nothing, Optional Disableforms As Boolean = True) As Boolean
            Dim rVal As Boolean = False
            Dim dtNow As DateTime = DateTime.Now
            WriteLog("Log parser started")
            Try
                If ShowBussy Then
                    delegateFactory.BussyBox.ShowForm("Parsing logs... please wait", True, Nothing, Disableforms)
                Else
                    delegateFactory.SetMessage("Parsing logs... please wait")
                End If
                Dim lWorkUnits As New List(Of clsWU)
                Dim lClientInfo As New Dictionary(Of String, List(Of clsClientInfo))
                Dim lClientConfig As New Dictionary(Of String, List(Of clsClientConfig))
                Dim dLogFiles As New Dictionary(Of String, List(Of clsLogFile))
                Dim lEOC As New List(Of String)
                For Each Client As Client In Clients.Clients
                    If Not Client.Enabled Then
                        WriteLog("-Skipping parse for disabled client: " & Client.ClientName)
                        If ShowBussy Then
                            delegateFactory.BussyBox.SetMessage("-Skipping parse for disabled client: " & Client.ClientName)
                        Else
                            delegateFactory.SetMessage("-Skipping parse for disabled client: " & Client.ClientName)
                        End If
                        GoTo SkipClient
                    End If
                    'Check files
                    Dim startLog As LogStartObject = New LogStartObject
                    Dim nFiles As New List(Of String)
#If CONFIG = "Debug" Then
                    'GoTo UpdateOnly
#End If
                    'If Client.LastLineIndex > 0 Then GoTo UpdateOnly
                    Try
                        If Not Client.ClientLocation = Clients.LocalClient.ClientLocation Then
                            If Not My.Computer.Network.IsAvailable Then
                                WriteLog("Network not available, skipping parse of " & Client.ClientName, eSeverity.Critical)
                                Client.ClientConfig = New clsClientConfig
                                Client.ClientConfig.Configuration = sqdata.ClientConfigBeforeDT(Client.ClientName, DateTime.Now)
                                Client.ClientInfo = New clsClientInfo
                                Client.ClientInfo.Info = sqdata.ClientInfoBeforeDT(Client.ClientName, DateTime.Now)
                                Client.Enabled = False
                                GoTo SkipClient
                            Else
                                Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = Nothing
                                Dim aGetFiles As New dAsyncGetFiles(AddressOf AsyncGetFiles)
                                Dim aGetFilesResult As IAsyncResult = aGetFiles.BeginInvoke(Client.ClientLocation, lFiles, Nothing, Nothing)
                                aGetFilesResult.AsyncWaitHandle.WaitOne()
                                Dim bRes As Boolean = aGetFiles.EndInvoke(lFiles, aGetFilesResult)
                                aGetFilesResult.AsyncWaitHandle.Close()
                                If Not bRes Then
                                    WriteLog("Client " & Client.ClientName & " can't be accessed, skipping!", eSeverity.Critical)
                                    'Set clientconfig and info to latest stored 
                                    Client.ClientConfig = New clsClientConfig
                                    Client.ClientConfig.Configuration = sqdata.ClientConfigBeforeDT(Client.ClientName, DateTime.Now)
                                    Client.ClientInfo = New clsClientInfo
                                    Client.ClientInfo.Info = sqdata.ClientInfoBeforeDT(Client.ClientName, DateTime.Now)
                                    Client.Reachable = False
                                    GoTo SkipClient
                                Else
                                    Client.Reachable = True
                                End If
                                For Each File As String In lFiles
                                    If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
                                        nFiles.Add(File)
                                    End If
                                Next
                            End If
                        Else
                            Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Client.ClientLocation & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
                            For Each File As String In lFiles
                                If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
                                    nFiles.Add(File)
                                End If
                            Next
                        End If
                        'Add old queued work units
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        If ShowBussy Then
                            delegateFactory.BussyBox.CloseForm()
                        End If
                        rVal = False
                        GoTo Finish
                    End Try
UpdateOnly:
                    startLog.lQueued.AddRange(sqdata.QueuedWorkUnits(Client.ClientName))
                    nFiles.Add(Client.ClientLocation & "\log.txt")
                    With startLog
                        .ClientName = Client.ClientName
                        .ParentForm = ParentForm
                        .ShowUI = ShowBussy
                        .Files = nFiles
                    End With
                    Dim lParser As New AsyncParser()
                    Dim caller As New AsyncParseLogs(AddressOf lParser.ParseLogs)
                    Dim aResult As IAsyncResult = caller.BeginInvoke(startLog, Nothing, Nothing)
                    aResult.AsyncWaitHandle.WaitOne()
                    startLog = caller.EndInvoke(aResult)
                    aResult.AsyncWaitHandle.Close()
                    If startLog.Failed Then
                        Dim nFailed As New MyEventArgs.ParserFailedEventArgs
                        nFailed.Clientname = startLog.ClientName
                        nFailed.Exception = startLog.Exception
                        delegateFactory.RaiseParserFailed(startLog, nFailed)
                        If ShowBussy And delegateFactory.BussyBox.IsFormVisible Then delegateFactory.BussyBox.CloseForm()
                        Return False
                    Else
                        lWorkUnits.AddRange(startLog.lWU)
                        lClientInfo.Add(startLog.ClientName, startLog.dClientInfo.Values.ToList)
                        lClientConfig.Add(startLog.ClientName, startLog.dClientConfig.Values.ToList)
                        dLogFiles.Add(startLog.ClientName, startLog.lLogFiles)
                        For Each DictionaryEntry In startLog.dClientConfig
                            If Not lEOC.Contains(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team) Then
                                lEOC.Add(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team)
                            End If
                            For Each xUser As clsClientConfig.clsConfiguration.clsExtraUser In DictionaryEntry.Value.Configuration.ExtraUsers
                                If Not lEOC.Contains(xUser.user & "#" & xUser.team) Then
                                    lEOC.Add(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team)
                                End If
                            Next
                        Next
                        Clients.Client(startLog.ClientName).ClientConfig = startLog.ClientConfig
                        Clients.Client(startLog.ClientName).ClientInfo = startLog.ClientInfo
                        Clients.Client(startLog.ClientName).LastLineIndex = startLog.LastLineIndex
                        Clients.Client(startLog.ClientName).LastLine = startLog.LastLine
                        Clients.Client(startLog.ClientName).LastLineDT = startLog.LastLineDT
                    End If
                    startLog.Dispose()
SkipClient:
                Next

                If ShowBussy Then
                    delegateFactory.BussyBox.SetMessage("Marking finished logs")
                Else
                    delegateFactory.SetMessage("Marking finished logs")
                End If

                For Each DictionaryEntry In dLogFiles
                    For Each LogFile As clsLogFile In DictionaryEntry.Value
                        If LogFile.AllDone Then
                            sqdata.SetLogStored(DictionaryEntry.Key, LogFile.fileName, LogFile.Log, Int32.Parse(LogFile.LineCount, NumberStyles.Integer))
                        End If
                    Next
                Next

                If ShowBussy Then
                    delegateFactory.BussyBox.SetMessage("Storing parse results")
                Else
                    delegateFactory.SetMessage("Storing parse results")
                End If

                sqdata.SaveWorkUnit(lWorkUnits)
                For Each DictionaryEntry In lClientConfig
                    sqdata.SaveFAHClientConfig(DictionaryEntry.Key, DictionaryEntry.Value)
                Next
                For Each DictionaryEntry In lClientInfo
                    sqdata.SaveFAHClientInfo(DictionaryEntry.Key, DictionaryEntry.Value)
                Next

                If ShowBussy Then
                    delegateFactory.BussyBox.SetMessage("Checking user accounts")
                Else
                    delegateFactory.SetMessage("Checking user accounts")
                End If

                For Each sLine As String In lEOC
                    Dim user As String = sLine.Substring(0, sLine.IndexOf("#", 0, StringComparison.InvariantCulture))
                    Dim team As String = sLine.Substring(sLine.IndexOf("#", 0, StringComparison.InvariantCulture) + 1)
                    EOCInfo.AddAccount(user, team, , True)
                Next

                'disable statistics generation if called from live?
                If Not ReferenceEquals(ParentForm, Live) Then
                    If ShowBussy Then
                        delegateFactory.BussyBox.SetMessage("Generating statistics:" & Environment.NewLine & Environment.NewLine & "Historical performance: pending" & Environment.NewLine & "Performance statistics: pending" & Environment.NewLine & "Project statistics: pending" & Environment.NewLine & "Hardware statistics: pending")
                    Else
                        delegateFactory.SetMessage("Generating statistics.. " & "Historical statistics: pending" & " Performance statistics: pending" & " Project statistics: pending" & " Hardware statistics: pending")
                    End If

                    Thread.CurrentThread.Priority = ThreadPriority.Lowest
                    Dim bHW As Boolean = False, bPerf As Boolean = False, bProj As Boolean = False, bHist As Boolean = False

                    Try
                        Dim asyncHistorical As New clsStatistics.clsHistoricalStatistics.GenerateHistoricalStatisticsDelegate(AddressOf clsStatistics.clsHistoricalStatistics.GenerateHistoricalStatistics)
                        Dim result As IAsyncResult = asyncHistorical.BeginInvoke(Nothing, Nothing)
                        result.AsyncWaitHandle.WaitOne()
                        bHist = asyncHistorical.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try

                    If ShowBussy Then
                        delegateFactory.BussyBox.SetMessage("Generating statistics:" & Environment.NewLine & Environment.NewLine & "Historical performance: " & bHist.ToString & Environment.NewLine & "Performance statistics: pending" & Environment.NewLine & "Project statistics: pending" & Environment.NewLine & "Hardware statistics: pending")
                    Else
                        delegateFactory.SetMessage("Generating statistics.. " & "Historical statistics: " & bHist.ToString & " Performance statistics: pending" & " Project statistics: pending" & " Hardware statistics: pending")
                    End If


                    Try
                        Dim asyncPerformance As New clsStatistics.clsPerformanceStatistics.dGeneratePerformanceStatistics(AddressOf clsStatistics.clsPerformanceStatistics.GeneratePerformanceStatistics)
                        Dim result = asyncPerformance.BeginInvoke(Nothing, Nothing)
                        result.AsyncWaitHandle.WaitOne()
                        bPerf = asyncPerformance.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try

                    If ShowBussy Then
                        delegateFactory.BussyBox.SetMessage("Generating statistics:" & Environment.NewLine & Environment.NewLine & "Historical statistics: " & bHist.ToString & Environment.NewLine & "Performance statistics: " & bPerf & Environment.NewLine & "Project statistics: pending" & Environment.NewLine & "Hardware statistics: pending")
                    Else
                        delegateFactory.SetMessage("Generating statistics.. " & "Historical statistics: " & bHist & " Performance statistics: " & bPerf & " Project statistics: pending" & " Hardware statistics: pending")
                    End If

                    Try
                        Dim asyncProject As New clsStatistics.clsProjectStatistics.dGenerateProjectStatistics(AddressOf clsStatistics.clsProjectStatistics.GenerateProjectStatistics)
                        Dim result As IAsyncResult = asyncProject.BeginInvoke(Nothing, Nothing)
                        result.AsyncWaitHandle.WaitOne()
                        bProj = asyncProject.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try

                    If ShowBussy Then
                        delegateFactory.BussyBox.SetMessage("Generating statistics:" & Environment.NewLine & Environment.NewLine & "Historical statistics: " & bHist & Environment.NewLine & "Performance statistics: " & bPerf & Environment.NewLine & "Project statistics: " & bPerf & Environment.NewLine & "Hardware statistics: pending")
                    Else
                        delegateFactory.SetMessage("Generating statistics.. " & "Historical statistics: " & bHist & " Performance statistics: " & bPerf & " Project statistics: " & bProj & " Hardware statistics: pending")
                    End If

                    Try
                        Dim asyncHardware As New clsStatistics.clsHardwareStatistics.dGenerateHardwareStatistics(AddressOf clsStatistics.clsHardwareStatistics.GenerateHardwareStatistics)
                        Dim result As IAsyncResult = asyncHardware.BeginInvoke(Nothing, Nothing)
                        result.AsyncWaitHandle.WaitOne()
                        bHW = asyncHardware.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                        Thread.CurrentThread.Priority = ThreadPriority.Normal
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try

                    If ShowBussy Then
                        delegateFactory.BussyBox.SetMessage("Generating statistics:" & Environment.NewLine & "Historical statistics: " & bHist & Environment.NewLine & "Performance statistics: " & bPerf & Environment.NewLine & "Project statistics: " & bPerf & Environment.NewLine & "Hardware statistics: " & bHW)
                    Else
                        delegateFactory.SetMessage("Generating statistics.. " & "Historical statistics: " & bHist & " Performance statistics: " & bPerf & " Project statistics: " & bProj & " Hardware statistics: " & bHW)
                    End If

                    rVal = bPerf AndAlso bProj AndAlso bHW
                Else
                    rVal = True
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                rVal = False
            Finally

                If ShowBussy And delegateFactory.BussyBox.IsFormVisible Then
                    delegateFactory.BussyBox.CloseForm()
                Else
                    delegateFactory.SetMessage("Logparser finished, took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
                End If

            End Try
Finish:
            WriteLog("Log parser took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return rVal
        End Function
#End Region
#Region "ParseCallback"
        Friend Shared Sub ParseCallback(Optional ShowBussy As Boolean = False)
            Try
                For Each Client As Client In Clients.Clients
                    'Check files
                    Dim startLog As LogStartObject = New LogStartObject
                    Dim nFiles As New List(Of String)
                    Try
                        If Not Client.ClientLocation = Clients.LocalClient.ClientLocation Then
                            If Not My.Computer.Network.IsAvailable Then
                                WriteLog("Network not available, skipping parse of " & Client.ClientName, eSeverity.Important)
                            Else
                                Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Client.ClientLocation & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
                                For Each File As String In lFiles
                                    If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
                                        nFiles.Add(File)
                                    End If
                                Next
                            End If
                        Else
                            Dim lFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(Client.ClientLocation & "\Logs\", FileIO.SearchOption.SearchTopLevelOnly, "log*.txt")
                            For Each File As String In lFiles
                                If Not sqdata.IsLogStored(Client.ClientName, File.Substring(File.LastIndexOf("\") + 1)) Then
                                    nFiles.Add(File)
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        startLog.Failed = True
                        startLog.Exception = ex
                        Return
                    Finally
                        If ShowBussy Then
                            delegateFactory.BussyBox.CloseForm()
                        End If
                    End Try

                    Dim callBack As AsyncCallback
                    callBack = AddressOf CallbackMethod


                    nFiles.Add(Client.ClientLocation & "\log.txt")
                    With startLog
                        .ClientName = Client.ClientName
                        .ShowUI = ShowBussy
                        .Files = nFiles
                    End With
                    Dim lParser As New AsyncParser()
                    ' Create the delegate.
                    Dim caller As New AsyncParseLogs(AddressOf lParser.ParseLogs)
                    ' Initiate the asynchronous call.
                    Dim ar As IAsyncResult = caller.BeginInvoke(startLog, callBack, lParser)
                    'startLog.Dispose()
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub CallbackMethod(ByVal result As IAsyncResult)
            ' Retrieve the delegate.
            Dim startLog As LogStartObject = CType(result.AsyncState, LogStartObject)
            Dim ar As AsyncResult = CType(result, AsyncResult)
            Dim dlgt As AsyncParser = CType(ar.AsyncState, AsyncParser)
            Try

            Catch ex As Exception

            End Try

            ' Call EndInvoke to retrieve the results.
            'Dim startLog As LogStartObject = dlgt.EndInvoke(ar)

            If Not startLog.Failed Then
                With startLog
                    sqdata.SaveWorkUnit(.lWU, True)
                    For Each DictionaryEntry In .dClientInfo
                        sqdata.SaveFAHClientInfo(startLog.ClientName, DictionaryEntry.Key, DictionaryEntry.Value.Info)
                    Next
                    Dim lAccounts As New List(Of String)
                    For Each DictionaryEntry In .dClientConfig
                        If Not lAccounts.Contains(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team) Then
                            lAccounts.Add(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team)
                        End If
                        For Each xUser As clsClientConfig.clsConfiguration.clsExtraUser In DictionaryEntry.Value.Configuration.ExtraUsers
                            If Not lAccounts.Contains(xUser.user & "#" & xUser.team) Then
                                lAccounts.Add(DictionaryEntry.Value.User & "#" & DictionaryEntry.Value.Team)
                            End If
                        Next
                        sqdata.SaveFAHClientConfig(startLog.ClientName, DictionaryEntry.Value.Configuration.ConfigurationDT, DictionaryEntry.Value.Configuration)
                    Next
                    For Each lFile As clsLogFile In .lLogFiles
                        If lFile.AllDone Then
                            sqdata.SetLogStored(startLog.ClientName, lFile.fileName, lFile.Log, CInt(lFile.LineCount))
                        End If
                    Next
                    For Each sLine As String In lAccounts
                        'Dim user As String = sLine.Substring(0, sLine.IndexOf("#", 0, System.StringComparison.InvariantCulture))
                        Dim user As String = sLine.Substring(0, sLine.IndexOf("#"))
                        Dim team As String = sLine.Substring(sLine.IndexOf("#") + 1)
                        EOCInfo.AddAccount(user, team, , True)
                    Next
                    'SyncLock Clients.Client(startLog.ClientName)
                    Clients.Client(startLog.ClientName).ClientConfig = .ClientConfig
                    Clients.Client(startLog.ClientName).ClientInfo = .ClientInfo
                    Clients.Client(startLog.ClientName).ActiveWU.Clear()
                    Clients.Client(startLog.ClientName).ActiveWU.AddRange(startLog.lActiveWU)
                    'End SyncLock
                    Dim pSucces As New MyEventArgs.ParserCompletedEventArgs
                    pSucces.Clientname = .ClientName
                    pSucces.HasActiveWU = CBool(.lActiveWU.Count > 0)
                    delegateFactory.RaiseParserCompleted(startLog, pSucces)
                End With
            Else
                Dim pSucces As New MyEventArgs.ParserFailedEventArgs
                pSucces.Clientname = startLog.ClientName
                pSucces.Exception = startLog.Exception
                pSucces.IsNowDisabled = Not Clients.Client(startLog.ClientName).Enabled
                delegateFactory.RaiseParserFailed(startLog, pSucces)
            End If
            startLog.Dispose()
        End Sub
#End Region
    End Class
#End Region
End Class


