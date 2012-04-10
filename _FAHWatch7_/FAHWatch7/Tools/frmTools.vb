Public Class frmTools
    Private Const urlTools As String = "http://folding.stanford.edu/English/DownloadUtils"
    Private Const fileStressCpuV2 As String = "stresscpu2_win32.exe"
    Private Const fileMemtestCL As String = "memtestCL.exe"
    Private Const fileMemtestG80 As String = "memtestG80.exe"

    Private Sub DownloadLinkClicked(sender As Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llblMemtestCL.LinkClicked, llblMemtestG80.LinkClicked, llblStressCpuV2.LinkClicked
        Process.Start(urlTools)
    End Sub
    Private Sub llbl_MouseHover(sender As Object, e As System.EventArgs) Handles llblMemtestCL.MouseHover, llblMemtestG80.MouseHover, llblStressCpuV2.MouseHover
        Try
            If ReferenceEquals(sender, llblMemtestCL) Or ReferenceEquals(sender, llblMemtestG80) Then
                lblWhat.Text = "MemtestG80 and MemtestCL are software-based testers to test for 'soft errors' in GPU memory or logic for NVIDIA CUDA-enabled GPUs (MemtestG80) or OpenCL-enabled CPUs and GPUs by any manufacturer, including both ATI and NVIDIA (MemtestCL). They use a variety of proven test patterns (some custom and some based on Memtest86) to verify the correct operation of GPU memory and logic. They are useful tools to ensure that given GPUs do not produce 'silent errors' which may corrupt the results of a computation without triggering an overt error"
            ElseIf ReferenceEquals(sender, llblStressCpuV2) Then
                lblWhat.Text = "StressCPU is a CPU stress tester, based on the Gromacs code found in the Folding@home (FAH) fahcores that process work units. StressCPU stresses all of the processor cores on a computer to help verify system stability, and is one of the best testing tools available, pushing CPUs harder and hotter than old school favorites like Prime95"
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub llbl_MouseLeave(sender As System.Object, e As System.EventArgs) Handles llblStressCpuV2.MouseLeave, llblMemtestCL.MouseLeave, llblMemtestG80.MouseLeave
        lblWhat.Text = "Hover over a tool's download link to view a short description."
    End Sub

    Private Sub frmTools_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER Or NativeMethods.AnimateWindowFlags.AW_HIDE)
    End Sub

    Private Sub frmTools_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
        Try
            If m_hwInfo.gpuInf.CudaDeviceCount > 0 Then
                llblMemtestG80.Enabled = True
            Else
                llblMemtestG80.Enabled = False
            End If
            If m_hwInfo.gpuInf.oclDevicesTotal > 0 Then
                llblMemtestCL.Enabled = True
            Else
                llblMemtestCL.Enabled = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.7-zip.org/")
    End Sub

    Private Sub tCheck_Tick(sender As System.Object, e As System.EventArgs) Handles tCheck.Tick
        Try
            If Not chkMemtestCL.Checked Then
                If My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\memtestCL\") Then
                    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\memtestCL\memtestCL.exe") Then
                        chkMemtestCL.Checked = True
                    End If
                End If
            End If
            If Not chkMemtestG80.Checked Then
                If My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\memtestG80\") Then
                    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\memtestG80\memtestG80.exe") Then
                        chkMemtestG80.Checked = True
                    End If
                End If
            End If
            If Not chkStressCpuV2.Checked Then
                If My.Computer.FileSystem.DirectoryExists(My.Application.Info.DirectoryPath & "\stresscpu2\") Then
                    If My.Computer.FileSystem.FileExists(My.Application.Info.DirectoryPath & "\stresscpu2\stresscpu2_win32.exe") Then
                        chkStressCpuV2.Checked = True
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
End Class