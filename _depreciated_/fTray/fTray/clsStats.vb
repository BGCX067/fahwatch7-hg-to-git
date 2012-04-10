Imports System.Xml
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Public Class clsStats
    Private _Path As String, _currentFile As String
    Private binSerializer As New BinaryFormatter
    Private Projects As New ArrayList
    Private ActiveProject As clsProject
    Private Client As clsClientConfiguration.clsClientConfigs.sClientSettings.sClient
    'Frame structure consists of begintime and timespan
    <Serializable()> _
    Public Class clsFrame
        Public CpuInfo As clsHWInfo.InstalledCPU
        Public GpuInfo As clsHWInfo.InstalledGPU
        Public BeginTime As DateTime
        Public FrameTime As TimeSpan
        Public Percentage As Int16
        Public bEmpty As Boolean = True
    End Class
    'Project structure consists of queue.dat slot / individual frames / client info
    <Serializable()> _
    Public Class clsProject
        Public PGClient As clsClientConfiguration.clsClientConfigs.sClientSettings.sClient.clsPandeGroup
        Private _QueueSlot As clsQueue.Entry
        Private _Frames(0 To 0) As clsFrame
        Public bHasReferenceValues As Boolean = False
        Public rv3Frames As TimeSpan
        Public rvAllFrames As TimeSpan
        Public ReadOnly Property rv3Frames_string() As String
            Get
                Try
                    Dim tsFrame As TimeSpan = rv3Frames
                    Dim strRet As String = ""
                    If tsFrame.Days > 0 Then
                        strRet &= tsFrame.Days & ":"
                    End If
                    If tsFrame.Hours > 0 Then
                        If tsFrame.Hours < 10 Then
                            strRet &= "0" & tsFrame.Hours & ":"
                        Else
                            strRet &= tsFrame.Hours & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Minutes > 0 Then
                        If tsFrame.Minutes < 10 Then
                            strRet &= "0" & tsFrame.Minutes & ":"
                        Else
                            strRet &= tsFrame.Minutes & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Seconds > 0 Then
                        If tsFrame.Seconds < 10 Then
                            strRet &= "0" & tsFrame.Seconds
                        Else
                            strRet &= tsFrame.Seconds
                        End If
                    Else
                        strRet &= "00"
                    End If
                    Return strRet
                Catch ex As Exception
                    LogWindow.WriteError("clsStats, clsProject, rv3Frames_String", Err, ex.Message)
                    Return ""
                End Try
            End Get
        End Property
        Public ReadOnly Property rvAllFrames_String() As String
            Get
                Try
                    Dim tsFrame As TimeSpan = rvAllFrames
                    Dim strRet As String = ""
                    If tsFrame.Days > 0 Then
                        strRet &= tsFrame.Days & ":"
                    End If
                    If tsFrame.Hours > 0 Then
                        If tsFrame.Hours < 10 Then
                            strRet &= "0" & tsFrame.Hours & ":"
                        Else
                            strRet &= tsFrame.Hours & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Minutes > 0 Then
                        If tsFrame.Minutes < 10 Then
                            strRet &= "0" & tsFrame.Minutes & ":"
                        Else
                            strRet &= tsFrame.Minutes & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Seconds > 0 Then
                        If tsFrame.Seconds < 10 Then
                            strRet &= "0" & tsFrame.Seconds
                        Else
                            strRet &= tsFrame.Seconds
                        End If
                    Else
                        strRet &= "00"
                    End If
                    Return strRet
                Catch ex As Exception
                    LogWindow.WriteError("clsStats, clsProject, rvAllFrames_String", Err, ex.Message)
                    Return ""
                End Try
            End Get
        End Property
        Public Sub ClearFrames()
            Try
                If _Frames(_Frames.Count - 1).FrameTime.TotalSeconds = 0 Then
                    ReDim _Frames(0 To 0)
                    _Frames(0) = New clsFrame
                End If
            Catch ex As Exception
                ReDim _Frames(0 To 0)
                _Frames(0) = New clsFrame
            End Try
        End Sub
        Public ReadOnly Property KnownFrames() As Int16
            Get
                Try
                    If _Frames(0).bEmpty Then Return 0
                    Return _Frames.Count
                Catch ex As Exception
                    LogWindow.WriteError("clsStats, clsProject, KnownFrames", Err, ex.Message)
                    Return 0
                End Try
            End Get
        End Property
        Public Property QueuSlot() As clsQueue.Entry
            Get
                Return _QueueSlot
            End Get
            Set(ByVal value As clsQueue.Entry)
                _QueueSlot = value
            End Set
        End Property
        Public Function AddFrame(ByVal FrameTS As TimeSpan, ByVal FrameEnd As DateTime, ByVal FramePercentage As Int16) As Boolean
            Try
                Dim nFrame As New clsFrame
                With nFrame
                    If PGClient.ClientType = clsClientConfiguration.clsClientConfigs.eClient.GXA Then
                        'Cpu client
                        If PGClient.AdditionalParameters.Contains("-gpu ") Then
                            Dim sIndex As String = Mid(PGClient.AdditionalParameters, PGClient.AdditionalParameters.IndexOf("-gpu ") + Len("-gpu "), 1)
                            .GpuInfo = hwInfo.GetGpu(CInt(sIndex) + 1)
                        Else
                            .GpuInfo = hwInfo.GetGpu(1)
                        End If
                    Else
                        'gpu client
                        .CpuInfo = hwInfo.GetCpu(1)
                    End If
                    .FrameTime = FrameTS
                    .BeginTime = FrameEnd.Subtract(FrameTS)
                    .Percentage = FramePercentage
                    .bEmpty = False
                End With
                If _Frames(_Frames.GetUpperBound(0)).Percentage = nFrame.Percentage Then Return False
                If Not _Frames(_Frames.GetUpperBound(0)).bEmpty Then
                    ReDim Preserve _Frames(0 To (_Frames.GetUpperBound(0)) + 1)
                End If
                _Frames(_Frames.GetUpperBound(0)) = nFrame
                If nFrame.Percentage >= 3 Then bHasReferenceValues = False
                Return True
            Catch ex As Exception
                LogWindow.WriteError("clsStats, clsProject, AddFrame", Err, ex.Message)
                Return False
            End Try
        End Function
        Public ReadOnly Property AverageFrameTime(Optional ByVal FrameCount As Int16 = 1) As TimeSpan
            Get
                Try
                    Dim tsTotal As New TimeSpan
                    Dim iFrames As Int16 = 0
                    For xInt As Int16 = _Frames.GetUpperBound(0) To _Frames.GetLowerBound(0) Step -1
                        tsTotal = tsTotal.Add(_Frames(xInt).FrameTime)
                        iFrames += 1
                        If iFrames = FrameCount Then Exit For
                    Next
                    Return TimeSpan.FromSeconds(tsTotal.TotalSeconds / iFrames)
                Catch ex As Exception
                    LogWindow.WriteError("clsStats, clsProject,  AverageFrameTime(" & FrameCount & ")", Err, ex.Message)
                    Return TimeSpan.FromSeconds(0)
                End Try
            End Get
        End Property
        Public ReadOnly Property strAvgFrametime(Optional ByVal FrameCount As Int16 = 1) As String
            Get
                Try
                    Dim tsFrame As TimeSpan = AverageFrameTime(FrameCount)
                    Dim strRet As String = ""
                    If tsFrame.Days > 0 Then
                        strRet &= tsFrame.Days & ":"
                    End If
                    If tsFrame.Hours > 0 Then
                        If tsFrame.Hours < 10 Then
                            strRet &= "0" & tsFrame.Hours & ":"
                        Else
                            strRet &= tsFrame.Hours & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Minutes > 0 Then
                        If tsFrame.Minutes < 10 Then
                            strRet &= "0" & tsFrame.Minutes & ":"
                        Else
                            strRet &= tsFrame.Minutes & ":"
                        End If
                    Else
                        strRet &= "00:"
                    End If
                    If tsFrame.Seconds > 0 Then
                        If tsFrame.Seconds < 10 Then
                            strRet &= "0" & tsFrame.Seconds
                        Else
                            strRet &= tsFrame.Seconds
                        End If
                    Else
                        strRet &= "00"
                    End If
                    Return strRet
                Catch ex As Exception
                    LogWindow.WriteError("clsStats, clsProject, strAvgFrameTime", Err, ex.Message)
                    Return ""
                End Try
            End Get
        End Property
        Public Sub New(ByVal Slot As clsQueue.Entry)
            Try
                ClearFrames()
                _QueueSlot = Slot
            Catch ex As Exception

            End Try
        End Sub
    End Class
    'Serializable structure for xml data
    Public Property ActivePRGC() As clsProject
        Get
            Return ActiveProject
        End Get
        Set(ByVal value As clsProject)
            ActiveProject = value
        End Set
    End Property
    Public Sub New(ByVal TheClient As clsClientConfiguration.clsClientConfigs.sClientSettings.sClient)
        Try
            Client = TheClient
            _Path = dPath & "\Statiscs\" & Client.GuiController.ShortName & "\"
            If Not My.Computer.FileSystem.DirectoryExists(_Path) Then My.Computer.FileSystem.CreateDirectory(_Path)

            Projects.Clear()
            Dim fNames = My.Computer.FileSystem.GetFiles(_Path, FileIO.SearchOption.SearchTopLevelOnly, "*.bin")

            For Each fName In fNames
                Dim nProject As New clsProject(TheClient.GuiController.Queue.ActiveSlot)
                Dim fsFile As New FileStream(fName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                ActiveProject = CType(binSerializer.Deserialize(fsFile), clsProject)
                fsFile.Close()
                fsFile = Nothing
                Projects.Add(nProject)
            Next

            _currentFile = _Path & Client.GuiController.Queue.StatsFile
            If My.Computer.FileSystem.FileExists(_Path & Client.GuiController.Queue.StatsFile) Then
                'Read current stats
                ActiveProject = New clsProject(TheClient.GuiController.Queue.ActiveSlot)
                Dim fsFile As New FileStream(_currentFile, FileMode.Open, FileAccess.Read, FileShare.None)
                ActiveProject = CType(binSerializer.Deserialize(fsFile), clsProject)
                fsFile.Close()
                fsFile = Nothing
                Exit Sub
            Else
                'Look for existing matching project, if found use that
                fNames = My.Computer.FileSystem.GetFiles(_Path, FileIO.SearchOption.SearchTopLevelOnly, Client.GuiController.Queue.ActiveSlot.Project.Project & "*.bin")
                If fNames.Count = 0 Then GoTo CreateNew
                Dim mySort As New clsSort, alFiles As New ArrayList
                For Each Fname In fNames
                    alFiles.Add(New FileInfo(Fname))
                Next
                alFiles.Sort(mySort)
                Dim fInfo As FileInfo = CType(alFiles(0), FileInfo)
                _currentFile = fInfo.FullName
                alFiles = Nothing
                fInfo = Nothing
                fNames = Nothing
                Dim ReferenceProject As New clsProject(TheClient.GuiController.Queue.ActiveSlot)
                Dim fsFile As New FileStream(_currentFile, FileMode.Open, FileAccess.Read, FileShare.None)
                ReferenceProject = CType(binSerializer.Deserialize(fsFile), clsProject)
                fsFile.Close()
                fsFile = Nothing
                ActiveProject = New clsProject(TheClient.GuiController.Queue.ActiveSlot)
                With ActiveProject
                    .PGClient = TheClient.PandeGroup
                    .bHasReferenceValues = True
                    .rv3Frames = ReferenceProject.AverageFrameTime(3)
                    .rvAllFrames = ReferenceProject.AverageFrameTime(100)
                End With
                ReferenceProject = Nothing
                _currentFile = _Path & Client.GuiController.Queue.StatsFile
                fsFile = New FileStream(_currentFile, FileMode.Create, FileAccess.Write, FileShare.None)
                binSerializer.Serialize(fsFile, ActiveProject)
                fsFile.Close()
                fsFile = Nothing
                Exit Sub
            End If
CreateNew:
            ActiveProject = New clsProject(Client.GuiController.Queue.ActiveSlot)
            ActiveProject.PGClient = Client.PandeGroup
            Dim fsStats As New FileStream(_currentFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
            binSerializer.Serialize(fsStats, ActiveProject)
            fsStats.Close()
        Catch ex As Exception
            LogWindow.WriteError("clsStats, New()", Err, ex.Message)
        End Try
    End Sub
    Public Sub Update()
        _currentFile = _Path & Client.GuiController.Queue.StatsFile
        Dim fsStats As New FileStream(_currentFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)
        binSerializer.Serialize(fsStats, ActiveProject)
        fsStats.Close()
    End Sub
    Public ReadOnly Property AllProjects() As ArrayList
        Get
            Try
                Return Projects
            Catch ex As Exception
                LogWindow.WriteError("clsStats, AllProjects", Err)
                Return New ArrayList
            End Try
        End Get
    End Property


    Private Class clsSort
        Implements IComparer
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            ' make it return this in descending order (newest first) 
            Return DateTime.Compare(DirectCast(x, FileInfo).CreationTime, DirectCast(y, FileInfo).CreationTime)
        End Function
    End Class

End Class


