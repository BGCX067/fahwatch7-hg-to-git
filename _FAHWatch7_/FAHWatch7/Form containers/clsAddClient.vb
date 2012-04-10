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
Friend Class clsAddClient
    Implements IDisposable
    Public Event FormClosed(sender As Object, e As System.EventArgs)
    Private fAdd As New frmAddClient
    Friend ReadOnly Property IsFormVisible As Boolean
        Get
            Return delegateFactory.IsFormVisible(fAdd)
        End Get
    End Property
    Friend Sub CloseForm()
        Try
            If IsNothing(fAdd) Or fAdd.IsDisposed Or fAdd.Disposing Then Return
        Catch ex As Exception : End Try
        fAdd.Dispose()
    End Sub
    'Private bInitialized As Boolean = False
    Friend Sub ShowForm(Optional OwnerForm As Form = Nothing)
        Try
            Try
                If IsNothing(fAdd) Or fAdd.IsDisposed Or fAdd.Disposing Then fAdd = New frmAddClient
            Catch ex As Exception
                fAdd = New frmAddClient
            End Try
            AddHandler fAdd.FormClosed, AddressOf fadd_Closed
            If IsNothing(OwnerForm) Then
                fAdd.ShowDialog()
            Else
                fAdd.ShowDialog(OwnerForm)
            End If
            RaiseEvent FormClosed(Me, New MyEventArgs.EmptyArgs)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub fadd_closed(sender As Object, e As EventArgs)
        RaiseEvent FormClosed(Me, New MyEventArgs.EmptyArgs)
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                fAdd.Dispose()
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
