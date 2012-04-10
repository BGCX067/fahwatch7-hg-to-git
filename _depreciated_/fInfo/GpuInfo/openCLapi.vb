'/*
' * openCL Info API Copyright Marvin Westmaas ( mtm )
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
Imports gpuInfo.gpuInfo

Namespace myOCL
    Public Structure oclAPI
        Public Enum CommandQueueProperties As ULong
            NONE = 0
            OUT_OF_ORDER_EXEC_MODE_ENABLE = 1
            PROFILING_ENABLE = 2
        End Enum
        Public Enum DeviceMemCacheType
            NONE = 0
            READ_ONLY_CACHE = 1
            READ_WRITE_CACHE = 2
        End Enum
        Public Enum DeviceLocalMemType
            LOCAL = 1
            [GLOBAL] = 2
        End Enum
        Public Enum DeviceExecCapabilities As ULong
            KERNEL = 1 << 0
            NATIVE_KERNEL = 1 << 1
        End Enum
        Public Enum FpConfig As ULong
            DENORM = 1 << 0
            INF_NAN = 1 << 1
            ROUND_TO_NEAREST = 1 << 2
            ROUND_TO_ZERO = 1 << 3
            ROUND_TO_INF = 1 << 4
            FMA = 1 << 5
            SOFT_FLOAT = 1 << 6
        End Enum

        Public Enum DeviceType As ULong
            [DEFAULT] = 1 << 0
            CPU = 1 << 1
            GPU = 1 << 3
            ACCELERATOR = 1 << 4
            ALL = UInt32.MaxValue
        End Enum

        Public Shared ReadOnly Property DeviceTypeString(ByVal DeviceType As DeviceType) As String
            Get
                Select Case DeviceType
                    Case Is = 1 << 0
                        Return "DEFAULT"
                    Case Is = 1 << 1
                        Return "CPU"
                    Case Is = 1 << 2
                        Return "GPU"
                    Case Is = 1 << 3
                        Return "ACCELERATOR"
                    Case Is = UInt32.MaxValue
                        Return "ALL"
                End Select
            End Get
        End Property

        Public Enum dInfo
            Type = 4096
            VendorID = 4097
            MaxComputeUnits = 4098
            MaxWorkItemDimensions = 4099
            MaxWorkGroupSize = 4100
            MaxWorkItemSizes = 4101
            PreferredVectorWidthChar = 4102
            PreferredVectorWidthShort = 4103
            PreferredVectorWidthInt = 4104
            PreferredVectorWidthLong = 4105
            PreferredVectorWidthFloat = 4106
            PreferredVectorWidthDouble = 4107
            MaxClockFrequency = 4108
            AddressBits = 4109
            MaxReadImageArgs = 4110
            MaxWriteImageArgs = 4111
            MaxMemAllocSize = 4112
            Image2DMaxWidth = 4113
            Image2DMaxHeight = 4114
            Image3DMaxWidth = 4115
            Image3DMaxHeight = 4116
            Image3DMaxDepth = 4117
            ImageSupport = 4118
            MaxParameterSize = 4119
            MaxSamplers = 4120
            MemBaseAddrAlign = 4121
            MinDataTypeAlignSize = 4122
            SingleFPConfig = 4123
            GlobalMemCacheType = 4124
            GlobalMemCacheLineSize = 4125
            GlobalMemCacheSize = 4126
            GlobalMemSize = 4127
            MaxConstantBufferSize = 4128
            MaxConstantArgs = 4129
            LocalMemType = 4130
            LocalMemSize = 4131
            ErrorCorrectionSupport = 4132
            ProfilingTimerResolution = 4133
            EndianLittle = 4134
            Available = 4135
            CompilerAvailable = 4136
            ExecutionCapabilities = 4137
            QueueProperties = 4138
            Name = 4139
            Vendor = 4140
            DriverVersion = 4141
            Profile = 4142
            Version = 4143
            Extensions = 4144
            Platform = 4145
        End Enum

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure CLPlatformID
            Public Value As IntPtr
        End Structure

        Public Enum CLPlatformInfo As UInt32
            ' Fields
            Extensions = 2308
            Name = 2306
            Profile = 2304
            Vender = 2307
            Version = 2305
        End Enum

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure CLDeviceID
            Public Value As IntPtr
        End Structure

        Public Enum CLDeviceType As UInt64
            ' Fields
            Accelerator = 8
            All = 4294967295
            CPU = 2
            [Default] = 1
            GPU = 4
        End Enum

        Public Enum CLDeviceInfo As UInt32
            ' Fields
            AddressBits = 4109
            Available = 4135
            CompilerAvailable = 4136
            DriverVersion = 4141
            EndianLittle = 4134
            ErrorCorrectionSupport = 4132
            ExecutionCapabilities = 4137
            Extensions = 4144
            GlobalMemCacheLineSize = 4125
            GlobalMemCacheSize = 4126
            GlobalMemCacheType = 4124
            GlobalMemSize = 4127
            Image2DMaxHeight = 4114
            Image2DMaxWidth = 4113
            Image3DMaxDepth = 4117
            Image3DMaxHeight = 4116
            Image3DMaxWidth = 4115
            ImageSupport = 4118
            LocalMemSize = 4131
            LocalMemType = 4130
            MaxClockFrequency = 4108
            MaxComputeUnits = 4098
            MaxConstantArgs = 4129
            MaxConstantBufferSize = 4128
            MaxMemAllocSize = 4112
            MaxParameterSize = 4119
            MaxReadImageArgs = 4110
            MaxSamplers = 4120
            MaxWorkGroupSize = 4100
            MaxWorkItemDimensions = 4099
            MaxWorkItemSizes = 4101
            MaxWriteImageArgs = 4111
            MemBaseAddrAlign = 4121
            MinDataTypeAlignSize = 4122
            Name = 4139
            Platform = 4145
            PreferredVectorWidthChar = 4102
            PreferredVectorWidthDouble = 4107
            PreferredVectorWidthFloat = 4106
            PreferredVectorWidthInt = 4104
            PreferredVectorWidthLong = 4105
            PreferredVectorWidthShort = 4103
            Profile = 4142
            ProfilingTimerResolution = 4133
            QueueProperties = 4138
            SingleFPConfig = 4123
            Type = 4096
            Vendor = 4140
            VendorID = 4097
            Version = 4143
        End Enum

        Public Enum CLError
            ' Fields
            BuildProgramFailure = -11
            DeviceCompilerNotAvailable = -3
            DeviceNotAvailable = -2
            DeviceNotFound = -1
            ImageFormatMismatch = -9
            ImageFormatNotSupported = -10
            InvalidArgIndex = -49
            InvalidArgSize = -51
            InvalidArgValue = -50
            InvalidBinary = -42
            InvalidBufferSize = -61
            InvalidBuildOptions = -43
            InvalidCommandQueue = -36
            InvalidContext = -34
            InvalidDevice = -33
            InvalidDeviceType = -31
            InvalidEvent = -58
            InvalidEventWaitList = -57
            InvalidGlobalOffset = -56
            InvalidGlobalWorkSize = -63
            InvalidGLObject = -60
            InvalidHostPtr = -37
            InvalidImageFormatDescriptor = -39
            InvalidImageSize = -40
            InvalidKernel = -48
            InvalidKernelArgs = -52
            InvalidKernelDefinition = -47
            InvalidKernelName = -46
            InvalidMemObject = -38
            InvalidMipLevel = -62
            InvalidOperation = -59
            InvalidPlatform = -32
            InvalidProgram = -44
            InvalidProgramExecutable = -45
            InvalidQueueProperties = -35
            InvalidSampler = -41
            InvalidValue = -30
            InvalidWorkDimension = -53
            InvalidWorkGroupSize = -54
            InvalidWorkItemSize = -55
            MapFailure = -12
            MemCopyOverlap = -8
            MemObjectAllocationFailure = -4
            OutOfHostMemory = -6
            OutOfResources = -5
            ProfilingInfoNotAvailable = -7
            Success = 0
        End Enum

        <DllImport("OpenCL")> _
        Public Shared Function clGetDeviceInfo(ByVal device As myOCL.oclAPI.CLDeviceID, ByVal param_name As CLDeviceInfo, ByVal param_value_size As SizeT, <Out()> ByVal param_value As Byte(), ByRef param_value_size_ret As SizeT) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetDeviceInfo(ByVal device As myOCL.oclAPI.CLDeviceID, ByVal param_name As CLDeviceInfo, ByVal param_value_size As SizeT, ByVal param_value As IntPtr, ByRef param_value_size_ret As SizeT) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetDeviceIDs(ByVal platform_id As CLPlatformID, ByVal device_type As CLDeviceType, ByVal num_entries As UInt32, ByVal devices As IntPtr, ByRef num_devices As UInt32) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetDeviceIDs(ByVal platform_id As CLPlatformID, ByVal device_type As CLDeviceType, ByVal num_entries As UInt32, <Out()> ByVal devices As CLDeviceID(), ByRef num_devices As UInt32) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetPlatformIDs(ByVal num_entries As UInt32, <Out()> ByVal platforms As CLPlatformID(), ByRef num_platforms As UInt32) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetPlatformIDs(ByVal num_entries As UInt32, ByVal platforms As IntPtr, ByRef num_platforms As UInt32) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetPlatformInfo(ByVal platform As CLPlatformID, ByVal param_name As CLPlatformInfo, ByVal param_value_size As SizeT, <Out()> ByVal param_value As Byte(), ByRef param_value_size_ret As SizeT) As CLError
        End Function
        <DllImport("OpenCL")> _
        Public Shared Function clGetPlatformInfo(ByVal platform As CLPlatformID, ByVal param_name As CLPlatformInfo, ByVal param_value_size As SizeT, ByVal param_value As IntPtr, ByRef param_value_size_ret As SizeT) As CLError
        End Function

    End Structure
End Namespace



