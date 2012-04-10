Imports ZedGraph

'/*
' * FAHWatch7 Copyright Marvin Westmaas ( mtm )
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
Public Class clsZGRAPH
    Implements IDisposable
    Private _frm As New frmZGRAPH
    Friend ReadOnly Property IsFormVisible As Boolean
        Get
            Return _frm.Visible
        End Get
    End Property
    Friend Sub HideForm()
        _frm.Hide()
    End Sub
    Friend Sub CloseForm()
        _frm.bAllowclose = True
        _frm.Close()
    End Sub
    Friend Sub ShowWUFrames(WU As clsWU)
        Try
            If Not _frm.Visible Then _frm.Show()
            _frm.ShowWUFrames(WU)
        Catch ex As Exception

        End Try
    End Sub
    Friend Sub ShowProjectStatistics(Optional TheseStats As clsStatistics.clsProjectStatistics.clsProject = Nothing)
        Try
            If Not _frm.Visible Then _frm.Show()
            _frm.ShowProjectStats(TheseStats)
        Catch ex As Exception

        End Try
    End Sub
    Friend Sub ShowPerformanceStatistics(Optional TheseStats As clsStatistics.clsPerformanceStatistics = Nothing, Optional HistoryForm As frmHistory = Nothing)
        Try
            Throw New NotImplementedException
            If Not IsNothing(TheseStats) Then

            Else

            End If
        Catch ex As Exception

        End Try
    End Sub
    Friend Sub ShowHardwareStatistics(Optional TheseStats As clsStatistics.clsHardwareStatistics = Nothing, Optional HistoryForm As frmHistory = Nothing)
        Try
            Throw New NotImplementedException
            If Not IsNothing(TheseStats) Then


            Else


            End If
        Catch ex As Exception

        End Try
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                _frm.Dispose()
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
