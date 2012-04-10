'/*
' * FAHWatch7 mySettings  Copyright Marvin Westmaas ( mtm )
' *
' * Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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

Imports System.Globalization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.InteropServices
Friend Class modMySettings
    Implements IDisposable
    Private Shared m_firstRun As Boolean = True
    Private Shared m_IsUpgrading As Boolean = False
#Region "Fixed properties and settings"
    Friend Shared Property Settings As Dictionary(Of String, String)
        Get
            Return m_Settings
        End Get
        Set(value As Dictionary(Of String, String))
            m_Settings = value
        End Set
    End Property
    Friend Overloads Shared Property LiveParserInterval As Double
        Get
            Return CDbl(m_Settings("live_parserinterval"))
        End Get
        Set(value As Double)
            m_Settings("live_parserinterval") = CStr(value)
        End Set
    End Property
    Friend Shared ReadOnly Property IsUpgrading As Boolean
        Get
            Return m_IsUpgrading
        End Get
    End Property
    Friend Shared Property NotificationLevel As Int32
        Get
            Return CInt(m_Settings("notification_level"))
        End Get
        Set(value As Int32)
            m_Settings("notification_level") = CStr(value)
        End Set
    End Property
    Friend Shared Property OverrideAffinity_Priority As Boolean
        Get
            Return CBool(m_Settings("overrideaffinity_priority"))
        End Get
        Set(value As Boolean)
            m_Settings("overrideaffinity_priority") = value.ToString
        End Set
    End Property
    Friend Shared Property DisableNotifications As Boolean
        Get
            Return CBool(m_Settings("notify_disable"))
        End Get
        Set(value As Boolean)
            m_Settings("notify_disable") = value.ToString
        End Set
    End Property
    Friend Shared Property AlwaysTrack As Boolean
        Get
            Return CBool(m_Settings("alwaystrack"))
        End Get
        Set(value As Boolean)
            m_Settings("alwaystrack") = value.ToString
        End Set
    End Property
    Friend Shared Property SendException As Boolean
        Get
            Return CBool(m_Settings("sendexception"))
        End Get
        Set(value As Boolean)
            m_Settings("sendexception") = value.ToString
        End Set
    End Property
    Friend Shared Property DisableExceptionReport As Boolean
        Get
            Return CBool(m_Settings("disablecrashreport"))
        End Get
        Set(value As Boolean)
            m_Settings("disablecrashreport") = value.ToString
        End Set
    End Property
    Friend Shared Property NoAutoSizeColumns As Boolean
        Get
            Return CBool(m_Settings("noautoresizecolumns"))
        End Get
        Set(value As Boolean)
            m_Settings("noautoresizecolumns") = value.ToString
        End Set
    End Property
    Friend Shared Property Email_Notify As Boolean
        Get
            Return CBool(m_Settings("email_notify"))
        End Get
        Set(value As Boolean)
            m_Settings("email_notify") = value.ToString
        End Set
    End Property
    Friend Shared Property Email_provider As String
        Get
            Return m_Settings("email_provider")
        End Get
        Set(value As String)
            m_Settings("email_provider") = value
        End Set
    End Property
    Friend Shared Property Email_custom As String
        Get
            Return m_Settings("email_custom")
        End Get
        Set(value As String)
            m_Settings("email_custom") = value
        End Set
    End Property
    Friend Shared Property live_scLiveGraph_splitterdistance As Int32
        Get
            Return CInt(m_Settings("live_sclivegraph_splitterdistance"))
        End Get
        Set(value As Int32)
            m_Settings("live_sclivegraph_splitterdistance") = CStr(value)
        End Set
    End Property
    Friend Enum eLiveSelectedTab
        Frames
        Log
        Queue
    End Enum
    Friend Property live_selectedtab As eLiveSelectedTab
        Get
            Select Case m_Settings("live_selectedtab")
                Case Is = "frames"
                    Return eLiveSelectedTab.Frames
                Case Is = "log"
                    Return eLiveSelectedTab.Log
                Case Else
                    Return eLiveSelectedTab.Queue
            End Select
        End Get
        Set(value As eLiveSelectedTab)
            Select Case value
                Case eLiveSelectedTab.Frames
                    m_Settings("live_selectedtab") = "frames"
                Case eLiveSelectedTab.Log
                    m_Settings("live_selectedtab") = "log"
                Case eLiveSelectedTab.Queue
                    m_Settings("live_selectedtab") = "queue"
            End Select
        End Set
    End Property
    Friend Shared Property live_showsensors As Boolean
        Get
            Return CBool(m_Settings("live_showdetails"))
        End Get
        Set(value As Boolean)
            m_Settings("live_showdetails") = value.ToString
        End Set
    End Property
    Friend Enum eEtaStyle
        ShowDate
        ShowTimeToGo
    End Enum
    Friend Shared Property live_etastyle As eEtaStyle
        Get
            If m_Settings("live_etastyle") = "date" Then
                Return eEtaStyle.ShowDate
            Else
                Return eEtaStyle.ShowTimeToGo
            End If
        End Get
        Set(value As eEtaStyle)
            If value = eEtaStyle.ShowDate Then
                m_Settings("live_etastyle") = "date"
            Else
                m_Settings("live_etastyle") = "timespan"
            End If
        End Set
    End Property
    Friend Shared Property live_showdetails As Boolean
        Get
            Return CBool(m_Settings("live_showdetails"))
        End Get
        Set(value As Boolean)
            m_Settings("live_showdetails") = value.ToString
        End Set
    End Property
    Friend Shared Property live_windowstate As FormWindowState
        Get
            Dim rVal As FormWindowState = FormWindowState.Normal
            Try
                rVal = CType([Enum].Parse(GetType(FormWindowState), m_Settings("live_windowstate")), FormWindowState)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
        Set(value As FormWindowState)
            m_Settings("live_windowstate") = value.ToString
        End Set
    End Property
    Friend Shared Property live_formlocation As Point
        Get
            Return New Point(CInt(m_Settings("live_formlocation_x")), CInt(m_Settings("live_formlocation_y")))
        End Get
        Set(value As Point)
            m_Settings("live_formlocation_x") = CStr(value.X)
            m_Settings("live_formlocation_y") = CStr(value.Y)
        End Set
    End Property
    Friend Shared Property live_formsize As Size
        Get
            Return New Size(CInt(m_Settings("live_formsize_width")), CInt(m_Settings("live_formsize_height")))
        End Get
        Set(value As Size)
            m_Settings("live_formsize_width") = CStr(value.Width)
            m_Settings("live_formsize_height") = CStr(value.Height)
        End Set
    End Property
    Friend Shared Property ConvertUTC As Boolean
        Get
            Return CBool(m_Settings("displaytimeasutc"))
        End Get
        Set(value As Boolean)
            m_Settings("displaytimeasutc") = value.ToString
        End Set
    End Property
    Friend Shared Property HideInactiveMessageStrip As Boolean
        Get
            Return CBool(m_Settings("hideinactivemessagestrip"))
        End Get
        Set(value As Boolean)
            If Not value = CBool(m_Settings("hideinactivemessagestrip")) Then
                delegateFactory.RaiseHideInactiveMessageStripChanged(value)
            End If
            m_Settings("hideinactivemessagestrip") = value.ToString
        End Set
    End Property

    Friend Enum defaultStatisticsEnum
        Unknown
        Overall
        Current
    End Enum
    Friend Shared Property defaultStatistics As defaultStatisticsEnum
        Get
            Select Case m_Settings("defaultstatistics")
                Case Is = "overall"
                    Return defaultStatisticsEnum.Overall
                Case Is = "current"
                    Return defaultStatisticsEnum.Current
                Case Else
                    Return defaultStatisticsEnum.Unknown
            End Select
        End Get
        Set(value As defaultStatisticsEnum)
            Select Case value
                Case defaultStatisticsEnum.Current
                    m_Settings("defaultstatistics") = "current"
                Case Else
                    m_Settings("defaultstatistics") = "overall"
            End Select
            delegateFactory.RaiseDefaultStatisticsChanged(value)
        End Set
    End Property
    Friend Shared Property viewEocUser As Boolean
        Get
            Return CBool(m_Settings("eoc_viewuser"))
        End Get
        Set(value As Boolean)
            m_Settings("eoc_viewuser") = value.ToString
            delegateFactory.RaiseEOCViewUserChanged(value)
        End Set
    End Property
    Friend Shared Property viewEocTeam As Boolean
        Get
            Return CBool(m_Settings("eoc_viewteam"))
        End Get
        Set(value As Boolean)
            m_Settings("eoc_viewteam") = value.ToString
            delegateFactory.RaiseEOCViewTeamChanged(value)
        End Set
    End Property
    Friend Shared Property primaryEocAccount As String
        Get
            Return m_Settings("eoc_primary")
        End Get
        Set(value As String)
            m_Settings("eoc_primary") = value
        End Set
    End Property
    Friend Shared Property EOC_Net_Failure As DateTime
        Get
            Return DateTime.Parse(m_Settings("eoc_net_failure"), CultureInfo.InvariantCulture)
        End Get
        Set(value As DateTime)
            m_Settings("eoc_net_failure") = value.ToString("s")
        End Set
    End Property
    Friend Shared Property DefaultSummary As String
        Get
            Return m_Settings("defaultsummary")
        End Get
        Set(ByVal value As String)
            m_Settings("defaultsummary") = value
        End Set
    End Property
    Friend Shared Property StartWithWindows As Boolean
        Get
            Return CBool(m_Settings("startwithwindows"))
        End Get
        Set(ByVal value As Boolean)
            m_Settings("startwithwindows") = value.ToString
        End Set
    End Property
    Friend Shared Property MinimizeToTray As Boolean
        Get
            Return CBool(m_Settings("minimizetotray"))
        End Get
        Set(ByVal value As Boolean)
            m_Settings("minimizetotray") = value.ToString
        End Set
    End Property
    Friend Shared Property NetworkPort As String
        Get
            Try
                Return m_Settings("network_port")
            Catch ex As Exception
                Return "49153"
            End Try
        End Get
        Set(value As String)
            m_Settings("network_port") = value
        End Set
    End Property
    Friend Shared Property ShowEOCIcon As Boolean
        Get
            Return CBool(m_Settings("eoc_icon"))
        End Get
        Set(value As Boolean)
            m_Settings("eoc_icon") = CStr(value)
        End Set
    End Property
    Friend Shared ReadOnly Property FirstRun As Boolean
        Get
            Return m_firstRun
        End Get
    End Property
    Friend Shared Property DefaultView As String
        Get
            Return m_Settings("defaultview")
        End Get
        Set(value As String)
            m_Settings("defaultview") = value
        End Set
    End Property
    Friend Enum eMainForm
        History
        Live
    End Enum
    Friend Shared Property MainForm As eMainForm
        Get
            If DefaultView = "historical" Then
                Return eMainForm.History
            Else
                Return eMainForm.Live
            End If
        End Get
        Set(value As eMainForm)
            If value = eMainForm.History Then
                DefaultView = "historical"
            Else
                DefaultView = "live"
            End If
        End Set
    End Property
    Friend Shared Property LocalClientName As String
        Get
            Return m_Settings("localname")
        End Get
        Set(value As String)
            m_Settings("localname") = value
        End Set
    End Property
    Friend Shared Property StartFC As Boolean
        Get
            Return CBool(m_Settings("startfc"))
        End Get
        Set(value As Boolean)
            m_Settings("startfc") = value.ToString
        End Set
    End Property
    Friend Shared Property StartMinimized As Boolean
        Get
            Return CBool(m_Settings("startminimized"))
        End Get
        Set(value As Boolean)
            m_Settings("startminimized") = value.ToString
        End Set
    End Property
    Friend Shared Property CheckFocus As Boolean
        Get
            Return CBool(m_Settings("checkfocus"))
        End Get
        Set(value As Boolean)
            m_Settings("checkfocus") = value.ToString
        End Set
    End Property
    Friend Shared Property AutoDownloadSummary As Boolean
        Get
            Return Not CBool(m_Settings("disable_summaryupdater"))
        End Get
        Set(value As Boolean)
            m_Settings("disable_summaryupdater") = (Not value).ToString
        End Set
    End Property
    Friend Shared Property EOCNotify As Boolean
        Get
            Return CBool(m_Settings("eoc_notify"))
        End Get
        Set(value As Boolean)
            m_Settings("eocnotify") = value.ToString
        End Set
    End Property
    Friend Shared Property NotifyOption As clsNotifyOptions.eNotifyOption
        Get
            Try
                Return CType([Enum].Parse(GetType(clsNotifyOptions.eNotifyOption), m_Settings("notify_by")), clsNotifyOptions.eNotifyOption)
            Catch ex As Exception
                Return clsNotifyOptions.eNotifyOption.TrayIcon
            End Try
        End Get
        Set(value As clsNotifyOptions.eNotifyOption)
            If HasSetting("notifyfail_by") Then
                ChangeSetting("notifyfail_by", value.ToString)
            Else
                AddSetting("notifyfail_by", value.ToString)
            End If
        End Set
    End Property
    Friend Shared Property Notify_EUE As Boolean
        Get
            Return CBool(m_Settings("notify_eue"))
        End Get
        Set(value As Boolean)
            m_Settings("notify_eue") = value.ToString
        End Set
    End Property
    Friend Shared Property Notify_EUE_ByRate As Boolean
        Get
            Return CBool(m_Settings("notify_eue_rate"))
        End Get
        Set(value As Boolean)
            m_Settings("notify_eue_rate") = value.ToString
        End Set
    End Property
    Friend Shared Property Notify_EUE_Always As Boolean
        Get
            Return CBool(m_Settings("notify_eue_always"))
        End Get
        Set(value As Boolean)
            m_Settings("notify_eue_always") = value.ToString
        End Set
    End Property
    Friend Shared Property Notify_Failure As Boolean
        Get
            Return CBool(m_Settings("notify_fail"))
        End Get
        Set(value As Boolean)
            m_Settings("notify_fail") = value.ToString
        End Set
    End Property
    Friend Shared Property NotifyRate As Double
        Get
            Return CDbl(m_Settings("notify_rate"))
        End Get
        Set(value As Double)
            m_Settings("notify_rate") = CStr(value)
        End Set
    End Property
    Friend Shared Property Notify_UseRules As Boolean
        Get
            Return CBool(m_Settings("usefilterrules"))
        End Get
        Set(value As Boolean)
            m_Settings("usefilterrules") = value.ToString
        End Set
    End Property
    Friend Shared Property DisableEOC As Boolean
        Get
            Return CBool(m_Settings("eoc_disable"))
        End Get
        Set(value As Boolean)
            m_Settings("eoc_disable") = value.ToString
            delegateFactory.RaiseEOCEnabledChanged(Not value)
        End Set
    End Property
    Friend Shared Property EocCustomSignature As String
        Get
            Return m_Settings("eoc_customsignature")
        End Get
        Set(value As String)
            m_Settings("eoc_customsignature") = value
        End Set
    End Property
    Friend Shared Property ParseLogsOnInterval As Boolean
        Get
            Return CBool(m_Settings("parseoninterval"))
        End Get
        Set(value As Boolean)
            m_Settings("parseoninterval") = value.ToString
            delegateFactory.RaiseParseOnIntervalChanged(value)
        End Set
    End Property
    Friend Shared Property ParseOnEOCUpdate As Boolean
        Get
            Return CBool(m_Settings("eoc_parselog"))
        End Get
        Set(value As Boolean)
            m_Settings("eoc_parselog") = value.ToString
        End Set
    End Property
    Friend Shared Property ParserInterval As TimeSpan
        Get
            Return TimeSpan.Parse(m_Settings("parserinterval"))
        End Get
        Set(value As TimeSpan)
            m_Settings("parserinterval") = value.ToString
            delegateFactory.RaiseParseOnIntervalValueChanged(value)
        End Set
    End Property
    Friend Shared Property history_ViewDetails As Boolean
        Get
            Return CBool(m_Settings("history_viewdetails"))
        End Get
        Set(value As Boolean)
            m_Settings("history_viewdetails") = value.ToString
        End Set
    End Property
    Friend Shared Property history_tcMain_splitterdistance As Int32
        Get
            Return CInt(m_Settings("history_tcmain_splitterdistance"))
        End Get
        Set(value As Int32)
            m_Settings("history_tcmain_splitterdistance") = CStr(value)
        End Set
    End Property
    Friend Shared Property history_tcdetails_splitterdistance As Int32
        Get
            Return (CInt(m_Settings("history_tcdetails_splitterdistance")))
        End Get
        Set(value As Int32)
            m_Settings("history_tcdetails_splitterdistance") = CStr(value)
        End Set
    End Property
    Friend Shared Property history_windowstate As FormWindowState
        Get
            Dim rVal As FormWindowState = FormWindowState.Normal
            Try
                rVal = CType([Enum].Parse(GetType(FormWindowState), m_Settings("history_windowstate")), FormWindowState)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
        Set(value As FormWindowState)
            m_Settings("history_windowstate") = value.ToString
        End Set
    End Property
    Friend Shared Property history_formlocation As Point
        Get
            Return New Point(CInt(m_Settings("history_formlocation_x")), CInt(m_Settings("history_formlocation_y")))
        End Get
        Set(value As Point)
            m_Settings("history_formlocation_x") = CStr(value.X)
            m_Settings("history_formlocation_y") = CStr(value.Y)
        End Set
    End Property
    Friend Shared Property history_formsize As Size
        Get
            Return New Size(CInt(m_Settings("history_formsize_width")), CInt(m_Settings("history_formsize_height")))
        End Get
        Set(value As Size)
            m_Settings("history_formsize_width") = CStr(value.Width)
            m_Settings("history_formsize_height") = CStr(value.Height)
        End Set
    End Property
    Friend Shared Property eoc_lastattempt As DateTime
        Get
            Return CDate(m_Settings("eoc_lastattempt"))
        End Get
        Set(value As DateTime)
            m_Settings("eoc_lastattempt") = value.ToString("s")
        End Set
    End Property
    Friend Shared Property history_details_index As String
        Get
            Return m_Settings("history_details_index")
        End Get
        Set(value As String)
            m_Settings("history_details_index") = value
        End Set
    End Property
    Friend Enum eDetail
        WU
        Performance
        Projects
        Hardware
        None
    End Enum
    Friend Shared ReadOnly Property history_detail As eDetail
        Get
            Select Case history_details_index
                Case Is = "0"
                    Return eDetail.WU
                Case Is = "1"
                    Return eDetail.Performance
                Case Is = "2"
                    Return eDetail.Projects
                Case Is = "3"
                    Return eDetail.Hardware
                Case Else
                    Return eDetail.None
            End Select
        End Get
    End Property
    Friend Shared Property LimitLogWrites As Boolean
        Get
            If Not m_Settings.ContainsKey("limit_logwrites") Then
                Return Nothing
            Else
                Return CBool(m_Settings("limit_logwrites"))
            End If
        End Get
        Set(value As Boolean)
            m_Settings("limit_logwrites") = value.ToString
        End Set
    End Property
#Region "Settings functions"
    Friend Shared Function GetSettings() As Boolean
        Try
            Dim _b As Dictionary(Of String, String) = m_Settings
            m_Settings = sqdata.ReadSettings
            For Each _default In _b
                If Not m_Settings.ContainsKey(_default.Key) Then m_Settings.Add(_default.Key, _default.Value)
            Next
            Return m_Settings.Count > 0
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Shared Function SaveSettings() As Boolean
        Try
            Return sqdata.StoreSettings(m_Settings)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Shared ReadOnly Property HasSetting(Name As String) As Boolean
        Get
            Return m_Settings.ContainsKey(Name)
        End Get
    End Property
    Friend Shared ReadOnly Property Setting(Name As String) As Object
        Get
            If m_Settings.ContainsKey(Name) Then
                Return m_Settings(Name)
            Else
                Return ""
            End If
        End Get
    End Property
    Friend Shared Sub ChangeSetting(Name As String, Value As String)
        If m_Settings.ContainsKey(Name) Then
            m_Settings(Name) = Value
        Else
            AddSetting(Name, Value)
        End If
    End Sub
    Friend Shared Sub AddSetting(p1 As String, p2 As String)
        If Not m_Settings.ContainsKey(p1) Then
            m_Settings.Add(p1, p2)
        Else
            ChangeSetting(p1, p2)
        End If
    End Sub
#End Region
#End Region
#Region "Form actions/properties"
#Region "Options form actions/properties"
    Private Shared mfrmOptions As frmOptions
    Private Shared m_Settings As New Dictionary(Of String, String)
    Friend Shared ReadOnly Property IsOptionsFormActive As Boolean
        Get
            Return delegateFactory.IsFormVisible(mfrmOptions)
        End Get
    End Property
    Friend Shared Function ShowOptionsForm(Optional ByVal Owner As Form = Nothing) As DialogResult
        Try
            If IsNothing(mfrmOptions) Or (Not IsNothing(mfrmOptions) AndAlso mfrmOptions.IsDisposed) Then mfrmOptions = New frmOptions
            Return mfrmOptions.ShowOptions(Owner)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return DialogResult.Abort
        End Try
    End Function
    Friend Shared Sub CloseOptionsForm()
        mfrmOptions.CloseForm()
    End Sub
#End Region
#Region "Mail settings form actions/properties"
    Private Shared mFrmMail As frmMail
    Friend Shared ReadOnly Property IsMailFormActive As Boolean
        Get
            Return delegateFactory.IsFormVisible(mFrmMail)
        End Get
    End Property
    Friend Shared Function ShowMailForm(Optional Owner As Form = Nothing) As DialogResult
        Try
            If IsNothing(mFrmMail) Or (Not IsNothing(mFrmMail) AndAlso mFrmMail.IsDisposed Or mFrmMail.Disposing) Then mFrmMail = New frmMail
            Return mFrmMail.ShowDialog
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return DialogResult.Abort
        End Try
    End Function
#End Region
#Region "Notification form actions/properties"
    Private Shared mFrmNotify As frmAdvancedOptions
    Private Shared mNotifyOptions As New clsNotifyOptions
    Friend Class clsNotifyOptions
        Friend Shared NotifyEUE As Boolean = False
        Friend Shared EUE_by_Rate As Boolean = False
        Friend Shared EUE_always As Boolean = True
        Friend Shared NotifyRATE As Double = 80.0
        Friend Shared notifyFAILURE As Boolean = True
        Friend Shared NotifyOption As eNotifyOption = eNotifyOption.TrayIcon
        Friend Shared NotifyRules As Boolean = False
        Friend Enum eNotifyOption
            TrayIcon
            PopUpForm
        End Enum
    End Class
    Friend Shared Property notify As clsNotifyOptions
        Get
            Return mNotifyOptions
        End Get
        Set(value As clsNotifyOptions)
            mNotifyOptions = value
        End Set
    End Property
    Friend Shared ReadOnly Property IsNotifyFormActive As Boolean
        Get
            Return mFrmNotify.Visible
        End Get
    End Property
    Friend Shared Sub ShowNotifyForm(Optional Owner As Form = Nothing)
        Try
            Try
                If IsNothing(mFrmNotify) Or mFrmNotify.IsDisposed Or mFrmNotify.Disposing Then mFrmNotify = New frmAdvancedOptions
            Catch ex As Exception
                mFrmNotify = New frmAdvancedOptions
            End Try
            mFrmNotify.Init(Owner)
            m_firstRun = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#Region "Notify filters"
    Friend Class clsNFilter
        Friend ucFilter As ucNotifyFilter
        Friend fName As String = String.Empty  ' filter name
        Friend ReadOnly Property ClientName As String
            Get
                Dim cName As String = ""
                If InStr(fName, "_") = 0 Then
                    cName = fName
                Else
                    cName = fName.Substring(8, fName.IndexOf("_", 8) - 8)
                End If
                Return cName
            End Get
        End Property
        Friend ReadOnly Property SlotID As String 'return -1 for client rule
            Get
                Dim cName As String = ""
                If InStr(fName, "_") = 0 Then
                    cName = fName
                Else
                    cName = fName.Substring(8, fName.IndexOf("_", 8) - 8)
                End If
                If fName.IndexOf("_") = -1 Then
                    Return "-1"
                Else
                    Dim sIndex As String = fName.Substring(fName.LastIndexOf("_") + 1, fName.IndexOf(":") - fName.LastIndexOf("_") - 1)
                    If sIndex.Length = 1 Then sIndex = "0" & sIndex
                    Return sIndex
                End If
            End Get
        End Property
        Friend fEnabled As Boolean = False
        Friend bFail As Boolean = True
        Friend bRate As Boolean = False
        Friend bEUE As Boolean = False
        Friend iRATE As Double = 80.0
        Friend ReadOnly Property currentRate As String
            Get
                'Return Statistics.Clients(fName.Substring 
                '"filters_" & C.ClientName & "_" & S.id & ":" & S.type
                Try
                    Dim cName As String = ""
                    If InStr(fName, "_") = 0 Then
                        cName = fName
                    Else
                        cName = fName.Substring(8, fName.IndexOf("_", 8) - 8)
                    End If
                    If fName.IndexOf("_") = -1 Then
                        Return clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(cName).SuccesRate
                    Else
                        Dim sIndex As String = fName.Substring(fName.LastIndexOf("_") + 1, fName.IndexOf(":") - fName.LastIndexOf("_") - 1)
                        If sIndex.Length = 1 Then sIndex = "0" & sIndex
                        Return clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(cName).Slots(sIndex).SuccesRate
                    End If
                Catch ex As Exception
                    Return "-1"
                End Try
            End Get
        End Property
        Friend bError As Boolean = False
    End Class
    Private Shared m_nFilters As New List(Of clsNFilter)
    Friend Shared ReadOnly Property nFilters As List(Of clsNFilter)
        Get
            Return m_nFilters
        End Get
        'Set(value As List(Of clsNFilter))
        '    m_nFilters = value
        'End Set
    End Property
    Friend Shared Function UpdateFilterSettings() As Boolean
        Try
            For Each ucF As clsNFilter In m_nFilters
                Dim fPrefix As String = ucF.fName
                ChangeSetting(fPrefix & "_enabled", ucF.ucFilter.chkRuleName.Checked.ToString)
                ChangeSetting(fPrefix & "_eue", ucF.ucFilter.rbEUE_Always.Checked.ToString)
                ChangeSetting(fPrefix & "_rate", ucF.ucFilter.rbEUE_ratio.Checked.ToString)
                ChangeSetting(fPrefix & "_currentrate", ucF.ucFilter.txtRatio_Actual.Text)
                ChangeSetting(fPrefix & "_irate", CInt(ucF.ucFilter.nudRatio_Warning.Value).ToString)
            Next
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Shared Function InitializeFilters() As Boolean
        If m_nFilters.Count > 0 Then
            Return True
            WriteLog("Aborting filter initialization due to already being initialized", eSeverity.Important)
        End If
        Dim bErr As Boolean = False
        Try
            Dim cS As clsStatistics.clsPerformanceStatistics = clsStatistics.clsPerformanceStatistics.CurrentStatistics
            'enum Clients.clients, add client filter and slot filters
            For Each C As Client In Clients.Clients
                Dim nFilter As New clsNFilter
                nFilter.fName = C.ClientName
                Dim fPrefix As String = FormatSQLString("filters_" & C.ClientName)
                If HasSetting(fPrefix) Then
                    nFilter.fEnabled = CBool(Setting(fPrefix & "_enabled"))
                    nFilter.bEUE = CBool(Settings(fPrefix & "_eue"))
                    nFilter.bFail = CBool(Settings(fPrefix & "_fail"))
                    nFilter.bRate = CBool(Setting(fPrefix & "_rate"))
                    nFilter.iRATE = CDbl(Setting(fPrefix & "_irate"))
                    nFilter.ucFilter = New ucNotifyFilter
                Else
                    AddSetting(fPrefix, Boolean.TrueString) 'boolean value indicate filter availability
                    AddSetting(fPrefix & "_enabled", Boolean.TrueString)
                    AddSetting(fPrefix & "_eue", Boolean.FalseString)
                    AddSetting(fPrefix & "_rate", Boolean.FalseString)
                    AddSetting(fPrefix & "_fail", Boolean.TrueString)
                    AddSetting(fPrefix & "_irate", "80.0")
                    nFilter.ucFilter = New ucNotifyFilter
                End If
                m_nFilters.Add(nFilter)
                If Not IsNothing(C.ClientConfig.Configuration.slots) Then
                    For Each S As clsClientConfig.clsConfiguration.sSlot In C.ClientConfig.Configuration.slots
                        Dim sFilter As New clsNFilter
                        fPrefix = FormatSQLString("filters_" & C.ClientName & "_" & S.id & ":" & S.type)
                        sFilter.fName = fPrefix
                        If HasSetting(fPrefix) Then
                            sFilter.fEnabled = CBool(Setting(fPrefix & "_enabled"))
                            sFilter.bFail = CBool(Settings(fPrefix & "_fail"))
                            sFilter.bEUE = CBool(Settings(fPrefix & "_eue"))
                            sFilter.bRate = CBool(Setting(fPrefix & "_rate"))
                            sFilter.iRATE = CInt(Setting(fPrefix & "_irate"))
                            sFilter.ucFilter = New ucNotifyFilter
                        Else
                            AddSetting(fPrefix, Boolean.FalseString) 'boolean value indicate filter availability
                            AddSetting(fPrefix & "_enabled", Boolean.FalseString)
                            AddSetting(fPrefix & "_fail", Boolean.TrueString)
                            AddSetting(fPrefix & "_eue", Boolean.FalseString)
                            AddSetting(fPrefix & "_rate", Boolean.FalseString)
                            AddSetting(fPrefix & "_irate", "80")
                            sFilter.ucFilter = New ucNotifyFilter
                        End If
                        m_nFilters.Add(sFilter)
                    Next
                End If
            Next
        Catch ex As Exception
            bErr = True
            WriteError(ex.Message, Err)
        End Try
        Return Not bErr AndAlso CBool(m_nFilters.Count > 0)
    End Function
#End Region
#End Region
#End Region
#Region "Remote clients"
    Friend Class clsRemoteClients
        Friend Shared Sub Clear()
            Try
                Do While Clients.Clients.Count > 1
                    Clients.RemoveClient(Clients.Clients(1).ClientName)
                Loop
                sqdata.SaveRemoteClients()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub RemoveRemoteClient(ByVal Name As String)
            Try
                Clients.RemoveClient(Name)
                sqdata.SaveRemoteClients()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub AddRemoteClient(ByVal Name As String, ByVal Location As String, FCPort As String, PWD As String, FWPort As String, Enabled As Boolean)
            Try
                Clients.AddClient(Name, Location, FCPort, PWD, FWPort, Enabled)
                sqdata.SaveRemoteClients()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub SetState(ByVal Name As String, ByVal Enabled As Boolean)
            Try
                Clients.SetClientState(Name, Enabled)
                sqdata.SaveRemoteClients()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
    End Class
    Friend Shared RemoteClients As New clsRemoteClients
#End Region
#Region "ListView column information"
    Friend Class sColumnInfo
        Friend Header As String = Nothing
        Friend Index As Int32 = Nothing
        Friend DisplayIndex As Int32 = Nothing
        Friend Width As Int32 = Nothing
        Friend Visible As Boolean = Nothing
    End Class
    Friend Class ColumnSettings
        ' Instead of calling InitMaster/InitColumns after altering, update the changes to the dictionaries only!
        Private Shared dColumnMasters As New Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Private Shared dColumnSettings As New Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Friend Shared Sub UpdateColumnVisible(listview As ListView, Header As String, Visible As Boolean)
            WriteLog("Updating column visibility, " & sqdata.lvMasterTable(listview) & Header & ":" & sqdata.UpdateColumnVisible(listview, Header, Visible).ToString)
            InitMasters()
        End Sub
        Shared ReadOnly Property MasterSettings(ListView As ListView) As SortedList(Of Int32, sColumnInfo)
            Get
                If dColumnMasters.ContainsKey(sqdata.lvMasterTable(ListView)) Then
                    Return dColumnMasters(sqdata.lvMasterTable(ListView))
                Else
                    Return New SortedList(Of Int32, sColumnInfo)
                End If
            End Get
        End Property
        Shared ReadOnly Property ColumnSettings(ListView As ListView) As SortedList(Of Int32, sColumnInfo)
            Get
                If dColumnSettings.ContainsKey(sqdata.lvColumnTable(ListView)) Then
                    Return dColumnSettings(sqdata.lvColumnTable(ListView))
                Else
                    Return MasterSettings(ListView)
                End If
            End Get
        End Property
        Shared Sub CreateMaster(ListView As ListView)
            WriteLog("ColumnSettins_CreateMaster(" & ListView.FindForm.Name & "_" & ListView.Name & ")::" & sqdata.CreateColumnMaster(ListView).ToString)
            InitMasters()
        End Sub
        Shared Sub UpdateColumnSettings(Listview As ListView)
            WriteLog("ColumnSettins_UpdateSettings(" & Listview.FindForm.Name & "_" & Listview.Name & ")::" & sqdata.UpdateColumnSettings(Listview).ToString)
            InitColumns()
        End Sub
        Shared Sub InitMasters()
            dColumnMasters = sqdata.GetColumnMasters
        End Sub
        Shared Sub InitColumns()
            dColumnSettings = sqdata.GetColumnSettings
        End Sub
    End Class
#End Region
#Region "Hardware gauges"
    <Serializable()>
    Public Class sSettings
        Implements ISerializable
        Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
            With info
                .AddValue("lSettings", lSettings)
                .AddValue("mHWSettings", mHWsettings)
            End With
        End Sub
        Public Enum eStartMethod
            Registry = 0
            RegistryMinimized = 1
        End Enum
        Public Enum eEocLimit
            None = 0
            Minimal = 1
            OneDay = 2
            OneWeek = 3
            OneMonth = 4
        End Enum
        <Serializable()>
        Public Class sHWsettings
            Implements ISerializable
            Public Sub New()

            End Sub
            Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
                Try
                    If IsNothing(info) Then
                        Throw New System.ArgumentNullException("info")
                    End If
                    Identifier = info.GetString("Identifier")
                    ClocksAsGauges = info.GetBoolean("ClocksAsGauges")
                    HideClocks = info.GetBoolean("HideClocks")
                Catch ex As Exception
                End Try
            End Sub
            Public Identifier As String
            Public ClocksAsGauges As Boolean
            Public HideClocks As Boolean
            Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
                info.AddValue("Identifier", Identifier)
                info.AddValue("ClocksAsGauges", ClocksAsGauges)
                info.AddValue("HideClocks", HideClocks)
            End Sub
        End Class
        Private mHWsettings As New List(Of sHWsettings)
        Public ReadOnly Property HasHWSettings(ByVal Identifier As String) As Boolean
            Get
                Try
                    For Each gs As sHWsettings In mHWsettings
                        If gs.Identifier = Identifier Then Return True
                    Next
                    Return False
                Catch ex As Exception
                    Return False
                End Try
            End Get
        End Property
        Public ReadOnly Property HWSettings(ByVal Identifier As String) As sHWsettings
            Get
                Try
                    For Each hwSetting As sHWsettings In mHWsettings
                        If hwSetting.Identifier = Identifier Then Return hwSetting
                    Next
                    Return Nothing
                Catch ex As Exception
                    WriteError("mySettings_GaugeSettings", Err)
                    Return Nothing
                End Try
            End Get
        End Property
        Public Function SaveHWSettings(ByVal Identifier As String, ByVal Settings As modMySettings.sSettings.sHWsettings) As Boolean
            Try
                For Each Setting As sHWsettings In mHWsettings
                    If Setting.Identifier = Identifier Then
                        Setting = Settings
                        Return True
                    End If
                Next
                mHWsettings.Add(Settings)
                Return mHWsettings.Contains(Settings)
            Catch ex As Exception
                WriteError("mySettings_SaveSettings", Err)
                Return False
            End Try
        End Function
        <Serializable()>
        Public Class sGaugeSettings
            Implements ISerializable
            Public Sub New()

            End Sub
            Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
                Try
                    If Not IsNothing(info) Then
                        Identifier = info.GetString("Identifier")
                        backColor = Color.FromName(info.GetString("backColor"))
                        valueColor = Color.FromName(info.GetString("valueColor"))
                        rangeColor = Color.FromName(info.GetString("rangeColor"))
                        minmaxColor = Color.FromName(info.GetString("minmaxColor"))
                        warningColor = Color.FromName(info.GetString("warningColor"))
                        warningStart = info.GetInt32("warningStart")
                        normalMIN = info.GetInt32("normalMIN")
                        valueMAX = info.GetInt32("valueMAX")
                        valueMIN = info.GetInt32("valueMIN")
                        stepSmall = info.GetInt32("stepSmall")
                        stepLarge = info.GetInt32("stepLarge")
                        vSize = info.GetInt32("vSize")
                        Try
                            AlternateImage = info.GetString("AlternateImage")
                        Catch ex As Exception

                        End Try
                        Try
                            Hide = info.GetBoolean("Hide")
                        Catch ex As Exception

                        End Try
                        Try
                            iBorder = info.GetInt32("border")
                        Catch ex As Exception

                        End Try
                        Try
                            borderColor = Color.FromName(info.GetString("borderColor"))
                        Catch ex As Exception

                        End Try
                        Try
                            iCorner = info.GetInt32("iCorner")
                        Catch ex As Exception

                        End Try
                    End If
                Catch ex As Exception

                End Try
            End Sub
            Public Hide As Boolean
            Public Identifier As String
            Public backColor As Color
            Public valueColor As Color
            Public rangeColor As Color
            Public minmaxColor As Color
            Public warningColor As Color
            Public valueMAX As Integer
            Public valueMIN As Integer
            Public warningStart As Integer
            Public normalMIN As Integer
            Public stepSmall As Integer
            Public stepLarge As Integer
            Public vSize As Integer = 3
            Public iCorner As Int32 = 20
            Public iBorder As Int32 = 3
            Public borderColor As Color = Color.Black
            Public AlternateImage As String
            Public wSound As String
            Public wIcon As Boolean
            Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
                Try
                    With info
                        .AddValue("Hide", Hide)
                        .AddValue("iCorner", iCorner)
                        .AddValue("border", iBorder)
                        .AddValue("borderColor", borderColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("Identifier", Identifier.ToString)
                        .AddValue("backColor", backColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("rangeColor", rangeColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("minmaxColor", minmaxColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("valueColor", valueColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("warningColor", warningColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                        .AddValue("valueMAX", valueMAX)
                        .AddValue("valueMIN", valueMIN)
                        .AddValue("normalMIN", normalMIN)
                        .AddValue("warningStart", warningStart)
                        .AddValue("stepSmall", stepSmall)
                        .AddValue("stepLarge", stepLarge)
                        .AddValue("vSize", vSize)
                        If Not IsNothing(AlternateImage) Then .AddValue("AlternateImage", AlternateImage)
                    End With
                Catch ex As Exception

                End Try

            End Sub
        End Class
        Private lSettings As New List(Of sGaugeSettings)
        Private lCSounds As New List(Of String)
        Public ReadOnly Property HasGaugeSettings(ByVal Identifier As String) As Boolean
            Get
                Try
                    For Each gs As sGaugeSettings In lSettings
                        If gs.Identifier = Identifier Then Return True
                    Next
                    Return False
                Catch ex As Exception
                    Return False
                End Try
            End Get
        End Property
        Public ReadOnly Property GaugeSettings(ByVal Identifier As String) As sGaugeSettings
            Get
                Try
                    For xInt As Int32 = 0 To lSettings.Count - 1
                        If lSettings(xInt).Identifier = Identifier Then
                            Return lSettings(xInt)
                        End If
                    Next
                    Return Nothing
                Catch ex As Exception
                    WriteError("mySettings_GaugeSettings", Err)
                    Return Nothing
                End Try
            End Get
        End Property
        Public Function SaveGaugeSettings(ByVal Identifier As String, ByVal GSettings As modMySettings.sSettings.sGaugeSettings) As Boolean
            Try
                Dim iIndex As Int32 = 0
                For xInt As Int32 = 0 To lSettings.Count - 1
                    If lSettings(xInt).Identifier = Identifier Then
                        lSettings.RemoveAt(xInt)
                        lSettings.Insert(xInt, GSettings)
                        Return lSettings.Contains(GSettings)
                    End If
                Next
                lSettings.Add(GSettings)
                Return True
            Catch ex As Exception
                WriteError("mySettings_SaveSettings", Err)
                Return False
            End Try
        End Function
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            Try
                If IsNothing(info) Then
                    Throw New System.ArgumentNullException("info")
                End If
                lSettings = CType(info.GetValue("lSettings", lSettings.GetType), Global.System.Collections.Generic.List(Of Global.FAHWatch7.modMySettings.sSettings.sGaugeSettings))
                mHWsettings = CType(info.GetValue("mHWSettings", mHWsettings.GetType), Global.System.Collections.Generic.List(Of Global.FAHWatch7.modMySettings.sSettings.sHWsettings))
            Catch ex As Exception

            End Try
        End Sub
        Public Sub New()

        End Sub
    End Class
    Private WithEvents mHWSettings As New sSettings
    Friend Property MySettings As sSettings = mHWSettings
    Private fileSettings As String = ""
    Private mBEmpty As Boolean
    Private Function SaveHWSettings() As Boolean
        Try
            Dim Serializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileSettings, FileMode.Create, FileAccess.Write, FileShare.Inheritable)
            Serializer.Serialize(DataFile, MySettings)
            DataFile.Close()
            DataFile = Nothing
            Serializer = Nothing
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Function ReadSettings() As Boolean
        Try
            Dim Deserializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileSettings, FileMode.Open, FileAccess.Read, FileShare.Inheritable)
            MySettings = CType(Deserializer.Deserialize(DataFile), modMySettings.sSettings)
            DataFile.Close()
            Deserializer = Nothing
            DataFile = Nothing
            mBEmpty = False
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            mBEmpty = True
            Return False
        End Try

    End Function
#End Region
#Region "Legacy non fatal CoreStatus messages"
    Friend Class NonFatal
        Private Shared lNonFatal As List(Of String)
        Friend Shared ReadOnly Property Codes As List(Of String)
            Get
                Return lNonFatal
            End Get
        End Property
        Friend Shared Sub Init()
            lNonFatal = sqdata.LegacyNonFatal
        End Sub
        Friend Shared Sub ResetToDefaults()
            sqdata.UpdateLegacyNonFatal(New List(Of String))
        End Sub
        Friend Shared Sub AddNonFatal(Code As String)
            If Not lNonFatal.Contains(Code) Then
                lNonFatal.Add(Code)
            Else
                WriteLog("Attempt to add an existing non fatal core status", eSeverity.Important)
            End If
        End Sub
        Friend Shared Sub RemoveNonFatal(Code As String)
            If Not lNonFatal.Contains(Code) Then
                WriteLog("Attempt to remove an non existing non fatal core status", eSeverity.Important)
            Else
                lNonFatal.Remove(Code)
                UpdateDB()
            End If
        End Sub
        Friend Shared Sub UpdateDB()
            sqdata.UpdateLegacyNonFatal(lNonFatal)
        End Sub
    End Class
#End Region
#Region "Alternate pSummary url's"
    Friend Class AlternateUrls
        Friend Shared ReadOnly Property Urls As List(Of String)
            Get
                Try
                    Dim rVal As New List(Of String)
                    For Each strName In m_Settings.Keys
                        If strName.Contains("alternate_summary_") Then
                            rVal.Add(m_Settings(strName))
                        End If
                    Next
                    Return rVal
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return New List(Of String)
                End Try
            End Get
        End Property
        Friend Shared Sub RemoveUrl(Url As String)
            Try
                If m_Settings.Values.Contains(Url) Then
                    m_Settings.Remove(m_Settings.Keys(m_Settings.Values.ToList.IndexOf(Url)))
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub AddUrl(Url As String)
            Try
                For xInt As Int32 = 1 To Integer.MaxValue
                    If Not m_Settings.ContainsKey("alternate_summary_" & xInt.ToString) Then
                        m_Settings.Add("alternate_summary_" & xInt.ToString, Url)
                        Exit For
                    End If
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
    End Class
#End Region
#Region "Graph settings"
    Friend Class clsGraphSettings
        Private Shared WithEvents frm As New frmGraphSettings
        Friend Shared Sub ShowOptions(Optional OwnerForm As Form = Nothing)
            If IsNothing(OwnerForm) Then
                frm.Show()
            Else
                frm.ShowDialog(OwnerForm)
            End If
        End Sub
        Private Shared Sub frm_formclosing(sender As Object, e As FormClosingEventArgs) Handles frm.FormClosing
            If (e.CloseReason = CloseReason.UserClosing) Or (e.CloseReason = CloseReason.TaskManagerClosing) Then
                e.Cancel = True
                frm.Hide()
            End If
            SaveSettings()
        End Sub
        Friend Enum GraphStyleEnum
            Unknown
            Individual
            Stacked
        End Enum
        Public Shared Property GrapStyle As GraphStyleEnum
            Get
                Select Case m_Settings("graphstyle")
                    Case Is = "1"
                        Return GraphStyleEnum.Individual
                    Case Is = "2"
                        Return GraphStyleEnum.Stacked
                    Case Else
                        WriteLog("Graphstyle enum now known, " & m_Settings("graphstyle"), eSeverity.Critical)
                        Return GraphStyleEnum.Unknown
                End Select
            End Get
            Set(value As GraphStyleEnum)
                Select Case value
                    Case GraphStyleEnum.Individual
                        ChangeSetting("graphstyle", "1")
                    Case GraphStyleEnum.Stacked
                        ChangeSetting("graphstyle", "2")
                End Select
            End Set
        End Property
        Friend Enum maxPaneItemsEnum
            Unknown
            All
            Five
            Ten
        End Enum
        Public Shared Property maxPaneItems As maxPaneItemsEnum
            Get
                Select Case m_Settings("maxpaneitems")
                    Case Is = "all"
                        Return maxPaneItemsEnum.All
                    Case Is = "5"
                        Return maxPaneItemsEnum.Five
                    Case Is = "10"
                        Return maxPaneItemsEnum.Ten
                    Case Else
                        WriteLog("maxPaneItems unknown enum, " & m_Settings("maxpaneitems"), eSeverity.Critical)
                        Return maxPaneItemsEnum.Unknown
                End Select
            End Get
            Set(value As maxPaneItemsEnum)
                Select Case value
                    Case maxPaneItemsEnum.All
                        ChangeSetting("maxpaneitems", "all")
                    Case maxPaneItemsEnum.Five
                        ChangeSetting("maxpaneitems", "5")
                    Case maxPaneItemsEnum.Ten
                        ChangeSetting("maxpaneitems", "10")
                End Select
            End Set
        End Property
        Public Shared Property minColorTpf As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("mincolortpf")))
            End Get
            Set(value As Color)
                ChangeSetting("mincolortpf", CStr(value.ToArgb))
            End Set
        End Property
        Public Shared Property avgColorTpf As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("avgcolortpf")))
            End Get
            Set(value As Color)
                ChangeSetting("avgcolortpf", CStr(value.ToArgb))
            End Set
        End Property
        Public Shared Property maxColorTpf As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("maxcolortpf")))
            End Get
            Set(value As Color)
                ChangeSetting("maxcolortpf", CStr(value.ToArgb))
            End Set
        End Property
        Public Shared Property minColorPpd As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("mincolorppd")))
            End Get
            Set(value As Color)
                ChangeSetting("mincolorPpd", CStr(value.ToArgb))
            End Set
        End Property
        Public Shared Property avgColorPpd As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("avgcolorppd")))
            End Get
            Set(value As Color)
                ChangeSetting("avgcolorPpd", CStr(value.ToArgb))
            End Set
        End Property
        Public Shared Property maxColorPpd As Color
            Get
                Return Color.FromArgb(CInt(m_Settings("maxcolorppd")))
            End Get
            Set(value As Color)
                ChangeSetting("maxcolorPpd", CStr(value.ToArgb))
            End Set
        End Property
        Friend Shared Sub ResetDefaults()
            'graph settings
            ChangeSetting("mincolorppd", "-4144960")
            ChangeSetting("avgcolorppd", "-16711872")
            ChangeSetting("maxcolorppd", "-8388608")
            ChangeSetting("mincolortpf", "-128")
            ChangeSetting("avgcolortpf", "-8355840")
            ChangeSetting("maxcolortpf", "-65536")
            ChangeSetting("maxpaneitems", "5")
            ChangeSetting("graphstyle", "1")
        End Sub
    End Class
#End Region
#Region "Creation and destruction"
    Friend Shared Sub Init()
        'Call after initializing sqdata!
        Try
            m_firstRun = Not sqdata.HasSettings
            If Not m_firstRun Then
                m_IsUpgrading = sqdata.IsUpgrading
            End If
            GetSettings()
            ColumnSettings.InitMasters()
            ColumnSettings.InitColumns()
            NonFatal.Init()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Shared Sub New()
        'notification level
        m_Settings.Add("notification_level", "1")
        'affinity/priority overrides
        m_Settings.Add("overrideaffinity_priority", Boolean.FalseString)
        'disable all notifications
        m_Settings.Add("notify_disabled", Boolean.FalseString)
        'always track clients
        m_Settings.Add("alwaystrack", Boolean.TrueString)
        'disable crash report
        m_Settings.Add("sendexception", Boolean.TrueString)
        m_Settings.Add("disablecrashreport", Boolean.FalseString)
        'autoresizecolumns
        m_Settings.Add("noautoresizecolumns", Boolean.FalseString)
        'live
        m_Settings.Add("live_parserinterval", CStr(TimeSpan.FromMinutes(3).TotalMilliseconds))
        m_Settings.Add("live_sclivegraph_splitterdistance", "300")
        m_Settings.Add("live_showdetails", Boolean.TrueString)
        m_Settings.Add("live_showsensors", Boolean.FalseString)
        m_Settings.Add("live_etastyle", "date")
        m_Settings.Add("live_selectedtab", "frames")
        m_Settings.Add("live_windowstate", "Normal")
        m_Settings.Add("live_formlocation_x", "0")
        m_Settings.Add("live_formlocation_y", "0")
        m_Settings.Add("live_formsize_width", "0")
        m_Settings.Add("live_formsize_height", "0")
        'displaytimeasutc
        m_Settings.Add("displaytimeasutc", Boolean.TrueString)
        'message strip
        m_Settings.Add("hideinactivemessagestrip", Boolean.FalseString)
        'update history view
        m_Settings.Add("defaultstatistics", "current")
        m_Settings.Add("checkfocus", Boolean.TrueString)
        'graph settings
        m_Settings.Add("mincolorppd", "-4144960")
        m_Settings.Add("avgcolorppd", "-16711872")
        m_Settings.Add("maxcolorppd", "-8388608")
        m_Settings.Add("mincolortpf", "-128")
        m_Settings.Add("avgcolortpf", "-8355840")
        m_Settings.Add("maxcolortpf", "-65536")
        m_Settings.Add("maxpaneitems", "5")
        m_Settings.Add("graphstyle", "1")
        'store delay for ppd calculator
        m_Settings.Add("chkDelay", Boolean.TrueString)
        'use timed log write function
        m_Settings.Add("limit_logwrites", Boolean.TrueString)
        'default form
        m_Settings.Add("defaultview", "historical")
        'http://fah-web.stanford.edu/psummary.html
        'http://calxalot.net/downloads/others/psummary.html
        'http://cftunity.googlecode.com/files/psummary.txt
#If CONFIG = "Debug" Then
        m_Settings.Add("defaultsummary", "http://cftunity.googlecode.com/files/psummary.txt")
        m_Settings.Add("alternate_summary_1", "http://fah-web.stanford.edu/psummary.html")
        m_Settings.Add("alternate_summary_2", "http://calxalot.net/downloads/others/psummary.html")
#Else
        m_Settings.Add("defaultsummary", "http://fah-web.stanford.edu/psummary.html")
        m_Settings.Add("alternate_summary_1", "http://calxalot.net/downloads/others/psummary.html")
#End If
        m_Settings.Add("startwithwindows", Boolean.FalseString)
        m_Settings.Add("startminimized", Boolean.FalseString)
        m_Settings.Add("minimizetotray", Boolean.FalseString)
        m_Settings.Add("startfc", Boolean.FalseString)
        m_Settings.Add("network_port", "49153")
        m_Settings.Add("localname", "local")
        m_Settings.Add("disable_summaryupdater", Boolean.FalseString)
        'Data miner
        m_Settings.Add("eoc_parselog", Boolean.TrueString)
        m_Settings.Add("parseoninterval", Boolean.TrueString)
        m_Settings.Add("parserinterval", New TimeSpan(1, 0, 0).ToString)
        'Notify by 
        m_Settings.Add("notify_by", clsNotifyOptions.eNotifyOption.TrayIcon.ToString)
        'Notify by failure
        m_Settings.Add("notify_fail", Boolean.TrueString)
        'Notify mail
        m_Settings.Add("email_notify", Boolean.FalseString)
        m_Settings.Add("email_provider", "")
        m_Settings.Add("email_custom", "")
        'Notify by eue
        m_Settings.Add("notify_eue", Boolean.FalseString)
        m_Settings.Add("notify_eue_always", Boolean.TrueString)
        m_Settings.Add("notify_eue_rate", Boolean.FalseString)
        m_Settings.Add("notify_rate", "80")
        'Notify by rules
        m_Settings.Add("usefilterrules", Boolean.FalseString)
        'Notifications eoc
        m_Settings.Add("eoc_disable", Boolean.TrueString)
        m_Settings.Add("eoc_notify", Boolean.TrueString)
        m_Settings.Add("eoc_customsignature", "")
        m_Settings.Add("eoc_icon", Boolean.TrueString)
        m_Settings.Add("eoc_interval", New TimeSpan(0, 15, 0).ToString)
        m_Settings.Add("eoc_net_failure", #1/1/2000#.ToString("s"))
        m_Settings.Add("eoc_lastattempt", #1/1/2000#.ToString("s"))
        m_Settings.Add("eoc_primary", String.Empty)
        m_Settings.Add("eoc_viewuser", Boolean.TrueString)
        m_Settings.Add("eoc_viewteam", Boolean.TrueString)
        'history form
        m_Settings.Add("history_tcdetails_splitterdistance", "300")
        m_Settings.Add("history_tcmain_splitterdistance", "400")
        m_Settings.Add("history_viewdetails", Boolean.TrueString)
        m_Settings.Add("history_details_index", "0")
        m_Settings.Add("history_windowstate", "Normal")
        m_Settings.Add("history_formlocation_x", "0")
        m_Settings.Add("history_formlocation_y", "0")
        m_Settings.Add("history_formsize_width", "0")
        m_Settings.Add("history_formsize_height", "0")
        clsNotifyOptions.NotifyEUE = Notify_EUE
        clsNotifyOptions.EUE_by_Rate = Notify_EUE_ByRate
        clsNotifyOptions.EUE_always = Notify_EUE_Always
        clsNotifyOptions.notifyFAILURE = Notify_Failure
        clsNotifyOptions.NotifyRATE = NotifyRate
        clsNotifyOptions.NotifyOption = NotifyOption
        clsNotifyOptions.NotifyRules = Notify_UseRules
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                mFrmNotify.Dispose()
                mfrmOptions.Dispose()
                mFrmMail.Dispose()
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
#End Region
End Class
