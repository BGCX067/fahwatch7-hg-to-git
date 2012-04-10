'   FAHWatch7 Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
Friend Class clsFilters
    Implements IDisposable
#Region "Filter collection"
    Private mFilters As New Dictionary(Of String, String)
    Friend ReadOnly Property Filters As Dictionary(Of String, String)
        Get
            Return mFilters
        End Get
    End Property
    Friend ReadOnly Property FilterNames As List(Of String)
        Get
            Return mFilters.Keys.ToList
        End Get
    End Property
    Friend Function AddFilter(Name As String, sql As String) As Boolean
        Try
            If mFilters.ContainsKey(Name) Then
                mFilters(Name) = sql
                Return True
            End If
            If mFilters.ContainsValue(sql) Then Return False
            mFilters.Add(Name, sql)
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend ReadOnly Property sql(FilterName As String) As String
        Get
            If Not mFilters.ContainsKey(FilterName) Then Return ""
            Return mFilters(FilterName)
        End Get
    End Property
    Friend Function GetSqlClientLimit(ClientName As String) As String
        Try
            Return "where Client ='" & FormatSQLString(ClientName, False, True) & "' "
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return String.Empty
        End Try
    End Function
    Friend Function GetSqlHardwareLimit(Hardware As String) As String
        Try
            Return "where HW ='" & Hardware & "' "
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return String.Empty
        End Try
    End Function
    Friend Function GetSqlProjectLimit(ProjectNumber As String) As String
        Try
            Return "where Project = " & CInt(ProjectNumber) & " "
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return String.Empty
        End Try
    End Function
    Friend Function GetSqlProjectRangeLimit(iLower As Integer, iUpper As Integer) As String
        Try
            Return "where Project >= " & CInt(iLower) & " and Project <= " & CInt(iUpper) & " "
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return String.Empty
        End Try
    End Function
#End Region
#Region "Form"
    Private mForm As New frmMyFilters
    Private hndParent As IntPtr
    Friend ReadOnly Property IsVisible As Boolean
        Get
            Return delegateFactory.IsFormVisible(mForm)
        End Get
    End Property
    Friend Sub ShowForm(Optional owner As Form = Nothing)
        Try
            Try
                If IsNothing(mForm) Or mForm.IsDisposed Or mForm.Disposing Then mForm = New frmMyFilters
            Catch ex As Exception
                mForm = New frmMyFilters
            End Try

            NativeMethods.AnimateWindow(mForm.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            If IsNothing(owner) Then
                mForm.ShowDialog()
                'delegateFactory.ShowFade(mForm, 50)
                '_frm.Show()
            Else
                mForm.ShowDialog(owner)
                'hndParent = owner.Handle
                'delegateFactory.EnableForm(owner, False)
                'AddHandler mForm.FormClosed, AddressOf FormClosed
                'delegateFactory.ShowFade(mForm, 50)
                '_frm.ShowDialog(owner)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub FormClosed(sender As Object, e As System.EventArgs)
        delegateFactory.EnableForm(hndParent, True)
    End Sub
    Friend Sub HideForm()
        Try
            delegateFactory.HideFade(mForm, 50)
            '_frm.Hide()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub CloseForm()
        Try
            mForm.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                mForm.Dispose()
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
