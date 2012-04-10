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
Namespace fInfo
    <Serializable()>
    Public Class clsPci
        Public Class clsEntries
            Public VendorID As String
            Public DeviceID As String
            Public VendorName As String
            Public DeviceDescription As String
        End Class
        Private _Entries As New List(Of clsEntries)
        Public ReadOnly Property Entries As List(Of clsEntries)
            Get
                Return _Entries
            End Get
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
                        Threading.Thread.SpinWait(2500)
                        Application.DoEvents()
                    End While
                    Dim aText As String = sReader.ReadToEnd
                    If Not aText.Contains("VendorID:DeviceID:Vendor Name:Description") Then
                        ' TODO decide to check anyway
                        Return False
                    End If
                    Dim Lines() As String = aText.Split(Environment.NewLine)
                    _Entries.Clear()
                    For xInt As Int16 = 1 To Lines.Count - 1
                        If Lines(xInt).Contains(vbNewLine) Then Lines(xInt) = Lines(xInt).Replace(vbNewLine, "")
                        If Lines(xInt).Trim = "" Then Exit For
                        Dim nEntry As New clsEntries
                        Lines(xInt) = Lines(xInt).Trim
                        nEntry.VendorID = Lines(xInt).Substring(2, 4)
                        nEntry.DeviceID = Lines(xInt).Substring(9, 4)
                        nEntry.VendorName = Lines(xInt).Substring(14, Lines(xInt).IndexOf(":", 14) - 14)
                        nEntry.DeviceDescription = Lines(xInt).Substring(Lines(xInt).LastIndexOf(":") + 1)
                        _Entries.Add(nEntry)
                    Next
                End If
                Return _Entries.Count > 0
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class

End Namespace
