Imports System.Web
Imports System.Web.Services
Imports FAHWatch7.Business
Imports mojoPortal.Business
Imports System.Configuration
Imports log4net
Imports mojoPortal.Web

Public Class webScheduleHandler
    Implements System.Web.IHttpHandler
    Private Shared log As ILog = log4net.LogManager.GetLogger("webScheduleHandler")
    Private Shared iFailed As Int32 = 0
    Private Shared myID As Guid = Guid.NewGuid
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim xmlString As String = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "utf-16" & Chr(34) & "?><status>"
        log.Info("webScheduleHanlder called from: " & context.Request.UserHostAddress & " failed count: " & iFailed & " Request:" & context.Request.QueryString.ToString)

        If iFailed > 3 Then
            xmlString &= "aborted(" & iFailed.ToString & ")</status>"
            GoTo SendResponse
        End If
        Dim pCrawler As New pSummaryParser
        If Not pSummaryParser.GetProjects("http://fah-web.stanford.edu/psummary.html") Then
            iFailed += 1
            log.Info("Incremented iFailed to " & iFailed.ToString)
        End If
        xmlString &= "Succes</status>"
        GoTo SendResponse
        '
        Try
            If context.Request.QueryString IsNot Nothing Then
                Dim bRunning As Boolean = False
                If String.Equals(context.Request.QueryString.ToString, "start", StringComparison.InvariantCultureIgnoreCase) Then
                    If TaskQueue.UnfinishedTaskExists("fw7 pSummary parser") Then
                        bRunning = True
                        xmlString &= "AlreadyRunning</status>"
                    Else
                        Dim task As New pSummaryParser
                        task.TaskGuid = New Guid
                        task.QueuedBy = myID
                        task.QueueTask()
                        Threading.Thread.Sleep(10000)
                        WebTaskManager.StartOrResumeTasks()
                        xmlString &= "Starting</status>"
                    End If
                ElseIf String.Equals(context.Request.QueryString.ToString, "stop", StringComparison.InvariantCultureIgnoreCase) Then
                    If TaskQueue.UnfinishedTaskExists("fw7 pSummary parser") Then
                        TaskQueue.DeleteByType("fw7 pSummary parser")
                        xmlString &= "Stopping</status>"
                    Else
                        xmlString &= "NotRunning</status>"
                    End If
                Else
                    GoTo BeatenPath
                End If
            Else
BeatenPath:
                'If iFailed > 3 Then
                '    xmlString &= "aborted(" & iFailed.ToString & ")</status>"
                '    GoTo SendResponse
                'End If
                'Dim pCrawler As New pSummaryParser
                'If Not pSummaryParser.GetProjects("http://fah-web.stanford.edu/psummary.html") Then
                '    iFailed += 1
                '    log.Info("Incremented iFailed to " & iFailed.ToString)
                'End If
                'xmlString &= "Succes</status>"
            End If
        Catch ex As Exception
            log.Error(ex)
            iFailed += 1
            xmlString &= "ex:" & ex.Message & "</status>"
        End Try
SendResponse:
        context.Response.AddHeader("Content-Length", CStr(xmlString.Length))
        context.Response.AddHeader("Connection", "close")
        context.Response.ContentType = "text/xml"
        context.Response.Write(xmlString)
        context.Response.Flush()
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class