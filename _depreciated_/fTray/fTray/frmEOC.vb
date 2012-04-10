﻿Imports System.Xml
Imports System.Xml.XPath
Public Class frmEOC
    Private mLocation As New System.Drawing.Point
    Private UserName As String = ""
    Private TeamNumber As String = ""
    Private UserID As String = ""
    Private bOnTop As Boolean = False
    Private _Client As clsClientControl.clsEOCInfo
    Private dtShown As New DateTime
    Private WithEvents tFade As New System.Timers.Timer
    Private _FadeTimeOut As Double = 5000
    Private _OnDesktop As Boolean = False
    Public ReadOnly Property MyPosition() As System.Drawing.Point
        Get
            Return mLocation
        End Get
    End Property
    Public ReadOnly Property OnDesktop As Boolean
        Get
            Return _OnDesktop
        End Get
    End Property
    Public Function ShowSig(ByVal Client As clsClientControl.clsEOCInfo, Optional ByVal FadeTimeOut As Double = 5000, Optional ByVal ShowIcon As Boolean = True) As Boolean
        Try
            If Not Me.Created Then
                Me.WindowState = FormWindowState.Minimized
                Me.WindowState = FormWindowState.Normal
            End If

            tFade = New System.Timers.Timer
            _FadeTimeOut = New Double
            _FadeTimeOut = FadeTimeOut
            Me.TopMost = True
            _FadeTimeOut = FadeTimeOut
            UserName = Client.UserName
            TeamNumber = Client.TeamNumber
            _Client = Client
            If _Client.EocID <> "" Then UserID = _Client.EocID
            ClientControl.EOC.EOCStats.RefreshEOC()
            If Client.EOCStats.Initialized Then
                pb.Image = Client.EOCStats.SignatureImage
                pb.Refresh()
            Else
                MsgBox("There is no EOC signature image stored, and attempts to refresh it have failed")
                Return False
            End If
            If ShowIcon Then
                nIcon.Text = "ExtremeOverClocking" & vbNewLine & UserName & " (" & TeamNumber & ")"
                nIcon.ContextMenuStrip = cMenu
                cMenu.Items.Clear()
                cMenu.Items.Add("View user stats")
                cMenu.Items.Add("View team stats")
                cMenu.Items.Add("-")
                cMenu.Items.Add("Close sig image")
                nIcon.Icon = My.Resources.fTray_EOC
                dtLastUpdate = DateTime.Now
            End If
            Me.Visible = True
            Return True
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, ShowSig", Err, ex.Message)
            Me.Close()
            Return False
        End Try
    End Function
    Private Sub nIcon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseClick
        Try
            If e.Button = Windows.Forms.MouseButtons.Right Then
                cMenu.Show()
            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                If Me.Visible = False Then
                    Me.Visible = True
                Else
                    Me.Visible = False
                    tFade.Enabled = False
                End If
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, nIcon.MouseClick", Err, ex.Message)
        End Try
    End Sub
    Private Sub cMenu_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cMenu.ItemClicked
        Try
            Select Case e.ClickedItem.Text
                Case "Close sig image"
                    ClientControl.EOC.bStatsInit = False
                    If Not ReferenceEquals(_FadeThread, Nothing) Then _FadeThread.Abort()
                    If Not ReferenceEquals(_SetOpThread, Nothing) Then _SetOpThread.Abort()
                    _FadeThread = Nothing
                    _SetOpThread = Nothing
                    nIcon.Visible = False
                    Me.Close()
                Case "View user stats"
                    If UserID = "" Then
                        Try
                            Dim xSettings As XmlReaderSettings = New XmlReaderSettings()
                            xSettings.IgnoreComments = True
                            xSettings.IgnoreProcessingInstructions = True
                            xSettings.IgnoreWhitespace = True
                            Dim xResolver As XmlUrlResolver = New XmlUrlResolver()
                            xResolver.Credentials = System.Net.CredentialCache.DefaultCredentials
                            ' Set the reader settings object to use the resolver.
                            xSettings.XmlResolver = xResolver
                            Using xReader As XmlReader = XmlReader.Create("http://folding.extremeoverclocking.com/xml/user_summary.php?un=" & UserName & "&t=" & TeamNumber, xSettings)
                                With xReader
                                    .ReadToFollowing("UserID")
                                    UserID = .ReadElementString("UserID")
                                End With
                            End Using
                            _Client.EocID = UserID
                        Catch ex As Exception
                            'Set userid to -1
                            UserID = "-1"
                        End Try
                    End If
                    If Not UserID = "-1" Then Process.Start("http://folding.extremeoverclocking.com/user_summary.php?s=&u=" & UserID)
                Case "View team stats"
                    Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&t=" & TeamNumber)
            End Select
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, cMenu.Itemclicked", Err, ex.Message)
        End Try
    End Sub
    Private dtLastUpdate As New DateTime
    Delegate Sub RefPb()
    Private Sub RefreshPB()
        Try
            dtLastUpdate = DateTime.Now
            pb.Image = _Client.EOCStats.SignatureImage
            pb.Refresh()
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, RefreshPB", Err, ex.Message)
            'image could not be refreshed, close form
            Me.Close()
        End Try
    End Sub
    Private Sub nIcon_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles nIcon.MouseDown
        Try
          
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, nIcon.MouseDown", Err, ex.Message)
        End Try
    End Sub
    Private Sub frmEOC_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            nIcon.Visible = False
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmEOC_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        Try
            If Me.Visible Then
                mLocation.Y = Screen.PrimaryScreen.WorkingArea.Bottom - pb.Height
                mLocation.X = Screen.PrimaryScreen.WorkingArea.Right - pb.Width
                'commented out only needed for stacking multiple signature images 
                'For Xind As Int16 = 1 To Clients.GetUpperBound(0)
                'If Clients(Xind).Index <> _Client.Index And Clients(Xind).GuiController.StatsForm.Visible Then
                'mLocation.Y -= pb.Height
                'End If
                'Next
                Me.Location = mLocation
                If _FadeTimeOut > 0 Then
                    tFade.Enabled = False
                    tFade.AutoReset = False
                    tFade.Interval = _FadeTimeOut
                    tFade.Enabled = True
                End If
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, VisibleChanged", Err, ex.Message)
        End Try

    End Sub
    Delegate Sub fFade()
    Private _FadeThread As Threading.Thread
    Private _SetOpThread As Threading.Thread
    Private Sub Fade()
        Try
            If Me.Created = False Then
                Exit Sub
            End If
            While Me.Opacity > 0
                Dim _SetOpThread As New Threading.Thread(AddressOf SetOpacity)
                _SetOpThread.Start(Me.Opacity - 0.01)
                Threading.Thread.Sleep(5)
            End While
            Reset()
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, Fade", Err, ex.Message)
        End Try
    End Sub
    Delegate Sub ResetMe()
    Private Sub Reset()
        Try
            If Not Me.Created Then
                Exit Sub
            End If
            If ReferenceEquals(Me, Nothing) Then Exit Sub
            If Me.InvokeRequired Then
                Dim mRes As New ResetMe(AddressOf Reset)
                Me.Invoke(mRes)
            Else
                Me.Visible = False
                Me.Opacity = 100
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, Reset", Err, ex.Message)
        End Try
    End Sub
    Delegate Sub SetOp(ByVal Value As Double)
    Private Sub SetOpacity(ByVal Value As Double)
        Try
            If Me.InvokeRequired Then
                Dim mInv As New SetOp(AddressOf SetOpacity)
                Me.Invoke(mInv, New Object() {Value})
            Else
                Me.Opacity = Value
            End If
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, SetOpacity", Err, ex.Message)
        End Try
    End Sub
    Private Sub tFade_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tFade.Elapsed
        Try
            Dim _FadeThread As New Threading.Thread(AddressOf Fade)
            _FadeThread.Start()
        Catch ex As Exception
            LogWindow.WriteError("frmEOC, tFade_Elapsed", Err, ex.Message)
        End Try
    End Sub
End Class