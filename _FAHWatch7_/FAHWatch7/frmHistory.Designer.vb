<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHistory))
        Me.PRCG = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Downloaded = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Submitted = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.WorkServer = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.FahCore = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Hardware = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Slot = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Client = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.DetailedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.tscHistory = New System.Windows.Forms.ToolStripContainer()
        Me.lvWU = New System.Windows.Forms.ListView()
        Me.TPF = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.Credit = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.PPD = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.FahCoreVersion = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.CoreStatus = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.ServerResponse = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.UploadSize = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.UploadSpeed = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.DownloadSize = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.DownloadSpeed = CType(New System.Windows.Forms.ColumnHeader(),System.Windows.Forms.ColumnHeader)
        Me.tsHistory = New System.Windows.Forms.ToolStrip()
        Me.tsCmdClearSort = New System.Windows.Forms.ToolStripButton()
        Me.tsSepClear = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.tsHQFClient = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tsHQFHardware = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.tsHQFProject = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.tsHQFTimeLimit = New System.Windows.Forms.ToolStripComboBox()
        Me.tsTimeLimitLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.sFilters = New System.Windows.Forms.StatusStrip()
        Me.scDetail = New System.Windows.Forms.SplitContainer()
        Me.tcDetail = New System.Windows.Forms.TabControl()
        Me.tpWU = New System.Windows.Forms.TabPage()
        Me.rtWU = New System.Windows.Forms.RichTextBox()
        Me.cmDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tpPerformance = New System.Windows.Forms.TabPage()
        Me.rtPerformance = New System.Windows.Forms.RichTextBox()
        Me.tpProjects = New System.Windows.Forms.TabPage()
        Me.scProjects = New System.Windows.Forms.SplitContainer()
        Me.tsProjects = New System.Windows.Forms.ToolStrip()
        Me.tsProjects_cmbProjects = New System.Windows.Forms.ToolStripComboBox()
        Me.tsProjects_cmbRCG = New System.Windows.Forms.ToolStripComboBox()
        Me.rtProjects = New System.Windows.Forms.RichTextBox()
        Me.tpHardware = New System.Windows.Forms.TabPage()
        Me.rtHW = New System.Windows.Forms.RichTextBox()
        Me.zgProject = New ZedGraph.ZedGraphControl()
        Me.sStripStatistics = New System.Windows.Forms.StatusStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsStatistics = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DefaultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectColumnsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LargeGraphToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ProjectInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectInfoListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectInfoCalculatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.EditFiltersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsiFilters = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FiltersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClientsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HardwareToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.msMAIN = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LiveMonitoringToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LicenseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.LogMessagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GeneralToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdvancedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.tscMain = New System.Windows.Forms.ToolStripContainer()
        Me.sStripEOC = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sStripEoc2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sStripMessage = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tShow = New System.Windows.Forms.Timer(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.cmDetailWU = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tscLog = New System.Windows.Forms.ToolStripContainer()
        Me.tsLog = New System.Windows.Forms.ToolStrip()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.tscHistory.ContentPanel.SuspendLayout()
        Me.tscHistory.TopToolStripPanel.SuspendLayout()
        Me.tscHistory.SuspendLayout()
        Me.tsHistory.SuspendLayout()
        Me.scDetail.Panel1.SuspendLayout()
        Me.scDetail.Panel2.SuspendLayout()
        Me.scDetail.SuspendLayout()
        Me.tcDetail.SuspendLayout()
        Me.tpWU.SuspendLayout()
        Me.tpPerformance.SuspendLayout()
        Me.tpProjects.SuspendLayout()
        Me.scProjects.Panel1.SuspendLayout()
        Me.scProjects.Panel2.SuspendLayout()
        Me.scProjects.SuspendLayout()
        Me.tsProjects.SuspendLayout()
        Me.tpHardware.SuspendLayout()
        Me.sStripStatistics.SuspendLayout()
        Me.msMAIN.SuspendLayout()
        Me.tscMain.BottomToolStripPanel.SuspendLayout()
        Me.tscMain.ContentPanel.SuspendLayout()
        Me.tscMain.TopToolStripPanel.SuspendLayout()
        Me.tscMain.SuspendLayout()
        Me.sStripEOC.SuspendLayout()
        Me.sStripEoc2.SuspendLayout()
        Me.sStripMessage.SuspendLayout()
        Me.tscLog.ContentPanel.SuspendLayout()
        Me.tscLog.TopToolStripPanel.SuspendLayout()
        Me.tscLog.SuspendLayout()
        Me.SuspendLayout()
        '
        'PRCG
        '
        Me.PRCG.Text = "PRCG"
        Me.PRCG.Width = 65
        '
        'Downloaded
        '
        Me.Downloaded.Text = "Downloaded"
        Me.Downloaded.Width = 160
        '
        'Submitted
        '
        Me.Submitted.Text = "Submitted"
        Me.Submitted.Width = 69
        '
        'WorkServer
        '
        Me.WorkServer.Text = "WorkServer"
        Me.WorkServer.Width = 88
        '
        'FahCore
        '
        Me.FahCore.Text = "FahCore"
        Me.FahCore.Width = 66
        '
        'Hardware
        '
        Me.Hardware.Text = "Hardware"
        '
        'Slot
        '
        Me.Slot.Text = "Slot"
        Me.Slot.Width = 41
        '
        'Client
        '
        Me.Client.Text = "Client"
        '
        'DetailedToolStripMenuItem
        '
        Me.DetailedToolStripMenuItem.CheckOnClick = True
        Me.DetailedToolStripMenuItem.Name = "DetailedToolStripMenuItem"
        Me.DetailedToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DetailedToolStripMenuItem.Text = "Detailed"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(151, 6)
        '
        'scMain
        '
        Me.scMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.tscHistory)
        Me.scMain.Panel1.Controls.Add(Me.sFilters)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.scDetail)
        Me.scMain.Size = New System.Drawing.Size(1338, 504)
        Me.scMain.SplitterDistance = 261
        Me.scMain.TabIndex = 11
        '
        'tscHistory
        '
        Me.tscHistory.BottomToolStripPanelVisible = False
        '
        'tscHistory.ContentPanel
        '
        Me.tscHistory.ContentPanel.Controls.Add(Me.lvWU)
        Me.tscHistory.ContentPanel.Size = New System.Drawing.Size(1334, 232)
        Me.tscHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tscHistory.LeftToolStripPanelVisible = False
        Me.tscHistory.Location = New System.Drawing.Point(0, 0)
        Me.tscHistory.Name = "tscHistory"
        Me.tscHistory.RightToolStripPanelVisible = False
        Me.tscHistory.Size = New System.Drawing.Size(1334, 257)
        Me.tscHistory.TabIndex = 7
        Me.tscHistory.Text = "ToolStripContainer2"
        '
        'tscHistory.TopToolStripPanel
        '
        Me.tscHistory.TopToolStripPanel.Controls.Add(Me.tsHistory)
        '
        'lvWU
        '
        Me.lvWU.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lvWU.AllowColumnReorder = True
        Me.lvWU.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Client, Me.Slot, Me.Hardware, Me.PRCG, Me.TPF, Me.Credit, Me.PPD, Me.Downloaded, Me.Submitted, Me.FahCore, Me.FahCoreVersion, Me.CoreStatus, Me.ServerResponse, Me.WorkServer, Me.UploadSize, Me.UploadSpeed, Me.DownloadSize, Me.DownloadSpeed})
        Me.lvWU.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvWU.FullRowSelect = True
        Me.lvWU.GridLines = True
        Me.lvWU.HideSelection = False
        Me.lvWU.Location = New System.Drawing.Point(0, 0)
        Me.lvWU.MultiSelect = False
        Me.lvWU.Name = "lvWU"
        Me.lvWU.ShowItemToolTips = True
        Me.lvWU.Size = New System.Drawing.Size(1334, 232)
        Me.lvWU.TabIndex = 5
        Me.lvWU.UseCompatibleStateImageBehavior = False
        Me.lvWU.View = System.Windows.Forms.View.Details
        '
        'TPF
        '
        Me.TPF.Text = "TPF"
        Me.TPF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TPF.Width = 43
        '
        'Credit
        '
        Me.Credit.Text = "Credit"
        Me.Credit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Credit.Width = 50
        '
        'PPD
        '
        Me.PPD.Text = "PPD"
        Me.PPD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.PPD.Width = 44
        '
        'FahCoreVersion
        '
        Me.FahCoreVersion.Text = "FahCoreVersion"
        Me.FahCoreVersion.Width = 99
        '
        'CoreStatus
        '
        Me.CoreStatus.Text = "CoreStatus"
        Me.CoreStatus.Width = 80
        '
        'ServerResponse
        '
        Me.ServerResponse.Text = "ServerResponse"
        Me.ServerResponse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ServerResponse.Width = 95
        '
        'UploadSize
        '
        Me.UploadSize.Text = "ResultSize"
        Me.UploadSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.UploadSize.Width = 79
        '
        'UploadSpeed
        '
        Me.UploadSpeed.Text = "UploadSpeed"
        Me.UploadSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.UploadSpeed.Width = 81
        '
        'DownloadSize
        '
        Me.DownloadSize.Text = "DownloadSize"
        Me.DownloadSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.DownloadSize.Width = 85
        '
        'DownloadSpeed
        '
        Me.DownloadSpeed.Text = "DownloadSpeed"
        Me.DownloadSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.DownloadSpeed.Width = 92
        '
        'tsHistory
        '
        Me.tsHistory.Dock = System.Windows.Forms.DockStyle.None
        Me.tsHistory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsHistory.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsCmdClearSort, Me.tsSepClear, Me.ToolStripLabel4, Me.tsHQFClient, Me.ToolStripLabel1, Me.tsHQFHardware, Me.ToolStripLabel3, Me.tsHQFProject, Me.ToolStripSeparator2, Me.ToolStripLabel2, Me.tsHQFTimeLimit, Me.tsTimeLimitLabel, Me.ToolStripSeparator3})
        Me.tsHistory.Location = New System.Drawing.Point(0, 0)
        Me.tsHistory.Name = "tsHistory"
        Me.tsHistory.Size = New System.Drawing.Size(1334, 25)
        Me.tsHistory.Stretch = True
        Me.tsHistory.TabIndex = 0
        '
        'tsCmdClearSort
        '
        Me.tsCmdClearSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsCmdClearSort.Image = CType(resources.GetObject("tsCmdClearSort.Image"), System.Drawing.Image)
        Me.tsCmdClearSort.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsCmdClearSort.Name = "tsCmdClearSort"
        Me.tsCmdClearSort.Size = New System.Drawing.Size(61, 22)
        Me.tsCmdClearSort.Text = "Clear sort"
        Me.tsCmdClearSort.ToolTipText = "Clear sorting of items"
        Me.tsCmdClearSort.Visible = False
        '
        'tsSepClear
        '
        Me.tsSepClear.Name = "tsSepClear"
        Me.tsSepClear.Size = New System.Drawing.Size(6, 25)
        Me.tsSepClear.Visible = False
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(43, 22)
        Me.ToolStripLabel4.Text = "Clients"
        '
        'tsHQFClient
        '
        Me.tsHQFClient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.tsHQFClient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.tsHQFClient.DropDownWidth = 200
        Me.tsHQFClient.Name = "tsHQFClient"
        Me.tsHQFClient.Size = New System.Drawing.Size(200, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(58, 22)
        Me.ToolStripLabel1.Text = "Hardware"
        '
        'tsHQFHardware
        '
        Me.tsHQFHardware.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.tsHQFHardware.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.tsHQFHardware.DropDownWidth = 250
        Me.tsHQFHardware.Name = "tsHQFHardware"
        Me.tsHQFHardware.Size = New System.Drawing.Size(250, 25)
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(49, 22)
        Me.ToolStripLabel3.Text = "Projects"
        '
        'tsHQFProject
        '
        Me.tsHQFProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsHQFProject.Name = "tsHQFProject"
        Me.tsHQFProject.Size = New System.Drawing.Size(121, 25)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(61, 22)
        Me.ToolStripLabel2.Text = "Time limit"
        '
        'tsHQFTimeLimit
        '
        Me.tsHQFTimeLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsHQFTimeLimit.Enabled = False
        Me.tsHQFTimeLimit.Items.AddRange(New Object() {"Disabled", "Before", "After", "Between", "EOC_LastUpdate", "EOC_Today", "EOC_Week", "EOC_Month"})
        Me.tsHQFTimeLimit.Name = "tsHQFTimeLimit"
        Me.tsHQFTimeLimit.Size = New System.Drawing.Size(121, 25)
        '
        'tsTimeLimitLabel
        '
        Me.tsTimeLimitLabel.Name = "tsTimeLimitLabel"
        Me.tsTimeLimitLabel.Size = New System.Drawing.Size(120, 22)
        Me.tsTimeLimitLabel.Text = "time limit description"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'sFilters
        '
        Me.sFilters.Location = New System.Drawing.Point(0, 235)
        Me.sFilters.Name = "sFilters"
        Me.sFilters.Size = New System.Drawing.Size(1334, 22)
        Me.sFilters.SizingGrip = False
        Me.sFilters.TabIndex = 6
        Me.sFilters.Visible = False
        '
        'scDetail
        '
        Me.scDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.scDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scDetail.Location = New System.Drawing.Point(0, 0)
        Me.scDetail.Name = "scDetail"
        '
        'scDetail.Panel1
        '
        Me.scDetail.Panel1.Controls.Add(Me.tcDetail)
        '
        'scDetail.Panel2
        '
        Me.scDetail.Panel2.Controls.Add(Me.zgProject)
        Me.scDetail.Size = New System.Drawing.Size(1338, 239)
        Me.scDetail.SplitterDistance = 557
        Me.scDetail.TabIndex = 1
        '
        'tcDetail
        '
        Me.tcDetail.Controls.Add(Me.tpWU)
        Me.tcDetail.Controls.Add(Me.tpPerformance)
        Me.tcDetail.Controls.Add(Me.tpProjects)
        Me.tcDetail.Controls.Add(Me.tpHardware)
        Me.tcDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcDetail.Location = New System.Drawing.Point(0, 0)
        Me.tcDetail.Name = "tcDetail"
        Me.tcDetail.SelectedIndex = 0
        Me.tcDetail.Size = New System.Drawing.Size(553, 235)
        Me.tcDetail.TabIndex = 3
        '
        'tpWU
        '
        Me.tpWU.Controls.Add(Me.tscLog)
        Me.tpWU.Location = New System.Drawing.Point(4, 22)
        Me.tpWU.Name = "tpWU"
        Me.tpWU.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWU.Size = New System.Drawing.Size(545, 209)
        Me.tpWU.TabIndex = 0
        Me.tpWU.Text = "WU details"
        Me.tpWU.UseVisualStyleBackColor = True
        '
        'rtWU
        '
        Me.rtWU.BackColor = System.Drawing.SystemColors.Info
        Me.rtWU.ContextMenuStrip = Me.cmDetails
        Me.rtWU.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtWU.Location = New System.Drawing.Point(0, 0)
        Me.rtWU.Name = "rtWU"
        Me.rtWU.ReadOnly = True
        Me.rtWU.Size = New System.Drawing.Size(539, 203)
        Me.rtWU.TabIndex = 1
        Me.rtWU.Text = ""
        '
        'cmDetails
        '
        Me.cmDetails.Name = "cmDetails"
        Me.cmDetails.Size = New System.Drawing.Size(61, 4)
        '
        'tpPerformance
        '
        Me.tpPerformance.Controls.Add(Me.rtPerformance)
        Me.tpPerformance.Location = New System.Drawing.Point(4, 22)
        Me.tpPerformance.Name = "tpPerformance"
        Me.tpPerformance.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPerformance.Size = New System.Drawing.Size(545, 209)
        Me.tpPerformance.TabIndex = 1
        Me.tpPerformance.Text = "Performance"
        Me.tpPerformance.UseVisualStyleBackColor = True
        '
        'rtPerformance
        '
        Me.rtPerformance.ContextMenuStrip = Me.cmDetails
        Me.rtPerformance.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtPerformance.Location = New System.Drawing.Point(3, 3)
        Me.rtPerformance.Name = "rtPerformance"
        Me.rtPerformance.ReadOnly = True
        Me.rtPerformance.Size = New System.Drawing.Size(539, 203)
        Me.rtPerformance.TabIndex = 1
        Me.rtPerformance.Text = ""
        '
        'tpProjects
        '
        Me.tpProjects.Controls.Add(Me.scProjects)
        Me.tpProjects.Location = New System.Drawing.Point(4, 22)
        Me.tpProjects.Name = "tpProjects"
        Me.tpProjects.Padding = New System.Windows.Forms.Padding(3)
        Me.tpProjects.Size = New System.Drawing.Size(545, 209)
        Me.tpProjects.TabIndex = 2
        Me.tpProjects.Text = "Projects"
        Me.tpProjects.UseVisualStyleBackColor = True
        '
        'scProjects
        '
        Me.scProjects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scProjects.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scProjects.IsSplitterFixed = True
        Me.scProjects.Location = New System.Drawing.Point(3, 3)
        Me.scProjects.Name = "scProjects"
        Me.scProjects.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scProjects.Panel1
        '
        Me.scProjects.Panel1.Controls.Add(Me.tsProjects)
        '
        'scProjects.Panel2
        '
        Me.scProjects.Panel2.Controls.Add(Me.rtProjects)
        Me.scProjects.Size = New System.Drawing.Size(539, 203)
        Me.scProjects.SplitterDistance = 25
        Me.scProjects.SplitterWidth = 1
        Me.scProjects.TabIndex = 0
        '
        'tsProjects
        '
        Me.tsProjects.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsProjects_cmbProjects, Me.tsProjects_cmbRCG})
        Me.tsProjects.Location = New System.Drawing.Point(0, 0)
        Me.tsProjects.Name = "tsProjects"
        Me.tsProjects.Size = New System.Drawing.Size(539, 25)
        Me.tsProjects.Stretch = True
        Me.tsProjects.TabIndex = 2
        '
        'tsProjects_cmbProjects
        '
        Me.tsProjects_cmbProjects.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.tsProjects_cmbProjects.BackColor = System.Drawing.SystemColors.Control
        Me.tsProjects_cmbProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsProjects_cmbProjects.Name = "tsProjects_cmbProjects"
        Me.tsProjects_cmbProjects.Size = New System.Drawing.Size(121, 25)
        '
        'tsProjects_cmbRCG
        '
        Me.tsProjects_cmbRCG.BackColor = System.Drawing.SystemColors.Control
        Me.tsProjects_cmbRCG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsProjects_cmbRCG.DropDownWidth = 300
        Me.tsProjects_cmbRCG.Name = "tsProjects_cmbRCG"
        Me.tsProjects_cmbRCG.Size = New System.Drawing.Size(300, 25)
        '
        'rtProjects
        '
        Me.rtProjects.ContextMenuStrip = Me.cmDetails
        Me.rtProjects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtProjects.Location = New System.Drawing.Point(0, 0)
        Me.rtProjects.Name = "rtProjects"
        Me.rtProjects.ReadOnly = True
        Me.rtProjects.Size = New System.Drawing.Size(539, 177)
        Me.rtProjects.TabIndex = 1
        Me.rtProjects.Text = ""
        '
        'tpHardware
        '
        Me.tpHardware.Controls.Add(Me.rtHW)
        Me.tpHardware.Location = New System.Drawing.Point(4, 22)
        Me.tpHardware.Name = "tpHardware"
        Me.tpHardware.Padding = New System.Windows.Forms.Padding(3)
        Me.tpHardware.Size = New System.Drawing.Size(545, 209)
        Me.tpHardware.TabIndex = 3
        Me.tpHardware.Text = "Hardware"
        Me.tpHardware.UseVisualStyleBackColor = True
        '
        'rtHW
        '
        Me.rtHW.ContextMenuStrip = Me.cmDetails
        Me.rtHW.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtHW.Location = New System.Drawing.Point(3, 3)
        Me.rtHW.Name = "rtHW"
        Me.rtHW.ReadOnly = True
        Me.rtHW.Size = New System.Drawing.Size(539, 203)
        Me.rtHW.TabIndex = 0
        Me.rtHW.Text = ""
        '
        'zgProject
        '
        Me.zgProject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgProject.Location = New System.Drawing.Point(0, 0)
        Me.zgProject.Name = "zgProject"
        Me.zgProject.ScrollGrace = 0.0R
        Me.zgProject.ScrollMaxX = 0.0R
        Me.zgProject.ScrollMaxY = 0.0R
        Me.zgProject.ScrollMaxY2 = 0.0R
        Me.zgProject.ScrollMinX = 0.0R
        Me.zgProject.ScrollMinY = 0.0R
        Me.zgProject.ScrollMinY2 = 0.0R
        Me.zgProject.Size = New System.Drawing.Size(773, 235)
        Me.zgProject.TabIndex = 4
        '
        'sStripStatistics
        '
        Me.sStripStatistics.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripStatistics.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus, Me.tsStatistics})
        Me.sStripStatistics.Location = New System.Drawing.Point(0, 44)
        Me.sStripStatistics.Name = "sStripStatistics"
        Me.sStripStatistics.Size = New System.Drawing.Size(1338, 22)
        Me.sStripStatistics.TabIndex = 0
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
        'DefaultToolStripMenuItem
        '
        Me.DefaultToolStripMenuItem.CheckOnClick = True
        Me.DefaultToolStripMenuItem.Name = "DefaultToolStripMenuItem"
        Me.DefaultToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DefaultToolStripMenuItem.Text = "Default"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DefaultToolStripMenuItem, Me.DetailedToolStripMenuItem, Me.SelectColumnsToolStripMenuItem, Me.ToolStripMenuItem6, Me.LargeGraphToolStripMenuItem, Me.ToolStripMenuItem1, Me.ProjectInfoToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'SelectColumnsToolStripMenuItem
        '
        Me.SelectColumnsToolStripMenuItem.Enabled = False
        Me.SelectColumnsToolStripMenuItem.Name = "SelectColumnsToolStripMenuItem"
        Me.SelectColumnsToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.SelectColumnsToolStripMenuItem.Text = "Select columns"
        '
        'LargeGraphToolStripMenuItem
        '
        Me.LargeGraphToolStripMenuItem.Name = "LargeGraphToolStripMenuItem"
        Me.LargeGraphToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.LargeGraphToolStripMenuItem.Text = "Large graph"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(151, 6)
        '
        'ProjectInfoToolStripMenuItem
        '
        Me.ProjectInfoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProjectInfoListToolStripMenuItem, Me.ProjectInfoCalculatorToolStripMenuItem})
        Me.ProjectInfoToolStripMenuItem.Name = "ProjectInfoToolStripMenuItem"
        Me.ProjectInfoToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.ProjectInfoToolStripMenuItem.Text = "Project Info"
        '
        'ProjectInfoListToolStripMenuItem
        '
        Me.ProjectInfoListToolStripMenuItem.Name = "ProjectInfoListToolStripMenuItem"
        Me.ProjectInfoListToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ProjectInfoListToolStripMenuItem.Text = "List"
        '
        'ProjectInfoCalculatorToolStripMenuItem
        '
        Me.ProjectInfoCalculatorToolStripMenuItem.Name = "ProjectInfoCalculatorToolStripMenuItem"
        Me.ProjectInfoCalculatorToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ProjectInfoCalculatorToolStripMenuItem.Text = "Graph calculator"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(137, 6)
        '
        'EditFiltersToolStripMenuItem
        '
        Me.EditFiltersToolStripMenuItem.Name = "EditFiltersToolStripMenuItem"
        Me.EditFiltersToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.EditFiltersToolStripMenuItem.Text = "Edit filters"
        '
        'tsiFilters
        '
        Me.tsiFilters.Name = "tsiFilters"
        Me.tsiFilters.Size = New System.Drawing.Size(140, 22)
        Me.tsiFilters.Text = "Stored filters"
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.ClearToolStripMenuItem.Text = "Clear"
        '
        'FiltersToolStripMenuItem
        '
        Me.FiltersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearToolStripMenuItem, Me.tsiFilters, Me.ClientsToolStripMenuItem, Me.HardwareToolStripMenuItem, Me.ProjectsToolStripMenuItem, Me.ToolStripMenuItem2, Me.EditFiltersToolStripMenuItem})
        Me.FiltersToolStripMenuItem.Name = "FiltersToolStripMenuItem"
        Me.FiltersToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.FiltersToolStripMenuItem.Text = "Filters"
        '
        'ClientsToolStripMenuItem
        '
        Me.ClientsToolStripMenuItem.Name = "ClientsToolStripMenuItem"
        Me.ClientsToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.ClientsToolStripMenuItem.Text = "Clients"
        '
        'HardwareToolStripMenuItem
        '
        Me.HardwareToolStripMenuItem.Name = "HardwareToolStripMenuItem"
        Me.HardwareToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.HardwareToolStripMenuItem.Text = "Hardware"
        '
        'ProjectsToolStripMenuItem
        '
        Me.ProjectsToolStripMenuItem.Name = "ProjectsToolStripMenuItem"
        Me.ProjectsToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.ProjectsToolStripMenuItem.Text = "Projects"
        '
        'msMAIN
        '
        Me.msMAIN.Dock = System.Windows.Forms.DockStyle.None
        Me.msMAIN.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.FiltersToolStripMenuItem, Me.HelpToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.EOCToolStripMenuItem, Me.ViewToolStripMenuItem, Me.DoUpdateToolStripMenuItem, Me.tsTools})
        Me.msMAIN.Location = New System.Drawing.Point(0, 0)
        Me.msMAIN.Name = "msMAIN"
        Me.msMAIN.Size = New System.Drawing.Size(1338, 24)
        Me.msMAIN.TabIndex = 10
        Me.msMAIN.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MinimizeToolStripMenuItem, Me.LiveMonitoringToolStripMenuItem, Me.ToolStripMenuItem3, Me.CloseToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'MinimizeToolStripMenuItem
        '
        Me.MinimizeToolStripMenuItem.Name = "MinimizeToolStripMenuItem"
        Me.MinimizeToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.MinimizeToolStripMenuItem.Text = "Minimize"
        '
        'LiveMonitoringToolStripMenuItem
        '
        Me.LiveMonitoringToolStripMenuItem.Name = "LiveMonitoringToolStripMenuItem"
        Me.LiveMonitoringToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.LiveMonitoringToolStripMenuItem.Text = "Live monitoring"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(155, 6)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.CloseToolStripMenuItem.Text = "Close"
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
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GeneralToolStripMenuItem, Me.AdvancedToolStripMenuItem, Me.GraphOptionsToolStripMenuItem, Me.ToolStripSeparator1, Me.SelectStatisticsToolStripMenuItem})
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
        'AdvancedToolStripMenuItem
        '
        Me.AdvancedToolStripMenuItem.Name = "AdvancedToolStripMenuItem"
        Me.AdvancedToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.AdvancedToolStripMenuItem.Text = "Advanced"
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
        'tscMain
        '
        '
        'tscMain.BottomToolStripPanel
        '
        Me.tscMain.BottomToolStripPanel.Controls.Add(Me.sStripEOC)
        Me.tscMain.BottomToolStripPanel.Controls.Add(Me.sStripEoc2)
        Me.tscMain.BottomToolStripPanel.Controls.Add(Me.sStripStatistics)
        Me.tscMain.BottomToolStripPanel.Controls.Add(Me.sStripMessage)
        '
        'tscMain.ContentPanel
        '
        Me.tscMain.ContentPanel.AutoScroll = True
        Me.tscMain.ContentPanel.Controls.Add(Me.scMain)
        Me.tscMain.ContentPanel.Size = New System.Drawing.Size(1338, 504)
        Me.tscMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tscMain.Location = New System.Drawing.Point(0, 0)
        Me.tscMain.Name = "tscMain"
        Me.tscMain.Size = New System.Drawing.Size(1338, 594)
        Me.tscMain.TabIndex = 12
        Me.tscMain.Text = "ToolStripContainer1"
        '
        'tscMain.TopToolStripPanel
        '
        Me.tscMain.TopToolStripPanel.Controls.Add(Me.msMAIN)
        '
        'sStripEOC
        '
        Me.sStripEOC.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripEOC.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2})
        Me.sStripEOC.Location = New System.Drawing.Point(0, 0)
        Me.sStripEOC.Name = "sStripEOC"
        Me.sStripEOC.Size = New System.Drawing.Size(1338, 22)
        Me.sStripEOC.TabIndex = 1
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
        Me.sStripEoc2.Size = New System.Drawing.Size(1338, 22)
        Me.sStripEoc2.TabIndex = 3
        Me.sStripEoc2.Text = "Eoc2"
        '
        'ToolStripStatusLabel3
        '
        Me.ToolStripStatusLabel3.Name = "ToolStripStatusLabel3"
        Me.ToolStripStatusLabel3.Size = New System.Drawing.Size(56, 17)
        Me.ToolStripStatusLabel3.Text = "EocTeam"
        '
        'sStripMessage
        '
        Me.sStripMessage.Dock = System.Windows.Forms.DockStyle.None
        Me.sStripMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.sStripMessage.Location = New System.Drawing.Point(0, 66)
        Me.sStripMessage.Name = "sStripMessage"
        Me.sStripMessage.Size = New System.Drawing.Size(17, 22)
        Me.sStripMessage.TabIndex = 2
        Me.sStripMessage.Visible = False
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 17)
        '
        'tShow
        '
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(444, 253)
        '
        'cmDetailWU
        '
        Me.cmDetailWU.Name = "cmLvWU"
        Me.cmDetailWU.Size = New System.Drawing.Size(61, 4)
        '
        'tscLog
        '
        '
        'tscLog.ContentPanel
        '
        Me.tscLog.ContentPanel.Controls.Add(Me.rtWU)
        Me.tscLog.ContentPanel.Size = New System.Drawing.Size(539, 203)
        Me.tscLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tscLog.Location = New System.Drawing.Point(3, 3)
        Me.tscLog.Name = "tscLog"
        Me.tscLog.Size = New System.Drawing.Size(539, 203)
        Me.tscLog.TabIndex = 2
        Me.tscLog.Text = "ToolStripContainer2"
        '
        'tscLog.TopToolStripPanel
        '
        Me.tscLog.TopToolStripPanel.Controls.Add(Me.tsLog)
        Me.tscLog.TopToolStripPanelVisible = False
        '
        'tsLog
        '
        Me.tsLog.Dock = System.Windows.Forms.DockStyle.None
        Me.tsLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsLog.Location = New System.Drawing.Point(0, 0)
        Me.tsLog.Name = "tsLog"
        Me.tsLog.Size = New System.Drawing.Size(539, 25)
        Me.tsLog.Stretch = True
        Me.tsLog.TabIndex = 0
        '
        'frmHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1338, 594)
        Me.Controls.Add(Me.tscMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmHistory"
        Me.Opacity = 0.0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "FAHWatch7 preview"
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel1.PerformLayout()
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.ResumeLayout(False)
        Me.tscHistory.ContentPanel.ResumeLayout(False)
        Me.tscHistory.TopToolStripPanel.ResumeLayout(False)
        Me.tscHistory.TopToolStripPanel.PerformLayout()
        Me.tscHistory.ResumeLayout(False)
        Me.tscHistory.PerformLayout()
        Me.tsHistory.ResumeLayout(False)
        Me.tsHistory.PerformLayout()
        Me.scDetail.Panel1.ResumeLayout(False)
        Me.scDetail.Panel2.ResumeLayout(False)
        Me.scDetail.ResumeLayout(False)
        Me.tcDetail.ResumeLayout(False)
        Me.tpWU.ResumeLayout(False)
        Me.tpPerformance.ResumeLayout(False)
        Me.tpProjects.ResumeLayout(False)
        Me.scProjects.Panel1.ResumeLayout(False)
        Me.scProjects.Panel1.PerformLayout()
        Me.scProjects.Panel2.ResumeLayout(False)
        Me.scProjects.ResumeLayout(False)
        Me.tsProjects.ResumeLayout(False)
        Me.tsProjects.PerformLayout()
        Me.tpHardware.ResumeLayout(False)
        Me.sStripStatistics.ResumeLayout(False)
        Me.sStripStatistics.PerformLayout()
        Me.msMAIN.ResumeLayout(False)
        Me.msMAIN.PerformLayout()
        Me.tscMain.BottomToolStripPanel.ResumeLayout(False)
        Me.tscMain.BottomToolStripPanel.PerformLayout()
        Me.tscMain.ContentPanel.ResumeLayout(False)
        Me.tscMain.TopToolStripPanel.ResumeLayout(False)
        Me.tscMain.TopToolStripPanel.PerformLayout()
        Me.tscMain.ResumeLayout(False)
        Me.tscMain.PerformLayout()
        Me.sStripEOC.ResumeLayout(False)
        Me.sStripEOC.PerformLayout()
        Me.sStripEoc2.ResumeLayout(False)
        Me.sStripEoc2.PerformLayout()
        Me.sStripMessage.ResumeLayout(False)
        Me.sStripMessage.PerformLayout()
        Me.tscLog.ContentPanel.ResumeLayout(False)
        Me.tscLog.TopToolStripPanel.ResumeLayout(False)
        Me.tscLog.TopToolStripPanel.PerformLayout()
        Me.tscLog.ResumeLayout(False)
        Me.tscLog.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PRCG As System.Windows.Forms.ColumnHeader
    Friend WithEvents Downloaded As System.Windows.Forms.ColumnHeader
    Friend WithEvents Submitted As System.Windows.Forms.ColumnHeader
    Friend WithEvents WorkServer As System.Windows.Forms.ColumnHeader
    Friend WithEvents FahCore As System.Windows.Forms.ColumnHeader
    Friend WithEvents Hardware As System.Windows.Forms.ColumnHeader
    Friend WithEvents Slot As System.Windows.Forms.ColumnHeader
    Friend WithEvents Client As System.Windows.Forms.ColumnHeader
    Friend WithEvents DetailedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents lvWU As System.Windows.Forms.ListView
    Friend WithEvents FahCoreVersion As System.Windows.Forms.ColumnHeader
    Friend WithEvents CoreStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents ServerResponse As System.Windows.Forms.ColumnHeader
    Friend WithEvents Credit As System.Windows.Forms.ColumnHeader
    Friend WithEvents PPD As System.Windows.Forms.ColumnHeader
    Friend WithEvents UploadSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents UploadSpeed As System.Windows.Forms.ColumnHeader
    Friend WithEvents scDetail As System.Windows.Forms.SplitContainer
    Friend WithEvents sStripStatistics As System.Windows.Forms.StatusStrip
    Friend WithEvents DefaultToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EditFiltersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsiFilters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FiltersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents msMAIN As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LicenseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GeneralToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdvancedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EOCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SignatureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents LogMessagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectColumnsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TPF As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClientsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HardwareToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sFilters As System.Windows.Forms.StatusStrip
    Friend WithEvents tscMain As System.Windows.Forms.ToolStripContainer
    Friend WithEvents LargeGraphToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tcDetail As System.Windows.Forms.TabControl
    Friend WithEvents tpWU As System.Windows.Forms.TabPage
    Friend WithEvents tpPerformance As System.Windows.Forms.TabPage
    Friend WithEvents tpProjects As System.Windows.Forms.TabPage
    Friend WithEvents tpHardware As System.Windows.Forms.TabPage
    Friend WithEvents rtWU As System.Windows.Forms.RichTextBox
    Friend WithEvents rtPerformance As System.Windows.Forms.RichTextBox
    Friend WithEvents rtHW As System.Windows.Forms.RichTextBox
    Friend WithEvents DoUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DownloadSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents DownloadSpeed As System.Windows.Forms.ColumnHeader
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LiveMonitoringToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MinimizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tShow As System.Windows.Forms.Timer
    Friend WithEvents tsProjects As System.Windows.Forms.ToolStrip
    Friend WithEvents tsProjects_cmbProjects As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsProjects_cmbRCG As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents rtProjects As System.Windows.Forms.RichTextBox
    Friend WithEvents scProjects As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsStatistics As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents zgProject As ZedGraph.ZedGraphControl
    Friend WithEvents GraphOptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sStripEOC As System.Windows.Forms.StatusStrip
    Friend WithEvents sStripMessage As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents sStripEoc2 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel3 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewUserStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewTeamStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenSiteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TeamStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UserStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tscHistory As System.Windows.Forms.ToolStripContainer
    Friend WithEvents tsHistory As System.Windows.Forms.ToolStrip
    Friend WithEvents tsHQFClient As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsHQFHardware As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tsHQFProject As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsHQFTimeLimit As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel4 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsCmdClearSort As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsSepClear As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsTimeLimitLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmDetailWU As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmDetails As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsTools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemtestG80ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MemtestCLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StressCPUV2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HowDoIEnableTheseToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectStatisticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OverallToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents WhatsTheDifferenceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DiagnosticsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectInfoListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectInfoCalculatorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tscLog As System.Windows.Forms.ToolStripContainer
    Friend WithEvents tsLog As System.Windows.Forms.ToolStrip
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
End Class
