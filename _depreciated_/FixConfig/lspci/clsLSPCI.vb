'/*
' * fInfo lsPCI class Copyright Marvin Westmaas ( mtm ) 
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
Imports System.IO
Imports System.Text

<Serializable()>
    Public Class clsPci
    Public Class clsEntries
        Public VendorID As String
        Public DeviceID As String
        Public VendorName As String
        Public DeviceDescription As String
    End Class
    Private _NVEntries As New List(Of clsEntries)
    Private _ATIEntries As New List(Of clsEntries)
    Private _Entries As New List(Of clsEntries)
    Public ReadOnly Property Nvidia As List(Of clsEntries)
        Get
            Return _NVEntries
        End Get
    End Property
    Public ReadOnly Property Ati As List(Of clsEntries)
        Get
            Return _ATIEntries
        End Get
    End Property
    Public ReadOnly Property Entries As List(Of clsEntries)
        Get
            Return _Entries
        End Get
    End Property
    Private _Report As String = ""
    Public Property report As String
        Get
            If _Report = "" Then
                Dim rSB As New StringBuilder
                Try
                    If _NVEntries.Count > 0 Then
                        rSB.AppendLine("NVIDIA:")
                        For Each Entry As clsEntries In _NVEntries
                            rSB.AppendLine(Entry.VendorID & ":" & Entry.DeviceID & ":" & Entry.VendorName & ":" & Entry.DeviceDescription)
                        Next
                    End If
                    If _ATIEntries.Count > 0 Then
                        rSB.AppendLine("ATI:")
                        For Each entry As clsEntries In _ATIEntries
                            rSB.AppendLine(entry.VendorID & ":" & entry.DeviceID & ":" & entry.VendorName & ":" & entry.DeviceDescription)
                        Next
                    End If
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                End Try
                _Report = rSB.ToString
            End If
            Return _Report
        End Get
        Set(value As String)
            _Report = value
        End Set
    End Property

    Public Function FillInfo() As Boolean
        Try
            Dim aPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), sReader As StreamReader
            If My.Computer.FileSystem.DirectoryExists(aPath & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(aPath & "\FAHClient\FAHClient.exe") Then
                Dim nP As New Process
                With nP.StartInfo
                    .FileName = "FAHClient.exe"
                    .RedirectStandardOutput = True
                    .CreateNoWindow = True
                    .WorkingDirectory = aPath & "\FAHClient"
                    .Arguments = "--lspci"
                    .UseShellExecute = False
                End With
                nP.Start()
                sReader = nP.StandardOutput
                While Not nP.HasExited
                    Threading.Thread.Sleep(500)
                End While
                Dim aText As String = sReader.ReadToEnd
                If Not aText.Contains("VendorID:DeviceID:Vendor Name:Description") Then
                    ' TODO decide to check anyway
                    Return False
                End If
                Dim Lines() As String = aText.Split(Environment.NewLine)
                _NVEntries.Clear() : _ATIEntries.Clear()
                For xInt As Int16 = 1 To Lines.Count - 1
                    If Lines(xInt).Contains(vbNewLine) Then Lines(xInt) = Lines(xInt).Replace(vbNewLine, "")
                    If Lines(xInt).Trim = "" Then Exit For
                    Lines(xInt) = Lines(xInt).Trim()
                    If Lines(xInt).Substring(2, 4) = "1002" Then
                        Dim nEntry As New clsEntries
                        Lines(xInt) = Lines(xInt).Trim
                        nEntry.VendorID = Lines(xInt).Substring(2, 4)
                        nEntry.DeviceID = Lines(xInt).Substring(9, 4)
                        nEntry.VendorName = Lines(xInt).Substring(14, Lines(xInt).IndexOf(":", 14) - 14)
                        nEntry.DeviceDescription = Lines(xInt).Substring(Lines(xInt).LastIndexOf(":") + 1)
                        _ATIEntries.Add(nEntry)
                        _Entries.Add(nEntry)
                    End If
                    If Lines(xInt).Substring(2, 4) = "10de" Then
                        Dim nEntry As New clsEntries
                        Lines(xInt) = Lines(xInt).Trim
                        nEntry.VendorID = Lines(xInt).Substring(2, 4)
                        nEntry.DeviceID = Lines(xInt).Substring(9, 4)
                        nEntry.VendorName = Lines(xInt).Substring(14, Lines(xInt).IndexOf(":", 14) - 14)
                        nEntry.DeviceDescription = Lines(xInt).Substring(Lines(xInt).LastIndexOf(":") + 1)
                        _NVEntries.Add(nEntry)
                        _Entries.Add(nEntry)
                    End If
                    ' Or
                Next
            End If
            Return (_NVEntries.Count + _ATIEntries.Count) > 0
        Catch ex As Exception
            Return False
        End Try
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

