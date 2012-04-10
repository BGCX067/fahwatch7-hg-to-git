<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.nudInt = New System.Windows.Forms.NumericUpDown()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.rtf = New System.Windows.Forms.RichTextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.cmdConnect = New System.Windows.Forms.Button()
        Me.txtHost = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cmdSend = New System.Windows.Forms.Button()
        Me.txtCmd = New System.Windows.Forms.TextBox()
        Me.rtfC = New System.Windows.Forms.RichTextBox()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudInt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.rtf)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.rtfC)
        Me.SplitContainer1.Size = New System.Drawing.Size(920, 310)
        Me.SplitContainer1.SplitterDistance = 411
        Me.SplitContainer1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.nudInt)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 271)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(411, 39)
        Me.Panel1.TabIndex = 6
        '
        'nudInt
        '
        Me.nudInt.Location = New System.Drawing.Point(3, 13)
        Me.nudInt.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
        Me.nudInt.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudInt.Name = "nudInt"
        Me.nudInt.Size = New System.Drawing.Size(95, 20)
        Me.nudInt.TabIndex = 8
        Me.nudInt.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(127, 14)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(50, 17)
        Me.CheckBox1.TabIndex = 7
        Me.CheckBox1.Text = "Loop"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(195, 9)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(203, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Update sensor readings"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rtf
        '
        Me.rtf.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtf.Location = New System.Drawing.Point(0, 0)
        Me.rtf.Name = "rtf"
        Me.rtf.Size = New System.Drawing.Size(411, 310)
        Me.rtf.TabIndex = 4
        Me.rtf.Text = ""
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.TextBox3)
        Me.Panel3.Controls.Add(Me.cmdConnect)
        Me.Panel3.Controls.Add(Me.txtHost)
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(505, 39)
        Me.Panel3.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(146, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(158, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Password ( leave blank if none )"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Adress"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(310, 8)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(116, 20)
        Me.TextBox3.TabIndex = 2
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(432, 6)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(61, 23)
        Me.cmdConnect.TabIndex = 1
        Me.cmdConnect.Text = "Connect"
        Me.cmdConnect.UseVisualStyleBackColor = True
        '
        'txtHost
        '
        Me.txtHost.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtHost.Location = New System.Drawing.Point(54, 8)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(86, 20)
        Me.txtHost.TabIndex = 0
        Me.txtHost.Text = "192.168.1.201:36330"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdSend)
        Me.Panel2.Controls.Add(Me.txtCmd)
        Me.Panel2.Location = New System.Drawing.Point(3, 271)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(505, 39)
        Me.Panel2.TabIndex = 1
        '
        'cmdSend
        '
        Me.cmdSend.Location = New System.Drawing.Point(396, 10)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(97, 23)
        Me.cmdSend.TabIndex = 1
        Me.cmdSend.Text = "Send"
        Me.cmdSend.UseVisualStyleBackColor = True
        '
        'txtCmd
        '
        Me.txtCmd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtCmd.Location = New System.Drawing.Point(211, 12)
        Me.txtCmd.Name = "txtCmd"
        Me.txtCmd.Size = New System.Drawing.Size(179, 20)
        Me.txtCmd.TabIndex = 0
        '
        'rtfC
        '
        Me.rtfC.Location = New System.Drawing.Point(2, 41)
        Me.rtfC.Name = "rtfC"
        Me.rtfC.Size = New System.Drawing.Size(502, 169)
        Me.rtfC.TabIndex = 0
        Me.rtfC.Text = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(920, 310)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudInt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents cmdConnect As System.Windows.Forms.Button
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents txtCmd As System.Windows.Forms.TextBox
    Friend WithEvents rtfC As System.Windows.Forms.RichTextBox
    Friend WithEvents nudInt As System.Windows.Forms.NumericUpDown

End Class
