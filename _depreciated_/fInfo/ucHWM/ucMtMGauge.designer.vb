<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mtmGauge
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
        Me.bgwAuto = New System.ComponentModel.BackgroundWorker()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResetMaxMinToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'bgwAuto
        '
        Me.bgwAuto.WorkerSupportsCancellation = True
        '
        'cMenu
        '
        Me.cMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResetMaxMinToolStripMenuItem, Me.SettingsToolStripMenuItem})
        Me.cMenu.Name = "cMenu"
        Me.cMenu.Size = New System.Drawing.Size(178, 52)
        '
        'ResetMaxMinToolStripMenuItem
        '
        Me.ResetMaxMinToolStripMenuItem.Name = "ResetMaxMinToolStripMenuItem"
        Me.ResetMaxMinToolStripMenuItem.Size = New System.Drawing.Size(177, 24)
        Me.ResetMaxMinToolStripMenuItem.Text = "Reset Max/Min"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(177, 24)
        Me.SettingsToolStripMenuItem.Text = "Setings"
        '
        'mtmGauge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.ContextMenuStrip = Me.cMenu
        Me.Name = "mtmGauge"
        Me.Size = New System.Drawing.Size(68, 150)
        Me.cMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bgwAuto As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents cMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResetMaxMinToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
