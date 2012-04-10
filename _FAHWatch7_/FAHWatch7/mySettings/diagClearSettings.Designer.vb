<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class diagClearSettings
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.chkRemoteClients = New System.Windows.Forms.CheckBox()
        Me.chkExceptions = New System.Windows.Forms.CheckBox()
        Me.chkEmail = New System.Windows.Forms.CheckBox()
        Me.chkAffinity = New System.Windows.Forms.CheckBox()
        Me.chkNonFatal = New System.Windows.Forms.CheckBox()
        Me.chkColumn = New System.Windows.Forms.CheckBox()
        Me.chkGeneral = New System.Windows.Forms.CheckBox()
        Me.chkGraphSettings = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(67, 212)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'chkRemoteClients
        '
        Me.chkRemoteClients.AutoSize = True
        Me.chkRemoteClients.Checked = True
        Me.chkRemoteClients.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRemoteClients.Location = New System.Drawing.Point(13, 13)
        Me.chkRemoteClients.Name = "chkRemoteClients"
        Me.chkRemoteClients.Size = New System.Drawing.Size(118, 17)
        Me.chkRemoteClients.TabIndex = 1
        Me.chkRemoteClients.Text = "Clear remote clients"
        Me.chkRemoteClients.UseVisualStyleBackColor = True
        '
        'chkExceptions
        '
        Me.chkExceptions.AutoSize = True
        Me.chkExceptions.Checked = True
        Me.chkExceptions.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkExceptions.Location = New System.Drawing.Point(13, 36)
        Me.chkExceptions.Name = "chkExceptions"
        Me.chkExceptions.Size = New System.Drawing.Size(136, 17)
        Me.chkExceptions.TabIndex = 2
        Me.chkExceptions.Text = "Clear stored exceptions"
        Me.chkExceptions.UseVisualStyleBackColor = True
        '
        'chkEmail
        '
        Me.chkEmail.AutoSize = True
        Me.chkEmail.Checked = True
        Me.chkEmail.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmail.Location = New System.Drawing.Point(13, 59)
        Me.chkEmail.Name = "chkEmail"
        Me.chkEmail.Size = New System.Drawing.Size(116, 17)
        Me.chkEmail.TabIndex = 3
        Me.chkEmail.Text = "Clear email settings"
        Me.chkEmail.UseVisualStyleBackColor = True
        '
        'chkAffinity
        '
        Me.chkAffinity.AutoSize = True
        Me.chkAffinity.Checked = True
        Me.chkAffinity.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAffinity.Location = New System.Drawing.Point(13, 82)
        Me.chkAffinity.Name = "chkAffinity"
        Me.chkAffinity.Size = New System.Drawing.Size(166, 17)
        Me.chkAffinity.TabIndex = 4
        Me.chkAffinity.Text = "Clear Affinity/Priority overrides"
        Me.chkAffinity.UseVisualStyleBackColor = True
        '
        'chkNonFatal
        '
        Me.chkNonFatal.AutoSize = True
        Me.chkNonFatal.Checked = True
        Me.chkNonFatal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNonFatal.Location = New System.Drawing.Point(13, 128)
        Me.chkNonFatal.Name = "chkNonFatal"
        Me.chkNonFatal.Size = New System.Drawing.Size(203, 17)
        Me.chkNonFatal.TabIndex = 6
        Me.chkNonFatal.Text = "Reset non fatal core status messages"
        Me.chkNonFatal.UseVisualStyleBackColor = True
        '
        'chkColumn
        '
        Me.chkColumn.AutoSize = True
        Me.chkColumn.Checked = True
        Me.chkColumn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkColumn.Location = New System.Drawing.Point(13, 105)
        Me.chkColumn.Name = "chkColumn"
        Me.chkColumn.Size = New System.Drawing.Size(130, 17)
        Me.chkColumn.TabIndex = 5
        Me.chkColumn.Text = "Reset column settings"
        Me.chkColumn.UseVisualStyleBackColor = True
        '
        'chkGeneral
        '
        Me.chkGeneral.AutoSize = True
        Me.chkGeneral.Checked = True
        Me.chkGeneral.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGeneral.Location = New System.Drawing.Point(13, 176)
        Me.chkGeneral.Name = "chkGeneral"
        Me.chkGeneral.Size = New System.Drawing.Size(178, 17)
        Me.chkGeneral.TabIndex = 8
        Me.chkGeneral.Text = "Reset general settings to default"
        Me.chkGeneral.UseVisualStyleBackColor = True
        '
        'chkGraphSettings
        '
        Me.chkGraphSettings.AutoSize = True
        Me.chkGraphSettings.Checked = True
        Me.chkGraphSettings.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGraphSettings.Location = New System.Drawing.Point(13, 153)
        Me.chkGraphSettings.Name = "chkGraphSettings"
        Me.chkGraphSettings.Size = New System.Drawing.Size(123, 17)
        Me.chkGraphSettings.TabIndex = 7
        Me.chkGraphSettings.Text = "Reset graph settings"
        Me.chkGraphSettings.UseVisualStyleBackColor = True
        '
        'diagClearSettings
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(225, 253)
        Me.Controls.Add(Me.chkGraphSettings)
        Me.Controls.Add(Me.chkGeneral)
        Me.Controls.Add(Me.chkColumn)
        Me.Controls.Add(Me.chkNonFatal)
        Me.Controls.Add(Me.chkAffinity)
        Me.Controls.Add(Me.chkEmail)
        Me.Controls.Add(Me.chkExceptions)
        Me.Controls.Add(Me.chkRemoteClients)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "diagClearSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Clear settings"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents chkRemoteClients As System.Windows.Forms.CheckBox
    Friend WithEvents chkExceptions As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmail As System.Windows.Forms.CheckBox
    Friend WithEvents chkAffinity As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonFatal As System.Windows.Forms.CheckBox
    Friend WithEvents chkColumn As System.Windows.Forms.CheckBox
    Friend WithEvents chkGeneral As System.Windows.Forms.CheckBox
    Friend WithEvents chkGraphSettings As System.Windows.Forms.CheckBox

End Class
