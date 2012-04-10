'/*
' * fInfo frmSettings copyright Marvin Westmaas ( mtm )
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
Public Class frmSettings
    Private bAllowClose As Boolean = False
    Public ReadOnly Property Allowclose As Boolean
        Get
            Return bAllowClose
        End Get
    End Property
    Public Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub

    Public Overloads Sub Show()
        Try
            pbOpt.Image = My.Resources.Red.ToBitmap
            pbSQ.Image = My.Resources.Red.ToBitmap
            pbOHM.Image = My.Resources.Red.ToBitmap
            chkAUTORUN.Checked = Settings.MySettings.StartWithWindows
            chkPCI.Checked = Settings.MySettings.lsPCI
            nudO.Value = Settings.MySettings.intOPT
            nudSQ.Value = Settings.MySettings.intSQ
            nudOHM.Value = Settings.MySettings.intOHM
            If ExtClient.Client.IsSQUpdating Then pbSQ.Image = My.Resources.Yellow.ToBitmap
            If ExtClient.Client.IsOptUpdating Then pbOpt.Image = My.Resources.Yellow.ToBitmap
            If ExtClient.ohmInt.IsUpdating Then pbOHM.Image = My.Resources.Yellow.ToBitmap
            chkEOC.Checked = Settings.MySettings.UseEOC
            Select Case Settings.MySettings.HistoryLimit
                Case fInfo.clsSettings.sSettings.eHistoryLimit.Minimal
                    cmbHistoryLimit.Text = cmbHistoryLimit.Items(0)
                Case fInfo.clsSettings.sSettings.eHistoryLimit.OneDay
                    cmbHistoryLimit.Text = cmbHistoryLimit.Items(1)
                Case fInfo.clsSettings.sSettings.eHistoryLimit.OneWeek
                    cmbHistoryLimit.Text = cmbHistoryLimit.Items(2)
                Case fInfo.clsSettings.sSettings.eHistoryLimit.OneMonth
                    cmbHistoryLimit.Text = cmbHistoryLimit.Items(3)
                Case fInfo.clsSettings.sSettings.eHistoryLimit.None
                    cmbHistoryLimit.Text = cmbHistoryLimit.Items(4)
            End Select
            chkEOCConfirmDelete.Checked = Settings.MySettings.EOCConfirmDelete
            chkEOCPopup.Checked = Settings.MySettings.EOCNotify
            chkEOCIcon.Checked = Settings.MySettings.EOCIcon

            chkPCI.Checked = Settings.MySettings.lsPCI
            chkAUTORUN.Checked = Settings.MySettings.StartWithWindows
            chkEUE.Checked = Settings.MySettings.TrackEUE
            For Each NonFatal As String In Settings.MySettings.NonFatal
                lbEUE.Items.Add(NonFatal)
            Next
            If CDec(Settings.MySettings.intOPT) < nudO.Minimum Then
                nudO.Value = nudO.Minimum
            Else
                If CDec(Settings.MySettings.intOPT) > nudO.Maximum Then
                    nudO.Value = nudO.Maximum
                Else
                    nudO.Value = CDec(Settings.MySettings.intOPT)
                End If
            End If
            If CDec(Settings.MySettings.intOHM) < nudOHM.Minimum Then
                nudOHM.Value = nudOHM.Minimum
            Else
                If CDec(Settings.MySettings.intOHM) > nudOHM.Maximum Then
                    nudOHM.Value = nudOHM.Maximum
                Else
                    nudOHM.Value = CDec(Settings.MySettings.intOHM)
                End If
            End If
            If CDec(Settings.MySettings.intSQ) < nudSQ.Minimum Then
                nudSQ.Value = nudSQ.Minimum
            Else
                If CDec(Settings.MySettings.intSQ) > nudSQ.Maximum Then
                    nudSQ.Value = nudSQ.Maximum
                Else
                    nudSQ.Value = CDec(Settings.MySettings.intSQ)
                End If
            End If
            chkConfirmExit.Checked = Settings.MySettings.ConfirmExit
            chkStartClient.Checked = Settings.MySettings.AutoStartClient

            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
            Me.Focus()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmSettings_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not Allowclose Then
            e.Cancel = True
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
        End If
    End Sub

    Private Sub cmSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
        Try
            LogWindow.WriteLog("Applying new settings.")
            With Settings.MySettings
                .Automation = True
                .StartWithWindows = chkAUTORUN.Checked
                .AutoStartClient = chkStartClient.Checked
                .lsPCI = chkPCI.Checked
                .intOHM = nudOHM.Value
                .intOPT = nudO.Value
                .intSQ = nudSQ.Value
                .ConfirmExit = chkConfirmExit.Checked
                Select Case cmbHistoryLimit.GetItemText(cmbHistoryLimit.SelectedIndex)
                    Case "Minimal"
                        .HistoryLimit = fInfo.clsSettings.sSettings.eHistoryLimit.Minimal
                    Case "One day"
                        .HistoryLimit = fInfo.clsSettings.sSettings.eHistoryLimit.OneDay
                    Case "One week"
                        .HistoryLimit = fInfo.clsSettings.sSettings.eHistoryLimit.OneWeek
                    Case "One month"
                        .HistoryLimit = fInfo.clsSettings.sSettings.eHistoryLimit.OneMonth
                    Case "None"
                        .HistoryLimit = fInfo.clsSettings.sSettings.eHistoryLimit.None
                End Select
                .EOCConfirmDelete = chkEOCConfirmDelete.Checked
                .EOCIcon = chkEOCIcon.Checked
                .UseEOC = chkEOC.Checked
                .EOCNotify = chkEOCPopup.Checked
                Settings.SaveSettings()
            End With
            If ExtClient.Client.IsOptUpdating Then
                ExtClient.Client.StartUpdates(nudO.Value, nudSQ.Value)
                LogWindow.WriteLog("Set client update intervals to- Options:" & nudO.Value.ToString & " Slot/Queue:" & nudSQ.Value.ToString)
            End If
            If ExtClient.ohmInt.IsUpdating Then
                ExtClient.ohmInt.AutoUpdate(nudOHM.Value)
            End If
            If Settings.MySettings.StartWithWindows Then
                Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
                rKey.SetValue("fInfo", Application.ExecutablePath)
                rKey.Close()
            Else
            Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            If rKey.GetValueNames.Contains("fInfo") Then
                rKey.DeleteValue("fInfo")
            End If
            rKey.Close()
            End If

            MsgBox("Settings applied!")
            Show()
            Me.Visible = False
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub chkEOC_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkEOC.CheckedChanged
        gbEOC.Enabled = chkEOC.Checked
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click
        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
    End Sub
    Private Sub frmSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size
        pbOpt.Image = My.Resources.Red.ToBitmap
        pbSQ.Image = My.Resources.Red.ToBitmap
        pbOHM.Image = My.Resources.Red.ToBitmap
        chkAUTORUN.Checked = Settings.MySettings.StartWithWindows
        chkPCI.Checked = Settings.MySettings.lsPCI
        nudO.Value = Settings.MySettings.intOPT
        nudSQ.Value = Settings.MySettings.intSQ
        nudOHM.Value = Settings.MySettings.intOHM
        If ExtClient.Client.IsSQUpdating Then pbSQ.Image = My.Resources.Yellow.ToBitmap
        If ExtClient.Client.IsOptUpdating Then pbOpt.Image = My.Resources.Yellow.ToBitmap
        If ExtClient.ohmInt.IsUpdating Then pbOHM.Image = My.Resources.Yellow.ToBitmap
        chkEOC.Checked = Settings.MySettings.UseEOC
        Select Case Settings.MySettings.HistoryLimit
            Case fInfo.clsSettings.sSettings.eHistoryLimit.Minimal
                cmbHistoryLimit.Text = cmbHistoryLimit.Items(0)
            Case fInfo.clsSettings.sSettings.eHistoryLimit.OneDay
                cmbHistoryLimit.Text = cmbHistoryLimit.Items(1)
            Case fInfo.clsSettings.sSettings.eHistoryLimit.OneWeek
                cmbHistoryLimit.Text = cmbHistoryLimit.Items(2)
            Case fInfo.clsSettings.sSettings.eHistoryLimit.OneMonth
                cmbHistoryLimit.Text = cmbHistoryLimit.Items(3)
            Case fInfo.clsSettings.sSettings.eHistoryLimit.None
                cmbHistoryLimit.Text = cmbHistoryLimit.Items(4)
        End Select
        chkEOCConfirmDelete.Checked = Settings.MySettings.EOCConfirmDelete
        chkEOCPopup.Checked = Settings.MySettings.EOCNotify
        chkEOCIcon.Checked = Settings.MySettings.EOCIcon
    End Sub
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
                'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
            Case Else
                'Make sure you pass unhandled messages back to the default message handler.
                MyBase.WndProc(m)
        End Select
    End Sub

    Private Sub gbEOC_Enter(sender As System.Object, e As System.EventArgs) Handles gbEOC.Enter

    End Sub
End Class