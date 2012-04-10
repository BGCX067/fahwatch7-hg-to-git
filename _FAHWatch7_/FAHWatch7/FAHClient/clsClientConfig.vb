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
' *
' * This class is based on the initial FAHClient config checkin from Harlam357 
' *
'/*	

Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.Win32
Imports System.IO
Imports System.Globalization

Friend Class clsClientConfig
    Implements IDisposable
    Friend Class clsConfiguration
        Implements IDisposable
        Private _ClientLocation As String, _DataLocation As String
        Private _ConfigDT As DateTime, _strConfigDT As String ' Add conversions!!!
        Private aPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
#Region "Properties"
        Friend Property strConfigurationDT As String
            Get
                Return _strConfigDT
            End Get
            Set(value As String)
                _strConfigDT = value
            End Set
        End Property
        Friend Property ConfigurationDT As DateTime
            Get
                Return _ConfigDT
            End Get
            Set(value As DateTime)
                _ConfigDT = value
            End Set
        End Property
        Friend ReadOnly Property DataLocation As String
            Get
                Return _DataLocation
            End Get
        End Property
        Friend ReadOnly Property ClientLocation As String
            Get
                Return _ClientLocation
            End Get
        End Property
#Region "Slots"
        Friend Class sSlot
            Friend type As String = ""
            Friend id As String = ""
            Friend mArguments As New Dictionary(Of String, String)
            Friend Sub AddArgument(ByVal Name As String, ByVal Value As String)
                mArguments.Add(Name, Value)
            End Sub
            Friend ReadOnly Property NumberOfArguments As Int32
                Get
                    Return mArguments.Count
                End Get
            End Property
            Friend Function HasKey(ByVal Key As String) As Boolean
                Return mArguments.ContainsKey(Key)
            End Function
            Friend Function ChangeKey(Key As String, Value As String) As Boolean
                mArguments(Key) = Value
                Return True
            End Function
            Friend Function GetValue(ByVal Key As String) As String
                If Not HasKey(Key) Then
                    WriteLog("Accessing a slot ket which doesn't excist - " & Key, eSeverity.Important)
                    Return ""
                Else
                    Return mArguments(Key)
                End If
            End Function
            Friend Function Keys() As List(Of String)
                Dim lNames As New List(Of String)
                lNames.AddRange(mArguments.Keys)
                Return lNames
            End Function
            Friend Overloads Function Equals(Slot As sSlot) As Boolean
                If Slot.NumberOfArguments <> mArguments.Count Then
                    Return False
                ElseIf Slot.type <> Me.type Then
                    Return False
                ElseIf Slot.id <> Me.id Then
                    Return False
                Else
                    For Each DictionaryEntry In mArguments
                        If Not Slot.HasKey(DictionaryEntry.Key) Then
                            Return False
                        ElseIf Slot.GetValue(DictionaryEntry.Key) <> DictionaryEntry.Value Then
                            Return False
                        End If
                    Next
                End If
                Return True
            End Function
        End Class
        Private mslots As New List(Of sSlot)
        Friend ReadOnly Property slots As List(Of sSlot)
            Get
                Return mslots
            End Get
        End Property
        Friend ReadOnly Property Slot(ID As String) As sSlot
            Get
                For Each Slot In mslots
                    If Slot.id.Length = 1 Then
                        If "0" & Slot.id = ID Then Return Slot
                    Else
                        If Slot.id = ID Then Return Slot
                    End If
                Next
                Return New sSlot
            End Get
        End Property
#End Region
#Region "Extra users"
        Friend Class clsExtraUser
            Friend slot As String ' id NOT index!
            Friend user As String = ""
            Friend team As String = ""
            Friend passkey As String = ""
        End Class
        Private mXUsers As New List(Of clsExtraUser)
        Friend ReadOnly Property ExtraUsers As List(Of clsExtraUser)
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
        Friend ReadOnly Property user(ByVal slot As String) As clsExtraUser ' slot = id NOT index!
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
                    WriteError(ex.Message, Err)
                End Try
                Return rUser
            End Get
        End Property
#End Region
#Region "Configuration sections"
        Friend Class clsConfigSection
            Friend Name As String
            Private mArguments As New Dictionary(Of String, String)
            Friend Sub AddArgument(ByVal Name As String, ByVal Value As String)
                mArguments.Add(Name, Value)
            End Sub
            Friend ReadOnly Property NumberOfArguments As Int32
                Get
                    Return mArguments.Count
                End Get
            End Property
            Friend Function HasKey(ByVal Key As String) As Boolean
                Return mArguments.ContainsKey(Key)
            End Function
            Friend Function ChangeKey(Key As String, Value As String) As Boolean
                mArguments(Key) = Value
                Return True
            End Function
            Friend Function GetValue(ByVal Key As String) As String
                Try
                    If Not HasKey(Key) Then
                        Return ""
                    Else
                        If mArguments(Key).ToUpperInvariant.Contains("BETA") Then
                            Return New String(CChar("*"), mArguments(Key).Length)
                        Else
                            Return mArguments(Key)
                        End If
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return "!VALUE ERROR!"
                End Try
            End Function
            Friend Function Keys() As List(Of String)
                Dim lNames As New List(Of String)
                lNames.AddRange(mArguments.Keys)
                Return lNames
            End Function
        End Class
        Private mConfigSection As New List(Of clsConfigSection)
        Friend ReadOnly Property ConfigSections As List(Of clsConfigSection)
            Get
                Return mConfigSection
            End Get
        End Property
#End Region
#Region "Remote command server"
        Friend Class sRemoteCommandServer
            Friend password As String = ""
            Friend port As String = "36330"
        End Class
        Friend RemoteCommandServer As New sRemoteCommandServer
#End Region
#Region "User information"
        Friend Class sUserInformation
            Friend passkey As String = ""
            Friend user As String = ""
            Friend team As String = ""

            Friend ReadOnly Property passkeyValid As Boolean
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
                        WriteError(ex.Message, Err)
                        Return False
                    End Try
                End Get
            End Property

        End Class
        Friend UserInformation As New sUserInformation
#End Region
        Friend ConfigString As String = ""
#End Region
#Region "Functions"
        Friend Function SetLocations(Optional ByVal DoLog As Boolean = False) As Boolean
            Try
                Dim rRoot As RegistryKey
                If DoLog Then WriteLog("Openining registry root LOCAL_MACHINE")
                rRoot = Registry.LocalMachine
                If DoLog Then WriteLog("Trying 'Software\Microsoft\Windows\CurrentVersion\Uninstall\FAHClient'")
                Dim rFKey As RegistryKey = rRoot.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Uninstall\FAHClient")
                If Not IsNothing(rFKey) Then
ReadKey:
                    If DoLog Then WriteLog("Key openend succesfully!")
                    For Each kVName As String In rFKey.GetValueNames
                        If DoLog Then WriteLog("Found '" & kVName & "' value='" & rFKey.GetValue(kVName).ToString & "'")
                        If kVName = "DataDirectory" Then
                            If DoLog Then WriteLog("DataLocation set to " & rFKey.GetValue(kVName).ToString)
                            _DataLocation = rFKey.GetValue(kVName).ToString
                        ElseIf kVName = "DisplayIcon" Then
                            If DoLog Then WriteLog("FAHClient location set to " & rFKey.GetValue(kVName).ToString.Replace("\FAHClient.ico", ""))
                            _ClientLocation = rFKey.GetValue(kVName).ToString.Replace("\FAHClient.ico", "")
                        End If
                    Next
                Else
                    If DoLog Then WriteLog("Opening registry keys in order")
                    If DoLog Then WriteLog(" -> software")
                    rFKey = rRoot.OpenSubKey("SOFTWARE")
                    If Not IsNothing(rFKey) Then
                        If DoLog Then WriteLog(" -> Microsoft")
                        rFKey = rFKey.OpenSubKey("Microsoft")
                        If Not IsNothing(rFKey) Then
                            If DoLog Then WriteLog(" -> Windows")
                            rFKey = rFKey.OpenSubKey("Windows")
                            If Not IsNothing(rFKey) Then
                                If DoLog Then WriteLog(" -> CurrentVersion")
                                rFKey = rFKey.OpenSubKey("CurrentVersion")
                                If Not IsNothing(rFKey) Then
                                    If DoLog Then WriteLog(" -> Uninstall")
                                    rFKey = rFKey.OpenSubKey("Uninstall")
                                    If Not IsNothing(rFKey) Then
                                        If DoLog Then WriteLog(" -> FAHClient")
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
                    If DoLog Then WriteLog("Query application data location")
                    If My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\config.xml") Then
                        If DoLog Then WriteLog("Directory found in this users application data, config.xml found =" & My.Computer.FileSystem.FileExists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\Config.xml"))
                        _DataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient"
                    ElseIf My.Computer.FileSystem.DirectoryExists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient") Then
                        If DoLog Then WriteLog("Directory found in common application data, config.xml found=" & Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient\Config.xml")
                        _DataLocation = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) & "\FAHClient"
                    End If
                    If DataLocation = "" And DoLog = True Then WriteLog("All methods exhausted for data location!")
                End If
                If ClientLocation = "" Then
                    If DoLog Then WriteLog("Searching %path% for FAHClient.exe")
                    Dim dirs() As String = Environment.GetEnvironmentVariable("path").ToString.Split(CChar(";"))
                    For Each dStr As String In dirs
                        If My.Computer.FileSystem.FileExists(dStr & "\FAHClient.exe") Then
                            If DoLog Then WriteLog("Found FAHClient.exe, location=" & dStr)
                            _ClientLocation = dStr
                            Exit For
                        End If
                    Next
                    If ClientLocation = "" And DoLog = True Then WriteLog("All methods exhausted for Client location!")
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return DataLocation <> "" AndAlso ClientLocation <> ""
        End Function
        Friend Function ReadString(ByVal TheString As String, Optional ByVal DoLog As Boolean = False) As Boolean
            Try
                'Remove!
                If DoLog Then WriteLog("Reading Configuration string")
                ConfigString = TheString
                Dim strXML As String = TheString
                strXML = strXML.Replace("'", Chr(34))
                Dim strReader As StringReader = Nothing
                Try
                    Me.mConfigSection.Clear()
                    Me.mslots.Clear()
                    Me.mXUsers.Clear()
                    Me.RemoteCommandServer = New clsClientConfig.clsConfiguration.sRemoteCommandServer
                    Me.UserInformation = New clsClientConfig.clsConfiguration.sUserInformation
                    strReader = New StringReader(strXML)
                    Using xmlR As Xml.XmlReader = XmlReader.Create(strReader)
                        If xmlR.ReadToFollowing("config") Then
                            xmlR.Read()
                            Do
                                If xmlR.NodeType = XmlNodeType.Comment Then
                                    Select Case xmlR.Value.ToString.Trim.ToLower(CultureInfo.InvariantCulture)
                                        Case "remote command server"
                                            WriteDebug("-Reading remote command server settings")
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
                                            WriteDebug("-Finished reading remote command server settings")
                                        Case "user information"
                                            WriteDebug("-Reading user information")
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
                                            Loop Until xmlR.EOF
                                            WriteDebug("-Finished reading user information")
                                        Case "folding slots"
                                            WriteDebug("-Start slot parse")
                                            Do
                                                If Not xmlR.IsStartElement Then xmlR.Read()
                                                If xmlR.Name = "slot" Then
                                                    Dim nSlot As New sSlot
                                                    Dim nUser As New clsClientConfig.clsConfiguration.clsExtraUser
                                                    nSlot.id = xmlR.Item(0).ToString
                                                    nUser.slot = nSlot.id
                                                    nSlot.type = xmlR.Item(1).ToString
                                                    Dim slotXML As String = xmlR.ReadInnerXml
                                                    WriteDebug("-Parsed " & nSlot.id & ":" & nSlot.type)
                                                    If Not slotXML = "" Then
                                                        WriteDebug("-Parsed " & nSlot.id & ":" & nSlot.type & ", from: " & slotXML.Replace(Environment.NewLine, "\n"))
                                                        Dim sReader As StringReader = Nothing
                                                        Dim slotString As String = "<slot>" & slotXML.Trim & "</slot>"
                                                        Try
                                                            sReader = New StringReader(slotString)
                                                            Using ixmlR As XmlReader = XmlReader.Create(sReader)
                                                                sReader = Nothing
                                                                Do
                                                                    If ixmlR.Read Then
                                                                        If ixmlR.NodeType = XmlNodeType.Element Then
                                                                            If Not ixmlR.Name = "slot" Then nSlot.AddArgument(ixmlR.Name, ixmlR.Item(0).ToString)
                                                                        End If
                                                                    Else
                                                                        Exit Do
                                                                    End If
                                                                Loop Until ixmlR.EOF
                                                            End Using
                                                        Finally
                                                            If sReader IsNot Nothing Then sReader.Dispose()
                                                        End Try
                                                    End If
                                                    mslots.Add(nSlot)
                                                    WriteDebug("-Slot added")
                                                End If
                                            Loop Until xmlR.EOF Or xmlR.NodeType = XmlNodeType.Comment
                                        Case Else
                                            Dim nSection As New clsConfigSection
                                            nSection.Name = xmlR.Value.Trim.ToLower(CultureInfo.InvariantCulture)
                                            xmlR.Read()
                                            Do
                                                If xmlR.NodeType = XmlNodeType.Element Then
                                                    If xmlR.Name = "slot" Then
                                                        Dim nSlot As New sSlot
                                                        nSlot.type = xmlR.Value
                                                        nSlot.id = CStr(slots.Count + 1)
                                                        If Not slots.Contains(nSlot) Then mslots.Add(nSlot)
                                                    End If
                                                    Try
                                                        If xmlR.HasAttributes Then nSection.AddArgument(xmlR.Name, xmlR.Item(0).ToString)
                                                        If xmlR.HasValue Then nSection.AddArgument(xmlR.Name, xmlR.Value.ToString)
                                                    Catch ex As Exception
                                                        WriteError(ex.Message, Err)
                                                    End Try
                                                End If
                                xmlR.Read()
                            Loop Until xmlR.NodeType = XmlNodeType.Comment Or xmlR.EOF
                                            ConfigSections.Add(nSection)
                                    End Select
                                End If
                                If Not xmlR.EOF And Not xmlR.NodeType = XmlNodeType.Comment Then xmlR.Read()
                            Loop While Not xmlR.EOF
                            WriteDebug("-Finished parsing client configuration")
                        Else
                            WriteLog("Failed to parse the client configuration", eSeverity.Critical)
                            Return False
                        End If
                    End Using
                    Return True
                Finally
                    If strReader IsNot Nothing Then strReader.Close()
                End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
                WriteLog("- " & ConfigString)
                Return False
            End Try
        End Function
        Friend Function ReadFile(Optional ByVal DoLog As Boolean = False, Optional ByVal FillLocation As Boolean = False) As Boolean
            Try
                If FillLocation Then
                    If Not SetLocations() Then
                        WriteLog("Can not find config.xml", eSeverity.Critical)
                        Return False
                    End If
                End If
                If DoLog Then WriteLog("Reading Config.xml file.")
                Dim strXML As String = My.Computer.FileSystem.ReadAllText(DataLocation & "\Config.xml")
                ConfigString = strXML
                strXML = strXML.Replace("'", Chr(34))
                Return ReadString(strXML, DoLog)
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Friend Function Report(Optional ByVal HideKey As Boolean = False) As String
            Dim sb As New StringBuilder
            Try
                sb.AppendLine("Client config report - " & ConfigurationDT.ToString("s"))
                sb.AppendLine("Remote command server, port: " & RemoteCommandServer.port & " Password length(!): " & RemoteCommandServer.password.Length.ToString)
                sb.AppendLine("User configaration, name: " & UserInformation.user & " Team: " & UserInformation.team & " passkey length: " & UserInformation.passkey.Length.ToString & " valid: " & UserInformation.passkeyValid.ToString)
                sb.AppendLine("Number of folding slots: " & slots.Count)
                For Each singleSlot In slots
                    sb.AppendLine("Slot: " & singleSlot.id & " - " & singleSlot.type)
                    For iInd As Int32 = 0 To singleSlot.NumberOfArguments - 1
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
                    For iInd As Int32 = 0 To cSection.NumberOfArguments - 1
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
#End Region

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
        Friend Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
    Friend Configuration As New clsConfiguration
    Friend ReadOnly Property Port As String
        Get
            Return Configuration.RemoteCommandServer.port
        End Get
    End Property
    Friend ReadOnly Property PWD As String
        Get
            Return Configuration.RemoteCommandServer.password
        End Get
    End Property
    Friend ReadOnly Property Team As String
        Get
            Return Configuration.UserInformation.team
        End Get
    End Property
    Friend ReadOnly Property User As String
        Get
            Return Configuration.UserInformation.user
        End Get
    End Property
    Friend ReadOnly Property PassKey As String
        Get
            Return Configuration.UserInformation.passkey
        End Get
    End Property

    Friend Function ReadFAHClientConfig(Optional ByVal DoLog As Boolean = False, Optional ByVal FillLocations As Boolean = False) As Boolean
        Try
            Return Configuration.ReadFile(DoLog, FillLocations)
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
                Configuration.Dispose()
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
    Friend Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

