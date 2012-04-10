Public Class frmPBStatus
    Private Delegate Sub _SetMessage(ByVal Message As String)
    Private Delegate Sub _SetPBvalue(ByVal Value As Int16)
    Private Delegate Sub _SetPBMax(ByVal Value As Int16)
    Public Sub SetPBMax(ByVal Value As Int16)
        Try
            If pBar.InvokeRequired Then
                Dim pMax As New _SetPBMax(AddressOf _PBmax)
                pBar.Invoke(pMax, New Object() {Value})
            Else
                pBar.Maximum = Value
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub _PBmax(ByVal Value As Int16)
        Try
            pBar.Maximum = Value
        Catch ex As Exception

        End Try
    End Sub
    Public Sub SetMessage(ByVal Message As String)
        Try
            If sStrip.InvokeRequired Then
                Dim nMessage As New _SetMessage(AddressOf _SetM)
                sStrip.Invoke(nMessage, New Object() {Message})
            Else
                tsStatus.Text = Message
            End If
        Catch ex As Exception
            LogWindow.WriteError("SetMessage", Err)
        End Try
    End Sub
    Private Sub _SetM(ByVal Message As String)
        Try
            tsStatus.Text = Message
        Catch ex As Exception
            LogWindow.WriteError("_SetM", Err)
        End Try
    End Sub
    Public Sub SetPBValue(ByVal Value As Int16)
        Try
            If Value = -2 Then
                'Do not set value, but start timed step increase to indicate activity
                SetPBMax(100)
                SetPBValue(1)
                pBar.Style = ProgressBarStyle.Marquee
                tFake.Enabled = True
            ElseIf Value = -1 Then
                tFake.Enabled = False
                SetPBValue(100)
            Else
                pBar.Style = ProgressBarStyle.Continuous
                If pBar.InvokeRequired Then
                    Dim nPB As New _SetPBvalue(AddressOf _SetPB)
                    pBar.Invoke(nPB, New Object() {Value})
                Else
                    pBar.Value = Value
                End If
            End If

        Catch ex As Exception
            LogWindow.WriteError("SetPBValue", Err)
        End Try
    End Sub
    Private Sub _SetPB(ByVal Value As Int16)
        Try
            pBar.Value = Value
        Catch ex As Exception
            LogWindow.WriteError("_SetPB", Err)
        End Try
    End Sub

    Private Sub tFake_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tFake.Tick
        Try
            If pBar.Value < pBar.Maximum - 10 Then
                SetPBValue(pBar.Value + 10)
                Application.DoEvents()
            Else
                SetPBValue(1)
                Application.DoEvents()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class