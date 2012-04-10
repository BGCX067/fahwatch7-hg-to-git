<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvancedOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdvancedOptions))
        Me.nudRatio_Warning = New System.Windows.Forms.NumericUpDown()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdConfigureMail = New System.Windows.Forms.Button()
        Me.chkMail = New System.Windows.Forms.CheckBox()
        Me.rbMsg = New System.Windows.Forms.RadioButton()
        Me.rbTray = New System.Windows.Forms.RadioButton()
        Me.pnlEUE_2 = New System.Windows.Forms.Panel()
        Me.txtRatio_Actual = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtParserInterval = New System.Windows.Forms.TextBox()
        Me.flp = New System.Windows.Forms.FlowLayoutPanel()
        Me.rbMineInterval = New System.Windows.Forms.RadioButton()
        Me.nudParser = New System.Windows.Forms.NumericUpDown()
        Me.rbMineSyncEOC = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.pnAutoParse = New System.Windows.Forms.Panel()
        Me.chkAutoParse = New System.Windows.Forms.CheckBox()
        Me.pnlDATAMINER = New System.Windows.Forms.Panel()
        Me.pnlInterval = New System.Windows.Forms.Panel()
        Me.rbAlwaysTrack = New System.Windows.Forms.RadioButton()
        Me.rbDisableTracking = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnlEUE_1 = New System.Windows.Forms.Panel()
        Me.rbEUE_ratio = New System.Windows.Forms.RadioButton()
        Me.rbEUE_Always = New System.Windows.Forms.RadioButton()
        Me.rbRules = New System.Windows.Forms.RadioButton()
        Me.cmdValidate = New System.Windows.Forms.Button()
        Me.pnlEOC = New System.Windows.Forms.Panel()
        Me.cmbEocPrimary = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkEocIcon = New System.Windows.Forms.CheckBox()
        Me.txtSignatureImage = New System.Windows.Forms.TextBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.llblSig = New System.Windows.Forms.LinkLabel()
        Me.lvEOC = New System.Windows.Forms.ListView()
        Me.chUser = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chTeam = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkEocSigImg = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkDisableEOC = New System.Windows.Forms.CheckBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.rbEUE = New System.Windows.Forms.RadioButton()
        Me.rbSlotFail = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.gbIndividualRules = New System.Windows.Forms.GroupBox()
        Me.gbConfigureNotifications = New System.Windows.Forms.GroupBox()
        Me.tbParanoia = New System.Windows.Forms.TrackBar()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.rbDisableNotify = New System.Windows.Forms.RadioButton()
        Me.chkUnreachableClients = New System.Windows.Forms.CheckBox()
        Me.gbEUE = New System.Windows.Forms.GroupBox()
        Me.chkFocusCheck = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.cmdResetSettings = New System.Windows.Forms.Button()
        Me.cmdManageExceptions = New System.Windows.Forms.Button()
        Me.chkDisableCrashReport = New System.Windows.Forms.CheckBox()
        Me.chkNoAutoSizeColumns = New System.Windows.Forms.CheckBox()
        Me.chkConvertUTC = New System.Windows.Forms.CheckBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.pnMessageStrip = New System.Windows.Forms.Panel()
        Me.rbHideMessage = New System.Windows.Forms.RadioButton()
        Me.rbMessageAlways = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAffinityPriority = New System.Windows.Forms.Button()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.chkOverride = New System.Windows.Forms.CheckBox()
        CType(Me.nudRatio_Warning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEUE_2.SuspendLayout()
        CType(Me.nudParser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.pnAutoParse.SuspendLayout()
        Me.pnlDATAMINER.SuspendLayout()
        Me.pnlInterval.SuspendLayout()
        Me.pnlEUE_1.SuspendLayout()
        Me.pnlEOC.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.gbIndividualRules.SuspendLayout()
        Me.gbConfigureNotifications.SuspendLayout()
        CType(Me.tbParanoia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.gbEUE.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.pnMessageStrip.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.SuspendLayout()
        '
        'nudRatio_Warning
        '
        Me.nudRatio_Warning.Location = New System.Drawing.Point(91, 23)
        Me.nudRatio_Warning.Minimum = New Decimal(New Integer() {80, 0, 0, 0})
        Me.nudRatio_Warning.Name = "nudRatio_Warning"
        Me.nudRatio_Warning.Size = New System.Drawing.Size(52, 20)
        Me.nudRatio_Warning.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.nudRatio_Warning, "Set the treshold ratio here")
        Me.nudRatio_Warning.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCancel.Location = New System.Drawing.Point(474, 587)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 36
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Notify by"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Warn if ratio <"
        '
        'cmdConfigureMail
        '
        Me.cmdConfigureMail.Location = New System.Drawing.Point(15, 40)
        Me.cmdConfigureMail.Name = "cmdConfigureMail"
        Me.cmdConfigureMail.Size = New System.Drawing.Size(106, 23)
        Me.cmdConfigureMail.TabIndex = 26
        Me.cmdConfigureMail.Text = "Configure"
        Me.ToolTip1.SetToolTip(Me.cmdConfigureMail, "Configure your email account")
        Me.cmdConfigureMail.UseVisualStyleBackColor = True
        '
        'chkMail
        '
        Me.chkMail.AutoSize = True
        Me.chkMail.Location = New System.Drawing.Point(15, 17)
        Me.chkMail.Name = "chkMail"
        Me.chkMail.Size = New System.Drawing.Size(106, 17)
        Me.chkMail.TabIndex = 25
        Me.chkMail.Text = "Send email alerts"
        Me.ToolTip1.SetToolTip(Me.chkMail, "Enable sending email alerts")
        Me.chkMail.UseVisualStyleBackColor = True
        '
        'rbMsg
        '
        Me.rbMsg.AutoSize = True
        Me.rbMsg.Location = New System.Drawing.Point(137, 22)
        Me.rbMsg.Name = "rbMsg"
        Me.rbMsg.Size = New System.Drawing.Size(55, 17)
        Me.rbMsg.TabIndex = 23
        Me.rbMsg.Text = "Dialog"
        Me.ToolTip1.SetToolTip(Me.rbMsg, "Notifications are given through a dialog")
        Me.rbMsg.UseVisualStyleBackColor = True
        '
        'rbTray
        '
        Me.rbTray.AutoSize = True
        Me.rbTray.Location = New System.Drawing.Point(66, 22)
        Me.rbTray.Name = "rbTray"
        Me.rbTray.Size = New System.Drawing.Size(69, 17)
        Me.rbTray.TabIndex = 22
        Me.rbTray.Text = "Tray icon"
        Me.ToolTip1.SetToolTip(Me.rbTray, "Notifications are given through an icon in your system tray")
        Me.rbTray.UseVisualStyleBackColor = True
        '
        'pnlEUE_2
        '
        Me.pnlEUE_2.Controls.Add(Me.nudRatio_Warning)
        Me.pnlEUE_2.Controls.Add(Me.Label6)
        Me.pnlEUE_2.Controls.Add(Me.txtRatio_Actual)
        Me.pnlEUE_2.Controls.Add(Me.Label5)
        Me.pnlEUE_2.Location = New System.Drawing.Point(6, 47)
        Me.pnlEUE_2.Name = "pnlEUE_2"
        Me.pnlEUE_2.Size = New System.Drawing.Size(162, 46)
        Me.pnlEUE_2.TabIndex = 24
        '
        'txtRatio_Actual
        '
        Me.txtRatio_Actual.BackColor = System.Drawing.SystemColors.Info
        Me.txtRatio_Actual.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtRatio_Actual.Location = New System.Drawing.Point(91, 1)
        Me.txtRatio_Actual.Name = "txtRatio_Actual"
        Me.txtRatio_Actual.ReadOnly = True
        Me.txtRatio_Actual.Size = New System.Drawing.Size(52, 20)
        Me.txtRatio_Actual.TabIndex = 1
        Me.txtRatio_Actual.TabStop = False
        Me.ToolTip1.SetToolTip(Me.txtRatio_Actual, "This is your current ratio")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Current ratio"
        '
        'txtParserInterval
        '
        Me.txtParserInterval.Location = New System.Drawing.Point(23, 2)
        Me.txtParserInterval.Name = "txtParserInterval"
        Me.txtParserInterval.ReadOnly = True
        Me.txtParserInterval.Size = New System.Drawing.Size(147, 20)
        Me.txtParserInterval.TabIndex = 5
        '
        'flp
        '
        Me.flp.AutoScroll = True
        Me.flp.BackColor = System.Drawing.SystemColors.Window
        Me.flp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flp.Location = New System.Drawing.Point(6, 19)
        Me.flp.Name = "flp"
        Me.flp.Size = New System.Drawing.Size(241, 173)
        Me.flp.TabIndex = 27
        Me.flp.WrapContents = False
        '
        'rbMineInterval
        '
        Me.rbMineInterval.AutoSize = True
        Me.rbMineInterval.Location = New System.Drawing.Point(4, 39)
        Me.rbMineInterval.Name = "rbMineInterval"
        Me.rbMineInterval.Size = New System.Drawing.Size(81, 17)
        Me.rbMineInterval.TabIndex = 4
        Me.rbMineInterval.TabStop = True
        Me.rbMineInterval.Text = "Parse every"
        Me.rbMineInterval.UseVisualStyleBackColor = True
        '
        'nudParser
        '
        Me.nudParser.Location = New System.Drawing.Point(4, 3)
        Me.nudParser.Maximum = New Decimal(New Integer() {1198931278, 106, 0, 0})
        Me.nudParser.Minimum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.nudParser.Name = "nudParser"
        Me.nudParser.Size = New System.Drawing.Size(18, 20)
        Me.nudParser.TabIndex = 1
        Me.nudParser.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'rbMineSyncEOC
        '
        Me.rbMineSyncEOC.Location = New System.Drawing.Point(4, 5)
        Me.rbMineSyncEOC.Name = "rbMineSyncEOC"
        Me.rbMineSyncEOC.Size = New System.Drawing.Size(266, 23)
        Me.rbMineSyncEOC.TabIndex = 3
        Me.rbMineSyncEOC.TabStop = True
        Me.rbMineSyncEOC.Text = "Sync with EOC updates ( 3 hour interval )"
        Me.rbMineSyncEOC.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.pnAutoParse)
        Me.GroupBox3.Controls.Add(Me.rbAlwaysTrack)
        Me.GroupBox3.Controls.Add(Me.rbDisableTracking)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(292, 174)
        Me.GroupBox3.TabIndex = 26
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Monitoring options"
        '
        'pnAutoParse
        '
        Me.pnAutoParse.Controls.Add(Me.chkAutoParse)
        Me.pnAutoParse.Controls.Add(Me.pnlDATAMINER)
        Me.pnAutoParse.Location = New System.Drawing.Point(5, 65)
        Me.pnAutoParse.Name = "pnAutoParse"
        Me.pnAutoParse.Size = New System.Drawing.Size(283, 101)
        Me.pnAutoParse.TabIndex = 7
        '
        'chkAutoParse
        '
        Me.chkAutoParse.AutoSize = True
        Me.chkAutoParse.Location = New System.Drawing.Point(6, 6)
        Me.chkAutoParse.Name = "chkAutoParse"
        Me.chkAutoParse.Size = New System.Drawing.Size(207, 17)
        Me.chkAutoParse.TabIndex = 2
        Me.chkAutoParse.Text = "Automatically parse log files on interval"
        Me.chkAutoParse.UseVisualStyleBackColor = True
        '
        'pnlDATAMINER
        '
        Me.pnlDATAMINER.Controls.Add(Me.rbMineInterval)
        Me.pnlDATAMINER.Controls.Add(Me.rbMineSyncEOC)
        Me.pnlDATAMINER.Controls.Add(Me.pnlInterval)
        Me.pnlDATAMINER.Location = New System.Drawing.Point(3, 27)
        Me.pnlDATAMINER.Name = "pnlDATAMINER"
        Me.pnlDATAMINER.Size = New System.Drawing.Size(275, 69)
        Me.pnlDATAMINER.TabIndex = 4
        '
        'pnlInterval
        '
        Me.pnlInterval.Controls.Add(Me.txtParserInterval)
        Me.pnlInterval.Controls.Add(Me.nudParser)
        Me.pnlInterval.Location = New System.Drawing.Point(90, 34)
        Me.pnlInterval.Name = "pnlInterval"
        Me.pnlInterval.Size = New System.Drawing.Size(175, 28)
        Me.pnlInterval.TabIndex = 4
        '
        'rbAlwaysTrack
        '
        Me.rbAlwaysTrack.AutoSize = True
        Me.rbAlwaysTrack.Location = New System.Drawing.Point(9, 18)
        Me.rbAlwaysTrack.Name = "rbAlwaysTrack"
        Me.rbAlwaysTrack.Size = New System.Drawing.Size(118, 17)
        Me.rbAlwaysTrack.TabIndex = 0
        Me.rbAlwaysTrack.TabStop = True
        Me.rbAlwaysTrack.Text = "Always track clients"
        Me.ToolTip1.SetToolTip(Me.rbAlwaysTrack, "When ever the application is running it will track connected clients")
        Me.rbAlwaysTrack.UseVisualStyleBackColor = True
        '
        'rbDisableTracking
        '
        Me.rbDisableTracking.AutoSize = True
        Me.rbDisableTracking.Location = New System.Drawing.Point(9, 41)
        Me.rbDisableTracking.Name = "rbDisableTracking"
        Me.rbDisableTracking.Size = New System.Drawing.Size(244, 17)
        Me.rbDisableTracking.TabIndex = 1
        Me.rbDisableTracking.TabStop = True
        Me.rbDisableTracking.Text = "Don't track clients when the monitor is inactive"
        Me.ToolTip1.SetToolTip(Me.rbDisableTracking, "Disable updates while the monitor is inactive")
        Me.rbDisableTracking.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Notify"
        '
        'pnlEUE_1
        '
        Me.pnlEUE_1.Controls.Add(Me.Label2)
        Me.pnlEUE_1.Controls.Add(Me.rbEUE_ratio)
        Me.pnlEUE_1.Controls.Add(Me.rbEUE_Always)
        Me.pnlEUE_1.Location = New System.Drawing.Point(6, 19)
        Me.pnlEUE_1.Name = "pnlEUE_1"
        Me.pnlEUE_1.Size = New System.Drawing.Size(184, 21)
        Me.pnlEUE_1.TabIndex = 25
        '
        'rbEUE_ratio
        '
        Me.rbEUE_ratio.AutoSize = True
        Me.rbEUE_ratio.Location = New System.Drawing.Point(123, 3)
        Me.rbEUE_ratio.Name = "rbEUE_ratio"
        Me.rbEUE_ratio.Size = New System.Drawing.Size(59, 17)
        Me.rbEUE_ratio.TabIndex = 20
        Me.rbEUE_ratio.TabStop = True
        Me.rbEUE_ratio.Text = "by ratio"
        Me.ToolTip1.SetToolTip(Me.rbEUE_ratio, "Only generate an alert if the ratio drops below the treshold")
        Me.rbEUE_ratio.UseVisualStyleBackColor = True
        '
        'rbEUE_Always
        '
        Me.rbEUE_Always.AutoSize = True
        Me.rbEUE_Always.Location = New System.Drawing.Point(52, 3)
        Me.rbEUE_Always.Name = "rbEUE_Always"
        Me.rbEUE_Always.Size = New System.Drawing.Size(57, 17)
        Me.rbEUE_Always.TabIndex = 19
        Me.rbEUE_Always.TabStop = True
        Me.rbEUE_Always.Text = "always"
        Me.ToolTip1.SetToolTip(Me.rbEUE_Always, "Always generate an alert when an EUE occured")
        Me.rbEUE_Always.UseVisualStyleBackColor = True
        '
        'rbRules
        '
        Me.rbRules.AutoSize = True
        Me.rbRules.Location = New System.Drawing.Point(6, 64)
        Me.rbRules.Name = "rbRules"
        Me.rbRules.Size = New System.Drawing.Size(116, 17)
        Me.rbRules.TabIndex = 17
        Me.rbRules.TabStop = True
        Me.rbRules.Text = "Use individual rules"
        Me.ToolTip1.SetToolTip(Me.rbRules, "Use individual rules for either clients or slots")
        Me.rbRules.UseVisualStyleBackColor = True
        '
        'cmdValidate
        '
        Me.cmdValidate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdValidate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdValidate.Location = New System.Drawing.Point(257, 168)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.Size = New System.Drawing.Size(75, 23)
        Me.cmdValidate.TabIndex = 11
        Me.cmdValidate.Text = "Validate"
        Me.ToolTip1.SetToolTip(Me.cmdValidate, "Validate the custom formatting options")
        Me.cmdValidate.UseVisualStyleBackColor = True
        '
        'pnlEOC
        '
        Me.pnlEOC.Controls.Add(Me.cmbEocPrimary)
        Me.pnlEOC.Controls.Add(Me.Label4)
        Me.pnlEOC.Controls.Add(Me.chkEocIcon)
        Me.pnlEOC.Controls.Add(Me.cmdValidate)
        Me.pnlEOC.Controls.Add(Me.txtSignatureImage)
        Me.pnlEOC.Controls.Add(Me.CheckBox1)
        Me.pnlEOC.Controls.Add(Me.llblSig)
        Me.pnlEOC.Controls.Add(Me.lvEOC)
        Me.pnlEOC.Controls.Add(Me.Label1)
        Me.pnlEOC.Controls.Add(Me.chkEocSigImg)
        Me.pnlEOC.Enabled = False
        Me.pnlEOC.Location = New System.Drawing.Point(5, 35)
        Me.pnlEOC.Name = "pnlEOC"
        Me.pnlEOC.Size = New System.Drawing.Size(337, 217)
        Me.pnlEOC.TabIndex = 9
        '
        'cmbEocPrimary
        '
        Me.cmbEocPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEocPrimary.FormattingEnabled = True
        Me.cmbEocPrimary.Location = New System.Drawing.Point(149, 120)
        Me.cmbEocPrimary.Name = "cmbEocPrimary"
        Me.cmbEocPrimary.Size = New System.Drawing.Size(180, 21)
        Me.cmbEocPrimary.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(5, 123)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Primary folding account:"
        '
        'chkEocIcon
        '
        Me.chkEocIcon.AutoSize = True
        Me.chkEocIcon.Location = New System.Drawing.Point(257, 197)
        Me.chkEocIcon.Name = "chkEocIcon"
        Me.chkEocIcon.Size = New System.Drawing.Size(72, 17)
        Me.chkEocIcon.TabIndex = 13
        Me.chkEocIcon.Text = "EOC Icon"
        Me.chkEocIcon.UseVisualStyleBackColor = True
        '
        'txtSignatureImage
        '
        Me.txtSignatureImage.AllowDrop = True
        Me.txtSignatureImage.Location = New System.Drawing.Point(4, 170)
        Me.txtSignatureImage.Name = "txtSignatureImage"
        Me.txtSignatureImage.Size = New System.Drawing.Size(250, 20)
        Me.txtSignatureImage.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.txtSignatureImage, "Add custom formatting options, follow the link on the above label and add everyth" & _
        "ing which goes after your account url")
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(4, 147)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 8
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'llblSig
        '
        Me.llblSig.LinkArea = New System.Windows.Forms.LinkArea(27, 36)
        Me.llblSig.Location = New System.Drawing.Point(23, 147)
        Me.llblSig.Name = "llblSig"
        Me.llblSig.Size = New System.Drawing.Size(250, 19)
        Me.llblSig.TabIndex = 9
        Me.llblSig.TabStop = True
        Me.llblSig.Text = "Use custom signature image formatting options"
        Me.llblSig.UseCompatibleTextRendering = True
        '
        'lvEOC
        '
        Me.lvEOC.CheckBoxes = True
        Me.lvEOC.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chUser, Me.chTeam})
        Me.lvEOC.FullRowSelect = True
        Me.lvEOC.Location = New System.Drawing.Point(4, 24)
        Me.lvEOC.Name = "lvEOC"
        Me.lvEOC.Size = New System.Drawing.Size(325, 93)
        Me.lvEOC.TabIndex = 9
        Me.lvEOC.UseCompatibleStateImageBehavior = False
        Me.lvEOC.View = System.Windows.Forms.View.Details
        '
        'chUser
        '
        Me.chUser.Text = "user"
        Me.chUser.Width = 68
        '
        'chTeam
        '
        Me.chTeam.Text = "team"
        Me.chTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(5, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Uncheck account to disable"
        '
        'chkEocSigImg
        '
        Me.chkEocSigImg.AutoSize = True
        Me.chkEocSigImg.Location = New System.Drawing.Point(4, 196)
        Me.chkEocSigImg.Name = "chkEocSigImg"
        Me.chkEocSigImg.Size = New System.Drawing.Size(188, 17)
        Me.chkEocSigImg.TabIndex = 12
        Me.chkEocSigImg.Text = "Pop up signature image on update"
        Me.chkEocSigImg.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlEOC)
        Me.GroupBox1.Controls.Add(Me.chkDisableEOC)
        Me.GroupBox1.Location = New System.Drawing.Point(301, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(350, 258)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "ExtremeOverclocking.com XML updates"
        '
        'chkDisableEOC
        '
        Me.chkDisableEOC.AutoSize = True
        Me.chkDisableEOC.Location = New System.Drawing.Point(6, 16)
        Me.chkDisableEOC.Name = "chkDisableEOC"
        Me.chkDisableEOC.Size = New System.Drawing.Size(206, 17)
        Me.chkDisableEOC.TabIndex = 8
        Me.chkDisableEOC.Text = "Disable ExtremeOverclocking updates"
        Me.chkDisableEOC.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdOK.Location = New System.Drawing.Point(567, 587)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 37
        Me.cmdOK.Text = "&Accept"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'rbEUE
        '
        Me.rbEUE.AutoSize = True
        Me.rbEUE.Location = New System.Drawing.Point(6, 49)
        Me.rbEUE.Name = "rbEUE"
        Me.rbEUE.Size = New System.Drawing.Size(137, 17)
        Me.rbEUE.TabIndex = 16
        Me.rbEUE.TabStop = True
        Me.rbEUE.Text = "Notify on Early Unit End"
        Me.ToolTip1.SetToolTip(Me.rbEUE, "Alerts are generated on each Early Unit End, or by ratio")
        Me.rbEUE.UseVisualStyleBackColor = True
        '
        'rbSlotFail
        '
        Me.rbSlotFail.AutoSize = True
        Me.rbSlotFail.Location = New System.Drawing.Point(6, 34)
        Me.rbSlotFail.Name = "rbSlotFail"
        Me.rbSlotFail.Size = New System.Drawing.Size(117, 17)
        Me.rbSlotFail.TabIndex = 15
        Me.rbSlotFail.TabStop = True
        Me.rbSlotFail.Text = "Notify on slot failure"
        Me.ToolTip1.SetToolTip(Me.rbSlotFail, "Alerts are generated only if a slot has a Failed status")
        Me.rbSlotFail.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gbIndividualRules)
        Me.GroupBox2.Controls.Add(Me.gbConfigureNotifications)
        Me.GroupBox2.Controls.Add(Me.GroupBox6)
        Me.GroupBox2.Controls.Add(Me.gbEUE)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 267)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(648, 231)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Early Unit End and slot failure notifications"
        '
        'gbIndividualRules
        '
        Me.gbIndividualRules.Controls.Add(Me.flp)
        Me.gbIndividualRules.Location = New System.Drawing.Point(385, 19)
        Me.gbIndividualRules.Name = "gbIndividualRules"
        Me.gbIndividualRules.Size = New System.Drawing.Size(255, 206)
        Me.gbIndividualRules.TabIndex = 31
        Me.gbIndividualRules.TabStop = False
        Me.gbIndividualRules.Text = "Individual rules"
        '
        'gbConfigureNotifications
        '
        Me.gbConfigureNotifications.Controls.Add(Me.tbParanoia)
        Me.gbConfigureNotifications.Controls.Add(Me.GroupBox10)
        Me.gbConfigureNotifications.Controls.Add(Me.rbTray)
        Me.gbConfigureNotifications.Controls.Add(Me.Label3)
        Me.gbConfigureNotifications.Controls.Add(Me.rbMsg)
        Me.gbConfigureNotifications.Location = New System.Drawing.Point(9, 130)
        Me.gbConfigureNotifications.Name = "gbConfigureNotifications"
        Me.gbConfigureNotifications.Size = New System.Drawing.Size(370, 95)
        Me.gbConfigureNotifications.TabIndex = 22
        Me.gbConfigureNotifications.TabStop = False
        Me.gbConfigureNotifications.Text = "Configure notifications"
        '
        'tbParanoia
        '
        Me.tbParanoia.AutoSize = False
        Me.tbParanoia.LargeChange = 1
        Me.tbParanoia.Location = New System.Drawing.Point(20, 49)
        Me.tbParanoia.Maximum = 2
        Me.tbParanoia.Name = "tbParanoia"
        Me.tbParanoia.Size = New System.Drawing.Size(172, 32)
        Me.tbParanoia.TabIndex = 24
        Me.ToolTip1.SetToolTip(Me.tbParanoia, "Configure your paranoia")
        Me.tbParanoia.Value = 1
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.cmdConfigureMail)
        Me.GroupBox10.Controls.Add(Me.chkMail)
        Me.GroupBox10.Location = New System.Drawing.Point(218, 13)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(140, 76)
        Me.GroupBox10.TabIndex = 29
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Email alerts"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.rbDisableNotify)
        Me.GroupBox6.Controls.Add(Me.chkUnreachableClients)
        Me.GroupBox6.Controls.Add(Me.rbSlotFail)
        Me.GroupBox6.Controls.Add(Me.rbEUE)
        Me.GroupBox6.Controls.Add(Me.rbRules)
        Me.GroupBox6.Location = New System.Drawing.Point(9, 19)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(162, 105)
        Me.GroupBox6.TabIndex = 30
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Configure triggers"
        '
        'rbDisableNotify
        '
        Me.rbDisableNotify.AutoSize = True
        Me.rbDisableNotify.Location = New System.Drawing.Point(6, 19)
        Me.rbDisableNotify.Name = "rbDisableNotify"
        Me.rbDisableNotify.Size = New System.Drawing.Size(119, 17)
        Me.rbDisableNotify.TabIndex = 14
        Me.rbDisableNotify.TabStop = True
        Me.rbDisableNotify.Text = "Disable notifications"
        Me.ToolTip1.SetToolTip(Me.rbDisableNotify, "Disable notifcations, never raise alerts")
        Me.rbDisableNotify.UseVisualStyleBackColor = True
        '
        'chkUnreachableClients
        '
        Me.chkUnreachableClients.Location = New System.Drawing.Point(6, 81)
        Me.chkUnreachableClients.Name = "chkUnreachableClients"
        Me.chkUnreachableClients.Size = New System.Drawing.Size(148, 17)
        Me.chkUnreachableClients.TabIndex = 18
        Me.chkUnreachableClients.Text = "Notify unreachable clients"
        Me.ToolTip1.SetToolTip(Me.chkUnreachableClients, "Generate additional allerts when a client is not reachable")
        Me.chkUnreachableClients.UseVisualStyleBackColor = True
        '
        'gbEUE
        '
        Me.gbEUE.Controls.Add(Me.pnlEUE_1)
        Me.gbEUE.Controls.Add(Me.pnlEUE_2)
        Me.gbEUE.Location = New System.Drawing.Point(177, 19)
        Me.gbEUE.Name = "gbEUE"
        Me.gbEUE.Size = New System.Drawing.Size(202, 105)
        Me.gbEUE.TabIndex = 30
        Me.gbEUE.TabStop = False
        Me.gbEUE.Text = "Early Unit End options"
        '
        'chkFocusCheck
        '
        Me.chkFocusCheck.Location = New System.Drawing.Point(7, 19)
        Me.chkFocusCheck.Name = "chkFocusCheck"
        Me.chkFocusCheck.Size = New System.Drawing.Size(212, 24)
        Me.chkFocusCheck.TabIndex = 28
        Me.chkFocusCheck.Text = "Don't update a focused Graph"
        Me.ToolTip1.SetToolTip(Me.chkFocusCheck, "When a graph has mouse focus, it won't be updated untill the mouse moves out of t" & _
        "he graph")
        Me.chkFocusCheck.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cmdResetSettings)
        Me.GroupBox4.Controls.Add(Me.cmdManageExceptions)
        Me.GroupBox4.Controls.Add(Me.chkDisableCrashReport)
        Me.GroupBox4.Controls.Add(Me.chkNoAutoSizeColumns)
        Me.GroupBox4.Controls.Add(Me.chkConvertUTC)
        Me.GroupBox4.Controls.Add(Me.chkFocusCheck)
        Me.GroupBox4.Location = New System.Drawing.Point(3, 505)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(379, 105)
        Me.GroupBox4.TabIndex = 27
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Miscelanious"
        '
        'cmdResetSettings
        '
        Me.cmdResetSettings.Location = New System.Drawing.Point(5, 72)
        Me.cmdResetSettings.Name = "cmdResetSettings"
        Me.cmdResetSettings.Size = New System.Drawing.Size(166, 23)
        Me.cmdResetSettings.TabIndex = 32
        Me.cmdResetSettings.Text = "Clear settings"
        Me.ToolTip1.SetToolTip(Me.cmdResetSettings, "This option will erase all your user settings, use with caution")
        Me.cmdResetSettings.UseVisualStyleBackColor = True
        '
        'cmdManageExceptions
        '
        Me.cmdManageExceptions.Enabled = False
        Me.cmdManageExceptions.Location = New System.Drawing.Point(192, 72)
        Me.cmdManageExceptions.Name = "cmdManageExceptions"
        Me.cmdManageExceptions.Size = New System.Drawing.Size(181, 23)
        Me.cmdManageExceptions.TabIndex = 33
        Me.cmdManageExceptions.Text = "Manage exception ignore list"
        Me.ToolTip1.SetToolTip(Me.cmdManageExceptions, "Manage automatic exception reporting and ignoring")
        Me.cmdManageExceptions.UseVisualStyleBackColor = True
        '
        'chkDisableCrashReport
        '
        Me.chkDisableCrashReport.AutoSize = True
        Me.chkDisableCrashReport.Location = New System.Drawing.Point(192, 49)
        Me.chkDisableCrashReport.Name = "chkDisableCrashReport"
        Me.chkDisableCrashReport.Size = New System.Drawing.Size(182, 17)
        Me.chkDisableCrashReport.TabIndex = 31
        Me.chkDisableCrashReport.Text = "Never show the Exception dialog"
        Me.ToolTip1.SetToolTip(Me.chkDisableCrashReport, "Turn of the Exception dial, and any logic depending on it")
        Me.chkDisableCrashReport.UseVisualStyleBackColor = True
        '
        'chkNoAutoSizeColumns
        '
        Me.chkNoAutoSizeColumns.AutoSize = True
        Me.chkNoAutoSizeColumns.Location = New System.Drawing.Point(7, 49)
        Me.chkNoAutoSizeColumns.Name = "chkNoAutoSizeColumns"
        Me.chkNoAutoSizeColumns.Size = New System.Drawing.Size(145, 17)
        Me.chkNoAutoSizeColumns.TabIndex = 29
        Me.chkNoAutoSizeColumns.Text = "Do not auto size columns"
        Me.ToolTip1.SetToolTip(Me.chkNoAutoSizeColumns, "Allows resizing of columns")
        Me.chkNoAutoSizeColumns.UseVisualStyleBackColor = True
        '
        'chkConvertUTC
        '
        Me.chkConvertUTC.AutoSize = True
        Me.chkConvertUTC.Location = New System.Drawing.Point(192, 23)
        Me.chkConvertUTC.Name = "chkConvertUTC"
        Me.chkConvertUTC.Size = New System.Drawing.Size(147, 17)
        Me.chkConvertUTC.TabIndex = 30
        Me.chkConvertUTC.Text = "Convert UTC to local time"
        Me.ToolTip1.SetToolTip(Me.chkConvertUTC, "Convert log date and time to local time")
        Me.chkConvertUTC.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.pnMessageStrip)
        Me.GroupBox5.Location = New System.Drawing.Point(3, 179)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(292, 82)
        Me.GroupBox5.TabIndex = 28
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Status message options"
        '
        'pnMessageStrip
        '
        Me.pnMessageStrip.Controls.Add(Me.rbHideMessage)
        Me.pnMessageStrip.Controls.Add(Me.rbMessageAlways)
        Me.pnMessageStrip.Location = New System.Drawing.Point(10, 20)
        Me.pnMessageStrip.Name = "pnMessageStrip"
        Me.pnMessageStrip.Size = New System.Drawing.Size(275, 52)
        Me.pnMessageStrip.TabIndex = 1
        '
        'rbHideMessage
        '
        Me.rbHideMessage.AutoSize = True
        Me.rbHideMessage.Location = New System.Drawing.Point(7, 31)
        Me.rbHideMessage.Name = "rbHideMessage"
        Me.rbHideMessage.Size = New System.Drawing.Size(211, 17)
        Me.rbHideMessage.TabIndex = 7
        Me.rbHideMessage.TabStop = True
        Me.rbHideMessage.Text = "Hide the message strip when not active"
        Me.ToolTip1.SetToolTip(Me.rbHideMessage, "The status strip displaying messages will dissapear after messages have been show" & _
        "n")
        Me.rbHideMessage.UseVisualStyleBackColor = True
        '
        'rbMessageAlways
        '
        Me.rbMessageAlways.AutoSize = True
        Me.rbMessageAlways.Location = New System.Drawing.Point(7, 5)
        Me.rbMessageAlways.Name = "rbMessageAlways"
        Me.rbMessageAlways.Size = New System.Drawing.Size(171, 17)
        Me.rbMessageAlways.TabIndex = 6
        Me.rbMessageAlways.TabStop = True
        Me.rbMessageAlways.Text = "Always show the message strip"
        Me.ToolTip1.SetToolTip(Me.rbMessageAlways, "The status strip displaying messages will always be visible")
        Me.rbMessageAlways.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.ShowAlways = True
        '
        'cmdAffinityPriority
        '
        Me.cmdAffinityPriority.Location = New System.Drawing.Point(7, 42)
        Me.cmdAffinityPriority.Name = "cmdAffinityPriority"
        Me.cmdAffinityPriority.Size = New System.Drawing.Size(230, 23)
        Me.cmdAffinityPriority.TabIndex = 35
        Me.cmdAffinityPriority.Text = "Configure overrides"
        Me.ToolTip1.SetToolTip(Me.cmdAffinityPriority, "Configure overrides for core affinity and priority")
        Me.cmdAffinityPriority.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.cmdAffinityPriority)
        Me.GroupBox7.Controls.Add(Me.chkOverride)
        Me.GroupBox7.Location = New System.Drawing.Point(394, 505)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(256, 75)
        Me.GroupBox7.TabIndex = 29
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Affinity/Priority overrides"
        '
        'chkOverride
        '
        Me.chkOverride.AutoSize = True
        Me.chkOverride.Location = New System.Drawing.Point(7, 19)
        Me.chkOverride.Name = "chkOverride"
        Me.chkOverride.Size = New System.Drawing.Size(230, 17)
        Me.chkOverride.TabIndex = 34
        Me.chkOverride.Text = "Override default affinity and priorrity settings"
        Me.chkOverride.UseVisualStyleBackColor = True
        '
        'frmAdvancedOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(656, 622)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAdvancedOptions"
        Me.Text = "Advanced options"
        CType(Me.nudRatio_Warning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEUE_2.ResumeLayout(False)
        Me.pnlEUE_2.PerformLayout()
        CType(Me.nudParser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.pnAutoParse.ResumeLayout(False)
        Me.pnAutoParse.PerformLayout()
        Me.pnlDATAMINER.ResumeLayout(False)
        Me.pnlDATAMINER.PerformLayout()
        Me.pnlInterval.ResumeLayout(False)
        Me.pnlInterval.PerformLayout()
        Me.pnlEUE_1.ResumeLayout(False)
        Me.pnlEUE_1.PerformLayout()
        Me.pnlEOC.ResumeLayout(False)
        Me.pnlEOC.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.gbIndividualRules.ResumeLayout(False)
        Me.gbConfigureNotifications.ResumeLayout(False)
        Me.gbConfigureNotifications.PerformLayout()
        CType(Me.tbParanoia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.gbEUE.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.pnMessageStrip.ResumeLayout(False)
        Me.pnMessageStrip.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents nudRatio_Warning As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents rbMsg As System.Windows.Forms.RadioButton
    Friend WithEvents rbTray As System.Windows.Forms.RadioButton
    Friend WithEvents pnlEUE_2 As System.Windows.Forms.Panel
    Friend WithEvents txtRatio_Actual As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtParserInterval As System.Windows.Forms.TextBox
    Friend WithEvents flp As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents rbMineInterval As System.Windows.Forms.RadioButton
    Friend WithEvents nudParser As System.Windows.Forms.NumericUpDown
    Friend WithEvents rbMineSyncEOC As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlDATAMINER As System.Windows.Forms.Panel
    Friend WithEvents pnlInterval As System.Windows.Forms.Panel
    Friend WithEvents chkAutoParse As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pnlEUE_1 As System.Windows.Forms.Panel
    Friend WithEvents rbEUE_ratio As System.Windows.Forms.RadioButton
    Friend WithEvents rbEUE_Always As System.Windows.Forms.RadioButton
    Friend WithEvents rbRules As System.Windows.Forms.RadioButton
    Friend WithEvents cmdValidate As System.Windows.Forms.Button
    Friend WithEvents pnlEOC As System.Windows.Forms.Panel
    Friend WithEvents txtSignatureImage As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents llblSig As System.Windows.Forms.LinkLabel
    Friend WithEvents lvEOC As System.Windows.Forms.ListView
    Friend WithEvents chUser As System.Windows.Forms.ColumnHeader
    Friend WithEvents chTeam As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkEocSigImg As System.Windows.Forms.CheckBox
    Friend WithEvents chkEocIcon As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkDisableEOC As System.Windows.Forms.CheckBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents rbEUE As System.Windows.Forms.RadioButton
    Friend WithEvents rbSlotFail As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkFocusCheck As System.Windows.Forms.CheckBox
    Friend WithEvents cmbEocPrimary As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents pnMessageStrip As System.Windows.Forms.Panel
    Friend WithEvents rbHideMessage As System.Windows.Forms.RadioButton
    Friend WithEvents rbMessageAlways As System.Windows.Forms.RadioButton
    Friend WithEvents chkConvertUTC As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cmdConfigureMail As System.Windows.Forms.Button
    Friend WithEvents chkMail As System.Windows.Forms.CheckBox
    Friend WithEvents pnAutoParse As System.Windows.Forms.Panel
    Friend WithEvents rbAlwaysTrack As System.Windows.Forms.RadioButton
    Friend WithEvents rbDisableTracking As System.Windows.Forms.RadioButton
    Friend WithEvents chkUnreachableClients As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableCrashReport As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoAutoSizeColumns As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents gbEUE As System.Windows.Forms.GroupBox
    Friend WithEvents rbDisableNotify As System.Windows.Forms.RadioButton
    Friend WithEvents gbConfigureNotifications As System.Windows.Forms.GroupBox
    Friend WithEvents gbIndividualRules As System.Windows.Forms.GroupBox
    Friend WithEvents tbParanoia As System.Windows.Forms.TrackBar
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdAffinityPriority As System.Windows.Forms.Button
    Friend WithEvents chkOverride As System.Windows.Forms.CheckBox
    Friend WithEvents cmdResetSettings As System.Windows.Forms.Button
    Friend WithEvents cmdManageExceptions As System.Windows.Forms.Button
End Class
