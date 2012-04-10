<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogWindow))
        Me.nIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.rtLog = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'nIcon
        '
        Me.nIcon.Icon = CType(resources.GetObject("nIcon.Icon"), System.Drawing.Icon)
        '
        'rtLog
        '
        Me.rtLog.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtLog.Location = New System.Drawing.Point(0, 0)
        Me.rtLog.Name = "rtLog"
        Me.rtLog.ReadOnly = True
        Me.rtLog.Size = New System.Drawing.Size(770, 436)
        Me.rtLog.TabIndex = 0
        Me.rtLog.Text = ""
        '
        'frmLogWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 436)
        Me.Controls.Add(Me.rtLog)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmLogWindow"
        Me.Text = "Log messages"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents rtLog As System.Windows.Forms.RichTextBox
    Public WithEvents nIcon As System.Windows.Forms.NotifyIcon
End Class
