'/*
' * FAHWatch7  Copyright Marvin Westmaas ( mtm )
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

Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.Win32
Imports System.IO

Public Class clsClientConfig
    Public Class clsConfiguration
        Private _ClientLocation As String, _DataLocation As String
        Public ReadOnly Property DataLocation As String
            Get
                Return _DataLocation
            End Get
        End Property
        Public ReadOnly Property ClientLocation As String
            Get
                Return _ClientLocation
            End Get
        End Property
        Public Function SetLocations(Optional ByVal DoLog As Boolean = False) As Boolean
            Try
                Dim rRoot As RegistryKey
                If DoLog Then LogWindow.WriteLog("Openining registry root LOCAL_MACHINE")
                rRoot = Registry.LocalMachine
                If DoLog Then LogWindow.WriteLog("Trying 'Software\Microsoft\Windows\CurrentVersion\Uninstall\FAHClient'")
                Dim rFKey As RegistryKey = rRoot.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Uninstall\FAHClient")
                If Not IsNothing(rFKey) Then
ReadKey:
                    If DoLog Then LogWindow.WriteLog("Key openend succesfully!")
                    For Each kVName As String In rFKey.GetValueNames
                        If DoLog Then LogWindow.WriteLog("Found '" & kVName & "' value='" & rFKey.GetValue(kVName) & "'")
                        If kVName = "DataDirectory" Then
                            If DoLog Then LogWindow.WriteLog("DataLocation set to " & rFKey.GetValue(kVName))
                            _DataLocation = rFKey.GetValue(kVName)
                        ElseIf kVName = "DisplayIcon" Then
                            If DoLog Then LogWindow.WriteLog("FAHClient location set to " & rFKey.GetValue(kVName).ToString.Replace("\FAHClient.ico", ""))
                            _ClientLocation = rFKey.GetValue(kVName).ToString.Replace("\FAHClient.ico", "")
                        End If
                    Next
                Else
                    If DoLog Then LogWindow.WriteLog("Opening registry keys in order")
                    If DoLog Then LogWindow.WriteLog(" -> software")
                    rFKey = rRoot.OpenSubKey("SOFTWARE")
                    If Not IsNothing(rFKey) Then
                        If DoLog Then LogWindow.WriteLog(" -> Microsoft")
                        rFKey = rFKey.OpenSubKey("Microsoft")
                        If Not IsNothing(rFKey) Then
                            If DoLog Then LogWindow.WriteLog(" -> Windows")
                            rFKey = rFKey.OpenSubKey("Windows")
                            If Not IsNothing(rFKey) Then
                                If DoLog Then LogWindow.WriteLog(" -> CurrentVersion")
                                rFKey = rFKey.OpenSubKey("CurrentVersion")
                                If Not IsNothing(rFKey) Then
                                    If DoLog Then LogWindow.WriteLog(" -> Uninstall")
                                    rFKey = rFKey.OpenSubKey("Uninstall")
                                    If Not IsNothing(rFKey) Then
                                        If DoLog Then LogWindow.WriteLog(" -> FAHClient")
                                        rFKey = rFKey.OpenSubKey("FAHClient")
                                        If Not IsNothing(rFKey) Then
                                            GoTo ReadKey
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If DataLocation = "" Then
                    If DoLog Then LogWindow.WriteLog("Query application data location")
                    If My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\config.xml") Then
                        If DoLog Then LogWindow.WriteLog("Directory found in this users application data, config.xml found =" & My.Computer.FileSystem.FileExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\Config.xml"))
                        _DataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient"
                    ElseIf My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient") Then
                        If DoLog Then LogWindow.WriteLog("Directory found in common application data, config.xml found=" & Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient\Config.xml")
                        _DataLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient"
                    End If
                    If DataLocation = "" And DoLog = True Then LogWindow.WriteLog("All methods exhausted for data location!")
                End If
                If ClientLocation = "" Then
                    If DoLog Then LogWindow.WriteLog("Searching %path% for FAHClient.exe")
                    Dim dirs() As String = Environment.GetEnvironmentVariable("path").ToString.Split(";")
                    For Each dStr As String In dirs
                        If My.Computer.FileSystem.FileExists(dStr & "\FAHClient.exe") Then
                            If DoLog Then LogWindow.WriteLog("Found FAHClient.exe, location=" & dStr)
                            _ClientLocation = dStr
                            Exit For
                        End If
                    Next
                    If ClientLocation = "" And DoLog = True Then LogWindow.WriteLog("All methods exhausted for Client location!")
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
            Return DataLocation <> "" AndAlso ClientLocation <> ""
        End Function
        Public Function ReadString(ByVal TheString As String, Optional ByVal DoLog As Boolean = False) As Boolean
            Try
                If DoLog Then LogWindow.WriteLog("Reading Config.xml file.")
                ConfigString = TheString
                Dim strXML As String = TheString
                strXML = strXML.Replace("'", Chr(34))
                Dim strReader As New IO.StringReader(strXML)
                Using xmlR As Xml.XmlReader = XmlReader.Create(strReader)
                    If xmlR.ReadToFollowing("config") Then
                        xmlR.Read()
                        Do
                            If xmlR.NodeType = XmlNodeType.Comment Then
                                Select Case xmlR.Value
                                    Case " Remote Command Server "
                                        xmlR.Read() ' whitespace 
                                        Do
                                            If xmlR.Read Then
                                                If xmlR.NodeType = XmlNodeType.Element Then
                                                    Select Case xmlR.Name
                                                        Case Is = "password"
                                                            RemoteCommandServer.password = xmlR.Item(0).ToString
                                                        Case Is = "port"
                                                            RemoteCommandServer.port = xmlR.Item(0).ToString
                                                    End Select
                                                Else
                                                    If xmlR.NodeType = XmlNodeType.Comment Then Exit Do
                                                End If
                                            End If
                                        Loop
                                    Case " User Information "
                                        xmlR.Read()
                                        Do
                                            If xmlR.Read Then
                                                If xmlR.NodeType = XmlNodeType.Element Then
                                                    Select Case xmlR.Name
                                                        Case Is = "passkey"
                                                            UserInformation.passkey = xmlR.Item(0).ToString
                                                        Case Is = "user"
                                                            UserInformation.user = xmlR.Item(0).ToString
                                                        Case Is = "team"
                                                            UserInformation.team = xmlR.Item(0).ToString
                                                    End Select
                                                Else
                                                    If xmlR.NodeType = XmlNodeType.Comment Then Exit Do
                                                End If
                                            End If
                                        Loop
                                    Case " Folding Slots "
                                        Do
                                            While Not xmlR.IsStartElement
                                                xmlR.Read()
                                                If xmlR.EOF Then
                                                    Exit Do
                                                End If
                                            End While
                                            Dim nSlot As New sSlot
                                            'nSlot.user = ClientConfig.Configuration.UserInformation.user
                                            'nSlot.team = ClientConfig.Configuration.UserInformation.team
                                            'nSlot.passkey = ClientConfig.Configuration.UserInformation.passkey
                                            ' Only works if order of sections doesn't change!
                                            Dim nUser As New clsClientConfig.clsConfiguration.clsExtraUser
                                            nSlot.id = xmlR.Item(0).ToString
                                            nUser.slot = nSlot.id
                                            nSlot.type = xmlR.Item(1).ToString
                                            Do
                                                If xmlR.Read Then
                                                    If xmlR.NodeType = XmlNodeType.Element Then
                                                        nSlot.AddArgument(xmlR.Name, xmlR.Item(0).ToString)
                                                    Else
                                                        If xmlR.NodeType = XmlNodeType.EndElement Then
                                                            mslots.Add(nSlot)
                                                            If nUser.user <> "" Or nUser.team <> "" Or nUser.passkey <> "" Then mXUsers.Add(nUser)
                                                            Exit Do
                                                        End If
                                                    End If
                                                End If
                                            Loop
                                        Loop
                                    Case Else
                                        Dim nSection As New clsConfigSection
                                        nSection.Name = xmlR.Value
                                        xmlR.Read()
                                        Do
                                            If xmlR.NodeType = XmlNodeType.Element Then
                                                If xmlR.Name = "slot" Then
                                                    Dim nSlot As New sSlot
                                                    nSlot.type = xmlR.Value
                                                    nSlot.id = slots.Count + 1
                                                    slots.Add(nSlot)
                                                End If
                                                Try
                                                    If xmlR.HasAttributes Then nSection.AddArgument(xmlR.Name, xmlR.Item(0).ToString)
                                                    If xmlR.HasValue Then nSection.AddArgument(xmlR.Name, xmlR.Value.ToString)
                                                Catch ex As Exception

                                                End Try
                                            End If
                                            xmlR.Read()
                                        Loop Until xmlR.NodeType = XmlNodeType.Comment Or xmlR.EOF
                                        ConfigSections.Add(nSection)
                                End Select
                            End If
                            If Not xmlR.EOF And Not xmlR.NodeType = XmlNodeType.Comment Then xmlR.Read()
                        Loop While Not xmlR.EOF
                    Else
                        If DoLog Then LogWindow.WriteLog("Could not parse the FAHClient.xml file.")
                        Return False
                    End If
                End Using
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Function ReadFile(Optional ByVal DoLog As Boolean = False, Optional ByVal FillLocation As Boolean = False) As Boolean
            Try
                If FillLocation Then
                    If Not SetLocations() Then
                        If DoLog Then LogWindow.WriteLog("Can not find config.xml.")
                        Return False
                    End If
                End If
                If DoLog Then LogWindow.WriteLog("Reading Config.xml file.")
                Dim strXML As String = My.Computer.FileSystem.ReadAllText(DataLocation & "\Config.xml")
                ConfigString = strXML
                strXML = strXML.Replace("'", Chr(34))
                Dim strReader As New IO.StringReader(strXML)
                Using xmlR As Xml.XmlReader = XmlReader.Create(strReader)
                    If xmlR.ReadToFollowing("config") Then
                        xmlR.Read()
                        Do
                            If xmlR.NodeType = XmlNodeType.Comment Then
                                Select Case xmlR.Value
                                    Case " Remote Command Server "
                                        xmlR.Read() ' whitespace 
                                        Do
                                            If xmlR.Read Then
                                                If xmlR.NodeType = XmlNodeType.Element Then
                                                    Select Case xmlR.Name
                                                        Case Is = "password"
                                                            RemoteCommandServer.password = xmlR.Item(0).ToString
                                                        Case Is = "port"
                                                            RemoteCommandServer.port = xmlR.Item(0).ToString
                                                    End Select
                                                Else
                                                    If xmlR.NodeType = XmlNodeType.Comment Then Exit Do
                                                End If
                                            End If
                                        Loop
                                    Case " User Information "
                                        xmlR.Read()
                                        Do
                                            If xmlR.Read Then
                                                If xmlR.NodeType = XmlNodeType.Element Then
                                                    Select Case xmlR.Name
                                                        Case Is = "passkey"
                                                            UserInformation.passkey = xmlR.Item(0).ToString
                                                        Case Is = "user"
                                                            UserInformation.user = xmlR.Item(0).ToString
                                                        Case Is = "team"
                                                            UserInformation.team = xmlR.Item(0).ToString
                                                    End Select
                                                Else
                                                    If xmlR.NodeType = XmlNodeType.Comment Then Exit Do
                                                End If
                                            End If
                                        Loop
                                    Case " Folding Slots "
                                        Do
                                            While Not xmlR.IsStartElement
                                                xmlR.Read()
                                                If xmlR.EOF Then
                                                    Exit Do
                                                End If
                                            End While
                                            Dim nSlot As New sSlot
                                            ' Only works if order of sections doesn't change!
                                            Dim nUser As New clsClientConfig.clsConfiguration.clsExtraUser

                                            If xmlR.NodeType = XmlNodeType.Element Then
                                                nSlot.id = xmlR.Item(0).ToString
                                                nUser.slot = nSlot.id
                                                nSlot.type = xmlR.Item(1).ToString
                                                Do
                                                    If xmlR.Read Then
                                                        If xmlR.Name = "slot" Then
                                                            mslots.Add(nSlot)
                                                            If nUser.user <> "" Or nUser.team <> "" Or nUser.passkey <> "" Then mXUsers.Add(nUser)
                                                            Exit Do
                                                        End If
                                                        If xmlR.NodeType = XmlNodeType.Element Then
                                                            If xmlR.Name = "user" Then
                                                                nUser.user = xmlR.Item(0).ToString
                                                            End If
                                                            If xmlR.Name = "passkey" Then
                                                                nUser.passkey = xmlR.Item(0).ToString
                                                            End If
                                                            If xmlR.Name = "team" Then
                                                                nUser.team = xmlR.Item(0).ToString
                                                            End If
                                                            nSlot.AddArgument(xmlR.Name, xmlR.Item(0).ToString)
                                                        Else
                                                            If xmlR.NodeType = XmlNodeType.EndElement Then
                                                                mslots.Add(nSlot)
                                                                If nUser.user <> "" Or nUser.team <> "" Or nUser.passkey <> "" Then mXUsers.Add(nUser)
                                                                Exit Do
                                                            End If
                                                        End If
                                                    End If
                                                Loop
                                            End If
                                        Loop
                                    Case Else
                                        Dim nSection As New clsConfigSection
                                        nSection.Name = xmlR.Value
                                        xmlR.Read()
                                        Do
                                            If xmlR.NodeType = XmlNodeType.Element Then
                                                If xmlR.Name = "slot" Then
                                                    Dim nSlot As New sSlot
                                                    nSlot.type = xmlR.Value
                                                    nSlot.id = slots.Count + 1
                                                    slots.Add(nSlot)
                                                End If
                                                nSection.AddArgument(xmlR.Name, xmlR.Item(0).ToString)
                                            End If
                                            xmlR.Read()
                                        Loop Until xmlR.NodeType = XmlNodeType.Comment Or xmlR.EOF
                                        ConfigSections.Add(nSection)
                                End Select
                            End If
                            If Not xmlR.EOF And Not xmlR.NodeType = XmlNodeType.Comment Then xmlR.Read()
                        Loop While Not xmlR.EOF
                    Else
                        If DoLog Then LogWindow.WriteLog("Could not parse the FAHClient.xml file.")
                        Return False
                    End If
                End Using
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Function Report(Optional ByVal HideKey As Boolean = False) As String
            Dim sb As New StringBuilder
            Try
                sb.AppendLine("Client config report")
                sb.AppendLine("Remote command server, port: " & RemoteCommandServer.port & " Password length(!): " & RemoteCommandServer.password.Length.ToString)
                sb.AppendLine("User configaration, name: " & UserInformation.user & " Team: " & UserInformation.team & " passkey length: " & UserInformation.passkey.Length.ToString & " valid: " & UserInformation.passkeyValid.ToString)
                sb.AppendLine("Number of folding slots: " & slots.Count)
                For Each singleSlot In slots
                    sb.AppendLine("Slot: " & singleSlot.id & " - " & singleSlot.type)
                    For iInd As Short = 0 To singleSlot.NumberOfArguments - 1
                        sb.AppendLine("-" & singleSlot.Keys(iInd) & ": " & singleSlot.GetValue(singleSlot.Keys(iInd)))
                    Next
                Next
                sb.AppendLine("User count: " & 1 + ExtraUsers.Count)
                For Each xUser In ExtraUsers
                    sb.AppendLine("user: " & xUser.user & " team: " & xUser.team & " passkey present(!): " & (xUser.passkey.Length > 0).ToString)
                Next
                sb.AppendLine("Additional configuration sections: " & ConfigSections.Count)
                For Each cSection In ConfigSections
                    sb.AppendLine(cSection.Name)
                    For iInd As Short = 0 To cSection.NumberOfArguments - 1
                        If Not cSection.Keys(iInd) = "passkey" Then
                            sb.AppendLine("-" & cSection.Keys(iInd) & ": " & cSection.GetValue(cSection.Keys(iInd)))
                        Else
                            sb.AppendLine("-Passkey present")
                        End If

                    Next
                Next
                sb.AppendLine("")
                sb.AppendLine("Original file:")
                If HideKey Then
                    If ConfigString.Contains("<passkey v='") Then
                        sb.Append(ConfigString.Substring(0, ConfigString.IndexOf("<passkey v='") + Len("<passkey v='")))
                        sb.Append("**hidden**")
                        sb.Append(ConfigString.Substring(ConfigString.IndexOf("/>", ConfigString.IndexOf("<passkey v='"))))
                    End If
                Else
                    sb.Append(ConfigString)
                End If
            Catch ex As Exception
                sb.AppendLine("Exception: " & ex.Message)
                sb.AppendLine("-stacktrace: " & ex.StackTrace)
            End Try
            Return sb.ToString
        End Function
        Public Class sSlot
            Public type As String = ""
            Public id As String = ""
          
            Private mArguments As New Dictionary(Of String, String)
            Public Sub AddArgument(ByVal Name As String, ByVal Value As String)
                mArguments.Add(Name, Value)
            End Sub
            Public ReadOnly Property NumberOfArguments As Int16
                Get
                    Return mArguments.Count
                End Get
            End Property
            Public Function HasKey(ByVal Key As String) As Boolean
                Return mArguments.ContainsKey(Key)
            End Function
            Public Function ChangeKey(Key As String, Value As String) As Boolean
                mArguments(Key) = Value
                Return True
            End Function
            Public Function GetValue(ByVal Key As String) As String
                If Not HasKey(Key) Then
                    Return ""
                Else
                    Return mArguments(Key)
                End If
            End Function
            Public Function Keys() As List(Of String)
                Dim lNames As New List(Of String)
                lNames.AddRange(mArguments.Keys)
                Return lNames
            End Function
        End Class
        Private mslots As New List(Of sSlot)
        Public ReadOnly Property slots As List(Of sSlot)
            Get
                Return mslots
            End Get
        End Property
        Public ReadOnly Property Slot(ID As String) As sSlot
            Get
                For Each Slot In mslots
                    If Slot.id = ID Then Return Slot
                Next
            End Get
        End Property
        Public Class clsExtraUser
            Public slot As String ' id NOT index!
            Public user As String = ""
            Public team As String = ""
            Public passkey As String = ""
        End Class
        Private mXUsers As New List(Of clsExtraUser)
        Public ReadOnly Property ExtraUsers As List(Of clsExtraUser)
            Get
                Dim nXUsers As New List(Of clsExtraUser)
                For Each nUser In mXUsers
                    If nUser.passkey = "" Then nUser.passkey = UserInformation.passkey
                    If nUser.team = "" Then nUser.team = UserInformation.team
                    If nUser.user = "" Then nUser.user = UserInformation.user
                    nXUsers.Add(nUser)
                Next
                Return nXUsers
            End Get
        End Property
        Public ReadOnly Property user(ByVal slot As String) As clsExtraUser ' slot = id NOT index!
            Get
                Dim rUser As New clsExtraUser
                rUser.user = ClientConfig.Configuration.UserInformation.user
                rUser.team = ClientConfig.Configuration.UserInformation.team
                rUser.passkey = ClientConfig.Configuration.UserInformation.passkey
                Try
                    For Each xUser In mXUsers
                        If xUser.slot = slot Then
                            rUser.slot = slot
                            If xUser.user <> "" Then rUser.user = xUser.user
                            If xUser.team <> "" Then rUser.team = xUser.team
                            If xUser.passkey <> "" Then rUser.passkey = xUser.passkey
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                End Try
                Return rUser
            End Get
        End Property
        Public Class clsConfigSection
            Public Name As String
            Private mArguments As New Dictionary(Of String, String)
            Public Sub AddArgument(ByVal Name As String, ByVal Value As String)
                mArguments.Add(Name, Value)
            End Sub
            Public ReadOnly Property NumberOfArguments As Int16
                Get
                    Return mArguments.Count
                End Get
            End Property
            Public Function HasKey(ByVal Key As String) As Boolean
                Return mArguments.ContainsKey(Key)
            End Function
            Public Function ChangeKey(Key As String, Value As String) As Boolean
                mArguments(Key) = Value
                Return True
            End Function
            Public Function GetValue(ByVal Key As String) As String
                Try
                    If Not HasKey(Key) Then
                        Return ""
                    Else
                        If mArguments(Key).ToUpper.Contains("BETA") Then
                            Return New String("*", mArguments(Key))
                        Else
                            Return mArguments(Key)
                        End If
                    End If
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                    Return "!VALUE ERROR!"
                End Try
            End Function
            Public Function Keys() As List(Of String)
                Dim lNames As New List(Of String)
                lNames.AddRange(mArguments.Keys)
                Return lNames
            End Function
        End Class
        Private mConfigSection As New List(Of clsConfigSection)
        Public ReadOnly Property ConfigSections As List(Of clsConfigSection)
            Get
                Return mConfigSection
            End Get
        End Property
        Public Class sRemoteCommandServer
            Public password As String = ""
            Public port As String = "36330"
        End Class
        Public Class sUserInformation
            Public passkey As String = ""
            Public user As String = ""
            Public team As String = ""

            Public ReadOnly Property passkeyValid As Boolean
                Get
                    Try
                        If passkey.Length = 32 Then
                            For xInt As Int16 = 0 To 31
                                If Mid(passkey, xInt + 1, 1) <> ("a") And Mid(passkey, xInt + 1, 1) <> ("b") And Mid(passkey, xInt + 1, 1) <> ("c") And Mid(passkey, xInt + 1, 1) <> ("d") And Mid(passkey, xInt + 1, 1) <> ("e") And Mid(passkey, xInt + 1, 1) <> ("f") And Mid(passkey, xInt + 1, 1) <> ("0") And Mid(passkey, xInt + 1, 1) <> ("1") And Mid(passkey, xInt + 1, 1) <> ("2") And Mid(passkey, xInt + 1, 1) <> ("3") And Mid(passkey, xInt + 1, 1) <> ("4") And Mid(passkey, xInt + 1, 1) <> ("5") And Mid(passkey, xInt + 1, 1) <> ("6") And Mid(passkey, xInt + 1, 1) <> ("7") And Mid(passkey, xInt + 1, 1) <> ("8") And Mid(passkey, xInt + 1, 1) <> ("9") Then
                                    Return False
                                End If
                            Next
                            Return True
                        Else
                            Return False
                        End If
                    Catch ex As Exception
                        LogWindow.WriteError(ex.Message, Err)
                        Return False
                    End Try
                End Get
            End Property

        End Class
        Public UserInformation As New sUserInformation
        Public RemoteCommandServer As New sRemoteCommandServer
        Public ConfigString As String = ""
    End Class
    Public Configuration As New clsConfiguration
    Private aPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
    Public ReadOnly Property Port As String
        Get
            Return Configuration.RemoteCommandServer.port
        End Get
    End Property
    Public ReadOnly Property PWD As String
        Get
            Return Configuration.RemoteCommandServer.password
        End Get
    End Property
    Public ReadOnly Property Team As String
        Get
            Return Configuration.UserInformation.team
        End Get
    End Property
    Public ReadOnly Property User As String
        Get
            Return Configuration.UserInformation.user
        End Get
    End Property
    Public ReadOnly Property PassKey As String
        Get
            Return Configuration.UserInformation.passkey
        End Get
    End Property

    Public Function ReadFAHClientConfig(Optional ByVal DoLog As Boolean = False, Optional ByVal FillLocations As Boolean = False) As Boolean
        Try
            Return Configuration.ReadFile(DoLog, FillLocations)
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#Region "sqlite"
    'Public ReadOnly Property ToBlob As Array
    '    Get
    '        'ToBlob
    '        Dim bf As BinaryFormatter = New BinaryFormatter
    '        Dim ms As MemoryStream = New MemoryStream
    '        bf.Serialize(ms, Me)
    '        Return ms.ToArray
    '    End Get
    'End Property
    'Public ReadOnly Property FromBlob(ByVal Blob As Object) As clsClientConfig.clsConfiguration
    '    Get
    '        Dim nInfo As New clsClientInfo.Info
    '        If Not TypeOf (Blob) Is Array Then
    '            Blob = DirectCast(Blob, Byte())
    '        End If
    '        Using stream As New MemoryStream()
    '            stream.Write(Blob, 0, Blob.Length)
    '            stream.Seek(0, SeekOrigin.Begin)
    '            Dim formatter As New BinaryFormatter()
    '            Return (DirectCast(formatter.Deserialize(stream), clsClientConfig.clsConfiguration))
    '        End Using
    '    End Get
    'End Property
#End Region
End Class

