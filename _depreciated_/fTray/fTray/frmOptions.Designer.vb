<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.chkStart = New System.Windows.Forms.CheckBox()
        Me.chkClientStart = New System.Windows.Forms.CheckBox()
        Me.cmdAccept = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmbStart = New System.Windows.Forms.ComboBox()
        Me.lbNF = New System.Windows.Forms.ListBox()
        Me.cmdNFAdd = New System.Windows.Forms.Button()
        Me.cmdNFEdit = New System.Windows.Forms.Button()
        Me.cmdNFRemove = New System.Windows.Forms.Button()
        Me.chkEOC = New System.Windows.Forms.CheckBox()
        Me.gbEOC = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbEOCHistory = New System.Windows.Forms.ComboBox()
        Me.chkEOCNotify = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSummary = New System.Windows.Forms.TextBox()
        Me.chkStartMin = New System.Windows.Forms.CheckBox()
        Me.lnblSafe = New System.Windows.Forms.LinkLabel()
        Me.chkSafe = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.gbEOC.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkStart
        '
        Me.chkStart.AutoSize = True
        Me.chkStart.Location = New System.Drawing.Point(12, 12)
        Me.chkStart.Name = "chkStart"
        Me.chkStart.Size = New System.Drawing.Size(144, 21)
        Me.chkStart.TabIndex = 0
        Me.chkStart.Text = "Start with windows"
        Me.chkStart.UseVisualStyleBackColor = True
        '
        'chkClientStart
        '
        Me.chkClientStart.AutoSize = True
        Me.chkClientStart.Location = New System.Drawing.Point(12, 39)
        Me.chkClientStart.Name = "chkClientStart"
        Me.chkClientStart.Size = New System.Drawing.Size(172, 21)
        Me.chkClientStart.TabIndex = 1
        Me.chkClientStart.Text = "Start client automaticly"
        Me.chkClientStart.UseVisualStyleBackColor = True
        '
        'cmdAccept
        '
        Me.cmdAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAccept.Location = New System.Drawing.Point(254, 444)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.Size = New System.Drawing.Size(75, 27)
        Me.cmdAccept.TabIndex = 4
        Me.cmdAccept.Text = "&Accept"
        Me.cmdAccept.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(12, 444)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 27)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmbStart
        '
        Me.cmbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStart.FormattingEnabled = True
        Me.cmbStart.Items.AddRange(New Object() {"Registry", "Minimized - Registry"})
        Me.cmbStart.Location = New System.Drawing.Point(162, 10)
        Me.cmbStart.Name = "cmbStart"
        Me.cmbStart.Size = New System.Drawing.Size(167, 24)
        Me.cmbStart.TabIndex = 6
        '
        'lbNF
        '
        Me.lbNF.FormattingEnabled = True
        Me.lbNF.ItemHeight = 16
        Me.lbNF.Location = New System.Drawing.Point(12, 67)
        Me.lbNF.Name = "lbNF"
        Me.lbNF.Size = New System.Drawing.Size(317, 116)
        Me.lbNF.TabIndex = 7
        '
        'cmdNFAdd
        '
        Me.cmdNFAdd.Location = New System.Drawing.Point(253, 190)
        Me.cmdNFAdd.Name = "cmdNFAdd"
        Me.cmdNFAdd.Size = New System.Drawing.Size(75, 23)
        Me.cmdNFAdd.TabIndex = 8
        Me.cmdNFAdd.Text = "Add"
        Me.cmdNFAdd.UseVisualStyleBackColor = True
        '
        'cmdNFEdit
        '
        Me.cmdNFEdit.Location = New System.Drawing.Point(172, 190)
        Me.cmdNFEdit.Name = "cmdNFEdit"
        Me.cmdNFEdit.Size = New System.Drawing.Size(75, 23)
        Me.cmdNFEdit.TabIndex = 9
        Me.cmdNFEdit.Text = "Edit"
        Me.cmdNFEdit.UseVisualStyleBackColor = True
        '
        'cmdNFRemove
        '
        Me.cmdNFRemove.Location = New System.Drawing.Point(91, 190)
        Me.cmdNFRemove.Name = "cmdNFRemove"
        Me.cmdNFRemove.Size = New System.Drawing.Size(75, 23)
        Me.cmdNFRemove.TabIndex = 10
        Me.cmdNFRemove.Text = "Remove"
        Me.cmdNFRemove.UseVisualStyleBackColor = True
        '
        'chkEOC
        '
        Me.chkEOC.AutoSize = True
        Me.chkEOC.Location = New System.Drawing.Point(13, 222)
        Me.chkEOC.Name = "chkEOC"
        Me.chkEOC.Size = New System.Drawing.Size(277, 21)
        Me.chkEOC.TabIndex = 11
        Me.chkEOC.Text = "Use EOC xml feed and signature image"
        Me.chkEOC.UseVisualStyleBackColor = True
        '
        'gbEOC
        '
        Me.gbEOC.Controls.Add(Me.Label2)
        Me.gbEOC.Controls.Add(Me.cmbEOCHistory)
        Me.gbEOC.Controls.Add(Me.chkEOCNotify)
        Me.gbEOC.Location = New System.Drawing.Point(12, 249)
        Me.gbEOC.Name = "gbEOC"
        Me.gbEOC.Size = New System.Drawing.Size(317, 110)
        Me.gbEOC.TabIndex = 12
        Me.gbEOC.TabStop = False
        Me.gbEOC.Text = "EOC xml options"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(290, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Keep ExtremeOverClocking XML feed history"
        '
        'cmbEOCHistory
        '
        Me.cmbEOCHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEOCHistory.FormattingEnabled = True
        Me.cmbEOCHistory.Items.AddRange(New Object() {"Keep minimal history", "Keep history from one day", "Keep history from one week", "Keep history from one month", "Keep unlimited history"})
        Me.cmbEOCHistory.Location = New System.Drawing.Point(6, 48)
        Me.cmbEOCHistory.Name = "cmbEOCHistory"
        Me.cmbEOCHistory.Size = New System.Drawing.Size(304, 24)
        Me.cmbEOCHistory.TabIndex = 3
        '
        'chkEOCNotify
        '
        Me.chkEOCNotify.AutoSize = True
        Me.chkEOCNotify.Location = New System.Drawing.Point(6, 78)
        Me.chkEOCNotify.Name = "chkEOCNotify"
        Me.chkEOCNotify.Size = New System.Drawing.Size(228, 21)
        Me.chkEOCNotify.TabIndex = 1
        Me.chkEOCNotify.Text = "Enable notifications on updates"
        Me.chkEOCNotify.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 362)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(166, 17)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Project summary location"
        '
        'txtSummary
        '
        Me.txtSummary.Location = New System.Drawing.Point(11, 382)
        Me.txtSummary.Name = "txtSummary"
        Me.txtSummary.Size = New System.Drawing.Size(317, 22)
        Me.txtSummary.TabIndex = 14
        '
        'chkStartMin
        '
        Me.chkStartMin.AutoSize = True
        Me.chkStartMin.Location = New System.Drawing.Point(203, 39)
        Me.chkStartMin.Name = "chkStartMin"
        Me.chkStartMin.Size = New System.Drawing.Size(126, 21)
        Me.chkStartMin.TabIndex = 15
        Me.chkStartMin.Text = "Start minimized"
        Me.chkStartMin.UseVisualStyleBackColor = True
        '
        'lnblSafe
        '
        Me.lnblSafe.AutoSize = True
        Me.lnblSafe.Location = New System.Drawing.Point(235, 411)
        Me.lnblSafe.Name = "lnblSafe"
        Me.lnblSafe.Size = New System.Drawing.Size(34, 17)
        Me.lnblSafe.TabIndex = 19
        Me.lnblSafe.TabStop = True
        Me.lnblSafe.Text = "( ? )"
        '
        'chkSafe
        '
        Me.chkSafe.AutoSize = True
        Me.chkSafe.Location = New System.Drawing.Point(90, 410)
        Me.chkSafe.Name = "chkSafe"
        Me.chkSafe.Size = New System.Drawing.Size(147, 21)
        Me.chkSafe.TabIndex = 18
        Me.chkSafe.Text = "Disable safe mode"
        Me.chkSafe.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(72, 408)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 17)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "*"
        '
        'frmOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 483)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lnblSafe)
        Me.Controls.Add(Me.chkSafe)
        Me.Controls.Add(Me.chkStartMin)
        Me.Controls.Add(Me.txtSummary)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.gbEOC)
        Me.Controls.Add(Me.chkEOC)
        Me.Controls.Add(Me.cmdNFRemove)
        Me.Controls.Add(Me.cmdNFEdit)
        Me.Controls.Add(Me.cmdNFAdd)
        Me.Controls.Add(Me.lbNF)
        Me.Controls.Add(Me.cmbStart)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdAccept)
        Me.Controls.Add(Me.chkClientStart)
        Me.Controls.Add(Me.chkStart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmOptions"
        Me.Text = "Options"
        Me.TopMost = True
        Me.gbEOC.ResumeLayout(False)
        Me.gbEOC.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkStart As System.Windows.Forms.CheckBox
    Friend WithEvents chkClientStart As System.Windows.Forms.CheckBox
    Friend WithEvents cmdAccept As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmbStart As System.Windows.Forms.ComboBox
    Friend WithEvents lbNF As System.Windows.Forms.ListBox
    Friend WithEvents cmdNFAdd As System.Windows.Forms.Button
    Friend WithEvents cmdNFEdit As System.Windows.Forms.Button
    Friend WithEvents cmdNFRemove As System.Windows.Forms.Button
    Friend WithEvents chkEOC As System.Windows.Forms.CheckBox
    Friend WithEvents gbEOC As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSummary As System.Windows.Forms.TextBox
    Friend WithEvents chkEOCNotify As System.Windows.Forms.CheckBox
    Friend WithEvents cmbEOCHistory As System.Windows.Forms.ComboBox
    Friend WithEvents chkStartMin As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lnblSafe As System.Windows.Forms.LinkLabel
    Friend WithEvents chkSafe As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
