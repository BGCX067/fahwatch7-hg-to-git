<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmControl))
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.lblMin = New System.Windows.Forms.Label()
        Me.lblClose = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblHelp
        '
        Me.lblHelp.BackColor = System.Drawing.Color.Transparent
        Me.lblHelp.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblHelp.Location = New System.Drawing.Point(682, 94)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(61, 22)
        Me.lblHelp.TabIndex = 6
        '
        'lblMin
        '
        Me.lblMin.BackColor = System.Drawing.Color.Transparent
        Me.lblMin.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblMin.Location = New System.Drawing.Point(856, 17)
        Me.lblMin.Name = "lblMin"
        Me.lblMin.Size = New System.Drawing.Size(33, 22)
        Me.lblMin.TabIndex = 5
        '
        'lblClose
        '
        Me.lblClose.BackColor = System.Drawing.Color.Transparent
        Me.lblClose.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.lblClose.Location = New System.Drawing.Point(721, 17)
        Me.lblClose.Name = "lblClose"
        Me.lblClose.Size = New System.Drawing.Size(57, 22)
        Me.lblClose.TabIndex = 4
        '
        'frmControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.UI
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(802, 604)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.lblMin)
        Me.Controls.Add(Me.lblClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmControl"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(221, Byte), Integer))
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents lblMin As System.Windows.Forms.Label
    Friend WithEvents lblClose As System.Windows.Forms.Label
End Class
