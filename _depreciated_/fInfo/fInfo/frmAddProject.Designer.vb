<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddProject
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
        Me.txtkFactor = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtProjectNumber = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNOatoms = New System.Windows.Forms.TextBox()
        Me.txtServerIP = New System.Windows.Forms.TextBox()
        Me.txtContact = New System.Windows.Forms.TextBox()
        Me.txtFinalDeadline = New System.Windows.Forms.TextBox()
        Me.txtCredit = New System.Windows.Forms.TextBox()
        Me.txtPrefferdDays = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtkFactor
        '
        Me.txtkFactor.Location = New System.Drawing.Point(259, 96)
        Me.txtkFactor.Name = "txtkFactor"
        Me.txtkFactor.Size = New System.Drawing.Size(87, 20)
        Me.txtkFactor.TabIndex = 69
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(194, 98)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 68
        Me.Label6.Text = "kFactor"
        '
        'txtProjectNumber
        '
        Me.txtProjectNumber.Location = New System.Drawing.Point(96, 19)
        Me.txtProjectNumber.Name = "txtProjectNumber"
        Me.txtProjectNumber.Size = New System.Drawing.Size(87, 20)
        Me.txtProjectNumber.TabIndex = 54
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(21, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "Project"
        '
        'txtNOatoms
        '
        Me.txtNOatoms.Location = New System.Drawing.Point(259, 19)
        Me.txtNOatoms.Name = "txtNOatoms"
        Me.txtNOatoms.Size = New System.Drawing.Size(87, 20)
        Me.txtNOatoms.TabIndex = 62
        '
        'txtServerIP
        '
        Me.txtServerIP.Location = New System.Drawing.Point(259, 45)
        Me.txtServerIP.Name = "txtServerIP"
        Me.txtServerIP.Size = New System.Drawing.Size(87, 20)
        Me.txtServerIP.TabIndex = 63
        '
        'txtContact
        '
        Me.txtContact.Location = New System.Drawing.Point(259, 71)
        Me.txtContact.Name = "txtContact"
        Me.txtContact.Size = New System.Drawing.Size(87, 20)
        Me.txtContact.TabIndex = 64
        '
        'txtFinalDeadline
        '
        Me.txtFinalDeadline.Location = New System.Drawing.Point(96, 71)
        Me.txtFinalDeadline.Name = "txtFinalDeadline"
        Me.txtFinalDeadline.Size = New System.Drawing.Size(87, 20)
        Me.txtFinalDeadline.TabIndex = 57
        '
        'txtCredit
        '
        Me.txtCredit.Location = New System.Drawing.Point(96, 98)
        Me.txtCredit.Name = "txtCredit"
        Me.txtCredit.Size = New System.Drawing.Size(87, 20)
        Me.txtCredit.TabIndex = 60
        '
        'txtPrefferdDays
        '
        Me.txtPrefferdDays.Location = New System.Drawing.Point(96, 45)
        Me.txtPrefferdDays.Name = "txtPrefferdDays"
        Me.txtPrefferdDays.Size = New System.Drawing.Size(87, 20)
        Me.txtPrefferdDays.TabIndex = 55
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Location = New System.Drawing.Point(192, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 66
        Me.Label10.Text = "No. Atoms"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(192, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(44, 13)
        Me.Label8.TabIndex = 65
        Me.Label8.Text = "Contact"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(21, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "Credit"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(21, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Final deadline"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(21, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 58
        Me.Label3.Text = "Prefferd days"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(192, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 56
        Me.Label2.Text = "Server IP"
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Location = New System.Drawing.Point(271, 122)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(75, 23)
        Me.cmdUpdate.TabIndex = 52
        Me.cmdUpdate.Text = "&Add"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(18, 122)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 53
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'frmAddProject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.Dialog
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(368, 160)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtkFactor)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtProjectNumber)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNOatoms)
        Me.Controls.Add(Me.txtServerIP)
        Me.Controls.Add(Me.txtContact)
        Me.Controls.Add(Me.txtFinalDeadline)
        Me.Controls.Add(Me.txtCredit)
        Me.Controls.Add(Me.txtPrefferdDays)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Controls.Add(Me.cmdCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmAddProject"
        Me.ShowIcon = False
        Me.Text = "Add a project reference"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtkFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtProjectNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNOatoms As System.Windows.Forms.TextBox
    Friend WithEvents txtServerIP As System.Windows.Forms.TextBox
    Friend WithEvents txtContact As System.Windows.Forms.TextBox
    Friend WithEvents txtFinalDeadline As System.Windows.Forms.TextBox
    Friend WithEvents txtCredit As System.Windows.Forms.TextBox
    Friend WithEvents txtPrefferdDays As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdUpdate As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
End Class
