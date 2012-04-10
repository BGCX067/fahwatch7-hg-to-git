'   FAHWatch7 Advanced Options
'   Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Public Class frmAdvancedOptions
    Private bManual As Boolean = False
    Private iEocOnce As Int32 = 0, iEocEnabled As Int32 = 0
#Region "form from options/options from form"
    Private Sub FillFromOptions()
        Try
            bManual = True
            iEocEnabled = 0 : iEocOnce = 0
            For Each EOCAccount In EOCInfo.eocAccounts
                If EOCAccount.Enabled Then iEocEnabled += 1
            Next

            chkMail.Checked = modMySettings.Email_Notify
            chkOverride.Checked = modMySettings.OverrideAffinity_Priority
            cmdAffinityPriority.Enabled = chkOverride.Checked
            gbConfigureNotifications.Enabled = Not rbDisableNotify.Checked
            gbEUE.Enabled = Not rbDisableNotify.Checked AndAlso rbEUE.Checked
            gbIndividualRules.Enabled = Not rbDisableNotify.Checked AndAlso rbRules.Checked
            cmdManageExceptions.Enabled = CBool(Exceptions.StoredExceptions.Count > 0)
            chkDisableCrashReport.Checked = modMySettings.DisableExceptionReport
            chkNoAutoSizeColumns.Checked = modMySettings.NoAutoSizeColumns
            chkConvertUTC.Checked = modMySettings.ConvertUTC
            If modMySettings.HideInactiveMessageStrip Then
                rbHideMessage.Checked = True
            Else
                rbMessageAlways.Checked = True
            End If
            modMySettings.InitializeFilters()
            If modMySettings.NotifyOption = modMySettings.clsNotifyOptions.eNotifyOption.PopUpForm Then
                rbMsg.Checked = True
            Else
                rbTray.Checked = True
            End If
            If modMySettings.Notify_EUE_Always Then
                rbEUE_Always.Checked = True
            Else
                rbEUE_ratio.Checked = True
            End If
            nudRatio_Warning.Value = CDec(modMySettings.NotifyRate)
            txtRatio_Actual.Text = clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRate
            If modMySettings.Notify_UseRules Then
                rbRules.Checked = True
                flp.Enabled = True
                pnlEUE_1.Enabled = False
                pnlEUE_2.Enabled = False
            ElseIf modMySettings.Notify_Failure Then
                rbSlotFail.Checked = True
                flp.Enabled = False
                pnlEUE_1.Enabled = False
                pnlEUE_2.Enabled = False
            ElseIf modMySettings.Notify_EUE Then
                rbEUE.Checked = True
                flp.Enabled = False
                pnlEUE_1.Enabled = True
                pnlEUE_2.Enabled = rbEUE_ratio.Checked
            End If
            modMySettings.InitializeFilters()
            flp.Controls.Clear()
            For Each Rule As modMySettings.clsNFilter In modMySettings.nFilters
                Rule.ucFilter.Init(Rule)
                AddHandler Rule.ucFilter.EnabledChanged, AddressOf FilterEnabledChanged
                flp.Controls.Add(Rule.ucFilter)
            Next
            'Right - EOC
            chkEocIcon.Checked = modMySettings.ShowEOCIcon
            chkEocSigImg.Checked = modMySettings.EOCNotify
            chkDisableEOC.Checked = modMySettings.DisableEOC
            chkEocSigImg.Checked = Not CBool(modMySettings.EocCustomSignature = "")
            txtSignatureImage.Text = modMySettings.EocCustomSignature
            cmdValidate.Enabled = Not txtSignatureImage.Text = ""
            If modMySettings.AlwaysTrack Then
                rbAlwaysTrack.Checked = True
                chkAutoParse.Enabled = False
            Else
                rbDisableTracking.Checked = True
            End If
            lvEOC.Items.Clear()
            cmbEocPrimary.Items.Clear()
            For Each eocAccount As EOCInfo.sEOCAccount In EOCInfo.eocAccounts
                cmbEocPrimary.Items.Add(eocAccount.Username & "(" & eocAccount.Teamnumber & ")")
                Dim nLVI As New ListViewItem
                nLVI.Text = eocAccount.Username
                nLVI.Checked = eocAccount.Enabled
                nLVI.SubItems.Add(eocAccount.Teamnumber)
                lvEOC.Items.Add(nLVI)
                lvEOC.Items(lvEOC.Items.Count - 1).Checked = eocAccount.Enabled
            Next
            If modMySettings.FirstRun Then
                cmbEocPrimary.SelectedIndex = 0
            Else
                cmbEocPrimary.SelectedIndex = cmbEocPrimary.Items.IndexOf(modMySettings.primaryEocAccount.Replace("#", "(") & ")")
            End If
            pnlEOC.Enabled = Not chkDisableEOC.Checked
            'Data miner
            chkAutoParse.Checked = modMySettings.ParseLogsOnInterval
            pnlDATAMINER.Enabled = chkAutoParse.Checked AndAlso chkAutoParse.Enabled
            If Not modMySettings.DisableEOC Then
                rbMineSyncEOC.Checked = modMySettings.ParseOnEOCUpdate
            Else
                rbMineSyncEOC.Checked = False
                rbMineSyncEOC.Enabled = False
                rbMineInterval.Checked = True
            End If
            txtParserInterval.Text = modMySettings.ParserInterval.ToString
            chkFocusCheck.Checked = modMySettings.CheckFocus
            tbParanoia.Value = modMySettings.NotificationLevel
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub FillFromForm()
        Try
            modMySettings.NotificationLevel = tbParanoia.Value
            modMySettings.Email_Notify = chkMail.Checked
            modMySettings.OverrideAffinity_Priority = chkOverride.Checked
            modMySettings.DisableExceptionReport = chkDisableCrashReport.Checked
            modMySettings.NoAutoSizeColumns = chkNoAutoSizeColumns.Checked
            modMySettings.HideInactiveMessageStrip = rbHideMessage.Checked
            modMySettings.CheckFocus = chkFocusCheck.Checked
            modMySettings.ConvertUTC = chkConvertUTC.Checked
            If rbTray.Checked Then
                modMySettings.NotifyOption = modMySettings.clsNotifyOptions.eNotifyOption.TrayIcon
            Else
                modMySettings.NotifyOption = modMySettings.clsNotifyOptions.eNotifyOption.PopUpForm
            End If
            modMySettings.ShowEOCIcon = chkEocIcon.Checked
            modMySettings.EOCNotify = chkEocSigImg.Checked
            modMySettings.clsNotifyOptions.NotifyEUE = rbEUE.Checked
            modMySettings.clsNotifyOptions.notifyFAILURE = rbSlotFail.Checked
            modMySettings.clsNotifyOptions.NotifyRules = rbRules.Checked
            modMySettings.Notify_EUE_Always = rbEUE_Always.Checked
            modMySettings.Notify_EUE_ByRate = rbEUE_ratio.Checked
            modMySettings.NotifyRate = CDbl(nudRatio_Warning.Value)
            For Each Rule As modMySettings.clsNFilter In modMySettings.nFilters
                Rule.ucFilter.Update()
            Next
            Dim nLst As New List(Of EOCInfo.sEOCAccount)
            For Each lvItem As ListViewItem In lvEOC.Items
                Dim nAccount As New EOCInfo.sEOCAccount
                nAccount.Username = lvItem.Text
                nAccount.Teamnumber = lvItem.SubItems(0).ToString
                nAccount.Enabled = lvItem.Checked
                nLst.Add(nAccount)
            Next
            EOCInfo.eocAccounts.Clear()
            EOCInfo.eocAccounts.AddRange(nLst)
            modMySettings.primaryEocAccount = cmbEocPrimary.Text.Replace("(", "#").Replace(")", "")
            modMySettings.DisableEOC = chkDisableEOC.Checked OrElse EOCInfo.primaryAccount.Enabled
            modMySettings.EocCustomSignature = txtSignatureImage.Text
            modMySettings.ParseLogsOnInterval = chkAutoParse.Checked
            modMySettings.ParseOnEOCUpdate = rbMineSyncEOC.Checked
            modMySettings.ParserInterval = TimeSpan.Parse(txtParserInterval.Text)
            modMySettings.SaveSettings()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Form events"
    Private Sub NotifyOptions()
        Try
            If bManual Then Exit Sub
            bManual = True
            If rbSlotFail.Checked Then
                'rbSlotMsg.Checked = CBool(mySettings.Setting("notify_slot_msg"))
                pnlEUE_1.Enabled = False
                pnlEUE_2.Enabled = False
                flp.Enabled = False
            ElseIf rbEUE.Checked Then
                flp.Enabled = False
                pnlEUE_1.Enabled = True
                pnlEUE_2.Enabled = rbEUE_Always.Checked
            ElseIf rbRules.Checked Then
                pnlEUE_1.Enabled = False
                pnlEUE_2.Enabled = False
                gbIndividualRules.Enabled = True
                flp.Enabled = True
            End If
            bManual = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub rbEUE_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbEUE.CheckedChanged
        If bManual Then Exit Sub
        NotifyOptions()
    End Sub
    Private Sub rbRules_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbRules.CheckedChanged
        If bManual Then Exit Sub
        NotifyOptions()
    End Sub
    Private Sub rbSlotFail_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbSlotFail.CheckedChanged
        If bManual Then Exit Sub
        NotifyOptions()
    End Sub
    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Hide()
    End Sub
    Public Sub Init(Optional ParentForm As Form = Nothing)
        Try
            FillFromOptions()
            NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            If IsNothing(Owner) Then
                Me.ShowIcon = True
                Me.Icon = My.Resources.iTray
                Me.ShowInTaskbar = True
                cmdCancel.Enabled = False
                Me.ShowDialog()
                'Me.Show()
                'While delegateFactory.IsFormVisible(Me)
                '    Application.DoEvents()
                'End While
                'While Me.Visible
                '    Application.DoEvents()
                'End While
            Else
                cmdCancel.Enabled = True
                Me.ShowIcon = False
                Me.ShowInTaskbar = False
                Me.ShowDialog(Owner)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
        Try
            FillFromForm()
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub llblSig_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblSig.LinkClicked
        Process.Start("http://folding.extremeoverclocking.com/?nav=IMAGES")
    End Sub
    Private Sub nudParser_ValueChanged(sender As Object, e As System.EventArgs) Handles nudParser.ValueChanged
        Try
            If bManual Then Exit Sub
            Dim ts As New TimeSpan(0, CInt(nudParser.Value) * 15, 0)
            txtParserInterval.Text = ts.ToString
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub rbEUE_ratio_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbEUE_ratio.CheckedChanged
        If bManual Then Exit Sub
        pnlEUE_2.Enabled = rbEUE_ratio.Checked
    End Sub
    Private Sub chkAutoParse_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAutoParse.CheckedChanged
        Try
            If bManual Then Exit Sub
            bManual = True
            pnlDATAMINER.Enabled = chkAutoParse.Checked
            pnlInterval.Enabled = rbMineInterval.Checked
            If EOCInfo.eocAccounts.Count = 0 OrElse EOCInfo.eocAccounts.Count = 0 Or chkDisableEOC.Checked Then
                rbMineSyncEOC.Enabled = False
                rbMineInterval.Checked = True
            End If
            bManual = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub FilterEnabledChanged(sender As Object, e As ucNotifyFilter.eEventArgs)
        If bManual Then Exit Sub
        Try
            bManual = True
            If e.slotID = "-1" Then
                'Client settings overide all slot options, disable/enable slot uc's
                For Each Rule As modMySettings.clsNFilter In modMySettings.nFilters
                    Dim cName As String = Rule.ClientName
                    If cName = e.Client And e.slotID = Rule.SlotID Then
                        'Don't change anything
                    ElseIf cName = e.Client And e.slotID = "-1" And Rule.SlotID <> "-1" Then
                        Rule.ucFilter.Enabled = Not e.IsEnabled
                    End If
                Next
            Else
                'Disable the client's overall rule 
                For Each Rule As modMySettings.clsNFilter In modMySettings.nFilters
                    Dim cName As String = Rule.ClientName
                    Dim sIndex As String = Rule.SlotID
                    If cName = e.Client And sIndex = "-1" Then
                        Rule.fEnabled = False
                    End If
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub chkDisableEOC_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkDisableEOC.CheckedChanged
        Try
            If bManual Then Exit Sub
            pnlEOC.Enabled = Not chkDisableEOC.Checked
            rbMineSyncEOC.Enabled = Not chkDisableEOC.Checked
            If rbMineSyncEOC.Checked And Not rbMineSyncEOC.Enabled Then rbMineInterval.Checked = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmNotifyOptions_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub chkDisableMessage_CheckedChanged(sender As System.Object, e As System.EventArgs)
        Try
            If bManual Then Exit Sub
            rbHideMessage.Checked = modMySettings.HideInactiveMessageStrip
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvEOC_ItemChecked(sender As System.Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvEOC.ItemChecked
        Try
            If bManual Then Exit Sub
            If iEocOnce < iEocEnabled Then
                iEocOnce += 1
                'Checked listbox check changed event does not fire untill after form.show ( can't use bManual on 'fill from settings' )
                Exit Sub
            End If
            If e.Item.Text & "(" & e.Item.SubItems(1).Text & ")" = cmbEocPrimary.SelectedItem.ToString And e.Item.Checked = False Then
                bManual = True
                e.Item.Checked = True
                bManual = False
                If MsgBox("You can't disable your primary account, do you want to disable the ExtremeOverclocking f@h stats tracker instead?", CType(MsgBoxStyle.Question Or MsgBoxStyle.OkCancel, MsgBoxStyle)) = MsgBoxResult.Ok Then
                    chkDisableEOC.Checked = True
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmNotifyOptions_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        bManual = False
    End Sub
    Private Sub tbParanoia_ValueChanged(sender As System.Object, e As System.EventArgs) Handles tbParanoia.ValueChanged
        If tbParanoia.Value = 0 Then
            If rbTray.Checked Then
                ToolTip1.SetToolTip(tbParanoia, "There will be a static Alert icon in your system tray")
            Else
                ToolTip1.SetToolTip(tbParanoia, "The alert dialog will show behind current topmost windows")
            End If
        ElseIf tbParanoia.Value = 1 Then
            If rbTray.Checked Then
                ToolTip1.SetToolTip(tbParanoia, "There will be an animated Alert icon in your system tray")
            Else
                ToolTip1.SetToolTip(tbParanoia, "The alert dialog will show infront of current windows, but won't prevent focus changes")
            End If
        ElseIf tbParanoia.Value = 2 Then
            If rbTray.Checked Then
                ToolTip1.SetToolTip(tbParanoia, "There will be an animated Alert icon in your system tray, which show a balloon tip warning every 30 seconds!")
            Else
                ToolTip1.SetToolTip(tbParanoia, "The alert dialog will show infront of current windows and will disable changing focus to other windows untill dismissed!")
            End If
        End If
    End Sub
    Private Sub rbDisableTracking_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbDisableTracking.CheckedChanged
        pnAutoParse.Enabled = rbDisableTracking.Checked
        chkAutoParse.Enabled = rbDisableTracking.Checked
    End Sub
    Private Sub rbDisableNotify_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbDisableNotify.CheckedChanged
        If bManual Then Exit Sub
        gbConfigureNotifications.Enabled = Not rbDisableNotify.Checked
        gbEUE.Enabled = Not rbDisableNotify.Checked AndAlso rbEUE.Checked
        gbIndividualRules.Enabled = Not rbDisableNotify.Checked AndAlso rbRules.Checked
        chkUnreachableClients.Enabled = Not rbDisableNotify.Checked
    End Sub
    Private Sub chkDisableCrashReport_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkDisableCrashReport.CheckedChanged
        If bManual Then Exit Sub
        cmdManageExceptions.Enabled = CBool(Exceptions.StoredExceptions.Count > 0)
    End Sub
    Private Sub rbTray_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbTray.CheckedChanged, rbMsg.CheckedChanged
        Call tbParanoia_ValueChanged(Me, Nothing)
    End Sub
    Private Sub cmdConfigureMail_Click(sender As System.Object, e As System.EventArgs) Handles cmdConfigureMail.Click
        Mail.ShowMailSettings()
    End Sub
    Private Sub cmdResetSettings_Click(sender As System.Object, e As System.EventArgs) Handles cmdResetSettings.Click
        Try
            diagClearSettings = New diagClearSettings
            diagClearSettings.ShowDialog(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub chkOverride_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkOverride.CheckedChanged
        If bManual Then Exit Sub
        cmdAffinityPriority.Enabled = chkOverride.Checked
    End Sub
    Private Sub cmdAffinityPriority_Click(sender As System.Object, e As System.EventArgs) Handles cmdAffinityPriority.Click
        AffinityPriorityOverride.ShowForm()
    End Sub
#End Region
End Class