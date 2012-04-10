<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPBStatus
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
        Me.sStrip = New System.Windows.Forms.StatusStrip()
        Me.tsStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pBar = New System.Windows.Forms.ProgressBar()
        Me.tFake = New System.Windows.Forms.Timer(Me.components)
        Me.sStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'sStrip
        '
        Me.sStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus})
        Me.sStrip.Location = New System.Drawing.Point(0, 31)
        Me.sStrip.Name = "sStrip"
        Me.sStrip.Size = New System.Drawing.Size(433, 25)
        Me.sStrip.SizingGrip = False
        Me.sStrip.Stretch = False
        Me.sStrip.TabIndex = 0
        Me.sStrip.Text = "sStripkykkghjddjdghj"
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(154, 20)
        Me.tsStatus.Text = "ToolStripStatusLabel1"
        '
        'pBar
        '
        Me.pBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pBar.Location = New System.Drawing.Point(0, 0)
        Me.pBar.Maximum = 5000
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(433, 31)
        Me.pBar.Step = 1
        Me.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pBar.TabIndex = 1
        '
        'tFake
        '
        '
        'frmPBStatus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 56)
        Me.ControlBox = False
        Me.Controls.Add(Me.pBar)
        Me.Controls.Add(Me.sStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPBStatus"
        Me.sStrip.ResumeLayout(False)
        Me.sStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents pBar As System.Windows.Forms.ProgressBar
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tFake As System.Windows.Forms.Timer
End Class
