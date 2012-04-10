'   FAHWatch7   
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

' Placing the notifyIcon here has solved the events tracking issue.
Module modIcon
    Private WithEvents nIconMain As New NotifyIcon
    Private WithEvents nIconLog As New NotifyIcon
    Private WithEvents cMenu As New ContextMenuStrip
    Private ilMenu As New ImageList
    Sub New()
        ilMenu.Images.AddRange({My.Resources.iStartBMP, My.Resources.iStopBMP})
        nIconMain.ContextMenuStrip = cMenu
    End Sub
#Region "MainIcon events"
    Public Event BalloonTipClicked(Sender As Object, e As System.EventArgs)
    Private Sub nIconMain_BalloonTipClicked(sender As Object, e As System.EventArgs) Handles nIconMain.BalloonTipClicked
        Logwindow.ShowAndActivateLog()
    End Sub
    Public Event BalloonTipClosed(sender As Object, e As System.EventArgs)
    Private Sub nIconMain_BalloonTipClosed(sender As Object, e As System.EventArgs) Handles nIconMain.BalloonTipClosed
        RaiseEvent BalloonTipClosed(sender, e)
    End Sub
    Public Event BalloonTipShown(sender As Object, e As System.EventArgs)
    Private Sub nIconMain_BalloonTipShown(sender As Object, e As System.EventArgs) Handles nIconMain.BalloonTipShown
        RaiseEvent BalloonTipShown(sender, e)
    End Sub
    Public Event Click(sender As Object, e As System.EventArgs)
    Private Sub nIconMain_Click(sender As Object, e As System.EventArgs) Handles nIconMain.Click
        RaiseEvent Click(sender, e)
    End Sub
    Public Event Disposed(sender As Object, e As System.EventArgs)
    Private Sub nIconMain_Disposed(sender As Object, e As System.EventArgs) Handles nIconMain.Disposed
        RaiseEvent Disposed(sender, e)
    End Sub
    Public Event DoubleClick(sender As Object, e As System.EventArgs)
    Private Sub nIconMain_DoubleClick(sender As Object, e As System.EventArgs) Handles nIconMain.DoubleClick
        RaiseEvent DoubleClick(sender, e)
    End Sub
    Public Event MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconMain_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconMain.MouseClick
        Try
            If e.Button = MouseButtons.Left Then
                If Logwindow.ActiveWarning Then
                    Logwindow.ShowAndActivateLog()
                Else
                    Select Case modMySettings.MainForm
                        Case modMySettings.eMainForm.History
                            If delegateFactory.GetFormWindowState(History) = FormWindowState.Minimized Then
                                If Not History.HasBeenShown Then
                                    History.Show()
                                Else
                                    Application.DoEvents()
                                    NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                                    delegateFactory.ShowFormActivated(History)
                                    delegateFactory.SetFormWindowState(History, modMySettings.history_windowstate)
                                    delegateFactory.ActivateForm(History)
                                    History.Invalidate() : History.Refresh()
                                End If
                            ElseIf delegateFactory.IsFormVisible(History) Then
                                History.HideForm()
                            Else
                                If Not History.HasBeenShown Then
                                    History.Show()
                                Else
                                    NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                                    delegateFactory.ShowFormActivated(History)
                                    delegateFactory.ActivateForm(History)
                                    History.Invalidate() : History.Refresh()
                                End If
                            End If
                        Case modMySettings.eMainForm.Live
                            If delegateFactory.IsFormVisible(Live) Then
                                Live.HideForm()
                            Else
                                Live.ShowForm()
                            End If
                    End Select
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Public Event MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconMain_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconMain.MouseDoubleClick
        Select Case modMySettings.MainForm
            Case modMySettings.eMainForm.History
                If delegateFactory.GetFormWindowState(History) = FormWindowState.Minimized Then
                    If Not History.HasBeenShown Then
                        History.Show()
                    Else
                        NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                        delegateFactory.ShowFormActivated(History)
                        delegateFactory.SetFormWindowState(History, modMySettings.history_windowstate)
                        delegateFactory.ActivateForm(History)
                        History.Invalidate() : History.Refresh()
                    End If
                ElseIf delegateFactory.IsFormVisible(History) Then
                    History.HideForm()
                Else
                    If Not History.HasBeenShown Then
                        History.Show()
                    Else
                        NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                        delegateFactory.ShowFormActivated(History)
                        delegateFactory.ActivateForm(History)
                        History.Invalidate() : History.Refresh()
                    End If
                End If
            Case modMySettings.eMainForm.Live
                If delegateFactory.IsFormVisible(Live) Then
                    Live.HideForm()
                Else
                    Live.ShowForm()
                End If
        End Select
        'RaiseEvent MouseDoubleClick(sender, e)
    End Sub
    Public Event MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconMain_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconMain.MouseDown
        RaiseEvent MouseDown(sender, e)
    End Sub
    Public Event MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconMain_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconMain.MouseMove
        RaiseEvent MouseMove(sender, e)
    End Sub
    Public Event MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconMain_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconMain.MouseUp
        RaiseEvent MouseUp(sender, e)
    End Sub
#End Region
#Region "LogIcon events"
    Public Event logBalloonTipClicked(Sender As Object, e As System.EventArgs)
    Private Sub nIconLog_BalloonTipClicked(sender As Object, e As System.EventArgs) Handles nIconLog.BalloonTipClicked
        Logwindow.ShowAndActivateLog()
    End Sub
    Public Event logBalloonTipClosed(sender As Object, e As System.EventArgs)
    Private Sub nIconLog_BalloonTipClosed(sender As Object, e As System.EventArgs) Handles nIconLog.BalloonTipClosed
        RaiseEvent BalloonTipClosed(sender, e)
    End Sub
    Public Event logBalloonTipShown(sender As Object, e As System.EventArgs)
    Private Sub nIconLog_BalloonTipShown(sender As Object, e As System.EventArgs) Handles nIconLog.BalloonTipShown
        RaiseEvent BalloonTipShown(sender, e)
    End Sub
    Public Event logClick(sender As Object, e As System.EventArgs)
    Private Sub nIconLog_Click(sender As Object, e As System.EventArgs) Handles nIconLog.Click
        RaiseEvent Click(sender, e)
    End Sub
    Public Event logDisposed(sender As Object, e As System.EventArgs)
    Private Sub nIconLog_Disposed(sender As Object, e As System.EventArgs) Handles nIconLog.Disposed
        RaiseEvent Disposed(sender, e)
    End Sub
    Public Event logDoubleClick(sender As Object, e As System.EventArgs)
    Private Sub nIconLog_DoubleClick(sender As Object, e As System.EventArgs) Handles nIconLog.DoubleClick
        RaiseEvent DoubleClick(sender, e)
    End Sub
    Public Event logMouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconLog_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconLog.MouseClick
        Try
            If e.Button = MouseButtons.Left Then
                If Logwindow.ActiveWarning Then
                    Logwindow.ShowAndActivateLog()
                Else
                    Select Case modMySettings.MainForm
                        Case modMySettings.eMainForm.History
                            If delegateFactory.GetFormWindowState(History) = FormWindowState.Minimized Then
                                If Not History.HasBeenShown Then
                                    History.Show()
                                Else
                                    Application.DoEvents()
                                    NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                                    delegateFactory.ShowFormActivated(History)
                                    delegateFactory.SetFormWindowState(History, modMySettings.history_windowstate)
                                    delegateFactory.ActivateForm(History)
                                    History.Invalidate() : History.Refresh()
                                End If
                            ElseIf delegateFactory.IsFormVisible(History) Then
                                History.HideForm()
                            Else
                                If Not History.HasBeenShown Then
                                    History.Show()
                                Else
                                    NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                                    delegateFactory.ShowFormActivated(History)
                                    delegateFactory.ActivateForm(History)
                                    History.Invalidate() : History.Refresh()
                                End If
                            End If
                        Case modMySettings.eMainForm.Live
                            If delegateFactory.IsFormVisible(Live) Then
                                Live.HideForm()
                            Else
                                Live.ShowForm()
                            End If
                    End Select
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Public Event logMouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconLog_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconLog.MouseDoubleClick
        Select Case modMySettings.MainForm
            Case modMySettings.eMainForm.History
                If delegateFactory.GetFormWindowState(History) = FormWindowState.Minimized Then
                    If Not History.HasBeenShown Then
                        History.Show()
                    Else
                        NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                        delegateFactory.ShowFormActivated(History)
                        delegateFactory.SetFormWindowState(History, modMySettings.history_windowstate)
                        delegateFactory.ActivateForm(History)
                        History.Invalidate() : History.Refresh()
                    End If
                ElseIf delegateFactory.IsFormVisible(History) Then
                    History.HideForm()
                Else
                    If Not History.HasBeenShown Then
                        History.Show()
                    Else
                        NativeMethods.AnimateWindow(History.Handle, 50, NativeMethods.AnimateWindowFlags.AW_ACTIVATE Or NativeMethods.AnimateWindowFlags.AW_CENTER)
                        delegateFactory.ShowFormActivated(History)
                        delegateFactory.ActivateForm(History)
                        History.Invalidate() : History.Refresh()
                    End If
                End If
            Case modMySettings.eMainForm.Live
                If delegateFactory.IsFormVisible(Live) Then
                    Live.HideForm()
                Else
                    Live.ShowForm()
                End If
        End Select
        'RaiseEvent MouseDoubleClick(sender, e)
    End Sub
    Public Event logMouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconLog_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconLog.MouseDown
        RaiseEvent MouseDown(sender, e)
    End Sub
    Public Event logMouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconLog_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconLog.MouseMove
        RaiseEvent MouseMove(sender, e)
    End Sub
    Public Event logMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
    Private Sub nIconLog_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles nIconLog.MouseUp
        RaiseEvent MouseUp(sender, e)
    End Sub
#End Region
#Region "Main tray icon"
    Private Sub cMenu_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cMenu.ItemClicked
        If e.ClickedItem.Text = "Exit" Then
            cMenu.Close()
            nIconMain.Visible = False
            appContext.ExitThread()
        ElseIf e.ClickedItem.Text = "View history" Then
            History.Show()
        ElseIf e.ClickedItem.Text = "Hide history" Then
            History.HideForm()
        ElseIf e.ClickedItem.Text = "View monitor" Then
            Live.ShowForm()
        ElseIf e.ClickedItem.Text = "Hide monitor" Then
            Live.HideForm()
        ElseIf e.ClickedItem.Text = "View log" Then
            Logwindow.ShowAndActivateLog()
        ElseIf e.ClickedItem.Text = "Hide log" Then
            Logwindow.HideDebugWindow()
        End If
    End Sub

    Private Sub cMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cMenu.Opening
        cMenu.Items.Clear()
        'If Logwindow.ActiveWarning Then
        If delegateFactory.IsFormVisible(Logwindow.Form) Then
            cMenu.Items.Add("Hide log")
        Else
            cMenu.Items.Add("View log")
        End If
        cMenu.Items.Add("-")
        'End If
        If delegateFactory.IsFormVisible(Live) Then
            cMenu.Items.Add("Hide monitor")
        Else
            cMenu.Items.Add("View monitor")
        End If
        If delegateFactory.IsFormVisible(History) Then
            cMenu.Items.Add("Hide history")
        Else
            cMenu.Items.Add("View history")
        End If
        cMenu.Items.Add("-")
        Dim allRunning As New ToolStripMenuItem("Stop all")
        allRunning.Image = My.Resources.iStopBMP
        If Clients.AllStopped Then allRunning.Enabled = False
        Dim allStoped As New ToolStripMenuItem("Start all")
        allStoped.Image = My.Resources.iStartBMP
        If Clients.AllRunning Then allStoped.Enabled = False
        cMenu.Items.Add(allRunning)
        cMenu.Items.Add(allStoped)
        cMenu.Items.Add("-")
        For Each Client In Clients.Clients
            Dim nI As New ToolStripMenuItem
            nI.Text = Client.ClientName
            If Not Client.Reachable Then
                nI.Image = My.Resources.iWarning1
            Else
                nI.Image = My.Resources.iTray1
            End If
            Dim allRunningC As New ToolStripMenuItem("Stop all")
            allRunningC.Image = My.Resources.iStopBMP
            Dim allStopedC As New ToolStripMenuItem("Start all")
            allStopedC.Image = My.Resources.iStartBMP
            If Client.AllRunning Then allStopedC.Enabled = False
            If Client.AllStopped Then allRunningC.Enabled = False
            nI.DropDownItems.Add(allRunningC)
            nI.DropDownItems.Add(allStopedC)
            nI.DropDownItems.Add("-")
            For Each Slot In Client.Slots
                Dim sI As New ToolStripMenuItem
                sI.Text = Slot.Hardware
                If Slot.Status = "PAUSED" Then
                    sI.Image = My.Resources.iStartBMP
                Else
                    sI.Image = My.Resources.iStopBMP
                End If
                nI.DropDownItems.Add(sI)
            Next
            cMenu.Items.Add(nI)
        Next
        cMenu.Items.Add("-")
        cMenu.Items.Add("Exit")
    End Sub
    Friend Sub CloseApplication(Optional ConfirmExit As Boolean = False)
        If ConfirmExit Then
            If MsgBox("Exit application?", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Question, MsgBoxStyle), "Confirm exit") = MsgBoxResult.Cancel Then Return
        End If
        'Me.Close()
    End Sub
    Friend Property IconVisible(Optional IsLogIcon As Boolean = False) As Boolean
        Get
            If IsLogIcon Then
                Return nIconLog.Visible
            Else
                Return nIconMain.Visible
            End If
        End Get
        Set(value As Boolean)
            If IsLogIcon Then
                nIconLog.Visible = value
            Else
                nIconMain.Visible = value
            End If
        End Set
    End Property
    Friend Sub IconHide(Optional IsLogIcon As Boolean = False)
        If IsLogIcon Then
            nIconLog.Visible = False
        Else
            nIconMain.Visible = False
        End If
    End Sub
    Friend Sub ShowIcon(Optional Icon As Icon = Nothing, Optional IsLogIcon As Boolean = False)
        If Not IsLogIcon Then
            If IsNothing(Icon) Then
                nIconMain.Visible = True
                If IsNothing(nIconMain.Icon) Then nIconMain.Icon = My.Resources.iTray
            Else
                nIconMain.Icon = Icon
                nIconMain.Visible = True
            End If
            nIconMain.Text = My.Application.Info.AssemblyName & Chr(32) & My.Application.Info.Version.ToString
        Else
            If IsNothing(Icon) Then
                nIconLog.Visible = True
                If IsNothing(nIconLog.Icon) Then nIconLog.Icon = My.Resources.iLog
            Else
                nIconLog.Icon = Icon
                nIconLog.Visible = True
            End If
            nIconLog.Text = My.Application.Info.AssemblyName & Chr(32) & My.Application.Info.Version.ToString
        End If
    End Sub
    Friend Property IconText(Optional IsLogIcon As Boolean = False) As String
        Get
            If Not IsLogIcon Then
                Return nIconMain.Text
            Else
                Return nIconLog.Text
            End If
        End Get
        Set(value As String)
            If Not IsLogIcon Then
                nIconMain.Text = value
            Else
                nIconLog.Text = value
            End If
        End Set
    End Property
    Friend Property BalloonTipText(Optional IsLogIcon As Boolean = False) As String
        Get
            If IsLogIcon Then
                Return nIconLog.BalloonTipText
            Else
                Return nIconMain.BalloonTipText
            End If
        End Get
        Set(value As String)
            If IsLogIcon Then
                nIconLog.BalloonTipText = value
            Else
                nIconMain.BalloonTipText = value
            End If
        End Set
    End Property
    Friend Property BalloonTipIcon(Optional IsLogIcon As Boolean = False) As ToolTipIcon
        Get
            If IsLogIcon Then
                Return nIconLog.BalloonTipIcon
            Else
                Return nIconMain.BalloonTipIcon
            End If
        End Get
        Set(value As ToolTipIcon)
            If IsLogIcon Then
                nIconLog.BalloonTipIcon = value
            Else
                nIconMain.BalloonTipIcon = value
            End If
        End Set
    End Property
    Friend Property BalloonTipTitle(Optional IsLogIcon As Boolean = False) As String
        Get
            If IsLogIcon Then
                Return nIconLog.BalloonTipTitle
            Else
                Return nIconMain.BalloonTipTitle
            End If
        End Get
        Set(value As String)
            If IsLogIcon Then
                nIconLog.BalloonTipTitle = value
            Else
                nIconMain.BalloonTipTitle = value
            End If
        End Set
    End Property
    Friend Sub ShowBalloonTip(Optional TimeOut As Integer = 15000, Optional IsLogIcon As Boolean = False)
        If IsLogIcon Then
            nIconLog.ShowBalloonTip(TimeOut)
        Else
            nIconMain.ShowBalloonTip(TimeOut)
        End If
    End Sub
#End Region
End Module


