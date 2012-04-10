Imports System.Text
Imports System.Globalization
Imports System.Text.RegularExpressions

Friend Class clsWU
    Implements IDisposable
#Region "Properties"
    'Line identifier 
    Friend Property iSkip As Int32 = Int32.MinValue
    Friend Property Log As String = ""
    Friend Property lineIndex As Int32 = 0
    Friend Property LineDT As DateTime = #1/1/2000#
    Friend Property Line As String = ""
    Friend Property ClientName As String = ""
#Region "General work unit fields"
#Region "Project(run, clone, gen)"
    Friend Property PRCG As String = ""
    Friend ReadOnly Property Project As String
        Get
            Dim p As String = PRCG.ToLower(CultureInfo.InvariantCulture).Replace("project:", "")
            Return p.Substring(0, p.IndexOf(" ", 0, StringComparison.InvariantCulture))
        End Get
    End Property
    Friend ReadOnly Property RCG As String
        Get
            Return PRCG.ToLower(CultureInfo.InvariantCulture).Substring(PRCG.ToLower(CultureInfo.InvariantCulture).IndexOf("run:"))
        End Get
    End Property
    Friend ReadOnly Property RCG_Short As String
        Get
            Return "R" & Run & "C" & Clone & "G" & Gen
        End Get
    End Property
    Friend ReadOnly Property Run As String
        Get
            Try
                Return RCG.ToLower(CultureInfo.InvariantCulture).Substring(4, RCG.ToLower(CultureInfo.InvariantCulture).IndexOf(" clone") - 4)
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Friend ReadOnly Property Clone As String
        Get
            Try
                Return RCG.ToLower(CultureInfo.InvariantCulture).Substring(RCG.ToLower(CultureInfo.InvariantCulture).IndexOf("clone:") + 6, RCG.ToLower(CultureInfo.InvariantCulture).IndexOf(" gen") - (RCG.ToLower(CultureInfo.InvariantCulture).IndexOf("clone:") + Len("clone:")))
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Friend ReadOnly Property Gen As String
        Get
            Try
                Return RCG.ToLower(CultureInfo.InvariantCulture).Substring(RCG.IndexOf("gen:") + 4)
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
#End Region
    Friend Property ID As String = ""
    Friend Property unit As String = ""
    Friend Property CS As String = ""
    Friend Property WS As String = ""
    Private strSlot As String = ""
    Friend Property Slot As String
        Get
            Return strSlot
        End Get
        Set(value As String)
            strSlot = value
        End Set
    End Property
    Friend Property HW As String = ""
    Friend Property Percentage As String = ""
    Private strCoreStatus As String = ""
    Friend Property CoreStatus As String
        Get
            Return strCoreStatus
        End Get
        Set(value As String)
            strCoreStatus = value
        End Set
    End Property
    Friend Property Credit As String = ""
    Friend Property PPD As String = ""
    Friend Property ServerResponce As String = ""
    Friend Property CoreSnippet As String = ""
    Friend Property CoreVersion As String = ""
    Friend Property CoreCompiler As String = ""
    Friend Property Core As String = ""
    Friend Property BoardType As String = ""
#End Region
#Region "Date time objects"
    'Private mDTLastUploadAttempt As DateTime = #1/1/2000#
    'Friend Property LastUploadAttempt As DateTime
    '    Set(value As DateTime)
    '        mDTLastUploadAttempt = New DateTime(value.Ticks, DateTimeKind.Utc)
    '    End Set
    '    Get
    '        If modMySettings.ConvertUTC Then
    '            Return TimeZoneInfo.ConvertTimeFromUtc(mDTLastUploadAttempt, TimeZoneInfo.Local)
    '        Else
    '            Return mDTLastUploadAttempt
    '        End If
    '    End Get
    'End Property
    'Friend ReadOnly Property utcLastuploadAttempt As DateTime
    '    Get
    '        Return mDTLastUploadAttempt
    '    End Get
    'End Property
    Private mDTStartDownload As DateTime = #1/1/2000#
    Friend ReadOnly Property utcStartDownload As DateTime
        Get
            Return mDTStartDownload
        End Get
    End Property
    Private mDTDownloaded As DateTime = #1/1/2000#
    Friend ReadOnly Property utcDownloaded As DateTime
        Get
            Return mDTDownloaded
        End Get
    End Property
    Private mDTStartUpload As DateTime = #1/1/2000#
    Friend ReadOnly Property utcStartUpload As DateTime
        Get
            Return mDTStartUpload
        End Get
    End Property
    Private mDTSubmitted As DateTime = #1/1/2000#
    Friend ReadOnly Property utcSubmitted As DateTime
        Get
            Return mDTSubmitted
        End Get
    End Property
    Private mDTStarted As DateTime = #1/1/2000#
    Friend ReadOnly Property utcStarted As DateTime
        Get
            Return mDTStarted
        End Get
    End Property
    Private mDTCompleted As DateTime = #1/1/2000#
    Friend ReadOnly Property utcCompleted As DateTime
        Get
            Return mDTCompleted
        End Get
    End Property
    Friend Property dtStartUpload As DateTime
        Get
            If mDTStartUpload = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                'Dim dtLocal As DateTime = TimeZoneInfo.ConvertTime(Last_Update_CST, System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"), TimeZoneInfo.Local)
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTStartUpload, TimeZoneInfo.Local)
                Return mDTStartUpload.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTStartUpload
            End If
        End Get
        Set(value As DateTime)
            mDTStartUpload = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
    Friend Property dtStartDownload As DateTime
        Get
            If mDTStartDownload = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTStartDownload, TimeZoneInfo.Local)
                Return mDTStartDownload.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTStartDownload
            End If
        End Get
        Set(value As DateTime)
            mDTStartDownload = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
    Friend Property dtDownloaded As DateTime
        Get
            If mDTDownloaded = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTDownloaded, TimeZoneInfo.Local)
                Return mDTDownloaded.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTDownloaded
            End If
        End Get
        Set(value As DateTime)
            mDTDownloaded = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
    Friend Property dtStarted As DateTime
        Get
            If mDTStarted = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTStarted, TimeZoneInfo.Local)
                Return mDTStarted.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTStarted
            End If
        End Get
        Set(value As DateTime)
            mDTStarted = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
    Friend Property dtCompleted As DateTime
        Get
            If mDTCompleted = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTCompleted, TimeZoneInfo.Local)
                Return mDTCompleted.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTCompleted
            End If
        End Get
        Set(value As DateTime)
            mDTCompleted = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
    Friend Property dtSubmitted As DateTime
        Get
            If mDTSubmitted = #1/1/2000# Then
                Return #1/1/2000#
            ElseIf modMySettings.ConvertUTC Then
                Return TimeZoneInfo.ConvertTimeFromUtc(mDTSubmitted, TimeZoneInfo.Local)
                Return mDTSubmitted.AddHours(ClientInfo.UTC_Offset)
            Else
                Return mDTSubmitted
            End If
        End Get
        Set(value As DateTime)
            mDTSubmitted = New DateTime(value.Ticks, DateTimeKind.Utc)
        End Set
    End Property
#End Region
#Region "Configuration/clientinfo/diagnostics"
    Private mSlotConfig As clsClientConfig.clsConfiguration.sSlot
    Friend Property SlotConfig As clsClientConfig.clsConfiguration.sSlot
        Get
            Return mSlotConfig
        End Get
        Set(value As clsClientConfig.clsConfiguration.sSlot)
            mSlotConfig = value
        End Set
    End Property
    Friend Sub SetNewSlotConfig(Slot As clsClientConfig.clsConfiguration.sSlot, configDT As DateTime)
        Dim dtNewConfig As DateTime = New DateTime(configDT.Ticks, DateTimeKind.Utc)
        AddLogLine(dtNewConfig, "---Slot configuration changed---")
        For Each DictionaryEntry In Slot.mArguments
            AddLogLine(dtNewConfig, DictionaryEntry.Key & " = " & DictionaryEntry.Value)
        Next
        AddLogLine(dtNewConfig, "---/Slot configuration changed---")
        mSlotConfig = Slot
    End Sub
    Friend ReadOnly Property ClientConfig As clsClientConfig.clsConfiguration
        'ClientConfig per wu is read back from DB 
        Get
            Try
                Return sqdata.ClientConfigBeforeDT(ClientName, mDTDownloaded)
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New clsClientConfig.clsConfiguration
            End Try
        End Get
    End Property
    Friend ReadOnly Property ClientInfo As clsClientInfo.FAHClientInfo
        Get
            Try
                Return sqdata.ClientInfoBeforeDT(ClientName, mDTDownloaded)
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New clsClientInfo.FAHClientInfo
            End Try
        End Get
    End Property
    Friend ReadOnly Property Diagnostics As String
        Get
            Try
                Return ""
                'Throw New NotImplementedException
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return ""
            End Try
        End Get
    End Property
#End Region
#Region "Upload size/speed"
    Friend UploadSize As String = ""
    Friend DownloadSize As String = ""
    Private _iUploadSize As Double = 0
    Private _iUploadSpeed As Double = 0
    Private _iDownloadSize As Double = 0
    Private _iDownloadSpeed As Double = 0
    Friend ReadOnly Property tsUpload As TimeSpan
        Get
            Try
                If mDTSubmitted = #1/1/2000# Or mDTStartUpload = #1/1/2000# Then
                    Return New TimeSpan(0)
                Else
                    Return mDTSubmitted.Subtract(mDTStartUpload)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property tsDownload As TimeSpan
        Get
            Try
                If mDTStartDownload = #1/1/2000# Or mDTDownloaded = #1/1/2000# Then
                    Return New TimeSpan(0)
                Else
                    Return mDTDownloaded.Subtract(mDTStartDownload)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend Property iUploadSize As Double 'Always in bytes ( changed from KiB )
        Get
            Try
                If _iUploadSize <> 0 Then Return _iUploadSize
                If UploadSize = "" Then Return 0
                If UploadSize.Contains("KiB") Then
                    If Not CDbl(CDbl(UploadSize.Replace("KiB", "")) * 1024).ToString.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) Then
                        _iUploadSize = CDbl(CDbl(UploadSize.Replace("KiB", "")) * 1024)
                    Else
                        _iUploadSize = CDbl((CDbl(UploadSize.Replace("KiB", "")) * 1024).ToString.Substring(0, (CDbl(UploadSize.Replace("KiB", "")) * 1024).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))))
                    End If
                    Return _iUploadSize
                ElseIf UploadSize.Contains("MiB") Then
                    If Not (CDbl(UploadSize.Replace("MiB", "")) * 1024 * 1024).ToString.Contains((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) Then
                        _iUploadSize = CDbl(UploadSize.Replace("MiB", "")) * 1024 * 1024
                    Else
                        _iUploadSize = CDbl((CDbl(UploadSize.Replace("MiB", "")) * 1024 * 1024).ToString.Substring(0, (CDbl(UploadSize.Replace("MiB", "")) * 1024 * 1024).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))))
                    End If
                    Return _iUploadSize
                ElseIf UploadSize.Contains("B") Then
                    If Not CDbl(UploadSize.Replace("B", "")).ToString.Contains((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) Then
                        _iUploadSize = CDbl(UploadSize.Replace("B", ""))
                    Else
                        _iUploadSize = CDbl(CDbl(UploadSize.Replace("B", "")).ToString.Substring(0, CDbl(UploadSize.Replace("B", "")).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))).ToString)
                    End If
                    Return _iUploadSize
                End If
                Return 0
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(value As Double)
            _iUploadSize = value
        End Set
    End Property
    Friend Property iDownloadSize As Double
        Get
            Try
                If _iDownloadSize <> 0 Then Return _iDownloadSize
                If DownloadSize = "" Then Return 0
                If DownloadSize.Contains("KiB") Then
                    If Not CDbl(CDbl(DownloadSize.Replace("KiB", "")) * 1024).ToString.Contains((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) Then
                        _iDownloadSize = CDbl(CDbl(DownloadSize.Replace("KiB", "")) * 1024)
                    Else
                        _iDownloadSize = CDbl((CDbl(DownloadSize.Replace("KiB", "")) * 1024).ToString.Substring(0, (CDbl(DownloadSize.Replace("KiB", "")) * 1024).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))))
                    End If
                    Return _iDownloadSize
                ElseIf DownloadSize.Contains("MiB") Then
                    If Not (CDbl(DownloadSize.Replace("MiB", "")) * 1024 * 1024).ToString.Contains((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) Then
                        _iDownloadSize = CDbl(DownloadSize.Replace("MiB", "")) * 1024 * 1024
                    Else
                        _iDownloadSize = CDbl((CDbl(DownloadSize.Replace("MiB", "")) * 1024 * 1024).ToString.Substring(0, (CDbl(DownloadSize.Replace("MiB", "")) * 1024 * 1024).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))))
                    End If
                    Return _iDownloadSize
                ElseIf DownloadSize.Contains("B") Then
                    If Not CDbl(DownloadSize.Replace("B", "")).ToString.Contains((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)) Then
                        _iDownloadSize = CDbl(DownloadSize.Replace("B", ""))
                    Else
                        _iDownloadSize = CDbl(CDbl(DownloadSize.Replace("B", "")).ToString.Substring(0, CDbl(DownloadSize.Replace("B", "")).ToString.IndexOf((CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))).ToString)
                    End If
                    Return _iDownloadSize
                End If
                Return 0
            Catch ex As Exception
                Return 0
            End Try
        End Get
        Set(value As Double)
            _iDownloadSize = value
        End Set
    End Property
    Friend Property iUploadSpeed As Double 'Always xxxxxB/s ( format string when displaying )
        Get
            Try
                'Check
                If _iUploadSpeed <> 0 Then Return _iUploadSpeed
                If iUploadSize = 0 Then Return 0
                Dim tsUpload As TimeSpan = dtSubmitted.Subtract(dtStartUpload)
                If tsUpload.TotalSeconds < 1 Then tsUpload.Add(New TimeSpan(0, 0, 1))
                Return iUploadSize / tsUpload.TotalSeconds
            Catch ex As Exception
                WriteLog("Couldn't calculate upload speed.")
                Return iUploadSize
            End Try
        End Get
        Set(value As Double)
            _iUploadSpeed = value
        End Set
    End Property
    Friend ReadOnly Property UploadSpeed As String
        Get
            Try
                If dtSubmitted = dtStartUpload Then
                    Return (UploadSize & "/s")
                ElseIf iUploadSpeed < 1024 Then
                    Return Math.Round(iUploadSpeed, 2).ToString & "B/s"
                ElseIf iUploadSpeed > 1024 And iUploadSpeed < 1024 * 1024 Then
                    Return Math.Round(iUploadSpeed / 1024, 2).ToString & "KiB/s"
                Else
                    Return Math.Round(iUploadSpeed / 1024 / 1024, 2).ToString & "MiB/s"
                End If
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    Friend Property iDownloadSpeed As Double 'Always xxxxxB/s ( format string when displaying )
        Get
            Try
                'Check
                If _iDownloadSpeed <> 0 Then Return _iDownloadSpeed
                If iUploadSize = 0 Then Return 0
                Dim tsUpload As TimeSpan = dtSubmitted.Subtract(dtStartUpload)
                If tsUpload.TotalSeconds < 1 Then tsUpload.Add(New TimeSpan(0, 0, 1))
                Return iUploadSize / tsUpload.TotalSeconds
            Catch ex As Exception
                WriteLog("Couldn't calculate upload speed.")
                Return iUploadSize
            End Try
        End Get
        Set(value As Double)
            _iDownloadSpeed = value
        End Set
    End Property
    Friend ReadOnly Property DownloadSpeed As String
        Get
            Try
                If dtSubmitted = dtStartUpload Then
                    Return (UploadSize & "/s")
                ElseIf iDownloadSpeed < 1024 Then
                    Return Math.Round(iDownloadSpeed, 2).ToString & "B/s"
                ElseIf iDownloadSpeed > 1024 And iDownloadSpeed < 1024 * 1024 Then
                    Return Math.Round(iDownloadSpeed / 1024, 2).ToString & "KiB/s"
                Else
                    Return Math.Round(iDownloadSpeed / 1024 / 1024, 2).ToString & "MiB/s"
                End If
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
#End Region
#Region "FrameInfo"
    Friend Class clsFrame
        Private dtFrame As DateTime
        Private strFrame As String
        Friend Sub New(ByVal Line As String, ByVal FrameDT As DateTime)
            Try
                strFrame = Line
                dtFrame = New DateTime(FrameDT.Ticks, DateTimeKind.Utc)
            Catch ex As Exception

            End Try
        End Sub
        Friend ReadOnly Property LogString As String
            Get
                Return strFrame
            End Get
        End Property
        Friend ReadOnly Property strPercentage As String
            Get
                Dim rVal As String = ""
                For xIP As Int32 = strFrame.IndexOf("%") To 1 Step -1
                    If strFrame.Substring(xIP, 1) = " " Or strFrame.Substring(xIP, 1) = "(" Then
                        rVal = strFrame.Substring(xIP, strFrame.IndexOf("%") - xIP).Trim.Replace("(", "").Trim
                        Exit For
                    End If
                Next
                Return rVal
            End Get
        End Property
        Friend ReadOnly Property Percentage As Int32
            Get
                Return Integer.Parse(strPercentage)
            End Get
        End Property
        Public ReadOnly Property utcFrame As DateTime
            Get
                Return dtFrame
            End Get
        End Property
        Friend ReadOnly Property FrameDT As DateTime
            Get
                Try
                    If modMySettings.ConvertUTC Then
                        Return TimeZoneInfo.ConvertTimeFromUtc(dtFrame, TimeZoneInfo.Local)
                    Else
                        Return dtFrame
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return #1/1/2000#
                End Try
            End Get
        End Property
    End Class
    Private lstFrames As New Dictionary(Of String, clsFrame)
    Friend ReadOnly Property activeFrame As clsFrame
        Get
            If Frames.Count > 0 Then
                Return Frames(Frames.Count - 1)
            Else
                Return New clsFrame("invalid 0%", #1/1/2000#)
            End If
        End Get
    End Property
    Friend Function AddFrame(ByVal Frame As String, ByVal dtFrame As DateTime) As Boolean
        Dim rVal As Boolean = False
        Try
            If lstFrames.Keys.Contains(Frame) Then Exit Try
            Dim nFrame As New clsFrame(Frame, dtFrame)
            'Not all wu's print 0%, so I'll be using wu.dtStarted to wu.frames(0) to wu.frames(99)
            If nFrame.Percentage = 0 Then
                Exit Try
            End If
            For Each sFrame In lstFrames.Values
                If sFrame.Percentage = nFrame.Percentage Then Exit Try
            Next
            lstFrames.Add(Frame, nFrame)
            rVal = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
        Return rVal
    End Function
    Friend ReadOnly Property Frames As List(Of clsFrame)
        Get
            Return lstFrames.Values.ToList
        End Get
    End Property
    Friend Sub ClearFrames(Optional iPercentage As Int32 = -1)
        Try
            If iPercentage = -1 Then
                lstFrames.Clear()
            Else
                Dim lKeys As New List(Of String)
                For xInt As Int32 = iPercentage To lstFrames.Count - 1
                    lKeys.Add(lstFrames.Keys(xInt))
                Next
                For Each Key In lKeys
                    lstFrames.Remove(Key)
                Next
            End If
        Catch ex As Exception
            WriteLog("Clearing frames has failed! " & iPercentage & " - " & Frames(Frames.Count - 1).strPercentage, eSeverity.Critical)
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property tsTPF_cleaned As TimeSpan
        Get
            Try
                If Frames.Count > 0 Then
                    Dim tsFrame As New TimeSpan, iCount As Int32 = 0
                    Dim dLower As Double = tsTPF.TotalSeconds * 0.5, dUpper As Double = tsTPF.TotalSeconds * 1.05 ' 10% swing allowed?
                    For xInt As Int32 = Frames.Count - 1 To 1 Step -1
                        'Only count subsequent frames
                        If Frames(xInt).Percentage = Frames(xInt - 1).Percentage + 1 Then
                            Dim tsNew As TimeSpan = Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT)
                            If tsNew.TotalSeconds > dLower And tsNew.TotalSeconds < dUpper Then
                                tsFrame += tsNew
                                iCount += 1
                            End If
                        End If
                    Next
                    Return New TimeSpan(CLng(tsFrame.Ticks / iCount))
                Else
                    Return New TimeSpan(0)
                End If
            Catch ex As Exception
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property tsTPF As TimeSpan
        Get
            Try
                If Frames.Count > 0 Then
                    If Frames.Count = 1 Then
                        Return Frames(0).utcFrame.Subtract(utcStarted)
                    Else
                        Dim tsTmp As TimeSpan, tsFrame As New TimeSpan, iCount As Int32 = 0
                        For xInt As Int32 = Frames.Count - 1 To 1 Step -1
                            'only count subsequent frames 
                            If Frames(xInt).Percentage = Frames(xInt - 1).Percentage + 1 Then
                                tsFrame = tsFrame.Add(Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT))
                                iCount += 1
                            End If
                        Next
                        tsTmp = New TimeSpan(CLng(tsFrame.Ticks / iCount))
                        tsFrame = Nothing
                        Return tsTmp
                    End If
                Else
                    Return New TimeSpan(0)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property tsTPF_Max(Optional Margin As Int32 = 0) As TimeSpan
        Get
            Try
                If Frames.Count > 1 Then
                    Dim rVal As TimeSpan = TimeSpan.MinValue, rVal2 As TimeSpan = TimeSpan.MinValue
                    Dim tsAvg As TimeSpan = tsTPF
                    Dim marginUpper As Double = tsAvg.TotalSeconds + ((tsAvg.TotalSeconds / 100) * Margin)
                    Dim marginLower As Double = tsAvg.TotalSeconds - ((tsAvg.TotalSeconds / 100) * Margin)
                    If bTpfTestExt Then
#If CONFIG = "Debug" Then
                        If Not bNoConsole Then Console.WriteLine("-- Avg:" & vbTab & tsAvg.ToString & vbTab & "marginL: " & CStr(marginLower) & vbTab & "marginU: " & CStr(marginUpper))
#End If
                    End If
                    For xInt As Int32 = Frames.Count - 1 To 1 Step -1
                        'only use subsequent frames
                        If Frames(xInt).Percentage = Frames(xInt - 1).Percentage + 1 Then
                            Dim Duration As TimeSpan = Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT).Duration
                            If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                If Not bNoConsole Then Console.WriteLine("-- Frame: " & Duration.TotalSeconds.ToString & " seconds" & vbTab & "rVal: " & rVal.ToString & vbTab & "rVal2: " & rVal2.ToString)
#End If
                            End If
                            If Duration > rVal Then
                                If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                    If Not bNoConsole Then Console.WriteLine("-- Frame longer then rVal, rval: " & rVal.ToString & " rVal2: " & rVal2.ToString & " frame: " & Duration.ToString & vbTab & "margin: " & Margin)
#End If
                                End If
                                'check if rval2 should be replaced, can only happen if rVal2 is empty
                                If (rVal.TotalSeconds >= marginLower) And (rVal.TotalSeconds <= marginUpper) And rVal > rVal2 Then
                                    If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                        If Not bNoConsole Then Console.WriteLine("---- Previous rVal falls in margin and is shorter then rVal2, replacing rVal2")
#End If
                                    End If
                                    rVal2 = rVal
                                End If
                                rVal = Duration
                            End If
                            If (Not Duration = rVal) And (Duration < rVal2) And (Duration.TotalSeconds >= marginLower) And (Duration.TotalSeconds <= marginUpper) Then
                                If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                    If Not bNoConsole Then Console.WriteLine("-- Frame shorter then rVal2, rval: " & rVal.ToString & " rVal2: " & rVal2.ToString & " frame: " & Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT).Duration.ToString & vbTab & "margin: " & Margin)
                                    If Not bNoConsole Then Console.WriteLine("-- setting rVal2: " & Duration.ToString)
#End If
                                End If
                                rVal2 = Duration
                            End If
                        End If
                    Next
                    If Margin = 0 Then
                        If bTpfTestExt Then
#If CONFIG = "Debug" Then
                            If Not bNoConsole Then Console.WriteLine("-- Margin: 0")
                            If Not bNoConsole Then Console.WriteLine("-- returning rVal ( " & rVal.TotalSeconds.ToString & " )")
#End If
                        End If
                        Return rVal
                    Else
                        'margin is in reference to tpf avg
                        If rVal2 = TimeSpan.MinValue Then
                            If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                If Not bNoConsole Then Console.WriteLine("-- rVal2 empty, returning rVal")
#End If
                            End If
                            Return rVal
                        Else
                            If bTpfTestExt Then
#If CONFIG = "Debug" Then
                                If Not bNoConsole Then Console.WriteLine("-- returning rVal2 ( " & rVal2.TotalSeconds.ToString & " )")
#End If
                            End If
                            Return rVal2
                        End If
                    End If
                Else
                    If Frames.Count = 1 Then
                        Return Frames(0).FrameDT.Subtract(dtStarted)
                    Else
                        Return New TimeSpan(0)
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property TPFmax(Optional Margin As Int32 = 0) As String
        Get
            Return FormatTimeSpan(tsTPF_Max(Margin))
        End Get
    End Property
    Friend ReadOnly Property tsTPF_Min As TimeSpan
        Get
            Try
                If Frames.Count > 1 Then
                    Dim rVal As TimeSpan = TimeSpan.MaxValue
                    For xInt As Int32 = Frames.Count - 1 To 1 Step -1
                        'Only use subsequent frames 
                        If Frames(xInt).Percentage = Frames(xInt - 1).Percentage + 1 Then
                            If Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT).Duration < rVal Then rVal = Frames(xInt).FrameDT.Subtract(Frames(xInt - 1).FrameDT).Duration
                        End If
                    Next
                    Return rVal
                Else
                    If Frames.Count = 1 Then
                        Return Frames(0).FrameDT.Subtract(dtStarted)
                    Else
                        Return New TimeSpan(0)
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property TPFmin As String
        Get
            Return FormatTimeSpan(tsTPF_Min)
        End Get
    End Property
    Public ReadOnly Property tpfVariance As String
        Get
            Return FormatTimeSpan(tsTpfVariance)
        End Get
    End Property
    Public ReadOnly Property tsTpfVariance As TimeSpan
        Get
            Return (-tsTPF_Min + tsTPF_Max).Duration
        End Get
    End Property
    Friend ReadOnly Property TPF As String
        Get
            Try
                If tsTPF.TotalSeconds > 0 Then
                    Return FormatTimeSpan(tsTPF)
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return ""
            End Try
        End Get
    End Property
    Friend Property tpfDB As String = ""
    Friend Property tpfMinDB As String = ""
    Friend Property tpfMaxDB As String = ""
    Friend ReadOnly Property tsTPFMaxDB As TimeSpan
        Get
            If tpfMaxDB = "" Then Return New TimeSpan(0)
            Try
                Return TimeSpan.Parse(tpfMaxDB)
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
    Friend ReadOnly Property tsTPFMinDB As TimeSpan
        Get
            If tpfMinDB = "" Then Return New TimeSpan(0)
            Try
                Return TimeSpan.Parse(tpfMinDB)
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New TimeSpan(0)
            End Try
        End Get
    End Property
#End Region
#Region "Restarted work unit info"
    Friend bHasRestarted As Boolean = False
    Friend Class clsRestarted
        Implements IDisposable
        Friend Class clsRInfo
            Private _CoreStatus As String = ""
            Friend ReadOnly Property CoreStatus As String
                Get
                    Return _CoreStatus
                End Get
            End Property
            Private _LastFrame As clsFrame
            Friend ReadOnly Property LastFrame As clsFrame
                Get
                    Return _LastFrame
                End Get
            End Property
            Friend Sub New(ByVal CoreStatus As String, ByVal Frame As clsFrame)
                _CoreStatus = CoreStatus : _LastFrame = Frame
            End Sub
        End Class
        Private dRestartInfo As New Dictionary(Of DateTime, clsRInfo)
        Friend ReadOnly Property RestartInfo As List(Of clsRInfo)
            Get
                Return dRestartInfo.Values.ToList
            End Get
        End Property
        Friend Sub AddRestart(ByVal CoreStatus As String, ByVal Frame As clsFrame)
            Try
                If Not dRestartInfo.ContainsKey(Frame.FrameDT) Then
                    dRestartInfo.Add(Frame.FrameDT, New clsRInfo(CoreStatus, Frame))
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    dRestartInfo = Nothing
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
    Friend restartInfo As New clsRestarted
#End Region
#Region "Work unit Report"
    Friend ReadOnly Property Report As String
        Get
            Dim sb As New StringBuilder
            sb.AppendLine("PRCG: " & vbTab & vbTab & PRCG)
            sb.AppendLine("unit: " & vbTab & vbTab & unit)
            sb.AppendLine("client: " & vbTab & vbTab & ClientName)
            sb.AppendLine("Slot: " & vbTab & vbTab & Slot)
            sb.AppendLine("Hardware: " & vbTab & HW)
            sb.AppendLine("Core: " & vbTab & vbTab & Core)
            sb.AppendLine("Corestatus: " & vbTab & CoreStatus)
            If CoreCompiler <> "" Then sb.AppendLine("Compiler: " & vbTab & vbTab & CoreCompiler)
            If BoardType <> "" Then sb.AppendLine("BoardType: " & vbTab & BoardType)
            sb.AppendLine("Downloaded: " & vbTab & dtDownloaded.ToString(CultureInfo.CurrentCulture))
            sb.AppendLine("Download size:" & vbTab & DownloadSize)
            sb.AppendLine("Download speed:" & vbTab & DownloadSpeed)
            sb.AppendLine("Completed: " & vbTab & dtCompleted.ToString(CultureInfo.CurrentCulture))
            sb.AppendLine("Submitted: " & vbTab & dtSubmitted.ToString(CultureInfo.CurrentCulture))
            sb.AppendLine("Upload size: " & vbTab & UploadSize)
            sb.AppendLine("Upload speed: " & vbTab & UploadSpeed)
            sb.AppendLine("ServerResponce: " & vbTab & ServerResponce)
            If Credit <> "" Then sb.AppendLine("Credit: " & vbTab & vbTab & Credit)
            If PPD <> "" Then sb.AppendLine("PPD: " & vbTab & vbTab & PPD)
            If tpfDB <> "" Then
                sb.AppendLine("Avg TPF: " & vbTab & tpfDB)
            Else
                If TPF <> "" Then sb.AppendLine("Avg TPF: " & vbTab & FormatTimeSpan(TimeSpan.Parse(TPF)))
            End If
            If tpfMinDB <> "" Then
                sb.AppendLine("Min TPF: " & vbTab & vbTab & tpfMinDB)
            Else
                If tsTPF_Min.TotalSeconds > 0 Then sb.AppendLine("Min TPF: " & vbTab & vbTab & TPFmin)
            End If
            If tpfMaxDB <> "" Then
                sb.AppendLine("Max TPF: " & vbTab & tpfMaxDB)
            Else
                If tsTPF_Max.TotalSeconds > 0 Then sb.AppendLine("Max TPF: " & vbTab & TPFmax)
            End If
            sb.AppendLine("WorkServer: " & vbTab & WS)
            If CS <> "" And CS <> WS Then sb.AppendLine("CollectionServer: " & vbTab & CS)
            If bHasRestarted Then
                sb.AppendLine("")
                For Each rInfo As clsRestarted.clsRInfo In Me.restartInfo.RestartInfo
                    If IsNothing(rInfo.LastFrame) Then
                        sb.AppendLine("Work unit has been restarted with status: " & rInfo.CoreStatus)
                    Else
                        sb.AppendLine("Work unit has been restarted at " & rInfo.LastFrame.strPercentage & " with status: " & rInfo.CoreStatus)
                    End If
                Next
            End If
            Return sb.ToString
        End Get
    End Property
#End Region
#Region "Properties for active work units"
#Region "Log"
    Friend Function ConvertSnippetToActiveLog() As Boolean
        Try
            If CoreSnippet.Length = 0 Then Return False
            Dim lines() As String = CoreSnippet.Split({Environment.NewLine, Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            Dim logDT As DateTime = utcStartDownload
            Dim iIndex As Int32 = 0
            'check lines from 0 to start of download for day change!
            For xInt As Int32 = 0 To lines.Count
                If lines(xInt).Contains("FW7:---/Slot configuration---") Then
                    If lines(xInt + 1).Contains("Received Unit") Then
                        'new log type, FSxx:WUxx
                        logDT = utcDownloaded
                        If ID = "" Then
                            If lines(xInt + 1).Contains("id:") AndAlso lines(xInt + 1).Contains("state:") Then ID = lines(xInt + 1).Substring(lines(xInt + 1).IndexOf("id:") + 3, lines(xInt + 1).IndexOf("state:") - (lines(xInt + 1).IndexOf("id:") + 3)).Trim
                        End If
                    Else
                        'old log type, UNIT xx:
                        logDT = utcStarted
                        If ID = "" Then
                            If Regex.IsMatch(lines(xInt + 1), "[uU][nN][iI][tT][ ]") Then ID = lines(xInt + 1).Substring(lines(xInt + 1).ToUpperInvariant.IndexOf("UNIT ") + Len("UNIT "), 2)
                        End If
                    End If
                    Exit For
                End If
            Next
            For xInt As Int32 = 0 To lines.Count - 1
                Dim Line As String = lines(xInt)
                Dim bDateChange As Boolean = False
                If Line.Contains("FW7:Log started:") Then
                    Try
                        Dim tmpStr As String = Line.Substring(9).Replace("FW7:Log started:", "").Trim(CChar(" "))
                        If DateTime.TryParse(tmpStr, logDT) Then
                            AddLogLine(logDT.Date.Add(TimeSpan.Parse(Line.Substring(0, 8))), Line)
                            bDateChange = True
                        Else
                            WriteLog("Converting CoreSnippet to active log failed!", eSeverity.Critical)
                            Return False
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                ElseIf Line.Contains("FW7:Date changed:") Then
                    Try
                        Dim tmpStr As String = Line.Substring(9).Replace("FW7:Date changed:", "").Trim(Chr(34))
                        If DateTime.TryParse(tmpStr, logDT) Then
                            AddLogLine(logDT.Date.Add(TimeSpan.Parse(Line.Substring(0, 8))), Line)
                            bDateChange = True
                        Else
                            WriteLog("Converting CoreSnippet to active log failed!", eSeverity.Critical)
                            Return False
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                Else
                    If xInt > 0 AndAlso Not bDateChange Then
                        If Regex.IsMatch(lines(xInt - 1), "^\d{2}[:]\d{2}[:]\d{2}[:]") AndAlso Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:]") Then
                            Try
                                Dim tsLine As TimeSpan = TimeSpan.Parse(Line.Substring(0, 8))
                                Dim tsPrev As TimeSpan = TimeSpan.Parse(lines(xInt - 1).Substring(0, 8))
                                If tsPrev > tsLine Then
                                    logDT = logDT.Date.AddDays(1)
                                    AddLogLine(logDT.Date.Add(tsLine), Line.Substring(0, 9) & "FW7:Date changed: " & logDT.Date.ToLongDateString)
                                End If
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                Return False
                            End Try
                        End If
                    End If
                    If Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:]") Then
                        AddLogLine(logDT.Date.Add(TimeSpan.Parse(Line.Substring(0, 8))), Line)
                    Else
                        '
                        If Line.Contains("Slot configuration") Then
                            'find last 
                            Dim tsNextLine As TimeSpan = Nothing
                            Dim lConfig As New List(Of String)
                            For zInt As Int32 = xInt To lines.Count - 1
                                If Regex.IsMatch(lines(zInt), "^\d{2}[:]\d{2}[:]\d{2}[:]") Then
                                    tsNextLine = TimeSpan.Parse(lines(zInt).Substring(0, 8))
                                    xInt = zInt
                                    Exit For
                                Else
                                    lConfig.Add(lines(zInt))
                                End If
                            Next
                            If IsNothing(tsNextLine) Then
                                WriteLog("Converting CoreSnippet to active log failed!", eSeverity.Critical)
                                Return False
                            Else
                                For Each cLine As String In lConfig
                                    AddLogLine(logDT.Date.Add(tsNextLine), FormatTimeSpan(logDT.Date.Add(tsNextLine).TimeOfDay) & cLine)
                                Next
                            End If
                        Else
                            WriteLog("Converting CoreSnippet to active log failed!", eSeverity.Critical)
                            Return False
                        End If
                    End If
                End If
            Next
            'CoreSnippet = ""
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Activelog As New Dictionary(Of String, DateTime)
    Friend ReadOnly Property dictLog As Dictionary(Of String, DateTime)
        Get
            Return Activelog
        End Get
    End Property
    Friend Sub AddLogLine(utcLineDT As DateTime, LogLine As String)
        Try
            If Not Activelog.ContainsKey(LogLine) Then
                If Not utcLineDT.Kind = DateTimeKind.Utc Then
                    Activelog.Add(LogLine, New DateTime(utcLineDT.Ticks, DateTimeKind.Utc))
                Else
                    Activelog.Add(LogLine, utcLineDT)
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub ClearLog()
        Activelog.Clear()
    End Sub
    Friend ReadOnly Property ActiveLogfileUTC(Optional IncludeWarningAndErrors As Boolean = False, Optional IncludeOtherWUs As Boolean = False) As String
        Get
            Try
                Dim sb As New StringBuilder
                For Each Line As String In Activelog.Keys
                    If Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(Line, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                        If IncludeWarningAndErrors Then
                            If (Regex.IsMatch(Line, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(Line, "[W][U][" & Slot.Substring(0, 1) & "][" & Slot.Substring(1, 1) & "]") Then
                                If IncludeOtherWUs Then sb.AppendLine(Line)
                            Else
                                sb.AppendLine(Line)
                            End If
                        End If
                    Else
                        If Regex.IsMatch(Line, "[W][U][0-9][0-9]") Then
                            If Regex.IsMatch(Line, "[W][U][" & ID.Substring(0, 1) & "][" & ID.Substring(1, 1) & "]") Then
                                sb.AppendLine(Line)
                            Else
                                If IncludeOtherWUs Then sb.AppendLine(Line)
                            End If
                        Else
                            sb.AppendLine(Line)
                        End If
                    End If
                Next
                Return sb.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return ""
            End Try
        End Get
    End Property
    Friend ReadOnly Property ActiveLogfile(Optional IncludeWarningAndErrors As Boolean = False, Optional IncludeOtherWUs As Boolean = False) As String
        Get
            Try
                Dim sb As New StringBuilder
                For Each DictionaryEntry In Activelog
                    If Regex.IsMatch(DictionaryEntry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][wW][aA][rR][nN][iI][nN][gG]") OrElse Regex.IsMatch(DictionaryEntry.Key, "^\d{2}[:]\d{2}[:]\d{2}[:][eE][rR][rR][oO][rR]") Then
                        If IncludeWarningAndErrors Then
                            If (Regex.IsMatch(DictionaryEntry.Key, "[W][U][0-9][0-9]")) And Not Regex.IsMatch(DictionaryEntry.Key, "[W][U][" & Slot.Substring(0, 1) & "][" & Slot.Substring(1, 1) & "]") Then
                                If IncludeOtherWUs Then
                                    Dim dtNow As TimeSpan = TimeZoneInfo.ConvertTimeFromUtc(DictionaryEntry.Value, TimeZoneInfo.Local).TimeOfDay
                                    sb.AppendLine(FormatTimeSpan(dtNow) & DictionaryEntry.Key.Substring(8))
                                End If
                            Else
                                Dim dtNow As TimeSpan = TimeZoneInfo.ConvertTimeFromUtc(DictionaryEntry.Value, TimeZoneInfo.Local).TimeOfDay
                                sb.AppendLine(FormatTimeSpan(dtNow) & DictionaryEntry.Key.Substring(8))
                            End If
                        End If
                    Else
                        If Regex.IsMatch(DictionaryEntry.Key, "[W][U][0-9][0-9]") Then
                            If Regex.IsMatch(DictionaryEntry.Key, "[W][U][" & ID.Substring(0, 1) & "][" & ID.Substring(1, 1) & "]") Then
                                Dim dtNow As TimeSpan = TimeZoneInfo.ConvertTimeFromUtc(DictionaryEntry.Value, TimeZoneInfo.Local).TimeOfDay
                                sb.AppendLine(FormatTimeSpan(dtNow) & DictionaryEntry.Key.Substring(8))
                            Else
                                If IncludeOtherWUs Then
                                    Dim dtNow As TimeSpan = TimeZoneInfo.ConvertTimeFromUtc(DictionaryEntry.Value, TimeZoneInfo.Local).TimeOfDay
                                    sb.AppendLine(FormatTimeSpan(dtNow) & DictionaryEntry.Key.Substring(8))
                                End If
                            End If
                        Else
                            Dim dtNow As TimeSpan = TimeZoneInfo.ConvertTimeFromUtc(DictionaryEntry.Value, TimeZoneInfo.Local).TimeOfDay
                            sb.AppendLine(FormatTimeSpan(dtNow) & DictionaryEntry.Key.Substring(8))
                        End If
                    End If
                Next
                Return sb.ToString
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return ""
            End Try
        End Get
    End Property
#End Region
    Friend ReadOnly Property IsActive As Boolean
        Get
            Return CBool(utcStarted <> #1/1/2000# AndAlso CoreStatus = "" AndAlso ServerResponce = "")
        End Get
    End Property
    Friend ReadOnly Property IsQueue As Boolean
        Get
            If utcStarted = #1/1/2000# Then
                'Queued for start
                Return True
            ElseIf Not IsActive And ServerResponce = "" Then
                'Queued for upload
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Friend ReadOnly Property Eta(Optional ForceUTC As Boolean = False) As DateTime
        Get
            Dim dtRet As DateTime = #1/1/2000#
            Try
                If Frames.Count = 0 Then Return dtRet
                Dim dtStart As DateTime = Frames(Frames.Count - 1).FrameDT
                Dim dtToGo As TimeSpan = TimeSpan.FromTicks(tsTPF.Ticks * (100 - Frames(Frames.Count - 1).Percentage))
                Return dtStart.Add(dtToGo)
                'frameDT already does the conversion if needed
                'If modMySettings.ConvertUTC Then
                '    Return TimeZoneInfo.ConvertTimeFromUtc(dtStart.Add(dtToGo), TimeZoneInfo.Local)
                '    'Return dtStart.Add(dtToGo).AddHours(ClientInfo.UTC_Offset)
                'Else
                '    Return dtStart.Add(dtToGo)
                'End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return dtRet
        End Get
    End Property
    Friend ReadOnly Property Worth As ProjectInfo.sProjectPPD
        Get
            Dim nPPD As New ProjectInfo.sProjectPPD
            Try
                If ProjectInfo.KnownProject(Project) Then nPPD = ProjectInfo.GetEffectivePPD_sqrt(dtDownloaded, Eta, Project)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return nPPD
        End Get
    End Property
#Region "FahCore path and arguments"
    Private mWorker As String = String.Empty
    Friend Property Worker As String
        Get
            Return mWorker
        End Get
        Set(value As String)
            mWorker = value
        End Set
    End Property
    Private mWorkerArgs As String = String.Empty
    Friend Property WorkerArguments As String
        Get
            Return mWorkerArgs
        End Get
        Set(value As String)
            mWorkerArgs = value
        End Set
    End Property
#End Region
#End Region
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                Me.lstFrames.Clear()
                Me.lstFrames = Nothing
                Me.Activelog.Clear()
                Me.Activelog = Nothing
                Me.restartInfo.Dispose()
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