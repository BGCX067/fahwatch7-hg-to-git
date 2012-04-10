Partial Friend Class frmBussy
    Friend ballowclose As Boolean = False
    Private Sub frmBussy_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Not ballowclose Then
                e.Cancel = True
                delegateFactory.SetFormWindowState(Me, FormWindowState.Minimized)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub New(Message As String, BorderStyle As FormBorderStyle, Optional pb As ProgressBar = Nothing)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.Text = My.Application.Info.AssemblyName & " " & My.Application.Info.Version.ToString
        Try
            lblMessage.Text = Message
            scPbar.Panel2Collapsed = IsNothing(pb)
            If Not IsNothing(pb) Then
                Me.pbProgress.Minimum = pb.Minimum
                Me.pbProgress.Maximum = pb.Maximum
                Me.pbProgress.Style = pb.Style
                Me.pbProgress.Step = pb.Step
                Me.pbProgress.MarqueeAnimationSpeed = pb.MarqueeAnimationSpeed
            End If
            Me.FormBorderStyle = BorderStyle
            If BorderStyle = Windows.Forms.FormBorderStyle.None Then
                With Me
                    .ShowInTaskbar = False
                    .ShowIcon = False
                    .StartPosition = FormStartPosition.CenterParent
                    .MdiParent = MdiParent
                End With
            Else
                If IsNothing(MdiParent) Then
                    With Me
                        .StartPosition = FormStartPosition.CenterScreen
                        .ControlBox = True
                        .UseWaitCursor = True
                        .ShowIcon = True
                        .ShowInTaskbar = True
                    End With
                Else
                    With Me
                        .ShowInTaskbar = False
                        .ShowIcon = False
                        .StartPosition = FormStartPosition.CenterParent
                        .MdiParent = MdiParent
                        .TopMost = True
                    End With
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
End Class