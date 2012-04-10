Public Class frmGraphSettings
    Private Sub frmGraphSettings_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, CType(NativeMethods.AnimateWindowFlags.AW_BLEND + NativeMethods.AnimateWindowFlags.AW_HIDE, NativeMethods.AnimateWindowFlags))
    End Sub
    Private Sub frmGraphSettings_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
    End Sub
    Private Sub frmGraphSettings_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Select Case modMySettings.clsGraphSettings.GrapStyle
            Case modMySettings.clsGraphSettings.GraphStyleEnum.Individual
                cmbGraphStyle.SelectedIndex = 0
            Case modMySettings.clsGraphSettings.GraphStyleEnum.Stacked
                cmbGraphStyle.SelectedIndex = 1
        End Select
        Select Case modMySettings.clsGraphSettings.maxPaneItems
            Case modMySettings.clsGraphSettings.maxPaneItemsEnum.All
                cmbPaneItems.SelectedIndex = 0
            Case modMySettings.clsGraphSettings.maxPaneItemsEnum.Five
                cmbPaneItems.SelectedIndex = 1
            Case modMySettings.clsGraphSettings.maxPaneItemsEnum.Ten
                cmbPaneItems.SelectedIndex = 2
        End Select
        pnAvgColorPpd.BackColor = modMySettings.clsGraphSettings.avgColorPpd
        pnAvgColorTpf.BackColor = modMySettings.clsGraphSettings.avgColorTpf
        pnMaxColorPpd.BackColor = modMySettings.clsGraphSettings.maxColorPpd
        pnMaxColorTpf.BackColor = modMySettings.clsGraphSettings.maxColorTpf
        pnMinColorPpd.BackColor = modMySettings.clsGraphSettings.minColorPpd
        pnMinColorTpf.BackColor = modMySettings.clsGraphSettings.minColorTpf
    End Sub
    Private Sub cmbPaneItems_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbPaneItems.SelectedIndexChanged
        If cmbPaneItems.SelectedIndex = -1 Then Exit Sub
        Select Case cmbPaneItems.SelectedIndex
            Case Is = 0
                modMySettings.clsGraphSettings.maxPaneItems = modMySettings.clsGraphSettings.maxPaneItemsEnum.All
            Case Is = 1
                modMySettings.clsGraphSettings.maxPaneItems = modMySettings.clsGraphSettings.maxPaneItemsEnum.Five
            Case Is = 2
                modMySettings.clsGraphSettings.maxPaneItems = modMySettings.clsGraphSettings.maxPaneItemsEnum.Ten
        End Select
    End Sub
    Private Sub cmbGraphStyle_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbGraphStyle.SelectedIndexChanged
        If cmbGraphStyle.SelectedIndex = -1 Then Exit Sub
        Select Case cmbGraphStyle.SelectedIndex
            Case Is = 0
                modMySettings.clsGraphSettings.GrapStyle = modMySettings.clsGraphSettings.GraphStyleEnum.Individual
            Case Is = 1
                modMySettings.clsGraphSettings.GrapStyle = modMySettings.clsGraphSettings.GraphStyleEnum.Stacked
        End Select
    End Sub
    Private Sub cmdMinColorTpf_Click(sender As System.Object, e As System.EventArgs) Handles cmdMinColorTpf.Click
        cDialog.Color = pnMinColorTpf.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnMinColorTpf.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.minColorTpf = cDialog.Color
        End If
    End Sub
    Private Sub cmdAvgColorTpf_Click(sender As System.Object, e As System.EventArgs) Handles cmdAvgColorTpf.Click
        cDialog.Color = pnAvgColorTpf.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnAvgColorTpf.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.avgColorTpf = cDialog.Color
        End If
    End Sub
    Private Sub cmdMaxColorTpf_Click(sender As System.Object, e As System.EventArgs) Handles cmdMaxColorTpf.Click
        cDialog.Color = pnMaxColorTpf.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnMaxColorTpf.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.maxColorTpf = cDialog.Color
        End If
    End Sub
    Private Sub cmdMinColorPpd_Click(sender As System.Object, e As System.EventArgs) Handles cmdMinColorPpd.Click
        cDialog.Color = pnMinColorPpd.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnMinColorPpd.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.minColorPpd = cDialog.Color
        End If
    End Sub
    Private Sub cmdAvgColorPpd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAvgColorPpd.Click
        cDialog.Color = pnAvgColorPpd.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnAvgColorPpd.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.avgColorPpd = cDialog.Color
        End If
    End Sub
    Private Sub cmdMaxColorPpd_Click(sender As System.Object, e As System.EventArgs) Handles cmdMaxColorPpd.Click
        cDialog.Color = pnMaxColorPpd.BackColor
        If cDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            pnMaxColorPpd.BackColor = cDialog.Color
            modMySettings.clsGraphSettings.maxColorPpd = cDialog.Color
        End If
    End Sub
    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
End Class