<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTools
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblWhat = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.llblMemtestCL = New System.Windows.Forms.LinkLabel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.llblMemtestG80 = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.llblStressCpuV2 = New System.Windows.Forms.LinkLabel()
        Me.chkStressCpuV2 = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkMemtestCL = New System.Windows.Forms.CheckBox()
        Me.chkMemtestG80 = New System.Windows.Forms.CheckBox()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.tCheck = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LinkLabel1)
        Me.GroupBox1.Controls.Add(Me.lblWhat)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.llblMemtestCL)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.llblMemtestG80)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.llblStressCpuV2)
        Me.GroupBox1.Controls.Add(Me.chkStressCpuV2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.chkMemtestCL)
        Me.GroupBox1.Controls.Add(Me.chkMemtestG80)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(464, 364)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'lblWhat
        '
        Me.lblWhat.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblWhat.Location = New System.Drawing.Point(18, 210)
        Me.lblWhat.Name = "lblWhat"
        Me.lblWhat.Size = New System.Drawing.Size(431, 94)
        Me.lblWhat.TabIndex = 9
        Me.lblWhat.Text = "Hover over a tool's download link to view a short description."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(105, 344)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(344, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "* Checked tools are already detected and don't need to be downloaded"
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Location = New System.Drawing.Point(18, 187)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(431, 15)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "C:\Program Files (x86)\Marvin Westmaas\FAHWatch7\memtestG80"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'llblMemtestCL
        '
        Me.llblMemtestCL.AutoSize = True
        Me.llblMemtestCL.Location = New System.Drawing.Point(338, 314)
        Me.llblMemtestCL.Name = "llblMemtestCL"
        Me.llblMemtestCL.Size = New System.Drawing.Size(110, 13)
        Me.llblMemtestCL.TabIndex = 7
        Me.llblMemtestCL.TabStop = True
        Me.llblMemtestCL.Text = "Download memtestCL"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(18, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(431, 31)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Extract each archive to a folder with the name of the tool you downloaded, locate" & _
    "d in the FAHWatch7 directory.  If you used the installer, an example would be:"
        '
        'llblMemtestG80
        '
        Me.llblMemtestG80.AutoSize = True
        Me.llblMemtestG80.Location = New System.Drawing.Point(186, 314)
        Me.llblMemtestG80.Name = "llblMemtestG80"
        Me.llblMemtestG80.Size = New System.Drawing.Size(117, 13)
        Me.llblMemtestG80.TabIndex = 6
        Me.llblMemtestG80.TabStop = True
        Me.llblMemtestG80.Text = "Download memtestG80"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(18, 69)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(431, 31)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "When you select 'download', the page hosting these tools will be shown in your de" & _
    "fault internet browser. "
        '
        'llblStressCpuV2
        '
        Me.llblStressCpuV2.AutoSize = True
        Me.llblStressCpuV2.Location = New System.Drawing.Point(38, 314)
        Me.llblStressCpuV2.Name = "llblStressCpuV2"
        Me.llblStressCpuV2.Size = New System.Drawing.Size(119, 13)
        Me.llblStressCpuV2.TabIndex = 5
        Me.llblStressCpuV2.TabStop = True
        Me.llblStressCpuV2.Text = "Download StressCpuV2"
        '
        'chkStressCpuV2
        '
        Me.chkStressCpuV2.AutoSize = True
        Me.chkStressCpuV2.Enabled = False
        Me.chkStressCpuV2.Location = New System.Drawing.Point(17, 314)
        Me.chkStressCpuV2.Name = "chkStressCpuV2"
        Me.chkStressCpuV2.Size = New System.Drawing.Size(15, 14)
        Me.chkStressCpuV2.TabIndex = 2
        Me.chkStressCpuV2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(18, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(431, 41)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "To enable the external tools in this menu, you first need to download them. Depen" & _
    "ding on the hardware and prequisites like gpgpu capable drivers, there is a list" & _
    " below showing the usable tools."
        '
        'chkMemtestCL
        '
        Me.chkMemtestCL.AutoSize = True
        Me.chkMemtestCL.Enabled = False
        Me.chkMemtestCL.Location = New System.Drawing.Point(317, 314)
        Me.chkMemtestCL.Name = "chkMemtestCL"
        Me.chkMemtestCL.Size = New System.Drawing.Size(15, 14)
        Me.chkMemtestCL.TabIndex = 4
        Me.chkMemtestCL.UseVisualStyleBackColor = True
        '
        'chkMemtestG80
        '
        Me.chkMemtestG80.AutoSize = True
        Me.chkMemtestG80.Enabled = False
        Me.chkMemtestG80.Location = New System.Drawing.Point(165, 313)
        Me.chkMemtestG80.Name = "chkMemtestG80"
        Me.chkMemtestG80.Size = New System.Drawing.Size(15, 14)
        Me.chkMemtestG80.TabIndex = 3
        Me.chkMemtestG80.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.Location = New System.Drawing.Point(395, 376)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(75, 23)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "&Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.LinkArea = New System.Windows.Forms.LinkArea(66, 5)
        Me.LinkLabel1.Location = New System.Drawing.Point(18, 108)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(431, 32)
        Me.LinkLabel1.TabIndex = 10
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Each download is an archive, tgz for StressCPUV2 ( you might need 7-zip or someth" & _
    "ing simular for this ) and zip for the other two."
        Me.LinkLabel1.UseCompatibleTextRendering = True
        '
        'tCheck
        '
        Me.tCheck.Enabled = True
        Me.tCheck.Interval = 1000
        '
        'frmTools
        '
        Me.AcceptButton = Me.cmdClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.cmdClose
        Me.ClientSize = New System.Drawing.Size(476, 405)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTools"
        Me.Text = "Tools"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblWhat As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents llblMemtestCL As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents llblMemtestG80 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents llblStressCpuV2 As System.Windows.Forms.LinkLabel
    Friend WithEvents chkStressCpuV2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkMemtestCL As System.Windows.Forms.CheckBox
    Friend WithEvents chkMemtestG80 As System.Windows.Forms.CheckBox
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents tCheck As System.Windows.Forms.Timer
End Class
