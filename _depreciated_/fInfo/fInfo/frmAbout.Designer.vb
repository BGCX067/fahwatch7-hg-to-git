<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.rtfEULA = New System.Windows.Forms.RichTextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.rtfChangeLog = New System.Windows.Forms.RichTextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.lnklblChangeLog = New System.Windows.Forms.LinkLabel()
        Me.lblPatchSummary = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblUpdatesAvailable = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblConStatus = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.lblMin = New System.Windows.Forms.Label()
        Me.lblClose = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblCreation = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lbFiles = New System.Windows.Forms.ListBox()
        Me.GroupBox2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.TabControl1)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.GroupBox2.Location = New System.Drawing.Point(20, 250)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(760, 327)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "EULA"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(3, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(754, 308)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.rtfEULA)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(746, 282)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "EULA"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'rtfEULA
        '
        Me.rtfEULA.BackColor = System.Drawing.SystemColors.Info
        Me.rtfEULA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtfEULA.ForeColor = System.Drawing.SystemColors.InfoText
        Me.rtfEULA.Location = New System.Drawing.Point(2, 3)
        Me.rtfEULA.Name = "rtfEULA"
        Me.rtfEULA.Size = New System.Drawing.Size(742, 280)
        Me.rtfEULA.TabIndex = 1
        Me.rtfEULA.Text = ""
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.rtfChangeLog)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(746, 282)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Changelog"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'rtfChangeLog
        '
        Me.rtfChangeLog.BackColor = System.Drawing.SystemColors.Info
        Me.rtfChangeLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtfChangeLog.ForeColor = System.Drawing.SystemColors.InfoText
        Me.rtfChangeLog.Location = New System.Drawing.Point(2, -6)
        Me.rtfChangeLog.Name = "rtfChangeLog"
        Me.rtfChangeLog.Size = New System.Drawing.Size(742, 294)
        Me.rtfChangeLog.TabIndex = 1
        Me.rtfChangeLog.Text = ""
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox3.Controls.Add(Me.cmdUpdate)
        Me.GroupBox3.Controls.Add(Me.CheckBox1)
        Me.GroupBox3.Controls.Add(Me.lnklblChangeLog)
        Me.GroupBox3.Controls.Add(Me.lblPatchSummary)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.lblUpdatesAvailable)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.lblConStatus)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.ForeColor = System.Drawing.SystemColors.InfoText
        Me.GroupBox3.Location = New System.Drawing.Point(495, 45)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(285, 202)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Updates"
        '
        'cmdUpdate
        '
        Me.cmdUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Yellow
        Me.cmdUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdUpdate.Location = New System.Drawing.Point(160, 173)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(118, 21)
        Me.cmdUpdate.TabIndex = 8
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(10, 175)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(114, 17)
        Me.CheckBox1.TabIndex = 7
        Me.CheckBox1.Text = "Automatic updates"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'lnklblChangeLog
        '
        Me.lnklblChangeLog.AutoSize = True
        Me.lnklblChangeLog.ForeColor = System.Drawing.SystemColors.InfoText
        Me.lnklblChangeLog.LinkColor = System.Drawing.Color.Blue
        Me.lnklblChangeLog.Location = New System.Drawing.Point(7, 146)
        Me.lnklblChangeLog.Name = "lnklblChangeLog"
        Me.lnklblChangeLog.Size = New System.Drawing.Size(86, 13)
        Me.lnklblChangeLog.TabIndex = 6
        Me.lnklblChangeLog.TabStop = True
        Me.lnklblChangeLog.Text = "Open changelog"
        Me.lnklblChangeLog.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        '
        'lblPatchSummary
        '
        Me.lblPatchSummary.BackColor = System.Drawing.SystemColors.Info
        Me.lblPatchSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPatchSummary.Location = New System.Drawing.Point(108, 83)
        Me.lblPatchSummary.Name = "lblPatchSummary"
        Me.lblPatchSummary.Size = New System.Drawing.Size(171, 76)
        Me.lblPatchSummary.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.InfoText
        Me.Label4.Location = New System.Drawing.Point(7, 83)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Patch summary:"
        '
        'lblUpdatesAvailable
        '
        Me.lblUpdatesAvailable.AutoSize = True
        Me.lblUpdatesAvailable.BackColor = System.Drawing.Color.Transparent
        Me.lblUpdatesAvailable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblUpdatesAvailable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblUpdatesAvailable.Location = New System.Drawing.Point(108, 54)
        Me.lblUpdatesAvailable.Name = "lblUpdatesAvailable"
        Me.lblUpdatesAvailable.Size = New System.Drawing.Size(2, 15)
        Me.lblUpdatesAvailable.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.InfoText
        Me.Label3.Location = New System.Drawing.Point(7, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Updates available:"
        '
        'lblConStatus
        '
        Me.lblConStatus.AutoSize = True
        Me.lblConStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblConStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblConStatus.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblConStatus.Location = New System.Drawing.Point(108, 23)
        Me.lblConStatus.Name = "lblConStatus"
        Me.lblConStatus.Size = New System.Drawing.Size(2, 15)
        Me.lblConStatus.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.InfoText
        Me.Label2.Location = New System.Drawing.Point(7, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Connection status:"
        '
        'lblHelp
        '
        Me.lblHelp.BackColor = System.Drawing.Color.Transparent
        Me.lblHelp.Location = New System.Drawing.Point(382, 234)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(61, 22)
        Me.lblHelp.TabIndex = 5
        '
        'lblMin
        '
        Me.lblMin.BackColor = System.Drawing.Color.Transparent
        Me.lblMin.Location = New System.Drawing.Point(474, 253)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.Size = New System.Drawing.Size(33, 22)
        Me.lblMin.TabIndex = 4
        '
        'lblClose
        '
        Me.lblClose.BackColor = System.Drawing.Color.Transparent
        Me.lblClose.Location = New System.Drawing.Point(339, 253)
        Me.lblClose.Name = "lblClose"
        Me.lblClose.Size = New System.Drawing.Size(57, 22)
        Me.lblClose.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.InfoText
        Me.Label1.Location = New System.Drawing.Point(25, 234)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(249, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "* Select a file to view it's EULA if provided"
        '
        'lblCreation
        '
        Me.lblCreation.AutoSize = True
        Me.lblCreation.BackColor = System.Drawing.Color.Transparent
        Me.lblCreation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreation.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCreation.Location = New System.Drawing.Point(207, 79)
        Me.lblCreation.Name = "lblCreation"
        Me.lblCreation.Size = New System.Drawing.Size(94, 13)
        Me.lblCreation.TabIndex = 9
        Me.lblCreation.Text = "%creationtime%"
        '
        'lblCopyright
        '
        Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
        Me.lblCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCopyright.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblCopyright.Location = New System.Drawing.Point(207, 108)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(266, 41)
        Me.lblCopyright.TabIndex = 8
        Me.lblCopyright.Text = "%copyright%"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblVersion.Location = New System.Drawing.Point(207, 48)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(67, 13)
        Me.lblVersion.TabIndex = 7
        Me.lblVersion.Text = "%Version%"
        '
        'lbFiles
        '
        Me.lbFiles.BackColor = System.Drawing.SystemColors.Info
        Me.lbFiles.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lbFiles.FormattingEnabled = True
        Me.lbFiles.Location = New System.Drawing.Point(28, 48)
        Me.lbFiles.Name = "lbFiles"
        Me.lbFiles.Size = New System.Drawing.Size(156, 173)
        Me.lbFiles.TabIndex = 6
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.UI
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(806, 605)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCreation)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lbFiles)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.lblMin)
        Me.Controls.Add(Me.lblClose)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(806, 605)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(806, 605)
        Me.Name = "frmAbout"
        Me.Text = "About"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(221, Byte), Integer))
        Me.GroupBox2.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents lblMin As System.Windows.Forms.Label
    Friend WithEvents lblClose As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblCreation As System.Windows.Forms.Label
    Friend WithEvents lblCopyright As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lbFiles As System.Windows.Forms.ListBox
    Friend WithEvents cmdUpdate As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents lnklblChangeLog As System.Windows.Forms.LinkLabel
    Friend WithEvents lblPatchSummary As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblUpdatesAvailable As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblConStatus As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents rtfEULA As System.Windows.Forms.RichTextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents rtfChangeLog As System.Windows.Forms.RichTextBox
End Class
