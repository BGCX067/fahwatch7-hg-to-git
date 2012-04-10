'/*
' * fInfo clsControl class Copyright Marvin Westmaas ( mtm ) 
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
Namespace fInfo
    Public Class clsControl
        Private WithEvents _frmControl As New frmControl
        Public ReadOnly Property Form As frmControl
            Get
                Return _frmControl
            End Get
        End Property
        Public Sub ShowForm()
            _frmControl.ButtonDisabled = frmControl.eDisableButtons.DisableNone
            _frmControl.Show()
            _frmControl.Focus()
        End Sub
        Public ReadOnly Property IsFormActive As Boolean
            Get
                Return _frmControl.Visible
            End Get
        End Property
        Public Sub HideForm()
            _frmControl.Visible = False
        End Sub
        Public Sub Close()
            LogWindow.WriteLog("Closing control class")
            _frmControl.CloseForm()
        End Sub
    End Class

End Namespace
