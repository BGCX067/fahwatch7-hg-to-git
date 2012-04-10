<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLive
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLive))
        Me.lvLive = New System.Windows.Forms.ListView()
        Me.chClient = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chSlot = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chHW = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chProgress = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chCredit = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chPpd = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chTpf = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chPRCG = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.chEta = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.tParse = New System.Windows.Forms.Timer(Me.components)
        Me.tFill = New System.Windows.Forms.Timer(Me.components)
        Me.scLive = New System.Windows.Forms.SplitContainer()
        Me.tlpLive = New System.Windows.Forms.TableLayoutPanel()
        Me.sStripLive = New System.Windows.Forms.StatusStrip()
        Me.tsLblLive = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdViewGraph = New System.Windows.Forms.Button()
        Me.scLiveGraph = New System.Windows.Forms.SplitContainer()
        Me.scZgFramesSelection = New System.Windows.Forms.SplitContainer()
        Me.scListboxUpdate = New System.Windows.Forms.SplitContainer()
        Me.chklbWU = New System.Windows.Forms.CheckedListBox()
        Me.tlpWUSelection = New System.Windows.Forms.TableLayoutPanel()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdPrev = New System.Windows.Forms.Button()
        Me.cmdUpdateFrames = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.cmdSelectAll = New System.Windows.Forms.Button()
        Me.tcFramesLogQueue = New System.Windows.Forms.TabControl()
        Me.tpFramesGraph = New System.Windows.Forms.TabPage()
        Me.zgFrames = New ZedGraph.ZedGraphControl()
        Me.tpLog = New System.Windows.Forms.TabPage()
        Me.tlpLog = New System.Windows.Forms.TableLayoutPanel()
        Me.tsLog = New System.Windows.Forms.ToolStrip()
        Me.tsIncludeOtherWUs = New System.Windows.Forms.ToolStripButton()
        Me.tsWarningsAndErrors = New System.Windows.Forms.ToolStripButton()
        Me.tsFollowLog = New System.Windows.Forms.ToolStripButton()
        Me.tsCombinedLog = New System.Windows.Forms.ToolStripButton()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.tpQueue = New System.Windows.Forms.TabPage()
        Me.scQueueLogMain = New System.Windows.Forms.SplitContainer()
        Me.scQueue = New System.Windows.Forms.SplitContainer()
        Me.lvQueue = New System.Windows.Forms.ListView()
        Me.chClientQueue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chSlotQueue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPrcgQueue = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chFinalDeadline = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLastAttempt = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chCreditRemaining = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPPDRemaining = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdToggleQueueLog = New System.Windows.Forms.Button()
        Me.txtLogQueue = New System.Windows.Forms.TextBox()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.sStripEOC = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sStripEoc2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sStripStatistics = New System.Windows.Forms.StatusStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsStatistics = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sStripMessage = New System.Windows.Forms.StatusStrip()
        Me.tsStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.scHardware = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.cmdHWSensors = New System.Windows.Forms.Button()
        Me.msLive = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GeneralToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotificationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GraphOptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SelectStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OverallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CurrentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripSeparator()
        Me.WhatsTheDifferenceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EOCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenSiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.TeamStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserStatisticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewUserStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewTeamStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripSeparator()
        Me.SignatureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LicenseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.LogMessagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FrameGraphsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QueuedWusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.HardwareSensorsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToggleETAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ProjectInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectInfoListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectInfoGraphCalculatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DoUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.MemtestG80ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MemtestCLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StressCPUV2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.HowDoIEnableTheseToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.DiagnosticsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tEta = New System.Windows.Forms.Timer(Me.components)
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.scLive.Panel1.SuspendLayout()
        Me.scLive.Panel2.SuspendLayout()
        Me.scLive.SuspendLayout()
        Me.tlpLive.SuspendLayout()
        Me.sStripLive.SuspendLayout()
        Me.scLiveGraph.Panel1.SuspendLayout()
        Me.scLiveGraph.Panel2.SuspendLayout()
        Me.scLiveGraph.SuspendLayout()
        Me.scZgFramesSelection.Panel1.SuspendLayout()
        Me.scZgFramesSelection.Panel2.SuspendLayout()
        Me.scZgFramesSelection.SuspendLayout()
        Me.scListboxUpdate.Panel1.SuspendLayout()
        Me.scListboxUpdate.Panel2.SuspendLayout()
        Me.scListboxUpdate.SuspendLayout()
        Me.tlpWUSelection.SuspendLayout()
        Me.tcFramesLogQueue.SuspendLayout()
        Me.tpFramesGraph.SuspendLayout()
        Me.tpLog.SuspendLayout()
        Me.tlpLog.SuspendLayout()
        Me.tsLog.SuspendLayout()
        Me.tpQueue.SuspendLayout()
        Me.scQueueLogMain.Panel1.SuspendLayout()
        Me.scQueueLogMain.Panel2.SuspendLayout()
        Me.scQueueLogMain.SuspendLayout()
        Me.scQueue.Panel1.SuspendLayout()
        Me.scQueue.Panel2.SuspendLayout()
        Me.scQueue.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.sStripEOC.SuspendLayout()
        Me.sStripEoc2.SuspendLayout()
        Me.sStripStatistics.SuspendLayout()
        Me.sStripMessage.SuspendLayout()
        Me.scHardware.Panel1.SuspendLayout()
        Me.scHardware.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.msLive.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvLive
        '
        Me.lvLive.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chClient, Me.chSlot, Me.chHW, Me.chProgress, Me.chCredit, Me.chPpd, Me.chTpf, Me.chPRCG, Me.chEta})
        Me.lvLive.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvLive.FullRowSelect = True
        Me.lvLive.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvLive.HideSelection = False
        Me.lvLive.Location = New System.Drawing.Point(3, 3)
        Me.lvLive.MultiSelect = False
        Me.lvLive.Name = "lvLive"
        Me.lvLive.ShowGroups = False
        Me.lvLive.ShowItemToolTips = True
        Me.lvLive.Size = New System.Drawing.Size(1073, 257)
        Me.lvLive.TabIndex = 0
        Me.lvLive.UseCompatibleStateImageBehavior = False
        Me.lvLive.View = System.Windows.Forms.View.Details
        '
        'chClient
        '
        Me.chClient.Text = "Client"
        '
        'chSlot
        '
        Me.chSlot.Text = "Slot"
        Me.chSlot.Width = 33
        '
        'chHW
        '
        Me.chHW.Text = "Slot type"
        '
        'chProgress
        '
        Me.chProgress.Text = "Progress"
        Me.chProgress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chProgress.Width = 58
        '
        'chCredit
        '
        Me.chCredit.Text = "Credit"
        Me.chCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chCredit.Width = 61
        '
        'chPpd
        '
        Me.chPpd.Text = "Ppd"
        Me.chPpd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chPpd.Width = 68
        '
        'chTpf
        '
        Me.chTpf.Text = "TPF"
        Me.chTpf.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.chTpf.Width = 50
        '
        'chPRCG
        '
        Me.chPRCG.Text = "PRCG"
        Me.chPRCG.Width = 180
        '
        'chEta
        '
        Me.chEta.Text = "Eta"
        Me.chEta.Width = 108
        '
        'tParse
        '
        Me.tParse.Interval = 180000
        '
        'tFill
        '
        Me.tFill.Interval = 500
        '
        'scLive
        '
        Me.scLive.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scLive.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scLive.IsSplitterFixed = True
        Me.scLive.Location = New System.Drawing.Point(0, 0)
        Me.scLive.Name = "scLive"
        Me.scLive.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLive.Panel1
        '
        Me.scLive.Panel1.Controls.Add(Me.tlpLive)
        '
        'scLive.Panel2
        '
        Me.scLive.Panel2.Controls.Add(Me.cmdViewGraph)
        Me.scLive.Panel2MinSize = 10
        Me.scLive.Size = New System.Drawing.Size(1079, 300)
        Me.scLive.SplitterDistance = 285
        Me.scLive.TabIndex = 2
        '
        'tlpLive
        '
        Me.tlpLive.ColumnCount = 1
        Me.tlpLive.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpLive.Controls.Add(Me.lvLive, 0, 0)
        Me.tlpLive.Controls.Add(Me.sStripLive, 0, 1)
        Me.tlpLive.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpLive.Location = New System.Drawing.Point(0, 0)
        Me.tlpLive.Name = "tlpLive"
        Me.tlpLive.RowCount = 2
        Me.tlpLive.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpLive.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.tlpLive.Size = New System.Drawing.Size(1079, 285)
        Me.tlpLive.TabIndex = 1
        '
        'sStripLive
        '
        Me.sStripLive.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsLblLive})
        Me.sStripLive.Location = New System.Drawing.Point(0, 263)
        Me.sStripLive.Name = "sStripLive"
        Me.sStripLive.Size = New System.Drawing.Size(1079, 22)
        Me.sStripLive.SizingGrip = False
        Me.sStripLive.TabIndex = 1
        Me.sStripLive.Text = "StatusStrip1"
        '
        'tsLblLive
        '
        Me.tsLblLive.Name = "tsLblLive"
        Me.tsLblLive.Size = New System.Drawing.Size(0, 17)
        '
        'cmdViewGraph
        '
        Me.cmdViewGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdViewGraph.Location = New System.Drawing.Point(0, 0)
        Me.cmdViewGraph.Name = "cmdViewGraph"
        Me.cmdViewGraph.Size = New System.Drawing.Size(1079, 11)
        Me.cmdViewGraph.TabIndex = 0
        Me.tTip.SetToolTip(Me.cmdViewGraph, "Toggle details")
        Me.cmdViewGraph.UseVisualStyleBackColor = True
        '
        'scLiveGraph
        '
        Me.scLiveGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scLiveGraph.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scLiveGraph.Location = New System.Drawing.Point(0, 0)
        Me.scLiveGraph.Name = "scLiveGraph"
        Me.scLiveGraph.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scLiveGraph.Panel1
        '
        Me.scLiveGraph.Panel1.Controls.Add(Me.scLive)
        Me.scLiveGraph.Panel1MinSize = 160
        '
        'scLiveGraph.Panel2
        '
        Me.scLiveGraph.Panel2.Controls.Add(Me.scZgFramesSelection)
        Me.scLiveGraph.Size = New System.Drawing.Size(1079, 591)
        Me.scLiveGraph.SplitterDistance = 300
        Me.scLiveGraph.TabIndex = 3
        '
        'scZgFramesSelection
        '
        Me.scZgFramesSelection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scZgFramesSelection.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scZgFramesSelection.IsSplitterFixed = True
        Me.scZgFramesSelection.Location = New System.Drawing.Point(0, 0)
        Me.scZgFramesSelection.Name = "scZgFramesSelection"
        '
        'scZgFramesSelection.Panel1
        '
        Me.scZgFramesSelection.Panel1.Controls.Add(Me.scListboxUpdate)
        '
        'scZgFramesSelection.Panel2
        '
        Me.scZgFramesSelection.Panel2.Controls.Add(Me.tcFramesLogQueue)
        Me.scZgFramesSelection.Size = New System.Drawing.Size(1079, 287)
        Me.scZgFramesSelection.SplitterDistance = 148
        Me.scZgFramesSelection.SplitterWidth = 1
        Me.scZgFramesSelection.TabIndex = 1
        '
        'scListboxUpdate
        '
        Me.scListboxUpdate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scListboxUpdate.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scListboxUpdate.IsSplitterFixed = True
        Me.scListboxUpdate.Location = New System.Drawing.Point(0, 0)
        Me.scListboxUpdate.Name = "scListboxUpdate"
        Me.scListboxUpdate.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scListboxUpdate.Panel1
        '
        Me.scListboxUpdate.Panel1.Controls.Add(Me.chklbWU)
        Me.scListboxUpdate.Panel1MinSize = 120
        '
        'scListboxUpdate.Panel2
        '
        Me.scListboxUpdate.Panel2.Controls.Add(Me.tlpWUSelection)
        Me.scListboxUpdate.Panel2MinSize = 120
        Me.scListboxUpdate.Size = New System.Drawing.Size(148, 287)
        Me.scListboxUpdate.SplitterDistance = 166
        Me.scListboxUpdate.SplitterWidth = 1
        Me.scListboxUpdate.TabIndex = 0
        '
        'chklbWU
        '
        Me.chklbWU.CheckOnClick = True
        Me.chklbWU.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chklbWU.FormattingEnabled = True
        Me.chklbWU.Location = New System.Drawing.Point(0, 0)
        Me.chklbWU.Name = "chklbWU"
        Me.chklbWU.Size = New System.Drawing.Size(148, 166)
        Me.chklbWU.TabIndex = 0
        Me.tTip.SetToolTip(Me.chklbWU, "A list of selectable slots for the line graph")
        '
        'tlpWUSelection
        '
        Me.tlpWUSelection.ColumnCount = 2
        Me.tlpWUSelection.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.31446!))
        Me.tlpWUSelection.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.68554!))
        Me.tlpWUSelection.Controls.Add(Me.cmdNext, 1, 0)
        Me.tlpWUSelection.Controls.Add(Me.cmdPrev, 0, 0)
        Me.tlpWUSelection.Controls.Add(Me.cmdUpdateFrames, 0, 1)
        Me.tlpWUSelection.Controls.Add(Me.cmdClear, 0, 2)
        Me.tlpWUSelection.Controls.Add(Me.cmdSelectAll, 0, 3)
        Me.tlpWUSelection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpWUSelection.Location = New System.Drawing.Point(0, 0)
        Me.tlpWUSelection.Name = "tlpWUSelection"
        Me.tlpWUSelection.RowCount = 4
        Me.tlpWUSelection.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpWUSelection.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpWUSelection.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpWUSelection.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpWUSelection.Size = New System.Drawing.Size(148, 120)
        Me.tlpWUSelection.TabIndex = 0
        '
        'cmdNext
        '
        Me.cmdNext.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdNext.Location = New System.Drawing.Point(77, 3)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(68, 24)
        Me.cmdNext.TabIndex = 1
        Me.cmdNext.Text = ">>"
        Me.tTip.SetToolTip(Me.cmdNext, "Next slot")
        Me.cmdNext.UseVisualStyleBackColor = True
        '
        'cmdPrev
        '
        Me.cmdPrev.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdPrev.Location = New System.Drawing.Point(3, 3)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(68, 24)
        Me.cmdPrev.TabIndex = 0
        Me.cmdPrev.Text = "<<"
        Me.tTip.SetToolTip(Me.cmdPrev, "Previous slot")
        Me.cmdPrev.UseVisualStyleBackColor = True
        '
        'cmdUpdateFrames
        '
        Me.tlpWUSelection.SetColumnSpan(Me.cmdUpdateFrames, 2)
        Me.cmdUpdateFrames.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdUpdateFrames.Location = New System.Drawing.Point(3, 33)
        Me.cmdUpdateFrames.Name = "cmdUpdateFrames"
        Me.cmdUpdateFrames.Size = New System.Drawing.Size(142, 24)
        Me.cmdUpdateFrames.TabIndex = 7
        Me.cmdUpdateFrames.Text = "Update"
        Me.tTip.SetToolTip(Me.cmdUpdateFrames, "Update the frame graph")
        Me.cmdUpdateFrames.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.tlpWUSelection.SetColumnSpan(Me.cmdClear, 2)
        Me.cmdClear.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdClear.Location = New System.Drawing.Point(3, 63)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(142, 24)
        Me.cmdClear.TabIndex = 4
        Me.cmdClear.Text = "Clear all"
        Me.tTip.SetToolTip(Me.cmdClear, "Clear all checkboxes")
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'cmdSelectAll
        '
        Me.tlpWUSelection.SetColumnSpan(Me.cmdSelectAll, 2)
        Me.cmdSelectAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdSelectAll.Location = New System.Drawing.Point(3, 93)
        Me.cmdSelectAll.Name = "cmdSelectAll"
        Me.cmdSelectAll.Size = New System.Drawing.Size(142, 24)
        Me.cmdSelectAll.TabIndex = 8
        Me.cmdSelectAll.Text = "Select all"
        Me.tTip.SetToolTip(Me.cmdSelectAll, "Select all slots")
        Me.cmdSelectAll.UseVisualStyleBackColor = True
        '
        'tcFramesLogQueue
        '
        Me.tcFramesLogQueue.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.tcFramesLogQueue.Controls.Add(Me.tpFramesGraph)
        Me.tcFramesLogQueue.Controls.Add(Me.tpLog)
        Me.tcFramesLogQueue.Controls.Add(Me.tpQueue)
        Me.tcFramesLogQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcFramesLogQueue.Location = New System.Drawing.Point(0, 0)
        Me.tcFramesLogQueue.Name = "tcFramesLogQueue"
        Me.tcFramesLogQueue.SelectedIndex = 0
        Me.tcFramesLogQueue.ShowToolTips = True
        Me.tcFramesLogQueue.Size = New System.Drawing.Size(930, 287)
        Me.tcFramesLogQueue.TabIndex = 1
        '
        'tpFramesGraph
        '
        Me.tpFramesGraph.Controls.Add(Me.zgFrames)
        Me.tpFramesGraph.Location = New System.Drawing.Point(4, 4)
        Me.tpFramesGraph.Name = "tpFramesGraph"
        Me.tpFramesGraph.Padding = New System.Windows.Forms.Padding(3)
        Me.tpFramesGraph.Size = New System.Drawing.Size(922, 261)
        Me.tpFramesGraph.TabIndex = 0
        Me.tpFramesGraph.Text = "Frames graph"
        Me.tpFramesGraph.ToolTipText = "View TPF as line graph"
        Me.tpFramesGraph.UseVisualStyleBackColor = True
        '
        'zgFrames
        '
        Me.zgFrames.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgFrames.IsShowPointValues = True
        Me.zgFrames.Location = New System.Drawing.Point(3, 3)
        Me.zgFrames.Name = "zgFrames"
        Me.zgFrames.ScrollGrace = 0.0R
        Me.zgFrames.ScrollMaxX = 0.0R
        Me.zgFrames.ScrollMaxY = 0.0R
        Me.zgFrames.ScrollMaxY2 = 0.0R
        Me.zgFrames.ScrollMinX = 0.0R
        Me.zgFrames.ScrollMinY = 0.0R
        Me.zgFrames.ScrollMinY2 = 0.0R
        Me.zgFrames.Size = New System.Drawing.Size(916, 255)
        Me.zgFrames.TabIndex = 0
        '
        'tpLog
        '
        Me.tpLog.Controls.Add(Me.tlpLog)
        Me.tpLog.Location = New System.Drawing.Point(4, 4)
        Me.tpLog.Name = "tpLog"
        Me.tpLog.Padding = New System.Windows.Forms.Padding(3)
        Me.tpLog.Size = New System.Drawing.Size(922, 261)
        Me.tpLog.TabIndex = 1
        Me.tpLog.Text = "Log"
        Me.tpLog.ToolTipText = "View the log"
        Me.tpLog.UseVisualStyleBackColor = True
        '
        'tlpLog
        '
        Me.tlpLog.ColumnCount = 1
        Me.tlpLog.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpLog.Controls.Add(Me.tsLog, 0, 0)
        Me.tlpLog.Controls.Add(Me.txtLog, 0, 1)
        Me.tlpLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpLog.Location = New System.Drawing.Point(3, 3)
        Me.tlpLog.Name = "tlpLog"
        Me.tlpLog.RowCount = 2
        Me.tlpLog.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpLog.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpLog.Size = New System.Drawing.Size(916, 255)
        Me.tlpLog.TabIndex = 1
        '
        'tsLog
        '
        Me.tsLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsLog.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsIncludeOtherWUs, Me.tsWarningsAndErrors, Me.tsFollowLog, Me.tsCombinedLog})
        Me.tsLog.Location = New System.Drawing.Point(0, 0)
        Me.tsLog.Name = "tsLog"
        Me.tsLog.Size = New System.Drawing.Size(916, 20)
        Me.tsLog.Stretch = True
        Me.tsLog.TabIndex = 1
        Me.tsLog.Text = "tsLog"
        Me.tTip.SetToolTip(Me.tsLog, "Scroll the log on each update")
        '
        'tsIncludeOtherWUs
        '
        Me.tsIncludeOtherWUs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsIncludeOtherWUs.AutoToolTip = False
        Me.tsIncludeOtherWUs.CheckOnClick = True
        Me.tsIncludeOtherWUs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsIncludeOtherWUs.Image = CType(resources.GetObject("tsIncludeOtherWUs.Image"), System.Drawing.Image)
        Me.tsIncludeOtherWUs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsIncludeOtherWUs.Name = "tsIncludeOtherWUs"
        Me.tsIncludeOtherWUs.Size = New System.Drawing.Size(239, 17)
        Me.tsIncludeOtherWUs.Text = "Include messages from previous work units"
        Me.tsIncludeOtherWUs.ToolTipText = "View messages from previous work units for this slot"
        '
        'tsWarningsAndErrors
        '
        Me.tsWarningsAndErrors.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsWarningsAndErrors.AutoToolTip = False
        Me.tsWarningsAndErrors.CheckOnClick = True
        Me.tsWarningsAndErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsWarningsAndErrors.Image = CType(resources.GetObject("tsWarningsAndErrors.Image"), System.Drawing.Image)
        Me.tsWarningsAndErrors.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsWarningsAndErrors.Name = "tsWarningsAndErrors"
        Me.tsWarningsAndErrors.Size = New System.Drawing.Size(117, 17)
        Me.tsWarningsAndErrors.Text = "Warnings and Errors"
        Me.tsWarningsAndErrors.ToolTipText = "View warning and errors for this work unit"
        '
        'tsFollowLog
        '
        Me.tsFollowLog.Checked = True
        Me.tsFollowLog.CheckOnClick = True
        Me.tsFollowLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsFollowLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsFollowLog.Image = CType(resources.GetObject("tsFollowLog.Image"), System.Drawing.Image)
        Me.tsFollowLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsFollowLog.Name = "tsFollowLog"
        Me.tsFollowLog.Size = New System.Drawing.Size(46, 17)
        Me.tsFollowLog.Text = "Follow"
        '
        'tsCombinedLog
        '
        Me.tsCombinedLog.CheckOnClick = True
        Me.tsCombinedLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsCombinedLog.Image = CType(resources.GetObject("tsCombinedLog.Image"), System.Drawing.Image)
        Me.tsCombinedLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsCombinedLog.Name = "tsCombinedLog"
        Me.tsCombinedLog.Size = New System.Drawing.Size(174, 17)
        Me.tsCombinedLog.Text = "View combined log from client"
        '
        'txtLog
        '
        Me.txtLog.BackColor = System.Drawing.SystemColors.Info
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.Location = New System.Drawing.Point(3, 23)
        Me.txtLog.MaxLength = 2147483647
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(910, 229)
        Me.txtLog.TabIndex = 2
        '
        'tpQueue
        '
        Me.tpQueue.Controls.Add(Me.scQueueLogMain)
        Me.tpQueue.Location = New System.Drawing.Point(4, 4)
        Me.tpQueue.Name = "tpQueue"
        Me.tpQueue.Padding = New System.Windows.Forms.Padding(3)
        Me.tpQueue.Size = New System.Drawing.Size(922, 261)
        Me.tpQueue.TabIndex = 2
        Me.tpQueue.Text = "Queued work units"
        Me.tpQueue.ToolTipText = "View queued work units"
        Me.tpQueue.UseVisualStyleBackColor = True
        '
        'scQueueLogMain
        '
        Me.scQueueLogMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scQueueLogMain.Location = New System.Drawing.Point(3, 3)
        Me.scQueueLogMain.Name = "scQueueLogMain"
        Me.scQueueLogMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scQueueLogMain.Panel1
        '
        Me.scQueueLogMain.Panel1.Controls.Add(Me.scQueue)
        '
        'scQueueLogMain.Panel2
        '
        Me.scQueueLogMain.Panel2.Controls.Add(Me.txtLogQueue)
        Me.scQueueLogMain.Size = New System.Drawing.Size(916, 255)
        Me.scQueueLogMain.SplitterDistance = 149
        Me.scQueueLogMain.TabIndex = 2
        '
        'scQueue
        '
        Me.scQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scQueue.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scQueue.IsSplitterFixed = True
        Me.scQueue.Location = New System.Drawing.Point(0, 0)
        Me.scQueue.Name = "scQueue"
        Me.scQueue.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scQueue.Panel1
        '
        Me.scQueue.Panel1.Controls.Add(Me.lvQueue)
        '
        'scQueue.Panel2
        '
        Me.scQueue.Panel2.Controls.Add(Me.cmdToggleQueueLog)
        Me.scQueue.Panel2MinSize = 10
        Me.scQueue.Size = New System.Drawing.Size(916, 149)
        Me.scQueue.SplitterDistance = 138
        Me.scQueue.SplitterWidth = 1
        Me.scQueue.TabIndex = 0
        '
        'lvQueue
        '
        Me.lvQueue.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chClientQueue, Me.chSlotQueue, Me.chPrcgQueue, Me.chFinalDeadline, Me.chLastAttempt, Me.chCreditRemaining, Me.chPPDRemaining})
        Me.lvQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvQueue.FullRowSelect = True
        Me.lvQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvQueue.Location = New System.Drawing.Point(0, 0)
        Me.lvQueue.MultiSelect = False
        Me.lvQueue.Name = "lvQueue"
        Me.lvQueue.Size = New System.Drawing.Size(916, 138)
        Me.lvQueue.TabIndex = 1
        Me.lvQueue.UseCompatibleStateImageBehavior = False
        Me.lvQueue.View = System.Windows.Forms.View.Details
        '
        'chClientQueue
        '
        Me.chClientQueue.Text = "Client"
        Me.chClientQueue.Width = 180
        '
        'chSlotQueue
        '
        Me.chSlotQueue.Text = "Slot"
        Me.chSlotQueue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chSlotQueue.Width = 40
        '
        'chPrcgQueue
        '
        Me.chPrcgQueue.Text = "PRCG"
        Me.chPrcgQueue.Width = 180
        '
        'chFinalDeadline
        '
        Me.chFinalDeadline.Text = "Final deadline"
        Me.chFinalDeadline.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chFinalDeadline.Width = 180
        '
        'chLastAttempt
        '
        Me.chLastAttempt.Text = "Last attempt"
        Me.chLastAttempt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chLastAttempt.Width = 160
        '
        'chCreditRemaining
        '
        Me.chCreditRemaining.Text = "Credit"
        Me.chCreditRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chPPDRemaining
        '
        Me.chPPDRemaining.Text = "PPD"
        Me.chPPDRemaining.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdToggleQueueLog
        '
        Me.cmdToggleQueueLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdToggleQueueLog.Location = New System.Drawing.Point(0, 0)
        Me.cmdToggleQueueLog.Name = "cmdToggleQueueLog"
        Me.cmdToggleQueueLog.Size = New System.Drawing.Size(916, 10)
        Me.cmdToggleQueueLog.TabIndex = 0
        Me.tTip.SetToolTip(Me.cmdToggleQueueLog, "Toggle queued work unit log")
        Me.cmdToggleQueueLog.UseVisualStyleBackColor = True
        '
        'txtLogQueue
        '
        Me.txtLogQueue.BackColor = System.Drawing.SystemColors.Info
        Me.txtLogQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLogQueue.Location = New System.Drawing.Point(0, 0)
        Me.txtLogQueue.Multiline = True
        Me.txtLogQueue.Name = "txtLogQueue"
        Me.txtLogQueue.ReadOnly = True
        Me.txtLogQueue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLogQueue.Size = New System.Drawing.Size(916, 102)
        Me.txtLogQueue.TabIndex = 0
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.sStripEOC)
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.sStripEoc2)
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.sStripStatistics)
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.sStripMessage)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.scHardware)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1093, 591)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1093, 703)
        Me.ToolStripContainer1.TabIndex = 4
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.msLive)
        '
        'sStripEOC
        '
        Me.sStripEOC.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripEOC.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2})
        Me.sStripEOC.Location = New System.Drawing.Point(0, 0)
        Me.sStripEOC.Name = "sStripEOC"
        Me.sStripEOC.Size = New System.Drawing.Size(1093, 22)
        Me.sStripEOC.TabIndex = 5
        Me.sStripEOC.Text = "Eoc"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(49, 17)
        Me.ToolStripStatusLabel2.Text = "EocUser"
        '
        'sStripEoc2
        '
        Me.sStripEoc2.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripEoc2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel3})
        Me.sStripEoc2.Location = New System.Drawing.Point(0, 22)
        Me.sStripEoc2.Name = "sStripEoc2"
        Me.sStripEoc2.Size = New System.Drawing.Size(1093, 22)
        Me.sStripEoc2.TabIndex = 6
        Me.sStripEoc2.Text = "Eoc2"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(56, 17)
        Me.ToolStripStatusLabel3.Text = "EocTeam"
        '
        'sStripStatistics
        '
        Me.sStripStatistics.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripStatistics.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus, Me.tsStatistics})
        Me.sStripStatistics.Location = New System.Drawing.Point(0, 44)
        Me.sStripStatistics.Name = "sStripStatistics"
        Me.sStripStatistics.Size = New System.Drawing.Size(1093, 22)
        Me.sStripStatistics.TabIndex = 4
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(0, 17)
        '
        'tsStatistics
        '
        Me.tsStatistics.Name = "tsStatistics"
        Me.tsStatistics.Size = New System.Drawing.Size(62, 17)
        Me.tsStatistics.Text = "tsStatistics"
        '
        'sStripMessage
        '
        Me.sStripMessage.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatusLabel})
        Me.sStripMessage.Location = New System.Drawing.Point(0, 66)
        Me.sStripMessage.Name = "sStripMessage"
        Me.sStripMessage.Size = New System.Drawing.Size(1093, 22)
        Me.sStripMessage.SizingGrip = False
        Me.sStripMessage.TabIndex = 2
        '
        'tsStatusLabel
        '
        Me.tsStatusLabel.Name = "tsStatusLabel"
        Me.tsStatusLabel.Size = New System.Drawing.Size(0, 17)
        '
        'scHardware
        '
        Me.scHardware.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scHardware.Location = New System.Drawing.Point(0, 0)
        Me.scHardware.Name = "scHardware"
        '
        'scHardware.Panel1
        '
        Me.scHardware.Panel1.Controls.Add(Me.SplitContainer2)
        Me.scHardware.Panel2Collapsed = True
        Me.scHardware.Size = New System.Drawing.Size(1093, 591)
        Me.scHardware.SplitterDistance = 840
        Me.scHardware.TabIndex = 4
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.scLiveGraph)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.cmdHWSensors)
        Me.SplitContainer2.Panel2MinSize = 10
        Me.SplitContainer2.Size = New System.Drawing.Size(1093, 591)
        Me.SplitContainer2.SplitterDistance = 1079
        Me.SplitContainer2.TabIndex = 0
        '
        'cmdHWSensors
        '
        Me.cmdHWSensors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdHWSensors.Location = New System.Drawing.Point(0, 0)
        Me.cmdHWSensors.Name = "cmdHWSensors"
        Me.cmdHWSensors.Size = New System.Drawing.Size(10, 591)
        Me.cmdHWSensors.TabIndex = 0
        Me.tTip.SetToolTip(Me.cmdHWSensors, "Toggle hardware sensors")
        Me.cmdHWSensors.UseVisualStyleBackColor = True
        '
        'msLive
        '
        Me.msLive.Dock = System.Windows.Forms.DockStyle.None
        Me.msLive.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.EOCToolStripMenuItem, Me.HelpToolStripMenuItem, Me.ViewToolStripMenuItem, Me.DoUpdateToolStripMenuItem, Me.tsTools})
        Me.msLive.Location = New System.Drawing.Point(0, 0)
        Me.msLive.Name = "msLive"
        Me.msLive.Size = New System.Drawing.Size(1093, 24)
        Me.msLive.TabIndex = 0
        Me.msLive.Text = "msLive"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MinimizeToolStripMenuItem, Me.HistoryToolStripMenuItem, Me.ToolStripMenuItem3, Me.CloseToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'MinimizeToolStripMenuItem
        '
        Me.MinimizeToolStripMenuItem.Name = "MinimizeToolStripMenuItem"
        Me.MinimizeToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.MinimizeToolStripMenuItem.Text = "Minimize"
        '
        'HistoryToolStripMenuItem
        '
        Me.HistoryToolStripMenuItem.Name = "HistoryToolStripMenuItem"
        Me.HistoryToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.HistoryToolStripMenuItem.Text = "History"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(120, 6)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneralToolStripMenuItem, Me.NotificationsToolStripMenuItem, Me.GraphOptionsToolStripMenuItem, Me.ToolStripSeparator1, Me.SelectStatisticsToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'GeneralToolStripMenuItem
        '
        Me.GeneralToolStripMenuItem.Name = "GeneralToolStripMenuItem"
        Me.GeneralToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.GeneralToolStripMenuItem.Text = "General"
        '
        'NotificationsToolStripMenuItem
        '
        Me.NotificationsToolStripMenuItem.Name = "NotificationsToolStripMenuItem"
        Me.NotificationsToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.NotificationsToolStripMenuItem.Text = "Advanced"
        '
        'GraphOptionsToolStripMenuItem
        '
        Me.GraphOptionsToolStripMenuItem.Name = "GraphOptionsToolStripMenuItem"
        Me.GraphOptionsToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.GraphOptionsToolStripMenuItem.Text = "Graph options"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(150, 6)
        '
        'SelectStatisticsToolStripMenuItem
        '
        Me.SelectStatisticsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OverallToolStripMenuItem, Me.CurrentToolStripMenuItem, Me.ToolStripMenuItem9, Me.WhatsTheDifferenceToolStripMenuItem})
        Me.SelectStatisticsToolStripMenuItem.Name = "SelectStatisticsToolStripMenuItem"
        Me.SelectStatisticsToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.SelectStatisticsToolStripMenuItem.Text = "Select statistics"
        '
        'OverallToolStripMenuItem
        '
        Me.OverallToolStripMenuItem.CheckOnClick = True
        Me.OverallToolStripMenuItem.Name = "OverallToolStripMenuItem"
        Me.OverallToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.OverallToolStripMenuItem.Text = "Overall"
        '
        'CurrentToolStripMenuItem
        '
        Me.CurrentToolStripMenuItem.CheckOnClick = True
        Me.CurrentToolStripMenuItem.Name = "CurrentToolStripMenuItem"
        Me.CurrentToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.CurrentToolStripMenuItem.Text = "Current"
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(188, 6)
        '
        'WhatsTheDifferenceToolStripMenuItem
        '
        Me.WhatsTheDifferenceToolStripMenuItem.Name = "WhatsTheDifferenceToolStripMenuItem"
        Me.WhatsTheDifferenceToolStripMenuItem.Size = New System.Drawing.Size(191, 22)
        Me.WhatsTheDifferenceToolStripMenuItem.Text = "What's the difference?"
        '
        'EOCToolStripMenuItem
        '
        Me.EOCToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenSiteToolStripMenuItem, Me.StatusToolStripMenuItem, Me.IconToolStripMenuItem, Me.ToolStripMenuItem10, Me.SignatureToolStripMenuItem})
        Me.EOCToolStripMenuItem.Name = "EOCToolStripMenuItem"
        Me.EOCToolStripMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.EOCToolStripMenuItem.Text = "EOC"
        '
        'OpenSiteToolStripMenuItem
        '
        Me.OpenSiteToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartPageToolStripMenuItem, Me.ToolStripMenuItem8, Me.TeamStatisticsToolStripMenuItem, Me.UserStatisticsToolStripMenuItem})
        Me.OpenSiteToolStripMenuItem.Name = "OpenSiteToolStripMenuItem"
        Me.OpenSiteToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.OpenSiteToolStripMenuItem.Text = "Open site"
        '
        'StartPageToolStripMenuItem
        '
        Me.StartPageToolStripMenuItem.Name = "StartPageToolStripMenuItem"
        Me.StartPageToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.StartPageToolStripMenuItem.Text = "Start page"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(149, 6)
        '
        'TeamStatisticsToolStripMenuItem
        '
        Me.TeamStatisticsToolStripMenuItem.Name = "TeamStatisticsToolStripMenuItem"
        Me.TeamStatisticsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.TeamStatisticsToolStripMenuItem.Text = "Team statistics"
        '
        'UserStatisticsToolStripMenuItem
        '
        Me.UserStatisticsToolStripMenuItem.Name = "UserStatisticsToolStripMenuItem"
        Me.UserStatisticsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.UserStatisticsToolStripMenuItem.Text = "User statistics"
        '
        'StatusToolStripMenuItem
        '
        Me.StatusToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewUserStatusToolStripMenuItem, Me.ViewTeamStatusToolStripMenuItem})
        Me.StatusToolStripMenuItem.Name = "StatusToolStripMenuItem"
        Me.StatusToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.StatusToolStripMenuItem.Text = "Status"
        '
        'ViewUserStatusToolStripMenuItem
        '
        Me.ViewUserStatusToolStripMenuItem.CheckOnClick = True
        Me.ViewUserStatusToolStripMenuItem.Name = "ViewUserStatusToolStripMenuItem"
        Me.ViewUserStatusToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.ViewUserStatusToolStripMenuItem.Text = "View user status"
        '
        'ViewTeamStatusToolStripMenuItem
        '
        Me.ViewTeamStatusToolStripMenuItem.CheckOnClick = True
        Me.ViewTeamStatusToolStripMenuItem.Name = "ViewTeamStatusToolStripMenuItem"
        Me.ViewTeamStatusToolStripMenuItem.Size = New System.Drawing.Size(163, 22)
        Me.ViewTeamStatusToolStripMenuItem.Text = "View team status"
        '
        'IconToolStripMenuItem
        '
        Me.IconToolStripMenuItem.Name = "IconToolStripMenuItem"
        Me.IconToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.IconToolStripMenuItem.Text = "Show icon"
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(165, 6)
        '
        'SignatureToolStripMenuItem
        '
        Me.SignatureToolStripMenuItem.Name = "SignatureToolStripMenuItem"
        Me.SignatureToolStripMenuItem.Size = New System.Drawing.Size(168, 22)
        Me.SignatureToolStripMenuItem.Text = "Show signature(s)"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.LicenseToolStripMenuItem, Me.ToolStripMenuItem7, Me.LogMessagesToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'LicenseToolStripMenuItem
        '
        Me.LicenseToolStripMenuItem.Name = "LicenseToolStripMenuItem"
        Me.LicenseToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.LicenseToolStripMenuItem.Text = "License"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(145, 6)
        '
        'LogMessagesToolStripMenuItem
        '
        Me.LogMessagesToolStripMenuItem.Name = "LogMessagesToolStripMenuItem"
        Me.LogMessagesToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.LogMessagesToolStripMenuItem.Text = "Log messages"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FrameGraphsToolStripMenuItem, Me.LogToolStripMenuItem, Me.QueuedWusToolStripMenuItem, Me.ToolStripMenuItem6, Me.HardwareSensorsToolStripMenuItem, Me.ToolStripMenuItem1, Me.ToggleETAToolStripMenuItem, Me.ToolStripMenuItem2, Me.ProjectInfoToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'FrameGraphsToolStripMenuItem
        '
        Me.FrameGraphsToolStripMenuItem.Name = "FrameGraphsToolStripMenuItem"
        Me.FrameGraphsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.FrameGraphsToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.FrameGraphsToolStripMenuItem.Text = "Frame graphs"
        '
        'LogToolStripMenuItem
        '
        Me.LogToolStripMenuItem.Name = "LogToolStripMenuItem"
        Me.LogToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.LogToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.LogToolStripMenuItem.Text = "Log"
        '
        'QueuedWusToolStripMenuItem
        '
        Me.QueuedWusToolStripMenuItem.Name = "QueuedWusToolStripMenuItem"
        Me.QueuedWusToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.QueuedWusToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.QueuedWusToolStripMenuItem.Text = "Queued wu's"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(183, 6)
        '
        'HardwareSensorsToolStripMenuItem
        '
        Me.HardwareSensorsToolStripMenuItem.Name = "HardwareSensorsToolStripMenuItem"
        Me.HardwareSensorsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.HardwareSensorsToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.HardwareSensorsToolStripMenuItem.Text = "Hardware sensors"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(183, 6)
        '
        'ToggleETAToolStripMenuItem
        '
        Me.ToggleETAToolStripMenuItem.Name = "ToggleETAToolStripMenuItem"
        Me.ToggleETAToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.ToggleETAToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ToggleETAToolStripMenuItem.Text = "Toggle ETA"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(183, 6)
        '
        'ProjectInfoToolStripMenuItem
        '
        Me.ProjectInfoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProjectInfoListToolStripMenuItem, Me.ProjectInfoGraphCalculatorToolStripMenuItem})
        Me.ProjectInfoToolStripMenuItem.Name = "ProjectInfoToolStripMenuItem"
        Me.ProjectInfoToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ProjectInfoToolStripMenuItem.Text = "Project Info"
        '
        'ProjectInfoListToolStripMenuItem
        '
        Me.ProjectInfoListToolStripMenuItem.Name = "ProjectInfoListToolStripMenuItem"
        Me.ProjectInfoListToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ProjectInfoListToolStripMenuItem.Text = "List"
        '
        'ProjectInfoGraphCalculatorToolStripMenuItem
        '
        Me.ProjectInfoGraphCalculatorToolStripMenuItem.Name = "ProjectInfoGraphCalculatorToolStripMenuItem"
        Me.ProjectInfoGraphCalculatorToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ProjectInfoGraphCalculatorToolStripMenuItem.Text = "Graph calculator"
        '
        'DoUpdateToolStripMenuItem
        '
        Me.DoUpdateToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UpdateToolStripMenuItem})
        Me.DoUpdateToolStripMenuItem.Name = "DoUpdateToolStripMenuItem"
        Me.DoUpdateToolStripMenuItem.Size = New System.Drawing.Size(77, 20)
        Me.DoUpdateToolStripMenuItem.Text = "Do update "
        '
        'UpdateToolStripMenuItem
        '
        Me.UpdateToolStripMenuItem.Name = "UpdateToolStripMenuItem"
        Me.UpdateToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.UpdateToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.UpdateToolStripMenuItem.Text = "Update"
        '
        'tsTools
        '
        Me.tsTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MemtestG80ToolStripMenuItem, Me.MemtestCLToolStripMenuItem, Me.StressCPUV2ToolStripMenuItem, Me.ToolStripMenuItem4, Me.HowDoIEnableTheseToolsToolStripMenuItem, Me.ToolStripMenuItem5, Me.DiagnosticsToolStripMenuItem})
        Me.tsTools.Name = "tsTools"
        Me.tsTools.Size = New System.Drawing.Size(48, 20)
        Me.tsTools.Text = "Tools"
        '
        'MemtestG80ToolStripMenuItem
        '
        Me.MemtestG80ToolStripMenuItem.Enabled = False
        Me.MemtestG80ToolStripMenuItem.Name = "MemtestG80ToolStripMenuItem"
        Me.MemtestG80ToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
        Me.MemtestG80ToolStripMenuItem.Text = "memtestG80"
        '
        'MemtestCLToolStripMenuItem
        '
        Me.MemtestCLToolStripMenuItem.Enabled = False
        Me.MemtestCLToolStripMenuItem.Name = "MemtestCLToolStripMenuItem"
        Me.MemtestCLToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
        Me.MemtestCLToolStripMenuItem.Text = "memtestCL"
        '
        'StressCPUV2ToolStripMenuItem
        '
        Me.StressCPUV2ToolStripMenuItem.Enabled = False
        Me.StressCPUV2ToolStripMenuItem.Name = "StressCPUV2ToolStripMenuItem"
        Me.StressCPUV2ToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
        Me.StressCPUV2ToolStripMenuItem.Text = "StressCPU V2"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(222, 6)
        '
        'HowDoIEnableTheseToolsToolStripMenuItem
        '
        Me.HowDoIEnableTheseToolsToolStripMenuItem.Name = "HowDoIEnableTheseToolsToolStripMenuItem"
        Me.HowDoIEnableTheseToolsToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
        Me.HowDoIEnableTheseToolsToolStripMenuItem.Text = "How do I enable these tools?"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(222, 6)
        '
        'DiagnosticsToolStripMenuItem
        '
        Me.DiagnosticsToolStripMenuItem.Name = "DiagnosticsToolStripMenuItem"
        Me.DiagnosticsToolStripMenuItem.Size = New System.Drawing.Size(225, 22)
        Me.DiagnosticsToolStripMenuItem.Text = "Diagnostics"
        '
        'tEta
        '
        Me.tEta.Interval = 2500
        '
        'tTip
        '
        Me.tTip.ShowAlways = True
        '
        'frmLive
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1093, 703)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.msLive
        Me.Name = "frmLive"
        Me.scLive.Panel1.ResumeLayout(False)
        Me.scLive.Panel2.ResumeLayout(False)
        Me.scLive.ResumeLayout(False)
        Me.tlpLive.ResumeLayout(False)
        Me.tlpLive.PerformLayout()
        Me.sStripLive.ResumeLayout(False)
        Me.sStripLive.PerformLayout()
        Me.scLiveGraph.Panel1.ResumeLayout(False)
        Me.scLiveGraph.Panel2.ResumeLayout(False)
        Me.scLiveGraph.ResumeLayout(False)
        Me.scZgFramesSelection.Panel1.ResumeLayout(False)
        Me.scZgFramesSelection.Panel2.ResumeLayout(False)
        Me.scZgFramesSelection.ResumeLayout(False)
        Me.scListboxUpdate.Panel1.ResumeLayout(False)
        Me.scListboxUpdate.Panel2.ResumeLayout(False)
        Me.scListboxUpdate.ResumeLayout(False)
        Me.tlpWUSelection.ResumeLayout(False)
        Me.tcFramesLogQueue.ResumeLayout(False)
        Me.tpFramesGraph.ResumeLayout(False)
        Me.tpLog.ResumeLayout(False)
        Me.tlpLog.ResumeLayout(False)
        Me.tlpLog.PerformLayout()
        Me.tsLog.ResumeLayout(False)
        Me.tsLog.PerformLayout()
        Me.tpQueue.ResumeLayout(False)
        Me.scQueueLogMain.Panel1.ResumeLayout(False)
        Me.scQueueLogMain.Panel2.ResumeLayout(False)
        Me.scQueueLogMain.Panel2.PerformLayout()
        Me.scQueueLogMain.ResumeLayout(False)
        Me.scQueue.Panel1.ResumeLayout(False)
        Me.scQueue.Panel2.ResumeLayout(False)
        Me.scQueue.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.sStripEOC.ResumeLayout(False)
        Me.sStripEOC.PerformLayout()
        Me.sStripEoc2.ResumeLayout(False)
        Me.sStripEoc2.PerformLayout()
        Me.sStripStatistics.ResumeLayout(False)
        Me.sStripStatistics.PerformLayout()
        Me.sStripMessage.ResumeLayout(False)
        Me.sStripMessage.PerformLayout()
        Me.scHardware.Panel1.ResumeLayout(False)
        Me.scHardware.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.msLive.ResumeLayout(False)
        Me.msLive.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvLive As System.Windows.Forms.ListView
    Friend WithEvents chClient As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSlot As System.Windows.Forms.ColumnHeader
    Friend WithEvents chHW As System.Windows.Forms.ColumnHeader
    Friend WithEvents chProgress As System.Windows.Forms.ColumnHeader
    Friend WithEvents chCredit As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPpd As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEta As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPRCG As System.Windows.Forms.ColumnHeader
    Friend WithEvents tParse As System.Windows.Forms.Timer
    Friend WithEvents tFill As System.Windows.Forms.Timer
    Friend WithEvents chTpf As System.Windows.Forms.ColumnHeader
    Friend WithEvents scLive As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdViewGraph As System.Windows.Forms.Button
    Friend WithEvents scLiveGraph As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents sStripMessage As System.Windows.Forms.StatusStrip
    Friend WithEvents tsStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents zgFrames As ZedGraph.ZedGraphControl
    Friend WithEvents scZgFramesSelection As System.Windows.Forms.SplitContainer
    Friend WithEvents scListboxUpdate As System.Windows.Forms.SplitContainer
    Friend WithEvents chklbWU As System.Windows.Forms.CheckedListBox
    Friend WithEvents scHardware As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdHWSensors As System.Windows.Forms.Button
    Friend WithEvents tlpWUSelection As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateFrames As System.Windows.Forms.Button
    Friend WithEvents cmdPrev As System.Windows.Forms.Button
    Friend WithEvents cmdNext As System.Windows.Forms.Button
    Friend WithEvents cmdSelectAll As System.Windows.Forms.Button
    Friend WithEvents msLive As System.Windows.Forms.MenuStrip
    Friend WithEvents EOCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenSiteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TeamStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewUserStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewTeamStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SignatureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneralToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NotificationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GraphOptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OverallToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents WhatsTheDifferenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MinimizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HistoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LicenseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents LogMessagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sStripStatistics As System.Windows.Forms.StatusStrip
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsStatistics As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sStripEOC As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sStripEoc2 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ProjectInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DoUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemtestG80ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemtestCLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StressCPUV2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HowDoIEnableTheseToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DiagnosticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FrameGraphsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HardwareSensorsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tlpLive As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents sStripLive As System.Windows.Forms.StatusStrip
    Friend WithEvents tsLblLive As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tcFramesLogQueue As System.Windows.Forms.TabControl
    Friend WithEvents tpFramesGraph As System.Windows.Forms.TabPage
    Friend WithEvents tpLog As System.Windows.Forms.TabPage
    Friend WithEvents tlpLog As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tpQueue As System.Windows.Forms.TabPage
    Friend WithEvents lvQueue As System.Windows.Forms.ListView
    Friend WithEvents chClientQueue As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSlotQueue As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPrcgQueue As System.Windows.Forms.ColumnHeader
    Friend WithEvents chFinalDeadline As System.Windows.Forms.ColumnHeader
    Friend WithEvents QueuedWusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToggleETAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tEta As System.Windows.Forms.Timer
    Friend WithEvents tsLog As System.Windows.Forms.ToolStrip
    Friend WithEvents tsFollowLog As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsWarningsAndErrors As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsCombinedLog As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsIncludeOtherWUs As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents scQueueLogMain As System.Windows.Forms.SplitContainer
    Friend WithEvents scQueue As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdToggleQueueLog As System.Windows.Forms.Button
    Friend WithEvents txtLogQueue As System.Windows.Forms.TextBox
    Friend WithEvents chCreditRemaining As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPPDRemaining As System.Windows.Forms.ColumnHeader
    Friend WithEvents ProjectInfoListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectInfoGraphCalculatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chLastAttempt As System.Windows.Forms.ColumnHeader
    Friend WithEvents tTip As System.Windows.Forms.ToolTip
End Class
