Imports FAHWatch7.NativeMethods
Imports System.Threading
Module delegateFactory
#Region "IsFormVisible"
    Private Delegate Function IsFormVisibleDelegate(form As Form) As Boolean
    Private Function dIsFormVisible(form As Form) As Boolean
        Return form.Visible
    End Function
    Friend Function IsFormVisible(form As Form) As Boolean
        Try
            If Not form.IsHandleCreated Then Return False
            Dim nI As New IsFormVisibleDelegate(AddressOf dIsFormVisible)
            Dim result As IAsyncResult = form.BeginInvoke(nI, form)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As Boolean = CBool(form.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRes
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "IsFormFocused"
    Private Delegate Function IsFormFocusedDelegate(form As Form) As Boolean
    Private Function dIsFormFocussed(form As Form) As Boolean
        Return form.Focused
    End Function
    Friend Function IsFormFocused(form As Form) As Boolean
        Try
            Dim nI As New IsFormFocusedDelegate(AddressOf dIsFormFocussed)
            Dim result As IAsyncResult = form.BeginInvoke(nI, form)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(form.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Enable/Disable form"
    Private Delegate Function SetFormEnabledDelegate(frm As Form, Enabled As Boolean) As Boolean
    Private Function dSetFormEnabled(frm As Form, Enabled As Boolean) As Boolean
        Try
            frm.Enabled = Enabled
            Return frm.Enabled = Enabled
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function SetFormEnabled(frm As Form, Enabled As Boolean) As Boolean
        Try
            Dim nI As New SetFormEnabledDelegate(AddressOf dSetFormEnabled)
            Dim result As IAsyncResult = frm.BeginInvoke(nI, {frm, Enabled})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As Boolean = CBool(frm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRes
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Delegate Function IsFormEnabledDelegate(frm As Form) As Boolean
    Private Function dIsFormEnabled(frm As Form) As Boolean
        Return frm.Enabled
    End Function
    Friend Function IsFormEnabled(frm As Form) As Boolean
        Try
            Dim nI As New IsFormEnabledDelegate(AddressOf dIsFormEnabled)
            Dim result As IAsyncResult = frm.BeginInvoke(nI, frm)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(frm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Close form"
    Private Delegate Function CloseFormDelegate(frm As Form) As Boolean
    Private Function dCloseForm(frm As Form) As Boolean
        Try
            frm.Close()
            frm.Dispose()
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function CloseForm(frm As Form) As Boolean
        Try
            Dim nI As New CloseFormDelegate(AddressOf dCloseForm)
            Dim result As IAsyncResult = frm.BeginInvoke(nI, {frm})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(frm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "ShowInTaskbar"
    Private Delegate Function SetShowInTaskbarDelegate(frm As Form, Show As Boolean) As Boolean
    Private Function dSetShowInTaskbar(frm As Form, Show As Boolean) As Boolean
        Try
            frm.ShowInTaskbar = Show
            Return frm.ShowInTaskbar = Show
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function SetFormShowInTaskbar(frm As Form, Show As Boolean) As Boolean
        Try
            Dim nI As New SetShowInTaskbarDelegate(AddressOf dSetShowInTaskbar)
            Dim result As IAsyncResult = frm.BeginInvoke(nI, {frm, Show})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(frm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Delegate Function GetShowInTaskbarDelegate(frm As Form) As Boolean
    Private Function dGetShowInTaskbar(frm As Form) As Boolean
        Return frm.ShowInTaskbar
    End Function
    Friend Function GetShowInTaskbar(frm As Form) As Boolean
        Try
            Dim nI As New GetShowInTaskbarDelegate(AddressOf dGetShowInTaskbar)
            Dim result As IAsyncResult = frm.BeginInvoke(nI, frm)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(frm.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "GetFormWindowState"
    Private Delegate Function GetFormWindowStateDelegate(form As Form) As FormWindowState
    Private Function dGetFormWindowState(form As Form) As FormWindowState
        Return form.WindowState
    End Function
    Friend Function GetFormWindowState(form As Form) As FormWindowState
        Try
            If Not form.IsHandleCreated Then Return FormWindowState.Minimized 'return minimized to prevent action being taken due to this form
            Dim nI As New GetFormWindowStateDelegate(AddressOf dGetFormWindowState)
            Dim result As IAsyncResult = form.BeginInvoke(nI, form)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As FormWindowState = CType(form.EndInvoke(result), FormWindowState)
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return Nothing
        End Try
    End Function
#End Region
#Region "SetFormWindowState"
    Private Delegate Function SetFormWindowStateDelegate(form As Form, windowState As FormWindowState) As Boolean
    Private Function dSetFormWindowState(form As Form, windowState As FormWindowState) As Boolean
        Try
            form.WindowState = windowState
            Return form.WindowState = windowState
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function SetFormWindowState(form As Form, windowState As FormWindowState) As Boolean
        Try
            Dim nI As New SetFormWindowStateDelegate(AddressOf dSetFormWindowState)
            Dim result As IAsyncResult = form.BeginInvoke(nI, {form, windowState})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean = CBool(form.EndInvoke(result))
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "GetFormHandle"
    Private Delegate Function GetFormHandleDelegate(Form As Form) As IntPtr
    Private Function dGetFormHandle(Form As Form) As IntPtr
        Return Form.Handle
    End Function
    Friend Function GetFormHandle(Form As Form) As IntPtr
        Try
            Dim nI As New GetFormHandleDelegate(AddressOf dGetFormHandle)
            Dim result As IAsyncResult = Form.BeginInvoke(nI, {Form})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As IntPtr = CType(Form.EndInvoke(result), IntPtr)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return IntPtr.Zero
        End Try
    End Function
#End Region
#Region "ShowWindow"
    Private Delegate Sub ShowWindowDelegate(form As Form, flags As ShowWindowCommands)
    Private Sub dShowWindowDelegate(form As Form, flags As ShowWindowCommands)
        NativeMethods.ShowWindow(form.Handle, flags)
        If flags = ShowWindowCommands.SW_HIDE Then
            form.Visible = False
        ElseIf flags = ShowWindowCommands.SW_SHOW Then
            form.Visible = True
        End If
    End Sub
    Friend Sub ShowWindow(form As Form, flags As ShowWindowCommands)
        Try
            Dim nI As New ShowWindowDelegate(AddressOf dShowWindowDelegate)
            Dim result As IAsyncResult = form.BeginInvoke(nI, {form, flags})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            form.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "ShowTopmostFormWithoutFocus"
    Friend Sub ShowTopmostWithoutFocus(form As Form)
        Try
            NativeMethods.ShowWindow(form.Handle, ShowWindowCommands.SW_SHOWNA)
            SetWindowPos(form.Handle, ShowWindowHandles.HWND_TOPMOST, form.Left, form.Top, form.Width, form.Height, SetWindowPosFlags.SWP_NOACTIVATE)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Set Topmost/Not Topmost"
    Friend Sub SetFormTopMost(form As Form, topMost As Boolean)
        Try
            If topMost Then
                SetWindowPos(form.Handle, ShowWindowHandles.HWND_TOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE Or SetWindowPosFlags.SWP_NOMOVE)
            Else
                SetWindowPos(form.Handle, ShowWindowHandles.HWND_NOTOPMOST, 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE Or SetWindowPosFlags.SWP_NOMOVE)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Function IsFormTopMost(form As Form) As Boolean
        Try
            Return CBool(NativeMethods.GetForegroundWindow = form.Handle)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "ShowFormActivated"
    Friend Sub ShowFormActivated(form As Form)
        Try
            ShowWindow(form, ShowWindowCommands.SW_SHOW)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        'NativeMethods.ShowWindow(form.Handle, ShowWindowCommands.SW_SHOW)
    End Sub
#End Region
#Region "ActivateForm"
    Friend Function ActivateForm(form As Form) As Boolean
        Return NativeMethods.SetForegroundWindow(form.Handle)
    End Function
#End Region
#Region "SetFormTop"
    Friend Sub SetFormTop(form As Form)
        SetWindowPos(form.Handle, ShowWindowHandles.HWND_TOP, form.Left, form.Top, form.Width, form.Height, SetWindowPosFlags.SWP_SHOWWINDOW)
    End Sub
#End Region
#Region "Animate forms"
    Friend Sub ShowFade(form As Form, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, AnimateWindowFlags.AW_BLEND)
        ShowWindow(form, ShowWindowCommands.SW_SHOW)
    End Sub
    Friend Sub HideFade(form As Form, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, CType(AnimateWindowFlags.AW_BLEND Or AnimateWindowFlags.AW_HIDE, AnimateWindowFlags))
        ShowWindow(form, ShowWindowCommands.SW_HIDE)
    End Sub
    Friend Sub ShowExpand(form As Form, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, AnimateWindowFlags.AW_CENTER)
        ShowWindow(form, ShowWindowCommands.SW_SHOW)
    End Sub
    Friend Sub HideCollaps(form As Form, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, CType(AnimateWindowFlags.AW_CENTER Or AnimateWindowFlags.AW_HIDE, AnimateWindowFlags))
        ShowWindow(form, ShowWindowCommands.SW_HIDE)
    End Sub
    Friend Sub ShowAnimated(form As Form, [flags] As AnimateWindowFlags, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, flags)
        ShowWindow(form, ShowWindowCommands.SW_SHOW)
    End Sub
    Friend Sub HideAnimated(form As Form, [flags] As AnimateWindowFlags, Optional duration As Integer = 50)
        AnimateWindow(form.Handle, duration, flags)
        ShowWindow(form, ShowWindowCommands.SW_HIDE)
    End Sub
#End Region
#Region "SetWindowPosition"
    Friend Function SetWindowPosition(form As Form, x As Integer, y As Integer, width As Integer, height As Integer, [flags] As SetWindowPosFlags, position As IntPtr) As Boolean
        Return NativeMethods.SetWindowPos(form.Handle, position, x, y, width, height, flags)
    End Function
#End Region
#Region "Enable/Disable form"
    Friend Function EnableForm(form As Form, enabled As Boolean) As Boolean
        Return NativeMethods.EnableWindow(form.Handle, enabled)
    End Function
    Friend Function EnableForm(handle As IntPtr, enabled As Boolean) As Boolean
        Return NativeMethods.EnableWindow(handle, enabled)
    End Function
#End Region
#Region "Change window styles"
    Friend Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As IntPtr) As IntPtr
        If IntPtr.Size = 4 Then
            Return NativeMethods.SetWindowLongPtr32(hWnd, CType(nIndex, WindowLongFlags), dwNewLong)
        Else
            Return NativeMethods.SetWindowLongPtr64(hWnd, CType(nIndex, WindowLongFlags), dwNewLong)
        End If
    End Function
#End Region
#Region "Which form should recieve messages?"
    Friend Enum MessageFormEnum
        None
        History
        Live
    End Enum
    Friend ReadOnly Property MessageForm As MessageFormEnum
        Get
            Try
                If IsFormVisible(Live) And IsFormFocused(Live) Then
                    Return MessageFormEnum.Live
                ElseIf IsFormVisible(History) And IsFormFocused(History) Then
                    Return MessageFormEnum.History
                Else
                    Return MessageFormEnum.None
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return MessageFormEnum.None
            End Try
        End Get
    End Property
#End Region
#Region "StatusStripVisible | can replace eoc and message strip functions with this"
    Private Delegate Function IsStatusStripVisibleDelegate(sStrip As StatusStrip) As Boolean
    Private Function dIsStatusStripVisible(sStrip As StatusStrip) As Boolean
        Return sStrip.Visible
    End Function
    Private Delegate Sub SetStatusStripVisibleDelegate(sStrip As StatusStrip, Visible As Boolean)
    Private Sub dSetStatusStripVisible(sStrip As StatusStrip, Visible As Boolean)
        sStrip.Visible = Visible
    End Sub
    Friend Property StatusStripVisible(sStrip As StatusStrip) As Boolean
        Get
            Try
                If Not sStrip.IsHandleCreated Then Return False
                Dim nI As New IsStatusStripVisibleDelegate(AddressOf dIsStatusStripVisible)
                Dim result As IAsyncResult = sStrip.BeginInvoke(nI, sStrip)
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                Dim bRet As Boolean = CBool(sStrip.EndInvoke(result))
                result.AsyncWaitHandle.Close()
                Return bRet
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
        Set(value As Boolean)
            Try
                If Not sStrip.IsHandleCreated Then
                    If value = True Then sStrip.Show()
                Else
                    Dim nI As New SetStatusStripVisibleDelegate(AddressOf dSetStatusStripVisible)
                    Dim result As IAsyncResult = sStrip.BeginInvoke(nI, {sStrip, value})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    sStrip.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Set
    End Property
#End Region
#Region "MessageStrip updates"
#Region "MessageStripTimerFactory"
    Friend Class MessageStripTimerFactory
        Implements IDisposable
        Public Event timerElapsed(sender As Object, e As MyEventArgs.MessageTimerElapsedArgs)
        Private sStrip As StatusStrip
        Private WithEvents tMessage As New System.Windows.Forms.Timer
        Private Sub tMessage_Tick(sender As Object, e As System.EventArgs) Handles tMessage.Tick
            tMessage.Enabled = False
            RaiseEvent timerElapsed(Me, New MyEventArgs.MessageTimerElapsedArgs(sStrip))
        End Sub
        Friend Sub New(MessageStrip As StatusStrip, Optional Interval As Integer = 10000)
            sStrip = MessageStrip
            tMessage = New System.Windows.Forms.Timer
            tMessage.Interval = Interval
            tMessage.Enabled = True
        End Sub
        Friend Sub ResetMe(Optional Interval As Integer = 10000)
            tMessage.Interval = Interval
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls
        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    tMessage.Dispose()
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
    Friend dActiveTimers As New Dictionary(Of StatusStrip, MessageStripTimerFactory)
    Private Delegate Sub ClearStatusDelegate(MessageStrip As StatusStrip)
    Private Sub dClearStatus(MessageStrip As StatusStrip)
        MessageStrip.Items(0).Text = ""
    End Sub
    Friend Sub ClearMessageStrip(MessageStrip As StatusStrip)
        Try
            If Not MessageStrip.IsHandleCreated Then Exit Sub
            Dim nI As New ClearStatusDelegate(AddressOf dClearStatus)
            Dim result As IAsyncResult = MessageStrip.BeginInvoke(nI, MessageStrip)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            MessageStrip.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub MessageStripFactory_TimerElapsed(sender As Object, e As MyEventArgs.MessageTimerElapsedArgs)
        Try
            RemoveHandler CType(sender, MessageStripTimerFactory).timerElapsed, AddressOf MessageStripFactory_TimerElapsed
            WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay) & " -- MessageStripFactory_TimerElapsed: " & e.MessageStrip.FindForm.Name)
            ClearMessageStrip(e.MessageStrip)
            If modMySettings.HideInactiveMessageStrip Then SetMessageStripVisibility(e.MessageStrip.FindForm, False)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            dActiveTimers.Remove(e.MessageStrip)
            CType(sender, MessageStripTimerFactory).Dispose()
        End Try
    End Sub
#End Region
#Region "IsMessageStripClear"
    Private Delegate Function IsMessageStripClearDelegate(form As Form) As Boolean
    Private Function dIsMessageStripClear(Form As Form) As Boolean
        Try
            If ReferenceEquals(Form, Live) Then
                Return Live.sStripMessage.Text = ""
            Else
                Return History.sStripMessage.Text = ""
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function IsMessageStripClear(Form As Form) As Boolean
        Try
            If Not Form.IsHandleCreated Then Return True
            Dim nI As New IsMessageStripClearDelegate(AddressOf dIsMessageStripClear)
            Dim result As IAsyncResult
            If ReferenceEquals(Form, Live) Then
                result = Live.BeginInvoke(nI, {Form})
            Else
                result = History.BeginInvoke(nI, {Form})
            End If
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRet As Boolean
            If ReferenceEquals(Form, Live) Then
                bRet = CBool(Live.EndInvoke(result))
            Else
                bRet = CBool(History.EndInvoke(result))
            End If
            result.AsyncWaitHandle.Close()
            Return bRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "SetMessageStripVisibility"
    Private Delegate Sub SetMessageStripVisibilityDelegate(Visible As Boolean, MessageStrip As StatusStrip)
    Private Sub dSetMessageStripVisibility(Visible As Boolean, MessageStrip As StatusStrip)
        MessageStrip.Visible = Visible
    End Sub
    Friend Sub SetMessageStripVisibility(form As Form, visible As Boolean)
        Try
            If Not form.IsHandleCreated Then Exit Sub
            Dim sStripMessage As StatusStrip
            If ReferenceEquals(form, Live) Then
                sStripMessage = Live.sStripMessage
            Else
                sStripMessage = History.sStripMessage
            End If
            If Not sStripMessage.IsHandleCreated Then
                sStripMessage.Show()
            End If
            Dim nI As New SetMessageStripVisibilityDelegate(AddressOf dSetMessageStripVisibility)
            WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetMessageStripVisibility: " & form.Name)
            Dim result As IAsyncResult = sStripMessage.BeginInvoke(nI, {visible, sStripMessage})
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetMessageStripVisibility: " & form.Name)
            sStripMessage.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Exit Sub
        End Try
    End Sub
#End Region
#Region "GetMessageStripVisibility"
    Private Delegate Function IsMessageStripVisibleDelegate(MessageStrip As StatusStrip) As Boolean
    Private Function dIsMessageStripVisible(MessageStrip As StatusStrip) As Boolean
        Return MessageStrip.Visible
    End Function
    Friend ReadOnly Property IsMessageStipVisible(form As Form) As Boolean
        Get
            Try
                If Not form.IsHandleCreated Then Return False
                Dim mStrip As StatusStrip = New StatusStrip
                If ReferenceEquals(form, Live) Then
                    mStrip = Live.sStripMessage
                ElseIf ReferenceEquals(form, History) Then
                    mStrip = History.sStripMessage
                End If
                If Not mStrip.IsHandleCreated Then Return False
                Dim nI As New IsMessageStripVisibleDelegate(AddressOf dIsMessageStripVisible)
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dIsMessageStripVisible: " & form.Name)
                Dim result As IAsyncResult = form.BeginInvoke(nI, mStrip)
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dIsMessageStripVisible: " & form.Name)
                Dim bRet As Boolean = CBool(form.EndInvoke(result))
                result.AsyncWaitHandle.Close()
                Return bRet
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
#End Region
#Region "SetMessage"
    Private Delegate Sub SetMessageDelegate(Message As String, MessageStrip As StatusStrip)
    Private Sub dSetMessage(Message As String, MessageStrip As StatusStrip)
        MessageStrip.Items(0).Text = Message
    End Sub
    Friend Sub SetMessage(Message As String)
        Try
            If (Not IsFormVisible(History) And Not IsFormVisible(Live)) Then
                WriteLog("MESSAGE: " & Message)
                Return
            End If
            'Run message updates on both form's if visible 
            'If modMySettings.MainForm = modMySettings.eMainForm.History Then
            If IsFormVisible(History) Then
                If Not IsMessageStipVisible(History) Then SetMessageStripVisibility(History, True)
                Try
                    Dim nI As New SetMessageDelegate(AddressOf dSetMessage)
                    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke History.dSetMessage")
                    Dim result As IAsyncResult = History.BeginInvoke(nI, {Message, History.sStripMessage})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke History.dSetMessage")
                    History.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                If Not dActiveTimers.ContainsKey(History.sStripMessage) Then
                    Dim hideTimer As New MessageStripTimerFactory(History.sStripMessage)
                    dActiveTimers.Add(History.sStripMessage, hideTimer)
                    AddHandler hideTimer.timerElapsed, AddressOf MessageStripFactory_TimerElapsed
                Else
                    dActiveTimers(History.sStripMessage).ResetMe()
                End If
            End If
            'Else
            If IsFormVisible(Live) Then
                If Not IsMessageStipVisible(Live) Then SetMessageStripVisibility(Live, True)
                Try
                    Dim nI As New SetMessageDelegate(AddressOf dSetMessage)
                    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke Live.dSetMessage")
                    Dim result As IAsyncResult = Live.BeginInvoke(nI, {Message, Live.sStripMessage})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke Live.dSetMessage")
                    Live.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- AsyncWaitHandle.closed")
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                If Not dActiveTimers.ContainsKey(Live.sStripMessage) Then
                    Dim hideTimer As New MessageStripTimerFactory(Live.sStripMessage)
                    dActiveTimers.Add(Live.sStripMessage, hideTimer)
                    AddHandler hideTimer.timerElapsed, AddressOf MessageStripFactory_TimerElapsed
                Else
                    dActiveTimers(Live.sStripMessage).ResetMe()
                End If
            End If
            'End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#End Region
#Region "Crossed  messages"
#Region "Logparser"
    Friend Event ParserFailed(sender As Object, e As MyEventArgs.ParserFailedEventArgs)
    Friend Sub RaiseParserFailed(sender As Object, e As MyEventArgs.ParserFailedEventArgs)
        RaiseEvent ParserFailed(sender, e)
    End Sub
    Friend Event ParserCompleted(sender As Object, e As MyEventArgs.ParserCompletedEventArgs)
    Friend Sub RaiseParserCompleted(sender As Object, e As MyEventArgs.ParserCompletedEventArgs)
        RaiseEvent ParserCompleted(sender, e)
    End Sub
#End Region
#Region "EOC"
    Friend Event EOC_EnabledChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
    Friend Sub RaiseEOCEnabledChanged(Enabled As Boolean, Optional sender As Object = Nothing)
        RaiseEvent EOC_EnabledChanged(sender, New MyEventArgs.EOCEnabledChangedArgs(Enabled))
    End Sub
    Friend Event EOC_ViewTeamChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
    Friend Sub RaiseEOCViewUserChanged(Enabled As Boolean, Optional sender As Object = Nothing)
        RaiseEvent EOC_ViewUserChanged(sender, New MyEventArgs.EOCEnabledChangedArgs(Enabled))
    End Sub
    Friend Event EOC_ViewUserChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
    Friend Sub RaiseEOCViewTeamChanged(Enabled As Boolean, Optional sender As Object = Nothing)
        RaiseEvent EOC_ViewTeamChanged(sender, New MyEventArgs.EOCEnabledChangedArgs(Enabled))
    End Sub
    Friend Event EOC_UpdateRecieved(sender As Object, e As MyEventArgs.EocUpdateArgs)
    Friend Sub RaiseEOCUpdateRecieved(e As MyEventArgs.EocUpdateArgs, Optional sender As Object = Nothing)
        RaiseEvent EOC_UpdateRecieved(sender, e)
    End Sub
#End Region
#Region "Default statistics"
    Friend Event DefaultStatisticsChanged(sender As Object, e As MyEventArgs.DefaultStatisticsArgs)
    Friend Sub RaiseDefaultStatisticsChanged(DefaultStatistics As modMySettings.defaultStatisticsEnum, Optional sender As Object = Nothing)
        RaiseEvent DefaultStatisticsChanged(sender, New MyEventArgs.DefaultStatisticsArgs(DefaultStatistics))
    End Sub
#End Region
#Region "Message strip"
    Friend Event HideInactiveMessageStripChanged(Sender As Object, e As MyEventArgs.HideInactiveMessageStripArgs)
    Friend Sub RaiseHideInactiveMessageStripChanged(Hide As Boolean, Optional sender As Object = Nothing)
        RaiseEvent HideInactiveMessageStripChanged(sender, New MyEventArgs.HideInactiveMessageStripArgs(Hide))
    End Sub
#End Region
#Region "Live eta style"
    Friend Event Live_EtaStyleChanged(sender As Object, e As MyEventArgs.EtaStyleArgs)
    Friend Sub RaiseEtaStyleChanged(NewStyle As modMySettings.eEtaStyle, Optional sender As Object = Nothing)
        RaiseEvent Live_EtaStyleChanged(sender, New MyEventArgs.EtaStyleArgs(NewStyle))
    End Sub
#End Region
#Region "Live minimized"
    Friend Event LiveMinimized(sender As Object, e As EventArgs)
    Friend Event LiveRestored(sender As Object, e As EventArgs)
    Friend Sub RaiseLiveMinimized(Optional sender As Object = Nothing)
        RaiseEvent LiveMinimized(sender, New MyEventArgs.EmptyArgs)
    End Sub
    Friend Sub RaiseLiveRestored(Optional sender As Object = Nothing)
        RaiseEvent LiveRestored(sender, New MyEventArgs.EmptyArgs)
    End Sub
#End Region
#Region "Convert from UTC"
    Friend Event ConvertFromUtcChanged(sender As Object, e As MyEventArgs.ConvertFromUtcArgs)
    Friend Sub RaiseConvertFromUtcChanged(Convert As Boolean, Optional sender As Object = Nothing)
        RaiseEvent ConvertFromUtcChanged(sender, New MyEventArgs.ConvertFromUtcArgs(Convert))
    End Sub
#End Region
#Region "ParseOnInterval"
    Friend Event ParseOnIntervalChanged(sender As Object, e As MyEventArgs.ParseOnIntervalArgs)
    Friend Sub RaiseParseOnIntervalChanged(Enabled As Boolean, Optional sender As Object = Nothing)
        RaiseEvent ParseOnIntervalChanged(sender, New MyEventArgs.ParseOnIntervalArgs(Enabled))
    End Sub
    Friend Event ParseOnIntervalValueChanged(sender As Object, e As MyEventArgs.ParseOnIntervalValueArgs)
    Friend Sub RaiseParseOnIntervalValueChanged(interval As TimeSpan, Optional sender As Object = Nothing)
        RaiseEvent ParseOnIntervalValueChanged(sender, New MyEventArgs.ParseOnIntervalValueArgs(interval))
    End Sub
#End Region
#End Region
#Region "Bussy box"
    Public Class BussyBox
        Implements IDisposable
#Region "Declarations"
        Private Shared mBussy As frmBussy
        Private Shared bRestore As Boolean = True
        Private Shared Property FocusedForm As Form
        Private Structure sStoredState
            Property Enabled As Boolean
            Property WindowState As FormWindowState
            Property ShowInTaskBar As Boolean
        End Structure
        Private Shared dStoredState As New Dictionary(Of Form, sStoredState)
        Private Shared mOwnerHandle As IntPtr
        Private Shared mOwnerRect As Rectangle
        Private Shared mInterlock As ManualResetEvent
        Private Shared IsAsDialog As Boolean = False
        Private Shared mBussyMessage As String = Nothing
        Private Shared pBar As ProgressBar
        Private Shared iMin As Int32 = -1
        Private Shared iMax As Int32 = -1
#End Region
#Region "Form handling"
        Friend Shared Function IsFormVisible() As Boolean
            Try
                If Not IsNothing(mBussy) AndAlso mBussy.IsHandleCreated Then
                    Return delegateFactory.IsFormVisible(mBussy)
                Else
                    Return False
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Friend Shared Sub CloseForm()
            Try
                If IsNothing(mBussy) Then
                    Exit Sub
                Else
                    If Not mBussy.IsHandleCreated Then
                        Exit Sub
                    End If
                End If
                mBussy.ballowclose = True
                WriteLog("Closing Bussybox: " & delegateFactory.CloseForm(mBussy).ToString)
                If dStoredState.Count > 0 Then
                    For Each Frm As Form In Application.OpenForms
                        If dStoredState.ContainsKey(Frm) Then
                            WriteLog("BussyBox::Restoring:" & Frm.Name & "_WindowState :" & delegateFactory.SetFormWindowState(Frm, dStoredState(Frm).WindowState).ToString)
                            WriteLog("BussyBox::Restoring:" & Frm.Name & "_Enabled: " & delegateFactory.SetFormEnabled(Frm, dStoredState(Frm).Enabled).ToString)
                            WriteLog("BussyBox::Restoring:" & Frm.Name & "_showInTaskbar: " & SetFormShowInTaskbar(Frm, dStoredState(Frm).ShowInTaskBar).ToString)
                        End If
                    Next
                    If Not IsNothing(FocusedForm) Then
                        WriteLog("Activating form " & FocusedForm.Name & ": " & delegateFactory.ActivateForm(FocusedForm).ToString)
                    End If
                    dStoredState.Clear()
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub ShowForm(Optional Message As String = "... please wait", Optional Marquee As Boolean = False, Optional Owner As Form = Nothing, Optional DisableForms As Boolean = True, Optional MinValue As Int32 = -1, Optional maxValue As Int32 = -1)
            Try
                Dim bSet As Boolean = False
                If IsNothing(mBussy) Then
                    bSet = True
                Else
                    If mBussy.IsDisposed OrElse mBussy.Disposing Then
                        bSet = True
                    End If
                End If
                If bSet Then
                    '' 
                    If Not IsNothing(Owner) Then
                        Debug.Assert(Owner.Created)
                        mOwnerHandle = Owner.Handle
                        mOwnerRect = Owner.Bounds
                        IsAsDialog = True
                    Else
                        IsAsDialog = False
                    End If
                    '' 
                    bRestore = DisableForms
                    If DisableForms Then
                        dStoredState.Clear()
                        For Each frm As Form In Application.OpenForms
                            If delegateFactory.IsFormFocused(frm) Then FocusedForm = frm
                            If Not ReferenceEquals(frm, Logwindow.Form) Then
                                Dim nState As New sStoredState
                                nState.Enabled = delegateFactory.IsFormEnabled(frm)
                                nState.WindowState = delegateFactory.GetFormWindowState(frm)
                                nState.ShowInTaskBar = delegateFactory.GetShowInTaskbar(frm)
                                dStoredState.Add(frm, nState)
                                If nState.Enabled Then WriteLog("BussyBox::SetFormDisabled: " & delegateFactory.SetFormEnabled(frm, False).ToString)
                            End If
                        Next
                    End If
                    Dim pBar As ProgressBar = Nothing
                    If Marquee OrElse (Not MinValue = -1 AndAlso Not maxValue = -1) Then
                        pBar = New ProgressBar
                        If (Not MinValue = -1 AndAlso Not maxValue = -1) Then
                            pBar.Style = ProgressBarStyle.Continuous
                            pBar.Minimum = MinValue
                            pBar.Maximum = maxValue
                        Else
                            pBar.Style = ProgressBarStyle.Marquee
                        End If
                    End If
                    ''
                    mInterlock = New ManualResetEvent(False)
                    Dim t As Thread = New Thread(AddressOf dlgStart)
                    t.SetApartmentState(ApartmentState.STA)
                    Dim bStart As New bussyStartOptions
                    bStart.Pbar = pBar
                    bStart.Message = Message
                    t.Start(bStart)
                    mInterlock.WaitOne()
                End If
                'BussyBox.SetMessage(Message.ToString)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
#Region "Set BussyBox message"
        Private Delegate Sub SetMessageDelegate(Message As String, Progress As Int32)
        Private Shared Sub dSetMessage(Message As String, Progress As Int32)
            Try
                mBussy.lblMessage.Text = Message
                If Not Progress = -1 Then
                    mBussy.pbProgress.Value = Progress
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub SetMessage(Message As String, Optional Progress As Int32 = -1)
            Try
                If IsNothing(mBussy) OrElse Not mBussy.IsHandleCreated Then
                    WriteLog("Attempt to set BussyBox message while BussyBox has not created a form", eSeverity.Critical)
                    WriteLog("Original message := " & Message, eSeverity.Critical)
                Else
                    Dim nI As New SetMessageDelegate(AddressOf dSetMessage)
                    Dim result As IAsyncResult = mBussy.BeginInvoke(nI, {Message, Progress})
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    mBussy.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
#Region "Set new min/max values for progressbar"
        Private Delegate Sub SetNewBoundriesDelegate(Min As Int32, Max As Int32, Current As Int32, Style As ProgressBarStyle)
        Private Shared Sub dSetNewBoundries(Min As Int32, Max As Int32, Current As Int32, Style As ProgressBarStyle)
            Try
                If Not Min = -1 AndAlso Not Max = -1 Then
                    mBussy.pbProgress.Minimum = Min : mBussy.pbProgress.Maximum = Max : mBussy.pbProgress.Value = Current : mBussy.pbProgress.Style = Style
                    If mBussy.scPbar.Panel2Collapsed Then mBussy.scPbar.Panel2Collapsed = False
                Else
                    mBussy.scPbar.Panel2Collapsed = True
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub SetNewProgressBoundries(Min As Int32, Max As Int32, Current As Int32, Style As ProgressBarStyle)
            Try
                Dim nI As New SetNewBoundriesDelegate(AddressOf dSetNewBoundries)
                Dim result As IAsyncResult = mBussy.BeginInvoke(nI, {Min, Max, Current, Style})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                mBussy.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
#Region "Set BussyBox progress"
        Private Delegate Sub SetProgressDelegate(Progress As Int32)
        Private Shared Sub dSetProgress(Progress As Int32)
            Try
                mBussy.pbProgress.Value = Progress
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared Sub SetProgress(Progress As Int32)
            Try
                If IsNothing(mBussy) OrElse Not mBussy.IsHandleCreated Then
                    WriteLog("Attempt to set BussyBox message while BussyBox has not created a form", eSeverity.Critical)
                Else
                    Dim nI As New SetProgressDelegate(AddressOf dSetProgress)
                    Dim result As IAsyncResult = mBussy.BeginInvoke(nI, Progress)
                    While Not result.IsCompleted
                        Application.DoEvents()
                    End While
                    mBussy.EndInvoke(result)
                    result.AsyncWaitHandle.Close()
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
#Region "BussyBox thread creation, updates, destruction"
        Private Structure bussyStartOptions
            Property Pbar As ProgressBar
            Property Message As String
        End Structure
        Private Shared Sub dlgStart(Start As Object)
            Try
                Dim bStart As bussyStartOptions = CType(Start, bussyStartOptions)
                mBussy = New frmBussy(bStart.Message, FormBorderStyle.FixedDialog, bStart.Pbar)
                AddHandler mBussy.Load, AddressOf bussyLoad
                AddHandler mBussy.FormClosing, AddressOf bussyClosing
                If IsAsDialog Then
                    mBussy.StartPosition = FormStartPosition.Manual
                    NativeMethods.EnableWindow(mOwnerHandle, False)
                    SetWindowLong(mBussy.Handle, -8, mOwnerHandle)
                End If
                Application.Run(mBussy)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Shared Sub bussyLoad(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If IsAsDialog Then
                    mBussy.Location = New Point(mOwnerRect.Left + (mOwnerRect.Width - mBussy.Width) \ 2, mOwnerRect.Top + (mOwnerRect.Height - mBussy.Height) \ 2)
                End If
                mInterlock.Set()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Shared Sub bussyClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
            Try
                If IsAsDialog Then NativeMethods.EnableWindow(mOwnerHandle, True)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Sub dlgClose()
            mBussy.Close()
            mBussy = Nothing
        End Sub
        Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As IntPtr) As IntPtr
            If IntPtr.Size = 4 Then
                Return NativeMethods.SetWindowLongPtr32(hWnd, CType(nIndex, NativeMethods.WindowLongFlags), dwNewLong)
            Else
                Return NativeMethods.SetWindowLongPtr64(hWnd, CType(nIndex, NativeMethods.WindowLongFlags), dwNewLong)
            End If
        End Function
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
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
#End Region
    End Class
#End Region
End Module
