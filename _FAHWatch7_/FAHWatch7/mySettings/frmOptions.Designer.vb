<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.chkMinimizeToTray = New System.Windows.Forms.CheckBox()
        Me.chkAutoRun = New System.Windows.Forms.CheckBox()
        Me.lvRClients = New System.Windows.Forms.ListView()
        Me.chName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chLocation = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chkSysTrayStart = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdRemove = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.chkDisableAutoDownloadSummary = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdLegacyClients = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbDefaultView = New System.Windows.Forms.ComboBox()
        Me.txtLocalClient = New System.Windows.Forms.TextBox()
        Me.chkStartFC = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cmbSummaryUrl = New System.Windows.Forms.ComboBox()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkMinimizeToTray
        '
        Me.chkMinimizeToTray.AutoSize = True
        Me.chkMinimizeToTray.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkMinimizeToTray.Location = New System.Drawing.Point(221, 117)
        Me.chkMinimizeToTray.Name = "chkMinimizeToTray"
        Me.chkMinimizeToTray.Size = New System.Drawing.Size(133, 17)
        Me.chkMinimizeToTray.TabIndex = 4
        Me.chkMinimizeToTray.Text = "Minimize to system tray"
        Me.tTip.SetToolTip(Me.chkMinimizeToTray, "When checked, minimized forms will move to the system tray ( always checked if st" & _
        "arting from the system tray )")
        Me.chkMinimizeToTray.UseVisualStyleBackColor = True
        '
        'chkAutoRun
        '
        Me.chkAutoRun.AutoSize = True
        Me.chkAutoRun.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkAutoRun.Location = New System.Drawing.Point(18, 94)
        Me.chkAutoRun.Name = "chkAutoRun"
        Me.chkAutoRun.Size = New System.Drawing.Size(151, 17)
        Me.chkAutoRun.TabIndex = 1
        Me.chkAutoRun.Text = "Start FAHWatch7 at logon"
        Me.tTip.SetToolTip(Me.chkAutoRun, "Checking this will add FAHWatch7 to the applications which will be started at log" & _
        "on")
        Me.chkAutoRun.UseVisualStyleBackColor = True
        '
        'lvRClients
        '
        Me.lvRClients.CheckBoxes = True
        Me.lvRClients.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chName, Me.chLocation})
        Me.lvRClients.FullRowSelect = True
        Me.lvRClients.GridLines = True
        Me.lvRClients.HideSelection = False
        Me.lvRClients.Location = New System.Drawing.Point(18, 19)
        Me.lvRClients.MultiSelect = False
        Me.lvRClients.Name = "lvRClients"
        Me.lvRClients.Size = New System.Drawing.Size(359, 95)
        Me.lvRClients.TabIndex = 5
        Me.tTip.SetToolTip(Me.lvRClients, "This list contains your remote clients, uncheck a client to disable it")
        Me.lvRClients.UseCompatibleStateImageBehavior = False
        Me.lvRClients.View = System.Windows.Forms.View.Details
        '
        'chName
        '
        Me.chName.Text = "Name"
        Me.chName.Width = 113
        '
        'chLocation
        '
        Me.chLocation.Text = "Location"
        Me.chLocation.Width = 239
        '
        'chkSysTrayStart
        '
        Me.chkSysTrayStart.AutoSize = True
        Me.chkSysTrayStart.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkSysTrayStart.Location = New System.Drawing.Point(18, 117)
        Me.chkSysTrayStart.Name = "chkSysTrayStart"
        Me.chkSysTrayStart.Size = New System.Drawing.Size(144, 17)
        Me.chkSysTrayStart.TabIndex = 2
        Me.chkSysTrayStart.Text = "Start from the system tray"
        Me.tTip.SetToolTip(Me.chkSysTrayStart, "When checked FAHClient will start from the system tray")
        Me.chkSysTrayStart.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label7.Location = New System.Drawing.Point(13, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Default PSummary URL"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(15, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Default mode:"
        '
        'cmdRemove
        '
        Me.cmdRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdRemove.Location = New System.Drawing.Point(99, 118)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(75, 23)
        Me.cmdRemove.TabIndex = 7
        Me.cmdRemove.Text = "Remove"
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(218, 46)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(164, 18)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "*used for remote hw monitoring"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(15, 46)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(87, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "FAHWatch7 port"
        '
        'cmdAdd
        '
        Me.cmdAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdAdd.Location = New System.Drawing.Point(18, 118)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(75, 23)
        Me.cmdAdd.TabIndex = 6
        Me.cmdAdd.Text = "Add"
        Me.tTip.SetToolTip(Me.cmdAdd, "Open the Add Client dialog ( ctrl + mouseclick opens simple browser )")
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'txtPort
        '
        Me.txtPort.Enabled = False
        Me.txtPort.Location = New System.Drawing.Point(111, 42)
        Me.txtPort.MaxLength = 5
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(100, 20)
        Me.txtPort.TabIndex = 1
        Me.txtPort.TabStop = False
        Me.txtPort.Text = "49153"
        Me.tTip.SetToolTip(Me.txtPort, "FAHWatch7 will communicate with other instances over this port to facilitate remo" & _
        "te hardware monitoring")
        '
        'chkDisableAutoDownloadSummary
        '
        Me.chkDisableAutoDownloadSummary.AutoSize = True
        Me.chkDisableAutoDownloadSummary.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkDisableAutoDownloadSummary.Location = New System.Drawing.Point(18, 58)
        Me.chkDisableAutoDownloadSummary.Name = "chkDisableAutoDownloadSummary"
        Me.chkDisableAutoDownloadSummary.Size = New System.Drawing.Size(253, 17)
        Me.chkDisableAutoDownloadSummary.TabIndex = 10
        Me.chkDisableAutoDownloadSummary.Text = "Disable downloading new definitions automaticly"
        Me.tTip.SetToolTip(Me.chkDisableAutoDownloadSummary, "Check this to disable automatic project information updates")
        Me.chkDisableAutoDownloadSummary.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdLegacyClients)
        Me.GroupBox1.Controls.Add(Me.lvRClients)
        Me.GroupBox1.Controls.Add(Me.cmdRemove)
        Me.GroupBox1.Controls.Add(Me.cmdAdd)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 142)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(389, 147)
        Me.GroupBox1.TabIndex = 26
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Remote clients"
        '
        'cmdLegacyClients
        '
        Me.cmdLegacyClients.Enabled = False
        Me.cmdLegacyClients.Location = New System.Drawing.Point(280, 118)
        Me.cmdLegacyClients.Name = "cmdLegacyClients"
        Me.cmdLegacyClients.Size = New System.Drawing.Size(97, 23)
        Me.cmdLegacyClients.TabIndex = 8
        Me.cmdLegacyClients.Text = "Legacy clients"
        Me.tTip.SetToolTip(Me.cmdLegacyClients, "Manage legacy (V6) clients")
        Me.cmdLegacyClients.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkMinimizeToTray)
        Me.GroupBox2.Controls.Add(Me.chkSysTrayStart)
        Me.GroupBox2.Controls.Add(Me.chkAutoRun)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtPort)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.cmbDefaultView)
        Me.GroupBox2.Controls.Add(Me.txtLocalClient)
        Me.GroupBox2.Controls.Add(Me.chkStartFC)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 1)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(388, 137)
        Me.GroupBox2.TabIndex = 22
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Local FAHClient && FAHWatch7 options"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(15, 22)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Name"
        '
        'cmbDefaultView
        '
        Me.cmbDefaultView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDefaultView.Items.AddRange(New Object() {"historical", "live"})
        Me.cmbDefaultView.Location = New System.Drawing.Point(99, 67)
        Me.cmbDefaultView.Name = "cmbDefaultView"
        Me.cmbDefaultView.Size = New System.Drawing.Size(112, 21)
        Me.cmbDefaultView.TabIndex = 0
        Me.tTip.SetToolTip(Me.cmbDefaultView, "Default mode determins which form is shown when left clicking on the tray icon, a" & _
        "nd which form is shown at startup ( unless 'Start from system tray' is checked )" & _
        "")
        '
        'txtLocalClient
        '
        Me.txtLocalClient.Location = New System.Drawing.Point(111, 19)
        Me.txtLocalClient.Name = "txtLocalClient"
        Me.txtLocalClient.ReadOnly = True
        Me.txtLocalClient.Size = New System.Drawing.Size(271, 20)
        Me.txtLocalClient.TabIndex = 0
        Me.txtLocalClient.TabStop = False
        '
        'chkStartFC
        '
        Me.chkStartFC.AutoSize = True
        Me.chkStartFC.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkStartFC.Location = New System.Drawing.Point(221, 94)
        Me.chkStartFC.Name = "chkStartFC"
        Me.chkStartFC.Size = New System.Drawing.Size(98, 17)
        Me.chkStartFC.TabIndex = 3
        Me.chkStartFC.Text = "Start FAHClient"
        Me.tTip.SetToolTip(Me.chkStartFC, "If FAHWatch7 starts, it will start FAHClient if not already running")
        Me.chkStartFC.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmbSummaryUrl)
        Me.GroupBox3.Controls.Add(Me.chkDisableAutoDownloadSummary)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 295)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(387, 82)
        Me.GroupBox3.TabIndex = 25
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "pSummary options"
        '
        'cmbSummaryUrl
        '
        Me.cmbSummaryUrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSummaryUrl.FormattingEnabled = True
        Me.cmbSummaryUrl.Location = New System.Drawing.Point(18, 33)
        Me.cmbSummaryUrl.Name = "cmbSummaryUrl"
        Me.cmbSummaryUrl.Size = New System.Drawing.Size(357, 21)
        Me.cmbSummaryUrl.TabIndex = 9
        Me.tTip.SetToolTip(Me.cmbSummaryUrl, "Choose your default pSummary url to be used to download new project definitions")
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdCancel.Location = New System.Drawing.Point(6, 383)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.Text = "&Cancel"
        Me.tTip.SetToolTip(Me.cmdCancel, "Cancel setup")
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cmdOK.Location = New System.Drawing.Point(318, 383)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 12
        Me.cmdOK.Text = "&Accept"
        Me.tTip.SetToolTip(Me.cmdOK, "Accept settings")
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'tTip
        '
        Me.tTip.AutomaticDelay = 250
        Me.tTip.AutoPopDelay = 5000
        Me.tTip.InitialDelay = 250
        Me.tTip.ReshowDelay = 50
        Me.tTip.ShowAlways = True
        '
        'frmOptions
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(402, 411)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.Text = "Configure FAHWatch7"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkMinimizeToTray As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoRun As System.Windows.Forms.CheckBox
    Friend WithEvents lvRClients As System.Windows.Forms.ListView
    Friend WithEvents chName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chLocation As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkSysTrayStart As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents chkDisableAutoDownloadSummary As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbDefaultView As System.Windows.Forms.ComboBox
    Friend WithEvents txtLocalClient As System.Windows.Forms.TextBox
    Friend WithEvents chkStartFC As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmbSummaryUrl As System.Windows.Forms.ComboBox
    Friend WithEvents cmdLegacyClients As System.Windows.Forms.Button
    Private WithEvents tTip As System.Windows.Forms.ToolTip
End Class
