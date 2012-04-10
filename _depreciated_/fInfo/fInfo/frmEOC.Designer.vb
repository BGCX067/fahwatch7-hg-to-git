<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEOC
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEOC))
        Me.pb = New System.Windows.Forms.PictureBox()
        Me.nIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.cMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        CType(Me.pb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pb
        '
        Me.pb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pb.Location = New System.Drawing.Point(0, 0)
        Me.pb.Name = "pb"
        Me.pb.Size = New System.Drawing.Size(330, 293)
        Me.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pb.TabIndex = 0
        Me.pb.TabStop = False
        Me.pb.WaitOnLoad = True
        '
        'nIcon
        '
        Me.nIcon.ContextMenuStrip = Me.cMenu
        Me.nIcon.Icon = CType(resources.GetObject("nIcon.Icon"), System.Drawing.Icon)
        Me.nIcon.Text = "EOC Stats"
        Me.nIcon.Visible = True
        '
        'cMenu
        '
        Me.cMenu.Name = "cMenu"
        Me.cMenu.Size = New System.Drawing.Size(61, 4)
        '
        'frmEOC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.ClientSize = New System.Drawing.Size(330, 293)
        Me.ControlBox = False
        Me.Controls.Add(Me.pb)
        Me.Enabled = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmEOC"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        CType(Me.pb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pb As System.Windows.Forms.PictureBox
    Friend WithEvents nIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents cMenu As System.Windows.Forms.ContextMenuStrip

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
