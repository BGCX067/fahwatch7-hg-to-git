'   EOC Signature form
'   Copyright (c) 2010 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.

Public Class frmEOC
    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
            '
            Return MyBase.ShowWithoutActivation
        End Get
    End Property
    Private mLocation As New System.Drawing.Point
    Private UserName As String = ""
    Private TeamNumber As String = ""
    Private UserID As String = ""
    Private bOnTop As Boolean = False
    Private EOCinfo As EOCInfo = modMAIN.EOC
    Private dtShown As New DateTime
    Private WithEvents tFade As New System.Timers.Timer
    Private _FadeTimeOut As Double = 5000
    Private _OnDesktop As Boolean = False
    Friend ReadOnly Property MyPosition() As System.Drawing.Point
        Get
            Return mLocation
        End Get
    End Property
    Friend ReadOnly Property OnDesktop As Boolean
        Get
            Return _OnDesktop
        End Get
    End Property
    Friend Delegate Function ShowSigDelegate(ByVal FadeTimeOut As Double) As Boolean
    Private Function dShowSig(Optional FadeTimeOut As Double = 5000) As Boolean
        Try
#If CONFIG = "Debug" Then
            Console.WriteLine("dShowsig: " & Threading.Thread.CurrentThread.ManagedThreadId & " invokerequired: " & Me.InvokeRequired.ToString)
#End If
            Me.Opacity = 1
            If Not Me.Created Then
                Me.WindowState = FormWindowState.Minimized
                Me.WindowState = FormWindowState.Normal
            End If
            tFade = New System.Timers.Timer
            _FadeTimeOut = New Double
            _FadeTimeOut = FadeTimeOut
            _FadeTimeOut = FadeTimeOut
            'EOC.EOCStats(UserName, TeamNumber).RefreshEOC()
            If IsNothing(EOCinfo.EOCStats(UserName, TeamNumber)) Then EOCinfo.Init()
            If EOCinfo.EOCStats(UserName, TeamNumber).Initialized Then
                RefreshPB()
            Else
                MsgBox("There is no EOC signature image stored, and attempts to refresh it have failed")
                Return False
            End If
            mLocation.Y = Screen.PrimaryScreen.WorkingArea.Bottom - Me.BackgroundImage.Height
            mLocation.X = Screen.PrimaryScreen.WorkingArea.Right - Me.BackgroundImage.Width
            'commented out only needed for stacking multiple signature images 
            For Xind As Int32 = 1 To FAHWatch7.EOCInfo.NoVisible
                mLocation.Y -= Me.BackgroundImage.Height
            Next
            Me.Location = mLocation
            delegateFactory.ShowTopmostWithoutFocus(Me)
            'NativeMethods.ShowWindow(Me.Handle, CType(NativeMethods.SW_SHOWNOACTIVATE, NativeMethods.ShowWindowCommands))
            'NativeMethods.SetWindowPos(Me.Handle.ToInt32(), NativeMethods.HWND_TOPMOST, Me.Left, Me.Top, Me.Width, Me.Height, NativeMethods.SWP_NOACTIVATE)
#If CONFIG = "Debug" Then
            Console.WriteLine("-Activate_size: " & Me.Size.ToString)
            Console.WriteLine("-Visible: " & Me.Visible.ToString)
            Console.WriteLine("-OnTop: " & Me.TopMost.ToString)
#End If
            _OnDesktop = True
            If _FadeTimeOut > 0 Then
                Application.DoEvents()
                tFade.Enabled = False
                tFade.AutoReset = False
                tFade.Interval = _FadeTimeOut
                tFade.Enabled = True
            End If
            Return delegateFactory.IsFormVisible(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function ShowSig(Optional ByVal FadeTimeOut As Double = 5000) As Boolean
        Try
            Console.WriteLine("showsig: " & Threading.Thread.CurrentThread.ManagedThreadId & " invokerequired: " & Me.InvokeRequired.ToString)
            If Not Me.InvokeRequired Then
                Return dShowSig(FadeTimeOut)
            Else
                Dim nI As New ShowSigDelegate(AddressOf dShowSig)
                Dim result As IAsyncResult = Me.BeginInvoke(nI, {FadeTimeOut})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                Dim bRes As Boolean = CBool(Me.EndInvoke(result))
                result.AsyncWaitHandle.Close()
                Return bRes
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Delegate Sub refreshPBDelegate()
    Private Sub dRefreshPB()
        Try
            Me.BackgroundImage = EOCinfo.EOCStats(UserName, TeamNumber).SignatureImage
            Me.Size = Me.BackgroundImage.Size
            'pbSignature.Refresh()
            'Me.Size = pbSignature.Size
        Catch ex As Exception
            WriteError(ex.Message, Err)
            'image could not be refreshed, close form
            Me.Close()
        End Try
    End Sub
    Private Sub RefreshPB()
        Try
            If Me.InvokeRequired Then
                Dim nI As New refreshPBDelegate(AddressOf dRefreshPB)
                Dim result As IAsyncResult = Me.BeginInvoke(nI)
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                Me.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Else
                dRefreshPB()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Delegate Sub tFade_ElapsedDelagate(Sender As Object, e As System.Timers.ElapsedEventArgs)
    Private Sub dTFade_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs)
        Try
            If Me.InvokeRequired Then
                Dim dHide As New dHideMe(AddressOf HideMe)
                Dim iAsyn As IAsyncResult = Me.BeginInvoke(dHide)
                While Not iAsyn.IsCompleted
                    Application.DoEvents()
                End While
                Me.EndInvoke(iAsyn)
            Else
                HideMe()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tFade_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tFade.Elapsed
        Try
            If Me.InvokeRequired Then
                Dim nI As New tFade_ElapsedDelagate(AddressOf dTFade_Elapsed)
                Dim result As IAsyncResult = Me.BeginInvoke(nI, {sender, e})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                Me.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Else
                dTFade_Elapsed(sender, e)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Delegate Sub dHideMe()
    Private Sub HideMe()
        Try
            If Not Me.InvokeRequired Then
                If Me.Opacity > 0 Then
                    While Me.Opacity > 0
                        Me.Opacity -= 0.05
                        Application.DoEvents()
                        Threading.Thread.Sleep(10)
                    End While
                End If
                Me.Visible = False
                _OnDesktop = False
                Me.Hide()
                Me.Opacity = 1
            Else
                Dim dHide As New dHideMe(AddressOf HideMe)
                Dim iAsyn As IAsyncResult = Me.BeginInvoke(dHide)
                While Not iAsyn.IsCompleted
                    Application.DoEvents()
                End While
                Me.EndInvoke(iAsyn)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Friend Sub New(ByVal mUserName As String, ByVal mTeamNumber As String)
        ' This call is required by the designer.
        InitializeComponent()
        UserName = mUserName
        TeamNumber = mTeamNumber
        'Me.Show()
        'Me.Hide()
        'RefreshPB()
    End Sub

End Class