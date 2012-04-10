Imports System.Drawing.Imaging

'/*
' * fInfo frmControl class Copyright Marvin Westmaas ( mtm ) 
' *
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
Public Class frmControl
    Private bAllowClose As Boolean = False
    Public Enum eDisableButtons
        DisableMin
        DisableClose
        DisableHelp
        DisableMin_Close
        DisableMin_Help
        DisableHelp_Close
        DisableAll
        DisableNone
    End Enum
    Public Property ButtonDisabled As eDisableButtons = eDisableButtons.DisableNone

    Public ReadOnly Property AllowClose As Boolean
        Get
            Return bAllowClose
        End Get
    End Property
    Public Sub CloseForm()
        bAllowClose = True
        Me.Close()
    End Sub
    Private Sub frmControl_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not bAllowClose Then
            e.Cancel = True
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
        End If
    End Sub

    Private Sub frmControl_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Size = Me.BackgroundImage.Size
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size
        Me.SetStyle(ControlStyles.UserPaint, True)
        'Handle image loading/placement here
        lblClose.Visible = False : lblHelp.Visible = False : lblMin.Visible = False
        If Not ButtonDisabled = eDisableButtons.DisableMin_Close And Not ButtonDisabled = eDisableButtons.DisableHelp_Close And Not ButtonDisabled = eDisableButtons.DisableClose Then
            ' Enable close button
            lblClose.Size = My.Resources.Close_button.Size : lblClose.Image = My.Resources.Close_button
            lblClose.Location = New Point(735, 8)
            lblClose.Visible = True
        ElseIf Not ButtonDisabled = eDisableButtons.DisableHelp_Close And Not ButtonDisabled = eDisableButtons.DisableMin_Help And Not ButtonDisabled = eDisableButtons.DisableHelp Then
            ' Enable help button
            lblHelp.Size = My.Resources.Help_button.Size : lblHelp.Image = My.Resources.Help_button
            lblHelp.Location = New Point(615, 8)
            lblHelp.Visible = True
        ElseIf Not ButtonDisabled = eDisableButtons.DisableMin And Not ButtonDisabled = eDisableButtons.DisableMin_Close And Not ButtonDisabled = eDisableButtons.DisableMin_Help Then
            ' Enable min button 
            lblMin.Size = My.Resources.Min_button.Size : lblMin.Image = My.Resources.Min_button
            lblMin.Location = New Point(675, 8)
            lblHelp.Visible = True
        End If
    End Sub

    Private Sub frmControl_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then Me.Visible = False
    End Sub
    Public Enum eWizzard
        ConfirmCancelWizzard
        ConfirmExitForm
        NoConfirmExitForm
        ConfirmExitApplication
        NoConfirmExitApplication
    End Enum
    Public Property CloseOption As eWizzard = eWizzard.NoConfirmExitApplication


    Private Sub frmUI_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Size = Me.BackgroundImage.Size
            Me.MaximumSize = Me.Size
            Me.MaximizeBox = False
            Me.SetStyle(ControlStyles.UserPaint, True)
            'Handle image loading/placement here
            Dim imClose As Image = My.Resources.Close_button
            Dim imMin As Image = My.Resources.Min_button
            Dim imHelp As Image = My.Resources.Help_button
            lblClose.Size = imClose.Size : lblClose.Image = imClose
            lblMin.Size = imMin.Size : lblMin.Image = imMin
            lblHelp.Size = imHelp.Size : lblHelp.Image = imHelp
            lblHelp.Location = New Point(615, 8)
            lblMin.Location = New Point(675, 8)
            lblClose.Location = New Point(735, 8)
        Catch ex As Exception

        End Try
    End Sub

#Region " ClientAreaMove Handling and UI layout "
    Private Sub lblHelp_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblHelp.MouseEnter
        lblHelp.Image = My.Resources.Help_button_mouse_over
    End Sub
    Private Sub lblHelp_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblHelp.MouseLeave
        lblHelp.Image = My.Resources.Help_button
    End Sub
    Private Sub lblHelp_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblHelp.MouseDown
        lblHelp.Image = My.Resources.Help_button_mouse_down
    End Sub
    Private Sub lblHelp_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblHelp.MouseUp
        lblHelp.Image = My.Resources.Help_button_mouse_over
    End Sub

    Private Sub lblMin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.Click
        Me.Visible = False
    End Sub

    Private Sub lblMin_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.MouseEnter
        lblMin.Image = My.Resources.Min_button_mouse_over
    End Sub
    Private Sub lblMin_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblMin.MouseLeave
        lblMin.Image = My.Resources.Min_button
    End Sub
    Private Sub lblMin_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblMin.MouseDown
        lblMin.Image = My.Resources.Min_button_mouse_down
    End Sub
    Private Sub lblMin_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblMin.MouseUp
        lblMin.Image = My.Resources.Min_button_mouse_over
    End Sub

    Private Sub lblClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.Click
        Try
            ExitApplication()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lblClose_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.MouseEnter
        lblClose.Image = My.Resources.Close_button_mouse_over
    End Sub
    Private Sub lblClose_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblClose.MouseLeave
        lblClose.Image = My.Resources.Close_button
    End Sub
    Private Sub lblClose_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblClose.MouseDown
        lblClose.Image = My.Resources.Close_button_mouse_down
    End Sub
    Private Sub lblClose_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblClose.MouseUp
        lblClose.Image = My.Resources.Close_button_mouse_over
    End Sub
    Protected Sub lblMin_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblMin.Paint
        Try
            Dim bmp As Bitmap = lblMin.Image
            Dim g As Graphics = Graphics.FromImage(bmp)
            Dim attr As New ImageAttributes
            g.DrawImage(lblMin.Image, New Point(0, 0))
            attr.SetColorKey(bmp.GetPixel(0, 0), bmp.GetPixel(0, 0))
            Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, _
                GraphicsUnit.Pixel, attr)

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lblHelp_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblHelp.Paint
        Try
            Dim bmp As Bitmap = lblHelp.Image
            Dim g As Graphics = Graphics.FromImage(bmp)
            Dim Attr As New ImageAttributes
            Attr.SetColorKey(Color.White, Color.White)
            Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, Attr)

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lblClose_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblClose.Paint
        Dim bmp As Bitmap = lblClose.Image
        Dim g As Graphics = Graphics.FromImage(bmp)
        Dim Attr As New ImageAttributes
        Attr.SetColorKey(Color.White, Color.White)
        Dim dstRect As New Rectangle(0, 0, bmp.Width, bmp.Height)
        e.Graphics.DrawImage(bmp, dstRect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, Attr)

    End Sub
    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
                'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
            Case Else
                'Make sure you pass unhandled messages back to the default message handler.
                MyBase.WndProc(m)
        End Select
    End Sub
#End Region

End Class

