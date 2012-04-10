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

Imports System.IO
Imports System.Text
Imports Microsoft.Win32
Public Class clsClientInfo
    Public Class Info
        Inherits Dictionary(Of String, String)
        Private bIsEmpty As Boolean = True
        Public ReadOnly Property IsEmpty As Boolean
            Get
                Return bIsEmpty
            End Get
        End Property
        Public ReadOnly Property Website As String
            Get
                Return Me("Website").ToString.Replace(Chr(34), "").Replace("Website:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Copyright As String
            Get
                Return Me("Copyright").ToString.Replace(Chr(34), "").Replace("Copyright:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Author As String
            Get
                Return Me("Author").ToString.Replace(Chr(34), "").Replace("Author:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Args As String
            Get
                Return Me("Args").ToString.Replace(Chr(34), "").Replace("Args:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Config As String
            Get
                Return Me("Config").ToString.Replace(Chr(34), "").Replace("Config:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Version As Version
            Get
                Return New Version(Me("Version").ToString.Replace(Chr(34), "").Replace("Version:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property strVersion As String
            Get
                Return Me("Version").ToString.Replace(Chr(34), "").Replace("Version:", "").Trim
            End Get
        End Property
        Public ReadOnly Property dtDate As DateTime
            Get
                'Idk timezone info, no conversion yet but will change no doubt ;)
                'Find first space
                Dim valuestring As String = Me("Date").ToString.Replace(Chr(34), "").Replace("Date:", "").Trim
                Dim strMonth As String = valuestring.Substring(0, valuestring.IndexOf(" "))
                valuestring = valuestring.Replace(strMonth, "")
                While valuestring(0) = Chr(32)
                    valuestring = valuestring.Substring(1)
                End While
                Dim strDay As String = valuestring.Substring(0, valuestring.IndexOf(Chr(32)))
                Dim strYear As String = valuestring.Replace(strDay, "").Trim
                If Len(strDay) = 1 Then strDay = "0" & strDay
                If Len(strMonth) = 1 Then strMonth = "0" & strMonth
                Dim dtrt As New DateTime
                Try
                    If DateTime.TryParse(strDay & "/" & strMonth & "/" & strYear & " " & Me("Time").ToString.Replace(Chr(34), "").Replace("Time:", "").Trim, dtrt) Then
                        Return dtrt
                    Else
                        Return DateTime.MinValue
                    End If
                Catch ex As Exception
                    Return DateTime.MinValue
                End Try


            End Get
        End Property
        Public ReadOnly Property strDate As String
            Get
                Return Me("Date").ToString.Replace(Chr(34), "").Replace("Date:", "").Trim & " " & Me("Time").ToString.Replace(Chr(34), "").Replace("Time:", "").Trim
            End Get
        End Property
        Public ReadOnly Property SVN_rev As String
            Get
                Return Me("SVN Rev").ToString.Replace(Chr(34), "").Replace("SVN Rev:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Compiler As String
            Get
                Return Me("Compiler").ToString.Replace(Chr(34), "").Replace("Compiler:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Options As String
            Get
                Return Me("Options").ToString.Replace(Chr(34), "").Replace("Options:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Platform As String
            Get
                Return Me("Platform").ToString.Replace(Chr(34), "").Replace("Platform:", "").Trim
            End Get
        End Property
        Public Enum eBits
            x86
            x64
        End Enum
        Public ReadOnly Property Bits As eBits
            Get
                If Me("Bits").ToString.Replace(Chr(34), "").Replace("Bits:", "").Trim = "32" Then
                    Return eBits.x86
                Else
                    Return eBits.x86
                End If
            End Get
        End Property
        Public ReadOnly Property strBits As String
            Get
                Return Me("Bits").ToString.Replace(Chr(34), "").Replace("Bits:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Mode As String
            Get
                Return Me("Mode").ToString.Replace(Chr(34), "").Replace("Mode:", "").Trim
            End Get
        End Property
        Public ReadOnly Property OS As String
            Get
                Return Me("OS").ToString.Replace(Chr(34), "").Replace("OS:", "").Trim
            End Get
        End Property
        Public ReadOnly Property CPU_ID As String
            Get
                Return Me("CPU ID").ToString.Replace(Chr(34), "").Replace("CPU ID:", "").Trim
            End Get
        End Property
        Public ReadOnly Property CPUs As Short
            Get
                Return CShort(Me("CPUs").ToString.Replace(Chr(34), "").Replace("CPUs:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property Memory As String 'maybe convert to long later
            Get
                Return Me("Memory").ToString.Replace(Chr(34), "").Replace("Memory:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Free_Memory As String 'same as above
            Get
                Return Me("Free Memory").ToString.Replace(Chr(34), "").Replace("Free Memory:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Threads As String 'Get all values into enum
            Get
                Return Me("Threads").ToString.Replace(Chr(34), "").Replace("Threads:", "").Trim
            End Get
        End Property
        Public ReadOnly Property GPUs As Short
            Get
                Return CShort(Me("GPUs").ToString.Replace(Chr(34), "").Replace("GPUs:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property GPU(ByVal Index As Short) As sGPU
            Get
                Dim rGPU As New sGPU
                With rGPU
                    .Index = ("GPU " & Index.ToString)
                    .Description = Me("GPU " & Index).ToString.Replace(Chr(34), "").Replace("GPU " & Index.ToString & ":", "").Trim()
                End With
                Return rGPU
            End Get
        End Property
        Public Structure sGPU
            Private _Desc As String, _Ind As String
            Public Property Index As String
                Get
                    Return _Ind
                End Get
                Set(ByVal value As String)
                    _Ind = value
                End Set
            End Property
            Public ReadOnly Property GPU_Index As Short
                Get
                    Return CShort(_Ind.Replace("GPU", "").Trim)
                End Get
            End Property
            Public Property Description(Optional ByVal bString As Boolean = True) As String
                Get
                    Return _Desc
                End Get
                Set(ByVal value As String)
                    _Desc = value
                End Set
            End Property
            Public ReadOnly Property DeviceID As String
                Get
                    If _Desc = "" Then Return vbNullString
                    Return _Desc.Substring(0, _Desc.IndexOf(Chr(32), _Desc.IndexOf(":")))
                End Get
            End Property
        End Structure
        Public ReadOnly Property CUDA As Boolean
            Get
                Return Not Me("CUDA").ToString.ToUpper.Contains("NOT DETECTED")
            End Get
        End Property
        Public ReadOnly Property On_Battery As Boolean
            Get
                Return CBool(Me("On Battery").ToString.Replace(Chr(34), "").Replace("On Battery:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property UTC_Offset As Short
            Get
                Return CShort(Me("UTC offset").ToString.Replace(Chr(34), "").Replace("UTC offset:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property PID As Int16
            Get
                Try
                    Return CInt(Me("PID").ToString.Replace(Chr(34), "").Replace("PID:", "").Trim)
                Catch ex As KeyNotFoundException
                    Return 0
                End Try
            End Get
        End Property
        Public ReadOnly Property CWD As String
            Get
                Return Me("CWD").ToString.Replace(Chr(34), "").Replace("CWD:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Win32_Service As Boolean
            Get
                Return CBool(Me("Win32 Service").ToString.Replace(Chr(34), "").Replace("Win32 Service:", "").Trim)
            End Get
        End Property
        Public Function ParseString(ByVal TheString As String) As Info
            Try
                Dim lines() As String = TheString.Split(Environment.NewLine)
                For Each line In lines
                    If line.Contains(":") Then
                        Me.Add(line.Substring(0, line.IndexOf(":")).Trim, line.Substring(line.IndexOf(":") + 1).Trim)
                    End If
                Next
                Return Me
            Catch ex As Exception

            End Try
        End Function
        Public Shared Function Parse(Optional ByVal Location As String = "") As Info
            Dim rInfo As New Info
            Try
                If Location = "" Then
                    If Not IsNothing(ClientConfig.Configuration.ClientLocation) Then
                        Location = ClientConfig.Configuration.ClientLocation
                        Location.Trim("\")
                    End If
                    If Not My.Computer.FileSystem.FileExists(Location & "\FAHClient.exe") Then
                        Dim rRoot As RegistryKey
                        rRoot = Registry.LocalMachine
                        Dim rFKey As RegistryKey = rRoot.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Uninstall\FAHClient")
                        If Not IsNothing(rFKey) Then
                            For Each kVName As String In rFKey.GetValueNames
                                If kVName = "DisplayIcon" Then
                                    Location = rFKey.GetValue(kVName).ToString.Replace("\FAHClient.ico", "")
                                    Exit For
                                End If
                            Next
                        End If
                        If Not My.Computer.FileSystem.FileExists(Location & "\FAHClient.exe") Then Return New Info
                    End If
                End If
                Dim nP As New Process
                With nP.StartInfo
                    .FileName = "FAHClient.exe"
                    .WorkingDirectory = Location
                    .Arguments = "--info"
                    .CreateNoWindow = True
                    .UseShellExecute = False
                    .RedirectStandardOutput = True
                End With
                nP.Start()
                Dim cOUT As StreamReader = nP.StandardOutput
                Dim infoText As String = cOUT.ReadToEnd
                Dim lines() As String = infoText.Split(Environment.NewLine)
                For Each line In lines
                    If line.Contains(":") Then
                        rInfo.Add(line.Substring(0, line.IndexOf(":")).Trim, line.Substring(line.IndexOf(":") + 1).Trim)
                    End If
                Next
                Return rInfo
            Catch ex As Exception
                ' Return empty info, edit later if errors make existing data go 'poof' :)
                LogWindow.WriteError(ex.Message, Err)
                Return rInfo
            End Try
        End Function
        Public Function Report() As String
            Dim sb As New StringBuilder
            Try
                sb.AppendLine("FAHCLient --info")
                For xInt As Int32 = 0 To Me.Keys.Count - 1
                    sb.AppendLine(Me.Keys(xInt) & ": " & Me.Values(xInt))
                Next
            Catch ex As Exception
                sb.Append(ex.Message)
                sb.Append(ex.StackTrace)
            End Try
            Return sb.ToString
        End Function

#Region "Log extender"
        Public Class cLW
            Public Event Log(ByVal Message As String)
            Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
            Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
                RaiseEvent LogError(Message, EObj)
            End Sub
            Public Sub WriteLog(ByVal Message As String)
                RaiseEvent Log(Message)
            End Sub
        End Class
        Public Shared WithEvents LogWindow As New cLW
        Public Shared Event Log(ByVal Message As String)
        Public Shared Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Private Shared Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
            RaiseEvent Log(Message)
        End Sub
        Private Shared Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
            RaiseEvent LogError(Message, EObj)
        End Sub
#End Region

    End Class
    Public FAHClient_Info As New Info

    'Public ReadOnly Property ToBlob As Array
    '    Get
    '        'ToBlob
    '        Dim bf As BinaryFormatter = New BinaryFormatter
    '        Dim ms As MemoryStream = New MemoryStream
    '        bf.Serialize(ms, Me)
    '        Return ms.ToArray
    '    End Get
    'End Property
    'Public ReadOnly Property FromBlob(ByVal Blob As Object) As clsClientInfo.Info
    '    Get
    '        Dim nInfo As New clsClientInfo.Info
    '        If Not TypeOf (Blob) Is Array Then
    '            Blob = DirectCast(Blob, Byte())
    '        End If
    '        Using stream As New MemoryStream()
    '            stream.Write(Blob, 0, Blob.Length)
    '            stream.Seek(0, SeekOrigin.Begin)
    '            Dim formatter As New BinaryFormatter()
    '            Return (DirectCast(formatter.Deserialize(stream), clsClientInfo.Info))
    '        End Using
    '    End Get
    'End Property
End Class