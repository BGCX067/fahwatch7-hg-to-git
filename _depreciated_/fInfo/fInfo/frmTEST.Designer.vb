<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTEST
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkDRTF = New System.Windows.Forms.CheckBox()
        Me.nudInt = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.rtf = New System.Windows.Forms.RichTextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.nudOpt = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkCPU = New System.Windows.Forms.CheckBox()
        Me.chkDRTFC = New System.Windows.Forms.CheckBox()
        Me.cmdClientUpdates = New System.Windows.Forms.Button()
        Me.nudClient = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.cmdConnect = New System.Windows.Forms.Button()
        Me.txtHost = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cmdSend = New System.Windows.Forms.Button()
        Me.txtCmd = New System.Windows.Forms.TextBox()
        Me.rtfC = New System.Windows.Forms.RichTextBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudInt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.nudOpt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudClient, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.SplitContainer1.Size = New System.Drawing.Size(1018, 398)
        Me.SplitContainer1.SplitterDistance = 484
        Me.SplitContainer1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.chkDRTF)
        Me.Panel1.Controls.Add(Me.nudInt)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 359)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(484, 39)
        Me.Panel1.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(180, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Interval (ms)"
        '
        'chkDRTF
        '
        Me.chkDRTF.AutoSize = True
        Me.chkDRTF.Location = New System.Drawing.Point(72, 10)
        Me.chkDRTF.Name = "chkDRTF"
        Me.chkDRTF.Size = New System.Drawing.Size(102, 17)
        Me.chkDRTF.TabIndex = 9
        Me.chkDRTF.Text = "Disable scrolling"
        Me.chkDRTF.UseVisualStyleBackColor = True
        '
        'nudInt
        '
        Me.nudInt.Location = New System.Drawing.Point(249, 7)
        Me.nudInt.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
        Me.nudInt.Name = "nudInt"
        Me.nudInt.Size = New System.Drawing.Size(64, 20)
        Me.nudInt.TabIndex = 8
        Me.nudInt.Value = New Decimal(New Integer() {2500, 0, 0, 0})
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(319, 7)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(162, 21)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Update sensor readings"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rtf
        '
        Me.rtf.Location = New System.Drawing.Point(12, 7)
        Me.rtf.Name = "rtf"
        Me.rtf.Size = New System.Drawing.Size(371, 235)
        Me.rtf.TabIndex = 4
        Me.rtf.Text = ""
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.nudOpt)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.chkCPU)
        Me.Panel3.Controls.Add(Me.chkDRTFC)
        Me.Panel3.Controls.Add(Me.cmdClientUpdates)
        Me.Panel3.Controls.Add(Me.nudClient)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.TextBox3)
        Me.Panel3.Controls.Add(Me.cmdConnect)
        Me.Panel3.Controls.Add(Me.txtHost)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(530, 104)
        Me.Panel3.TabIndex = 2
        '
        'nudOpt
        '
        Me.nudOpt.Location = New System.Drawing.Point(373, 54)
        Me.nudOpt.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
        Me.nudOpt.Minimum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudOpt.Name = "nudOpt"
        Me.nudOpt.Size = New System.Drawing.Size(78, 20)
        Me.nudOpt.TabIndex = 11
        Me.nudOpt.Value = New Decimal(New Integer() {10000, 0, 0, 0})
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(262, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(102, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Options interval (ms)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(37, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Queue/Slot interval (ms)"
        '
        'chkCPU
        '
        Me.chkCPU.AutoSize = True
        Me.chkCPU.Location = New System.Drawing.Point(12, 33)
        Me.chkCPU.Name = "chkCPU"
        Me.chkCPU.Size = New System.Drawing.Size(101, 17)
        Me.chkCPU.TabIndex = 8
        Me.chkCPU.Text = "Save cpu mode"
        Me.chkCPU.UseVisualStyleBackColor = True
        '
        'chkDRTFC
        '
        Me.chkDRTFC.AutoSize = True
        Me.chkDRTFC.Location = New System.Drawing.Point(184, 33)
        Me.chkDRTFC.Name = "chkDRTFC"
        Me.chkDRTFC.Size = New System.Drawing.Size(154, 17)
        Me.chkDRTFC.TabIndex = 7
        Me.chkDRTFC.Text = "Only output change events"
        Me.chkDRTFC.UseVisualStyleBackColor = True
        '
        'cmdClientUpdates
        '
        Me.cmdClientUpdates.Location = New System.Drawing.Point(344, 29)
        Me.cmdClientUpdates.Name = "cmdClientUpdates"
        Me.cmdClientUpdates.Size = New System.Drawing.Size(149, 23)
        Me.cmdClientUpdates.TabIndex = 6
        Me.cmdClientUpdates.Text = "Enable updates"
        Me.cmdClientUpdates.UseVisualStyleBackColor = True
        '
        'nudClient
        '
        Me.nudClient.Location = New System.Drawing.Point(164, 54)
        Me.nudClient.Maximum = New Decimal(New Integer() {60000, 0, 0, 0})
        Me.nudClient.Minimum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudClient.Name = "nudClient"
        Me.nudClient.Size = New System.Drawing.Size(78, 20)
        Me.nudClient.TabIndex = 5
        Me.nudClient.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(173, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Password"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Adress"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(232, 7)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(106, 20)
        Me.TextBox3.TabIndex = 2
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(344, 6)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(149, 23)
        Me.cmdConnect.TabIndex = 1
        Me.cmdConnect.Text = "Connect"
        Me.cmdConnect.UseVisualStyleBackColor = True
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(54, 7)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(113, 20)
        Me.txtHost.TabIndex = 0
        Me.txtHost.Text = "192.168.1.201:36330"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdSend)
        Me.Panel2.Controls.Add(Me.txtCmd)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 359)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(530, 39)
        Me.Panel2.TabIndex = 1
        '
        'cmdSend
        '
        Me.cmdSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSend.Location = New System.Drawing.Point(421, 7)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(97, 23)
        Me.cmdSend.TabIndex = 1
        Me.cmdSend.Text = "Send"
        Me.cmdSend.UseVisualStyleBackColor = True
        '
        'txtCmd
        '
        Me.txtCmd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCmd.Location = New System.Drawing.Point(236, 9)
        Me.txtCmd.Name = "txtCmd"
        Me.txtCmd.Size = New System.Drawing.Size(179, 20)
        Me.txtCmd.TabIndex = 0
        '
        'rtfC
        '
        Me.rtfC.Location = New System.Drawing.Point(0, 152)
        Me.rtfC.Name = "rtfC"
        Me.rtfC.Size = New System.Drawing.Size(502, 201)
        Me.rtfC.TabIndex = 0
        Me.rtfC.Text = ""
        '
        'Timer2
        '
        Me.Timer2.Interval = 1000
        '
        'Timer3
        '
        Me.Timer3.Interval = 5000
        '
        'frmTEST
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1018, 398)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmTEST"
        Me.Text = "fInfo test form"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudInt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.nudOpt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudClient, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents rtf As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
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
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents cmdClientUpdates As System.Windows.Forms.Button
    Friend WithEvents nudClient As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkDRTF As System.Windows.Forms.CheckBox
    Friend WithEvents chkDRTFC As System.Windows.Forms.CheckBox
    Friend WithEvents chkCPU As System.Windows.Forms.CheckBox
    Friend WithEvents nudOpt As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents Label5 As System.Windows.Forms.Label

End Class
