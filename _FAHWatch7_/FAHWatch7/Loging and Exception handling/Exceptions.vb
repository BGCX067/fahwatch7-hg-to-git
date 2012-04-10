Imports System.Text
Imports System.Threading
Imports System.Security.Cryptography
Friend Class Exceptions
#Region "Stored exceptions"
    Friend Class StoredExceptions
        Friend Overloads Shared ReadOnly Property StoredExceptionIdentifier(StoredException As StoredExceptions.Exceptions) As String
            Get
                Try
                    Return FormatSQLString(StoredException.ErrText, True)
                    'Return New Guid(StoredException.ErrText).ToString
                    'Dim strID As String = StoredException.Source & "#" & StoredException.ErrText
                    'Dim b() As Byte = New UnicodeEncoding().GetBytes(strID)
                    'Dim md5 As New MD5CryptoServiceProvider
                    'Dim bh() As Byte = md5.ComputeHash(b)
                    'Return Convert.ToBase64String(bh)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return String.Empty
                End Try
            End Get
        End Property
        Friend Overloads Shared ReadOnly Property StoredExceptionIdentifier(Ex As Exception, Err As ErrObject) As String
            Get
                Return StoredExceptionIdentifier(New Exceptions(Ex, Err))
            End Get
        End Property
#Region "Stored exceptions declarations"
        Friend Structure Exceptions
            Private mErrNumber As String
            Private mExceptionSource As String
            Private mExceptionMessage As String
            Private mInnerExceptionSource As String
            Private mInnerExceptionMessage As String
            Private mErrText As String
            Private mReported As Boolean
            Friend Property IsReported As Boolean
                Get
                    Return mReported
                End Get
                Set(value As Boolean)
                    mReported = value
                End Set
            End Property
            Friend ReadOnly Property ErrorNumber As String
                Get
                    Return mErrNumber
                End Get
            End Property
            Friend ReadOnly Property ErrText As String
                Get
                    Return mErrText
                End Get
            End Property
            Friend ReadOnly Property Source As String
                Get
                    Return mExceptionSource
                End Get
            End Property
            Friend ReadOnly Property InnerExceptionSource As String
                Get
                    Return mInnerExceptionSource
                End Get
            End Property
            Friend ReadOnly Property ExceptionMessage As String
                Get
                    Return mExceptionMessage
                End Get
            End Property
            Friend ReadOnly Property InnerExceptionMessage As String
                Get
                    Return mInnerExceptionMessage
                End Get
            End Property
            Friend Sub New(ExceptionSource As String, ExceptionMessage As String, ErrNumber As String, ErrText As String, IsReported As Boolean)
                Try
                    Me.mExceptionSource = ExceptionSource
                    Me.mExceptionMessage = ExceptionMessage
                    Me.mErrNumber = ErrNumber
                    Me.mErrText = ErrText
                    Me.mReported = IsReported
                Catch ex As Exception

                End Try
            End Sub
            Friend Sub New(Exception As Exception, ErrObj As ErrObject, Optional IsReported As Boolean = False)
                Try
                    Me.mErrText = GetErrorText(Exception, ErrObj)
                    Me.mExceptionSource = Exception.Source
                    Me.mExceptionMessage = Exception.Message
                    Me.mErrNumber = CStr(ErrObj.Number)
                    Me.mReported = IsReported
                Catch ex As Exception

                End Try
            End Sub
            Private Function GetErrorText(Ex As Exception, ErrObject As ErrObject) As String
                'On Error Resume Next
                Dim sb As New StringBuilder
                Try
                    sb.AppendLine("CRITICAL: " & Ex.Message)
                    sb.AppendLine("CRITICAL: " & " - Err.source: " & ErrObject.Source & " Line: " & ErrObject.Erl & " - Err.number : " & ErrObject.Number)
                    sb.AppendLine("CRITICAL:  - Err.description: " & ErrObject.Description)
                    sb.AppendLine("CRITICAL:  - stacktrace: " & Ex.StackTrace.ToString)
                    If Not IsNothing(Ex.InnerException) Then
                        sb.AppendLine("CRITICAL::InnerException: " & Ex.InnerException.Message)
                        sb.AppendLine("CRITICAL::InnerException - stacktrace: " & Ex.InnerException.StackTrace)
                    End If
                Catch exC As Exception

                End Try
                Return sb.ToString
            End Function
        End Structure
        Private Shared dIgnore As New Dictionary(Of String, Exceptions)
#End Region
#Region "Stored exceptions functions"
        Friend Shared Sub Init()
            dIgnore = sqdata.GetStoredExceptions
        End Sub
        Friend Shared ReadOnly Property dExceptions As Dictionary(Of String, Exceptions)
            Get
                Return dIgnore
            End Get
        End Property
        Friend Shared ReadOnly Property ShouldIgnore(ex As Exception, Err As ErrObject) As Boolean
            Get
                Return dIgnore.ContainsKey(FormatSQLString(StoredExceptionIdentifier(ex, Err), True))
            End Get
        End Property
        Friend Overloads Shared Sub UpdateReportedStatus(Ex As Exception, Err As ErrObject, Reported As Boolean)
            Try
                Dim nException As New StoredExceptions.Exceptions(Ex, Err, Reported)
                sqdata.AddOrUpdateException(nException)
                dIgnore = sqdata.GetStoredExceptions
            Catch exC As Exception
                WriteError(exC.Message, Err)
            End Try
        End Sub
        Friend Overloads Shared Sub UpdateReportedStatus(Exception As StoredExceptions.Exceptions, Reported As Boolean)
            Try
                Exception.IsReported = Reported
                sqdata.AddOrUpdateException(Exception)
                dIgnore = sqdata.GetStoredExceptions
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Shared ReadOnly Property Count As Int32
            Get
                Return dIgnore.Count
            End Get
        End Property
        Friend Shared Sub RemoveExceptionFromList(StoredException As StoredExceptions.Exceptions)
            Try
                sqdata.RemoveException(StoredException)
                dIgnore = sqdata.GetStoredExceptions
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
#Region "Exceptions ignore list declarations"
        Private Shared mStoredList As diagExceptionIgnoreList
#End Region
#Region "Exceptions ignore list dialog"
        Friend Shared Sub ShowList()
            Try
                If IsNothing(mStoredList) OrElse mStoredList.IsDisposed OrElse mStoredList.Disposing Then
                    mStoredList = New diagExceptionIgnoreList
                End If
                mStoredList.ShowDialog()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            Finally
                mStoredList.Dispose()
            End Try
        End Sub
#End Region
    End Class
#End Region
#Region "Exception dialog declarations"
    Private Structure ExceptionReportProperties
        Property Message As String
        Property ErrObj As ErrObject
    End Structure
    Private Shared mInterlock As ManualResetEvent
    Private Shared mDialog As ExceptionReport
    Private Shared mDiagResult As DialogResult
    Private Shared mNonFatal As Boolean = False
    Private Shared bRestore As Boolean = True
    Private Shared Property FocusedForm As Form
    Private Structure sStoredState
        Property Enabled As Boolean
        Property WindowState As FormWindowState
        Property ShowInTaskBar As Boolean
    End Structure
    Private Shared dStoredState As New Dictionary(Of Form, sStoredState)
#End Region
#Region "Exception dialog handling"
    Private Shared mIsActive As Boolean = False
    Public Shared ReadOnly Property IsReporting As Boolean
        Get
            Return mIsActive
        End Get
    End Property
    Friend Shared Sub ShowReport(Message As String, TheErr As ErrObject)
        Try
            mIsActive = True
            'Fill store state
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

            mDialog = New ExceptionReport
            AddHandler mDialog.FormClosing, AddressOf DiagClose
            mDialog.SetProperties(Message, TheErr)
            Dim dRes As DialogResult = mDialog.ShowDialog()


            'Call DiagClose(Message, New Windows.Forms.FormClosingEventArgs(CloseReason.UserClosing, False))

            'mInterlock = New ManualResetEvent(False)
            'Dim t As Thread = New Thread(AddressOf dlgStart)
            'Dim objStart As New ExceptionReportProperties
            'objStart.Exception = TheException
            'objStart.ErrObj = TheErr
            't.SetApartmentState(ApartmentState.STA)
            't.Start(objStart)
            'mInterlock.WaitOne(System.Threading.Timeout.Infinite, False)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub dlgStart(Start As Object)
        Try
            mDialog = New ExceptionReport
            mDialog.SetProperties(CType(Start, ExceptionReportProperties).Message, CType(Start, ExceptionReportProperties).ErrObj)
            AddHandler mDialog.Load, AddressOf DialogLoad
            AddHandler mDialog.FormClosing, AddressOf DiagClose
            Application.Run(mDialog)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub DialogLoad(sender As Object, e As EventArgs)
        mInterlock.Set()
    End Sub
    Private Shared Sub DiagClose(sender As Object, e As FormClosingEventArgs)
        Try
            If dStoredState.Count > 0 Then
                For Each Frm As Form In Application.OpenForms
                    If dStoredState.ContainsKey(Frm) Then
                        WriteLog("ExceptionReport::Restoring:" & Frm.Name & "_WindowState :" & delegateFactory.SetFormWindowState(Frm, dStoredState(Frm).WindowState).ToString)
                        WriteLog("ExceptionReport::Restoring:" & Frm.Name & "_Enabled: " & delegateFactory.SetFormEnabled(Frm, dStoredState(Frm).Enabled).ToString)
                        WriteLog("ExceptionReport::Restoring:" & Frm.Name & "_showInTaskbar: " & SetFormShowInTaskbar(Frm, dStoredState(Frm).ShowInTaskBar).ToString)
                    End If
                Next
                If Not IsNothing(FocusedForm) Then
                    WriteLog("Activating form " & FocusedForm.Name & ": " & delegateFactory.ActivateForm(FocusedForm).ToString)
                End If
                dStoredState.Clear()
            End If
            mDiagResult = mDialog.DialogResult
            mDialog.Dispose()
            If mDiagResult = DialogResult.None Or mDiagResult = DialogResult.Cancel Then
                WriteLog("Setting flag to indicate the database backup has to be retained untill next application start")
                bKeepBackup = True 'keep database backup
                If Not mNonFatal Then
                    WriteLog("Exception threated as fatal, exiting")
                    ExitApplication(True)
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            mIsActive = False
        End Try
    End Sub
#End Region

    Friend Shared Sub Init()
        StoredExceptions.Init()
    End Sub
    Friend Shared ReadOnly Property ShouldIgnore(Ex As Exception, Err As ErrObject) As Boolean
        Get
            Return StoredExceptions.ShouldIgnore(Ex, Err)
        End Get
    End Property
    Public Shared WriteOnly Property SetExceptionIsNonFatal As Boolean
        Set(value As Boolean)
            mNonFatal = value
        End Set
    End Property
    Public Shared ReadOnly Property IsExceptionNonFatal As Boolean
        Get
            Return mNonFatal
        End Get
    End Property
    
End Class

