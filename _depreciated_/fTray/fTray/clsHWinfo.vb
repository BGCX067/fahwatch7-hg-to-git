'   fTray Hardware info class
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
Imports System.Management
Imports System.IO
Imports System.Threading
Imports System.Security.Principal
Imports OpenCLNet.OpenCL
Imports OpenHardwareMonitor
Imports OpenHardwareMonitor.Hardware
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Public Class clsHWInfo
#Region "Hardware queries and monitoring"
    Private ohmComp As New OpenHardwareMonitor.Hardware.Computer
    Private ohm_visitor As OpenHardwareMonitor.Hardware.IVisitor
    Private WithEvents tClock As System.Timers.Timer
    Private iVisitor As OpenHardwareMonitor.Hardware.IVisitor
    Private iHandler As New OpenHardwareMonitor.Hardware.SensorEventHandler(AddressOf SensorEvenentHandler)
    Public Event Outputrecieved(ByVal Message As String)
    Public Sub SensorEvenentHandler(ByVal Sensor As OpenHardwareMonitor.Hardware.Sensor)
        Try
            RaiseEvent Outputrecieved(DateTime.Now.ToLongTimeString & " -  " & Sensor.Name & " : " & Sensor.Value & " : " & Sensor.SensorType.ToString & vbNewLine)
            Debug.Print(DateTime.Now.ToLongTimeString & " -  " & Sensor.Name & " : " & Sensor.Value & " : " & Sensor.SensorType.ToString)
            Debug.Print(Sensor.Hardware.Name & " - " & Sensor.Hardware.HardwareType.ToString & " - " & " - " & Sensor.Identifier.ToString)
            'parse through and fill properties, allow queries to readonly properties to update gui
        Catch ex As Exception

        End Try
    End Sub
    Private Sub HardwareAdded(ByVal hardware As IHardware)
        Try
            hardware.Update()
            iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
            hardware.Accept(iVisitor)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub HardwareRemoved(ByVal Hardware As OpenHardwareMonitor.Hardware.IHardware)

    End Sub
    Private Sub tClock_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tClock.Elapsed
        Try
            iVisitor.VisitComputer(ohmComp)
            Return
            For Each Hardware In ohmComp.Hardware
                Hardware.Update()
                iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
                Hardware.Accept(iVisitor)
                For Each SubHardware As Hardware In Hardware.SubHardware
                    SubHardware.Update()
                    iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
                    SubHardware.Accept(iVisitor)
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Function InitOHM() As Boolean
        Try
            ohmComp = New Computer
            AddHandler ohmComp.HardwareAdded, AddressOf HardwareAdded
            AddHandler ohmComp.HardwareRemoved, AddressOf HardwareRemoved
            ohmComp.Open()
            'logwindow.WriteLog("OpenHardware monitor initialized succesfully")
            tClock = New Timers.Timer
            tClock.Interval = 1000
            tClock.AutoReset = True
            tClock.Enabled = True
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            'logwindow.WriteError("HWinf_InitOHM", Err)
            Return False
        End Try
    End Function
    Public Sub CloseOHM()
        tClock.Enabled = False
        ohmComp.Close()
    End Sub


    Public Structure sGpuInfo
        Public IsEmpty As Boolean
        Public Driver As String
        Public Name As String
        Public Vendor As String
        Public CoreLoad As Int16
        Public MemoryControllerLoad As Int16
        Public Temperature As Int16
        Public FanSpeed As Int16
        Public MemoryUsage As Int16
        Public CoreClock As Int16
        Public RamClock As Int16
        Public ShadersClock As Int16
        Public AdaptorRam As Int64
        Public Index As Short
    End Structure
    Private _Ginf() As sGpuInfo
    Public ReadOnly Property GPU_Count As Int16
        Get
            Return _Ginf.Count
        End Get
    End Property
    Public Function Gpu_Inf(ByVal Index) As sGpuInfo
        Return _Ginf(Index)
    End Function
    Public ReadOnly Property GetGpu(ByVal Index As Short) As sGpuInfo
        Get
            Try
                Return _Ginf(Index)
            Catch ex As Exception

            End Try
        End Get
    End Property
    Public Function FillGpuInfo() As Boolean
        Try
            For Each oclPlatform As OpenCLNet.Platform In OpenCLNet.OpenCL.GetPlatforms
                Dim devices() As OpenCLNet.Device
                devices = oclPlatform.QueryDevices(OpenCLNet.DeviceType.GPU)
                For Each gpu As OpenCLNet.Device In devices
                    If gpu.Vendor.ToUpper.Contains("NVIDIA") Or gpu.Vendor.ToUpper.Contains("ATI") Or gpu.Vendor.ToUpper.Contains("AMD") Then
                        Dim nGpu As New sGpuInfo
                        With nGpu
                            .AdaptorRam = gpu.GlobalMemSize / 1024 / 1024
                            .ShadersClock = gpu.MaxClockFrequency
                            .Name = gpu.Name
                            .Driver = gpu.DriverVersion
                            .Vendor = gpu.Vendor
                        End With
                        Dim bAdded As Boolean = False
                        Try
                            If CType(_Ginf(_Ginf.GetUpperBound(0)), sGpuInfo).IsEmpty Then
                                _Ginf(_Ginf.GetUpperBound(0)) = nGpu
                            End If
                        Catch ex As NullReferenceException
                            'array not initialized
                            ReDim _Ginf(0 To 0)
                            _Ginf(_Ginf.GetUpperBound(0)) = nGpu
                        Finally
                            bAdded = ReferenceEquals(_Ginf(_Ginf.GetUpperBound(0)), nGpu)
                        End Try
                        If Not bAdded Then
                            ReDim Preserve _Ginf(0 To _Ginf.GetUpperBound(0) + 1)
                            _Ginf(_Ginf.GetUpperBound(0)) = nGpu
                        End If
                    End If
                Next
            Next
            Return True
        Catch ex As Exception
            LogWindow.WriteError("HWinfo_FillGpus", Err)
            Return False
        End Try
    End Function
    Public Function UpdateGpu(ByVal Index As Int16) As Boolean
        Try
            'Update gpu with info from ohm

            Return True
        Catch ex As Exception
            LogWindow.WriteError("hwInfo_UpdateGPU", Err)
            Return False
        End Try
    End Function
    Public Structure sCpuInfo
        Public IsEmpty As Boolean
        Public Name As String
        Public Vendor As String
        Public Structure sCoreInfo
            Public Index As Short
            Public IsEmpty As Boolean
            Public Temperature As Int32
            Public Load As Int32
            Public Clock As Int32
        End Structure
        Private _Core() As sCoreInfo
        Public CoreCount As Int16
        Public LogicalCPUs As Int16
        Public TotalLoad As Int32
        Public Sub AddACore(ByVal Core As sCoreInfo)
            Try
                Try
                    If _Core(0).IsEmpty Then
                        ReDim _Core(0 To 0)
                    End If
                Catch ex As Exception
                    ReDim _Core(0 To 0)
                End Try
                ReDim Preserve _Core(0 To _Core.GetUpperBound(0) + 1)
                _Core(_Core.GetUpperBound(0)) = Core
            Catch ex As Exception
                LogWindow.WriteError("sCpuInfo_AddACore", Err)
            End Try
        End Sub
        Public Property Core(ByVal Index) As sCoreInfo
            Get
                Return _Core(Index)
            End Get
            Set(ByVal value As sCoreInfo)
                _Core(Index) = value
            End Set
        End Property
        Public ReadOnly Property AvgTemp
            Get
                Try
                    Dim iTotal As Int32 = 0
                    For Each dCore As sCoreInfo In _Core
                        iTotal += dCore.Temperature
                    Next
                    Return (iTotal / _Core.Count)
                Catch ex As Exception
                    Return 0
                End Try
            End Get
        End Property
        Public ReadOnly Property AvgLoad
            Get
                Try
                    Dim iTotal As Int32 = 0
                    For Each dCore As sCoreInfo In _Core
                        iTotal += dCore.Load
                    Next
                    Return (iTotal / _Core.Count)
                Catch ex As Exception
                    Return 0
                End Try
            End Get
        End Property
        Public ReadOnly Property HT As Boolean
            Get
                Return (LogicalCPUs <> CoreCount)
            End Get
        End Property
    End Structure
    Private _Cinf() As sCpuInfo
    Public ReadOnly Property CPU_Count
        Get
            Return _Cinf.Count
        End Get
    End Property
    Public ReadOnly Property CPU_Inf(ByVal Index As Int16) As sCpuInfo
        Get
            Return _Cinf(Index)
        End Get
    End Property
    Public Function FillCpuInfo() As Boolean
        Try
            Dim query As ObjectQuery, searcher As ManagementObjectSearcher, queryCollection As ManagementObjectCollection
            query = New ObjectQuery("SELECT * FROM Win32_Processor")
            searcher = New ManagementObjectSearcher(query)
            queryCollection = searcher.Get()
            For Each m As ManagementObject In queryCollection
                Dim nCpu As New sCpuInfo
                nCpu.IsEmpty = True
                With nCpu
                    .CoreCount = m.GetPropertyValue("NumberOfCores")
                    _intCpuCoresTotal += CInt(.CoreCount)
                    _intCpuCount += 1
                    .LogicalCPUs = m.GetPropertyValue("NumberOfLogicalProcessors")
                    .Name = m.GetPropertyValue("Name")
                    .Vendor = m.GetPropertyValue("Manufacturer")
                    Dim nCores(0 To .CoreCount) As sCpuInfo.sCoreInfo
                    For xInt As Int16 = 0 To .CoreCount
                        With nCores(xInt)
                            .IsEmpty = False
                            .Index = xInt
                            .Clock = m.GetPropertyValue("CurrentClockSpeed")
                        End With
                        nCpu.AddACore(nCores(xInt))
                    Next
                    If .LogicalCPUs <> .CoreCount Then _ht = True
                End With
                nCpu.IsEmpty = False
                Dim bAdded As Boolean = False
                Try
                    If CType(_Cinf(_Cinf.GetUpperBound(0)), sCpuInfo).IsEmpty Then
                        _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                    End If
                Catch ex As NullReferenceException
                    'array not initialized
                    ReDim _Cinf(0 To 0)
                    _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                Finally
                    bAdded = ReferenceEquals(_Cinf(_Cinf.GetUpperBound(0)), nCpu)
                End Try
                If Not bAdded Then
                    ReDim Preserve _Cinf(0 To _Cinf.GetUpperBound(0) + 1)
                    _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                End If
            Next
            Return True
        Catch ex As Exception
            LogWindow.WriteError("HWinfo_WMICPU", Err)
            LogWindow.WriteLog("Trying registry method")
            Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("HARDWARE\DESCRIPTION\System\CentralProcessor")
            Dim sKey() As String = rKey.GetSubKeyNames
            If sKey.GetUpperBound(0) = 0 Then
                LogWindow.WriteLog("Could not get cpu info from registry")
                Return False
            Else
                Try
                    Dim vKey As Microsoft.Win32.RegistryKey
                    vKey = rKey.OpenSubKey(sKey(0))
                    Dim nCpu As New sCpuInfo
                    With nCpu
                        .CoreCount = sKey.Count
                        _intCpuCoresTotal += CInt(.CoreCount)
                        _intCpuCount += 1
                        .LogicalCPUs = .CoreCount
                        .Name = vKey.GetValue("ProcessorNameString")
                        .Vendor = vKey.GetValue("VendorIdentifier")
                        Dim nCores(0 To .CoreCount) As sCpuInfo.sCoreInfo
                        For xInt As Int16 = 0 To .CoreCount
                            With nCores(xInt)
                                .IsEmpty = False
                                .Index = xInt
                                .Clock = vKey.GetValue("~MHz")
                            End With
                            nCpu.AddACore(nCores(xInt))
                        Next
                        If .LogicalCPUs <> .CoreCount Then _ht = True
                    End With
                    nCpu.IsEmpty = False
                    Dim bAdded As Boolean = False
                    Try
                        If CType(_Cinf(_Cinf.GetUpperBound(0)), sCpuInfo).IsEmpty Then
                            _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                        End If
                    Catch ex2 As NullReferenceException
                        'array not initialized
                        ReDim _Cinf(0 To 0)
                        _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                    Finally
                        bAdded = ReferenceEquals(_Cinf(_Cinf.GetUpperBound(0)), nCpu)
                    End Try
                    If Not bAdded Then
                        ReDim Preserve _Cinf(0 To _Cinf.GetUpperBound(0) + 1)
                        _Cinf(_Cinf.GetUpperBound(0)) = nCpu
                    End If
                    Return True
                Catch ex1 As Exception
                    LogWindow.WriteError("HWinfo_RegistryCPU", Err)
                    Return False
                End Try
            End If
        End Try
    End Function
    Public Function UpdateCpuInf() As Boolean
        Try
            'Fill with ohm output here

            Return True
        Catch ex As Exception
            LogWindow.WriteError("hwInfo_UpdateCpuInf", Err)
            Return False
        End Try
    End Function

    Private _X64 As Boolean, _intCpuCount As Int16, _intCpuCoresTotal As Int16, _strCpuName As String, _strOSName As String, _OsSupported As Boolean
    Private _strUserName As String, _IsAdmin As Boolean, _CanElevate As Boolean
    Public Enum eWindowsPlatform
        Unsupported = 0
        WindowsXPsp3 = 1
        Windows2003 = 2
        WindowsVista = 3
        Windows2008 = 4
        Windows7 = 5
    End Enum
    Private _WinPlatform As eWindowsPlatform = eWindowsPlatform.Unsupported
    Public ReadOnly Property WindowsPlatform() As eWindowsPlatform
        Get
            Return _WinPlatform
        End Get
    End Property
    Private _Password As String = vbNullString
    Public Property AccPassword() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property
    Private _ht As Boolean = False
    Public ReadOnly Property HasHT() As Boolean
        Get
            Return _ht
        End Get
    End Property
    Public ReadOnly Property IsAdmin() As Boolean
        Get
            Return _IsAdmin
        End Get
    End Property
    Public ReadOnly Property CanElevate As Boolean
        Get
            Try
                Return _CanElevate
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property
    Public ReadOnly Property IsX64() As Boolean
        Get
            Return _X64
        End Get
    End Property
    Public ReadOnly Property OsName() As String
        Get
            Return Environment.OSVersion.VersionString
        End Get
    End Property
    Public ReadOnly Property TotalCores() As Int16
        Get
            Return _intCpuCoresTotal
        End Get
    End Property
    Public ReadOnly Property InstalledCPUs() As Int16
        Get
            Return _intCpuCount
        End Get
    End Property

#End Region
    
    Public Function Is2kHotFix() As Boolean
        Try
            ' select * from Win32_QuickFixEngineering where hotfixid = 982861
        Catch ex As Exception

        End Try
    End Function
    Public Function FillOsInfo() As Boolean
        Try
            If Environment.OSVersion.Version.Major < 5 Or (Environment.OSVersion.Version.Major = 5 And Environment.OSVersion.Version.Minor = 0) Then
                _WinPlatform = eWindowsPlatform.Unsupported
            ElseIf Environment.OSVersion.Version.Major = 5 Then
                Select Case Environment.OSVersion.Version.Minor
                    Case 0I
                        _WinPlatform = eWindowsPlatform.Unsupported
                    Case 1I
                        If Environment.OSVersion.ServicePack < 3 Then
                            _WinPlatform = eWindowsPlatform.Unsupported
                        Else
                            _WinPlatform = eWindowsPlatform.WindowsXPsp3
                        End If
                    Case 2I
                        _WinPlatform = eWindowsPlatform.Windows2003
                End Select
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                Select Case Environment.OSVersion.Version.Minor
                    Case 0I
                        _WinPlatform = eWindowsPlatform.WindowsVista
                    Case 1I
                        _WinPlatform = eWindowsPlatform.Windows2008
                    Case 2I
                        _WinPlatform = eWindowsPlatform.Windows7
                End Select
            Else
                LogWindow.WriteLog("Could not identify platform from the following version object: " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.MajorRevision & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.MinorRevision & "." & Environment.OSVersion.Version.Revision & "." & Environment.OSVersion.Version.Build)
                _WinPlatform = eWindowsPlatform.Unsupported
            End If
            Return True
        Catch ex As Exception
            LogWindow.WriteError("HWinfo_FillOSinfo", Err)
            Return False
        End Try
    End Function
    Public Function FillIsX64() As Boolean
        Try
            If IntPtr.Size = 8 Then
                'win64
                _X64 = True
            ElseIf IntPtr.Size = 4 Then
                'win32
                _X64 = False
            End If
        Catch ex As Exception
            LogWindow.WriteError("HWinfo_IsX64", Err)
            _X64 = False
            Return False
        End Try
    End Function
    Public Function FillIsAdmin() As Boolean
        Try
            Dim mIdentity As System.Security.Principal.WindowsIdentity = WindowsIdentity.GetCurrent()
            _strUserName = mIdentity.Name
            Dim mPrincipal = New WindowsPrincipal(mIdentity)
            If mPrincipal.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
                _IsAdmin = True
            Else
                FillCanElevate()
                _IsAdmin = False
            End If
            Return True
        Catch ex As Exception
            LogWindow.WriteLog("Could not check account details")
            _IsAdmin = False
            Return False
        End Try
    End Function
    Public Function FillCanElevate() As Boolean
        Try
            If Environment.OSVersion.Version.Major > 5 Then
                Try
                    Dim myToken As IntPtr
                    Dim elevationType As TOKEN_ELEVATION_TYPE
                    Dim dwSize As UInteger
                    Dim pElevationType As IntPtr = Marshal.AllocHGlobal(INT_SIZE)
                    OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, myToken)
                    GetTokenInformation(myToken, TOKEN_INFO_CLASS.TokenElevationType, pElevationType, INT_SIZE, dwSize)                'CAST THE RESULT TO ENUM TYPE
                    elevationType = DirectCast(Marshal.ReadInt32(pElevationType), TOKEN_ELEVATION_TYPE)
                    Marshal.FreeHGlobal(pElevationType)
                    Return (elevationType = TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited) OrElse (elevationType = TOKEN_ELEVATION_TYPE.TokenElevationTypeFull)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Return False
                End Try
            Else
                Return _IsAdmin
            End If
        Catch ex As Exception
            LogWindow.WriteError("hwInfo_FillCanElevate", Err)
            Return False
        End Try
    End Function
    
End Class
