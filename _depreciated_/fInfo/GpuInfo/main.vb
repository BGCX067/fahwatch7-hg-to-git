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
Imports System.ServiceProcess
Imports gpuInfo.myOCL.oclAPI
Imports gpuInfo.myCuda
Imports gpuInfo.myCuda.cudaAPI
Imports gpuInfo.myCAL
Imports System.Text
Imports System.Security.Permissions
Namespace gpuInfo
    Public Module Main
#Region "openCL"
        <Serializable()>
        Public Class oclInfo
            <Serializable()>
            Public Structure sopenclPlatform
                Public Name As String
                Public Vendor As String
                Public Version As String
                Public Extensions As String
                Public Profile As String
                <Serializable()>
                Public Structure sopenCLDevice
                    Public Property Index As String
                    Private alProperties As ArrayList
                    <Serializable()>
                    Public Structure sProperty
                        Public Name As String
                        Public iEnum As Int32
                        Public Value As Object
                        Public ConvertedValue As Object
                    End Structure
                    Public Sub SetValue(ByVal iEnum As Int32, ByVal Value As Object)
                        For xInt As Int32 = 0 To alProperties.Count - 1
                            If CType(alProperties(xInt), sProperty).iEnum = iEnum Then
                                Dim np As sProperty = CType(alProperties(xInt), sProperty)
                                np.Value = Value
                                Select Case np.iEnum
                                    Case Is = dInfo.AddressBits
                                        np.ConvertedValue = BitConverter.ToUInt32(Value, 0)
                                    Case Is = dInfo.Available
                                        np.ConvertedValue = Main.ByteToBool(Value)
                                    Case Is = dInfo.CompilerAvailable
                                        np.ConvertedValue = Main.ByteToBool(Value)
                                    Case Is = dInfo.DriverVersion
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Is = dInfo.EndianLittle
                                        np.ConvertedValue = Main.ByteToBool(Value)
                                    Case Is = dInfo.ErrorCorrectionSupport
                                        np.ConvertedValue = Main.ByteToBool(Value)
                                    Case Is = dInfo.ExecutionCapabilities
                                        np.ConvertedValue = Main.ByteToULong(Value)
                                    Case Is = dInfo.Extensions
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Is = dInfo.LocalMemType
                                        np.ConvertedValue = Main.ByteToULong(Value)
                                    Case Is = dInfo.ImageSupport
                                        np.ConvertedValue = Main.ByteToBool(Value)
                                    Case Is = dInfo.Type
                                        Dim dConv As ULong = Main.ByteToULong(Value)
                                        np.ConvertedValue = DeviceTypeString(dConv)

                                    Case Is = dInfo.Name
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Is = dInfo.Vendor
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Is = dInfo.VendorID
                                        np.ConvertedValue = Main.ByteToUint(Value)
                                    Case Is = dInfo.Version
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Is = dInfo.SingleFPConfig
                                        np.ConvertedValue = Main.ByteToULong(Value)
                                    Case Is = dInfo.Profile
                                        np.ConvertedValue = Main.ByteToString(Value)
                                    Case Else
                                        np.ConvertedValue = Main.ByteToUint(Value)
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
                                    Return CType(sProp, sProperty).ConvertedValue
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
                            Dim infoNames() As String = [Enum].GetNames(GetType(myOCL.oclAPI.dInfo))
                            Dim infoValues() As Main.SizeT = [Enum].GetValues(GetType(oclInfo.sopenclPlatform.sopenCLDevice.sProperty))
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
                        If colDevices.Contains(DeviceID) Then Return colDevices(DeviceID)
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
            Public Function Init() As Boolean
                Try
                    Dim bOnce As Boolean = True
                    alPlatforms.Clear()
Start:
                    Dim pIDcount As IntPtr, pID() As myOCL.oclAPI.CLPlatformID
                    Dim nEntries As Main.SizeT, nPlatforms As Main.SizeT
                    If clGetPlatformIDs(nEntries, pIDcount, nPlatforms) = myOCL.oclAPI.CLError.Success Then
                        'Log("found " & nPlatforms.ToString & " platforms")
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
                                                        'Log("-Name " & sName)
                                                        oclP.Name = sName
                                                    Case Is = 1
                                                        'Log("-Version " & sName)
                                                        oclP.Version = sName
                                                    Case Is = 2
                                                        'Log("-Vendor " & sName)
                                                        oclP.Vendor = sName
                                                    Case Is = 3
                                                        'Log("-Profile " & sName)
                                                        oclP.Profile = sName
                                                    Case Is = 4
                                                        'Log("-Extensions " & sName)
                                                        oclP.Extensions = sName
                                                End Select
                                            End If
                                        End If
                                    End If
                                    loopCount += 1
                                Loop While loopCount < 5
                                Dim dID() As myOCL.oclAPI.CLDeviceID
                                Dim _did As IntPtr
                                Dim nDev As Long
                                If myOCL.oclAPI.clGetDeviceIDs(oPL, CLDeviceType.GPU, nEntries, _did, nDev) = myOCL.oclAPI.CLError.Success Then
                                    ReDim dID(0 To nDev - 1)
                                    If myOCL.oclAPI.clGetDeviceIDs(oPL, CLDeviceType.GPU, nDev, dID, nDev) = myOCL.oclAPI.CLError.Success Then
                                        Dim iID As Int32 = 0
                                        For Each sDid As myOCL.oclAPI.CLDeviceID In dID
                                            Dim oclD As New Main.oclInfo.sopenclPlatform.sopenCLDevice
                                            oclD.Init()
                                            oclD.Index = iID
                                            oclD.Index += 1
                                            Dim infoNames() As String = [Enum].GetNames(GetType(myOCL.oclAPI.dInfo))
                                            Dim infoValues() As UInt32 = [Enum].GetValues(GetType(myOCL.oclAPI.dInfo))
                                            oclD.AddNames(infoNames, infoValues)
                                            Dim clDI As dInfo
                                            For xInt As Int32 = 0 To infoNames.Count - 1
                                                'Log(infoNames(xInt) & " " & infoValues(xInt))
                                                clDI = infoValues(xInt)
                                                Dim pvSize As Main.SizeT, vSize As Main.SizeT, vIntPtr As IntPtr, Value() As Byte
                                                If myOCL.oclAPI.clGetDeviceInfo(sDid, clDI, pvSize, vIntPtr, vSize) = myOCL.oclAPI.CLError.Success Then
                                                    ReDim Value(0 To vSize)
                                                    If clGetDeviceInfo(sDid, clDI, vSize, Value, vSize) = myOCL.oclAPI.CLError.Success Then
                                                        oclD.SetValue(clDI, Value)
                                                    End If
                                                End If
                                            Next
                                            oclP.colDevices.Add(oclD)
                                        Next
                                    End If
                                End If
                                Platform(NumberOfPlatforms) = oclP
                            Next
                        End If
                    Else
                        If bOnce Then
                            bOnce = False
                            GoTo Start
                        End If
                    End If
                    Return NumberOfPlatforms > 0
                Catch ex As Exception
                    LogWindow.WriteError("Interop_openCLInit", Err)
                    Return False
                    GoTo Start
                End Try
            End Function
        End Class
        Public oclInf As New Main.oclInfo
        Private Function openCL_init() As Boolean
            Try
                Return oclInf.Init
            Catch ex As Exception
                LogWindow.WriteError("Interop_openCLInit", Err)
                Return Init()
            End Try
        End Function
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
                                Debug.WriteLine(np.Name & " : " & np.Value)

                            Catch ex As Exception

                            End Try
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
                        Dim r(0 To alProperties.Count - 1) As sProperty
                        For xint As Int32 = 0 To alProperties.Count - 1
                            r(xint) = CType(alProperties(xint), sProperty)
                        Next
                        Return r
                    End Get
                End Property

                Public Sub Init()
                    alProperties = New ArrayList
                End Sub

            End Structure
            Private alDevices As New ArrayList
            Public Property cudaDevice(ByVal Index As Short) As scudaDevice
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
            Public Function Init() As Boolean
                Try
                    alDevices.Clear()
                    If myCuda.cudaAPI.cuInit(0) <> myCuda.cudaError.cudaSuccess Then
                        setCudaError()
                        Return False
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
                        vMajor = Nothing : vMinor = Nothing : vStr = Nothing : dV = Nothing
                    End If
                    Dim dCount As Int32
                    If myCuda.cudaAPI.cuDeviceGetCount(dCount) <> myCuda.cudaError.cudaSuccess Then
                        setCudaError()
                        Return False
                    Else
                        For xInt As Int32 = 0 To dCount - 1
                            Dim cDev As myCuda.CUdevice
                            If myCuda.cudaAPI.cuDeviceGet(cDev, 0) <> myCuda.cudaError.cudaSuccess Then
                                setCudaError()
                                Return False
                            Else
                                Dim CudaDevice As New cudaInfo.scudaDevice
                                CudaDevice.Device = cDev

                                Dim deviceID As myCuda.CUdevice = CudaDevice.Device
                                Dim b(0 To 255) As Byte, valSize As Int32 = 255
                                If myCuda.cudaAPI.cuDeviceGetName(b, valSize, deviceID) <> myCuda.cudaError.cudaSuccess Then
                                    setCudaError()
                                    Return False
                                Else
                                    CudaDevice.Name = ByteToString(b)
                                End If
                                b = Nothing : valSize = Nothing : Dim iMajor As Int32 : Dim iMinor As Int32
                                If myCuda.cudaAPI.cuDeviceComputeCapability(iMajor, iMinor, deviceID) <> myCuda.cudaError.cudaSuccess Then
                                    setCudaError()
                                    Return False
                                Else
                                    CudaDevice.ComputeCapabilities = iMajor.ToString & "." & iMinor.ToString
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
                                If CInt(_DriverVersion) >= 4 Then
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute4))
                                    Dim infoValues() As UInt32 = [Enum].GetValues(GetType(myCuda.CUDeviceAttribute4))
                                    CudaDevice.AddNames(infoNames, infoValues)
                                    Dim clDI As myCuda.CUDeviceAttribute4
                                    For YInt As Int32 = 0 To infoNames.Count - 1
                                        Try
                                            Debug.WriteLine(infoNames(YInt) & " " & infoValues(YInt))
                                            clDI = infoValues(YInt)
                                            Dim rVal As Int32
                                            If myCuda.cudaAPI.cuDeviceGetAttribute(rVal, clDI, deviceID) <> myCuda.cudaError.cudaSuccess AndAlso Not clDI = infoValues(35) Then
                                                If Not clDI = myCuda.CUDeviceAttribute4.CU_DEVICE_ATTRIBUTE_TCC_DRIVER Then
                                                    setCudaError()
                                                    Return False
                                                End If
                                            Else
                                                CudaDevice.SetValue(infoValues(YInt), rVal)
                                            End If
                                        Catch ex As Exception

                                        End Try

                                    Next
                                Else
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute3))
                                    Dim infoValues() As UInt32 = [Enum].GetValues(GetType(myCuda.CUDeviceAttribute3))
                                    CudaDevice.AddNames(infoNames, infoValues)
                                    Dim clDI As myCuda.CUDeviceAttribute3
                                    For YInt As Int32 = 0 To infoNames.Count - 1
                                        Try
                                            Debug.WriteLine(infoNames(YInt) & " " & infoValues(YInt))
                                            clDI = infoValues(YInt)
                                            Dim rVal As Int32
                                            If myCuda.cudaAPI.cuDeviceGetAttribute(rVal, clDI, deviceID) <> myCuda.cudaError.cudaSuccess AndAlso Not clDI = infoValues(35) Then
                                                If Not clDI = myCuda.CUDeviceAttribute3.CU_DEVICE_ATTRIBUTE_TCC_DRIVER Then
                                                    setCudaError()
                                                    Return False
                                                End If
                                            Else
                                                CudaDevice.SetValue(infoValues(YInt), rVal)
                                            End If
                                        Catch ex As Exception

                                        End Try
                                    Next
                                End If
                                AddDevice(CudaDevice)
                                CudaDevice = Nothing
                            End If
                        Next
                    End If
                    Return DeviceCount <> 0
                Catch ex As Exception

                End Try
            End Function
        End Class
        Public cudaInf As New cudaInfo
        Private Sub setCudaError()
            Dim cE As myCuda.cudaError = myCuda.cudaAPI.cudaGetLastError()
            Dim cS As String = myCuda.cudaAPI.cudaGetErrorString(cE)
            cudaInf.SetLastError(cS)
        End Sub
#End Region
#Region "CAL"
        <Serializable()>
        Public Structure scalInfo
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
            <Serializable()>
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
                    Return alDevices.Count
                End Get
            End Property
            Public Function Init() As Boolean
                Try
                    alDevices = New ArrayList
                    sRunTime = ""

                    If IntPtr.Size = 4 Then
                        Dim cMa As UInt32, cMi As UInt32, cIm As UInt32
                        Dim iCount As UInt32 = 0
                        'X86
                        cResX86 = CALRuntimeX86.calInit
                        bX86 = (cResX86 = CALresult.CAL_RESULT_OK Or cResX86 = CALresult.CAL_RESULT_ALREADY)
                        If Not bX86 Then Return False

                        If CALRuntimeX86.calGetVersion(cMa, cMi, cIm) <> CALresult.CAL_RESULT_OK Then
                            SetcalError(eRunTime.X86)
                            GoTo calClose
                        Else
                            sRunTime = cMa.ToString & "." & cMi.ToString & "." & cIm.ToString
                        End If
                        cMa = Nothing : cMi = Nothing : cIm = Nothing
                        Try
                            Dim iCal As Int32 = Get_calCount(eRunTime.X86)
                            If iCal = -1 Then
                                SetcalError(eRunTime.X86)
                                GoTo calClose
                            Else
                                For xInt As UInt32 = 0 To CInt(iCal) - 1
                                    Dim nD As New scalDevice
                                    nD.Init(xInt)
                                    nD.Ordinal = xInt
                                    With nD
                                        Dim dI As CALdeviceinfo
                                        If CALRuntimeX86.calDeviceGetInfo(dI, xInt) <> CALresult.CAL_RESULT_OK Then
                                            SetcalError(eRunTime.X86)
                                            dI = Nothing
                                            GoTo calClose
                                        Else
                                            nD.SetInfo(dI)
                                            dI = Nothing
                                        End If
                                        Dim dA As New myCAL.CALdeviceattribs
                                        dA.struct_size = System.Runtime.InteropServices.Marshal.SizeOf(dA)
                                        Dim mD As New CALdevice
                                        If CALRuntimeX86.calDeviceOpen(mD, xInt) <> CALresult.CAL_RESULT_OK Then
                                            cResX86 = CALRuntimeX86.calDeviceGetAttribs(dA, xInt)
                                            If Not cResX86 = CALresult.CAL_RESULT_OK Then
                                                If cResX86 <> CALresult.CAL_RESULT_INVALID_PARAMETER Then
                                                    SetcalError(eRunTime.X86)
                                                    dA = Nothing
                                                    GoTo calClose
                                                Else
                                                    dA = Nothing
                                                    Dim dA2 As New myCAL.CALdeviceattribs_2
                                                    dA2.struct_size = Marshal.SizeOf(dA2)
                                                    cResX86 = CALRuntimeX86.calDeviceGetAttribs(dA2, xInt)
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
                                            CALRuntimeX86.calDeviceClose(mD)
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
                            SetcalError(eRunTime.X86)
                        End Try


                    ElseIf IntPtr.Size = 8 Then
                        'X64
                        Dim cMa As UInt64, cMi As UInt64, cIm As UInt64
                        Dim iCount As UInt64 = 0
                        cResX64 = CALRuntimeX64.calInit
                        bX64 = (cResX64 = CALresult.CAL_RESULT_OK Or cResX64 = CALresult.CAL_RESULT_ALREADY)
                        If Not bX64 Then Return False
                        Dim VersionResult As CALresult = CALRuntimeX64.calGetVersion(cMa, cMi, cIm)
                        If CALRuntimeX64.calGetVersion(cMa, cMi, cIm) <> CALresult.CAL_RESULT_OK Then
                            SetcalError(eRunTime.X64)
                            GoTo calClose
                        Else
                            sRunTime = cMa.ToString & "." & cMi.ToString & "." & cIm.ToString
                        End If
                        cMa = Nothing : cMi = Nothing : cIm = Nothing
                        If CALRuntimeX64.calDeviceGetCount(iCount) <> CALresult.CAL_RESULT_OK Then
                            SetcalError(eRunTime.X64)
                            GoTo calClose
                        Else
                            Dim iCal As Int32 = Get_calCount(bX64)
                            For xInt As UInt32 = 0 To CInt(iCal) - 1
                                Dim nD As New scalDevice
                                nD.Init(xInt)
                                nD.Ordinal = xInt
                                With nD
                                    Dim dI As CALdeviceinfo
                                    If CALRuntimeX64.calDeviceGetInfo(dI, xInt) <> CALresult.CAL_RESULT_OK Then
                                        SetcalError(eRunTime.X64)
                                        dI = Nothing
                                        GoTo calClose
                                    Else
                                        nD.SetInfo(dI)
                                        dI = Nothing
                                    End If
                                    Dim dA As New myCAL.CALdeviceattribs
                                    dA.struct_size = System.Runtime.InteropServices.Marshal.SizeOf(dA)
                                    Dim mD As New CALdevice
                                    If CALRuntimeX64.calDeviceOpen(mD, xInt) <> CALresult.CAL_RESULT_OK Then
                                        cResX64 = CALRuntimeX64.calDeviceGetAttribs(dA, xInt)
                                        If Not cResX64 = CALresult.CAL_RESULT_OK Then
                                            If cResX64 <> CALresult.CAL_RESULT_INVALID_PARAMETER Then
                                                SetcalError(eRunTime.X64)
                                                dA = Nothing
                                                GoTo calClose
                                            Else
                                                dA = Nothing
                                                Dim dA2 As New myCAL.CALdeviceattribs_2
                                                dA2.struct_size = Marshal.SizeOf(dA2)
                                                cResX64 = CALRuntimeX64.calDeviceGetAttribs(dA2, xInt)
                                                If cResX64 <> CALresult.CAL_RESULT_OK Then
                                                    SetcalError(eRunTime.X64)
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
                                        CALRuntimeX64.calDeviceClose(mD)
                                    Else
                                        SetcalError(eRunTime.X64)
                                        GoTo calClose
                                    End If
                                End With
                                alDevices.Add(nD)
                            Next
                        End If

                    End If


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
                    If Runtime = eRunTime.X64 Then
                        cError = CALRuntimeX64.calGetErrorString
                    Else
                        cError = CALRuntimeX86.calGetErrorString
                    End If
                Catch ex As Exception
                    cError = ex.Message
                End Try
            End Sub
            Private Function Get_calCount(ByVal RunTime As eRunTime) As Int32
                On Error Resume Next
                Dim iCount As Int32 = 0
                If RunTime = eRunTime.X86 Then
                    If CALRuntimeX86.calDeviceGetCount(iCount) <> CALresult.CAL_RESULT_OK Then
                        SetcalError(eRunTime.X86)
                        Return -1
                    End If
                ElseIf RunTime = eRunTime.X64 Then
                    If CALRuntimeX64.calDeviceGetCount(iCount) <> CALresult.CAL_RESULT_OK Then
                        SetcalError(eRunTime.X64)
                        Return -1
                    End If
                End If
                Return iCount
            End Function
            Public Sub Close()
                If bX64 Then
                    cResX64 = CALRuntimeX64.calShutdown
                    bX64 = (cResX64 = CALresult.CAL_RESULT_OK Or cResX64 = CALresult.CAL_RESULT_ALREADY)
                ElseIf bX86 Then
                    cResX86 = CALRuntimeX86.calShutdown
                    bX86 = (cResX86 = CALresult.CAL_RESULT_OK Or cResX86 = CALresult.CAL_RESULT_ALREADY)
                End If
            End Sub
        End Structure
        Public cInfo As New scalInfo
        Private Function cal_Init() As Boolean
            Try
                cInfo.Init()
                If cInfo.bX64 Or cInfo.bX86 Then
                    cInfo.Close()
                End If
                Return cInfo.DeviceCount > 0
            Catch ex As Exception
                LogWindow.WriteError("cal_init", Err)
                Return False
            End Try
        End Function
#End Region
#Region "Text reports"
        Public Function deviceReport() As String
            'Dim sR As New StringBuilder("")
            'Try
            '    Dim gInfo As devManager.devManager = gpuInfo.devManager
            '    Dim lDev As New List(Of devManager.devManager.sDevice)
            '    lDev = gInfo.GetDeviceList
            '    For Each Device As devManager.devManager.sDevice In lDev
            '        With sR
            '            .AppendLine("Name:-" & Device.Name)
            '            .AppendLine("-GUID:-" & Device.DeviceInfo.classGuid.ToString & environment.newline)
            '            .AppendLine("-DevInstall:-" & Device.DeviceInfo.devInst & environment.newline)
            '            .AppendLine("-Reserved:-" & Device.DeviceInfo.reserved.ToString & environment.newline)
            '            .AppendLine("")
            '        End With
            '    Next
            'Catch ex As Exception
            '    ExReport(ex, sR)
            'End Try
            'Return sR.ToString
            Throw New NotImplementedException
        End Function
        Public Function displayReport() As String
            'Dim rS As New StringBuilder("")
            'Try
            '    rS.AppendLine("DisplayCount:-" & disManager.m_Displays.Count.ToString)
            '    For Each disPlay As displayManager.Display In disManager.m_Displays
            '        rS.AppendLine(disPlay.DeviceName & ":-" & disPlay.AdapterName)
            '        rS.AppendLine(disPlay.MonitorName & ":-" & disPlay.Attached.ToString)
            '        rS.AppendLine("")
            '    Next
            '    Return rS.ToString
            'Catch ex As Exception
            '    'log error
            '    Return rS.ToString
            'End Try
            Throw New NotImplementedException
        End Function


#End Region
        Public Function Init() As Boolean
            Try
                cInfo = Nothing
                cudaInf = Nothing
                oclInf = Nothing
                cInfo = New scalInfo
                cudaInf = New cudaInfo
                oclInf = New oclInfo
                Dim bCal As Boolean = cal_Init()
                If bCal Then
                    LogWindow.WriteLog("CAL platform found!")
                End If
                Dim bOcl As Boolean = openCL_init()
                If bOcl Then
                    LogWindow.WriteLog("openCL platform(s) found!")
                End If
                Dim bCuda As Boolean = cudaInf.Init()
                If bCuda Then
                    LogWindow.WriteLog("cuda platform found!")
                End If
                Return bCal Or bOcl Or bCuda
            Catch ex As Exception
                LogWindow.WriteError("GpuInfo init", Err)
                Return False
            End Try
        End Function
        Public Function ByteToUint(ByVal value() As Byte) As UInt32
            Try
                Return BitConverter.ToUInt32(value, 0)
            Catch ex As Exception
                LogWindow.WriteError("ByteToUint", Err)
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
                LogWindow.WriteError("ByteToString", Err)
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
        <Serializable()> <StructLayout(LayoutKind.Sequential)> _
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
        Public Class cLW
            Public Event Log(ByVal Message As String)
            Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
            Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                RaiseEvent LogError(Message, EObj)
            End Sub
            Public Sub WriteLog(ByVal Message As String)
                RaiseEvent Log(Message)
            End Sub
        End Class
        Public WithEvents LogWindow As New cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
            RaiseEvent Log(Message)
        End Sub
        Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
            RaiseEvent LogError(Message, EObj)
        End Sub
    End Module

End Namespace