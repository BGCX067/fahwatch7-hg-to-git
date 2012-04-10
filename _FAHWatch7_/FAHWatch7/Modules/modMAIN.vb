'   FAHWatch7   
'
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports HWInfo
'Imports gpuInfo.gpuInfo
Imports System.Runtime.InteropServices
Imports System.Text
Imports lspci
Imports System.Globalization
Imports FAHWatch7.pSummary
Imports FAHWatch7.ProjectInfo

Module modMAIN
#Region "Declarations"
    Friend dbFolder As String = ""
    Friend appContext As clsAppContext
    Friend bKeepBackup As Boolean = False
    Friend WithEvents LumberJack As New LumberJack.LumberJack
    Friend lFilesToRemove As New List(Of String)
    Friend bTpfTest As Boolean = False
    Friend bTpfTestExt As Boolean = False
    Friend bNoConsole As Boolean = True
#If CONFIG = "Debug" Then
    'Set to false unless you want to suffer performance.. or true if you're tracking some issue
    Friend DebugOutput As Boolean = False 'set to false for distributing debug build for verbose error tracking!
#Else
    Friend DebugOutput As Boolean = False
#End If
    Friend Const urlIssuesWithWU As String = "http://foldingforum.org/viewforum.php?f=19"
    Friend bTestForm As Boolean = False
    Friend simpleNetworkbrowser As Boolean = False
    Friend m_hwInfo As New HWInfo.clsHWInfo.cHWInfo
    Friend logwindow As New Logwindow
    Friend ClientConfig As New clsClientConfig
    Friend ClientInfo As New clsClientInfo
    'Friend ProjectInfo As New clsProjectInfo
    Friend EOC As New EOCInfo
    Friend mySettings As New modMySettings
    Friend Clients As New Clients
    Friend Live As New frmLive
    Friend sqdata As New Data
    Friend sqlFilters As New clsFilters
    Friend PerformanceStatistics As New clsStatistics.clsPerformanceStatistics
    Friend ProjectStatistics As New clsStatistics.clsProjectStatistics
    Friend HardwareStatistics As New clsStatistics.clsHardwareStatistics
    Friend HistoricalStatistics As New clsStatistics.clsHistoricalStatistics
    Friend about As New clsAbout
    Friend license As New clsLicense
    Friend zGraph As New clsZGRAPH
    Friend lsPCI As New lspci.clsPci
    Friend eLog As New clsEventLog
    Friend Splash As SplashScreen = Nothing
    Friend History As New frmHistory
    Friend bClosing As Boolean = False
    Friend ProjectInfo As New FAHWatch7.ProjectInfo
    Friend Enum eSeverity
        Critical
        Important
        Informative
        Debug
    End Enum
#End Region
#Region "Diagnostics and whitelist functions"
    Friend ReadOnly Property Diagnostic(Optional ByVal HidePassKey As Boolean = True, Optional ByVal Close As Boolean = False) As String
        Get
            WriteDebug("Diagnostic property accessed")
            Static strDiag As String = ""
            If strDiag <> "" Then
                WriteDebug("Returning previous diagnostic string")
                Return strDiag
            End If
            Dim sb As New StringBuilder
            Try
                If Not m_hwInfo.ohmInterface.IsOpen Then m_hwInfo.Init(LumberJack)
                sb.AppendLine("System diagnostic")
                If m_hwInfo.Territory.IsX64 Then
                    sb.AppendLine("Os: " & m_hwInfo.Territory.OsName & " (x64)")
                Else
                    sb.AppendLine("Os: " & m_hwInfo.Territory.OsName & " (x86)")
                End If
                Dim lString As String = Mid(m_hwInfo.ohmInterface.OHMReport, 1, m_hwInfo.ohmInterface.OHMReport.IndexOf("SMBIOS Table"))
                sb.AppendLine(lString)
                If Not IsNothing(m_hwInfo.ohmInterface.ADL) Then sb.Append(m_hwInfo.ohmInterface.ADL.Report)
                If Not IsNothing(m_hwInfo.ohmInterface.NVAPI) Then sb.Append(m_hwInfo.ohmInterface.NVAPI.Report)
                If Not IsNothing(m_hwInfo.gpuInf) Then sb.Append(m_hwInfo.gpuInf.TextReport)
                sb.AppendLine("Reading client configuration: " & ClientConfig.ReadFAHClientConfig(False, True).ToString)
                If Not IsNothing(ClientConfig.Configuration) Then sb.AppendLine(ClientConfig.Configuration.Report(HidePassKey))
                sb.AppendLine("Running -lspci: " & lsPCI.FillInfo().ToString)
                sb.AppendLine(lsPCI.Report)
                ClientInfo.Info = clsClientInfo.FAHClientInfo.Parse()
                sb.AppendLine("Client info:")
                sb.AppendLine(ClientInfo.Info.Report)
                If Close Then
                    sb.Append("Query event log: ")
                    If eLog.QueryEventLog Then
                        sb.Append("succes" & Environment.NewLine)
                        sb.AppendLine(eLog.Report)
                    Else
                        sb.AppendLine("Failed to find any warning or error entries.")
                    End If
                End If
                sb.AppendLine("Closing diagnostics")
                If Close Then
                    WriteDebug("Diagnosts finished, cleaning up")
                    m_hwInfo.ohmInterface.Close()
                    m_hwInfo.gpuInf = Nothing
                    m_hwInfo.ohmInterface = Nothing
                    m_hwInfo = Nothing
                Else
                    WriteDebug("Diagnosts finished")
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            strDiag = sb.ToString
            Return sb.ToString
        End Get
    End Property
    'Replace Diagnostics code with call to diagnostic?
    Friend Sub Diagnostics()
        WriteDebug("Starting Diagnostic")
        Splash = New SplashScreen
        Splash.ApplicationTitle.Text &= Environment.NewLine & "Diagnostics"
        Splash.Show()
        'Dim rVal As MsgBoxResult = MsgBox("Diagnostics output will contain your original Config.xml file, including your passkey. This can be helpfull, but brings the risk that you accidentally share your private passkey by posting the output on a forum. If you plan on posting the output to obtain help with a problem, the passkey can be checked on valid input and be hidden from the report." & Environment.NewLine & Environment.NewLine & "Do you want to hide the passkey in the report?", CType(vbQuestion & vbYesNo, MsgBoxStyle), "Hide your passkey?")
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Diagnostics.txt") Then
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Diagnostics.txt")
        End If
        Try
            Dim strDiag As String = Diagnostic(True, True)
            My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\Diagnostic.txt", strDiag, False)
            'logwindow.CreateLog(Application.StartupPath & "\Diagnostics.txt")
            'logwindow.WriteLog("Starting diagnostic run")
            'If Not m_hwInfo.ohmInterface.IsOpen Then m_hwInfo.Init(LumberJack)
            'If Not m_hwInfo.Territory.IsAdmin Then
            '    WriteDebug("No administrator rights, aborting diagnostics")
            '    MsgBox("Need full rights to run, administrator account windows xp and 'run as admin' on vista / windows 7.")
            '    logwindow.CloseLog()
            '    If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Diagnostics.txt") Then
            '        WriteDebug("Deleting Diagnostics.txt")
            '        My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Diagnostics.txt")
            '    End If
            '    m_hwInfo.Close()
            '    Exit Try
            'End If
            'If m_hwInfo.Territory.IsX64 Then
            '    logwindow.WriteLog("Os: " & m_hwInfo.Territory.OsName & " (x64)")
            'Else
            '    logwindow.WriteLog("Os: " & m_hwInfo.Territory.OsName & " (x86)")
            'End If
            'Dim lString As String = Mid(m_hwInfo.ohmInterface.OHMReport, 1, m_hwInfo.ohmInterface.OHMReport.IndexOf("SMBIOS Table"))
            'logwindow.WriteLog(lString)
            'If Not IsNothing(m_hwInfo.ohmInterface.ADL) Then logwindow.WriteLog(m_hwInfo.ohmInterface.ADL.Report)
            'If Not IsNothing(m_hwInfo.ohmInterface.NVAPI) Then logwindow.WriteLog(m_hwInfo.ohmInterface.NVAPI.Report)
            'If Not IsNothing(m_hwInfo.gpuInf) Then logwindow.WriteLog(m_hwInfo.gpuInf.TextReport)
            'logwindow.WriteLog("Reading client configuration: " & ClientConfig.ReadFAHClientConfig(False, True).ToString)
            'If Not IsNothing(ClientConfig.Configuration) Then logwindow.WriteLog(ClientConfig.Configuration.Report(rVal = vbYes))
            'logwindow.WriteLog("Running -lspci: " & lsPCI.FillInfo().ToString)
            'logwindow.WriteLog(lsPCI.Report)
            'clsClientInfo.FAHClientInfo.Parse()
            'logwindow.WriteLog("Client info:")
            'logwindow.WriteLog(ClientInfo.Info.Report)
            'logwindow.WriteLog("Query event log.. ")
            'If eLog.QueryEventLog Then
            '    logwindow.WriteLog(eLog.Report)
            'Else
            '    logwindow.WriteLog("Failed to find any warning or error entries.")
            'End If
            'logwindow.WriteLog("Closing diagnostics")
            'm_hwInfo.ohmInterface.Close()

            Dim mRes As MsgBoxResult = MsgBox("Do you want the report placed on the clipboard? It will always be saved to " & Application.StartupPath & "\Diagnostics.txt as well.", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Question, MsgBoxStyle), "Diagnostics finished")
            If mRes = vbYes Then Clipboard.SetText("[code]" & strDiag & "[/code]")
            'logwindow.CloseLog()
        Catch ex As Exception
            MsgBox(Err.Source & "-" & Err.Erl & ":" & Err.Description & Environment.NewLine & Err.GetException.StackTrace.ToString)
        End Try
        Splash.Close()
    End Sub
    Friend Sub WhiteList()
        Splash.Show()
        Splash.ApplicationTitle.Text &= Environment.NewLine & "Whitelist"
        Try
            Dim lsPCI As clsPci = Nothing
            Try
                logwindow.CreateLog("WhitelistMyGPU")
                lsPCI = New clsPci
                If lsPCI.FillInfo Then
                    WriteDebug("lsPCI filled info")
                    logwindow.WriteLog("Running --lspci: true")
                    logwindow.WriteLog(lsPCI.Report)
                    Dim rVal As MsgBoxResult = MsgBox("Do you want to place the report on the clipboard?" & Environment.NewLine & Environment.NewLine & "The report will always be saved to " & My.Application.Info.DirectoryPath & "\WhitelistMyGPU.txt.", CType(MsgBoxStyle.Question + MsgBoxStyle.OkCancel, MsgBoxStyle), Application.ProductName & " " & Application.ProductVersion.ToString)
                    If rVal = vbOK Then
                        Dim cbT As New StringBuilder
                        cbT.AppendLine("[code]")
                        cbT.Append(logwindow.Form.Log & "[/code]")
                        Clipboard.SetText(cbT.ToString)
                        cbT = Nothing
                        'I removed the browser part as people who need to run this probably already have a tab open.
                        'Process.Start("http://foldingforum.org/viewforum.php?f=67")
                    End If
                Else
                    WriteDebug("lsPCI failed to fill info")
                    MsgBox("Failed to generate a whitelist report", CType(vbOKOnly + vbCritical, MsgBoxStyle))
                End If
                logwindow.CloseLog()
            Finally
                If lsPCI IsNot Nothing Then lsPCI.Dispose()
            End Try
        Catch ex As Exception
            MsgBox(Err.Source & "-" & Err.Erl & ":" & Err.Description & Environment.NewLine & Err.GetException.StackTrace.ToString)
        End Try
        Splash.Close()
    End Sub
#End Region
#Region "Easy log access"
    Friend Sub WriteLog(ByVal message As String, Optional ByVal severity As eSeverity = eSeverity.Informative)
        Select Case severity
            Case eSeverity.Critical
                message = "CRITICAL: " & message
            Case eSeverity.Important
                message = "WARNING: " & message
            Case eSeverity.Informative
                message = message
            Case eSeverity.Debug
                message = "DEBUG: " & message
        End Select
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine(message)
#End If
        If Not IsNothing(logwindow) AndAlso Not IsNothing(logwindow.Form) AndAlso Not logwindow.Form.IsDisposed Then logwindow.WriteLog(message, severity)
    End Sub
    Friend Sub WriteDebug(ByVal message As String)
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine("DEBUG:" & message)
#End If
        If Not DebugOutput Then Exit Sub
        If Not IsNothing(logwindow) AndAlso Not IsNothing(logwindow.Form) AndAlso Not logwindow.Form.IsDisposed Then logwindow.WriteLog(message, eSeverity.Debug)
    End Sub
    Friend Sub WriteError(ByVal message As String, ByVal err As ErrObject)
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine("ERROR:" & message)
#End If
        If Not IsNothing(logwindow) AndAlso Not logwindow.Form.IsDisposed Then logwindow.WriteError(message, err)
    End Sub
#End Region
#Region "Accredit work unit"
    ' NOTE: needs an edit so a false return doesn't cause the logparser to fail, instead a table with 'missing' project info should be made ( can be crosschecked to recredit work unit's when downloading a new summary with the missing projects in it.
    Public Function AccreditWorkunit(ByVal WU As clsWU) As Boolean
        Dim rVal As Boolean = False
        Try
            If Not ProjectInfo.KnownProject(WU.Project) Then
                If Not sqdata.UnknownProject(WU.Project) Then
                    sqdata.InsertUnknownProject(WU.Project)
                    WriteLog(WorkUnitLogHeader(WU) & "Adding " & WU.Project & " to unknown project list", eSeverity.Important)
                End If
                Return True
            End If
            If WU.utcSubmitted.Subtract(WU.utcDownloaded).TotalSeconds < 0 Then
                If Not WU.utcDownloaded = #1/1/2000# AndAlso Not WU.utcSubmitted = #1/1/2000# Then
                    WriteLog(WorkUnitLogHeader(WU) & "Attempt to credit a work unit which has a negative completion timespan", eSeverity.Critical)
                Else
                    WriteDebug(WorkUnitLogHeader(WU) & "Can't accredit a work unit which has no completion time")
                End If
                'Return true to indicate no error has occured
                Return True
            End If
            'Check credit rating
            WriteDebug(WorkUnitLogHeader(WU) & "Starting wu credit check")
            WriteDebug(WorkUnitLogHeader(WU) & "Credit: " & WU.Credit & Chr(32) & "Ppd: " & WU.PPD)
            If WU.Credit = "" Or WU.Credit = "0" Then
                If FAHWatch7.ProjectInfo.KnownProject(WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:"))) And WU.CoreStatus.Contains("FINISHED_UNIT") And WU.ServerResponce.Contains("WORK_ACK") And Not WU.utcDownloaded = #1/1/2000# And Not WU.utcSubmitted = #1/1/2000# Then
                    'Only calculate if finished_unit and work_ack
                    Dim sPPD As sProjectPPD = FAHWatch7.ProjectInfo.GetEffectivePPD_sqrt(WU.utcDownloaded, WU.utcSubmitted, WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:")))
                    If sPPD.PPD = "-1" Then
                        WriteLog(WorkUnitLogHeader(WU) & "Failed to GetEffectivePPD_sqrt(" & WU.utcDownloaded.ToString(CultureInfo.CurrentCulture) & "," & WU.utcSubmitted.ToString(CultureInfo.CurrentCulture) & "," & WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:")), eSeverity.Important)
                        Return False
                    End If
                    WU.Credit = sPPD.Credit
                    WU.PPD = sPPD.PPD
                    WriteDebug(WorkUnitLogHeader(WU) & "Work unit credited to " & WU.Credit & " ppd: " & WU.PPD)
                End If
            ElseIf WU.PPD = "" And FAHWatch7.ProjectInfo.KnownProject(WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:"))) And WU.ServerResponce.Contains("WORK_ACK") And WU.Credit <> "" Then
                'If credit was given for partial results this will calculate PPD as well
                Dim sPPD As String = FAHWatch7.ProjectInfo.GetEffectivePPD_sqrt(WU.utcDownloaded, WU.utcSubmitted, WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:"))).PPD
                If sPPD = "-1" Then
                    WriteLog(WorkUnitLogHeader(WU) & "Failed to GetEffectivePPD_sqrt(" & WU.utcDownloaded.ToString(CultureInfo.CurrentCulture) & "," & WU.utcSubmitted.ToString(CultureInfo.CurrentCulture) & "," & WU.PRCG.Substring(Len("project:"), WU.PRCG.IndexOf(" ") - Len("project:")), eSeverity.Important)
                    Return False
                Else
                    WU.PPD = sPPD
                End If
                WriteDebug(WorkUnitLogHeader(WU) & "Work unit credited to " & WU.Credit & " ppd: " & WU.PPD)
            ElseIf WU.PPD = "" And WU.Credit <> "" Then
                WriteDebug(WorkUnitLogHeader(WU) & "has server assigned credit, calculating ppd by processing time")
                'Calculate ppd from processing time in a day
                Dim rv As New sProjectPPD
                Dim pCompletionTime As TimeSpan = WU.utcSubmitted.Subtract(WU.utcDownloaded)
                rv.Credit = WU.Credit
                'How many frames per 24/h
                Dim iPPD As Double = 0
                Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
                Do
                    If tsDay.Subtract(pCompletionTime).TotalSeconds >= 0 Then
                        iPPD += CInt(rv.Credit)
                        tsDay = tsDay.Subtract(pCompletionTime)
                    Else
                        Exit Do
                    End If
                Loop
                'get fraction of _tsFrame to be done in remaining seconds
                Dim iRfraction As Double
                If tsDay.TotalSeconds > 0 Then
                    iRfraction = tsDay.TotalSeconds / pCompletionTime.TotalSeconds
                    iPPD += iRfraction * CInt(rv.Credit)
                End If
                rv.PPD = CStr(Math.Round(iPPD, 2))
                WU.PPD = rv.PPD
                pCompletionTime = Nothing
                rv = Nothing
                iPPD = Nothing
                tsDay = Nothing
                WriteDebug(WorkUnitLogHeader(WU) & "Work unit ppd set to " & WU.PPD)
            End If
            rVal = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            rVal = False
        End Try
        Return rVal
    End Function
#End Region
#Region "String formatting"
    Public ReadOnly Property strID(WorkUnit As clsWU) As String
        Get
            Return "WU" & WorkUnit.ID & ":FS" & WorkUnit.Slot
        End Get
    End Property
    Public ReadOnly Property WorkUnitLogHeader(WorkUnit As clsWU, Optional BussyBox As Boolean = False) As String
        Get
            If Not BussyBox Then
                Return WorkUnit.ClientName & ":" & strID(WorkUnit) & ":" & WorkUnit.PRCG & ":" & WorkUnit.unit & "::"
            Else
                Return WorkUnit.ClientName & ":" & strID(WorkUnit) & ":" & WorkUnit.PRCG & ":" & WorkUnit.unit
            End If
        End Get
    End Property
    Public ReadOnly Property FormatTimeSpan(ByVal TS As TimeSpan, Optional ByVal MS As Boolean = False) As String
        'Wouldn't need this with .net 4
        Get
            Dim strAvg As String = String.Empty
            'WriteDebug("Starting timespan formatting for " & TS.ToString)
            Try
                If MS Then
                    If TS.TotalDays > 1 Then
                        Return String.Format("{0} Days, {1} hours, {2} minutes,{3} seconds and {4} milliseconds", {TS.Days, TS.Hours, TS.Minutes, TS.Seconds, TS.Milliseconds})
                    Else
                        Dim sb As New StringBuilder
                        If TS.Hours > 9 Then
                            sb.Append(CStr(TS.Hours) & ":")
                        ElseIf TS.Hours > 0 Then
                            sb.Append("0" & CStr(TS.Hours) & ":")
                        Else
                            sb.Append("00:")
                        End If
                        If TS.Minutes > 9 Then
                            sb.Append(CStr(TS.Minutes) & ":")
                        ElseIf TS.Minutes > 0 Then
                            sb.Append("0" & CStr(TS.Minutes) & ":")
                        Else
                            sb.Append("00:")
                        End If
                        If TS.Seconds > 9 Then
                            sb.Append(CStr(TS.Seconds) & ":")
                        ElseIf TS.Seconds > 0 Then
                            sb.Append("0" & CStr(TS.Seconds) & ":")
                        Else
                            sb.Append("00:")
                        End If
                        sb.Append(CStr(TS.Milliseconds))
                        Return sb.ToString
                    End If
                Else
                    Dim sb As New StringBuilder
                    If TS.Days >= 1 Then
                        Return String.Format("{0} Days, {1} hours, {2} minutes and {3} seconds", {TS.Days, TS.Hours, TS.Minutes, TS.Seconds})
                    Else
                        If TS.Hours = 0 Then
                            sb.Append("00:")
                        ElseIf TS.Hours > 0 And TS.Hours < 10 Then
                            sb.Append("0" & TS.Hours & ":")
                        Else
                            sb.Append(CStr(TS.Hours) & ":")
                        End If
                        If TS.Minutes = 0 Then
                            sb.Append("00:")
                        ElseIf TS.Minutes > 0 And TS.Minutes < 10 Then
                            sb.Append("0" & CStr(TS.Minutes) & ":")
                        Else
                            sb.Append(CStr(TS.Minutes) & ":")
                        End If
                        If TS.Seconds = 0 Then
                            sb.Append("00")
                        ElseIf TS.Seconds > 0 And TS.Seconds < 10 Then
                            sb.Append("0" & CStr(TS.Seconds))
                        Else
                            sb.Append(CStr(TS.Seconds))
                        End If
                        'WriteDebug("Timespan formatted to: " & sb.ToString)
                        Return sb.ToString
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                strAvg = "ERROR"
            End Try
            Return strAvg
        End Get
    End Property
    Public ReadOnly Property FormatSQLString(ByVal SQL As String, Optional ByVal ReturnString As Boolean = False, Optional ByVal ContainsTablename As Boolean = False) As String
        Get
            'Check
            'WriteDebug("Formatting string for sql, '" & SQL & "' with options ReturnString:" & ReturnString.ToString & " TableName:" & ContainsTablename.ToString)
            Dim rVal As String = SQL
            Try
                'chr(34) & chr(37) & chr(39) & chr(42)
                If ReturnString Then
                    rVal = rVal.Replace(Chr(255), Chr(34))
                    rVal = rVal.Replace(Chr(254), Chr(37))
                    rVal = rVal.Replace(Chr(253), Chr(39))
                    rVal = rVal.Replace(Chr(252), Chr(42))
                Else
                    rVal = rVal.Replace(Chr(34), Chr(255))
                    rVal = rVal.Replace(Chr(37), Chr(254))
                    rVal = rVal.Replace(Chr(39), Chr(253))
                    rVal = rVal.Replace(Chr(42), Chr(252))
                End If
                If ContainsTablename And Not ReturnString Then
                    rVal = rVal.Replace("-", "_")
                ElseIf ContainsTablename And ReturnString Then
                    rVal = rVal.Replace("_", "-")
                End If
                'WriteDebug("String formatted to: " & rVal)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
    Public ReadOnly Property FormatPPD(ByVal PPD As String) As String
        Get
            Dim sSep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            If PPD.Contains(sSep) Then
                If PPD.Length - (PPD.IndexOf(sSep) + 1) < 2 Then
                    Return CStr(PPD & "0")
                Else
                    Return Math.Round(CDbl(PPD), 2).ToString
                End If
            Else
                Return CStr(PPD & ".00")
            End If
        End Get
    End Property
#End Region
#Region "Share enumeration - http://www.pinvoke.net/default.aspx/netapi32.netshareenum"
    'I changed some of the data types, seems they were wrong, though it worked before.. ( code analyzer said so )
    Friend ServerShareCollection As ShareCollection
    Friend Structure ShareType
        Dim Name As String
        Dim Path As String
        Dim Type As Integer
        Dim Remark As String
    End Structure
    Friend Structure ShareCollection
        Dim Shares() As ShareType
        Dim ShareCount As Integer
        Sub Clear()
            Shares = Nothing
        End Sub
        Sub Add(ByVal Name As String, ByVal Path As String, ByVal Type As Integer, ByVal Remark As String)
            WriteDebug("Sharecollection add method called with name=" & Name & " path=" & Path & " Type=" & Type & " Remark=" & Remark)
            Try
                If Shares Is Nothing Then
                    ReDim Shares(0)
                    Shares(0).Name = Name
                    Shares(0).Path = Path
                    Shares(0).Type = Type
                    Shares(0).Remark = Remark
                Else
                    ReDim Preserve Shares(Shares.Length)
                    Shares(Shares.Length - 1).Name = Name
                    Shares(Shares.Length - 1).Path = Path
                    Shares(Shares.Length - 1).Type = Type
                    Shares(Shares.Length - 1).Remark = Remark
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            If Type = 0 Then
                WriteDebug("Share added to collection")
                ShareCount = ShareCount + 1
            Else
                WriteDebug("Share is not a valid share")
            End If
        End Sub
    End Structure
#Region "Constants"

    Const MAX_PREFERRED_LENGTH As Integer = -1      ' originally 0
    Const ERROR_SUCCESS As Integer = 0&        ' No errors encountered.
    Const NERR_Success As Integer = 0&
    Const ERROR_ACCESS_DENIED As Integer = 5&      ' The user has insufficient privilege for this operation.
    Const ERROR_NOT_ENOUGH_MEMORY As Integer = 8&      ' Not enough memory
    Const ERROR_NETWORK_ACCESS_DENIED As Integer = 65&     ' Network access is denied.
    Const ERROR_INVALID_PARAMETER As Integer = 87&     ' Invalid parameter specified.
    Const ERROR_BAD_NETPATH As Integer = 53&       ' The network path was not found.
    Const ERROR_INVALID_NAME As Integer = 123&     ' Invalid name
    Const ERROR_INVALID_LEVEL As Integer = 124&    ' Invalid level parameter.
    Const ERROR_MORE_DATA As Integer = 234&        ' More data available, buffer too small.
    Const NERR_BASE As Integer = 2100&
    Const NERR_NetNotStarted As Integer = 2102&    ' Device driver not installed.
    Const NERR_RemoteOnly As Integer = 2106&       ' This operation can be performed only on a server.
    Const NERR_ServerNotStarted As Integer = 2114&     ' Server service not installed.
    Const NERR_BufTooSmall As Integer = 2123&      ' Buffer too small for fixed-length data.
    Const NERR_RemoteErr As Integer = 2127&        ' Error encountered while remotely.  executing function
    Const NERR_WkstaNotStarted As Integer = 2138&      ' The Workstation service is not started.
    Const NERR_BadTransactConfig As Integer = 2141&    ' The server is not configured for this transaction;  IPC$ is not shared.
    Const NERR_NetNameNotFound As Integer = 2340&  ' Sharename not found.
    Const NERR_InvalidComputer As Integer = 2351&      ' Invalid computername specified.

#End Region ' "Constants"
#Region "API Calls With Structures"

    <StructLayout(LayoutKind.Sequential)> _
    Structure SHARE_INFO_1
        <MarshalAs(UnmanagedType.LPWStr)> Dim netname As String
        Public ShareType As Integer
        <MarshalAs(UnmanagedType.LPWStr)> Dim Remark As String

    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Structure SHARE_INFO_2
        <MarshalAs(UnmanagedType.LPWStr)> Dim shi2_netname As String
        Dim shi2_type As Integer
        <MarshalAs(UnmanagedType.LPWStr)> Dim shi2_remark As String
        Dim shi2_permissions As Integer
        Dim shi2_max_uses As Integer
        Dim shi2_current_uses As Integer
        <MarshalAs(UnmanagedType.LPWStr)> Dim shi2_path As String
        <MarshalAs(UnmanagedType.LPWStr)> Dim shi2_passwd As String
    End Structure

#End Region ' "API Calls With Structures"
    Friend Function GetSharesFromHost(ByVal server As String) As ShareCollection
        WriteDebug("GetSharesFromHost function called for server " & server)
        Dim Shares As New ShareCollection
        Shares.Clear()
        Try
            Dim level As Integer = 2
            Dim svr As New StringBuilder(server)
            Dim entriesRead As Integer, totalEntries As Integer, nRet As Integer, hResume As Integer = 0
            Dim pBuffer As IntPtr = IntPtr.Zero
            Try
                nRet = NativeMethods.NetShareEnum(svr, level, pBuffer, -1, entriesRead, totalEntries, _
                 hResume)
                If ERROR_ACCESS_DENIED = nRet Then
                    'Need admin for level 2, drop to level 1
                    WriteDebug("NetshareEnum ACCESS_DENIED, dropping to level 1")
                    level = 1
                    nRet = NativeMethods.NetShareEnum(svr, level, pBuffer, -1, entriesRead, totalEntries, _
                     hResume)
                End If

                If 0 = nRet AndAlso entriesRead > 0 Then
                    WriteDebug("Enumerating shared for server")
                    Application.DoEvents()
                    Dim t As Type = CType(IIf((2 = level), GetType(SHARE_INFO_2), GetType(SHARE_INFO_1)), Type)
                    Dim offset As Integer = Marshal.SizeOf(t)
                    Dim i As Integer = 0, lpItem As Integer = pBuffer.ToInt32()
                    While i < entriesRead
                        Application.DoEvents()
                        Dim pItem As New IntPtr(lpItem)
                        If 1 = level Then
                            Dim si As SHARE_INFO_1 = DirectCast(Marshal.PtrToStructure(pItem, t), SHARE_INFO_1)
                            Shares.Add(si.netname, "Access Denied", si.ShareType, si.Remark)
                        Else
                            Dim si As SHARE_INFO_2 = DirectCast(Marshal.PtrToStructure(pItem, t), SHARE_INFO_2)
                            Shares.Add(si.shi2_netname, si.shi2_path, si.shi2_type, si.shi2_remark)
                        End If
                        i += 1
                        lpItem += offset
                    End While
                    WriteDebug("Finished enumerating shares, number of shares in collection is " & Shares.ShareCount)
                End If
            Finally
                ' Clean up buffer allocated by system
                WriteDebug("Cleaning buffer")
                If IntPtr.Zero <> pBuffer Then
                    NativeMethods.NetApiBufferFree(pBuffer)
                End If
            End Try
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return Shares
    End Function
#End Region
#Region "Close application"
    Friend Sub ExitApplication(Optional ByVal Force As Boolean = False, Optional ByVal Confirm As Boolean = False, Optional ByVal DeleteDB As Boolean = False)
        Try
            If bClosing Then Exit Sub
            WriteDebug("Exit application called at " & DateTime.Now.ToString & " Force=" & Force.ToString & " Confirm=" & Confirm.ToString)
            If Confirm Then
                Dim rVal As MsgBoxResult = MsgBox("Exit application?", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Question, MsgBoxStyle))
                If rVal = MsgBoxResult.Cancel Then
                    WriteLog("User canceld exit")
                    Exit Sub
                End If
                WriteLog("User confirmed exit")
            End If
            modIcon.IconHide()
            If Not IsNothing(History) Then
                If Not History.IsDisposed And Not History.Disposing Then
                    History.SilentClose = True
                    History.HideForm()
                    History.Close()
                    History.Dispose()
                End If
            End If
            If Not IsNothing(Live) Then
                If Not Live.IsDisposed And Not Live.Disposing Then
                    Live.SilentClose = True
                    Live.HideForm()
                    Live.Close()
                    Live.Dispose()
                End If
            End If
            DebugOutput = False 'throws errors.. 
            If Not IsNothing(mySettings) Then
                WriteLog("Saving settings:" & modMySettings.SaveSettings.ToString)
            End If
            bClosing = True
            Timers.dispose()
            'If Not IsNothing(Bussy) Then
            delegateFactory.BussyBox.CloseForm()
            'End If
            If Not IsNothing(EOC) Then
                EOCInfo.IconVisible = False
            End If
            If Not IsNothing(about) Then
                about.CloseAbout()
            End If
            If Not IsNothing(ProjectInfo) Then
                ProjectInfo = Nothing
            End If
            If Not IsNothing(sqdata) AndAlso Not Data.dbPool.IsClosed Then
                Data.dbPool.close(DeleteDB)
            End If
            If Not IsNothing(logwindow.Form) Then
                logwindow.CloseLog()
            End If
            For Each File In lFilesToRemove
                Try
                    My.Computer.FileSystem.DeleteFile(File, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                Catch ex As Exception

                End Try
            Next
            Application.Exit()
        Catch ex As Exception
            MsgBox(ex.Message & Environment.NewLine & ex.StackTrace.ToString, CType(MsgBoxStyle.Critical + MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle))
            End
        End Try
    End Sub
#End Region
#Region "Set form state"
    'Friend Delegate Sub dWindowState(fState As FormWindowState)
    'Friend Sub SetWindowState(State As FormWindowState)
    '    If Me.InvokeRequired Then
    '        Dim nI As New dWindowState(AddressOf SetWindowState)
    '        nI.Invoke([State])
    '    Else
    '        Me.WindowState = State
    '    End If
    'End Sub
    'Friend Delegate Sub dSetEnabled(Enabled As Boolean)
    'Friend Sub SetEnabled(Enabled As Boolean)
    '    If Me.InvokeRequired Then
    '        Dim nI As New dSetEnabled(AddressOf SetEnabled)
    '        nI.Invoke([Enabled])
    '    Else
    '        Me.Enabled = Enabled
    '    End If
    'End Sub
#End Region
#Region "LumberJack extension"
    Private Sub LumberJack_ErrorWritten(sender As Object, message As String, ErrObj As Microsoft.VisualBasic.ErrObject) Handles LumberJack.ErrorWritten
        If Not IsNothing(sender) Then
            WriteError(sender.ToString & ":" & message, Err)
        Else
            WriteError(message, ErrObj)
        End If
    End Sub
    Private Sub LumberJack_LogWritten(sender As Object, message As String) Handles LumberJack.LogWritten
        If Not IsNothing(sender) Then
            WriteLog(sender.ToString & ":" & message)
        Else
            WriteLog(message)
        End If
    End Sub
#End Region
#Region "Random generator"
    Private rndGen As New Random()
    Public ReadOnly Property RandomInt(Optional iMax As Int32 = Nothing, Optional iMin As Int32 = Nothing) As Int32
        Get
            If IsNothing(iMin) And IsNothing(iMax) Then
                Return rndGen.Next
            ElseIf Not IsNothing(iMin) AndAlso Not IsNothing(iMax) Then
                Return rndGen.Next(iMin, iMax)
            Else
                Return rndGen.Next(iMax)
            End If
        End Get
    End Property
#End Region
#Region "Color list generator"
    Friend Function GenerateColorList(ItemCount As Int32) As List(Of Color)
        Try
            Dim rVal As New List(Of Color)
            Dim iColor As Int32 = RandomInt(5, 0)
            For xInt As Int32 = 0 To ItemCount
                If iColor = 0 Then
                    rVal.Add(modMySettings.clsGraphSettings.minColorPpd)
                ElseIf iColor = 1 Then
                    rVal.Add(modMySettings.clsGraphSettings.avgColorPpd)
                ElseIf iColor = 2 Then
                    rVal.Add(modMySettings.clsGraphSettings.maxColorPpd)
                ElseIf iColor = 3 Then
                    rVal.Add(modMySettings.clsGraphSettings.minColorTpf)
                ElseIf iColor = 4 Then
                    rVal.Add(modMySettings.clsGraphSettings.avgColorTpf)
                ElseIf iColor = 5 Then
                    rVal.Add(modMySettings.clsGraphSettings.maxColorTpf)
                End If
                If iColor = 5 Then
                    iColor = 0
                Else
                    iColor += 1
                End If
            Next
            Return rVal
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New List(Of Color)
        End Try
    End Function
#End Region
#Region "dbFolder"
    Friend Sub ReadFolder()
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\dbfolder") Then dbFolder = (My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\dbFolder")).Replace(Chr(34), "")
    End Sub
    Friend Sub SetFolder()
        My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\dbfolder", dbFolder, False)
    End Sub
#End Region
End Module
