Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Namespace devManager
    '
    ' * HardwareHelperLib
    ' * ===========================================================
    ' * Windows XP SP2, VS2005 C#.NET, DotNet 2.0
    ' * HH Lib is a hardware helper for library for C#.
    ' * It can be used for notifications of hardware add/remove
    ' * events, retrieving a list of hardware currently connected,
    ' * and enabling or disabling devices.
    ' * ===========================================================
    ' * LOG:      Who?    When?       What?
    ' * (v)1.0.0  WJF     11/26/07    Original Implementation
    ' 


#Region "Unmanaged"
    'Now marshall the params prior to calling the functions
    'Name:     ChangeDeviceState
    'Inputs:   pointer to hdev, bool on or off
    'Outputs:  boll
    'Errors:   This method may log the following errors.
    '          NONE
    'Remarks:  Attempts to enable or disable a device driver.
    Public Class Native
        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function RegisterDeviceNotification(ByVal hRecipient As IntPtr, ByVal NotificationFilter As DEV_BROADCAST_DEVICEINTERFACE, ByVal Flags As UInt32) As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
        Public Shared Function UnregisterDeviceNotification(ByVal hHandle As IntPtr) As UInteger
        End Function

        <DllImport("setupapi.dll", SetLastError:=True)> _
        Public Shared Function SetupDiGetClassDevs(ByRef gClass As Guid, ByVal iEnumerator As UInt32, ByVal hParent As IntPtr, ByVal nFlags As UInt32) As IntPtr
        End Function

        <DllImport("setupapi.dll", SetLastError:=True)> _
        Public Shared Function SetupDiDestroyDeviceInfoList(ByVal lpInfoSet As IntPtr) As Integer
        End Function

        <DllImport("setupapi.dll", SetLastError:=True)> _
        Public Shared Function SetupDiEnumDeviceInfo(ByVal lpInfoSet As IntPtr, ByVal dwIndex As UInt32, ByVal devInfoData As SP_DEVINFO_DATA) As Boolean
        End Function

        <DllImport("setupapi.dll", SetLastError:=True)> _
        Public Shared Function SetupDiGetDeviceRegistryProperty(ByVal lpInfoSet As IntPtr, ByVal DeviceInfoData As SP_DEVINFO_DATA, ByVal [Property] As UInt32, ByVal PropertyRegDataType As UInt32, ByVal PropertyBuffer As StringBuilder, ByVal PropertyBufferSize As UInt32, _
   ByVal RequiredSize As IntPtr) As Boolean
        End Function

        <DllImport("setupapi.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
        Public Shared Function SetupDiSetClassInstallParams(ByVal DeviceInfoSet As IntPtr, ByVal DeviceInfoData As IntPtr, ByVal ClassInstallParams As IntPtr, ByVal ClassInstallParamsSize As Integer) As Boolean
        End Function
        <DllImport("setupapi.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function SetupDiCallClassInstaller(ByVal InstallFunction As UInt32, ByVal DeviceInfoSet As IntPtr, ByVal DeviceInfoData As IntPtr) As [Boolean]
        End Function


        ' Structure with information for RegisterDeviceNotification.
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure DEV_BROADCAST_HANDLE
            Public dbch_size As Integer
            Public dbch_devicetype As Integer
            Public dbch_reserved As Integer
            Public dbch_handle As IntPtr
            Public dbch_hdevnotify As IntPtr
            Public dbch_eventguid As Guid
            Public dbch_nameoffset As Long
            Public dbch_data As Byte
            Public dbch_data1 As Byte
        End Structure

        ' Struct for parameters of the WM_DEVICECHANGE message
        <StructLayout(LayoutKind.Sequential)> _
        Public Class DEV_BROADCAST_DEVICEINTERFACE
            Public dbcc_size As Integer
            Public dbcc_devicetype As Integer
            Public dbcc_reserved As Integer
        End Class

        'SP_DEVINFO_DATA
        <StructLayout(LayoutKind.Sequential)> _
        Public Class SP_DEVINFO_DATA
            Public cbSize As Integer
            Public classGuid As Guid
            Public devInst As Integer
            Public reserved As ULong
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Class SP_DEVINSTALL_PARAMS
            Public cbSize As Integer
            Public Flags As Integer
            Public FlagsEx As Integer
            Public hwndParent As IntPtr
            Public InstallMsgHandler As IntPtr
            Public InstallMsgHandlerContext As IntPtr
            Public FileQueue As IntPtr
            Public ClassInstallReserved As IntPtr
            Public Reserved As Integer
            <MarshalAs(UnmanagedType.LPTStr)> _
            Public DriverPath As String
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Class SP_PROPCHANGE_PARAMS
            Public ClassInstallHeader As New SP_CLASSINSTALL_HEADER()
            Public StateChange As Integer
            Public Scope As Integer
            Public HwProfile As Integer
        End Class

        <StructLayout(LayoutKind.Sequential)> _
        Public Class SP_CLASSINSTALL_HEADER
            Public cbSize As Integer
            Public InstallFunction As Integer
        End Class

        'PARMS
        Public Const DIGCF_ALLCLASSES As Integer = (&H4)
        Public Const DIGCF_PRESENT As Integer = (&H2)
        Public Const INVALID_HANDLE_VALUE As Integer = -1
        Public Const SPDRP_DEVICEDESC As Integer = (&H0)
        Public Const MAX_DEV_LEN As Integer = 1000
        Public Const DEVICE_NOTIFY_WINDOW_HANDLE As Integer = (&H0)
        Public Const DEVICE_NOTIFY_SERVICE_HANDLE As Integer = (&H1)
        Public Const DEVICE_NOTIFY_ALL_INTERFACE_CLASSES As Integer = (&H4)
        Public Const DBT_DEVTYP_DEVICEINTERFACE As Integer = (&H5)
        Public Const DBT_DEVNODES_CHANGED As Integer = (&H7)
        Public Const WM_DEVICECHANGE As Integer = (&H219)
        Public Const DIF_PROPERTYCHANGE As Integer = (&H12)
        Public Const DICS_FLAG_GLOBAL As Integer = (&H1)
        Public Const DICS_FLAG_CONFIGSPECIFIC As Integer = (&H2)
        Public Const DICS_ENABLE As Integer = (&H1)
        Public Const DICS_DISABLE As Integer = (&H2)
    End Class

#End Region

    Public Class devManager

        Private m_Version As New Version(1, 0, 0)
        Private Function ChangeDeviceState(ByVal hDevInfo As IntPtr, ByVal devInfoData As Native.SP_DEVINFO_DATA, ByVal bEnable As Boolean) As Boolean
            Try
                'Marshalling vars
                Dim szOfPcp As Integer
                Dim ptrToPcp As IntPtr
                Dim szDevInfoData As Integer
                Dim ptrToDevInfoData As IntPtr

                Dim pcp As New Native.SP_PROPCHANGE_PARAMS()
                If bEnable Then
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_ENABLE
                    pcp.Scope = Native.DICS_FLAG_GLOBAL
                    pcp.HwProfile = 0

                    'Marshal the params
                    szOfPcp = Marshal.SizeOf(pcp)
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp)
                    Marshal.StructureToPtr(pcp, ptrToPcp, True)
                    szDevInfoData = Marshal.SizeOf(devInfoData)
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData)

                    If Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(GetType(Native.SP_PROPCHANGE_PARAMS))) Then

                        Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData)
                    End If
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_ENABLE
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC
                    pcp.HwProfile = 0
                Else
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_DISABLE
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC
                    pcp.HwProfile = 0
                End If
                'Marshal the params
                szOfPcp = Marshal.SizeOf(pcp)
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp)
                Marshal.StructureToPtr(pcp, ptrToPcp, True)
                szDevInfoData = Marshal.SizeOf(devInfoData)
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData)
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, True)
                Dim rslt1 As Boolean = Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(GetType(Native.SP_PROPCHANGE_PARAMS)))
                Dim rstl2 As Boolean = Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData)
                If (Not rslt1) OrElse (Not rstl2) Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
#Region "Public Methods"
        Private lDevices As New List(Of sDevice)
        Public Class sDevice
            Public Property Name As String = ""
            Public Property intPtrDeviceHandle As New IntPtr
            Public Property DeviceInfo As New Native.SP_DEVINFO_DATA
            Public Property DeviceInterface As New Native.DEV_BROADCAST_DEVICEINTERFACE
            Sub init()
                Name = New String("")
                intPtrDeviceHandle = New IntPtr(0)
                DeviceInfo = New Native.SP_DEVINFO_DATA
                DeviceInterface = New Native.DEV_BROADCAST_DEVICEINTERFACE
            End Sub
        End Class
        'Private lDevices As New List(Of sDevice)
        Public Function EnumDevices() As Boolean
            Try
                lDevices.Clear()
                Dim myGUID As Guid = System.Guid.Empty
                'Dim hDevInfo As IntPtr = Native.SetupDiGetClassDevs(myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES Or Native.DIGCF_PRESENT)
                Dim hDevInfo As IntPtr = Native.SetupDiGetClassDevs(myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES) ' Or Native.DIGCF_PRESENT)
                If hDevInfo.ToInt32() = Native.INVALID_HANDLE_VALUE Then
                    Throw New Exception("Invalid Handle")
                End If
                Dim DeviceInfoData As Native.SP_DEVINFO_DATA
                DeviceInfoData = New Native.SP_DEVINFO_DATA()
                DeviceInfoData.cbSize = 28
                'is devices exist for class
                DeviceInfoData.devInst = 0
                DeviceInfoData.classGuid = System.Guid.Empty
                DeviceInfoData.reserved = 0
                Dim i As UInt32
                Dim DeviceName As New StringBuilder("")
                DeviceName.Capacity = Native.MAX_DEV_LEN
                i = 0
                While Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData)
                    Static dName As String = DeviceName.ToString
                    Dim dEnd As DateTime = DateTime.Now.AddSeconds(1)
                    While Not Native.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Native.SPDRP_DEVICEDESC, 0, DeviceName, Native.MAX_DEV_LEN, IntPtr.Zero)
                        If DateTime.Now > dEnd Then Exit While
                    End While
                    If Not dName = DeviceName.ToString Then
                        Dim nDev As New sDevice
                        nDev.init()
                        With nDev
                            .Name = DeviceName.ToString
                            .intPtrDeviceHandle = hDevInfo
                            .DeviceInfo() = DeviceInfoData
                        End With
                        lDevices.Add(nDev)
                    End If
                    i += 1
                    DeviceInfoData = New Native.SP_DEVINFO_DATA()
                    DeviceInfoData.cbSize = 28
                    'is devices exist for class
                    DeviceInfoData.devInst = i
                    DeviceInfoData.classGuid = System.Guid.Empty
                    DeviceInfoData.reserved = 0
                End While
                Native.SetupDiDestroyDeviceInfoList(hDevInfo)
                Return CBool(lDevices.Count > 0)
            Catch ex As Exception
                'log error
                Return False
            End Try
        End Function
        Public Function GetDeviceList() As List(Of sDevice)
            Return lDevices
        End Function
        Public Function ChangeDeviceState(ByVal Device As sDevice, ByVal Enabled As Boolean) As Boolean
            Try
                REM If X64 then use x64.exe
                If IntPtr.Size = 4 Then
                    Dim sI As New ProcessStartInfo
                    With sI
                        .FileName = "x64.exe"
                        .WorkingDirectory = My.Application.Info.DirectoryPath
                        .UseShellExecute = True
                        If Enabled Then
                            .Arguments = "ENABLE-" & Device.Name & "-" & Device.intPtrDeviceHandle.ToString
                        Else
                            .Arguments = "DISABLE-" & Device.Name & "-" & Device.intPtrDeviceHandle.ToString
                        End If
                    End With
                    Dim pProc As Process = Process.Start(sI)
                    While Not pProc.HasExited
                        Threading.Thread.SpinWait(150)
                    End While
                    Return True
                End If
                ChangeIt(Device.intPtrDeviceHandle, Device.DeviceInfo, Enabled, Device.Name)
            Catch ex As Exception
                Throw New Exception("Failed to enumerate device tree!", ex)
                Return False
            End Try
            Return True
        End Function
        'Name:     GetAll
        'Inputs:   none
        'Outputs:  string array
        'Errors:   This method may throw the following errors.
        '          Failed to enumerate device tree!
        '          Invalid handle!
        'Remarks:  This is code I cobbled together from a number of newsgroup threads
        '          as well as some C++ stuff I translated off of MSDN.  Seems to work.
        '          The idea is to come up with a list of devices, same as the device
        '          manager does.  Currently it uses the actual "system" names for the
        '          hardware.  It is also possible to use hardware IDs.  See the docs
        '          for SetupDiGetDeviceRegistryProperty in the MS SDK for more details.
        Public Function GetAll() As String()
            Dim HWList As New List(Of String)()
            Try
                Dim myGUID As Guid = System.Guid.Empty
                Dim hDevInfo As IntPtr = Native.SetupDiGetClassDevs(myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES Or Native.DIGCF_PRESENT)
                If hDevInfo.ToInt32() = Native.INVALID_HANDLE_VALUE Then
                    Throw New Exception("Invalid Handle")
                End If
                Dim DeviceInfoData As Native.SP_DEVINFO_DATA
                DeviceInfoData = New Native.SP_DEVINFO_DATA()
                DeviceInfoData.cbSize = 28
                'is devices exist for class
                DeviceInfoData.devInst = 0
                DeviceInfoData.classGuid = System.Guid.Empty
                DeviceInfoData.reserved = 0
                Dim i As UInt32
                Dim DeviceName As New StringBuilder("")
                DeviceName.Capacity = Native.MAX_DEV_LEN
                i = 0
                While Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData)
                    Dim dEnd As DateTime = DateTime.Now.AddSeconds(1)
                    While Not Native.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Native.SPDRP_DEVICEDESC, 0, DeviceName, Native.MAX_DEV_LEN, IntPtr.Zero) And Not DeviceName.ToString.ToUpper.Contains("USB")
                        If DateTime.Now > dEnd Then Exit While
                    End While
                    If Not HWList.Contains((DeviceName.ToString() & " " & DeviceInfoData.devInst.ToString)) Then HWList.Add(DeviceName.ToString() & " " & DeviceInfoData.devInst.ToString)
                    i += 1
                End While
                Native.SetupDiDestroyDeviceInfoList(hDevInfo)
            Catch ex As Exception
                Throw New Exception("Failed to enumerate device tree!", ex)
            End Try
            Return HWList.ToArray()
        End Function
        'Name:     SetDeviceState
        'Inputs:   string[],bool
        'Outputs:  bool
        'Errors:   This method may throw the following exceptions.
        '          Failed to enumerate device tree!
        'Remarks:  This is nearly identical to the method above except it
        '          tries to match the hardware description against the criteria
        '          passed in.  If a match is found, that device will the be
        '          enabled or disabled based on bEnable.
        Public Overloads Function SetDeviceState(ByVal match As String, ByVal bEnable As Boolean) As Boolean
            Try
                REM If X64 then use x64.exe
                If IntPtr.Size = 4 Then
                    Dim sI As New ProcessStartInfo
                    With sI
                        .FileName = "x64.exe"
                        .WorkingDirectory = My.Application.Info.DirectoryPath
                        .UseShellExecute = True
                        If bEnable Then
                            .Arguments = "ENABLE-NVIDIA"
                        Else
                            .Arguments = "DISABLE-NVIDIA"
                        End If
                    End With
                    Dim pProc As Process = Process.Start(sI)
                    While Not pProc.HasExited
                        Threading.Thread.SpinWait(150)
                    End While
                    Return True
                End If
                Dim myGUID As Guid = System.Guid.Empty
                Dim hDevInfo As IntPtr = Native.SetupDiGetClassDevs(myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES Or Native.DIGCF_PRESENT)
                If hDevInfo.ToInt32() = Native.INVALID_HANDLE_VALUE Then
                    Return False
                End If
                Dim DeviceInfoData As Native.SP_DEVINFO_DATA
                DeviceInfoData = New Native.SP_DEVINFO_DATA()
                DeviceInfoData.cbSize = 28
                'is devices exist for class
                DeviceInfoData.devInst = 0
                DeviceInfoData.classGuid = System.Guid.Empty
                DeviceInfoData.reserved = 0
                Dim i As UInt32
                Dim DeviceName As New StringBuilder("")
                DeviceName.Capacity = Native.MAX_DEV_LEN
                i = 0
                While Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData)
                    'Declare vars
                    Dim Dend As DateTime = DateTime.Now.AddSeconds(1)
                    While Not Native.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Native.SPDRP_DEVICEDESC, 0, DeviceName, Native.MAX_DEV_LEN, IntPtr.Zero)
                        If DateTime.Now > Dend Then Exit While
                    End While
                    Dim dS As String = DeviceName.ToString.ToUpper
                    Debug.WriteLine(i.ToString & "-" & dS)
                    If dS.ToUpper.IndexOf(match.ToUpper) <> -1 Or dS.ToUpper = match.ToUpper Then
                        ChangeIt(hDevInfo, DeviceInfoData, bEnable, DeviceName.ToString)
                    End If
                    i += 1
                End While
                Native.SetupDiDestroyDeviceInfoList(hDevInfo)
            Catch ex As Exception
                Throw New Exception("Failed to enumerate device tree!", ex)
                Return False
            End Try
            Return True
        End Function
#End Region
#Region "Private Methods"

        'Name:     ChangeIt
        'Inputs:   pointer to hdev, SP_DEV_INFO, bool
        'Outputs:  bool
        'Errors:   This method may throw the following exceptions.
        '          Unable to change device state!
        'Remarks:  Attempts to enable or disable a device driver.  
        '          IMPORTANT NOTE!!!   This code currently does not check the reboot flag.
        '          =================   Some devices require you reboot the OS for the change
        '                              to take affect.  If this describes your device, you 
        '                              will need to look at the SDK call:
        '                              SetupDiGetDeviceInstallParams.  You can call it 
        '                              directly after ChangeIt to see whether or not you need 
        '                              to reboot the OS for you change to go into effect.
        Private Function ChangeIt(ByVal hDevInfo As IntPtr, ByVal devInfoData As Native.SP_DEVINFO_DATA, ByVal bEnable As Boolean, ByVal DeviceName As String) As Boolean
            Try
                'Marshalling vars
                Dim szOfPcp As Integer
                Dim ptrToPcp As IntPtr
                Dim szDevInfoData As Integer
                Dim ptrToDevInfoData As IntPtr

                Dim pcp As New Native.SP_PROPCHANGE_PARAMS()
                If bEnable Then
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_ENABLE
                    pcp.Scope = Native.DICS_FLAG_GLOBAL
                    pcp.HwProfile = 0

                    'Marshal the params
                    szOfPcp = Marshal.SizeOf(pcp)
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp)
                    Marshal.StructureToPtr(pcp, ptrToPcp, True)
                    szDevInfoData = Marshal.SizeOf(devInfoData)
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData)

                    If Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(GetType(Native.SP_PROPCHANGE_PARAMS))) Then
                        Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData)
                    End If
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_ENABLE
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC
                    pcp.HwProfile = 0
                Else
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(GetType(Native.SP_CLASSINSTALL_HEADER))
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE
                    pcp.StateChange = Native.DICS_DISABLE
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC
                    pcp.HwProfile = 0
                End If
                'Marshal the params
                szOfPcp = Marshal.SizeOf(pcp)
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp)
                Marshal.StructureToPtr(pcp, ptrToPcp, True)
                szDevInfoData = Marshal.SizeOf(devInfoData)
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData)
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, True)
                Dim rslt1 As Boolean = Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(GetType(Native.SP_PROPCHANGE_PARAMS)))
                If Not rslt1 Then
                    Debug.WriteLine("Error:- " & LastError())
                End If
                Dim rstl2 As Boolean = Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData)
                If (Not rslt1) OrElse (Not rstl2) Then
                    Dim ilasterror As Integer = System.Runtime.InteropServices.Marshal.GetExceptionCode()
                    Dim errorMessage As String = New Win32Exception().Message
                    If errorMessage.Contains("0xe0000235") Then
                        Dim nProc As New ProcessStartInfo
                        With nProc
                            If bEnable Then
                                .Arguments = "ENABLE-" & DeviceName
                            Else
                                .Arguments = "DISABLE-" & DeviceName
                            End If
                            .WorkingDirectory = My.Application.Info.DirectoryPath
                            .FileName = .WorkingDirectory & "\x64DevHandler.exe"
                            .UseShellExecute = True
                        End With
                        REM Process.Start(nProc)
                    Else
                        Throw New Exception("Unable to change device state!")
                        Return False
                    End If
                Else
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
        Private Function LastError() As String
            Dim ilasterror As Integer = System.Runtime.InteropServices.Marshal.GetExceptionCode()
            Dim errorMessage As String = New Win32Exception().Message
            Return errorMessage
        End Function

#End Region
    End Class
End Namespace

