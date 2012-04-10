Imports System.Text
Public Class clsAppContext
    Inherits ApplicationContext
    Public Sub New()
        MyBase.New()
        AddHandler Application.ApplicationExit, AddressOf OnApplicationExit
        Try
            If Not modMySettings.StartMinimized Then
                Timers.IntervalBasedParserEnabled = modMySettings.ParseLogsOnInterval And Not modMySettings.ParseOnEOCUpdate
                If modMySettings.MainForm = modMySettings.eMainForm.History Then
                    AddHandler History.FormClosing, AddressOf HandleFormClosing
                    History.ShowForm()
                Else
                    AddHandler Live.FormClosing, AddressOf HandleFormClosing
                    Live.ShowForm()
                End If
            Else
                modIcon.ShowIcon()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Me.ExitThread()
        End Try
    End Sub
    Private Sub HandleFormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs)
        Try
            If ReferenceEquals(sender, History) Then
                If History.SilentClose Or e.CloseReason <> CloseReason.UserClosing Then
                    modMySettings.ColumnSettings.UpdateColumnSettings(History.lvWU)
                    Return
                End If

                If e.CloseReason = CloseReason.UserClosing Then
                    If modMySettings.MinimizeToTray Then
                        e.Cancel = True
                        If IsFormVisible(History) Then HideFade(History)
                        History.Visible = False
                        'If Me.Visible Then HideForm()
                        modIcon.ShowIcon()
                        Return
                    Else
                        If modMySettings.MainForm = modMySettings.eMainForm.History Then
                            If MsgBox("Exit application?", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Question, MsgBoxStyle), "Confirm exit") = MsgBoxResult.Cancel Then
                                e.Cancel = True
                                Return
                            End If
                            History.SilentClose = True
                            Live.SilentClose = True
                            modMySettings.ColumnSettings.UpdateColumnSettings(History.lvWU)
                            ExitApplication()
                            Me.ExitThread()
                        Else

                        End If
                    End If
                End If
            Else
                If Live.SilentClose Or e.CloseReason <> CloseReason.UserClosing Then
                    modMySettings.ColumnSettings.UpdateColumnSettings(Live.lvLive)
                    Return
                End If
                If e.CloseReason = CloseReason.UserClosing Then
                    If modMySettings.MinimizeToTray Then
                        e.Cancel = True
                        If IsFormVisible(Live) Then HideFade(Live)
                        Live.Visible = False
                        'If Me.Visible Then HideForm()
                        modIcon.ShowIcon()
                        Return
                    Else
                        If modMySettings.MainForm = modMySettings.eMainForm.Live Then
                            If MsgBox("Exit application?", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Question, MsgBoxStyle), "Confirm exit") = MsgBoxResult.Cancel Then
                                e.Cancel = True
                                Return
                            End If
                            History.SilentClose = True
                            Live.SilentClose = True
                            modMySettings.ColumnSettings.UpdateColumnSettings(Live.lvLive)
                            ExitApplication()
                            Me.ExitThread()
                        Else

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Me.ExitThread()
        End Try
    End Sub
    Private Sub HandleFormClosed(sender As Object, e As EventArgs)
        modMySettings.SaveSettings()
        ExitThread()
    End Sub
    Private Sub OnApplicationExit(sender As Object, e As EventArgs)
        Me.ExitThread()
    End Sub
End Class
