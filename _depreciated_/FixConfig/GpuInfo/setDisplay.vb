'THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS CODE PROJECT OPEN LICENSE ("LICENSE").
'THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
'ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.

'BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HEREIN, 
'YOU ACCEPT AND AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. 
'THE AUTHOR GRANTS YOU THE RIGHTS CONTAINED HEREIN IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS. 
'IF YOU DO NOT AGREE TO ACCEPT AND BE BOUND BY THE TERMS OF THIS LICENSE, YOU CANNOT MAKE ANY USE OF THE WORK.

'API Declarations have been taken from various web sources including allapi.net and ApiViewer 2004
Imports System.Runtime.InteropServices
Public Class WinAPI
    Public Const CCHDEVICENAME As Int32 = 32
    Public Const CCHFORMNAME As Int32 = 32

    Public Enum DEVMODE_SETTINGS
        ENUM_CURRENT_SETTINGS = (-1)
        ENUM_REGISTRY_SETTINGS = (-2)
    End Enum

    Public Enum Display_Device_Stateflags
        DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = &H1
        DISPLAY_DEVICE_MIRRORING_DRIVER = &H8
        DISPLAY_DEVICE_MODESPRUNED = &H8000000
        DISPLAY_DEVICE_MULTI_DRIVER = &H2
        DISPLAY_DEVICE_PRIMARY_DEVICE = &H4
        DISPLAY_DEVICE_VGA_COMPATIBLE = &H10
    End Enum

    Public Enum DeviceFlags
        CDS_FULLSCREEN = &H4
        CDS_GLOBAL = &H8
        CDS_NORESET = &H10000000
           CDS_RESET = &H40000000
        CDS_SET_PRIMARY = &H10
        CDS_TEST = &H2
        CDS_UPDATEREGISTRY = &H1
        CDS_VIDEOPARAMETERS = &H20
    End Enum

    Public Enum DEVMODE_Flags
        DM_BITSPERPEL = &H40000I
        DM_DISPLAYFLAGS = &H200000I
        DM_DISPLAYFREQUENCY = &H400000I
        DM_PELSHEIGHT = &H100000I
        DM_PELSWIDTH = &H80000I
        DM_POSITION = &H20I
    End Enum

    Public Enum DisplaySetting_Results
        DISP_CHANGE_BADFLAGS = -4
        DISP_CHANGE_BADMODE = -2
        DISP_CHANGE_BADPARAM = -5
        DISP_CHANGE_FAILED = -1
        DISP_CHANGE_NOTUPDATED = -3
        DISP_CHANGE_RESTART = 1
        DISP_CHANGE_SUCCESSFUL = 0
    End Enum

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure POINTL
        <MarshalAs(UnmanagedType.I4)> _
        Public x As Integer
        <MarshalAs(UnmanagedType.I4)> _
        Public y As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
    Public Structure DEVMODE
        ' You can define the following constant 
        ' but OUTSIDE the structure because you know 
        ' that size and layout of the structure 
        ' is very important 
        ' CCHDEVICENAME = 32 = 0x50 
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public dmDeviceName As String
        ' In addition you can define the last character array 
        ' as following: 
        '[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] 
        'public Char[] dmDeviceName; 

        ' After the 32-bytes array 
        <MarshalAs(UnmanagedType.U2)> _
        Public dmSpecVersion As UInt16

        <MarshalAs(UnmanagedType.U2)> _
        Public dmDriverVersion As UInt16

        <MarshalAs(UnmanagedType.U2)> _
        Public dmSize As UInt16

        <MarshalAs(UnmanagedType.U2)> _
        Public dmDriverExtra As UInt16

        <MarshalAs(UnmanagedType.U4)> _
        Public dmFields As DEVMODE_Flags
        'Public dmFields As UInt32

        Public dmPosition As POINTL

        <MarshalAs(UnmanagedType.U4)> _
        Public dmDisplayOrientation As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmDisplayFixedOutput As UInt32

        <MarshalAs(UnmanagedType.I2)> _
        Public dmColor As Int16

        <MarshalAs(UnmanagedType.I2)> _
        Public dmDuplex As Int16

        <MarshalAs(UnmanagedType.I2)> _
        Public dmYResolution As Int16

        <MarshalAs(UnmanagedType.I2)> _
        Public dmTTOption As Int16

        <MarshalAs(UnmanagedType.I2)> _
        Public dmCollate As Int16

        ' CCHDEVICENAME = 32 = 0x50 
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public dmFormName As String
        ' Also can be defined as 
        '[MarshalAs(UnmanagedType.ByValArray, 
        ' SizeConst = 32, ArraySubType = UnmanagedType.U1)] 
        'public Byte[] dmFormName; 
        <MarshalAs(UnmanagedType.U2)> _
        Public dmLogPixels As UInt16

        <MarshalAs(UnmanagedType.U4)> _
        Public dmBitsPerPel As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmPelsWidth As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmPelsHeight As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmDisplayFlags As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmDisplayFrequency As UInt32

        <MarshalAs(UnmanagedType.U4)> _
          Public dmICMMethod As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmICMIntent As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmMediaType As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmDitherType As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmReserved1 As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmReserved2 As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmPanningWidth As UInt32

        <MarshalAs(UnmanagedType.U4)> _
        Public dmPanningHeight As UInt32

        Public Overrides Function ToString() As String
            Return String.Format("{0} x {1}, {2} bits {3}htz", New Object() {Me.dmPelsWidth, Me.dmPelsHeight, Me.dmBitsPerPel, Me.dmDisplayFrequency})
        End Function

    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure DISPLAY_DEVICE
        Dim cb As Integer
        <VBFixedString(32), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)> Public DeviceName As String
        <VBFixedString(128), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)> Public DeviceString As String
        Dim StateFlags As Display_Device_Stateflags
        <VBFixedString(128), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)> Public DeviceID As String
        <VBFixedString(128), MarshalAs(UnmanagedType.ByValTStr, SizeConst:=128)> Public DeviceKey As String
    End Structure

    Public Declare Function ChangeDisplaySettingsEx Lib "user32.dll" Alias "ChangeDisplaySettingsExA" ( _
    ByVal lpszDeviceName As String, _
    ByRef lpDevMode As DEVMODE, _
    ByVal hwnd As Int32, _
    ByVal dwflags As DeviceFlags, _
    ByVal lParam As Int32) As Int32
    Public Declare Function EnumDisplayDevices Lib "user32.dll" Alias "EnumDisplayDevicesA" ( _
    ByVal lpDevice As String, _
    ByVal iDevNum As Int32, _
    ByRef lpDisplayDevice As DISPLAY_DEVICE, _
    ByVal dwFlags As Int32) As DisplaySetting_Results
    <DllImport("user32.dll")> _
    Public Shared Function EnumDisplaySettings(ByVal deviceName As String, ByVal modeNum As Integer, ByRef devMode As DEVMODE) As Integer
    End Function

    Public Declare Sub OutputDebugString Lib "kernel32.dll" Alias "OutputDebugStringA" ( _
    ByRef lpOutputString As String)

    Public Const WM_DISPLAYCHANGE As Int32 = &H7E

    Public Shared Sub OutputDebugStringEx(ByVal lpOutputString As String)
        OutputDebugString(lpOutputString & vbCrLf)
    End Sub

    Public Declare Unicode Function PickIconDlg _
    Lib "shell32.dll" Alias "#62" ( _
    ByVal hwndOwner As IntPtr, _
    ByVal lpstrFile As String, _
    ByVal nMaxFile As Integer, _
    ByRef lpdwIconIndex As Integer) _
    As Integer
    Public Declare Function ExtractIcon _
    Lib "shell32.dll" Alias "ExtractIconA" ( _
    ByVal hInst As IntPtr, _
    ByVal lpszExeFileName As String, _
    ByVal nIconIndex As Integer) _
    As Integer

    Public Declare Function DestroyIcon _
    Lib "user32.dll" (ByVal hIcon As Integer) _
    As Integer
End Class
Public Class DevModeSorter
    Implements System.Collections.Generic.IComparer(Of WinAPI.DEVMODE)

    Public Function Compare(ByVal x As WinAPI.DEVMODE, ByVal y As WinAPI.DEVMODE) As Integer Implements System.Collections.Generic.IComparer(Of WinAPI.DEVMODE).Compare
        If Not x.dmPelsWidth = y.dmPelsWidth Then
            Return x.dmPelsWidth > y.dmPelsWidth
        ElseIf Not x.dmPelsHeight = y.dmPelsHeight Then
            Return x.dmPelsHeight > y.dmPelsHeight
        ElseIf Not x.dmBitsPerPel = y.dmBitsPerPel Then
            Return x.dmBitsPerPel > y.dmBitsPerPel
        ElseIf Not x.dmDisplayFrequency = y.dmDisplayFrequency Then
            Return x.dmDisplayFrequency > y.dmDisplayFrequency
        Else
            Return 0
        End If
    End Function

End Class
