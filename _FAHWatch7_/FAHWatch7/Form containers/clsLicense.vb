'/*
' * FAHWatch7 About class Copyright 2011 Marvin Westmaas ( mtm )
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
Friend Class clsLicense
    Implements IDisposable
    Private frmLicense As New frmLicense
    Friend Sub ShowLicense(Optional ByVal PF As Form = Nothing)
        Try
            Try
                If IsNothing(frmLicense) OrElse frmLicense.IsDisposed Or frmLicense.Disposing Then
                    frmLicense = New frmLicense
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return
            End Try
            frmLicense.rtf.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\license and about\license.txt")
            NativeMethods.AnimateWindow(frmLicense.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            If IsNothing(PF) Then
                frmLicense.ShowDialog()
            Else
                frmLicense.ShowDialog(PF)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub CloseLicense()
        Try
            If Not IsNothing(frmLicense) Then
                If Not frmLicense.IsDisposed OrElse Not frmLicense.Disposing Then
                    frmLicense.Close()
                    frmLicense.Dispose()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                frmLicense.Dispose()
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
