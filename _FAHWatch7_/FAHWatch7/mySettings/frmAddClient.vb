'   cftUnity AddClient form
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports System.IO.DirectoryInfo
Imports System.Management
Imports System.DirectoryServices
Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.IO
Imports System.Net
Public Class frmAddClient
    'WMI network enumeration taken from http://www.codeproject.com/KB/vb/Simple_Network_Browser.aspx
    'lost the functions where one could use environment variables, changed some of the api declarations
    Private m_local_domain_name As String
    Private m_local_computer_name As String
    Private m_local_domain_name2 As String
    Private m_local_computer_name2 As String
    Private Shared domainEntry As DirectoryEntry
    Private tEPC As Threading.Thread
    Private Sub GetInf(domain As Object)
        domainEntry = New DirectoryEntry("WinNT://" + CStr(domain))
        domainEntry.Children.SchemaFilter.Add("computer")
    End Sub
    Private Function GetLocalComputerInfo() As Boolean
        Dim query As ManagementObjectSearcher = Nothing
        Dim queryCollection As ManagementObjectCollection = Nothing
        Try
            Dim query_command As String = "SELECT * FROM Win32_ComputerSystem"
            Dim msc As ManagementScope = New ManagementScope("root\cimv2")
            Dim select_query As SelectQuery = New SelectQuery(query_command)
            query = New ManagementObjectSearcher(msc, select_query)
            queryCollection = query.Get()
            For Each management_object As ManagementObject In queryCollection
                m_local_domain_name2 = CStr(management_object("Domain"))
                m_local_computer_name2 = CStr(management_object("Name"))
            Next management_object
            msc = Nothing
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            If query IsNot Nothing Then query.Dispose()
            If queryCollection IsNot Nothing Then queryCollection.Dispose()
        End Try
    End Function
    Dim getHostName As String = System.Net.Dns.GetHostName
    Private Function myDomainName() As String
        Dim getHostName As String = System.Net.Dns.GetHostName
        Dim ipHost As System.Net.IPHostEntry = Dns.GetHostEntry(getHostName)
        Dim domainName As String = ipHost.HostName
        Return domainName
    End Function
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")> Private Function GetComputersInfoCollection(ByVal domain As String) As DirectoryEntry
        Try
            'Return ((New DirectoryEntry("WinNT://" + domain).Children.SchemaFilter.Add("computer")))
            Dim domainEntry As DirectoryEntry = New DirectoryEntry("WinNT://" + domain)
            domainEntry.Children.SchemaFilter.Add("computer")
            Return domainEntry
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New DirectoryEntry("WinNT://" + domain)
        End Try
    End Function
    Private Sub tvEnum_AfterSelect(sender As Object, e As System.Windows.Forms.TreeViewEventArgs) Handles tvEnum.AfterSelect
        Try
            txtPort.Text = ""
            txtPWD.Text = ""
            txtLocation.Text = ""
            txtFCVersion.Text = ""
            txtFWatch_port.Text = ""
            txtFWVersion.Text = ""
            chkFwatchInstance.Checked = False
            cmdAccept.Enabled = False
            If e.Node.Nodes.Count = 0 Then Exit Sub
            'Check folder content for FAHClient structure
            If e.Node.FullPath = m_local_domain_name Then
                Exit Sub
            ElseIf e.Node.FullPath.Substring(m_local_domain_name.Length + 1).IndexOf("\") <= 0 Then
                Exit Sub
            End If
            Dim fP As String = "//" & e.Node.FullPath.Replace(m_local_domain_name & "\", "").Replace("\", "/")
            Dim sFolders = My.Computer.FileSystem.GetDirectories(fP)
            If sFolders.Contains(fP.Replace("/", "\") & "\work") And sFolders.Contains(fP.Replace("/", "\") & "\logs") And sFolders.Contains(fP.Replace("/", "\") & "\cores") And sFolders.Contains(fP.Replace("/", "\") & "\configs") Then
                Dim sFiles = My.Computer.FileSystem.GetFiles(fP)
                If sFiles.Contains(fP.Replace("/", "\") & "\config.xml") And sFiles.Contains(fP.Replace("/", "\") & "\log.txt") And sFiles.Contains(fP.Replace("/", "\") & "\FAHControl.db") Then
                    Dim nConfig As New clsClientConfig.clsConfiguration
                    Dim cStream As FileStream = Nothing, TheString As String = String.Empty
                    Try
                        cStream = New FileStream(fP.Replace("/", "\") & "\config.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, FileOptions.None)
                        Using cReader As StreamReader = New StreamReader(cStream)
                            cStream = Nothing
                            If Not cReader.EndOfStream Then
                                TheString = cReader.ReadToEnd
                            End If
                        End Using
                    Finally
                        If cStream IsNot Nothing Then cStream.Dispose()
                    End Try
                    If nConfig.ReadString(TheString) Then
                        txtLocation.Text = fP.Replace("/", "\")
                        txtPort.Text = nConfig.RemoteCommandServer.port
                        txtPWD.Text = nConfig.RemoteCommandServer.password
                    End If
                    Dim fStream As FileStream = Nothing
                    Try
                        fStream = New FileStream(fP.Replace("/", "\") & "\log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                        Using fRead As StreamReader = New StreamReader(fStream)
                            fStream = Nothing
                            While Not fRead.EndOfStream
                                Dim tString As String = fRead.ReadLine
                                If tString.Contains("Version: ") Then
                                    txtFCVersion.Text = tString.Substring(tString.IndexOf("Version: ") + Len("Version: "))
                                    Exit While
                                End If
                            End While
                        End Using
                    Finally
                        If fStream IsNot Nothing Then fStream.Dispose()
                    End Try

                    If sFiles.Contains(fP.Replace("/", "\") & "\Diagnostics.txt") Then
                        chkFwatchInstance.Checked = True
                        Dim fStream2 As FileStream = Nothing
                        Try
                            fStream2 = New FileStream(fP.Replace("/", "\") & "\Diagnostics.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.Asynchronous)
                            Using fRead2 = New StreamReader(fStream2)
                                fStream2 = Nothing
                                While Not fRead2.EndOfStream
                                    Dim tString As String = fRead2.ReadLine
                                    If tString.Contains("FAHWatch7 ") Then
                                        txtFWVersion.Text = tString.Substring(tString.IndexOf("FAHWatch7 ") + Len("FAHWatch7 "), 7)
                                    ElseIf tString.Contains("Com port: ") Then
                                        txtFWatch_port.Text = tString.Substring(tString.IndexOf("Com port: ") + Len("Com port: "))
                                        Exit While
                                    End If
                                End While
                            End Using
                        Finally
                            If fStream2 IsNot Nothing Then fStream2.Dispose()
                        End Try
                    End If
                    cmdAccept.Enabled = True
                Else
                    cmdAccept.Enabled = False
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub tvEnum_BeforeExpand(sender As Object, e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvEnum.BeforeExpand
        Try
            If e.Node.Nodes.Count > 0 And e.Node.Nodes(0).Text = "-haschild-" Then
                e.Node.Nodes(0).Remove()
                Dim fP As String = "//" & e.Node.FullPath.Replace(m_local_domain_name & "\", "").Replace("\", "/")
                Dim sFolders = My.Computer.FileSystem.GetDirectories(fP)
                For Each folder In sFolders
                    Dim nNode As New TreeNode
                    nNode.Text = folder.Replace(fP.Replace("/", "\"), "").Trim(CChar("\"))
                    If My.Computer.FileSystem.GetDirectories(fP & "/" & nNode.Text).Count > 0 Then
                        nNode.Nodes.Add("-haschild-")
                    End If
                    e.Node.Nodes.Add(nNode)
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub txtPWD_DoubleClick(sender As Object, e As System.EventArgs) Handles txtPWD.DoubleClick
        Try
            If txtPWD.PasswordChar = "*" Then
                txtPWD.PasswordChar = CChar("")
            Else
                txtPWD.PasswordChar = CChar("*")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdAccept_Click(sender As System.Object, e As System.EventArgs) Handles cmdAccept.Click
        Try
            Dim ClientName As String = txtLocation.Text.Replace("\\", "")
            ClientName = ClientName.Substring(0, ClientName.IndexOf("\"))
            modMySettings.clsRemoteClients.AddRemoteClient(ClientName, txtLocation.Text, txtPort.Text, txtPWD.Text, txtFWatch_port.Text, True)
            NativeMethods.AnimateWindow(Me.Handle, 500, NativeMethods.AnimateWindowFlags.AW_BLEND Or NativeMethods.AnimateWindowFlags.AW_HIDE)
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub frmAddClient_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not IsNothing(tEPC) AndAlso tEPC.IsAlive Then tEPC.Abort()
        NativeMethods.AnimateWindow(Me.Handle, 50, CType(NativeMethods.AnimateWindowFlags.AW_BLEND + NativeMethods.AnimateWindowFlags.AW_HIDE, NativeMethods.AnimateWindowFlags))
    End Sub
    Private Sub frmAddClient_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
    End Sub

    Private Sub tShow_Tick(sender As System.Object, e As System.EventArgs) Handles tShow.Tick
        Try
            tShow.Enabled = False
            tvEnum.Nodes.Clear()
            txtPort.Text = ""
            txtPWD.Text = ""
            txtLocation.Text = ""
            txtFCVersion.Text = ""
            txtFWatch_port.Text = ""
            txtFWVersion.Text = ""
            chkFwatchInstance.Checked = False
            cmdAccept.Enabled = False
            m_local_computer_name = Environment.MachineName
            If My.Computer.Network.IsAvailable Then
                If simpleNetworkbrowser Then GoTo SkipQuestion
                Me.Cursor = Cursors.WaitCursor
                Panel1.Enabled = False
                tsStatus.Text = "Enumerating computers"
                Dim nInfo As NetworkInformation = NetworkInformation.LocalComputer
                m_local_domain_name = nInfo.DomainName
                Dim dtNow As DateTime = DateTime.Now
                tEPC = New Threading.Thread(AddressOf GetInf)
                WriteDebug("GetComputersInfoCollection")
                tEPC.Start(m_local_domain_name)
                Do
                    Application.DoEvents()
                    'GoTo manual
                    If DateTime.Now.Subtract(dtNow).TotalSeconds > 60 Then
                        tEPC.Abort()
                        GoTo manual
                    End If
                Loop While tEPC.IsAlive
                WriteDebug("/GetComputersInfoCollection")
                Dim rNode As New TreeNode
                rNode.Text = m_local_domain_name
                Try
                    For Each M As DirectoryEntry In domainEntry.Children
                        If Not M.Name = m_local_computer_name Then
                            Dim nNode As New TreeNode
                            nNode.Text = M.Name
                            Dim sCollection As ShareCollection = GetSharesFromHost(M.Name)
                            For Each s As ShareType In sCollection.Shares
                                Application.DoEvents()
                                If Not s.Name.Contains("$") Then
                                    Dim sNode As New TreeNode
                                    sNode.Text = s.Name
                                    If My.Computer.FileSystem.GetDirectories("//" & M.Name & "/" & s.Name).Count > 0 Then
                                        sNode.Nodes.Add("-haschild-")
                                    End If
                                    nNode.Nodes.Add(sNode)
                                End If
                            Next
                            rNode.Nodes.Add(nNode)
                        End If
                    Next
                Catch ex As Exception
                    WriteLog("Network browser failed, using manual add procedure", eSeverity.Important)
                    GoTo manual
                End Try
                tvEnum.Nodes.Add(rNode)
                tvEnum.ShowRootLines = True
                tvEnum.Nodes(0).Expand()
                For Each childNode As TreeNode In tvEnum.Nodes(0).Nodes
                    childNode.Expand()
                Next
                tsStatus.Text = ""
                Me.Cursor = Cursors.Default
                Panel1.Enabled = True
                tvEnum.Enabled = True
                GoTo ShowForm
            Else
                tsStatus.Text = "The network is not available."
                tvEnum.Enabled = False
                GoTo ShowForm
            End If

manual:
            Do
                Dim rVal As MsgBoxResult = MsgBox("Do you want to manually browse for a shared folder?" & Environment.NewLine & Environment.NewLine & "The folder should be the root application data location from FAHClient, containing log.txt and a subfolder called logs.", CType(MsgBoxStyle.Question & MsgBoxStyle.OkCancel, MsgBoxStyle), "Locate remote FAHClient instance")
                If rVal = MsgBoxResult.Cancel Then GoTo Cancel
SkipQuestion:
                Using fbD As New FolderBrowserDialog
                    clsFBExtension.FolderBrowserDialogEx.SetRootFolder(fbD, clsFBExtension.FolderBrowserDialogEx.CsIdl.Network)
                    fbD.ShowDialog()
                    Dim sPath As String = fbD.SelectedPath
                    If Not sPath = "" Then
                        Dim sFolders = My.Computer.FileSystem.GetDirectories(sPath)
                        If sFolders.Contains(sPath & "\work") And sFolders.Contains(sPath & "\logs") And sFolders.Contains(sPath & "\cores") And sFolders.Contains(sPath & "\configs") Then
                            Try
                                'Add node for pc and path
                                Dim mName As String = sPath.Substring(2, sPath.IndexOf("\", 3) - 2)
                                Dim nNode As New TreeNode
                                nNode.Text = mName
                                Dim tNodeString As String = sPath.Replace("\\" & mName, "").Substring(1)
                                Do
                                    If tNodeString.IndexOf("\") = -1 Then
                                        nNode.Nodes.Add(tNodeString)
                                        Exit Do
                                    Else
                                        nNode.Nodes.Add(tNodeString.Substring(0, tNodeString.IndexOf("\")))
                                        tNodeString = tNodeString.Substring(tNodeString.IndexOf("\") + 1)
                                    End If
                                Loop While tNodeString.IndexOf("\") = -1
                                tNodeString = Nothing
                                tvEnum.Nodes.Add(nNode)
                                tvEnum.ExpandAll()
                                tvEnum.Enabled = False
                                Dim nConfig As New clsClientConfig.clsConfiguration
                                Dim cFileStream As FileStream = Nothing
                                Dim theString As String = String.Empty
                                Try
                                    cFileStream = New FileStream(sPath & "\config.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, FileOptions.None)
                                    Using cReader As StreamReader = New StreamReader(cFileStream)
                                        cFileStream = Nothing
                                        If Not cReader.EndOfStream Then
                                            theString = cReader.ReadToEnd
                                        End If
                                    End Using
                                Finally
                                    If cFileStream IsNot Nothing Then cFileStream.Dispose()
                                End Try

                                If nConfig.ReadString(theString) Then
                                    txtLocation.Text = sPath
                                    txtPort.Text = nConfig.RemoteCommandServer.port
                                    txtPWD.Text = nConfig.RemoteCommandServer.password
                                End If
                                Dim fStream As FileStream = Nothing
                                Try
                                    fStream = New FileStream(sPath & "\log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.None)
                                    Using fRead As StreamReader = New StreamReader(fStream)
                                        fStream = Nothing
                                        While Not fRead.EndOfStream
                                            Dim tString As String = fRead.ReadLine
                                            If tString.Contains("Version: ") Then
                                                txtFCVersion.Text = tString.Substring(tString.IndexOf("Version: ") + Len("Version: "))
                                                Exit While
                                            End If
                                        End While
                                    End Using
                                Finally
                                    If fStream IsNot Nothing Then fStream.Dispose()
                                End Try
                                Dim sFiles = My.Computer.FileSystem.GetFiles(sPath)
                                If sFiles.Contains(sPath & "\Diagnostics.txt") Then
                                    chkFwatchInstance.Checked = True
                                    Dim fStream2 As FileStream = Nothing
                                    Try
                                        fStream2 = New FileStream(sPath & "\Diagnostics.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 1024, System.IO.FileOptions.None)
                                        Using fRead2 = New StreamReader(fStream2)
                                            fStream2 = Nothing
                                            While Not fRead2.EndOfStream
                                                Dim tString As String = fRead2.ReadLine
                                                If tString.Contains("FAHWatch7 ") Then
                                                    txtFWVersion.Text = tString.Substring(tString.IndexOf("FAHWatch7 ") + Len("FAHWatch7 "), 7)
                                                ElseIf tString.Contains("Com port: ") Then
                                                    txtFWatch_port.Text = tString.Substring(tString.IndexOf("Com port: ") + Len("Com port: "))
                                                    Exit While
                                                End If
                                            End While
                                        End Using
                                    Finally
                                        If fStream2 IsNot Nothing Then fStream2.Dispose()
                                    End Try
                                End If
                                cmdAccept.Enabled = True
                                tsStatus.Text = ""
                                Me.Cursor = Cursors.Default
                                Panel1.Enabled = True
                                GoTo ShowForm
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                MsgBox("An error prevents adding a network client at this time, sorry", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, MsgBoxStyle))
                                GoTo cancel
                            End Try
                        Else
                            MsgBox("This does not seem like a valid location", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle))
                        End If
                    Else
                        Exit Do
                    End If
                End Using
            Loop
cancel:
            Me.Close()
            Exit Sub
ShowForm:
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub frmAddClient_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Call tShow_Tick(Me, New MyEventArgs.EmptyArgs)
        'tShow.Enabled = True
    End Sub
End Class

