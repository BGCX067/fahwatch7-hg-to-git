<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGraphSettings
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnMaxColorTpf = New System.Windows.Forms.Panel()
        Me.pnAvgColorTpf = New System.Windows.Forms.Panel()
        Me.pnMinColorTpf = New System.Windows.Forms.Panel()
        Me.cmdMaxColorTpf = New System.Windows.Forms.Button()
        Me.cmdAvgColorTpf = New System.Windows.Forms.Button()
        Me.cmdMinColorTpf = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cDialog = New System.Windows.Forms.ColorDialog()
        Me.cmbPaneItems = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbGraphStyle = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.pnMaxColorPpd = New System.Windows.Forms.Panel()
        Me.pnAvgColorPpd = New System.Windows.Forms.Panel()
        Me.pnMinColorPpd = New System.Windows.Forms.Panel()
        Me.cmdMaxColorPpd = New System.Windows.Forms.Button()
        Me.cmdAvgColorPpd = New System.Windows.Forms.Button()
        Me.cmdMinColorPpd = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.Controls.Add(Me.pnMaxColorTpf)
        Me.GroupBox1.Controls.Add(Me.pnAvgColorTpf)
        Me.GroupBox1.Controls.Add(Me.pnMinColorTpf)
        Me.GroupBox1.Controls.Add(Me.cmdMaxColorTpf)
        Me.GroupBox1.Controls.Add(Me.cmdAvgColorTpf)
        Me.GroupBox1.Controls.Add(Me.cmdMinColorTpf)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 120)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Time Per Frame"
        '
        'pnMaxColorTpf
        '
        Me.pnMaxColorTpf.Location = New System.Drawing.Point(117, 78)
        Me.pnMaxColorTpf.Name = "pnMaxColorTpf"
        Me.pnMaxColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.pnMaxColorTpf.TabIndex = 5
        '
        'pnAvgColorTpf
        '
        Me.pnAvgColorTpf.Location = New System.Drawing.Point(117, 49)
        Me.pnAvgColorTpf.Name = "pnAvgColorTpf"
        Me.pnAvgColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.pnAvgColorTpf.TabIndex = 4
        '
        'pnMinColorTpf
        '
        Me.pnMinColorTpf.Location = New System.Drawing.Point(117, 21)
        Me.pnMinColorTpf.Name = "pnMinColorTpf"
        Me.pnMinColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.pnMinColorTpf.TabIndex = 3
        '
        'cmdMaxColorTpf
        '
        Me.cmdMaxColorTpf.Location = New System.Drawing.Point(9, 78)
        Me.cmdMaxColorTpf.Name = "cmdMaxColorTpf"
        Me.cmdMaxColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.cmdMaxColorTpf.TabIndex = 2
        Me.cmdMaxColorTpf.Text = "Color max value"
        Me.cmdMaxColorTpf.UseVisualStyleBackColor = True
        '
        'cmdAvgColorTpf
        '
        Me.cmdAvgColorTpf.Location = New System.Drawing.Point(9, 49)
        Me.cmdAvgColorTpf.Name = "cmdAvgColorTpf"
        Me.cmdAvgColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.cmdAvgColorTpf.TabIndex = 1
        Me.cmdAvgColorTpf.Text = "Color avg value"
        Me.cmdAvgColorTpf.UseVisualStyleBackColor = True
        '
        'cmdMinColorTpf
        '
        Me.cmdMinColorTpf.Location = New System.Drawing.Point(9, 19)
        Me.cmdMinColorTpf.Name = "cmdMinColorTpf"
        Me.cmdMinColorTpf.Size = New System.Drawing.Size(92, 23)
        Me.cmdMinColorTpf.TabIndex = 0
        Me.cmdMinColorTpf.Text = "Color min value"
        Me.cmdMinColorTpf.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(153, 349)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cDialog
        '
        Me.cDialog.AnyColor = True
        '
        'cmbPaneItems
        '
        Me.cmbPaneItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPaneItems.FormattingEnabled = True
        Me.cmbPaneItems.Items.AddRange(New Object() {"All", "5", "10"})
        Me.cmbPaneItems.Location = New System.Drawing.Point(117, 19)
        Me.cmbPaneItems.Name = "cmbPaneItems"
        Me.cmbPaneItems.Size = New System.Drawing.Size(92, 21)
        Me.cmbPaneItems.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Items per pane"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Graph style"
        '
        'cmbGraphStyle
        '
        Me.cmbGraphStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGraphStyle.FormattingEnabled = True
        Me.cmbGraphStyle.Items.AddRange(New Object() {"Individual bars", "Stacked bars"})
        Me.cmbGraphStyle.Location = New System.Drawing.Point(117, 51)
        Me.cmbGraphStyle.Name = "cmbGraphStyle"
        Me.cmbGraphStyle.Size = New System.Drawing.Size(92, 21)
        Me.cmbGraphStyle.TabIndex = 9
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmbGraphStyle)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.cmbPaneItems)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 259)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(222, 84)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Graphs"
        '
        'GroupBox3
        '
        Me.GroupBox3.AutoSize = True
        Me.GroupBox3.Controls.Add(Me.pnMaxColorPpd)
        Me.GroupBox3.Controls.Add(Me.pnAvgColorPpd)
        Me.GroupBox3.Controls.Add(Me.pnMinColorPpd)
        Me.GroupBox3.Controls.Add(Me.cmdMaxColorPpd)
        Me.GroupBox3.Controls.Add(Me.cmdAvgColorPpd)
        Me.GroupBox3.Controls.Add(Me.cmdMinColorPpd)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 130)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(222, 120)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Points Per Day"
        '
        'pnMaxColorPpd
        '
        Me.pnMaxColorPpd.Location = New System.Drawing.Point(117, 78)
        Me.pnMaxColorPpd.Name = "pnMaxColorPpd"
        Me.pnMaxColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.pnMaxColorPpd.TabIndex = 5
        '
        'pnAvgColorPpd
        '
        Me.pnAvgColorPpd.Location = New System.Drawing.Point(117, 49)
        Me.pnAvgColorPpd.Name = "pnAvgColorPpd"
        Me.pnAvgColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.pnAvgColorPpd.TabIndex = 4
        '
        'pnMinColorPpd
        '
        Me.pnMinColorPpd.Location = New System.Drawing.Point(117, 21)
        Me.pnMinColorPpd.Name = "pnMinColorPpd"
        Me.pnMinColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.pnMinColorPpd.TabIndex = 3
        '
        'cmdMaxColorPpd
        '
        Me.cmdMaxColorPpd.Location = New System.Drawing.Point(9, 78)
        Me.cmdMaxColorPpd.Name = "cmdMaxColorPpd"
        Me.cmdMaxColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.cmdMaxColorPpd.TabIndex = 2
        Me.cmdMaxColorPpd.Text = "Color max value"
        Me.cmdMaxColorPpd.UseVisualStyleBackColor = True
        '
        'cmdAvgColorPpd
        '
        Me.cmdAvgColorPpd.Location = New System.Drawing.Point(9, 49)
        Me.cmdAvgColorPpd.Name = "cmdAvgColorPpd"
        Me.cmdAvgColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.cmdAvgColorPpd.TabIndex = 1
        Me.cmdAvgColorPpd.Text = "Color avg value"
        Me.cmdAvgColorPpd.UseVisualStyleBackColor = True
        '
        'cmdMinColorPpd
        '
        Me.cmdMinColorPpd.Location = New System.Drawing.Point(9, 19)
        Me.cmdMinColorPpd.Name = "cmdMinColorPpd"
        Me.cmdMinColorPpd.Size = New System.Drawing.Size(92, 23)
        Me.cmdMinColorPpd.TabIndex = 0
        Me.cmdMinColorPpd.Text = "Color min value"
        Me.cmdMinColorPpd.UseVisualStyleBackColor = True
        '
        'frmGraphSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(232, 377)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGraphSettings"
        Me.Text = "Configure graphs"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdMaxColorTpf As System.Windows.Forms.Button
    Friend WithEvents cmdAvgColorTpf As System.Windows.Forms.Button
    Friend WithEvents cmdMinColorTpf As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents pnMaxColorTpf As System.Windows.Forms.Panel
    Friend WithEvents pnAvgColorTpf As System.Windows.Forms.Panel
    Friend WithEvents pnMinColorTpf As System.Windows.Forms.Panel
    Friend WithEvents cmbGraphStyle As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPaneItems As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents pnMaxColorPpd As System.Windows.Forms.Panel
    Friend WithEvents pnAvgColorPpd As System.Windows.Forms.Panel
    Friend WithEvents pnMinColorPpd As System.Windows.Forms.Panel
    Friend WithEvents cmdMaxColorPpd As System.Windows.Forms.Button
    Friend WithEvents cmdAvgColorPpd As System.Windows.Forms.Button
    Friend WithEvents cmdMinColorPpd As System.Windows.Forms.Button
End Class
