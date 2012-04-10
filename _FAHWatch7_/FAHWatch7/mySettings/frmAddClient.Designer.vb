<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddClient
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddClient))
        Me.sStrip = New System.Windows.Forms.StatusStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtFWVersion = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFCVersion = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFWatch_port = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkFwatchInstance = New System.Windows.Forms.CheckBox()
        Me.txtLocation = New System.Windows.Forms.TextBox()
        Me.txtPWD = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tvEnum = New System.Windows.Forms.TreeView()
        Me.cmdAccept = New System.Windows.Forms.Button()
        Me.tShow = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.sStrip.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'sStrip
        '
        Me.sStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus})
        Me.sStrip.Location = New System.Drawing.Point(0, 293)
        Me.sStrip.Name = "sStrip"
        Me.sStrip.Size = New System.Drawing.Size(689, 22)
        Me.sStrip.SizingGrip = False
        Me.sStrip.TabIndex = 3
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(0, 17)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.cmdAccept)
        Me.Panel1.Location = New System.Drawing.Point(7, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(680, 285)
        Me.Panel1.TabIndex = 4
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(5, 255)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 12
        Me.Button3.Text = "Cancel"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.tvEnum)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(669, 249)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(328, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(278, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "* if the folder seems valid values will be read automatically"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(120, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Shared FAHClient folder"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.txtFWVersion)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.txtFCVersion)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.txtFWatch_port)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.chkFwatchInstance)
        Me.Panel2.Controls.Add(Me.txtLocation)
        Me.Panel2.Controls.Add(Me.txtPWD)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.txtPort)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(328, 31)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(333, 210)
        Me.Panel2.TabIndex = 12
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 117)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(255, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "This will be checked if there is a Diagnostic.txt found"
        '
        'txtFWVersion
        '
        Me.txtFWVersion.Location = New System.Drawing.Point(254, 166)
        Me.txtFWVersion.Name = "txtFWVersion"
        Me.txtFWVersion.ReadOnly = True
        Me.txtFWVersion.Size = New System.Drawing.Size(65, 20)
        Me.txtFWVersion.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 169)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(103, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "FAHWatch7 version"
        '
        'txtFCVersion
        '
        Me.txtFCVersion.Location = New System.Drawing.Point(254, 86)
        Me.txtFCVersion.Name = "txtFCVersion"
        Me.txtFCVersion.ReadOnly = True
        Me.txtFCVersion.Size = New System.Drawing.Size(65, 20)
        Me.txtFCVersion.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 89)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(91, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "FAHClient version"
        '
        'txtFWatch_port
        '
        Me.txtFWatch_port.Location = New System.Drawing.Point(254, 141)
        Me.txtFWatch_port.Name = "txtFWatch_port"
        Me.txtFWatch_port.ReadOnly = True
        Me.txtFWatch_port.Size = New System.Drawing.Size(65, 20)
        Me.txtFWatch_port.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 144)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "FAHWatch7 communications port"
        '
        'chkFwatchInstance
        '
        Me.chkFwatchInstance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFwatchInstance.Enabled = False
        Me.chkFwatchInstance.Location = New System.Drawing.Point(300, 112)
        Me.chkFwatchInstance.Name = "chkFwatchInstance"
        Me.chkFwatchInstance.Size = New System.Drawing.Size(16, 24)
        Me.chkFwatchInstance.TabIndex = 9
        Me.chkFwatchInstance.UseVisualStyleBackColor = True
        '
        'txtLocation
        '
        Me.txtLocation.Location = New System.Drawing.Point(14, 12)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.ReadOnly = True
        Me.txtLocation.Size = New System.Drawing.Size(305, 20)
        Me.txtLocation.TabIndex = 8
        '
        'txtPWD
        '
        Me.txtPWD.Location = New System.Drawing.Point(187, 62)
        Me.txtPWD.Name = "txtPWD"
        Me.txtPWD.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPWD.ReadOnly = True
        Me.txtPWD.Size = New System.Drawing.Size(132, 20)
        Me.txtPWD.TabIndex = 7
        Me.txtPWD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Connection PWD"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(254, 38)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.ReadOnly = True
        Me.txtPort.Size = New System.Drawing.Size(65, 20)
        Me.txtPort.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(214, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Select port for remote FAHClient connection"
        '
        'tvEnum
        '
        Me.tvEnum.Location = New System.Drawing.Point(6, 31)
        Me.tvEnum.Name = "tvEnum"
        Me.tvEnum.Size = New System.Drawing.Size(316, 210)
        Me.tvEnum.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.tvEnum, "Browse for a shared FAHClient data folder")
        '
        'cmdAccept
        '
        Me.cmdAccept.Enabled = False
        Me.cmdAccept.Location = New System.Drawing.Point(599, 255)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.Size = New System.Drawing.Size(75, 23)
        Me.cmdAccept.TabIndex = 11
        Me.cmdAccept.Text = "Accept"
        Me.cmdAccept.UseVisualStyleBackColor = True
        '
        'tShow
        '
        Me.tShow.Interval = 50
        '
        'ToolTip1
        '
        Me.ToolTip1.ShowAlways = True
        '
        'frmAddClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(689, 315)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.sStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddClient"
        Me.ShowIcon = False
        Me.Text = "Add client"
        Me.sStrip.ResumeLayout(False)
        Me.sStrip.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents cmdAccept As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtPWD As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tvEnum As System.Windows.Forms.TreeView
    Friend WithEvents txtFWatch_port As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkFwatchInstance As System.Windows.Forms.CheckBox
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents txtFWVersion As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFCVersion As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tShow As System.Windows.Forms.Timer
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
