<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.pbOpt = New System.Windows.Forms.PictureBox()
        Me.pbSQ = New System.Windows.Forms.PictureBox()
        Me.pbOHM = New System.Windows.Forms.PictureBox()
        Me.chkPCI = New System.Windows.Forms.CheckBox()
        Me.chkStartClient = New System.Windows.Forms.CheckBox()
        Me.chkConfirmExit = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nudO = New System.Windows.Forms.NumericUpDown()
        Me.nudSQ = New System.Windows.Forms.NumericUpDown()
        Me.nudOHM = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.chkAUTORUN = New System.Windows.Forms.CheckBox()
        Me.chkEOC = New System.Windows.Forms.CheckBox()
        Me.gbEOC = New System.Windows.Forms.GroupBox()
        Me.lblEOCDT = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkEOCConfirmDelete = New System.Windows.Forms.CheckBox()
        Me.chkEOCPopup = New System.Windows.Forms.CheckBox()
        Me.chkEOCIcon = New System.Windows.Forms.CheckBox()
        Me.cmbHistoryLimit = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkMinimized = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdEUER = New System.Windows.Forms.Button()
        Me.cmdEUEA = New System.Windows.Forms.Button()
        Me.lbEUE = New System.Windows.Forms.ListBox()
        Me.chkEUE = New System.Windows.Forms.CheckBox()
        CType(Me.pbOpt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSQ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbOHM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudO, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSQ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOHM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEOC.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbOpt
        '
        Me.pbOpt.Location = New System.Drawing.Point(279, 124)
        Me.pbOpt.Name = "pbOpt"
        Me.pbOpt.Size = New System.Drawing.Size(16, 16)
        Me.pbOpt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbOpt.TabIndex = 39
        Me.pbOpt.TabStop = False
        '
        'pbSQ
        '
        Me.pbSQ.Location = New System.Drawing.Point(279, 100)
        Me.pbSQ.Name = "pbSQ"
        Me.pbSQ.Size = New System.Drawing.Size(16, 16)
        Me.pbSQ.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbSQ.TabIndex = 38
        Me.pbSQ.TabStop = False
        '
        'pbOHM
        '
        Me.pbOHM.Location = New System.Drawing.Point(279, 76)
        Me.pbOHM.Name = "pbOHM"
        Me.pbOHM.Size = New System.Drawing.Size(16, 16)
        Me.pbOHM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbOHM.TabIndex = 37
        Me.pbOHM.TabStop = False
        '
        'chkPCI
        '
        Me.chkPCI.AutoSize = True
        Me.chkPCI.BackColor = System.Drawing.Color.Transparent
        Me.chkPCI.Location = New System.Drawing.Point(148, 27)
        Me.chkPCI.Name = "chkPCI"
        Me.chkPCI.Size = New System.Drawing.Size(147, 17)
        Me.chkPCI.TabIndex = 7
        Me.chkPCI.Text = "Get pciID from FAHClient "
        Me.chkPCI.UseVisualStyleBackColor = False
        '
        'chkStartClient
        '
        Me.chkStartClient.AutoSize = True
        Me.chkStartClient.BackColor = System.Drawing.Color.Transparent
        Me.chkStartClient.Location = New System.Drawing.Point(148, 50)
        Me.chkStartClient.Name = "chkStartClient"
        Me.chkStartClient.Size = New System.Drawing.Size(98, 17)
        Me.chkStartClient.TabIndex = 4
        Me.chkStartClient.Text = "Start FAHClient"
        Me.chkStartClient.UseVisualStyleBackColor = False
        '
        'chkConfirmExit
        '
        Me.chkConfirmExit.AutoSize = True
        Me.chkConfirmExit.BackColor = System.Drawing.Color.Transparent
        Me.chkConfirmExit.Location = New System.Drawing.Point(408, 241)
        Me.chkConfirmExit.Name = "chkConfirmExit"
        Me.chkConfirmExit.Size = New System.Drawing.Size(80, 17)
        Me.chkConfirmExit.TabIndex = 5
        Me.chkConfirmExit.Text = "Confirm exit"
        Me.chkConfirmExit.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(237, 126)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 13)
        Me.Label8.TabIndex = 33
        Me.Label8.Text = "ms"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(237, 102)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(20, 13)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "ms"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(236, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 13)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "ms"
        '
        'nudO
        '
        Me.nudO.Increment = New Decimal(New Integer() {2500, 0, 0, 0})
        Me.nudO.Location = New System.Drawing.Point(152, 122)
        Me.nudO.Maximum = New Decimal(New Integer() {30000, 0, 0, 0})
        Me.nudO.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudO.Name = "nudO"
        Me.nudO.Size = New System.Drawing.Size(79, 20)
        Me.nudO.TabIndex = 3
        Me.nudO.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudSQ
        '
        Me.nudSQ.Increment = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudSQ.Location = New System.Drawing.Point(152, 98)
        Me.nudSQ.Maximum = New Decimal(New Integer() {30000, 0, 0, 0})
        Me.nudSQ.Minimum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudSQ.Name = "nudSQ"
        Me.nudSQ.Size = New System.Drawing.Size(79, 20)
        Me.nudSQ.TabIndex = 2
        Me.nudSQ.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudOHM
        '
        Me.nudOHM.Increment = New Decimal(New Integer() {250, 0, 0, 0})
        Me.nudOHM.Location = New System.Drawing.Point(152, 74)
        Me.nudOHM.Maximum = New Decimal(New Integer() {30000, 0, 0, 0})
        Me.nudOHM.Minimum = New Decimal(New Integer() {250, 0, 0, 0})
        Me.nudOHM.Name = "nudOHM"
        Me.nudOHM.Size = New System.Drawing.Size(79, 20)
        Me.nudOHM.TabIndex = 1
        Me.nudOHM.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(21, 126)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "Client options interval"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(21, 102)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(124, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Client slot/queue interval"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(21, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 13)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Hardware sensor interval"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(418, 278)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(70, 23)
        Me.cmdSave.TabIndex = 24
        Me.cmdSave.Text = "&Save"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'chkAUTORUN
        '
        Me.chkAUTORUN.AutoSize = True
        Me.chkAUTORUN.BackColor = System.Drawing.Color.Transparent
        Me.chkAUTORUN.Location = New System.Drawing.Point(21, 27)
        Me.chkAUTORUN.Name = "chkAUTORUN"
        Me.chkAUTORUN.Size = New System.Drawing.Size(114, 17)
        Me.chkAUTORUN.TabIndex = 6
        Me.chkAUTORUN.Text = "Start with windows"
        Me.chkAUTORUN.UseVisualStyleBackColor = False
        '
        'chkEOC
        '
        Me.chkEOC.AutoSize = True
        Me.chkEOC.BackColor = System.Drawing.Color.Transparent
        Me.chkEOC.Location = New System.Drawing.Point(21, 151)
        Me.chkEOC.Name = "chkEOC"
        Me.chkEOC.Size = New System.Drawing.Size(112, 17)
        Me.chkEOC.TabIndex = 8
        Me.chkEOC.Text = "Use EOC xml feed"
        Me.chkEOC.UseVisualStyleBackColor = False
        '
        'gbEOC
        '
        Me.gbEOC.BackColor = System.Drawing.Color.Transparent
        Me.gbEOC.Controls.Add(Me.lblEOCDT)
        Me.gbEOC.Controls.Add(Me.Label2)
        Me.gbEOC.Controls.Add(Me.chkEOCConfirmDelete)
        Me.gbEOC.Controls.Add(Me.chkEOCPopup)
        Me.gbEOC.Controls.Add(Me.chkEOCIcon)
        Me.gbEOC.Location = New System.Drawing.Point(21, 174)
        Me.gbEOC.Name = "gbEOC"
        Me.gbEOC.Size = New System.Drawing.Size(467, 61)
        Me.gbEOC.TabIndex = 41
        Me.gbEOC.TabStop = False
        Me.gbEOC.Text = "EOC options"
        '
        'lblEOCDT
        '
        Me.lblEOCDT.AutoSize = True
        Me.lblEOCDT.Location = New System.Drawing.Point(107, 16)
        Me.lblEOCDT.Name = "lblEOCDT"
        Me.lblEOCDT.Size = New System.Drawing.Size(83, 13)
        Me.lblEOCDT.TabIndex = 7
        Me.lblEOCDT.Text = "Not updated yet"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Last time updated:"
        '
        'chkEOCConfirmDelete
        '
        Me.chkEOCConfirmDelete.AutoSize = True
        Me.chkEOCConfirmDelete.Location = New System.Drawing.Point(10, 38)
        Me.chkEOCConfirmDelete.Name = "chkEOCConfirmDelete"
        Me.chkEOCConfirmDelete.Size = New System.Drawing.Size(190, 17)
        Me.chkEOCConfirmDelete.TabIndex = 5
        Me.chkEOCConfirmDelete.Text = "Confirm before deleting saved data"
        Me.chkEOCConfirmDelete.UseVisualStyleBackColor = True
        '
        'chkEOCPopup
        '
        Me.chkEOCPopup.AutoSize = True
        Me.chkEOCPopup.Location = New System.Drawing.Point(258, 38)
        Me.chkEOCPopup.Name = "chkEOCPopup"
        Me.chkEOCPopup.Size = New System.Drawing.Size(165, 17)
        Me.chkEOCPopup.TabIndex = 4
        Me.chkEOCPopup.Text = "Pop up new signature images"
        Me.chkEOCPopup.UseVisualStyleBackColor = True
        '
        'chkEOCIcon
        '
        Me.chkEOCIcon.AutoSize = True
        Me.chkEOCIcon.Location = New System.Drawing.Point(258, 15)
        Me.chkEOCIcon.Name = "chkEOCIcon"
        Me.chkEOCIcon.Size = New System.Drawing.Size(132, 17)
        Me.chkEOCIcon.TabIndex = 3
        Me.chkEOCIcon.Text = "Use seperate tray icon"
        Me.chkEOCIcon.UseVisualStyleBackColor = True
        '
        'cmbHistoryLimit
        '
        Me.cmbHistoryLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbHistoryLimit.FormattingEnabled = True
        Me.cmbHistoryLimit.Items.AddRange(New Object() {"Minimal", "OneDay", "OneWeek", "OneMonth", "None"})
        Me.cmbHistoryLimit.Location = New System.Drawing.Point(98, 238)
        Me.cmbHistoryLimit.Name = "cmbHistoryLimit"
        Me.cmbHistoryLimit.Size = New System.Drawing.Size(114, 21)
        Me.cmbHistoryLimit.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(21, 241)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Historical limit"
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(21, 279)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(70, 23)
        Me.cmdCancel.TabIndex = 42
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'cmdHelp
        '
        Me.cmdHelp.Location = New System.Drawing.Point(222, 279)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(70, 22)
        Me.cmdHelp.TabIndex = 43
        Me.cmdHelp.Text = "&Help"
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(230, 242)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(162, 13)
        Me.Label9.TabIndex = 44
        Me.Label9.Text = "* Applies to EOC xml feed as well"
        '
        'chkMinimized
        '
        Me.chkMinimized.AutoSize = True
        Me.chkMinimized.BackColor = System.Drawing.Color.Transparent
        Me.chkMinimized.Location = New System.Drawing.Point(21, 50)
        Me.chkMinimized.Name = "chkMinimized"
        Me.chkMinimized.Size = New System.Drawing.Size(96, 17)
        Me.chkMinimized.TabIndex = 45
        Me.chkMinimized.Text = "Start minimized"
        Me.chkMinimized.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Info
        Me.Panel1.Controls.Add(Me.cmdEUER)
        Me.Panel1.Controls.Add(Me.cmdEUEA)
        Me.Panel1.Controls.Add(Me.lbEUE)
        Me.Panel1.Controls.Add(Me.chkEUE)
        Me.Panel1.Location = New System.Drawing.Point(301, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(187, 141)
        Me.Panel1.TabIndex = 46
        '
        'cmdEUER
        '
        Me.cmdEUER.Location = New System.Drawing.Point(4, 112)
        Me.cmdEUER.Name = "cmdEUER"
        Me.cmdEUER.Size = New System.Drawing.Size(75, 23)
        Me.cmdEUER.TabIndex = 3
        Me.cmdEUER.Text = "&Remove"
        Me.cmdEUER.UseVisualStyleBackColor = True
        '
        'cmdEUEA
        '
        Me.cmdEUEA.Location = New System.Drawing.Point(105, 113)
        Me.cmdEUEA.Name = "cmdEUEA"
        Me.cmdEUEA.Size = New System.Drawing.Size(75, 23)
        Me.cmdEUEA.TabIndex = 2
        Me.cmdEUEA.Text = "&Add"
        Me.cmdEUEA.UseVisualStyleBackColor = True
        '
        'lbEUE
        '
        Me.lbEUE.FormattingEnabled = True
        Me.lbEUE.Location = New System.Drawing.Point(6, 29)
        Me.lbEUE.Name = "lbEUE"
        Me.lbEUE.Size = New System.Drawing.Size(174, 82)
        Me.lbEUE.TabIndex = 1
        '
        'chkEUE
        '
        Me.chkEUE.AutoSize = True
        Me.chkEUE.Location = New System.Drawing.Point(6, 5)
        Me.chkEUE.Name = "chkEUE"
        Me.chkEUE.Size = New System.Drawing.Size(174, 17)
        Me.chkEUE.TabIndex = 0
        Me.chkEUE.Text = "Track Early Unit End messages"
        Me.chkEUE.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.fInfo.My.Resources.Resources.Settings
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(515, 315)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.chkMinimized)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.cmdHelp)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.gbEOC)
        Me.Controls.Add(Me.chkEOC)
        Me.Controls.Add(Me.pbOpt)
        Me.Controls.Add(Me.cmbHistoryLimit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pbSQ)
        Me.Controls.Add(Me.pbOHM)
        Me.Controls.Add(Me.chkPCI)
        Me.Controls.Add(Me.chkStartClient)
        Me.Controls.Add(Me.chkConfirmExit)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.nudO)
        Me.Controls.Add(Me.nudSQ)
        Me.Controls.Add(Me.nudOHM)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.chkAUTORUN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmSettings"
        Me.Text = "Settings"
        CType(Me.pbOpt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSQ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbOHM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudO, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSQ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOHM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEOC.ResumeLayout(False)
        Me.gbEOC.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbOpt As System.Windows.Forms.PictureBox
    Friend WithEvents pbSQ As System.Windows.Forms.PictureBox
    Friend WithEvents pbOHM As System.Windows.Forms.PictureBox
    Friend WithEvents chkPCI As System.Windows.Forms.CheckBox
    Friend WithEvents chkStartClient As System.Windows.Forms.CheckBox
    Friend WithEvents chkConfirmExit As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents nudO As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSQ As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudOHM As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents chkAUTORUN As System.Windows.Forms.CheckBox
    Friend WithEvents chkEOC As System.Windows.Forms.CheckBox
    Friend WithEvents gbEOC As System.Windows.Forms.GroupBox
    Friend WithEvents cmbHistoryLimit As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents chkEOCPopup As System.Windows.Forms.CheckBox
    Friend WithEvents chkEOCIcon As System.Windows.Forms.CheckBox
    Friend WithEvents chkEOCConfirmDelete As System.Windows.Forms.CheckBox
    Friend WithEvents cmdHelp As System.Windows.Forms.Button
    Friend WithEvents lblEOCDT As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkMinimized As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmdEUER As System.Windows.Forms.Button
    Friend WithEvents cmdEUEA As System.Windows.Forms.Button
    Friend WithEvents lbEUE As System.Windows.Forms.ListBox
    Friend WithEvents chkEUE As System.Windows.Forms.CheckBox
End Class
