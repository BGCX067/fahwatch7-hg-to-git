<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucHWM
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.cMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ClocksGaugesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideClocksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FloatToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'cMenu
        '
        Me.cMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClocksGaugesToolStripMenuItem, Me.HideClocksToolStripMenuItem, Me.FloatToolStripMenuItem})
        Me.cMenu.Name = "cMenu"
        Me.cMenu.Size = New System.Drawing.Size(165, 70)
        '
        'ClocksGaugesToolStripMenuItem
        '
        Me.ClocksGaugesToolStripMenuItem.AutoToolTip = True
        Me.ClocksGaugesToolStripMenuItem.CheckOnClick = True
        Me.ClocksGaugesToolStripMenuItem.Name = "ClocksGaugesToolStripMenuItem"
        Me.ClocksGaugesToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ClocksGaugesToolStripMenuItem.Text = "Clocks as gauges"
        Me.ClocksGaugesToolStripMenuItem.ToolTipText = "When checked clocks will be drawn as gauges"
        '
        'HideClocksToolStripMenuItem
        '
        Me.HideClocksToolStripMenuItem.AutoToolTip = True
        Me.HideClocksToolStripMenuItem.CheckOnClick = True
        Me.HideClocksToolStripMenuItem.Name = "HideClocksToolStripMenuItem"
        Me.HideClocksToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.HideClocksToolStripMenuItem.Text = "Hide clocks"
        Me.HideClocksToolStripMenuItem.ToolTipText = "When checked no clock info will be displayed."
        '
        'FloatToolStripMenuItem
        '
        Me.FloatToolStripMenuItem.AutoToolTip = True
        Me.FloatToolStripMenuItem.CheckOnClick = True
        Me.FloatToolStripMenuItem.Name = "FloatToolStripMenuItem"
        Me.FloatToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.FloatToolStripMenuItem.Text = "Float"
        Me.FloatToolStripMenuItem.ToolTipText = "Opens a new floating window"
        '
        'ucHWM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.Transparent
        Me.ContextMenuStrip = Me.cMenu
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "ucHWM"
        Me.Size = New System.Drawing.Size(0, 0)
        Me.cMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ClocksGaugesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FloatToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideClocksToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
