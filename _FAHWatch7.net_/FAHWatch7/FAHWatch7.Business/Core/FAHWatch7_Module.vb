Imports System.Web
Imports mojoPortal
Imports mojoPortal.Web
Imports FAHWatch7.Data
Imports log4net
Partial Public Class FAHWatch7_Module
    Implements IHttpModule
    Private Shared WithEvents _context As HttpApplication
    Private Shared bHasStarted As Boolean = False
    Private Shared log As ILog = LogManager.GetLogger(GetType(FAHWatch7_Module))
#Region "IHttpModule Members"
    Public Sub Dispose() Implements IHttpModule.Dispose
        ' Clean-up code here
        ' Clear table of pSummaryParser
    End Sub
    Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
        _context = context
        If Not bHasStarted Then
            InitDB()
            StartParser()
        End If
    End Sub
#End Region
#Region "Initialize application"
    Private Sub InitDB()
        log.InfoFormat("Init accounts: " & Data.Accounts.InitDB().ToString)
        log.InfoFormat("Init connections: " & Data.Connections.InitDB().ToString)
        log.InfoFormat("Init psummary: " & Data.dbProjectInfo.InitDB().ToString)
        log.InfoFormat("Init statistics: " & Data.Statistics.InitDB().ToString)
        If My.Computer.FileSystem.FileExists(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_ProjectSummary.sdf")) Then
            If Data.dbProjectInfo.ImportSQLCE Then
                Try
                    My.Computer.FileSystem.DeleteFile(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_ProjectSummary.sdf"))
                Catch ex As Exception
                    log.ErrorFormat("Exception while trying to remove the pSummary SQLCe database file: {9}", ex.Message)
                End Try
                log.InfoFormat("Sucessfully imported SQLCe psummary table")
            Else
                log.Warn("Failed to import the SQLCe psummary table")
            End If
        End If
        If My.Computer.FileSystem.FileExists(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/psummary.tab")) Then
            If Data.dbProjectInfo.ImportTabbed(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/psummary.tab")) Then
                Try
                    My.Computer.FileSystem.DeleteFile(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/psummary.tab"))
                Catch ex As Exception
                    log.ErrorFormat("Exception while trying to remove the psummary.tab file: {0}", ex.Message)
                End Try
            End If
        End If
        log.InfoFormat("Loading pSummary: " & Business.ProjectInfo.LoadProjects().ToString)
        log.InfoFormat("Loading pSummary change log: " & Business.ProjectInfo.LoadDepreciated().ToString)
    End Sub
    Private Sub StartParser()
        Dim sParser As New pSummaryParser
        sParser.QueueTask()
        bHasStarted = True
        WebTaskManager.StartOrResumeTasks()
    End Sub
#End Region
#Region "Request events"
    Private Shared Sub _context_BeginRequest(sender As Object, e As System.EventArgs) Handles _context.BeginRequest
        Debug.WriteLine("Begin request: " & _context.Request.Url.AbsoluteUri)
    End Sub
    Private Shared Sub _context_EndRequest(sender As Object, e As System.EventArgs) Handles _context.EndRequest
        Debug.WriteLine("End request: " & _context.Request.Url.AbsoluteUri)
    End Sub
#End Region
    Public Shared Sub OnLogRequest(ByVal source As Object, ByVal e As EventArgs) Handles _context.LogRequest
        ' Handles the LogRequest event to provide a custom logging 
        ' implementation for it
    End Sub
End Class


