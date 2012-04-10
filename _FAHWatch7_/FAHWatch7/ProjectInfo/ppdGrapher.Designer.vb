<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ppdGrapher
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ppdGrapher))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lvProjects = New System.Windows.Forms.ListView()
        Me.Project = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Kfactor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Preferred = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Deadline = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdToggleList = New System.Windows.Forms.Button()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.cmdToggleConfigure = New System.Windows.Forms.Button()
        Me.gbDetails = New System.Windows.Forms.GroupBox()
        Me.zg = New ZedGraph.ZedGraphControl()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.15534!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.84466!))
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainer1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainer2, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.zg, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1030, 564)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvProjects)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.cmdToggleList)
        Me.SplitContainer1.Panel2MinSize = 10
        Me.TableLayoutPanel1.SetRowSpan(Me.SplitContainer1, 2)
        Me.SplitContainer1.Size = New System.Drawing.Size(284, 558)
        Me.SplitContainer1.SplitterDistance = 273
        Me.SplitContainer1.SplitterWidth = 1
        Me.SplitContainer1.TabIndex = 2
        '
        'lvProjects
        '
        Me.lvProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Project, Me.Kfactor, Me.Preferred, Me.Deadline})
        Me.lvProjects.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvProjects.Location = New System.Drawing.Point(0, 0)
        Me.lvProjects.Name = "lvProjects"
        Me.lvProjects.Size = New System.Drawing.Size(273, 558)
        Me.lvProjects.TabIndex = 1
        Me.lvProjects.UseCompatibleStateImageBehavior = False
        Me.lvProjects.View = System.Windows.Forms.View.Details
        '
        'Project
        '
        Me.Project.Text = "Project"
        Me.Project.Width = 89
        '
        'Kfactor
        '
        Me.Kfactor.Text = "KFactor"
        '
        'Preferred
        '
        Me.Preferred.Text = "Preferred"
        '
        'Deadline
        '
        Me.Deadline.Text = "Final"
        '
        'cmdToggleList
        '
        Me.cmdToggleList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdToggleList.Location = New System.Drawing.Point(0, 0)
        Me.cmdToggleList.Name = "cmdToggleList"
        Me.cmdToggleList.Size = New System.Drawing.Size(10, 558)
        Me.cmdToggleList.TabIndex = 0
        Me.cmdToggleList.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(293, 474)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.cmdToggleConfigure)
        Me.SplitContainer2.Panel1MinSize = 10
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.gbDetails)
        Me.SplitContainer2.Size = New System.Drawing.Size(734, 87)
        Me.SplitContainer2.SplitterDistance = 10
        Me.SplitContainer2.SplitterWidth = 1
        Me.SplitContainer2.TabIndex = 3
        '
        'cmdToggleConfigure
        '
        Me.cmdToggleConfigure.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdToggleConfigure.Location = New System.Drawing.Point(0, 0)
        Me.cmdToggleConfigure.Name = "cmdToggleConfigure"
        Me.cmdToggleConfigure.Size = New System.Drawing.Size(734, 10)
        Me.cmdToggleConfigure.TabIndex = 0
        Me.cmdToggleConfigure.UseVisualStyleBackColor = True
        '
        'gbDetails
        '
        Me.gbDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDetails.Location = New System.Drawing.Point(0, 0)
        Me.gbDetails.Name = "gbDetails"
        Me.gbDetails.Size = New System.Drawing.Size(734, 76)
        Me.gbDetails.TabIndex = 0
        Me.gbDetails.TabStop = False
        Me.gbDetails.Text = "Configure graphs"
        '
        'zg
        '
        Me.zg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zg.Location = New System.Drawing.Point(293, 3)
        Me.zg.Name = "zg"
        Me.zg.ScrollGrace = 0.0R
        Me.zg.ScrollMaxX = 0.0R
        Me.zg.ScrollMaxY = 0.0R
        Me.zg.ScrollMaxY2 = 0.0R
        Me.zg.ScrollMinX = 0.0R
        Me.zg.ScrollMinY = 0.0R
        Me.zg.ScrollMinY2 = 0.0R
        Me.zg.Size = New System.Drawing.Size(734, 465)
        Me.zg.TabIndex = 4
        '
        'ppdGrapher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1030, 564)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ppdGrapher"
        Me.Text = "Graphical ppd calculator"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lvProjects As System.Windows.Forms.ListView
    Friend WithEvents Project As System.Windows.Forms.ColumnHeader
    Friend WithEvents Kfactor As System.Windows.Forms.ColumnHeader
    Friend WithEvents Preferred As System.Windows.Forms.ColumnHeader
    Friend WithEvents Deadline As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdToggleList As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents cmdToggleConfigure As System.Windows.Forms.Button
    Friend WithEvents gbDetails As System.Windows.Forms.GroupBox
    Friend WithEvents zg As ZedGraph.ZedGraphControl
End Class
