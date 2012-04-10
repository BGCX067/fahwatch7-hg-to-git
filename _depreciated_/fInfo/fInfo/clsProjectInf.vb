Imports ProjectInfo
Public Class clsProjectInf
    Public Event Log(Message As String)
    Public Event LogError(Message As String, ErrObj As ErrObject)
    Private WithEvents Form As New frmPBList
    Public WithEvents ProjectInfo As clsProjectInfo
    Public Sub Init()
        ProjectInfo = New clsProjectInfo()
        AddHandler ProjectInfo.Log, AddressOf LogWindow_Log
        AddHandler ProjectInfo.LogError, AddressOf LogWindow_LogError
    End Sub
    Public Sub Close()
        Form.CloseForm()
        MyBase.Finalize()
    End Sub
    Public Function GetProjects(Optional URL As String = "", Optional ShowUI As Boolean = False) As Boolean
        Return ProjectInfo.GetProjects(URL, ShowUI)
    End Function
    Public ReadOnly Property Projects As clsProjectInfo.sProject
        Get
            Return ProjectInfo.Projects
        End Get
    End Property
    Public Function Purge() As Boolean
        Return ProjectInfo.Purge
    End Function
    Public Function RemoveProject(ProjectNumber As String) As Boolean
        Return ProjectInfo.RemoveProject(ProjectNumber)
    End Function
    Public Sub ShowBrowser()
        Form.FillView()
        Form.Show()
        Form.Focus()
    End Sub
    Public Sub HideBrowser()
        Form.Visible = False
    End Sub
    Public ReadOnly Property IsFormActive As Boolean
        Get
            Return Form.Visible
        End Get
    End Property
#Region "Database access"
    Public Function UpdateDatabase() As Boolean
        Return ExtClient.Data.Update_ProjectInfo(Me.ProjectInfo)
    End Function
    Public Function ReadDatabase() As Boolean
        Me.ProjectInfo.Projects = ExtClient.Data.ReadProjects
        Return Not Projects.IsEmpty
    End Function
#End Region

#Region "Log extender"
    Public Class cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public WithEvents LogWindow As New cLW
    Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub
#End Region
End Class
