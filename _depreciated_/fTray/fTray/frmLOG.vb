'   fTray Extended console class
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
Imports System.IO
Imports System.Diagnostics

Public Class frmLOG
#Region "Declares"
    Private WithEvents QueueEvent As clsQueue
    Private tMinInterval As Long
    Private WithEvents tInterval As New Timer
    Private WithEvents tCheckB As New ToolStripCheckBox
    Private fEOCXML As frmEOCXML

#End Region
#Region "Form events"
    Public Sub New()
        InitializeComponent()
    End Sub
    Private Sub nIcon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseClick
        Try
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If Me.Visible Then
                    Me.Hide()
                Else
                    If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Normal
                    Me.Show()
                    Application.DoEvents()
                    Me.Focus()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub nIcon_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseUp
        Try
            nIcon.ContextMenuStrip = Nothing
            If e.Button = MouseButtons.Right Then
                For Each oForm As Form In Application.OpenForms
                    If oForm.Text.Contains(" -configonly") Then
                        oForm.Activate()
                        Exit Sub
                    End If
                Next
                nIcon.ContextMenuStrip = fLog.cMenu
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cMenu_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cMenu.Opening
        Try
            If ClientControl.ConsoleStatus = clsClientControl.eStatus.stopped Then
                StopToolStripMenuItem.Enabled = False
                StartToolStripMenuItem.Enabled = True
            ElseIf ClientControl.ConsoleStatus = clsClientControl.eStatus.active Then
                StartToolStripMenuItem.Enabled = False
                StopToolStripMenuItem.Enabled = True
            End If
            If fLog.Visible Then
                ShowHideToolStripMenuItem.Text = "Hide"
            Else
                ShowHideToolStripMenuItem.Text = "Show"
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub StopToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopToolStripMenuItem.Click
        Try
            ClientControl.StopClient()
            'tIcon.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub StartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartToolStripMenuItem.Click
        Try
            ClientControl.StartClient()
            'tIcon.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub PauseToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ClientControl.PauseClient()
            'tIcon.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ShowHideToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowHideToolStripMenuItem.Click
        Try
            If Me.Visible Then
                Me.Hide()
            Else
                Me.Show()
                Me.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CloseFTrayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseFTrayToolStripMenuItem.Click
        Try
            LogWindow.WriteLog("fTray closed from systemtray")
            LogWindow.bAllowClose = True
            'make console visible always on exit
            If Not ClientControl.IsService And ClientControl.WindowState = clsClientControl.eWindowState.Hidden Then ClientControl.ShowConsole()
            nIcon.Visible = False
            Application.Exit()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ConfigureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureToolStripMenuItem.Click
        Try
            Me.Enabled = False
            If ClientControl.ConfigureClient Then nIcon.ContextMenu = Nothing
        Catch ex As Exception

        End Try
    End Sub
    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        Try
            AboutBox.Show()
            'frmOptions.Show()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub tCheckB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tCheckB.CheckedChanged
        Try
            Dim _eta2 As String = ClientControl.Queue.Eta2
            Dim _eta As DateTime = ClientControl.Queue.Eta
            If tCheckB.Checked Then
                If _eta2 = "0.00:00:00" Then
                    lblEta.Text = "-unknown-"
                    lblEta.ForeColor = Color.Red
                    Exit Sub
                Else
                    lblEta.Text = _eta2
                End If
            Else
                If _eta = #1/1/2000# Then
                    lblEta.Text = "-unknown-"
                    lblEta.ForeColor = Color.Red
                    Exit Sub
                Else
                    lblEta.Text = _eta
                End If
            End If
            If ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                If _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).FinalDeadline.Replace(".", ",")) Then
                    lblEta.ForeColor = Color.Red
                ElseIf _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) Then
                    lblEta.ForeColor = Color.Blue
                Else
                    lblEta.ForeColor = Control.DefaultForeColor
                End If
            Else
                lblEta.ForeColor = Control.DefaultForeColor
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmLOG_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If e.CloseReason = CloseReason.UserClosing Then
                e.Cancel = True
                Me.Visible = False
            Else
                If ClientControl.WindowState = clsClientControl.eWindowState.Hidden And ClientControl.IsService = False Then
                    ClientControl.ShowConsole()
                End If
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmLog_FormClosing", Err, "Sender=" & sender & vbNewLine & "e=" & e.CloseReason.ToString)
        End Try
    End Sub
    Private Sub frmLOG_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'check windowstate, if minimized then make normal and hide
            If Me.WindowState = FormWindowState.Minimized Then
                Me.WindowState = FormWindowState.Normal
                Me.Visible = False
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmLog_load", Err)
        End Try
    End Sub
    Private Sub cmbQslots_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbQslots.SelectedIndexChanged
        Try
            Dim qSlot As clsQueue.Entry = ClientControl.Queue.Slot(cmbQslots.SelectedIndex)
            With qSlot
                If .Status = "1" Then
                    lblStatus.Text = "Active"
                    lblStatus.ForeColor = Control.DefaultForeColor
                ElseIf .Status = "0" Then
                    If .UploadStatus = "1" Then
                        lblStatus.Text = "Finished"
                        lblStatus.ForeColor = Color.Blue
                    Else
                        lblStatus.Text = "No active work"
                        lblStatus.ForeColor = Color.Red
                    End If
                End If
                lblUser.Text = .UserName
                lblTeam.Text = .TeamNumber
                If .PassKey <> "" Then
                    lblPK.Text = "Yes"
                Else
                    lblPK.Text = "No"
                End If
                If .TimeData.BeginTime = #1/1/2000# Then
                    lblBT.Text = "-empty-"
                Else
                    lblBT.Text = .TimeData.BeginTime
                End If
                If cmbQslots.SelectedIndex = ClientControl.Queue.Current Then
                    'Active slot
                    If ClientControl.ConsoleStatus = clsClientControl.eStatus.active Then
                        If ClientControl.Queue.Eta = #1/1/2000# Then
                            'active no frame data
                            lblET.Text = "-no frame data-"
                            lblET.ForeColor = Color.Red
                        Else
                            If ProjectInfo.Projects.KnownProject(.Project.Project) Then
                                'active with projectinfo
                                'check if eta >= pref or eta >= final
                                If ClientControl.Queue.Eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).FinalDeadline.Replace(".", ",")) Then
                                    lblET.Text = ClientControl.Queue.Eta
                                    lblET.ForeColor = Color.Red
                                ElseIf ClientControl.Queue.Eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) Then
                                    lblET.Text = ClientControl.Queue.Eta
                                    lblET.ForeColor = Color.Blue
                                Else
                                    lblET.Text = ClientControl.Queue.Eta
                                    lblET.ForeColor = Control.DefaultForeColor
                                End If
                            Else
                                'active with no projectinfo
                                lblET.Text = ClientControl.Queue.Eta
                                lblET.ForeColor = Control.DefaultForeColor
                            End If
                        End If
                    Else
                        lblET.Text = "-stopped-"
                        lblET.ForeColor = Color.Red
                    End If
                Else
                    If .TimeData.EndTime = #1/1/2000# Then
                        lblET.Text = "-empty-"
                        lblET.ForeColor = Control.DefaultForeColor
                    Else
                        If ProjectInfo.Projects.KnownProject(.Project.Project) Then
                            'active with projectinfo
                            'check if eta >= pref or eta >= final
                            If ClientControl.Queue.Eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).FinalDeadline.Replace(".", ",")) Then
                                lblET.Text = .TimeData.EndTime
                                lblET.ForeColor = Color.Red
                            ElseIf ClientControl.Queue.Eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) Then
                                lblET.Text = .TimeData.EndTime
                                lblET.ForeColor = Color.Blue
                            Else
                                lblET.Text = .TimeData.EndTime
                                lblET.ForeColor = Control.DefaultForeColor
                            End If
                        Else
                            'active with no projectinfo
                            lblET.Text = .TimeData.EndTime
                            lblET.ForeColor = Control.DefaultForeColor
                        End If
                    End If
                    lblET.ForeColor = Control.DefaultForeColor
                End If
                'If timedate is filled, might be nice to add effective ppd based on total completion time to the upload field 
                'when the queueevents are working properly this will also be added to statistics
                If .UploadStatus = "1" Then
                    If ProjectInfo.Projects.KnownProject(qSlot.Project.Project) Then
                        lblUS.Text = .UploadStatus & " (Eff. ppd: " & ProjectInfo.GetEffectivePPD(qSlot.Issued, qSlot.TimeData.EndTime, qSlot.Project.Project) & " )"
                    Else
                        lblUS.Text = "1"
                    End If
                Else
                    lblUS.Text = .UploadStatus
                End If
                lblCore.Text = "FahCore_" & .CoreNumber
                lblUF.Text = .UploadFailures
                If .BenchMark = "0" Then
                    lblBident.Text = "Flops"
                    lblBench.Text = .Flops
                Else
                    lblBench.Text = .BenchMark
                    lblBident.Text = "Benchmark"
                End If
                lblIssued.Text = .Issued
                If ProjectInfo.Projects.KnownProject(.Project.Project) Then
                    lblPreff.Text = .Due.AddDays(ProjectInfo.Projects.Project(.Project.Project).PreferredDays.Replace(".", ","))
                    lblExpires.Text = .Due.AddDays(ProjectInfo.Projects.Project(.Project.Project).FinalDeadline.Replace(".", ","))
                Else
                    lblPreff.Text = "unknown"
                    lblExpires.Text = "unknown"
                End If
            End With
        Catch ex As Exception
            LogWindow.WriteError("cmbQslots_SelectedIndexChanged", Err)
        End Try
    End Sub
    Private Sub OpenLogfileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenLogfileToolStripMenuItem.Click
        Process.Start(ClientControl.ClientLocation & "\Fahlog.txt")
    End Sub
    Private Sub ViewClientFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewClientFilesToolStripMenuItem.Click
        Process.Start(ClientControl.ClientLocation)
    End Sub
    Private Sub ToolStripDropDownButton1_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsBottomDropDown.DropDownOpening
        Try
            If ClientControl.IsService Then
                tsBottomShowHide.Visible = False
                tsSepShowHide.Visible = False
            ElseIf ClientControl.ConsoleStatus = clsClientControl.eStatus.active Then
                tsSepShowHide.Visible = True
                If ClientControl.WindowState = clsClientControl.eWindowState.Visible Then
                    tsBottomShowHide.Text = "Hide console"
                Else
                    tsBottomShowHide.Text = "Show console"
                End If
                tsBottomShowHide.Visible = True
            Else
                tsBottomShowHide.Visible = False
                tsSepShowHide.Visible = False
            End If
            If LogWindow.IsWindowActive Then
                tsShowDebug.Text = "Hide debug messages"
            Else
                tsShowDebug.Text = "Show debug messages"
            End If
        Catch ex As Exception
            LogWindow.WriteError("ToolStripDropDownButton1_DropDownOpening", Err)
        End Try
    End Sub
    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
    Private Sub tsMenuItemPB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsMenuItemPB.Click
        Try
            Dim pbL As New frmPBList
            pbL.FillView()
            pbL.ShowDialog(Me)
        Catch ex As Exception
            LogWindow.WriteError("tsMenuItemPB_Click", Err)
        End Try
    End Sub
    Private Sub tsBottomAdvancedOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsBottomAdvancedOptions.Click
        Try
            Dim nOptions As New frmOptions
            nOptions.ShowDialog(Me)
        Catch ex As Exception
            LogWindow.WriteError("tsBottomAdvancedOptions_Click", Err)
        End Try
    End Sub
    Private Sub ProjectBrowserwebToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProjectBrowserwebToolStripMenuItem.Click
        Try
            Process.Start(MySettings.MySettings.URLsummary)
        Catch ex As Exception
            LogWindow.WriteError("ProjectBrowserwebToolStripMenuItem_Click", Err)
        End Try
    End Sub
    Private Sub ExitFTrayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitFTrayToolStripMenuItem.Click
        Try
            LogWindow.bAllowClose = True
            LogWindow.WriteLog("fTray closed from contextmenu")
            Application.Exit()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub tsBottomShowHide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsBottomShowHide.Click
        Try
            If tsBottomShowHide.Text = "Show console" Then
                ClientControl.ShowConsole()
            Else
                ClientControl.HideConsole()
            End If
        Catch ex As Exception
            LogWindow.WriteError("tsBottomShowHide_Click", Err)
        End Try
    End Sub
    Private Sub tsStanfordUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsStanfordUser.Click
        Try
            ''http://fah-web.stanford.edu/cgi-bin/main.py?qtype=userstats&submitopt=search&searchtype=st_exact&qname=mtm&qpasskey=fsdfsdfsdfasddf&qteam=
            Dim httpString As String = "http://fah-web.stanford.edu/cgi-bin/main.py?qtype=userstats&submitopt=search&searchtype=st_exact&qname=" & ClientControl.Queue.ActiveSlot.UserName & "&qpasskey=" & ClientControl.Queue.ActiveSlot.PassKey & "&qteam=" & ClientControl.Queue.ActiveSlot.TeamNumber
            Process.Start(httpString)
        Catch ex As Exception
            LogWindow.WriteError("tsStanfordUser_Click", Err)
        End Try
    End Sub
    Private Sub EOCSignatureImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EOCSignatureImage.Click
        Try
            ClientControl.EOC.ShowStatsForm(True)
        Catch ex As Exception
            LogWindow.WriteError("EOCSignatureImage_Click", Err)
        End Try
    End Sub
    Private Sub tsEocWebStatistics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsEocWebStatistics.Click
        Try
            'http://folding.extremeoverclocking.com/xml/user_summary.php?un=EOC_Jason&t=11314 
            If MySettings.MySettings.EOCid = "" Then
                'get ID from xml feed
                Dim xReader As Xml.XmlReader = Xml.XmlReader.Create("http://folding.extremeoverclocking.com/xml/user_summary.php?un=" & ClientControl.Queue.ActiveSlot.UserName & "&t=" & ClientControl.Queue.ActiveSlot.TeamNumber)
                xReader.ReadToFollowing("UserID")
                MySettings.MySettings.EOCid = CStr(xReader.ReadElementContentAsInt())
                MySettings.SaveSettings()
            End If
            If MySettings.MySettings.EOCid = "" Then
                MsgBox("Could not get EOCid used to go to the html page")
                Exit Sub
            End If
            Process.Start("http://folding.extremeoverclocking.com/user_summary.php?s=&u=" & MySettings.MySettings.EOCid)
        Catch ex As Exception
            LogWindow.WriteError("tsEocWebStatistics_Click", Err)
        End Try
    End Sub
    Private Sub tsBottomStatistics_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsBottomStatistics.DropDownOpening
        Try
            EOCSignatureImage.Visible = MySettings.MySettings.UseEOC
            tsEOCXMLStatistics.Visible = MySettings.MySettings.UseEOC
        Catch ex As Exception
            LogWindow.WriteError("tsBottomStatistics_DropDownOpening", Err)
        End Try
    End Sub
    Private Sub tsShowDebug_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsShowDebug.Click
        Try
            If LogWindow.IsWindowActive Then
                LogWindow.HideDebugWindow()
            Else
                LogWindow.ShowDebugWindow(clsLogwindow.TrayIcon.Log)
            End If
        Catch ex As Exception
            LogWindow.WriteError("tsShowDebug_click", Err)
        End Try
    End Sub
    Private Sub cmdStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        Try
            ClientControl.StopClient()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdStart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdStart.Click
        Try
            ClientControl.StartClient()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmdConfigure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConfigure.Click
        Try
            Me.Enabled = False
            If ClientControl.ConfigureClient Then
                nIcon.ContextMenu = Nothing
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmLog_cmdConfigure.click", Err)
        End Try
    End Sub
    Private Sub lblDescription_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblDescription.LinkClicked
        Try
            If Not lblDescription.Text = "overusingIPswillbebanned" Then
                Process.Start(lblDescription.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region
#Region "Init"
    Public Sub DoInit()
        Try
            LogWindow.WriteLog("frmLog init started")
            tClock.Enabled = False
            'init the queue before trying to read properties/values from the class
            If Not ClientControl.MyProject.IsEmpty Then ClientControl.Queue.InitQueue()
            'Add handlers for attaching to client process and client process exit event
            AddHandler ClientControl.ConsoleExited, AddressOf ClientProcExit
            AddHandler ClientControl.ConsoleAttached, AddressOf ClientProcAttached
            'Fist calls need queue.initqueue!
            If ClientControl.IsService = False And ClientControl.ConsoleStatus = clsClientControl.eStatus.active And ClientControl.WindowState = clsClientControl.eWindowState.Visible Then
                'hide the console window
                ClientControl.HideConsole()
            End If
            'Set window placement/content 
            rtf.Parent = tsCont.ContentPanel
            rtf.Dock = DockStyle.Fill
            Dim mLen As Int32 = Int32.MaxValue
            rtf.MaxLength = mLen
            tCheckB.Text = "Change ETA style"
            tsTop.Items.Add(tCheckB)
            tsTop.Items(tsTop.Items.Count - 1).Alignment = ToolStripItemAlignment.Right
            tsCont.TopToolStripPanel.Controls.Add(tsTop)
            tsTop.AutoSize = True
            tsCont.BottomToolStripPanel.Controls.Add(tsBottom)
            tsCont.BottomToolStripPanel.Show()
            tsCont.TopToolStripPanel.Show()
            Me.Text = ClientControl.MyClient.ClientEXE & " " & ClientControl.MyClient.ClientVersion
            'Do first updates     
            If Not ClientControl.MyProject.IsEmpty Then
                InvokeUpdate()
            Else
                InvokeStatus("No active project, start the client", Color.Blue)
            End If
            QueueEvent = ClientControl.Queue
            QueueEvent.StartWatching()
            tClock.Enabled = True
        Catch ex As Exception
            LogWindow.WriteError("frmLog_DoInit", Err)
        End Try
    End Sub
#End Region
#Region "Updating"
    Public Delegate Sub tTick()
    Public Delegate Sub DoUpdateP()
    Public Delegate Sub DoPrune()
    Public Delegate Sub DoLog()
    Public Delegate Sub DoStatus(ByVal Message As String, ByVal Color As Color)
    Private Sub tClock_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tClock.Tick
        Try
            'Fixed double calls
            Static _cStatus As clsClientControl.eStatus = clsClientControl.eStatus.failure
            Dim currentStatus As clsClientControl.eStatus = ClientControl.ConsoleStatus
            Debug.Print(DateTime.Now & ": Tclock.tick - _cStatus = " & _cStatus.ToString & " currentstatus = " & currentStatus.ToString)
            If _cStatus <> currentStatus Then
                _cStatus = currentStatus
                'do update icon for running state
                Select Case currentStatus
                    Case Is = clsClientControl.eStatus.stopped
                        UpdateIcon(eIconState.Stopped)
                    Case Is = clsClientControl.eStatus.active
                        tCheckB.Enabled = True
                        UpdateIcon(eIconState.Active)
                    Case Is = clsClientControl.eStatus.failure
                        UpdateIcon(eIconState.LogWarning)
                    Case Is = clsClientControl.eStatus.paused
                        UpdateIcon(eIconState.Paused)
                End Select
                InvokeUpdate() 'should be called from process attachement
            End If
            'Check if eta needs updating
            If tCheckB.Checked And _cStatus = clsClientControl.eStatus.active Then
                Dim doTick As New tTick(AddressOf UpdateTick)
                Me.Invoke(doTick)
            End If
        Catch ex As Exception
            LogWindow.WriteError("tClock_tick", Err)
        End Try
    End Sub
    Public Enum eIconState
        Active
        Paused
        Stopped
        LogWarning
        Log
        _skip
    End Enum
    Public Sub UpdateTick()
        Try
            Debug.Print(DateTime.Now & ": Updatetick called")
            Dim _eta2 As String = ClientControl.Queue.Eta2
            Dim _eta As DateTime = ClientControl.Queue.Eta
            If tCheckB.Checked Then
                If _eta2 = "0.00:00:00" Then
                    lblEta.Text = "-unknown-"
                    lblEta.ForeColor = Color.Red
                    Exit Sub
                Else
                    lblEta.Text = _eta2
                End If
            Else
                If _eta = #1/1/2000# Then
                    lblEta.Text = "-unknown-"
                    lblEta.ForeColor = Color.Red
                    Exit Sub
                Else
                    lblEta.Text = _eta
                End If
            End If
            If ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                If _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).FinalDeadline.Replace(".", ",")) Then
                    lblEta.ForeColor = Color.Red
                ElseIf _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) Then
                    lblEta.ForeColor = Color.Blue
                Else
                    lblEta.ForeColor = Control.DefaultForeColor
                End If
            Else
                lblEta.ForeColor = Control.DefaultForeColor
            End If
        Catch ex As Exception
            LogWindow.WriteError("UpdateTick", Err)
        End Try
    End Sub
    Public Function UpdateIcon(Optional ByVal IconState As eIconState = eIconState._skip)
        Try
            If IconState = eIconState._skip Then
                Select Case ClientControl.ConsoleStatus
                    Case Is = clsClientControl.eStatus.active
                        nIcon.Icon = My.Resources.fTray
                    Case Is = clsClientControl.eStatus.failure
                        nIcon.Icon = My.Resources.fTray_Warning
                        nIcon.Text = "Warning: "
                    Case Is = clsClientControl.eStatus.paused
                        nIcon.Icon = My.Resources.fTray_stop
                        nIcon.Text = "Stopped!"
                    Case Is = clsClientControl.eStatus.stopped
                        nIcon.Icon = My.Resources.fTray_Start
                        nIcon.Text = "Stopped"
                End Select
            Else
                Select Case IconState
                    Case Is = eIconState.Active
                        nIcon.Icon = My.Resources.fTray
                        'nIcon.Text = "Progress: " & ClientControl.Queue.Progress & "%" & vbNewLine & "Eta: " & ClientControl.Queue.Eta2
                    Case Is = eIconState.LogWarning
                        nIcon.Icon = My.Resources.fTray_Warning
                    Case Is = eIconState.Stopped
                        nIcon.Icon = My.Resources.fTray_Start
                End Select
            End If
            'nIcon.Icon = My.Resources.Resource1.fTray
            Return True
        Catch ex As Exception
            LogWindow.WriteError("UpdateIcon", Err)
            Return False
        Finally
            nIcon.Visible = True
        End Try
    End Function
    Public Sub InvokeUpdate()
        Try
            If Not Me.IsHandleCreated Then Exit Sub
            Dim Update As New DoUpdateP(AddressOf UpdateProgress)
            Me.Invoke(Update)
        Catch ex As Exception
            LogWindow.WriteError("InvokeUpdate", Err)
        End Try
    End Sub
    Public Shadows Sub UpdateProgress()
        Try
            Dim dStart As DateTime = DateTime.Now
            Debug.Print(DateTime.Now & ": Updateprogress called")
            'Check control states
            Dim _IsClientService As Boolean = ClientControl.IsService
            Dim _ClientSerStatus As ServiceProcess.ServiceControllerStatus
            If _IsClientService Then
                _ClientSerStatus = ClientControl.ServiceStatus
            Else
                'this is why I had custom status, need to change it back or accept this is messy.
                _ClientSerStatus = ServiceProcess.ServiceControllerStatus.ContinuePending
            End If
            Dim _ClientControlStatus As clsClientControl.eStatus = ClientControl.ConsoleStatus
            If _IsClientService Then
                If _ClientSerStatus = ServiceProcess.ServiceControllerStatus.Running Or _
                    _ClientSerStatus = ServiceProcess.ServiceControllerStatus.StartPending Then
                    cmdStop.Enabled = True
                    cmdStart.Enabled = False
                    cmdStop.ContextMenuStrip = Nothing
                ElseIf _ClientSerStatus = ServiceProcess.ServiceControllerStatus.Stopped Or _
                    _ClientSerStatus = ServiceProcess.ServiceControllerStatus.StopPending Then
                    cmdStop.Enabled = False
                    cmdStart.Enabled = True
                    cmdStop.ContextMenuStrip = Nothing
                End If
            Else
                If _ClientControlStatus = clsClientControl.eStatus.active Then
                    cmdStart.Enabled = False
                    cmdStop.Enabled = True
                    cmdStop.ContextMenuStrip = cmStop
                Else
                    cmdStop.Enabled = False
                    cmdStart.Enabled = True
                    cmdStop.ContextMenuStrip = Nothing
                End If
            End If
            If ClientControl.MyProject.IsEmpty Then Exit Sub
            'set new frame info
            If _ClientControlStatus = clsClientControl.eStatus.active Then
                'Update icon tooltip
                nIcon.Text = "Progress: " & ClientControl.Queue.Progress & "%" & vbNewLine & "Eta: " & ClientControl.Queue.Eta
                If ClientControl.Queue.ActiveSlot.Status = "1" Then
                    InvokeStatus("Client is running", Control.DefaultForeColor)
                Else
                    InvokeStatus("Client is running - CORE NOT ENGAGED", Color.Red)
                End If
                pBar.Value = ClientControl.Queue.Progress
                pBar.ToolTipText = pBar.Value.ToString
                If ClientControl.Queue.tsFrame.TotalSeconds = 0 Then
                    lblFrameTime.Text = "-no frame data-"
                    lblFrameTime.ForeColor = Color.Blue
                Else
                    lblFrameTime.Text = ClientControl.Queue.tsFrame.ToString
                    lblFrameTime.ForeColor = Control.DefaultForeColor
                End If
                lblProject.Text = ClientControl.Queue.ActiveSlot.PRCG
                Dim _eta2 As String = ClientControl.Queue.Eta2
                Dim _eta As DateTime = ClientControl.Queue.Eta
                If tCheckB.Checked Then
                    If _eta2 = "0.00:00:00" Then
                        lblEta.Text = "-unknown-"
                        lblEta.ForeColor = Color.Red
                    Else
                        lblEta.Text = _eta2
                    End If
                Else
                    If _eta = #1/1/2000# Then
                        lblEta.Text = "-unknown-"
                        lblEta.ForeColor = Color.Red
                    Else
                        lblEta.Text = _eta
                    End If
                End If
                If ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                    If _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).FinalDeadline.Replace(".", ",")) Then
                        lblEta.ForeColor = Color.Red
                    ElseIf _eta > ClientControl.Queue.ActiveSlot.Issued.AddDays(ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project).PreferredDays.Replace(".", ",")) Then
                        lblEta.ForeColor = Color.Blue
                    Else
                        lblEta.ForeColor = Control.DefaultForeColor
                    End If
                Else
                    lblEta.ForeColor = Control.DefaultForeColor
                End If
                If cmbQslots.SelectedIndex = ClientControl.Queue.Current Then
                    If _ClientControlStatus = clsClientControl.eStatus.active Then
                        If _eta = #1/1/2000# Then
                            lblET.Text = "-no frame data-"
                            lblET.ForeColor = Color.Blue
                        Else
                            lblET.Text = ClientControl.Queue.Eta
                            lblET.ForeColor = Color.Blue
                        End If
                    Else
                        lblET.Text = "-stopped-"
                        lblET.ForeColor = Control.DefaultForeColor
                    End If
                End If
            Else
                InvokeStatus("Client is not running", Control.DefaultForeColor)
                pBar.Value = 0
                pBar.ToolTipText = pBar.Value.ToString
                lblFrameTime.Text = "-stopped-"
                lblFrameTime.ForeColor = Color.Red
                lblProject.Text = ClientControl.Queue.ActiveSlot.PRCG
                lblEta.Text = "-stopped-"
                tCheckB.Enabled = False
                If cmbQslots.SelectedIndex = ClientControl.Queue.Current Then
                    lblET.Text = "-stopped-"
                    lblET.ForeColor = Color.Red
                End If
            End If
            'fill queue info
            With ClientControl.Queue
                lblQVersion.Text = .Version
                lblPerfFraction.Text = .PerformanceFraction & " (" & .PerformanceFractionUnits & ")"
                lblDR.Text = .DownloadRate & " (" & .DownloadRateUnits & ")"
                lblUR.Text = .UploadRate & " (" & .UploadRateUnits & ")"
                lblTUF.Text = .TotalUploadFailures.ToString
                'Check number of entries in cmbQueueslots
                If cmbQslots.Items.Count > 0 Then
                    'For changing queue content, use queueevents ( project end/ project start -> needs fixing! )
                    'quicker fix.. compare content and update if needed
                    Dim bQueddiff As Boolean = False
                    For xInt As Short = 0 To 9
                        Dim qSlot As clsQueue.Entry = .Slot(xInt)
                        If cmbQslots.Items(xInt).ToString <> xInt.ToString & " " & qSlot.PRCG Then
                            bQueddiff = True
                            Exit For
                        End If
                    Next
                    If bQueddiff Then
                        cmbQslots.Items.Clear()
                        GoTo DoAdd
                    End If
                Else
DoAdd:
                    'Add items from queue  
                    For xInt As Short = 0 To 9
                        Dim qSlot As clsQueue.Entry = .Slot(xInt)
                        cmbQslots.Items.Add(xInt.ToString & " " & qSlot.PRCG)
                    Next
                    cmbQslots.Text = cmbQslots.Items(.Current)
                    'Prune log / suggest project start/stop
                    InvokePrune()
                End If
            End With
            'fill wu info
            If ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                With ProjectInfo.Projects.Project(ClientControl.Queue.ActiveSlot.Project.Project)
                    lblWuName.Text = .WUName
                    If ClientControl.Queue.CoreVersion <> "" Then
                        lblWUCORE.Text = ClientControl.Queue.CoreVersion & " " & .Code
                    Else
                        lblWUCORE.Text = .Code
                    End If
                    lblWUmax.Text = .FinalDeadline
                    lblWUPref.Text = .PreferredDays
                    lblAtoms.Text = .NumberOfAtoms
                    lblCredit.Text = .Credit
                    lblDescription.Text = .Description 'mixed up
                    lblServer.Text = .ServerIP
                    lblKfactor.Text = .kFactor
                End With
            Else
                lblWUCORE.Text = "-"
                lblWUmax.Text = "-"
                lblWUPref.Text = "-"
                lblAtoms.Text = "-"
                lblCredit.Text = "-"
                lblDescription.Text = "-"
                lblServer.Text = "-"
                lblKfactor.Text = "-"
                lblWuName.Text = "-"
            End If
            'Put ppd in caption
            If _ClientControlStatus = clsClientControl.eStatus.active Then
                Me.Text = ClientControl.MyClient.ClientEXE & " " & ClientControl.MyClient.ClientVersion & " Eff. ppd: " & ClientControl.Queue.PPD_Effective & " Last TPF ppd:  " & ClientControl.Queue.PPD_LastFrame
            Else
                Me.Text = ClientControl.MyClient.ClientEXE & " " & ClientControl.MyClient.ClientVersion
            End If
            LogWindow.WriteLog ("Update finished in " & DateTime.Now.Subtract(dStart).TotalMilliseconds & "ms")
        Catch ex As Exception
            LogWindow.WriteError("UpdateProgress", Err)
        End Try
    End Sub
    Public Sub InvokePrune()
        Try
            If Not Me.IsHandleCreated Then Exit Sub
            Dim nPrune As New DoPrune(AddressOf PruneLog)
            Me.Invoke(nPrune)
        Catch ex As Exception
            LogWindow.WriteError("InvokePrune", Err)
        End Try
    End Sub
    Public Sub InvokeLog()
        Try
            If Not Me.IsHandleCreated Then Exit Sub
            Dim nLog As New DoLog(AddressOf UpdateLog)
            Me.Invoke(nLog)
        Catch ex As Exception
            LogWindow.WriteError("InvokeLog", Err)
        End Try
    End Sub
    Public Sub InvokeStatus(ByVal MessageString As String, ByVal MessageColor As Color)
        Try
            Dim InvokeStatus As New DoStatus(AddressOf UpdateStatus)
            InvokeStatus(MessageString, MessageColor)
        Catch ex As Exception
            LogWindow.WriteError("InvokeStatus", Err)
        End Try
    End Sub
    Private Shadows Sub UpdateLog()
        Try
            'update rtf log
            Dim lFile As String = ClientControl.ClientLocation & "\fahlog.txt"
            If Not My.Computer.FileSystem.FileExists(lFile) Then Exit Sub
            Dim fStream As FileStream = New FileStream(lFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim sRead As New StreamReader(fStream)
            If sRead.EndOfStream Then Exit Sub
            Dim strRead As String = ""
            strRead = sRead.ReadToEnd
            strRead = strRead.TrimEnd([vbNewLine], [vbCr], [vbCrLf])
            strRead = Mid(strRead, 1, strRead.Length - 2)
            fStream.Close()
            sRead = Nothing
            fStream = Nothing
            'Look for last " + Processing work unit"
            Dim xPos As Int32 = strRead.ToUpper.LastIndexOf(("+ Processing work unit").ToString.ToUpper)
            If xPos > 0 Then
                'cutt off log 
                strRead = Mid(strRead, xPos - 11)
            End If
            If rtf.TextLength = 0 Then
                rtf.Text = strRead
                strRead = Nothing
            Else
                'split strRead, compare from last line
                Dim nLines() As String = strRead.Split(vbLf)
                For xInt As Int32 = (nLines.Count - 1) To 0 Step -1
                    If rtf.Text.Contains(nLines(xInt)) Then
                        'Last known same line, add all new lines
                        For yInt As Int32 = xInt To (nLines.Count - 1)
                            rtf.Text &= nLines(yInt) & vbLf
                        Next
                        Exit For
                    End If
                Next
            End If
            rtf.Text = rtf.Text.TrimEnd(vbLf)
            rtf.SelectionStart = rtf.Text.Length
        Catch ex As Exception
            LogWindow.WriteError("UpdateLog", Err)
        End Try
    End Sub
    Private Shadows Sub PruneLog()
        Try
            Dim lFile As String = ClientControl.ClientLocation & "\fahlog.txt"
            If Not My.Computer.FileSystem.FileExists(lFile) Then Exit Sub
            Dim fStream As FileStream = New FileStream(lFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim sRead As New StreamReader(fStream)
            If sRead.EndOfStream Then Exit Sub
            Dim strRead As String = ""
            strRead = sRead.ReadToEnd
            strRead = strRead.TrimEnd([vbNewLine], [vbCr], [vbCrLf])
            strRead = Mid(strRead, 1, strRead.Length - 2)
            fStream.Close()
            sRead = Nothing
            fStream = Nothing
            'Prune log to last "+ Working ..."
            Dim xPos As Int32 = strRead.ToUpper.LastIndexOf(("+ Processing work unit").ToString.ToUpper)
            If xPos > 0 Then
                'cutt off log 
                strRead = Mid(strRead, xPos - 11)
            End If
            rtf.Clear()
            rtf.Text = strRead
            rtf.SelectionStart = rtf.Text.Length
        Catch ex As Exception
            LogWindow.WriteError("PruneLog", Err)
        End Try
    End Sub
    Private Shadows Sub UpdateStatus(ByVal Message As String, ByVal MessageColor As Color)
        Try
            tsStatus.Text = Message
            tsStatus.ForeColor = MessageColor
        Catch ex As Exception
            LogWindow.WriteError("UpdateStatus", Err)
        End Try
    End Sub
#Region "Process events and Queue events"
#Region "Process events"
    Private Sub ClientProcExit()
        Try
            LogWindow.WriteLog("ClientProcExit - InvokeUpdate")
            InvokeUpdate()
        Catch ex As Exception
            LogWindow.WriteError("ClientProcExit", Err)
        End Try
    End Sub
    Private Sub ClientProcAttached()
        Try
            If Not Me.Created Then
                'First attachment, show Updating
                Dim fstatus As New frmPBStatus
                With fstatus
                    .StartPosition = FormStartPosition.CenterScreen
                    .SetMessage("Updating client to database, please be patient...")
                    .SetPBMax(100)
                    .Show()
                    Application.DoEvents()
                    .SetPBValue(-2)
                    ClientControl.MyClient = fConfig.ReadCurrentSettings(ClientControl.ClientEXE)
                    Application.DoEvents()
                    If ClientControl.UpdateClientToDatabase(ClientControl.MyClient) Then
                        .Close()
                    Else
                        MsgBox("shit")
                        Application.Exit()
                    End If
                End With
            End If
            LogWindow.WriteLog("ClientProcAttached - InvokeUpdate/InvokeLog")
            LogWindow.WriteLog("Updating client database")
            UpdateStatus("Updating client settings", Color.Blue)
            ClientControl.MyClient = fConfig.ReadCurrentSettings(ClientControl.MyClient.ClientEXE)
            ClientControl.UpdateClientToDatabase(ClientControl.MyClient)
            UpdateStatus("Client settings updated", Control.DefaultForeColor)
            InvokeUpdate()
            InvokePrune()
        Catch ex As Exception
            LogWindow.WriteError("ClientProcAttached", Err)
        End Try
    End Sub
#End Region
#Region "QueueEvents"
    Private Sub QueueEvent_EUE(ByVal CoreStatus As clsQueue.clsCoreStatus) Handles QueueEvent.EUE
        Try
            LogWindow.WriteLog("QueueEvent_EUE - InvokeStatus/InvokeUpdate")
            InvokeStatus(DateTime.Now.ToShortTimeString & " An EUE has occured!", Color.Red)
            Dim nEUE As New frmEUE
            nEUE.NotifyEUE(CoreStatus)
            'ClientControl should handle writing EUE to EUE database

            InvokeUpdate()
        Catch ex As Exception
            LogWindow.WriteError("QueueEvent_EUE", Err)
        End Try
    End Sub
    Private Sub QueueEvent_FailedUpload(ByVal Slot As clsQueue.Entry) Handles QueueEvent.FailedUpload
        Try
            LogWindow.WriteLog("QueueEvent_FailedUpload - InvokeStatus/InvokeUpdate")
            InvokeUpdate()
            InvokeStatus(DateTime.Now.ToShortTimeString & " The client has failed to upload a completed work unit", Color.Red)
            'should invokestatus revert back after these mesages to a more current status info? It will get updated on a new frame ect so maybe not 100% needed
        Catch ex As Exception
            LogWindow.WriteError("QueueEvent_FailedUpload", Err)
        End Try
    End Sub
    Private Sub QueueEvent_NewFrame(ByVal CurrentProgress As Short, ByVal FrameTime As System.TimeSpan, ByVal dtFrameEnd As Date) Handles QueueEvent.NewFrame
        Try
            LogWindow.WriteLog("QueueEvent_NewFrame - InvokeUpdate/InvokeLog")
            'ClientControl should write the new frame to the db
            InvokeUpdate()
            InvokeLog()
        Catch ex As Exception
            LogWindow.WriteError("QueueEvent_NewFrame", Err)
        End Try
    End Sub
    Private Sub QueueEvent_ProjectEnd(ByVal Slot As clsQueue.Entry, ByVal FrameTime As System.TimeSpan, ByVal dtFrameBegin As Date, ByVal Percentage As Short) Handles QueueEvent.ProjectEnd
        Try
            LogWindow.WriteLog("QueueEvent_ProjectEnd - InvokeUpdate")
            'ClientControl should write this to the db
            InvokeUpdate()
        Catch ex As Exception
            LogWindow.WriteError("QueueEvent_ProjectEnd", Err)
        End Try
    End Sub
    Private Sub QueueEvent_ProjectStart(ByVal Slot As clsQueue.Entry) Handles QueueEvent.ProjectStart
        Try
            LogWindow.WriteLog("QueueEvent_ProjectStart")
            'ClientControl should write this to the db

            If Not ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                LogWindow.WriteLog("Unknown project being processed, downloading new definitions")
                InvokeStatus("Unknown project being processed, downloading new definitions", Color.Blue)
                If ProjectInfo.GetProjects() Then
                    If Not ProjectInfo.Projects.KnownProject(ClientControl.Queue.ActiveSlot.Project.Project) Then
                        LogWindow.WriteLog("Downloading new definitions - succes")
                        InvokeStatus("Project information downloaded succesfully", Control.DefaultForeColor)
                    Else
                        LogWindow.WriteLog("Downloading new definitions - succes")
                        InvokeStatus("Project not in summary, try update again at each 10% marker", Color.Blue)
                    End If
                Else
                    LogWindow.WriteLog("Definitions could not be downloaded")
                    InvokeStatus("Downloading new definitions failed", Color.Red)
                End If
            End If
            InvokeUpdate()
            InvokeLog()
        Catch ex As Exception
            LogWindow.WriteError("QueueEvent_ProjectStart", Err)
        End Try
    End Sub
#End Region
#End Region
#End Region
#Region "Toolstrip checkbox"
    'Based on msdn sample for adding controls to a toolstrip checkbox
    Public Class ToolStripCheckBox
        Inherits ToolStripControlHost
        Public Sub New()
            MyBase.New(New CheckBox)
            CheckBoxControl.BackColor = Color.Transparent
        End Sub
        Public Sub SetText(ByVal Text As String)
            CheckBoxControl.Text = Text
        End Sub
        Private ReadOnly Property CheckBoxControl() As CheckBox
            Get
                Return CType(Control, CheckBox)
            End Get
        End Property
        'Expose the CheckBoxControl's Checked Property
        Public Property Checked() As Boolean
            Get
                Return CheckBoxControl.Checked
            End Get
            Set(ByVal value As Boolean)
                CheckBoxControl.Checked = value
            End Set
        End Property
        'Subscribe and Unsubscribe the events you wish to expose
        Protected Overrides Sub OnSubscribeControlEvents(ByVal control As System.Windows.Forms.Control)
            'Connect the base events
            MyBase.OnSubscribeControlEvents(control)
            'Cast the control to a ChckBox control
            Dim checkBoxControl As CheckBox = CType(control, CheckBox)
            'Add any events you want to expose
            AddHandler checkBoxControl.CheckedChanged, AddressOf CheckedChangedHandler
        End Sub
        Protected Overrides Sub OnUnsubscribeControlEvents(ByVal control As System.Windows.Forms.Control)
            'Disconnect the base events
            MyBase.OnUnsubscribeControlEvents(control)
            'Cast the control to a CheckBox control
            Dim checkBoxControl As CheckBox = CType(control, CheckBox)
            'Remove any events you have exposed
            RemoveHandler checkBoxControl.CheckedChanged, AddressOf CheckedChangedHandler
        End Sub
        'Declare the event
        Public Event CheckedChanged As EventHandler
        'Raise the event
        Private Sub CheckedChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
            RaiseEvent CheckedChanged(Me, e)
        End Sub
    End Class
#End Region
    Private Sub frmLOG_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Static bOnce As Boolean = False
            If Not bOnce Then
                'Main icon showed, check EOC icon
                If MySettings.MySettings.UseEOC Then
                    ClientControl.EOC = New clsClientControl.clsEOCInfo
                    ClientControl.EOC.InitEOC()
                    ClientControl.EOC.ShowStatsForm(True)
                End If
                If ClientControl.ConsoleStatus <> clsClientControl.eStatus.active And MySettings.MySettings.AutoStartClient Then
                    LogWindow.WriteLog("Autostarting client")
                    ClientControl.StartClient()
                End If
                bOnce = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub StopAfterUnitCompletesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopAfterUnitCompletesToolStripMenuItem.Click
        Try
            If StopAfterUnitCompletesToolStripMenuItem.Text = "Stop after unit completes" Then
                If Not ClientControl.IsService Then
                    ClientControl.StopClient()
                    While Not ClientControl.ConsoleStatus = clsClientControl.eStatus.stopped
                        Application.DoEvents()
                    End While
                    ClientControl.StartClient("-oneunit")
                End If
            Else
                If Not ClientControl.IsService Then
                    ClientControl.StopClient()
                    While Not ClientControl.ConsoleStatus = clsClientControl.eStatus.stopped
                        Application.DoEvents()
                    End While
                    ClientControl.StartClient()
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub cmStop_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmStop.Opening
        Try
            If ClientControl.IsService Then
                e.Cancel = True
                Exit Sub
            ElseIf ClientControl.MyClient.ClientArguments.ToUpper.Contains("-ONEUNIT") Then
                cmStop.Items(1).Text = "Remove -oneunit"
            Else
                cmStop.Items(1).Text = "Stop after unit completes"
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub tsEOCXMLStatistics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsEOCXMLStatistics.Click
        Try
            If ReferenceEquals(fEOCXML, Nothing) Then
                fEOCXML = New frmEOCXML
                fEOCXML.Show()
            ElseIf fEOCXML.Created = False Then
                fEOCXML.Show()
            Else
                fEOCXML.Close()
            End If
        Catch ex As Exception
            fEOCXML = New frmEOCXML
            fEOCXML.Show()
        End Try
    End Sub
    Private Sub frmLOG_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        Try
            If Me.Visible Then
                Me.TopMost = True
                Me.TopMost = False
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class