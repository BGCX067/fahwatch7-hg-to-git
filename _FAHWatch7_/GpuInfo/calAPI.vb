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
    Public Enum CALboolean
        CAL_FALSE = 0
        CAL_TRUE = 1
    End Enum
    Public Enum CALresallocflags
        CAL_RESALLOC_CACHEABLE = 2
        CAL_RESALLOC_GLOBAL_BUFFER = 1
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
#End Region
#Region "Structures"
    <StructLayout(LayoutKind.Sequential)> Public Structure CALname
        Public Value As UInt32
    End Structure
    <StructLayout(LayoutKind.Sequential)> Public Structure CALdeviceinfo
        Public target As CALtarget
        Public maxResource1DWidth As UInteger
        Public maxResource2DWidth As UInteger
        Public maxResource2DHeight As UInteger
    End Structure
    <StructLayout(LayoutKind.Sequential)> Public Structure CALdeviceattribs
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
    <StructLayout(LayoutKind.Sequential)> Public Structure CALdeviceattribs_2
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
    <StructLayout(LayoutKind.Sequential)> Public Structure CALdevice
        Public Value As UInt32
    End Structure
#End Region
#Region "X86"
    Public Class NativeMethods
        ' Methods
        Public Sub New()
        End Sub
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
        Public Shared Function calDeviceOpen(ByVal dev As CALdevice, ByVal ordinal As UInt32) As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calGetErrorString() As String
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calGetVersion(<Out()> ByRef major As UInt32, <Out()> ByRef minor As UInt32, <Out()> ByRef imp As UInt32) As CALresult
        End Function
        <DllImport("aticalrt"), CLSCompliant(False)> _
        Public Shared Function calInit() As CALresult
        End Function
        <DllImport("aticalrt")> _
        Public Shared Function calShutdown() As CALresult
        End Function
    End Class
#End Region
End Namespace

