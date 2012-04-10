<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExceptionReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExceptionReport))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rtException = New System.Windows.Forms.RichTextBox()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.chkSendException = New System.Windows.Forms.CheckBox()
        Me.cmdStoredExceptions = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkContinue = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Exception details"
        '
        'rtException
        '
        Me.rtException.BackColor = System.Drawing.SystemColors.Info
        Me.rtException.Location = New System.Drawing.Point(16, 29)
        Me.rtException.Name = "rtException"
        Me.rtException.ReadOnly = True
        Me.rtException.Size = New System.Drawing.Size(550, 142)
        Me.rtException.TabIndex = 1
        Me.rtException.Text = ""
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(504, 177)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(62, 23)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Text = "Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'chkSendException
        '
        Me.chkSendException.AutoSize = True
        Me.chkSendException.Checked = True
        Me.chkSendException.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSendException.Location = New System.Drawing.Point(149, 181)
        Me.chkSendException.Name = "chkSendException"
        Me.chkSendException.Size = New System.Drawing.Size(210, 17)
        Me.chkSendException.TabIndex = 5
        Me.chkSendException.Text = "Send exception report to the developer"
        Me.ToolTip1.SetToolTip(Me.chkSendException, "Send this exception to the developer ( uses SMTP )")
        Me.chkSendException.UseVisualStyleBackColor = True
        '
        'cmdStoredExceptions
        '
        Me.cmdStoredExceptions.Location = New System.Drawing.Point(16, 177)
        Me.cmdStoredExceptions.Name = "cmdStoredExceptions"
        Me.cmdStoredExceptions.Size = New System.Drawing.Size(127, 23)
        Me.cmdStoredExceptions.TabIndex = 6
        Me.cmdStoredExceptions.Text = "View stored exceptions"
        Me.ToolTip1.SetToolTip(Me.cmdStoredExceptions, "View the list of stored exception reports")
        Me.cmdStoredExceptions.UseVisualStyleBackColor = True
        '
        'chkContinue
        '
        Me.chkContinue.AutoSize = True
        Me.chkContinue.Checked = True
        Me.chkContinue.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkContinue.Location = New System.Drawing.Point(401, 181)
        Me.chkContinue.Name = "chkContinue"
        Me.chkContinue.Size = New System.Drawing.Size(97, 17)
        Me.chkContinue.TabIndex = 7
        Me.chkContinue.Text = "Try to continue"
        Me.ToolTip1.SetToolTip(Me.chkContinue, "When checked, FAHWatch7 will try and continue executing. A back of the previous d" & _
        "atabase is not removed at final shutdown so it's always possible to go back.")
        Me.chkContinue.UseVisualStyleBackColor = True
        '
        'ExceptionReport
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 208)
        Me.Controls.Add(Me.chkContinue)
        Me.Controls.Add(Me.cmdStoredExceptions)
        Me.Controls.Add(Me.chkSendException)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.rtException)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ExceptionReport"
        Me.Text = "FAHWatch7 has generated an exception"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rtException As System.Windows.Forms.RichTextBox
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents chkSendException As System.Windows.Forms.CheckBox
    Friend WithEvents cmdStoredExceptions As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chkContinue As System.Windows.Forms.CheckBox
End Class
