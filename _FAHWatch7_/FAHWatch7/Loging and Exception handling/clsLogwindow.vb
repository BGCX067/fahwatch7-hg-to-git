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
Imports System.Text
Imports FAHWatch7.MyEventArgs
Imports FAHWatch7.modIcon
Imports System.Threading

Friend Class Logwindow
    Implements IDisposable
#Region "warning status - set by a logged error "
    Private Shared bWarning As Boolean = False 'Set when an error has been logged
    Private Shared mErrorEvent As MyEventArgs.ErrorEventArgs
    Private Shared WithEvents mForm As frmLogWindow
    Friend Shared bAllowClose As Boolean = False
    Friend ReadOnly Property IconVisible As Boolean
        Get
            Return modIcon.IconVisible
        End Get
    End Property
    Friend Shared ReadOnly Property ActiveWarning As Boolean
        Get
            Return bWarning
        End Get
    End Property
    Friend Shared ReadOnly Property WarningEvent As MyEventArgs.ErrorEventArgs
        Get
            Return mErrorEvent
        End Get
    End Property
    Friend Shared Sub ClearWarning(sender As Object, e As ClearWarningEventArgs)
        Try
            tAnimate.Enabled = False
            modIcon.ShowIcon(My.Resources.iTray)
            If Not mForm.InvokeRequired Then
                mForm.Icon = My.Resources.iLog
            Else
                Dim dS As New dSetFormIcon(AddressOf SetIcon)
                mForm.Invoke(dS, {My.Resources.iLog})
            End If
            If Not modMySettings.MinimizeToTray Then
                modIcon.IconVisible = False
            End If
            bWarning = False
            mErrorEvent = Nothing
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "delegates / callbacks"
    Friend Delegate Sub ActivateLogDelegate()
    Friend Shared ActivateLogCallBack As New AsyncCallback(AddressOf acbActivateLog)
    Friend Shared Sub acbActivateLog(result As IAsyncResult)
        mForm.EndInvoke(result)
        result.AsyncWaitHandle.Close()
    End Sub
    Private Shared Sub dActivateLog()
        If ActiveWarning Then
            Dim nI As New dSetFormIcon(AddressOf SetIcon)
#If CONFIG = "Debug" Then
            If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke dSetFormIcon")
#End If
            Dim result As IAsyncResult = mForm.BeginInvoke(nI, {My.Resources.iWarning})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
#If CONFIG = "Debug" Then
            If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke dSetFormIcon")
#End If
            mForm.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        End If
        mForm.Activate()
        Dim nI2 As New IsFormFocusedDelegate(AddressOf dIsFormFocused)
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke IsFormFocussedDelegate")
#End If
        Dim result2 As IAsyncResult = mForm.BeginInvoke(nI2)
        While Not result2.IsCompleted
            Application.DoEvents()
        End While
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke IsFormFocussedDelegate")
#End If
        Dim bRes As Boolean = CBool(mForm.EndInvoke(result2))
        result2.AsyncWaitHandle.Close()
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine("Log has focus: " & bRes.ToString)
#End If
        If Not bRes Then
#If CONFIG = "Debug" Then
            If Not bNoConsole Then Console.WriteLine(delegateFactory.GetFormWindowState(mForm).ToString)
            If Not bNoConsole Then Console.WriteLine("Trying to place form on top of Zorder")
            delegateFactory.SetFormTop(mForm)
#End If
        End If
    End Sub
    Friend Delegate Function IsFormEnabledDelegate(frm As Form) As Boolean
    Friend Shared IsFormEnabledCallBack As New AsyncCallback(AddressOf acbIsFormEnabled)
    Friend Shared Function acbIsFormEnabled(result As IAsyncResult) As Boolean
        Dim rBool As Boolean = CBool(mForm.EndInvoke(result))
        result.AsyncWaitHandle.Close()
        Return rBool
    End Function
    Friend Shared Function IsFormEnabled(frm As Form) As Boolean
        Return frm.Enabled
    End Function
    Friend Delegate Function IsFormFocusedDelegate() As Boolean
    Friend Shared IsFormFocusedCallBack As New AsyncCallback(AddressOf acbIsFormFocused)
    Friend Shared Function acbIsFormFocused(result As IAsyncResult) As Boolean
        Dim rVal As Boolean = CBool(mForm.EndInvoke(result))
        result.AsyncWaitHandle.Close()
        Return rVal
    End Function
    Friend Shared Function dIsFormFocused() As Boolean
        Return mForm.Focused
    End Function
    Friend Delegate Sub HideLogDelegate()
    Friend Shared HideLogCallBack As New AsyncCallback(AddressOf acbHideLog)
    Friend Shared Sub acbHideLog(result As IAsyncResult)
        mForm.EndInvoke(result)
        result.AsyncWaitHandle.Close()
    End Sub
    Friend Shared Sub dHideLog()
        mForm.Hide()
        mForm.Visible = False
    End Sub
    Friend Delegate Function IsFormVisibleDelegate() As Boolean
    Friend Shared IsFormVisibleCallBack As New AsyncCallback(AddressOf acbIsFormVisible)
    Friend Shared Function acbIsFormVisible(result As IAsyncResult) As Boolean
        Dim rVal As Boolean = CBool(mForm.EndInvoke(result))
        result.AsyncWaitHandle.Close()
        Return rVal
    End Function
    Friend Shared Function dIsFormVisible() As Boolean
        Return mForm.Visible
    End Function
    Friend Delegate Sub dSetFormIcon(TheIcon As Icon)
    Friend Shared SetFormIconCallBack As New AsyncCallback(AddressOf acbSetFormIcon)
    Friend Shared Sub acbSetFormIcon(result As IAsyncResult)
        mForm.EndInvoke(result)
        result.AsyncWaitHandle.Close()
    End Sub
    Friend Shared Sub SetIcon(TheIcon As Icon)
        mForm.Icon = TheIcon
    End Sub
    Friend Delegate Sub WriteErrorDelegate(Message As String, ErrObj As ErrObject)
    Friend Shared WriteErrorCallBack As New AsyncCallback(AddressOf acbWriteError)
    Friend Shared Sub acbWriteError(result As IAsyncResult)
        mForm.EndInvoke(result)
        result.AsyncWaitHandle.Close()
    End Sub
    Private Shared Sub dWriteError(Message As String, ErrObj As ErrObject)
        Try
            If delegateFactory.BussyBox.IsFormVisible Then
                'restore active windows when appliable, workaround.
                WriteLog("Closing bussybox do to exception", eSeverity.Important)
                delegateFactory.BussyBox.CloseForm()
            End If
            'Add to log
            mForm.WriteError(Message, ErrObj)
            'Check exceptions
            If Not modMySettings.DisableExceptionReport AndAlso Not Exceptions.ShouldIgnore(ErrObj.GetException, Err) Then
                'show exception report
                Exceptions.ShowReport(Message, ErrObj)
            Else
                If Not modMySettings.DisableExceptionReport AndAlso Exceptions.ShouldIgnore(ErrObj.GetException, ErrObj) Then
                    'Don't show balloon tip/play sound 
                    CheckWarning(Message, ErrObj, False)
                Else
                    'show balloontip
                    CheckWarning(Message, ErrObj, True)
                End If
            End If
            mErrorEvent = New MyEventArgs.ErrorEventArgs
            mErrorEvent.Message = Message
            mErrorEvent.Err = ErrObj
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub CheckWarning(Message As String, ErrObj As ErrObject, ExtendedWarning As Boolean)
        Try
            If Not bWarning Then
                If ExtendedWarning Then My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                modIcon.ShowIcon(My.Resources.iWarning)
                If Not mForm.InvokeRequired Then
                    mForm.Icon = My.Resources.iWarning
                Else
                    Dim nI As New dSetFormIcon(AddressOf SetIcon)
#If CONFIG = "Debug" Then
                    If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke dSetFormIcon")
#End If
                    Dim result As IAsyncResult = mForm.BeginInvoke(nI, {My.Resources.iWarning})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
#If CONFIG = "Debug" Then
                    If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke dSetFormIcon")
#End If
                    mForm.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                End If
                tAnimate.Interval = 500
                tAnimate.AutoReset = False
                If Message.Length < 63 Then
                    modIcon.IconText = Message
                Else
                    modIcon.IconText = Message.Substring(0, 63)
                End If
                modIcon.IconVisible = True
                modIcon.BalloonTipText = WarningText(ErrObj)
                modIcon.BalloonTipIcon = ToolTipIcon.Error
                If ExtendedWarning Then modIcon.ShowBalloonTip(300000)
                tAnimate.Enabled = True
                bWarning = True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Delegate Sub ShowAndActivateLogDelegate()
    Private Shared Sub dShowAndActivateLog()
        Try
            mForm.WindowState = mLastwindowState
            NativeMethods.AnimateWindow(mForm.Handle, 100, NativeMethods.AnimateWindowFlags.AW_BLEND)
            delegateFactory.SetFormWindowState(Logwindow.Form, mLastwindowState)
            delegateFactory.ShowWindow(mForm, NativeMethods.ShowWindowCommands.SW_SHOW)
            delegateFactory.ActivateForm(mForm)
            mForm.ScrollToEnd()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub FirstShow()
        Try
            delegateFactory.SetFormShowInTaskbar(Logwindow.Form, True)
            delegateFactory.SetFormWindowState(Logwindow.Form, FormWindowState.Normal)
            NativeMethods.AnimateWindow(delegateFactory.GetFormHandle(mForm), 100, NativeMethods.AnimateWindowFlags.AW_BLEND Or NativeMethods.AnimateWindowFlags.AW_ACTIVATE)
            delegateFactory.ShowFormActivated(Logwindow.Form)
            'delegateFactory.ShowWindow(mForm, NativeMethods.ShowWindowCommands.SW_SHOW)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub ShowAndActivateLog()
        Try
            Logwindow.Form.ShowAndActivate()
            'If Not mBeenShown Then
            '    FirstShow()
            '    mBeenShown = True
            'Else
            '    Dim nI As New ShowAndActivateLogDelegate(AddressOf dShowAndActivateLog)
            '    Dim result As IAsyncResult = Logwindow.Form.BeginInvoke(nI)
            '    While Not result.IsCompleted
            '        Application.DoEvents()
            '    End While
            '    Logwindow.Form.EndInvoke(result)
            '    result.AsyncWaitHandle.Close()
            'End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Icon animation"
    Private Shared WithEvents tAnimate As New System.Timers.Timer
    Private Shared iCurrent As Int32 = 0
    Private Shared Sub AnimateIcon() Handles tAnimate.Elapsed
        'Should use some nicer icons
        Try
            Dim bDoNext As Boolean = True
            If modIcon.IconVisible Then
                If iCurrent = 0 Then
                    modIcon.ShowIcon(My.Resources.iTray)
                    If Not mForm.InvokeRequired Then
                        mForm.Icon = My.Resources.iTray
                    Else
                        Dim dS As New dSetFormIcon(AddressOf SetIcon)
                        mForm.Invoke(dS, {My.Resources.iTray})
                    End If
                    iCurrent = 1
                Else
                    modIcon.ShowIcon(My.Resources.iWarning)
                    If Not mForm.InvokeRequired Then
                        mForm.Icon = My.Resources.iWarning
                    Else
                        Dim dS As New dSetFormIcon(AddressOf SetIcon)
                        mForm.Invoke(dS, {My.Resources.iWarning})
                    End If
                    iCurrent = 0
                End If
            Else
                bDoNext = False
            End If
            If bWarning And bDoNext Then tAnimate.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "log form"
    Private Shared mLastwindowState As System.Windows.Forms.FormWindowState, mBeenShown As Boolean = False
    Private Shared mInterlock As ManualResetEvent
    Friend Overloads Shared Sub ShowDebugWindow(Optional ByVal theIcon As Icon = Nothing, Optional ByVal exitApp As Boolean = False)
        Try
            If Not IsNothing(theIcon) Then
                If Not mForm.InvokeRequired Then
                    mForm.Icon = theIcon
                Else
                    Dim nI As New dSetFormIcon(AddressOf SetIcon)
#If CONFIG = "Debug" Then
                    If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke dSetFormIcon")
#End If
                    Dim result As IAsyncResult = mForm.BeginInvoke(nI, {theIcon})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
#If CONFIG = "Debug" Then
                    If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke dSetFormIcon")
#End If
                    mForm.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                End If
            Else
                If Not ActiveWarning Then
                    If Not mForm.InvokeRequired Then
                        mForm.Icon = My.Resources.iLog
                    Else
                        Dim nI As New dSetFormIcon(AddressOf SetIcon)
#If CONFIG = "Debug" Then
                        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke dSetFormIcon")
#End If
                        Dim result As IAsyncResult = mForm.BeginInvoke(nI, {My.Resources.iLog})
                        While Not result.IsCompleted
                            Application.DoEvents()
                        End While
#If CONFIG = "Debug" Then
                        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke dSetFormIcon")
#End If
                        mForm.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                    End If
                Else
                    If Not mForm.InvokeRequired Then
                        mForm.Icon = My.Resources.iWarning
                    Else
                        Dim nI As New dSetFormIcon(AddressOf SetIcon)
#If CONFIG = "Debug" Then
                        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke dSetFormIcon")
#End If
                        Dim result As IAsyncResult = mForm.BeginInvoke(nI, {My.Resources.iWarning})
                        While Not result.IsCompleted
                            Application.DoEvents()
                        End While
#If CONFIG = "Debug" Then
                        If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke dSetFormIcon")
#End If
                        mForm.EndInvoke(result)
                        result.AsyncWaitHandle.Close()
                    End If
                End If
            End If
            mForm.AllowClose = exitApp
            ShowAndActivateLog()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub HideDebugWindow()
        
            Dim nI As New HideLogDelegate(AddressOf dHideLog)
#If CONFIG = "Debug" Then
            If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke HideLogDelegate")
#End If
            Dim result As IAsyncResult = mForm.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
#If CONFIG = "Debug" Then
            If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke HideLogDelegate")
#End If
            mForm.EndInvoke(result)
            result.AsyncWaitHandle.Close()

    End Sub
    Friend Shared ReadOnly Property IsWindowActive() As Boolean
        Get
            If Not mForm.InvokeRequired Then
                Return mForm.Visible
            Else
                Dim nI As New IsFormVisibleDelegate(AddressOf dIsFormVisible)
#If CONFIG = "Debug" Then
                If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke IsFormVisibleDelegate")
#End If
                Dim result As IAsyncResult = mForm.BeginInvoke(nI)
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
#If CONFIG = "Debug" Then
                If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke IsFormVisibleDelegate")
#End If
                Dim bRet As Boolean = CBool(mForm.EndInvoke(result))
                result.AsyncWaitHandle.Close()
                Return bRet
            End If
        End Get
    End Property
    Friend Shared ReadOnly Property Form As frmLogWindow
        Get
            Return mForm
        End Get
    End Property
    Private Shared Sub mFormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles mForm.FormClosing
        Try
            If e.CloseReason = CloseReason.WindowsShutDown Or e.CloseReason = CloseReason.ApplicationExitCall Then
                bAllowClose = True
                WriteLog("Application being closed because of  " & [Enum].GetName(GetType(CloseReason), e.CloseReason).ToString)
            End If
            If Not bAllowClose Then
                e.Cancel = True
                If ActiveWarning Then
                    modIcon.IconVisible = True
                    tAnimate.Enabled = True
                End If
                mForm.Hide()
            Else
                RemoveHandler mForm.FormClosing, AddressOf mFormClosing
                If Not ActiveWarning Then
                    tAnimate.Enabled = False
                    modIcon.ShowIcon(My.Resources.iTray)
                Else
                    tAnimate.Enabled = True
                    modIcon.ShowIcon(My.Resources.iWarning)
                End If
                'modIcon.IconVisible = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub mForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles mForm.Resize
        If mForm.WindowState = FormWindowState.Minimized Then
            HideDebugWindow()
        Else
            mLastwindowState = mForm.WindowState
        End If
    End Sub
#End Region
#Region "logging"
    Friend Shared Sub WriteLog(ByVal Message As String, Optional severity As eSeverity = eSeverity.Informative)
#If CONFIG = "Debug" Then
        If Not bNoConsole Then Console.WriteLine(Message)
#End If
        mForm.WriteLog(Message, severity)
    End Sub
    Private Shared Function WarningText(ErrObj As ErrObject) As String
        Dim rVal As String = " - Err.source: " & ErrObj.Source & " Line: " & ErrObj.Erl & " - Err.number : " & ErrObj.Number & Environment.NewLine
        rVal &= " - Err.description: " & ErrObj.Description & Environment.NewLine
        rVal &= " - stacktrace: " & ErrObj.GetException.StackTrace.ToString & Environment.NewLine
        Return rVal
    End Function
    Friend Shared Sub WriteError(ByVal message As String, ByVal errObject As ErrObject)
        Try
            If Not mForm.InvokeRequired Then
                Call dWriteError(message, errObject)
            Else
                Dim nI As New WriteErrorDelegate(AddressOf dWriteError)
#If CONFIG = "Debug" Then
                If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- BeginInvoke WriteErrorDelegate")
#End If
                Dim result As IAsyncResult = mForm.BeginInvoke(nI, {message, errObject})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
#If CONFIG = "Debug" Then
                If Not bNoConsole Then Console.WriteLine(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- EndInvoke WriteErrorDelegate")
#End If
                mForm.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Create / dispose"
    Protected Friend Structure LogStartOptions
        Shared Property Filename As String = ""
        Shared Property ShowIcon As Boolean = False
        Shared Property ShowForm As Boolean = False
    End Structure
    Public Class logContext
        Inherits ApplicationContext
        Public Event formLoaded(sender As Object, e As EventArgs)
        Public Sub mf_loaded(sender As Object, e As EventArgs) Handles Me.formLoaded
            RaiseEvent formLoaded(sender, e)
        End Sub
        Sub New()
            Me.MainForm = mForm
        End Sub
    End Class
    Private Shared lContext As logContext
    Friend Shared Sub CreateLog(Optional ByVal filename As String = "", Optional ByVal showicon As Boolean = False)
        mInterlock = New ManualResetEvent(False)
        Dim t As Thread = New Thread(AddressOf LogStart)
        t.SetApartmentState(ApartmentState.STA)
        Dim lStart As New LogStartOptions
        LogStartOptions.Filename = filename
        LogStartOptions.ShowIcon = showicon
        t.Start(lStart)
        mInterlock.WaitOne()
    End Sub
    Private Shared Sub LogStart(lStart As Object)
        mForm = New frmLogWindow
        AddHandler mForm.HandleCreated, AddressOf HandleLoad
        Dim logStart As LogStartOptions = DirectCast(lStart, LogStartOptions)
        Application.Run(mForm)
    End Sub
    Private Shared Sub HandleLoad(sender As Object, e As EventArgs)
        mInterlock.Set()
    End Sub
    Friend Shared Sub CloseLog()
        WriteLog("Closing logging class")
        mForm.AllowClose = True
        mForm.CloseForm()
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                mForm.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    Friend Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region



    Public Sub New()

    End Sub
End Class
