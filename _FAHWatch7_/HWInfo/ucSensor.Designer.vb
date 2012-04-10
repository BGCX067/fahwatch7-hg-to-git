<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucSensor
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
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'tTip
        '
        Me.tTip.ShowAlways = True
        '
        'ucSensor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MinimumSize = New System.Drawing.Size(10, 10)
        Me.Name = "ucSensor"
        Me.Size = New System.Drawing.Size(140, 20)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tTip As System.Windows.Forms.ToolTip

End Class
