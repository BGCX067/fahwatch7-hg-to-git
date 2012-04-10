Imports System.ComponentModel
Imports System.Data.SqlServerCe
Imports System.Data
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization
Imports FAHWatch7.Business
Imports FAHWatch7.Business.Accounts
Imports log4net
Imports System.Configuration
Imports FAHWatch7.Definitions.Definitions
Imports FAHWatch7.Core.Definitions

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://FAHWatch7.net/WebServices/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Services
    Inherits WebService
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(Services))
#Region "submissions"
    Public Function SubmitSummary(Submission As String) As Boolean

    End Function

#End Region

#Region "Reports"
    ''' <summary>
    ''' 'Handles an application exception report
    ''' </summary>
    ''' <param name="ExSource">Source object name</param>
    ''' <param name="ErrNumber">Error number</param>
    ''' <param name="ExMessage">Message</param>
    ''' <param name="StackTrace">Stacktrace</param>
    ''' <param name="ErrTimeStamp">Datetime of exception occurance</param>
    ''' <param name="Client_Version">Client version</param>
    ''' <param name="Application_ID">ApplicationID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod(MessageName:="ReportError")> _
    Public Function ReportError(ExSource As String, ErrNumber As Integer, ExMessage As String, StackTrace As String, ErrTimeStamp As DateTime, Client_Version As String, Application_ID As String) As Boolean
        If Not Accounts.ValidateConnection(Context, Application_ID) Then
            log.Info("Failed to validate connection, aborting store of Error report")
            Return False
        End If
        Try
            Try
                Dim mConnection As SqlCeConnection = New SqlCeConnection(ConfigurationManager.ConnectionStrings("fw7SqlCeExceptionsWrite").ConnectionString)
                mConnection.Open()
                'check if error is already reported for this client
                Dim sql As String = "select * from Reports where  ClientID like '" & Application_ID & "' and ClientVersion like '" & Client_Version & "' and ExSource like '" & ExSource & "' and ErrNumber like '" & ErrNumber & "' and ErrDescription like '" & ExMessage & "' and StrackTrace like '" & StackTrace & "'"
                Dim mCMD As SqlCeCommand = New SqlCeCommand(sql, mConnection)
                Dim mReader As SqlCeDataReader = mCMD.ExecuteReader(CommandBehavior.Default)
                Dim bHas As Boolean = False
                If mReader.Read Then
                    bHas = True
                End If
                If bHas Then
                    'Return true 
                    Return False
                End If
                mReader.Close()
                If mConnection.State <> ConnectionState.Open Then
                    mConnection.Open()
                End If
                sql = "insert into ErrorReports ( ClientID, ClientVersion, ErrSource , ErrNumber, ErrDescription, StackTrace, ErrTimeStamp) values(@ClientID, @ClientVersion, @ErrSource, @ErrNumber, @ErrDescription, @StackTrace,  @error_timestamp)"
                mCMD = New SqlCeCommand(sql, mConnection)
                mCMD.Parameters.AddWithValue("@ClientID", Application_ID)
                mCMD.Parameters.AddWithValue("@ClientVersion", Client_Version)
                mCMD.Parameters.AddWithValue("@ErrSource", ExSource)
                mCMD.Parameters.AddWithValue("@ErrNumber", ErrNumber)
                mCMD.Parameters.AddWithValue("@ErrDescription", ExMessage)
                mCMD.Parameters.AddWithValue("@StackTrace", StackTrace)
                mCMD.Parameters.AddWithValue("@error_timestamp", ErrTimeStamp)
                mCMD.ExecuteNonQuery()
            Catch ex As Exception
                log.Error(ex)
                Return False
            End Try
            Return True
        Catch ex As Exception
            log.Error(ex)
            Return False
        End Try
    End Function
#End Region
#Region "pSummary"
    <WebMethod(enablesession:=True, MessageName:="Get all project changes")> _
    Public Function GetPSummaryChanges() As String
        Try
            If ProjectInfo.DepreciatedCount = 0 Then
                If ProjectInfo.LoadDepreciated And ProjectInfo.SummaryCount > 0 Then GoTo ReturnData
                Return "Error:zero projects"
            Else
ReturnData:
                Dim lPS As List(Of pSummary) = ProjectInfo.DepreciatedProjectsList
                Dim rVal As New ProjectInfo.struct_pSummary
                rVal.pSummaries.AddRange(lPS)
                rVal.Count = lPS.Count
                Dim x As New XmlSerializer(rVal.GetType)
                Dim sw As New IO.StringWriter()
                x.Serialize(sw, rVal)
                Return sw.ToString
            End If
        Catch ex As Exception
            log.Error(ex)
            Return "error:application error"
        End Try
    End Function

#Region "Download project definition(s)"
    ''' <summary>
    ''' 'Get's the complete psummary collection
    ''' </summary>
    ''' <returns>xml serialized psummary</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True, MessageName:="Get all project summaries")> _
    Public Function GetProjectsummary() As String
        Try
            'If Not ValidateConnection(Context, ApplicationId) Then
            '    Return "Error:invalid connection"
            'Else
            If ProjectInfo.Projects.Count = 0 Then
                If ProjectInfo.LoadProjects And ProjectInfo.SummaryCount > 0 Then GoTo ReturnData
                Return "Error:zero projects"
            Else
ReturnData:
                Dim rVal As New ProjectInfo.struct_pSummary
                rVal.pSummaries = ProjectInfo.Projects
                rVal.Count = ProjectInfo.SummaryCount
                Dim x As New XmlSerializer(rVal.GetType)
                Dim sw As New IO.StringWriter()
                x.Serialize(sw, rVal)
                Return sw.ToString
            End If
        Catch ex As Exception
            log.Error(ex)
            Return "Error:application error"
        End Try
    End Function
    ''' <summary>
    ''' 'Get's the project summary for the requested project or an empty psummary
    ''' </summary>
    ''' <param name="ProjectNumber">Project to recieve</param>
    ''' <returns>xml serialized project summary, will be empty if project is not known</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True, MessageName:="Get a project summary for a specific project")> _
    Public Function GetProjectSummaryByNumber(ProjectNumber As String) As String
        Try
            'ApplicationID As String, 
            'If Not ValidateConnection(Context, ApplicationId) Then
            'Return "error:unregisteredconnection"
            'Else
            If ProjectInfo.SummaryCount = 0 AndAlso Not ProjectInfo.LoadProjects Then
                Return "error:noprojects"
            Else
                If Not ProjectInfo.KnownProject(ProjectNumber) Then
                    Return "error:unknown"
                Else
                    If ProjectInfo.KnownProject(ProjectNumber) Then
                        Dim pS As pSummary = ProjectInfo.Project(ProjectNumber)
                        Dim x As New XmlSerializer(pS.GetType)
                        Dim sw As New IO.StringWriter()
                        x.Serialize(sw, pS)
                        Return sw.ToString
                    Else
                        Return "error:unknown"
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error(ex)
            Return "Error:application error"
        End Try
    End Function
#End Region
#End Region
#Region "Validate FAHWatch7 connection"
    ''' <summary>
    ''' 'Validates the current connection to the supplied userID
    ''' </summary>
    ''' <param name="application_id">The application id to validate the connection for</param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' To many invalid request will blacklist the calling ip adress
    ''' </remarks>
    <WebMethod(EnableSession:=True, MessageName:="Validates the current http context")> _
    Public Function Validate(application_id As String) As Boolean
        Try
            Return ValidateConnection(Context, application_id)
        Catch ex As Exception
            log.Error(ex)
            Return False
        End Try
    End Function
#End Region
#Region "Register FAHWatch7 services"
    ' ''' <summary>
    ' ''' 'Register an application instance to use the site services
    ' ''' </summary>
    ' ''' <param name="Name">Username for the site</param>
    ' ''' <param name="Secret">Answer to your secret question</param>
    ' ''' <returns>Application id</returns>
    ' ''' <remarks></remarks>
    <WebMethod(EnableSession:=True, MessageName:="Register an application instance")> _
    Public Function Register(Name As String, Secret As String) As String
        Try
            If IsBlacklisted(RequestIP(Context)) Then
                log.Warn("Attempt to register from a blacklisted ip, Name: " & Name & ", Secret: " & Secret & ", ip: " & RequestIP(Context))
                Return "error:blacklistedip"
            End If

            If Not ValidateUser(Name, Secret) Then
                'don't allow registration
                log.Warn("Attempt to register an FAHWatch7 instance without a forum account, name:" & Name & " Secret:" & Secret & " ip: " & RequestIP(Context))
                Return "error:wrongcredentials"
            End If
            'generate new
            Dim application_id As Guid
            Dim ip As String = RequestIP(Context)
            Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                mConnection.Open()
                Do
                    application_id = Guid.NewGuid
                    'check if error is already reported for this client
                    Dim sql As String = "select * from Instances where application_id like '" & application_id.ToString & "'"
                    Dim mCMD As SqlCeCommand = New SqlCeCommand(sql, mConnection)
                    Dim mReader As SqlCeDataReader = mCMD.ExecuteReader(CommandBehavior.Default)
                    Dim bHas As Boolean = False
                    If mReader.Read Then
                        bHas = True
                    End If
                    mReader.Close()
                    If Not bHas Then
                        'Insert guid, then exit 
                        sql = "insert into Instances (username, application_id, ip_address,last_connected) values(@username, @application_id, @ip_address, @last_connected)"
                        mCMD = New SqlCeCommand(sql, mConnection)
                        mCMD.Parameters.AddWithValue("@username", Name)
                        mCMD.Parameters.AddWithValue("@application_id", application_id.ToString)
                        mCMD.Parameters.AddWithValue("@ip_address", ip)
                        mCMD.Parameters.AddWithValue("@last_connected", DateTime.UtcNow)
                        mCMD.ExecuteNonQuery()
                        log.Info("Created guid for FAHWatch7 instance, User: " & Name & " Secret:" & Secret & " ip: " & ip)
                        Exit Do
                    End If
                Loop
                Return application_id.ToString
            End Using
        Catch ex As Exception
            log.Error(ex)
            Return "error:application error"
        End Try
    End Function
    ''' <summary>
    ''' 'Registers a new ip for the given applicationID/UserName/SecretAnswer 
    ''' </summary>
    ''' <param name="ApplicationID"></param>
    ''' <param name="UserName"></param>
    ''' <param name="SecretAnswer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True, MessageName:="Add the current httpcontexts ip to the validated list")> _
    Public Function RegisterIP(ApplicationID As String, UserName As String, SecretAnswer As String) As Boolean
        Try
            Dim ip As String = RequestIP(Context)
            If IsBlacklisted(ip) Then
                log.Warn("This ip is blacklisted, aborting ip registration for " & ip & " user: " & UserName & " Secret: " & SecretAnswer)
                Return False
            ElseIf Not ValidateUser(UserName, SecretAnswer) Then
                log.Warn("User validation failed, aborting ip registration for " & ip & " user: " & UserName & " Secret: " & SecretAnswer)
                Return False
            Else
                Using mConnection As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_Accounts.sdf") & ";Persist Security Info=False;")
                    mConnection.Open()
                    Dim sql As String = "insert into Instances (UserName, ClientID, IpAddress, LastConnected) values(@username, @application_id, @ip_address, @last_connected)"
                    Dim mCMD As New SqlCeCommand(sql, mConnection)
                    mCMD.Parameters.AddWithValue("@username", UserName)
                    mCMD.Parameters.AddWithValue("@application_id", ApplicationID)
                    mCMD.Parameters.AddWithValue("@ip_address", ip)
                    mCMD.Parameters.AddWithValue("@last_connected", DateTime.UtcNow)
                    mCMD.ExecuteNonQuery()
                    log.Info("Registerd ip for FAHWatch7 instance, application_id: " & ApplicationID & " user: " & UserName & " Secret: " & SecretAnswer & " ip: " & ip)
                    Return True
                End Using
            End If
        Catch ex As Exception
            log.Error(ex)
            Return False
        End Try
    End Function
#End Region
End Class

