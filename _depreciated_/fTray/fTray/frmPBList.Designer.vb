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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPBList))
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
        Me.cmDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmContact = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmKFactor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cMenuD = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewProjectDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdPurge = New System.Windows.Forms.Button()
        Me.cMenuD.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvProjects
        '
        Me.lvProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.cmProject, Me.cmServerIP, Me.cmWUName, Me.cmAtoms, Me.cmPreffered, Me.cmFinal, Me.cmCredit, Me.cmFrames, Me.cmCode, Me.cmDescription, Me.cmContact, Me.cmKFactor})
        Me.lvProjects.ContextMenuStrip = Me.cMenuD
        Me.lvProjects.Dock = System.Windows.Forms.DockStyle.Top
        Me.lvProjects.FullRowSelect = True
        Me.lvProjects.GridLines = True
        Me.lvProjects.Location = New System.Drawing.Point(0, 0)
        Me.lvProjects.Margin = New System.Windows.Forms.Padding(4)
        Me.lvProjects.Name = "lvProjects"
        Me.lvProjects.Size = New System.Drawing.Size(1084, 308)
        Me.lvProjects.TabIndex = 4
        Me.lvProjects.UseCompatibleStateImageBehavior = False
        Me.lvProjects.View = System.Windows.Forms.View.Details
        '
        'cmProject
        '
        Me.cmProject.Text = "Projectnumber"
        '
        'cmServerIP
        '
        Me.cmServerIP.Text = "Server IP"
        '
        'cmWUName
        '
        Me.cmWUName.Text = "Work Unit Name"
        '
        'cmAtoms
        '
        Me.cmAtoms.Text = "Number of Atoms"
        '
        'cmPreffered
        '
        Me.cmPreffered.Text = "Preferred days"
        Me.cmPreffered.Width = 63
        '
        'cmFinal
        '
        Me.cmFinal.Text = "Final deadline"
        '
        'cmCredit
        '
        Me.cmCredit.Text = "Credit"
        '
        'cmFrames
        '
        Me.cmFrames.Text = "Frames"
        '
        'cmCode
        '
        Me.cmCode.Text = "Code"
        '
        'cmDescription
        '
        Me.cmDescription.Text = "Description (link)"
        '
        'cmContact
        '
        Me.cmContact.Text = "Contact"
        '
        'cmKFactor
        '
        Me.cmKFactor.Text = "kFactor"
        '
        'cMenuD
        '
        Me.cMenuD.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewProjectDescriptionToolStripMenuItem})
        Me.cMenuD.Name = "cMenuD"
        Me.cMenuD.Size = New System.Drawing.Size(240, 28)
        '
        'ViewProjectDescriptionToolStripMenuItem
        '
        Me.ViewProjectDescriptionToolStripMenuItem.Name = "ViewProjectDescriptionToolStripMenuItem"
        Me.ViewProjectDescriptionToolStripMenuItem.Size = New System.Drawing.Size(239, 24)
        Me.ViewProjectDescriptionToolStripMenuItem.Text = "View project description"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(968, 316)
        Me.cmdClose.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(100, 28)
        Me.cmdClose.TabIndex = 5
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.Location = New System.Drawing.Point(744, 316)
        Me.cmdDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(100, 28)
        Me.cmdDelete.TabIndex = 6
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 322)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(516, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "*To delete project, select them and press the delete button. To edit, double clic" & _
            "k."
        '
        'cmdAdd
        '
        Me.cmdAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAdd.Location = New System.Drawing.Point(856, 316)
        Me.cmdAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(100, 28)
        Me.cmdAdd.TabIndex = 8
        Me.cmdAdd.Text = "&Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'cmdPurge
        '
        Me.cmdPurge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPurge.Location = New System.Drawing.Point(632, 316)
        Me.cmdPurge.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdPurge.Name = "cmdPurge"
        Me.cmdPurge.Size = New System.Drawing.Size(100, 28)
        Me.cmdPurge.TabIndex = 9
        Me.cmdPurge.Text = "&Purge"
        Me.cmdPurge.UseVisualStyleBackColor = True
        '
        'frmPBList
        '
        Me.AcceptButton = Me.cmdClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(1084, 350)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdPurge)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.lvProjects)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmPBList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Project list"
        Me.cMenuD.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
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
    Friend WithEvents cmDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmContact As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdDelete As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdPurge As System.Windows.Forms.Button
    Friend WithEvents cmKFactor As System.Windows.Forms.ColumnHeader
    Friend WithEvents cMenuD As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewProjectDescriptionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
