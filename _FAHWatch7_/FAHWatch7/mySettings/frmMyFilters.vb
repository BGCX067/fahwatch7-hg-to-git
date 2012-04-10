'   FAHWatch7 sql query 
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Public Class frmMyFilters
    Private bAllowClose As Boolean = False
    Private _activeFilter As clsFilters
    Private Sub frmMyFilters_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_HIDE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
        'Try
        '    If e.CloseReason = CloseReason.WindowsShutDown Or e.CloseReason = CloseReason.ApplicationExitCall Then
        '        bAllowClose = True
        '        NativeMethods.AnimateWindow(Me.Handle, 100, CType(NativeMethods.AnimateWindowFlags.AW_HIDE + NativeMethods.AnimateWindowFlags.AW_BLEND, NativeMethods.AnimateWindowFlags))
        '    End If
        '    If Not bAllowClose Then
        '        e.Cancel = True
        '        delegateFactory.HideFade(Me, 100)
        '    End If
        'Catch ex As Exception
        '    WriteError(ex.Message, Err)
        'End Try
    End Sub

    Private Sub frmMyFilters_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
    End Sub
    Private Sub frmMyFilters_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            lbFilters.Items.Clear()
            For Each fName As String In sqlFilters.FilterNames
                lbFilters.Items.Add(fName)
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lbFilters_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lbFilters.SelectedIndexChanged
        Try
            If lbFilters.SelectedIndex = -1 Then Exit Sub
            txtSQL.Text = sqlFilters.sql(lbFilters.Items(lbFilters.SelectedIndex).ToString)
            Select Case lbFilters.SelectedItem.ToString
                Case Is = "EUE"
                    cmdRemove.Enabled = False
                    cmdValidate.Enabled = False
                    GroupBox1.Enabled = False
                Case Is = "dumped"
                    cmdRemove.Enabled = False
                    cmdValidate.Enabled = False
                    GroupBox1.Enabled = False
                Case Else
                    cmdRemove.Enabled = True
                    cmdValidate.Enabled = True
                    GroupBox1.Enabled = True
            End Select
            txtSQL.Focus()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        Try
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        Try
            Dim rVal As String = InputBox("Select a name for the filter.")
            If rVal = "" Then Exit Sub
            lbFilters.Items.Add(rVal)
            lbFilters.SelectedIndex = lbFilters.Items.IndexOf(rVal)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdValidate_Click(sender As System.Object, e As System.EventArgs) Handles cmdValidate.Click
        Try
            cmdValidate.Enabled = False
            Me.UseWaitCursor = True
            Dim myTest As List(Of clsWU) = sqdata.WorkUnits(txtSQL.Text)
            Application.DoEvents()
            If Not sqdata.LastSQL = String.Empty Then
                txtSQL.AppendText(Environment.NewLine & sqdata.LastSQL)
                cmdStore.Visible = False
            Else
                cmdStore.Visible = True
            End If
        Catch ex As Exception
            WriteLog("The following error was reported when testing the sql query - " & txtSQL.Text.Replace(Environment.NewLine & sqdata.LastSQL, ""), eSeverity.Important)
            WriteLog(ex.Message, eSeverity.Important)
        Finally
            cmdValidate.Enabled = True
            Me.UseWaitCursor = False
        End Try
    End Sub
    Private Sub cmdStore_Click(sender As System.Object, e As System.EventArgs) Handles cmdStore.Click
        Try
            If sqlFilters.AddFilter(lbFilters.Items(lbFilters.SelectedIndex).ToString, txtSQL.Text) Then cmdStore.Hide()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub txtSQL_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSQL.TextChanged
        Try
            cmdStore.Hide()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
End Class