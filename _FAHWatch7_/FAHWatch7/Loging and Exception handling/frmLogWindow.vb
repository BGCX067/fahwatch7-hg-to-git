'/*
' * FAHWatch7  Copyright Marvin Westmaas ( mtm )
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
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.RichTextBox
Imports System.Text
Imports System.Threading
Public Class frmLogWindow
    Private bAllowClose As Boolean = False
    Private bIsUpdating As Boolean = False
    Private bContentChanged As Boolean = True
    Private bIsShown As Boolean = False
    Private mLogFile As String
    Private iLastSave As Int32 = 0
    Private lLog As New List(Of String)
    Friend Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Try
            Me.CreateControl()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Friend Property LogFile As String
        Get
            Return mLogFile
        End Get
        Set(value As String)
            mLogFile = value
        End Set
    End Property
    Private Sub tLimitLog_Tick(sender As System.Object, e As System.EventArgs) Handles tLimitLog.Tick
        If bIsUpdating Then Exit Sub
        If Not bContentChanged Then Exit Sub
        bIsUpdating = True
        Try
            SyncLock lLog
                If iLastSave = 0 Then
                    'create file
                    iLastSave = lLog.Count - 1
                    Dim sb As StringBuilder = Nothing
                    Try
                        sb = New StringBuilder()
                        For Each Line As String In lLog
                            sb.AppendLine(Line)
                        Next
                        My.Computer.FileSystem.WriteAllText(mLogFile, sb.ToString, False, Encoding.UTF8)
                    Finally
                        If sb IsNot Nothing Then sb = Nothing
                    End Try
                Else
                    Dim sb As StringBuilder = Nothing
                    Try
                        sb = New StringBuilder()
                        For iInt As Int32 = iLastSave To lLog.Count - 1
                            sb.AppendLine(lLog(iInt))
                        Next
                        My.Computer.FileSystem.WriteAllText(mLogFile, sb.ToString, True, Encoding.UTF8)
                    Finally
                        If sb IsNot Nothing Then sb = Nothing
                    End Try
                    iLastSave = lLog.Count
                End If
            End SyncLock
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bIsUpdating = False
        End Try
    End Sub
    Friend Delegate Sub ScrollToEndDelegate()
    Private Sub dScrollToEnd()
        rtLog.SelectionStart = rtLog.TextLength
        rtLog.ScrollToCaret()
        rtLog.Refresh()
    End Sub
    Friend Sub ScrollToEnd()
        Try
            Dim nInv As New ScrollToEndDelegate(AddressOf dScrollToEnd)
            Dim result As IAsyncResult = rtLog.BeginInvoke(nInv)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            rtLog.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Property AllowClose As Boolean
        Get
            Return bAllowClose
        End Get
        Set(value As Boolean)
            bAllowClose = value
        End Set
    End Property
    Private Delegate Sub CloseFormDelegate()
    Private Sub dCloseForm()
        bAllowClose = True
        Close()
    End Sub
    Friend Sub CloseForm()
        Dim nI As New CloseFormDelegate(AddressOf dCloseForm)
        Dim result As IAsyncResult = Me.BeginInvoke(nI)
        While Not result.IsCompleted
            Application.DoEvents()
        End While
        Me.EndInvoke(result)
        result.AsyncWaitHandle.Close()
    End Sub
    Private Delegate Sub ShowAndActivateDelegate()
    Private Sub dShowAndActivate()
        Try
            If Not bIsShown Then
                Me.Show()
                Me.Activate()
                Me.WindowState = FormWindowState.Normal
                Me.ShowInTaskbar = True
                bIsShown = True
            Else
                delegateFactory.ShowExpand(Me)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub ShowAndActivate()
        Try
            Dim nI As New ShowAndActivateDelegate(AddressOf dShowAndActivate)
            Dim result As IAsyncResult = Me.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub rtLog_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles rtLog.MouseDoubleClick
        Try
            If Logwindow.ActiveWarning Then
                Dim rVal As MsgBoxResult
                rVal = MsgBox("Do you want to clear the warning icon?", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Information, MsgBoxStyle), "Clear warning notification")
                If rVal = MsgBoxResult.No Then Exit Sub
                Dim nArr As New MyEventArgs.ClearWarningEventArgs
                nArr.IconRemoved = True
                nArr.Message = "Cleared by user at-" & DateTime.Now.ToString("s")
                Logwindow.ClearWarning(Me, nArr)
                nArr = Nothing
                Icon = My.Resources.iLog
                Text = Application.ProductName & " " & Application.ProductVersion & " log"
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmLogWindow_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Me.Visible = False
            NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            If Not Me.IsHandleCreated Then Me.CreateHandle()
            Dim nI As New CreateLogDelegate(AddressOf dCreateLog)
            Dim result As IAsyncResult = Me.BeginInvoke(nI, {Logwindow.LogStartOptions.Filename, Logwindow.LogStartOptions.ShowIcon, Logwindow.LogStartOptions.ShowForm})
            'result.AsyncWaitHandle.WaitOne()
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmLogWindow_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Try
            If IsNothing(Me) Then Exit Sub
            If Not Visible Then Exit Sub
            If rtLog.TextLength > 0 Then
                rtLog.SelectionStart = rtLog.TextLength
                rtLog.ScrollToCaret()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property Log As String
        Get
            Return rtLog.Text
        End Get
    End Property
#Region "logging"
    Private Delegate Sub Addoutput(ByVal [text] As String)
    Private Delegate Sub CreateLogDelegate(Filename As String, ShowIcon As Boolean, ShowForm As Boolean)
    Private Sub dCreateLog(Filename As String, ShowIcon As Boolean, ShowForm As Boolean)
        Try
            lLog.Clear()
            rtLog.Clear()
            rtLog.MaxLength = Int32.MaxValue
            Icon = My.Resources.iLog
            If Filename = "" Then
                If mLogFile = "" Then
                    Filename = Application.StartupPath & "\" & Application.ProductName & ".log"
                Else
                    Filename = mLogFile
                End If
            Else
                Filename = Filename
            End If
            mLogFile = Filename
            If My.Computer.FileSystem.FileExists(Filename) Then
                If My.Computer.FileSystem.FileExists(Filename & ".old") Then My.Computer.FileSystem.DeleteFile(Filename & ".old")
                My.Computer.FileSystem.CopyFile(Filename, Filename & ".old", True)
                My.Computer.FileSystem.DeleteFile(Filename, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)
            End If
            ' TODO remove after diagnostics
            If modMySettings.LimitLogWrites Then
                tLimitLog.Interval = 1000
                tLimitLog.Enabled = True
                bContentChanged = False
            End If
            If ShowIcon Then
                modIcon.ShowIcon(My.Resources.iLog)
            End If
            If ShowForm Then
                Logwindow.ShowAndActivateLog()
            Else
                delegateFactory.SetFormWindowState(Me, FormWindowState.Minimized)
                delegateFactory.SetFormShowInTaskbar(Me, False)
                delegateFactory.HideFade(Me, 0)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub WriteLog(ByVal Message As String, Optional severity As eSeverity = eSeverity.Informative)
        Try
            If severity = eSeverity.Debug And Not DebugOutput Then Exit Sub
            If rtLog.IsDisposed Or rtLog.Disposing Then Exit Sub
            Message = "[" & DateTime.Now.ToShortDateString & "] * " & DateTime.Now.ToLongTimeString & " - " & Message
            lLog.Add(Message)
            If Me.InvokeRequired Then
                Dim nInvoke As New Addoutput(AddressOf wInv)
                Dim result As IAsyncResult = Me.BeginInvoke(nInvoke, {[Message]})
                'result.AsyncWaitHandle.WaitOne()
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                Me.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Else
                rtLog.AppendText(Message & Environment.NewLine)
                If delegateFactory.IsFormVisible(Me) Then ScrollToEnd()
                If Not IsNothing(modMySettings.LimitLogWrites) Then
                    If Not modMySettings.LimitLogWrites Then
                        rtLog.SaveFile(mLogFile, RichTextBoxStreamType.PlainText)
                        bContentChanged = False
                    Else
                        bContentChanged = True
                    End If
                Else
                    rtLog.SaveFile(mLogFile, RichTextBoxStreamType.PlainText)
                    bContentChanged = False
                End If
            End If

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub WriteError(ByVal message As String, ByVal errObject As ErrObject)
        Try
            WriteLog(message, eSeverity.Critical)
            WriteLog(" - Err.source: " & errObject.Source & " Line: " & errObject.Erl & " - Err.number : " & errObject.Number, eSeverity.Critical)
            WriteLog(" - Err.description: " & errObject.Description, eSeverity.Critical)
            WriteLog(" - stacktrace: " & errObject.GetException.StackTrace.ToString, eSeverity.Critical)
            If Not IsNothing(errObject.GetException.InnerException) Then
                WriteLog("CRITICAL::InnerException: " & errObject.GetException.InnerException.Message)
                WriteLog("CRITICAL::InnerException - stacktrace: " & errObject.GetException.InnerException.StackTrace)
            End If
        Catch ex As Exception 'cascading error's???
            WriteError(ex.Message, Err)
        Finally
            Try
                If Me.InvokeRequired Then
                    Dim nI As New SetIconDelegate(AddressOf dSetIcon)
                    Dim result As IAsyncResult = Me.BeginInvoke(nI, {My.Resources.iWarning})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    Me.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                Else
                    Me.Icon = My.Resources.iWarning
                End If
            Catch ex As Exception 'cascading error's???
                WriteError(ex.Message, Err)
            End Try
        End Try
    End Sub
    Friend Delegate Sub SetIconDelegate(Icon As Icon)
    Friend Sub dSetIcon(Icon As Icon)
        Me.Icon = Icon
    End Sub
    Friend Sub wInv(ByVal message As String)
        Try
            rtLog.AppendText(message & Environment.NewLine)
            If delegateFactory.IsFormVisible(Me) Then ScrollToEnd()
            If Not IsNothing(modMySettings.LimitLogWrites) Then
                If Not modMySettings.LimitLogWrites Then
                    rtLog.SaveFile(mLogFile, RichTextBoxStreamType.PlainText)
                    bContentChanged = False
                Else
                    bContentChanged = True
                End If
            Else
                rtLog.SaveFile(mLogFile, RichTextBoxStreamType.PlainText)
                bContentChanged = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    
End Class

