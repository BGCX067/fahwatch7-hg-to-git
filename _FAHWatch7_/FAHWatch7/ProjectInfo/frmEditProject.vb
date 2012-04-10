Imports System.Security.Permissions

'   Edit projects form
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
Friend Class frmEditProject
    Private _Project As pSummary
    Friend Property Project() As pSummary
        Get
            Return _Project
        End Get
        Set(ByVal value As pSummary)
            _Project = value
        End Set
    End Property

    Private Sub frmEditProject_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub frmEditProject_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

    End Sub
    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With _Project
            .PreferredDays = txtPrefferdDays.Text
            .FinalDeadline = txtFinalDeadline.Text
            .Credit = txtCredit.Text
            .NumberOfAtoms = txtNOatoms.Text
            .ServerIP = txtServerIP.Text
            .kFactor = txtKfactor.Text
        End With
        'If Not ProjectInfo.UpdateProject(_Project) Then
        '    MsgBox("Edit failed for some reason?")
        'End If
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmEditProject_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
    End Sub

    Private Sub frmEditProject_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        With Project
            txtKfactor.Text = .kFactor
            txtFinalDeadline.Text = .FinalDeadline
            txtCredit.Text = .Credit
            txtNOatoms.Text = .NumberOfAtoms
            txtPrefferdDays.Text = .PreferredDays
            txtServerIP.Text = .ServerIP
        End With
    End Sub
    'Const WM_NCHITTEST As Integer = &H84
    'Const HTCLIENT As Integer = &H1
    'Const HTCAPTION As Integer = &H2
    '<EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = True)> _
    'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    '    Select Case m.Msg
    '        Case WM_NCHITTEST
    '            MyBase.WndProc(m)
    '            If CInt(m.Result) = HTCLIENT Then m.Result = CType(HTCAPTION, IntPtr)
    '            'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
    '        Case Else
    '            'Make sure you pass unhandled messages back to the default message handler.
    '            MyBase.WndProc(m)
    '    End Select
    'End Sub
End Class