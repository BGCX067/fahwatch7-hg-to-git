'   FAHWatch7 options
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Friend Class frmOptions
    Private bAllowClose As Boolean = False, bMinimize As Boolean = False, bManual As Boolean = False
    Private mDialogResult As DialogResult = Windows.Forms.DialogResult.None
    Private hndParent As IntPtr
    Friend Overloads ReadOnly Property DialogResult As DialogResult
        Get
            Return mDialogResult
        End Get
    End Property
#Region "Form functions"
    Friend Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub
    Friend Sub HideForm()
        delegateFactory.HideFade(Me)
    End Sub
    Friend Function ShowOptions(Optional ParentForm As Form = Nothing) As DialogResult
        Try
            bManual = True
            If modMySettings.FirstRun Then
                'Init the default client or remote clients won't show up
                If Not Clients.initialized Then
                    modMySettings.LocalClientName = System.Environment.MachineName
                    Clients.Init(modMySettings.LocalClientName)
                    txtLocalClient.Text = System.Environment.MachineName
                Else
                    txtLocalClient.Text = modMySettings.LocalClientName
                End If
                txtPort.Text = modMySettings.NetworkPort
                cmbDefaultView.Text = cmbDefaultView.Items(0).ToString
                cmbSummaryUrl.Items.Clear()
                cmbSummaryUrl.Items.Add(modMySettings.DefaultSummary)
                For Each alternateSummary As String In modMySettings.AlternateUrls.Urls
                    cmbSummaryUrl.Items.Add(alternateSummary)
                Next
                cmbSummaryUrl.SelectedIndex = 0
                chkStartFC.Checked = modMySettings.StartFC
                chkAutoRun.Checked = modMySettings.StartWithWindows
                bMinimize = modMySettings.MinimizeToTray
                chkMinimizeToTray.Checked = modMySettings.MinimizeToTray
                chkSysTrayStart.Checked = modMySettings.StartMinimized
                chkDisableAutoDownloadSummary.Checked = modMySettings.AutoDownloadSummary
            Else
                FillFromSettings()
            End If
            mDialogResult = Windows.Forms.DialogResult.None
            NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            bManual = False
            If IsNothing(ParentForm) Then
                hndParent = Nothing
                Return Me.ShowDialog()
            Else
                hndParent = ParentForm.Handle
                Return Me.ShowDialog(ParentForm)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return Windows.Forms.DialogResult.None
        End Try
    End Function
    Private Sub FillFromSettings()
        Try
            bManual = True
            cmdCancel.Text = "Close"
            'Fill from settings 
            FillRemoteClients()
            txtLocalClient.Text = modMySettings.LocalClientName
            txtPort.Text = modMySettings.NetworkPort
            cmbSummaryUrl.Items.Clear()
            cmbSummaryUrl.Items.Add(modMySettings.DefaultSummary)
            For Each alternateSummary As String In modMySettings.AlternateUrls.Urls
                cmbSummaryUrl.Items.Add(alternateSummary)
            Next
            cmbSummaryUrl.SelectedIndex = 0
            chkDisableAutoDownloadSummary.Checked = Not modMySettings.AutoDownloadSummary
            cmbDefaultView.Text = modMySettings.DefaultView
            chkStartFC.Checked = modMySettings.StartFC
            chkMinimizeToTray.Checked = modMySettings.MinimizeToTray
            chkSysTrayStart.Checked = modMySettings.StartMinimized
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Try
            If modMySettings.FirstRun Then
                Dim rval As MsgBoxResult = MsgBox("Canceling the configuration process now will require you to configure FAHWatch7 upon the next exectution. Are you sure?", vbOKCancel, "Cancel configuration")
                If rval = vbOK Then
                    WriteLog("Canceling first run wizard and closing application")
                    Me.Visible = False
                    bAllowClose = True
                    mDialogResult = Windows.Forms.DialogResult.Cancel
                    ExitApplication(True, False, True)
                End If
            Else
                mDialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        Try
            modMySettings.DefaultSummary = cmbSummaryUrl.Text
            modMySettings.StartWithWindows = chkAutoRun.Checked
            modMySettings.StartMinimized = chkSysTrayStart.Checked
            modMySettings.StartFC = chkStartFC.Checked
            modMySettings.MinimizeToTray = chkMinimizeToTray.Checked
            modMySettings.LocalClientName = txtLocalClient.Text
            modMySettings.DefaultView = cmbDefaultView.Text
            modMySettings.AutoDownloadSummary = chkDisableAutoDownloadSummary.Checked
            modMySettings.SaveSettings()
            If Not Clients.initialized Then Clients.Init(FormatSQLString(txtLocalClient.Text, False, True))
            mDialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Remote Clients"
    Private WithEvents AddClient As New clsAddClient : Private bRestoreSimple As Boolean = False
    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Try
            If My.Computer.Keyboard.CtrlKeyDown Then
                simpleNetworkbrowser = True
                bRestoreSimple = True
            End If
            If Not Clients.initialized Then
                Clients.Init(FormatSQLString(txtLocalClient.Text, False, True))
            End If
            AddClient.ShowForm(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub AddClient_FormClosed() Handles AddClient.FormClosed
        Try
            FillRemoteClients()
            If bRestoreSimple Then simpleNetworkbrowser = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
        Try
            If IsNothing(lvRClients.SelectedItems) Then Exit Sub : If Not Me.Created Then Exit Sub
            If lvRClients.SelectedItems.Count = 0 Then Exit Sub
            If lvRClients.SelectedItems(0).Text = Clients.LocalClient.ClientName Then
                MsgBox("The local client can not be removed.", vbInformation)
                Exit Sub
            End If
            MsgBox("Removing a remote client will not remove the work unit history already stored in the database.")
            modMySettings.clsRemoteClients.RemoveRemoteClient(lvRClients.SelectedItems(0).Text)
            sqdata.SaveRemoteClients()
            lvRClients.Items.RemoveAt(lvRClients.SelectedIndices(0))
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvRClients_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvRClients.ItemCheck
        Try
            If bManual Then Exit Sub : If Not Me.Created Then Exit Sub
            Dim strName As String = lvRClients.Items(e.Index).Text
            If e.NewValue = CheckState.Checked Then
                modMySettings.clsRemoteClients.SetState(strName, True)
            Else
                modMySettings.clsRemoteClients.SetState(strName, False)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub FillRemoteClients()
        Try
            bManual = True
            lvRClients.Items.Clear()
            For xInt As Int32 = 1 To Clients.Clients.Count - 1
                Dim rc As Client = Clients.Clients(xInt)
                Dim nI As New ListViewItem
                nI.Text = rc.ClientName
                nI.SubItems.Add(rc.ClientLocation)
                nI.SubItems.Add(rc.FCPort)
                If rc.FWPort = "" Then
                    nI.SubItems.Add(Boolean.FalseString)
                Else
                    nI.SubItems.Add(Boolean.TrueString)
                End If
                nI.Checked = rc.Enabled
                lvRClients.Items.Add(nI)
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
#End Region
#Region "Control updates"
    Private Sub chkSysTrayStart_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSysTrayStart.CheckedChanged
        Try
            If bManual Then Exit Sub
            If chkSysTrayStart.Checked Then
                bManual = True
                chkMinimizeToTray.Checked = True
                bManual = False
                chkMinimizeToTray.Enabled = False
            Else
                chkMinimizeToTray.Enabled = True
                chkMinimizeToTray.Checked = bMinimize
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub chkMinimizeToTray_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMinimizeToTray.CheckedChanged
        Try
            If bManual Then Exit Sub
            If chkSysTrayStart.Checked And chkMinimizeToTray.Checked = False Then
                chkMinimizeToTray.Checked = True
            Else
                bMinimize = chkMinimizeToTray.Checked
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

    Private Sub frmOptions_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        delegateFactory.ActivateForm(Me)
    End Sub
End Class