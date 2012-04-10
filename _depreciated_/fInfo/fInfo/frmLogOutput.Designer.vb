<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogOutput
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
        Me.rtf = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'rtf
        '
        Me.rtf.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtf.Location = New System.Drawing.Point(0, 0)
        Me.rtf.Name = "rtf"
        Me.rtf.ReadOnly = True
        Me.rtf.Size = New System.Drawing.Size(924, 545)
        Me.rtf.TabIndex = 0
        Me.rtf.Text = ""
        '
        'frmLogOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(924, 545)
        Me.Controls.Add(Me.rtf)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmLogOutput"
        Me.Text = "fInfo "
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
End Class
