'/*
' * FAHWatch7 Mail  Copyright Marvin Westmaas ( mtm )
' *
' * Copyright (c) 2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Public Class frmMail
    Private Sub frmMail_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 500, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub frmMail_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 500, NativeMethods.AnimateWindowFlags.AW_CENTER)
        Try
            For Each Provider As String In Mail.MailProviders
                cmbProvider.Items.Add(Provider)
            Next
            cmbProvider.Items.Add("Custom")
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ClearMail()
        txtSMTP.Text = "" : txtPort.Text = "" : txtUsername.Text = "" : txtPassword.Text = ""
    End Sub
    Private Sub cmbProvider_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbProvider.SelectedIndexChanged
        Try
            If cmbProvider.SelectedIndex = -1 Then Exit Sub
            clearMail()
            If Not cmbProvider.SelectedItem.ToString = "Custom" Then
                txtSMTP.Text = Mail.Provider(cmbProvider.SelectedItem.ToString).SMTP
                txtPort.Text = CStr(Mail.Provider(cmbProvider.SelectedItem.ToString).Port)
                If Not Mail.Provider(cmbProvider.SelectedItem.ToString).AccountName = "" Then txtUsername.Text = Mail.Provider(cmbProvider.SelectedItem.ToString).AccountName
                If Not Mail.Provider(cmbProvider.SelectedItem.ToString).AccountPassword = "" Then txtPassword.Text = Mail.Provider(cmbProvider.SelectedItem.ToString).AccountPassword
                cmdRemoveAccount.Enabled = txtUsername.Text <> "" AndAlso txtPassword.Text <> ""

            Else
                txtSMTP.ReadOnly = False
                txtPort.ReadOnly = False
                txtSMTP.Text = "" : txtPort.Text = ""
                txtSMTP.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class