<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPBList
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPBList))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lvProjects = New System.Windows.Forms.ListView()
        Me.cmProject = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmServerIP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmWUName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmAtoms = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmPreffered = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmFinal = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmCredit = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmFrames = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmCode = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmContact = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmKFactor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.txtDescription = New System.Windows.Forms.RichTextBox()
        Me.cmdPurge = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvProjects)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(813, 559)
        Me.SplitContainer1.SplitterDistance = 457
        Me.SplitContainer1.TabIndex = 10
        '
        'lvProjects
        '
        Me.lvProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.cmProject, Me.cmServerIP, Me.cmWUName, Me.cmAtoms, Me.cmPreffered, Me.cmFinal, Me.cmCredit, Me.cmFrames, Me.cmCode, Me.cmContact, Me.cmKFactor, Me.cmType})
        Me.lvProjects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvProjects.FullRowSelect = True
        Me.lvProjects.GridLines = True
        Me.lvProjects.Location = New System.Drawing.Point(0, 0)
        Me.lvProjects.Name = "lvProjects"
        Me.lvProjects.Size = New System.Drawing.Size(813, 457)
        Me.lvProjects.TabIndex = 5
        Me.lvProjects.UseCompatibleStateImageBehavior = False
        Me.lvProjects.View = System.Windows.Forms.View.Details
        '
        'cmProject
        '
        Me.cmProject.Text = "Projectnumber"
        Me.cmProject.Width = 84
        '
        'cmServerIP
        '
        Me.cmServerIP.Text = "Server IP"
        '
        'cmWUName
        '
        Me.cmWUName.Text = "Work Unit Name"
        Me.cmWUName.Width = 98
        '
        'cmAtoms
        '
        Me.cmAtoms.Text = "Number of Atoms"
        Me.cmAtoms.Width = 102
        '
        'cmPreffered
        '
        Me.cmPreffered.Text = "Preferred days"
        Me.cmPreffered.Width = 92
        '
        'cmFinal
        '
        Me.cmFinal.Text = "Final deadline"
        Me.cmFinal.Width = 84
        '
        'cmCredit
        '
        Me.cmCredit.Text = "Credit"
        Me.cmCredit.Width = 40
        '
        'cmFrames
        '
        Me.cmFrames.Text = "Frames"
        Me.cmFrames.Width = 47
        '
        'cmCode
        '
        Me.cmCode.Text = "Code"
        Me.cmCode.Width = 39
        '
        'cmContact
        '
        Me.cmContact.Text = "Contact"
        Me.cmContact.Width = 49
        '
        'cmKFactor
        '
        Me.cmKFactor.Text = "kFactor"
        Me.cmKFactor.Width = 50
        '
        'cmType
        '
        Me.cmType.Text = "unit type"
        Me.cmType.Width = 53
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtDescription)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.cmdPurge)
        Me.SplitContainer2.Panel2.Controls.Add(Me.cmdAdd)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer2.Panel2.Controls.Add(Me.cmdDelete)
        Me.SplitContainer2.Panel2.Controls.Add(Me.cmdClose)
        Me.SplitContainer2.Size = New System.Drawing.Size(813, 98)
        Me.SplitContainer2.SplitterDistance = 62
        Me.SplitContainer2.SplitterWidth = 2
        Me.SplitContainer2.TabIndex = 0
        '
        'txtDescription
        '
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Info
        Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDescription.ForeColor = System.Drawing.SystemColors.InfoText
        Me.txtDescription.Location = New System.Drawing.Point(0, 0)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(813, 62)
        Me.txtDescription.TabIndex = 0
        Me.txtDescription.Text = ""
        '
        'cmdPurge
        '
        Me.cmdPurge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPurge.Location = New System.Drawing.Point(474, 5)
        Me.cmdPurge.Name = "cmdPurge"
        Me.cmdPurge.Size = New System.Drawing.Size(75, 23)
        Me.cmdPurge.TabIndex = 14
        Me.cmdPurge.Text = "&Purge"
        Me.cmdPurge.UseVisualStyleBackColor = True
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.Location = New System.Drawing.Point(642, 5)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(75, 23)
        Me.cmdAdd.TabIndex = 13
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(388, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "*To delete project, select them and press the delete button. To edit, double clic" & _
    "k."
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.Location = New System.Drawing.Point(558, 5)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(75, 23)
        Me.cmdDelete.TabIndex = 11
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(726, 5)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 10
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'frmPBList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(813, 559)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPBList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Known Projects list"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lvProjects As System.Windows.Forms.ListView
    Friend WithEvents cmProject As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmServerIP As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmWUName As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmAtoms As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmPreffered As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmFinal As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmCredit As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmFrames As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmCode As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmContact As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmKFactor As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmType As System.Windows.Forms.ColumnHeader
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdPurge As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents txtDescription As System.Windows.Forms.RichTextBox
End Class
