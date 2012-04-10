Imports ProjectInfo

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
Public Class frmEditProject
    Private _Project As clsProjectInfo.sProject.clsProject
    Public Property Project() As clsProjectInfo.sProject.clsProject
        Get
            Return _Project
        End Get
        Set(ByVal value As clsProjectInfo.sProject.clsProject)
            _Project = value
        End Set
    End Property
    Private Sub frmEditProject_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Alt And e.KeyCode = Keys.S Then

        ElseIf e.Alt And e.KeyCode = Keys.C Then
            Me.Close()
        End If
    End Sub
    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
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

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
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
End Class