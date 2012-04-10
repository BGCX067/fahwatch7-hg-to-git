Public Class frmLogViewer
    Private mSlots As New List(Of String)
    Private mProjects As New List(Of String)
    Private mRCGs As New List(Of String)
    Private mUnits As New List(Of String)
    Private mText As String = String.Empty
    Friend Property LogText As String
        Get
            Return mText
        End Get
        Set(value As String)
            mText = value
        End Set
    End Property

#Region "Copy to clipboard"
    Private Sub CopyToolStripButton_Click(sender As System.Object, e As System.EventArgs) Handles CopyToolStripButton.Click
        If rt.SelectedText.Length = 0 Then
            SetStatus("No text selected")
        Else
            Clipboard.SetText(rt.SelectedText)
            SetStatus("Copied selected text to clipboard")
        End If
    End Sub
#End Region
#Region "Status"
    Private Sub SetStatus(Message As String, Optional StartTimer As Boolean = True)
        tsLblStatus.Text = Message
        tStatus.Enabled = StartTimer
    End Sub
    Private Sub tStatus_Tick(sender As System.Object, e As System.EventArgs) Handles tStatus.Tick
        tsLblStatus.Text = ""
    End Sub
#End Region
#Region "Form events"
    Private Sub frmLogViewer_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            delegateFactory.ActivateForm(Me)
            Me.UseWaitCursor = True
            SetStatus("Initializing filters", False)

            Dim iLastIndex As Int32 = 0
            Do While mText.IndexOf("Recieved Unit:", iLastIndex) > -1

            Loop

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
End Class