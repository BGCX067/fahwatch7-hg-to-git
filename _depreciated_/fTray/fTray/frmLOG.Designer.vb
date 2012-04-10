<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLOG
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLOG))
        Me.tClock = New System.Windows.Forms.Timer(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmExit = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitFTrayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblWuName = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.lblKfactor = New System.Windows.Forms.Label()
        Me.lblWUmax = New System.Windows.Forms.Label()
        Me.lblWUPref = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.lblAtoms = New System.Windows.Forms.Label()
        Me.lblWUCORE = New System.Windows.Forms.Label()
        Me.lblCredit = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.LinkLabel()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmdConfigure = New System.Windows.Forms.Button()
        Me.cmdStop = New System.Windows.Forms.Button()
        Me.cmStop = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StopAfterUnitCompletesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdStart = New System.Windows.Forms.Button()
        Me.tsCont = New System.Windows.Forms.ToolStripContainer()
        Me.tsBottom = New System.Windows.Forms.ToolStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripLabel()
        Me.tsBottomDropDown = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsBottomShowHide = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSepShowHide = New System.Windows.Forms.ToolStripSeparator()
        Me.tsShowDebug = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewClientFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenLogfileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsBottomAdvancedOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ProjectBrowserwebToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsMenuItemPB = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsBottomStatistics = New System.Windows.Forms.ToolStripDropDownButton()
        Me.EOCSignatureImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsEOCXMLStatistics = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsEocWebStatistics = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsStanfordUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsThisClient = New System.Windows.Forms.ToolStripMenuItem()
        Me.rtf = New System.Windows.Forms.RichTextBox()
        Me.tsTop = New System.Windows.Forms.ToolStrip()
        Me.pBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.lblProject = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.lblFrameTime = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.lblEta = New System.Windows.Forms.ToolStripLabel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblTUF = New System.Windows.Forms.Label()
        Me.lblUR = New System.Windows.Forms.Label()
        Me.lblDR = New System.Windows.Forms.Label()
        Me.lblPerfFraction = New System.Windows.Forms.Label()
        Me.lblQVersion = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblExpires = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.lblPreff = New System.Windows.Forms.Label()
        Me.lblIssued = New System.Windows.Forms.Label()
        Me.lblUF = New System.Windows.Forms.Label()
        Me.lblBench = New System.Windows.Forms.Label()
        Me.lblCore = New System.Windows.Forms.Label()
        Me.lblUS = New System.Windows.Forms.Label()
        Me.lblET = New System.Windows.Forms.Label()
        Me.lblBT = New System.Windows.Forms.Label()
        Me.lblPK = New System.Windows.Forms.Label()
        Me.lblTeam = New System.Windows.Forms.Label()
        Me.lblUser = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblBident = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbQslots = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StopToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ConfigureToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseFTrayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.nIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.cmExit.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.cmStop.SuspendLayout()
        Me.tsCont.BottomToolStripPanel.SuspendLayout()
        Me.tsCont.ContentPanel.SuspendLayout()
        Me.tsCont.TopToolStripPanel.SuspendLayout()
        Me.tsCont.SuspendLayout()
        Me.tsBottom.SuspendLayout()
        Me.tsTop.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.cMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'tClock
        '
        Me.tClock.Interval = 1000
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.MinimumSize = New System.Drawing.Size(0, 50)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 50)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.MinimumSize = New System.Drawing.Size(0, 35)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 35)
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
        Me.ContentPanel.Size = New System.Drawing.Size(899, 349)
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.tsCont)
        Me.SplitContainer1.Panel1MinSize = 0
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Button1)
        Me.SplitContainer1.Panel2MinSize = 0
        Me.SplitContainer1.Size = New System.Drawing.Size(1048, 451)
        Me.SplitContainer1.SplitterDistance = 720
        Me.SplitContainer1.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmdClose)
        Me.GroupBox3.Controls.Add(Me.GroupBox4)
        Me.GroupBox3.Controls.Add(Me.cmdConfigure)
        Me.GroupBox3.Controls.Add(Me.cmdStop)
        Me.GroupBox3.Controls.Add(Me.cmdStart)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 278)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(714, 168)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        '
        'cmdClose
        '
        Me.cmdClose.ContextMenuStrip = Me.cmExit
        Me.cmdClose.Location = New System.Drawing.Point(7, 133)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(95, 32)
        Me.cmdClose.TabIndex = 4
        Me.cmdClose.Text = "Hide"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmExit
        '
        Me.cmExit.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitFTrayToolStripMenuItem})
        Me.cmExit.Name = "cmExit"
        Me.cmExit.Size = New System.Drawing.Size(140, 28)
        '
        'ExitFTrayToolStripMenuItem
        '
        Me.ExitFTrayToolStripMenuItem.Name = "ExitFTrayToolStripMenuItem"
        Me.ExitFTrayToolStripMenuItem.Size = New System.Drawing.Size(139, 24)
        Me.ExitFTrayToolStripMenuItem.Text = "Exit fTray"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblWuName)
        Me.GroupBox4.Controls.Add(Me.Label22)
        Me.GroupBox4.Controls.Add(Me.lblKfactor)
        Me.GroupBox4.Controls.Add(Me.lblWUmax)
        Me.GroupBox4.Controls.Add(Me.lblWUPref)
        Me.GroupBox4.Controls.Add(Me.lblServer)
        Me.GroupBox4.Controls.Add(Me.lblAtoms)
        Me.GroupBox4.Controls.Add(Me.lblWUCORE)
        Me.GroupBox4.Controls.Add(Me.lblCredit)
        Me.GroupBox4.Controls.Add(Me.lblDescription)
        Me.GroupBox4.Controls.Add(Me.Label26)
        Me.GroupBox4.Controls.Add(Me.Label25)
        Me.GroupBox4.Controls.Add(Me.Label24)
        Me.GroupBox4.Controls.Add(Me.Label23)
        Me.GroupBox4.Controls.Add(Me.Label21)
        Me.GroupBox4.Controls.Add(Me.Label20)
        Me.GroupBox4.Controls.Add(Me.Label18)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Location = New System.Drawing.Point(108, 18)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(600, 149)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Protein"
        '
        'lblWuName
        '
        Me.lblWuName.AutoEllipsis = True
        Me.lblWuName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWuName.Location = New System.Drawing.Point(75, 28)
        Me.lblWuName.Name = "lblWuName"
        Me.lblWuName.Size = New System.Drawing.Size(290, 23)
        Me.lblWuName.TabIndex = 18
        Me.lblWuName.Text = "-"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(6, 128)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(79, 17)
        Me.Label22.TabIndex = 17
        Me.Label22.Text = "Description"
        '
        'lblKfactor
        '
        Me.lblKfactor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKfactor.Location = New System.Drawing.Point(474, 100)
        Me.lblKfactor.Name = "lblKfactor"
        Me.lblKfactor.Size = New System.Drawing.Size(120, 23)
        Me.lblKfactor.TabIndex = 16
        Me.lblKfactor.Text = "-"
        '
        'lblWUmax
        '
        Me.lblWUmax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWUmax.Location = New System.Drawing.Point(474, 76)
        Me.lblWUmax.Name = "lblWUmax"
        Me.lblWUmax.Size = New System.Drawing.Size(120, 23)
        Me.lblWUmax.TabIndex = 15
        Me.lblWUmax.Text = "-"
        '
        'lblWUPref
        '
        Me.lblWUPref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWUPref.Location = New System.Drawing.Point(474, 53)
        Me.lblWUPref.Name = "lblWUPref"
        Me.lblWUPref.Size = New System.Drawing.Size(120, 23)
        Me.lblWUPref.TabIndex = 14
        Me.lblWUPref.Text = "-"
        '
        'lblServer
        '
        Me.lblServer.AutoEllipsis = True
        Me.lblServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblServer.Location = New System.Drawing.Point(474, 28)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(120, 23)
        Me.lblServer.TabIndex = 13
        Me.lblServer.Text = "-"
        '
        'lblAtoms
        '
        Me.lblAtoms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblAtoms.Location = New System.Drawing.Point(75, 100)
        Me.lblAtoms.Name = "lblAtoms"
        Me.lblAtoms.Size = New System.Drawing.Size(290, 23)
        Me.lblAtoms.TabIndex = 12
        Me.lblAtoms.Text = "-"
        '
        'lblWUCORE
        '
        Me.lblWUCORE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblWUCORE.Location = New System.Drawing.Point(75, 76)
        Me.lblWUCORE.Name = "lblWUCORE"
        Me.lblWUCORE.Size = New System.Drawing.Size(290, 23)
        Me.lblWUCORE.TabIndex = 11
        Me.lblWUCORE.Text = "-"
        '
        'lblCredit
        '
        Me.lblCredit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCredit.Location = New System.Drawing.Point(75, 53)
        Me.lblCredit.Name = "lblCredit"
        Me.lblCredit.Size = New System.Drawing.Size(290, 23)
        Me.lblCredit.TabIndex = 10
        Me.lblCredit.Text = "-"
        '
        'lblDescription
        '
        Me.lblDescription.AutoEllipsis = True
        Me.lblDescription.Location = New System.Drawing.Point(94, 128)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(500, 23)
        Me.lblDescription.TabIndex = 9
        Me.lblDescription.TabStop = True
        Me.lblDescription.Text = "overusingIPswillbebaned"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(371, 29)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(50, 17)
        Me.Label26.TabIndex = 8
        Me.Label26.Text = "Server"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(371, 77)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(100, 17)
        Me.Label25.TabIndex = 7
        Me.Label25.Text = "Maximum days"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(371, 54)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(102, 17)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "Preferred days"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(371, 101)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(57, 17)
        Me.Label23.TabIndex = 5
        Me.Label23.Text = "K factor"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(6, 77)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(38, 17)
        Me.Label21.TabIndex = 3
        Me.Label21.Text = "Core"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 54)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(45, 17)
        Me.Label20.TabIndex = 2
        Me.Label20.Text = "Credit"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 101)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(68, 17)
        Me.Label18.TabIndex = 1
        Me.Label18.Text = "No atoms"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 29)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(45, 17)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Name"
        '
        'cmdConfigure
        '
        Me.cmdConfigure.Location = New System.Drawing.Point(7, 95)
        Me.cmdConfigure.Name = "cmdConfigure"
        Me.cmdConfigure.Size = New System.Drawing.Size(95, 32)
        Me.cmdConfigure.TabIndex = 2
        Me.cmdConfigure.Text = "Configure"
        Me.cmdConfigure.UseVisualStyleBackColor = True
        '
        'cmdStop
        '
        Me.cmdStop.ContextMenuStrip = Me.cmStop
        Me.cmdStop.Location = New System.Drawing.Point(7, 57)
        Me.cmdStop.Name = "cmdStop"
        Me.cmdStop.Size = New System.Drawing.Size(95, 32)
        Me.cmdStop.TabIndex = 1
        Me.cmdStop.Text = "Stop Client"
        Me.cmdStop.UseVisualStyleBackColor = True
        '
        'cmStop
        '
        Me.cmStop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StopAfterUnitCompletesToolStripMenuItem})
        Me.cmStop.Name = "cmStop"
        Me.cmStop.Size = New System.Drawing.Size(247, 28)
        '
        'StopAfterUnitCompletesToolStripMenuItem
        '
        Me.StopAfterUnitCompletesToolStripMenuItem.Name = "StopAfterUnitCompletesToolStripMenuItem"
        Me.StopAfterUnitCompletesToolStripMenuItem.Size = New System.Drawing.Size(246, 24)
        Me.StopAfterUnitCompletesToolStripMenuItem.Text = "Stop after unit completes"
        '
        'cmdStart
        '
        Me.cmdStart.Location = New System.Drawing.Point(7, 19)
        Me.cmdStart.Name = "cmdStart"
        Me.cmdStart.Size = New System.Drawing.Size(95, 32)
        Me.cmdStart.TabIndex = 0
        Me.cmdStart.Text = "Start client"
        Me.cmdStart.UseVisualStyleBackColor = True
        '
        'tsCont
        '
        '
        'tsCont.BottomToolStripPanel
        '
        Me.tsCont.BottomToolStripPanel.Controls.Add(Me.tsBottom)
        '
        'tsCont.ContentPanel
        '
        Me.tsCont.ContentPanel.Controls.Add(Me.rtf)
        Me.tsCont.ContentPanel.Size = New System.Drawing.Size(720, 218)
        Me.tsCont.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tsCont.Location = New System.Drawing.Point(0, 0)
        Me.tsCont.MaximumSize = New System.Drawing.Size(0, 280)
        Me.tsCont.Name = "tsCont"
        Me.tsCont.Size = New System.Drawing.Size(720, 280)
        Me.tsCont.TabIndex = 3
        Me.tsCont.Text = "ToolStripContainer1"
        '
        'tsCont.TopToolStripPanel
        '
        Me.tsCont.TopToolStripPanel.Controls.Add(Me.tsTop)
        Me.tsCont.TopToolStripPanel.MinimumSize = New System.Drawing.Size(0, 35)
        '
        'tsBottom
        '
        Me.tsBottom.Dock = System.Windows.Forms.DockStyle.None
        Me.tsBottom.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus, Me.tsBottomDropDown, Me.ToolStripDropDownButton1, Me.tsBottomStatistics})
        Me.tsBottom.Location = New System.Drawing.Point(0, 0)
        Me.tsBottom.Name = "tsBottom"
        Me.tsBottom.Size = New System.Drawing.Size(720, 27)
        Me.tsBottom.Stretch = True
        Me.tsBottom.TabIndex = 0
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(0, 24)
        '
        'tsBottomDropDown
        '
        Me.tsBottomDropDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsBottomDropDown.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsBottomShowHide, Me.tsSepShowHide, Me.tsShowDebug, Me.ViewClientFilesToolStripMenuItem, Me.OpenLogfileToolStripMenuItem, Me.tsBottomAdvancedOptions})
        Me.tsBottomDropDown.Image = CType(resources.GetObject("tsBottomDropDown.Image"), System.Drawing.Image)
        Me.tsBottomDropDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBottomDropDown.Name = "tsBottomDropDown"
        Me.tsBottomDropDown.Size = New System.Drawing.Size(104, 24)
        Me.tsBottomDropDown.Text = "Advanced"
        '
        'tsBottomShowHide
        '
        Me.tsBottomShowHide.Name = "tsBottomShowHide"
        Me.tsBottomShowHide.Size = New System.Drawing.Size(231, 24)
        Me.tsBottomShowHide.Text = "Show console"
        '
        'tsSepShowHide
        '
        Me.tsSepShowHide.Name = "tsSepShowHide"
        Me.tsSepShowHide.Size = New System.Drawing.Size(228, 6)
        Me.tsSepShowHide.Tag = ""
        '
        'tsShowDebug
        '
        Me.tsShowDebug.Name = "tsShowDebug"
        Me.tsShowDebug.Size = New System.Drawing.Size(231, 24)
        Me.tsShowDebug.Text = "Show Debug messages"
        '
        'ViewClientFilesToolStripMenuItem
        '
        Me.ViewClientFilesToolStripMenuItem.Name = "ViewClientFilesToolStripMenuItem"
        Me.ViewClientFilesToolStripMenuItem.Size = New System.Drawing.Size(231, 24)
        Me.ViewClientFilesToolStripMenuItem.Text = "View client files"
        '
        'OpenLogfileToolStripMenuItem
        '
        Me.OpenLogfileToolStripMenuItem.Name = "OpenLogfileToolStripMenuItem"
        Me.OpenLogfileToolStripMenuItem.Size = New System.Drawing.Size(231, 24)
        Me.OpenLogfileToolStripMenuItem.Text = "Open logfile"
        '
        'tsBottomAdvancedOptions
        '
        Me.tsBottomAdvancedOptions.Name = "tsBottomAdvancedOptions"
        Me.tsBottomAdvancedOptions.Size = New System.Drawing.Size(231, 24)
        Me.tsBottomAdvancedOptions.Text = "Options"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProjectBrowserwebToolStripMenuItem, Me.tsMenuItemPB})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(90, 24)
        Me.ToolStripDropDownButton1.Text = "Projects"
        '
        'ProjectBrowserwebToolStripMenuItem
        '
        Me.ProjectBrowserwebToolStripMenuItem.Name = "ProjectBrowserwebToolStripMenuItem"
        Me.ProjectBrowserwebToolStripMenuItem.Size = New System.Drawing.Size(227, 24)
        Me.ProjectBrowserwebToolStripMenuItem.Text = "Project browser (web)"
        '
        'tsMenuItemPB
        '
        Me.tsMenuItemPB.Name = "tsMenuItemPB"
        Me.tsMenuItemPB.Size = New System.Drawing.Size(227, 24)
        Me.tsMenuItemPB.Text = "Project browser (local)"
        '
        'tsBottomStatistics
        '
        Me.tsBottomStatistics.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsBottomStatistics.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EOCSignatureImage, Me.tsEOCXMLStatistics, Me.tsEocWebStatistics, Me.tsStanfordUser, Me.tsThisClient})
        Me.tsBottomStatistics.Image = CType(resources.GetObject("tsBottomStatistics.Image"), System.Drawing.Image)
        Me.tsBottomStatistics.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsBottomStatistics.Name = "tsBottomStatistics"
        Me.tsBottomStatistics.Size = New System.Drawing.Size(96, 24)
        Me.tsBottomStatistics.Text = "Statistics"
        '
        'EOCSignatureImage
        '
        Me.EOCSignatureImage.Name = "EOCSignatureImage"
        Me.EOCSignatureImage.Size = New System.Drawing.Size(219, 24)
        Me.EOCSignatureImage.Text = "EOC Signature image"
        '
        'tsEOCXMLStatistics
        '
        Me.tsEOCXMLStatistics.Name = "tsEOCXMLStatistics"
        Me.tsEOCXMLStatistics.Size = New System.Drawing.Size(219, 24)
        Me.tsEOCXMLStatistics.Text = "EOC XML Statistics"
        '
        'tsEocWebStatistics
        '
        Me.tsEocWebStatistics.Name = "tsEocWebStatistics"
        Me.tsEocWebStatistics.Size = New System.Drawing.Size(219, 24)
        Me.tsEocWebStatistics.Text = "EOC statistics"
        '
        'tsStanfordUser
        '
        Me.tsStanfordUser.Name = "tsStanfordUser"
        Me.tsStanfordUser.Size = New System.Drawing.Size(219, 24)
        Me.tsStanfordUser.Text = "Stanford statistics"
        '
        'tsThisClient
        '
        Me.tsThisClient.Name = "tsThisClient"
        Me.tsThisClient.Size = New System.Drawing.Size(219, 24)
        Me.tsThisClient.Text = "This client"
        '
        'rtf
        '
        Me.rtf.BackColor = System.Drawing.Color.White
        Me.rtf.Cursor = System.Windows.Forms.Cursors.Default
        Me.rtf.HideSelection = False
        Me.rtf.Location = New System.Drawing.Point(31, 22)
        Me.rtf.Name = "rtf"
        Me.rtf.ReadOnly = True
        Me.rtf.Size = New System.Drawing.Size(100, 96)
        Me.rtf.TabIndex = 0
        Me.rtf.Text = ""
        '
        'tsTop
        '
        Me.tsTop.Dock = System.Windows.Forms.DockStyle.None
        Me.tsTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsTop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.pBar, Me.ToolStripSeparator1, Me.ToolStripLabel1, Me.lblProject, Me.ToolStripSeparator2, Me.ToolStripLabel2, Me.lblFrameTime, Me.ToolStripSeparator4, Me.ToolStripLabel3, Me.lblEta})
        Me.tsTop.Location = New System.Drawing.Point(0, 0)
        Me.tsTop.MinimumSize = New System.Drawing.Size(0, 30)
        Me.tsTop.Name = "tsTop"
        Me.tsTop.Size = New System.Drawing.Size(720, 33)
        Me.tsTop.Stretch = True
        Me.tsTop.TabIndex = 0
        '
        'pBar
        '
        Me.pBar.MarqueeAnimationSpeed = 5000
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(150, 30)
        Me.pBar.Step = 1
        Me.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 33)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(58, 30)
        Me.ToolStripLabel1.Text = "Project:"
        '
        'lblProject
        '
        Me.lblProject.Name = "lblProject"
        Me.lblProject.Size = New System.Drawing.Size(55, 30)
        Me.lblProject.Text = "Project"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 33)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(35, 30)
        Me.ToolStripLabel2.Text = "TPF:"
        '
        'lblFrameTime
        '
        Me.lblFrameTime.Name = "lblFrameTime"
        Me.lblFrameTime.Size = New System.Drawing.Size(83, 30)
        Me.lblFrameTime.Text = "FrameTime"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 33)
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(38, 30)
        Me.ToolStripLabel3.Text = "ETA:"
        '
        'lblEta
        '
        Me.lblEta.Name = "lblEta"
        Me.lblEta.Size = New System.Drawing.Size(30, 30)
        Me.lblEta.Text = "Eta"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblTUF)
        Me.GroupBox1.Controls.Add(Me.lblUR)
        Me.GroupBox1.Controls.Add(Me.lblDR)
        Me.GroupBox1.Controls.Add(Me.lblPerfFraction)
        Me.GroupBox1.Controls.Add(Me.lblQVersion)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(309, 442)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Queue"
        '
        'lblTUF
        '
        Me.lblTUF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTUF.Location = New System.Drawing.Point(150, 98)
        Me.lblTUF.Name = "lblTUF"
        Me.lblTUF.Size = New System.Drawing.Size(147, 17)
        Me.lblTUF.TabIndex = 12
        Me.lblTUF.Text = "-"
        '
        'lblUR
        '
        Me.lblUR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblUR.Location = New System.Drawing.Point(150, 78)
        Me.lblUR.Name = "lblUR"
        Me.lblUR.Size = New System.Drawing.Size(147, 17)
        Me.lblUR.TabIndex = 11
        Me.lblUR.Text = "-"
        '
        'lblDR
        '
        Me.lblDR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDR.Location = New System.Drawing.Point(150, 58)
        Me.lblDR.Name = "lblDR"
        Me.lblDR.Size = New System.Drawing.Size(147, 17)
        Me.lblDR.TabIndex = 10
        Me.lblDR.Text = "-"
        '
        'lblPerfFraction
        '
        Me.lblPerfFraction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPerfFraction.Location = New System.Drawing.Point(150, 38)
        Me.lblPerfFraction.Name = "lblPerfFraction"
        Me.lblPerfFraction.Size = New System.Drawing.Size(147, 17)
        Me.lblPerfFraction.TabIndex = 9
        Me.lblPerfFraction.Text = "-"
        '
        'lblQVersion
        '
        Me.lblQVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQVersion.Location = New System.Drawing.Point(150, 18)
        Me.lblQVersion.Name = "lblQVersion"
        Me.lblQVersion.Size = New System.Drawing.Size(147, 17)
        Me.lblQVersion.TabIndex = 8
        Me.lblQVersion.Text = "-"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblExpires)
        Me.GroupBox2.Controls.Add(Me.Label19)
        Me.GroupBox2.Controls.Add(Me.lblPreff)
        Me.GroupBox2.Controls.Add(Me.lblIssued)
        Me.GroupBox2.Controls.Add(Me.lblUF)
        Me.GroupBox2.Controls.Add(Me.lblBench)
        Me.GroupBox2.Controls.Add(Me.lblCore)
        Me.GroupBox2.Controls.Add(Me.lblUS)
        Me.GroupBox2.Controls.Add(Me.lblET)
        Me.GroupBox2.Controls.Add(Me.lblBT)
        Me.GroupBox2.Controls.Add(Me.lblPK)
        Me.GroupBox2.Controls.Add(Me.lblTeam)
        Me.GroupBox2.Controls.Add(Me.lblUser)
        Me.GroupBox2.Controls.Add(Me.lblStatus)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.lblBident)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.cmbQslots)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 121)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(294, 315)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Queue slots"
        '
        'lblExpires
        '
        Me.lblExpires.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblExpires.Location = New System.Drawing.Point(111, 286)
        Me.lblExpires.Name = "lblExpires"
        Me.lblExpires.Size = New System.Drawing.Size(177, 17)
        Me.lblExpires.TabIndex = 26
        Me.lblExpires.Text = "-"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(7, 286)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(54, 17)
        Me.Label19.TabIndex = 25
        Me.Label19.Text = "Expires"
        '
        'lblPreff
        '
        Me.lblPreff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPreff.Location = New System.Drawing.Point(111, 267)
        Me.lblPreff.Name = "lblPreff"
        Me.lblPreff.Size = New System.Drawing.Size(177, 17)
        Me.lblPreff.TabIndex = 24
        Me.lblPreff.Text = "-"
        '
        'lblIssued
        '
        Me.lblIssued.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblIssued.Location = New System.Drawing.Point(111, 248)
        Me.lblIssued.Name = "lblIssued"
        Me.lblIssued.Size = New System.Drawing.Size(177, 17)
        Me.lblIssued.TabIndex = 23
        Me.lblIssued.Text = "-"
        '
        'lblUF
        '
        Me.lblUF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblUF.Location = New System.Drawing.Point(111, 229)
        Me.lblUF.Name = "lblUF"
        Me.lblUF.Size = New System.Drawing.Size(177, 17)
        Me.lblUF.TabIndex = 22
        Me.lblUF.Text = "-"
        '
        'lblBench
        '
        Me.lblBench.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBench.Location = New System.Drawing.Point(111, 210)
        Me.lblBench.Name = "lblBench"
        Me.lblBench.Size = New System.Drawing.Size(177, 17)
        Me.lblBench.TabIndex = 21
        Me.lblBench.Text = "-"
        '
        'lblCore
        '
        Me.lblCore.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCore.Location = New System.Drawing.Point(111, 191)
        Me.lblCore.Name = "lblCore"
        Me.lblCore.Size = New System.Drawing.Size(177, 17)
        Me.lblCore.TabIndex = 20
        Me.lblCore.Text = "-"
        '
        'lblUS
        '
        Me.lblUS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblUS.Location = New System.Drawing.Point(111, 172)
        Me.lblUS.Name = "lblUS"
        Me.lblUS.Size = New System.Drawing.Size(177, 17)
        Me.lblUS.TabIndex = 19
        Me.lblUS.Text = "-"
        '
        'lblET
        '
        Me.lblET.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblET.Location = New System.Drawing.Point(111, 153)
        Me.lblET.Name = "lblET"
        Me.lblET.Size = New System.Drawing.Size(177, 17)
        Me.lblET.TabIndex = 18
        Me.lblET.Text = "-"
        '
        'lblBT
        '
        Me.lblBT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBT.Location = New System.Drawing.Point(111, 134)
        Me.lblBT.Name = "lblBT"
        Me.lblBT.Size = New System.Drawing.Size(177, 17)
        Me.lblBT.TabIndex = 17
        Me.lblBT.Text = "-"
        '
        'lblPK
        '
        Me.lblPK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPK.Location = New System.Drawing.Point(111, 115)
        Me.lblPK.Name = "lblPK"
        Me.lblPK.Size = New System.Drawing.Size(177, 17)
        Me.lblPK.TabIndex = 16
        Me.lblPK.Text = "-"
        '
        'lblTeam
        '
        Me.lblTeam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblTeam.Location = New System.Drawing.Point(111, 96)
        Me.lblTeam.Name = "lblTeam"
        Me.lblTeam.Size = New System.Drawing.Size(177, 17)
        Me.lblTeam.TabIndex = 15
        Me.lblTeam.Text = "-"
        '
        'lblUser
        '
        Me.lblUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblUser.Location = New System.Drawing.Point(111, 77)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(177, 17)
        Me.lblUser.TabIndex = 14
        Me.lblUser.Text = "-"
        '
        'lblStatus
        '
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblStatus.Location = New System.Drawing.Point(111, 58)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(177, 17)
        Me.lblStatus.TabIndex = 13
        Me.lblStatus.Text = "-"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(7, 267)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(68, 17)
        Me.Label17.TabIndex = 12
        Me.Label17.Text = "Preferred"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(7, 248)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(49, 17)
        Me.Label16.TabIndex = 11
        Me.Label16.Text = "Issued"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(7, 229)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(103, 17)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "Upload failures"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(7, 115)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 17)
        Me.Label14.TabIndex = 9
        Me.Label14.Text = "Using passkey"
        '
        'lblBident
        '
        Me.lblBident.AutoSize = True
        Me.lblBident.Location = New System.Drawing.Point(7, 210)
        Me.lblBident.Name = "lblBident"
        Me.lblBident.Size = New System.Drawing.Size(79, 17)
        Me.lblBident.TabIndex = 8
        Me.lblBident.Text = "Benchmark"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 96)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(44, 17)
        Me.Label12.TabIndex = 7
        Me.Label12.Text = "Team"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 77)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(73, 17)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Username"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 191)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(38, 17)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Core"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 172)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 17)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Upload status"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 153)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 17)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "End time"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 134)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 17)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Begin time"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 58)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 17)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Status"
        '
        'cmbQslots
        '
        Me.cmbQslots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbQslots.FormattingEnabled = True
        Me.cmbQslots.Location = New System.Drawing.Point(7, 22)
        Me.cmbQslots.Name = "cmbQslots"
        Me.cmbQslots.Size = New System.Drawing.Size(281, 24)
        Me.cmbQslots.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 98)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(81, 17)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "WU's ready"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 78)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Upload rate"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Download rate"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Perf. fraction"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Version"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(837, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cMenu
        '
        Me.cMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartToolStripMenuItem, Me.StopToolStripMenuItem, Me.ToolStripMenuItem1, Me.ConfigureToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripMenuItem2, Me.OptionsToolStripMenuItem, Me.CloseFTrayToolStripMenuItem})
        Me.cMenu.Name = "cMenu"
        Me.cMenu.Size = New System.Drawing.Size(153, 160)
        '
        'StartToolStripMenuItem
        '
        Me.StartToolStripMenuItem.Name = "StartToolStripMenuItem"
        Me.StartToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.StartToolStripMenuItem.Text = "Start"
        '
        'StopToolStripMenuItem
        '
        Me.StopToolStripMenuItem.Name = "StopToolStripMenuItem"
        Me.StopToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.StopToolStripMenuItem.Text = "Stop"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(149, 6)
        '
        'ConfigureToolStripMenuItem
        '
        Me.ConfigureToolStripMenuItem.Name = "ConfigureToolStripMenuItem"
        Me.ConfigureToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.ConfigureToolStripMenuItem.Text = "Configure"
        '
        'ShowHideToolStripMenuItem
        '
        Me.ShowHideToolStripMenuItem.Name = "ShowHideToolStripMenuItem"
        Me.ShowHideToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.ShowHideToolStripMenuItem.Text = "Show/Hide"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(149, 6)
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.OptionsToolStripMenuItem.Text = "About"
        '
        'CloseFTrayToolStripMenuItem
        '
        Me.CloseFTrayToolStripMenuItem.Name = "CloseFTrayToolStripMenuItem"
        Me.CloseFTrayToolStripMenuItem.Size = New System.Drawing.Size(152, 24)
        Me.CloseFTrayToolStripMenuItem.Text = "Close fTray"
        '
        'nIcon
        '
        Me.nIcon.ContextMenuStrip = Me.cMenu
        Me.nIcon.Text = "fTray.."
        '
        'frmLOG
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1048, 451)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmLOG"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.cmExit.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.cmStop.ResumeLayout(False)
        Me.tsCont.BottomToolStripPanel.ResumeLayout(False)
        Me.tsCont.BottomToolStripPanel.PerformLayout()
        Me.tsCont.ContentPanel.ResumeLayout(False)
        Me.tsCont.TopToolStripPanel.ResumeLayout(False)
        Me.tsCont.TopToolStripPanel.PerformLayout()
        Me.tsCont.ResumeLayout(False)
        Me.tsCont.PerformLayout()
        Me.tsBottom.ResumeLayout(False)
        Me.tsBottom.PerformLayout()
        Me.tsTop.ResumeLayout(False)
        Me.tsTop.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.cMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tClock As System.Windows.Forms.Timer
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tsCont As System.Windows.Forms.ToolStripContainer
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
    Friend WithEvents tsTop As System.Windows.Forms.ToolStrip
    Friend WithEvents pBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lblProject As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lblFrameTime As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents lblEta As System.Windows.Forms.ToolStripLabel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblTUF As System.Windows.Forms.Label
    Friend WithEvents lblUR As System.Windows.Forms.Label
    Friend WithEvents lblDR As System.Windows.Forms.Label
    Friend WithEvents lblPerfFraction As System.Windows.Forms.Label
    Friend WithEvents lblQVersion As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblPreff As System.Windows.Forms.Label
    Friend WithEvents lblIssued As System.Windows.Forms.Label
    Friend WithEvents lblUF As System.Windows.Forms.Label
    Friend WithEvents lblBench As System.Windows.Forms.Label
    Friend WithEvents lblCore As System.Windows.Forms.Label
    Friend WithEvents lblUS As System.Windows.Forms.Label
    Friend WithEvents lblET As System.Windows.Forms.Label
    Friend WithEvents lblBT As System.Windows.Forms.Label
    Friend WithEvents lblPK As System.Windows.Forms.Label
    Friend WithEvents lblTeam As System.Windows.Forms.Label
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblBident As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbQslots As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblExpires As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdConfigure As System.Windows.Forms.Button
    Friend WithEvents cmdStop As System.Windows.Forms.Button
    Friend WithEvents cmdStart As System.Windows.Forms.Button
    Friend WithEvents cmStop As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StopAfterUnitCompletesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StopToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ConfigureToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseFTrayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents nIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblKfactor As System.Windows.Forms.Label
    Friend WithEvents lblWUmax As System.Windows.Forms.Label
    Friend WithEvents lblWUPref As System.Windows.Forms.Label
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents lblAtoms As System.Windows.Forms.Label
    Friend WithEvents lblWUCORE As System.Windows.Forms.Label
    Friend WithEvents lblCredit As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.LinkLabel
    Friend WithEvents lblWuName As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents tsBottom As System.Windows.Forms.ToolStrip
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsBottomDropDown As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ViewClientFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenLogfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsSepShowHide As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsBottomShowHide As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents ToolStripDropDownButton1 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsMenuItemPB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsBottomAdvancedOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectBrowserwebToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmExit As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitFTrayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsBottomStatistics As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsThisClient As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsStanfordUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsEocWebStatistics As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EOCSignatureImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsEOCXMLStatistics As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsShowDebug As System.Windows.Forms.ToolStripMenuItem
End Class
