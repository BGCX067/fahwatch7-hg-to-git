﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPBList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPBList))
        Me.txtDescription = New System.Windows.Forms.RichTextBox()
        Me.cmdPpdGraph = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdImport = New System.Windows.Forms.Button()
        Me.cmdEditProject = New System.Windows.Forms.Button()
        Me.chkDelay = New System.Windows.Forms.CheckBox()
        Me.cmdResetTPF = New System.Windows.Forms.Button()
        Me.lbMissingProjects = New System.Windows.Forms.ListBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtTpf3 = New System.Windows.Forms.TextBox()
        Me.txtTpf2 = New System.Windows.Forms.TextBox()
        Me.cmdAddProject = New System.Windows.Forms.Button()
        Me.txtTpf = New System.Windows.Forms.TextBox()
        Me.cmdDeleteProject = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdNewUrl = New System.Windows.Forms.Button()
        Me.cmdUpdateSummary = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.tslblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scTwo = New System.Windows.Forms.SplitContainer()
        Me.scThree = New System.Windows.Forms.SplitContainer()
        Me.lvProjects = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdToggleDescriptions = New System.Windows.Forms.Button()
        Me.gbPpdCalc = New System.Windows.Forms.GroupBox()
        Me.txtDelay3 = New System.Windows.Forms.TextBox()
        Me.txtDelay2 = New System.Windows.Forms.TextBox()
        Me.txtPPD = New System.Windows.Forms.TextBox()
        Me.txtDelay = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.gbProjects = New System.Windows.Forms.GroupBox()
        Me.txtFindProject = New System.Windows.Forms.TextBox()
        Me.gbPsummary = New System.Windows.Forms.GroupBox()
        Me.cmdSetDefaultSummary = New System.Windows.Forms.Button()
        Me.cmdRemoveURL = New System.Windows.Forms.Button()
        Me.cmdURLOpenBrowser = New System.Windows.Forms.Button()
        Me.lbpSummary = New System.Windows.Forms.ListBox()
        Me.txtSummaryURL = New System.Windows.Forms.TextBox()
        Me.tCleanStatus = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.scTwo.Panel1.SuspendLayout()
        Me.scTwo.Panel2.SuspendLayout()
        Me.scTwo.SuspendLayout()
        Me.scThree.Panel1.SuspendLayout()
        Me.scThree.Panel2.SuspendLayout()
        Me.scThree.SuspendLayout()
        Me.gbPpdCalc.SuspendLayout()
        Me.gbProjects.SuspendLayout()
        Me.gbPsummary.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDescription
        '
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Info
        Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.InfoText
        Me.txtDescription.Location = New System.Drawing.Point(0, 0)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(1190, 155)
        Me.txtDescription.TabIndex = 1
        Me.txtDescription.TabStop = False
        Me.txtDescription.Text = ""
        '
        'cmdPpdGraph
        '
        Me.cmdPpdGraph.Location = New System.Drawing.Point(6, 145)
        Me.cmdPpdGraph.Name = "cmdPpdGraph"
        Me.cmdPpdGraph.Size = New System.Drawing.Size(136, 23)
        Me.cmdPpdGraph.TabIndex = 102
        Me.cmdPpdGraph.Text = "Graphic calculator"
        Me.cmdPpdGraph.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Missing projects"
        '
        'cmdImport
        '
        Me.cmdImport.Location = New System.Drawing.Point(10, 147)
        Me.cmdImport.Name = "cmdImport"
        Me.cmdImport.Size = New System.Drawing.Size(136, 23)
        Me.cmdImport.TabIndex = 14
        Me.cmdImport.Text = "Import descriptions"
        Me.cmdImport.UseVisualStyleBackColor = True
        '
        'cmdEditProject
        '
        Me.cmdEditProject.Location = New System.Drawing.Point(10, 52)
        Me.cmdEditProject.Name = "cmdEditProject"
        Me.cmdEditProject.Size = New System.Drawing.Size(136, 23)
        Me.cmdEditProject.TabIndex = 18
        Me.cmdEditProject.Text = "Edit project"
        Me.cmdEditProject.UseVisualStyleBackColor = True
        '
        'chkDelay
        '
        Me.chkDelay.AutoSize = True
        Me.chkDelay.Location = New System.Drawing.Point(6, 56)
        Me.chkDelay.Name = "chkDelay"
        Me.chkDelay.Size = New System.Drawing.Size(53, 17)
        Me.chkDelay.TabIndex = 101
        Me.chkDelay.Text = "Delay"
        Me.chkDelay.UseVisualStyleBackColor = True
        '
        'cmdResetTPF
        '
        Me.cmdResetTPF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.cmdResetTPF.Location = New System.Drawing.Point(6, 19)
        Me.cmdResetTPF.Name = "cmdResetTPF"
        Me.cmdResetTPF.Size = New System.Drawing.Size(136, 23)
        Me.cmdResetTPF.TabIndex = 13
        Me.cmdResetTPF.Text = "Reset"
        Me.cmdResetTPF.UseVisualStyleBackColor = True
        '
        'lbMissingProjects
        '
        Me.lbMissingProjects.FormattingEnabled = True
        Me.lbMissingProjects.Location = New System.Drawing.Point(10, 48)
        Me.lbMissingProjects.Name = "lbMissingProjects"
        Me.lbMissingProjects.Size = New System.Drawing.Size(120, 121)
        Me.lbMissingProjects.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lbMissingProjects)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(152, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(142, 179)
        Me.GroupBox1.TabIndex = 19
        Me.GroupBox1.TabStop = False
        '
        'txtTpf3
        '
        Me.txtTpf3.Location = New System.Drawing.Point(120, 87)
        Me.txtTpf3.MaxLength = 2
        Me.txtTpf3.Name = "txtTpf3"
        Me.txtTpf3.Size = New System.Drawing.Size(22, 20)
        Me.txtTpf3.TabIndex = 12
        Me.txtTpf3.Text = "ss"
        '
        'txtTpf2
        '
        Me.txtTpf2.Location = New System.Drawing.Point(89, 86)
        Me.txtTpf2.MaxLength = 2
        Me.txtTpf2.Name = "txtTpf2"
        Me.txtTpf2.Size = New System.Drawing.Size(22, 20)
        Me.txtTpf2.TabIndex = 11
        Me.txtTpf2.Text = "mm"
        '
        'cmdAddProject
        '
        Me.cmdAddProject.Location = New System.Drawing.Point(10, 117)
        Me.cmdAddProject.Name = "cmdAddProject"
        Me.cmdAddProject.Size = New System.Drawing.Size(136, 23)
        Me.cmdAddProject.TabIndex = 6
        Me.cmdAddProject.Text = "New project"
        Me.cmdAddProject.UseVisualStyleBackColor = True
        '
        'txtTpf
        '
        Me.txtTpf.Location = New System.Drawing.Point(58, 86)
        Me.txtTpf.MaxLength = 2
        Me.txtTpf.Name = "txtTpf"
        Me.txtTpf.Size = New System.Drawing.Size(22, 20)
        Me.txtTpf.TabIndex = 10
        Me.txtTpf.Text = "hh"
        '
        'cmdDeleteProject
        '
        Me.cmdDeleteProject.Location = New System.Drawing.Point(10, 85)
        Me.cmdDeleteProject.Name = "cmdDeleteProject"
        Me.cmdDeleteProject.Size = New System.Drawing.Size(136, 23)
        Me.cmdDeleteProject.TabIndex = 5
        Me.cmdDeleteProject.Text = "Remove project"
        Me.cmdDeleteProject.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Quick find"
        '
        'cmdNewUrl
        '
        Me.cmdNewUrl.Location = New System.Drawing.Point(322, 147)
        Me.cmdNewUrl.Name = "cmdNewUrl"
        Me.cmdNewUrl.Size = New System.Drawing.Size(101, 23)
        Me.cmdNewUrl.TabIndex = 9
        Me.cmdNewUrl.Text = "New URL"
        Me.cmdNewUrl.UseVisualStyleBackColor = True
        '
        'cmdUpdateSummary
        '
        Me.cmdUpdateSummary.Location = New System.Drawing.Point(322, 19)
        Me.cmdUpdateSummary.Name = "cmdUpdateSummary"
        Me.cmdUpdateSummary.Size = New System.Drawing.Size(101, 23)
        Me.cmdUpdateSummary.TabIndex = 8
        Me.cmdUpdateSummary.Text = "Update"
        Me.cmdUpdateSummary.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(1045, 126)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(133, 45)
        Me.cmdClose.TabIndex = 31
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.StatusStrip1)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.AutoScroll = True
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.scMain)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1190, 559)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1190, 581)
        Me.ToolStripContainer1.TabIndex = 33
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslblStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1190, 22)
        Me.StatusStrip1.TabIndex = 0
        '
        'tslblStatus
        '
        Me.tslblStatus.Name = "tslblStatus"
        Me.tslblStatus.Size = New System.Drawing.Size(0, 17)
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scMain.IsSplitterFixed = True
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.scTwo)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.cmdClose)
        Me.scMain.Panel2.Controls.Add(Me.gbPpdCalc)
        Me.scMain.Panel2.Controls.Add(Me.gbProjects)
        Me.scMain.Panel2.Controls.Add(Me.gbPsummary)
        Me.scMain.Size = New System.Drawing.Size(1190, 559)
        Me.scMain.SplitterDistance = 375
        Me.scMain.SplitterWidth = 1
        Me.scMain.TabIndex = 0
        '
        'scTwo
        '
        Me.scTwo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scTwo.Location = New System.Drawing.Point(0, 0)
        Me.scTwo.Name = "scTwo"
        Me.scTwo.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scTwo.Panel1
        '
        Me.scTwo.Panel1.Controls.Add(Me.scThree)
        '
        'scTwo.Panel2
        '
        Me.scTwo.Panel2.Controls.Add(Me.txtDescription)
        Me.scTwo.Size = New System.Drawing.Size(1190, 375)
        Me.scTwo.SplitterDistance = 216
        Me.scTwo.TabIndex = 0
        '
        'scThree
        '
        Me.scThree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scThree.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scThree.IsSplitterFixed = True
        Me.scThree.Location = New System.Drawing.Point(0, 0)
        Me.scThree.Name = "scThree"
        Me.scThree.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scThree.Panel1
        '
        Me.scThree.Panel1.Controls.Add(Me.lvProjects)
        '
        'scThree.Panel2
        '
        Me.scThree.Panel2.Controls.Add(Me.cmdToggleDescriptions)
        Me.scThree.Panel2MinSize = 10
        Me.scThree.Size = New System.Drawing.Size(1190, 216)
        Me.scThree.SplitterDistance = 205
        Me.scThree.SplitterWidth = 1
        Me.scThree.TabIndex = 0
        '
        'lvProjects
        '
        Me.lvProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11})
        Me.lvProjects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvProjects.FullRowSelect = True
        Me.lvProjects.GridLines = True
        Me.lvProjects.HideSelection = False
        Me.lvProjects.Location = New System.Drawing.Point(0, 0)
        Me.lvProjects.MultiSelect = False
        Me.lvProjects.Name = "lvProjects"
        Me.lvProjects.Size = New System.Drawing.Size(1190, 205)
        Me.lvProjects.TabIndex = 3
        Me.lvProjects.UseCompatibleStateImageBehavior = False
        Me.lvProjects.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Projectnumber"
        Me.ColumnHeader1.Width = 84
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Server IP"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Work Unit Name"
        Me.ColumnHeader3.Width = 98
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Number of Atoms"
        Me.ColumnHeader4.Width = 102
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Preferred days"
        Me.ColumnHeader5.Width = 92
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Final deadline"
        Me.ColumnHeader6.Width = 84
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Credit"
        Me.ColumnHeader7.Width = 40
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Frames"
        Me.ColumnHeader8.Width = 47
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Code"
        Me.ColumnHeader9.Width = 39
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Contact"
        Me.ColumnHeader10.Width = 49
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "kFactor"
        Me.ColumnHeader11.Width = 50
        '
        'cmdToggleDescriptions
        '
        Me.cmdToggleDescriptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdToggleDescriptions.Location = New System.Drawing.Point(0, 0)
        Me.cmdToggleDescriptions.Name = "cmdToggleDescriptions"
        Me.cmdToggleDescriptions.Size = New System.Drawing.Size(1190, 10)
        Me.cmdToggleDescriptions.TabIndex = 0
        Me.cmdToggleDescriptions.UseVisualStyleBackColor = True
        '
        'gbPpdCalc
        '
        Me.gbPpdCalc.Controls.Add(Me.cmdPpdGraph)
        Me.gbPpdCalc.Controls.Add(Me.chkDelay)
        Me.gbPpdCalc.Controls.Add(Me.cmdResetTPF)
        Me.gbPpdCalc.Controls.Add(Me.txtTpf3)
        Me.gbPpdCalc.Controls.Add(Me.txtTpf2)
        Me.gbPpdCalc.Controls.Add(Me.txtTpf)
        Me.gbPpdCalc.Controls.Add(Me.txtDelay3)
        Me.gbPpdCalc.Controls.Add(Me.txtDelay2)
        Me.gbPpdCalc.Controls.Add(Me.txtPPD)
        Me.gbPpdCalc.Controls.Add(Me.txtDelay)
        Me.gbPpdCalc.Controls.Add(Me.Label3)
        Me.gbPpdCalc.Location = New System.Drawing.Point(741, 5)
        Me.gbPpdCalc.Name = "gbPpdCalc"
        Me.gbPpdCalc.Size = New System.Drawing.Size(153, 179)
        Me.gbPpdCalc.TabIndex = 28
        Me.gbPpdCalc.TabStop = False
        Me.gbPpdCalc.Text = "PPD calculator"
        '
        'txtDelay3
        '
        Me.txtDelay3.Location = New System.Drawing.Point(120, 54)
        Me.txtDelay3.MaxLength = 2
        Me.txtDelay3.Name = "txtDelay3"
        Me.txtDelay3.Size = New System.Drawing.Size(22, 20)
        Me.txtDelay3.TabIndex = 9
        Me.txtDelay3.Text = "ss"
        '
        'txtDelay2
        '
        Me.txtDelay2.Location = New System.Drawing.Point(89, 53)
        Me.txtDelay2.MaxLength = 2
        Me.txtDelay2.Name = "txtDelay2"
        Me.txtDelay2.Size = New System.Drawing.Size(22, 20)
        Me.txtDelay2.TabIndex = 8
        Me.txtDelay2.Text = "mm"
        '
        'txtPPD
        '
        Me.txtPPD.Location = New System.Drawing.Point(6, 119)
        Me.txtPPD.Name = "txtPPD"
        Me.txtPPD.ReadOnly = True
        Me.txtPPD.Size = New System.Drawing.Size(136, 20)
        Me.txtPPD.TabIndex = 100
        Me.txtPPD.TabStop = False
        Me.txtPPD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDelay
        '
        Me.txtDelay.Location = New System.Drawing.Point(58, 53)
        Me.txtDelay.MaxLength = 2
        Me.txtDelay.Name = "txtDelay"
        Me.txtDelay.Size = New System.Drawing.Size(22, 20)
        Me.txtDelay.TabIndex = 7
        Me.txtDelay.Text = "hh"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(23, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Tpf"
        '
        'gbProjects
        '
        Me.gbProjects.Controls.Add(Me.GroupBox1)
        Me.gbProjects.Controls.Add(Me.cmdImport)
        Me.gbProjects.Controls.Add(Me.cmdEditProject)
        Me.gbProjects.Controls.Add(Me.cmdAddProject)
        Me.gbProjects.Controls.Add(Me.cmdDeleteProject)
        Me.gbProjects.Controls.Add(Me.Label1)
        Me.gbProjects.Controls.Add(Me.txtFindProject)
        Me.gbProjects.Location = New System.Drawing.Point(3, 5)
        Me.gbProjects.Name = "gbProjects"
        Me.gbProjects.Size = New System.Drawing.Size(295, 179)
        Me.gbProjects.TabIndex = 30
        Me.gbProjects.TabStop = False
        Me.gbProjects.Text = "Projects"
        '
        'txtFindProject
        '
        Me.txtFindProject.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtFindProject.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtFindProject.Location = New System.Drawing.Point(71, 20)
        Me.txtFindProject.Name = "txtFindProject"
        Me.txtFindProject.Size = New System.Drawing.Size(75, 20)
        Me.txtFindProject.TabIndex = 4
        '
        'gbPsummary
        '
        Me.gbPsummary.Controls.Add(Me.cmdNewUrl)
        Me.gbPsummary.Controls.Add(Me.cmdUpdateSummary)
        Me.gbPsummary.Controls.Add(Me.cmdSetDefaultSummary)
        Me.gbPsummary.Controls.Add(Me.cmdRemoveURL)
        Me.gbPsummary.Controls.Add(Me.cmdURLOpenBrowser)
        Me.gbPsummary.Controls.Add(Me.lbpSummary)
        Me.gbPsummary.Controls.Add(Me.txtSummaryURL)
        Me.gbPsummary.Location = New System.Drawing.Point(301, 5)
        Me.gbPsummary.Name = "gbPsummary"
        Me.gbPsummary.Size = New System.Drawing.Size(434, 179)
        Me.gbPsummary.TabIndex = 29
        Me.gbPsummary.TabStop = False
        Me.gbPsummary.Text = "pSummary"
        '
        'cmdSetDefaultSummary
        '
        Me.cmdSetDefaultSummary.Location = New System.Drawing.Point(322, 117)
        Me.cmdSetDefaultSummary.Name = "cmdSetDefaultSummary"
        Me.cmdSetDefaultSummary.Size = New System.Drawing.Size(101, 23)
        Me.cmdSetDefaultSummary.TabIndex = 7
        Me.cmdSetDefaultSummary.Text = "Set default"
        Me.cmdSetDefaultSummary.UseVisualStyleBackColor = True
        '
        'cmdRemoveURL
        '
        Me.cmdRemoveURL.Location = New System.Drawing.Point(322, 85)
        Me.cmdRemoveURL.Name = "cmdRemoveURL"
        Me.cmdRemoveURL.Size = New System.Drawing.Size(101, 23)
        Me.cmdRemoveURL.TabIndex = 5
        Me.cmdRemoveURL.Text = "Remove URL"
        Me.cmdRemoveURL.UseVisualStyleBackColor = True
        '
        'cmdURLOpenBrowser
        '
        Me.cmdURLOpenBrowser.Location = New System.Drawing.Point(322, 52)
        Me.cmdURLOpenBrowser.Name = "cmdURLOpenBrowser"
        Me.cmdURLOpenBrowser.Size = New System.Drawing.Size(101, 23)
        Me.cmdURLOpenBrowser.TabIndex = 4
        Me.cmdURLOpenBrowser.Text = "Open in browser"
        Me.cmdURLOpenBrowser.UseVisualStyleBackColor = True
        '
        'lbpSummary
        '
        Me.lbpSummary.FormattingEnabled = True
        Me.lbpSummary.Location = New System.Drawing.Point(6, 19)
        Me.lbpSummary.Name = "lbpSummary"
        Me.lbpSummary.Size = New System.Drawing.Size(309, 121)
        Me.lbpSummary.TabIndex = 3
        '
        'txtSummaryURL
        '
        Me.txtSummaryURL.Location = New System.Drawing.Point(6, 149)
        Me.txtSummaryURL.Name = "txtSummaryURL"
        Me.txtSummaryURL.Size = New System.Drawing.Size(309, 20)
        Me.txtSummaryURL.TabIndex = 2
        '
        'tCleanStatus
        '
        '
        'frmPBList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1190, 581)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmPBList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Project information"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.ResumeLayout(False)
        Me.scTwo.Panel1.ResumeLayout(False)
        Me.scTwo.Panel2.ResumeLayout(False)
        Me.scTwo.ResumeLayout(False)
        Me.scThree.Panel1.ResumeLayout(False)
        Me.scThree.Panel2.ResumeLayout(False)
        Me.scThree.ResumeLayout(False)
        Me.gbPpdCalc.ResumeLayout(False)
        Me.gbPpdCalc.PerformLayout()
        Me.gbProjects.ResumeLayout(False)
        Me.gbProjects.PerformLayout()
        Me.gbPsummary.ResumeLayout(False)
        Me.gbPsummary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents cmdPpdGraph As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdImport As System.Windows.Forms.Button
    Friend WithEvents cmdEditProject As System.Windows.Forms.Button
    Friend WithEvents chkDelay As System.Windows.Forms.CheckBox
    Friend WithEvents cmdResetTPF As System.Windows.Forms.Button
    Friend WithEvents lbMissingProjects As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtTpf3 As System.Windows.Forms.TextBox
    Friend WithEvents txtTpf2 As System.Windows.Forms.TextBox
    Friend WithEvents cmdAddProject As System.Windows.Forms.Button
    Friend WithEvents txtTpf As System.Windows.Forms.TextBox
    Friend WithEvents cmdDeleteProject As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdNewUrl As System.Windows.Forms.Button
    Friend WithEvents cmdUpdateSummary As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents scTwo As System.Windows.Forms.SplitContainer
    Friend WithEvents scThree As System.Windows.Forms.SplitContainer
    Friend WithEvents lvProjects As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdToggleDescriptions As System.Windows.Forms.Button
    Friend WithEvents gbPpdCalc As System.Windows.Forms.GroupBox
    Friend WithEvents txtDelay3 As System.Windows.Forms.TextBox
    Friend WithEvents txtDelay2 As System.Windows.Forms.TextBox
    Friend WithEvents txtPPD As System.Windows.Forms.TextBox
    Friend WithEvents txtDelay As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents gbProjects As System.Windows.Forms.GroupBox
    Friend WithEvents txtFindProject As System.Windows.Forms.TextBox
    Friend WithEvents gbPsummary As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSetDefaultSummary As System.Windows.Forms.Button
    Friend WithEvents cmdRemoveURL As System.Windows.Forms.Button
    Friend WithEvents cmdURLOpenBrowser As System.Windows.Forms.Button
    Friend WithEvents lbpSummary As System.Windows.Forms.ListBox
    Friend WithEvents txtSummaryURL As System.Windows.Forms.TextBox
    Friend WithEvents tCleanStatus As System.Windows.Forms.Timer
    Friend WithEvents tslblStatus As System.Windows.Forms.ToolStripStatusLabel
End Class
