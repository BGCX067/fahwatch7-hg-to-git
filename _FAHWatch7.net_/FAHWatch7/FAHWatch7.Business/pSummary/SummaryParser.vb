Imports Microsoft.VisualBasic
Imports System.Threading
Imports log4net
Imports mojoPortal.Business
Imports System.Globalization
Imports System.Net
Imports System.IO
Imports System.Configuration
Imports FAHWatch7.Business
Imports FAHWatch7.Data
Imports FAHWatch7.Core.Definitions

<Serializable()>
Public Class pSummaryParser
    Implements ITaskQueueTask
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(pSummaryParser))
    Private mtaskGuid As Guid = Guid.Empty
    Private msiteGuid As Guid = Guid.Empty
    Private mqueuedBy As Guid = Guid.Empty
    Private mailFrom As String = "psummaryParser@fahwatch7.net"
    Private mailTo As String = "tasknotify@fahwatch7.net"
    Private mailSubject As String = "psummary parser task message"
    Private mailNotifyCompletion As Boolean = True
    Private mTaskName As String = "fw7 pSummary parser"
    Private mTaskCompleteMessage As String = String.Empty
    Private mStatusQueuedMessage As String = "Queued"
    Private mStatusStartedMessage As String = "Started"
    Private mStatusRunningMessage As String = "Running"
    Private mStatusCompleteMessage As String = "Complete"
    Private mCanStop As Boolean = False
    Private mCanResume As Boolean = True
    ' report status every 30 seconds by default
    Private mUpdateFrequency As Integer = 30
    Private iFailedCount As Int32
    Public ReadOnly Property CanResume As Boolean Implements mojoPortal.Business.ITaskQueueTask.CanResume
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property CanStop As Boolean Implements mojoPortal.Business.ITaskQueueTask.CanStop
        Get
            Return False
        End Get
    End Property

    Public Property NotificationFromEmail As String Implements mojoPortal.Business.ITaskQueueTask.NotificationFromEmail
        Get
            Return mailFrom
        End Get
        Set(value As String)
            mailFrom = value
        End Set
    End Property

    Public Property NotificationSubject As String Implements mojoPortal.Business.ITaskQueueTask.NotificationSubject
        Get
            Return mailSubject
        End Get
        Set(value As String)
            mailSubject = value
        End Set
    End Property

    Public Property NotificationToEmail As String Implements mojoPortal.Business.ITaskQueueTask.NotificationToEmail
        Get
            Return mailTo
        End Get
        Set(value As String)
            mailTo = value
        End Set
    End Property

    Public Property NotifyOnCompletion As Boolean Implements mojoPortal.Business.ITaskQueueTask.NotifyOnCompletion
        Get
            Return mailNotifyCompletion
        End Get
        Set(value As Boolean)
            mailNotifyCompletion = value
        End Set
    End Property

    Public Property QueuedBy As System.Guid Implements mojoPortal.Business.ITaskQueueTask.QueuedBy
        Get
            Return mqueuedBy
        End Get
        Set(value As System.Guid)
            mqueuedBy = value
        End Set
    End Property

    Public Property SiteGuid As System.Guid Implements mojoPortal.Business.ITaskQueueTask.SiteGuid
        Get
            Return msiteGuid
        End Get
        Set(value As System.Guid)
            msiteGuid = value
        End Set
    End Property

    Public Property StatusCompleteMessage As String Implements mojoPortal.Business.ITaskQueueTask.StatusCompleteMessage
        Get
            Return mStatusCompleteMessage
        End Get
        Set(value As String)
            mStatusCompleteMessage = value
        End Set
    End Property

    Public Property StatusQueuedMessage As String Implements mojoPortal.Business.ITaskQueueTask.StatusQueuedMessage
        Get
            Return mStatusQueuedMessage
        End Get
        Set(value As String)
            mStatusQueuedMessage = value
        End Set
    End Property

    Public Property StatusRunningMessage As String Implements mojoPortal.Business.ITaskQueueTask.StatusRunningMessage
        Get
            Return mStatusRunningMessage
        End Get
        Set(value As String)
            mStatusRunningMessage = value
        End Set
    End Property

    Public Property StatusStartedMessage As String Implements mojoPortal.Business.ITaskQueueTask.StatusStartedMessage
        Get
            Return mStatusStartedMessage
        End Get
        Set(value As String)
            mStatusStartedMessage = value
        End Set
    End Property

    Public Property TaskCompleteMessage As String Implements mojoPortal.Business.ITaskQueueTask.TaskCompleteMessage
        Get
            Return mTaskCompleteMessage
        End Get
        Set(value As String)
            mTaskCompleteMessage = value
        End Set
    End Property

    Public Property TaskGuid As System.Guid Implements mojoPortal.Business.ITaskQueueTask.TaskGuid
        Get
            Return mtaskGuid
        End Get
        Set(value As System.Guid)
            mtaskGuid = value
        End Set
    End Property

    Public Property TaskName As String Implements mojoPortal.Business.ITaskQueueTask.TaskName
        Get
            Return mTaskName
        End Get
        Set(value As String)
            mTaskName = value
        End Set
    End Property

    Public ReadOnly Property UpdateFrequency As Integer Implements mojoPortal.Business.ITaskQueueTask.UpdateFrequency
        Get
            Return mUpdateFrequency
        End Get
    End Property

#Region "Work"

    Public Sub QueueTask() Implements mojoPortal.Business.ITaskQueueTask.QueueTask
        If Me.TaskGuid <> Guid.Empty Then
            Return
        End If
        Dim task As New TaskQueue()
        task.SiteGuid = Me.SiteGuid
        task.TaskName = Me.TaskName
        task.Status = "Queued"
        task.LastStatusUpdateUTC = DateTime.UtcNow
        Me.TaskGuid = task.NewGuid
        task.SerializedTaskObject = SerializationHelper.SerializeToString(Me)
        task.SerializedTaskType = Me.[GetType]().AssemblyQualifiedName
        task.Save()
    End Sub

    Public Sub StartTask() Implements mojoPortal.Business.ITaskQueueTask.StartTask

        If Me.TaskGuid = Guid.Empty Then
            Return
        End If

        Dim task As New TaskQueue(Me.TaskGuid)

        If task.Guid = Guid.Empty Then
            Return
        End If
        ' task not found
        If Not ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf RunTaskOnNewThread), Me) Then
            Throw New Exception("Couldn't queue the fw7 pSummary parser task on a new thread.")
        End If

        task.Status = "Starting"
        task.StartUTC = DateTime.UtcNow
        task.LastStatusUpdateUTC = DateTime.UtcNow
        task.Save()

        log.Infoformat("Queued pSummary parser on a new thread")

    End Sub

    Public Sub StopTask() Implements mojoPortal.Business.ITaskQueueTask.StopTask
        ReportAborting()
        TaskQueue.Delete(Me.TaskGuid)
    End Sub
    Public Sub ResumeTask() Implements mojoPortal.Business.ITaskQueueTask.ResumeTask
        StartTask()
    End Sub

    Private Shared Sub RunTaskOnNewThread(threadSleepTask As Object)
        If threadSleepTask Is Nothing Then
            Return
        End If
        Dim task As pSummaryParser = DirectCast(threadSleepTask, pSummaryParser)
        log.Infoformat("deserialized pSummaryParser task")
        ' give a little time to make sure the taskqueue was updated after spawning the thread
        Thread.Sleep(10000)
        ' 10 seconds
        task.RunTask()
        log.Infoformat("started pSummaryParser task")
    End Sub
    ''' <summary>
    ''' 'Start summary collection
    ''' </summary>
    ''' <remarks>If I want more task reports I should probably move the actual logic from into this method</remarks>
    Private Sub RunTask()
        Try
            'Clear summary
            FAHWatch7.Business.ProjectInfo.ClearSummary(Me)
            'Innitial populate
            For Each np As pSummary In FAHWatch7.Data.dbProjectInfo.fw7_ReadProjectInfo2
                FAHWatch7.Business.ProjectInfo.AddProject(np)
            Next
            Do
                ReportParsing()
                Dim dtNow As DateTime = DateTime.UtcNow, bHasBeenParsed As Boolean = True
                log.Infoformat("Starting pSummary parser for page: " & ConfigurationManager.AppSettings("urlPsummary"))
                If Not GetProjects(ConfigurationManager.AppSettings("urlPsummary")) Then
                    log.Warn("Failed to parse the pSummary page!")
                    iFailedCount += 1
                    If iFailedCount > 4 Then
                        log.Fatal("To many failures, stopping pSummaryParser task")
                        Exit Sub
                    End If
                End If
                log.Infoformat("Parsing pSummary took " & FAHWatch7.Business.Formatting.Format_ts(DateTime.UtcNow.Subtract(dtNow)))
                FAHWatch7.Business.ProjectInfo.LastParse = DateTime.UtcNow
                Do
                    ReportSleeping()
                    Thread.Sleep(10000)
                    If ShouldStop Then
                        ReportAborting()
                        Exit Sub
                    End If
                Loop Until DateTime.UtcNow.Subtract(FAHWatch7.Business.ProjectInfo.LastParse).TotalMinutes > 15
            Loop
        Catch ex As Exception
            log.Errorformat(ex.message)
        Finally
            Thread.CurrentThread.Abort()
        End Try
    End Sub
    Private Sub ReportAborting()
        Dim task As New TaskQueue(Me.TaskGuid)
        task.Status = "Aborting"
        task.LastStatusUpdateUTC = DateTime.UtcNow
        task.Save()
    End Sub
    Private Sub ReportParsing()
        Dim task As New TaskQueue(Me.TaskGuid)
        task.Status = "running update."
        task.LastStatusUpdateUTC = DateTime.UtcNow
        task.Save()
    End Sub
    Private Sub ReportSleeping()
        Dim task As New TaskQueue(Me.TaskGuid)
        task.Status = "sleeping, last parse: " & FAHWatch7.Business.ProjectInfo.LastParse.ToString & " summary count: " & FAHWatch7.Business.ProjectInfo.SummaryCount.ToString
        task.LastStatusUpdateUTC = DateTime.UtcNow
        task.Save()
    End Sub
    Private ReadOnly Property ShouldStop As Boolean
        Get
            Try
                Dim task As New TaskQueue(Me.TaskGuid)
                Dim rVal As Boolean = IsNothing(task.Guid)
                Return rVal
            Catch ex As Exception
                log.Errorformat(ex.message)
                Return True
            End Try
        End Get
    End Property
#Region "ProjectSummary parser"
    Private Const parseHeader As String = "<TD>Credit</TD><TD>Frames</TD><TD>Code</TD><TD>Description</TD>"
    Private Const parseEnd As String = "</TABLE>"
#Region "Project download and parsing"
    Public Class AsyncDownloader
        Public Function GetText(url As String) As String
            Dim allText As String = vbNullString
            Try
                Dim request As HttpWebRequest = CType(HttpWebRequest.Create(url), HttpWebRequest)
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    Dim rStream As IO.Stream = response.GetResponseStream
                    Dim rReader As StreamReader = New StreamReader(rStream)
                    allText = rReader.ReadToEnd
                End If
            Catch ex As Exception
                log.Errorformat(ex.message)
            End Try
            Return allText
        End Function
    End Class
    Public Delegate Function AsyncDownload(url As String) As String
    Public Shared Function GetProjects(Optional Url As String = "") As Boolean
        If Not My.Computer.Network.IsAvailable Then Return False
        Dim dStart As DateTime = DateTime.Now
        Dim Result As Boolean = True
        Try
            If IsNothing(Url) OrElse Url = "" Then Url = "http://fah-web.stanford.edu/psummary.html"
            log.Infoformat("Attempting to download new project information from " & Url)
            Dim aP As New AsyncDownloader
            Dim caller As New AsyncDownload(AddressOf aP.GetText)

            Dim allText As String = caller.Invoke(Url)
            If InStr(allText, "Currently Running Projects") = 0 Then
                log.Infoformat("Content not as expected, parsing aborted")
                Result = False
                GoTo HandleResult
            Else
                allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
            End If
            If Not ParseProjects(allText) Then
                log.ErrorFormat("Content could not be parsed")
                Result = False
                GoTo HandleResult
            Else
                log.Infoformat("New project info downloaded, starting database update")
                FAHWatch7.Data.dbProjectInfo.fw7_UpdateProjectInfo(FAHWatch7.Business.ProjectInfo.SummaryToDictionary)
                log.Infoformat("Refreshing pSummary collection")
                FAHWatch7.Business.ProjectInfo.LoadProjects()
                FAHWatch7.Business.ProjectInfo.LastParse = DateTime.UtcNow
            End If
HandleResult:
        Catch ex As Exception
            log.Errorformat(ex.message)
            Result = False
        End Try
        Return Result
    End Function
    Public Shared Function ParseProjects(ByVal allText As String) As Boolean
        Try
            Dim colKey As String ' every collection key is the projectnumber
            Dim strTmp As String ' needed because parsing in one time is sh1t
            Dim dtNow As DateTime = DateTime.UtcNow
            Do
                'Cut to after <TD>
                Try
                    allText = Mid(allText, InStr(allText, "<td>") + 4)
                    colKey = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                    If Not FAHWatch7.Business.ProjectInfo.KnownProject(colKey) Then
                        Dim nProject As New pSummary
                        nProject.ProjectNumber = colKey
                        'set blank
                        nProject.WUName = "" : nProject.ServerIP = "" : nProject.PreferredDays = "" : nProject.NumberOfAtoms = "" : nProject.Frames = "" : nProject.FinalDeadline = "" : nProject.Code = "" : nProject.Contact = "" : nProject.Credit = "" : nProject.Description = "" : nProject.kFactor = "" : nProject.dtSummary = #1/1/2000#
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.ServerIP = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.WUName = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.NumberOfAtoms = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.PreferredDays = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.FinalDeadline = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Credit = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Frames = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Code = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Looks like "<a href=http://fah-web.stanford.edu/cgi-bin/fahproject?p=772>Description</a>"
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Description = Mid(strTmp, InStr(strTmp, "http"), InStr(strTmp, ">Des") - 11)
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'looks like "<font size=-1>vvishal</font>"
                        strTmp = Mid(strTmp, InStr(strTmp, ">") + 1)
                        nProject.Contact = Mid(strTmp, 1, Len(strTmp) - 7)
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        'looks like asson</font></td><td>26.40</td></tr>
                        nProject.kFactor = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        nProject.dtSummary = dtNow
                        FAHWatch7.Business.ProjectInfo.AddProject(nProject)
                    Else
                        'cut alltext and override project type, parse regular, comprehensive then beta 
                        allText = Mid(allText, InStr(allText, "</tr>") + 3)
                    End If
                Catch ex As Exception
                    If allText.IndexOf("</tr>") <> -1 Then
                        allText = Mid(allText, allText.IndexOf("</tr>") + 5)
                    Else
                        Exit Do
                    End If
                End Try
                If allText = "" Then Exit Do
                If allText.IndexOf("<td>") = -1 Then Exit Do
            Loop
            Return True
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function
#End Region
#End Region
#End Region
End Class
