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
        Me.tFake = New System.Windows.Forms.Timer(Me.components)
        Me.pBar = New System.Windows.Forms.ProgressBar()
        Me.sStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'sStrip
        '
        Me.sStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.sStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsStatus})
        Me.sStrip.Location = New System.Drawing.Point(0, 23)
        Me.sStrip.Name = "sStrip"
        Me.sStrip.Size = New System.Drawing.Size(400, 22)
        Me.sStrip.SizingGrip = False
        Me.sStrip.TabIndex = 3
        '
        'tsStatus
        '
        Me.tsStatus.Name = "tsStatus"
        Me.tsStatus.Size = New System.Drawing.Size(121, 17)
        Me.tsStatus.Text = "ToolStripStatusLabel1"
        '
        'tFake
        '
        '
        'pBar
        '
        Me.pBar.Dock = System.Windows.Forms.DockStyle.Top
        Me.pBar.Location = New System.Drawing.Point(0, 0)
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(400, 23)
        Me.pBar.TabIndex = 2
        '
        'frmPBStatus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(400, 45)
        Me.ControlBox = False
        Me.Controls.Add(Me.sStrip)
        Me.Controls.Add(Me.pBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmPBStatus"
        Me.sStrip.ResumeLayout(False)
        Me.sStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tsStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tFake As System.Windows.Forms.Timer
    Friend WithEvents pBar As System.Windows.Forms.ProgressBar
End Class
