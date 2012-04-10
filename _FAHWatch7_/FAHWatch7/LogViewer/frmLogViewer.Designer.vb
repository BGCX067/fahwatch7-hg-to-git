<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogViewer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogViewer))
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.sStrip = New System.Windows.Forms.StatusStrip()
        Me.tsLblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.rt = New System.Windows.Forms.RichTextBox()
        Me.tStrip = New System.Windows.Forms.ToolStrip()
        Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.tsCmbSlots = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.tsCmbProjects = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel4 = New System.Windows.Forms.ToolStripLabel()
        Me.tsCmbRCG = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabel3 = New System.Windows.Forms.ToolStripLabel()
        Me.tsCmbUnit = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsCmdFilter = New System.Windows.Forms.ToolStripButton()
        Me.tStatus = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.sStrip.SuspendLayout()
        Me.tStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.sStrip)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.rt)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(1165, 387)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(1165, 434)
        Me.ToolStripContainer1.TabIndex = 0
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.tStrip)
        '
        'sStrip
        '
        Me.sStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.sStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsLblStatus})
        Me.sStrip.Location = New System.Drawing.Point(0, 0)
        Me.sStrip.Name = "sStrip"
        Me.sStrip.Size = New System.Drawing.Size(1165, 22)
        Me.sStrip.TabIndex = 0
        '
        'tsLblStatus
        '
        Me.tsLblStatus.Name = "tsLblStatus"
        Me.tsLblStatus.Size = New System.Drawing.Size(0, 17)
        '
        'rt
        '
        Me.rt.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rt.Location = New System.Drawing.Point(0, 0)
        Me.rt.Name = "rt"
        Me.rt.Size = New System.Drawing.Size(1165, 387)
        Me.rt.TabIndex = 0
        Me.rt.Text = ""
        '
        'tStrip
        '
        Me.tStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.tStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton, Me.ToolStripLabel1, Me.tsCmbSlots, Me.ToolStripLabel2, Me.tsCmbProjects, Me.ToolStripLabel4, Me.tsCmbRCG, Me.ToolStripLabel3, Me.tsCmbUnit, Me.ToolStripSeparator2, Me.tsCmdFilter})
        Me.tStrip.Location = New System.Drawing.Point(0, 0)
        Me.tStrip.Name = "tStrip"
        Me.tStrip.Size = New System.Drawing.Size(1165, 25)
        Me.tStrip.Stretch = True
        Me.tStrip.TabIndex = 0
        '
        'CopyToolStripButton
        '
        Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
        Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripButton.Name = "CopyToolStripButton"
        Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CopyToolStripButton.Text = "&Copy"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.HelpToolStripButton.Text = "He&lp"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(30, 22)
        Me.ToolStripLabel1.Text = "Slot:"
        '
        'tsCmbSlots
        '
        Me.tsCmbSlots.BackColor = System.Drawing.SystemColors.Control
        Me.tsCmbSlots.DropDownWidth = 80
        Me.tsCmbSlots.Name = "tsCmbSlots"
        Me.tsCmbSlots.Size = New System.Drawing.Size(75, 25)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(52, 22)
        Me.ToolStripLabel2.Text = "Projects:"
        '
        'tsCmbProjects
        '
        Me.tsCmbProjects.BackColor = System.Drawing.SystemColors.Control
        Me.tsCmbProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsCmbProjects.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tsCmbProjects.Name = "tsCmbProjects"
        Me.tsCmbProjects.Size = New System.Drawing.Size(100, 25)
        '
        'ToolStripLabel4
        '
        Me.ToolStripLabel4.Name = "ToolStripLabel4"
        Me.ToolStripLabel4.Size = New System.Drawing.Size(89, 22)
        Me.ToolStripLabel4.Text = "Run Clone Gen:"
        '
        'tsCmbRCG
        '
        Me.tsCmbRCG.BackColor = System.Drawing.SystemColors.Control
        Me.tsCmbRCG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsCmbRCG.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tsCmbRCG.Name = "tsCmbRCG"
        Me.tsCmbRCG.Size = New System.Drawing.Size(160, 25)
        '
        'ToolStripLabel3
        '
        Me.ToolStripLabel3.Name = "ToolStripLabel3"
        Me.ToolStripLabel3.Size = New System.Drawing.Size(32, 22)
        Me.ToolStripLabel3.Text = "Unit:"
        '
        'tsCmbUnit
        '
        Me.tsCmbUnit.BackColor = System.Drawing.SystemColors.Control
        Me.tsCmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsCmbUnit.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.tsCmbUnit.Name = "tsCmbUnit"
        Me.tsCmbUnit.Size = New System.Drawing.Size(180, 25)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsCmdFilter
        '
        Me.tsCmdFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsCmdFilter.CheckOnClick = True
        Me.tsCmdFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsCmdFilter.Image = CType(resources.GetObject("tsCmdFilter.Image"), System.Drawing.Image)
        Me.tsCmdFilter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsCmdFilter.Name = "tsCmdFilter"
        Me.tsCmdFilter.Size = New System.Drawing.Size(37, 22)
        Me.tsCmdFilter.Text = "Filter"
        '
        'tStatus
        '
        Me.tStatus.Interval = 5000
        '
        'frmLogViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1165, 434)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLogViewer"
        Me.Text = "Log viewer"
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.sStrip.ResumeLayout(False)
        Me.sStrip.PerformLayout()
        Me.tStrip.ResumeLayout(False)
        Me.tStrip.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents sStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents rt As System.Windows.Forms.RichTextBox
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsCmbSlots As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsCmbProjects As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel4 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsCmbRCG As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripLabel3 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsCmbUnit As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsCmdFilter As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsLblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tStatus As System.Windows.Forms.Timer
End Class
