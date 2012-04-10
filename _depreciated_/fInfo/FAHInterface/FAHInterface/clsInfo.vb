'/*
' * Code converted to vb.net from Harlam357 code hosted on http://code.google.com/p/hfm-net/source/browse/#svn%2Ftrunk%2Fsrc%2FHFM.Client 
' * Edits made mtm
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

Imports Newtonsoft.Json
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports System.Runtime.Serialization
Namespace Client
    <Serializable()>
    Public Class Info
        Inherits Dictionary(Of String, String)
        Implements ISerializable
        Implements IEquatable(Of Info)
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
        Public ReadOnly Property GPU(Index As Short) As sGPU
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
                Set(value As String)
                    _Ind = value
                End Set
            End Property
            Public ReadOnly Property GPU_Index As Short
                Get
                    Return CShort(_Ind.Replace("GPU", "").Trim)
                End Get
            End Property
            Public Property Description(Optional bString As Boolean = True) As String
                Get
                    Return _Desc
                End Get
                Set(value As String)
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
        Public Shared Function Parse(value As String) As Info
            Try

                value = value.Substring(value.IndexOf("Website") - 1)
                value = "{" & vbLf & value
                value = value.Replace(Chr(34) & ",", Chr(34) & ": ")
                value = value.Replace("],", "," & vbLf)
                value = value.Replace(": " & vbLf, ": ")
                Dim Client As String = value.Substring(0, value.IndexOf("Build") - 2)
                value = value.Replace(Client, "")
                Client &= vbLf & "}"
                value = value.Replace("Build" & Chr(34) & ":", "")
                value = "{" & vbLf & value.Substring(value.IndexOf("Version") - 1)
                Dim Built As String = value.Substring(0, value.IndexOf("System" & Chr(34) & ":") - 1)
                Built = Built.Substring(0, Built.LastIndexOf(Chr(34)))
                value = value.Replace(Built, "")
                value = "{" & vbLf & value.Substring(value.IndexOf("OS") - 1)
                Built &= Chr(34) & vbLf & "}"
                value &= vbLf & "}"
                Dim o = JObject.Parse(Client)
                Dim info = New Info()
                For Each prop As Object In o.Properties()
                    info.Add(prop.Name, GetValue(prop))
                Next
                o = JObject.Parse(Built)
                For Each prop As Object In o.Properties()
                    info.Add(prop.Name, GetValue(prop))
                Next
                o = JObject.Parse(value)
                For Each prop As Object In o.Properties()
                    info.Add(prop.Name, GetValue(prop))
                Next
                value = Nothing
                Built = Nothing
                Client = Nothing
                o = Nothing
                Return info
            Catch ex As Exception
                ' Return empty info, edit later if errors make existing data go 'poof' :)
                Return New Info
            End Try
        End Function

        Private Shared Function GetValue(prop As JProperty) As Object
            If prop.Value.Type.Equals(JTokenType.[String]) Then
                '  Return DirectCast(prop, String)
                Return DirectCast(prop.ToString, String)
            End If
            If prop.Value.Type.Equals(JTokenType.[Integer]) Then
                Return CInt(prop)
            End If
            Return [String].Empty
        End Function

        Public Function Equals1(other As Info) As Boolean Implements System.IEquatable(Of Info).Equals
            'finish later
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

End Namespace