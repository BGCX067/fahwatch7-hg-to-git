'/*
' * fInfo modMAIN copyright Marvin Westmaas ( mtm )
' * 
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
Imports gpuInfo
Imports HWInfo
Imports Data
Imports FAHInterface.FAHInterface
Imports FAHInterface.Client
Imports System.Xml
Imports System.IO
Imports fInfo.fInfo
Imports ProjectInfo

Module modMAIN
    Public LogWindow As New clsLogwindow
    Public Settings As New clsSettings

    Public ExtClient As clsExtClient
    Public Control As New clsControl
    Public About As New clsAbout
    Sub main()
        Try
            ExtClient = New fInfo.clsExtClient
            ExtClient.LogWindow = LogWindow
            Dim p As String = "", pwd As String = "", cmd As String = ""
            If My.Application.CommandLineArgs.Count > 0 Then
                Settings.MySettings.Automation = False
                For Each arg As String In My.Application.CommandLineArgs
                    If arg.ToUpper.Contains("PORT:") Then p = arg.Substring(5)
                    If arg.ToUpper.Contains("PWD:") Then pwd = arg.Substring(4)
                    If arg.ToUpper.Contains("CMD:") Then cmd = arg.Substring(4)
                Next
                Try
                    If cmd = "?" Then
                        ' TODO Add console/window output to "?" argument
                    ElseIf cmd <> "" Then
                        ' Scheduled commands will only work when a local client is already running!
                        LogWindow.CreateLog(Application.StartupPath & "\" & Application.ProductName & "-external.log")
                        LogWindow.WriteLog("Command line argument given: " & cmd)
                        LogWindow.WriteLog("Connecting to client.")
                        Dim c As New FAHInterface.Client.ClientAccess
                        c.Connect("127.0.0.1", CInt(p))
                        If Not c.Connected Then
                            LogWindow.WriteLog("Could not connect to client.")
                            ExitApplication(True)
                        Else
                            c.SendCommand("auth " & pwd)
                            c.Update()
                            If c.ValidPassword And c.Connected Then
                                LogWindow.WriteLog("Sending command: " & cmd)
                                c.SendCommand(cmd)
                                'c.Update() Don't call update, we're not checking validity or results anyway at this point in time..
                                Call ExitApplication(True)
                                Exit Sub
                            Else
                                LogWindow.WriteLog("Password provided was not valid!")
                                Call ExitApplication(True)
                                Exit Sub
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Exit Sub
                End Try
            Else
                Application.Run(ExtClient.Init(p, pwd))
            End If
        Catch ex As Exception
            If Not IsNothing(LogWindow) Then
                LogWindow.WriteError(ex.Message, Err)
                LogWindow.CloseLog()
            End If
        End Try
    End Sub
    Public Sub ExitApplication(Optional Overide As Boolean = False)
        Try
            If Settings.MySettings.ConfirmExit And Not Overide Then
                Dim rVal As MsgBoxResult = MsgBox("Confirm exit?", vbOKCancel + MsgBoxStyle.Information, My.Application.Info.ProductName)
                If rVal = MsgBoxResult.Cancel Then Exit Sub
            End If
            If Not IsNothing(ExtClient.ohmInt) Then
                LogWindow.WriteLog("Closing OpenHardwareMonitorLib.")
                ExtClient.ohmInt.AutoUpdate(0)
                ExtClient.ohmInt.Close()
            End If
            If Not IsNothing(ExtClient.Data) Then
                LogWindow.WriteLog("Closing database connection.")
                ExtClient.Data.Close()
            End If
            If Not IsNothing(Settings) Then
                LogWindow.WriteLog("Closing down application settings container.")
                Settings.CloseSettings()
            End If
            If Not IsNothing(Control.Form) Then
                LogWindow.WriteLog("Closing control form.")
                Control.Close()
            End If
            If Not IsNothing(LogWindow) Then
                LogWindow.WriteLog("Closing down application.")
                LogWindow.CloseLog()
            End If

            Application.Exit()
        Catch ex As Exception

        End Try
    End Sub

    Public Function MD5CalcFile(ByVal TheFile As String) As String
        ' Using statements make sure garbage collector has it easy..
        Using reader As New System.IO.FileStream(TheFile, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                Dim hash() As Byte = md5.ComputeHash(reader)
                Return ByteArrayToString(hash)
            End Using
        End Using
    End Function
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)
        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function

End Module