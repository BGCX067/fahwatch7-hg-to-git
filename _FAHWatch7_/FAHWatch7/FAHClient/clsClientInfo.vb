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
' * This class is based on the initial FAHClient info checkin from Harlam357 
' *
'/*	

Imports System.IO
Imports System.Text
Imports Microsoft.Win32
Imports System.Globalization

Friend Class clsClientInfo
    Implements IDisposable
    Friend Class FAHClientInfo
        Inherits Dictionary(Of String, String)
        Implements IDisposable
#Region "Properties"
        Private bIsEmpty As Boolean = True
        Public dtDB As DateTime = #1/1/2000#
        Friend ReadOnly Property IsEmpty As Boolean
            Get
                Return bIsEmpty
            End Get
        End Property
        Friend ReadOnly Property Website As String
            Get
                Return Me("Website").ToString.Replace(Chr(34), "").Replace("Website:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Copyright As String
            Get
                Return Me("Copyright").ToString.Replace(Chr(34), "").Replace("Copyright:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Author As String
            Get
                Return Me("Author").ToString.Replace(Chr(34), "").Replace("Author:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Args As String
            Get
                Return Me("Args").ToString.Replace(Chr(34), "").Replace("Args:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Config As String
            Get
                Return Me("Config").ToString.Replace(Chr(34), "").Replace("Config:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Version As Version
            Get
                Return New Version(Me("Version").ToString.Replace(Chr(34), "").Replace("Version:", "").Trim)
            End Get
        End Property
        Friend ReadOnly Property strVersion As String
            Get
                Return Me("Version").ToString.Replace(Chr(34), "").Replace("Version:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property dtDate As DateTime
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
        Friend ReadOnly Property strDate As String
            Get
                Return Me("Date").ToString.Replace(Chr(34), "").Replace("Date:", "").Trim & " " & Me("Time").ToString.Replace(Chr(34), "").Replace("Time:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property SVN_rev As String
            Get
                Return Me("SVN Rev").ToString.Replace(Chr(34), "").Replace("SVN Rev:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Compiler As String
            Get
                Return Me("Compiler").ToString.Replace(Chr(34), "").Replace("Compiler:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Options As String
            Get
                Return Me("Options").ToString.Replace(Chr(34), "").Replace("Options:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Platform As String
            Get
                Return Me("Platform").ToString.Replace(Chr(34), "").Replace("Platform:", "").Trim
            End Get
        End Property
        Friend Enum eBits
            x86
            x64
        End Enum
        Friend ReadOnly Property Bits As eBits
            Get
                If Me("Bits").ToString.Replace(Chr(34), "").Replace("Bits:", "").Trim = "32" Then
                    Return eBits.x86
                Else
                    Return eBits.x86
                End If
            End Get
        End Property
        Friend ReadOnly Property strBits As String
            Get
                Return Me("Bits").ToString.Replace(Chr(34), "").Replace("Bits:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Mode As String
            Get
                Return Me("Mode").ToString.Replace(Chr(34), "").Replace("Mode:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property OS As String
            Get
                Return Me("OS").ToString.Replace(Chr(34), "").Replace("OS:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property CPU As String
            Get
                Try
                    Return Me("CPU")
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return ""
                End Try
            End Get
        End Property
        Friend ReadOnly Property CPU_ID As String
            Get
                Return Me("CPU ID").ToString.Replace(Chr(34), "").Replace("CPU ID:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property CPUs As Short
            Get
                Return CShort(Me("CPUs").ToString.Replace(Chr(34), "").Replace("CPUs:", "").Trim)
            End Get
        End Property
        Friend ReadOnly Property Memory As String 'maybe convert to long later
            Get
                Return Me("Memory").ToString.Replace(Chr(34), "").Replace("Memory:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Free_Memory As String 'same as above
            Get
                Return Me("Free Memory").ToString.Replace(Chr(34), "").Replace("Free Memory:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Threads As String 'Get all values into enum
            Get
                Return Me("Threads").ToString.Replace(Chr(34), "").Replace("Threads:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property GPUs As Short
            Get
                Return CShort(Me("GPUs").ToString.Replace(Chr(34), "").Replace("GPUs:", "").Trim)
            End Get
        End Property
        Friend ReadOnly Property GPU(ByVal Index As Int32) As sGPU
            Get
                Dim rGPU As New sGPU
                With rGPU
                    .Index = ("GPU " & Index.ToString)
                    .Description = Me("GPU " & Index).Trim()
                End With
                Return rGPU
            End Get
        End Property
        Friend Structure sGPU
            Private _Desc As String, _Ind As String
            Friend Property Index As String
                Get
                    Return _Ind
                End Get
                Set(ByVal value As String)
                    _Ind = value
                End Set
            End Property
            Friend ReadOnly Property gpu_index As Int32
                Get
                    Return Integer.Parse(_Ind.Replace("GPU", "").Trim, CultureInfo.CurrentCulture.NumberFormat)
                End Get
            End Property
            Friend Property Description(Optional ByVal bString As Boolean = True) As String
                Get
                    Return _Desc
                End Get
                Set(ByVal value As String)
                    _Desc = value
                End Set
            End Property
            Friend ReadOnly Property DeviceName As String
                Get
                    If _Desc = "" Then Return String.Empty
                    Return _Desc.Substring(_Desc.IndexOf(Chr(32)) + 1)
                End Get
            End Property
            Friend ReadOnly Property DeviceType As String
                Get
                    If _Desc = "" Then Return vbNullString
                    Return _Desc.Substring(0, _Desc.IndexOf(Chr(32)))
                End Get
            End Property
        End Structure
        Friend ReadOnly Property CUDA As Boolean
            Get
                Return Not Me("CUDA").ToString.ToUpper(CultureInfo.InvariantCulture).Contains("NOT DETECTED")
            End Get
        End Property
        Friend ReadOnly Property On_Battery As Boolean
            Get
                Return CBool(Me("On Battery").ToString.Replace(Chr(34), "").Replace("On Battery:", "").Trim)
            End Get
        End Property
        Friend ReadOnly Property UTC_Offset As Short
            Get
                Return Short.Parse(Me("UTC offset").ToString.Replace(Chr(34), "").Replace("UTC offset:", "").Trim, NumberStyles.Integer)
            End Get
        End Property
        Friend ReadOnly Property PID As Int32
            Get
                Try
                    Return Int32.Parse(Me("PID").ToString.Replace(Chr(34), "").Replace("PID:", "").Trim, NumberStyles.Integer)
                Catch ex As KeyNotFoundException
                    Return 0
                End Try
            End Get
        End Property
        Friend ReadOnly Property CWD As String
            Get
                Return Me("CWD").ToString.Replace(Chr(34), "").Replace("CWD:", "").Trim
            End Get
        End Property
        Friend ReadOnly Property Win32_Service As Boolean
            Get
                Return CBool(Me("Win32 Service").ToString.Replace(Chr(34), "").Replace("Win32 Service:", "").Trim)
            End Get
        End Property
#End Region
#Region "Functions"
        Friend Function ParseString(ByVal TheString As String) As FAHClientInfo
            Try
                Dim lines() As String = TheString.Split(GetChar(Environment.NewLine, 1))
                For Each line In lines
                    If line.Contains(":") Then
                        If Not Me.ContainsKey(line.Substring(0, line.IndexOf(":")).Trim) Then Me.Add(line.Substring(0, line.IndexOf(":")).Trim, line.Substring(line.IndexOf(":") + 1).Trim)
                    End If
                Next
                Return Me
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New clsClientInfo.FAHClientInfo
            End Try
        End Function
        Friend Shared Function Parse(Optional ByVal Location As String = "") As FAHClientInfo
            Dim rInfo As New FAHClientInfo
            Try
                If Location = "" Then
                    If Not IsNothing(ClientConfig.Configuration.ClientLocation) Then
                        Location = ClientConfig.Configuration.ClientLocation
                        Location.Trim(CChar("\"))
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
                        If Not My.Computer.FileSystem.FileExists(Location & "\FAHClient.exe") Then Return New FAHClientInfo
                    End If
                End If
                Dim nP As Process = Nothing
                Try
                    nP = New Process
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
                    Dim lines() As String = infoText.Split(CChar(Environment.NewLine))
                    For Each line In lines
                        If line.Contains(":") Then
                            rInfo.Add(line.Substring(0, line.IndexOf(":")).Trim, line.Substring(line.IndexOf(":") + 1).Trim)
                        End If
                    Next
                Finally
                    If nP IsNot Nothing Then nP.Dispose()
                End Try
                Return rInfo
            Catch ex As Exception
                ' Return empty info, edit later if errors make existing data go 'poof' :)
                WriteError(ex.Message, Err)
                Return rInfo
            End Try
        End Function
        Friend Function Report() As String
            Dim sb As New StringBuilder
            Try
                Try
                    If dtDB <> #1/1/2000# Then
                        sb.AppendLine("FAHCLient info - " & dtDB.ToString("s"))
                    Else
                        sb.AppendLine("FAHClient info")
                    End If
                    For xInt As Int32 = 0 To Me.Keys.Count - 1
                        sb.AppendLine(Me.Keys(xInt) & ": " & Me.Values(xInt))
                    Next
                Catch ex As Exception
                    sb.Append(ex.Message)
                    sb.Append(ex.StackTrace)
                End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
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
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
    Public Info As New FAHClientInfo
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                Info.Dispose()
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
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class