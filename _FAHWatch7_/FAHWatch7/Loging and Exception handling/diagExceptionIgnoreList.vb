'   FAHWatch7 Exception ignore list
'   Copyright (c) 2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports System.Windows.Forms
Public Class diagExceptionIgnoreList
    Private dExceptions As New Dictionary(Of String, Exceptions.StoredExceptions.Exceptions)
    Private bManual As Boolean = False
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub diagExceptionIgnoreList_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            lvExceptions.Columns(3).Width = -2
            Exceptions.StoredExceptions.Init()
            dExceptions = Exceptions.StoredExceptions.dExceptions
            lvExceptions.Items.Clear()
            rtMessage.Clear()
            For Each DictionaryEntry In dExceptions
                Dim nI As New ListViewItem
                nI.Text = DictionaryEntry.Value.Source
                nI.SubItems.Add(DictionaryEntry.Value.ErrorNumber)
                If DictionaryEntry.Value.IsReported Then
                    nI.SubItems.Add("True")
                Else
                    nI.SubItems.Add("False")
                End If
                lvExceptions.Items.Add(nI)
            Next
            If lvExceptions.Items.Count > 0 Then
                lvExceptions.Items(0).Selected = True
            Else
                cmdSendNow.Enabled = False
                cmdRemove.Enabled = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvExceptions_ItemSelectionChanged(sender As System.Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvExceptions.ItemSelectionChanged
        If bManual Then Exit Sub
        Try
            If Not e.IsSelected Then
                rtMessage.Clear()
                cmdSendNow.Enabled = False
                cmdRemove.Enabled = False
            Else
                rtMessage.Text = dExceptions.Values(e.ItemIndex).ErrText
                cmdSendNow.Enabled = Not dExceptions.Values(e.ItemIndex).IsReported
                cmdRemove.Enabled = True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdSendAll_Click(sender As System.Object, e As System.EventArgs) Handles cmdSendAll.Click
        Try
            GroupBox1.Enabled = False : OK_Button.Enabled = False : Me.UseWaitCursor = True
            For Each DictionaryEntry In dExceptions
                If Not DictionaryEntry.Value.IsReported Then
                    If Not Mail.SendCrashReport(DictionaryEntry.Value.ErrText) Then
                        WriteLog("Failed to send an Exception report, smtp status: " & Mail.SMTPStatus, eSeverity.Important)
                        Exceptions.StoredExceptions.UpdateReportedStatus(DictionaryEntry.Value, False)
                        MsgBox("Send mail failed, the smtp server returned: " & Mail.SMTPStatus & New String(CChar(Environment.NewLine), 2) & "Unsend exceptions will be retried at application startup, and cleared at the next upgrade", CType(MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, MsgBoxStyle), "Failed to send Exception report")
                    Else
                        WriteLog("Succesfully send exception report for " & DictionaryEntry.Value.Source & "#" & DictionaryEntry.Value.ExceptionMessage)
                        'WriteLog("Succesfully send exception report for " & Exceptions.StoredExceptions.StoredExceptionIdentifier(DictionaryEntry.Value))
                        Exceptions.StoredExceptions.UpdateReportedStatus(DictionaryEntry.Value, True)
                    End If
                End If
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            Call diagExceptionIgnoreList_Load(Me, New MyEventArgs.EmptyArgs)
            GroupBox1.Enabled = True : OK_Button.Enabled = True : Me.UseWaitCursor = False
        End Try
    End Sub
    Private Sub cmdRemove_Click(sender As System.Object, e As System.EventArgs) Handles cmdRemove.Click
        Try
            If MsgBox("Are you sure?", CType(MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, MsgBoxStyle), "Confirm removal") = MsgBoxResult.Ok Then
                rtMessage.Clear()
                Exceptions.StoredExceptions.RemoveExceptionFromList(dExceptions.Values(lvExceptions.SelectedIndices(0)))
                Call diagExceptionIgnoreList_Load(Me, New MyEventArgs.EmptyArgs)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdClearAll_Click(sender As System.Object, e As System.EventArgs) Handles cmdClearAll.Click
        Try
            If MsgBox("Are you sure?", CType(MsgBoxStyle.OkCancel Or MsgBoxStyle.Question, MsgBoxStyle), "Confirm removal of all stored exceptions") = MsgBoxResult.Ok Then
                sqdata.ClearExceptions()
                Call diagExceptionIgnoreList_Load(Me, New MyEventArgs.EmptyArgs)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdSendNow_Click(sender As System.Object, e As System.EventArgs) Handles cmdSendNow.Click
        Try
            If Mail.SendCrashReport(dExceptions.Values(lvExceptions.SelectedIndices(0)).ErrText) Then
                'WriteLog("Succesfully send exception report for " & Exceptions.StoredExceptions.StoredExceptionIdentifier(dExceptions.Values(lvExceptions.SelectedIndices(0))))
                WriteLog("Succesfully send exception report for " & dExceptions.Values(lvExceptions.SelectedIndices(0)).Source & "#" & dExceptions.Values(lvExceptions.SelectedIndices(0)).ExceptionMessage)
                Exceptions.StoredExceptions.UpdateReportedStatus(dExceptions.Values(lvExceptions.SelectedIndices(0)), True)
                Call diagExceptionIgnoreList_Load(Me, New MyEventArgs.EmptyArgs)
            Else
                WriteLog("Failed to send an Exception report, smtp status: " & Mail.SMTPStatus, eSeverity.Important)
                Exceptions.StoredExceptions.UpdateReportedStatus(dExceptions.Values(lvExceptions.SelectedIndices(0)), False)
                MsgBox("Send mail failed, the smtp server returned: " & Mail.SMTPStatus & New String(CChar(Environment.NewLine), 2) & "Unsend exceptions will be retried at application startup, and cleared at the next upgrade", CType(MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, MsgBoxStyle), "Failed to send Exception report")
                Call diagExceptionIgnoreList_Load(Me, New MyEventArgs.EmptyArgs)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
End Class
