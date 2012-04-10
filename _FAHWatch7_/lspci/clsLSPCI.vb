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
Imports System.Security
Imports System.Security.Permissions
Public Class clsPci
    Implements IDisposable

    Public Class clsEntries
        Public VendorID As String
        Public DeviceID As String
        Public VendorName As String
        Public DeviceDescription As String
    End Class
    Private _NVEntries As New List(Of clsEntries)
    Private _ATIEntries As New List(Of clsEntries)
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
    Private _Report As String = ""
    Public Property Report As String
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
                    Dim nErr As New ErrorEventArgs
                    nErr.Message = ex.Message
                    nErr.Ex = ex
                    nErr.Err = Err()
                    RaiseEvent WriteError(Me, nErr)
                    Err.Clear()
                End Try
                _Report = rSB.ToString
            End If
            Return _Report
        End Get
        Set(value As String)
            _Report = value
        End Set
    End Property
    Public Function FillInfo(Optional location As String = Nothing) As Boolean
        Dim bRet As Boolean = False
        Try
            Dim filePath As String = String.Empty
            If IsNothing(location) Then
                filePath = modLinkDemand.EnvironmentSetting("programfiles")
            Else
                filePath = location
            End If
            If My.Computer.FileSystem.DirectoryExists(filePath & "\FAHClient") AndAlso My.Computer.FileSystem.FileExists(filePath & "\FAHClient\FAHClient.exe") Then

                Dim Text As String
                Using nP As New Process
                    With nP.StartInfo
                        .FileName = "FAHClient.exe"
                        .RedirectStandardOutput = True
                        .CreateNoWindow = True
                        .WorkingDirectory = filePath & "\FAHClient"
                        .Arguments = "--lspci"
                        .UseShellExecute = False
                    End With
                    nP.Start()
                    Using sReader As StreamReader = nP.StandardOutput
                        While Not nP.HasExited
                            Threading.Thread.Sleep(50)
                        End While
                        Text = sReader.ReadToEnd
                    End Using
                End Using

                If Not Text.Contains("VendorID:DeviceID:Vendor Name:Description") Then
                    'not expected
                    Exit Try
                End If

                Dim Lines() As String = Text.Split(GetChar(Environment.NewLine, 1))
                _NVEntries.Clear() : _ATIEntries.Clear()
                For xInt As Int32 = 1 To Lines.Count - 1
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
                    End If
                    If Lines(xInt).Substring(2, 4) = "10de" Then
                        Dim nEntry As New clsEntries
                        Lines(xInt) = Lines(xInt).Trim
                        nEntry.VendorID = Lines(xInt).Substring(2, 4)
                        nEntry.DeviceID = Lines(xInt).Substring(9, 4)
                        nEntry.VendorName = Lines(xInt).Substring(14, Lines(xInt).IndexOf(":", 14) - 14)
                        nEntry.DeviceDescription = Lines(xInt).Substring(Lines(xInt).LastIndexOf(":") + 1)
                        _NVEntries.Add(nEntry)
                    End If
                Next
            End If
            bRet = (_NVEntries.Count + _ATIEntries.Count) > 0
        Catch ex As Exception
            Dim nErr As New ErrorEventArgs
            nErr.Message = ex.Message
            nErr.Ex = ex
            nErr.Err = Err()
            RaiseEvent WriteError(Me, nErr)
            Err.Clear()
            bRet = False
        End Try
        Return bRet
    End Function
    Shared Event WriteError(sender As Object, e As ErrorEventArgs)
    Public Class ErrorEventArgs
        Inherits EventArgs
        Property Message As String
        Property Ex As Exception
        Property Err As ErrObject
    End Class

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

