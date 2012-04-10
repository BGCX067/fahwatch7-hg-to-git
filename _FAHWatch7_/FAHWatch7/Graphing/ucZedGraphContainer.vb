Imports System.Drawing
Imports FAHWatch7.clsStatistics
Imports ZedGraph
Imports System.ComponentModel

Friend Class ucZedGraphContainer
    Friend graphGenerator As New zedGraphGenerator
#Region "Buttons"
#Region "painting"
    Private Sub cmdDown_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles cmdDownMinor.Paint, cmdDownMajor.Paint
        Try
            Dim g As Graphics = e.Graphics
            Dim drawformat As System.Drawing.StringFormat
            If scMain.Orientation = Orientation.Vertical Then
                drawformat = New StringFormat(StringFormatFlags.DirectionVertical)
            Else
                drawformat = New StringFormat(StringFormatFlags.DirectionVertical)
            End If
            drawformat.Alignment = StringAlignment.Center
            drawformat.LineAlignment = StringAlignment.Center
            If ReferenceEquals(sender, cmdDownMinor) Then
                g.DrawString(">>", New Font("Sans Serrif", 10), Brushes.Black, CType(sender, Button).ClientRectangle, drawformat)
            Else
                g.DrawString(">>>>", New Font("Sans Serrif", 10), Brushes.Black, CType(sender, Button).ClientRectangle, drawformat)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdUp_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles cmdUpMajor.Paint, cmdUpMinor.Paint
        Try
            Dim g As Graphics = e.Graphics
            Dim drawformat As System.Drawing.StringFormat
            If scMain.Orientation = Orientation.Vertical Then
                drawformat = New StringFormat(StringFormatFlags.DirectionVertical)
            Else
                drawformat = New StringFormat(StringFormatFlags.DirectionVertical)
            End If
            drawformat.Alignment = StringAlignment.Center
            drawformat.LineAlignment = StringAlignment.Center
            If ReferenceEquals(sender, cmdUpMinor) Then
                g.DrawString("<<", New Font("Sans Serrif", 10), Brushes.Black, CType(sender, Button).ClientRectangle, drawformat)
            Else
                g.DrawString("<<<<", New Font("Sans Serrif", 10), Brushes.Black, CType(sender, Button).ClientRectangle, drawformat)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    Public Event UpMajor(sender As Object, e As System.EventArgs)
    Public Event UpMinor(sender As Object, e As System.EventArgs)
    Public Event DownMajor(sender As Object, e As System.EventArgs)
    Public Event DownMinor(sender As Object, e As System.EventArgs)
    <BrowsableAttribute(True)>
    Public WriteOnly Property UpMinorEnabled As Boolean
        Set(value As Boolean)
            cmdUpMajor.Enabled = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property UpMajorEnabled As Boolean
        Set(value As Boolean)
            cmdUpMajor.Enabled = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property DownMinorEnabled As Boolean
        Set(value As Boolean)
            cmdDownMinor.Enabled = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property DownMajorEnabled As Boolean
        Set(value As Boolean)
            cmdDownMajor.Enabled = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property UpMinorVisible As Boolean
        Set(value As Boolean)
            cmdUpMajor.Visible = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property UpMajorVisible As Boolean
        Set(value As Boolean)
            cmdUpMajor.Visible = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property DownMinorVisible As Boolean
        Set(value As Boolean)
            cmdDownMinor.Visible = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property DownMajorVisible As Boolean
        Set(value As Boolean)
            cmdDownMajor.Visible = value
        End Set
    End Property
    <BrowsableAttribute(True)>
    Public WriteOnly Property ButtonsVisible As Boolean
        Set(value As Boolean)
            scMain.Panel1Collapsed = value
        End Set
    End Property
#End Region
#Region "zedGraph"
    Friend WithEvents zg As ZedGraph.ZedGraphControl = zedGraph
    ''' <summary>
    ''' 'Graph mode so pointvalue event can be used
    ''' 'Frames -> display frame completion time and timespan
    ''' 'Projects -> display ppd and timespan
    ''' 'TpfCredit -> display tpf and credit
    ''' 'Performance -> ignored, use labels
    ''' </summary>
    ''' <remarks></remarks>
    Friend Enum GraphModeEnum
        Frames
        Projects
        Performance
        TpfCredit
    End Enum
    <BrowsableAttribute(True)>
    Friend Property GraphMode As GraphModeEnum = GraphModeEnum.Frames
    ''' <summary>
    ''' 'Capture mousewheel and scroll or pan?
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub zg_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles zg.MouseWheel
        If e.Delta = 1 Then
            RaiseEvent UpMinor(Me, New MyEventArgs.EmptyArgs)
        ElseIf e.Delta > 1 Then
            RaiseEvent UpMajor(Me, New MyEventArgs.EmptyArgs)
        ElseIf e.Delta = -1 Then
            RaiseEvent DownMinor(Me, New MyEventArgs.EmptyArgs)
        ElseIf e.Delta < -1 Then
            RaiseEvent DownMajor(Me, New MyEventArgs.EmptyArgs)
        End If
    End Sub

    ''' <summary>
    ''' 'Show point values
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="pane"></param>
    ''' <param name="curve"></param>
    ''' <param name="iPt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function zg_PointValueEvent(sender As ZedGraph.ZedGraphControl, pane As ZedGraph.GraphPane, curve As ZedGraph.CurveItem, iPt As Integer) As String Handles zg.PointValueEvent
        Return ""
    End Function

    Private Sub zg_Scroll(sender As Object, e As System.Windows.Forms.ScrollEventArgs) Handles zg.Scroll

    End Sub
    Private Sub zg_ScrollDoneEvent(sender As ZedGraph.ZedGraphControl, scrollBar As System.Windows.Forms.ScrollBar, oldState As ZedGraph.ZoomState, newState As ZedGraph.ZoomState) Handles zg.ScrollDoneEvent

    End Sub
    Private Sub zg_ScrollEvent(sender As Object, e As System.Windows.Forms.ScrollEventArgs) Handles zg.ScrollEvent

    End Sub
    Private Sub zg_ScrollProgressEvent(sender As ZedGraph.ZedGraphControl, scrollBar As System.Windows.Forms.ScrollBar, oldState As ZedGraph.ZoomState, newState As ZedGraph.ZoomState) Handles zg.ScrollProgressEvent

    End Sub
    Private Sub zg_ZoomEvent(sender As ZedGraph.ZedGraphControl, oldState As ZedGraph.ZoomState, newState As ZedGraph.ZoomState) Handles zg.ZoomEvent

    End Sub
#End Region
#Region "Modes"
    ''' <summary>
    ''' 'Frame graph
    ''' </summary>
    ''' <remarks></remarks>
    Private mWorkUnit As New clsWU
    Friend Sub SetWU(WorkUnit As clsWU)
        mWorkUnit = WorkUnit
    End Sub

    ''' <summary>
    ''' 'Project graph
    ''' </summary>
    ''' <remarks></remarks>
    Private ActiveProject As clsProjectStatistics.clsProject
    Private iProjects As Int32 = 0
    Private iRCG As Int32 = 0
    ''' <summary>
    ''' 'Index of active project from projectstatistics
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Property ProjectsIndex As Int32
        Get
            Return iProjects
        End Get
        Set(value As Int32)
            iProjects = value
            'Set buttons
            cmdUpMajor.Enabled = Not iProjects = ProjectStatistics.lProjects.Count
            cmdDownMajor.Enabled = Not iProjects = 0
            If iProjects > 0 Then ' 0 is oversight 
                ActiveProject = ProjectStatistics.ProjectStatistics(iProjects - 1)
            Else
                ActiveProject = Nothing
            End If
            RcgIndex = 0
        End Set
    End Property

    ''' <summary>
    ''' 'Index of active rcg for activeproject
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <BrowsableAttribute(True)>
    Friend Property RcgIndex As Int32
        Get
            Return iRCG
        End Get
        Set(value As Int32)
            iRCG = value
            If iProjects = 0 Then
                cmdUpMinor.Enabled = False
                cmdDownMinor.Enabled = ProjectStatistics.ProjectStatistics(iProjects - 1).Projects.Count > 0
            Else
                cmdDownMinor.Enabled = Not iRCG < 1
                cmdDownMinor.Enabled = ProjectStatistics.ProjectStatistics(iProjects - 1).Projects.Count > iRCG
            End If
        End Set
    End Property


#End Region
#Region "update"
    Public Sub DoUpdate()
        If GraphMode = GraphModeEnum.Frames Then
            DrawFrames()
        ElseIf GraphMode = GraphModeEnum.Performance Then

        ElseIf GraphMode = GraphModeEnum.Projects Then

        ElseIf GraphMode = GraphModeEnum.TpfCredit Then

        End If
    End Sub
    Private Sub DrawFrames()
        scMain.Panel1Collapsed = True

    End Sub
#End Region

End Class
