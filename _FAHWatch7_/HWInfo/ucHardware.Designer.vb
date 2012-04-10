<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucHardware
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
        Me.lblName = New System.Windows.Forms.Label()
        Me.tlpSensors = New System.Windows.Forms.TableLayoutPanel()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblName.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblName.Location = New System.Drawing.Point(0, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(300, 19)
        Me.lblName.TabIndex = 2
        Me.lblName.Text = "Label1"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tlpSensors
        '
        Me.tlpSensors.ColumnCount = 2
        Me.tlpSensors.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSensors.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSensors.Dock = System.Windows.Forms.DockStyle.Top
        Me.tlpSensors.Location = New System.Drawing.Point(0, 19)
        Me.tlpSensors.Name = "tlpSensors"
        Me.tlpSensors.RowCount = 1
        Me.tlpSensors.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpSensors.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpSensors.Size = New System.Drawing.Size(300, 39)
        Me.tlpSensors.TabIndex = 3
        '
        'ucHardware
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tlpSensors)
        Me.Controls.Add(Me.lblName)
        Me.Name = "ucHardware"
        Me.Size = New System.Drawing.Size(300, 249)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents tlpSensors As System.Windows.Forms.TableLayoutPanel

End Class
