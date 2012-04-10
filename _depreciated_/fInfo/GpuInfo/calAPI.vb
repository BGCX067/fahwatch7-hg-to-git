'/*
' * CAL Info API Copyright Marvin Westmaas ( mtm )
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

Namespace myCAL
#Region "Enumerations"
    Public Enum CALlanguage
        CAL_LANGUAGE_IL = 1
    End Enum
    Public Enum CALtarget
        CAL_TARGET_600                '/**< R600 GPU ISA */     
        CAL_TARGET_610                '/**< RV610 GPU ISA */    
        CAL_TARGET_630                '/**< RV630 GPU ISA */
        CAL_TARGET_670                ' /**< RV670 GPU ISA */
        CAL_TARGET_7XX                '/**< R700 class GPU ISA */
        CAL_TARGET_770                '/**< RV770 GPU ISA */
        CAL_TARGET_710                '/**< RV710 GPU ISA */
        CAL_TARGET_730                '/**< RV730 GPU ISA */
        CAL_TARGET_CYPRESS            '/**< CYPRESS GPU ISA */
        CAL_TARGET_JUNIPER            '/**< JUNIPER GPU ISA */
        CAL_TARGET_REDWOOD            '/**< REDWOOD GPU ISA */
        CAL_TARGET_CEDAR
    End Enum
    Public Enum CALformat
        CAL_FORMAT_UBYTE_1 = 1
        CAL_FORMAT_UBYTE_2 = 2
        CAL_FORMAT_UBYTE_4 = 3
        CAL_FORMAT_USHORT_1 = 4
        CAL_FORMAT_USHORT_2 = 5
        CAL_FORMAT_USHORT_4 = 6
        CAL_FORMAT_UINT_4 = 7
        CAL_FORMAT_BYTE_4 = 8
        CAL_FORMAT_SHORT_1 = 9
        CAL_FORMAT_SHORT_2 = 10
        CAL_FORMAT_SHORT_4 = 11
        CAL_FORMAT_FLOAT_1 = 12
        CAL_FORMAT_FLOAT_2 = 13
        CAL_FORMAT_FLOAT_4 = 14
        CAL_FORMAT_DOUBLE_1 = 15
        CAL_FORMAT_DOUBLE_2 = 16
        CAL_FORMAT_UINT_1 = 17
        CAL_FORMAT_UINT_2 = 18
        CAL_FORMAT_BYTE_1 = 19
        CAL_FORMAT_BYTE_2 = 20
        CAL_FORMAT_INT_1 = 21
        CAL_FORMAT_INT_2 = 22
        CAL_FORMAT_INT_4 = 23
    End Enum
    Public Enum CALresult
        CAL_RESULT_OK = 0
        CAL_RESULT_ERROR = 1
        CAL_RESULT_INVALID_PARAMETER = 2
        CAL_RESULT_NOT_SUPPORTED = 3
        CAL_RESULT_ALREADY = 4
        CAL_RESULT_NOT_INITIALIZED = 5
        CAL_RESULT_BAD_HANDLE = 6
        CAL_RESULT_BAD_NAME_TYPE = 7
        CAL_RESULT_PENDING = 8
        CAL_RESULT_BUSY = 9
        CAL_RESULT_WARNING = 10
    End Enum
    Public Enum CALboolean
        CAL_FALSE = 0
        CAL_TRUE = 1
    End Enum
    Public Enum CALresallocflags
        CAL_RESALLOC_CACHEABLE = 2
        CAL_RESALLOC_GLOBAL_BUFFER = 1
    End Enum
#End Region
#Region "Structures"
    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALfunc
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
     Public Structure CALfuncInfo
        Public maxScratchRegsNeeded As UInteger
        Public numSharedGPRUser As UInteger
        Public numSharedGPRTotal As UInteger
        Public eCsSetupMode As CALboolean
        Public numThreadPerGroup As UInteger
        Public totalNumThreadGroup As UInteger
        Public wavefrontPerSIMD As UInteger
        Public numWavefrontPerSIMD As UInteger
        Public isMaxNumWavePerSIMD As CALboolean
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALimage
        Public Value As IntPtr
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALname
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
     Public Structure CALobject
        Public Value As IntPtr
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
     Public Structure CALprogramGrid
        Public func As CALfunc
        Public gridBlock As CALdomain3D
        Public gridSize As CALdomain3D
        Public flags As UInteger
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
     Public Structure CALmem
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALdevicestatus
        Public struct_size As UInteger
        Public availLocalRAM As UInteger
        Public availUncachedRemoteRAM As UInteger
        Public availCachedRemoteRAM As UInteger
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
   Public Structure CALmodule
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
   Public Structure CALdeviceinfo
        Public target As CALtarget
        Public maxResource1DWidth As UInteger
        Public maxResource2DWidth As UInteger
        Public maxResource2DHeight As UInteger
    End Structure


    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALdeviceattribs
        Public struct_size As UInt32         '/**< Client filled out size of CALdeviceattribs struct */
        Public target As CALtarget            '/**< Asic identifier */
        Public localRAM As UInt32            '/**< Amount of local GPU RAM in megabytes */
        Public uncachedRemoteRAM As UInt32   '/**< Amount of uncached remote GPU memory in megabytes */
        Public cachedRemoteRAM As UInt32     '/**< Amount of cached remote GPU memory in megabytes */
        Public engineClock As UInt32         '/**< GPU device clock rate in megahertz */
        Public memoryClock As UInt32         '/**< GPU memory clock rate in megahertz */
        Public wavefrontSize As UInt32       '/**< Wavefront size */
        Public numberOfSIMD As UInt32        '/**< Number of SIMDs */
        Public doublePrecision As CALboolean    '/**< double precision supported *'/
        Public localDataShare As CALboolean      '/**< local data share supported *'/
        Public globalDataShare As CALboolean     '/**< global data share supported *'/
        Public globalGPR As CALboolean            '/**< global GPR supported *'/
        Public computeShader As CALboolean       '/**< compute shader supported *'/
        Public memExport As CALboolean           '/**< memexport supported *'/
        Public pitch_alignment As UInt32     '/**< Required alignment for calCreateRes allocations (in data elements) *'/
        Public surface_alignment As UInt32   '/**< Required start address alignment for calCreateRes allocations (in bytes) *'/
        Public numberOfUAVs As UInt32        '/**< Number of UAVs */
        Public bUAVMemExport As CALboolean      '/**< Hw only supports mem export to simulate 1 UAV */
        Public b3dProgramGrid As CALboolean      '/**< CALprogramGrid for have height and depth bigger than 1*/
        Public numberOfShaderEngines As UInt32 '/**< Number of shader engines */
        Public targetRevision As UInt32      '/**< Asic family revision */
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
   Public Structure CALdeviceattribs_2
        Public struct_size As UInt32         '/**< Client filled out size of CALdeviceattribs struct */
        Public target As CALtarget            '/**< Asic identifier */
        Public localRAM As UInt32            '/**< Amount of local GPU RAM in megabytes */
        Public uncachedRemoteRAM As UInt32   '/**< Amount of uncached remote GPU memory in megabytes */
        Public cachedRemoteRAM As UInt32     '/**< Amount of cached remote GPU memory in megabytes */
        Public engineClock As UInt32         '/**< GPU device clock rate in megahertz */
        Public memoryClock As UInt32         '/**< GPU memory clock rate in megahertz */
        Public wavefrontSize As UInt32       '/**< Wavefront size */
        Public numberOfSIMD As UInt32        '/**< Number of SIMDs */
        Public doublePrecision As CALboolean    '/**< double precision supported *'/
        Public localDataShare As CALboolean      '/**< local data share supported *'/
        Public globalDataShare As CALboolean     '/**< global data share supported *'/
        Public globalGPR As CALboolean            '/**< global GPR supported *'/
        Public computeShader As CALboolean       '/**< compute shader supported *'/
        Public memExport As CALboolean           '/**< memexport supported *'/
        Public pitch_alignment As UInt32     '/**< Required alignment for calCreateRes allocations (in data elements) *'/
        Public surface_alignment As UInt32   '/**< Required start address alignment for calCreateRes allocations (in bytes) *'/
    End Structure








    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALdevice
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALdomain
        Public x As UInteger
        Public y As UInteger
        Public width As UInteger
        Public height As UInteger
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)>
  Public Structure CALcontext
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
  Public Structure CALdomain3D
        Public width As UInteger
        Public height As UInteger
        Public depth As UInteger
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
   Public Structure CALprogramGridArray
        <MarshalAs(UnmanagedType.LPArray)> _
        Public gridArray As CALprogramGrid()
        Public num As UInteger
        Public flags As UInteger
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
  Public Structure CALresource
        Public Value As UInt32
    End Structure

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CALevent
        Public Value As UInt32
    End Structure

#End Region
#Region "X64"
    Public Class CALRuntimeX64
        ' Methods
        Public Sub New()
        End Sub
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxCreate(ByVal ctx As CALcontext, ByVal dev As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxDestroy(ByVal ctx As CALcontext) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxFlush(ByVal ctx As CALcontext) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxGetMem(ByVal mem As CALmem, ByVal ctx As CALcontext, ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxIsEventDone(ByVal ctx As CALcontext, ByVal e As CALevent) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxReleaseMem(ByVal ctx As CALcontext, ByVal mem As CALmem) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxRunProgram(ByVal e As CALevent, ByVal ctx As CALcontext, ByVal func As CALfunc, ByRef domain As CALdomain) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxRunProgramGrid(ByVal e As CALevent, ByVal ctx As CALcontext, ByRef pProgramGrid As CALprogramGrid) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxRunProgramGridArray(ByVal e As CALevent, ByVal ctx As CALcontext, <[In]()> ByVal pGridArray As CALprogramGridArray()) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calCtxSetMem(ByVal ctx As CALcontext, ByVal name As CALname, ByVal mem As CALmem) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calDeviceClose(ByVal dev As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Overloads Shared Function calDeviceGetAttribs(<Out()> ByRef attribs As CALdeviceattribs, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Overloads Shared Function calDeviceGetAttribs(<Out()> ByRef attribs As CALdeviceattribs_2, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calDeviceGetCount(<Out()> ByVal count As UInt64) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calDeviceGetInfo(<Out()> ByVal info As CALdeviceinfo, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calDeviceGetStatus(<Out()> ByVal status As CALdevicestatus, ByVal device As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calDeviceOpen(ByVal dev As CALdevice, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calGetErrorString() As String
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calGetVersion(<Out()> ByVal major As UInt32, <Out()> ByVal minor As UInt32, <Out()> ByVal imp As UInt32) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calImageFree(ByVal image As CALimage) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calImageRead(ByVal image As CALimage, ByVal buffer As Byte(), ByVal size As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calInit() As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calMemCopy(ByVal e As CALevent, ByVal ctx As CALcontext, ByVal srcMem As CALmem, ByVal dstMem As CALmem, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calModuleGetEntry(ByVal func As CALfunc, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal procName As String) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calModuleGetFuncInfo(ByVal pInfo As CALfuncInfo, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal func As CALfunc) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calModuleGetName(ByVal name As CALname, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal varName As String) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calModuleLoad(ByVal [module] As CALmodule, ByVal ctx As CALcontext, ByVal image As CALimage) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calModuleUnload(ByVal ctx As CALcontext, ByVal [module] As CALmodule) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResAllocLocal1D(ByVal res As CALresource, ByVal dev As CALdevice, ByVal width As UInteger, ByVal format As CALformat, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResAllocLocal2D(ByVal res As CALresource, ByVal dev As CALdevice, ByVal width As UInteger, ByVal height As UInteger, ByVal format As CALformat, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResAllocRemote1D(ByVal res As CALresource, <[In]()> ByVal dev As CALdevice(), ByVal deviceCount As UInteger, ByVal width As UInteger, ByVal format As CALformat, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResAllocRemote2D(ByVal res As CALresource, <[In]()> ByVal dev As CALdevice(), ByVal deviceCount As UInteger, ByVal width As UInteger, ByVal height As UInteger, ByVal format As CALformat, _
  ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResFree(ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResMap(ByVal pPtr As IntPtr, ByVal pitch As UInteger, ByVal res As CALresource, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calResUnmap(ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt64")> _
        Public Shared Function calShutdown() As CALresult
        End Function
    End Class
#End Region
#Region "X86"
    Public Class CALRuntimeX86
        ' Methods
        Public Sub New()
        End Sub
        <DllImport("aticalrt")> _
        Public Shared Function calCtxCreate(ByVal ctx As CALcontext, ByVal dev As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxDestroy(ByVal ctx As CALcontext) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxFlush(ByVal ctx As CALcontext) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxGetMem(ByVal mem As CALmem, ByVal ctx As CALcontext, ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxIsEventDone(ByVal ctx As CALcontext, ByVal e As CALevent) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxReleaseMem(ByVal ctx As CALcontext, ByVal mem As CALmem) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxRunProgram(ByVal e As CALevent, ByVal ctx As CALcontext, ByVal func As CALfunc, ByRef domain As CALdomain) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxRunProgramGrid(ByVal e As CALevent, ByVal ctx As CALcontext, ByRef pProgramGrid As CALprogramGrid) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxRunProgramGridArray(ByVal e As CALevent, ByVal ctx As CALcontext, <[In]()> ByVal pGridArray As CALprogramGridArray()) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calCtxSetMem(ByVal ctx As CALcontext, ByVal name As CALname, ByVal mem As CALmem) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calDeviceClose(ByVal dev As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Overloads Shared Function calDeviceGetAttribs(<Out()> ByRef attribs As CALdeviceattribs, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Overloads Shared Function calDeviceGetAttribs(<Out()> ByRef attribs As CALdeviceattribs_2, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calDeviceGetCount(<Out()> ByRef count As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")>
        Public Shared Function calDeviceGetInfo(<Out()> ByRef info As CALdeviceinfo, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calDeviceGetStatus(<Out()> ByVal status As CALdevicestatus, ByVal device As CALdevice) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calDeviceOpen(ByVal dev As CALdevice, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calGetErrorString() As String
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calGetVersion(<Out()> ByRef major As UInt32, <Out()> ByRef minor As UInt32, <Out()> ByRef imp As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calImageFree(ByVal image As CALimage) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calImageRead(ByVal image As CALimage, ByVal buffer As Byte(), ByVal size As UInt32) As CALresult
        End Function
        <DllImport("aticalrt"), CLSCompliant(False)> _
        Public Shared Function calInit() As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calMemCopy(ByVal e As CALevent, ByVal ctx As CALcontext, ByVal srcMem As CALmem, ByVal dstMem As CALmem, ByVal flags As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calModuleGetEntry(ByVal func As CALfunc, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal procName As String) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calModuleGetFuncInfo(ByVal pInfo As CALfuncInfo, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal func As CALfunc) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calModuleGetName(ByVal name As CALname, ByVal ctx As CALcontext, ByVal [module] As CALmodule, ByVal varName As String) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calModuleLoad(ByVal [module] As CALmodule, ByVal ctx As CALcontext, ByVal image As CALimage) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calModuleUnload(ByVal ctx As CALcontext, ByVal [module] As CALmodule) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResAllocLocal1D(ByVal res As CALresource, ByVal dev As CALdevice, ByVal width As UInt32, ByVal format As CALformat, ByVal flags As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResAllocLocal2D(ByVal res As CALresource, ByVal dev As CALdevice, ByVal width As UInteger, ByVal height As UInteger, ByVal format As CALformat, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResAllocRemote1D(ByVal res As CALresource, <[In]()> ByVal dev As CALdevice(), ByVal deviceCount As UInt32, ByVal width As UInt32, ByVal format As CALformat, ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResAllocRemote2D(ByVal res As CALresource, <[In]()> ByVal dev As CALdevice(), ByVal deviceCount As UInt32, ByVal width As UInt32, ByVal height As UInt32, ByVal format As CALformat, _
  ByVal flags As UInteger) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResFree(ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResMap(ByVal pPtr As IntPtr, ByVal pitch As UInt32, ByVal res As CALresource, ByVal flags As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calResUnmap(ByVal res As CALresource) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calShutdown() As CALresult
        End Function
    End Class
#End Region
End Namespace