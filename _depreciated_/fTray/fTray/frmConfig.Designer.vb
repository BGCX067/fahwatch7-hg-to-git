<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
        Me.gbC = New System.Windows.Forms.GroupBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.lnblCores = New System.Windows.Forms.LinkLabel()
        Me.cmbCores = New System.Windows.Forms.ComboBox()
        Me.llblLog = New System.Windows.Forms.LinkLabel()
        Me.cmbMachineID = New System.Windows.Forms.ComboBox()
        Me.lblID = New System.Windows.Forms.Label()
        Me.rtf = New System.Windows.Forms.RichTextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.nudMem = New System.Windows.Forms.NumericUpDown()
        Me.cmbCorePriority = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbProxy = New System.Windows.Forms.ComboBox()
        Me.nudInterval = New System.Windows.Forms.NumericUpDown()
        Me.cmbService = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmbBattery = New System.Windows.Forms.ComboBox()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nudCPUusage = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cmbAdv = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.cmbDisableAssembly = New System.Windows.Forms.ComboBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtTeamNumber = New System.Windows.Forms.TextBox()
        Me.txtParam = New System.Windows.Forms.TextBox()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.cmbCpuAffinity = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPassKey = New System.Windows.Forms.TextBox()
        Me.cmbProxyPassword = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbAskNet = New System.Windows.Forms.ComboBox()
        Me.lblService = New System.Windows.Forms.Label()
        Me.cmbBigWu = New System.Windows.Forms.ComboBox()
        Me.cmbLocalDeadlines = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.lblS = New System.Windows.Forms.Label()
        Me.gbProxy = New System.Windows.Forms.GroupBox()
        Me.gbProxyPassword = New System.Windows.Forms.GroupBox()
        Me.txtPpassword = New System.Windows.Forms.TextBox()
        Me.txtPusername = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtProxyPort = New System.Windows.Forms.TextBox()
        Me.txtProxyName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gbC.SuspendLayout()
        CType(Me.nudMem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCPUusage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbProxy.SuspendLayout()
        Me.gbProxyPassword.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbC
        '
        Me.gbC.Controls.Add(Me.LinkLabel1)
        Me.gbC.Controls.Add(Me.lnblCores)
        Me.gbC.Controls.Add(Me.cmbCores)
        Me.gbC.Controls.Add(Me.llblLog)
        Me.gbC.Controls.Add(Me.cmbMachineID)
        Me.gbC.Controls.Add(Me.lblID)
        Me.gbC.Controls.Add(Me.rtf)
        Me.gbC.Controls.Add(Me.Label11)
        Me.gbC.Controls.Add(Me.nudMem)
        Me.gbC.Controls.Add(Me.cmbCorePriority)
        Me.gbC.Controls.Add(Me.Label16)
        Me.gbC.Controls.Add(Me.Label12)
        Me.gbC.Controls.Add(Me.cmdCancel)
        Me.gbC.Controls.Add(Me.Label14)
        Me.gbC.Controls.Add(Me.cmbProxy)
        Me.gbC.Controls.Add(Me.nudInterval)
        Me.gbC.Controls.Add(Me.cmbService)
        Me.gbC.Controls.Add(Me.Label13)
        Me.gbC.Controls.Add(Me.Label1)
        Me.gbC.Controls.Add(Me.cmdNext)
        Me.gbC.Controls.Add(Me.cmbBattery)
        Me.gbC.Controls.Add(Me.txtUserName)
        Me.gbC.Controls.Add(Me.Label7)
        Me.gbC.Controls.Add(Me.nudCPUusage)
        Me.gbC.Controls.Add(Me.Label9)
        Me.gbC.Controls.Add(Me.Label15)
        Me.gbC.Controls.Add(Me.cmbAdv)
        Me.gbC.Controls.Add(Me.Label24)
        Me.gbC.Controls.Add(Me.cmbDisableAssembly)
        Me.gbC.Controls.Add(Me.Label26)
        Me.gbC.Controls.Add(Me.txtTeamNumber)
        Me.gbC.Controls.Add(Me.txtParam)
        Me.gbC.Controls.Add(Me.txtIP)
        Me.gbC.Controls.Add(Me.Label25)
        Me.gbC.Controls.Add(Me.cmbCpuAffinity)
        Me.gbC.Controls.Add(Me.Label3)
        Me.gbC.Controls.Add(Me.Label2)
        Me.gbC.Controls.Add(Me.txtPassKey)
        Me.gbC.Controls.Add(Me.cmbProxyPassword)
        Me.gbC.Controls.Add(Me.Label8)
        Me.gbC.Controls.Add(Me.cmbAskNet)
        Me.gbC.Controls.Add(Me.lblService)
        Me.gbC.Controls.Add(Me.cmbBigWu)
        Me.gbC.Controls.Add(Me.cmbLocalDeadlines)
        Me.gbC.Controls.Add(Me.Label17)
        Me.gbC.Controls.Add(Me.Label21)
        Me.gbC.Controls.Add(Me.lblS)
        Me.gbC.Controls.Add(Me.gbProxy)
        Me.gbC.Location = New System.Drawing.Point(4, -2)
        Me.gbC.Margin = New System.Windows.Forms.Padding(4)
        Me.gbC.Name = "gbC"
        Me.gbC.Padding = New System.Windows.Forms.Padding(4)
        Me.gbC.Size = New System.Drawing.Size(539, 863)
        Me.gbC.TabIndex = 120
        Me.gbC.TabStop = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkArea = New System.Windows.Forms.LinkArea(24, 5)
        Me.LinkLabel1.Location = New System.Drawing.Point(11, 728)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LinkLabel1.Size = New System.Drawing.Size(188, 20)
        Me.LinkLabel1.TabIndex = 166
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Additional parameters ( help? )"
        Me.LinkLabel1.UseCompatibleTextRendering = True
        '
        'lnblCores
        '
        Me.lnblCores.AutoSize = True
        Me.lnblCores.LinkArea = New System.Windows.Forms.LinkArea(27, 5)
        Me.lnblCores.Location = New System.Drawing.Point(244, 728)
        Me.lnblCores.Name = "lnblCores"
        Me.lnblCores.Size = New System.Drawing.Size(213, 20)
        Me.lnblCores.TabIndex = 165
        Me.lnblCores.TabStop = True
        Me.lnblCores.Text = "Number of cores assigned ( help? )"
        Me.lnblCores.UseCompatibleTextRendering = True
        Me.lnblCores.Visible = False
        '
        'cmbCores
        '
        Me.cmbCores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCores.FormattingEnabled = True
        Me.cmbCores.Location = New System.Drawing.Point(464, 725)
        Me.cmbCores.Name = "cmbCores"
        Me.cmbCores.Size = New System.Drawing.Size(65, 24)
        Me.cmbCores.TabIndex = 164
        Me.cmbCores.Visible = False
        '
        'llblLog
        '
        Me.llblLog.Location = New System.Drawing.Point(199, 807)
        Me.llblLog.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llblLog.Name = "llblLog"
        Me.llblLog.Size = New System.Drawing.Size(137, 17)
        Me.llblLog.TabIndex = 162
        Me.llblLog.TabStop = True
        Me.llblLog.Text = "General help"
        Me.llblLog.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmbMachineID
        '
        Me.cmbMachineID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMachineID.FormattingEnabled = True
        Me.cmbMachineID.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16"})
        Me.cmbMachineID.Location = New System.Drawing.Point(463, 618)
        Me.cmbMachineID.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbMachineID.Name = "cmbMachineID"
        Me.cmbMachineID.Size = New System.Drawing.Size(65, 24)
        Me.cmbMachineID.TabIndex = 21
        '
        'lblID
        '
        Me.lblID.AutoSize = True
        Me.lblID.Location = New System.Drawing.Point(11, 622)
        Me.lblID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(74, 17)
        Me.lblID.TabIndex = 161
        Me.lblID.Text = "MachineID"
        '
        'rtf
        '
        Me.rtf.Location = New System.Drawing.Point(381, 886)
        Me.rtf.Margin = New System.Windows.Forms.Padding(4)
        Me.rtf.Name = "rtf"
        Me.rtf.Size = New System.Drawing.Size(88, 78)
        Me.rtf.TabIndex = 98
        Me.rtf.Text = ""
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(11, 288)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(404, 52)
        Me.Label11.TabIndex = 123
        Me.Label11.Text = "Acceptable size of work assignment and work result packets (bigger units may have" & _
            " large memory demands) -- 'small' is <5MB, 'normal' is <10MB, and 'big' is >10MB" & _
            " "
        '
        'nudMem
        '
        Me.nudMem.Location = New System.Drawing.Point(415, 482)
        Me.nudMem.Margin = New System.Windows.Forms.Padding(4)
        Me.nudMem.Maximum = New Decimal(New Integer() {8500, 0, 0, 0})
        Me.nudMem.Minimum = New Decimal(New Integer() {64, 0, 0, 0})
        Me.nudMem.Name = "nudMem"
        Me.nudMem.Size = New System.Drawing.Size(113, 22)
        Me.nudMem.TabIndex = 18
        Me.nudMem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudMem.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.nudMem.Value = New Decimal(New Integer() {64, 0, 0, 0})
        '
        'cmbCorePriority
        '
        Me.cmbCorePriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCorePriority.FormattingEnabled = True
        Me.cmbCorePriority.Items.AddRange(New Object() {"low", "idle"})
        Me.cmbCorePriority.Location = New System.Drawing.Point(195, 351)
        Me.cmbCorePriority.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbCorePriority.Name = "cmbCorePriority"
        Me.cmbCorePriority.Size = New System.Drawing.Size(85, 24)
        Me.cmbCorePriority.TabIndex = 13
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(291, 354)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(144, 17)
        Me.Label16.TabIndex = 139
        Me.Label16.Text = "Cpu usage requested"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(11, 354)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(187, 20)
        Me.Label12.TabIndex = 131
        Me.Label12.Text = "Core Priority "
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(11, 802)
        Me.cmdCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(92, 26)
        Me.cmdCancel.TabIndex = 25
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(11, 485)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(178, 17)
        Me.Label14.TabIndex = 130
        Me.Label14.Text = "Memory, in MB, to indicate "
        '
        'cmbProxy
        '
        Me.cmbProxy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProxy.FormattingEnabled = True
        Me.cmbProxy.Items.AddRange(New Object() {"yes", "no"})
        Me.cmbProxy.Location = New System.Drawing.Point(120, 114)
        Me.cmbProxy.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbProxy.Name = "cmbProxy"
        Me.cmbProxy.Size = New System.Drawing.Size(65, 24)
        Me.cmbProxy.TabIndex = 5
        '
        'nudInterval
        '
        Me.nudInterval.Location = New System.Drawing.Point(461, 450)
        Me.nudInterval.Margin = New System.Windows.Forms.Padding(4)
        Me.nudInterval.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.nudInterval.Minimum = New Decimal(New Integer() {3, 0, 0, 0})
        Me.nudInterval.Name = "nudInterval"
        Me.nudInterval.Size = New System.Drawing.Size(67, 22)
        Me.nudInterval.TabIndex = 17
        Me.nudInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudInterval.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        Me.nudInterval.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'cmbService
        '
        Me.cmbService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbService.FormattingEnabled = True
        Me.cmbService.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbService.Location = New System.Drawing.Point(443, 257)
        Me.cmbService.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbService.Name = "cmbService"
        Me.cmbService.Size = New System.Drawing.Size(85, 24)
        Me.cmbService.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 453)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(270, 17)
        Me.Label13.TabIndex = 136
        Me.Label13.Text = "Interval, in minutes, between checkpoints "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 118)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 17)
        Me.Label1.TabIndex = 143
        Me.Label1.Text = "Use proxy"
        '
        'cmdNext
        '
        Me.cmdNext.Location = New System.Drawing.Point(445, 802)
        Me.cmdNext.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(79, 26)
        Me.cmdNext.TabIndex = 26
        Me.cmdNext.Text = "Ok"
        Me.cmdNext.UseVisualStyleBackColor = True
        '
        'cmbBattery
        '
        Me.cmbBattery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBattery.FormattingEnabled = True
        Me.cmbBattery.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbBattery.Location = New System.Drawing.Point(463, 417)
        Me.cmbBattery.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbBattery.Name = "cmbBattery"
        Me.cmbBattery.Size = New System.Drawing.Size(65, 24)
        Me.cmbBattery.TabIndex = 16
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(120, 19)
        Me.txtUserName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(227, 22)
        Me.txtUserName.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(349, 22)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(96, 17)
        Me.Label7.TabIndex = 119
        Me.Label7.Text = "Team number"
        '
        'nudCPUusage
        '
        Me.nudCPUusage.Location = New System.Drawing.Point(441, 352)
        Me.nudCPUusage.Margin = New System.Windows.Forms.Padding(4)
        Me.nudCPUusage.Name = "nudCPUusage"
        Me.nudCPUusage.Size = New System.Drawing.Size(87, 22)
        Me.nudCPUusage.TabIndex = 14
        Me.nudCPUusage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudCPUusage.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 22)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 17)
        Me.Label9.TabIndex = 118
        Me.Label9.Text = "Username"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(11, 514)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(379, 37)
        Me.Label15.TabIndex = 128
        Me.Label15.Text = "Set -advmethods flag always, requesting new advanced" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " scientific cores and/or wo" & _
            "rk units if available"
        '
        'cmbAdv
        '
        Me.cmbAdv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAdv.FormattingEnabled = True
        Me.cmbAdv.Items.AddRange(New Object() {"yes", "no"})
        Me.cmbAdv.Location = New System.Drawing.Point(463, 526)
        Me.cmbAdv.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbAdv.Name = "cmbAdv"
        Me.cmbAdv.Size = New System.Drawing.Size(65, 24)
        Me.cmbAdv.TabIndex = 19
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(11, 421)
        Me.Label24.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(360, 17)
        Me.Label24.TabIndex = 126
        Me.Label24.Text = "Pause if battery power is being used (useful for laptops)"
        '
        'cmbDisableAssembly
        '
        Me.cmbDisableAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDisableAssembly.FormattingEnabled = True
        Me.cmbDisableAssembly.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbDisableAssembly.Location = New System.Drawing.Point(463, 384)
        Me.cmbDisableAssembly.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbDisableAssembly.Name = "cmbDisableAssembly"
        Me.cmbDisableAssembly.Size = New System.Drawing.Size(65, 24)
        Me.cmbDisableAssembly.TabIndex = 15
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(11, 388)
        Me.Label26.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(258, 17)
        Me.Label26.TabIndex = 125
        Me.Label26.Text = "Disable highly optimized assembly code"
        '
        'txtTeamNumber
        '
        Me.txtTeamNumber.Location = New System.Drawing.Point(452, 19)
        Me.txtTeamNumber.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTeamNumber.Name = "txtTeamNumber"
        Me.txtTeamNumber.Size = New System.Drawing.Size(76, 22)
        Me.txtTeamNumber.TabIndex = 2
        '
        'txtParam
        '
        Me.txtParam.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtParam.Location = New System.Drawing.Point(11, 756)
        Me.txtParam.Margin = New System.Windows.Forms.Padding(4)
        Me.txtParam.Name = "txtParam"
        Me.txtParam.Size = New System.Drawing.Size(517, 22)
        Me.txtParam.TabIndex = 24
        '
        'txtIP
        '
        Me.txtIP.Location = New System.Drawing.Point(315, 690)
        Me.txtIP.Margin = New System.Windows.Forms.Padding(4)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(213, 22)
        Me.txtIP.TabIndex = 23
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(11, 694)
        Me.Label25.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(245, 17)
        Me.Label25.TabIndex = 157
        Me.Label25.Text = "IP address to bind core to (for viewer)"
        '
        'cmbCpuAffinity
        '
        Me.cmbCpuAffinity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCpuAffinity.FormattingEnabled = True
        Me.cmbCpuAffinity.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbCpuAffinity.Location = New System.Drawing.Point(463, 657)
        Me.cmbCpuAffinity.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbCpuAffinity.Name = "cmbCpuAffinity"
        Me.cmbCpuAffinity.Size = New System.Drawing.Size(65, 24)
        Me.cmbCpuAffinity.TabIndex = 22
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 661)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(161, 17)
        Me.Label3.TabIndex = 155
        Me.Label3.Text = "Disable CPU affinity lock"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 54)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 17)
        Me.Label2.TabIndex = 154
        Me.Label2.Text = "Passkey"
        '
        'txtPassKey
        '
        Me.txtPassKey.Location = New System.Drawing.Point(120, 51)
        Me.txtPassKey.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPassKey.Name = "txtPassKey"
        Me.txtPassKey.Size = New System.Drawing.Size(408, 22)
        Me.txtPassKey.TabIndex = 3
        '
        'cmbProxyPassword
        '
        Me.cmbProxyPassword.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProxyPassword.FormattingEnabled = True
        Me.cmbProxyPassword.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbProxyPassword.Location = New System.Drawing.Point(463, 114)
        Me.cmbProxyPassword.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbProxyPassword.Name = "cmbProxyPassword"
        Me.cmbProxyPassword.Size = New System.Drawing.Size(65, 24)
        Me.cmbProxyPassword.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(221, 118)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(158, 17)
        Me.Label8.TabIndex = 147
        Me.Label8.Text = "Use proy authentication"
        '
        'cmbAskNet
        '
        Me.cmbAskNet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAskNet.FormattingEnabled = True
        Me.cmbAskNet.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbAskNet.Location = New System.Drawing.Point(463, 79)
        Me.cmbAskNet.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbAskNet.Name = "cmbAskNet"
        Me.cmbAskNet.Size = New System.Drawing.Size(65, 24)
        Me.cmbAskNet.TabIndex = 4
        '
        'lblService
        '
        Me.lblService.AutoSize = True
        Me.lblService.Location = New System.Drawing.Point(11, 261)
        Me.lblService.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblService.Name = "lblService"
        Me.lblService.Size = New System.Drawing.Size(377, 17)
        Me.lblService.TabIndex = 122
        Me.lblService.Text = "Launch automatically, install as a service in this directory ?"
        '
        'cmbBigWu
        '
        Me.cmbBigWu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBigWu.FormattingEnabled = True
        Me.cmbBigWu.Items.AddRange(New Object() {"small", "normal", "big"})
        Me.cmbBigWu.Location = New System.Drawing.Point(443, 314)
        Me.cmbBigWu.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbBigWu.Name = "cmbBigWu"
        Me.cmbBigWu.Size = New System.Drawing.Size(85, 24)
        Me.cmbBigWu.TabIndex = 12
        '
        'cmbLocalDeadlines
        '
        Me.cmbLocalDeadlines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalDeadlines.FormattingEnabled = True
        Me.cmbLocalDeadlines.Items.AddRange(New Object() {"no", "yes"})
        Me.cmbLocalDeadlines.Location = New System.Drawing.Point(463, 578)
        Me.cmbLocalDeadlines.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbLocalDeadlines.Name = "cmbLocalDeadlines"
        Me.cmbLocalDeadlines.Size = New System.Drawing.Size(65, 24)
        Me.cmbLocalDeadlines.TabIndex = 20
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(11, 570)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(425, 34)
        Me.Label17.TabIndex = 140
        Me.Label17.Text = "Ignore any deadline information (mainly useful if system clock frequently has err" & _
            "ors)"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(11, 89)
        Me.Label21.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(225, 17)
        Me.Label21.TabIndex = 145
        Me.Label21.Text = "Ask before fetching/sending work?"
        '
        'lblS
        '
        Me.lblS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblS.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblS.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblS.Location = New System.Drawing.Point(4, 836)
        Me.lblS.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblS.Name = "lblS"
        Me.lblS.Size = New System.Drawing.Size(531, 23)
        Me.lblS.TabIndex = 152
        Me.lblS.Text = "-idle-"
        Me.lblS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbProxy
        '
        Me.gbProxy.Controls.Add(Me.gbProxyPassword)
        Me.gbProxy.Controls.Add(Me.txtProxyPort)
        Me.gbProxy.Controls.Add(Me.txtProxyName)
        Me.gbProxy.Controls.Add(Me.Label5)
        Me.gbProxy.Controls.Add(Me.Label4)
        Me.gbProxy.Location = New System.Drawing.Point(8, 144)
        Me.gbProxy.Margin = New System.Windows.Forms.Padding(4)
        Me.gbProxy.Name = "gbProxy"
        Me.gbProxy.Padding = New System.Windows.Forms.Padding(4)
        Me.gbProxy.Size = New System.Drawing.Size(521, 106)
        Me.gbProxy.TabIndex = 142
        Me.gbProxy.TabStop = False
        Me.gbProxy.Text = "Proxy settings"
        '
        'gbProxyPassword
        '
        Me.gbProxyPassword.Controls.Add(Me.txtPpassword)
        Me.gbProxyPassword.Controls.Add(Me.txtPusername)
        Me.gbProxyPassword.Controls.Add(Me.Label20)
        Me.gbProxyPassword.Controls.Add(Me.Label19)
        Me.gbProxyPassword.Location = New System.Drawing.Point(261, 22)
        Me.gbProxyPassword.Margin = New System.Windows.Forms.Padding(4)
        Me.gbProxyPassword.Name = "gbProxyPassword"
        Me.gbProxyPassword.Padding = New System.Windows.Forms.Padding(4)
        Me.gbProxyPassword.Size = New System.Drawing.Size(252, 75)
        Me.gbProxyPassword.TabIndex = 97
        Me.gbProxyPassword.TabStop = False
        '
        'txtPpassword
        '
        Me.txtPpassword.Location = New System.Drawing.Point(84, 44)
        Me.txtPpassword.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPpassword.Name = "txtPpassword"
        Me.txtPpassword.Size = New System.Drawing.Size(159, 22)
        Me.txtPpassword.TabIndex = 10
        '
        'txtPusername
        '
        Me.txtPusername.Location = New System.Drawing.Point(84, 12)
        Me.txtPusername.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPusername.Name = "txtPusername"
        Me.txtPusername.Size = New System.Drawing.Size(159, 22)
        Me.txtPusername.TabIndex = 9
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(5, 48)
        Me.Label20.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(69, 17)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "Password"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(5, 16)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(73, 17)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Username"
        '
        'txtProxyPort
        '
        Me.txtProxyPort.Location = New System.Drawing.Point(97, 65)
        Me.txtProxyPort.Margin = New System.Windows.Forms.Padding(4)
        Me.txtProxyPort.Name = "txtProxyPort"
        Me.txtProxyPort.Size = New System.Drawing.Size(155, 22)
        Me.txtProxyPort.TabIndex = 8
        '
        'txtProxyName
        '
        Me.txtProxyName.Location = New System.Drawing.Point(97, 33)
        Me.txtProxyName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtProxyName.Name = "txtProxyName"
        Me.txtProxyName.Size = New System.Drawing.Size(155, 22)
        Me.txtProxyName.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 69)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 17)
        Me.Label5.TabIndex = 92
        Me.Label5.Text = "Proxy port"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 37)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 17)
        Me.Label4.TabIndex = 91
        Me.Label4.Text = "Proxy name"
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(547, 859)
        Me.ControlBox = False
        Me.Controls.Add(Me.gbC)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmConfig"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Client configuration"
        Me.TopMost = True
        Me.gbC.ResumeLayout(False)
        Me.gbC.PerformLayout()
        CType(Me.nudMem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCPUusage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbProxy.ResumeLayout(False)
        Me.gbProxy.PerformLayout()
        Me.gbProxyPassword.ResumeLayout(False)
        Me.gbProxyPassword.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbC As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents nudMem As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmbCorePriority As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents cmbProxy As System.Windows.Forms.ComboBox
    Friend WithEvents nudInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmbService As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdNext As System.Windows.Forms.Button
    Friend WithEvents cmbBattery As System.Windows.Forms.ComboBox
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents nudCPUusage As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cmbAdv As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents cmbDisableAssembly As System.Windows.Forms.ComboBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtTeamNumber As System.Windows.Forms.TextBox
    Friend WithEvents txtParam As System.Windows.Forms.TextBox
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents cmbCpuAffinity As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPassKey As System.Windows.Forms.TextBox
    Friend WithEvents cmbProxyPassword As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbAskNet As System.Windows.Forms.ComboBox
    Friend WithEvents lblService As System.Windows.Forms.Label
    Friend WithEvents cmbBigWu As System.Windows.Forms.ComboBox
    Friend WithEvents cmbLocalDeadlines As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lblS As System.Windows.Forms.Label
    Friend WithEvents gbProxy As System.Windows.Forms.GroupBox
    Friend WithEvents gbProxyPassword As System.Windows.Forms.GroupBox
    Friend WithEvents txtPpassword As System.Windows.Forms.TextBox
    Friend WithEvents txtPusername As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtProxyPort As System.Windows.Forms.TextBox
    Friend WithEvents txtProxyName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
    Friend WithEvents cmbMachineID As System.Windows.Forms.ComboBox
    Friend WithEvents lblID As System.Windows.Forms.Label
    Friend WithEvents llblLog As System.Windows.Forms.LinkLabel
    Friend WithEvents lnblCores As System.Windows.Forms.LinkLabel
    Friend WithEvents cmbCores As System.Windows.Forms.ComboBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
End Class
