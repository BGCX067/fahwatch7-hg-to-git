<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDownload
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
        Me.lblClient = New System.Windows.Forms.Label()
        Me.lblDownload = New System.Windows.Forms.Label()
        Me.tSpeed = New System.Windows.Forms.Timer(Me.components)
        Me.bitsec = New System.Windows.Forms.Label()
        Me.PbarDownload = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'lblClient
        '
        Me.lblClient.AutoSize = True
        Me.lblClient.Location = New System.Drawing.Point(43, 108)
        Me.lblClient.Name = "lblClient"
        Me.lblClient.Size = New System.Drawing.Size(0, 13)
        Me.lblClient.TabIndex = 8
        '
        'lblDownload
        '
        Me.lblDownload.AutoSize = True
        Me.lblDownload.Location = New System.Drawing.Point(78, 96)
        Me.lblDownload.Margin = New System.Windows.Forms.Padding(3, 1, 3, 1)
        Me.lblDownload.Name = "lblDownload"
        Me.lblDownload.Size = New System.Drawing.Size(0, 13)
        Me.lblDownload.TabIndex = 6
        '
        'tSpeed
        '
        Me.tSpeed.Interval = 1000
        '
        'bitsec
        '
        Me.bitsec.BackColor = System.Drawing.Color.Transparent
        Me.bitsec.Location = New System.Drawing.Point(163, 42)
        Me.bitsec.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.bitsec.Name = "bitsec"
        Me.bitsec.Size = New System.Drawing.Size(120, 16)
        Me.bitsec.TabIndex = 7
        Me.bitsec.Text = "Bit/sec "
        Me.bitsec.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'PbarDownload
        '
        Me.PbarDownload.Location = New System.Drawing.Point(17, 16)
        Me.PbarDownload.Name = "PbarDownload"
        Me.PbarDownload.Size = New System.Drawing.Size(266, 19)
        Me.PbarDownload.TabIndex = 6
        '
        'frmDownload
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.Dialog
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(300, 65)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblClient)
        Me.Controls.Add(Me.bitsec)
        Me.Controls.Add(Me.lblDownload)
        Me.Controls.Add(Me.PbarDownload)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.Name = "frmDownload"
        Me.ShowInTaskbar = False
        Me.Text = "1132"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblClient As System.Windows.Forms.Label
    Friend WithEvents lblDownload As System.Windows.Forms.Label
    Friend WithEvents tSpeed As System.Windows.Forms.Timer
    Friend WithEvents bitsec As System.Windows.Forms.Label
    Friend WithEvents PbarDownload As System.Windows.Forms.ProgressBar
End Class
