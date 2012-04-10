Public Class Tools
    Private Shared WithEvents frmDownloadTool As New frmTools
    Friend Shared Sub ShowDownload(Optional Parent As Form = Nothing)
        Try
            Try
                If IsNothing(frmDownloadTool) Then
                    frmDownloadTool = New frmTools
                Else
                    If frmDownloadTool.IsDisposed Or frmDownloadTool.Disposing Then
                        frmDownloadTool = New frmTools
                    End If
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            If IsNothing(Parent) Then
                frmDownloadTool.ShowDialog()
            Else
                frmDownloadTool.ShowDialog(Parent)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared mMemtestCL_present As Boolean = False
    Private Shared mMemtestG80_present As Boolean = False
    Private Shared mStressCpuV2_present As Boolean = False
    Sub New()

    End Sub
End Class
