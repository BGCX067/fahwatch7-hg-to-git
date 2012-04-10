'/*
' * HWInfo class Copyright 2011-2012 Marvin Westmaas ( mtm )
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
Imports System.Management
Imports System.Management.Instrumentation
Imports Microsoft.Win32
Imports System.IO
Imports System.Threading
Imports System.Security.Principal
Imports OpenHardwareMonitor
Imports OpenHardwareMonitor.Hardware
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Text
Imports System
Imports gpuInfo.Main
Imports gpuInfo
Public Class clsHWInfo
    Implements IDisposable
    Public Class cHWInfo
        Implements IDisposable
#Region "GpuInfo.dll functions"
        Public Class cGInfo
            Implements IDisposable
            Public Function openCLReport() As String
                Dim sr As New StringBuilder("")
                Try
                    sr.AppendLine("Opencl enumeration:- " & oclInf.NumberOfPlatforms & " platforms")
                    If oclInf.NumberOfPlatforms > 0 Then
                        For xInt As Int32 = 0 To oclInf.NumberOfPlatforms - 1
                            Dim oP As oclInfo.sopenclPlatform = oclInf.Platform(xInt)
                            sr.AppendLine("platform:- " & xInt.ToString)
                            sr.AppendLine(":-- " & oP.Name & " - " & oP.Vendor & "- " & oP.Version)
                            sr.AppendLine("Extensions:- " & oP.Extensions)
                            el(sr)
                            sr.AppendLine("Number of devices:- " & oclInf.Platform(xInt).NumberOfDevices.ToString)
                            If oclInf.Platform(xInt).NumberOfDevices > 0 Then
                                For yInt As Int32 = 1 To oclInf.Platform(xInt).NumberOfDevices
                                    Dim openCLD As oclInfo.sopenclPlatform.sopenCLDevice = oclInf.Platform(xInt).Device_ByIndex(yInt)
                                    sr.AppendLine("Index:- " & openCLD.Index)
                                    sr.AppendLine("Attribute_count:= " & openCLD.AttributeCount.ToString)
                                    If openCLD.AttributeCount > 0 Then
                                        Dim At() As oclInfo.sopenclPlatform.sopenCLDevice.sProperty = CType(openCLD.DeviceAttributes, Main.oclInfo.sopenclPlatform.sopenCLDevice.sProperty())
                                        For Each atr As oclInfo.sopenclPlatform.sopenCLDevice.sProperty In At
                                            sr.AppendLine(atr.Name & ":- " & CStr(atr.ConvertedValue))
                                        Next
                                    End If
                                    el(sr)
                                Next
                            End If
                        Next
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return sr.ToString
            End Function
            Public Function cudaReport() As String
                Dim sr As New StringBuilder("")
                Try
                    sr.AppendLine("CUDA enumeration:- " & cudaInf.DeviceCount & " devices")
                    If cudaInf.DeviceCount > 0 Then
                        sr.AppendLine("Cuda_driver:- " & cudaInf.DriverVersion)
                        For xint As Int32 = 1 To cudaInf.DeviceCount - 1
                            Dim mC As cudaInfo.scudaDevice = cudaInf.cudaDevice(xint)
                            sr.AppendLine("Name:- " & mC.Name)
                            sr.AppendLine("Compute capabilities:- " & mC.ComputeCapabilities)
                            sr.AppendLine("Attribute_count:- " & mC.AttributeCount.ToString)
                            If mC.AttributeCount > 0 Then
                                Dim cP() As cudaInfo.scudaDevice.sProperty = CType(mC.DeviceAttributes, Main.cudaInfo.scudaDevice.sProperty())
                                If CInt(cudaInfo.DriverVersion) >= 4 Then
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute2))
                                    Dim infoValues() As UInt32 = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute2)), UInteger())
                                    For Each dA In cP
                                        sr.AppendLine(dA.Name & ":- " & CStr(dA.Value))
                                    Next
                                Else
                                    Dim infoNames() As String = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute1))
                                    Dim infoValues() As UInt32 = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute1)), UInteger())
                                    For Each dA In cP
                                        sr.AppendLine(dA.Name & ":- " & CStr(dA.Value))
                                    Next
                                End If
                            End If
                        Next
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return sr.ToString
            End Function
            Public Function calReport() As String
                Dim sR As New StringBuilder("")
                Try
                    sR.AppendLine("CAL Runtime:- " & calInf.Runtime)
                    If calInf.calError <> "" And calInf.DeviceCount = 0 Then
                        sR.AppendLine("++CAL_ERROR:- " & calInf.calError)
                    End If
                    If calInf.DeviceCount > 0 Then
                        Dim cD As ArrayList = calInf.Devices
                        For Each cDev As scalInfo.scalDevice In cD
                            sR.AppendLine("Ordinal:- " & cDev.Ordinal.ToString)
                            sR.AppendLine("Device_info_maxResource1DWidth:- " & cDev.DeviceInfo.maxResource1DWidth.ToString)
                            sR.AppendLine("Device_info_maxResource2DHeight :- " & cDev.DeviceInfo.maxResource2DHeight.ToString)
                            sR.AppendLine("Device_info_maxResource2DWidth:- " & cDev.DeviceInfo.maxResource2DWidth.ToString)
                            sR.AppendLine("Target:- " & cDev.DeviceInfo.target.ToString)
                            If cDev.AttributeStructure = scalInfo.scalDevice.eAttributeStructure.default Then
                                Dim cDevAt As myCAL.CALdeviceattribs = cDev.DeviceAttributes
                                sR.AppendLine("b3dProgramGrid:- " & cDevAt.b3dProgramGrid.ToString)
                                sR.AppendLine("bUAVMemExport:- " & cDevAt.bUAVMemExport.ToString)
                                sR.AppendLine("cachedRemoteram:- " & cDevAt.cachedRemoteRAM.ToString)
                                sR.AppendLine("computeShader:- " & cDevAt.computeShader.ToString)
                                sR.AppendLine("doublePrecision:- " & cDevAt.doublePrecision.ToString)
                                sR.AppendLine("engineClock:- " & cDevAt.engineClock.ToString)
                                sR.AppendLine("globalDataShare:- " & cDevAt.globalDataShare.ToString)
                                sR.AppendLine("globalGPR:- " & cDevAt.globalGPR.ToString)
                                sR.AppendLine("localDataShare:- " & cDevAt.localDataShare.ToString)
                                sR.AppendLine("localRam:- " & cDevAt.localRAM.ToString)
                                sR.AppendLine("localMemExport:- " & cDevAt.memExport.ToString)
                                sR.AppendLine("localMemoryClock:- " & cDevAt.memoryClock.ToString)
                                sR.AppendLine("numberOfShaderEngines:- " & cDevAt.numberOfShaderEngines.ToString)
                                sR.AppendLine("numberOfSIMD:- " & cDevAt.numberOfSIMD.ToString)
                                sR.AppendLine("numberOfUAVs:- " & cDevAt.numberOfUAVs.ToString)
                                sR.AppendLine("pitch_alignment:- " & cDevAt.pitch_alignment.ToString)
                                sR.AppendLine("surface_alignment:- " & cDevAt.surface_alignment.ToString)
                                sR.AppendLine("target:- " & cDevAt.target.ToString)
                                sR.AppendLine("targetRevision:- " & cDevAt.targetRevision.ToString)
                                sR.AppendLine("uncachedRemoteRAM:- " & cDevAt.uncachedRemoteRAM.ToString)
                                sR.AppendLine("wavefrontSize:- " & cDevAt.wavefrontSize.ToString)
                                el(sR)
                            Else
                                Dim cDevAt As myCAL.CALdeviceattribs_2 = cDev.DeviceAttributes2
                                '.AppendLine("b3dProgramGrid:- " & cDevAt.b3dProgramGrid.ToString )
                                '.AppendLine("bUAVMemExport:- " & cDevAt.bUAVMemExport.ToString )
                                sR.AppendLine("cachedRemoteram:- " & cDevAt.cachedRemoteRAM.ToString)
                                sR.AppendLine("computeShader:- " & cDevAt.computeShader.ToString)
                                sR.AppendLine("doublePrecision:- " & cDevAt.doublePrecision.ToString)
                                sR.AppendLine("engineClock:- " & cDevAt.engineClock.ToString)
                                sR.AppendLine("globalDataShare:- " & cDevAt.globalDataShare.ToString)
                                sR.AppendLine("globalGPR:- " & cDevAt.globalGPR.ToString)
                                sR.AppendLine("localDataShare:- " & cDevAt.localDataShare.ToString)
                                sR.AppendLine("localRam:- " & cDevAt.localRAM.ToString)
                                sR.AppendLine("localMemExport:- " & cDevAt.memExport.ToString)
                                sR.AppendLine("localMemoryClock:- " & cDevAt.memoryClock.ToString)
                                'sR.AppendLine("numberOfShaderEngines:- " & cDevAt.numberOfShaderEngines.ToString )
                                sR.AppendLine("numberOfSIMD:- " & cDevAt.numberOfSIMD.ToString)
                                '.AppendLine("numberOfUAVs:- " & cDevAt.numberOfUAVs.ToString )
                                sR.AppendLine("pitch_alignment:- " & cDevAt.pitch_alignment.ToString)
                                sR.AppendLine("surface_alignment:- " & cDevAt.surface_alignment.ToString)
                                sR.AppendLine("target:- " & cDevAt.target.ToString)
                                '.AppendLine("targetRevision:- " & cDevAt.targetRevision.ToString )
                                sR.AppendLine("uncachedRemoteRAM:- " & cDevAt.uncachedRemoteRAM.ToString)
                                sR.AppendLine("wavefrontSize:- " & cDevAt.wavefrontSize.ToString)
                                el(sR)
                            End If
                        Next
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return sR.ToString
            End Function
            Private Sub ExReport(ByVal theExeption As Exception, ByVal sR As StringBuilder)
                sR.AppendLine("Error:-" & theExeption.Message)
                sR.AppendLine("Stack:-" & theExeption.StackTrace.ToString)
            End Sub
            Public Function TextReport() As String
                Dim sR As New StringBuilder
                Try
                    sR.AppendLine(vbTab & My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString & Environment.NewLine)
                    el(sR)
                    sR.AppendLine("Opencl enumeration:- " & oclInf.NumberOfPlatforms & " platforms")
                    If oclInf.NumberOfPlatforms > 0 Then
                        For xInt As Int32 = 0 To oclInf.NumberOfPlatforms - 1
                            Dim oP As oclInfo.sopenclPlatform = oclInf.Platform(xInt)
                            sR.AppendLine("platform:- " & xInt.ToString)
                            sR.AppendLine(":-- " & oP.Name & " - " & oP.Vendor & "- " & oP.Version)
                            sR.AppendLine("Extensions:- " & oP.Extensions)
                            el(sR)
                            sR.AppendLine("Number of devices:- " & oclInf.Platform(xInt).NumberOfDevices.ToString)
                            If oclInf.Platform(xInt).NumberOfDevices > 0 Then
                                For yInt As Int32 = 1 To oclInf.Platform(xInt).NumberOfDevices
                                    Dim openCLD As oclInfo.sopenclPlatform.sopenCLDevice = oclInf.Platform(xInt).Device_ByIndex(yInt)
                                    sR.AppendLine("Index:- " & openCLD.Index)
                                    sR.AppendLine("Attribute_count:= " & openCLD.AttributeCount.ToString)
                                    If openCLD.AttributeCount > 0 Then
                                        Dim At() As oclInfo.sopenclPlatform.sopenCLDevice.sProperty = CType(openCLD.DeviceAttributes, Main.oclInfo.sopenclPlatform.sopenCLDevice.sProperty())
                                        For Each atr As oclInfo.sopenclPlatform.sopenCLDevice.sProperty In At
                                            sR.AppendLine(atr.Name & ":- " & CStr(atr.ConvertedValue))
                                        Next
                                    End If
                                    el(sR)
                                Next
                            End If
                        Next
                    End If
                    Try
                        sR.AppendLine("CUDA enumeration:- " & cudaInf.DeviceCount & " devices")
                        If cudaInf.DeviceCount > 0 Then
                            sR.AppendLine("Cuda_driver:- " & cudaInf.DriverVersion)
                            For xint As Int32 = 1 To cudaInf.DeviceCount
                                Dim mC As cudaInfo.scudaDevice = cudaInf.cudaDevice(xint)
                                sR.AppendLine("Name:- " & mC.Name)
                                sR.AppendLine("Compute capabilities:- " & mC.ComputeCapabilities)
                                sR.AppendLine("Attribute_count:- " & mC.AttributeCount.ToString)
                                If mC.AttributeCount > 0 Then
                                    Dim infoNames() As String, infoValues() As UInteger
                                    If CInt(cudaInf.DriverVersion) >= 4 Then
                                        infoNames = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute2))
                                        infoValues = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute2)), UInteger())
                                    Else
                                        infoNames = [Enum].GetNames(GetType(myCuda.CUDeviceAttribute1))
                                        infoValues = CType([Enum].GetValues(GetType(myCuda.CUDeviceAttribute1)), UInteger())
                                    End If
                                    Dim cP() As cudaInfo.scudaDevice.sProperty = CType(mC.DeviceAttributes, Main.cudaInfo.scudaDevice.sProperty())
                                    For Each dA In cP
                                        sR.AppendLine(dA.Name & ":- " & CStr(dA.Value))
                                    Next
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                    el(sR)
                    If Not calInfo.Runtime = "" Then
                        sR.AppendLine("CAL Runtime:- " & calInf.Runtime)
                    End If
                    If calInf.calError <> "" And calInf.DeviceCount = 0 Then
                        sR.AppendLine("++CAL_ERROR:- " & calInf.calError)
                    End If
                    If calInf.DeviceCount > 0 Then
                        Dim cD As ArrayList = calInf.Devices
                        For Each cDev As scalInfo.scalDevice In cD
                            sR.AppendLine("Ordinal:- " & cDev.Ordinal.ToString)
                            sR.AppendLine("Device_info_maxResource1DWidth:- " & cDev.DeviceInfo.maxResource1DWidth.ToString)
                            sR.AppendLine("Device_info_maxResource2DHeight :- " & cDev.DeviceInfo.maxResource2DHeight.ToString)
                            sR.AppendLine("Device_info_maxResource2DWidth:- " & cDev.DeviceInfo.maxResource2DWidth.ToString)
                            sR.AppendLine("Target:- " & cDev.DeviceInfo.target.ToString)

                            If cDev.AttributeStructure = scalInfo.scalDevice.eAttributeStructure.default Then
                                Dim cDevAt As myCAL.CALdeviceattribs = cDev.DeviceAttributes
                                sR.AppendLine("-b3dProgramGrid:- " & cDevAt.b3dProgramGrid.ToString)
                                sR.AppendLine("-bUAVMemExport:- " & cDevAt.bUAVMemExport.ToString)
                                sR.AppendLine("-cachedRemoteram:- " & cDevAt.cachedRemoteRAM.ToString)
                                sR.AppendLine("-computeShader:- " & cDevAt.computeShader.ToString)
                                sR.AppendLine("-doublePrecision:- " & cDevAt.doublePrecision.ToString)
                                sR.AppendLine("-engineClock:- " & cDevAt.engineClock.ToString)
                                sR.AppendLine("-globalDataShare:- " & cDevAt.globalDataShare.ToString)
                                sR.AppendLine("-globalGPR:- " & cDevAt.globalGPR.ToString)
                                sR.AppendLine("-localDataShare:- " & cDevAt.localDataShare.ToString)
                                sR.AppendLine("-localRam:- " & cDevAt.localRAM.ToString)
                                sR.AppendLine("-localMemExport:- " & cDevAt.memExport.ToString)
                                sR.AppendLine("-localMemoryClock:- " & cDevAt.memoryClock.ToString)
                                sR.AppendLine("-numberOfShaderEngines:- " & cDevAt.numberOfShaderEngines.ToString)
                                sR.AppendLine("-numberOfSIMD:- " & cDevAt.numberOfSIMD.ToString)
                                sR.AppendLine("-numberOfUAVs:- " & cDevAt.numberOfUAVs.ToString)
                                sR.AppendLine("-pitch_alignment:- " & cDevAt.pitch_alignment.ToString)
                                sR.AppendLine("-surface_alignment:- " & cDevAt.surface_alignment.ToString)
                                sR.AppendLine("-target:- " & cDevAt.target.ToString)
                                sR.AppendLine("-targetRevision:- " & cDevAt.targetRevision.ToString)
                                sR.AppendLine("-uncachedRemoteRAM:- " & cDevAt.uncachedRemoteRAM.ToString)
                                sR.AppendLine("-wavefrontSize:- " & cDevAt.wavefrontSize.ToString)
                                el(sR)
                            Else
                                Dim cDevAt As myCAL.CALdeviceattribs_2 = cDev.DeviceAttributes2
                                '.AppendLine("b3dProgramGrid:- " & cDevAt.b3dProgramGrid.ToString )
                                '.AppendLine("bUAVMemExport:- " & cDevAt.bUAVMemExport.ToString )
                                sR.AppendLine("-cachedRemoteram:- " & cDevAt.cachedRemoteRAM.ToString)
                                sR.AppendLine("-computeShader:- " & cDevAt.computeShader.ToString)
                                sR.AppendLine("-doublePrecision:- " & cDevAt.doublePrecision.ToString)
                                sR.AppendLine("-engineClock:- " & cDevAt.engineClock.ToString)
                                sR.AppendLine("-globalDataShare:- " & cDevAt.globalDataShare.ToString)
                                sR.AppendLine("-globalGPR:- " & cDevAt.globalGPR.ToString)
                                sR.AppendLine("-localDataShare:- " & cDevAt.localDataShare.ToString)
                                sR.AppendLine("-localRam:- " & cDevAt.localRAM.ToString)
                                sR.AppendLine("-localMemExport:- " & cDevAt.memExport.ToString)
                                sR.AppendLine("-localMemoryClock:- " & cDevAt.memoryClock.ToString)
                                'sR.AppendLine("numberOfShaderEngines:- " & cDevAt.numberOfShaderEngines.ToString )
                                sR.AppendLine("-numberOfSIMD:- " & cDevAt.numberOfSIMD.ToString)
                                '.AppendLine("numberOfUAVs:- " & cDevAt.numberOfUAVs.ToString )
                                sR.AppendLine("-pitch_alignment:- " & cDevAt.pitch_alignment.ToString)
                                sR.AppendLine("-surface_alignment:- " & cDevAt.surface_alignment.ToString)
                                sR.AppendLine("-target:- " & cDevAt.target.ToString)
                                '.AppendLine("targetRevision:- " & cDevAt.targetRevision.ToString )
                                sR.AppendLine("-uncachedRemoteRAM:- " & cDevAt.uncachedRemoteRAM.ToString)
                                sR.AppendLine("-wavefrontSize:- " & cDevAt.wavefrontSize.ToString)
                                el(sR)
                            End If
                        Next
                    End If
                    Return sR.ToString
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return sR.ToString
                End Try
            End Function
            Private Sub el(ByVal sBuilder As StringBuilder)
                sBuilder.AppendLine("")
                Return
            End Sub
            Private calInf As New Main.scalInfo
            Private cudaInf As New Main.cudaInfo
            Private oclInf As New Main.oclInfo
            Public Function initGpuInfo(TheLumberJack As LumberJack.LumberJack) As Boolean
                Try
                    WriteLog("Gpgpu detection started")
                    Dim bCal As Boolean = calInf.Init(TheLumberJack)
                    Dim bCuda As Boolean = cudaInf.Init(TheLumberJack)
                    Dim bOcl As Boolean = oclInfo.Init(TheLumberJack)
                    If Not bOcl Then bOcl = oclInfo.Init(TheLumberJack)
                    If Not bCuda Then bCuda = cudaInf.Init(TheLumberJack)
                    'If bOcl Then WriteLog("openCL detected!")
                    'If bCuda Then WriteLog("CUDA detected!")
                    'If bCal Then WriteLog("CAL detected!")
                    Return bCal Or bCuda Or bOcl
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
            Public ReadOnly Property cudaInfo As Main.cudaInfo
                Get
                    Return cudaInf
                End Get
            End Property
            Public ReadOnly Property oclInfo As Main.oclInfo
                Get
                    Return oclInf
                End Get
            End Property
            Public ReadOnly Property calInfo As Main.scalInfo
                Get
                    Return calInf
                End Get
            End Property
            Public ReadOnly Property oclPlatform_count As Int32
                Get
                    Return oclInfo.NumberOfPlatforms
                End Get
            End Property
            Public ReadOnly Property oclPlatform(ByVal Index As Int32) As Main.oclInfo.sopenclPlatform
                Get
                    Return oclInf.Platform(Index)
                End Get
            End Property
            Public ReadOnly Property oclPlatform_DeviceCount(ByVal Platform As Int32) As Int32
                Get
                    If Platform > oclInf.NumberOfPlatforms - 1 Then Return 0
                    Return oclInf.Platform(Platform).NumberOfDevices
                End Get
            End Property
            Public ReadOnly Property oclDevicesTotal As Int32
                Get
                    Dim iTotal As Int32 = 0
                    For xint As Int32 = 0 To oclInfo.NumberOfPlatforms - 1
                        iTotal += oclPlatform(xint).NumberOfDevices
                    Next
                    Return iTotal
                End Get
            End Property
            Public ReadOnly Property oclDevice(ByVal Platform As Int32, ByVal Index As Int32) As Main.oclInfo.sopenclPlatform.sopenCLDevice
                Get
                    If oclInf.NumberOfPlatforms = 0 Then Return Nothing
                    If Platform > oclInf.NumberOfPlatforms - 1 Then Return Nothing
                    With oclInfo.Platform(Platform)
                        If .NumberOfDevices = 0 Then Return Nothing
                        If Index > .NumberOfDevices - 1 Then Return Nothing
                        Return .Device_ByIndex(Index)
                    End With
                End Get
            End Property
            Public ReadOnly Property cuDevice(ByVal Index As Short) As cudaInfo.scudaDevice
                Get
                    Return cudaInf.cudaDevice(Index)
                End Get
            End Property
            Public ReadOnly Property CudaDeviceCount As Int32
                Get
                    Return cudaInf.DeviceCount
                End Get
            End Property
            Public ReadOnly Property calDeviceCount As Int32
                Get
                    Return calInf.DeviceCount
                End Get
            End Property
            Public ReadOnly Property calDevice(ByVal Index As Int32) As scalInfo.scalDevice
                Get
                    Return CType(calInf.Devices(Index), scalInfo.scalDevice)
                End Get
            End Property
#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                    End If

                    ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    ' TODO: set large fields to null.
                End If
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class
        Public WithEvents gpuInf As New cGInfo
#End Region
#Region "OHM interface"
        Public Class cOHMInterface
            Implements IDisposable
#Region "NVAPI"
            Public Class ohmNVAPI
                Public Class clsAdapter
                    Public Property Name As String
                    Public Property Index As Short
                    Public Property Driver_Version As String
                    Public Property Driver_Branch As String
                    Public Property Info As String
                    Public Property DeviceID As String
                End Class
                Public Adapter As New List(Of clsAdapter)
                Public Property NumberOfAdapters As Short
                Public Property NVAPI_Version As String
                Public ReadOnly Property Report As String
                    Get
                        Dim sb As New StringBuilder
                        Try
                            sb.AppendLine("NVAPI" & Environment.NewLine & Environment.NewLine & "Version: " & Me.NVAPI_Version & Environment.NewLine & Environment.NewLine & "Number of GPUs: " & Me.NumberOfAdapters.ToString & Environment.NewLine & Environment.NewLine)
                            For Index = 0 To Me.NumberOfAdapters - 1
                                sb.AppendLine(Me.Adapter(Index).Info & Environment.NewLine & Environment.NewLine)
                            Next
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        Return sb.ToString
                    End Get
                End Property
                Public ReadOnly Property AdapterReport(Index As Short) As String
                    Get
                        Dim sB As StringBuilder = New StringBuilder("NVAPI" & Environment.NewLine & Environment.NewLine & "Version: " & Me.NVAPI_Version & Environment.NewLine & Environment.NewLine & "Number of GPUs: " & Me.NumberOfAdapters.ToString & Environment.NewLine & Environment.NewLine & "Apdapter report (" & Index & ")" & Environment.NewLine & Environment.NewLine)
                        With Adapter(Index)
                            sB.AppendLine(.Info)
                            Return sB.ToString
                        End With
                    End Get
                End Property
#Region "Log extender"
                Public Event Log(ByVal Message As String)
                Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
                Private Sub WriteLog(ByVal Message As String)
                    RaiseEvent Log(Message)
                End Sub
                Private Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                    RaiseEvent LogError(Message, EObj)
                End Sub
#End Region
            End Class
            Private WithEvents _NVAPI As ohmNVAPI
            Public ReadOnly Property NVAPI As ohmNVAPI
                Get
                    Return _NVAPI
                End Get
            End Property
#End Region
#Region "ADL"
            Public Class ohmADL
                Public Class clsAdapter
                    Public Property AdapterIndex As Short
                    Public Property IsActive As Boolean
                    Public Property AdapterName As String = ""
                    Public Property UDID As String
                    Public Property Present As Boolean
                    Public Property VendorID As String = ""
                    Public Property DeviceID As String = ""
                    Public Property BusNumber As Short
                    Public Property DeviceNumber As Short
                    Public Property FunctionNumber As Short
                    Public Property AdapterID As String = ""
                End Class
                Public Adapter As New List(Of clsAdapter)
                Public Property NumberOfAdapters As Short
                Public Property Status As String = ""
                Public ReadOnly Property Report As String
                    Get
                        Dim sb As New StringBuilder
                        Try
                            sb.AppendLine("Ati Display Library" & Environment.NewLine & Environment.NewLine & "Status: " & Me.Status & Environment.NewLine & Environment.NewLine & "Number of GPUs: " & Me.NumberOfAdapters.ToString & Environment.NewLine & "Driver Version: " & Me.DriverString & Environment.NewLine & Environment.NewLine)
                            For Index = 0 To Me.NumberOfAdapters - 1
                                sb.AppendLine("Adapter report (" & Index & ")" & Environment.NewLine & Environment.NewLine)
                                With Adapter(Index)
                                    sb.AppendLine("Present: " & .Present.ToString)
                                    sb.AppendLine("Active: " & .IsActive.ToString)
                                    sb.AppendLine("AdapterID: " & .AdapterID)
                                    sb.AppendLine("AdapterIndex: " & .AdapterIndex.ToString)
                                    sb.AppendLine("AdapterName: " & .AdapterName)
                                    sb.AppendLine("Bus: " & .BusNumber.ToString)
                                    sb.AppendLine("DeviceID: " & .DeviceID)
                                    sb.AppendLine("Device Number: " & .DeviceNumber.ToString)
                                    sb.AppendLine("Function Number: " & .FunctionNumber.ToString)
                                    sb.AppendLine("UDID: " & .UDID)
                                    sb.AppendLine("VendorID: " & .VendorID)
                                    sb.AppendLine(Environment.NewLine & Environment.NewLine)
                                End With
                            Next
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        Return sb.ToString
                    End Get
                End Property
                Public Property DriverString As String = ""
                Public Property ReleaseString As String = ""
                Public ReadOnly Property DriverVersion As Version
                    Get
                        Return New Version(DriverString)
                    End Get
                End Property
                Public ReadOnly Property AdapterReport(Index As Short) As String
                    Get
                        Dim sB As StringBuilder = New StringBuilder("Ati Display Library" & Environment.NewLine & Environment.NewLine & "Status: " & Me.Status & Environment.NewLine & Environment.NewLine & "Number of GPUs: " & Me.NumberOfAdapters.ToString & Environment.NewLine & "Driver Version: " & Me.DriverString & Environment.NewLine & Environment.NewLine & "Apdapter report (" & Index & ")" & Environment.NewLine & Environment.NewLine)
                        With Adapter(Index)
                            sB.AppendLine("Present: " & .Present.ToString)
                            sB.AppendLine("Active: " & .IsActive.ToString)
                            sB.AppendLine("AdapterID: " & .AdapterID)
                            sB.AppendLine("AdapterIndex: " & .AdapterIndex.ToString)
                            sB.AppendLine("AdapterName: " & .AdapterName)
                            sB.AppendLine("Bus: " & .BusNumber.ToString)
                            sB.AppendLine("DeviceID: " & .DeviceID)
                            sB.AppendLine("Device Number: " & .DeviceNumber.ToString)
                            sB.AppendLine("Function Number: " & .FunctionNumber.ToString)
                            sB.AppendLine("UDID: " & .UDID)
                            sB.AppendLine("VendorID: " & .VendorID)
                            Return sB.ToString
                        End With
                    End Get
                End Property
#Region "Log extender"
                Public Event Log(ByVal Message As String)
                Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
                Private Sub WriteLog(ByVal Message As String)
                    RaiseEvent Log(Message)
                End Sub
                Private Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                    RaiseEvent LogError(Message, EObj)
                End Sub
#End Region
            End Class
            Private WithEvents _ADL As ohmADL
            Public ReadOnly Property ADL As ohmADL
                Get
                    Return _ADL
                End Get
            End Property
#End Region
#Region "Sensors"
            Public Event SensorUpdate(Sensor As ohmSensors)
            Public Class ohmSensors
                Public Name As String
                Public Enum eSensorType
                    Voltage
                    Clock
                    Temperature
                    Control
                    Fan
                    Load
                End Enum
                Public SensorType As eSensorType
                Public Identifier As String
                Public MaxValue As Double
                Public MinValue As Double
                Public CurrentValue As Double
                Public EventTime As DateTime
                Public Sensor As ISensor
            End Class
#Region "Timer based updates"
            Public Property CancelUpdates As Boolean = False 'Set cancelupdates to true before exiting!
            Private iVisitor As OpenHardwareMonitor.Hardware.IVisitor
            Private iHandler As New SensorEventHandler(AddressOf SensorEvenentHandler)
            Private WithEvents autoTimer As New System.Timers.Timer
            Public ReadOnly Property IsUpdating As Boolean
                Get
                    Return autoTimer.Enabled
                End Get
            End Property
            Public Function AutoUpdate(ByVal Interval As Integer) As Boolean
                Try
                    If IsNothing(autoTimer) Then autoTimer = New System.Timers.Timer
                    If Interval = 0 Then
                        WriteLog("Setting hardware update timer to 0 ( disabeld ).")
                        CancelUpdates = True
                        autoTimer.Enabled = False
                        autoTimer.Dispose()
                        autoTimer = Nothing
                        Return True
                    End If
                    autoTimer.AutoReset = True
                    autoTimer.Interval = Interval
                    WriteLog("Setting hardware update timer to " & Interval.ToString & "ms.")
                    autoTimer.Enabled = True
                    CancelUpdates = False
                    Return True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
            Private Sub autoTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles autoTimer.Elapsed
                Try
                    If CancelUpdates Then Exit Sub
                    Dim nInv As New delUpdate(AddressOf doUpdate)
                    nInv.Invoke()
                    nInv = Nothing
                Catch ex As Exception
                    WriteError("ucHWM_AutotimerElapsed", Err)
                End Try
            End Sub
            Private Delegate Sub delUpdate()

            ' TODO Decide what path to take with sensor readings
#Region "Old"
            'Using commented out section results in a rampage of event's
            'New section should raise an update event per individual detected hardware
            Private Sub doUpdate()
                Try
                    If CancelUpdates Then Exit Sub
                    For Each ohmATI In _ohmAti
                        iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
                        ohmATI.Hardware.Update()
                        ohmATI.Hardware.Accept(iVisitor)
                    Next
                    For Each ohmNvidia In _ohmNvidia
                        ohmNvidia.Hardware.Update()
                        iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
                        ohmNvidia.Hardware.Accept(iVisitor)
                    Next
                    For Each ohmCPU In _ohmCpu
                        ohmCPU.Hardware.Update()
                        iVisitor = New OpenHardwareMonitor.Hardware.SensorVisitor(iHandler)
                        ohmCPU.Hardware.Accept(iVisitor)
                    Next
                Catch ex As Exception
                    WriteError("ucHWM_doUpdate", Err)
                End Try

            End Sub
            Public Sub SensorEvenentHandler(ByVal Sensor As ISensor)
                Try
                    If CancelUpdates Then Exit Sub
                    'Console.WriteLine("---- " & Sensor.Name & " - " & Sensor.Value)
                    Select Case Sensor.Hardware.HardwareType
                        Case Is = HardwareType.CPU
                            'find HW
                            For Each ohmCPU In _ohmCpu
                                If ohmCPU.Hardware.Identifier = Sensor.Hardware.Identifier Then
                                    'Find sensor
                                    For Each ohmSensor As ohmSensors In ohmCPU.Sensors
                                        If ohmSensor.Identifier = Sensor.Identifier.ToString Then
                                            'If Not (ohmSensor.MaxValue = Sensor.Max And ohmSensor.MinValue = Sensor.Min And ohmSensor.CurrentValue = Sensor.Value) Then
                                            'RaiseEvent
                                            If Not IsNothing(Sensor.Min) Then ohmSensor.MinValue = CDbl(Sensor.Min)
                                            If Not IsNothing(Sensor.Max) Then ohmSensor.MaxValue = CDbl(Sensor.Max)
                                            If Not IsNothing(Sensor.Value) Then ohmSensor.CurrentValue = CDbl(Sensor.Value)
                                            ohmSensor.EventTime = DateTime.Now
                                            RaiseEvent SensorUpdate(ohmSensor)
                                            'End If
                                            Return
                                        End If
                                    Next
                                End If
                            Next
                        Case Is = HardwareType.GpuAti
                            For Each ohmAti In _ohmAti
                                If ohmAti.Hardware.Identifier = Sensor.Hardware.Identifier Then
                                    'Find sensor
                                    For Each ohmSensor As ohmSensors In ohmAti.Sensors
                                        If ohmSensor.Identifier = Sensor.Identifier.ToString Then
                                            'If Not (ohmSensor.MaxValue = Sensor.Max And ohmSensor.MinValue = Sensor.Min And ohmSensor.CurrentValue = Sensor.Value) Then
                                            'RaiseEvent
                                            If Not IsNothing(Sensor.Min) Then ohmSensor.MinValue = CDbl(Sensor.Min)
                                            If Not IsNothing(Sensor.Max) Then ohmSensor.MaxValue = CDbl(Sensor.Max)
                                            If IsNothing(Sensor.Value) Then
                                                If Not IsNothing(Sensor.Max) And Not IsNothing(Sensor.Min) Then
                                                    ohmSensor.CurrentValue = Math.Round(CDbl(Sensor.Min) + CDbl(Sensor.Max), 2)
                                                Else
                                                    ohmSensor.CurrentValue = -1
                                                End If
                                            Else
                                                ohmSensor.CurrentValue = CDbl(Sensor.Value)
                                            End If
                                            ohmSensor.EventTime = DateTime.Now
                                            RaiseEvent SensorUpdate(ohmSensor)
                                        End If
                                        Return
                                        'End If
                                    Next
                                End If
                            Next
                        Case Is = HardwareType.GpuNvidia
                            For Each ohmNvidia In _ohmNvidia
                                If ohmNvidia.Hardware.Identifier = Sensor.Hardware.Identifier Then
                                    'Find sensor
                                    For Each ohmSensor As ohmSensors In ohmNvidia.Sensors
                                        If ohmSensor.Identifier = Sensor.Identifier.ToString Then
                                            'If Not (ohmSensor.MaxValue = Sensor.Max And ohmSensor.MinValue = Sensor.Min And ohmSensor.CurrentValue = Sensor.Value) Then
                                            'RaiseEvent
                                            If Not IsNothing(Sensor.Min) Then ohmSensor.MinValue = CDbl(Sensor.Min)
                                            If Not IsNothing(Sensor.Max) Then ohmSensor.MaxValue = CDbl(Sensor.Max)
                                            If Not IsNothing(Sensor.Value) Then ohmSensor.CurrentValue = CDbl(Sensor.Value)
                                            ohmSensor.EventTime = DateTime.Now
                                            RaiseEvent SensorUpdate(ohmSensor)
                                            'End If
                                            Return
                                        End If
                                    Next
                                End If
                            Next
                    End Select

                Catch ex As Exception
                    WriteError("ucHWM_SensorEventHandler", Err)
                End Try
            End Sub

#End Region
#End Region
#End Region
#Region "CPU"
            Public Class ohmCPU
                Public Property CpuName As String
                Public Property Identifier As String
                Public Report As String
                Public Property CoreCount As String
                Public Property ThreadsPerCore As String
                Public Property MicroArchitecture As String
                Public Property Clockspeed As String
                Public Property Multiplier As String
                Public Property BusSpeed As String
                Public Sensors As New List(Of ohmSensors)
                Public Hardware As IHardware
            End Class
            Private _ohmCpu As New List(Of ohmCPU)
            Public Overloads ReadOnly Property CPU(Identifier As String) As ohmCPU
                Get
                    For Each ocpu As ohmCPU In _ohmCpu
                        If ocpu.Identifier = Identifier Then
                            Return ocpu
                            Exit Property
                        End If
                    Next
                    Return Nothing
                End Get
            End Property
            Public ReadOnly Property CpuCheck(Identifier As String) As Boolean
                Get
                    For Each oCpu As ohmCPU In _ohmCpu
                        If oCpu.Identifier = Identifier Then Return True
                    Next
                    Return False
                End Get
            End Property
            Public Overloads ReadOnly Property CPU(Index As Int32) As ohmCPU
                Get
                    If Index <= _ohmCpu.Count - 1 Then Return _ohmCpu(Index)
                    Return Nothing
                End Get
            End Property
#End Region
#Region "ATI GPU"
            Public Class ohmATI
                Public Driver As String
                Public Report As String
                Public Identifier As String
                Public Name As String
                Public Vendor As String
                Public pciID As String
                Public DeviceID As String
                Public Slot As Short
                Public Sensors As New List(Of ohmSensors)
                Public Hardware As IHardware
            End Class
            Private _ohmAti As New List(Of ohmATI)
            Public Overloads Property Ati(Index As Int32) As ohmATI
                Get
                    If Index <= _ohmAti.Count - 1 Then Return _ohmAti(Index)
                    Return Nothing
                End Get
                Set(value As ohmATI)
                    If _ohmAti.Count - 1 < Index Then
                        _ohmAti(Index) = value
                    Else
                        _ohmAti.Add(value)
                    End If
                End Set
            End Property
            Public Overloads ReadOnly Property AtiCheck(Identifier As String) As Boolean
                Get
                    For Each oati As ohmATI In _ohmAti
                        If oati.Identifier = Identifier Then Return True
                    Next
                    Return False
                End Get
            End Property
            Public Overloads ReadOnly Property Ati(Identifier As String) As ohmATI
                Get
                    For Each oati As ohmATI In _ohmAti
                        If oati.Identifier = Identifier Then
                            Return oati
                            Exit Property
                        End If
                    Next
                    Return Nothing
                End Get
            End Property
            Public ReadOnly Property ATICount As Int32
                Get
                    Return _ohmAti.Count
                End Get
            End Property
            Public ReadOnly Property AtiGpu(ByVal Index As Int32) As IHardware
                Get
                    If Index > _ohmAti.Count - 1 Or _ohmAti.Count = 0 Then Return Nothing
                    Return _ohmAti(Index).Hardware
                End Get
            End Property
#End Region
#Region "NVIDIA GPU"
            Public Class ohmNvidia
                Public Driver As String
                Public Report As String
                Public Identifier As String
                Public Name As String
                Public Vendor As String
                Public pciID As String
                Public ReadOnly Property DeviceID As String
                    Get
                        Try
                            Return pciID.Substring(pciID.ToUpper.IndexOf("DEV_") + 4, 4)
                        Catch ex As Exception
                            WriteError("Could not get deviceID from string: " & pciID, Err)
                            Return ""
                        End Try
                    End Get
                End Property
                Public Slot As Short
                Public Sensors As New List(Of ohmSensors)
                Public Hardware As IHardware
#Region "Log extender"
                Public Event Log(ByVal Message As String)
                Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
                Private Sub WriteLog(ByVal Message As String)
                    RaiseEvent Log(Message)
                End Sub
                Private Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                    RaiseEvent LogError(Message, EObj)
                End Sub
#End Region
            End Class
            Private WithEvents _ohmNvidia As New List(Of ohmNvidia)
            Public Overloads Property Nvidia(Index As Int32) As ohmNvidia
                Get
                    If _ohmNvidia.Count - 1 <= Index Then
                        Return _ohmNvidia(Index)
                    Else
                        Return Nothing
                    End If
                End Get
                Set(value As ohmNvidia)
                    If Index > _ohmNvidia.Count - 1 Then
                        _ohmNvidia(Index) = value
                    Else
                        _ohmNvidia.Add(value)
                    End If
                End Set
            End Property
            Public Overloads ReadOnly Property Nvidia(Identifier As String) As ohmNvidia
                Get
                    For Each nNvidia As ohmNvidia In _ohmNvidia
                        If nNvidia.Identifier = Identifier Then
                            Return nNvidia
                            Exit Property
                        End If
                    Next
                    Return Nothing
                End Get
            End Property
            Public Overloads ReadOnly Property NvidiaCheck(Identifier As String) As Boolean
                Get
                    For Each nNvidia As ohmNvidia In _ohmNvidia
                        If nNvidia.Identifier = Identifier Then Return True
                    Next
                    Return False
                End Get
            End Property
            Public ReadOnly Property NVIDIACount As Int32
                Get
                    Return _ohmNvidia.Count
                End Get
            End Property
            Public ReadOnly Property NvidiaGpu(ByVal Index As Int32) As IHardware
                Get
                    If Index > _ohmNvidia.Count - 1 Or _ohmNvidia.Count = 0 Then Return Nothing
                    Return _ohmNvidia(Index).Hardware
                End Get
            End Property
#End Region
#Region "OHM"
            Private _ohmReport As String
            Public ReadOnly Property OHMReport As String
                Get
                    Return _ohmReport
                End Get
            End Property
            Private myOHM As New OpenHardwareMonitor.Hardware.Computer
            Private Enum eOhmOpen
                Closed
                Open
            End Enum
            Private OhmOpen As eOhmOpen = eOhmOpen.Closed
            Public ReadOnly Property IsOpen As Boolean
                Get
                    Return OhmOpen = eOhmOpen.Open
                End Get
            End Property
            Public Function Close() As Boolean
                If OhmOpen = eOhmOpen.Open Then CloseOhm()
                Return OhmOpen = eOhmOpen.Closed
            End Function
            Private Sub OpenOhm()
                myOHM.Open()
                OhmOpen = eOhmOpen.Open
            End Sub
            Private Sub CloseOhm()
                myOHM.Close()
                OhmOpen = eOhmOpen.Closed
            End Sub
            Public Function Init(Optional CloseOHM As Boolean = True, Optional TheLumberJack As LumberJack.LumberJack = Nothing) As Boolean
                ' On Error Resume Next
                If Not IsNothing(TheLumberJack) Then modLumberJack.LumberJack = TheLumberJack
                Try
                    WriteLog("OpenHardwareMonitorLib interface started")
                    If Not IsOpen Then OpenOhm()
                    _ohmReport = myOHM.GetReport()
#If CONFIG = "Debug" Then
                    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\ohmfail.txt") Then
                        '_ohmReport = My.Computer.FileSystem.ReadAllText(My.Application.Info.DirectoryPath & "\ohmfail.txt")
                    End If
#End If
                    Dim lstNV As New List(Of sNVRegistry)
                    lstNV = Me.NVIDsFromRegistry
                    If _ohmReport.Contains(vbCrLf & "NVAPI" & vbCrLf) Then
                        _NVAPI = New ohmNVAPI
                        AddHandler _NVAPI.Log, AddressOf WriteLog
                        AddHandler _NVAPI.LogError, AddressOf WriteError
                        Dim nGpu As New ohmNVAPI.clsAdapter
                        Dim iNVstart As Int32 = _ohmReport.IndexOf("NVAPI"), iNVGPU As Int32 = _ohmReport.IndexOf("--------------------------------------------------------------------------------" & vbCrLf, iNVstart), InfoText As String = _ohmReport.Substring(iNVstart, iNVGPU - iNVstart)
                        _NVAPI.NVAPI_Version = InfoText.Substring(InfoText.IndexOf("Version: ") + 9, -(InfoText.IndexOf("Version: ") + 9) + InfoText.IndexOf(vbCrLf, InfoText.IndexOf("Version: ")))
                        _NVAPI.NumberOfAdapters = CShort(InfoText.Substring(InfoText.IndexOf("Number of GPUs: ") + Len("Number of GPUs: ")).Trim)
                        Dim Text As String = _ohmReport.Substring(_ohmReport.IndexOf(InfoText) + InfoText.Length)
                        For xShort As Short = 1 To _NVAPI.NumberOfAdapters
                            nGpu = New ohmNVAPI.clsAdapter
                            nGpu.DeviceID = CStr(lstNV(xShort - 1).DeviceID)
                            Dim GpuText As String
                            If xShort = _NVAPI.NumberOfAdapters Then
                                ' All text is info text
                                nGpu.Info = (Text & Environment.NewLine & Environment.NewLine & "DeviceID: " & nGpu.DeviceID).Trim
                            Else
                                GpuText = Text.Substring("--------------------------------------------------------------------------------".Length, Text.IndexOf("--------------------------------------------------------------------------------", 1) - "--------------------------------------------------------------------------------".Length)
                                Text = Text.Replace(GpuText, "").Substring("----------------------------------------------------------------------------------------------------------------------------------------------------------------".Length)
                                GpuText = GpuText.Trim
                                Text = Text.Trim
                                GpuText &= Environment.NewLine & Environment.NewLine & "DeviceID: " & nGpu.DeviceID
                                nGpu.Info = GpuText
                            End If
                            nGpu.Name = nGpu.Info.Substring(nGpu.Info.IndexOf("Name: ") + "Name: ".Length, -(nGpu.Info.IndexOf("Name: ") + "Name: ".Length) + nGpu.Info.IndexOf(vbCrLf, nGpu.Info.IndexOf("Name: ") + "Name: ".Length)).Replace("Name: ", "").Trim
                            nGpu.Index = CShort(nGpu.Info.Substring(nGpu.Info.IndexOf("Index: ") + "Index: ".Length, -(nGpu.Info.IndexOf("Index: ") + "Index: ".Length) + nGpu.Info.IndexOf(vbCrLf, nGpu.Info.IndexOf("Index: ") + "Index: ".Length)).Replace("Index: ", "").Trim)
                            nGpu.Driver_Version = nGpu.Info.Substring(nGpu.Info.IndexOf("Driver Version: ") + "Driver Version: ".Length, -(nGpu.Info.IndexOf("Driver Version: ") + "Driver Version: ".Length) + nGpu.Info.IndexOf(vbCrLf, nGpu.Info.IndexOf("Driver Version: ") + "Driver Version: ".Length)).Replace("Driver Version: ", "").Trim
                            nGpu.Driver_Branch = nGpu.Info.Substring(nGpu.Info.IndexOf("Driver Branch: ") + "Driver Branch: ".Length, -(nGpu.Info.IndexOf("Driver Branch: ") + "Driver Branch: ".Length) + nGpu.Info.IndexOf(vbCrLf, nGpu.Info.IndexOf("Driver Branch: ") + "Driver Branch: ".Length)).Replace("Driver Branch: ", "").Trim
                            _NVAPI.Adapter.Add(nGpu)
                            'If xShort < _NVAPI.NumberOfAdapters Then Text = Text.Substring(Text.IndexOf("--------------------------------------------------------------------------------"))
                        Next
                    End If
                    If _ohmReport.Contains("AMD Display Library") And Not _ohmReport.Contains("AMD Display Library" & Environment.NewLine & Environment.NewLine & "Status: -1") Then
                        _ADL = New ohmADL
                        AddHandler _ADL.Log, AddressOf WriteLog
                        AddHandler _ADL.LogError, AddressOf WriteError
                        ' TODO Replace registry with AGS get version?
                        _ADL.DriverString = AtiDriverStringWMI()
                        '_ADL.DriverString = DriverStringRegistry("", "1002", "", "")
                        '_ADL.ReleaseString = DriverStringRegistry("", "1002", "", "ReleaseVersion")
                        Dim aGpu As ohmADL.clsAdapter = Nothing, bExit As Boolean = False
                        Dim Text As String = String.Empty
                        Try 'Make this prettier!!!
                            Text = _ohmReport.Substring(_ohmReport.IndexOf("AMD Display Library"))
                        Catch ex As Exception
                            GoTo DoOHM
                        End Try
                        If InStr(_ohmReport.IndexOf("AMD Display Library"), _ohmReport, "--------------------------------------------------------------------------------") <> -1 Then
                            Text = Text.Substring(0, Text.IndexOf("--------------------------------------------------------------------------------"))
                        End If
                        Do
                            If Text.IndexOf(vbCrLf) = 0 Then
                                Text = Text.Substring(1)
                            ElseIf Text.IndexOf(vbCrLf) = -1 Then
                                If Text.Contains("Status: ") Then
                                    _ADL.Status = Text.Replace("Status: ", "")
                                ElseIf Text.Contains("Number of adapters: ") Then
                                    _ADL.NumberOfAdapters = CShort(Text.Replace("Number of adapters: ", ""))
                                ElseIf Text.Contains("AdapterIndex: ") Then
                                    If IsNothing(aGpu) Then
                                        aGpu = New ohmADL.clsAdapter
                                    Else
                                        _ADL.Adapter.Add(aGpu)
                                        aGpu = New ohmADL.clsAdapter
                                    End If
                                    aGpu.AdapterIndex = CShort(Text.Replace("AdapterIndex: ", ""))
                                ElseIf Text.Contains("isActive: ") Then
                                    aGpu.IsActive = Text.Replace("isActive: ", "") = "1"
                                ElseIf Text.Contains("AdapterName: ") Then
                                    aGpu.AdapterName = Text.Trim
                                ElseIf Text.Contains("UDID: ") Then
                                    aGpu.UDID = Text.Trim
                                    aGpu.VendorID = "0x" & aGpu.UDID.Substring(aGpu.UDID.IndexOf("VEN_") + 4, aGpu.UDID.IndexOf("&") - (aGpu.UDID.IndexOf("VEN_") + 4))
                                    aGpu.DeviceID = "0x" & aGpu.UDID.Substring(aGpu.UDID.IndexOf("DEV_") + 4, aGpu.UDID.IndexOf("&", aGpu.UDID.IndexOf("DEV_") + 4) - (aGpu.UDID.IndexOf("DEV_") + 4))
                                ElseIf Text.Contains("Present: ") Then
                                    aGpu.Present = Text.Replace("Present: ", "") = "1"
                                ElseIf Text.Contains("BusNumber: ") Then
                                    aGpu.BusNumber = CShort(Text.Replace("BusNumber: ", ""))
                                ElseIf Text.Contains("DeviceNumber: ") Then
                                    aGpu.DeviceNumber = CShort(Text.Replace("DeviceNumber: ", ""))
                                ElseIf Text.Contains("FunctionNumber: ") Then
                                    aGpu.FunctionNumber = CShort(Text.Replace("FunctionNumber: ", ""))
                                ElseIf Text.Contains("AdapterID: ") Then
                                    aGpu.AdapterID = Text.Replace("AdapterID: ", "").Trim
                                End If
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("Status: ") Then
                                _ADL.Status = Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("Status: ", "")
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("Number of adapters: ") Then
                                _ADL.NumberOfAdapters = CShort(Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("Number of adapters: ", ""))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("AdapterIndex: ") Then
                                If IsNothing(aGpu) Then
                                    aGpu = New ohmADL.clsAdapter
                                Else
                                    _ADL.Adapter.Add(aGpu)
                                    aGpu = New ohmADL.clsAdapter
                                End If
                                aGpu.AdapterIndex = CShort(Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("AdapterIndex: ", ""))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("isActive: ") Then
                                aGpu.IsActive = Text.Substring(0, Text.IndexOf(vbCrLf)).ToString.Replace("isActive: ", "") = "1"
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("AdapterName: ") Then
                                aGpu.AdapterName = Text.Substring(0, Text.IndexOf(vbCrLf)).Trim
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("UDID: ") Then
                                aGpu.UDID = Text.Substring(0, Text.IndexOf(vbCrLf)).Trim
                                aGpu.VendorID = "0x" & aGpu.UDID.Substring(aGpu.UDID.IndexOf("VEN_") + 4, aGpu.UDID.IndexOf("&") - (aGpu.UDID.IndexOf("VEN_") + 4))
                                aGpu.DeviceID = "0x" & aGpu.UDID.Substring(aGpu.UDID.IndexOf("DEV_") + 4, aGpu.UDID.IndexOf("&", aGpu.UDID.IndexOf("DEV_") + 4) - (aGpu.UDID.IndexOf("DEV_") + 4))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("Present: ") Then
                                aGpu.Present = Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("Present: ", "") = "1"
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("BusNumber: ") Then
                                aGpu.BusNumber = CShort(Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("BusNumber: ", ""))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("DeviceNumber: ") Then
                                aGpu.DeviceNumber = CShort(Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("DeviceNumber: ", ""))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("FunctionNumber: ") Then
                                aGpu.FunctionNumber = CShort(Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("FunctionNumber: ", ""))
                            ElseIf Text.Substring(0, Text.IndexOf(vbCrLf)).Contains("AdapterID: ") Then
                                aGpu.AdapterID = Text.Substring(0, Text.IndexOf(vbCrLf)).Replace("AdapterID: ", "").Trim
                            End If
                            If bExit Then Exit Do
                            If Text.IndexOf(vbCrLf) <> -1 Then
                                Text = Text.Substring(Text.IndexOf(vbCrLf) + 1).Trim
                            Else
                                bExit = True
                            End If
                        Loop
                        _ADL.Adapter.Add(aGpu)
                    End If
DoOHM:
                    For Each hw As OpenHardwareMonitor.Hardware.IHardware In myOHM.Hardware
                        If hw.HardwareType = OpenHardwareMonitor.Hardware.HardwareType.CPU Then
                            hw.Update()
                            Dim oCpu As New ohmCPU
                            oCpu.CpuName = hw.Name
                            oCpu.Identifier = hw.Identifier.ToString
                            oCpu.Report = hw.GetReport
                            Dim txtTmp As String = oCpu.Report.Substring(oCpu.Report.IndexOf("Number of Cores: "), oCpu.Report.IndexOf(Environment.NewLine, oCpu.Report.IndexOf("Number of Cores: ")) - oCpu.Report.IndexOf("Number of Cores: ")).Replace("Number of Cores: ", "")
                            oCpu.CoreCount = txtTmp
                            oCpu.ThreadsPerCore = CStr(oCpu.Report.Substring(oCpu.Report.IndexOf("Threads per Core: "), oCpu.Report.IndexOf(Environment.NewLine, oCpu.Report.IndexOf("Threads per Core: ")) - oCpu.Report.IndexOf("Threads per Core: ")).Replace("Threads per Core: ", ""))
                            oCpu.MicroArchitecture = oCpu.Report.Substring(oCpu.Report.IndexOf("Microarchitecture: "), oCpu.Report.IndexOf(Environment.NewLine, oCpu.Report.IndexOf("Microarchitecture: ")) - oCpu.Report.IndexOf("Microarchitecture: ")).Replace("Microarchitecture: ", "")
                            oCpu.Clockspeed = oCpu.Report.Substring(oCpu.Report.IndexOf("Time Stamp Counter Frequency: "), oCpu.Report.IndexOf(Environment.NewLine, oCpu.Report.IndexOf("Time Stamp Counter Frequency: ")) - oCpu.Report.IndexOf("Time Stamp Counter Frequency: ")).Replace("Time Stamp Counter Frequency: ", "")
                            oCpu.Multiplier = CStr(oCpu.Report.Substring(oCpu.Report.IndexOf("Time Stamp Counter Multiplier:"), oCpu.Report.IndexOf(Environment.NewLine, oCpu.Report.IndexOf("Time Stamp Counter Multiplier:")) - oCpu.Report.IndexOf("Time Stamp Counter Multiplier:")).Replace("Time Stamp Counter Multiplier:", ""))
                            oCpu.BusSpeed = CStr(CInt(oCpu.Clockspeed.Replace("MHz", "").Trim) / CDbl(oCpu.Multiplier))
                            oCpu.Hardware = hw
                            For Each oSensor As ISensor In hw.Sensors
                                Dim nSens As New ohmSensors
                                Select Case oSensor.SensorType
                                    Case Is = SensorType.Clock
                                        nSens.SensorType = ohmSensors.eSensorType.Clock
                                    Case Is = SensorType.Control
                                        nSens.SensorType = ohmSensors.eSensorType.Control
                                    Case Is = SensorType.Fan
                                        nSens.SensorType = ohmSensors.eSensorType.Fan
                                    Case Is = SensorType.Load
                                        nSens.SensorType = ohmSensors.eSensorType.Load
                                    Case Is = SensorType.Temperature
                                        nSens.SensorType = ohmSensors.eSensorType.Temperature
                                    Case Is = SensorType.Voltage
                                        nSens.SensorType = ohmSensors.eSensorType.Voltage
                                End Select
                                nSens.Name = oSensor.Name
                                nSens.Identifier = oSensor.Identifier.ToString
                                If Not IsNothing(oSensor.Max) Then
                                    nSens.MaxValue = CDbl(oSensor.Max)
                                Else
                                    nSens.MaxValue = -1
                                End If
                                If Not IsNothing(oSensor.Min) Then
                                    nSens.MinValue = CDbl(oSensor.Min)
                                Else
                                    nSens.MinValue = -1
                                End If
                                If Not IsNothing(oSensor.Value) Then
                                    nSens.CurrentValue = CDbl(oSensor.Value)
                                Else
                                    nSens.CurrentValue = -1
                                End If
                                nSens.Sensor = oSensor
                                ' TODO Check formating time string is needed or not?
                                nSens.EventTime = DateTime.Now
                                oCpu.Sensors.Add(nSens)
                            Next
                            _ohmCpu.Add(oCpu)
                            txtTmp = Nothing
                            'lOhmHW.Add(hw)
                        ElseIf hw.HardwareType = OpenHardwareMonitor.Hardware.HardwareType.GpuNvidia Then
                            hw.Update()
                            Dim NV As New ohmNvidia
                            NV.Name = hw.Name
                            NV.Vendor = "Nvidia"
                            NV.Identifier = hw.Identifier.ToString
                            NV.Report = hw.GetReport
                            ' TODO replace registry with nvapi
                            If lstNV.Count > 0 Then
                                If hw.Name = lstNV(_ohmNvidia.Count).DeviceName Then
                                    NV.pciID = CStr(lstNV(_ohmNvidia.Count).DeviceID)
                                End If
                            End If
                            NV.Hardware = hw
                            NV.Driver = NV.Report.Substring(NV.Report.IndexOf("Driver Version: ") + 16, NV.Report.IndexOf(vbCrLf, NV.Report.IndexOf("Driver Version: ")) - (NV.Report.IndexOf("Driver Version: ") + 16))
                            For Each oSensor As ISensor In hw.Sensors
                                Dim nSens As New ohmSensors
                                ' TODO match ohmlib enum values to loose select case 
                                Select Case oSensor.SensorType
                                    Case Is = SensorType.Clock
                                        nSens.SensorType = ohmSensors.eSensorType.Clock
                                    Case Is = SensorType.Control
                                        nSens.SensorType = ohmSensors.eSensorType.Control
                                    Case Is = SensorType.Fan
                                        nSens.SensorType = ohmSensors.eSensorType.Fan
                                    Case Is = SensorType.Load
                                        nSens.SensorType = ohmSensors.eSensorType.Load
                                    Case Is = SensorType.Temperature
                                        nSens.SensorType = ohmSensors.eSensorType.Temperature
                                    Case Is = SensorType.Voltage
                                        nSens.SensorType = ohmSensors.eSensorType.Voltage
                                End Select
                                nSens.Name = oSensor.Name
                                nSens.Identifier = oSensor.Identifier.ToString
                                nSens.MaxValue = CDbl(oSensor.Max)
                                nSens.MinValue = CDbl(oSensor.Min)
                                nSens.CurrentValue = CDbl(oSensor.Value)
                                nSens.Sensor = oSensor
                                ' TODO Check formating time string is needed or not?
                                nSens.EventTime = DateTime.Now
                                NV.Sensors.Add(nSens)
                            Next
                            If NV.pciID = "" AndAlso Not IsNothing(_ADL) Then
                                If _ADL.Status = "OK" Then
                                    Dim iohmAti As Int16 = CShort(NV.Hardware.Identifier.ToString)
                                    For Each Adapter In _ADL.Adapter
                                        If Adapter.AdapterIndex = iohmAti Then
                                            'match?
                                            NV.pciID = Adapter.UDID.Replace("UDID: ", "").Trim
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                            _ohmNvidia.Add(NV)
                        ElseIf hw.HardwareType = OpenHardwareMonitor.Hardware.HardwareType.GpuAti Then
                            hw.Update()
                            Dim nATI As New ohmATI
                            nATI.Name = hw.Name
                            nATI.Vendor = "ATI"
                            nATI.Identifier = hw.Identifier.ToString
                            For Each oSensor As ISensor In hw.Sensors
                                Dim nSens As New ohmSensors
                                Select Case oSensor.SensorType
                                    Case Is = SensorType.Clock
                                        nSens.SensorType = ohmSensors.eSensorType.Clock
                                    Case Is = SensorType.Control
                                        nSens.SensorType = ohmSensors.eSensorType.Control
                                    Case Is = SensorType.Fan
                                        nSens.SensorType = ohmSensors.eSensorType.Fan
                                    Case Is = SensorType.Load
                                        nSens.SensorType = ohmSensors.eSensorType.Load
                                    Case Is = SensorType.Temperature
                                        nSens.SensorType = ohmSensors.eSensorType.Temperature
                                    Case Is = SensorType.Voltage
                                        nSens.SensorType = ohmSensors.eSensorType.Voltage
                                End Select
                                nSens.Name = oSensor.Name
                                nSens.Identifier = oSensor.Identifier.ToString
                                nSens.MaxValue = CDbl(oSensor.Max)
                                nSens.MinValue = CDbl(oSensor.Min)
                                nSens.CurrentValue = CDbl(oSensor.Value)
                                nSens.Sensor = oSensor
                                ' TODO Check formating time string is needed or not?
                                nSens.EventTime = DateTime.Now
                                nATI.Sensors.Add(nSens)
                            Next
                            Dim sInd As Short = CShort(nATI.Identifier.ToString.Replace("/atigpu/", ""))
                            nATI.Report = ADL.AdapterReport(sInd)
                            nATI.Hardware = hw
                            If Not IsNothing(_ADL) Then
                                If _ADL.Status = "OK" Then
                                    Dim iohmAti As Int16 = CShort(nATI.Hardware.Identifier.ToString.Replace("/atigpu/", ""))
                                    For Each Adapter In _ADL.Adapter
                                        If Adapter.AdapterIndex = iohmAti Then
                                            'match?
                                            nATI.pciID = Adapter.UDID.Replace("UDID: ", "").Trim
                                            nATI.DeviceID = "0x" & nATI.pciID.Substring(nATI.pciID.IndexOf("DEV_") + 4, 4)
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                            _ohmAti.Add(nATI)
                        End If
                    Next
                    If _ohmCpu.Count > 0 Or _ohmAti.Count > 0 Or _ohmNvidia.Count > 0 Then
                        WriteLog("OpenHardwareMonitorLib succesfully reported hardware")
                        Return True
                    Else
                        WriteLog("OpenHardwareMonitorLib failed to find any hardware")
                        Return False
                    End If
                Catch ex As Exception
                    Dim nEx As New Exception(ex.Message & New String(CChar(Environment.NewLine), 2) & _ohmReport, ex)
                    WriteError(nEx.Message, Err, Me)
                    Return False
                Finally
                    If CloseOHM Then
                        Close()
                    End If
                End Try
            End Function
            Public ReadOnly Property CpuCount As Int32
                Get
                    Return _ohmCpu.Count
                End Get
            End Property
            Public ReadOnly Property GpuCount As Int32
                Get
                    Return _ohmAti.Count + _ohmNvidia.Count
                End Get
            End Property
#End Region
#Region "Registry access"
            ' TODO Expand registry section to class for enum gpu additional information
            Private Structure sNVRegistry
                Property DeviceName As String
                Property DeviceID As String
            End Structure
            Private Function NVIDsFromRegistry() As List(Of sNVRegistry)
                Try
                    Dim nl As New List(Of sNVRegistry)
                    Dim moSearch As ManagementObjectSearcher
                    Dim moObj As ManagementObject
                    moSearch = New ManagementObjectSearcher("SELECT * FROM Win32_VideoController")
                    For Each moObj In moSearch.Get
                        'Check for Nvidia
                        If moObj.Item("AdapterCompatibility").ToString.ToUpperInvariant.Contains("NVIDIA") Then
                            Dim ni As New sNVRegistry
                            ni.DeviceName = CStr(moObj.Item("Caption"))
                            ni.DeviceID = CStr(moObj.Item("PNPDeviceID"))
                            nl.Add(ni)
                        End If
                    Next
                    Return nl
                    ' TODO decide to pernamently dith registry acces
                    'Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\ControlSet001\Control\Video")
                    'Dim sKey() As String = rKey.GetSubKeyNames
                    'For Each kName As String In sKey
                    '    Dim aKey As RegistryKey = rKey.OpenSubKey(kName) ' Add \0000 
                    '    If aKey.GetSubKeyNames.Contains("0000") Then
                    '        aKey = aKey.OpenSubKey("0000")
                    '        If Not IsNothing(aKey) Then
                    '            Dim vNames() As String = aKey.GetValueNames
                    '            If Not IsNothing(vNames) AndAlso vNames.Contains("MatchingDeviceId") Then
                    '                Dim pciID As String = aKey.GetValue("MatchingDeviceId").ToString
                    '                If pciID.ToUpper.Contains("PCI\VEN_10DE") Then
                    '                    Dim nI As New sNVRegistry
                    '                    nI.DeviceID = pciID
                    '                    If aKey.GetValueNames.Contains("Device Description") Then nI.DeviceName = aKey.GetValue("Device Description").ToString
                    '                    If aKey.GetValueNames.Contains("Description") Then nI.DeviceName = aKey.GetValue("Description").ToString
                    '                    nl.Add(nI)
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'Next
                    'Return nl
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return New List(Of sNVRegistry)
                End Try
            End Function
            Private Function AtiDriverStringWMI() As String
                Try
                    Dim moSearch As ManagementObjectSearcher
                    Dim moObj As ManagementObject
                    moSearch = New ManagementObjectSearcher("SELECT * FROM Win32_VideoController")
                    For Each moObj In moSearch.Get
                        'Check for Nvidia
                        If moObj.Item("AdapterCompatibility").ToString.ToUpper.Contains("ATI") Or moObj.Item("AdapterCompatibility").ToString.ToUpperInvariant = "ADVANCED MICRO DEVICES, INC." Then
                            Return CStr(moObj.Item("DriverVersion"))
                        End If
                    Next
                    Return ""
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return ""
                End Try
            End Function
            Private Overloads Function DriverStringRegistry(Optional Key As String = "", Optional VendorID As String = "", Optional DeviceID As String = "", Optional OtherValue As String = "") As String ' Key for registry acces directly from possible output from ohm, VendorID first from vendor, deviceID match - empty string returned on failure, user alternatevalue to acces other fields
                Try
                    If Key = "" Then
                        ' Enum control entries
                        Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\ControlSet001\Control\Video")
                        Dim sKey() As String = rKey.GetSubKeyNames
                        For Each kName As String In sKey
                            Dim aKey As RegistryKey = rKey.OpenSubKey(kName) ' Add \0000 
                            If aKey.GetSubKeyNames.Contains("0000") Then
                                aKey = aKey.OpenSubKey("0000")
                                Dim vNames() As String = aKey.GetValueNames
                                If vNames.Contains("MatchingDeviceId") Then
                                    ' Vendor first
                                    If Not VendorID = "" AndAlso Not DeviceID = "" Then
                                        If aKey.GetValueNames.Contains("MatchingDeviceID") AndAlso aKey.GetValue("MatchingDeviceId").ToString.ToUpper.Contains("PCI\VEN_" & VendorID & "&" & DeviceID) Then
                                            If OtherValue <> "" Then
                                                If vNames.Contains(OtherValue) Then
                                                    Return aKey.GetValue(OtherValue).ToString
                                                Else
                                                    Return ""
                                                End If
                                            Else
                                                Return aKey.GetValue("DriverVersion").ToString
                                            End If
                                        End If
                                    ElseIf Not VendorID = "" Then
                                        If aKey.GetValueNames.Contains("MatchingDeviceID") AndAlso aKey.GetValue("MatchingDeviceId").ToString.ToUpper.Contains("PCI\VEN_" & VendorID) Then
                                            If OtherValue <> "" Then
                                                If vNames.Contains(OtherValue) Then
                                                    Return aKey.GetValue(OtherValue).ToString
                                                Else
                                                    Return ""
                                                End If
                                            Else
                                                Return aKey.GetValue("DriverVersion").ToString
                                            End If
                                        End If

                                    End If
                                End If
                            End If

                        Next
                        Return "" ' No matches
                    Else
                        Throw New NotImplementedException("Ohm output insufficient use VendorID or DeviceID")
                    End If
                    Return vbNullString
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return vbNullString
                End Try
            End Function
#End Region
#Region "IDisposable Support"
            Protected Overrides Sub Finalize()
                Close()
                MyBase.Finalize()
            End Sub
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                    End If

                    ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    ' TODO: set large fields to null.
                End If
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class
        Public ohmInterface As New cOHMInterface
        Private mHasError As Boolean = False
        Public ReadOnly Property HasError As Boolean
            Get
                Return mHasError
            End Get
        End Property
#End Region
#Region "Environment"
        Public Class cEnvironment
            Implements IDisposable
            Private _X64 As Boolean, _IsAdmin As Boolean
            Private _strUserName As String, _CanElevate As Boolean, _strOSName As String, _OsSupported As Boolean
            Public Enum eWindowsPlatform
                Unsupported = 0
                WindowsXPsp3 = 1
                Windows2003 = 2
                WindowsVista = 3
                Windows2008 = 4
                Windows7 = 5
                XP_2003 = 6
                Vista_08_07 = 7
                Linux = 9
            End Enum
            Private _WinPlatform As eWindowsPlatform = eWindowsPlatform.Unsupported
            Private _WinGroup As eWindowsPlatform = eWindowsPlatform.Unsupported
            Public ReadOnly Property WinGroup As eWindowsPlatform
                Get
                    If _WinPlatform = eWindowsPlatform.Windows2003 Or _WinPlatform = eWindowsPlatform.WindowsXPsp3 Then
                        Return eWindowsPlatform.XP_2003
                    ElseIf _WinPlatform <> eWindowsPlatform.Unsupported Then
                        Return eWindowsPlatform.Vista_08_07
                    Else
                        Return eWindowsPlatform.Unsupported
                    End If
                End Get
            End Property
            Public ReadOnly Property WindowsPlatform() As eWindowsPlatform
                Get
                    Return _WinPlatform
                End Get
            End Property
            Public ReadOnly Property IsAdmin() As Boolean
                Get
                    Return _IsAdmin
                End Get
            End Property
            Public ReadOnly Property CurrentUser As String
                Get
                    Return _strUserName
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
            Private Function Is2kHotFix() As Boolean
                Try
                    Dim query As ObjectQuery, searcher As ManagementObjectSearcher, queryCollection As ManagementObjectCollection
                    query = New ObjectQuery("select * from Win32_QuickFixEngineering where hotfixid = 982861")
                    searcher = New ManagementObjectSearcher(query)
                    queryCollection = searcher.Get()
                    Return queryCollection.Count > 0
                    ' select * from Win32_QuickFixEngineering where hotfixid = 982861
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
            Public Function FillOsInfo() As Boolean
                Try
                    If Environment.OSVersion.Platform = PlatformID.Unix Then
                        _WinPlatform = eWindowsPlatform.Linux
                        Return True
                    ElseIf Environment.OSVersion.Version.Major < 5 Or (Environment.OSVersion.Version.Major = 5 And Environment.OSVersion.Version.Minor = 0) Then
                        _WinPlatform = eWindowsPlatform.Unsupported
                        Return True
                    ElseIf Environment.OSVersion.Version.Major = 5 Then
                        If Environment.OSVersion.Version.Minor = 0 Then
                            _WinPlatform = eWindowsPlatform.Unsupported
                        ElseIf Environment.OSVersion.Version.Minor = 1 Then
                            If Not Environment.OSVersion.ServicePack.Contains("3") Then
                                _WinPlatform = eWindowsPlatform.Unsupported
                            Else
                                _WinPlatform = eWindowsPlatform.WindowsXPsp3
                            End If
                        ElseIf Environment.OSVersion.Version.Minor = 2 Then
                            _WinPlatform = eWindowsPlatform.Windows2003
                        End If
                    ElseIf Environment.OSVersion.Version.Major = 6 Then
                        If Environment.OSVersion.Version.Minor = 0 Then
                            If Not My.Computer.Info.OSFullName.ToUpper.Contains("SERVER") Then
                                _WinPlatform = eWindowsPlatform.WindowsVista
                            Else
                                _WinPlatform = eWindowsPlatform.Windows2008
                            End If
                        ElseIf Environment.OSVersion.Version.Minor = 1 Then
                            If Not My.Computer.Info.OSFullName.ToUpper.Contains("SERVER") Then
                                _WinPlatform = eWindowsPlatform.Windows7
                            Else
                                _WinPlatform = eWindowsPlatform.Windows2008
                            End If
                        End If
                    Else
                        WriteLog("Could not identify platform from the following version object: " & Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.MajorRevision & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.MinorRevision & "." & Environment.OSVersion.Version.Revision & "." & Environment.OSVersion.Version.Build)
                        _WinPlatform = eWindowsPlatform.Unsupported
                    End If
                    Return True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
            Private Function FillIsX64() As Boolean
                Try
                    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\Diag_cons.exe") Then
                        Dim nP As New Process
                        With nP.StartInfo
                            .FileName = "Diag_cons.exe"
                            .WorkingDirectory = My.Application.Info.DirectoryPath
                            .CreateNoWindow = True ' Same as fahcontrol
                            .UseShellExecute = False
                            .RedirectStandardOutput = True
                        End With
                        nP.Start()
                        Dim cOUT As StreamReader = nP.StandardOutput
                        Dim infoText As String = cOUT.ReadToEnd
                        If infoText.Contains("64") Then
                            _X64 = True
                        Else
                            _X64 = False
                        End If
                        Return True
                    End If
                    If Environment.GetEnvironmentVariables.Contains("PROCESSOR_ARCHITEW6432") AndAlso Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432").Contains("64") Then
                        _X64 = True
                    Else
                        _X64 = False
                    End If
                    Return True
                    'TODO decide what is the best way??
                    If IntPtr.Size = 8 Then
                        'win64
                        _X64 = True
                    ElseIf IntPtr.Size = 4 Then
                        'win32
                        _X64 = False
                    End If
                    Return True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    _X64 = False
                    Return False
                End Try
            End Function
            Private Function FillIsAdmin() As Boolean
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
                    WriteError(ex.Message, Err)
                    _IsAdmin = False
                    Return False
                End Try
            End Function
            Private Function FillCanElevate() As Boolean
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
                            Return False
                        End Try
                    Else
                        Return _IsAdmin
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
            Public Function FillEnvironment(TheLumberJack As LumberJack.LumberJack) As Boolean
                Try
                    modLumberJack.LumberJack = TheLumberJack
                    Dim bAll As Boolean = False
                    WriteLog("Getting environment information:")
                    FillIsX64()
                    If FillIsAdmin() Then
                        If _IsAdmin Then
                            WriteLog("Administrator rights detected.")
OsInfo:
                            If FillOsInfo() Then
                                If _WinPlatform = eWindowsPlatform.Unsupported Then
                                    WriteLog("Unsupported platform found!")
                                    Return False
                                Else
                                    WriteLog("Platform: " & _WinPlatform.ToString)
                                    Return True
                                End If
                            End If
                        Else
                            WriteLog("Non admin user rights detected.")
                            If Not _CanElevate Then
                                WriteLog("User rights can not be elevated!")
                                Return False
                            Else
                                WriteLog("User rights can be elevated.")
                                GoTo OsInfo
                            End If
                        End If
                    End If
                    Return False
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return False
                End Try
            End Function
#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                    End If

                    ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                    ' TODO: set large fields to null.
                End If
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class
        Public WithEvents Territory As New cEnvironment
#End Region
        Public Function Init(TheLumberJack As LumberJack.LumberJack, Optional CloseOHM As Boolean = True) As Boolean
            Try
                modLumberJack.LumberJack = TheLumberJack
                ' Don't threat failure's as error
                Dim bGpu As Boolean = gpuInf.initGpuInfo(TheLumberJack)
                WriteLog("Starting gpgpu enumeration: " & bGpu.ToString, Me)
                Dim bEnv As Boolean = Territory.FillEnvironment(TheLumberJack)
                WriteLog("Starting Environment query: " & bEnv.ToString, Me)
                Dim bOhm As Boolean = ohmInterface.Init(CloseOHM, TheLumberJack)
                WriteLog("Starting OpenHardwareMonitorlib.dll extension: " & bOhm.ToString, Me)
                Return True
            Catch ex As Exception
                mHasError = True
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Sub Close()
            ohmInterface.Close()
            ohmInterface = Nothing
            gpuInf = Nothing
            Territory = Nothing
        End Sub
#Region "IDisposable Support"
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ohmInterface.Dispose()
                    gpuInf.Dispose()
                    Territory.Dispose()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
