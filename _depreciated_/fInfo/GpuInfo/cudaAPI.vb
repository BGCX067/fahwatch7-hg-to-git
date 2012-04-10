'/*
' * CUDA Info API Copyright Marvin Westmaas ( mtm )
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
Namespace myCuda
    Public Enum cudaError
        cudaErrorAddressOfConstant = 22
        cudaErrorApiFailureBase = 10000
        cudaErrorCudartUnloading = 29
        cudaErrorInitializationError = 3
        cudaErrorInsufficientDriver = 35
        cudaErrorInvalidChannelDescriptor = 20
        cudaErrorInvalidConfiguration = 9
        cudaErrorInvalidDevice = 10
        cudaErrorInvalidDeviceFunction = 8
        cudaErrorInvalidDevicePointer = 17
        cudaErrorInvalidFilterSetting = 26
        cudaErrorInvalidHostPointer = 16
        cudaErrorInvalidMemcpyDirection = 21
        cudaErrorInvalidNormSetting = 27
        cudaErrorInvalidPitchValue = 12
        cudaErrorInvalidResourceHandle = 33
        cudaErrorInvalidSymbol = 13
        cudaErrorInvalidTexture = 18
        cudaErrorInvalidTextureBinding = 19
        cudaErrorInvalidValue = 11
        cudaErrorLaunchFailure = 4
        cudaErrorLaunchOutOfResources = 7
        cudaErrorLaunchTimeout = 6
        cudaErrorMapBufferObjectFailed = 14
        cudaErrorMemoryAllocation = 2
        cudaErrorMemoryValueTooLarge = 32
        cudaErrorMissingConfiguration = 1
        cudaErrorMixedDeviceExecution = 28
        cudaErrorNoDevice = 37
        cudaErrorNotReady = 34
        cudaErrorNotYetImplemented = 31
        cudaErrorPriorLaunchFailure = 5
        cudaErrorSetOnActiveProcess = 36
        cudaErrorStartupFailure = 127
        cudaErrorSynchronizationError = 25
        cudaErrorTextureFetchFailed = 23
        cudaErrorTextureNotBound = 24
        cudaErrorUnknown = 30
        cudaErrorUnmapBufferObjectFailed = 15
        cudaSuccess = 0
    End Enum

    <Serializable()> <StructLayout(LayoutKind.Sequential)> _
    Public Structure CUdevice
        Public Pointer As Integer
    End Structure

    Public Enum CUDeviceAttribute4
        'CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_BLOCK 	 Maximum number of threads per block
        'CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_X 	 Maximum block dimension X
        'CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Y 	 Maximum block dimension Y
        'CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Z 	 Maximum block dimension Z
        'CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_X 	 Maximum grid dimension X
        'CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Y 	 Maximum grid dimension Y
        'CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Z 	 Maximum grid dimension Z
        'CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK 	 Maximum shared memory available per block in bytes
        'CU_DEVICE_ATTRIBUTE_SHARED_MEMORY_PER_BLOCK 	 Deprecated, use CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK
        'CU_DEVICE_ATTRIBUTE_TOTAL_CONSTANT_MEMORY 	 Memory available on device for __constant__ variables in a CUDA C kernel in bytes
        'CU_DEVICE_ATTRIBUTE_WARP_SIZE 	 Warp size in threads
        'CU_DEVICE_ATTRIBUTE_MAX_PITCH 	 Maximum pitch in bytes allowed by memory copies
        'CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK 	 Maximum number of 32-bit registers available per block
        'CU_DEVICE_ATTRIBUTE_REGISTERS_PER_BLOCK 	 Deprecated, use CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK
        'CU_DEVICE_ATTRIBUTE_CLOCK_RATE 	 Peak clock frequency in kilohertz
        'CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT 	 Alignment requirement for textures
        'CU_DEVICE_ATTRIBUTE_GPU_OVERLAP 	 Device can possibly copy memory and execute a kernel concurrently. Deprecated. Use instead CU_DEVICE_ATTRIBUTE_ASYNC_ENGINE_COUNT.
        'CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT 	 Number of multiprocessors on device
        'CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT 	 Specifies whether there is a run time limit on kernels
        'CU_DEVICE_ATTRIBUTE_INTEGRATED 	 Device is integrated with host memory
        'CU_DEVICE_ATTRIBUTE_CAN_MAP_HOST_MEMORY 	 Device can map host memory into CUDA address space
        'CU_DEVICE_ATTRIBUTE_COMPUTE_MODE 	 Compute mode (See CUcomputemode for details)
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH 	 Maximum 1D texture width
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_WIDTH 	 Maximum 2D texture width
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_HEIGHT 	 Maximum 2D texture height
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH 	 Maximum 3D texture width
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT 	 Maximum 3D texture height
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH 	 Maximum 3D texture depth
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_WIDTH 	 Maximum 2D layered texture width
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_HEIGHT 	 Maximum 2D layered texture height
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_LAYERS 	 Maximum layers in a 2D layered texture
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_WIDTH 	 Deprecated, use CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_WIDTH
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_HEIGHT 	 Deprecated, use CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_HEIGHT
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_NUMSLICES 	 Deprecated, use CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_LAYERED_LAYERS
        'CU_DEVICE_ATTRIBUTE_SURFACE_ALIGNMENT 	 Alignment requirement for surfaces
        'CU_DEVICE_ATTRIBUTE_CONCURRENT_KERNELS 	 Device can possibly execute multiple kernels concurrently
        'CU_DEVICE_ATTRIBUTE_ECC_ENABLED 	 Device has ECC support enabled
        'CU_DEVICE_ATTRIBUTE_PCI_BUS_ID 	 PCI bus ID of the device
        'CU_DEVICE_ATTRIBUTE_PCI_DEVICE_ID 	 PCI device ID of the device
        'CU_DEVICE_ATTRIBUTE_TCC_DRIVER 	 Device is using TCC driver model
        'CU_DEVICE_ATTRIBUTE_MEMORY_CLOCK_RATE 	 Peak memory clock frequency in kilohertz
        'CU_DEVICE_ATTRIBUTE_GLOBAL_MEMORY_BUS_WIDTH 	 Global memory bus width in bits
        'CU_DEVICE_ATTRIBUTE_L2_CACHE_SIZE 	 Size of L2 cache in bytes
        'CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_MULTIPROCESSOR 	 Maximum resident threads per multiprocessor
        'CU_DEVICE_ATTRIBUTE_ASYNC_ENGINE_COUNT 	 Number of asynchronous engines
        'CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING 	 Device uses shares a unified address space with the host
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_WIDTH 	 Maximum 1D layered texture width
        'CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_LAYERS 	 Maximum layers in a 1D layered texture

      CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_BLOCK = 1
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_X = 2
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Y = 3
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Z = 4
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_X = 5
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Y = 6
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Z = 7
        CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK = 8
        CU_DEVICE_ATTRIBUTE_SHARED_MEMORY_PER_BLOCK = 8
        CU_DEVICE_ATTRIBUTE_TOTAL_CONSTANT_MEMORY = 9
        CU_DEVICE_ATTRIBUTE_WARP_SIZE = 10
        CU_DEVICE_ATTRIBUTE_MAX_PITCH = 11
        CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK = 12
        CU_DEVICE_ATTRIBUTE_REGISTERS_PER_BLOCK = 12
        CU_DEVICE_ATTRIBUTE_CLOCK_RATE = 13
        CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT = 14
        CU_DEVICE_ATTRIBUTE_GPU_OVERLAP = 15
        CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT = 16
        CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT = 17
        CU_DEVICE_ATTRIBUTE_INTEGRATED = 18
        CU_DEVICE_ATTRIBUTE_CAN_MAP_HOST_MEMORY = 19
        CU_DEVICE_ATTRIBUTE_COMPUTE_MODE = 20
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH = 21
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_WIDTH = 22
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_HEIGHT = 23
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH = 24
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT = 25
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH = 26
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_WIDTH = 27
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_HEIGHT = 28
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_NUMSLICES = 29
        CU_DEVICE_ATTRIBUTE_SURFACE_ALIGNMENT = 30
        CU_DEVICE_ATTRIBUTE_CONCURRENT_KERNELS = 31
        CU_DEVICE_ATTRIBUTE_ECC_ENABLED = 32
        CU_DEVICE_ATTRIBUTE_PCI_BUS_ID = 33
        CU_DEVICE_ATTRIBUTE_PCI_DEVICE_ID = 34
        CU_DEVICE_ATTRIBUTE_TCC_DRIVER = 35
        CU_DEVICE_ATTRIBUTE_MEMORY_CLOCK_RATE = 36 'Peak memory clock frequency in kilohertz
        CU_DEVICE_ATTRIBUTE_GLOBAL_MEMORY_BUS_WIDTH = 37 'Global memory bus width in bits
        CU_DEVICE_ATTRIBUTE_L2_CACHE_SIZE = 38 'Size of L2 cache in bytes
        CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_MULTIPROCESSOR = 39 'Maximum resident threads per multiprocessor
        CU_DEVICE_ATTRIBUTE_ASYNC_ENGINE_COUNT = 40 'Number of asynchronous engines
        CU_DEVICE_ATTRIBUTE_UNIFIED_ADDRESSING = 41 'Device uses shares a unified address space with the host
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_WIDTH = 42 'Maximum 1D layered texture width
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_LAYERED_LAYERS = 43 'Maximum layers in a 1D layered texture
    End Enum

    Public Enum CUDeviceAttribute3
        CU_DEVICE_ATTRIBUTE_MAX_THREADS_PER_BLOCK = 1
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_X = 2
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Y = 3
        CU_DEVICE_ATTRIBUTE_MAX_BLOCK_DIM_Z = 4
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_X = 5
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Y = 6
        CU_DEVICE_ATTRIBUTE_MAX_GRID_DIM_Z = 7
        CU_DEVICE_ATTRIBUTE_MAX_SHARED_MEMORY_PER_BLOCK = 8
        CU_DEVICE_ATTRIBUTE_SHARED_MEMORY_PER_BLOCK = 8
        CU_DEVICE_ATTRIBUTE_TOTAL_CONSTANT_MEMORY = 9
        CU_DEVICE_ATTRIBUTE_WARP_SIZE = 10
        CU_DEVICE_ATTRIBUTE_MAX_PITCH = 11
        CU_DEVICE_ATTRIBUTE_MAX_REGISTERS_PER_BLOCK = 12
        CU_DEVICE_ATTRIBUTE_REGISTERS_PER_BLOCK = 12
        CU_DEVICE_ATTRIBUTE_CLOCK_RATE = 13
        CU_DEVICE_ATTRIBUTE_TEXTURE_ALIGNMENT = 14
        CU_DEVICE_ATTRIBUTE_GPU_OVERLAP = 15
        CU_DEVICE_ATTRIBUTE_MULTIPROCESSOR_COUNT = 16
        CU_DEVICE_ATTRIBUTE_KERNEL_EXEC_TIMEOUT = 17
        CU_DEVICE_ATTRIBUTE_INTEGRATED = 18
        CU_DEVICE_ATTRIBUTE_CAN_MAP_HOST_MEMORY = 19
        CU_DEVICE_ATTRIBUTE_COMPUTE_MODE = 20
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE1D_WIDTH = 21
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_WIDTH = 22
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_HEIGHT = 23
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_WIDTH = 24
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_HEIGHT = 25
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE3D_DEPTH = 26
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_WIDTH = 27
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_HEIGHT = 28
        CU_DEVICE_ATTRIBUTE_MAXIMUM_TEXTURE2D_ARRAY_NUMSLICES = 29
        CU_DEVICE_ATTRIBUTE_SURFACE_ALIGNMENT = 30
        CU_DEVICE_ATTRIBUTE_CONCURRENT_KERNELS = 31
        CU_DEVICE_ATTRIBUTE_ECC_ENABLED = 32
        CU_DEVICE_ATTRIBUTE_PCI_BUS_ID = 33
        CU_DEVICE_ATTRIBUTE_PCI_DEVICE_ID = 34
        CU_DEVICE_ATTRIBUTE_TCC_DRIVER = 35
    End Enum


    Public Structure cudaAPI

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDeviceGetName(<Out()> ByVal b As Byte(), ByVal size As Integer, ByVal CUDevice As myCuda.CUdevice) As myCuda.cudaError
        End Function


        <DllImport("nvcuda.dll")> _
        Public Overloads Shared Function cuDeviceGetAttribute(ByRef ReturnValue As Int32, ByVal CUDeviceAttribute As myCuda.CUDeviceAttribute3, ByVal CUDevice As myCuda.CUdevice) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Overloads Shared Function cuDeviceGetAttribute(ByRef ReturnValue As Int32, ByVal CUDeviceAttribute As myCuda.CUDeviceAttribute4, ByVal CUDevice As myCuda.CUdevice) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDeviceGetCount(ByRef count As Integer) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDeviceGet(ByRef CUDevice As myCuda.CUdevice, ByVal ordinal As Integer) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDeviceComputeCapability(ByRef Major As Integer, ByRef Minor As Integer, ByVal CUDevice As myCuda.CUdevice) As myCuda.cudaError
        End Function

        <DllImport("cudart.dll", CharSet:=CharSet.Ansi)> _
        Public Shared Function cudaGetErrorString(ByVal [error] As cudaError) As String
        End Function

        <DllImport("cudart.dll")> _
        Public Shared Function cudaGetLastError() As cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDeviceTotalMem(ByRef bytes As UInt32, ByVal dev As myCuda.CUdevice) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuDriverGetVersion(ByRef driverVersion As Integer) As myCuda.cudaError
        End Function

        <DllImport("nvcuda.dll")> _
        Public Shared Function cuInit(ByVal Flags As UInt32) As myCuda.cudaError
        End Function

    End Structure
End Namespace



