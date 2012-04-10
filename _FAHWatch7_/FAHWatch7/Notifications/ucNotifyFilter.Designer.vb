<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucNotifyFilter
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
        Me.chkRuleName = New System.Windows.Forms.CheckBox()
        Me.pnlOptions = New System.Windows.Forms.Panel()
        Me.rbEUE = New System.Windows.Forms.RadioButton()
        Me.rbSlotFail = New System.Windows.Forms.RadioButton()
        Me.pnlEUE_1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rbEUE_ratio = New System.Windows.Forms.RadioButton()
        Me.rbEUE_Always = New System.Windows.Forms.RadioButton()
        Me.pnlEUE_2 = New System.Windows.Forms.Panel()
        Me.nudRatio_Warning = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRatio_Actual = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnlOptions.SuspendLayout()
        Me.pnlEUE_1.SuspendLayout()
        Me.pnlEUE_2.SuspendLayout()
        CType(Me.nudRatio_Warning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkRuleName
        '
        Me.chkRuleName.AutoSize = True
        Me.chkRuleName.Location = New System.Drawing.Point(3, 4)
        Me.chkRuleName.Name = "chkRuleName"
        Me.chkRuleName.Size = New System.Drawing.Size(15, 14)
        Me.chkRuleName.TabIndex = 0
        Me.chkRuleName.UseVisualStyleBackColor = True
        '
        'pnlOptions
        '
        Me.pnlOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlOptions.Controls.Add(Me.rbEUE)
        Me.pnlOptions.Controls.Add(Me.rbSlotFail)
        Me.pnlOptions.Controls.Add(Me.pnlEUE_1)
        Me.pnlOptions.Controls.Add(Me.pnlEUE_2)
        Me.pnlOptions.Location = New System.Drawing.Point(4, 24)
        Me.pnlOptions.Name = "pnlOptions"
        Me.pnlOptions.Size = New System.Drawing.Size(208, 132)
        Me.pnlOptions.TabIndex = 1
        '
        'rbEUE
        '
        Me.rbEUE.AutoSize = True
        Me.rbEUE.Location = New System.Drawing.Point(6, 29)
        Me.rbEUE.Name = "rbEUE"
        Me.rbEUE.Size = New System.Drawing.Size(137, 17)
        Me.rbEUE.TabIndex = 22
        Me.rbEUE.TabStop = True
        Me.rbEUE.Text = "Notify on Early Unit End"
        Me.rbEUE.UseVisualStyleBackColor = True
        '
        'rbSlotFail
        '
        Me.rbSlotFail.AutoSize = True
        Me.rbSlotFail.Location = New System.Drawing.Point(6, 6)
        Me.rbSlotFail.Name = "rbSlotFail"
        Me.rbSlotFail.Size = New System.Drawing.Size(117, 17)
        Me.rbSlotFail.TabIndex = 21
        Me.rbSlotFail.TabStop = True
        Me.rbSlotFail.Text = "Notify on slot failure"
        Me.rbSlotFail.UseVisualStyleBackColor = True
        '
        'pnlEUE_1
        '
        Me.pnlEUE_1.Controls.Add(Me.Label2)
        Me.pnlEUE_1.Controls.Add(Me.rbEUE_ratio)
        Me.pnlEUE_1.Controls.Add(Me.rbEUE_Always)
        Me.pnlEUE_1.Location = New System.Drawing.Point(6, 52)
        Me.pnlEUE_1.Name = "pnlEUE_1"
        Me.pnlEUE_1.Size = New System.Drawing.Size(197, 21)
        Me.pnlEUE_1.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Notify"
        '
        'rbEUE_ratio
        '
        Me.rbEUE_ratio.AutoSize = True
        Me.rbEUE_ratio.Location = New System.Drawing.Point(123, 3)
        Me.rbEUE_ratio.Name = "rbEUE_ratio"
        Me.rbEUE_ratio.Size = New System.Drawing.Size(59, 17)
        Me.rbEUE_ratio.TabIndex = 1
        Me.rbEUE_ratio.TabStop = True
        Me.rbEUE_ratio.Text = "by ratio"
        Me.rbEUE_ratio.UseVisualStyleBackColor = True
        '
        'rbEUE_Always
        '
        Me.rbEUE_Always.AutoSize = True
        Me.rbEUE_Always.Location = New System.Drawing.Point(52, 3)
        Me.rbEUE_Always.Name = "rbEUE_Always"
        Me.rbEUE_Always.Size = New System.Drawing.Size(57, 17)
        Me.rbEUE_Always.TabIndex = 0
        Me.rbEUE_Always.TabStop = True
        Me.rbEUE_Always.Text = "always"
        Me.rbEUE_Always.UseVisualStyleBackColor = True
        '
        'pnlEUE_2
        '
        Me.pnlEUE_2.Controls.Add(Me.nudRatio_Warning)
        Me.pnlEUE_2.Controls.Add(Me.Label6)
        Me.pnlEUE_2.Controls.Add(Me.txtRatio_Actual)
        Me.pnlEUE_2.Controls.Add(Me.Label5)
        Me.pnlEUE_2.Location = New System.Drawing.Point(6, 78)
        Me.pnlEUE_2.Name = "pnlEUE_2"
        Me.pnlEUE_2.Size = New System.Drawing.Size(197, 46)
        Me.pnlEUE_2.TabIndex = 19
        '
        'nudRatio_Warning
        '
        Me.nudRatio_Warning.Location = New System.Drawing.Point(91, 23)
        Me.nudRatio_Warning.Name = "nudRatio_Warning"
        Me.nudRatio_Warning.Size = New System.Drawing.Size(52, 20)
        Me.nudRatio_Warning.TabIndex = 5
        Me.nudRatio_Warning.Value = New Decimal(New Integer() {80, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Warn if ratio <"
        '
        'txtRatio_Actual
        '
        Me.txtRatio_Actual.BackColor = System.Drawing.SystemColors.Window
        Me.txtRatio_Actual.Location = New System.Drawing.Point(91, 1)
        Me.txtRatio_Actual.Name = "txtRatio_Actual"
        Me.txtRatio_Actual.ReadOnly = True
        Me.txtRatio_Actual.Size = New System.Drawing.Size(36, 20)
        Me.txtRatio_Actual.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Actual ratio"
        '
        'ucNotifyFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.pnlOptions)
        Me.Controls.Add(Me.chkRuleName)
        Me.Name = "ucNotifyFilter"
        Me.Size = New System.Drawing.Size(216, 162)
        Me.pnlOptions.ResumeLayout(False)
        Me.pnlOptions.PerformLayout()
        Me.pnlEUE_1.ResumeLayout(False)
        Me.pnlEUE_1.PerformLayout()
        Me.pnlEUE_2.ResumeLayout(False)
        Me.pnlEUE_2.PerformLayout()
        CType(Me.nudRatio_Warning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlOptions As System.Windows.Forms.Panel
    Public WithEvents chkRuleName As System.Windows.Forms.CheckBox
    Friend WithEvents pnlEUE_1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbEUE_ratio As System.Windows.Forms.RadioButton
    Friend WithEvents rbEUE_Always As System.Windows.Forms.RadioButton
    Friend WithEvents pnlEUE_2 As System.Windows.Forms.Panel
    Friend WithEvents nudRatio_Warning As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRatio_Actual As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rbSlotFail As System.Windows.Forms.RadioButton
    Friend WithEvents rbEUE As System.Windows.Forms.RadioButton

End Class
