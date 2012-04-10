<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmZGRAPH
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                myWU.Dispose()
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmZGRAPH))
        Me.zgProject = New ZedGraph.ZedGraphControl()
        Me.SuspendLayout()
        '
        'zgProject
        '
        Me.zgProject.AutoSize = True
        Me.zgProject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgProject.EditButtons = System.Windows.Forms.MouseButtons.None
        Me.zgProject.Location = New System.Drawing.Point(0, 0)
        Me.zgProject.Name = "zgProject"
        Me.zgProject.ScrollGrace = 0.0R
        Me.zgProject.ScrollMaxX = 0.0R
        Me.zgProject.ScrollMaxY = 0.0R
        Me.zgProject.ScrollMaxY2 = 0.0R
        Me.zgProject.ScrollMinX = 0.0R
        Me.zgProject.ScrollMinY = 0.0R
        Me.zgProject.ScrollMinY2 = 0.0R
        Me.zgProject.Size = New System.Drawing.Size(1041, 466)
        Me.zgProject.TabIndex = 1
        '
        'frmZGRAPH
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1041, 466)
        Me.Controls.Add(Me.zgProject)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmZGRAPH"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents zgProject As ZedGraph.ZedGraphControl
End Class
