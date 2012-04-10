'   fTray client options class
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
Imports Microsoft.Win32
Imports Microsoft.Win32.Registry

Public Class frmOptions
    Public WithEvents nADD As New frmAddNF

    Private Sub frmOptions_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            DoGUIfromSettings()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub chkStart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStart.CheckedChanged
        Try
            cmbStart.Enabled = chkStart.Checked
        Catch ex As Exception

        End Try
    End Sub
    Private Sub chkEOC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEOC.CheckedChanged
        Try
            gbEOC.Enabled = chkEOC.Checked
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DoSettingsFromGUI()
        Try
            With MySettings.MySettings
                .AutoStartClient = chkClientStart.Checked
                .EOCNotify = chkEOCNotify.Checked
                Select Case cmbStart.Text
                    Case Is = "Registry"
                        .StartMethod = clsSettings.Settings.eStartMethod.Registry
                    Case Is = "Minimized - Registry"
                        .StartMethod = clsSettings.Settings.eStartMethod.RegistryMinimized
                End Select
                Select Case cmbEOCHistory.Text
                    Case Is = "Keep unlimited history"
                        .EocLimit = clsSettings.Settings.eEocLimit.None
                    Case Is = "Keep minimal history"
                        .EocLimit = clsSettings.Settings.eEocLimit.Minimal
                    Case Is = "Keep history from one day"
                        .EocLimit = clsSettings.Settings.eEocLimit.OneDay
                    Case Is = "Keep history from one week"
                        .EocLimit = clsSettings.Settings.eEocLimit.OneWeek
                    Case Is = "Keep history from one month"
                        .EocLimit = clsSettings.Settings.eEocLimit.OneMonth
                End Select
                .StartWithWindows = chkStart.Checked
                .StartMinimized = chkStartMin.Checked
                .URLsummary = txtSummary.Text
                .UseEOC = chkEOC.Checked
                .SafeMode = Not (chkSafe.Checked)
                .NonFatal.Clear()
                For Each strItem As String In lbNF.Items
                    'Get code for key enum
                    Dim xPos As Int16 = strItem.LastIndexOf("(")
                    Dim yPos As Int16 = strItem.LastIndexOf(")")
                    Dim vHex = Hex(CInt(Mid(strItem, xPos + 2, yPos - xPos - 1)))
                    .NonFatal.Add(vHex, vHex)
                Next
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DoGUIfromSettings()
        Try
            With MySettings.MySettings
                chkClientStart.Checked = .AutoStartClient
                chkEOCNotify.Checked = .EOCNotify
                Select Case .StartMethod
                    Case Is = clsSettings.Settings.eStartMethod.Registry
                        cmbStart.Text = "Registry"
                    Case Is = clsSettings.Settings.eStartMethod.RegistryMinimized
                        cmbStart.Text = "Minimized - Registry"
                End Select
                Select Case .EocLimit
                    Case Is = clsSettings.Settings.eEocLimit.None
                        cmbEOCHistory.Text = "Keep unlimited history"
                    Case Is = clsSettings.Settings.eEocLimit.Minimal
                        cmbEOCHistory.Text = "Keep minimal history"
                    Case Is = clsSettings.Settings.eEocLimit.OneDay
                        cmbEOCHistory.Text = "Keep history from one day"
                    Case Is = clsSettings.Settings.eEocLimit.OneWeek
                        cmbEOCHistory.Text = "Keep history from one week"
                    Case Is = clsSettings.Settings.eEocLimit.OneMonth
                        cmbEOCHistory.Text = "Keep history from one month"
                End Select
                chkStart.Checked = .StartWithWindows
                chkStartMin.Checked = .StartMinimized
                txtSummary.Text = .URLsummary
                Try
                    If ClientControl.MyClient.UserName = "" Or ClientControl.MyClient.UserName.ToUpper = "Anonymous".ToUpper Then
                        chkEOC.Checked = False
                        chkEOC.Enabled = False
                    Else
                        chkEOC.Checked = .UseEOC
                    End If
                Catch ex As Exception
                    chkEOC.Checked = False
                    chkEOC.Enabled = False
                End Try
              
                lbNF.Items.Clear()
                For Each strItem As String In .NonFatal
                    Dim nString As String = "Corestatus = " & strItem & " (" & Convert.ToInt64(strItem, 16) & ")"
                    lbNF.Items.Add(nString)
                Next
                gbEOC.Enabled = chkEOC.Checked
                cmbStart.Enabled = chkStart.Checked
                chkSafe.Checked = Not (MySettings.MySettings.SafeMode)
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAccept.Click
        Try
            DoSettingsFromGUI()
            MySettings.SaveSettings()
            SetStartupmethod()
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetStartupmethod()
        Try
            If chkStart.Checked Then
                Select Case MySettings.MySettings.StartMethod
                    Case Is = clsSettings.Settings.eStartMethod.Registry
                        Dim regKey As Microsoft.Win32.RegistryKey
                        regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                        regKey.SetValue(ClientControl.ClientPath & "\ftray.exe", ClientControl.ClientPath & "\ftray.exe")
                        regKey.Close()
                    Case Is = clsSettings.Settings.eStartMethod.RegistryMinimized
                        Dim regKey As Microsoft.Win32.RegistryKey
                        regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                        regKey.SetValue(ClientControl.ClientPath & "\ftray.exe", Chr(34) & ClientControl.ClientPath & "\ftray.exe" & Chr(34) & " minimized")
                        regKey.Close()
                End Select
            Else
                'check both 
                Try
                    Dim regKey As Microsoft.Win32.RegistryKey
                    regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
                    regKey.DeleteValue(ClientControl.ClientPath & "\ftray.exe")
                Catch ex As Exception : End Try

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub
    Private Sub cmdNFEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNFEdit.Click
        Try
            nADD = New frmAddNF
            'AddHandler nADD.NonFatal, AddressOf AddNewNF
            nADD.NFmode = frmAddNF.eNFmode.DoEdit
            nADD.Index = lbNF.SelectedIndex
            nADD.txtHex.Text = MySettings.MySettings.NonFatal(lbNF.SelectedIndex + 1)
            nADD.Show(Me)
            nADD.txtHex.Focus()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdNFAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNFAdd.Click
        Try
            nADD = New frmAddNF
            'AddHandler nADD.NonFatal, AddressOf AddNewNF
            nADD.NFmode = frmAddNF.eNFmode.AddNew
            nADD.txtHex.Text = ""
            nADD.Show(Me)
            nADD.txtHex.Focus()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub AddNewNF(ByVal NF As String) Handles nADD.NonFatal
        Try
            If nADD.NFmode = frmAddNF.eNFmode.AddNew Then
                If MySettings.MySettings.NonFatal.Contains(NF) Then
                    MsgBox("That core code is already described as being non fatal!")
                Else
                    MySettings.MySettings.NonFatal.Add(NF, NF)
                    DoGUIfromSettings()
                End If
            Else
                MySettings.MySettings.NonFatal.Remove(nADD.Index + 1)
                MySettings.MySettings.NonFatal.Add(NF)
                DoGUIfromSettings()
            End If
        Catch ex As Exception

        Finally
            nADD.Close()
        End Try
    End Sub
    Private Sub cmdNFRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNFRemove.Click
        Try
            If lbNF.SelectedIndex = -1 Then Exit Sub
            Dim rVal As MsgBoxResult = MsgBox("Remove " & lbNF.SelectedItem, vbOKCancel And vbApplicationModal And vbInformation, "Confirm removal")
            If rVal = MsgBoxResult.Cancel Then Exit Sub
            MySettings.MySettings.NonFatal.Remove(lbNF.SelectedIndex + 1)
            DoGUIfromSettings()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lnblSafe_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnblSafe.LinkClicked
        Try
            MsgBox("By checking this box, you are allowing the wrapper to configure clients while they are running. This is not considerd safe by everyone, and those not familiar with the settings might not get the effect they would expect ( as the settings will only be appleid after the current project is finished or if the client is restarted. By keeping this box unchecked, any write changes to the client will be done after the client is stopped.", MsgBoxStyle.OkOnly + vbInformation, "Safe mode")
        Catch ex As Exception

        End Try
    End Sub
End Class