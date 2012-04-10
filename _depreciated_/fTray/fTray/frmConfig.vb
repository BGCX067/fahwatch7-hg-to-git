'   fTray ConsoleConfig class
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
Imports System.IO
Imports System.Configuration
Imports Microsoft.Win32
Imports System.Threading
Imports System.Management
Imports System.ServiceProcess
Imports Microsoft.Win32.Registry
Public Class frmConfig
#Region "clsClientConfig substitute"
    Public Class clsClientInfo
        Public ClientExe As String
        Public ClientLocation As String
        Public RecommendedParameters As String
        Public Enum eClientType
            Smp = 1
            Classic = 2
            Gpu = 3
        End Enum
        Public TypeOfClient As eClientType
        Public Class clsPandeGroup
            Public AcceptedWUSize
            Public AdditionalParameters
            Public AskNetwork
            Public CheckpointInterval
            Public CorePriority
            Public CpuUsage
            Public DisableAffinitylock
            Public DisableAssembly
            Public ForceAdvMethods
            Public IgnoreDeadline
            Public IPAdress
            Public MachineID
            Public MemoryUsage
            Public PassKey
            Public PauseBattery
            Public ProxyHost
            Public ProxyPassword
            Public ProxyPort
            Public ProxyUserName
            Public TeamNumber
            Public UseProxy
            Public UseProxyPassword
            Public UserName
        End Class
        Public PandeGroup As clsPandeGroup
    End Class
#End Region
#Region "Process - streamreaders/writers and events"
    Public Enum eConfigError
        Succes = 0
        UserName = 1
        Team = 2
        Passkey = 3
        AskNet = 4
        UseProxy = 5
        ProxyHost = 6
        ProxyPort = 7
        UseProxyPassword = 8
        ProxyUserName = 9
        ProxyPassword = 10
        WuSize = 11
        AddRemoveService = 12
        CorePriority = 13
        CpuUsage = 14
        DisableAssembly = 15
        PauseBattery = 16
        CPInterval = 17
        MemIndication = 18
        IgnoreDeadline = 19
        MachineID = 20
        CpuAffinity = 21
        Parameters = 22
        IPadress = 23
        AdvMethods = 24
    End Enum
    Private _Cerror As eConfigError = eConfigError.Succes
    Private cancelprocessing As Boolean = False
    Public WithEvents pConfigure As New Process
    Public pWriter As StreamWriter, pReader As StreamReader
    Delegate Sub Addoutput(ByVal [text] As String)
    Private rString As String = vbNullString
    Private _IsRunning As Boolean = False
    Public Enum eDoWhat
        DunKnow
        StartConfig
        DoConfig
    End Enum
    Public DoWhatWhenRun As eDoWhat = eDoWhat.DunKnow
    Public Class sPGroup
        Public Enum eType
            CoreFirst = 1
            ServiceFirst = 2
        End Enum
        Public Enum eAddRemove
            Keep = 0
            Manual = 1
            Service = 2
        End Enum
        Private _cType As clsClientInfo.eClientType
        Public Property GUIType() As clsClientInfo.eClientType
            Get
                Return _cType
            End Get
            Set(ByVal value As clsClientInfo.eClientType)
                _cType = value
            End Set
        End Property
        Private _AddRemove As eAddRemove
        Public Property ServiceCheck() As eAddRemove
            Get
                Return _AddRemove
            End Get
            Set(ByVal value As eAddRemove)
                _AddRemove = value
            End Set
        End Property
        Private _IsService As Boolean = False
        Public ReadOnly Property ServiceMode() As Boolean
            Get
                Try
                    Dim mID As String = "0"
                    If My.Computer.FileSystem.FileExists(ClientPath & "\client.cfg") Then
                        Dim fText As String = My.Computer.FileSystem.ReadAllText(ClientPath & "\client.cfg")
                        Dim lText() As String = fText.Split(vbLf)
                        For Each cString As String In lText
                            If cString.ToUpper.Contains("MACHINEID") Then
                                mID = cString.ToUpper.Replace("MACHINEID=", "")
                                Exit For
                            End If
                        Next
                        If mID = "0" Then Return False
                    End If
                    Dim sString As String
                    If _cType < 5 Then
                        sString = "Folding@Home-CPU-[" & mID & "]"
                    Else
                        sString = "Folding@Home-GPU-[" & mID & "]"
                    End If
                    Dim rKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services")
                    Dim sVal() As String = rKey.GetSubKeyNames
                    For Each sSer As String In sVal
                        If sSer.ToUpper = sString.ToUpper Then
                            Return True
                        End If
                    Next
                    Return False
                Catch ex As Exception
                    LogWindow.WriteError("sPG_Servicemode", Err)
                    Debug.Print(ex.Message)
                    Return Nothing
                End Try
            End Get
        End Property
        Public ClientEXE As String
        Public ReadOnly Property ClientPath As String
            Get
                Return Mid(ClientEXE, 1, ClientEXE.LastIndexOf("\"))
            End Get
        End Property
        Public ClientType As eType
        Public ClientVersion As String = ""
        Public Name As String = ""
        Public Team As String = ""
        Public PassKey As String = ""
        Public AskNet As String = ""
        Public UseProxy As String = ""
        Public UseProxyPass As String = ""
        Public ProxyName As String = ""
        Public ProxyPort As String = ""
        Public ProxyUserName As String = ""
        Public ProxyPassword As String = ""
        Public WUSize As String = ""
        Public CorePriority As String = ""
        Public CpuUsage As String = ""
        Public DissableAssembly As String = ""
        Public PauseBattery As String = ""
        Public CPInterval As String = ""
        Public MemoryIndication As String = ""
        Public AdvMethods As String = ""
        Public IgnoreDeadline As String = ""
        Public MachineID As String = ""
        Public AddRemoveService As String = ""
        Public DisableAffinty As String = ""
        Public Parameters As String = ""
        Public IPadress As String = ""
        Public ClientArguments As String = ""
        'End first section
    End Class
    Private pGR As New sPGroup
    Private pgW As New sPGroup
    Private pgBackUp As New sPGroup
    Private _bDoRestart As Boolean = False
    Public Property PandeGroup() As sPGroup
        Get
            Return pGR
        End Get
        Set(ByVal value As sPGroup)
            pGR = value
        End Set
    End Property
    ReadOnly Property pRunning() As Boolean
        Get
            Return _IsRunning
        End Get
    End Property
    Private Sub SetStartUpInfo()
        Try
            _InvalidParams = False
            _SMPerror = False
            _ConfigOutputReady = False
            rtf.Text = ""
            Application.DoEvents()
            pConfigure = New Process
            With pConfigure.StartInfo
                .FileName = Me.Client.ClientExe
                .Arguments = "-configonly"
                .UseShellExecute = False
                .CreateNoWindow = True
                .WorkingDirectory = Me.Client.ClientLocation
                .RedirectStandardInput = True
                .RedirectStandardOutput = True
                .StandardOutputEncoding = System.Text.Encoding.Default
            End With
            pConfigure.Start()
            _ConfigID = pConfigure.Id
            _IsRunning = True
            pWriter = pConfigure.StandardInput
            pConfigure.BeginOutputReadLine()
            While Not rtf.Text.ToUpper.Contains("CONFIGURING FOLDING@HOME") And Not _SMPerror
                Application.DoEvents()
            End While
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub pConfigure_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles pConfigure.Disposed
        _IsRunning = False
        _ConfigID = -1
    End Sub
    Private Sub pConfigure_ErrorDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles pConfigure.ErrorDataReceived
        If rtf.InvokeRequired Then
            rString = e.Data
            Dim tSafe As New Addoutput(AddressOf AddRtfOutput)
            Me.Invoke(tSafe, New Object() {[rString]})
        Else
            rtf.Text = rtf.Text & DateTime.Now.ToLongTimeString & " - " & e.Data & vbNewLine
        End If
    End Sub
    Private Sub pConfigure_Exited(ByVal sender As Object, ByVal e As System.EventArgs) Handles pConfigure.Exited
        _IsRunning = False
    End Sub
    Private Shared _SMPerror As Boolean = False
    Private Shared _InvalidParams As Boolean = False
    Private Sub pConfigure_OutputDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs) Handles pConfigure.OutputDataReceived
        Try
            If e.Data.ToUpper.Contains("CONFIGONLY FLAG GIVEN") Then
                _ConfigOutputReady = True
            ElseIf e.Data.ToUpper.Contains("PLEASE CHECK YOUR CONFIGURATION") Or e.Data.ToUpper.Contains("SOCK ERROR") And Not e.Data.ToUpper.Contains("sock error: generic socket failure, error stack:".ToUpper) Then
                _ConfigOutputReady = True
                _SMPerror = True
            ElseIf e.Data.ToString.ToString.ToUpper.Contains("Invalid parameters entered.".ToUpper) Then
                _InvalidParams = True
            End If
        Catch ex As Exception
        End Try
        Try
            If rtf.InvokeRequired Then
                rString = e.Data
                Dim tSafe As New Addoutput(AddressOf AddRtfOutput)
                Me.Invoke(tSafe, New Object() {[rString]})
            Else
                rtf.Text = rtf.Text & e.Data & vbNewLine
            End If
        Catch ex As Exception
        End Try

    End Sub
    Private Shared _ConfigOutputReady As Boolean = False
    Private Sub AddRtfOutput(ByVal [text] As String)
        rtf.Text = rtf.Text & DateTime.Now.ToLongTimeString & " - " & [text] & vbNewLine
        Debug.WriteLine(text)
    End Sub
    Private Sub ReadSettings(Optional ByVal DoGui As Boolean = True)
        Dim sOldS As String = lblS.Text
        gbC.Enabled = False
        If DoGui Then lblS.Text = "Getting standard settings!"
        Application.DoEvents()
        Dim bService As Boolean = pgBackUp.ServiceMode
        Try
            If _IsRunning Then Exit Sub
            SetStartUpInfo()
            'newline for reading up to advanced options
            While Not pConfigure.HasExited
                pWriter.WriteLine("")
                WaitMS(10)
            End While
            While Not _ConfigOutputReady
                Application.DoEvents()
            End While
            WaitMS(0)
            If rtf.Text.ToUpper.Contains("PLEASE CHECK YOUR CONFIGURATION") Or rtf.Text.ToUpper.Contains("SOCK ERROR") And Not rtf.Text.ToUpper.Contains("Folding@Home Client Version 6.3".ToUpper) Then
                _SMPerror = True
            End If
            If _InvalidParams Then GoTo InvalidP
            If _SMPerror Then GoTo NoSMP
            'Check current settings
            Dim vString As String = rtf.Text.ToString 'Getting values as enterd
            Dim pStart As Integer = vString.ToUpper.IndexOf("CONFIGURING FOLDING@HOME")
            Dim pEnd As Integer = vString.ToUpper.IndexOf("CONFIGONLY FLAG GIVEN")
            Dim HasAdvanced As Boolean = (vString.ToUpper.IndexOf("CORE PRIORITY") <> -1)
            vString = Mid(vString, pStart)
            If pStart = 0 Or pEnd = 0 Then
                'MsgBox("error")
                Me._Done = True
                Me.Close()
                Exit Sub
            ElseIf vString.ToUpper.IndexOf("USER NAME") <> 0 Then
                Dim uLen As Integer
                'name
                Dim uS1 As Integer = vString.ToUpper.IndexOf("USER NAME") + Len("user name")
                Dim uS2 As Integer = vString.IndexOf("[", uS1)
                Dim uE As Integer = vString.IndexOf("]", uS2)
                If Not uE - uS2 = 1 Then
                    uS2 = uS2 + 2
                    uLen = (uE - uS2) + 1
                    pGR.Name = Mid(vString, uS2, uLen)
                End If
                pStart = uS2
                'team
                uS1 = vString.ToUpper.IndexOf("TEAM NUMBER", pStart) + Len("team number")
                uS2 = vString.IndexOf("[", uS1)
                uE = vString.IndexOf("]", uS2)
                If Not uE - uS2 = 1 Then
                    uS2 = uS2 + 2
                    uLen = (uE - uS2) + 1
                    pGR.Team = Mid(vString, uS2, uLen)
                Else
                    pGR.Team = ""
                End If
                pStart = uS2
                'key
                uS1 = vString.ToUpper.IndexOf("PASSKEY", pStart)
                uS2 = vString.IndexOf("[", uS1)
                uE = vString.IndexOf("]", uS2)
                If Not uE - uS2 = 1 Then
                    uS2 = uS2 + 2
                    uLen = (uE - uS2) + 1
                    pGR.PassKey = Mid(vString, uS2, uLen)
                Else
                    pGR.PassKey = ""
                End If
                pStart = uS2
                If Not bService Then
                    'asknet
                    uS1 = vString.ToUpper.IndexOf("FETCHING/SENDING", pStart) + Len("fetching/sending")
                    uS2 = vString.IndexOf("[", uS1) + 2
                    uE = vString.IndexOf("]", uS2)
                    uLen = (uE - uS2) + 1
                    pGR.AskNet = Mid(vString, uS2, uLen)
                    pStart = uS2
                Else
                    pGR.AskNet = "no"
                    cmbAskNet.Text = "no"
                    cmbAskNet.Enabled = False
                End If
                'UsePRoxy
                uS1 = vString.ToUpper.IndexOf("USE PROXY", pStart) + Len("USE PROXY")
                If vString.IndexOf("?", uS1) > vString.IndexOf("[", pStart) Then
                    pGR.UseProxy = "no"
                Else
                    uS2 = vString.IndexOf("[", uS1) + 2
                    uE = vString.IndexOf("]", uS2)
                    uLen = (uE - uS2) + 1
                    pGR.UseProxy = Mid(vString, uS2, uLen)
                End If
                pStart = uS2
                If pGR.UseProxy.ToUpper = "YES" Then
                    'pHost
                    uS1 = vString.ToUpper.IndexOf("PROXY NAME", pStart) + Len("PROXY NAME")
                    uS2 = vString.IndexOf("[", uS1) + 2
                    uE = vString.IndexOf("]", uS2)
                    uLen = (uE - uS2) + 1
                    pGR.ProxyName = Mid(vString, uS2, uLen)
                    pStart = uS2
                    'Port
                    uS1 = vString.ToUpper.IndexOf("PROXY PORT", pStart) + Len("PROXY PORT")
                    uS2 = vString.IndexOf("[", uS1) + 2
                    uE = vString.IndexOf("]", uS2)
                    uLen = (uE - uS2) + 1
                    pGR.ProxyPort = Mid(vString, uS2, uLen)
                    pStart = uS2
                    'UsePpass
                    uS1 = vString.ToUpper.IndexOf("PASSWORD WITH PROXY", pStart) + Len("PASSWORD WITH PROXY")
                    If vString.IndexOf("?", uS1) < vString.IndexOf("[", uS1) Then
                        pGR.UseProxyPass = "no"
                    Else
                        uS2 = vString.IndexOf("[", uS1) + 2
                        uE = vString.IndexOf("]", uS2)
                        uLen = (uE - uS2) + 1
                        pGR.UseProxyPass = Mid(vString, uS2, uLen)
                    End If
                    pStart = uS2
                    If pGR.UseProxyPass.ToUpper = "YES" Then
                        'Pusername
                        uS1 = vString.ToUpper.IndexOf("PROXY USERNAME", pStart) + Len("PROXY USERNAME")
                        uS2 = vString.IndexOf("[", uS1) + 2
                        uE = vString.IndexOf("]", uS2)
                        uLen = (uE - uS2) + 1
                        pGR.ProxyUserName = Mid(vString, uS2, uLen)
                        pStart = uS2
                        'Ppass
                        uS1 = vString.ToUpper.IndexOf("PROXY PASSWORD", pStart) + Len("PROXY PASSWORD")
                        uS2 = vString.IndexOf("[", uS1) + 2
                        uE = vString.IndexOf("]", uS2)
                        uLen = (uE - uS2) + 1
                        pGR.ProxyPassword = Mid(vString, uS2, uLen)
                        pStart = uS2
                    Else
                        pGR.ProxyUserName = ""
                        pGR.ProxyPassword = ""
                    End If
                Else
                    pGR.ProxyName = ""
                    pGR.ProxyPassword = ""
                    pGR.UseProxyPass = "no"
                    pGR.ProxyUserName = ""
                    pGR.UseProxy = "no"
                End If
                'Wu size
                uS1 = vString.ToUpper.IndexOf("ACCEPTABLE SIZE", pStart) + Len("ACCEPTABLE SIZE")
                uS2 = vString.IndexOf("[", uS1) + 2
                uE = vString.IndexOf("]", uS2)
                uLen = (uE - uS2) + 1
                pGR.WUSize = Mid(vString, uS2, uLen)
                pStart = uS2
            Else
                'Snippet of text is bad??
                'MsgBox("error!")
                GoTo NoInit
            End If
        Catch ex As Exception
            GoTo NoInit
        End Try
        If DoGui Then lblS.Text = "Getting advanced settings!"
        Try
            SetStartUpInfo()
            Dim dEnd As DateTime
            dEnd = DateTime.Now.AddSeconds(2)
            While DateTime.Now < dEnd
                Application.DoEvents()
            End While
            With pWriter
                .WriteLine("")   'name
                WaitMS(50)
                .WriteLine("")   'team   
                WaitMS(50)
                .WriteLine("")   'passkey
                WaitMS(50)
                If Not bService Then .WriteLine("") 'asknet
                WaitMS(50)
                .WriteLine("")   'use proxy
                WaitMS(50)
                If pGR.UseProxy.ToUpper = "YES" Then
                    .WriteLine("")   'host
                    WaitMS(50)
                    .WriteLine("")   'port
                    WaitMS(50)
                    .WriteLine("")   'use password
                    WaitMS(50)
                    If pGR.UseProxyPass.ToUpper = "YES" Then
                        .WriteLine("")   'user name
                        WaitMS(50)
                        .WriteLine("")   'password
                        WaitMS(50)
                    End If
                End If
                .WriteLine("") 'wu size
                WaitMS(50)
                .WriteLine("yes") 'Get advanced options
                WaitMS(50)
                While Not pConfigure.HasExited
                    pWriter.WriteLine("")
                End While
                While Not _ConfigOutputReady
                    Application.DoEvents()
                End While
            End With
            If _SMPerror Then GoTo NoSMP
            'rtf.text not filled??? ' use output flag
            Dim vString As String = rtf.Text.ToString 'Getting values as enterd
            Dim pStart As Integer = vString.ToUpper.IndexOf("CONFIGURING FOLDING@HOME")
            Dim pEnd As Integer = vString.ToUpper.IndexOf("CONFIGONLY FLAG GIVEN")
            Dim HasAdvanced As Boolean = (vString.ToUpper.IndexOf("CORE PRIORITY") <> -1)
            Dim s1 As Integer, s2 As Integer, uE As Integer, uLen As Integer
            If Not HasAdvanced Then
                GoTo NoGo
            End If
            If vString.ToUpper.IndexOf("CORE PRIORITY") < vString.ToUpper.IndexOf("SERVICE") Then
                'Gpu client, start with core priority
                pGR.ClientType = sPGroup.eType.CoreFirst
                s1 = vString.ToUpper.IndexOf("CORE PRIORITY")
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CorePriority = Mid(vString, s2, uLen)
                'cpu usage
                s1 = vString.ToUpper.IndexOf("CPU USAGE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CpuUsage = Mid(vString, s2, uLen)
                'cpu usage
                s1 = vString.ToUpper.IndexOf("ASSEMBLY CODE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.DissableAssembly = Mid(vString, s2, uLen)
                'Pause battery
                s1 = vString.ToUpper.IndexOf("PAUSE IF BATTERY", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.PauseBattery = Mid(vString, s2, uLen)
                'CPint
                s1 = vString.ToUpper.IndexOf("BETWEEN CHECKPOINTS", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CPInterval = Mid(vString, s2, uLen)
                'Memory
                s1 = vString.ToUpper.IndexOf("TO INDICATE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.MemoryIndication = Mid(vString, s2, uLen)
                'AdvMethods
                s1 = vString.ToUpper.IndexOf("SET -ADVMETHODS", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.AdvMethods = Mid(vString, s2, uLen)
                'Ignore deadline
                s1 = vString.ToUpper.IndexOf("IGNORE ANY DEADLINE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.IgnoreDeadline = Mid(vString, s2, uLen)
                'MachineID
                s1 = vString.ToUpper.IndexOf("MACHINE ID", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.MachineID = Mid(vString, s2, uLen)
                'AddRemove
                s1 = vString.ToUpper.IndexOf("SERVICE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.AddRemoveService = Mid(vString, s2, uLen)
                'Affinity lock
                s1 = vString.ToUpper.IndexOf("DISABLE CPU AFFINITY", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.DisableAffinty = Mid(vString, s2, uLen)
                'Params
                s1 = vString.ToUpper.IndexOf("CLIENT PARAMETERS", uE)
                s2 = vString.IndexOf("[", s1)
                uE = vString.IndexOf("]", s2)
                If uE - s2 = 1 Then
                    pGR.Parameters = ""
                Else
                    s2 += 2
                    uLen = (uE - s2) + 1
                    pGR.Parameters = Mid(vString, s2, uLen)
                End If
                'IP
                s1 = vString.ToUpper.IndexOf("IP ADDRESS", uE)
                s2 = vString.IndexOf("[", s1)
                uE = vString.IndexOf("]", s2)
                If uE - s2 = 1 Then
                    pGR.IPadress = ""
                Else
                    s2 += 2
                    uLen = (uE - s2) + 1
                    pGR.IPadress = Mid(vString, s2, uLen)
                End If
            Else
                'smp
                pGR.ClientType = sPGroup.eType.ServiceFirst
                'AddRemove
                s1 = vString.ToUpper.IndexOf("SERVICE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.AddRemoveService = Mid(vString, s2, uLen)
                s1 = vString.ToUpper.IndexOf("CORE PRIORITY")
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CorePriority = Mid(vString, s2, uLen)
                'cpu usage
                s1 = vString.ToUpper.IndexOf("CPU USAGE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CpuUsage = Mid(vString, s2, uLen)
                'cpu usage
                s1 = vString.ToUpper.IndexOf("ASSEMBLY CODE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.DissableAssembly = Mid(vString, s2, uLen)
                'Pause battery
                s1 = vString.ToUpper.IndexOf("PAUSE IF BATTERY", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.PauseBattery = Mid(vString, s2, uLen)
                'CPint
                s1 = vString.ToUpper.IndexOf("BETWEEN CHECKPOINTS", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.CPInterval = Mid(vString, s2, uLen)
                'Memory
                s1 = vString.ToUpper.IndexOf("TO INDICATE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.MemoryIndication = Mid(vString, s2, uLen)
                'AdvMethods
                s1 = vString.ToUpper.IndexOf("SET -ADVMETHODS", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.AdvMethods = Mid(vString, s2, uLen)
                'Ignore deadline
                s1 = vString.ToUpper.IndexOf("IGNORE ANY DEADLINE", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.IgnoreDeadline = Mid(vString, s2, uLen)
                'MachineID
                s1 = vString.ToUpper.IndexOf("MACHINE ID", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.MachineID = Mid(vString, s2, uLen)
                'Affinity lock
                s1 = vString.ToUpper.IndexOf("DISABLE CPU AFFINITY", uE)
                s2 = vString.IndexOf("[", s1) + 2
                uE = vString.IndexOf("]", s2)
                uLen = (uE - s2) + 1
                pGR.DisableAffinty = Mid(vString, s2, uLen)
                'Params
                s1 = vString.ToUpper.IndexOf("CLIENT PARAMETERS", uE)
                s2 = vString.IndexOf("[", s1)
                uE = vString.IndexOf("]", s2)
                If uE - s2 = 1 Then
                    pGR.Parameters = ""
                Else
                    s2 += 2
                    uLen = (uE - s2) + 1
                    pGR.Parameters = Mid(vString, s2, uLen)
                End If
                'IP
                s1 = vString.ToUpper.IndexOf("IP ADDRESS", uE)
                s2 = vString.IndexOf("[", s1)
                uE = vString.IndexOf("]", s2)
                If uE - s2 = 1 Then
                    pGR.IPadress = ""
                Else
                    s2 += 2
                    uLen = (uE - s2) + 1
                    pGR.IPadress = Mid(vString, s2, uLen)
                End If
            End If
            'Get type of client and client version
            Dim Lines() As String
            If rtf.Text.Contains(vbCrLf) Then
                Lines = rtf.Text.Split(vbCrLf)
            ElseIf rtf.Text.Contains(vbNewLine) Then
                Lines = rtf.Text.Split(vbNewLine)
            ElseIf rtf.Text.Contains(vbLf) Then
                Lines = rtf.Text.Split(vbLf)
            ElseIf rtf.Text.Contains(vbCr) Then
                Lines = rtf.Text.Split(vbCr)
            End If
            Dim bArg As Boolean = False, bVer As Boolean = False
            For xInd As Integer = Lines.GetUpperBound(0) To 0 Step -1
                With Lines(xInd).ToUpper
                    If .Contains("Arguments:".ToUpper) Then
                        bArg = True
                        pGR.ClientArguments = Mid(Lines(xInd).ToUpper, Lines(xInd).ToUpper.LastIndexOf("Arguments:".ToUpper) + Len("Arguments:") + 1)
                        pGR.ClientArguments = pGR.ClientArguments.Trim(vbCrLf).Replace("-CONFIGONLY", "")
                    ElseIf .Contains("Folding@Home Client Version".ToUpper) Then
                        bVer = True
                        pGR.ClientVersion = Mid((Lines(xInd).ToUpper), (Lines(xInd).ToUpper.IndexOf("Folding@Home Client Version".ToUpper) + Len("Folding@Home Client Version") + 1))
                        pGR.ClientVersion = pGR.ClientVersion.TrimEnd(vbLf).TrimEnd(vbNewLine).TrimEnd(vbCrLf)
                        pGR.ClientVersion = pGR.ClientVersion.Replace(" ", "")
                    End If
                    If bVer And bArg Then
                        LogWindow.WriteLog("Finished getting client info")
                        Exit For
                    End If
                End With
            Next
            Dim md5 As String = MD5CalcFile(Client.ClientExe)
            If md5 = md5_Classic Then
                pGR.GUIType = clsClientInfo.eClientType.Classic
            ElseIf md5 = md5_Gpu2VistaW7 Then
                pGR.GUIType = clsClientInfo.eClientType.Gpu
            ElseIf md5 = md5_Gpu2Xp03 Then
                pGR.GUIType = clsClientInfo.eClientType.Gpu
            ElseIf md5 = md5_Gpu3VistaW7 Then
                pGR.GUIType = clsClientInfo.eClientType.Gpu
            ElseIf md5 = md5_Gpu3Xp03 Then
                pGR.GUIType = clsClientInfo.eClientType.Gpu
            ElseIf md5 = md5_SMP2 Then
                pGR.GUIType = clsClientInfo.eClientType.Smp
            Else
                LogWindow.WriteLog("MD5 hash of client is not known, trying to parse log output")
                For xInt As Int16 = 0 To Lines.GetUpperBound(0)
                    If Lines(xInt).ToString.ToUpper.Contains("# Windows") Then
                        If Lines(xInt).ToUpper.Contains("GPU") Then
                            pGR.GUIType = clsClientInfo.eClientType.Gpu
                        ElseIf Lines(xInt).ToUpper.Contains("SMP") Then
                            pGR.GUIType = clsClientInfo.eClientType.Smp
                        ElseIf Lines(xInt).ToUpper.Contains("CPU") Then
                            pGR.GUIType = clsClientInfo.eClientType.Classic
                        Else
                            LogWindow.WriteLog("Client type could not be determined, using standard profile")
                            pGR.GUIType = clsClientInfo.eClientType.Classic
                        End If
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            GoTo NoInit
        End Try

        If DoGui Then ConfigureGUI()
        Exit Sub
NoInit:
        lblS.Text = "Err: " & Err.Description
        gbC.Enabled = True
        _ReadFailure = True
        Exit Sub
NoGo:
        lblS.Text = "Err: can not get advanced settings from console!"
        gbC.Enabled = True
        _ReadFailure = True
        Exit Sub
NoSMP:
        lblS.Text = "Err: smp has a problem!"
        gbC.Enabled = True
        Exit Sub
InvalidP:
        'reconfigure first
        lblS.Text = "Invalid parameters enterd, reconfigure needed"
        lblS.ForeColor = Color.Red
        gbC.Enabled = True
        Exit Sub
    End Sub
    Private _ReadFailure As Boolean = False
    Private Sub ConfigureGUI()
        lblS.Text = "Configuring GUI!"
        Application.DoEvents()
        Try
            cancelprocessing = True
            'Generate config form
            txtUserName.Text = pGR.Name
            txtTeamNumber.Text = pGR.Team
            'If Me.Client.PandeGroup.TeamNumber <> txtTeamNumber.Text Then txtTeamNumber.Text = Me.Client.PandeGroup.TeamNumber
            txtPassKey.Text = pGR.PassKey
            txtIP.Text = pGR.IPadress
            txtParam.Text = pGR.Parameters.Trim
            txtProxyName.Text = pGR.ProxyName
            txtProxyPort.Text = pGR.ProxyPort
            txtPusername.Text = pGR.ProxyUserName
            txtPpassword.Text = pGR.ProxyPassword
            cmbAskNet.Text = pGR.AskNet
            cmbProxy.Text = pGR.UseProxy
            cmbProxyPassword.Text = pGR.UseProxyPass
            'Check for service
            If pGR.ServiceMode Then
                lblService.Text = "Launch manually, remove  the service?"
            Else
                lblService.Text = "Launch automaticly, install as a service in this directory?"
            End If
            cmbService.Text = pGR.AddRemoveService
            cmbBigWu.Text = pGR.WUSize
            cmbCorePriority.Text = pGR.CorePriority
            nudCPUusage.Value = CInt(pGR.CpuUsage)
            nudMem.Maximum = CInt(My.Computer.Info.TotalPhysicalMemory / 1024 / 1024)
            nudMem.Value = CInt(pGR.MemoryIndication)
            cmbDisableAssembly.Text = pGR.DissableAssembly
            cmbBattery.Text = pGR.PauseBattery
            cmbAdv.Text = pGR.AdvMethods
            cmbLocalDeadlines.Text = pGR.IgnoreDeadline
            cmbCpuAffinity.Text = pGR.DisableAffinty
            'If pGR.MachineID <> Me.Client.PandeGroup.MachineID Then pGR.MachineID = Me.Client.PandeGroup.MachineID 'Override read settings for initial install
            cmbMachineID.Items.Clear()
            For x As Integer = 1 To 16
                cmbMachineID.Items.Add(x.ToString)
            Next
            If Not cmbMachineID.Items.Contains(pGR.MachineID) Then
                lblID.Text = "MachineID *Error*"
            Else
                lblID.Text = "MachineID"
            End If
            cmbMachineID.Text = pGR.MachineID
            'if gpu client disable install as service
            If pGR.GUIType = clsClientInfo.eClientType.Gpu Then
                lblService.Text = "Gpu client can not run as service"
                cmbService.Enabled = False
                cmbCorePriority.Text = "Low"
            End If
            If pGR.GUIType = clsClientInfo.eClientType.Smp Then
                'fill cmbcores with the number of cores, if more then 2?
                lblS.Text = "Use the combobox the set no. cores"
                cmbCores.Items.Clear()
                For xInt As Int16 = 2 To Environment.ProcessorCount
                    cmbCores.Items.Add(xInt.ToString)
                Next
                cmbCores.SelectedIndex = cmbCores.Items.Count - 1
                cmbCores.Visible = True
                lnblCores.Visible = True
            Else
                cmbCores.Visible = False
                lnblCores.Visible = False
                lblS.Text = "Ready"
            End If
        Catch ex As Exception
            GoTo NoInit
        End Try
        gbC.Enabled = True
        If NoCancel Then cmdCancel.Enabled = False
        Exit Sub
NoInit:
        lblS.Text = "Err: " & Err.Description
    End Sub
#End Region
#Region "Basic form handling"
    Private boolCancel As Boolean = False
    Public bCleanUp As Boolean = False
    Private _Done As Boolean = False
    Public ReadOnly Property CancelPressed() As Boolean
        Get
            Return boolCancel
        End Get
    End Property
    Public ReadOnly Property Done() As Boolean
        Get
            Return _Done
        End Get
    End Property
    Private _Client As New clsClientInfo  ' clsClientInfo
    Public Property Client() As clsClientInfo
        Get
            Try
                Return _Client
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(ByVal value As clsClientInfo)
            _Client = value
        End Set
    End Property
    Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Try
            If txtUserName.TextLength = 0 Or txtUserName.Text.ToUpper = "ANONYMOUS" Or txtPassKey.Text = "" Then
                Dim fuserName As New frmUseAnonymous
                Dim rVal As DialogResult = fuserName.ShowDialog
                If rVal = DialogResult.Cancel Then Exit Sub
            End If
            pgBackUp = pGR   'Back up to first read settings 
            gbC.Enabled = False
            With pgW
                .Name = txtUserName.Text
                .Team = txtTeamNumber.Text
                .PassKey = txtPassKey.Text
                .AskNet = cmbAskNet.Text
                .AddRemoveService = cmbService.Text
                If cmbService.Text = "yes" Then
                    If .ServiceMode Then
                        .ServiceCheck = sPGroup.eAddRemove.Manual
                    Else
                        .ServiceCheck = sPGroup.eAddRemove.Service
                    End If
                Else
                    .ServiceCheck = sPGroup.eAddRemove.Keep
                End If
                .AdvMethods = cmbAdv.Text
                .ClientType = pGR.ClientType
                .CorePriority = cmbCorePriority.Text
                .CPInterval = nudInterval.Value.ToString
                .CpuUsage = nudCPUusage.Value.ToString
                .DisableAffinty = cmbCpuAffinity.Text
                .DissableAssembly = cmbDisableAssembly.Text
                .IgnoreDeadline = cmbLocalDeadlines.Text
                .IPadress = txtIP.Text
                .CPInterval = nudInterval.Value.ToString
                'machine ID
                .MachineID = cmbMachineID.Text
                .MemoryIndication = nudMem.Value.ToString
                .Parameters = txtParam.Text.Trim
                txtParam.Text = txtParam.Text.Trim
                .PauseBattery = cmbBattery.Text
                .UseProxy = cmbProxy.Text
                .UseProxyPass = cmbProxyPassword.Text
                .ProxyName = txtProxyName.Text
                .ProxyPort = txtProxyPort.Text
                .ProxyPassword = txtPpassword.Text
                .ProxyUserName = txtPusername.Text
                .WUSize = cmbBigWu.Text
            End With
            'Try apply config
            Try
                _bDoRestart = False
                If MySettings.MySettings.SafeMode And ClientControl.ConsoleStatus = clsClientControl.eStatus.active Then
                    lblS.Text = "Stopping running client"
                    _bDoRestart = True
                    ClientControl.StopClient()
                    Do
                        WaitMS(500)
                    Loop While Not ClientControl.ConsoleStatus = clsClientControl.eStatus.stopped
                Else
                    _bDoRestart = False
                End If
            Catch ex As Exception : End Try
            lblS.Text = "Starting up.."
            SetStartUpInfo()
            WaitMS(500)
            With pWriter
                lblS.Text = "Configuring standard settings.."
                .WriteLine(pgW.Name)
                WaitMS(100)
                .WriteLine(pgW.Team)
                WaitMS(100)
                .WriteLine(pgW.PassKey)
                WaitMS(100)
                .WriteLine(pgW.AskNet)
                WaitMS(100)
                .WriteLine(pgW.UseProxy)
                WaitMS(100)
                If pgW.UseProxy.ToUpper = "YES" Then
                    .WriteLine(pgW.ProxyName)
                    WaitMS(100)
                    .WriteLine(pgW.ProxyPort)
                    WaitMS(100)
                    WriteLine(pgW.UseProxyPass)
                    WaitMS(100)
                    If pgW.UseProxyPass.ToUpper = "YES" Then
                        .WriteLine(pgW.ProxyUserName)
                        WaitMS(100)
                        .WriteLine(pgW.ProxyPassword)
                        WaitMS(100)
                    End If
                End If
                .WriteLine(pgW.WUSize)
                WaitMS(100)
                .WriteLine("yes") 'adv options
                WaitMS(100)
                lblS.Text = "Configuring advanced options.."
                If pgW.ClientType = sPGroup.eType.ServiceFirst Then
                    .WriteLine(pgW.AddRemoveService)
                    WaitMS(100)
                    .WriteLine(pgW.CorePriority)
                    WaitMS(100)
                    .WriteLine(pgW.CpuUsage)
                    WaitMS(100)
                    .WriteLine(pgW.DissableAssembly)
                    WaitMS(100)
                    .WriteLine(pgW.PauseBattery)
                    WaitMS(100)
                    .WriteLine(pgW.CPInterval)
                    WaitMS(250)
                    .WriteLine(pgW.MemoryIndication)
                    WaitMS(100)
                    .WriteLine(pgW.AdvMethods)
                    WaitMS(100)
                    .WriteLine(pgW.IgnoreDeadline)
                    WaitMS(100)
                    .WriteLine(pgW.MachineID)
                    WaitMS(100)
                    .WriteLine(pgW.DisableAffinty)
                    WaitMS(100)
                    .WriteLine(pgW.Parameters)
                    WaitMS(100)
                    .WriteLine(pgW.IPadress)
                    WaitMS(100)
                    While Not pConfigure.HasExited
                        .WriteLine("")
                        WaitMS(100)
                    End While
                Else
                    .WriteLine(pgW.CorePriority)
                    WaitMS(100)
                    .WriteLine(pgW.CpuUsage)
                    WaitMS(100)
                    .WriteLine(pgW.DissableAssembly)
                    WaitMS(100)
                    .WriteLine(pgW.PauseBattery)
                    WaitMS(100)
                    .WriteLine(pgW.CPInterval)
                    WaitMS(100)
                    .WriteLine(pgW.MemoryIndication)
                    WaitMS(100)
                    .WriteLine(pgW.AdvMethods)
                    WaitMS(100)
                    .WriteLine(pgW.IgnoreDeadline)
                    WaitMS(100)
                    .WriteLine(pgW.MachineID)
                    WaitMS(100)
                    .WriteLine(pgW.AddRemoveService)
                    WaitMS(100)
                    .WriteLine(pgW.DisableAffinty)
                    WaitMS(100)
                    .WriteLine(pgW.Parameters)
                    WaitMS(100)
                    .WriteLine(pgW.IPadress)
                    WaitMS(100)
                    While Not pConfigure.HasExited
                        .WriteLine("")
                        WaitMS(50)
                        Application.DoEvents()
                    End While
                    While Not _ConfigOutputReady
                        Application.DoEvents()
                    End While
                    WaitMS(500) 'give outputhandler some extra time
                    If rtf.Text.ToUpper.Contains("PLEASE CHECK YOUR CONFIGURATION") Or rtf.Text.ToUpper.Contains("SOCK ERROR") Then
                        _SMPerror = True
                    End If
                End If
            End With
            pGR = New sPGroup
            pGR.ClientEXE = _Client.ClientExe 'need this for service check!
            ReadSettings(True)
            gbC.Enabled = False
            lblS.Text = "Comparing.."
            Dim bEqual As Boolean = True
            If Not txtIP.Text = pgW.IPadress Then
                _Cerror = eConfigError.IPadress
                bEqual = False
            ElseIf Not txtParam.Text = pgW.Parameters Then
                _Cerror = eConfigError.Parameters
                bEqual = False
            ElseIf Not txtPassKey.Text = pgW.PassKey Then
                _Cerror = eConfigError.Passkey
                bEqual = False
            ElseIf Not txtPpassword.Text = pgW.ProxyPassword Then
                _Cerror = eConfigError.ProxyPassword
                bEqual = False
            ElseIf Not txtProxyName.Text = pgW.ProxyName Then
                _Cerror = eConfigError.ProxyHost
                bEqual = False
            ElseIf Not txtProxyPort.Text = pgW.ProxyPort Then
                _Cerror = eConfigError.ProxyPort
                bEqual = False
            ElseIf Not txtPusername.Text = pgW.ProxyUserName Then
                _Cerror = eConfigError.ProxyUserName
                bEqual = False
            ElseIf Not txtTeamNumber.Text = pgW.Team Then
                _Cerror = eConfigError.Team
                bEqual = False
            ElseIf Not txtUserName.Text = pgW.Name Then
                _Cerror = eConfigError.UserName
                bEqual = False
            ElseIf Not cmbAdv.Text = pgW.AdvMethods Then
                _Cerror = eConfigError.AdvMethods
                bEqual = False
            ElseIf Not cmbAskNet.Text = pgW.AskNet Then
                _Cerror = eConfigError.AskNet
                bEqual = False
            ElseIf Not cmbBattery.Text = pgW.PauseBattery Then
                _Cerror = eConfigError.PauseBattery
                bEqual = False
            ElseIf Not cmbBigWu.Text = pgW.WUSize Then
                _Cerror = eConfigError.WuSize
                bEqual = False
            ElseIf Not cmbCorePriority.Text = pgW.CorePriority Then
                _Cerror = eConfigError.CorePriority
                bEqual = False
            ElseIf Not cmbCpuAffinity.Text = pgW.DisableAffinty Then
                _Cerror = eConfigError.CpuAffinity
                bEqual = False
            ElseIf Not cmbDisableAssembly.Text = pgW.DissableAssembly Then
                _Cerror = eConfigError.DisableAssembly
                bEqual = False
            ElseIf Not cmbLocalDeadlines.Text = pgW.IgnoreDeadline Then
                _Cerror = eConfigError.IgnoreDeadline
                bEqual = False
            ElseIf Not cmbMachineID.Text = pgW.MachineID Then
                _Cerror = eConfigError.MachineID
                bEqual = False
            ElseIf Not cmbProxy.Text = pgW.UseProxy Then
                _Cerror = eConfigError.UseProxy
                bEqual = False
            ElseIf Not cmbProxyPassword.Text = pgW.UseProxyPass Then
                _Cerror = eConfigError.UseProxyPassword
                bEqual = False
            End If
            'Check service
            If pgW.AddRemoveService.ToUpper = "YES" Then
                If pgW.ServiceCheck = sPGroup.eAddRemove.Manual Then
                    If pGR.ServiceMode Then
                        bEqual = False
                        _Cerror = eConfigError.AddRemoveService
                    End If
                Else
                    If Not pGR.ServiceMode Then
                        bEqual = False
                        _Cerror = eConfigError.AddRemoveService
                    End If
                End If
            End If
            If bEqual And Not _ReadFailure And Not _SMPerror And Not _InvalidParams Then
                If _bDoRestart Then
                    lblS.Text = "Restarting client"
                    Application.DoEvents()
                    ClientControl.StartClient()
                    Do
                        WaitMS(50)
                    Loop While Not ClientControl.ConsoleStatus = pRunning
                End If
                lblS.Text = "Succes!"
                Try
                    llblLog.Visible = False
                Catch ex As Exception
                End Try
                Me.Close()
                nIcon.ContextMenuStrip = fLog.cMenu
                fLog.Enabled = True
                fLog.Focus()
            Else
                Select Case _Cerror
                    Case eConfigError.AddRemoveService
                        lblS.Text = "Error: Add/Remove service"
                    Case eConfigError.AdvMethods
                        lblS.Text = "Error: Advanced methods"
                    Case eConfigError.AskNet
                        lblS.Text = "Error: Ask before using network"
                    Case eConfigError.CorePriority
                        lblS.Text = "Error: Core priority"
                    Case eConfigError.CPInterval
                        lblS.Text = "Error: Check point interval"
                    Case eConfigError.CpuAffinity
                        lblS.Text = "Error: Cpu affinity"
                    Case eConfigError.CpuUsage
                        lblS.Text = "Error: Cpu usage"
                    Case eConfigError.DisableAssembly
                        lblS.Text = "Error: Disable Assembly"
                    Case eConfigError.IgnoreDeadline
                        lblS.Text = "Error: Ignore deadlines"
                    Case eConfigError.IPadress
                        lblS.Text = "Error: IP adress"
                    Case eConfigError.MachineID
                        lblS.Text = "Error: MachineID"
                    Case eConfigError.MemIndication
                        lblS.Text = "Error: Memory indication"
                    Case eConfigError.Parameters
                        lblS.Text = "Error: Additional parameters"
                    Case eConfigError.Passkey
                        lblS.Text = "Error: Passkey"
                    Case eConfigError.PauseBattery
                        lblS.Text = "Error: Pause on battery usage"
                    Case eConfigError.ProxyHost
                        lblS.Text = "Error: Proxy host"
                    Case eConfigError.ProxyPassword
                        lblS.Text = "Error: Proxy password"
                    Case eConfigError.ProxyPort
                        lblS.Text = "Error: Proxy port"
                    Case eConfigError.ProxyUserName
                        lblS.Text = "Error: Proxy username"
                    Case eConfigError.Team
                        lblS.Text = "Error: Team number"
                    Case eConfigError.UseProxy
                        lblS.Text = "Error: Use proxy"
                    Case eConfigError.UseProxyPassword
                        lblS.Text = "Error: User proxy password"
                    Case eConfigError.UserName
                        lblS.Text = "Error: User name"
                    Case eConfigError.WuSize
                        lblS.Text = "Error: Acceptable size of work units"
                    Case Else
                        If _SMPerror Then
                            lblS.Text = "Error: SMP has a problem!"
                        ElseIf _InvalidParams Then
                            lblS.Text = "Invalid parameters enterd, reconfigure needed"
                        Else
                            lblS.Text = "Error: -read failure?-"
                        End If
                End Select
                llblLog.Text = "View console output!"
            End If
            gbC.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
    Public bDialog As Boolean = False
    Public NoCancel As Boolean = False
    Public DoLock As Boolean = True
    Public aClient As clsClientInfo
    Private Sub frmConfig_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If bDialog Then
            Try
                Me.Client = aClient
                Me.Text = aClient.ClientExe & " -configonly"
                pGR.GUIType = aClient.TypeOfClient
                pgBackUp.GUIType = aClient.TypeOfClient
                pgW.GUIType = aClient.TypeOfClient
                _Done = False
                Application.DoEvents()
                ReadSettings()
            Catch ex As Exception

            End Try
        End If
        nudMem.Maximum = My.Computer.Info.TotalPhysicalMemory / 1024 / 1024
        If cmbProxy.Text.ToUpper = "NO" Then
            cmbProxyPassword.Enabled = False
            gbProxy.Enabled = False
        Else
            cmbProxyPassword.Enabled = True
            gbProxy.Enabled = True
            If cmbProxyPassword.Text.ToUpper = "YES" Then
                gbProxyPassword.Enabled = True
            Else
                gbProxyPassword.Enabled = False
            End If
        End If
        cancelprocessing = False
    End Sub
    Public Function ReadCurrentSettings(ByVal ClientExecutable As String) As clsClientControl.sClient
        Try
            'Only fills console settings, fill the other properties from clientcontrol class
            _Client = New clsClientInfo
            With _Client
                .ClientExe = ClientExecutable
                .ClientLocation = Mid(.ClientExe, 1, .ClientExe.LastIndexOf("\") + 1)
            End With
            Dim rClient As New clsClientControl.sClient
            pgBackUp.ClientEXE = ClientExecutable
            rClient.ClientEXE = _Client.ClientExe
            ReadSettings(False)
            With rClient
                .AcceptedWUSize = pGR.WUSize
                .AdditionalParameters = pGR.Parameters
                .AskNetwork = pGR.AskNet
                If pGR.ServiceCheck Then
                    .UseAsService = "yes"
                Else
                    .UseAsService = "no"
                End If
                .CheckpointInterval = pGR.CPInterval
                .CorePriority = pGR.CorePriority
                .CpuUsage = pGR.CpuUsage
                .DisableAffinitylock = pGR.DisableAffinty
                .DisableAssembly = pGR.DissableAssembly
                .ForceAdvMethods = pGR.AdvMethods
                .IgnoreDeadline = pGR.IgnoreDeadline
                .IPAdress = pGR.IPadress
                .MachineID = pGR.MachineID
                .MemoryUsage = pGR.MemoryIndication
                .PassKey = pGR.PassKey
                .PauseBattery = pGR.PauseBattery
                .ProxyHost = pGR.ProxyName
                .ProxyPassword = pGR.ProxyPassword
                .ProxyPort = pGR.ProxyPort
                .ProxyUserName = pGR.ProxyUserName
                .TeamNumber = pGR.Team
                .UseProxy = pGR.UseProxy
                .UseProxyPassword = pGR.UseProxyPass
                .UserName = pGR.Name
                .ClientArguments = pGR.ClientArguments
                .TypeOfClient = pGR.GUIType
                .ClientVersion = pGR.ClientVersion
            End With
            Return rClient
        Catch ex As Exception
            LogWindow.WriteError("ReadCurrentSettings", Err)
            Return Nothing
        End Try
    End Function
    Public Function WriteTheseSettings(ByVal Client As clsClientControl.sClient) As Boolean
        Try
            _Client = New clsClientInfo
            With _Client
                .ClientExe = Client.ClientEXE
                .ClientLocation = Mid(.ClientExe, 1, .ClientExe.LastIndexOf("\") + 1)
            End With
            'Set pgw to client settings
            With pgW
                .Name = Client.UserName
                .Team = Client.TeamNumber
                .PassKey = Client.PassKey
                .AskNet = Client.AskNetwork
                .ClientType = Client.TypeOfClient
                .AdvMethods = Client.ForceAdvMethods
                If Client.TypeOfClient = clsClientControl.eTypeOfClient.GPU Then
                    .GUIType = clsClientInfo.eClientType.Gpu
                Else
                    .GUIType = clsClientInfo.eClientType.Smp
                End If
                .CorePriority = Client.CorePriority
                .CPInterval = Client.CheckpointInterval
                .CpuUsage = Client.CpuUsage
                .DisableAffinty = Client.DisableAffinitylock
                .DissableAssembly = Client.DisableAssembly
                .IgnoreDeadline = Client.IgnoreDeadline
                .IPadress = Client.IPAdress
                .CPInterval = Client.CheckpointInterval
                .MachineID = Client.MachineID
                .MemoryIndication = Client.MemoryUsage
                .Parameters = Client.AdditionalParameters
                .PauseBattery = Client.PauseBattery
                .UseProxy = Client.UseProxy
                .UseProxyPass = Client.UseProxyPassword
                .ProxyName = Client.ProxyHost
                .ProxyPort = Client.ProxyPort
                .ProxyPassword = Client.ProxyPassword
                .ProxyUserName = Client.ProxyUserName
                .WUSize = Client.AcceptedWUSize
                'client type,  
                If IsThisClientAService(Client.ClientEXE) Then
                    .ClientType = sPGroup.eType.CoreFirst
                    If Client.UseAsService = "yes" Then
                        .AddRemoveService = "yes"
                    Else
                        .AddRemoveService = "no"
                    End If
                Else
                    .ClientType = sPGroup.eType.ServiceFirst
                    If Client.UseAsService = "yes" Then
                        .AddRemoveService = "no"
                    Else
                        .AddRemoveService = "yes"
                    End If
                End If
            End With
            SetStartUpInfo()
            Dim dEnd As DateTime
            WaitMS(1000)
            With pWriter
                .WriteLine(pgW.Name)
                WaitMS(250)
                .WriteLine(pgW.Team)
                WaitMS(250)
                .WriteLine(pgW.PassKey)
                WaitMS(250)
                .WriteLine(pgW.AskNet)
                WaitMS(250)
                .WriteLine(pgW.UseProxy)
                WaitMS(250)
                If pgW.UseProxy.ToUpper = "YES" Then
                    .WriteLine(pgW.ProxyName)
                    WaitMS(250)
                    .WriteLine(pgW.ProxyPort)
                    WaitMS(250)
                    WriteLine(pgW.UseProxyPass)
                    WaitMS(250)
                    If pgW.UseProxyPass.ToUpper = "YES" Then
                        .WriteLine(pgW.ProxyUserName)
                        WaitMS(250)
                        .WriteLine(pgW.ProxyPassword)
                        dEnd = DateTime.Now.AddMilliseconds(500)
                        WaitMS(250)
                    End If
                End If
                .WriteLine(pgW.WUSize)
                WaitMS(250)
                .WriteLine("yes") 'adv options
                WaitMS(250)
                If pgW.ClientType = sPGroup.eType.ServiceFirst Then
                    .WriteLine(pgW.AddRemoveService)
                    WaitMS(250)
                    .WriteLine(pgW.CorePriority)
                    WaitMS(250)
                    .WriteLine(pgW.CpuUsage)
                    WaitMS(250)
                    .WriteLine(pgW.DissableAssembly)
                    WaitMS(250)
                    .WriteLine(pgW.PauseBattery)
                    WaitMS(250)
                    .WriteLine(pgW.CPInterval)
                    WaitMS(250)
                    .WriteLine(pgW.MemoryIndication)
                    WaitMS(250)
                    .WriteLine(pgW.AdvMethods)
                    WaitMS(250)
                    .WriteLine(pgW.IgnoreDeadline)
                    WaitMS(250)
                    .WriteLine(pgW.MachineID)
                    WaitMS(250)
                    .WriteLine(pgW.DisableAffinty)
                    WaitMS(250)
                    .WriteLine(pgW.Parameters)
                    WaitMS(250)
                    .WriteLine(pgW.IPadress)
                    WaitMS(250)
                    While Not pConfigure.HasExited
                        .WriteLine("")
                        WaitMS(250)
                    End While
                Else
                    .WriteLine(pgW.CorePriority)
                    WaitMS(250)
                    .WriteLine(pgW.CpuUsage)
                    WaitMS(250)
                    .WriteLine(pgW.DissableAssembly)
                    WaitMS(250)
                    .WriteLine(pgW.PauseBattery)
                    WaitMS(250)
                    .WriteLine(pgW.CPInterval)
                    WaitMS(250)
                    .WriteLine(pgW.MemoryIndication)
                    WaitMS(250)
                    .WriteLine(pgW.AdvMethods)
                    WaitMS(250)
                    .WriteLine(pgW.IgnoreDeadline)
                    WaitMS(250)
                    .WriteLine(pgW.MachineID)
                    WaitMS(250)
                    .WriteLine(pgW.AddRemoveService)
                    WaitMS(250)
                    .WriteLine(pgW.DisableAffinty)
                    WaitMS(250)
                    .WriteLine(pgW.Parameters)
                    WaitMS(250)
                    .WriteLine(pgW.IPadress)
                    WaitMS(250)
                    While Not pConfigure.HasExited
                        .WriteLine("")
                        WaitMS(250)
                        Application.DoEvents()
                    End While
                    While Not _ConfigOutputReady
                        Application.DoEvents()
                    End While
                    WaitMS(500) 'give outputhandler some extra time
                    If rtf.Text.ToUpper.Contains("PLEASE CHECK YOUR CONFIGURATION") Or rtf.Text.ToUpper.Contains("SOCK ERROR") Then
                        _SMPerror = True
                    End If
                End If
            End With
            Return Not _SMPerror AndAlso Not _InvalidParams
        Catch ex As Exception
            LogWindow.WriteError("frmConfig_WriteTheseSettings", Err)
            Return False
        End Try
    End Function
    Public Sub StartConfig(ByVal aClient As clsClientInfo, Optional ByVal NoCancel As Boolean = False, Optional ByVal DoLock As Boolean = True)
        Try
            Me.Client = aClient
            Me.Text = aClient.ClientExe & " -configonly"
            pGR.GUIType = aClient.TypeOfClient
            pgBackUp.ClientEXE = aClient.ClientExe
            pGR.ClientEXE = aClient.ClientExe
            pgBackUp.GUIType = aClient.TypeOfClient
            pgW.GUIType = aClient.TypeOfClient
            _Done = False
            Me.Show()
            Application.DoEvents()
            ReadSettings()
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub
    Public Sub DoConfig(ByVal Client As clsClientControl.sClient, Optional ByVal NoCancel As Boolean = False)
        Try
            _Client = New clsClientInfo
            With _Client
                .ClientExe = Client.ClientEXE
                .ClientLocation = Mid(.ClientExe, 1, .ClientExe.LastIndexOf("\") + 1)
            End With
            Dim rClient As New clsClientControl.sClient
            pgBackUp.ClientEXE = Client.ClientEXE
            rClient.ClientEXE = Client.ClientEXE
            If NoCancel Then cmdCancel.Enabled = False
            ReadSettings(True)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub llblLog_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblLog.LinkClicked
        Try
            If Not llblLog.Text = "General help" Then
                If rtf.TextLength = 0 Then Exit Sub
                Dim nLog As New frmLogOutput
                nLog.rtf.Text = rtf.Text
                nLog.ShowDialog(Me)
            Else
                If pGR.GUIType = clsClientInfo.eClientType.Gpu Then
                    'check what gpu!
                    Dim hwI As New clsHWInfo
                    If hwI.FillGpuInfo Then
                        If hwI.GPU_Count > 0 Then
                            Dim dGpu As clsHWInfo.sGpuInfo = hwI.GetGpu(0)
                            If dGpu.Vendor.ToUpper.Contains("ATI") Or dGpu.Vendor.ToUpper.Contains("AMD") Then
                                Process.Start("http://folding.stanford.edu/English/FAQ-ATI2")
                            Else
                                If MD5CalcFile(Client.ClientExe) = "b41301886881958c64c1907b3ed6acae" Or _
                                    MD5CalcFile(Client.ClientExe) = "885e36a477d247487f8009335bd4e3cc" Then
                                    'GPU3
                                    Process.Start("http://www.stanford.edu/group/pandegroup/cgi-bin/index.php?n=English.FAQ-NVIDIA-GPU3")
                                Else
                                    Process.Start("http://www.stanford.edu/group/pandegroup/cgi-bin/index.php?n=English.FAQ-NVIDIA")
                                End If
                            End If
                        Else : GoTo LaunchIndex
                        End If
                    Else
LaunchIndex:
                        LogWindow.WriteLog("Could not launch to specific FAQ, FAQ index launched instead")
                        Process.Start("http://folding.stanford.edu/English/FAQ")
                    End If
                ElseIf pGR.GUIType = clsClientInfo.eClientType.Smp Then
                    Process.Start("http://folding.stanford.edu/English/FAQ-SMP")
                ElseIf pGR.GUIType = clsClientInfo.eClientType.Classic Then
                    Process.Start("http://folding.stanford.edu/English/FAQ-Configure")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        'Cancel, coltakenhwid has item machineid for this client removed, use pgr
        'If Not Cfg.CfgManager.colTakenHWID.Contains(pGR.MachineID.ToString) Then Cfg.CfgManager.colTakenHWID.Add(pGR.MachineID, pGR.MachineID.ToString)
        'If bDialog Then
        'Me.Close()
        'Else
        'boolCancel = True
        'End If
        Try
            If bCleanUp Then
                Dim rVal As MsgBoxResult = MsgBox("Canceling at this moment means the application will undo any changes to client config and then exit. Ok will continue with cleanup, cancel will take you back to the configuration screen.", vbOKCancel, "Clean up and exit?")
                If rVal = MsgBoxResult.Cancel Then Return
                Dim cPath As String = Mid(_Client.ClientExe, 1, _Client.ClientExe.LastIndexOf("\"))
                If My.Computer.FileSystem.FileExists(cPath & "\client.cfg") Then My.Computer.FileSystem.DeleteFile(cPath & "\client.cfg", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                If My.Computer.FileSystem.FileExists(cPath & "\dbstats.db") Then My.Computer.FileSystem.DeleteFile(cPath & "\dbStats.db", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                If My.Computer.FileSystem.FileExists(cPath & "\projects.dat") Then My.Computer.FileSystem.DeleteFile(cPath & "\projects.dat", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                If My.Computer.FileSystem.FileExists(cPath & "\MyFolding.html") Then My.Computer.FileSystem.DeleteFile(cPath & "\MyFolding.html", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                If My.Computer.FileSystem.FileExists(cPath & "\fahlog.txt") Then My.Computer.FileSystem.DeleteFile(cPath & "\fahlog.txt", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                End
            End If
        Catch ex As Exception
            MsgBox("Unhandeld error." & vbNewLine & ex.Message)
            End
        End Try
        _ConfigID = -1
        Me.Close()
        fLog.Enabled = True
        If fLog.Visible Then
            fLog.BringToFront()
            fLog.Focus()
        End If
        nIcon.ContextMenuStrip = fLog.cMenu
    End Sub
#End Region
#Region "Proxy gb"
    Private Sub cmbProxy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbProxy.SelectedIndexChanged
        'If cancelProcessing Then Exit Sub
        If cmbProxy.Text.ToUpper = "NO" Then
            cmbProxyPassword.Enabled = False
            gbProxy.Enabled = False
        Else
            cmbProxyPassword.Enabled = True
            gbProxy.Enabled = True
            If cmbProxyPassword.Text.ToUpper = "YES" Then
                gbProxyPassword.Enabled = True
            Else
                gbProxyPassword.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmbProxyPassword_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbProxyPassword.SelectedIndexChanged
        'If cancelProcessing Then Exit Sub
        If cmbProxy.Text.ToUpper = "NO" Then
            cmbProxyPassword.Enabled = False
            gbProxy.Enabled = False
        Else
            cmbProxyPassword.Enabled = True
            gbProxy.Enabled = True
            If cmbProxyPassword.Text.ToUpper = "YES" Then
                gbProxyPassword.Enabled = True
            Else
                gbProxyPassword.Enabled = False
            End If
        End If
    End Sub
#End Region
#Region "Input filters for textboxes"
    Private Sub txtTeamNumber_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTeamNumber.KeyDown
        If Not (e.KeyCode >= Keys.D0 And e.KeyCode <= Keys.D9) Or Not (e.KeyCode >= Keys.NumPad0 And e.KeyCode <= Keys.NumPad9) Or Not e.KeyCode = Keys.Back Or Not e.KeyCode = Keys.Delete Or Not e.KeyData = Keys.Left Or e.KeyData = Keys.Right Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtProxyPort_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtProxyPort.KeyDown
        If Not (e.KeyCode >= Keys.D0 And e.KeyCode <= Keys.D9) Or Not (e.KeyCode >= Keys.NumPad0 And e.KeyCode <= Keys.NumPad9) Or Not e.KeyCode = Keys.Back Or Not e.KeyCode = Keys.Delete Or Not e.KeyData = Keys.Left Or e.KeyData = Keys.Right Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtUserName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUserName.KeyDown
        If e.KeyCode = Keys.Tab Then
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Space Then
            e.SuppressKeyPress = True
            txtUserName.Text = txtUserName.Text & "_"
        ElseIf e.KeyCode = Keys.Enter Then
            txtTeamNumber.Focus()
        End If
    End Sub
#End Region

    Private Sub lnblCores_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnblCores.LinkClicked
        Try
            Process.Start("http://fahwiki.net/index.php/How_do_I_know_what_the_client_flags_(-switches)_are%2C_and_what_they_do%3F#-smp_x")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmbCores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCores.SelectedIndexChanged
        Try
            If txtParam.Text.Contains("-smp") Then
                Dim tTemp As String = txtParam.Text
                'check if there is a number after -smp_
                Try
                    If tTemp.Length >= tTemp.LastIndexOf("-smp") + 4 + 2 Then
                        'check if it's the last param
                        If Not tTemp.LastIndexOf("-") > tTemp.LastIndexOf("-smp") Then
                            'just cut after
                            tTemp = Mid(tTemp, 1, tTemp.LastIndexOf("-smp"))
                            tTemp = tTemp.Trim()
                        Else
                            'find first "-" after -SMP
                            Dim xPos As Int16 = InStr(tTemp.LastIndexOf("-smp") + 2, tTemp, "-")
                            If xPos > 0 Then
                                Dim tmp2 As String = Mid(tTemp, xPos)
                                tTemp = Mid(tTemp, 1, tTemp.LastIndexOf("-smp"))
                                tTemp &= tmp2
                            End If
                        End If
                    Else
                        tTemp = tTemp.Replace("-smp", "")
                        tTemp = tTemp.Trim
                    End If
                Catch ex As Exception

                End Try
                If cmbCores.SelectedIndex = cmbCores.Items.Count - 1 Then
                    txtParam.Text = tTemp & " -smp"
                Else
                    txtParam.Text = tTemp & " -smp " & cmbCores.Text
                End If
            Else
                If txtParam.TextLength > 0 Then 'assume a paramater
                    If cmbCores.SelectedIndex = cmbCores.Items.Count Then
                        txtParam.Text &= " -smp"
                    Else
                        txtParam.Text &= " -smp " & cmbCores.Text
                    End If
                Else
                    If cmbCores.SelectedIndex = cmbCores.Items.Count - 1 Then
                        txtParam.Text = "-smp"
                    Else
                        txtParam.Text = "-smp " & cmbCores.Text
                    End If
                End If
            End If
            txtParam.Text = txtParam.Text.Trim
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://fahwiki.net/index.php/How_do_I_know_what_the_client_flags_(-switches)_are,_and_what_they_do%3F")
    End Sub
End Class
