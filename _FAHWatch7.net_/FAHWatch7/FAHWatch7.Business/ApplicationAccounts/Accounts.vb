Imports System.Data.SqlServerCe
Imports System.Data
Imports System.Net
Imports System.Web
Imports System.IO
Imports MySql.Data.MySqlClient
Imports MySql.Data
Imports log4net
''' <summary>
''' 'Functions for logging to custom db, application registration and validation, psummary parsing 
''' </summary>
''' <remarks></remarks>
Public Class Accounts
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(Accounts))
#Region "Registration"
    ''' <summary>
    ''' 'Validates the user account
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <param name="Secret"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateUser(UserName As String, Secret As String) As Boolean
        Return True
        'Edit back
        Try
            Dim rVal As Boolean = False
            'Change before compiling for deployment!
            Dim conString As String = "Server=localhost;Database=fahwatch7;Uid=register;Pwd=r3g1s73rw8w00rD"
            'Dim conString As String = "Server=mysql5.fahwatch7.net;Database=fahwatch7;Uid=register;Pwd=r3g1s73rw8w00rD"
            Using mCon As MySqlConnection = New MySqlConnection(conString)
                mCon.Open()
                Dim mCmd As New MySqlCommand("select UserGuid from fahwatch7.mp_users where Name like '" & UserName & "' AND PasswordAnswer like '" & Secret & "'", mCon)
                Dim mReader As MySqlDataReader = mCmd.ExecuteReader
                If mReader.Read Then
                    rVal = True
                End If
                mReader.Close()
                mCon.Close()
            End Using
            Return rVal
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function

#End Region

#Region "Validation"
    ''' <summary>
    ''' 'Return the ip for current context
    ''' </summary>
    ''' <param name="Context">http context for the connection</param>
    ''' <returns>ip address of connected client</returns>
    ''' <remarks></remarks>
    Public Shared Function RequestIP(Context As HttpContext) As String
        Dim ip As String = String.Empty
        If Context.Request.ServerVariables("HTTP_X_FORWARDED_FOR") = String.Empty Then
            ip = Context.Request.ServerVariables("REMOTE_ADDR")
        Else
            ip = Context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        End If
        Return ip
    End Function
    ''' <summary>
    ''' 'Returns boolean value indicating if the ip has been blacklisted/blocked
    ''' </summary>
    ''' <param name="ip"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function IsBlacklisted(ip As String) As Boolean
        Try
            Dim rVal As Boolean = True
            Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                mConnection.Open()
                Dim mCMD As New SqlCeCommand("select * from Blacklisted_ip where ip_address like '" & ip & "' order by beginTime desc", mConnection)
                Dim dIp As New SortedDictionary(Of DateTime, Boolean)
                Using mRDR As SqlCeDataReader = mCMD.ExecuteReader
                    If Not mRDR.Read Then
                        rVal = False
                    Else
                        If Not IsNothing(mRDR.Item("endTime")) OrElse Not IsDBNull(mRDR.Item("endTime")) Then
                            rVal = False
                        End If
                    End If
                End Using
            End Using
            Return rVal
        Catch ex As Exception
            log.Errorformat(ex.message)
            'can't validate, blacklist all connections 
            Return True
        End Try
    End Function
    Public Shared Sub SetBlacklistEndTime(IP As String, Optional dtEnd As DateTime = Nothing)
        Try
            Dim dtBegin As New DateTime
            Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                mConnection.Open()
                Dim mCMD As New SqlCeCommand("select * from Blacklisted_ip where ip_address like '" & IP & "' order by beginTime desc", mConnection)
                Using mRDR As SqlCeDataReader = mCMD.ExecuteReader
                    If mRDR.Read Then
                        dtBegin = CDate(mRDR.Item("beginTime"))
                    Else
                        log.Warn("Can't set end time for blacklisted ip: " & IP & ", can't find the ip!")
                        Return
                    End If
                End Using
                If IsNothing(dtEnd) Then
                    'set it to now
                    mCMD = New SqlCeCommand("set endTime = '" & DateTime.UtcNow & "' where IpAddress like '" & IP & "' and beginTime like '" & dtBegin & "'", mConnection)
                Else
                    'use supplied dt
                    mCMD = New SqlCeCommand("set endTime = '" & dtEnd & "' where IpAddress like '" & IP & "' and beginTime like '" & dtBegin & "'", mConnection)
                End If
                If mCMD.ExecuteNonQuery > 0 Then
                    log.Infoformat("Succesfully unblocked ip: " & IP)
                Else
                    log.Warn("Failed to unblock ip: " & IP)
                End If
            End Using
        Catch ex As Exception
            log.Errorformat(ex.message)
        End Try
    End Sub
    Public Shared Sub RemoveBlacklistedIP(IP As String)
        Try
            If Not IsBlacklisted(IP) Then Return
            Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                mConnection.Open()
                Dim mCMD As New SqlCeCommand("delete from BlacklistedIP where ipAddress like '" & IP & "'", mConnection)
                If mCMD.ExecuteNonQuery > 0 Then
                    log.Infoformat("Removed " & IP & " from blacklist")
                Else
                    log.Infoformat("Failed to remove " & IP & " from blacklist")
                End If
            End Using
        Catch ex As Exception
            log.Errorformat(ex.message)
        End Try
    End Sub
    ''' <summary>
    ''' 'Validates a client connection using the supplied applicationID and ipdress
    ''' </summary>
    ''' <param name="Context">http context of connecting client</param>
    ''' <param name="application_id">supplied application identifier</param>
    ''' <param name="UserName">Optional UserName for forum profile</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateConnection(Context As HttpContext, application_id As String, Optional UserName As String = "") As Boolean
        Return True
        'Edit back
        Try
            Dim ip As String = RequestIP(Context)
            If IsBlacklisted(ip) Then
                log.Warn("Connection from blocked IP " & ip)
                Return False
            End If
            Dim bValid As Boolean = False
            Dim dtNow As DateTime = DateTime.UtcNow
            Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                mConnection.Open()
                Dim sql As String = "select ip_address from Instances where ClientID like '" & application_id.ToString & "'"
                Dim mCMD As SqlCeCommand = New SqlCeCommand(sql, mConnection)
                Dim mReader As SqlCeDataReader = mCMD.ExecuteReader()
                While mReader.Read
                    If CStr(mReader(0)) = ip Then
                        bValid = True
                        Exit While
                    End If
                End While
                mReader.Close()
            End Using
            If bValid = False Then
                'add attempt to db
                Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                    mConnection.Open()
                    Dim mCMD As SqlCeCommand = New SqlCeCommand("insert into UnvalidatedConnections (ipAddress, ClientID, UserName, ConnectionAttempt) values(@ip_address, @application_id, @username, @attempt_datetime)", mConnection)
                    mCMD.Parameters.AddWithValue("@ip_address", ip)
                    mCMD.Parameters.AddWithValue("@application_id", application_id)
                    mCMD.Parameters.AddWithValue("@username", UserName)
                    mCMD.Parameters.AddWithValue("@attempt_timestamp", dtNow)
                    mCMD.ExecuteNonQuery()
                End Using
                'count instances 
                Dim iCount As Int32 = 0
                Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                    mConnection.Open()
                    Dim sql As String = "select count(*) from UnvalidatedConnections where ip_address like '" & ip & "'"
                    Dim mCMD As SqlCeCommand = New SqlCeCommand(sql, mConnection)
                    Dim mReader As SqlCeDataReader = mCMD.ExecuteReader()
                    If mReader.Read Then
                        iCount = CInt(mReader(0))
                    End If
                    mReader.Close()
                End Using
                If iCount > 5 Then
                    'blacklist ip
                    'Dim mCON As New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_ProjectSummary.sdf") & ";Persist Security Info=False;")
                    Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                        mConnection.Open()
                        Dim mCMD As New SqlCeCommand("insert into BlacklistedIP (IpAddress, beginTime) values(@ip, @beginTime)", mConnection)
                        mCMD.Parameters.AddWithValue("@ip", ip)
                        mCMD.Parameters.AddWithValue("@beginTime", DateTime.UtcNow)
                        mCMD.ExecuteNonQuery()
                    End Using
                Else
                    log.Warn("Blocking IP for exceeding max connection attemps(" & iCount.ToString & ") ip: " & ip & " id: " & application_id & " user: " & UserName & " ts: " & dtNow.ToString("s"))
                End If
            Else
                log.Debug("Application connection accepted from " & ip & " applicationID: " & application_id)
                Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                    mConnection.Open()
                    Dim sql As String = "update Instances set LastConnected = '" & dtNow & "' where UserName like '" & UserName & "' and ClientID like '" & application_id & "' and IpAddress like '" & ip & "'"
                    Dim mCMD As New SqlCeCommand(sql, mConnection)
                    If mCMD.ExecuteNonQuery = 1 Then
                        log.Infoformat("Set last login for account to " & dtNow.ToString("s"))
                    Else
                        log.Warn("Failed to set last login for account to " & dtNow.ToString("s"))
                    End If
                End Using
            End If
            Return bValid
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function
#End Region
End Class



