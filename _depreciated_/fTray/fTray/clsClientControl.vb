'   fTray client control class
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

Imports System.Runtime.InteropServices
Imports System.Management
Imports System.ServiceProcess
Imports Microsoft.Win32

Public Class clsClientControl
#Region "Structures for statistics class"
    <Serializable()>
    Public Enum eTypeOfClient
        Smp = 1
        Classic = 2
        Gpu = 3
    End Enum
    <Serializable()>
    Public Structure sClient
        Public Shared IsEmpty As Boolean = True
        'Use this structure for client database info
        Public TypeOfClient As eTypeOfClient
        Public ClientVersion As String
        Public ClientEXE As String 'use to match frames and projects to this client CLIENTID
        Public ClientArguments As String
        Public AcceptedWUSize As String
        Public AdditionalParameters As String
        Public AskNetwork As String
        Public CheckpointInterval As String
        Public CorePriority As String
        Public CpuUsage As String
        Public DisableAffinitylock As String
        Public DisableAssembly As String
        Public ForceAdvMethods As String
        Public IgnoreDeadline As String
        Public IPAdress As String
        Public MachineID As String
        Public MemoryUsage As String
        Public PassKey As String
        Public PauseBattery As String
        Public ProxyHost As String
        Public ProxyPassword As String
        Public ProxyPort As String
        Public ProxyUserName As String
        Public TeamNumber As String
        Public UseProxy As String
        Public UseProxyPassword As String
        Public UserName As String
        Public UseAsService As String
    End Structure
    <Serializable()>
    Public Structure sProject
        Public Shared IsEmpty As Boolean = True
        Public ClientID As String
        Public Qslot As clsQueue.Entry
        Public CoreStatus As clsQueue.clsCoreStatus
        Public ProjectID As String 'qs;pt.PRCG+ISSUED.tostring
        Public Worth As Double
        Public kFactor As Double
    End Structure
    <Serializable()>
    Public Structure sFrames
        Public Shared IsEmpty As Boolean = True
        Public ClientID As String
        Public ProjectID As String
        Public BeginTime As DateTime
        Public EndTime As DateTime
    End Structure
#End Region
#Region "Placeholder class for EOC interaction"
    Public Class clsEOCInfo
        Private WithEvents _EOCSTATS As clsEOC
        Private _bEOCInit As Boolean = False
        Public ReadOnly Property EOCStats() As clsEOC
            Get
                If Not _bEOCInit Then
                    _EOCSTATS = New clsEOC(ClientControl.Queue.ActiveSlot.UserName, ClientControl.Queue.ActiveSlot.TeamNumber)
                    _bEOCInit = True
                End If
                Return _EOCSTATS
            End Get
        End Property
        Public Property EocID As String
        Public Function InitEOC() As Boolean
            Try
                If _bEOCInit Then Return True
                _EOCSTATS = New clsEOC(ClientControl.Queue.ActiveSlot.UserName, ClientControl.Queue.ActiveSlot.TeamNumber)
                _bEOCInit = True
                Return _EOCSTATS.Initialized
            Catch ex As Exception
                LogWindow.WriteError("Guicontroller, InitEOC", Err, ex.Message)
                Return False
            End Try
        End Function
        'Eoc trayform
        Private _StatsForm As frmEOC
        Public bStatsInit As Boolean = False
        Public Function ShowStatsForm(Optional ByVal bTrayIcon As Boolean = True, Optional ByVal FadeTimeOut As Double = 5000) As Boolean
            Try
                If Not bStatsInit Then
                    'Ugly but works?
                    _StatsForm = New frmEOC
                    _StatsForm.Size = New Size(0, 0)
                    _StatsForm.Show()
                    _StatsForm.Hide()
                    bStatsInit = True
                End If
                Return _StatsForm.ShowSig(Me, 5000, bTrayIcon)
            Catch ex As Exception
                LogWindow.WriteError("ShowStatsform", Err)
                Return False
            End Try
        End Function
        Public ReadOnly Property UserName As String
            Get
                Try
                    Return ClientControl.Queue.ActiveSlot.UserName
                Catch ex As Exception
                    Return vbNullString
                End Try
            End Get
        End Property
        Public ReadOnly Property TeamNumber As String
            Get
                Try
                    Return ClientControl.Queue.ActiveSlot.TeamNumber
                Catch ex As Exception
                    Return vbNullString
                End Try
            End Get
        End Property
        Public Sub UpdateRecieved() Handles _EOCSTATS.UpdateRecieved
            Try
                If MySettings.MySettings.EOCNotify Then ShowStatsForm(True, TimeSpan.FromMinutes(1).TotalMilliseconds)
            Catch ex As Exception
                LogWindow.WriteError("UpdateRecieved", Err)
            End Try
        End Sub
    End Class
#End Region
#Region "Declares"
    Private _Location As String = ""
    Private _ClientEXE As String = ""
    Public WithEvents cProcess As New Process
    Public WithEvents fProcess As New Process
    Public MyClient As New sClient
    Public MyProject As New sProject
    Public sController As ServiceProcess.ServiceController
    Public Queue As clsQueue
    Private ConsoleControl As New clsConsoleControl
    Public Event ConsoleExited()
    Public Event ConsoleAttached()
    Public Statistics As clsStatistics
    Public EOC As clsEOCInfo
#End Region
#Region "Properties"
    Public ReadOnly Property ClientPath As String
        Get
            Try
                Dim tPath As String = Application.StartupPath
                If My.Computer.FileSystem.FileExists(tPath & "\Folding@home-Win32-x86.exe") Then
                    Return tPath
                ElseIf My.Computer.FileSystem.FileExists(tPath & "\Folding@home-Win32-GPU.exe") Then
                    Return tPath
                Else
                    Return "-1"
                End If
            Catch ex As Exception
                Return "-2"
            End Try
        End Get
    End Property
    Public ReadOnly Property ClientProcessName As String
        Get
            Try
                Dim tString As String = ClientEXE.Replace(".exe", "")
                Return Mid(tString, tString.LastIndexOf("\") + 2)
            Catch ex As Exception

                Return vbNullString
            End Try
        End Get
    End Property
    Public ReadOnly Property ClientEXE As String
        Get
            Return _ClientEXE
        End Get
    End Property
    Public ReadOnly Property ClientLocation As String
        Get
            Return _Location
        End Get
    End Property
    Public Enum eStatus
        active = 1
        stopped = 2
        paused = 3
        failure = 4
    End Enum
    Public Enum eWindowState
        Visible = 1
        Hidden = 2
        Failure = 0
    End Enum
    Public ReadOnly Property ConsoleStatus As eStatus
        Get
            Try
                Static _ClientID As IntPtr = 0
                Static _CoreID As IntPtr = 0
                Static _bClient As Boolean = False
                Static _bCore As Boolean = False
                Debug.Print(DateTime.Now & "Get console status")
                If _bClient Then
                    Try
                        If Process.GetProcessById(_ClientID).HasExited Then
                            _ClientID = 0
                            _bClient = False
                        End If
                    Catch ex As Exception
                        _ClientID = 0
                        _bClient = False
                    End Try
                End If
                If _bCore Then
                    Try
                        If Process.GetProcessById(_CoreID).HasExited Then
                            Queue.CheckCoreExit()
                            _bCore = False
                            _CoreID = 0
                        End If
                    Catch ex As Exception
                        Queue.CheckCoreExit()
                        _bCore = False
                        _CoreID = 0
                    End Try
                End If
                If Not _bClient Then
                    For Each pCheck As Process In Process.GetProcessesByName(ClientProcessName)
                        If pCheck.Id <> _ConfigID Then
                            If pCheck.MainModule.FileName.ToUpper = ClientEXE.ToUpper Then
                                _ClientID = pCheck.Id : _bClient = True
                                cProcess = pCheck
                                RaiseEvent ConsoleAttached()
                                AddHandler cProcess.Exited, AddressOf cProcess_Exited
                            End If
                        End If
                    Next
                End If
                If _bClient And Not _bCore Then
                    'get fahcore name
                    For Each fCheck As Process In Process.GetProcesses
                        If fCheck.ProcessName Like "FahCore_*" Then
                            If fCheck.MainModule.FileName.ToUpper.Contains(ClientPath.ToUpper) Then
                                _bCore = True
                                _CoreID = fCheck.Id
                                fProcess = Process.GetProcessById(fCheck.Id)
                                AddHandler fProcess.Exited, AddressOf fProcess_Exited
                                Exit For
                            End If
                        End If
                    Next
                End If

                If _bClient Then
                    Return eStatus.active
                Else
                    Return eStatus.stopped
                End If
            Catch ex As Exception
                Return eStatus.failure
            End Try
        End Get
    End Property
    Public ReadOnly Property WindowState() As eWindowState
        Get
            Try
                If cProcess.HasExited Then Return eWindowState.Failure
                cProcess = Process.GetProcessById(cProcess.Id)
                If cProcess.MainWindowHandle <> 0 Then
                    Return eWindowState.Visible
                Else
                    Return eWindowState.Hidden
                End If
            Catch ex As Exception
                Return eWindowState.Failure
            End Try
        End Get
    End Property
    Public ReadOnly Property IsService As Boolean
        Get
            Try
                Try
                    If sController.DisplayName <> "" Then
                        Return True
                    End If
                Catch ex As Exception

                End Try
                Dim mID As String = "0"
                'Check for clients.cfg
                If My.Computer.FileSystem.FileExists(ClientPath & "\client.cfg") Then
                    Dim fText As String = My.Computer.FileSystem.ReadAllText(ClientPath & "\client.cfg")
                    Dim lText() As String = fText.Split(vbLf)
                    For Each sString As String In lText
                        If sString.ToUpper.Contains("MACHINEID") Then
                            mID = sString.ToUpper.Replace("MACHINEID=", "")
                            Exit For
                        End If
                    Next
                    If mID = "0" Then
                        Return False
                    End If
                    Dim cString As String
                    If ClientEXE.Contains("Folding@home-Win32-GPU.exe") Then
                        cString = "Folding@home-GPU-[" & mID & "]"
                    Else
                        cString = "Folding@home-CPU-[" & mID & "]"
                    End If
                    Dim rKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services")
                    Dim sVal() As String = rKey.GetSubKeyNames
                    For Each sSer As String In sVal
                        If sSer.ToUpper = cString.ToUpper Then
                            For Each sService As ServiceController In ServiceController.GetServices
                                If sService.DisplayName.ToUpper = cString.ToUpper Then
                                    sController = sService
                                    Return True
                                End If
                            Next
                        End If
                    Next
                    Return False
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property ServiceStatus As ServiceControllerStatus
        Get
            Try
                If sController.ServiceName.ToUpper.Contains("FOLDING") Then
                    Return sController.Status
                Else
                    Return eStatus.failure
                End If
            Catch ex As Exception
                Return eStatus.failure
            End Try
        End Get
    End Property
    Public ReadOnly Property ServiceCanPause As Boolean
        Get
            Try
                If Not IsService Then
                    Return False
                Else
                    If sController.CanPauseAndContinue Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
#End Region
#Region "Functions"
#Region "Client Database functions"
    Public ReadOnly Property ClientNew() As Boolean
        Get
            Try
                Return Not Statistics.GetClient(_ClientEXE).ClientEXE = _ClientEXE
            Catch ex As Exception
                LogWindow.WriteError("ClientControl_ClientNew()", Err)
                Return False
            End Try
        End Get
    End Property
    Public Function AddClientToDatabse(ByVal Client As sClient) As Boolean
        Try
            Return Statistics.AddClient(Client)
        Catch ex As Exception
            LogWindow.WriteError("ClientControl_Addclienttodatabase", Err)
            Return False
        End Try
    End Function
    Public Function UpdateClientToDatabase(ByVal Client As sClient) As Boolean
        Try
            Return Statistics.UpdateClient(Client)
        Catch ex As Exception
            LogWindow.WriteError("ClientControl_UpdateClient", Err)
            Return False
        End Try
    End Function
#End Region
#Region "Project database functions"
    Public ReadOnly Property ProjectINDB(ByVal Project As sProject) As Boolean
        Get
            Try

            Catch ex As Exception

            End Try
        End Get
    End Property
    Public Function AddProjectToDB(ByVal Client As sClient, ByVal Project As sProject) As Boolean
        Try

        Catch ex As Exception
            LogWindow.WriteError("ClientControl_AddProjectToDatabse", Err)
            Return False
        End Try
    End Function
    Public Function UpdateProjectDB(ByVal Client As sClient, ByVal Project As sProject) As Boolean
        Try

        Catch ex As Exception
            LogWindow.WriteError("ClientControl_UpdateProjectDB", Err)
            Return False
        End Try
    End Function
#End Region
    Public Function StopClient() As Boolean
        Try
            If IsService Then
                Try
                    If sController.Status = ServiceControllerStatus.Stopped Or sController.Status = ServiceControllerStatus.StopPending Then GoTo Raise
                    sController.Stop()
                    sController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10))
                    Return (sController.Status = ServiceControllerStatus.Stopped)
                Catch ex As Exception
                    MsgBox("Error details: " & ex.Message, vbOKOnly & vbApplicationModal & vbCritical, "fTray warning")
                    Return False
                End Try
            Else
                Try
                    'old
                    If cProcess.HasExited Then Return True
                    If cProcess.MainWindowHandle = 0 Then
                        If ShowConsole() Then
                            GoTo CloseClient
                        Else
                            'MsgBox("Can't show console!")
                            Return False
                        End If
                    Else
CloseClient:
                        Debug.Print("Exit client called at: " & DateTime.Now)
                        Dim dE As DateTime = DateTime.Now.AddSeconds(5)
                        Dim xItter As Int32 = 0
                        Do
                            Dim ok As Boolean = AttachConsole(cProcess.Id)
                            If ok Then
                                Dim hd1 As IntPtr = GetConsoleWindow()
                                ShowWindow(hd1, SW_RESTORE)
                                FreeConsole()
                            End If
                            cProcess = Process.GetProcessById(cProcess.Id)
                            SetForegroundWindow(cProcess.MainWindowHandle)
                            WaitMS(5)
                            xItter += 1
                            Debug.Print("client window handle: " & cProcess.MainWindowHandle.ToString)
                            Debug.Print("Iteration: " & xItter)
                            Debug.Print("Setforegroundwindow: " & SetForegroundWindow(cProcess.MainWindowHandle))
                            WaitMS(1)
                            Debug.Print("Getforeground: " & GetForegroundWindow().ToString)
                            Debug.Print("Is equal :" & (cProcess.MainWindowHandle = GetForegroundWindow()).ToString)
                            If GetForegroundWindow() = cProcess.MainWindowHandle Then
                                Dim sK As SendKeys
                                sK.Send("^C")
                                Debug.Print("Used sendkeys")
                            Else
                                If cProcess.HasExited Then
                                    Debug.Print("Process exited after sendkeys")
                                    Exit Do
                                Else
                                    If DateTime.Now > dE Then
                                        Debug.Print("Max wait elapsed")
                                        'Send close, then do another loop
                                        cProcess.Close()
                                        Debug.Print("Used close function")
                                        SetForegroundWindow(cProcess.MainWindowHandle)
                                        WaitMS(1)
                                        If GetForegroundWindow() = cProcess.MainWindowHandle Then
                                            Dim sK As SendKeys
                                            sK.Send("^C")
                                        Else
                                            If cProcess.HasExited Then
                                                Exit Do
                                            Else
                                                cProcess.Close()
                                                SetForegroundWindow(cProcess.MainWindowHandle)
                                                WaitMS(1)
                                                Dim sK As SendKeys
                                                sK.Send("^C")
                                                WaitMS(1)
                                                Return cProcess.HasExited
                                            End If
                                        End If
                                    End If
                                    SetForegroundWindow(cProcess.MainWindowHandle)
                                    WaitMS(1)
                                End If
                            End If
                        Loop
                        fLog.InvokeUpdate()
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End If
Raise:
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function PauseClient() As Boolean
        Try
            If Not IsService Then Return False
            sController.Pause()
            sController.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(5))
            If sController.Status = ServiceControllerStatus.Paused Then Return True
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function StartClient(Optional ByVal ExtraParams As String = "") As Boolean
        Try

            If IsService() Then
                'Add params to registry? DANGEROUS because how to make sure the extra params are removed?
                sController.Start()
                sController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10))
                Return sController.Status = ServiceControllerStatus.Running
            Else
                Try
                    cProcess = New Process
                    With cProcess.StartInfo
                        .FileName = ClientEXE
                        .WorkingDirectory = Mid(ClientEXE, 1, ClientEXE.LastIndexOf("\"))
                        .Arguments = ExtraParams
                    End With
                    cProcess.Start() ' wait for main window to be created?!
                    Threading.Thread.Sleep(1000)
                    Application.DoEvents()
                    ClientControl.Queue.InitQueue()
                    'fLog.InvokeUpdate()

                    Return HideConsole()
                Catch ex As Exception
                    Return False
                End Try
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function ConfigureClient() As Boolean
        Try
            Dim fConfig As New frmConfig
            Dim cClient As New frmConfig.clsClientInfo
            With cClient
                .ClientExe = ClientEXE
                .ClientLocation = ClientPath
                If ClientEXE.Contains("Folding@home-Win32-x86.exe") Then
                    .TypeOfClient = frmConfig.clsClientInfo.eClientType.Smp
                Else
                    .TypeOfClient = frmConfig.clsClientInfo.eClientType.Gpu
                End If
            End With
            fConfig.StartConfig(cClient)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function HideConsole() As Boolean
        Try
            Return ConsoleControl.HideConsole(cProcess)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function ShowConsole() As Boolean
        Try
            Return ConsoleControl.ShowConsole(cProcess)
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region
#Region "Entry point"
    Public Sub New(ByVal Location As String, ByVal Exe As String)
        Try
            cProcess = New Process
            Location = Location
            _Location = Location
            _ClientEXE = Exe
            Statistics = New clsStatistics(Location)
            Queue = New clsQueue(Location)
            If Queue.ReadQueue() Then
                With MyProject
                    .IsEmpty = False
                    .Qslot = Queue.ActiveSlot
                    If .Qslot.TimeData.BeginTime = #1/1/2000# Then

                    End If
                    If ProjectInfo.Projects.KnownProject(.Qslot.Project.Project) Then
                        .Worth = CDbl(ProjectInfo.Projects.Project(.Qslot.Project.Project).Credit)
                        .kFactor = CDbl(ProjectInfo.Projects.Project(.Qslot.Project.Project).kFactor)
                    End If
                    .ProjectID = .Qslot.PRCG & .Qslot.TimeData.strIdentifier
                End With
            Else
                MyProject.IsEmpty = True
            End If
        Catch ex As Exception
            LogWindow.WriteError("ClientControl_New", Err)
        End Try
    End Sub
#End Region
#Region "Process event handlers"
    Private Sub cProcess_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles cProcess.Exited
        Try
            fLog.InvokeUpdate()
            'RaiseEvent ConsoleExited()
            cProcess = New Process
        Catch ex As Exception

        End Try
    End Sub
    Private Sub fProcess_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles fProcess.Exited
        Try
            Queue.CheckCoreExit()
        Catch ex As Exception

        End Try
    End Sub
#End Region
End Class
Public Class clsConsoleControl
    Public Function ShowConsole(ByVal _cProc As Process) As Boolean
        Try
            Dim ok As Boolean = AttachConsole(_cProc.Id)
            If ok Then
                Dim hd1 As IntPtr = GetConsoleWindow()
                ShowWindow(hd1, SW_RESTORE)
                FreeConsole()
            End If
            _cProc = Process.GetProcessById(_cProc.Id)
            'StartUpdate(True)
            SetForegroundWindow(_cProc.MainWindowHandle)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function HideConsole(ByVal _cProc As Process) As Boolean
        Try
            Dim ok As Boolean = AttachConsole(_cProc.Id)
            If ok Then
                Dim hd1 As IntPtr = GetConsoleWindow()
                ShowWindow(hd1, SW_HIDE)
                FreeConsole()
            End If
            _cProc = Process.GetProcessById(_cProc.Id)
            'StartUpdate(True)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class