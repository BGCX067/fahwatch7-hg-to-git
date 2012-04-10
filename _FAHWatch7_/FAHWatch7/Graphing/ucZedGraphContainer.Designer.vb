<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucZedGraphContainer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.tlpButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.cmdDownMajor = New System.Windows.Forms.Button()
        Me.cmdUpMajor = New System.Windows.Forms.Button()
        Me.cmdUpMinor = New System.Windows.Forms.Button()
        Me.cmdDownMinor = New System.Windows.Forms.Button()
        Me.zedGraph = New ZedGraph.ZedGraphControl()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.tlpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.tlpButtons)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.zedGraph)
        Me.scMain.Size = New System.Drawing.Size(772, 496)
        Me.scMain.SplitterDistance = 31
        Me.scMain.TabIndex = 0
        '
        'tlpButtons
        '
        Me.tlpButtons.ColumnCount = 1
        Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpButtons.Controls.Add(Me.cmdDownMajor, 0, 3)
        Me.tlpButtons.Controls.Add(Me.cmdUpMajor, 0, 0)
        Me.tlpButtons.Controls.Add(Me.cmdUpMinor, 0, 1)
        Me.tlpButtons.Controls.Add(Me.cmdDownMinor, 0, 2)
        Me.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpButtons.Location = New System.Drawing.Point(0, 0)
        Me.tlpButtons.Name = "tlpButtons"
        Me.tlpButtons.RowCount = 4
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tlpButtons.Size = New System.Drawing.Size(31, 496)
        Me.tlpButtons.TabIndex = 0
        '
        'cmdDownMajor
        '
        Me.cmdDownMajor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdDownMajor.Location = New System.Drawing.Point(3, 349)
        Me.cmdDownMajor.Name = "cmdDownMajor"
        Me.cmdDownMajor.Size = New System.Drawing.Size(25, 144)
        Me.cmdDownMajor.TabIndex = 2
        Me.cmdDownMajor.UseVisualStyleBackColor = True
        '
        'cmdUpMajor
        '
        Me.cmdUpMajor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdUpMajor.Location = New System.Drawing.Point(3, 3)
        Me.cmdUpMajor.Name = "cmdUpMajor"
        Me.cmdUpMajor.Size = New System.Drawing.Size(25, 142)
        Me.cmdUpMajor.TabIndex = 0
        Me.cmdUpMajor.UseVisualStyleBackColor = True
        '
        'cmdUpMinor
        '
        Me.cmdUpMinor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdUpMinor.Location = New System.Drawing.Point(3, 151)
        Me.cmdUpMinor.Name = "cmdUpMinor"
        Me.cmdUpMinor.Size = New System.Drawing.Size(25, 93)
        Me.cmdUpMinor.TabIndex = 3
        Me.cmdUpMinor.UseVisualStyleBackColor = True
        '
        'cmdDownMinor
        '
        Me.cmdDownMinor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmdDownMinor.Location = New System.Drawing.Point(3, 250)
        Me.cmdDownMinor.Name = "cmdDownMinor"
        Me.cmdDownMinor.Size = New System.Drawing.Size(25, 93)
        Me.cmdDownMinor.TabIndex = 4
        Me.cmdDownMinor.UseVisualStyleBackColor = True
        '
        'zedGraph
        '
        Me.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zedGraph.Location = New System.Drawing.Point(0, 0)
        Me.zedGraph.Name = "zedGraph"
        Me.zedGraph.ScrollGrace = 0.0R
        Me.zedGraph.ScrollMaxX = 0.0R
        Me.zedGraph.ScrollMaxY = 0.0R
        Me.zedGraph.ScrollMaxY2 = 0.0R
        Me.zedGraph.ScrollMinX = 0.0R
        Me.zedGraph.ScrollMinY = 0.0R
        Me.zedGraph.ScrollMinY2 = 0.0R
        Me.zedGraph.Size = New System.Drawing.Size(737, 496)
        Me.zedGraph.TabIndex = 0
        '
        'ucZedGraphContainer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.scMain)
        Me.Name = "ucZedGraphContainer"
        Me.Size = New System.Drawing.Size(772, 496)
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.ResumeLayout(False)
        Me.tlpButtons.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents tlpButtons As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmdUpMajor As System.Windows.Forms.Button
    Friend WithEvents cmdDownMajor As System.Windows.Forms.Button
    Friend WithEvents cmdUpMinor As System.Windows.Forms.Button
    Friend WithEvents cmdDownMinor As System.Windows.Forms.Button
    Friend WithEvents zedGraph As ZedGraph.ZedGraphControl

End Class
