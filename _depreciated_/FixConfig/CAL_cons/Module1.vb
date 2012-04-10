Imports CAL_cons.myCAL
Imports System.Runtime.InteropServices

Module Module1
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
                'Check for CAL.init hangs with external console app

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
                cError = CALRuntimeX86.calGetErrorString
            Catch ex As Exception
                cError = ex.Message
            End Try
        End Sub
        Private Function Get_calCount(ByVal RunTime As eRunTime) As Int32
            On Error Resume Next
            Dim iCount As Int32 = 0
            If CALRuntimeX86.calDeviceGetCount(iCount) <> CALresult.CAL_RESULT_OK Then
                SetcalError(eRunTime.X86)
                Return -1
            End If
            Return iCount
        End Function
        Public Sub Close()
            cResX86 = CALRuntimeX86.calShutdown
            bX86 = (cResX86 = CALresult.CAL_RESULT_OK Or cResX86 = CALresult.CAL_RESULT_ALREADY)
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
            Return False
        End Try
    End Function
    Sub Main()
        Console.WriteLine("CAL_INIT=" & cal_Init.ToString)
        'Use loop to simulate hang, tCheck time interval should be enough to catch a long init and enumeration ( I left in all the code, as I can't test what exactly causes the hang this is the safest method
        'Do
        '    Threading.Thread.Sleep(50)
        'Loop
    End Sub

End Module
