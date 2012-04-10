<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DiagGaugeSettings
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.nudBorder = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbSound = New System.Windows.Forms.ComboBox()
        Me.lblBorder = New System.Windows.Forms.Label()
        Me.cmdBorder = New System.Windows.Forms.Button()
        Me.nudCorner = New System.Windows.Forms.NumericUpDown()
        Me.chkWarningsound = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkWarningIcon = New System.Windows.Forms.CheckBox()
        Me.nudVsize = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.cmdIcon = New System.Windows.Forms.Button()
        Me.chkIcon = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Gauge = New mtmGauge()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.nudValue = New System.Windows.Forms.NumericUpDown()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nudStepSmall = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nudStepLarge = New System.Windows.Forms.NumericUpDown()
        Me.lblMinMax = New System.Windows.Forms.Label()
        Me.cmdMinMax = New System.Windows.Forms.Button()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.lblEarning = New System.Windows.Forms.Label()
        Me.lblRange = New System.Windows.Forms.Label()
        Me.lblBack = New System.Windows.Forms.Label()
        Me.nudNormal = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudWarning = New System.Windows.Forms.NumericUpDown()
        Me.nudMax = New System.Windows.Forms.NumericUpDown()
        Me.nudMin = New System.Windows.Forms.NumericUpDown()
        Me.cmdRange = New System.Windows.Forms.Button()
        Me.cmdValue = New System.Windows.Forms.Button()
        Me.cmdWarning = New System.Windows.Forms.Button()
        Me.cmdBack = New System.Windows.Forms.Button()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.nudBorder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudVsize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudValue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStepSmall, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStepLarge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNormal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(336, 258)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.nudBorder)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.cmbSound)
        Me.GroupBox2.Controls.Add(Me.lblBorder)
        Me.GroupBox2.Controls.Add(Me.cmdBorder)
        Me.GroupBox2.Controls.Add(Me.nudCorner)
        Me.GroupBox2.Controls.Add(Me.chkWarningsound)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.chkWarningIcon)
        Me.GroupBox2.Controls.Add(Me.nudVsize)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.pbIcon)
        Me.GroupBox2.Controls.Add(Me.cmdIcon)
        Me.GroupBox2.Controls.Add(Me.chkIcon)
        Me.GroupBox2.Controls.Add(Me.GroupBox1)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.nudStepSmall)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.nudStepLarge)
        Me.GroupBox2.Controls.Add(Me.lblMinMax)
        Me.GroupBox2.Controls.Add(Me.cmdMinMax)
        Me.GroupBox2.Controls.Add(Me.lblValue)
        Me.GroupBox2.Controls.Add(Me.lblEarning)
        Me.GroupBox2.Controls.Add(Me.lblRange)
        Me.GroupBox2.Controls.Add(Me.lblBack)
        Me.GroupBox2.Controls.Add(Me.nudNormal)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.nudWarning)
        Me.GroupBox2.Controls.Add(Me.nudMax)
        Me.GroupBox2.Controls.Add(Me.nudMin)
        Me.GroupBox2.Controls.Add(Me.cmdRange)
        Me.GroupBox2.Controls.Add(Me.cmdValue)
        Me.GroupBox2.Controls.Add(Me.cmdWarning)
        Me.GroupBox2.Controls.Add(Me.cmdBack)
        Me.GroupBox2.Location = New System.Drawing.Point(2, 2)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox2.Size = New System.Drawing.Size(478, 250)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        '
        'nudBorder
        '
        Me.nudBorder.Location = New System.Drawing.Point(354, 171)
        Me.nudBorder.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudBorder.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudBorder.Name = "nudBorder"
        Me.nudBorder.Size = New System.Drawing.Size(45, 20)
        Me.nudBorder.TabIndex = 97
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(290, 174)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 13)
        Me.Label9.TabIndex = 96
        Me.Label9.Text = "Border"
        '
        'cmbSound
        '
        Me.cmbSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSound.FormattingEnabled = True
        Me.cmbSound.Location = New System.Drawing.Point(211, 176)
        Me.cmbSound.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmbSound.Name = "cmbSound"
        Me.cmbSound.Size = New System.Drawing.Size(68, 21)
        Me.cmbSound.TabIndex = 89
        '
        'lblBorder
        '
        Me.lblBorder.AutoSize = True
        Me.lblBorder.Location = New System.Drawing.Point(94, 134)
        Me.lblBorder.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBorder.Name = "lblBorder"
        Me.lblBorder.Size = New System.Drawing.Size(39, 13)
        Me.lblBorder.TabIndex = 94
        Me.lblBorder.Text = "Label9"
        '
        'cmdBorder
        '
        Me.cmdBorder.Location = New System.Drawing.Point(6, 131)
        Me.cmdBorder.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdBorder.Name = "cmdBorder"
        Me.cmdBorder.Size = New System.Drawing.Size(74, 20)
        Me.cmdBorder.TabIndex = 93
        Me.cmdBorder.Text = "Border color"
        Me.cmdBorder.UseVisualStyleBackColor = True
        '
        'nudCorner
        '
        Me.nudCorner.Location = New System.Drawing.Point(354, 151)
        Me.nudCorner.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudCorner.Maximum = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudCorner.Name = "nudCorner"
        Me.nudCorner.Size = New System.Drawing.Size(45, 20)
        Me.nudCorner.TabIndex = 92
        '
        'chkWarningsound
        '
        Me.chkWarningsound.AutoSize = True
        Me.chkWarningsound.Location = New System.Drawing.Point(8, 178)
        Me.chkWarningsound.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkWarningsound.Name = "chkWarningsound"
        Me.chkWarningsound.Size = New System.Drawing.Size(188, 17)
        Me.chkWarningsound.TabIndex = 1
        Me.chkWarningsound.Text = "Play sound when in warning range"
        Me.chkWarningsound.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(290, 154)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 13)
        Me.Label8.TabIndex = 91
        Me.Label8.Text = "Corner"
        '
        'chkWarningIcon
        '
        Me.chkWarningIcon.AutoSize = True
        Me.chkWarningIcon.Location = New System.Drawing.Point(8, 156)
        Me.chkWarningIcon.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkWarningIcon.Name = "chkWarningIcon"
        Me.chkWarningIcon.Size = New System.Drawing.Size(226, 17)
        Me.chkWarningIcon.TabIndex = 0
        Me.chkWarningIcon.Text = "Show warning icon when in warning range"
        Me.chkWarningIcon.UseVisualStyleBackColor = True
        '
        'nudVsize
        '
        Me.nudVsize.Location = New System.Drawing.Point(354, 131)
        Me.nudVsize.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudVsize.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.nudVsize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudVsize.Name = "nudVsize"
        Me.nudVsize.Size = New System.Drawing.Size(45, 20)
        Me.nudVsize.TabIndex = 88
        Me.nudVsize.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(290, 132)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 87
        Me.Label5.Text = "value size"
        '
        'pbIcon
        '
        Me.pbIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbIcon.Location = New System.Drawing.Point(211, 201)
        Me.pbIcon.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(36, 39)
        Me.pbIcon.TabIndex = 86
        Me.pbIcon.TabStop = False
        '
        'cmdIcon
        '
        Me.cmdIcon.Location = New System.Drawing.Point(106, 220)
        Me.cmdIcon.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdIcon.Name = "cmdIcon"
        Me.cmdIcon.Size = New System.Drawing.Size(74, 20)
        Me.cmdIcon.TabIndex = 85
        Me.cmdIcon.Text = "Select file"
        Me.cmdIcon.UseVisualStyleBackColor = True
        '
        'chkIcon
        '
        Me.chkIcon.AutoSize = True
        Me.chkIcon.Location = New System.Drawing.Point(8, 199)
        Me.chkIcon.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkIcon.Name = "chkIcon"
        Me.chkIcon.Size = New System.Drawing.Size(177, 17)
        Me.chkIcon.TabIndex = 83
        Me.chkIcon.Text = "Use custom icon ( max 48x48p )"
        Me.chkIcon.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackgroundImage = Global.ucHWM.My.Resources.Resources.pCarbon
        Me.GroupBox1.Controls.Add(Me.Gauge)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.nudValue)
        Me.GroupBox1.Controls.Add(Me.cmdReset)
        Me.GroupBox1.Location = New System.Drawing.Point(404, 11)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(65, 240)
        Me.GroupBox1.TabIndex = 82
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Example"
        '
        'Gauge
        '
        Me.Gauge.BackColor = System.Drawing.Color.Transparent
        Me.Gauge.Border = 0
        Me.Gauge.borderColor = System.Drawing.Color.Black
        Me.Gauge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Gauge.colorBack = System.Drawing.Color.White
        Me.Gauge.colorCurrent = System.Drawing.Color.Blue
        Me.Gauge.colorMinMax = System.Drawing.Color.DarkGray
        Me.Gauge.colorRange = System.Drawing.Color.Yellow
        Me.Gauge.colorWarning = System.Drawing.Color.Red
        Me.Gauge.iCorner = 10
        Me.Gauge.Location = New System.Drawing.Point(16, 23)
        Me.Gauge.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Gauge.maxValue = 0
        Me.Gauge.minValue = 0
        Me.Gauge.Name = "Gauge"
        Me.Gauge.normalMin = 0
        Me.Gauge.Size = New System.Drawing.Size(28, 82)
        Me.Gauge.StepIntervalLarge = 0
        Me.Gauge.StepIntervalSmall = 0
        Me.Gauge.TabIndex = 0
        Me.Gauge.vSize = 6
        Me.Gauge.warningStart = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(10, 120)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(44, 20)
        Me.TextBox1.TabIndex = 74
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(10, 143)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(44, 20)
        Me.TextBox2.TabIndex = 75
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(10, 166)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(44, 20)
        Me.TextBox3.TabIndex = 76
        '
        'nudValue
        '
        Me.nudValue.Location = New System.Drawing.Point(10, 188)
        Me.nudValue.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudValue.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.nudValue.Name = "nudValue"
        Me.nudValue.Size = New System.Drawing.Size(43, 20)
        Me.nudValue.TabIndex = 58
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReset.Location = New System.Drawing.Point(10, 211)
        Me.cmdReset.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdReset.Name = "cmdReset"
        Me.cmdReset.Size = New System.Drawing.Size(43, 20)
        Me.cmdReset.TabIndex = 77
        Me.cmdReset.Text = "Reset"
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(290, 112)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 81
        Me.Label7.Text = "StepSmall"
        '
        'nudStepSmall
        '
        Me.nudStepSmall.Location = New System.Drawing.Point(354, 110)
        Me.nudStepSmall.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudStepSmall.Maximum = New Decimal(New Integer() {500000, 0, 0, 0})
        Me.nudStepSmall.Name = "nudStepSmall"
        Me.nudStepSmall.Size = New System.Drawing.Size(45, 20)
        Me.nudStepSmall.TabIndex = 80
        Me.nudStepSmall.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(290, 93)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 13)
        Me.Label6.TabIndex = 79
        Me.Label6.Text = "StepLarge"
        '
        'nudStepLarge
        '
        Me.nudStepLarge.Location = New System.Drawing.Point(354, 91)
        Me.nudStepLarge.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudStepLarge.Maximum = New Decimal(New Integer() {500000, 0, 0, 0})
        Me.nudStepLarge.Name = "nudStepLarge"
        Me.nudStepLarge.Size = New System.Drawing.Size(45, 20)
        Me.nudStepLarge.TabIndex = 78
        Me.nudStepLarge.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblMinMax
        '
        Me.lblMinMax.AutoSize = True
        Me.lblMinMax.Location = New System.Drawing.Point(94, 109)
        Me.lblMinMax.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMinMax.Name = "lblMinMax"
        Me.lblMinMax.Size = New System.Drawing.Size(39, 13)
        Me.lblMinMax.TabIndex = 71
        Me.lblMinMax.Text = "Label9"
        '
        'cmdMinMax
        '
        Me.cmdMinMax.Location = New System.Drawing.Point(6, 106)
        Me.cmdMinMax.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdMinMax.Name = "cmdMinMax"
        Me.cmdMinMax.Size = New System.Drawing.Size(74, 20)
        Me.cmdMinMax.TabIndex = 70
        Me.cmdMinMax.Text = "vMin/Max"
        Me.cmdMinMax.UseVisualStyleBackColor = True
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(94, 85)
        Me.lblValue.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(39, 13)
        Me.lblValue.TabIndex = 68
        Me.lblValue.Text = "Label9"
        '
        'lblEarning
        '
        Me.lblEarning.AutoSize = True
        Me.lblEarning.Location = New System.Drawing.Point(94, 61)
        Me.lblEarning.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEarning.Name = "lblEarning"
        Me.lblEarning.Size = New System.Drawing.Size(39, 13)
        Me.lblEarning.TabIndex = 67
        Me.lblEarning.Text = "Label8"
        '
        'lblRange
        '
        Me.lblRange.AutoSize = True
        Me.lblRange.Location = New System.Drawing.Point(94, 37)
        Me.lblRange.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRange.Name = "lblRange"
        Me.lblRange.Size = New System.Drawing.Size(39, 13)
        Me.lblRange.TabIndex = 66
        Me.lblRange.Text = "Label7"
        '
        'lblBack
        '
        Me.lblBack.AutoSize = True
        Me.lblBack.Location = New System.Drawing.Point(94, 14)
        Me.lblBack.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBack.Name = "lblBack"
        Me.lblBack.Size = New System.Drawing.Size(39, 13)
        Me.lblBack.TabIndex = 65
        Me.lblBack.Text = "Label6"
        '
        'nudNormal
        '
        Me.nudNormal.Location = New System.Drawing.Point(354, 52)
        Me.nudNormal.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudNormal.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.nudNormal.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudNormal.Name = "nudNormal"
        Me.nudNormal.Size = New System.Drawing.Size(45, 20)
        Me.nudNormal.TabIndex = 64
        Me.nudNormal.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(290, 73)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 62
        Me.Label4.Text = "Warning"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(290, 54)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 61
        Me.Label3.Text = "NormalMin"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(290, 34)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 60
        Me.Label2.Text = "Max"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(290, 15)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 13)
        Me.Label1.TabIndex = 59
        Me.Label1.Text = "Min"
        '
        'nudWarning
        '
        Me.nudWarning.Location = New System.Drawing.Point(354, 72)
        Me.nudWarning.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudWarning.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.nudWarning.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudWarning.Name = "nudWarning"
        Me.nudWarning.Size = New System.Drawing.Size(45, 20)
        Me.nudWarning.TabIndex = 57
        Me.nudWarning.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'nudMax
        '
        Me.nudMax.Location = New System.Drawing.Point(354, 32)
        Me.nudMax.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudMax.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.nudMax.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMax.Name = "nudMax"
        Me.nudMax.Size = New System.Drawing.Size(45, 20)
        Me.nudMax.TabIndex = 56
        Me.nudMax.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'nudMin
        '
        Me.nudMin.Location = New System.Drawing.Point(354, 13)
        Me.nudMin.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.nudMin.Maximum = New Decimal(New Integer() {150, 0, 0, 0})
        Me.nudMin.Name = "nudMin"
        Me.nudMin.Size = New System.Drawing.Size(45, 20)
        Me.nudMin.TabIndex = 55
        '
        'cmdRange
        '
        Me.cmdRange.Location = New System.Drawing.Point(5, 34)
        Me.cmdRange.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdRange.Name = "cmdRange"
        Me.cmdRange.Size = New System.Drawing.Size(75, 20)
        Me.cmdRange.TabIndex = 54
        Me.cmdRange.Text = "rangeColor"
        Me.cmdRange.UseVisualStyleBackColor = True
        '
        'cmdValue
        '
        Me.cmdValue.Location = New System.Drawing.Point(5, 82)
        Me.cmdValue.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdValue.Name = "cmdValue"
        Me.cmdValue.Size = New System.Drawing.Size(75, 20)
        Me.cmdValue.TabIndex = 53
        Me.cmdValue.Text = "valueColor"
        Me.cmdValue.UseVisualStyleBackColor = True
        '
        'cmdWarning
        '
        Me.cmdWarning.Location = New System.Drawing.Point(5, 58)
        Me.cmdWarning.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdWarning.Name = "cmdWarning"
        Me.cmdWarning.Size = New System.Drawing.Size(75, 20)
        Me.cmdWarning.TabIndex = 52
        Me.cmdWarning.Text = "warningColor"
        Me.cmdWarning.UseVisualStyleBackColor = True
        '
        'cmdBack
        '
        Me.cmdBack.Location = New System.Drawing.Point(5, 11)
        Me.cmdBack.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cmdBack.Name = "cmdBack"
        Me.cmdBack.Size = New System.Drawing.Size(75, 20)
        Me.cmdBack.TabIndex = 51
        Me.cmdBack.Text = "backColor"
        Me.cmdBack.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'ofd
        '
        Me.ofd.DefaultExt = "Image files|*.png;*.bmp;*.gif;*.ico"
        '
        'DiagGaugeSettings
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(487, 293)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DiagGaugeSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DiagGaugeSettings"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.nudBorder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudVsize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudValue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStepSmall, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStepLarge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNormal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWarning, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents nudStepSmall As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents nudStepLarge As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmdReset As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lblMinMax As System.Windows.Forms.Label
    Friend WithEvents cmdMinMax As System.Windows.Forms.Button
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents lblEarning As System.Windows.Forms.Label
    Friend WithEvents lblRange As System.Windows.Forms.Label
    Friend WithEvents lblBack As System.Windows.Forms.Label
    Friend WithEvents nudNormal As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudValue As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudWarning As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMax As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMin As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmdRange As System.Windows.Forms.Button
    Friend WithEvents cmdValue As System.Windows.Forms.Button
    Friend WithEvents cmdWarning As System.Windows.Forms.Button
    Friend WithEvents cmdBack As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Gauge As mtmGauge
    Friend WithEvents pbIcon As System.Windows.Forms.PictureBox
    Friend WithEvents cmdIcon As System.Windows.Forms.Button
    Friend WithEvents chkIcon As System.Windows.Forms.CheckBox
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents nudVsize As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbSound As System.Windows.Forms.ComboBox
    Friend WithEvents chkWarningsound As System.Windows.Forms.CheckBox
    Friend WithEvents chkWarningIcon As System.Windows.Forms.CheckBox
    Friend WithEvents lblBorder As System.Windows.Forms.Label
    Friend WithEvents cmdBorder As System.Windows.Forms.Button
    Friend WithEvents nudCorner As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents nudBorder As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
