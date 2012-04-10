Public Class frmDownload
    Private speed As Double
    Private speedTemp As Double
    Public Function download(ByVal sURL As String, ByVal pProgress As ProgressBar, ByVal Filename As String) As Boolean
        'Dim wRemote As System.Net.WebRequest
        Me.Text = "Downloading " & Filename.Replace(Application.StartupPath & "\", "")
        Dim URLReq As Net.HttpWebRequest
        Dim URLRes As Net.HttpWebResponse
        Dim FileStreamer As New IO.FileStream(Filename, IO.FileMode.Create)
        Dim bBuffer(999) As Byte
        Dim iBytesRead As Integer
        Application.DoEvents()
        Try
            URLReq = System.Net.WebRequest.Create(sURL)
            URLRes = URLReq.GetResponse
            Dim sChunks As IO.Stream = URLReq.GetResponse.GetResponseStream
            PbarDownload.Value = 0
            PbarDownload.Maximum = URLRes.ContentLength
            tSpeed.Enabled = True
            Do
                iBytesRead = sChunks.Read(bBuffer, 0, 1000)
                speed += iBytesRead
                lblDownload.Text = "Download " & Format(PbarDownload.Value / 1024, "#,###,###,###0.00") & " Kb from " & Format(PbarDownload.Maximum / 1024, "#,###,###,###0.00") & " kb"
                Application.DoEvents()
                If PbarDownload.Value + iBytesRead <= PbarDownload.Maximum Then
                    PbarDownload.Value += iBytesRead
                Else
                    PbarDownload.Value = PbarDownload.Maximum
                End If
                FileStreamer.Write(bBuffer, 0, iBytesRead)
            Loop Until iBytesRead = 0
            PbarDownload.Value = PbarDownload.Maximum
            bitsec.Text = ""
            speed = 0
            tSpeed.Enabled = False
            sChunks.Close()
            FileStreamer.Close()
            Return True
        Catch
            MsgBox(Err.Description)
            tSpeed.Enabled = False
            Return False
        End Try
    End Function
    Private Sub tspeed_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tSpeed.Tick
        speedTemp = speed / 1000
        speed = 0
        bitsec.Text = Format(speedTemp, "#,###,###,###0.00") & " Kb/sec"
    End Sub

End Class