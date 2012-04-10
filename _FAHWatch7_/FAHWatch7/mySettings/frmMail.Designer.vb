<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMail
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMail))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdRemoveAccount = New System.Windows.Forms.Button()
        Me.cmdNewAccount = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbProvider = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtSMTP = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.cmdValidate = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.gbTestSchedule = New System.Windows.Forms.GroupBox()
        Me.cmdTestSchedule = New System.Windows.Forms.Button()
        Me.dtpTestSchedule = New System.Windows.Forms.DateTimePicker()
        Me.txtAddR = New System.Windows.Forms.TextBox()
        Me.gbDetailsR = New System.Windows.Forms.GroupBox()
        Me.chkAlways = New System.Windows.Forms.CheckBox()
        Me.pnSchedule = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtpBefore = New System.Windows.Forms.DateTimePicker()
        Me.dtpAfter = New System.Windows.Forms.DateTimePicker()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.rbBetween = New System.Windows.Forms.RadioButton()
        Me.rbWeekends = New System.Windows.Forms.RadioButton()
        Me.rbWeekDays = New System.Windows.Forms.RadioButton()
        Me.cmdRemoveR = New System.Windows.Forms.Button()
        Me.cmdAddR = New System.Windows.Forms.Button()
        Me.lbReciepients = New System.Windows.Forms.ListBox()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.gbTestSchedule.SuspendLayout()
        Me.gbDetailsR.SuspendLayout()
        Me.pnSchedule.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdRemoveAccount)
        Me.GroupBox1.Controls.Add(Me.cmdNewAccount)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.cmdValidate)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(489, 143)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'cmdRemoveAccount
        '
        Me.cmdRemoveAccount.Enabled = False
        Me.cmdRemoveAccount.Location = New System.Drawing.Point(123, 110)
        Me.cmdRemoveAccount.Name = "cmdRemoveAccount"
        Me.cmdRemoveAccount.Size = New System.Drawing.Size(110, 23)
        Me.cmdRemoveAccount.TabIndex = 16
        Me.cmdRemoveAccount.Text = "Clear Account"
        Me.cmdRemoveAccount.UseVisualStyleBackColor = True
        '
        'cmdNewAccount
        '
        Me.cmdNewAccount.Location = New System.Drawing.Point(7, 110)
        Me.cmdNewAccount.Name = "cmdNewAccount"
        Me.cmdNewAccount.Size = New System.Drawing.Size(110, 23)
        Me.cmdNewAccount.TabIndex = 15
        Me.cmdNewAccount.Text = "New account"
        Me.cmdNewAccount.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.cmbProvider)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.txtPassword)
        Me.GroupBox3.Controls.Add(Me.txtSMTP)
        Me.GroupBox3.Controls.Add(Me.txtUsername)
        Me.GroupBox3.Controls.Add(Me.txtPort)
        Me.GroupBox3.Location = New System.Drawing.Point(7, 9)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(471, 93)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Configure account"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "SMTP port"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select your mail host"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(230, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Password"
        '
        'cmbProvider
        '
        Me.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbProvider.FormattingEnabled = True
        Me.cmbProvider.Location = New System.Drawing.Point(116, 16)
        Me.cmbProvider.Name = "cmbProvider"
        Me.cmbProvider.Size = New System.Drawing.Size(349, 21)
        Me.cmbProvider.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.cmbProvider, resources.GetString("cmbProvider.ToolTip"))
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(230, 46)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Mail address"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "SMTP server"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(302, 66)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(163, 20)
        Me.txtPassword.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtPassword, "Enter your account password ( needed to access your mail providers smtp servers )" & _
        " *Warning password is stored in plain text")
        '
        'txtSMTP
        '
        Me.txtSMTP.Location = New System.Drawing.Point(116, 40)
        Me.txtSMTP.Name = "txtSMTP"
        Me.txtSMTP.ReadOnly = True
        Me.txtSMTP.Size = New System.Drawing.Size(106, 20)
        Me.txtSMTP.TabIndex = 6
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(302, 43)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(163, 20)
        Me.txtUsername.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.txtUsername, "Enter your mail adress here ( for example: yourname@yourprovider.com )")
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(116, 63)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.ReadOnly = True
        Me.txtPort.Size = New System.Drawing.Size(106, 20)
        Me.txtPort.TabIndex = 7
        '
        'cmdValidate
        '
        Me.cmdValidate.Enabled = False
        Me.cmdValidate.Location = New System.Drawing.Point(259, 108)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.Size = New System.Drawing.Size(219, 25)
        Me.cmdValidate.TabIndex = 13
        Me.cmdValidate.Text = "Send validation mail"
        Me.cmdValidate.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gbTestSchedule)
        Me.GroupBox2.Controls.Add(Me.txtAddR)
        Me.GroupBox2.Controls.Add(Me.gbDetailsR)
        Me.GroupBox2.Controls.Add(Me.cmdRemoveR)
        Me.GroupBox2.Controls.Add(Me.cmdAddR)
        Me.GroupBox2.Controls.Add(Me.lbReciepients)
        Me.GroupBox2.Location = New System.Drawing.Point(5, 152)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(489, 225)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Recipients"
        '
        'gbTestSchedule
        '
        Me.gbTestSchedule.Controls.Add(Me.cmdTestSchedule)
        Me.gbTestSchedule.Controls.Add(Me.dtpTestSchedule)
        Me.gbTestSchedule.Location = New System.Drawing.Point(12, 175)
        Me.gbTestSchedule.Name = "gbTestSchedule"
        Me.gbTestSchedule.Size = New System.Drawing.Size(299, 42)
        Me.gbTestSchedule.TabIndex = 6
        Me.gbTestSchedule.TabStop = False
        '
        'cmdTestSchedule
        '
        Me.cmdTestSchedule.Location = New System.Drawing.Point(6, 13)
        Me.cmdTestSchedule.Name = "cmdTestSchedule"
        Me.cmdTestSchedule.Size = New System.Drawing.Size(182, 23)
        Me.cmdTestSchedule.TabIndex = 4
        Me.cmdTestSchedule.Text = "Test schedule "
        Me.ToolTip1.SetToolTip(Me.cmdTestSchedule, "Simulate an event for which you set an alert to take place at the given time and " & _
        "test if the mail is sent to the right adress. ")
        Me.cmdTestSchedule.UseVisualStyleBackColor = True
        '
        'dtpTestSchedule
        '
        Me.dtpTestSchedule.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTestSchedule.Location = New System.Drawing.Point(194, 14)
        Me.dtpTestSchedule.Name = "dtpTestSchedule"
        Me.dtpTestSchedule.ShowUpDown = True
        Me.dtpTestSchedule.Size = New System.Drawing.Size(97, 20)
        Me.dtpTestSchedule.TabIndex = 3
        Me.dtpTestSchedule.Value = New Date(2012, 2, 19, 12, 0, 0, 0)
        '
        'txtAddR
        '
        Me.txtAddR.Enabled = False
        Me.txtAddR.Location = New System.Drawing.Point(12, 152)
        Me.txtAddR.Name = "txtAddR"
        Me.txtAddR.Size = New System.Drawing.Size(299, 20)
        Me.txtAddR.TabIndex = 5
        '
        'gbDetailsR
        '
        Me.gbDetailsR.Controls.Add(Me.chkAlways)
        Me.gbDetailsR.Controls.Add(Me.pnSchedule)
        Me.gbDetailsR.Location = New System.Drawing.Point(317, 14)
        Me.gbDetailsR.Name = "gbDetailsR"
        Me.gbDetailsR.Size = New System.Drawing.Size(161, 203)
        Me.gbDetailsR.TabIndex = 4
        Me.gbDetailsR.TabStop = False
        Me.gbDetailsR.Text = "Schedule"
        '
        'chkAlways
        '
        Me.chkAlways.AutoSize = True
        Me.chkAlways.Location = New System.Drawing.Point(6, 17)
        Me.chkAlways.Name = "chkAlways"
        Me.chkAlways.Size = New System.Drawing.Size(106, 17)
        Me.chkAlways.TabIndex = 7
        Me.chkAlways.Text = "Always send mail"
        Me.chkAlways.UseVisualStyleBackColor = True
        '
        'pnSchedule
        '
        Me.pnSchedule.Controls.Add(Me.Panel2)
        Me.pnSchedule.Controls.Add(Me.rbBetween)
        Me.pnSchedule.Controls.Add(Me.rbWeekends)
        Me.pnSchedule.Controls.Add(Me.rbWeekDays)
        Me.pnSchedule.Location = New System.Drawing.Point(6, 37)
        Me.pnSchedule.Name = "pnSchedule"
        Me.pnSchedule.Size = New System.Drawing.Size(149, 160)
        Me.pnSchedule.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dtpBefore)
        Me.Panel2.Controls.Add(Me.dtpAfter)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Location = New System.Drawing.Point(20, 80)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(113, 71)
        Me.Panel2.TabIndex = 3
        '
        'dtpBefore
        '
        Me.dtpBefore.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpBefore.Location = New System.Drawing.Point(7, 44)
        Me.dtpBefore.Name = "dtpBefore"
        Me.dtpBefore.ShowUpDown = True
        Me.dtpBefore.Size = New System.Drawing.Size(97, 20)
        Me.dtpBefore.TabIndex = 2
        Me.dtpBefore.Value = New Date(2012, 2, 19, 0, 0, 0, 0)
        '
        'dtpAfter
        '
        Me.dtpAfter.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpAfter.Location = New System.Drawing.Point(7, 5)
        Me.dtpAfter.Name = "dtpAfter"
        Me.dtpAfter.ShowUpDown = True
        Me.dtpAfter.Size = New System.Drawing.Size(97, 20)
        Me.dtpAfter.TabIndex = 1
        Me.dtpAfter.Value = New Date(2012, 2, 19, 12, 0, 0, 0)
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(33, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(25, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "and"
        '
        'rbBetween
        '
        Me.rbBetween.AutoSize = True
        Me.rbBetween.Location = New System.Drawing.Point(8, 56)
        Me.rbBetween.Name = "rbBetween"
        Me.rbBetween.Size = New System.Drawing.Size(70, 17)
        Me.rbBetween.TabIndex = 2
        Me.rbBetween.TabStop = True
        Me.rbBetween.Text = "Between "
        Me.rbBetween.UseVisualStyleBackColor = True
        '
        'rbWeekends
        '
        Me.rbWeekends.AutoSize = True
        Me.rbWeekends.Location = New System.Drawing.Point(8, 32)
        Me.rbWeekends.Name = "rbWeekends"
        Me.rbWeekends.Size = New System.Drawing.Size(108, 17)
        Me.rbWeekends.TabIndex = 1
        Me.rbWeekends.TabStop = True
        Me.rbWeekends.Text = "During weekends"
        Me.rbWeekends.UseVisualStyleBackColor = True
        '
        'rbWeekDays
        '
        Me.rbWeekDays.AutoSize = True
        Me.rbWeekDays.Location = New System.Drawing.Point(9, 8)
        Me.rbWeekDays.Name = "rbWeekDays"
        Me.rbWeekDays.Size = New System.Drawing.Size(93, 17)
        Me.rbWeekDays.TabIndex = 0
        Me.rbWeekDays.TabStop = True
        Me.rbWeekDays.Text = "On week days"
        Me.rbWeekDays.UseVisualStyleBackColor = True
        '
        'cmdRemoveR
        '
        Me.cmdRemoveR.Location = New System.Drawing.Point(236, 121)
        Me.cmdRemoveR.Name = "cmdRemoveR"
        Me.cmdRemoveR.Size = New System.Drawing.Size(75, 23)
        Me.cmdRemoveR.TabIndex = 3
        Me.cmdRemoveR.Text = "Remove"
        Me.cmdRemoveR.UseVisualStyleBackColor = True
        '
        'cmdAddR
        '
        Me.cmdAddR.Location = New System.Drawing.Point(158, 121)
        Me.cmdAddR.Name = "cmdAddR"
        Me.cmdAddR.Size = New System.Drawing.Size(75, 23)
        Me.cmdAddR.TabIndex = 1
        Me.cmdAddR.Text = "Add"
        Me.cmdAddR.UseVisualStyleBackColor = True
        '
        'lbReciepients
        '
        Me.lbReciepients.FormattingEnabled = True
        Me.lbReciepients.Location = New System.Drawing.Point(12, 20)
        Me.lbReciepients.Name = "lbReciepients"
        Me.lbReciepients.Size = New System.Drawing.Size(299, 95)
        Me.lbReciepients.TabIndex = 0
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(419, 383)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Text = "&Ok"
        Me.cmdOk.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(328, 383)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.ShowAlways = True
        '
        'frmMail
        '
        Me.AcceptButton = Me.cmdOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(501, 418)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMail"
        Me.Text = "Configure mail accounts"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.gbTestSchedule.ResumeLayout(False)
        Me.gbDetailsR.ResumeLayout(False)
        Me.gbDetailsR.PerformLayout()
        Me.pnSchedule.ResumeLayout(False)
        Me.pnSchedule.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbProvider As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdValidate As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtSMTP As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdRemoveR As System.Windows.Forms.Button
    Friend WithEvents cmdAddR As System.Windows.Forms.Button
    Friend WithEvents lbReciepients As System.Windows.Forms.ListBox
    Friend WithEvents gbDetailsR As System.Windows.Forms.GroupBox
    Friend WithEvents txtAddR As System.Windows.Forms.TextBox
    Friend WithEvents chkAlways As System.Windows.Forms.CheckBox
    Friend WithEvents pnSchedule As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dtpBefore As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpAfter As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents rbBetween As System.Windows.Forms.RadioButton
    Friend WithEvents rbWeekends As System.Windows.Forms.RadioButton
    Friend WithEvents rbWeekDays As System.Windows.Forms.RadioButton
    Friend WithEvents cmdOk As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents gbTestSchedule As System.Windows.Forms.GroupBox
    Friend WithEvents cmdTestSchedule As System.Windows.Forms.Button
    Friend WithEvents dtpTestSchedule As System.Windows.Forms.DateTimePicker
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents cmdRemoveAccount As System.Windows.Forms.Button
    Friend WithEvents cmdNewAccount As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
End Class
