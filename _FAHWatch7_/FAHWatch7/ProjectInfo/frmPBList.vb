'   FAHWatch7 Project browser form
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
'   GNU General Friend License for more details.
'
'   You should have received a copy of the GNU General Friend License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
Imports System.Globalization
Imports FAHWatch7.pSummary
Imports FAHWatch7.ProjectInfo
Friend Class frmPBList

#Region "Declarations"
    Private bAllowClose As Boolean = True
    Private SelectedProject As pSummary
    Private colProjects As New AutoCompleteStringCollection
    Private bManual As Boolean = False
#End Region

#Region "Form handling and functions"

    Private Sub frmPBList_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.WindowsShutDown Or e.CloseReason = CloseReason.ApplicationExitCall Then
            bAllowClose = True
            WriteLog("Application being closed by " & e.CloseReason.ToString)
        End If
        If Not bAllowClose Then
            e.Cancel = True
        Else
            NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
        End If
    End Sub

    Friend Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdToggleDescriptions_Click(sender As System.Object, e As System.EventArgs) Handles cmdToggleDescriptions.Click
        scTwo.Panel2Collapsed = Not scTwo.Panel2Collapsed
    End Sub

#Region "Clean status"

    Private Delegate Sub CleanDelegate()
    Private Sub dClean()
        Try
            tslblStatus.Text = ""
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub Clean()
        Try
            Dim nI As New CleanDelegate(AddressOf dClean)
            Dim result As IAsyncResult = StatusStrip1.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            StatusStrip1.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub tCleanStatus_Tick(sender As System.Object, e As System.EventArgs) Handles tCleanStatus.Tick
        Try
            tCleanStatus.Enabled = False
            Clean()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

#End Region

#End Region

#Region "lvProjects"

    Friend Function FillView() As Boolean
        Try
            tslblStatus.Text = "Filling list, please wait"
            Me.UseWaitCursor = True
            Me.Enabled = False
            lvProjects.Items.Clear()
            lvProjects.BeginUpdate()
            colProjects.Clear()
            Dim lProjects As List(Of pSummary) = FAHWatch7.ProjectInfo.pSummaryList
            Dim sPB As New ToolStripProgressBar
            sPB.Style = ProgressBarStyle.Continuous
            sPB.Maximum = lProjects.Count
            StatusStrip1.Items.Add(sPB)
            Dim iIndex As Int32 = 1
            For Each Project As pSummary In FAHWatch7.ProjectInfo.pSummaryList
                Dim nItem As New ListViewItem
                nItem.Text = Project.ProjectNumber
                nItem.SubItems.Add(Project.ServerIP)
                nItem.SubItems.Add(Project.WUName)
                nItem.SubItems.Add(Project.NumberOfAtoms)
                nItem.SubItems.Add(Project.PreferredDays)
                nItem.SubItems.Add(Project.FinalDeadline)
                nItem.SubItems.Add(Project.Credit)
                nItem.SubItems.Add(Project.Frames)
                nItem.SubItems.Add(Project.Code)
                nItem.SubItems.Add(Project.Contact)
                nItem.SubItems.Add(Project.kFactor)
                lvProjects.Items.Add(nItem)
                colProjects.Add(Project.ProjectNumber)
                sPB.Value = iIndex
                iIndex += 1
            Next
            sPB.Style = ProgressBarStyle.Marquee
            sPB.MarqueeAnimationSpeed = 100
            tslblStatus.Text = "Resizing columns"
            lvProjects.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            Dim iWidth(0 To lvProjects.Columns.Count - 1) As Int32
            For xInt As Int32 = 0 To lvProjects.Columns.Count - 1
                iWidth(xInt) = lvProjects.Columns(xInt).Width
            Next
            lvProjects.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            For xint As Int32 = 0 To lvProjects.Columns.Count - 1
                If lvProjects.Columns(xint).Width < iWidth(xint) Then lvProjects.Columns(xint).Width = iWidth(xint)
            Next
            lvProjects.EndUpdate()
            tslblStatus.Text = "Filling missing projects"
            lbMissingProjects.Items.Clear()
            lbMissingProjects.Items.AddRange(sqdata.UnknownProjects.ToArray)
            tslblStatus.Text = "Setting autocomplete source"
            txtFindProject.AutoCompleteCustomSource = colProjects
            gbProjects.Text = lvProjects.Items.Count & " known work units"
            tslblStatus.Text = "Updating calculator persistent settings"
            chkDelay.Checked = CBool(modMySettings.Settings("chkDelay"))
            If modMySettings.HasSetting("tsDelay") Then
                Dim sDelay As String = modMySettings.Settings("tsDelay")
                txtDelay.Text = sDelay.Substring(0, 2)
                txtDelay2.Text = sDelay.Substring(3, 2)
                txtDelay3.Text = sDelay.Substring(6, 2)
            End If
            tslblStatus.Text = "Filling pSummary information"
            lbpSummary.Items.Clear()
            lbpSummary.Items.Add("*" & modMySettings.DefaultSummary)
            For Each url As String In modMySettings.AlternateUrls.Urls
                lbpSummary.Items.Add(url)
            Next
            StatusStrip1.Items.Remove(sPB)
            tslblStatus.Text = "Finished"
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            Me.Enabled = True
            Me.UseWaitCursor = False
            tCleanStatus.Interval = 15000
            tCleanStatus.Enabled = True
        End Try
    End Function

    Private Sub lvProjects_ItemSelectionChanged(sender As Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvProjects.ItemSelectionChanged
        Try
            txtFindProject.Text = "" : txtPPD.Text = "" : ResetTPFCalc()
            If Not e.IsSelected Then
                SelectedProject = Nothing
                cmdEditProject.Text = "Edit project"
                cmdDeleteProject.Text = "Remove project"
                cmdEditProject.Enabled = False
                cmdDeleteProject.Enabled = False
            Else
                SelectedProject = FAHWatch7.ProjectInfo.Project(e.Item.Text)
                cmdEditProject.Text = "Edit " & e.Item.Text
                cmdDeleteProject.Text = "Remove " & e.Item.Text
                cmdEditProject.Enabled = True
                cmdDeleteProject.Enabled = True
                If SelectedProject.Description.ToString.ToUpper(CultureInfo.InvariantCulture).Contains("HTTP://") Then
                    txtDescription.Text = "This project has not been run yet, projectinfo is only known for projects which you have run locally, there is no code included to parse descriptions from the cgi pages only to read the descriptions from FAHControl.db." & Environment.NewLine & Environment.NewLine & "However, the following link will allow you to read the description at your own discretion, do take note of the recommodation hidden in the URL when doing so." & Environment.NewLine & SelectedProject.Description
                Else
                    txtDescription.Text = SelectedProject.Description
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub lvProjects_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvProjects.MouseDoubleClick
        Try
            Dim lItem As ListViewItem = lvProjects.GetItemAt(e.X, e.Y)
            Try
                If lItem.Text = "" Then Exit Sub
            Catch ex As Exception : End Try
            If Not FAHWatch7.ProjectInfo.KnownProject(lItem.Text) Then Exit Sub
            Dim eProject As pSummary = FAHWatch7.ProjectInfo.Project(lItem.Text)
            Dim fEdit As New frmEditProject
            fEdit.Text = "Edit project - " & eProject.ProjectNumber
            fEdit.Project = eProject
            AddHandler fEdit.FormClosed, AddressOf EditClosed
            fEdit.ShowDialog(Me)
            fEdit.Dispose()
        Catch ex As Exception
            WriteError("frmPBList_ViewProjectDescription", Err)
        End Try
    End Sub

#End Region

#Region "gbProjects"

    Private Sub EditClosed(sender As Object, e As System.EventArgs)
        FillView()
    End Sub

    Private Sub cmdEditProject_Click(sender As System.Object, e As System.EventArgs) Handles cmdEditProject.Click
        Try
            Dim fEdit As New frmEditProject
            fEdit.Text = "Edit project - " & SelectedProject.ProjectNumber
            fEdit.Project = SelectedProject
            AddHandler fEdit.FormClosed, AddressOf EditClosed
            fEdit.ShowDialog(Me)
            fEdit.Dispose()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdDeleteProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteProject.Click
        Try
            If lvProjects.SelectedItems.Count = 0 Then Exit Sub
            For Each sItem As ListViewItem In lvProjects.SelectedItems
                Dim nProject As pSummary = FAHWatch7.ProjectInfo.Project(sItem.Text)
                Dim rVal As MsgBoxResult = MsgBox("Confirm removal of project - " & nProject.ProjectNumber, MsgBoxStyle.OkCancel, "Remove")
                If Not rVal = MsgBoxResult.Cancel Then
                    If Not FAHWatch7.ProjectInfo.RemoveProject(nProject.ProjectNumber) Then
                        MsgBox("Removal of project failed for some reason")
                    End If
                End If
            Next
            FillView()
        Catch ex As Exception
            WriteError("frmPBList_ViewProjectDescription", Err)
        End Try
    End Sub

    Private Sub cmdAddProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddProject.Click
        Dim fAdd As New frmAddProject
        Try
            AddHandler fAdd.FormClosed, AddressOf EditClosed
            fAdd.ShowDialog(Me)
        Catch ex As Exception
            WriteError("frmPBList_ViewProjectDescription", Err)
        Finally
            fAdd.Dispose()
        End Try
    End Sub

    Private Sub txtFindProject_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFindProject.KeyUp
        'Ideally posisble input should be limited to the autocomplete source?
        Try
            If txtFindProject.TextLength = 0 Then Exit Sub
            If e.KeyCode <> Keys.Enter Then Exit Sub
            For Each lItem As ListViewItem In lvProjects.Items
                If lItem.Text = txtFindProject.Text Then
                    lItem.Selected = True
                    lvProjects.Select()
                    lItem.EnsureVisible()
                    Exit For
                End If
                Application.DoEvents()
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdImport_Click(sender As System.Object, e As System.EventArgs) Handles cmdImport.Click
        Try
            cmdImport.Enabled = False
            For Each client As Client In Clients.Clients
                sqdata.GetFAHControlProjectDescriptions(client.ClientConfig.Configuration.DataLocation)
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            cmdImport.Enabled = True
        End Try
    End Sub

#End Region

#Region "gbCalculator"

    Private Function CalcPPF_TPF() As String
        Try
            If bManual Then Return ""
            If txtTpf.Text = "" Or txtTpf2.Text = "" Or txtTpf3.Text = "" Then Return ""
            Dim tsFrame As New TimeSpan(CInt(txtTpf.Text), CInt(txtTpf2.Text), CInt(txtTpf3.Text))
            Dim tsCompletion As TimeSpan = TimeSpan.FromTicks((tsFrame.Ticks * 100))
            If txtDelay.Text <> "hh" And txtDelay2.Text <> "mm" And txtDelay3.Text <> "ss" Then
                Dim tsDelay As New TimeSpan(CInt(txtDelay.Text), CInt(txtDelay2.Text), CInt(txtDelay3.Text))
                tsCompletion = tsCompletion.Add(tsDelay)
            End If
            Dim sPPD As sProjectPPD = FAHWatch7.ProjectInfo.GetEffectivePPD_sqrt(DateTime.Now, DateTime.Now.Add(tsCompletion), SelectedProject.ProjectNumber)
            Return sPPD.PPD
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return "*"
        End Try
    End Function

    Private Sub ResetTPFCalc() Handles cmdResetTPF.Click
        bManual = True
        If Not chkDelay.Checked Then
            txtDelay.Text = "hh"
            txtDelay2.Text = "mm"
            txtDelay3.Text = "ss"
        End If
        txtTpf.Text = "hh"
        txtTpf2.Text = "mm"
        txtTpf3.Text = "ss"
        txtPPD.Text = ""
        bManual = False
    End Sub

    Private Sub txtTPF_TextChanged(sender As Object, e As System.EventArgs) Handles txtTpf.TextChanged, txtTpf2.TextChanged, txtTpf3.TextChanged
        Try
            If bManual Then Exit Sub
            If ReferenceEquals(sender, txtTpf) Then
                If Not txtTpf.Text = "hh" And txtTpf.TextLength = 2 Then txtTpf2.Focus()
            ElseIf ReferenceEquals(sender, txtTpf2) Then
                If Not txtTpf.Text = "hh" And txtTpf.TextLength = 2 Or Not txtTpf2.Text = "mm" And txtTpf2.TextLength = 2 Then txtTpf3.Focus()
            ElseIf ReferenceEquals(sender, txtTpf3) Then
                If Not txtTpf3.Text = "ss" And txtTpf3.TextLength = 2 Then
                    txtPPD.Text = CalcPPF_TPF()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub txtTPF_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTpf.KeyPress, txtTpf2.KeyPress, txtTpf3.KeyPress
        Try
            If lvProjects.SelectedItems.Count = 0 Then
                ResetTPFCalc()
                tslblStatus.Text = "No project selected.."
                Application.DoEvents()
                If Not tCleanStatus.Enabled Then
                    tCleanStatus.Enabled = True
                Else
                    tCleanStatus.Interval = 15000
                End If
                e.Handled = True
                Exit Sub
            Else
                If tCleanStatus.Enabled Then tCleanStatus.Enabled = False
                tslblStatus.Text = ""
            End If
            If ReferenceEquals(sender, txtTpf) Then
                If txtTpf.Text = "hh" Then txtTpf.Text = ""
            ElseIf ReferenceEquals(sender, txtTpf2) Then
                If txtTpf2.Text = "mm" Then txtTpf2.Text = ""
            ElseIf ReferenceEquals(sender, txtTpf3) Then
                If txtTpf3.Text = "ss" Then txtTpf3.Text = ""
            End If
            If e.KeyChar = ChrW(Keys.Delete) Or e.KeyChar = ChrW(Keys.Back) Then
                ResetTPFCalc()
                txtTpf.Focus()
                e.Handled = True
            ElseIf Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub txtDelay_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDelay.TextChanged, txtDelay2.TextChanged, txtDelay3.TextChanged
        Try
            If ReferenceEquals(sender, txtDelay) Then
                If Not txtDelay.Text = "hh" And txtDelay.TextLength = 2 Then txtDelay2.Focus()
            ElseIf ReferenceEquals(sender, txtDelay2) Then
                If (Not txtDelay.Text = "hh" And txtDelay.TextLength = 2) And (Not txtDelay2.Text = "mm" And txtDelay2.TextLength = 2) Then txtDelay3.Focus()
            ElseIf ReferenceEquals(sender, txtDelay3) Then
                If Not txtDelay.Text = "hh" And txtDelay.TextLength = 2 Or Not txtDelay2.Text = "mm" And txtDelay2.TextLength = 2 Or Not txtDelay3.Text = "ss" And txtDelay3.TextLength = 2 Then
                    txtPPD.Text = CalcPPF_TPF()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub txtDelay_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDelay.KeyPress, txtDelay2.KeyPress, txtDelay3.KeyPress
        If lvProjects.SelectedItems.Count = 0 Then
            ResetTPFCalc()
            txtTpf.Focus()
            e.Handled = True
            Exit Sub
        End If
        If e.KeyChar = ChrW(Keys.Delete) Or e.KeyChar = ChrW(Keys.Back) Then
            e.Handled = True
            txtDelay.Text = ""
        ElseIf Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtDelay_Enter(sender As System.Object, e As System.EventArgs) Handles txtDelay.Enter, txtDelay2.Enter, txtDelay3.Enter
        If bManual Then Exit Sub
        Try
            If ReferenceEquals(sender, txtDelay) Then
                If txtDelay.Text = "hh" Then
                    txtDelay.Text = ""
                End If
            ElseIf ReferenceEquals(sender, txtDelay2) Then
                If txtDelay.Text = "hh" Or txtDelay.Text.Length <> 2 Then
                    txtDelay.Focus()
                Else
                    If txtDelay2.Text = "mm" Then
                        txtDelay2.Text = ""
                    End If
                End If
            ElseIf ReferenceEquals(sender, txtDelay3) Then
                If txtDelay.Text = "hh" Or txtDelay.Text.Length <> 2 Then
                    txtDelay.Focus()
                Else
                    If txtDelay2.Text = "mm" Or txtDelay2.Text.Length <> 2 Then
                        txtDelay2.Focus()
                    Else
                        If txtDelay3.Text = "ss" Then
                            txtDelay3.Text = ""
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try

    End Sub

    Private Sub txtTpf_Enter(sender As System.Object, e As System.EventArgs) Handles txtTpf.Enter, txtTpf2.Enter, txtTpf3.Enter
        If bManual Then Exit Sub
        Try
            If ReferenceEquals(sender, txtTpf) Then
                If txtTpf.Text = "hh" Then
                    txtTpf.Text = ""
                End If
            ElseIf ReferenceEquals(sender, txtTpf2) Then
                If txtTpf.Text = "hh" Or txtTpf.Text.Length <> 2 Then
                    txtTpf.Focus()
                Else
                    If txtTpf2.Text = "mm" Then
                        txtTpf2.Text = ""
                    End If
                End If
            ElseIf ReferenceEquals(sender, txtTpf3) Then
                If txtTpf.Text = "hh" Or txtTpf.Text.Length <> 2 Then
                    txtTpf.Focus()
                Else
                    If txtTpf2.Text = "mm" Or txtTpf2.Text.Length <> 2 Then
                        txtTpf2.Focus()
                    Else
                        If txtTpf3.Text = "ss" Then
                            txtTpf3.Text = ""
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub chkDelay_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkDelay.CheckedChanged
        If Not modMySettings.HasSetting("chkDelay") Then
            modMySettings.AddSetting("chkDelay", chkDelay.Checked.ToString)
        Else
            modMySettings.ChangeSetting("chkDelay", chkDelay.Checked.ToString)
        End If
        If chkDelay.Checked Then
            If Not modMySettings.HasSetting("tsDelay") Then
                modMySettings.AddSetting("tsDelay", txtDelay.Text & ":" & txtDelay2.Text & ":" & txtDelay3.Text)
            Else
                modMySettings.ChangeSetting("tsDelay", txtDelay.Text & ":" & txtDelay2.Text & ":" & txtDelay3.Text)
            End If
        End If
        txtDelay.Enabled = Not chkDelay.Checked
        txtDelay2.Enabled = Not chkDelay.Checked
        txtDelay3.Enabled = Not chkDelay.Checked
    End Sub

    Private Sub cmdPpdGraph_Click(sender As System.Object, e As System.EventArgs) Handles cmdPpdGraph.Click
        Try


        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

#End Region

#Region "gbSummary"

    Private Sub cmdUpdateSummary_Click(sender As System.Object, e As System.EventArgs) Handles cmdUpdateSummary.Click
        Try
            If lbpSummary.SelectedIndex = -1 Then
                tslblStatus.Text = "You need to select an url"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            End If
            If ProjectInfo.GetProjects(lbpSummary.SelectedItem.ToString, True) Then
                FillView()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdURLOpenBrowser_Click(sender As System.Object, e As System.EventArgs) Handles cmdURLOpenBrowser.Click
        Try
            If lbpSummary.SelectedIndex = -1 Then
                tslblStatus.Text = "You need to select an url"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            End If
            Process.Start(lbpSummary.SelectedItem.ToString.Replace("*", ""))
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdRemoveURL_Click(sender As System.Object, e As System.EventArgs) Handles cmdRemoveURL.Click
        Try
            If lbpSummary.SelectedIndex = -1 Then
                tslblStatus.Text = "You need to select an url"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            ElseIf lbpSummary.SelectedItem.ToString.Contains("*") Then
                tslblStatus.Text = "You can not delete your default psummary"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            End If
            modMySettings.AlternateUrls.RemoveUrl(lbpSummary.SelectedItem.ToString)
            bManual = True
            lbpSummary.Items.Remove(lbpSummary.SelectedItem)
            bManual = False
            lbpSummary.SelectedIndex = 0
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdSetDefaultSummary_Click(sender As System.Object, e As System.EventArgs) Handles cmdSetDefaultSummary.Click
        Try
            If lbpSummary.SelectedIndex = -1 Then
                tslblStatus.Text = "You need to select an url"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            ElseIf lbpSummary.SelectedItem.ToString.Contains("*") Then
                tslblStatus.Text = "This already is the default psummary url"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                Exit Sub
            End If
            modMySettings.DefaultSummary = lbpSummary.SelectedItem.ToString
            For Each url As String In lbpSummary.Items
                If url.Contains("*") Then
                    lbpSummary.Items(lbpSummary.Items.IndexOf(url)) = url.Replace("*", "")
                    Exit For
                End If
            Next
            lbpSummary.SelectedItem = "*" & lbpSummary.SelectedItem.ToString
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdNewUrl_Click(sender As System.Object, e As System.EventArgs) Handles cmdNewUrl.Click
        Try
            If txtSummaryURL.Text = "" Then
                tslblStatus.Text = "There is no url entered"
                tCleanStatus.Interval = 15000
                tCleanStatus.Enabled = True
                txtSummaryURL.Focus()
                Exit Sub
            End If
            If ProjectInfo.GetProjects(txtSummaryURL.Text, True) Then
                modMySettings.AlternateUrls.AddUrl(txtSummaryURL.Text)
                modMySettings.SaveSettings()
                If MsgBox("Do you want to set this url as default?", CType(MsgBoxStyle.YesNo Or MsgBoxStyle.Question, MsgBoxStyle)) = MsgBoxResult.Yes Then
                    modMySettings.AlternateUrls.RemoveUrl(txtSummaryURL.Text)
                    modMySettings.AlternateUrls.AddUrl(modMySettings.DefaultSummary)
                    modMySettings.DefaultSummary = txtSummaryURL.Text
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

#End Region

    Private Sub txtDescription_LinkClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles txtDescription.LinkClicked
        Try
            Process.Start(e.LinkText)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

End Class

