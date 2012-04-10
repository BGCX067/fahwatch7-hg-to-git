Imports HWInfo.clsHWInfo
Imports gpuInfo
Imports gpuInfo.gpuInfo.Main
Imports lspci
Imports System.Text

Module modMain
    Public LogWindow As New clsLogwindow
    Public ClientConfig As New clsClientConfig
    Public ClientInfo As New clsClientInfo.Info
    Public HWInfo As New HWInfo.clsHWInfo.cHWInfo
    Public lsPCI As New clsPci
    Public Sub main()
        Try
            LogWindow.CreateLog()
            AddHandler HWInfo.Log, AddressOf WriteLog
            AddHandler HWInfo.LogError, AddressOf WriteError
            If HWInfo.Init Then
                If ClientConfig.ReadFAHClientConfig(False, True) Then
                    If lsPCI.FillInfo Then
                        ClientInfo = clsClientInfo.Info.Parse
                        ''Create backup
                        'If My.Computer.FileSystem.FileExists(ClientConfig.Configuration.DataLocation & "\config.xml.backup") Then
                        '    My.Computer.FileSystem.DeleteFile(ClientConfig.Configuration.DataLocation & "\config.xml.backup", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.DoNothing)
                        'End If
                        'My.Computer.FileSystem.CopyFile(ClientConfig.Configuration.DataLocation & "\config.xml", ClientConfig.Configuration.DataLocation & "\config.xml.backup")
                        Dim nGpuSlots As New List(Of clsClientConfig.clsConfiguration.sSlot) ' Need a new index so wrong entries may be removed.
                        For Each Slot In ClientConfig.Configuration.slots
                            If Slot.type <> "GPU" Then
                                Slot.id = nGpuSlots.Count
                                nGpuSlots.Add(Slot)
                            End If
                        Next
                        'enum lspci entries in order
                        Dim iATIOPENCL As Int16 = -1, iATICAL As Int16 = -1, iNVIDIA As Int16 = -1, iFERMI As Int16 = -1
                        'Check runtime entries for cards ( use openCL for simplicity )
                        For pciIndex As Int16 = 0 To lsPCI.Entries.Count - 1
                            Dim pciEntry As lspci.clsPci.clsEntries = lsPCI.Entries(pciIndex)
                            If pciEntry.VendorID = "1002" Then 'Ati
                                'Check deviceID for likely actual compute device and probable compute index
                                Dim bHas As Boolean = False
                                For Each adlAdapter As HWInfo.clsHWInfo.cHWInfo.cOHMInterface.ohmADL.clsAdapter In HWInfo.ohmInterface.ADL.Adapter
                                    If adlAdapter.DeviceID.ToUpper.Contains(pciEntry.DeviceID.ToUpper) Then
                                        bHas = True
                                        Exit For
                                    End If
                                Next
                                If bHas Then 'ATI:3 = cal / fahcore_11 ATI:4 = openCL / fahcore_16, check FAHClientInfo
                                    Dim nSlot As New clsClientConfig.clsConfiguration.sSlot
                                    nSlot.id = nGpuSlots.Count
                                    nSlot.AddArgument("gpu-index", pciIndex.ToString)
                                    nSlot.type = "GPU"
                                    If ClientInfo.GPU(pciIndex).Description.ToUpper.Contains("ATI:3") Then
                                        iATICAL += 1
                                        nSlot.AddArgument("cuda-index", iATICAL.ToString) 'still assuming cal uses cuda-index as it appears from my own config
                                    Else
                                        iATIOPENCL += 1
                                        nSlot.AddArgument("opencl-index", iATIOPENCL.ToString)
                                    End If
                                    'Check for custom slot arguments other then opencl and cuda index or gpuindex 
                                    For Each oldSlot In ClientConfig.Configuration.slots
                                        If oldSlot.type = "GPU" Then
                                            If oldSlot.HasKey("gpu-index") AndAlso oldSlot.GetValue("gpu-index") = pciIndex.ToString Then
                                                For Each sArgument In oldSlot.Keys
                                                    If sArgument <> "gpu-index" And sArgument <> "cuda-index" And sArgument <> "opencl-argument" And sArgument <> "type" And sArgument <> "id" Then
                                                        nSlot.AddArgument(sArgument, oldSlot.GetValue(sArgument))
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Next
                                    'Check DeviceName against openCL index
                                    Try
                                        For xInt As Int16 = 0 To HWInfo.gpuInf.oclInfo.NumberOfPlatforms
                                            With HWInfo.gpuInf.oclInfo.Platform(xInt)
                                                If Not .Name.ToUpper.Contains("NVIDIA") Then
                                                    If ClientInfo.GPU(pciIndex).Description.ToUpper.Contains("ATI:3") Then
                                                        'Check with iatical
                                                        If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iATICAL + 1).DeviceAttribute("Name").Replace("ATI", "").Trim) Then
                                                            nGpuSlots.Add(nSlot)
                                                            Exit Try
                                                        Else
                                                            'If iatical = 0 then find fist matching device name, then second ect
                                                            Dim iNewIndex As Int16 = -1
                                                            For iFound As Int16 = iATICAL + 1 To .NumberOfDevices
                                                                If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iFound).DeviceAttribute("Name").Replace("ATI", "").Trim) Then
                                                                    If iATICAL <= iFound Then
                                                                        iNewIndex = iFound - 1
                                                                    End If
                                                                End If
                                                            Next
                                                            If iNewIndex = -1 Then
                                                                MsgBox("Needs work, sorry")
                                                                End
                                                            Else
                                                                nSlot.ChangeKey("cuda-index", iNewIndex.ToString)
                                                                nGpuSlots.Add(nSlot)
                                                                Exit Try
                                                            End If
                                                        End If
                                                    Else
                                                        'Check with iatiopencl
                                                        If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iATIOPENCL + 1).DeviceAttribute("Name").Replace("ATI", "").Trim) Then
                                                            nGpuSlots.Add(nSlot)
                                                            Exit Try
                                                        Else
                                                            'If iatiopencl = 0 then find fist matching device name, then second ect
                                                            Dim iNewIndex As Int16 = -1
                                                            For iFound As Int16 = iATIOPENCL To .NumberOfDevices
                                                                If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iFound).DeviceAttribute("Name").Replace("ATI", "").Trim) Then
                                                                    If iATIOPENCL <= iFound Then
                                                                        iNewIndex = iFound - 1
                                                                    End If
                                                                End If
                                                            Next
                                                            If iNewIndex = -1 Then
                                                                MsgBox("Needs work, sorry")
                                                                End
                                                            Else
                                                                nSlot.ChangeKey("opencl-index", iNewIndex.ToString)
                                                                nGpuSlots.Add(nSlot)
                                                                Exit Try
                                                            End If
                                                        End If
                                                    End If
                                                    Exit Sub
                                                End If
                                            End With
                                        Next
                                    Catch ex As Exception
                                        LogWindow.WriteError(ex.Message, Err)
                                        MsgBox("Error, can't continue sorry :(")
                                        End
                                    End Try
                                Else
                                    'FAHClient probably needs to remove this deviceID from whitelist, let's forget about this slot
                                End If
                            Else                                'Nvidia
                                Dim bHas As Boolean = False
                                For Each nvAdapter As HWInfo.clsHWInfo.cHWInfo.cOHMInterface.ohmNVAPI.clsAdapter In HWInfo.ohmInterface.NVAPI.Adapter
                                    If nvAdapter.DeviceID.ToUpper.Contains(pciEntry.DeviceID.ToUpper) Then
                                        bHas = True
                                        Exit For
                                    End If
                                Next
                                If bHas Then 'NVIDIA for core_11 / FERMI core_15/16?
                                    Dim nSlot As New clsClientConfig.clsConfiguration.sSlot
                                    nSlot.id = nGpuSlots.Count
                                    nSlot.AddArgument("gpu-index", pciIndex.ToString)
                                    nSlot.type = "GPU"
                                    If ClientInfo.GPU(pciIndex).Description.ToUpper.Contains("NVIDIA:1") Then
                                        iNVIDIA += 1
                                        nSlot.AddArgument("cuda-index", iNVIDIA.ToString) 'cuda-index
                                    Else
                                        iFERMI += 1
                                        nSlot.AddArgument("opencl-index", iFERMI.ToString)
                                    End If
                                    'Check for custom slot arguments other then opencl and cuda index or gpuindex 
                                    For Each oldSlot In ClientConfig.Configuration.slots
                                        If oldSlot.type = "GPU" Then
                                            If oldSlot.HasKey("gpu-index") AndAlso oldSlot.GetValue("gpu-index") = pciIndex.ToString Then
                                                For Each sArgument In oldSlot.Keys
                                                    If sArgument <> "gpu-index" And sArgument <> "cuda-index" And sArgument <> "opencl-argument" And sArgument <> "type" And sArgument <> "id" Then
                                                        nSlot.AddArgument(sArgument, oldSlot.GetValue(sArgument))
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Next
                                    'Check DeviceName against openCL index
                                    Try
                                        For xInt As Int16 = 0 To HWInfo.gpuInf.oclInfo.NumberOfPlatforms
                                            With HWInfo.gpuInf.oclInfo.Platform(xInt)
                                                If .Name.ToUpper.Contains("NVIDIA") Then
                                                    If ClientInfo.GPU(pciIndex).Description.ToUpper.Contains("NVIDIA:1") Then
                                                        'Check with iatical
                                                        If pciEntry.DeviceDescription.ToUpper.Contains(.Device_ByIndex(iNVIDIA + 1).DeviceAttribute("Name").ToUpper) Then
                                                            nGpuSlots.Add(nSlot)
                                                            Exit Try
                                                        Else
                                                            'if invidia = 0 then find first matching device index, then second ect and correct index
                                                            Dim iNewIndex As Int16 = -1
                                                            For iFound As Int16 = iNVIDIA + 1 To .NumberOfDevices
                                                                If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iFound).DeviceAttribute("Name").ToUpper) Then
                                                                    If iNVIDIA <= iFound Then
                                                                        iNewIndex = iFound - 1
                                                                    End If
                                                                End If
                                                            Next
                                                            If iNewIndex = -1 Then
                                                                MsgBox("Needs work, sorry")
                                                                End
                                                            Else
                                                                nSlot.ChangeKey("cuda-index", iNVIDIA.ToString)
                                                                nGpuSlots.Add(nSlot)
                                                                Exit Try
                                                            End If
                                                        End If
                                                    Else
                                                        'Check with iatiopencl
                                                        If pciEntry.DeviceDescription.ToUpper.Contains(.Device_ByIndex(iFERMI + 1).DeviceAttribute("Name")) Then
                                                            nGpuSlots.Add(nSlot)
                                                            Exit Try
                                                        Else
                                                            'if iFermi = 0 then find first matching device index, then second ect and correct index
                                                            Dim iNewIndex As Int16 = -1
                                                            For iFound As Int16 = iFERMI + 1 To .NumberOfDevices
                                                                If pciEntry.DeviceDescription.Contains(.Device_ByIndex(iFound).DeviceAttribute("Name").ToUpper) Then
                                                                    If iFERMI <= iFound Then
                                                                        iNewIndex = iFound - 1
                                                                    End If
                                                                End If
                                                            Next
                                                            If iNewIndex = -1 Then
                                                                MsgBox("Needs work, sorry")
                                                                End
                                                            Else
                                                                nSlot.ChangeKey("opencl-index", iNewIndex.ToString)
                                                                nGpuSlots.Add(nSlot)
                                                                Exit Try
                                                            End If
                                                        End If
                                                    End If
                                                    Exit Sub
                                                End If
                                            End With
                                        Next
                                    Catch ex As Exception
                                        LogWindow.WriteError(ex.Message, Err)
                                        MsgBox("Error, can't continue sorry :(")
                                        End
                                    End Try
                                Else
                                    'FAHClient probably needs to remove this deviceID from whitelist, let's forget about this slot
                                End If
                            End If
                        Next
                        'NgpuSlots check
                        
                        ClientConfig.Configuration.ConfigString = ClientConfig.Configuration.ConfigString.Substring(0, ClientConfig.Configuration.ConfigString.IndexOf("  <!-- Folding Slots -->") + Len("  <!-- Folding Slots -->") + 1)
                        'Make sure folding slot configuration is correct.
                        For Each cfgSection In ClientConfig.Configuration.ConfigSections
                            If cfgSection.Name = " Folding Slot Configuration " Then
                                If iATICAL <> -1 Then
                                    If cfgSection.HasKey("cuda-index") Then
                                        cfgSection.ChangeKey("cuda-index", "8")
                                    Else
                                        cfgSection.AddArgument("cuda-index", "8")
                                    End If
                                ElseIf iATIOPENCL <> -1 Then
                                    If cfgSection.HasKey("opencl-index") Then
                                        cfgSection.ChangeKey("opencl-index", "8")
                                    Else
                                        cfgSection.AddArgument("opencl-index", "8")
                                    End If
                                ElseIf iNVIDIA <> -1 Or iFERMI <> -1 Then
                                    If cfgSection.HasKey("cuda-index") Then
                                        cfgSection.ChangeKey("cuda-index", "8")
                                    Else
                                        cfgSection.AddArgument("cuda-index", "8")
                                    End If
                                End If
                            End If
                        Next
                        Dim cSection As New StringBuilder
                        For Each Slot In nGpuSlots
                            cSection.AppendLine("  <slot id='" & Slot.id & "' type='" & Slot.type.ToUpper & "'/>")
                            If Slot.Keys.Count > 0 Then
                                For Each Key In Slot.Keys
                                    cSection.AppendLine("    <" & Key & " v='" & Slot.GetValue(Key) & "'/>")
                                Next
                                cSection.AppendLine("  </slot>")
                            End If
                        Next
                        cSection.AppendLine("</config>")
                        ClientConfig.Configuration.ConfigString &= cSection.ToString
                        My.Computer.FileSystem.WriteAllText(ClientConfig.Configuration.DataLocation & "\config.xml.proposed", ClientConfig.Configuration.ConfigString, True)
                        MsgBox(ClientConfig.Configuration.ConfigString & vbNewLine & vbNewLine & "Has been saved to config.xml.proposed")
                        Process.Start(ClientConfig.Configuration.DataLocation)
                    Else
                        MsgBox("Could not get -lspci output from client, sorry :(")
                    End If
                Else
                    MsgBox("Could not get Client configuration, sorry :(")
                End If
            Else
                MsgBox("Failed to get hardware info, sorry :(")
            End If
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            LogWindow.CloseLog()
            MsgBox("Unexpected error occured!" & vbNewLine & "Please inform the author of this program and provide the info in Error.txt", vbCritical & vbOKOnly, "Error!")
        End Try
    End Sub

    Public Sub WriteLog(ByVal Message As String)
        LogWindow.WriteLog(Message)
    End Sub
    Public Sub WriteError(ByVal Message As String, ByVal err As ErrObject)
        LogWindow.WriteError(Message, err)
    End Sub
End Module
