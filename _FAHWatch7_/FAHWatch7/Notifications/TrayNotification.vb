'   FAHWatch7 TrayNotification
'
'   Copyright (c) 2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
'
Public Class TrayNotification
    Implements IDisposable
#Region "Icon handling"
    Private nIcon As New NotifyIcon
#Region "Animate icon"
    Private tAnimateIcon As Threading.Timer
    Private Sub tIcon_Tick(state As Object)
        Try
            If ReferenceEquals(nIcon.Icon, My.Resources.iTray) Then
                nIcon.Icon = My.Resources.iWarning
            Else
                nIcon.Icon = My.Resources.iTray
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Balloon tip"
    Private tBalloonTip As Threading.Timer
    Private Sub tBalloonTip_tick(state As Object)
        Try
            nIcon.ShowBalloonTip(30000)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#End Region
    Private mMyNotification As Notifications.Notification
    Friend Event NotificationClosed(sender As Object, e As EventArgs)
    Friend WriteOnly Property Notification As Notifications.Notification
        Set(value As Notifications.Notification)
            mMyNotification = value
        End Set
    End Property
    Friend Sub ActivateTrayNotification()
        Try
            If IsNothing(mMyNotification) Then
                WriteLog("ActivateTrayNotification called without assignin a notification!", eSeverity.Critical)
                nIcon.Dispose()
                Exit Sub
            End If
            AddHandler nIcon.MouseClick, AddressOf showNotification
            AddHandler nIcon.MouseDoubleClick, AddressOf showNotification
            AddHandler nIcon.Click, AddressOf showNotification
            AddHandler nIcon.DoubleClick, AddressOf showNotification
            'Default settings
            nIcon.Icon = My.Resources.iWarning
            nIcon.Visible = True
            nIcon.Text = "Notification!"
            If modMySettings.NotificationLevel >= 1 Then
                '"There will be an animated Alert icon in your system tray"
                tAnimateIcon = New Threading.Timer(New Threading.TimerCallback(AddressOf tIcon_Tick), Nothing, 0, 500)
                If modMySettings.NotificationLevel = 2 Then
                    '"There will be an animated Alert icon in your system tray, which show a balloon tip warning every 30 seconds!"
                    AddHandler nIcon.BalloonTipClosed, AddressOf showNotification
                    AddHandler nIcon.BalloonTipClicked, AddressOf showNotification
                    nIcon.BalloonTipIcon = ToolTipIcon.Error
                    nIcon.BalloonTipText = mMyNotification.Source & New String(CChar(Environment.NewLine), 2) & mMyNotification.Reason
                    nIcon.BalloonTipTitle = "Notification!"
                    nIcon.ShowBalloonTip(30000)
                    tBalloonTip = New Threading.Timer(New Threading.TimerCallback(AddressOf tBalloonTip_tick), Nothing, 30000, 30000)
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub showNotification(sender As Object, e As EventArgs)
        Try
            Dim mNotify As New Notify
            AddHandler mNotify.FormClosed, AddressOf FormClosed
            Notify.Notification = mMyNotification
            Notify.ActivateNotification()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub FormClosed(sender As Object, e As EventArgs)
        Try
            RaiseEvent NotificationClosed(Me, Nothing)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Sub New()
        nIcon.Visible = False
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                If Not IsNothing(tAnimateIcon) Then
                    tAnimateIcon.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
                    tAnimateIcon.Dispose()
                ElseIf Not IsNothing(tBalloonTip) Then
                    tBalloonTip.Change(Threading.Timeout.Infinite, Threading.Timeout.Infinite)
                    tBalloonTip.Dispose()
                End If
                nIcon.Visible = False
                nIcon.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
