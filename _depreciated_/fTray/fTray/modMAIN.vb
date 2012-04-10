'   fTray modMain 
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
Imports Microsoft.Win32
Imports System.Data
Imports System.Text

Module modMAIN
#Region "Declares"
    Public WithEvents ClientControl As clsClientControl
    Public fLog As New frmLOG
    Public LogWindow As New clsLogwindow
    Public fConfig As New frmConfig
    Public ProjectInfo As clsProjectInfo
    Public MySettings As clsSettings
    Public nIcon As NotifyIcon
    Public _ConfigID As IntPtr
    Public fstatus As frmPBStatus
    Public hwInf As clsHWInfo
#End Region
#Region "md5 client hashes"
    Public Const md5_Classic As String = "969ee69d5c0d0519b97633b4b5c2646e"
    Public Const md5_Gpu2Xp03 As String = "68412662167e58f3f8e5561d3a878649"
    Public Const md5_Gpu2VistaW7 As String = "68412662167e58f3f8e5561d3a878649"
    Public Const md5_Gpu3VistaW7 As String = "df44cdcc9866280ccf22fa02a877a269"
    Public Const md5_Gpu3Xp03 As String = "df44cdcc9866280ccf22fa02a877a269"
    Public Const md5_SMP2 As String = "067ca01c02bbdebc2eba5f2110210702"
#End Region

#Region "Entry point"
    Sub main()
        Dim bTryClose As Boolean = False
        Try
          
            'set icon to flog.icon
            nIcon = fLog.nIcon
            'check if client is in path
            If Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\Folding@home-Win32-GPU.exe") And Not My.Computer.FileSystem.FileExists(Application.StartupPath & "\Folding@home-Win32-X86.exe") Then
                'Show install client screen
                MsgBox("No client found, application will now exit", vbOKOnly)
                Application.Exit()
            Else
                Dim ClientPath As String = Application.StartupPath
                Dim ClientExe As String
                If My.Computer.FileSystem.FileExists(ClientPath & "\Folding@home-Win32-GPU.exe") Then
                    ClientExe = ClientPath & "\Folding@home-Win32-GPU.exe"
                Else
                    ClientExe = ClientPath & "\Folding@home-Win32-X86.exe"
                End If
            
                'Dim mProc As Process = Process.GetCurrentProcess
                'mProc.PriorityClass = ProcessPriorityClass.Normal
                Cursor.Current = Cursors.AppStarting
                Try
                    LogWindow = LogWindow.CreateLog
                    LogWindow.EmptyLine()
                    LogWindow.WriteLog("fTray version " & My.Application.Info.Version.ToString)
                    LogWindow.EmptyLine()
                    LogWindow.WriteLog("fTray logging started")
                Catch ex As Exception
                    'Log file locked, two instances
                End Try

                hwInf = New clsHWInfo
                hwInf.FillOsInfo()
                hwInf.FillIsAdmin()
                If Environment.OSVersion.Platform = PlatformID.Unix Then
                    GoTo LinuxStart
                End If
                If Not hwInf.IsAdmin Then
                    If Not hwInf.CanElevate Then
                        MsgBox("You're not an administrator, application will exit now", vbOKOnly)
                        End
                    Else
                        Dim dElevate As New dialogElevate
                        Dim rVal As DialogResult = dElevate.ShowDialog
                        If rVal = DialogResult.Cancel Then Exit Sub
                        'Restart 
                        Dim proc As New ProcessStartInfo
                        proc.UseShellExecute = True
                        proc.WorkingDirectory = Environment.CurrentDirectory
                        proc.FileName = Application.ExecutablePath
                        proc.Verb = "runas"
                        Try
                            Process.Start(proc)
                        Catch
                            End
                        End Try
                        Application.Exit()
                    End If
                End If

                'check for existing processes
                Try
                    For Each dProcess As Process In Process.GetProcessesByName(Application.ProductName)
                        If dProcess.MainModule.FileName = Application.ExecutablePath Then
                            If Not dProcess.Id = Process.GetCurrentProcess.Id Then
                                If dProcess.MainWindowHandle <> 0 Then
                                    If Not ShowWindow(dProcess.MainWindowHandle, SW_SHOWDEFAULT) Then GoTo DoMsg
                                    End
                                Else
DoMsg:
                                    MsgBox("Another instance is already running")
                                    End
                                End If
                            End If
                        End If
                    Next
                Catch ex As Exception
                    'Probably not an admin, will handle later
                End Try

                Dim myOs As clsHWInfo.eWindowsPlatform = hwInf.WindowsPlatform
                If myOs = clsHWInfo.eWindowsPlatform.Unsupported Then
                    MsgBox("fTray is not supported on your operating system, application will exit")
                    End
                Else
                    LogWindow.WriteLog("Detected os = " & hwInf.OsName)
                End If
LinuxStart:
                Dim md5 As String = MD5CalcFile(ClientExe)
                If md5 = md5_Classic Then
                    LogWindow.WriteLog("Detected client = Classic console")
                ElseIf md5 = md5_Gpu2VistaW7 Then
                    LogWindow.WriteLog("Detected client = Gpu2VistaW7")
                ElseIf md5 = md5_Gpu2Xp03 Then
                    LogWindow.WriteLog("Detected client = Gpu2Xp03")
                ElseIf md5 = md5_Gpu3VistaW7 Then
                    LogWindow.WriteLog("Detected client = Gpu3VistaW7")
                ElseIf md5 = md5_Gpu3Xp03 Then
                    LogWindow.WriteLog("Detected client = Gpu3Xp03")
                ElseIf md5 = md5_SMP2 Then
                    LogWindow.WriteLog("Detected client = SMP2")
                Else
                    MsgBox("Your client does not match any of the tested clients, fTray might not work as expected!", vbOKOnly + vbInformation)
                    LogWindow.WriteLog("Client mismatch. Hash = " & md5 & " , Os information =" & hwInf.OsName)
                End If

                ClientControl = New clsClientControl(ClientPath, ClientExe)
                Application.DoEvents()
                If ClientControl.ClientNew Then
                    If Not My.Computer.FileSystem.FileExists(ClientPath & "\client.cfg") Then
                        LogWindow.WriteLog("Showing first configuration form for " & ClientExe)
                        fConfig.bDialog = True
                        'fConfig.DoLock = True
                        fConfig.NoCancel = False
                        fConfig.bCleanUp = True
                        fConfig.aClient = New frmConfig.clsClientInfo
                        fConfig.aClient.ClientExe = ClientExe
                        Application.Run(fConfig)
                        fConfig = New frmConfig
                        LogWindow.WriteLog("Reading back client, putting in the database")
                        ClientControl.MyClient = fConfig.ReadCurrentSettings(ClientExe)
                        If Not ClientControl.AddClientToDatabse(ClientControl.MyClient) Then
                            MsgBox("A problem occured when trying to store the client settings in the database")
                            End
                        End If
                    Else
                        LogWindow.WriteLog("Client not present in database, adding now..")
                        fstatus = New frmPBStatus
                        With fstatus
                            .StartPosition = FormStartPosition.CenterScreen
                            .SetMessage("Adding client to database, please be patient...")
                            .SetPBMax(100)
                            .Show()
                            Application.DoEvents()
                            .SetPBValue(-2)
                            ClientControl.MyClient = fConfig.ReadCurrentSettings(ClientControl.ClientEXE)
                            .SetMessage("Adding client to database")
                            Application.DoEvents()
                            If ClientControl.AddClientToDatabse(ClientControl.MyClient) Then
                                .Close()
                            Else
                                .SetMessage("Adding client to database failed!")
                                Application.DoEvents()
                                Dim rVal As MsgBoxResult = MsgBox("Please submit a bug report to the repository at http://code.google.com/p/mtray/ including the ftray.log file and the fahlog.txt located" & vbNewLine & _
                                                            ClientPath & " along with a description of your hardware and operating system." & vbNewLine & vbNewLine & "Application will not exit!", vbOKOnly + vbCritical + vbApplicationModal, "Crash!")
                                Application.Exit()
                            End If
                        End With
                    End If
                Else
                    'client will be read when monitor attaches!
                    ClientControl.MyClient.ClientEXE = ClientExe
                End If
                MySettings = New clsSettings(Application.StartupPath)
                If MySettings.IsEmpty Then
                    'Allow settings change first
                    MySettings.SetDefaults()
                    Dim nSettings As New frmOptions
                    nSettings.cmdCancel.Enabled = False
                    'Will return after save
                    Application.Run(nSettings)
                End If
                'init project info
                ProjectInfo = New clsProjectInfo(ClientPath)
                If ProjectInfo.Projects.IsEmpty Then ProjectInfo.GetProjects(MySettings.MySettings.URLsummary, True)

                'check run options here!
                Application.EnableVisualStyles()
                fLog.DoInit()
                Cursor.Current = Cursors.Default
                If MySettings.MySettings.StartMinimized Or My.Application.CommandLineArgs.Contains("minimized") Then
                    'should run minimized
                    fLog.WindowState = FormWindowState.Minimized
                    Application.Run(fLog)
                Else
                    fLog.Visible = True
                    Application.Run(fLog)
                End If
                bTryClose = True
            End If
        Catch ex As Exception
            MsgBox("Critical error!" & vbNewLine & ex.Message)
            LogWindow.WriteError("Submain", Err)
        Finally
            Try
                If bTryClose Then
                    If ClientControl.ConsoleStatus = clsClientControl.eStatus.active And ClientControl.WindowState = clsClientControl.eWindowState.Hidden Then
                        ClientControl.ShowConsole()
                    End If
                End If
            Catch ex As Exception

            End Try
        End Try
    End Sub
#End Region
#Region "Dll imports"
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Function GetConsoleWindow() As IntPtr
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Function AttachConsole(ByVal procid As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Function FreeConsole() As Boolean
    End Function
    <DllImport("user32.dll")> _
    Public Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function
    <DllImport("user32.dll")> _
    Public Function GetForegroundWindow() As Long
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)> _
    Public Function GenerateConsoleCtrlEvent(ByVal ctrlEvent As Integer, ByVal procid As Integer) As Boolean
    End Function
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Int32) As Boolean
    End Function
    <DllImport("kernel32.dll")> _
    Public Function GetCurrentProcess() As IntPtr
    End Function
    <DllImport("advapi32.dll", SetLastError:=True)> _
    Public Function OpenProcessToken(ByVal ProcessHandle As IntPtr, ByVal DesiredAccess As UInt32, ByRef TokenHandle As IntPtr) As Boolean
    End Function
    <DllImport("advapi32.dll", SetLastError:=True)> _
    Public Function GetTokenInformation(ByVal TokenHandle As IntPtr, ByVal TokenInformationClass As TOKEN_INFO_CLASS, ByVal TokenInformation As IntPtr, ByVal TokenInformationLength As Integer, ByRef ReturnLength As UInteger) As Boolean
    End Function
    <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Function DuplicateToken(ByVal ExistingTokenHandle As IntPtr, ByVal ImpersonationLevel As Security.Principal.TokenImpersonationLevel, <Out()> ByRef DuplicateTokenHandle As IntPtr) As Boolean
    End Function
#End Region
#Region "Properties"
#Region ".net version installed"
    Public Enum eDotNetVersion
        Err = 0
        Two = 1
        Three = 2
        ThreePointFive = 3
    End Enum
    Public ReadOnly Property InstalledDOTNETVersion() As eDotNetVersion
        Get
            Try
                Dim rKey As Microsoft.Win32.RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP")
                Dim sName() As String = rKey.GetSubKeyNames
                Dim rDot As eDotNetVersion = eDotNetVersion.Err
                For Each InstVersion As String In sName
                    InstVersion = InstVersion.Replace("v", "")
                    If Mid(InstVersion, 1, 3) = "2.0" And rDot < eDotNetVersion.Two Then
                        rDot = eDotNetVersion.Two
                    ElseIf Mid(InstVersion, 1, 3) = "3.0" And rDot < eDotNetVersion.Three Then
                        rDot = eDotNetVersion.Three
                    ElseIf Mid(InstVersion, 1, 3) = "3.5" And rDot < eDotNetVersion.ThreePointFive Then
                        rDot = eDotNetVersion.ThreePointFive
                    End If
                Next
                Return rDot
            Catch ex As Exception
                Return eDotNetVersion.Err
            End Try
        End Get
    End Property
#End Region

    Private _dtEOC_Failure As DateTime = DateTime.MinValue
    Private _dtFAHWEB_Failure As DateTime = DateTime.MinValue
    Public Property EOC_Net_Failure() As DateTime
        Get
            Return _dtEOC_Failure
        End Get
        Set(ByVal value As DateTime)
            _dtEOC_Failure = value
        End Set
    End Property
    Public Property FAHWEB_Failure() As DateTime
        Get
            Return _dtFAHWEB_Failure
        End Get
        Set(ByVal value As DateTime)
            _dtFAHWEB_Failure = value
        End Set
    End Property
#End Region
#Region "Constants"
    Public Enum TOKEN_ELEVATION_TYPE
        TokenElevationTypeDefault = 1
        TokenElevationTypeFull
        TokenElevationTypeLimited
    End Enum
    Public Enum TOKEN_INFO_CLASS
        TokenUser = 1
        TokenGroups
        TokenPrivileges
        TokenOwner
        TokenPrimaryGroup
        TokenDefaultDacl
        TokenSource
        TokenType
        TokenImpersonationLevel
        TokenStatistics
        TokenRestrictedSids
        TokenSessionId
        TokenGroupsAndPrivileges
        TokenSessionReference
        TokenSandBoxInert
        TokenAuditPolicy
        TokenOrigin
        TokenElevationType
        TokenLinkedToken
        TokenElevation
        TokenHasRestrictions
        TokenAccessInformation
        TokenVirtualizationAllowed
        TokenVirtualizationEnabled
        TokenIntegrityLevel
        TokenUIAccess
        TokenMandatoryPolicy
        TokenLogonSid
        MaxTokenInfoClass
        ' MaxTokenInfoClass should always be the last enum
    End Enum
    Public Const TOKEN_QUERY As UInt32 = &H8
    Public Const INT_SIZE As Integer = 4
    Public Const pciAti = "1002"
    Public Const pciNvidia = "10DE"
    Public Const SW_HIDE As Integer = 0
    Public Const SW_SHOWNORMAL As Integer = 1
    Public Const SW_SHOWMINIMIZED As Integer = 2
    Public Const SW_SHOWMAXIMIZED As Integer = 3
    Public Const SW_SHOWNOACTIVATE As Integer = 4
    Public Const SW_RESTORE As Integer = 9
    Public Const SW_SHOWDEFAULT As Integer = 10
    Public Const CTRL_C_EVENT As Integer = 0
#End Region
#Region "Functions and subs"
    Public Function MD5CalcFile(ByVal filepath As String) As String
        Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                Dim hash() As Byte = md5.ComputeHash(reader)
                Return bArrToStr(hash)
            End Using
        End Using
    End Function
    Private Function bArrToStr(ByVal bArr() As Byte) As String
        Dim strB As New System.Text.StringBuilder(bArr.Length * 2)
        For i As Integer = 0 To bArr.Length - 1
            strB.Append(bArr(i).ToString("X2"))
        Next
        Return strB.ToString().ToLower
    End Function
    Public ReadOnly Property IsThisClientAService(ByVal ClientEXE As String) As Boolean
        Get
            Try
                Dim strID As String = "0"
                Dim clientPath As String = Mid(ClientEXE, 1, ClientEXE.LastIndexOf("\"))
                If ClientEXE.Replace(clientPath, "") = "Folding@home-Win32-GPU.exe" Then Return False
                If My.Computer.FileSystem.FileExists(clientPath & "\client.cfg") Then
                    Dim fText As String = My.Computer.FileSystem.ReadAllText(clientPath & "\client.cfg")
                    Dim lText() As String = fText.Split(vbLf)
                    For Each cString As String In lText
                        If cString.ToUpper.Contains("MACHINEID") Then
                            strID = cString.ToUpper.Replace("MACHINEID=", "")
                            Exit For
                        End If
                    Next
                    If strID = "0" Then Return False
                End If
                Dim sString As String
                sString = "Folding@Home-CPU-[" & strID & "]"
                Dim rKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services")
                Dim sVal() As String = rKey.GetSubKeyNames
                For Each sSer As String In sVal
                    If sSer.ToUpper = sString.ToUpper Then
                        Return True
                    End If
                Next
                Return False
            Catch ex As Exception
                Debug.Print(ex.Message)
                Return Nothing
            End Try
        End Get
    End Property
    Public Sub WaitMS(ByVal MS As Long)
        Try 'Does not fix the core hogging while configuring/reading client settings
            Threading.Thread.Sleep(MS)
        Catch ex As Exception

        End Try
    End Sub
    ' Source http://www.vbdotnetheaven.com/UploadFile/mahesh/stringvbnet11052005021335AM/stringvbnet.aspx 
    Public Function GetPassword() As String
        Dim builder As New StringBuilder()
        For xLoop As Int16 = 2 To 20 Step 2
            builder.Append(RandomString(xLoop - 1, True))
            builder.Append(RandomNumber(1000, 9999))
            builder.Append(RandomString(xLoop + 1, False))
        Next
        Return builder.ToString()
    End Function 'GetPassword
    Private Function RandomNumber(ByVal min As Integer, ByVal max As Integer) As Integer
        Dim random As New Random()
        Return random.Next(min, max)
    End Function
    Private Function RandomString(ByVal size As Integer, ByVal lowerCase As Boolean) As String
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        Dim i As Integer
        For i = 0 To size - 1
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next
        If lowerCase Then
            Return builder.ToString().ToLower()
        End If
        Return builder.ToString()
    End Function 'RandomString
#End Region
End Module
