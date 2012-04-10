Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Xml.Serialization
Imports System.IO
Namespace displayManager
    'THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS CODE PROJECT OPEN LICENSE ("LICENSE").
    'THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. 
    'ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.

    'BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HEREIN, 
    'YOU ACCEPT AND AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. 
    'THE AUTHOR GRANTS YOU THE RIGHTS CONTAINED HEREIN IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS. 
    'IF YOU DO NOT AGREE TO ACCEPT AND BE BOUND BY THE TERMS OF THIS LICENSE, YOU CANNOT MAKE ANY USE OF THE WORK.

    'API Declarations have been taken from various web sources including allapi.net and ApiViewer 2004
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

    Public Structure displayManager
        Public m_Displays As Display()
        Private bDisplays As Display()
        Private Displays() As WinAPI.DISPLAY_DEVICE
        Private SelectedIndex As Int32
        Sub init()
            Dim dList As New List(Of Display)
            Dim d As New WinAPI.DISPLAY_DEVICE()
            d.cb = Marshal.SizeOf(d)
            Dim DevId As Integer = 0
            While WinAPI.EnumDisplayDevices(Nothing, DevId, d, 0)
                If Not d.StateFlags = WinAPI.Display_Device_Stateflags.DISPLAY_DEVICE_MIRRORING_DRIVER Then
                    Dim di As New WinAPI.DISPLAY_DEVICE
                    di.cb = Marshal.SizeOf(di)
                    WinAPI.EnumDisplayDevices(d.DeviceName, 0, di, 0)
                    Dim Disp As New Display(d, di)
                    dList.Add(Disp)
                End If
                DevId += 1
            End While
            m_DisplaySet = dList.ToArray
        End Sub
        Public Sub BackupDisplays()
            Try
                bDisplays = m_Displays
            Catch ex As Exception

            End Try
        End Sub
        Public Function RestoreBackUp() As WinAPI.DisplaySetting_Results
            Try
                If IsNothing(bDisplays) Then Return WinAPI.DisplaySetting_Results.DISP_CHANGE_NOTUPDATED
                Dim dR As WinAPI.DisplaySetting_Results = UpdateSettings(bDisplays)
                Return dR
            Catch ex As Exception
                Return ex.Message
            End Try
        End Function
        Private WriteOnly Property m_DisplaySet() As Display()
            Set(ByVal value As Display())
                m_Displays = value
                Me.SelectedIndex = 0
            End Set
        End Property

        Public ReadOnly Property PrimaryDisplayRegistryID() As Integer
            Get
                Try
                    Dim d As New WinAPI.DISPLAY_DEVICE
                    d.cb = Marshal.SizeOf(d)
                    Dim ID As Integer = 0
                    While WinAPI.EnumDisplayDevices(Nothing, ID, d, 0)
                        If d.StateFlags = (d.StateFlags Or WinAPI.Display_Device_Stateflags.DISPLAY_DEVICE_PRIMARY_DEVICE) Then
                            Return ID
                        End If
                        ID += 1
                        d = New WinAPI.DISPLAY_DEVICE
                        d.cb = Marshal.SizeOf(d)
                    End While
                Catch ex As Exception

                End Try
            End Get
        End Property
        Public Overloads Function UpdateSettings(ByVal Display As Display) As WinAPI.DisplaySetting_Results
            Try
                If Not Me.PrimaryDisplayID = Me.PrimaryDisplayRegistryID Then
                    SetPrimaryDisplay(Me.m_Displays(Me.PrimaryDisplayRegistryID).DeviceName, Me.m_Displays(Me.PrimaryDisplayID).DeviceName)
                End If
                Dim Result As WinAPI.DisplaySetting_Results
                If Not Display.Attached Then
                    Dim dm As New WinAPI.DEVMODE
                    dm.dmDeviceName = New [String](New Char(31) {})
                    dm.dmFormName = New [String](New Char(31) {})
                    dm.dmSize = CShort(Marshal.SizeOf(dm))
                    dm.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION Or WinAPI.DEVMODE_Flags.DM_PELSWIDTH Or WinAPI.DEVMODE_Flags.DM_PELSHEIGHT
                    dm.dmPelsWidth = 0
                    dm.dmPelsHeight = 0
                    dm.dmPosition.x = 0
                    dm.dmPosition.y = 0
                    Result = WinAPI.ChangeDisplaySettingsEx(Display.DeviceName, dm, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                Else
                    Dim dm As New WinAPI.DEVMODE
                    dm.dmDeviceName = New [String](New Char(31) {})
                    dm.dmFormName = New [String](New Char(31) {})
                    dm.dmSize = CShort(Marshal.SizeOf(dm))
                    dm.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION Or WinAPI.DEVMODE_Flags.DM_PELSWIDTH Or WinAPI.DEVMODE_Flags.DM_PELSHEIGHT Or WinAPI.DEVMODE_Flags.DM_DISPLAYFLAGS Or WinAPI.DEVMODE_Flags.DM_BITSPERPEL Or WinAPI.DEVMODE_Flags.DM_DISPLAYFREQUENCY
                    dm.dmPelsWidth = Display.Size.Width
                    dm.dmPelsHeight = Display.Size.Height
                    dm.dmPosition.x = Display.Location.X
                    dm.dmPosition.y = Display.Location.Y
                    dm.dmBitsPerPel = Display.BitsPerPixel
                    dm.dmDisplayFrequency = Display.Frequency
                    Result = WinAPI.ChangeDisplaySettingsEx(Display.DeviceName, dm, Nothing, WinAPI.DeviceFlags.CDS_SET_PRIMARY Or WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                End If
                If Result <> WinAPI.DisplaySetting_Results.DISP_CHANGE_SUCCESSFUL Then
                    MsgBox(Result.ToString)
                End If
                For i As Integer = Displays.Length - 1 To 0 Step -1
                    Dim dm1 As New WinAPI.DEVMODE
                    dm1.dmDeviceName = New [String](New Char(31) {})
                    dm1.dmFormName = New [String](New Char(31) {})
                    dm1.dmSize = CShort(Marshal.SizeOf(dm1))
                    Dim lStatus As WinAPI.DisplaySetting_Results = WinAPI.ChangeDisplaySettingsEx(Me.m_Displays(i).DeviceName, dm1, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY, Nothing)
                    Debug.Print(lStatus.ToString)
                    WinAPI.OutputDebugString(lStatus.ToString & vbCrLf)
                Next
                Me.init()
            Catch ex As Exception

            End Try
        End Function
        Public Overloads Function UpdateSettings(ByVal Displays As Display()) As WinAPI.DisplaySetting_Results
            Try
                If Not Me.PrimaryDisplayID = Me.PrimaryDisplayRegistryID Then
                    SetPrimaryDisplay(Me.m_Displays(Me.PrimaryDisplayRegistryID).DeviceName, Me.m_Displays(Me.PrimaryDisplayID).DeviceName)
                End If
                Dim Result As WinAPI.DisplaySetting_Results
                For Each disp As Display In Displays
                    If Not disp.Attached Then
                        Dim dm As New WinAPI.DEVMODE
                        dm.dmDeviceName = New [String](New Char(31) {})
                        dm.dmFormName = New [String](New Char(31) {})
                        dm.dmSize = CShort(Marshal.SizeOf(dm))
                        dm.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION Or WinAPI.DEVMODE_Flags.DM_PELSWIDTH Or WinAPI.DEVMODE_Flags.DM_PELSHEIGHT
                        dm.dmPelsWidth = 0
                        dm.dmPelsHeight = 0
                        dm.dmPosition.x = 0
                        dm.dmPosition.y = 0
                        Result = WinAPI.ChangeDisplaySettingsEx(disp.DeviceName, dm, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                        If Result <> WinAPI.DisplaySetting_Results.DISP_CHANGE_SUCCESSFUL Then
                            MsgBox(Result.ToString)
                        End If
                    Else
                        Dim dm As New WinAPI.DEVMODE
                        dm.dmDeviceName = New [String](New Char(31) {})
                        dm.dmFormName = New [String](New Char(31) {})
                        dm.dmSize = CShort(Marshal.SizeOf(dm))
                        dm.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION Or WinAPI.DEVMODE_Flags.DM_PELSWIDTH Or WinAPI.DEVMODE_Flags.DM_PELSHEIGHT Or WinAPI.DEVMODE_Flags.DM_DISPLAYFLAGS Or WinAPI.DEVMODE_Flags.DM_BITSPERPEL Or WinAPI.DEVMODE_Flags.DM_DISPLAYFREQUENCY
                        dm.dmPelsWidth = disp.Size.Width
                        dm.dmPelsHeight = disp.Size.Height
                        dm.dmPosition.x = disp.Location.X
                        dm.dmPosition.y = disp.Location.Y
                        dm.dmBitsPerPel = disp.BitsPerPixel
                        dm.dmDisplayFrequency = disp.Frequency
                        Result = WinAPI.ChangeDisplaySettingsEx(disp.DeviceName, dm, Nothing, WinAPI.DeviceFlags.CDS_SET_PRIMARY Or WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                        If Result <> WinAPI.DisplaySetting_Results.DISP_CHANGE_SUCCESSFUL Then
                            MsgBox(Result.ToString)
                        End If
                    End If
                Next
             
                For i As Integer = Displays.Length - 1 To 0 Step -1
                    Dim dm1 As New WinAPI.DEVMODE
                    dm1.dmDeviceName = New [String](New Char(31) {})
                    dm1.dmFormName = New [String](New Char(31) {})
                    dm1.dmSize = CShort(Marshal.SizeOf(dm1))
                    Dim lStatus As WinAPI.DisplaySetting_Results = WinAPI.ChangeDisplaySettingsEx(Me.m_Displays(i).DeviceName, dm1, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY, Nothing)
                    Debug.Print(lStatus.ToString)
                    WinAPI.OutputDebugString(lStatus.ToString & vbCrLf)
                Next
                Me.init()
            Catch ex As Exception

            End Try
        End Function
        Public Sub SetPrimaryDisplay(ByVal OldPrimary As String, ByVal NewPrimary As String)
            Try
                Dim Result As WinAPI.DisplaySetting_Results = 0

                Dim dm1 As WinAPI.DEVMODE = NewDevMode()
                WinAPI.EnumDisplaySettings(NewPrimary, WinAPI.DEVMODE_SETTINGS.ENUM_REGISTRY_SETTINGS, dm1)
                Dim dm3 As WinAPI.DEVMODE = NewDevMode()
                dm3.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION
                dm3.dmPosition.x = dm1.dmPelsWidth
                dm3.dmPosition.y = 0
                Result = WinAPI.ChangeDisplaySettingsEx(OldPrimary, dm3, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                Console.WriteLine(Result.ToString)

                Dim dm2 As WinAPI.DEVMODE = NewDevMode()
                WinAPI.EnumDisplaySettings(NewPrimary, WinAPI.DEVMODE_SETTINGS.ENUM_REGISTRY_SETTINGS, dm2)
                Dim dm4 As WinAPI.DEVMODE = NewDevMode()
                dm4.dmFields = WinAPI.DEVMODE_Flags.DM_POSITION
                dm4.dmPosition.x = 0
                dm4.dmPosition.y = 0
                Result = WinAPI.ChangeDisplaySettingsEx(NewPrimary, dm4, Nothing, WinAPI.DeviceFlags.CDS_SET_PRIMARY Or WinAPI.DeviceFlags.CDS_UPDATEREGISTRY Or WinAPI.DeviceFlags.CDS_NORESET, 0)
                Console.WriteLine(Result.ToString)

                Dim dm6 As WinAPI.DEVMODE = NewDevMode()
                Result = WinAPI.ChangeDisplaySettingsEx(NewPrimary, dm6, Nothing, WinAPI.DeviceFlags.CDS_SET_PRIMARY Or WinAPI.DeviceFlags.CDS_UPDATEREGISTRY, 0)

                Dim dm5 As WinAPI.DEVMODE = NewDevMode()
                Result = WinAPI.ChangeDisplaySettingsEx(OldPrimary, dm5, Nothing, WinAPI.DeviceFlags.CDS_UPDATEREGISTRY, Nothing)
                Console.WriteLine(Result.ToString)

                Console.WriteLine(Result.ToString)


            Catch ex As Exception

            End Try
        End Sub
        Public Function NewDevMode() As WinAPI.DEVMODE
            Dim dm As New WinAPI.DEVMODE
            dm.dmDeviceName = New [String](New Char(31) {})
            dm.dmFormName = New [String](New Char(31) {})
            dm.dmSize = CShort(Marshal.SizeOf(dm))
            Return dm
        End Function

        Public ReadOnly Property PrimaryDisplayID() As Integer
            Get
                For i As Integer = 0 To Me.m_Displays.Length - 1
                    If Me.m_Displays(i).Primary Then
                        Return i
                    End If
                Next
            End Get
        End Property
    End Structure

    <Serializable()> _
    Public MustInherit Class ContractBase(Of T)
        Implements ICloneable


        ''' Must have default constructor for xml serialization 
        Public Sub New()
        End Sub

        ''' Create an xml representation of this instance 
        Public Function Serialize() As String

            Dim serializer As New XmlSerializer(Me.[GetType]())
            Using stream As New StringWriter()
                serializer.Serialize(stream, Me)
                stream.Flush()
                Return stream.ToString()
            End Using
        End Function

        ''' Creata a new instance from an xml string. 
        ''' The client is responsible for deserialization of the correct type 
        Public Shared Function Deserialize(ByVal xml As String) As T
            If String.IsNullOrEmpty(xml) Then
                Throw New ArgumentNullException("xml")
            End If

            Dim serializer As New XmlSerializer(GetType(T))
            Using stream As New StringReader(xml)
                Try
                    Return DirectCast(serializer.Deserialize(stream), T)
                Catch ex As Exception
                    ' The serialization error messages are cryptic at best. 
                    ' Give a hint at what happened 
                    Throw New InvalidOperationException("Failed to create object from xml string", ex)
                End Try
            End Using
        End Function

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class
    <Serializable()> _
    Public Class Display
        Inherits ContractBase(Of Display)

#Region "Internal properties"
        <XmlElement("Name")> _
        Public m_DeviceName As String
        <XmlElement("AdapterName")> _
        Public m_AdapterName As String
        <XmlElement("MonitorName")> _
        Public m_MonitorName As String
        <XmlElement("Flags")> _
        Public m_StateFlags As Integer
        <XmlElement("Size")> _
        Public m_Size As Size
        <XmlElement("BitsPerPixel")> _
        Public m_BitPerPixel As Integer
        <XmlElement("Frequency")> _
        Public m_Frequency As Integer
        <XmlElement("Location")> _
        Public m_Location As Point
        <XmlElement("Attached")> _
        Public m_Attached As Boolean
        <XmlElement("Primary")> _
        Public m_Primary As Boolean
        <XmlIgnore()> _
        Private m_Modes As Mode()
        <XmlIgnore()> _
        Private WriteOnly Property ModesSet() As Mode()
            Set(ByVal value As Mode())
                m_Modes = value
            End Set
        End Property

#End Region

#Region "Public Events"
        Public Event SizeChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event BitPerPixelChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event FrequencyChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event LocationChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event AttachedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event PrimaryChanged(ByVal sender As Object, ByVal e As EventArgs)
        Public Event StateFlagsChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region "Public Properties"
        <XmlIgnore()> _
        Public ReadOnly Property DeviceName() As String
            Get
                Return m_DeviceName
            End Get
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AdapterName() As String
            Get
                Return m_AdapterName
            End Get
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property MonitorName() As String
            Get
                Return m_MonitorName
            End Get
        End Property

        <XmlIgnore()> _
        Public Property StateFlags() As WinAPI.Display_Device_Stateflags
            Get
                Return m_StateFlags
            End Get
            Set(ByVal value As WinAPI.Display_Device_Stateflags)
                Try
                    m_StateFlags = value
                    Me.m_Attached = (value = (value Or WinAPI.Display_Device_Stateflags.DISPLAY_DEVICE_ATTACHED_TO_DESKTOP))
                    Me.m_Primary = (value = (value Or WinAPI.Display_Device_Stateflags.DISPLAY_DEVICE_PRIMARY_DEVICE))
                    RaiseEvent StateFlagsChanged(Me, New EventArgs)
                Catch ex As Exception

                End Try
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Size() As Size
            Get
                Return m_Size
            End Get
            Set(ByVal value As Size)
                m_Size = value
                RaiseEvent SizeChanged(Me, New EventArgs)
            End Set
        End Property

        <XmlIgnore()> _
        Public Property BitsPerPixel() As Integer
            Get
                Return m_BitPerPixel
            End Get
            Set(ByVal value As Integer)
                Try
                    m_BitPerPixel = value
                    RaiseEvent BitPerPixelChanged(Me, New EventArgs)
                Catch ex As Exception

                End Try
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Frequency() As Integer
            Get
                Return m_Frequency
            End Get
            Set(ByVal value As Integer)
                Try
                    m_Frequency = value
                    RaiseEvent FrequencyChanged(Me, New EventArgs)
                Catch ex As Exception

                End Try
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Location() As Point
            Get
                Return m_Location
            End Get
            Set(ByVal value As Point)
                Try
                    m_Location = value
                    RaiseEvent LocationChanged(Me, New EventArgs)
                Catch ex As Exception

                End Try
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Attached() As Boolean
            Get
                Return m_Attached
            End Get
            Set(ByVal value As Boolean)
                Try
                    If Me.Primary And value = False Then
                        Throw New Exception("Unable to Detach Primary monitor")
                    End If
                    m_Attached = value
                    RaiseEvent AttachedChanged(Me, New EventArgs)
                Catch ex As Exception

                End Try
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property Primary() As Boolean
            Get
                Return m_Primary
            End Get
        End Property

        Public Sub SetPrimary()
            Try
                Me.m_Primary = True
                Me.m_Attached = True
                RaiseEvent PrimaryChanged(Me, New EventArgs)
            Catch ex As Exception

            End Try
        End Sub

        <XmlIgnore()> _
        Public ReadOnly Property Modes() As Mode()
            Get
                Return m_Modes
            End Get
        End Property
#End Region

#Region "New(s)"
        Public Sub New()

        End Sub

        Public Sub New(ByVal Device As WinAPI.DISPLAY_DEVICE, ByVal Monitor As WinAPI.DISPLAY_DEVICE)
            Try
                Me.m_DeviceName = Device.DeviceName
                Me.m_AdapterName = Device.DeviceString
                Me.m_MonitorName = IIf(Monitor.DeviceString = "", "(Default Monitor)", Monitor.DeviceString)
                Me.StateFlags = Device.StateFlags
                GetDisplaySettings()
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "Initialize"
        Private Sub GetDisplaySettings()
            Try
                'Set current settings
                Dim dm1 As New WinAPI.DEVMODE()
                dm1.dmDeviceName = New [String](New Char(31) {})
                dm1.dmFormName = New [String](New Char(31) {})
                dm1.dmSize = CShort(Marshal.SizeOf(dm1))
                WinAPI.EnumDisplaySettings(Me.m_DeviceName, -2, dm1)
                Me.Size = New Size(dm1.dmPelsWidth, dm1.dmPelsHeight)
                Me.BitsPerPixel = dm1.dmBitsPerPel
                Me.Frequency = dm1.dmDisplayFrequency
                Me.Location = New Point(dm1.dmPosition.x, dm1.dmPosition.y)

                'Set display modes
                Dim mList As New List(Of Mode)
                Dim dm As New WinAPI.DEVMODE()
                dm.dmDeviceName = New [String](New Char(31) {})
                dm.dmFormName = New [String](New Char(31) {})
                dm.dmSize = CShort(Marshal.SizeOf(dm))
                Dim ModeID As Integer = 0
                While WinAPI.EnumDisplaySettings(Me.DeviceName, ModeID, dm)
                    mList.Add(New Mode(dm))
                    dm.dmDeviceName = New [String](New Char(31) {})
                    dm.dmFormName = New [String](New Char(31) {})
                    dm.dmSize = CShort(Marshal.SizeOf(dm))
                    ModeID += 1
                End While
                Me.ModesSet = mList.ToArray
            Catch ex As Exception

            End Try
        End Sub
#End Region

    End Class
    <Serializable()> _
    Public Class Mode
        Private m_Size As Size
        Private m_bpp As Integer
        Private m_Frequency As Integer

        Public ReadOnly Property Size() As Size
            Get
                Return m_Size
            End Get
        End Property

        Public ReadOnly Property BitsPerPixel() As Integer
            Get
                Return m_bpp
            End Get
        End Property

        Public ReadOnly Property Frequency() As Integer
            Get
                Return m_Frequency
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal Settings As WinAPI.DEVMODE)
            Try
                Me.m_Size = New Size(Settings.dmPelsWidth, Settings.dmPelsHeight)
                Me.m_bpp = Settings.dmBitsPerPel
                Me.m_Frequency = Settings.dmDisplayFrequency
            Catch ex As Exception

            End Try
        End Sub
    End Class
End Namespace