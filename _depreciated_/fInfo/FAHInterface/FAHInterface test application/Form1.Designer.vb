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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.cmdSend = New System.Windows.Forms.Button()
        Me.txtCMD = New System.Windows.Forms.TextBox()
        Me.txtHost = New System.Windows.Forms.TextBox()
        Me.cmdConnect = New System.Windows.Forms.Button()
        Me.cmdDisconnect = New System.Windows.Forms.Button()
        Me.txtOUT = New System.Windows.Forms.RichTextBox()
        Me.tcNotes = New System.Windows.Forms.TabControl()
        Me.tcNew = New System.Windows.Forms.TabPage()
        Me.rtNote = New System.Windows.Forms.RichTextBox()
        Me.cmNotes = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.NotesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tcNotes.SuspendLayout()
        Me.tcNew.SuspendLayout()
        Me.cmNotes.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Top
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Button1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmdSend)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtCMD)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtHost)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmdConnect)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmdDisconnect)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtOUT)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.tcNotes)
        Me.SplitContainer1.Panel2.Controls.Add(Me.MenuStrip1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1304, 477)
        Me.SplitContainer1.SplitterDistance = 521
        Me.SplitContainer1.TabIndex = 7
        '
        'cmdSend
        '
        Me.cmdSend.AutoSize = True
        Me.cmdSend.Location = New System.Drawing.Point(13, 438)
        Me.cmdSend.Name = "cmdSend"
        Me.cmdSend.Size = New System.Drawing.Size(394, 32)
        Me.cmdSend.TabIndex = 12
        Me.cmdSend.Text = "Send"
        Me.cmdSend.UseVisualStyleBackColor = True
        '
        'txtCMD
        '
        Me.txtCMD.Location = New System.Drawing.Point(13, 412)
        Me.txtCMD.Name = "txtCMD"
        Me.txtCMD.Size = New System.Drawing.Size(502, 20)
        Me.txtCMD.TabIndex = 11
        '
        'txtHost
        '
        Me.txtHost.Location = New System.Drawing.Point(11, 9)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(242, 20)
        Me.txtHost.TabIndex = 9
        Me.txtHost.Text = "192.168.1.200:36330"
        '
        'cmdConnect
        '
        Me.cmdConnect.Location = New System.Drawing.Point(367, 2)
        Me.cmdConnect.Name = "cmdConnect"
        Me.cmdConnect.Size = New System.Drawing.Size(102, 32)
        Me.cmdConnect.TabIndex = 8
        Me.cmdConnect.Text = "Connect"
        Me.cmdConnect.UseVisualStyleBackColor = True
        '
        'cmdDisconnect
        '
        Me.cmdDisconnect.Enabled = False
        Me.cmdDisconnect.Location = New System.Drawing.Point(259, 3)
        Me.cmdDisconnect.Name = "cmdDisconnect"
        Me.cmdDisconnect.Size = New System.Drawing.Size(102, 32)
        Me.cmdDisconnect.TabIndex = 7
        Me.cmdDisconnect.Text = "Disconnect"
        Me.cmdDisconnect.UseVisualStyleBackColor = True
        '
        'txtOUT
        '
        Me.txtOUT.Location = New System.Drawing.Point(12, 41)
        Me.txtOUT.Name = "txtOUT"
        Me.txtOUT.ReadOnly = True
        Me.txtOUT.Size = New System.Drawing.Size(503, 365)
        Me.txtOUT.TabIndex = 10
        Me.txtOUT.Text = ""
        Me.txtOUT.WordWrap = False
        '
        'tcNotes
        '
        Me.tcNotes.Controls.Add(Me.tcNew)
        Me.tcNotes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcNotes.Location = New System.Drawing.Point(0, 24)
        Me.tcNotes.Name = "tcNotes"
        Me.tcNotes.SelectedIndex = 0
        Me.tcNotes.Size = New System.Drawing.Size(779, 453)
        Me.tcNotes.TabIndex = 1
        '
        'tcNew
        '
        Me.tcNew.Controls.Add(Me.rtNote)
        Me.tcNew.Location = New System.Drawing.Point(4, 22)
        Me.tcNew.Name = "tcNew"
        Me.tcNew.Padding = New System.Windows.Forms.Padding(3)
        Me.tcNew.Size = New System.Drawing.Size(771, 427)
        Me.tcNew.TabIndex = 0
        Me.tcNew.Text = "New"
        Me.tcNew.UseVisualStyleBackColor = True
        '
        'rtNote
        '
        Me.rtNote.ContextMenuStrip = Me.cmNotes
        Me.rtNote.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtNote.Location = New System.Drawing.Point(3, 3)
        Me.rtNote.Name = "rtNote"
        Me.rtNote.Size = New System.Drawing.Size(765, 421)
        Me.rtNote.TabIndex = 0
        Me.rtNote.Text = ""
        '
        'cmNotes
        '
        Me.cmNotes.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2})
        Me.cmNotes.Name = "cmNotes"
        Me.cmNotes.Size = New System.Drawing.Size(120, 48)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(119, 22)
        Me.ToolStripMenuItem1.Text = "Load file"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(119, 22)
        Me.ToolStripMenuItem2.Text = "Save"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NotesToolStripMenuItem, Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(779, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'NotesToolStripMenuItem
        '
        Me.NotesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.LoadToolStripMenuItem})
        Me.NotesToolStripMenuItem.Name = "NotesToolStripMenuItem"
        Me.NotesToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.NotesToolStripMenuItem.Text = "Notes"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.LoadToolStripMenuItem.Text = "Load"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Location = New System.Drawing.Point(11, 483)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(842, 324)
        Me.RichTextBox1.TabIndex = 8
        Me.RichTextBox1.Text = ""
        '
        'Button1
        '
        Me.Button1.AutoSize = True
        Me.Button1.Location = New System.Drawing.Point(425, 438)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(90, 32)
        Me.Button1.TabIndex = 13
        Me.Button1.Text = "Info"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1304, 819)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.RichTextBox1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tcNotes.ResumeLayout(False)
        Me.tcNew.ResumeLayout(False)
        Me.cmNotes.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents cmdConnect As System.Windows.Forms.Button
    Friend WithEvents cmdDisconnect As System.Windows.Forms.Button
    Friend WithEvents txtOUT As System.Windows.Forms.RichTextBox
    Friend WithEvents cmdSend As System.Windows.Forms.Button
    Friend WithEvents txtCMD As System.Windows.Forms.TextBox
    Friend WithEvents tcNotes As System.Windows.Forms.TabControl
    Friend WithEvents tcNew As System.Windows.Forms.TabPage
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents NotesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rtNote As System.Windows.Forms.RichTextBox
    Friend WithEvents cmNotes As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
