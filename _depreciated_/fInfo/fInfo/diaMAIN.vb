Imports System.Windows.Forms

Public Class diaMAIN
    Public Enum eDiagSize
        Minimal
        Listbox
    End Enum
    Public Enum eDiagButtons
        CancelOK
        Cancel
        OK
        Dismiss
    End Enum
    Private _DiagButtons As eDiagButtons = eDiagButtons.Dismiss
    Public Event ButtonClick(ByVal Button As eCloseButton)

    Public Property Diagbuttons As eDiagButtons
        Get
            Return _DiagButtons
        End Get
        Set(ByVal value As eDiagButtons)
            _DiagButtons = value
            Select Case value
                Case Is = eDiagButtons.CancelOK
                    cmdCancel.Text = "Cancel"
                    cmdCancel.Visible = True
                    cmdOK.Text = "OK"
                    cmdOK.Visible = True
                Case Is = eDiagButtons.Cancel
                    cmdCancel.Text = "Cancel"
                    cmdCancel.Visible = True
                    cmdOK.Visible = False
                Case Is = eDiagButtons.Dismiss
                    cmdCancel.Visible = False
                    cmdOK.Text = "Dismiss"
                    cmdOK.Visible = True
                Case Is = eDiagButtons.OK
                    cmdCancel.Visible = False
                    cmdOK.Text = "Ok"
                    cmdOK.Visible = True
                Case Else
                    Throw New NotImplementedException
            End Select
        End Set
    End Property
    Public Property DiagSize As eDiagSize = eDiagSize.Minimal
    Private WithEvents tLoop As New System.Timers.Timer
    Public Enum eCloseButton
        Dismiss
        Ok
        Cancel
        None
    End Enum
    Private _bClose As Boolean = False
    Public CloseReason As eCloseButton = eCloseButton.None
    Private Sub diaMAIN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tLoop.Enabled = False
        tLoop.Interval = 10
        tLoop.AutoReset = True
        If DiagSize = eDiagSize.Minimal Then
            Me.BackgroundImage = My.Resources.Dialog : Me.Size = Me.BackgroundImage.Size
            pBar.Visible = True : pBar.Location = New Point(50, 100) : pBar.Size = New Size(200, 25)
            lblStatus.Visible = True : lblStatus.Location = New Point(25, 25)
            rtfStatus.Visible = False
        Else
            Me.BackgroundImage = My.Resources.diagListbox : Me.Size = Me.BackgroundImage.Size
            rtfStatus.Visible = True : rtfStatus.Location = New Point(13, 13) : rtfStatus.Size = New Size(373, 207)
            pBar.Visible = True : pBar.Location = New Point(13, 228) : pBar.Size = New Size(373, 25)
            lblStatus.Visible = False
        End If
    End Sub
    Public Sub UpdateStatus(ByVal StatusMessage As String, ByVal Progress As Int32)
        If DiagSize = eDiagSize.Listbox Then
            rtfStatus.AppendText(StatusMessage)
        Else
            lblStatus.Text = StatusMessage
        End If
        pBar.Value = Progress
        Application.DoEvents()
    End Sub

    Const WM_NCHITTEST As Integer = &H84
    Const HTCLIENT As Integer = &H1
    Const HTCAPTION As Integer = &H2
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_NCHITTEST
                MyBase.WndProc(m)
                If m.Result = HTCLIENT Then m.Result = HTCAPTION
                'If m.Result.ToInt32 = HTCLIENT Then m.Result = IntPtr.op_Explicit(HTCAPTION) 'Try this in VS.NET 2002/2003 if the latter line of code doesn't do it... thx to Suhas for the tip.
            Case Else
                'Make sure you pass unhandled messages back to the default message handler.
                MyBase.WndProc(m)
        End Select
    End Sub


    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If cmdOK.Text = "Ok" Then
            CloseReason = eCloseButton.Ok
        ElseIf cmdOK.Text = "Dismiss" Then
            CloseReason = eCloseButton.Dismiss
        End If
        RaiseEvent ButtonClick(CloseReason)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        CloseReason = eCloseButton.Cancel
        RaiseEvent ButtonClick(CloseReason)
    End Sub
End Class
