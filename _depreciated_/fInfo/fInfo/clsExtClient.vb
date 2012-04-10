'/*
' * fInfo extClient class Copyright Marvin Westmaas ( mtm ) 
' * 
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
Imports HWInfo
Imports FAHInterface.FAHInterface
Imports System.Xml
Imports System.IO
Imports ProjectInfo

Namespace fInfo
    Public Class clsExtClient
#Region "Declarations"
        Public WithEvents LogWindow As New clsLogwindow
        Public WithEvents hwInf As New clsHWInfo.cHWInfo
        Public WithEvents FAHinterface As New clsFAHInterface
        Public WithEvents ohmInt As clsHWInfo.cHWInfo.cOHMInterface
        Public WithEvents Data As Data.Data
        Public WithEvents Client As FAHInterface.Client.ClientAccess
        Public ProjectInfo As New clsProjectInf
        Public EOC As clsEOCInfo
        'Public TestForm As New frmTEST
        Public lsPCI As clsPci
#End Region
#Region "Event handlers / Automation routines"
        Public Sub HandleLog(Message As String)
            LogWindow.WriteLog(Message)
        End Sub
        Public Sub HandleError(Message As String, ErrObj As ErrObject)
            LogWindow.WriteError(ErrObj.Source, ErrObj)
        End Sub
        Public Sub SensorEventHandler(Sensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)
            ' TestForm.SensorHandler(Sensor)
            If Not IsNothing(Data) Then
                If Not IsNothing(Client) Then
                    If Client.Connected Then Data.Update(Sensor)
                End If
            End If
        End Sub
        Private Sub FAHinterface_ResponceRecieved(ByVal Response As String) Handles FAHinterface.ResponseRecieved
            Append(Response)
        End Sub

        Private Sub Client_QueueChanged() Handles Client.QueueChanged
            If Not IsNothing(Data) Then
                Data.Update_ClientQueue(Client)
                LogWindow.WriteLog("FAHClient queue changed and saved to database")
            End If

        End Sub

        Private Sub Client_SlotsChanged() Handles Client.SlotsChanged
            If Not IsNothing(Data) Then
                Data.Update_ClientSlots(Client)
                LogWindow.WriteLog("FAHClient slots changed and saved to database")
            End If

        End Sub
        Private Sub Client_OptionsChanged() Handles Client.OptionsChanged
            If Not IsNothing(Data) Then
                Data.Update_ClientOptions(Client)
                LogWindow.WriteLog("FAHClient options changed and saved to database")
            End If

        End Sub
        Private WithEvents tReconnect As New Timers.Timer

        Private Sub tReconnect_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles tReconnect.Elapsed
            Try
                LogWindow.WriteLog("Retrying connection to local client")
                If Not ReconnectClient() Then
                    LogWindow.WriteLog("Failed, will retry in one minute")
                    For Each oForm As Form In Application.OpenForms
                        If Not IsNothing(oForm.Tag) Then
                            If oForm.Tag.ToString.Contains("connecting:") Then
                                Dim iCount As Int16 = CInt(oForm.Tag.ToString.Substring("connecting:".Length)) + 1
                                oForm.Tag = "connecting:" & iCount.ToString
                                oForm.Controls(0).Text = "Retrying in 60 seconds.. (" & iCount.ToString & ")"
                                Application.DoEvents()
                                Exit For
                            End If
                        End If
                    Next
                    tReconnect.Interval = 60000
                    tReconnect.AutoReset = False
                    tReconnect.Enabled = True
                Else
                    LogWindow.WriteLog("Connection succesfull!")
                    For Each oForm As Form In Application.OpenForms
                        If oForm.Tag.ToString.Contains("connecting:") Then
                            oForm.Close()
                        End If
                    Next
                    Data.Init(Application.StartupPath & "\Data\")
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Sub Client_Disconnected() Handles Client.Disconnected
            If tReconnect.Enabled Then Exit Sub
            LogWindow.WriteLog("Client disconnected!")
            If Settings.MySettings.Automation Then
                LogWindow.WriteLog("Automation enabeld, will retry connection with 1 minute intervals.")
                LogWindow.WriteLog("Retrying connection with local client.")
                RemoveHandler Client.Disconnected, AddressOf Client_Disconnected
                If ReconnectClient() Then
                    LogWindow.WriteLog("Attempt to reconnect succesfull")
                    LogWindow.ShowIcon(clsLogwindow.TrayIcon.Log)
                    If Not IsNothing(Data) Then Data.Init(Application.StartupPath & "\Data\")
                    AddHandler Client.Disconnected, AddressOf Client_Disconnected
                Else
                    LogWindow.WriteLog("Reconnecting failed, trying again in 1 minute.")
                    LogWindow.WriteLog("Buffer content: " & Client.LastResponce)
                    LogWindow.ShowIcon(clsLogwindow.TrayIcon.Warning)
                    tReconnect.Interval = 60000
                    tReconnect.AutoReset = False
                    tReconnect.Enabled = True
                End If
            End If
        End Sub
        Private Function ReconnectClient() As Boolean
            Try
                Client.Connect(Client.HostName, Client.Port)
                If Client.Connected Then
                    Client.SendCommand("auth " & Client.Password)
                    Client.Update()
                    If Client.ValidPassword Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Private Sub Client_WrongPassword() Handles Client.WrongPassword
            Try
                LogWindow.WriteLog("Connecting to local client failed, wrong password provided")
                If Settings.MySettings.Automation Then
                    'Not started with command line arguments, give visual feedback 
                    If Not ReadFAHClientConfig() Then
                        MsgBox("Password verification failed and client config could not be found, please enter values manually.")
                        Settings.ShowSettings()
                    Else
                        If Not ReconnectClient() Then
                            LogWindow.WriteLog("Configuration read in succesfully but reconnecting failed, retrying in 1 minute.")
                            tReconnect.Interval = 60000
                            tReconnect.AutoReset = False
                            tReconnect.Enabled = True
                        End If
                    End If
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
        End Sub


        Public Delegate Sub AppendText(ByVal Text As String)
        Public Sub Append(ByVal text As String)
            If frmTEST.rtfC.InvokeRequired Then
                Dim nAP As New AppendText(AddressOf Append)
                frmTEST.rtfC.Invoke(nAP, {text})
            Else
                frmTEST.rtfC.Clear()
                frmTEST.rtfC.WordWrap = False
                If text.Contains("{") Then

                End If
                frmTEST.rtfC.AppendText(text & Environment.NewLine)
                frmTEST.rtfC.SelectionStart = frmTEST.rtfC.TextLength
                frmTEST.rtfC.ScrollToCaret()
            End If
        End Sub
#End Region

        Public Sub ohmIntInterval(Interval As Integer)
            ohmInt.AutoUpdate(Interval)
        End Sub



        Private _Port As String = "", _Pwd As String = "", _User As String = "", _Team As String = "", _PassKey As String = ""
        Private aPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        Public ReadOnly Property Port As String
            Get
                Return _Port
            End Get
        End Property
        Public ReadOnly Property PWD As String
            Get
                Return _Pwd
            End Get
        End Property
        Public ReadOnly Property Team As String
            Get
                Return _Team
            End Get
        End Property
        Public ReadOnly Property User As String
            Get
                Return _User
            End Get
        End Property
        Public ReadOnly Property PassKey As String
            Get
                Return _PassKey
            End Get
        End Property
        Private Function ReadFAHClientConfig() As Boolean
            Try
                LogWindow.WriteLog("Searching for configuration file.")
                If My.Computer.FileSystem.DirectoryExists(aPath & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(aPath & "\FAHClient\Config.xml") Then
                    LogWindow.WriteLog("Reading Config.xml file.")
                    Dim strXML As String = My.Computer.FileSystem.ReadAllText(aPath & "\FAHClient\Config.xml")
                    strXML = strXML.Replace("'", Chr(34))
                    Dim iStream As New IO.StringReader(strXML)
                    Using xmlR As Xml.XmlReader = XmlReader.Create(iStream)
                        While Not xmlR.EOF
                            xmlR.Read()
                            Select Case xmlR.Name
                                Case Is = "password"
                                    _Pwd = xmlR.Item(0).ToString
                                Case Is = "user"
                                    _User = xmlR.Item(0).ToString
                                Case Is = "passkey"
                                    _PassKey = xmlR.Item(0).ToString
                                Case Is = "team"
                                    _Team = xmlR.Item(0).ToString
                                Case Is = "command-port"
                                    _Port = xmlR.Item(0).ToString
                            End Select
                        End While
                    End Using
                    If _Port = "" Then _Port = "36330"
                    Return True
                Else
                    LogWindow.WriteLog("No command line arguments were given and automation failed, use ? for help.") ' silent failure
                    Return False
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Private Sub nfActive(Sender As Object, e As System.EventArgs)
            Static bOnce As Boolean = False
            If bOnce Then Exit Sub
            bOnce = True
            With CType(Sender, Form)
                .Tag = "connecting:0"
                .Text = "Connecting to client"
                .FormBorderStyle = FormBorderStyle.FixedDialog
                .MaximizeBox = False
                .Controls.Add(New Label)
                With CType(.Controls(0), Label)
                    .AutoSize = True
                    .Top = 5
                    .Left = 5
                    .Text = "Retrying in 60 seconds.. (0)"
                End With
                .AutoSizeMode = AutoSizeMode.GrowAndShrink
                .AutoSize = True
            End With
            tReconnect.Interval = 6000
            tReconnect.AutoReset = False
            tReconnect.Enabled = True
        End Sub
        Public Function Init(Optional Port As String = "", Optional Password As String = "") As Object
            Try
                AddHandler hwInf.Log, AddressOf HandleLog
                AddHandler hwInf.LogError, AddressOf HandleError
                AddHandler FAHinterface.Log, AddressOf HandleLog
                AddHandler FAHinterface.LogError, AddressOf HandleError
                AddHandler Settings.Log, AddressOf HandleLog
                AddHandler Settings.LogError, AddressOf HandleError

                Settings = New clsSettings(Application.StartupPath)
                LogWindow = LogWindow.CreateLog("", #1/1/2000#, False)
                LogWindow.ShowIcon(clsLogwindow.TrayIcon.Log)
                LogWindow.ShowDebugWindow(clsLogwindow.TrayIcon.Log)
                If My.Application.CommandLineArgs.Count > 0 Then
                    Dim strLog As String = "Command line arguments given: "
                    For Each strArg As String In My.Application.CommandLineArgs
                        strLog &= strArg & " "
                    Next
                    LogWindow.WriteLog(strLog)
                    strLog = Nothing
                    LogWindow.WriteLog("Warning! arguments overide automation, and should only be used to schedule commands")
                End If
                If Settings.MySettings.Automation Then
                    If Not ReadFAHClientConfig() Then
                        LogWindow.WriteLog("Configuration file could not be read, application will now exit")
                        Call ExitApplication(True)
                    End If
                Else
                    _Port = Port
                    _Pwd = Password
                End If

                If Settings.MySettings.lsPCI Then
                    ' Fill detected pciID
                    lsPCI = New clsPci
                    LogWindow.WriteLog("Generating detected pciID list.")
                    Try
                        If Not lsPCI.FillInfo Then
                            LogWindow.WriteLog("Detection failed but application will continue!")
                        End If
                    Catch ex As Exception
                        LogWindow.WriteError(ex.Message, Err)
                        Return Nothing
                    End Try
                Else
                    LogWindow.WriteLog("Skipping pciID list")
                End If

                LogWindow.WriteLog("Accesing OpenHardwareMonitorLib.dll.")
                Try
                    hwInf.Init()
                    ohmInt = hwInf.ohmInterface
                    AddHandler ohmInt.SensorUpdate, AddressOf SensorEventHandler
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                    Return Nothing
                End Try


                LogWindow.WriteLog("Connecting to local client.")
                Client = New FAHInterface.Client.ClientAccess
                Try
                    RemoveHandler Client.Disconnected, AddressOf Client_Disconnected
                    Client.Connect("127.0.0.1", CInt(_Port))
                    If Not Client.Connected Then
                        LogWindow.WriteLog("Could not connect to local client.")
                        If Settings.MySettings.Automation Then
                            If Settings.MySettings.AutoStartClient Then
                                LogWindow.WriteLog("Checking running applications for FAHClient")
                                Dim Proc() As Process = Process.GetProcessesByName("FAHClient")
                                If Proc.Count = 0 Then
                                    LogWindow.WriteLog("FAHClient was not running, starting manually")
                                    Dim aPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), sReader As StreamReader
                                    If My.Computer.FileSystem.DirectoryExists(aPath & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(aPath & "\FAHClient\FAHClient.exe") Then
StartNew:
                                        Dim nP As New Process
                                        With nP.StartInfo
                                            .FileName = "FAHClient.exe"
                                            aPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient"
                                            .WorkingDirectory = aPath
                                            .Arguments = "--lifeline " & Process.GetCurrentProcess.Id.ToString & " --command-port=" & _Port
                                            .CreateNoWindow = True ' Same as fahcontrol
                                            .UseShellExecute = False
                                        End With
                                        LogWindow.WriteLog("Instance started with following paramaters: " & nP.StartInfo.Arguments.ToString)
                                        LogWindow.WriteLog("Workingdirectory: " & aPath)
                                        nP.Start()
                                        LogWindow.WriteLog("Instance started, waiting 15s to reconnect")
                                    Else
                                        For Each sProc As Process In Proc
                                            If sProc.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData & "\FAHClient") Then
                                                GoTo Found
                                            End If
                                        Next
                                        LogWindow.WriteLog("The FAHClient found was not started with the right startupinfo ( working directory ), creating proper instance!")
                                        LogWindow.WriteLog("Recommended user action: check taskmanager and kill rogue FAHClient instances manually")
                                        LogWindow.ShowDebugWindow(clsLogwindow.TrayIcon.Warning)
                                        GoTo StartNew
Found:
                                        LogWindow.WriteLog("Instance found, trying to reconnect in 15s")
                                    End If
                                End If
                            End If
                            Dim dtCont As DateTime = DateTime.Now.AddSeconds(15)
                            While DateTime.Now.Subtract(dtCont).TotalSeconds < 0
                                Threading.Thread.SpinWait(50000)
                                Application.DoEvents()
                            End While

                            LogWindow.WriteLog("Retrying connection")
                            If Not ReconnectClient() Then
                                LogWindow.WriteLog("Application failed to connect to a local fahclient instance and will exit!")
                                Call ExitApplication(True)
                                Return Nothing
                            End If
                        Else
                            LogWindow.WriteLog("Command line arguments override automation behaviour, application will exit")
                            Call ExitApplication(True)
                            End
                        End If
                    End If
                    If _Pwd <> "" Then
                        Try
                            Client.SendCommand("auth " & _Pwd)
                            Client.Update()
                            If Not Client.ValidPassword Then
                                LogWindow.WriteLog("Password verification failed, closing down application")
                                Call ExitApplication(True)
                                Return Nothing 'If not added, application sometimes continues this function regardless to call to exitapplication
                            End If
                            Do
                                If Not Client.Connected Then
                                    LogWindow.WriteLog("Client got disconnected while initializing, closing down application.")
                                    Call ExitApplication(True)
                                    Return Nothing
                                End If
                                Client.SendCommand("options -d")
                                Client.Update()
                                Client.SendCommand("info")
                                Client.Update()
                                Client.SendCommand("queue-info")
                                Client.Update()
                                Client.SendCommand("slot-info")
                                Client.Update()
                                Application.DoEvents()
                            Loop While IsNothing(Client.Info) Or IsNothing(Client.Options) Or IsNothing(Client.lQueue) Or IsNothing(Client.lSlots)
                        Catch ex As Exception
                            LogWindow.WriteError(ex.Message, Err)
                            Call ExitApplication(True)
                            Return Nothing
                        End Try
                    End If
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                    Call ExitApplication(True)
                    Return Nothing
                End Try
                AddHandler Client.Disconnected, AddressOf Client_Disconnected


                LogWindow.WriteLog("Initialing project info")
                ProjectInfo.Init()


                LogWindow.WriteLog("Initializing database access.")
                Data = New Data.Data
                AddHandler Data.Log, AddressOf HandleLog
                AddHandler Data.LogError, AddressOf HandleError

                If Data.Init(Application.StartupPath & "\Data\") Then
                    If Data.HasProjectInfo Then
                        ProjectInfo.ProjectInfo.Projects = Data.Read_Projects
                        If ProjectInfo.Projects.IsEmpty Then ' Database could be corrupted, no need to remove?
                            LogWindow.WriteLog("Project list is empty, trying to update")
                            If ProjectInfo.GetProjects("", True) Then
                                LogWindow.WriteLog("Projects updated succesfully")
                            Else
                                LogWindow.WriteLog("Could not update project list, the log may hold additional details")
                            End If
                        Else
                            LogWindow.WriteLog("Project list loaded, known projects: " & ProjectInfo.Projects.ProjectCount)
                        End If
                    Else
                        LogWindow.WriteLog("Project list is empty, trying to update")
                        If ProjectInfo.GetProjects("", True) Then
                            LogWindow.WriteLog("Projects updated succesfully")
                            Data.Update_ProjectInfo(ProjectInfo.ProjectInfo)
                        Else
                            LogWindow.WriteLog("Could not update project list, the log may hold additional details")
                        End If
                    End If
                    ' Moved data function handling to here so to better handle specific problems later on
                    If Not IsNothing(Client) And Client.Connected And Client.JsON_Info <> "" Then LogWindow.WriteLog("Updating client info to database: " & Data.Update_ClientInfo(Client).ToString)
                    If Not IsNothing(Client) And Client.Connected And Client.JsON_Options <> "" Then LogWindow.WriteLog("Updating client options to database: " & Data.Update_ClientOptions(Client).ToString)
                    If Not IsNothing(Client) And Client.Connected And Client.JsON_Queue <> "" Then LogWindow.WriteLog("Updating client queue to database: " & Data.Update_ClientQueue(Client).ToString)
                    If Not IsNothing(Client) And Client.Connected And Client.JsON_Slots <> "" Then LogWindow.WriteLog("Updating client slots to database: " & Data.Update_ClientSlots(Client).ToString)
                    Data.Update_CAL(hwInf)
                    Data.Update_CUDA(hwInf)
                    Data.Update_OHM(hwInf)
                    Data.Update_openCL(hwInf)
                    Data.UpdateHardware(hwInf)
                    LogWindow.WriteLog("Data access succesfull.")
                Else
                    LogWindow.ShowDebugWindow(clsLogwindow.TrayIcon.Warning)
                    LogWindow.WriteLog("Data access failed, the log may hold additional details")
                End If

                LogWindow.bAllowClose = True
                If Settings.IsEmpty Then
                    Settings = New clsSettings(Application.StartupPath)
                    Settings.SetDefaults()
                    Settings.InitialForm.Show()
                    While Settings.IsFormActive
                        Threading.Thread.SpinWait(25000)
                        Application.DoEvents()
                    End While
                End If



                If Settings.MySettings.Automation Then
                    LogWindow.WriteLog("Application startup completed, monitoring will commence with stored values")



                    ohmInt.AutoUpdate(Settings.MySettings.intOHM)
                    Client.StartUpdates(Settings.MySettings.intSQ, Settings.MySettings.intOPT)
                End If

                Return LogWindow.Form
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function


    End Class
End Namespace

