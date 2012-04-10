'/*
' *  FAHWatch7 Notify form 
' *
' *  Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
'/*
Public Class Notify
    Private mMyNotification As Notifications.Notification
    Friend WriteOnly Property Notification As Notifications.Notification
        Set(value As Notifications.Notification)
            mMyNotification = value
        End Set
    End Property
    Friend Sub ActivateNotification(Optional FromTray As Boolean = False)
        Try
            If FromTray Then
                delegateFactory.ShowFormActivated(Me)
                If modMySettings.NotificationLevel = 0 Then
                    delegateFactory.SetFormTopMost(Me, False)
                Else
                    delegateFactory.SetFormTopMost(Me, True)
                End If
            Else
                If modMySettings.NotificationLevel = 0 Then
                    delegateFactory.ShowWindow(Me, NativeMethods.ShowWindowCommands.SW_SHOWNOACTIVATE)
                    delegateFactory.SetFormTopMost(Me, False)
                Else
                    delegateFactory.ShowFormActivated(Me)
                    delegateFactory.SetFormTopMost(Me, True)
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Notify_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 250, NativeMethods.AnimateWindowFlags.AW_BLEND Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub
    Private Sub Notify_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 250, NativeMethods.AnimateWindowFlags.AW_BLEND)
    End Sub
    Private Sub Notify_LostFocus(sender As Object, e As System.EventArgs) Handles Me.LostFocus
        Try
            If modMySettings.NotificationLevel = 2 Then
                WriteLog("Forcing focus to notification form: " & delegateFactory.ActivateForm(Me).ToString)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private mLastMouseLocation As Point
    Private Sub Notify_MouseLeave(sender As Object, e As System.EventArgs) Handles Me.MouseLeave
        Try
            Cursor.Position = mLastMouseLocation
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Notify_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove, cmdClose.MouseMove, Label1.MouseMove, Label2.MouseMove, Label3.MouseMove, Label4.MouseMove, lblReason.MouseMove, lblSource.MouseMove, lblTimeStamp.MouseMove, rtInfo.MouseMove
        Try
            Dim chkRect As Rectangle = New Rectangle(New Point(Me.ClientRectangle.Location.X + 2, Me.ClientRectangle.Location.Y + 2), New Size(Me.ClientRectangle.Size.Width - 4, Me.ClientRectangle.Height - 4))
            If Not chkRect.Contains(e.Location) Then
                Cursor.Position = mLastMouseLocation
            Else
                mLastMouseLocation = e.Location
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Notify_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            lblReason.Text = mMyNotification.Reason
            lblTimeStamp.Text = mMyNotification.TimeStamp
            lblSource.Text = mMyNotification.Source
            rtInfo.Text = mMyNotification.Info
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
End Class