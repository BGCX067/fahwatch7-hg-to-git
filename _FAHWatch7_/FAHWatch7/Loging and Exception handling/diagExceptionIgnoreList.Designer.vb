<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class diagExceptionIgnoreList
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
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdSendAll = New System.Windows.Forms.Button()
        Me.cmdClearAll = New System.Windows.Forms.Button()
        Me.cmdRemove = New System.Windows.Forms.Button()
        Me.cmdSendNow = New System.Windows.Forms.Button()
        Me.rtMessage = New System.Windows.Forms.RichTextBox()
        Me.lvExceptions = New System.Windows.Forms.ListView()
        Me.chSource = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chErr = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chReported = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chEmpty = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(537, 431)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdSendAll)
        Me.GroupBox1.Controls.Add(Me.cmdClearAll)
        Me.GroupBox1.Controls.Add(Me.cmdRemove)
        Me.GroupBox1.Controls.Add(Me.cmdSendNow)
        Me.GroupBox1.Controls.Add(Me.rtMessage)
        Me.GroupBox1.Controls.Add(Me.lvExceptions)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(599, 417)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'cmdSendAll
        '
        Me.cmdSendAll.Location = New System.Drawing.Point(350, 381)
        Me.cmdSendAll.Name = "cmdSendAll"
        Me.cmdSendAll.Size = New System.Drawing.Size(75, 23)
        Me.cmdSendAll.TabIndex = 4
        Me.cmdSendAll.Text = "Send all"
        Me.ToolTip1.SetToolTip(Me.cmdSendAll, "Send all unsend exceptions to developer")
        Me.cmdSendAll.UseVisualStyleBackColor = True
        '
        'cmdClearAll
        '
        Me.cmdClearAll.Location = New System.Drawing.Point(516, 381)
        Me.cmdClearAll.Name = "cmdClearAll"
        Me.cmdClearAll.Size = New System.Drawing.Size(75, 23)
        Me.cmdClearAll.TabIndex = 2
        Me.cmdClearAll.Text = "Clear all"
        Me.ToolTip1.SetToolTip(Me.cmdClearAll, "Remove all exceptions from ignore list")
        Me.cmdClearAll.UseVisualStyleBackColor = True
        '
        'cmdRemove
        '
        Me.cmdRemove.Location = New System.Drawing.Point(433, 381)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(75, 23)
        Me.cmdRemove.TabIndex = 3
        Me.cmdRemove.Text = "Remove"
        Me.ToolTip1.SetToolTip(Me.cmdRemove, "Remove selected exception from ignore list")
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'cmdSendNow
        '
        Me.cmdSendNow.Location = New System.Drawing.Point(267, 381)
        Me.cmdSendNow.Name = "cmdSendNow"
        Me.cmdSendNow.Size = New System.Drawing.Size(75, 23)
        Me.cmdSendNow.TabIndex = 2
        Me.cmdSendNow.Text = "Send now"
        Me.ToolTip1.SetToolTip(Me.cmdSendNow, "Send selected exception to developer")
        Me.cmdSendNow.UseVisualStyleBackColor = True
        '
        'rtMessage
        '
        Me.rtMessage.BackColor = System.Drawing.SystemColors.Info
        Me.rtMessage.Location = New System.Drawing.Point(267, 14)
        Me.rtMessage.Name = "rtMessage"
        Me.rtMessage.ReadOnly = True
        Me.rtMessage.Size = New System.Drawing.Size(326, 361)
        Me.rtMessage.TabIndex = 1
        Me.rtMessage.Text = ""
        '
        'lvExceptions
        '
        Me.lvExceptions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chSource, Me.chErr, Me.chReported, Me.chEmpty})
        Me.lvExceptions.FullRowSelect = True
        Me.lvExceptions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvExceptions.HideSelection = False
        Me.lvExceptions.Location = New System.Drawing.Point(7, 14)
        Me.lvExceptions.Name = "lvExceptions"
        Me.lvExceptions.Size = New System.Drawing.Size(254, 390)
        Me.lvExceptions.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lvExceptions, "The list of known exceptions, reported exceptions will not result in the exceptio" & _
        "n report being displayed.")
        Me.lvExceptions.UseCompatibleStateImageBehavior = False
        Me.lvExceptions.View = System.Windows.Forms.View.Details
        '
        'chSource
        '
        Me.chSource.Text = "Source"
        Me.chSource.Width = 125
        '
        'chErr
        '
        Me.chErr.Text = "Err"
        Me.chErr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chReported
        '
        Me.chReported.Text = "Reported"
        Me.chReported.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.chReported.Width = 62
        '
        'chEmpty
        '
        Me.chEmpty.Text = ""
        Me.chEmpty.Width = 25
        '
        'ToolTip1
        '
        Me.ToolTip1.ShowAlways = True
        '
        'diagExceptionIgnoreList
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(609, 466)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "diagExceptionIgnoreList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Exceptions ignore list"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rtMessage As System.Windows.Forms.RichTextBox
    Friend WithEvents lvExceptions As System.Windows.Forms.ListView
    Friend WithEvents chSource As System.Windows.Forms.ColumnHeader
    Friend WithEvents chErr As System.Windows.Forms.ColumnHeader
    Friend WithEvents chReported As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEmpty As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents cmdSendNow As System.Windows.Forms.Button
    Friend WithEvents cmdClearAll As System.Windows.Forms.Button
    Friend WithEvents cmdSendAll As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
