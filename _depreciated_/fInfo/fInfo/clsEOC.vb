'   cftUnity EOC handling
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports System.Xml
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Net
Public Class clsEOC
#Region "xml Updates/Signature"
    <Serializable()>
    Public Class clsUpdates
        <Serializable()>
        Public Structure sUpdates
            <Serializable()>
            Public Structure sTeam
                Public TeamName As String
                Public TeamID As String
                Public Rank As String
                Public Users_Active As String
                Public Users As String
                Public Change_Rank_24h As String
                Public Change_Rank_7days As String
                Public Points_24h_Avg As String
                Public Points_Update As String
                Public Points_Today As String
                Public Points_Week As String
                Public Points As String
                Public WUs As String
            End Structure
            <Serializable()>
            Public Structure sUser
                Public User_Name As String
                Public UserID As String
                Public Team_Rank As String
                Public Overall_Rank As String
                Public Change_Rank_24h As String
                Public Change_Rank_7days As String
                Public Points_24h_Avg As String
                Public Points_Update As String
                Public Points_Today As String
                Public Points_Week As String
                Public Points As String
                Public WUs As String
            End Structure
            <Serializable()>
            Public Structure sUpdateStatus
                Public strLastUpdate As String
                Public strUnixTimeStamp As String
                Public Update_Status As String
            End Structure
            Public Team As sTeam
            Public User As sUser
            Public UpdateStatus As sUpdateStatus
        End Structure
        Public Update As sUpdates
        Public ReadOnly Property Last_Update_CST() As DateTime
            Get
                Dim iYear As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 1, 4))
                Dim iMonth As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 5, 2))
                Dim iDay As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 7, 2))
                Dim iHours As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 9, 2))
                Dim iMinutes As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 11, 2))
                Dim iSeconds As Int16 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 13, 2))
                Last_Update_CST = New DateTime(iYear, iMonth, iDay, iHours, iMinutes, iSeconds, DateTimeKind.Unspecified)
                Return Last_Update_CST
            End Get
        End Property
        'Only use converting properties with .net 3.5       -  darnit Central Daylight Time is not a known timezone on my pc, and I think lot's more. I need utc
        Public ReadOnly Property Last_Update_UTC() As DateTime
            Get
                Try
                    'Dim dtUTC As DateTime = TimeZoneInfo.ConvertTime(Last_Update_CST, System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"), TimeZoneInfo.Utc)
                    'Return dtUTC
                Catch ex As Exception
                    'Log to window
                    Return DateTime.MinValue
                End Try
            End Get
        End Property
        Public ReadOnly Property Last_Update_LocalTime() As DateTime
            Get
                Try
                    'Dim dtLocal As DateTime = TimeZoneInfo.ConvertTime(Last_Update_CST, System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"), TimeZoneInfo.Local)
                    'Return dtLocal
                Catch ex As Exception
                    'Log to window
                    Return DateTime.MinValue
                End Try
            End Get
        End Property
        Public ReadOnly Property LastUpdateString() As String
            Get
                Try
                    Dim strRet As String = Last_Update_LocalTime.ToShortDateString & " " & Last_Update_LocalTime.ToShortTimeString & " (" & Update.UpdateStatus.Update_Status & ")"
                    Return strRet
                Catch ex As Exception
                    LogWindow.WriteError("clsEOC, LastupdateString", Err)
                    Return vbNullString
                End Try
            End Get
        End Property
        Private _IsEmpty As Boolean = True
        Public Property IsEmpty() As Boolean
            Get
                Return _IsEmpty
            End Get
            Set(ByVal value As Boolean)
                _IsEmpty = value
            End Set
        End Property
    End Class
    Public Event UpdateRecieved()
    Private _UserName As String, _TeamNumber As String
    Private _ImageUrl As String = "http://folding.extremeoverclocking.com/sigs/sigimage.php?u="
    Private _bFilled As Boolean = False
    Private _bHasImage As Boolean = False
    Private _bImageRecent As Boolean = False
    Private _SignatureImage As Image
    Private _StatsForm As frmEOC
    Private WithEvents _tForceUpdate As New System.Timers.Timer
    Private Sub _tForceUpdate_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _tForceUpdate.Elapsed
        Try
            If ShouldRefresh Then
                If ReadWebXml() Then
                    Try
                        If Settings.MySettings.EOCNotify Then ShowStatsForm(True, TimeSpan.FromMinutes(1).TotalMilliseconds)
                    Catch ex As Exception
                        LogWindow.WriteError("clsEOC_Timed update-show statsform", Err)
                    End Try
                End If
            End If
        Catch ex As Exception
            LogWindow.WriteError("clsEOC, tForceUpdate.elapsed", Err)
        End Try
    End Sub
    Public ReadOnly Property LastUpdate() As clsEOC.clsUpdates
        Get
            'not familiar enough with sqlite to sort the table, think it would be quicker if done with sql but this works to
            'Dim rUpdate As New clsEOC.clsUpdates
            'Try
            '    Dim StatsDB As clsStatistics = modMAIN.Statistics
            '    Dim TS() As Long = StatsDB.GetXMLUpdateTimeStamps(_UserName, _TeamNumber)
            '    Dim lmax As Long = 0
            '    For xInt As Int16 = 0 To TS.GetUpperBound(0)
            '        If TS(xInt) > lmax Then lmax = TS(xInt)
            '    Next
            '    rUpdate = CType(StatsDB.GetXMLUpdate(lmax, _UserName, _TeamNumber), clsEOC.clsUpdates)
            '    Return rUpdate
            'Catch ex As Exception
            '    LogWindow.WriteError("clsEOC, LastUpdate", Err)
            '    Return rUpdate
            'End Try
        End Get
    End Property
    Public ReadOnly Property WU_Update() As String
        Get
            Try
                'Dim iNew As Double = CDbl(lastupdate in statistics)
                'Dim iOld As Double = CDbl(lastupdate in statistics)
                'Return CStr(iNew - iOld)
            Catch ex As Exception
                LogWindow.WriteError("clsEOC, sUpdates, WU_Update", Err)
                Return vbNullString
            End Try
        End Get
    End Property
    Public ReadOnly Property Initialized() As Boolean
        Get
            Return _bFilled
        End Get
    End Property
    Public ReadOnly Property ShouldRefresh() As Boolean
        Get
            Try
                Dim lUpd As clsEOC.clsUpdates = LastUpdate
                If lUpd.IsEmpty Then Return True
                If DateTime.Now.Subtract(lUpd.Last_Update_LocalTime).TotalHours > 3 Then
                    Return True
                Else
                    _bFilled = True
                    Return False
                End If
            Catch ex As Exception
                LogWindow.WriteError("clsEOC, ShouldRefresh", Err)
                Return True
            End Try
        End Get
    End Property
    Public Function RefreshEOC() As Boolean
        Try
            If Not ShouldRefresh Then Return False
            Return ReadWebXml()
        Catch ex As Exception
            'Log to window
            Return False
        End Try
    End Function
    Public ReadOnly Property SignatureImage As Image
        Get
            'Try
            '    If _SignatureImage.PropertyItems.Count > 0 Then Return _SignatureImage
            'Catch ex As Exception
            '    Dim statsDB As clsStatistics = modMAIN.Statistics
            '    _SignatureImage = statsDB.GetSIGImage(_UserName, _TeamNumber)
            '    statsDB = Nothing
            'End Try
            'Return _SignatureImage
        End Get
    End Property
    Private _NetFailure As DateTime = DateTime.MinValue
    Public ReadOnly Property LastUpdateFailure As DateTime
        Get
            Return _NetFailure
        End Get
    End Property
    Private Function ReadWebXml(Optional ByVal UserName As String = "", Optional ByVal TeamNumber As String = "") As Boolean
        Try
            If LastUpdateFailure <> DateTime.MinValue Then
                If DateTime.Now.Subtract(LastUpdateFailure).TotalMinutes < 15 Then
                    LogWindow.WriteLog("EOC xml feed outdated, but last failure less then 15 minutes ago, aborting update")
                    Return False
                End If
            End If
            Try
                If Not My.Computer.Network.Ping("extremeoverclocking.com") Then
                    LogWindow.WriteLog("Could not read xml feed from EOC, site seems down")
                    Return False
                Else
                    _NetFailure = DateTime.MinValue
                End If
            Catch ex As Exception
                LogWindow.WriteError("Trying to update EOC xml feed, ping error.", Err)
                Return False
            End Try

            If UserName = "" Then UserName = _UserName
            If TeamNumber = "" Then TeamNumber = _TeamNumber
            Dim newUpdate As New clsEOC.clsUpdates
            Dim xSettings As XmlReaderSettings = New XmlReaderSettings()
            xSettings.IgnoreComments = True
            xSettings.IgnoreProcessingInstructions = True
            xSettings.IgnoreWhitespace = True
            Dim xResolver As XmlUrlResolver = New XmlUrlResolver()
            xResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
            ' Set the reader settings object to use the resolver.
            xSettings.XmlResolver = xResolver
            Using xReader As XmlReader = XmlReader.Create("http://folding.extremeoverclocking.com/xml/user_summary.php?un=" & UserName & "&t=" & TeamNumber, xSettings)
                xReader.ReadToFollowing("Team_Name")
                With newUpdate.Update
                    .Team.TeamName = xReader.ReadElementString
                    .Team.TeamID = xReader.ReadElementString
                    .Team.Rank = xReader.ReadElementString
                    .Team.Users_Active = xReader.ReadElementString
                    .Team.Users = xReader.ReadElementString
                    .Team.Change_Rank_24h = xReader.ReadElementString
                    .Team.Change_Rank_7days = xReader.ReadElementString
                    .Team.Points_24h_Avg = xReader.ReadElementString
                    .Team.Points_Update = xReader.ReadElementString
                    .Team.Points_Today = xReader.ReadElementString
                    .Team.Points_Week = xReader.ReadElementString
                    .Team.Points = xReader.ReadElementString
                    .Team.WUs = xReader.ReadElementString
                    xReader.ReadToFollowing("User_Name")
                    .User.User_Name = xReader.ReadElementString
                    .User.UserID = xReader.ReadElementString
                    .User.Team_Rank = xReader.ReadElementString
                    .User.Overall_Rank = xReader.ReadElementString
                    .User.Change_Rank_24h = xReader.ReadElementString
                    .User.Change_Rank_7days = xReader.ReadElementString
                    .User.Points_24h_Avg = xReader.ReadElementString
                    .User.Points_Update = xReader.ReadElementString
                    .User.Points_Today = xReader.ReadElementString
                    .User.Points_Week = xReader.ReadElementString
                    .User.Points = xReader.ReadElementString
                    .User.WUs = xReader.ReadElementString
                    xReader.ReadToFollowing("Last_Update")
                    .UpdateStatus.strLastUpdate = xReader.ReadElementString
                    .UpdateStatus.strUnixTimeStamp = xReader.ReadElementString
                    .UpdateStatus.Update_Status = xReader.ReadElementString
                End With
                newUpdate.IsEmpty = False
            End Using
            xSettings = Nothing
            xResolver = Nothing
            Dim URL As String = _ImageUrl & newUpdate.Update.User.UserID
            Dim request As HttpWebRequest = WebRequest.Create(URL)
            Dim response As HttpWebResponse = request.GetResponse()
            Dim fBuff() As Byte
            If response.StatusCode = HttpStatusCode.OK Then
                Dim reader As BinaryReader = New BinaryReader(response.GetResponseStream())
                ReDim fBuff(0 To response.ContentLength)
                reader.Read(fBuff, 0, response.ContentLength)
                reader.Close()
                reader = Nothing
                'convert to image
                Dim ImageStream As MemoryStream
                Try
                    If fBuff.GetUpperBound(0) > 0 Then
                        ImageStream = New MemoryStream(fBuff)
                        _SignatureImage = Image.FromStream(ImageStream)
                        _bHasImage = True
                    Else
                        _SignatureImage = Nothing
                        _bHasImage = False
                    End If
                Catch ex As Exception
                    LogWindow.WriteError("ReadWebXML", Err)
                    _SignatureImage = Nothing
                    _bHasImage = False
                End Try
                ImageStream = Nothing
            Else
                LogWindow.WriteLog("HTTP responce for EOC signature image not as expected, code =" & response.StatusCode.ToString)
            End If
            request = Nothing
            response = Nothing
            _bFilled = True
            Return SaveToDatabase(newUpdate, fBuff)
        Catch ex As System.Net.WebException
            LogWindow.WriteError("clsEOC, readwebxml, webException", Err)
            Return False
        Catch ex As Exception
            LogWindow.WriteError("clsEOC, readwebxml", Err)
            Return False
        End Try
    End Function
    Private Function SaveToDatabase(ByVal MyUpdate As clsEOC.clsUpdates, ByVal imgArr() As Byte) As Boolean
        'Try
        '    Dim StatsDB As clsStatistics = modMAIN.Statistics
        '    Dim _xmlB As Boolean = False, _sigB As Boolean = False
        '    With StatsDB
        '        If CType(.GetXMLUpdate(CLng(MyUpdate.Update.UpdateStatus.strUnixTimeStamp), _UserName, _TeamNumber), clsEOC.clsUpdates).IsEmpty Then
        '            If .InsertXMLUpdate(MyUpdate) Then
        '                LogWindow.WriteLog("Saved EOC XML data to database")
        '                _xmlB = True
        '            Else
        '                LogWindow.WriteLog("Could not save EOC XML data to database")
        '            End If
        '        Else
        '            LogWindow.WriteLog("Saving XML EOC data aborted due to exisiting data")
        '            _xmlB = True
        '        End If
        '        _sigB = .SaveSIGImage(MyUpdate.Update.UpdateStatus.strUnixTimeStamp, _UserName, _TeamNumber, imgArr)
        '        Return _sigB And _xmlB
        '    End With
        'Catch ex As Exception
        '    LogWindow.WriteError("clsEOC_SaveToDatabase", Err)
        '    Return False
        'End Try
    End Function
    Public Sub New(ByVal UserName As String, ByVal TeamNumber As String)
        Try
            With _tForceUpdate
                .AutoReset = True
                .Interval = TimeSpan.FromMinutes(5).TotalMilliseconds
            End With
            _StatsForm.Size = New Size(0, 0)
            _StatsForm.Show()
            _StatsForm.Hide()
            _UserName = UserName
            _TeamNumber = TeamNumber
            If ShouldRefresh Then ReadWebXml()
            _StatsForm = New frmEOC(UserName, TeamNumber, LastUpdate.Update.User.UserID)
            _tForceUpdate.Enabled = True
        Catch ex As Exception
            LogWindow.WriteError("clsEOC, sub New(" & _UserName & "," & _TeamNumber & ")", Err)
            _bFilled = False
        End Try
    End Sub
    Public Function ShowStatsForm(Optional ByVal bTrayIcon As Boolean = True, Optional ByVal FadeTimeOut As Double = 5000) As Boolean
        Try
            Return _StatsForm.ShowSig(5000, bTrayIcon)
        Catch ex As Exception
            LogWindow.WriteError("ShowStatsform", Err)
            Return False
        End Try
    End Function
    Public ReadOnly Property SignatureOnDesktop As Boolean
        Get
            Return _StatsForm.OnDesktop
        End Get
    End Property
#End Region
End Class
Public Class clsEOCInfo
    Private WithEvents nIcon As New NotifyIcon
    Private colEOCStats As New Collection
    Private WithEvents _EOCSTATS As clsEOC
    Private _bEOCInit As Boolean = False
    'Eoc trayform
    Private _StatsForm As frmEOC
    Public bStatsInit As Boolean = False
    Public Property EocID As String
    Private Structure sEoc
        Public Username As String
        Public Teamnumber As String
    End Structure
    Public Event UpdateRecieved(ByVal EOCStats As clsEOC)
    Public ReadOnly Property IsIconVisible As Boolean
        Get
            Return nIcon.Visible()
        End Get
    End Property
    Public Sub ShowIcon(Optional TheIcon As Icon = Nothing)
        If Not IsNothing(TheIcon) Then
            nIcon.Icon = TheIcon
        Else
            nIcon.Icon = My.Resources.fTray_EOC
        End If
    End Sub
    Public Property IconText As String
        Get
            Return nIcon.Text
        End Get
        Set(value As String)
            nIcon.Text = value
        End Set
    End Property
    Public Sub ShowBalloon(TimeOut As Int32, Title As String, Text As String, Optional TheIcon As ToolTipIcon = Nothing)
        If Not nIcon.Visible Then Exit Sub
        If Not IsNothing(TheIcon) Then nIcon.BalloonTipIcon = TheIcon
        nIcon.ShowBalloonTip(TimeOut, Title, Text, TheIcon)
    End Sub
    Public ReadOnly Property EOCStats(ByVal UserName, ByVal Teamnumber) As clsEOC
        Get
            Try
                If colEOCStats.Contains(UserName & "-" & Teamnumber) Then Return CType(colEOCStats(UserName & "-" & Teamnumber), clsEOC)
                _EOCSTATS = New clsEOC(UserName, Teamnumber)
                colEOCStats.Add(_EOCSTATS, UserName & "-" & Teamnumber)
                Return _EOCSTATS
            Catch ex As Exception
                LogWindow.WriteError("clsEOC_EOCStats", Err)
                Return Nothing
            End Try
        End Get
    End Property
    Public ReadOnly Property NoVisible As Int16
        Get
            Try
                Dim iVisible As Int16 = 0
                For Each EOC As clsEOC In colEOCStats
                    If EOC.SignatureOnDesktop Then iVisible += 1
                Next
                Return iVisible
            Catch ex As Exception
                LogWindow.WriteError("clsEoc_NoVisible", Err)
                Return 0
            End Try
        End Get
    End Property

    Private Sub _EOCSTATS_UpdateRecieved() Handles _EOCSTATS.UpdateRecieved
        RaiseEvent UpdateRecieved(_EOCSTATS)
    End Sub
End Class
