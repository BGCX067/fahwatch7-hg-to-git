'   cftUnity EOC handling
'   Copyright (c) 2010-2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports System.Drawing
Imports System.Threading
Friend Class clsEOC
    Implements IDisposable
#Region "xml Updates/Signature"
    Friend Event UpdateRecieved(sender As Object, e As MyEventArgs.EOCUpdateArgs)
    Friend Class clsUpdates
        Friend Structure sUpdates
            Friend Structure sTeam
                Friend TeamName As String
                Friend TeamID As String
                Friend Rank As String
                Friend Users_Active As String
                Friend Users As String
                Friend Change_Rank_24h As String
                Friend Change_Rank_7days As String
                Friend Points_24h_Avg As String
                Friend Points_Update As String
                Friend Points_Today As String
                Friend Points_Week As String
                Friend Points As String
                Friend WUs As String
            End Structure
            Friend Structure sUser
                Friend User_Name As String
                Friend UserID As String
                Friend Team_Rank As String
                Friend Overall_Rank As String
                Friend Change_Rank_24h As String
                Friend Change_Rank_7days As String
                Friend Points_24h_Avg As String
                Friend Points_Update As String
                Friend Points_Today As String
                Friend Points_Week As String
                Friend Points As String
                Friend WUs As String
            End Structure
            Friend Structure sUpdateStatus
                Friend strLastUpdate As String
                Friend strUnixTimeStamp As String
                Friend dtUpdate As DateTime
                Friend Update_Status As String
            End Structure
            Friend Team As sTeam
            Friend User As sUser
            Friend UpdateStatus As sUpdateStatus
        End Structure
        Friend Update As sUpdates
        Friend ReadOnly Property Last_Update_CST() As DateTime
            Get
                Dim iYear As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 1, 4))
                Dim iMonth As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 5, 2))
                Dim iDay As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 7, 2))
                Dim iHours As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 9, 2))
                Dim iMinutes As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 11, 2))
                Dim iSeconds As Int32 = CInt(Mid(Update.UpdateStatus.strLastUpdate, 13, 2))
                Last_Update_CST = New DateTime(iYear, iMonth, iDay, iHours, iMinutes, iSeconds, DateTimeKind.Unspecified)
                Return Last_Update_CST
            End Get
        End Property
        Friend ReadOnly Property Last_Update_UTC() As DateTime
            Get
                Try
                    Dim dtUTC As DateTime = TimeZoneInfo.ConvertTime(Last_Update_CST, System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"), TimeZoneInfo.Utc)
                    Return dtUTC
                Catch ex As Exception
                    'Log to window
                    Return DateTime.MinValue
                End Try
            End Get
        End Property
        Friend ReadOnly Property Last_Update_LocalTime() As DateTime
            Get
                Try
                    Dim dtLocal As DateTime = TimeZoneInfo.ConvertTime(Last_Update_CST, System.TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"), TimeZoneInfo.Local)
                    Return dtLocal
                Catch ex As Exception
                    'Log to window
                    Return DateTime.MinValue
                End Try
            End Get
        End Property
        Friend ReadOnly Property LastUpdateString() As String
            Get
                Try
                    Dim strRet As String = Last_Update_LocalTime.ToShortDateString & " " & Last_Update_LocalTime.ToShortTimeString & " (" & Update.UpdateStatus.Update_Status & ")"
                    Return strRet
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return vbNullString
                End Try
            End Get
        End Property
        Private _IsEmpty As Boolean = True
        Friend Property IsEmpty() As Boolean
            Get
                Return _IsEmpty
            End Get
            Set(ByVal value As Boolean)
                _IsEmpty = value
            End Set
        End Property
    End Class
    Private m_UserName As String, m_TeamNumber As String
    Private m_eocID As String
    Private m_bFilled As Boolean = False
    Private m_bHasImage As Boolean = False
    Private m_bImageRecent As Boolean = False
    Private m_SignatureImage As Image
    Private m_StatsForm As frmEOC
    Private m_bEnabled As Boolean = False
    Private m_imgCustom As String = String.Empty
    Private m_Update As New clsEOC.clsUpdates
    Private Sub LoadLastUpdate()
        Try
            m_Update = sqdata.lasteocupdate(m_UserName, m_TeamNumber)
            m_SignatureImage = sqdata.GetSIGImage(m_UserName, m_TeamNumber)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property LastUpdate() As clsEOC.clsUpdates
        Get
            Return m_Update
        End Get
    End Property
    Friend ReadOnly Property Initialized() As Boolean
        Get
            Return Not LastUpdate.IsEmpty
        End Get
    End Property
    Friend ReadOnly Property ShouldRefresh() As Boolean
        Get
            Dim rVal As Boolean = False
            Try
                If modMySettings.EOC_Net_Failure <> #1/1/2000# AndAlso DateTime.Now.Subtract(modMySettings.EOC_Net_Failure).TotalMinutes < 15 Then
                    WriteLog("EOC xml feed outdated, but last failure less then 15 minutes ago, aborting update", eSeverity.Informative)
                    EOCInfo.Status = "Failed"
                    Exit Try
                End If
                If Not LastUpdate.Update.UpdateStatus.Update_Status = "Current" Or LastUpdate.IsEmpty = True Then
                    'should always refresh but take into account last attempt
                    EOCInfo.Status = "Outdated"
                    If Not modMySettings.eoc_lastattempt = #1/1/2000# Then
                        If DateTime.Now.Subtract(modMySettings.eoc_lastattempt).TotalMinutes > 5 Then
                            writedebug("Last eoc update is not current and last attempt was more then 15m ago, shouldrefresh=true")
                            rVal = True
                            Exit Try
                        Else
                            writedebug("Last eoc update is not current but last attempt was less then 15m ago, shouldrefresh=false")
                            Exit Try
                        End If
                    Else
                        writedebug("Last eoc update is not current and there has been no previous attempt to update, shouldrefresh=true")
                        rVal = True
                        Exit Try
                    End If
                Else
                    'Known update is current, compare update timestamp
                    If CBool(DateTime.Now.Subtract(LastUpdate.Last_Update_LocalTime).Hours >= 3 AndAlso DateTime.Now.Subtract(LastUpdate.Last_Update_LocalTime).Minutes >= 15) Then
                        '3h and 15m should be enough to attempt an update
                        EOCInfo.Status = "Outdated"
                        If Not modMySettings.eoc_lastattempt = #1/1/2000# Then
                            If DateTime.Now.Subtract(modMySettings.eoc_lastattempt).TotalMinutes > 5 Then
                                writedebug("Last eoc update is not current and last attempt was more then 15m ago, shouldrefresh=true")
                                rVal = True
                                Exit Try
                            Else
                                writedebug("Last eoc update is not current but last attempt was less then 15m ago, shouldrefresh=false")
                                EOCInfo.Status = "Failed"
                                Exit Try
                            End If
                        Else
                            writedebug("Last eoc update is not current and there has been no previous attempt to update, shouldrefresh=true")
                            rVal = True
                            Exit Try
                        End If
                    Else
                        EOCInfo.Status = "Current"
                        writedebug("Last eoc update is less then 3 hours and 15 minutes old, shouldrefresh=false")
                        Exit Try
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property SignatureImage As Image
        Get
            Try
                m_SignatureImage = sqdata.GetSIGImage(m_UserName, m_TeamNumber)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return m_SignatureImage
        End Get
    End Property
    Friend ReadOnly Property EocID As String
        Get
            Return m_eocID
        End Get
    End Property
    Friend ReadOnly Property UserName As String
        Get
            Return m_UserName
        End Get
    End Property
    Friend ReadOnly Property TeamNumber As String
        Get
            Return m_TeamNumber
        End Get
    End Property
    Friend Property CustomImage As String
        Get
            Return m_imgCustom
        End Get
        Set(value As String)
            m_imgCustom = value
        End Set
    End Property
    Friend Property Enabled() As Boolean
        Get
            Return m_bEnabled
        End Get
        Set(value As Boolean)
            m_bEnabled = value
        End Set
    End Property
    Private Function AsyncWEBXML() As Boolean
        Try
            modMySettings.eoc_lastattempt = DateTime.Now
            Dim newUpdate As New clsEOC.clsUpdates
            Dim xSettings As XmlReaderSettings = New XmlReaderSettings()
            xSettings.IgnoreComments = True
            xSettings.IgnoreProcessingInstructions = True
            xSettings.IgnoreWhitespace = True
            Dim xResolver As XmlUrlResolver = New XmlUrlResolver()
            xResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
            ' Set the reader settings object to use the resolver.
            xSettings.XmlResolver = xResolver
            modMySettings.eoc_lastattempt = DateTime.Now
            Try
                Using xReader As XmlReader = XmlReader.Create("http://folding.extremeoverclocking.com/xml/user_summary.php?u=" & m_eocID, xSettings)
                    Try
                        xReader.ReadToFollowing("Team_Name")
                    Catch ex As Exception
                        EOCInfo.Status = "Error: " & Err.Number.ToString
                        If Err.Number = 5 Then
                            WriteLog("Eoc xml content not as expected, dump below", eSeverity.Critical)
                            Dim dump As String = xReader.ReadOuterXml
                            Dim dumpL() As String = dump.Split(GetChar(Environment.NewLine, 1))
                            For Each line As String In dumpL
                                WriteLog("DUMP: " & line, eSeverity.Critical)
                            Next
                        End If
                    End Try
                    With newUpdate.Update
                        .Team.TeamName = xReader.ReadElementString
                        .Team.TeamID = xReader.ReadElementString("TeamID")
                        .Team.Rank = xReader.ReadElementString("Rank")
                        .Team.Users_Active = xReader.ReadElementString("Users_Active")
                        .Team.Users = xReader.ReadElementString("Users")
                        .Team.Change_Rank_24h = xReader.ReadElementString("Change_Rank_24hr")
                        .Team.Change_Rank_7days = xReader.ReadElementString("Change_Rank_7days")
                        .Team.Points_24h_Avg = xReader.ReadElementString("Points_24hr_Avg")
                        .Team.Points_Update = xReader.ReadElementString("Points_Update")
                        .Team.Points_Today = xReader.ReadElementString("Points_Today")
                        .Team.Points_Week = xReader.ReadElementString("Points_Week")
                        .Team.Points = xReader.ReadElementString("Points")
                        .Team.WUs = xReader.ReadElementString("WUs")
                        xReader.ReadToFollowing("User_Name")
                        .User.User_Name = xReader.ReadElementString
                        .User.UserID = xReader.ReadElementString("UserID")
                        .User.Team_Rank = xReader.ReadElementString("Team_Rank")
                        .User.Overall_Rank = xReader.ReadElementString("Overall_Rank")
                        .User.Change_Rank_24h = xReader.ReadElementString("Change_Rank_24hr")
                        .User.Change_Rank_7days = xReader.ReadElementString("Change_Rank_7days")
                        .User.Points_24h_Avg = xReader.ReadElementString("Points_24hr_Avg")
                        .User.Points_Update = xReader.ReadElementString("Points_Update")
                        .User.Points_Today = xReader.ReadElementString("Points_Today")
                        .User.Points_Week = xReader.ReadElementString("Points_Week")
                        .User.Points = xReader.ReadElementString("Points")
                        .User.WUs = xReader.ReadElementString("WUs")
                        xReader.ReadToFollowing("Last_Update")
                        .UpdateStatus.strLastUpdate = xReader.ReadElementString
                        .UpdateStatus.strUnixTimeStamp = xReader.ReadElementString("Last_Update_Unix_TimeStamp")
                        .UpdateStatus.Update_Status = xReader.ReadElementString("Update_Status")
                    End With
                    newUpdate.IsEmpty = False
                    m_Update = newUpdate
                End Using
            Catch exW As WebException
                WriteLog("EOC reported the following - " & exW.Status.ToString, eSeverity.Important)
                If Not IsNothing(CType(exW.Response, HttpWebResponse)) Then
                    EOCInfo.Status = CType(exW.Response, HttpWebResponse).StatusDescription
                    WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusCode, eSeverity.Important)
                    WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusDescription, eSeverity.Important)
                Else
                    EOCInfo.Status = exW.Status.ToString
                End If
                modMySettings.EOC_Net_Failure = DateTime.Now
                Me.Enabled = False
            Catch ex As Exception
                EOCInfo.Status = "Error: " & Err.Number
                WriteError(ex.Message, Err)
                modMySettings.EOC_Net_Failure = DateTime.Now
                Me.Enabled = False
            Finally
                xSettings = Nothing
                xResolver = Nothing
            End Try
            Dim imgSig As Image = m_SignatureImage
            Try
                If IsNothing(newUpdate.Update.User.UserID) Or newUpdate.IsEmpty Or newUpdate.Update.UpdateStatus.Update_Status <> "Current" Then Exit Try
                Dim URL As String = EOCInfo.ImageUrl & newUpdate.Update.User.UserID
                If Not CustomImage = "" Then
                    URL &= "&" & CustomImage
                End If
                Dim request As HttpWebRequest = CType(HttpWebRequest.Create(URL), HttpWebRequest)
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    imgSig = Image.FromStream(response.GetResponseStream)
                    m_bImageRecent = True
                    response.Close()
                    response = Nothing
                    request = Nothing
                Else
                    m_bImageRecent = False
                    WriteLog("HTTP responce for EOC signature image not as expected, code =" & response.StatusCode.ToString, eSeverity.Important)
                    modMySettings.EOC_Net_Failure = DateTime.Now
                End If
            Catch exW As System.Net.WebException
                modMySettings.EOC_Net_Failure = DateTime.Now
                m_bImageRecent = False
                EOCInfo.Status = CType(exW.Response, HttpWebResponse).StatusDescription
                WriteLog("WebException while trying to load the new signature image ( url=" & EOCInfo.ImageUrl & newUpdate.Update.User.UserID & " Extra formatting options=" & CustomImage, eSeverity.Critical)
                WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusCode, eSeverity.Important)
                WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusDescription, eSeverity.Important)
            Catch ex As Exception
                modMySettings.EOC_Net_Failure = DateTime.Now
                m_bImageRecent = False
                WriteLog("Exception while trying to load the new signature image ( url=" & EOCInfo.ImageUrl & newUpdate.Update.User.UserID & " Extra formatting options=" & CustomImage, eSeverity.Critical)
                WriteError(ex.Message, Err)
            End Try
            'convert to byte array
            If m_bImageRecent Then
                Dim bArray As Byte()
                Try
                    Dim ms As MemoryStream = New MemoryStream
                    If Not IsNothing(imgSig) Then imgSig.Save(ms, System.Drawing.Imaging.ImageFormat.Gif)
                    ms.Close()
                    bArray = ms.ToArray
                    m_SignatureImage = imgSig
                    imgSig.Dispose()
                    If newUpdate.Update.UpdateStatus.Update_Status = "Current" Then
                        Return SaveToDatabase(newUpdate, bArray)
                    Else
                        WriteLog("Processed EOC update at " & DateTime.Now.ToString("s") & " which has status of " & newUpdate.Update.UpdateStatus.Update_Status & ", aborting store", eSeverity.Important)
                        Return False
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            Else
                Return False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Delegate Function dAsyncWEBXML() As Boolean
    Friend Function ReadWebXml() As Boolean
        Try
            If Not My.Computer.Network.IsAvailable Then
                WriteLog("Network is unavailable, aborting EOC update request", eSeverity.Informative)
                EOCInfo.Status = "Network failure"
                Return False
            End If
            Dim aWebXML As New dAsyncWEBXML(AddressOf AsyncWEBXML)
            Dim Result As IAsyncResult = aWebXML.BeginInvoke(Nothing, Nothing)
            While Not Result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As Boolean = aWebXML.EndInvoke(Result)
            Result.AsyncWaitHandle.Close()
            Return bRes
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            GC.Collect()
        End Try
    End Function
    Private Function SaveToDatabase(ByVal MyUpdate As clsEOC.clsUpdates, ByVal imgArr() As Byte) As Boolean
        Try
            Dim _xmlB As Boolean = False, _sigB As Boolean = False
            With sqdata
                If CType(.GetXMLUpdate(MyUpdate.Last_Update_LocalTime, m_UserName, m_TeamNumber), clsEOC.clsUpdates).IsEmpty Then
                    If .InsertXMLUpdate(MyUpdate) Then
                        WriteLog("Saved EOC XML data to database", eSeverity.Informative)
                        _xmlB = True
                    Else
                        WriteLog("Could not save EOC XML data to database", eSeverity.Important)
                    End If
                Else
                    WriteLog("Saving XML EOC data aborted due to exisiting data", eSeverity.Important)
                    _xmlB = True
                End If
                If Not IsNothing(sqdata.GetSIGImage(UserName, TeamNumber, MyUpdate.Last_Update_LocalTime)) Then
                    WriteLog("Saving Signature image aborted due to exisiting image", eSeverity.Important)
                    _sigB = True
                Else
                    _sigB = .SaveSIGImage(MyUpdate.Last_Update_LocalTime, m_UserName, m_TeamNumber, imgArr)
                End If
                Return CBool(_sigB AndAlso _xmlB)
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Sub New(ByVal UserName As String, ByVal TeamNumber As String, eocID As String, Optional customImg As String = Nothing, Optional Enabled As Boolean = False)
        Try
            'Must have initialized the form
            m_StatsForm = New frmEOC(UserName, TeamNumber)
            m_StatsForm.Size = New Size(0, 0)
            m_bEnabled = Enabled
            If Not IsNothing(customImg) Then m_imgCustom = customImg
            m_StatsForm.Show()
            m_StatsForm.Hide()
            m_UserName = UserName
            m_TeamNumber = TeamNumber
            m_eocID = eocID
            LoadLastUpdate()
            If Not modMySettings.DisableEOC Then
                If ShouldRefresh AndAlso Enabled Then
                    ReadWebXml()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            m_bFilled = False
        End Try
    End Sub
    Friend Function ShowStatsForm(Optional ByVal FadeTimeOut As Double = 5000) As Boolean
        Try
            Dim nI As New frmEOC.ShowSigDelegate(AddressOf m_StatsForm.ShowSig)
            Dim result As IAsyncResult = m_StatsForm.BeginInvoke(nI, {FadeTimeOut})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As Boolean = CBool(m_StatsForm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRes
        Catch ex As Exception
            WriteError("ShowStatsform", Err)
            Return False
        End Try
    End Function
    Friend ReadOnly Property SignatureOnDesktop As Boolean
        Get
            Return m_StatsForm.OnDesktop
        End Get
    End Property
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_StatsForm.Dispose()

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
Friend Class EOCInfo
    Implements IDisposable
    'Share accessibility pointers?
    Private Shared dEOCStats As New Dictionary(Of String, clsEOC)
    Friend Shared ImageUrl As String = "http://folding.extremeoverclocking.com/sigs/sigimage.php?u="
    Private Shared m_bEOCInit As Boolean = False
    Friend Shared bStatsInit As Boolean = False
    Private Shared WithEvents nIcon As New NotifyIcon
    Private Shared WithEvents cMenu As New ContextMenuStrip
    Private Shared m_EOC_Status As String = ""
    Friend Shared Property Status As String
        Get
            If modMySettings.DisableEOC Then
                Return "Disabled"
            Else
                Return m_EOC_Status
            End If
        End Get
        Set(value As String)
            m_EOC_Status = value
            nIcon.Text = "ExtremeOverclocking - " & value
        End Set
    End Property
    Friend Shared ReadOnly Property EOCStats(ByVal UserName As String, ByVal Teamnumber As String) As clsEOC
        Get
            Try
                Dim sKey As String = UserName & "#" & Teamnumber
                If dEOCStats.ContainsKey(sKey) Then
                    Return dEOCStats(sKey)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return Nothing
            End Try
        End Get
    End Property
    Friend Shared ReadOnly Property NoVisible As Int32
        Get
            Try
                Dim iVisible As Int32 = 0
                For Each EOC As clsEOC In dEOCStats.Values
                    If EOC.SignatureOnDesktop Then iVisible += 1
                Next
                Return iVisible
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return 0
            End Try
        End Get
    End Property
    Private Shared m_Accounts As New Dictionary(Of String, sEOCAccount)
    Friend Class sEOCAccount
        Friend Username As String
        Friend Teamnumber As String
        Friend Enabled As Boolean
        Friend customIMG As String
        Friend ID As String
    End Class
    Friend Shared ReadOnly Property eocAccounts As List(Of sEOCAccount)
        Get
            Return m_Accounts.Values.ToList
        End Get
    End Property
    Friend Shared ReadOnly Property HasAccount(Username As String, TeamNumber As String) As Boolean
        Get
            Return m_Accounts.ContainsKey(Username & "#" & TeamNumber)
        End Get
    End Property
    Private Shared Function asyncEOCID(ByRef ID As String, ByVal UN As String, ByVal TN As String) As Boolean
        Dim bRet As Boolean = True
        Dim xSettings As XmlReaderSettings = New XmlReaderSettings()
        xSettings.IgnoreComments = True
        xSettings.IgnoreProcessingInstructions = True
        xSettings.IgnoreWhitespace = True
        Dim xResolver As XmlUrlResolver = New XmlUrlResolver()
        xResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
        ' Set the reader settings object to use the resolver.
        xSettings.XmlResolver = xResolver
        Try
            Using xReader As XmlReader = XmlReader.Create("http://folding.extremeoverclocking.com/xml/user_summary.php?un=" & UN & "&t=" & TN, xSettings)
                'Could access the userID here and use it for every query?
                xReader.ReadToFollowing("UserID")
                ID = xReader.ReadElementString
            End Using
        Catch webExc As Net.WebException
            bRet = False
            If IsNothing(webExc.Response) Then
                WriteLog("Trying to verify a folding account as being available through the ExtremeOverclocking xml updates failed due to an unknown account or a network problem", eSeverity.Critical)
                WriteLog("Setting the account as disabled", eSeverity.Critical)
                WriteError(webExc.Message, Err)
            ElseIf webExc.Status = WebExceptionStatus.ProtocolError And CType(webExc.Response, HttpWebResponse).StatusCode = HttpStatusCode.NotFound Then
                WriteLog("Trying to verify a folding account as being available through the ExtremeOverclocking xml updates failed due to an unknown account or a network problem", eSeverity.Critical)
                WriteLog("Setting the account as disabled", eSeverity.Critical)
                WriteError(webExc.Message, Err)
            End If
        Catch ex As Exception
            WriteLog("Trying to verify a folding account as being available through the ExtremeOverclocking xml updates failed due to a network error", eSeverity.Critical)
            WriteLog("Setting the account as disabled", eSeverity.Critical)
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    Friend Shared ReadOnly Property primaryAccount As clsEOC
        Get
            Return dEOCStats(modMySettings.primaryEocAccount)
        End Get
    End Property
    Private Delegate Function dAsyncEOCID(ByRef ID As String, ByVal UN As String, ByVal TN As String) As Boolean
    Private Shared mME As EOCInfo
    Friend Shared Function AddAccount(UserName As String, TeamNumber As String, Optional customImage As String = Nothing, Optional Enabled As Boolean = False) As Boolean
        Dim bRet As Boolean = False
        'check for webresponse?
        Try
            If HasAccount(UserName, TeamNumber) Then Exit Try
            Dim Account As New sEOCAccount
            Account.Username = UserName
            Account.Teamnumber = TeamNumber
            Account.Enabled = True
            Dim aEOCID As New dAsyncEOCID(AddressOf asyncEOCID)
            Dim result As IAsyncResult = aEOCID.BeginInvoke(Account.ID, UserName, TeamNumber, Nothing, Nothing)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bSucces As Boolean = aEOCID.EndInvoke(Account.ID, result)
            result.AsyncWaitHandle.Close()
            Account.Enabled = bSucces
            m_Accounts.Add(Account.Username & "#" & Account.Teamnumber, Account)
            'bRet = sqdata.SaveEOCAccounts(m_Accounts.Values.ToList)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    Friend Sub New()
        Try
            mME = Me
            nIcon.Icon = My.Resources.iEOC
            nIcon.ContextMenuStrip = cMenu
            nIcon.Visible = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Function ReadAccounts() As Boolean
        Dim bRet As Boolean = True
        Try
            For Each Account In sqdata.EOCAccounts
                If Not m_Accounts.ContainsKey(Account.Username & "#" & Account.Teamnumber) Then m_Accounts.Add(Account.Username & "#" & Account.Teamnumber, Account)
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        Finally
            GC.Collect()
        End Try
        Return bRet
    End Function
    Friend Shared Function Prepare() As Boolean
        Dim bRet As Boolean = True
        Try
            sqdata.SaveEOCAccounts(m_Accounts.Values.ToList)
            cMenu.Items.Add("ExtremeOverclocking F@H Stats")
            cMenu.Items.Add("-")
            For Each Account As sEOCAccount In m_Accounts.Values.ToList
                Dim cM As New ToolStripMenuItem(Account.Username & "(" & Account.Teamnumber & ")")
                Dim dUser As New ToolStripMenuItem("User statistics")
                Dim dTeam As New ToolStripMenuItem("Team statistics")
                cM.DropDownItems.Add(dUser)
                cM.DropDownItems.Add(dTeam)
                Dim cEnabled As New ToolStripMenuItem("Enabled")
                cEnabled.Checked = Account.Enabled
                cM.DropDownItems.Add(cEnabled)
                Dim dSignature As New ToolStripMenuItem("Show signature")
                cM.DropDownItems.Add(dSignature)
                AddHandler cM.DropDownItemClicked, AddressOf cMenu_ItemClicked
                cMenu.Items.Add(cM)
            Next
            cMenu.Items.Add("-")
            cMenu.Items.Add("Close")
            nIcon.ContextMenuStrip = cMenu
            nIcon.Text = "ExtremeOverclocking"
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")> Friend Shared Function Init() As Boolean
        'Keeping the objects in dictionary, can't dispose?
        Dim bRet As Boolean = False
        Try
            For Each Account In sqdata.EOCAccounts
                Try
                    Dim nEOC As New clsEOC(Account.Username, Account.Teamnumber, Account.ID, Account.customIMG, Account.Enabled)
                    dEOCStats.Add(Account.Username & "#" & Account.Teamnumber, nEOC)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                Finally
                    GC.Collect()
                End Try
            Next
            If Not modMySettings.DisableEOC Then
                nIcon.Visible = modMySettings.ShowEOCIcon
                Timers.StartEOCTimer(TimeSpan.FromMinutes(5).TotalMilliseconds)
                If modMySettings.EOCNotify Then
                    EOCInfo.showSigs()
                End If
            End If
            bRet = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    Friend Shared Property IconVisible As Boolean
        Get
            Return nIcon.Visible
        End Get
        Set(value As Boolean)
            nIcon.Visible = value
        End Set
    End Property
    Private Shared Sub nIcon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseClick
        'Pop up all signature images if left click
        Try
            If e.Button = MouseButtons.Left Then
                For Each eocAccount As clsEOC In dEOCStats.Values
                    If eocAccount.Enabled AndAlso eocAccount.LastUpdate.IsEmpty = False And Not eocAccount.SignatureOnDesktop And Not IsNothing(eocAccount.SignatureImage) Then
                        eocAccount.ShowStatsForm()
                    End If
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub cMenu_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cMenu.ItemClicked
        Try
            WriteLog("Eoc traymenu item clicked: " & e.ClickedItem.Text)
            Select Case e.ClickedItem.Text
                Case Is = "Close"
                    nIcon.Visible = False
                Case Is = "ExtremeOverclocking F@H Stats"
                    Process.Start("http://folding.extremeoverclocking.com/")
                Case Else
                    If IsNothing(e.ClickedItem.Tag) Then
                        Dim sAccount As String = e.ClickedItem.OwnerItem.Text.Replace("(", "#").Replace(")", "")
                        If e.ClickedItem.Text = "User statistics" Then
                            Process.Start("http://folding.extremeoverclocking.com/user_summary.php?s=&u=" & dEOCStats(sAccount).LastUpdate.Update.User.UserID)
                        ElseIf e.ClickedItem.Text = "Team statistics" Then
                            Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&t=" & dEOCStats(sAccount).LastUpdate.Update.Team.TeamID)
                        ElseIf e.ClickedItem.Text = "Show signature" Then
                            If Not IsNothing(dEOCStats(sAccount).SignatureImage) Then
                                dEOCStats(sAccount).ShowStatsForm()
                            Else
                                nIcon.ShowBalloonTip(5000, "Error", "There is no image data stored", ToolTipIcon.Error)
                            End If
                        ElseIf e.ClickedItem.Text = "Enabled" Then
                            m_Accounts(sAccount).Enabled = Not m_Accounts(sAccount).Enabled
                            sqdata.SaveEOCAccounts(m_Accounts.Values.ToList)
                        End If
                    End If
            End Select
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub cMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cMenu.Opening
        Try
            cMenu.Items.Clear()
            cMenu.Items.Add("ExtremeOverclocking F@H Stats")
            cMenu.Items.Add("-")
            For Each Account As sEOCAccount In m_Accounts.Values.ToList
                Dim cM As New ToolStripMenuItem(Account.Username & "(" & Account.Teamnumber & ")")
                cM.Tag = "useraccount"
                Dim dUser As New ToolStripMenuItem("User statistics")
                Dim dTeam As New ToolStripMenuItem("Team statistics")
                cM.DropDownItems.Add(dUser)
                cM.DropDownItems.Add(dTeam)
                Dim cEnabled As New ToolStripMenuItem("Enabled")
                cEnabled.Checked = Account.Enabled
                cM.DropDownItems.Add(cEnabled)
                Dim dSignature As New ToolStripMenuItem("Show signature")
                cM.DropDownItems.Add(dSignature)
                AddHandler cM.DropDownItemClicked, AddressOf cMenu_ItemClicked
                cMenu.Items.Add(cM)
            Next
            cMenu.Items.Add("-")
            cMenu.Items.Add("Close")
            nIcon.ContextMenuStrip = cMenu
            nIcon.Text = "ExtremeOverclocking - " & m_EOC_Status
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub showSigs()
        For Each eocAccount As clsEOC In dEOCStats.Values
            If eocAccount.Enabled AndAlso eocAccount.LastUpdate.IsEmpty = False AndAlso Not IsNothing(eocAccount.SignatureImage) Then
                eocAccount.ShowStatsForm()
            End If
        Next
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                nIcon.Visible = False
                nIcon.Dispose()
                For Each DictionaryEntry In dEOCStats
                    DictionaryEntry.Value.Dispose()
                Next
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
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
