'/*
' * FAHWatch7 Statistics class Copyright Marvin Westmaas ( mtm )
' *
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General public License
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
Imports System.Text
Imports System.Globalization
Imports System.Threading
Friend Class clsStatistics
    Friend Class clsHistoricalStatistics
        Private Shared mStats As clsPerformanceStatistics.sClient = Nothing
        Friend Delegate Function GenerateHistoricalStatisticsDelegate() As Boolean
        Friend Shared Function GenerateHistoricalStatistics() As Boolean
            Try
                mStats = Nothing
                mStats = Statistics
                Return True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            Finally
                GC.Collect()
            End Try
        End Function
        Friend Shared Shadows Property Statistics As clsPerformanceStatistics.sClient
            Get
                If IsNothing(mStats) Then
                    Try
                        Dim lWU As List(Of clsWU) = sqdata.WorkUnits("")
                        mStats = clsPerformanceStatistics.Statistics(lWU)
                        lWU = Nothing
                        Return mStats
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return mStats
                    Finally
                        GC.Collect()
                    End Try
                Else
                    Return mStats
                End If
            End Get
            Set(value As clsPerformanceStatistics.sClient)
                mStats = value
            End Set
        End Property
    End Class
    Friend Class clsPerformanceStatistics
        Friend dtStatistics As DateTime = #1/1/2000#
        Private mWUCompleted As Int32 = 0
        Private mWUFailed As Int32 = 0
        Friend Property Wu_Completed As String
            Get
                Return CStr(mWUCompleted)
            End Get
            Set(value As String)
                Dim iValue As Int32
                If Integer.TryParse(value, iValue) Then
                    mWUCompleted = iValue
                End If
            End Set
        End Property
        Friend Property Wu_EUE As String
            Get
                Return CStr(mWUFailed)
            End Get
            Set(value As String)
                Dim iValue As Int32
                If Integer.TryParse(value, iValue) Then
                    mWUFailed = iValue
                End If
            End Set
        End Property
        Friend ReadOnly Property SuccesRate As String
            Get
                If mWUCompleted = 0 Or mWUFailed = 0 Then Return "100%"
                'Return Math.Round(100 - Failed / (Count / 100), 2).ToString & "%"
                Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2).ToString & "%"
            End Get
        End Property
        Friend ReadOnly Property SuccesRateDBL As Double
            Get
                Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2)
            End Get
        End Property
        Friend ComputationTime As String = ""
        Friend TotalCredit As String = ""
        Private mLower As DateTime = DateTime.MaxValue
        Private mUpper As DateTime = DateTime.MinValue
        Friend Property dtLower As DateTime
            Get
                Return mLower
            End Get
            Set(value As DateTime)
                mLower = value
            End Set
        End Property
        Friend Property dtUpper As DateTime
            Get
                Return mUpper
            End Get
            Set(value As DateTime)
                mUpper = value
            End Set
        End Property
        Public ReadOnly Property AvgPPD As String
            Get
                Return FormatPPD(CStr(Math.Round(CInt(TotalCredit) / mUpper.Subtract(mLower).TotalDays, 2)))
            End Get
        End Property
        Friend Class sClient
            Friend Class sSlot
                Friend ID As String = ""
                Friend Hardware As String 'Match to gpu-index or info("cpu")
                Private mWUCompleted As Int32 = 0
                Private mWUFailed As Int32 = 0
                Friend Property Wu_Completed As String
                    Get
                        Return CStr(mWUCompleted)
                    End Get
                    Set(value As String)
                        Dim iValue As Int32
                        If Integer.TryParse(value, iValue) Then
                            mWUCompleted = iValue
                        End If
                    End Set
                End Property
                Friend Property Wu_EUE As String
                    Get
                        Return CStr(mWUFailed)
                    End Get
                    Set(value As String)
                        Dim iValue As Int32
                        If Integer.TryParse(value, iValue) Then
                            mWUFailed = iValue
                        End If
                    End Set
                End Property
                Friend ReadOnly Property SuccesRate As String
                    Get
                        If mWUCompleted = 0 Or mWUFailed = 0 Then Return "100%"
                        'Return Math.Round(100 - Failed / (Count / 100), 2).ToString & "%"
                        Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2).ToString & "%"
                    End Get
                End Property
                Friend ReadOnly Property SuccesRateDBL As Double
                    Get
                        Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2)
                    End Get
                End Property
                Friend ComputationTime As String = ""
                Friend TotalCredit As String = ""
                Private mLower As DateTime = DateTime.MaxValue
                Private mUpper As DateTime = DateTime.MinValue
                Friend Property dtLower As DateTime
                    Get
                        Return mLower
                    End Get
                    Set(value As DateTime)
                        mLower = value
                    End Set
                End Property
                Friend Property dtUpper As DateTime
                    Get
                        Return mUpper
                    End Get
                    Set(value As DateTime)
                        mUpper = value
                    End Set
                End Property
                Public ReadOnly Property AvgPPD As String
                    Get
                        If CInt(TotalCredit) > 0 Then
                            Return FormatPPD(CStr(Math.Round(CInt(TotalCredit) / mUpper.Subtract(mLower).TotalDays, 2)))
                        Else
                            Return "0"
                        End If
                    End Get
                End Property
            End Class
            Friend Slots As New Dictionary(Of String, sSlot)
            Friend Name As String = ""
            Private mWUCompleted As Int32 = 0
            Private mWUFailed As Int32 = 0
            Friend Property Wu_Completed As String
                Get
                    Return CStr(mWUCompleted)
                End Get
                Set(value As String)
                    Dim iValue As Int32
                    If Integer.TryParse(value, iValue) Then
                        mWUCompleted = iValue
                    End If
                End Set
            End Property
            Friend Property Wu_EUE As String
                Get
                    Return CStr(mWUFailed)
                End Get
                Set(value As String)
                    Dim iValue As Int32
                    If Integer.TryParse(value, iValue) Then
                        mWUFailed = iValue
                    End If
                End Set
            End Property
            Friend ReadOnly Property SuccesRate As String
                Get
                    If mWUCompleted = 0 Or mWUFailed = 0 Then Return "100%"
                    'Return Math.Round(100 - Failed / (Count / 100), 2).ToString & "%"
                    Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2).ToString & "%"
                End Get
            End Property
            Friend ReadOnly Property SuccesRateDBL As Double
                Get
                    Return Math.Round(100 - mWUFailed / (mWUCompleted / 100), 2)
                End Get
            End Property
            Friend ComputationTime As String = ""
            Friend TotalCredit As String = ""
            Private mLower As DateTime = DateTime.MaxValue
            Private mUpper As DateTime = DateTime.MinValue
            Friend Property dtLower As DateTime
                Get
                    Return mLower
                End Get
                Set(value As DateTime)
                    mLower = value
                End Set
            End Property
            Friend Property dtUpper As DateTime
                Get
                    Return mUpper
                End Get
                Set(value As DateTime)
                    mUpper = value
                End Set
            End Property
            Friend ReadOnly Property AvgPPD As String
                Get
                    Dim strPPD As String = ""
                    Try
                        strPPD = FormatPPD(CStr(Math.Round(CInt(TotalCredit) / mUpper.Subtract(mLower).TotalDays, 2)))
                    Catch ex As Exception
                        strPPD = ""
                    End Try
                    Return strPPD
                End Get
            End Property
        End Class
        Friend Clients As New Dictionary(Of String, sClient)
        Private Shared mReport As String = String.Empty
        Friend ReadOnly Property Report(Optional ClientName As String = Nothing, Optional SlotID As String = Nothing) As String
            Get
                If Not IsNothing(ClientName) Then
                    If Not IsNothing(SlotID) Then
                        If IsNothing(Clients(ClientName).Slots(SlotID)) Then
                            WriteLog("clsPerformanceStatistics.Report(" & ClientName & ").Slots(" & SlotID & ").report: Empty slot reference", eSeverity.Critical)
                            Return String.Empty
                        End If
                        Dim sbC As New StringBuilder
                        With Clients(ClientName).Slots(SlotID)
                            sbC.AppendLine("Client: " & ClientName & ":FS: " & .ID & " - " & .Hardware)
                            sbC.AppendLine("Total wu's completed: " & .Wu_Completed)
                            sbC.AppendLine("Total wu's failed: " & .Wu_EUE)
                            sbC.AppendLine("Succes rate: " & .SuccesRate)
                            sbC.AppendLine("Average PPD: " & .AvgPPD)
                            sbC.AppendLine("Total credits: " & .TotalCredit)
                            sbC.AppendLine("Computation time: " & .ComputationTime)
                        End With
                        Return sbC.ToString
                    Else
                        Dim sbC As New StringBuilder
                        Dim Client As sClient = Clients(ClientName)
                        sbC.AppendLine("Client: " & Client.Name)
                        sbC.AppendLine("Total wu's completed: " & Client.Wu_Completed)
                        sbC.AppendLine("Total wu's failed: " & Client.Wu_EUE)
                        sbC.AppendLine("Succes rate: " & Client.SuccesRate)
                        sbC.AppendLine("Computation time: " & Client.ComputationTime)
                        sbC.AppendLine("Average PPD: " & Client.AvgPPD)
                        sbC.AppendLine("Total credits: " & Client.TotalCredit)
                        sbC.AppendLine()
                        If Not IsNothing(Client.Slots) Then
                            For Each Slot In Client.Slots.Values.ToList
                                sbC.AppendLine("Slot: " & Slot.ID & " - " & Slot.Hardware)
                                sbC.AppendLine("Total wu's completed: " & Slot.Wu_Completed)
                                sbC.AppendLine("Total wu's failed: " & Slot.Wu_EUE)
                                sbC.AppendLine("Succes rate: " & Slot.SuccesRate)
                                sbC.AppendLine("Average PPD: " & Slot.AvgPPD)
                                sbC.AppendLine("Total credits: " & Slot.TotalCredit)
                                sbC.AppendLine("Computation time: " & Slot.ComputationTime)
                                sbC.AppendLine()
                            Next
                        End If
                        Return sbC.ToString
                    End If
                End If
                If Not mReport = String.Empty Then Return mReport
                Try
                    Dim sbC As New StringBuilder
                    If Not IsNothing(Clients) Then
                        For Each Client In Clients.Values.ToList
                            sbC.AppendLine("Client: " & Client.Name)
                            sbC.AppendLine("Total wu's completed: " & Client.Wu_Completed)
                            sbC.AppendLine("Total wu's failed: " & Client.Wu_EUE)
                            sbC.AppendLine("Succes rate: " & Client.SuccesRate)
                            sbC.AppendLine("Computation time: " & Client.ComputationTime)
                            sbC.AppendLine("Average PPD: " & Client.AvgPPD)
                            sbC.AppendLine("Total credits: " & Client.TotalCredit)
                            sbC.AppendLine()
                            If Not IsNothing(Client.Slots) Then
                                For Each Slot In Client.Slots.Values.ToList
                                    If CInt(Slot.Wu_Completed) > 0 Then
                                        sbC.AppendLine("Slot: " & Slot.ID & " - " & Slot.Hardware)
                                        sbC.AppendLine("Total wu's completed: " & Slot.Wu_Completed)
                                        sbC.AppendLine("Total wu's failed: " & Slot.Wu_EUE)
                                        sbC.AppendLine("Succes rate: " & Slot.SuccesRate)
                                        sbC.AppendLine("Average PPD: " & Slot.AvgPPD)
                                        sbC.AppendLine("Total credits: " & Slot.TotalCredit)
                                        sbC.AppendLine("Computation time: " & Slot.ComputationTime)
                                        sbC.AppendLine()
                                    Else
                                        sbC.AppendLine()
                                        sbC.AppendLine("Skipping slot " & Slot.ID & ":" & Slot.Hardware & " because it has 0 finished work units")
                                        sbC.AppendLine()
                                    End If
                                Next
                            End If
                        Next
                    End If
                    Dim sb As New StringBuilder
                    sb.AppendLine("Totals:")
                    sb.AppendLine("Total wu's completed: " & Wu_Completed)
                    sb.AppendLine("Total wu's failed: " & Wu_EUE)
                    sb.AppendLine("Succes rate: " & SuccesRate)
                    sb.AppendLine("Average PPD: " & AvgPPD)
                    sb.AppendLine("Total credits: " & TotalCredit)
                    sb.AppendLine("Total computation time: " & ComputationTime)
                    sb.AppendLine()
                    sb.Append(sbC.ToString)
                    mReport = sb.ToString
                    Return mReport
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return ""
                End Try
            End Get
        End Property
        Shared mPerformanceStatistics As clsPerformanceStatistics
        Friend Shared ReadOnly Property CurrentStatistics As clsPerformanceStatistics
            Get
                If mPerformanceStatistics.dtStatistics = #1/1/2000# Then
                    Dim dGen As New dGeneratePerformanceStatistics(AddressOf GeneratePerformanceStatistics)
                    Dim result As IAsyncResult = dGen.BeginInvoke(Nothing, Nothing)
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    Dim bRes As Boolean = dGen.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                    If Not bRes Then
                        WriteLog("Failed to generate performance statistics", eSeverity.Important)
                    End If
                End If
                Return mPerformanceStatistics
            End Get
        End Property
        Friend Delegate Function dGeneratePerformanceStatistics() As Boolean
        Friend Shared Function GeneratePerformanceStatistics() As Boolean
            Dim dtNow As DateTime = DateTime.Now
            WriteLog("Generating performance statistics")
            Thread.CurrentThread.Priority = Threading.ThreadPriority.Highest
            Dim rVal As Boolean = False
            Dim mBackup As clsPerformanceStatistics = mPerformanceStatistics
            Try
                Dim c As clsPerformanceStatistics = sqdata.PerformanceStatistics()
                mReport = String.Empty
                mPerformanceStatistics = c
                rVal = True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                rVal = False
            End Try
            If Not rVal Then
                mPerformanceStatistics = mBackup
            End If
            WriteLog("Generation took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return rVal
        End Function
        Friend Shared Function Statistics(WorkUnits As List(Of clsWU)) As sClient ' used for history view with wu selection based on sql query
            Dim rV As New sClient
            Try
                Dim iWU_Total As Int32 = WorkUnits.Count
                Dim iWU_credit As Int32 = 0 'Wu's to count for avg credit
                Dim iWU_EUE As Int32 = 0
                Dim tsProcessing As New TimeSpan
                Dim iTotalCredit As Double = 0
                Dim dslot As New Dictionary(Of String, Double)
                For Each lWU As clsWU In WorkUnits
                    If lWU.dtStartDownload < rV.dtLower Then rV.dtLower = lWU.dtStartDownload
                    If lWU.dtSubmitted > rV.dtUpper Then rV.dtUpper = lWU.dtSubmitted
                    'iWU_total += 1 changed to not count unfinished work units
                    If lWU.PPD <> "" And lWU.PPD <> "0" Then
                        If Not dslot.ContainsKey((lWU.ClientName & "##" & lWU.Slot & "##" & lWU.HW).Trim) Then
                            dslot.Add((lWU.ClientName & "##" & lWU.Slot & "##" & lWU.HW).Trim, CDbl(lWU.PPD))
                        Else
                            dslot((lWU.ClientName & "##" & lWU.Slot & "##" & lWU.HW).Trim) = Math.Round(CDbl(lWU.PPD) + dslot((lWU.ClientName & "##" & lWU.Slot & "##" & lWU.HW).Trim), 2)
                        End If
                        iTotalCredit += CDbl(lWU.Credit)
                    End If
                    If Not lWU.CoreStatus.ToUpper(CultureInfo.InvariantCulture).Contains("FINISHED_UNIT") Then
                        iWU_EUE += 1
                    End If
                    tsProcessing = tsProcessing.Add(lWU.dtCompleted.Subtract(lWU.dtStarted))
                    'If lWU.Frames.Count = 1 Then
                    '    tsProcessing.Add(lWU.tsTPF)
                    'ElseIf lWU.Frames.Count > 1 Then
                    '    tsProcessing = tsProcessing.Add(lWU.Frames(lWU.Frames.Count - 1).FrameDT - lWU.Frames(0).FrameDT)
                    'End If
                Next
                rV.Wu_Completed = iWU_Total.ToString
                rV.Wu_EUE = iWU_EUE.ToString
                rV.ComputationTime = FormatTimeSpan(tsProcessing)
                rV.TotalCredit = iTotalCredit.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rV
        End Function
    End Class
    Friend Class clsHardwareStatistics
        Friend Shared dtStatistics As DateTime = #1/1/2000#
        Private Shared mCurrentStatistics As New clsStatistics.clsHardwareStatistics
        Friend Shared mHardware As New Dictionary(Of String, clsHardware)
        Friend Class clsHardware
            Implements IDisposable
            Friend dtStatistics As DateTime = #1/1/2000#
            Friend Hardware As String
            Friend WU_Completed As String = ""
            Friend WU_EUE As String = ""
            Friend ReadOnly Property SuccesRate As String
                Get
                    If WU_Completed = "" Or WU_EUE = "" Then Return "100%"
                    Return Math.Round(100 - CInt(WU_EUE) / (CInt(WU_Completed) / 100), 2).ToString & "%"
                End Get
            End Property
            Friend ComputationTime As String = ""
            Friend TotalCredit As String = ""
            Friend AveragePPD As String
            Private mlWU As New List(Of clsWU)
            Friend Sub AddWUS(lWU As List(Of clsWU))
                mlWU.AddRange(lWU)
            End Sub
            Friend Sub AddWU(WU As clsWU)
                mlWU.Add(WU)
                WU_Completed = CStr(CInt(WU_Completed) + 1)
                If Not WU.CoreStatus.ToUpperInvariant.Contains("FINISHED_UNIT") Then
                    WU_EUE = CStr(CInt(WU_EUE) - 1)
                End If
                Try
                    Dim tsTMP As TimeSpan = TimeSpan.Parse(ComputationTime)
                    tsTMP = tsTMP.Add(WU.dtCompleted.Subtract(WU.dtStarted))
                    ComputationTime = tsTMP.ToString
                Catch ex As Exception
                    WriteLog("-Computation time: " & ComputationTime, eSeverity.Critical)
                    WriteLog("-tsTMP: " & WU.dtCompleted.Subtract(WU.dtStarted).ToString, eSeverity.Critical)
                    WriteError(ex.Message, Err)
                End Try
                TotalCredit = CStr(Double.Parse(TotalCredit) + Double.Parse(WU.Credit))
                Try
                    Dim ppdTmp As Double = Double.Parse(AveragePPD) * (CInt(WU_Completed) - 1) + Double.Parse(WU.PPD) / CInt(WU_Completed)
                    AveragePPD = FormatPPD(Math.Round(ppdTmp, 2).ToString)
                Catch ex As Exception
                    WriteLog("-Avg ppd: " & AveragePPD & Chr(32) & " wu.ppd: " & WU.PPD, eSeverity.Critical)
                    WriteError(ex.Message, Err)
                End Try
                dtStatistics = DateTime.Now
            End Sub
            Friend Sub ClearWUS()
                mlWU.Clear()
            End Sub
            Friend Function Report(Optional IncludeWUS As Boolean = False) As String
                Dim sb As New StringBuilder
                sb.AppendLine("Hardware: " & vbTab & vbTab & Hardware)
                sb.AppendLine("Completed work units: " & vbTab & WU_Completed)
                sb.AppendLine("Failed work units: " & vbTab & vbTab & WU_EUE)
                sb.AppendLine("Success rate: " & vbTab & vbTab & SuccesRate)
                sb.AppendLine("Total credit: " & vbTab & vbTab & TotalCredit)
                sb.AppendLine("Avg ppd: " & vbTab & vbTab & vbTab & FormatPPD(AveragePPD))
                sb.AppendLine("Total computation time:" & vbTab & FormatTimeSpan(TimeSpan.Parse(ComputationTime)))
                If IncludeWUS Then
                    sb.AppendLine("")
                    For Each WU As clsWU In mlWU
                        sb.Append(WU.Report)
                    Next
                End If
                Return sb.ToString
            End Function
#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls
            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                        mlWU = Nothing
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
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class
        Friend Shared Function Report(Optional Hardware As String = Nothing) As String
            If IsNothing(Hardware) Then
                Dim sb As New StringBuilder
                For Each DictionaryEntry In mHardware
                    sb.Append(DictionaryEntry.Value.Report)
                    sb.AppendLine("") : sb.AppendLine("")
                Next
                Return sb.ToString
            Else
                If mHardware.ContainsKey(Hardware) Then
                    Return mHardware(Hardware).Report
                Else
                    WriteLog("Attempt to access a non existing hardwarestatistics instance, " & Hardware, eSeverity.Important)
                    For Each DictionaryEntry In mHardware
                        WriteLog("--" & DictionaryEntry.Key & " is known.", eSeverity.Important)
                    Next
                    Return String.Empty
                End If
            End If
        End Function
        Friend Delegate Function dGenerateHardwareStatistics() As Boolean
        Friend Shared Function GenerateHardwareStatistics() As Boolean
            Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Highest
            Dim rVal As Boolean = True
            Dim backup As clsHardwareStatistics = Nothing
            If Not clsHardwareStatistics.dtStatistics = #1/1/2000# Then
                backup = clsHardwareStatistics.mCurrentStatistics
            End If
            Try
                mCurrentStatistics = sqdata.HardwareStatistics
                WriteDebug("Generated hardware statistics")
            Catch ex As Exception
                WriteError(ex.Message, Err)
                rVal = False
                mCurrentStatistics = backup
            End Try
            Return rVal
        End Function
    End Class
    Friend Class clsProjectStatistics
        Friend Shared dtStatistics As DateTime = #1/1/2000#
        Friend Class clsProject
            Private mProject As String = String.Empty
            Friend Property Number As String
                Get
                    Return mProject
                End Get
                Set(value As String)
                    mProject = value
                End Set
            End Property
            Private mAvgTPF As String
            Friend ReadOnly Property HW_Names As List(Of String)
                Get
                    Dim rVal As New List(Of String)
                    For Each wu In Projects
                        If Not rVal.Contains(wu.HW) Then rVal.Add(wu.HW)
                    Next
                    Return rVal
                End Get
            End Property

            Friend ReadOnly Property MinTpf(Optional HW As String = Nothing) As String
                Get
                    Try
                        If Not IsNothing(HW) Then
                            Dim tsTMP As TimeSpan = TimeSpan.MaxValue
                            For Each WU As clsWU In Projects
                                'Console.WriteLine(WU.tpfDB)
                                If (Not WU.tpfMinDB = "" And Not WU.tpfMinDB = "00:00:00" And WU.HW = HW) Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfMinDB, tsWU) Then
                                        If tsWU < tsTMP Then tsTMP = tsWU
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If Not tsTMP = TimeSpan.MaxValue Then
                                Return FormatTimeSpan(tsTMP)
                            Else
                                Return ""
                            End If
                        Else
                            Dim tsTMP As TimeSpan = TimeSpan.MaxValue
                            For Each WU As clsWU In Projects
                                If (Not WU.tpfMinDB = "" And Not WU.tpfMinDB = "00:00:00") Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfMinDB, tsWU) Then
                                        If tsWU < tsTMP Then tsTMP = tsWU
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If Not tsTMP = TimeSpan.MaxValue Then
                                Return FormatTimeSpan(tsTMP)
                            Else
                                Return ""
                            End If
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property

            Friend ReadOnly Property AvgTPF(Optional HW As String = Nothing) As String
                Get
                    Try
                        If Not IsNothing(HW) Then
                            Dim tsTMP As New TimeSpan, iCount As Int32 = 0
                            For Each WU As clsWU In Projects
                                'Console.WriteLine(WU.tpfDB)
                                If (Not WU.tpfDB = "" And Not WU.tpfDB = "00:00:00" And WU.HW = HW) Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfDB, tsWU) Then
                                        tsTMP = tsTMP.Add(TimeSpan.Parse(WU.tpfDB))
                                        iCount += 1
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If iCount = 0 Then
                                Return ""
                            Else
                                Dim tsAvg As TimeSpan = TimeSpan.FromTicks(CLng(tsTMP.Ticks / iCount))
                                Return (FormatTimeSpan(tsAvg))
                            End If
                        ElseIf IsNothing(mAvgTPF) Then
                            Dim tsTMP As New TimeSpan, iCount As Int32 = 0
                            For Each WU As clsWU In Projects
                                If (Not WU.tpfDB = "" And Not WU.tpfDB = "00:00:00") Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfDB, tsWU) Then
                                        tsTMP = tsTMP.Add(TimeSpan.Parse(WU.tpfDB))
                                        iCount += 1
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If iCount = 0 Then
                                Return ""
                            Else
                                Dim tsAvg As TimeSpan = TimeSpan.FromTicks(CLng(tsTMP.Ticks / iCount))
                                mAvgTPF = FormatTimeSpan(tsAvg)
                                Return mAvgTPF
                            End If
                        Else
                            Return mAvgTPF
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property

            Friend ReadOnly Property MaxTPF(Optional HW As String = Nothing) As String
                Get
                    Try
                        If Not IsNothing(HW) Then
                            Dim tsTMP As TimeSpan = TimeSpan.MinValue
                            For Each WU As clsWU In Projects
                                'Console.WriteLine(WU.tpfDB)
                                If (Not WU.tpfMaxDB = "" And Not WU.tpfMaxDB = "00:00:00" And WU.HW = HW) Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfMaxDB, tsWU) Then
                                        If tsWU > tsTMP Then tsTMP = tsWU
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If Not tsTMP = TimeSpan.MinValue Then
                                Return FormatTimeSpan(tsTMP)
                            Else
                                Return ""
                            End If
                        Else
                            Dim tsTMP As TimeSpan = TimeSpan.MaxValue
                            For Each WU As clsWU In Projects
                                If (Not WU.tpfMaxDB = "" And Not WU.tpfMaxDB = "00:00:00") Then
                                    Dim tsWU As New TimeSpan
                                    If TimeSpan.TryParse(WU.tpfMaxDB, tsWU) Then
                                        If tsWU > tsTMP Then tsTMP = tsWU
                                    Else
                                        WriteLog("Failed to parse a timespan from string: " & WU.tpfDB, eSeverity.Critical)
                                    End If
                                End If
                            Next
                            If Not tsTMP = TimeSpan.MinValue Then
                                Return FormatTimeSpan(tsTMP)
                            Else
                                Return ""
                            End If
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property

            Friend ReadOnly Property MinPPD(Optional HW As String = Nothing) As String
                Get
                    Try
                        Dim dblPPD As Double = Double.MaxValue
                        If IsNothing(HW) Then
                            For Each wu As clsWU In Projects
                                If wu.PPD <> "" Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        If dPPD < dblPPD Then dblPPD = dPPD
                                    End If
                                End If
                            Next
                        Else
                            For Each wu As clsWU In Projects
                                If wu.PPD <> "" AndAlso wu.HW = HW Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        If dPPD < dblPPD Then dblPPD = dPPD
                                    End If
                                End If
                            Next
                        End If
                        If Not dblPPD = Double.MaxValue Then
                            Return FormatPPD(CStr(Math.Round(dblPPD, 2)))
                        Else
                            Return ""
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property

            Friend ReadOnly Property AvgPPD(Optional HW As String = Nothing) As String
                Get
                    Try
                        Dim dblPPD As New Double, iCount As Int32 = 0
                        If IsNothing(HW) Then
                            For Each wu As clsWU In Projects
                                If wu.PPD <> "" Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        dblPPD += dPPD
                                        iCount += 1
                                    End If
                                End If
                            Next
                        Else
                            For Each wu As clsWU In Projects
                                If wu.HW = HW And wu.PPD <> "" Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        dblPPD += dPPD
                                        iCount += 1
                                    End If
                                End If
                            Next
                        End If
                        Return FormatPPD(CStr(Math.Round(dblPPD / iCount, 2)))
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property

            Friend ReadOnly Property MaxPPD(Optional HW As String = Nothing) As String
                Get
                    Try
                        Dim dblPPD As Double = Double.MinValue
                        If IsNothing(HW) Then
                            For Each wu As clsWU In Projects
                                If wu.PPD <> "" Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        If dPPD > dblPPD Then dblPPD = dPPD
                                    End If
                                End If
                            Next
                        Else
                            For Each wu As clsWU In Projects
                                If wu.PPD <> "" AndAlso wu.HW = HW Then
                                    Dim dPPD As Double
                                    If Double.TryParse(wu.PPD, dPPD) Then
                                        If dPPD > dblPPD Then dblPPD = dPPD
                                    End If
                                End If
                            Next
                        End If
                        If Not dblPPD = Double.MaxValue Then
                            Return FormatPPD(CStr(Math.Round(dblPPD, 2)))
                        Else
                            Return ""
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return ""
                    End Try
                End Get
            End Property


            Private mCount As Int32 = 0
            Friend Property Count As Int32
                Get
                    Return mCount
                End Get
                Set(value As Int32)
                    mCount = value
                End Set
            End Property
            Private mFailed As Int32 = 0
            Friend Property Failed As Int32
                Get
                    Return mFailed
                End Get
                Set(value As Int32)
                    mFailed = value
                End Set
            End Property
            Private mComputationTime As New TimeSpan(0)
            Friend ReadOnly Property ComputationTime As String
                Get
                    Return FormatTimeSpan(tsComputationTime)
                End Get
            End Property
            Friend Property tsComputationTime As TimeSpan
                Get
                    Return mComputationTime
                End Get
                Set(value As TimeSpan)
                    mComputationTime = value
                End Set
            End Property
            Friend ReadOnly Property SuccesRate As String
                Get
                    If Count = 0 Or Failed = 0 Then Return "100%"
                    Return Math.Round(100 - Failed / (Count / 100), 2).ToString & "%"
                End Get
            End Property
            Friend ReadOnly Property DBL_succesRate As Double
                Get
                    Return Math.Round(100 - Failed / (Count / 100), 2)
                End Get
            End Property
            Private mTotalCredit As Double = 0
            Friend Property TotalCredit As String
                Get
                    Return FormatPPD(CStr(mTotalCredit))
                End Get
                Set(value As String)
                    mTotalCredit = CDbl(value)
                End Set
            End Property
            Friend ReadOnly Property DBL_TotalCredit As Double
                Get
                    Return Math.Round(mTotalCredit, 2)
                End Get
            End Property
            Private mPPD As String = String.Empty
            Public Property AvgPPDDB As String
                Get
                    Return mPPD
                End Get
                Set(value As String)
                    mPPD = FormatPPD(value)
                End Set
            End Property
            Private mProjects As New List(Of clsWU)
            Friend ReadOnly Property Projects(Optional HW As String = Nothing) As List(Of clsWU)
                Get
                    Try
                        If IsNothing(HW) Then
                            Return mProjects
                        Else
                            Dim rVal As New List(Of clsWU)
                            For Each WU In mProjects
                                If WU.HW = HW Then rVal.Add(WU)
                            Next
                            Return rVal
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return New List(Of clsWU)
                    End Try
                End Get
            End Property
            Friend Overloads Function AddWU(WU As clsWU) As Boolean
                Dim rVal As Boolean = False
                Try
                    mProjects.Add(WU)
                    rVal = True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return rVal
            End Function
            Friend Overloads Function AddWU(lWU As List(Of clsWU)) As Boolean
                Dim rVal As Boolean = False
                Try
                    mProjects.AddRange(lWU)
                    rVal = True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return rVal
            End Function
            Friend ReadOnly Property Report() As String
                Get
                    Dim sb As New StringBuilder
                    Try
                        sb.AppendLine("Project number:" & vbTab & mProject)
                        sb.AppendLine("Count:" & vbTab & vbTab & CStr(mCount))
                        sb.AppendLine("Failed:" & vbTab & vbTab & CStr(Failed))
                        sb.AppendLine("Succes rate:" & vbTab & SuccesRate)
                        sb.AppendLine("Computation time:" & vbTab & ComputationTime)
                        sb.AppendLine("Avg ppd:" & vbTab & vbTab & AvgPPD)
                        sb.AppendLine("Total credit:" & vbTab & vbTab & TotalCredit)
                        sb.AppendLine("Min tpf:" & vbTab & vbTab & MinTpf)
                        sb.AppendLine("Avg tpf:" & vbTab & vbTab & AvgTPF)
                        sb.AppendLine("Max tpf:" & vbTab & vbTab & MaxTPF)
                        sb.AppendLine("Min ppd:" & vbTab & vbTab & MinPPD)
                        sb.AppendLine("Avg ppd:" & vbTab & vbTab & AvgPPD)
                        sb.AppendLine("Max ppd:" & vbTab & vbTab & MaxPPD)
                        If HW_Names.Count > 1 Then
                            sb.AppendLine("Number of different hw:" & vbTab & HW_Names.Count)
                            For xInt As Int32 = 0 To HW_Names.Count - 1
                                sb.AppendLine("(" & xInt.ToString & ")-" & HW_Names(xInt))
                                sb.AppendLine("(" & xInt.ToString & ")- Avg tpf:" & vbTab & AvgTPF(HW_Names(xInt)))
                            Next
                        End If
                        If mCount > 0 Then
                            sb.AppendLine("---------------------------")
                            sb.AppendLine()
                            For Each WU As clsWU In mProjects
                                sb.Append(WU.Report)
                                sb.AppendLine("")
                            Next
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                    Return sb.ToString
                End Get
            End Property
        End Class
        Private Shared dProjectStatistics As New Dictionary(Of String, clsProjectStatistics.clsProject)
        Public ReadOnly Property lProjects As List(Of String)
            Get
                Return dProjectStatistics.Keys.ToList
            End Get
        End Property
        Public ReadOnly Property HasProjectStatistics(ProjectNumber As String) As Boolean
            Get
                Return dProjectStatistics.ContainsKey(ProjectNumber)
            End Get
        End Property
        Public ReadOnly Property ProjectStatistics(ProjectNumber As String) As clsProject
            Get
                Return dProjectStatistics(ProjectNumber)
            End Get
        End Property
        Public ReadOnly Property ProjectStatistics(Index As Int32) As clsProject
            Get
                Return dProjectStatistics.Values.ToList(Index)
            End Get
        End Property
        Friend Delegate Function dGenerateProjectStatistics() As Boolean
        Friend Shared Function GenerateProjectStatistics() As Boolean
            Threading.Thread.CurrentThread.Priority = ThreadPriority.Highest
            Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
            WriteLog("Generating project statistics")
            Dim rVal As Boolean = False
            Dim mBackup As Dictionary(Of String, clsProjectStatistics.clsProject) = dProjectStatistics
            Try
                dProjectStatistics = sqdata.ProjectStatistics
                'clsProjectStatistics.mProjectStatistics.AddRange(mProjectStatistics)
                dtStatistics = DateTime.Now
                mReport = Report()
                rVal = True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                dProjectStatistics = mBackup
            End Try
            If Not bFailure Then WriteLog("-generation took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return rVal
        End Function
        Friend Overloads Shared Function Report(Lower As Int32, Upper As Int32) As String
            'Check ranges ect ect
            If Lower = ilower And Upper = iUpper And Not mRangeReport = String.Empty Then
                Return mRangeReport
            Else
                mRangeReport = String.Empty
            End If
            Try
                Dim sb As New StringBuilder
                For Each Project As clsProjectStatistics.clsProject In dProjectStatistics.Values.ToList
                    If CInt(Project.Number) >= Lower And CInt(Project.Number) <= Upper Then
                        With Project
                            sb.AppendLine("Project number:" & vbTab & .Number)
                            sb.AppendLine("Count:" & vbTab & vbTab & CStr(.Count))
                            sb.AppendLine("Failed:" & vbTab & vbTab & CStr(.Failed))
                            sb.AppendLine("Computation time:" & vbTab & .ComputationTime)
                            sb.AppendLine("Succes rate:" & vbTab & .SuccesRate)
                            sb.AppendLine("Avg ppd:" & vbTab & vbTab & FormatPPD(.AvgPPD))
                            sb.AppendLine("Total credit:" & vbTab & .TotalCredit)
                            sb.AppendLine("Avg tpf:" & vbTab & vbTab & .AvgTPF)
                            If .Count > 0 Then
                                sb.AppendLine("---------------------------")
                                sb.AppendLine()
                                For Each WU As clsWU In .Projects
                                    sb.Append(WU.Report)
                                    sb.AppendLine("")
                                Next
                            End If
                        End With
                    End If
                Next
                mRangeReport = sb.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return mRangeReport
        End Function
        Private Shared mReport As String = String.Empty, mRangeReport As String = String.Empty, mRange As String = String.Empty, iUpper As Int32 = -1, ilower As Int32 = -1
        Friend Overloads Shared Function Report(Optional Range As String = Nothing, Optional GenerateNewReport As Boolean = False) As String
            If dtStatistics = #1/1/2000# Then
                If Not GenerateProjectStatistics() Then Return String.Empty
            End If
            'Check mreport ect ect ect
            If GenerateNewReport Then
                mReport = String.Empty
                mRange = String.Empty
                mRangeReport = String.Empty
                ilower = -1
                iUpper = -1
            End If
            If IsNothing(Range) Then
                If mRangeReport = String.Empty And Not mReport = String.Empty Then
                    Return mReport
                End If
            ElseIf Range = mRange And Not mReport = String.Empty Then
                Return mReport
            Else
                mReport = String.Empty
                If IsNothing(Range) Then
                    mRange = String.Empty
                Else
                    mRange = Range
                End If
            End If
            Try
                Dim sb As New StringBuilder
                If IsNothing(Range) Then
                    For Each Project As clsProjectStatistics.clsProject In dProjectStatistics.Values.ToList
                        With Project
                            sb.AppendLine("Project number:" & vbTab & .Number)
                            sb.AppendLine("Count:" & vbTab & vbTab & CStr(.Count))
                            sb.AppendLine("Failed:" & vbTab & vbTab & CStr(.Failed))
                            sb.AppendLine("Computation time:" & vbTab & .ComputationTime)
                            sb.AppendLine("Succes rate:" & vbTab & .SuccesRate)
                            sb.AppendLine("Avg ppd:" & vbTab & vbTab & FormatPPD(.AvgPPD))
                            sb.AppendLine("Total credit:" & vbTab & .TotalCredit)
                            sb.AppendLine("Avg tpf:" & vbTab & vbTab & .AvgTPF)
                            If .HW_Names.Count > 1 Then
                                sb.AppendLine("Number of different hw:" & vbTab & .HW_Names.Count)
                                For xInt As Int32 = 0 To .HW_Names.Count - 1
                                    sb.AppendLine("(" & xInt.ToString & ")-" & .HW_Names(xInt))
                                    sb.AppendLine("(" & xInt.ToString & ")- Avg tpf:" & vbTab & .AvgTPF(.HW_Names(xInt)))
                                Next
                            End If
                            If .Count > 0 Then
                                sb.AppendLine("---------------------------")
                                sb.AppendLine()
                                For Each WU As clsWU In .Projects
                                    sb.Append(WU.Report)
                                    sb.AppendLine("")
                                Next
                            End If
                        End With
                    Next
                    mReport = sb.ToString
                    Return mReport
                Else
                    mRange = Range
                    Dim milower As Int32 = CInt(Range.Substring(0, Range.IndexOf(Chr(32))))
                    Dim miUpper As Int32 = CInt(Range.Substring(Range.LastIndexOf(Chr(32))))
                    Return Report(milower, miUpper)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return String.Empty
            End Try
        End Function
    End Class
End Class
