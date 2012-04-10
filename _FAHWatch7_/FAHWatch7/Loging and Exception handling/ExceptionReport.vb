Imports FAHWatch7.Exceptions

'allow adding exceptions to 'ignore' list
'block execution if not on ignore list, edit calls to writeerror to accomplish execution lock
'don't add 'never report', the exception dialog can be disabled entirely in advanced settings
Public Class ExceptionReport
    Private mException As Exception, mErr As ErrObject, mMessage As String
    Private mDialogResult As DialogResult = Windows.Forms.DialogResult.None
    Friend Sub SetProperties(Message As String, ErrObj As ErrObject)
        mMessage = Message
        mException = ErrObj.GetException
        mErr = ErrObj
    End Sub
    Public Overloads ReadOnly Property DialogResult As DialogResult
        Get
            Return mDialogResult
        End Get
    End Property
    Private Sub chkSendException_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSendException.CheckedChanged
        Try
            If Exceptions.IsExceptionNonFatal Then
                chkContinue.Enabled = False
                chkContinue.Checked = True
                ToolTip1.SetToolTip(cmdOk, "Dismiss this exception and try to continue FAHWatch7")
            Else
                If chkSendException.Checked Then
                    cmdOk.Text = "Ok"
                    chkContinue.Enabled = True
                    ToolTip1.SetToolTip(cmdOk, "Ignore this exception and try to continue FAHWatch7")
                Else
                    cmdOk.Text = "Quit"
                    chkContinue.Enabled = False
                    ToolTip1.SetToolTip(cmdOk, "Quit FAHWatch7")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub ExceptionReport_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            rtException.Text = mMessage & New String(CChar(Environment.NewLine), 2) & New StoredExceptions.Exceptions(mException, mErr, False).ErrText
            If modMySettings.SendException Then
                chkSendException.Checked = True
                If Exceptions.IsExceptionNonFatal Then
                    ToolTip1.SetToolTip(cmdOk, "Dismiss this exception and try to continue FAHWatch7")
                    chkContinue.Checked = True
                    chkContinue.Enabled = False
                Else
                    chkContinue.Checked = True
                    chkContinue.Enabled = True
                    ToolTip1.SetToolTip(cmdOk, "Ignore this exception and try to continue FAHWatch7")
                End If
            Else
                chkSendException.Checked = False
                If Exceptions.IsExceptionNonFatal Then
                    chkContinue.Checked = True
                    chkContinue.Enabled = False
                    ToolTip1.SetToolTip(cmdOk, "Dismiss this exception and try to continue FAHWatch7")
                Else
                    chkContinue.Checked = False
                    chkContinue.Enabled = False
                    cmdOk.Text = "Quit"
                    ToolTip1.SetToolTip(cmdOk, "Quit FAHWatch7")
                End If
            End If
            cmdStoredExceptions.Enabled = StoredExceptions.Count > 0
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdOk_Click(sender As System.Object, e As System.EventArgs) Handles cmdOk.Click
        Try
            For Each ctrl As Control In Me.Controls
                ctrl.Enabled = False
            Next
            If chkSendException.Checked Then
                'Send and attempt to continue 
                If Not Mail.SendCrashReport(rtException.Text) Then
                    WriteLog("Failed to send an Exception report, smtp status: " & Mail.SMTPStatus, eSeverity.Important)
                    StoredExceptions.UpdateReportedStatus(mException, mErr, False)
                    MsgBox("Send mail failed, the smtp server returned: " & Mail.SMTPStatus & New String(CChar(Environment.NewLine), 2) & "Unsend exceptions will be retried at application startup, and cleared at the next upgrade", CType(MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, MsgBoxStyle), "Failed to send Exception report")
                Else
                    WriteLog("Succesfully send exception report for " & Exceptions.StoredExceptions.StoredExceptionIdentifier(mException, mErr))
                    StoredExceptions.UpdateReportedStatus(mException, mErr, True)
                End If
                If chkContinue.Checked Then
                    mDialogResult = Windows.Forms.DialogResult.OK
                Else
                    mDialogResult = Windows.Forms.DialogResult.Cancel
                End If
            Else
                StoredExceptions.UpdateReportedStatus(mException, mErr, False)
                mDialogResult = Windows.Forms.DialogResult.Cancel ' cancel = quit
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            Me.Close()
        End Try
    End Sub
    Private Sub cmdStoredExceptions_Click(sender As System.Object, e As System.EventArgs) Handles cmdStoredExceptions.Click
        Try
            Exceptions.StoredExceptions.ShowList()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub ExceptionReport_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If mDialogResult = Windows.Forms.DialogResult.None Then
            mDialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
End Class