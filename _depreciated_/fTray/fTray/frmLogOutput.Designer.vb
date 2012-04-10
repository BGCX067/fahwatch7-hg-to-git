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
        Me.rtf.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rtf.Name = "rtf"
        Me.rtf.ReadOnly = True
        Me.rtf.Size = New System.Drawing.Size(1232, 671)
        Me.rtf.TabIndex = 0
        Me.rtf.Text = ""
        '
        'frmLogOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1232, 671)
        Me.Controls.Add(Me.rtf)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.Name = "frmLogOutput"
        Me.Text = "Fahlog.txt"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
End Class
