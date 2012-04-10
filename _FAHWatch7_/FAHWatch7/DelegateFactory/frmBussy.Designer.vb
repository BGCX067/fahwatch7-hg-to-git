<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBussy
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBussy))
        Me.scPbar = New System.Windows.Forms.SplitContainer()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.scPbar.Panel1.SuspendLayout()
        Me.scPbar.Panel2.SuspendLayout()
        Me.scPbar.SuspendLayout()
        Me.SuspendLayout()
        '
        'scPbar
        '
        Me.scPbar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scPbar.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scPbar.IsSplitterFixed = True
        Me.scPbar.Location = New System.Drawing.Point(0, 0)
        Me.scPbar.Name = "scPbar"
        Me.scPbar.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scPbar.Panel1
        '
        Me.scPbar.Panel1.Controls.Add(Me.lblMessage)
        '
        'scPbar.Panel2
        '
        Me.scPbar.Panel2.Controls.Add(Me.pbProgress)
        Me.scPbar.Size = New System.Drawing.Size(442, 171)
        Me.scPbar.SplitterDistance = 142
        Me.scPbar.TabIndex = 1
        '
        'lblMessage
        '
        Me.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMessage.Location = New System.Drawing.Point(0, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(442, 142)
        Me.lblMessage.TabIndex = 1
        Me.lblMessage.Text = "Working... please wait"
        Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbProgress
        '
        Me.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbProgress.Location = New System.Drawing.Point(0, 0)
        Me.pbProgress.MarqueeAnimationSpeed = 50
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(442, 25)
        Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbProgress.TabIndex = 0
        '
        'frmBussy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 171)
        Me.Controls.Add(Me.scPbar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmBussy"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.scPbar.Panel1.ResumeLayout(False)
        Me.scPbar.Panel2.ResumeLayout(False)
        Me.scPbar.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scPbar As System.Windows.Forms.SplitContainer
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
End Class
