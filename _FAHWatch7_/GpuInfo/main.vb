'/*
' * MAIN GpuInfo module Copyright Marvin Westmaas ( mtm )
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
'/*
Imports System.Runtime.InteropServices
Imports gpuInfo.myOCL.oclAPI
Imports gpuInfo.myCuda
Imports gpuInfo.myCuda.cudaAPI
Imports gpuInfo.myCAL
Imports System.Text
Imports System.Security.Permissions
Imports System.IO

Public Module Main
#Region "openCL"
    Public Class oclInfo
        Public Structure sopenclPlatform
            Public Name As String
            Public Vendor As String
            Public Version As String
            Public Extensions As String
            Public Profile As String
            Public Structure sopenCLDevice
                Public Property Index As String
                Private alProperties As ArrayList
                Public Structure sProperty
                    Public Name As String
                    Public iEnum As UInt32
                    Public Value As Object
                    Public ConvertedValue As Object
                End Structure
                Public Sub SetValue(ByVal iEnum As UInt32, ByVal Value As Object)
                    For xInt As Int32 = 0 To alProperties.Count - 1
                        If CType(alProperties(xInt), sProperty).iEnum = iEnum Then
                            Dim np As sProperty = CType(alProperties(xInt), sProperty)
                            np.Value = Value
                            Select Case np.iEnum
                                Case Is = CUInt(CLDeviceInfo.AddressBits)
                                    np.ConvertedValue = BitConverter.ToUInt32(CType(Value, Byte()), 0)
                                Case Is = CUInt(CLDeviceInfo.Available)
                                    np.ConvertedValue = Main.ByteToBool(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.CompilerAvailable)
                                    np.ConvertedValue = Main.ByteToBool(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.DriverVersion)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.EndianLittle)
                                    np.ConvertedValue = Main.ByteToBool(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.ErrorCorrectionSupport)
                                    np.ConvertedValue = Main.ByteToBool(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.ExecutionCapabilities)
                                    np.ConvertedValue = Main.ByteToULong(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.Extensions)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.LocalMemType)
                                    np.ConvertedValue = Main.ByteToULong(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.ImageSupport)
                                    np.ConvertedValue = Main.ByteToBool(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.Type)
                                    Dim dConv As ULong = Main.ByteToULong(CType(Value, Byte()))
                                    np.ConvertedValue = DeviceTypeString(CType(dConv, DeviceType))
                                Case Is = CUInt(CLDeviceInfo.Name)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.Vendor)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.VendorID)
                                    np.ConvertedValue = Main.ByteToUint(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.Version)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.SingleFPConfig)
                                    np.ConvertedValue = Main.ByteToULong(CType(Value, Byte()))
                                Case Is = CUInt(CLDeviceInfo.Profile)
                                    np.ConvertedValue = Main.ByteToString(CType(Value, Byte()))
                                Case Else
                                    np.ConvertedValue = Main.ByteToUint(CType(Value, Byte()))
                            End Select
                            alProperties(xInt) = np
                            Return
                        End If
                    Next
                End Sub
                Public Sub AddNames(ByVal Names() As String, ByVal InfoValue() As UInt32)
                    For xInt As Int32 = 0 To Names.Count - 1
                        Dim np As New sProperty
                        np.Name = Names(xInt)
                        np.iEnum = InfoValue(xInt)
                        alProperties.Add(np)
                    Next
                End Sub
                Public ReadOnly Property DeviceAttribute(ByVal Name As String) As String
                    Get
                        For Each sProp As sProperty In alProperties
                            If sProp.Name.ToUpper = Name.ToUpper Then
                                Return CStr(CType(sProp, sProperty).ConvertedValue)
                            End If
                        Next
                        Return Nothing
                    End Get
                End Property
                Public ReadOnly Property AttributeCount As Int32
                    Get
                        Return alProperties.Count
                    End Get
                End Property
                Public ReadOnly Property DeviceAttributes() As Array
                    Get
                        Dim r(0 To alProperties.Count - 1) As sProperty
                        For xint As Int32 = 0 To alProperties.Count - 1
                            r(xint) = CType(alProperties(xint), sProperty)
                        Next
                        Return r
                    End Get
                End Property
                Private Function ConvertValue(ByVal EnumTarget As Type, ByVal Value As Main.SizeT) As String
                    Try
                        Dim infoNames() As String = [Enum].GetNames(GetType(myOCL.oclAPI.CLDeviceInfo))
                        Dim infoValues() As Main.SizeT = CType([Enum].GetValues(GetType(oclInfo.sopenclPlatform.sopenCLDevice.sProperty)), SizeT())
                        For xInt As Int32 = 0 To infoNames.Count - 1
                            If infoValues(xInt) = Value Then
                                Return infoNames(xInt)
                            End If
                        Next
                        Return ""
                    Catch ex As Exception
                        Return ""
                    End Try
                End Function
                Public Sub Init()
                    alProperties = New ArrayList
                End Sub
            End Structure
            Public colDevices As Collection
            Public Sub Init()
                colDevices = New Collection
            End Sub
            Public ReadOnly Property NumberOfDevices As Int32
                Get
                    Return colDevices.Count
                End Get
            End Property
            Public ReadOnly Property Device_ByIndex(ByVal Index As Int32) As sopenCLDevice
                Get
                    Return CType(colDevices(Index), sopenCLDevice)
                End Get
            End Property
            Public Property Device(ByVal DeviceID As String) As sopenCLDevice
                Get
                    If colDevices.Contains(DeviceID) Then Return CType(colDevices(DeviceID), sopenCLDevice)
                    Return Nothing
                End Get
                Set(ByVal value As sopenCLDevice)
                    If colDevices.Contains(DeviceID) Then
                        colDevices.Remove(DeviceID)
                        colDevices.Add(value, CType(value, sopenCLDevice).Index)
                    End If
                End Set
            End Property
        End Structure
        Public ReadOnly Property DeviceCount As Int32
            Get
                If alPlatforms.Count = 0 Then Return 0
                Dim iTotal As Int32 = 0
                For xInt As Int32 = 0 To NumberOfPlatforms - 1
                    Dim oP As oclInfo.sopenclPlatform = Platform(xInt)
                    iTotal += Platform(xInt).NumberOfDevices
                Next
                Return iTotal
            End Get
        End Property
        Private alPlatforms As New ArrayList
        Public ReadOnly Property NumberOfPlatforms As Int32
            Get
                Return alPlatforms.Count
            End Get
        End Property
        Public Property Platform(ByVal Index As Int32) As sopenclPlatform
            Get
                If alPlatforms.Count > Index - 1 Then Return CType(alPlatforms(Index), sopenclPlatform)
                Return Nothing
            End Get
            Set(ByVal value As sopenclPlatform)
                If alPlatforms.Contains(value) Then
                    alPlatforms(alPlatforms.IndexOf(value)) = value
                Else
                    alPlatforms.Add(value)
                End If
            End Set
        End Property
        Public Function Init(TheLumberJack As LumberJack.LumberJack) As Boolean
            Try
                modLumberJack.LumberJack = TheLumberJack
                Dim bOnce As Boolean = True
                alPlatforms.Clear()
                Dim pIDcount As IntPtr, pID() As myOCL.oclAPI.CLPlatformID
                Dim nEntries As Main.SizeT, nPlatforms As Main.SizeT
                If clGetPlatformIDs(nEntries, pIDcount, nPlatforms) = myOCL.oclAPI.CLError.Success Then
                    WriteLog("found " & nPlatforms.ToString & " platforms", Me)
                    ReDim pID(0 To CInt(nPlatforms.ToString) - 1)
                    If myOCL.oclAPI.clGetPlatformIDs(nPlatforms, pID, nPlatforms) = myOCL.oclAPI.CLError.Success Then
                        For Each oPL As myOCL.oclAPI.CLPlatformID In pID
                            Dim oclP As New Main.oclInfo.sopenclPlatform
                            oclP.Init()
                            Dim pInfo As myOCL.oclAPI.CLPlatformInfo = CLPlatformInfo.Name
                            Dim loopCount As Byte = 0
                            Do
                                Select Case loopCount
                                    Case Is = 0
                                        pInfo = CLPlatformInfo.Name
                                    Case Is = 1
                                        pInfo = CLPlatformInfo.Version
                                    Case Is = 2
                                        pInfo = CLPlatformInfo.Vender
                                    Case Is = 3
                                        pInfo = CLPlatformInfo.Profile
                                    Case Is = 4
                                        pInfo = CLPlatformInfo.Extensions
                                End Select

                                Dim Value() As Byte, value_size As Main.SizeT, value_size_ret As Main.SizeT, intPtrvalue As IntPtr
                                If myOCL.oclAPI.clGetPlatformInfo(oPL, pInfo, value_size, intPtrvalue, value_size_ret) = myOCL.oclAPI.CLError.Success Then
                                    ReDim Value(0 To value_size_ret)
                                    If myOCL.oclAPI.clGetPlatformInfo(oPL, pInfo, value_size_ret, Value, value_size_ret) = myOCL.oclAPI.CLError.Success Then
                                        If Value.Count > 0 Then
                                            Dim sName As String = ""
                                            For Each _byte In Value
                                                If _byte = 0 Then Exit For
                                                sName &= ChrW(_byte)
                                            Next
                                            Select Case loopCount
                                                Case Is = 0
                                                    WriteLog("-Name " & sName, Me)
                                                    oclP.Name = sName
                                                Case Is = 1
                                                    WriteLog("-Version " & sName, Me)
                                                    oclP.Version = sName
                                                Case Is = 2
                                                    WriteLog("-Vendor " & sName, Me)
                                                    oclP.Vendor = sName
                                                Case Is = 3
                                                    WriteLog("-Profile " & sName, Me)
                                                    oclP.Profile = sName
                                                Case Is = 4
                                                    WriteLog("-Extensions " & sName, Me)
                                                    oclP.Extensions = sName
                                            End Select
                                        End If
                                    End If
                                End If
                                loopCount = CByte(loopCount + 1)
                            Loop While loopCount < 5
                            Dim dID() As myOCL.oclAPI.CLDeviceID
                            Dim _did As IntPtr
                            Dim nDev As UInt32 ' long
                            If myOCL.oclAPI.clGetDeviceIDs(oPL, CLDeviceType.GPU, nEntries, _did, nDev) = myOCL.oclAPI.CLError.Success Then
                                ReDim dID(0 To CInt(nDev - 1))
                                WriteLog("-Platform has " & CStr(CInt(nDev)) & " devices", Me)
                                If myOCL.oclAPI.clGetDeviceIDs(oPL, CLDeviceType.GPU, nDev, dID, nDev) = myOCL.oclAPI.CLError.Success Then
                                    Dim iID As Int32 = 0
                                    For Each sDid As myOCL.oclAPI.CLDeviceID In dID
                                        Dim oclD As New Main.oclInfo.sopenclPlatform.sopenCLDevice
                                        oclD.Init()
                                        oclD.Index = CStr(iID)
                                        iID += 1
                                        Dim infoNames() As String = [Enum].GetNames(GetType(myOCL.oclAPI.CLDeviceInfo))
                                        Dim infoValues() As UInt32 = CType([Enum].GetValues(GetType(myOCL.oclAPI.CLDeviceInfo)), UInteger())
                                        oclD.AddNames(infoNames, infoValues)
                                        Dim clDI As myOCL.oclAPI.CLDeviceInfo
                                        For xInt As Int32 = 0 To infoNames.Count - 1
                                            'Log(infoNames(xInt) & " " & infoValues(xInt))
                                            clDI = CType(infoValues(xInt), CLDeviceInfo)
                                            Dim pvSize As Main.SizeT, vSize As Main.SizeT, vIntPtr As IntPtr, Value() As Byte
                                            Dim oclSucces As myOCL.oclAPI.CLError = CLError.Success
                                            oclSucces = myOCL.oclAPI.clGetDeviceInfo(sDid, clDI, pvSize, vIntPtr, vSize)
                                            If oclSucces = myOCL.oclAPI.CLError.Success Then
                                                ReDim Value(0 To vSize)
                                                If clGetDeviceInfo(sDid, clDI, vSize, Value, vSize) = myOCL.oclAPI.CLError.Success Then
                                                    oclD.SetValue(CUInt(clDI), Value)
                                                End If
                                            End If
                                        Next
                                        WriteLog("-Device " & CStr(iID - 1) & ": " & oclD.DeviceAttribute("Name"), Me)
                                        oclP.colDevices.Add(oclD)
                                    Next
                                End If
                            Else
                                MsgBox("oops")
                            End If
                            Platform(NumberOfPlatforms) = oclP
                        Next
                    End If
                End If
                Return NumberOfPlatforms > 0
            Catch ex As Exception
                If Err.Number = 53 Then
                    'File not found = not an error
                    WriteLog("openCL Driver not found", Me)
                    Return False
                End If
                WriteError("Interop_openCLInit", Err)
                Return False
            End Try
        End Function
    End Class
    Public oclInf As New Main.oclInfo
#End Region
#Region "CUDA"
    <Serializable()>
    Public Class cudaInfo
        Private _DriverVersion As String = ""
        Private _cuLastError As String = ""
        Public ReadOnly Property CudaError As String
            Get
                Return _cuLastError
            End Get
        End Property
        Public Sub SetLastError(ByVal sError As String)
            _cuLastError = sError
            WriteLog("EXCEPTION: CUDA_ERROR " & sError, Me)
        End Sub
        Public ReadOnly Property DriverVersion As String
            Get
                Return _DriverVersion
            End Get
        End Property
        Public Sub SetDriverVersion(ByVal Version As String)
            _DriverVersion = Version
        End Sub
        <Serializable()>
        Public Structure scudaDevice
            Public Property Device As myCuda.CUdevice
            Public Property Name As String
            Public Property ComputeCapabilities As String
            Public Property DeviceTotalMemory As SizeT
            Private alProperties As ArrayList
            <Serializable()>
            Public Structure sProperty
                Public Name As String
                Public iEnum As Int32
                Public Value As Object
            End Structure
            Public Sub SetValue(ByVal iEnum As Int32, ByVal Value As Object)
                For xInt As Int32 = 0 To alProperties.Count - 1
                    If CType(alProperties(xInt), sProperty).iEnum = iEnum Then
                        Try
                            Dim np As sProperty = CType(alProperties(xInt), sProperty)
                            np.Value = Value
                            alProperties(xInt) = np
                            ' Debug.WriteLine(np.Name & " : " & CStr(np.Value))
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        Return
                    End If
                Next
            End Sub
            Public Sub AddNames(ByVal Names() As String, ByVal InfoValue() As UInt32)
                Dim bSkip As Boolean = False
                For xInt As Int32 = 0 To Names.Count - 1
                    Try
                        bSkip = False
                        For Each sProperty In alProperties
                            If CType(sProperty, sProperty).Name = Names(xInt) Then
                                bSkip = True
                            End If
                        Next
                        If Not bSkip Then
                            Dim np As New sProperty
                            np.Name = Names(xInt)
                            np.iEnum = CInt(InfoValue(xInt))
                            alProperties.Add(np)
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                Next
            End Sub
            Public ReadOnly Property DeviceAttribute(ByVal Name As String) As String
                Get
                    For Each sProp As sProperty In alProperties
                        If sProp.Name.ToUpper = Name.ToUpper Then
                            Return sProp.Value.ToString
                        End If
                    Next
                    Return Nothing
                End Get
            End Property
            Public ReadOnly Property AttributeCount As Int32
                Get
                    Return alProperties.Count
                End Get
            End Property
            Public ReadOnly Property DeviceAttributes() As Array
                Get
                    Try
                        Dim r(0 To alProperties.Count - 1) As sProperty
                        Try
                            For xInt As Int32 = 0 To alProperties.Count - 1
                                If TypeOf alProperties(xInt) Is sProperty Then
                                    If Not CType(alProperties(xInt), sProperty).Name = "" Then
                                        r(xInt) = CType(alProperties(xInt), sProperty)
                                    Else
                                        WriteLog("Skipping empty cuda property", Me)
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        Dim rVal(r.Count - 1) As sProperty
                        r.CopyTo(rVal, 0)
                        Return rVal.ToArray
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return Nothing
                    End Try
                End Get
            End Property

            Public Sub Init()
                alProperties = New ArrayList
            End Sub

        End Structure
        Private alDevices As New ArrayList
        Public Property cudaDevice(ByVal Index As Int32) As scudaDevice
            Get
                If alDevices.Count <= Index Then
                    Return CType(alDevices(Index - 1), scudaDevice)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As scudaDevice)
                If alDevices.Count - 1 <= value.Device.Pointer Then
                    alDevices(value.Device.Pointer) = value
                Else
                    alDevices.Add(value)
                End If
            End Set
        End Property
        Public Sub AddDevice(ByVal Device As scudaDevice)
            If alDevices.Contains(Device) Then
                alDevices(alDevices.IndexOf(Device)) = Device
            Else
                alDevices.Add(Device)
            End If
        End Sub
        Public Property Devices() As Array
            Get
                Return alDevices.ToArray
            End Get
            Set(ByVal value As Array)
                alDevices.Clear()
                alDevices.AddRange(value)
            End Set
        End Property
        Public ReadOnly Property DeviceCount As Int32
            Get
                Return alDevices.Count
            End Get
        End Property
        Public Function Init(TheLumberJack As LumberJack.LumberJack) As Boolean
            Try
                modLumberJack.LumberJack = TheLumberJack
                alDevices.Clear()
                If myCuda.cudaAPI.cuInit(0) <> myCuda.cudaError.cudaSuccess Then
                    setCudaError()
                    Return False
                Else
                    WriteLog("CUDA platform detected", Me)
                End If
                Dim dV As Int32
                If myCuda.cudaAPI.cuDriverGetVersion(dV) <> myCuda.cudaError.cudaSuccess Then
                    setCudaError()
                    Return False
                Else
                    Dim vMajor As String = Mid(CStr(dV.ToString), 1, 1)
                    Dim vMinor As String = Mid(CStr(dV.ToString), 3, 1)
                    Dim vStr As String = vMajor & "." & vMinor
                    SetDriverVersion(vStr)
                    WriteLog("-Driver: " & vStr, Me)
                    vMajor = Nothing : vMinor = Nothing : vStr = Nothing : dV = Nothing
                End If
                Dim dCount As Int32
                If myCuda.cudaAPI.cuDeviceGetCount(dCount) <> myCuda.cudaError.cudaSuccess Then
                    setCudaError()
                    Return False
                Else
                    WriteLog("-Devices: " & CStr(dCount), Me)
                    For xInt As Int32 = 0 To dCount - 1
                        Dim cDev As myCuda.CUdevice
                        If myCuda.cudaAPI.cuDeviceGet(cDev, 0) <> myCuda.cudaError.cudaSuccess Then
                            setCudaError()
                            Return False
                        Else
                            Dim CudaDevice As cudaInfo.scudaDevice
                            CudaDevice = New cudaInfo.scudaDevice
                            CudaDevice.Device = cDev
                            Dim deviceID As myCuda.CUdevice = CudaDevice.Device
                            Dim b(0 To 255) As Byte, valSize As Int32 = 255
                            If myCuda.cudaAPI.cuDeviceGetName(b, valSize, deviceID) <> myCuda.cudaError.cudaSuccess Then
                                setCudaError()
                                Return False
                            Else
                                CudaDevice.Name = ByteToString(b)
                                WriteLog("-Found device: " & CudaDevice.Name, Me)
                            End If
                            b = Nothing : valSize = Nothing : Dim iMajor As Int32 : Dim iMinor As Int32
                            If myCuda.cudaAPI.cuDeviceComputeCapability(iMajor, iMinor, deviceID) <> myCuda.cudaError.cudaSuccess Then
                                setCudaError()
                                Return False
                            Else
                                CudaDevice.ComputeCapabilities = iMajor.ToString & "." & iMinor.ToString
                                WriteLog("-Compute capabilities: " & CudaDevice.ComputeCapabilities, Me)
                            End If
                            Dim mSize As SizeT
                            If myCuda.cudaAPI.cuDeviceTotalMem(mSize, deviceID) <> myCuda.cudaError.cudaSuccess Then
                                setCudaError()
                                Return False
                            Else
                                CudaDevice.DeviceTotalMemory = mSize
                            End If
                            mSize = Nothing
                            'Fill attributes
                            CudaDevice.Init()
                            Try
                                If CInt(DriverVersion) >= 4 Then
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute2))
                                    Dim infoValues() As UInt32 = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute2)), UInteger())
                                    CudaDevice.AddNames(infoNames, infoValues)
                                    Dim clDI As myCuda.CUDeviceAttribute2
                                    For YInt As Int32 = 0 To infoNames.Count - 1
                                        ' Debug.WriteLine(infoNames(YInt) & " " & infoValues(YInt))
                                        clDI = CType(infoValues(YInt), CUDeviceAttribute2)
                                        Dim rVal As Int32
                                        If myCuda.cudaAPI.cuDeviceGetAttribute(rVal, clDI, deviceID) = myCuda.cudaError.cudaSuccess Then
                                            CudaDevice.SetValue(CInt(infoValues(YInt)), rVal)
                                        Else
#If CONFIG = "Debug" Then
                                            Console.WriteLine("Can't set cuda property value for cldi: " & clDI.ToString)
#End If
                                            ''CudaDevice.removeValueName(Name)
                                            'Do
                                            'Loop
                                        End If
                                    Next
                                Else
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute1))
                                    Dim infoValues() As UInteger = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute1)), UInteger())
                                    CudaDevice.AddNames(infoNames, infoValues)
                                    Dim clDI As myCuda.CUDeviceAttribute1
                                    For YInt As Int32 = 0 To infoNames.Count - 1
                                        Debug.WriteLine(infoNames(YInt) & " " & infoValues(YInt))
                                        clDI = CType(infoValues(YInt), CUDeviceAttribute1)
                                        Dim rVal As Int32
                                        If myCuda.cudaAPI.cuDeviceGetAttribute(rVal, clDI, deviceID) = myCuda.cudaError.cudaSuccess Then
                                            CudaDevice.SetValue(CInt(infoValues(YInt)), rVal)
                                        End If
                                    Next
                                End If
                            Catch ex As Exception
                                WriteError(ex.Message, Err, Me)
                            End Try
                            AddDevice(CudaDevice)
                        End If
                    Next
                End If
                Return DeviceCount > 0
            Catch ex As Exception
                If Err.Number = 53 Then
                    'FileNotFound = not an error
                    WriteLog("CUDA Driver not found", Me)
                    Return False
                End If
                WriteError(ex.Message, Err, Me)
                Return DeviceCount > 0
            End Try
        End Function
    End Class
    Public cudaInf As New cudaInfo
    Private Sub setCudaError()
        Try
            Dim cE As myCuda.cudaError = myCuda.cudaAPI.cudaGetLastError()
            Dim cS As String = myCuda.cudaAPI.cudaGetErrorString(cE)
            cudaInf.SetLastError(cS)
        Catch ex As Exception
            WriteError(ex.Message, Err, cudaInf)
        End Try
    End Sub
#End Region
#Region "CAL"
    Public Class scalInfo
        Private cResX86 As CALresult, cResX64 As CALresult
        Public bX64 As Boolean, bX86 As Boolean
        Private cError As String
        Public ReadOnly Property calError As String
            Get
                Return cError
            End Get
        End Property
        Private sRunTime As String
        Public ReadOnly Property Runtime As String
            Get
                Return sRunTime
            End Get
        End Property
        Public Structure scalDevice
            Public Property Ordinal As UInt32
            Private Info As myCAL.CALdeviceinfo
            Public Sub SetInfo(ByVal dInfo As myCAL.CALdeviceinfo)
                Info = dInfo
            End Sub
            Public ReadOnly Property DeviceInfo As myCAL.CALdeviceinfo
                Get
                    Return Info
                End Get
            End Property
            Private Attributes As Object
            Public Property AttributeStructure As eAttributeStructure
            Public Enum eAttributeStructure
                [default]
                Alternate
            End Enum
            Public Overloads Sub SetAttributes(ByVal dAttributes As myCAL.CALdeviceattribs)
                Attributes = dAttributes
            End Sub
            Public Overloads Sub SetAttributes(ByVal Da2 As myCAL.CALdeviceattribs_2)
                Attributes = Da2
            End Sub
            Public ReadOnly Property DeviceAttributes As myCAL.CALdeviceattribs
                Get
                    If TypeOf Attributes Is myCAL.CALdeviceattribs Then Return CType(Attributes, myCAL.CALdeviceattribs)
                End Get
            End Property
            Public ReadOnly Property DeviceAttributes2 As myCAL.CALdeviceattribs_2
                Get
                    If TypeOf Attributes Is myCAL.CALdeviceattribs_2 Then Return CType(Attributes, myCAL.CALdeviceattribs_2)
                End Get
            End Property

            Public Sub Init(ByVal uiOrdinal As UInt32)
                Ordinal = uiOrdinal
            End Sub
        End Structure
        Private alDevices As ArrayList
        Public ReadOnly Property Devices As ArrayList
            Get
                Return alDevices
            End Get
        End Property
        Public ReadOnly Property DeviceCount As Int32
            Get
                If Not IsNothing(alDevices) Then
                    Return alDevices.Count
                Else
                    Return 0
                End If
            End Get
        End Property
        Public Function Init(TheLumberJack As LumberJack.LumberJack) As Boolean
            Try
                modLumberJack.LumberJack = TheLumberJack
                alDevices = New ArrayList
                sRunTime = ""
                Dim cMa As UInt32, cMi As UInt32, cIm As UInt32
                Dim iCount As UInt32 = 0
                'X86
                cResX86 = NativeMethods.calInit
                bX86 = (cResX86 = CALresult.CAL_RESULT_OK Or cResX86 = CALresult.CAL_RESULT_ALREADY)
                If Not bX86 Then
                    WriteLog("CAL_Init: " & CStr(cResX86), Me)
                    Return False
                Else
                    WriteLog("CAL platform found", Me)
                End If
                If NativeMethods.calGetVersion(cMa, cMi, cIm) <> CALresult.CAL_RESULT_OK Then
                    SetcalError(eRunTime.X86)
                    GoTo calClose
                Else
                    sRunTime = cMa.ToString & "." & cMi.ToString & "." & cIm.ToString
                    WriteLog("-Runtime: " & sRunTime, Me)
                End If
                cMa = Nothing : cMi = Nothing : cIm = Nothing
                Try
                    Dim iCal As Int32 = Get_calCount(eRunTime.X86)
                    If iCal = -1 Then
                        SetcalError(eRunTime.X86)
                        GoTo calClose
                    Else
                        WriteLog("-CAL Device count: " & CStr(iCal), Me)
                        For xInt As UInt32 = 0 To CUInt(iCal - 1)
                            Dim nD As New scalDevice
                            nD.Init(xInt)
                            nD.Ordinal = xInt
                            With nD
                                Dim dI As CALdeviceinfo
                                If NativeMethods.calDeviceGetInfo(dI, xInt) <> CALresult.CAL_RESULT_OK Then
                                    SetcalError(eRunTime.X86)
                                    dI = Nothing
                                    GoTo calClose
                                Else
                                    nD.SetInfo(dI)
                                    dI = Nothing
                                End If
                                Dim dA As New myCAL.CALdeviceattribs
                                dA.struct_size = CUInt(System.Runtime.InteropServices.Marshal.SizeOf(dA))
                                Dim mD As New CALdevice
                                If NativeMethods.calDeviceOpen(mD, xInt) <> CALresult.CAL_RESULT_OK Then
                                    cResX86 = NativeMethods.calDeviceGetAttribs(dA, xInt)
                                    If Not cResX86 = CALresult.CAL_RESULT_OK Then
                                        If cResX86 <> CALresult.CAL_RESULT_INVALID_PARAMETER Then
                                            SetcalError(eRunTime.X86)
                                            dA = Nothing
                                            GoTo calClose
                                        Else
                                            dA = Nothing
                                            Dim dA2 As New myCAL.CALdeviceattribs_2
                                            dA2.struct_size = CUInt(Marshal.SizeOf(dA2))
                                            cResX86 = NativeMethods.calDeviceGetAttribs(dA2, xInt)
                                            If cResX86 <> CALresult.CAL_RESULT_OK Then
                                                SetcalError(eRunTime.X86)
                                                dA2 = Nothing
                                                GoTo calClose
                                            End If
                                            nD.SetAttributes(dA2)
                                            nD.AttributeStructure = scalDevice.eAttributeStructure.Alternate
                                            dA2 = Nothing
                                        End If
                                    Else
                                        nD.SetAttributes(dA)
                                        nD.AttributeStructure = scalDevice.eAttributeStructure.default
                                        dA = Nothing
                                    End If
                                    NativeMethods.calDeviceClose(mD)
                                    WriteLog("--Device " & CStr(xInt) & " target: " & nD.DeviceInfo.target.ToString, Me)
                                Else
                                    SetcalError(eRunTime.X86)
                                    GoTo calClose
                                End If
                            End With
                            alDevices.Add(nD)
                        Next
                    End If
                Catch AccesEx As AccessViolationException
                    REM Throw when there are displays active on nvidia?
                    REM Above comment was made with drivers from mid 2011, don't think it will still happen.
                    SetcalError(eRunTime.X86)
                End Try
calClose:
                Close()
                Return alDevices.Count <> 0
            Catch ex As Exception
                Close()
                Return False
            End Try
        End Function
        Private Enum eRunTime
            X64
            X86
        End Enum
        Private Sub SetcalError(ByVal Runtime As eRunTime)
            Try
                cError = NativeMethods.calGetErrorString
                WriteLog("EXCEPTION: CAL ERROR " & cError, Me)
            Catch ex As Exception
                cError = ex.Message
            End Try
        End Sub
        Private Function Get_calCount(ByVal RunTime As eRunTime) As Int32
            On Error Resume Next
            Dim iCount As UInt32 = 0
            If NativeMethods.calDeviceGetCount(iCount) <> CALresult.CAL_RESULT_OK Then
                SetcalError(eRunTime.X86)
                Return -1
            End If
            Return CInt(iCount)
        End Function
        Public Sub Close()
            Try
                cResX86 = NativeMethods.calShutdown
                bX86 = (cResX86 = CALresult.CAL_RESULT_OK Or cResX86 = CALresult.CAL_RESULT_ALREADY)
            Catch ex As Exception

            End Try
        End Sub
    End Class
    Public cInfo As New scalInfo
    Public Class clsCalCheck
        Public Enum eResult
            CAL_Present = 0
            CAL_Hang = 1
            CAL_Unavailable = 2
            Other_error = 3
        End Enum
        Public Shared eRes As eResult
        Public tCheck As Threading.Thread
        Public Shared pID As IntPtr
        Public ReadOnly Property CalCheck As eResult
            Get
                Try
                    tCheck = New Threading.Thread(AddressOf CheckCAL)
                    Dim dtStart As DateTime = DateTime.Now
                    tCheck.Start()
                    While tCheck.ThreadState = Threading.ThreadState.Running AndAlso DateTime.Now.Subtract(dtStart).TotalSeconds < 10
                        Threading.Thread.Sleep(500)
                    End While
                    If tCheck.ThreadState = Threading.ThreadState.Running Then
                        'Force kill
                        tCheck.Abort()
                        Process.GetProcessById(CInt(pID)).Kill()
                        eRes = eResult.CAL_Hang
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err, Me)
                    eRes = eResult.Other_error
                End Try
                Return eRes
            End Get
        End Property
        Sub CheckCAL()
            'Run cal_cons on seperate thread, check if thread has exited after 5 seconds if not consider it hung and skip CAL
            Dim calCons As String = My.Application.Info.DirectoryPath & "\CAL_cons.exe"
            If My.Computer.FileSystem.FileExists(calCons) Then
                Using nP As New Process
                    With nP.StartInfo
                        .FileName = "CAL_cons.exe"
                        .WorkingDirectory = My.Application.Info.DirectoryPath
                        .CreateNoWindow = True ' Same as fahcontrol
                        .UseShellExecute = False
                        .RedirectStandardOutput = True
                    End With
                    nP.Start()
                    pID = New IntPtr(nP.Id)
                    Dim cOUT As StreamReader = nP.StandardOutput
                    Dim infoText As String = cOUT.ReadToEnd
                    If infoText.ToUpper.Contains(Boolean.TrueString.ToUpper) Then
                        eRes = eResult.CAL_Present
                    Else
                        eRes = eResult.CAL_Unavailable
                    End If
                End Using
            End If
        End Sub
    End Class
    Public calCheck As New clsCalCheck
    'Private Function cal_Init() As Boolean
    '    Try
    '        Try
    '            'If Not calCheck.CalCheck = clsCalCheck.eResult.CAL_Present Then
    '            '    'Check why?
    '            '    Return False
    '            'End If
    '        Catch ex As Exception
    '            WriteError(ex.Message, Err)
    '            Return False
    '        End Try
    '        cInfo.Init()
    '        If cInfo.bX64 Or cInfo.bX86 Then
    '            cInfo.Close()
    '        End If
    '        Return cInfo.DeviceCount > 0
    '    Catch ex As Exception
    '        WriteError("cal_init", Err)
    '        Return False
    '    End Try
    'End Function
#End Region
#Region "Conversion"
    Public Function ByteToUint(ByVal value() As Byte) As UInt32
        Try
            Return BitConverter.ToUInt32(value, 0)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return UInt32.MinValue
        End Try
    End Function
    Public Function ByteToULong(ByVal value() As Byte) As ULong
        Try
            Return BitConverter.ToUInt64(value, 0)
        Catch ex As Exception
            Return ULong.MinValue
        End Try
    End Function
    Public Function ByteToString(ByVal Value() As Byte) As String
        Try
            Dim rString As String = ""
            For Each b As Byte In Value
                If b = 0 Then Exit For
                rString &= ChrW(b)
            Next
            Return rString
        Catch ex As Exception
            WriteError("ByteToString", Err)
            Return vbNullString
        End Try
    End Function
    Public Function ByteToBool(ByVal value() As Byte) As Boolean
        If BitConverter.ToUInt32(value, 0) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure SizeT
        Private value As IntPtr
        Public Sub New(ByVal value As Integer)
            Me.value = New IntPtr(value)
        End Sub

        Public Sub New(ByVal value As UInt32)
            Me.value = New IntPtr(CInt(value))
        End Sub

        Public Sub New(ByVal value As UInt64)
            Me.value = New IntPtr(CLng(value))
        End Sub

        Public Sub New(ByVal value As Long)
            Me.value = New IntPtr(value)
        End Sub

        Public Shared Widening Operator CType(ByVal t As SizeT) As Integer
            Return t.value.ToInt32
        End Operator

        Public Shared Widening Operator CType(ByVal t As SizeT) As UInt32
            Return Convert.ToUInt32(CInt(t.value))
        End Operator

        Public Shared Widening Operator CType(ByVal t As SizeT) As Long
            Return t.value.ToInt64
        End Operator

        Public Shared Widening Operator CType(ByVal t As SizeT) As UInt64
            Return CULng(CLng(t.value))
        End Operator

        Public Shared Widening Operator CType(ByVal value As Integer) As SizeT
            Return New SizeT(value)
        End Operator

        Public Shared Widening Operator CType(ByVal value As UInt32) As SizeT
            Return New SizeT(value)
        End Operator

        Public Shared Widening Operator CType(ByVal value As Long) As SizeT
            Return New SizeT(value)
        End Operator

        Public Shared Widening Operator CType(ByVal value As UInt64) As SizeT
            Return New SizeT(value)
        End Operator

        Public Shared Operator <>(ByVal val1 As SizeT, ByVal val2 As SizeT) As Boolean
            Return (val1.value <> val2.value)
        End Operator

        Public Shared Operator =(ByVal val1 As SizeT, ByVal val2 As SizeT) As Boolean
            Return (val1.value = val2.value)
        End Operator

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return Me.value.Equals(obj)
        End Function

        Public Overrides Function ToString() As String
            Return Me.value.ToString
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return Me.value.GetHashCode
        End Function
    End Structure
#End Region
End Module
