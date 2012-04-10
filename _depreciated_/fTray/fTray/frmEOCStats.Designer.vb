<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEOCXML
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEOCXML))
        Me.sCont = New System.Windows.Forms.SplitContainer()
        Me.zUser = New ZedGraph.ZedGraphControl()
        Me.zTeam = New ZedGraph.ZedGraphControl()
        Me.sCont.Panel1.SuspendLayout()
        Me.sCont.Panel2.SuspendLayout()
        Me.sCont.SuspendLayout()
        Me.SuspendLayout()
        '
        'sCont
        '
        Me.sCont.Dock = System.Windows.Forms.DockStyle.Fill
        Me.sCont.IsSplitterFixed = True
        Me.sCont.Location = New System.Drawing.Point(0, 0)
        Me.sCont.Name = "sCont"
        Me.sCont.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'sCont.Panel1
        '
        Me.sCont.Panel1.Controls.Add(Me.zUser)
        '
        'sCont.Panel2
        '
        Me.sCont.Panel2.Controls.Add(Me.zTeam)
        Me.sCont.Size = New System.Drawing.Size(964, 596)
        Me.sCont.SplitterDistance = 279
        Me.sCont.TabIndex = 1
        '
        'zUser
        '
        Me.zUser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zUser.EditButtons = System.Windows.Forms.MouseButtons.None
        Me.zUser.Location = New System.Drawing.Point(0, 0)
        Me.zUser.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.zUser.Name = "zUser"
        Me.zUser.ScrollGrace = 0.0R
        Me.zUser.ScrollMaxX = 0.0R
        Me.zUser.ScrollMaxY = 0.0R
        Me.zUser.ScrollMaxY2 = 0.0R
        Me.zUser.ScrollMinX = 0.0R
        Me.zUser.ScrollMinY = 0.0R
        Me.zUser.ScrollMinY2 = 0.0R
        Me.zUser.Size = New System.Drawing.Size(964, 279)
        Me.zUser.TabIndex = 0
        '
        'zTeam
        '
        Me.zTeam.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zTeam.Location = New System.Drawing.Point(0, 0)
        Me.zTeam.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.zTeam.Name = "zTeam"
        Me.zTeam.ScrollGrace = 0.0R
        Me.zTeam.ScrollMaxX = 0.0R
        Me.zTeam.ScrollMaxY = 0.0R
        Me.zTeam.ScrollMaxY2 = 0.0R
        Me.zTeam.ScrollMinX = 0.0R
        Me.zTeam.ScrollMinY = 0.0R
        Me.zTeam.ScrollMinY2 = 0.0R
        Me.zTeam.Size = New System.Drawing.Size(964, 313)
        Me.zTeam.TabIndex = 0
        '
        'frmEOCXML
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(964, 596)
        Me.Controls.Add(Me.sCont)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmEOCXML"
        Me.Text = "EOC XML STATS"
        Me.sCont.Panel1.ResumeLayout(False)
        Me.sCont.Panel2.ResumeLayout(False)
        Me.sCont.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sCont As System.Windows.Forms.SplitContainer
    Friend WithEvents zUser As ZedGraph.ZedGraphControl
    Friend WithEvents zTeam As ZedGraph.ZedGraphControl
End Class
