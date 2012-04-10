<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditProject
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
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.txtNOatoms = New System.Windows.Forms.TextBox()
        Me.txtServerIP = New System.Windows.Forms.TextBox()
        Me.txtKfactor = New System.Windows.Forms.TextBox()
        Me.txtFinalDeadline = New System.Windows.Forms.TextBox()
        Me.txtCredit = New System.Windows.Forms.TextBox()
        Me.txtPrefferdDays = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Location = New System.Drawing.Point(272, 99)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(75, 23)
        Me.cmdUpdate.TabIndex = 2
        Me.cmdUpdate.Text = "&Update"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(21, 99)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'txtNOatoms
        '
        Me.txtNOatoms.Location = New System.Drawing.Point(260, 21)
        Me.txtNOatoms.Name = "txtNOatoms"
        Me.txtNOatoms.Size = New System.Drawing.Size(87, 20)
        Me.txtNOatoms.TabIndex = 44
        '
        'txtServerIP
        '
        Me.txtServerIP.Location = New System.Drawing.Point(260, 47)
        Me.txtServerIP.Name = "txtServerIP"
        Me.txtServerIP.Size = New System.Drawing.Size(87, 20)
        Me.txtServerIP.TabIndex = 43
        '
        'txtKfactor
        '
        Me.txtKfactor.Location = New System.Drawing.Point(260, 73)
        Me.txtKfactor.Name = "txtKfactor"
        Me.txtKfactor.Size = New System.Drawing.Size(87, 20)
        Me.txtKfactor.TabIndex = 42
        '
        'txtFinalDeadline
        '
        Me.txtFinalDeadline.Location = New System.Drawing.Point(93, 47)
        Me.txtFinalDeadline.Name = "txtFinalDeadline"
        Me.txtFinalDeadline.Size = New System.Drawing.Size(87, 20)
        Me.txtFinalDeadline.TabIndex = 41
        '
        'txtCredit
        '
        Me.txtCredit.Location = New System.Drawing.Point(93, 73)
        Me.txtCredit.Name = "txtCredit"
        Me.txtCredit.Size = New System.Drawing.Size(87, 20)
        Me.txtCredit.TabIndex = 40
        '
        'txtPrefferdDays
        '
        Me.txtPrefferdDays.Location = New System.Drawing.Point(93, 21)
        Me.txtPrefferdDays.Name = "txtPrefferdDays"
        Me.txtPrefferdDays.Size = New System.Drawing.Size(87, 20)
        Me.txtPrefferdDays.TabIndex = 39
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(193, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "No. Atoms"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(193, 76)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "kFactor"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 76)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "Credit"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "Final deadline"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "Prefferd days"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(193, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Server IP"
        '
        'frmEditProject
        '
        Me.AcceptButton = Me.cmdUpdate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.Dialog
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(364, 136)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtNOatoms)
        Me.Controls.Add(Me.txtServerIP)
        Me.Controls.Add(Me.txtKfactor)
        Me.Controls.Add(Me.txtFinalDeadline)
        Me.Controls.Add(Me.txtCredit)
        Me.Controls.Add(Me.txtPrefferdDays)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdUpdate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmEditProject"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Edit project"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdUpdate As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents txtNOatoms As System.Windows.Forms.TextBox
    Friend WithEvents txtServerIP As System.Windows.Forms.TextBox
    Friend WithEvents txtKfactor As System.Windows.Forms.TextBox
    Friend WithEvents txtFinalDeadline As System.Windows.Forms.TextBox
    Friend WithEvents txtCredit As System.Windows.Forms.TextBox
    Friend WithEvents txtPrefferdDays As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
