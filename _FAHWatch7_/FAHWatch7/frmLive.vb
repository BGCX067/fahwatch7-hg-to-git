' This is all very rough, won't stay this way as I'm expecting troubles when running certain functions while the logparser is running for instance


Imports System.Globalization
Imports HWInfo.clsHWInfo
Imports HWInfo.clsHWInfo.cHWInfo.cOHMInterface.ohmSensors
Imports ZedGraph

Friend Class frmLive

#Region "Declarations"

    Private bManual As Boolean = False, bStatisticsChanging As Boolean = False
    Private mSilentClose As Boolean = False, bIsShown As Boolean = False, bSurpress As Boolean = True

#End Region

#Region "Form handling"
    Friend ReadOnly Property IsShown As Boolean
        Get
            Return bIsShown
        End Get
    End Property
    Private Sub cmdViewGraph_Click(sender As System.Object, e As System.EventArgs) Handles cmdViewGraph.Click
        Try
            scLiveGraph.Panel2Collapsed = Not scLiveGraph.Panel2Collapsed
            If Not scLiveGraph.Panel2Collapsed Then
                FillChklbWU()
                UpdateFrameGraph()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdHWSensors_Click(sender As System.Object, e As System.EventArgs) Handles cmdHWSensors.Click
        Try
            If bManual Then Exit Sub
            scHardware.Panel2Collapsed = Not scHardware.Panel2Collapsed
            If Not scHardware.Panel2Collapsed Then
                UpdateSensors()
            Else
                DisableSensors()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Property SilentClose As Boolean
        Get
            Return mSilentClose
        End Get
        Set(value As Boolean)
            mSilentClose = value
        End Set
    End Property
    Friend Sub ShowForm()
        Try
            If modMySettings.HideInactiveMessageStrip Then
                sStripMessage.Visible = False
            Else
                sStripMessage.Visible = True
            End If

            NativeMethods.AnimateWindow(Me.Handle, 100, NativeMethods.AnimateWindowFlags.AW_CENTER)
            If Not bIsShown Then
                Me.Show()
                delegateFactory.ActivateForm(Me)
            Else
                delegateFactory.ShowFormActivated(Me)
            End If
            tParse.Interval = CInt(TimeSpan.FromMinutes(3).TotalMilliseconds)
            tParse.Enabled = True
            tFill.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub HideForm()
        tParse.Enabled = False
        delegateFactory.HideFade(Me)
        Me.Visible = False
    End Sub
    Private Sub frmLive_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        modMySettings.ColumnSettings.CreateMaster(lvLive)
        modMySettings.ColumnSettings.CreateMaster(lvQueue)
    End Sub
    Private Sub frmLive_LocationChanged(sender As Object, e As System.EventArgs) Handles Me.LocationChanged
        Try
            If Not Me.Created Or bManual Or Me.WindowState = FormWindowState.Minimized Then Exit Sub
            modMySettings.live_formlocation = Me.Location
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmLive_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        Try
            If Not Me.Created Or bManual Then Exit Sub
            If Not Me.WindowState = FormWindowState.Minimized Then
                modMySettings.live_formlocation = Me.Location
                modMySettings.live_windowstate = Me.WindowState
                Timers.StartParseTimer(TimeSpan.FromMinutes(3).TotalMilliseconds)
                If Not Me.WindowState = FormWindowState.Maximized Then modMySettings.live_formsize = Me.Size
            Else
                WriteDebug(Me.Name & " gets minimized")
                If modMySettings.MinimizeToTray Then
                    WriteDebug(Me.Name & " playing hideFade animation")
                    delegateFactory.HideFade(Me)
                    WriteDebug("showing tray icon")
                    modIcon.ShowIcon()
                    Return
                End If
                If Not modMySettings.ParseLogsOnInterval Then
                    Timers.IntervalBasedParserEnabled = False
                Else
                    Timers.IntervalBasedParserEnabled = True
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmLive_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            If modMySettings.MinimizeToTray Then
                modIcon.ShowIcon()
                delegateFactory.HideFade(Me)
            End If
        ElseIf Not Me.WindowState = FormWindowState.Maximized Then
            modMySettings.live_formsize = Me.Size
        End If
    End Sub
    Private Sub frmLive_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            AddHandler delegateFactory.DefaultStatisticsChanged, AddressOf DefaultStatisticsChanged
            AddHandler delegateFactory.EOC_EnabledChanged, AddressOf EOC_EnabledChanged
            AddHandler delegateFactory.EOC_ViewTeamChanged, AddressOf EOC_ViewTeamChanged
            AddHandler delegateFactory.EOC_ViewUserChanged, AddressOf EOC_ViewUserChanged
            AddHandler delegateFactory.ParserCompleted, AddressOf HandleParserCompleted
            AddHandler delegateFactory.ParserFailed, AddressOf HandleParserFailed
            Timers.StartParseTimer(TimeSpan.FromMinutes(3).TotalMilliseconds)
            If Not modMySettings.live_formlocation.Equals(New Point(0, 0)) Then
                Me.Location = modMySettings.live_formlocation
            End If
            Me.WindowState = modMySettings.live_windowstate
            If Not modMySettings.live_windowstate = FormWindowState.Maximized Then
                If Not modMySettings.live_formsize.Equals(New Size(0, 0)) Then
                    Me.Size = modMySettings.live_formsize
                End If
            End If
            sStripMessage.Visible = Not modMySettings.HideInactiveMessageStrip
            sStripEOC.Visible = Not modMySettings.DisableEOC : sStripEoc2.Visible = Not modMySettings.DisableEOC
            If Not modMySettings.DisableEOC Then
                AddHandler delegateFactory.EOC_UpdateRecieved, AddressOf EocUpdateHandler
                Call EocUpdateHandler(Me, New MyEventArgs.EocUpdateArgs(EOCInfo.primaryAccount))
                bSurpress = False
            End If
            ViewTeamStatusToolStripMenuItem.Checked = modMySettings.viewEocTeam
            ViewUserStatusToolStripMenuItem.Checked = modMySettings.viewEocUser
            If modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current Then
                CurrentToolStripMenuItem.Checked = True
            Else
                OverallToolStripMenuItem.Checked = True
            End If
            tpQueue.Text = "Queued work units (" & Clients.QueuedWorkUnits.Count & ")"
            tFill.Interval = 1
            tFill.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tFill_Tick(sender As System.Object, e As System.EventArgs) Handles tFill.Tick
        Try
            tFill.Enabled = False
            InitListView()
            FillChklbWU()
            If Not scZgFramesSelection.Panel2Collapsed Then UpdateFrameGraph()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub frmLive_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            WriteDebug(Me.Name & " closing, " & e.CloseReason.ToString)
            If mSilentClose Then Return
            If e.CloseReason = CloseReason.UserClosing Then
                tParse.Enabled = False
                If modMySettings.MinimizeToTray Then
                    e.Cancel = True
                    delegateFactory.HideFade(Me, 500)
                    Me.Visible = False
                    modIcon.ShowIcon()
                ElseIf delegateFactory.IsFormVisible(History) Then
                    e.Cancel = True
                    delegateFactory.HideFade(Me, 500)
                    Me.Visible = False
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tlpLog_Resize(sender As System.Object, e As System.EventArgs) Handles tlpLog.Resize
        Try
            txtLog.Dock = DockStyle.Fill
            tlpLog.PerformLayout()
            If tsFollowLog.Checked Then
                txtLog.SelectionStart = txtLog.TextLength
                txtLog.ScrollToCaret()
                txtLog.Invalidate()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "listview live"
    Private Sub lvLive_ItemSelectionChanged(sender As System.Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvLive.ItemSelectionChanged
        Try
            If e.IsSelected Then
                If Not scLiveGraph.Panel2Collapsed AndAlso ReferenceEquals(tcFramesLogQueue.SelectedTab, tpLog) Then
                    UpdateLog()
                ElseIf Not scLiveGraph.Panel2Collapsed AndAlso ReferenceEquals(tcFramesLogQueue.SelectedTab, tpFramesGraph) Then
                    SyncLock chklbWU
                        bManual = True
                        Try
                            For xInt As Int32 = 0 To chklbWU.Items.Count - 1
                                chklbWU.SetItemChecked(xInt, False)
                            Next
                            chklbWU.SetItemChecked(chklbWU.FindString(e.Item.Text & ":" & e.Item.SubItems(1).Text), True)
                            UpdateFrameGraph()
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        bManual = False
                    End SyncLock
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Function lvItem(client As Client, workunit As clsWU) As ListViewItem
        Try
            If IsNothing(workunit) Then
                WriteDebug("Slot has no work unit attached")
                Return Nothing
            Else
                WriteDebug("ListViewItem creation called for " & strID(workunit))
                Dim nI As New ListViewItem
                nI.Text = client.ClientName
                nI.Name = workunit.unit
                nI.SubItems.Add(workunit.Slot)
                nI.SubItems.Add(workunit.HW)
                nI.SubItems.Add(workunit.Percentage)
                nI.SubItems.Add(workunit.Worth.Credit)
                nI.SubItems.Add(workunit.Worth.PPD)
                nI.SubItems.Add(workunit.TPF)
                nI.SubItems.Add(workunit.PRCG)
                If Not workunit.Eta = #1/1/2000# Then
                    If ProjectInfo.KnownProject(workunit.Project) Then
                        If workunit.Eta(True) > workunit.utcStartDownload.AddDays(CDbl(ProjectInfo.Project(workunit.Project).FinalDeadline)) Then
                            nI.BackColor = Color.OrangeRed
                            nI.ToolTipText = "Eta is past final deadline!"
                        ElseIf workunit.Eta(True) > workunit.utcStartDownload.AddDays(CDbl(ProjectInfo.Project(workunit.Project).PreferredDays)) Then
                            nI.BackColor = Color.LightYellow
                            nI.ToolTipText = "Eta is past preffered deadline!"
                        End If
                    End If
                    If modMySettings.live_etastyle = modMySettings.eEtaStyle.ShowDate Then
                        nI.SubItems.Add(workunit.Eta.ToString(CultureInfo.CurrentCulture))
                    Else
                        nI.SubItems.Add(FormatTimeSpan(workunit.Eta.Subtract(DateTime.Now)))
                    End If
                Else
                    nI.SubItems.Add("")
                    nI.BackColor = Color.LightBlue
                    nI.ToolTipText = "Work unit has not completed a frame yet"
                End If
                Return nI
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return Nothing
        End Try
    End Function
    Private Delegate Sub InitListViewDelegate()
    Private Sub dInitListView()
        Try
            tEta.Enabled = False
            Dim selKey As String = ""
            If lvLive.SelectedItems.Count > 0 Then
                selKey = lvLive.SelectedItems(0).Name
            End If
            SyncLock lvLive
                lvLive.BeginUpdate()
                lvLive.Items.Clear()
                Dim dblPpd As New Double
                For Each client In Clients.Clients
                    'lvLive.ShowGroups = True
                    'lvLive.Groups.Add(client.ClientName, client.ClientName)
                    For Each slot As Client.clsSlot In client.Slots
                        If Not IsNothing(slot.WorkUnit) Then
                            Try
                                Dim nItem As ListViewItem = lvItem(client, slot.WorkUnit)
                                If Not IsNothing(nItem) Then
                                    nItem.Tag = slot.WorkUnit
                                    dblPpd += CDbl(slot.WorkUnit.Worth.PPD)
                                    'lvLive.Groups(client.ClientName).Items.Add(nItem)
                                    lvLive.Items.Add(nItem)
                                End If
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                            End Try
                        Else
                            Try
                                For Each WorkUnit As clsWU In client.ActiveWU
                                    If WorkUnit.Slot = slot.Index Then
                                        Dim nItem As ListViewItem = lvItem(client, WorkUnit)
                                        If Not IsNothing(nItem) Then
                                            nItem.Tag = WorkUnit
                                            dblPpd += CDbl(WorkUnit.Worth.PPD)
                                            'lvLive.Groups(client.ClientName).Items.Add(nItem)
                                            lvLive.Items.Add(nItem)
                                        End If
                                        Exit For
                                    End If
                                Next
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                            End Try
                        End If
                    Next
                Next
                lvLive.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                For xInt As Int32 = 0 To lvLive.Columns.Count - 1
                    lvLive.Columns(xInt).Tag = lvLive.Columns(xInt).Width
                Next
                lvLive.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                For xint As Int32 = 0 To lvLive.Columns.Count - 1
                    If lvLive.Columns(xint).Width < CInt(lvLive.Columns(xint).Tag) Then lvLive.Columns(xint).Width = CInt(lvLive.Columns(xint).Tag)
                Next
                If lvLive.Items.ContainsKey(selKey) Then
                    lvLive.Items(selKey).Selected = True
                End If
                tsLblLive.Text = FormatPPD(CStr(Math.Round(dblPpd, 2))) & " PPD"
            End SyncLock
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            lvLive.EndUpdate()
            lvLive.Refresh()
            If modMySettings.live_etastyle = modMySettings.eEtaStyle.ShowTimeToGo Then
                tEta.Enabled = True
            Else
                tEta.Enabled = False
            End If
        End Try
    End Sub
    Friend Sub InitListView()
        Try
            Dim nI As New InitListViewDelegate(AddressOf dInitListView)
            Dim result As IAsyncResult = lvLive.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Hardware monitoring"
#Region "Update sensors"
    Private Delegate Sub UpdateSensorsDelegate()
    Private Sub dUpdateSensors()

    End Sub
    Friend Sub UpdateSensors()

    End Sub
#End Region
#Region "Disable sensors"
    Private Delegate Sub DisableSensorsDelegate()
    Private Sub dDisableSensors()

    End Sub
    Friend Sub DisableSensors()

    End Sub
#End Region

#End Region

#Region "framegraph"
#Region "chklbWU"
    Private Delegate Sub FillChkLbWUDelegate()
    Private Sub dFillChkLbWU()
        Try
            SyncLock Clients.Clients
                iIndex = 0
                chklbWU.BeginUpdate()
                chklbWU.Items.Clear()
                For Each Client As Client In Clients.Clients
                    For Each slot As Client.clsSlot In Client.Slots
                        chklbWU.Items.Add(Client.ClientName & ":" & slot.Index, True)
                    Next
                Next
                chklbWU.EndUpdate()
                chklbWU.Invalidate()
            End SyncLock
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub FillChklbWU()
        Try
            Dim nI As New FillChkLbWUDelegate(AddressOf dFillChkLbWU)
            Dim result As IAsyncResult = chklbWU.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            chklbWU.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private iIndex As Int32 = 0
    Private Sub cmdClear_Click(sender As System.Object, e As System.EventArgs) Handles cmdClear.Click
        ClearAll()
    End Sub
    Private Sub ClearAll()
        For xInt As Int32 = 0 To chklbWU.Items.Count - 1
            chklbWU.SetItemChecked(xInt, False)
        Next
    End Sub
    Private Sub cmdSelectAll_Click(sender As System.Object, e As System.EventArgs) Handles cmdSelectAll.Click
        For xInt As Int32 = 0 To chklbWU.Items.Count - 1
            chklbWU.SetItemChecked(xInt, True)
        Next
    End Sub
    Private Sub cmdNext_Click(sender As System.Object, e As System.EventArgs) Handles cmdNext.Click
        Try
            If iIndex = 0 Then
                'Start with the first, move down
                ClearAll()
                chklbWU.SetItemChecked(iIndex, True)
                iIndex += 1
            Else
                'move down or set back to 0
                ClearAll()
                If iIndex <= chklbWU.Items.Count - 1 Then
                    chklbWU.SetItemChecked(iIndex, True)
                    iIndex += 1
                Else
                    iIndex = 0
                    chklbWU.SetItemChecked(iIndex, True)
                    iIndex += 1
                End If
            End If
            UpdateFrameGraph()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmdPrev_Click(sender As System.Object, e As System.EventArgs) Handles cmdPrev.Click
        Try
            If iIndex = 0 Then
                'Start with the last, move up
                ClearAll()
                chklbWU.SetItemChecked(0, True)
                iIndex = chklbWU.Items.Count - 1
            Else
                'move up or set back to itemcount -1
                If iIndex >= 0 Then
                    If iIndex > chklbWU.Items.Count - 1 Then iIndex = chklbWU.Items.Count - 1
                    ClearAll()
                    chklbWU.SetItemChecked(iIndex, True)
                    iIndex -= 1
                Else
                    ClearAll()
                    chklbWU.SetItemChecked(chklbWU.Items.Count - 1, True)
                    iIndex = chklbWU.Items.Count - 2
                End If
            End If
            UpdateFrameGraph()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    Private Sub cmdUpdateFrames_Click(sender As System.Object, e As System.EventArgs) Handles cmdUpdateFrames.Click
        UpdateFrameGraph()
    End Sub
    Private Delegate Sub UpdateFrameGraphDelegate()
    Private Sub dUpdateFrameGraph()
        Try
            Dim lppProject As New Dictionary(Of clsWU, ZedGraph.PointPairList), bHour As Boolean = False
            Dim mMinValue As Double = Double.MaxValue, mMaxValue As Double = Double.MinValue
            Dim dtLow As DateTime = DateTime.MaxValue, dtHigh As DateTime = DateTime.MinValue, bGo As Boolean = False
            For Each Client As Client In Clients.Clients
                For Each slot As Client.clsSlot In Client.Slots
                    If chklbWU.CheckedItems.Contains(Client.ClientName & ":" & slot.Index) Then
                        Dim mWorkUnit As New clsWU
                        If Not IsNothing(slot.WorkUnit) Then
                            mWorkUnit = slot.WorkUnit
                        Else
                            For Each WorkUnit As clsWU In Client.ActiveWU
                                If WorkUnit.Slot = slot.Index Then
                                    mWorkUnit = WorkUnit
                                    Exit For
                                End If
                            Next
                        End If
                        Dim ppProject As New PointPairList, maxFrames As Int32 = 0
                        If mWorkUnit.Frames.Count = 0 Then
                            GoTo skip
                        End If
                        For xInt As Int32 = 0 To (mWorkUnit.Frames.Count - 1) Step 1
                            bGo = True
                            Dim tsFrame As TimeSpan
                            If xInt = 0 Then
                                tsFrame = mWorkUnit.Frames(0).FrameDT - mWorkUnit.dtStarted
                            Else
                                tsFrame = mWorkUnit.Frames(xInt).FrameDT - mWorkUnit.Frames(xInt - 1).FrameDT
                            End If
                            If tsFrame.TotalDays > mMaxValue Then mMaxValue = tsFrame.TotalDays
                            If tsFrame.TotalDays < mMinValue Then mMinValue = tsFrame.TotalDays
                            If mWorkUnit.Frames(xInt).FrameDT < dtLow Then dtLow = mWorkUnit.Frames(xInt).FrameDT
                            If mWorkUnit.Frames(xInt).FrameDT > dtHigh Then dtHigh = mWorkUnit.Frames(xInt).FrameDT
                            ppProject.Add(New XDate(mWorkUnit.Frames(xInt).FrameDT), tsFrame.TotalDays)
                        Next
skip:
                        lppProject.Add(mWorkUnit, ppProject)
                    End If
                Next
            Next
            If lppProject.Count = 0 Or Not bGo Then
                'just clear
                zgFrames.MasterPane.PaneList.Clear()
                zgFrames.GraphPane = New GraphPane
                zgFrames.GraphPane.Rect = zgFrames.ClientRectangle
                zgFrames.AxisChange()
                zgFrames.Invalidate()
                Exit Sub
            End If
            Dim lColors As List(Of Color) = GenerateColorList(lppProject.Count - 1)
            zgFrames.MasterPane.PaneList.Clear()
            Dim mPane As New GraphPane
            'mPane.Title.Text = mWorkUnit.HW & Chr(32) & mWorkUnit.ClientName & mWorkUnit.Slot & Chr(32) & mWorkUnit.PRCG
            mPane.XAxis.Title.Text = "Checkpoints occurance"
            mPane.XAxis.Title.FontSpec.Size = 10
            mPane.XAxis.Scale.FontSpec.Angle = 45
            mPane.XAxis.MinorTic.IsOpposite = False
            mPane.XAxis.MajorTic.IsOpposite = False
            mPane.YAxis.MinorTic.IsOpposite = False
            mPane.YAxis.MajorTic.IsOpposite = False
            mPane.YAxis.MajorGrid.PenWidth = 0.2
            mPane.YAxis.MajorGrid.IsVisible = True
            mPane.YAxis.MajorGrid.IsZeroLine = True
            
            mPane.YAxis.Scale.IsPreventLabelOverlap = True
            mPane.Legend.FontSpec.Size = 8
            mPane.XAxis.Type = AxisType.Date
            mPane.YAxis.Type = AxisType.Date
            ' mPane.YAxis.Scale.FormatAuto = False
            mPane.YAxis.Title.Text = "Time per frame ( discarding idle time )"
            mPane.YAxis.Title.FontSpec.Size = 10

            Dim iColor As Int32 = 0
            For Each DictionaryEntry In lppProject
                mPane.AddCurve(DictionaryEntry.Key.PRCG, DictionaryEntry.Value, lColors(iColor))
                iColor += 1
            Next

            'If mMaxValue > 1 / 24 Then
            '    mPane.YAxis.Scale.Format = "hh:mm:ss" : bHour = True
            '    'Set max scale to 15 minutes, round to closest whole 5 min marker
            '    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 15, 0))
            '    Dim rndMinutes As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalMinutes) / 5, 0, MidpointRounding.AwayFromZero) * 5)
            '    If rndMinutes < CInt(dtmax.TimeOfDay.TotalMinutes) Then
            '        'Add 15 minutes
            '        rndMinutes += 15
            '    Else
            '        'add ten minutes
            '        rndMinutes += 10
            '    End If
            '    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddMinutes(rndMinutes)).DateTime.TimeOfDay.TotalDays
            '    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 15, 0)).TotalDays > 0 Then
            '        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 15, 0))
            '        rndMinutes = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalMinutes) / 5, 0, MidpointRounding.AwayFromZero) * 5)
            '        If rndMinutes > CInt(dtMin.TimeOfDay.TotalMinutes) Then
            '            'substract 15
            '            rndMinutes -= 15
            '        Else
            '            rndMinutes -= 10
            '        End If
            '        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddMinutes(rndMinutes)).DateTime.TimeOfDay.TotalDays
            '    Else
            '        ' set to 0
            '        mPane.YAxis.Scale.Min = 0
            '    End If

            '    mPane.YAxis.Scale.MinorUnit = DateUnit.Minute
            '    mPane.YAxis.Scale.MinorStep = 15
            '    mPane.YAxis.MinorTic.IsAllTics = False
            '    mPane.YAxis.Scale.MajorUnit = DateUnit.Hour
            '    mPane.YAxis.Scale.MajorStep = 1
            '    mPane.YAxis.MajorTic.IsAllTics = False
            'Else
            Try
                mPane.YAxis.Scale.Format = "mm:ss"
                Dim tsSpan As TimeSpan = New XDate(mMaxValue).DateTime.Subtract(New XDate(mMinValue))
                If tsSpan.TotalMinutes > 30 Then
                    'scale 10 minutes 
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 10, 0)).TotalDays > 0 Then
                        ' scale to that
                        Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 10, 0))
                        mPane.YAxis.Scale.Min = New XDate(dtMin)
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If
                    Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 10, 0))
                    mPane.YAxis.Scale.Max = New XDate(dtMax)
                    If dtMax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 60
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 5
                ElseIf tsSpan.TotalMinutes > 15 Then
                    'scale 5 minutes 
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 5, 0))
                    Dim rndMinutes As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalMinutes) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndMinutes < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'add 5
                        rndMinutes += 5
                    Else
                        'keep
                        rndMinutes += 0
                    End If
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddMinutes(rndMinutes)).DateTime.TimeOfDay.TotalDays
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 5, 0)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 5, 0))
                        rndMinutes = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndMinutes > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 5
                            rndMinutes -= 5
                        Else
                            rndMinutes -= 0
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddMinutes(rndMinutes)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If
                ElseIf tsSpan.TotalMinutes > 10 Then
                    'scale 2 minutes
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 120))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 120 seconds
                        rndSeconds += 120
                    Else
                        'add 115 seconds
                        rndSeconds += 115
                    End If
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 120)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 60))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 120
                            rndSeconds -= 120
                        Else
                            'substract 115
                            rndSeconds -= 115
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If
                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 1
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 1
                    mPane.YAxis.MajorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorStep = 1
                ElseIf tsSpan.TotalMinutes > 5 Then
                    'scale 1 minute
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 60))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 60, 0, MidpointRounding.AwayFromZero) * 60)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 30 seconds
                        rndSeconds += 60
                    Else
                        'add 25 seconds
                        rndSeconds += 55
                    End If
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 60)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 60))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 60, 0, MidpointRounding.AwayFromZero) * 60)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 60
                            rndSeconds -= 60
                        Else
                            'substract 55
                            rndSeconds -= 55
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If
                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 30
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 1
                    mPane.YAxis.MajorTic.IsAllTics = True
                ElseIf tsSpan.TotalMinutes > 3 Then
                    'Scale 30 seconds
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 30))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 30 seconds
                        rndSeconds += 30
                    Else
                        'add 25 seconds
                        rndSeconds += 25
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 30)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 30))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 30
                            rndSeconds -= 30
                        Else
                            'substract 25
                            rndSeconds -= 25
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If
                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 1
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 1
                    mPane.YAxis.MajorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorStep = 1
                ElseIf tsSpan.TotalMinutes > 2 Then
                    'Scale 30 seconds
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 30))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 30, 0, MidpointRounding.AwayFromZero) * 30)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 15 seconds
                        rndSeconds += 30
                    Else
                        'add 10 seconds
                        rndSeconds += 25
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 30)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 30))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 30, 0, MidpointRounding.AwayFromZero) * 30)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 10
                            rndSeconds -= 30
                        Else
                            'substract 15
                            rndSeconds -= 25
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If

                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 5
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 0.25
                    mPane.YAxis.MajorTic.IsAllTics = True
                ElseIf tsSpan.TotalMinutes > 1 Then
                    'Scale 10 seconds
                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 10))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 5 seconds
                        rndSeconds += 10
                    Else
                        'add 10 seconds
                        rndSeconds += 0
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 10)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 10))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 5
                            rndSeconds -= 10
                        Else
                            rndSeconds -= 0
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If

                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 1
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 1
                    mPane.YAxis.MajorTic.IsAllTics = True
                Else
                    'Scale 5 seconds

                    Dim dtmax As DateTime = New DateTime(New XDate(mMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 5))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 5 seconds
                        rndSeconds += 5
                    Else
                        'add 10 seconds
                        rndSeconds += 0
                    End If
                    mPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        mPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(mMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 5))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 5
                            rndSeconds -= 5
                        Else
                            rndSeconds -= 0
                        End If
                        mPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        mPane.YAxis.Scale.Min = 0
                    End If

                    mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    mPane.YAxis.Scale.MinorStep = 1
                    mPane.YAxis.MinorTic.IsAllTics = True
                    mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    mPane.YAxis.Scale.MajorStep = 1
                    mPane.YAxis.MajorTic.IsAllTics = True

                    'Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 5))
                    'mPane.YAxis.Scale.Max = New XDate(dtMax)
                    'mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    'mPane.YAxis.Scale.MinorStep = 1
                    'mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    'mPane.YAxis.Scale.MajorStep = 1
                End If

            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            'End If

            mPane.Rect = zgFrames.ClientRectangle
            zgFrames.GraphPane = mPane
            zgFrames.AxisChange()
            If bHour Then mPane.YAxis.ResetAutoScale(zgFrames.GraphPane, zgFrames.CreateGraphics)
            'mPane.YAxis.Scale.Format = "hh:mm:ss"
            zgFrames.Refresh()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub UpdateFrameGraph()
        Try
            Dim nI As New UpdateFrameGraphDelegate(AddressOf dUpdateFrameGraph)
            Dim result As IAsyncResult = Me.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Function zgFrames_PointValueEvent(sender As ZedGraph.ZedGraphControl, pane As ZedGraph.GraphPane, curve As ZedGraph.CurveItem, iPt As System.Int32) As System.String Handles zgFrames.PointValueEvent
        With curve.Points(iPt)
            Dim dtC As XDate = New XDate(.X)
            Dim tsF As XDate = New XDate(.Y)
            If tsF.DateTime.TimeOfDay.TotalMilliseconds > 0 Then
                Return curve.Label.Text & Chr(32) & dtC.ToString & Chr(32) & "completed " & iPt + 1 & "% tpf: " & FormatTimeSpan(tsF.DateTime.TimeOfDay)
            Else
                Return curve.Label.Text & Chr(32) & dtC.ToString & Chr(32) & "completed " & iPt + 1 & "%"
            End If
        End With
    End Function
#End Region

#Region "Eta timer"
    Private Delegate Sub SetEtaDelegate()
    Private Sub dSetEta()
        Try
            SyncLock lvLive
                lvLive.BeginUpdate()
                lvLive.SuspendLayout()
                For Each item As ListViewItem In lvLive.Items
                    If modMySettings.ConvertUTC Then
                        If Not CType(item.Tag, clsWU).Eta = #1/1/2000# Then
                            If CType(item.Tag, clsWU).Eta < DateTime.Now Then
                                item.SubItems(8).Text = "00:00:00"
                            Else
                                item.SubItems(8).Text = FormatTimeSpan(CType(item.Tag, clsWU).Eta.Subtract(DateTime.Now))
                            End If
                        End If
                    Else
                        If Not CType(item.Tag, clsWU).Eta = #1/1/2000# Then
                            If CType(item.Tag, clsWU).Eta < DateTime.Now Then
                                item.SubItems(8).Text = "00:00:00"
                            Else
                                item.SubItems(8).Text = FormatTimeSpan(CType(item.Tag, clsWU).Eta.Subtract(DateTime.Now))
                                'item.SubItems(8).Text = FormatTimeSpan(CType(item.Tag, clsWU).Eta.Subtract(DateTime.UtcNow))
                            End If
                        End If
                    End If
                Next
                lvLive.EndUpdate()
                lvLive.ResumeLayout()
            End SyncLock
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub SetEta()
        Try
            Dim nI As New SetEtaDelegate(AddressOf dSetEta)
            Dim result As IAsyncResult = lvLive.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            lvLive.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tEta_Tick(sender As System.Object, e As System.EventArgs) Handles tEta.Tick
        Try
            tEta.Enabled = False
            SetEta()
            tEta.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Refresh"
    Private Sub RefreshView()
        Try
            InitListView()
            UpdateTPQueueText()
            If Not scLiveGraph.Panel2Collapsed Then
                If ReferenceEquals(tcFramesLogQueueTab, tpFramesGraph) Then
                    FillChklbWU()
                    UpdateFrameGraph()
                ElseIf ReferenceEquals(tcFramesLogQueueTab, tpQueue) Then
                    UpdateQueue()
                ElseIf ReferenceEquals(tcFramesLogQueueTab, tpLog) Then
                    UpdateLog()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "delegatefactory event handlers"
    Private Sub HandleParserFailed(sender As Object, e As MyEventArgs.ParserFailedEventArgs)

    End Sub
    Private Sub HandleParserCompleted(sender As Object, e As MyEventArgs.ParserCompletedEventArgs)
        Try
            WriteLog("Logparser completed with success")
            RefreshView()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub HideInactiveMessageStripChanged(sender As Object, e As MyEventArgs.HideInactiveMessageStripArgs)
        Try
            If Not e.Hide Then
                delegateFactory.SetMessageStripVisibility(Me, True)
            Else
                If Not delegateFactory.dActiveTimers.ContainsKey(Me.sStripMessage) Then
                    delegateFactory.SetMessageStripVisibility(Me, False)
                Else
                    'when timer is elapsed it checks hideinactive, should hide automaticly
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub MessageStripEnabledChanged(sender As Object, e As MyEventArgs.MessageStripEnabledArgs)
        Try
            If ReferenceEquals(sender, Me) Then Exit Sub ' ignore own notifications
            If e.Enabled Then
                If delegateFactory.IsMessageStipVisible(Me) AndAlso delegateFactory.IsMessageStripClear(Me) AndAlso e.HideInactive Then
                    delegateFactory.SetMessageStripVisibility(Me, False)
                ElseIf delegateFactory.IsMessageStipVisible(Me) AndAlso e.HideInactive And Not delegateFactory.dActiveTimers.ContainsKey(Me.sStripMessage) Then
                    Dim hideTimer As New MessageStripTimerFactory(Me.sStripMessage)
                    dActiveTimers.Add(Me.sStripMessage, hideTimer)
                    AddHandler hideTimer.timerElapsed, AddressOf delegateFactory.MessageStripFactory_TimerElapsed
                ElseIf Not e.HideInactive AndAlso Not delegateFactory.IsMessageStipVisible(Me) Then
                    delegateFactory.SetMessageStripVisibility(Me, True)
                End If
            Else
                delegateFactory.SetMessageStripVisibility(Me, False)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
   Private Sub EOC_EnabledChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
        Try
            If ReferenceEquals(sender, Me) Then Exit Sub : If Not Me.IsHandleCreated Then Exit Sub

            If delegateFactory.StatusStripVisible(sStripEOC) <> e.Enabled Then
                delegateFactory.StatusStripVisible(sStripEOC) = e.Enabled
            End If

            If delegateFactory.StatusStripVisible(sStripEoc2) <> e.Enabled Then
                delegateFactory.StatusStripVisible(sStripEoc2) = e.Enabled
            End If

            'Try
            '    Dim nI As New SetEocStripVisibilityDelegate2(AddressOf dSetEocStripVisibility2)
            '    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEocStripVisibility2")
            '    Dim result As IAsyncResult = sStripEoc2.BeginInvoke(nI, {e.Enabled})
            '    While Not result.IsCompleted
            '        Application.DoEvents()
            '    End While
            '    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEocStripVisibility2")
            '    sStripEoc2.EndInvoke(result)
            '    result.AsyncWaitHandle.Close()
            'Catch ex As Exception
            '    WriteError(ex.Message, Err)
            '    Exit Try
            'End Try

            'Try
            '    Dim nI As New SetEocStripVisibilityDelegate(AddressOf dSetEocStripVisibility)
            '    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEocStripVisibility")
            '    Dim result As IAsyncResult = sStripEOC.BeginInvoke(nI, {e.Enabled})
            '    While Not result.IsCompleted
            '        Application.DoEvents()
            '    End While
            '    WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEocStripVisibility")
            '    sStripEOC.EndInvoke(result)
            '    result.AsyncWaitHandle.Close()
            'Catch ex As Exception
            '    WriteError(ex.Message, Err)
            '    Exit Try
            'End Try

            If Not e.Enabled Then
                RemoveHandler delegateFactory.EOC_UpdateRecieved, AddressOf EocUpdateHandler
            Else
                AddHandler delegateFactory.EOC_UpdateRecieved, AddressOf EocUpdateHandler
                bSurpress = True
                Call EocUpdateHandler(Me, New MyEventArgs.EocUpdateArgs(EOCInfo.primaryAccount))
                bSurpress = False
            End If

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub EOC_ViewTeamChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
        Try
            If ReferenceEquals(sender, Me) Then Exit Sub : If Not sStripEoc2.IsHandleCreated Then Exit Sub
            bManual = True
            ViewTeamStatusToolStripMenuItem.Checked = e.Enabled
            bManual = False
            If delegateFactory.StatusStripVisible(sStripEoc2) <> e.Enabled Then
                delegateFactory.StatusStripVisible(sStripEoc2) = e.Enabled
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub EOC_ViewUserChanged(sender As Object, e As MyEventArgs.EOCEnabledChangedArgs)
        Try
            If ReferenceEquals(sender, Me) Then Exit Sub : If Not sStripEOC.IsHandleCreated Then Exit Sub
            bManual = True
            ViewUserStatusToolStripMenuItem.Checked = e.Enabled
            bManual = False
            If delegateFactory.StatusStripVisible(sStripEOC) <> e.Enabled Then
                delegateFactory.StatusStripVisible(sStripEOC) = e.Enabled
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub DefaultStatisticsChanged(sender As Object, e As MyEventArgs.DefaultStatisticsArgs)
        Try
            If ReferenceEquals(sender, Me) Then Exit Sub : If Not Me.IsHandleCreated Then Exit Sub : If bManual Then Exit Sub
            bStatisticsChanging = True
            If e.DefaultStatistics = modMySettings.defaultStatisticsEnum.Current Then
                CurrentToolStripMenuItem.Checked = True
                OverallToolStripMenuItem.Checked = False
            Else
                CurrentToolStripMenuItem.Checked = False
                OverallToolStripMenuItem.Checked = True
            End If
            bStatisticsChanging = False
            UpdateStatisticsStrip()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Update statistics"
    Private Delegate Sub UpdateStatisticsStripDelegate()
    Private Sub dUpdateStatisticsStrip()
        Try
            If OverallToolStripMenuItem.Checked Then
                tsStatistics.Text = "Total WUs: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_Completed & " Total credit: " & clsStatistics.clsHistoricalStatistics.Statistics.TotalCredit & " WU's Failed: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_EUE & " Succesrate: " & clsStatistics.clsHistoricalStatistics.Statistics.SuccesRate & " Total computation time: " & clsStatistics.clsHistoricalStatistics.Statistics.ComputationTime & " Avg PPD: " & clsStatistics.clsHistoricalStatistics.Statistics.AvgPPD
            Else
                tsStatistics.Text = "Total WUs: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Wu_Completed & " Total credit: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.TotalCredit & " WU's failed: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Wu_EUE & " Succesrate: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRate & " Total computation time: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.ComputationTime & " Avg PPD: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.AvgPPD
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UpdateStatisticsStrip()
        If Not Me.IsHandleCreated Then Exit Sub
        Try
            Dim nI As New UpdateStatisticsStripDelegate(AddressOf dUpdateStatisticsStrip)
            Dim result As IAsyncResult = Me.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "toolstrip menu handling"
    Private Sub HowDoIEnableTheseToolsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles HowDoIEnableTheseToolsToolStripMenuItem.Click
        Try
            Tools.ShowDownload(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub DiagnosticsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DiagnosticsToolStripMenuItem.Click
        Try
            Dim nF As New Form
            nF.FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow
            nF.Text = "Diagnostics"
            nF.Controls.Add(New RichTextBox)
            With nF.Controls(0)
                .Text = modMAIN.Diagnostic(False)
                .Dock = DockStyle.Fill
            End With
            nF.Size = New Size(400, 600)
            nF.ShowDialog(Me)
            nF.Dispose()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ProjectInfoListToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProjectInfoListToolStripMenuItem.Click
        Try
            ProjectInfo.ShowList(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ProjectInfoGraphCalculatorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProjectInfoGraphCalculatorToolStripMenuItem.Click
        Try
            ProjectInfo.ShowGraph(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub SignatureToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SignatureToolStripMenuItem.Click
        EOCInfo.showSigs()
    End Sub
    Private Sub HistoryToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles HistoryToolStripMenuItem.Click
        Try
            If delegateFactory.IsFormVisible(History) Then
                History.HideForm()
                HistoryToolStripMenuItem.Text = "Show History"
            Else
                History.ShowForm()
                HistoryToolStripMenuItem.Text = "Hide History"
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub WhatsTheDifferenceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles WhatsTheDifferenceToolStripMenuItem.Click
        MsgBox("Overal is based on all stored work units, calculating ppd from the first known unit to the the last one." & Environment.NewLine & "Current statistics are those which match your current client setup ( local and remote clients and slot configuration )." & Environment.NewLine & Environment.NewLine & "* note that when using a filter the statistics will be based on the filtered results", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle))
    End Sub
    Private Sub IconToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IconToolStripMenuItem.Click
        Try
            EOCInfo.IconVisible = Not IconToolStripMenuItem.Checked
            modMySettings.ShowEOCIcon = IconToolStripMenuItem.Checked
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub EOCToolStripMenuItem_DropDownOpening(sender As Object, e As System.EventArgs) Handles EOCToolStripMenuItem.DropDownOpening
        Try
            For Each tItem As ToolStripItem In EOCToolStripMenuItem.DropDownItems
                tItem.Enabled = Not modMySettings.DisableEOC
            Next
            IconToolStripMenuItem.Checked = EOCInfo.IconVisible
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub StartPageToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StartPageToolStripMenuItem.Click
        Try
            Process.Start("http://folding.extremeoverclocking.com/")
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub TeamStatisticsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TeamStatisticsToolStripMenuItem.Click
        Try
            If EOCInfo.primaryAccount.LastUpdate.IsEmpty Then
                SetMessage("The teamID isn't known, can't open team statistics")
            Else
                Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&t=" & EOCInfo.primaryAccount.LastUpdate.Update.Team.TeamID)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UserStatisticsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UserStatisticsToolStripMenuItem.Click
        Try
            If EOCInfo.primaryAccount.LastUpdate.IsEmpty Then
                SetMessage("The userID isn't known, can't open team statistics")
            Else
                Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&u=" & EOCInfo.primaryAccount.LastUpdate.Update.User.UserID)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ViewUserStatusToolStripMenuItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ViewUserStatusToolStripMenuItem.CheckedChanged
        Try
            If bManual Then Exit Sub
            modMySettings.viewEocUser = ViewUserStatusToolStripMenuItem.Checked
            If Not modMySettings.DisableEOC Then
                sStripEOC.Visible = ViewUserStatusToolStripMenuItem.Checked
            Else
                sStripEOC.Visible = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ViewTeamStatusToolStripMenuItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ViewTeamStatusToolStripMenuItem.CheckedChanged
        Try
            If bManual Then Exit Sub
            modMySettings.viewEocTeam = ViewTeamStatusToolStripMenuItem.Checked
            If Not modMySettings.DisableEOC Then
                sStripEoc2.Visible = ViewTeamStatusToolStripMenuItem.Checked
            Else
                sStripEoc2.Visible = False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub CurrentToolStripMenuItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CurrentToolStripMenuItem.CheckedChanged
        Try
            If bManual Or bStatisticsChanging Then Exit Sub
            bStatisticsChanging = True
            OverallToolStripMenuItem.Checked = Not CurrentToolStripMenuItem.Checked
            'UpdateStatisticsStrip()
            If CurrentToolStripMenuItem.Checked Then
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current
            Else
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Overall
            End If
            bStatisticsChanging = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub OverallToolStripMenuItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OverallToolStripMenuItem.CheckedChanged
        Try
            If bManual Or bStatisticsChanging Then Exit Sub
            bStatisticsChanging = True
            CurrentToolStripMenuItem.Checked = Not OverallToolStripMenuItem.Checked
            UpdateStatisticsStrip()
            bStatisticsChanging = False
            If CurrentToolStripMenuItem.Checked Then
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current
            Else
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Overall
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub GeneralToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GeneralToolStripMenuItem.Click
        Try
            Dim lCNames As New List(Of String)
            For xInt As Int32 = 1 To Clients.Clients.Count - 1
                lCNames.Add(Clients.Clients(xInt).ClientName)
            Next
            If modMySettings.ShowOptionsForm(Me) = Windows.Forms.DialogResult.OK Then
                Dim bReparse As Boolean = False
                If lCNames.Count = Clients.Clients.Count - 1 Then
                    For xInt As Int32 = 1 To Clients.Clients.Count - 1
                        If lCNames(xInt - 1) <> Clients.Clients(xInt).ClientName Then
                            bReparse = True
                            Exit For
                        End If
                    Next
                Else
                    bReparse = True
                End If
                If bReparse Then
                    WriteLog("Client collection changed, parsing log files")
                    Dim dtStart As DateTime = DateTime.Now
                    If Clients.ParseLogs(False, Nothing, False) Then
                        WriteDebug("Logparser finished, took " & FormatTimeSpan(DateTime.Now.Subtract(dtStart), True))
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub NotificationsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles NotificationsToolStripMenuItem.Click
        Try
            Dim bUtc As Boolean = modMySettings.ConvertUTC
            modMySettings.ShowNotifyForm(Me)
            If modMySettings.ConvertUTC <> bUtc Then
                MsgBox("Changes will take effect after selecting a new Filter, refreshing the current one, or by doing an update", CType(MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, MsgBoxStyle))
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub CloseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        If modMySettings.MainForm = modMySettings.eMainForm.Live Then
            Me.Close()
        Else
            History.Close()
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        about.ShowAbout(Me)
    End Sub
    Private Sub LicenseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LicenseToolStripMenuItem.Click
        license.ShowLicense(Me)
    End Sub
    Private Sub GraphOptionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GraphOptionsToolStripMenuItem.Click
        modMySettings.clsGraphSettings.ShowOptions(Me)
    End Sub
    Private Sub MinimizeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MinimizeToolStripMenuItem.Click
        If modMySettings.MinimizeToTray Then
            HideForm()
            Me.WindowState = FormWindowState.Minimized
        Else
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub
    Private Sub LogMessagesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogMessagesToolStripMenuItem.Click
        Try
            Logwindow.ShowDebugWindow()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub FrameGraphsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FrameGraphsToolStripMenuItem.Click
        Try
            If Not scLiveGraph.Panel2Collapsed AndAlso ReferenceEquals(tcFramesLogQueue.SelectedTab, tpFramesGraph) Then Exit Sub
            scLiveGraph.Panel2Collapsed = False
            tcFramesLogQueue.SelectedTab = tpFramesGraph
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub HardwareSensorsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles HardwareSensorsToolStripMenuItem.Click
        Try
            If bManual Then Exit Sub
            scHardware.Panel2Collapsed = Not scHardware.Panel2Collapsed
            If Not scHardware.Panel2Collapsed Then
                UpdateSensors()
            Else
                DisableSensors()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub LogToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogToolStripMenuItem.Click
        Try
            If Not scLiveGraph.Panel2Collapsed AndAlso ReferenceEquals(tcFramesLogQueue.SelectedTab, tpLog) Then Exit Sub
            scLiveGraph.Panel2Collapsed = False
            tcFramesLogQueue.SelectedTab = tpLog
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub QueuedWusToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles QueuedWusToolStripMenuItem.Click
        Try
            If Not scLiveGraph.Panel2Collapsed AndAlso ReferenceEquals(tcFramesLogQueue.SelectedTab, tpQueue) Then Exit Sub
            scLiveGraph.Panel2Collapsed = False
            tcFramesLogQueue.SelectedTab = tpQueue
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ToggleETAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ToggleETAToolStripMenuItem.Click
        Try
            If modMySettings.live_etastyle = modMySettings.eEtaStyle.ShowDate Then
                modMySettings.live_etastyle = modMySettings.eEtaStyle.ShowTimeToGo
            Else
                modMySettings.live_etastyle = modMySettings.eEtaStyle.ShowDate
            End If
            InitListView()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UpdateToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UpdateToolStripMenuItem.Click
        Try
            Clients.UpdateLogs(Me)
            'Me.UseWaitCursor = True
            'If Clients.ParseLogs(False, Me, False) Then
            '    WriteLog("Refreshing view")
            '    RefreshView()
            'Else
            '    WriteLog("Logparser failed", eSeverity.Important)
            'End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            Me.UseWaitCursor = False
        End Try
    End Sub
    Private Sub FileToolStripMenuItem_DropDownOpening(sender As System.Object, e As System.EventArgs) Handles FileToolStripMenuItem.DropDownOpening
        Try
            If delegateFactory.IsFormVisible(History) Then
                HistoryToolStripMenuItem.Text = "Hide History"
            Else
                HistoryToolStripMenuItem.Text = "Show History"
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Update queued wu count"
    Private Delegate Sub UpdateTPQueueTextDelegate()
    Private Sub dUpdateTPQueueText()
        tpQueue.Text = "Queued work units (" & Clients.QueuedWorkUnits.Count & ")"
    End Sub
    Private Sub UpdateTPQueueText()
        Try
            Dim nI As New UpdateTPQueueTextDelegate(AddressOf dUpdateTPQueueText)
            Dim result As IAsyncResult = tpQueue.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            tpQueue.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "tcFramesLogQueue"
    Private Delegate Function tcFramesLogQueueTabDelegate() As TabPage
    Private Function dTCFramesLogQueueTab() As TabPage
        Return tcFramesLogQueue.SelectedTab
    End Function
    Private Function tcFramesLogQueueTab() As TabPage
        Try
            Dim nI As New tcFramesLogQueueTabDelegate(AddressOf dTCFramesLogQueueTab)
            Dim result As IAsyncResult = tcFramesLogQueue.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Dim bRes As TabPage = CType(tcFramesLogQueue.EndInvoke(result), TabPage)
            result.AsyncWaitHandle.Close()
            Return bRes
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New TabPage("error")
        End Try
    End Function
    Private Sub tcFramesLogQueue_Selected(sender As System.Object, e As System.Windows.Forms.TabControlEventArgs) Handles tcFramesLogQueue.Selected
        Try
            If bManual Then Exit Sub
            If e.Action = TabControlAction.Selected Then
                If ReferenceEquals(e.TabPage, tpFramesGraph) Then
                    scZgFramesSelection.Panel1Collapsed = False
                    FillChklbWU()
                    UpdateFrameGraph()
                ElseIf ReferenceEquals(e.TabPage, tpLog) Then
                    scZgFramesSelection.Panel1Collapsed = True
                    InitLog()
                ElseIf ReferenceEquals(e.TabPage, tpQueue) Then
                    scZgFramesSelection.Panel1Collapsed = True
                    UpdateQueue()
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Queued work units"
    Private mSelectedQueueID As String = String.Empty
    Private mQueueIndex As Int32 = -1
#Region "listView Queue"
    Private Delegate Sub UpdateQueueDelegate()
    Private Sub dUpdateQueue()
        Try
            bManual = True
            lvQueue.BeginUpdate()
            lvQueue.Items.Clear()
            SyncLock Clients.QueuedWorkUnits
                For Each WU As clsWU In Clients.QueuedWorkUnits
                    Dim nI As New ListViewItem(WU.ClientName)
                    nI.SubItems.Add(WU.Slot)
                    nI.SubItems.Add(WU.PRCG)
                    If WU.utcStarted = #1/1/2000# Then
                        'Not started, make green
                        nI.BackColor = Color.LightGreen
                    Else
                        If ProjectInfo.KnownProject(WU.Project) Then
                            Dim dtFinal As DateTime = WU.utcStartDownload.AddDays(CDbl(ProjectInfo.Project(WU.Project).FinalDeadline))
                            If modMySettings.ConvertUTC Then
                                nI.SubItems.Add(TimeZoneInfo.ConvertTimeFromUtc(dtFinal, TimeZoneInfo.Local).ToString(CultureInfo.CurrentCulture))
                            Else
                                nI.SubItems.Add(dtFinal.ToString(CultureInfo.CurrentCulture))
                            End If
                        Else
                            nI.SubItems.Add("")
                        End If
                        If modMySettings.ConvertUTC Then
                            nI.SubItems.Add(WU.dtStartUpload.ToString(CultureInfo.CurrentCulture))
                        Else
                            nI.SubItems.Add(WU.utcStartUpload.ToString(CultureInfo.CurrentCulture))
                        End If
                        If ProjectInfo.KnownProject(WU.Project) Then
                            Dim Worth As ProjectInfo.sProjectPPD = ProjectInfo.GetEffectivePPD_sqrt(WU.utcStartDownload, DateTime.UtcNow, WU.Project)
                            nI.SubItems.Add(FormatPPD(Worth.Credit))
                            nI.SubItems.Add(FormatPPD(Worth.PPD))
                        Else
                            nI.SubItems.Add("-")
                            nI.SubItems.Add("-")
                        End If
                        If Not WU.CoreStatus.Contains("FINISHED_UNIT") Then
                            nI.BackColor = Color.Red
                        End If
                    End If
                    nI.Tag = WU.ClientName & "##" & WU.Slot & "##" & WU.unit & "##" & WU.utcDownloaded.ToString("s")
                    lvQueue.Items.Add(nI)
                    If CStr(nI.Tag) = mSelectedQueueID Then
                        mQueueIndex = lvQueue.Items.IndexOf(nI)
                    End If
                Next
                If mQueueIndex > -1 Then
                    lvQueue.Items(mQueueIndex).Selected = True
                End If
                If Not scQueueLogMain.Panel2Collapsed Then
                    UpdateLogQueue()
                End If
            End SyncLock
            lvQueue.EndUpdate()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub UpdateQueue()
        Try
            Dim nI As New UpdateQueueDelegate(AddressOf dUpdateQueue)
            Dim result As IAsyncResult = tpQueue.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            tpQueue.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Queue log"
    Private Delegate Sub UpdateLogQueueDelegate()
    Private Sub dUpdateLogQueue()
        Try
            If lvQueue.Items.Count > 0 AndAlso lvQueue.SelectedItems.Count = 0 Then
                lvQueue.Items(0).Selected = True
                mQueueIndex = 0
            End If
            If lvQueue.SelectedItems.Count > 0 Then
                Dim sTmp As String = CStr(lvQueue.Items(mQueueIndex).Tag)
                Dim strArr() As String = sTmp.Split({"##"}, StringSplitOptions.RemoveEmptyEntries)
                Dim tClient As String = strArr(0)
                Dim tSlot As String = strArr(1)
                Dim tUnit As String = strArr(2)
                Dim tDownload As DateTime = DateTime.Parse(strArr(3))
                WriteDebug("Loading data for selected wu")
                Dim mQueuedWU As clsWU = sqdata.QueuedWorkUnit(tClient, tSlot, tUnit, tDownload)
                If modMySettings.ConvertUTC Then
                    Dim nText As String = mQueuedWU.ActiveLogfile(True, True).TrimEnd(CChar(vbLf))
                    If nText.Contains(txtLogQueue.Text) And txtLogQueue.TextLength > 0 Then
                        txtLogQueue.AppendText(nText.Replace(txtLogQueue.Text, ""))
                    Else
                        txtLogQueue.Text = nText
                    End If
                Else
                    Dim nText As String = mQueuedWU.ActiveLogfile(True, True).TrimEnd(CChar(vbLf))
                    If nText.Contains(txtLogQueue.Text) And txtLogQueue.TextLength > 0 Then
                        txtLogQueue.AppendText(nText.Replace(txtLogQueue.Text, ""))
                    Else
                        txtLogQueue.Text = nText
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UpdateLogQueue()
        Try
            Dim nI As New UpdateLogQueueDelegate(AddressOf dUpdateLogQueue)
            Dim result As IAsyncResult = txtLogQueue.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            txtLogQueue.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    Private Sub cmdToggleQueueLog_Click(sender As System.Object, e As System.EventArgs) Handles cmdToggleQueueLog.Click
        Try
            scQueueLogMain.Panel2Collapsed = Not scQueueLogMain.Panel2Collapsed
            If Not scQueueLogMain.Panel2Collapsed Then
                UpdateQueue()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvQueue_ItemSelectionChanged(sender As Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvQueue.ItemSelectionChanged
        If bManual Then Exit Sub
        Try
            If Not e.IsSelected Then
                txtLogQueue.Clear()
                mSelectedQueueID = ""
            Else
                mSelectedQueueID = CStr(e.Item.Tag)
                UpdateLogQueue()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "Update log"
    Private Sub InitLog()
        Try
            bManual = True
            txtLog.Clear()
            If lvLive.SelectedItems.Count > 0 Then
                SyncLock lvLive
                    UpdateLog()
                End SyncLock
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Delegate Sub UpdateLogDelegate()
    Private Sub dUpdateLog()
        Try
            With CType(lvLive.SelectedItems(0).Tag, clsWU)
                If tsCombinedLog.Checked Then
                    If modMySettings.ConvertUTC Then
                        Dim nText As String = Clients.Client(CType(lvLive.SelectedItems(0).Tag, clsWU).ClientName).CombinedLog(tsWarningsAndErrors.Checked, tsIncludeOtherWUs.Checked).TrimEnd(CChar(vbLf))
                        If nText.Contains(txtLog.Text) And txtLog.TextLength > 0 Then
                            txtLog.AppendText(nText.Replace(txtLog.Text, ""))
                        Else
                            txtLog.Text = nText
                        End If
                    Else
                        Dim nText As String = Clients.Client(CType(lvLive.SelectedItems(0).Tag, clsWU).ClientName).CombinedLogUTC(tsWarningsAndErrors.Checked, tsIncludeOtherWUs.Checked).TrimEnd(CChar(vbLf))
                        If nText.Contains(txtLog.Text) And txtLog.TextLength > 0 Then
                            txtLog.AppendText(nText.Replace(txtLog.Text, ""))
                        Else
                            txtLog.Text = nText
                        End If
                    End If
                Else
                    If modMySettings.ConvertUTC Then
                        Dim nText As String = CType(lvLive.SelectedItems(0).Tag, clsWU).ActiveLogfile(tsWarningsAndErrors.Checked, tsIncludeOtherWUs.Checked).TrimEnd(CChar(vbLf))
                        If nText.Contains(txtLog.Text) And txtLog.TextLength > 0 Then
                            txtLog.AppendText(nText.Replace(txtLog.Text, ""))
                        Else
                            txtLog.Text = nText
                        End If
                    Else
                        Dim nText As String = CType(lvLive.SelectedItems(0).Tag, clsWU).ActiveLogfileUTC(tsWarningsAndErrors.Checked, tsIncludeOtherWUs.Checked).TrimEnd(CChar(vbLf))
                        If nText.Contains(txtLog.Text) And txtLog.TextLength > 0 Then
                            txtLog.AppendText(nText.Replace(txtLog.Text, ""))
                        Else
                            txtLog.Text = nText
                        End If
                    End If
                End If
            End With
            If tsFollowLog.Checked Then
                txtLog.SelectionStart = txtLog.TextLength
                txtLog.ScrollToCaret()
                txtLog.Refresh()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UpdateLog()
        Try
            If Not Me.IsHandleCreated Then Exit Sub
            Dim nI As New UpdateLogDelegate(AddressOf dUpdateLog)
            Dim result As IAsyncResult = Me.BeginInvoke(nI)
            While Not result.IsCompleted
                Application.DoEvents()
            End While
            Me.EndInvoke(result)
            result.AsyncWaitHandle.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region

#Region "EOC status strips"

    Private Delegate Sub SetEocStripVisibilityDelegate(Visible As Boolean)
    Private Sub dSetEocStripVisibility(Visible As Boolean)
        sStripEOC.Visible = Visible
    End Sub

    Private Delegate Sub SetEocStripVisibilityDelegate2(Visible As Boolean)
    Private Sub dSetEocStripVisibility2(Visible As Boolean)
        sStripEoc2.Visible = Visible
    End Sub

    Private Delegate Sub SetEocDelegate(Message As String)
    Private Sub dSetEoc(Message As String)
        sStripEOC.Items(0).Text = Message
    End Sub

    Private Delegate Sub SetEocDelegate2(Message As String)
    Private Sub dSetEoc2(Message As String)
        sStripEoc2.Items(0).Text = Message
    End Sub

    Friend Sub EocUpdateHandler(sender As Object, e As MyEventArgs.EocUpdateArgs)
        If Not e.EOCAccount.Equals(EOCInfo.primaryAccount) Then
            WriteDebug("Update recieved for non primary Eoc account, not updating status")
            Exit Sub
        End If
        Try
            Try
                Dim nI As New SetEocStripVisibilityDelegate2(AddressOf dSetEocStripVisibility2)
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEocStripVisibility2")
                Dim result As IAsyncResult = sStripEoc2.BeginInvoke(nI, {True})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEocStripVisibility2")
                sStripEoc2.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Exit Sub
            End Try

            Try
                Dim nI As New SetEocStripVisibilityDelegate(AddressOf dSetEocStripVisibility)
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEocStripVisibility")
                Dim result As IAsyncResult = sStripEOC.BeginInvoke(nI, {True})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEocStripVisibility")
                sStripEOC.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Exit Sub
            End Try

            Dim Message As String = String.Empty
            With e.EOCAccount.LastUpdate.Update
                Message = "EOC Update (" & e.EOCAccount.LastUpdate.Last_Update_LocalTime.ToString(CultureInfo.CurrentCulture) & "): " & .UpdateStatus.Update_Status & " User: " & .User.User_Name & " Total points: " & .User.Points & " 24h Avg: " & .User.Points_24h_Avg & " Points today: " & .User.Points_Today & " Points update: " & .User.Points_Update & " Work units: " & .User.WUs
            End With
            Try
                Dim nI As New SetEocDelegate(AddressOf dSetEoc)
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEoc")
                Dim result As IAsyncResult = sStripEOC.BeginInvoke(nI, {Message})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEoc")
                sStripEOC.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Exit Sub
            End Try

            With e.EOCAccount.LastUpdate.Update
                Message = "Team: " & .Team.TeamName & " Users: " & .Team.Users & " Active: " & .Team.Users_Active & " Points: " & .Team.Points & " 24h Avg: " & .Team.Points_24h_Avg & " Points update: " & .Team.Points_Update & " Work units: " & .Team.WUs
            End With

            Try
                Dim nI As New SetEocDelegate2(AddressOf dSetEoc2)
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- BeginInvoke dSetEoc2")
                Dim result As IAsyncResult = sStripEoc2.BeginInvoke(nI, {Message})
                While Not result.IsCompleted
                    Application.DoEvents()
                End While
                WriteDebug(FormatTimeSpan(DateTime.Now.TimeOfDay, True) & " -- EndInvoke dSetEoc2")
                sStripEoc2.EndInvoke(result)
                result.AsyncWaitHandle.Close()
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Exit Sub
            End Try

        Catch ex As Exception
            WriteError(ex.Message, Err)
            Exit Sub
        End Try
        If Not bSurpress Then SetMessage("EOC update recieved")
    End Sub

#End Region

#Region "Log selection limiters"
    Private Sub tsWarningsAndErrors_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles tsWarningsAndErrors.CheckedChanged
        If bManual Then Exit Sub
        UpdateLog()
    End Sub
    Private Sub tsCombinedLog_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles tsCombinedLog.CheckedChanged
        If bManual Then Exit Sub
        UpdateLog()
    End Sub
    Private Sub tsIncludeOtherWUs_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles tsIncludeOtherWUs.CheckedChanged
        If bManual Then Exit Sub
        UpdateLog()
    End Sub
#End Region

    Private Sub zgFrames_ZoomEvent(sender As ZedGraph.ZedGraphControl, oldState As ZedGraph.ZoomState, newState As ZedGraph.ZoomState) Handles zgFrames.ZoomEvent
        Try
            Dim iYBars As Int32 = sender.GraphPane.YAxis.Scale.CalcMaxLabels(sender.CreateGraphics, sender.GraphPane, 1)
            Dim yMinValue As XDate = sender.GraphPane.YAxis.Scale.Min : Dim yMaxValue As XDate = sender.GraphPane.YAxis.Scale.Max
            Dim iXBars As Int32 = sender.GraphPane.XAxis.Scale.CalcMaxLabels(sender.CreateGraphics, sender.GraphPane, 1)
            Dim xMinValue As XDate = sender.GraphPane.XAxis.Scale.Min : Dim xMaxValue As XDate = sender.GraphPane.XAxis.Scale.Max
            Dim bHour As Boolean = yMaxValue.DateTime.TimeOfDay.TotalHours > 0
            Select Case yMaxValue.DateTime.Subtract(yMinValue.DateTime)
                Case Is > TimeSpan.FromHours(3)

                Case Is > TimeSpan.FromHours(1)

                Case Is > TimeSpan.FromMinutes(10)

                Case Is > TimeSpan.FromSeconds(30)
                    'Scale 10 seconds
                    Dim dtmax As DateTime = New DateTime(New XDate(yMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 10))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 5 seconds
                        rndSeconds += 10
                    Else
                        'add 10 seconds
                        rndSeconds += 0
                    End If
                    sender.GraphPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        sender.GraphPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(yMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 10)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(yMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 10))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 5
                            rndSeconds -= 10
                        Else
                            rndSeconds -= 0
                        End If
                        sender.GraphPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        sender.GraphPane.YAxis.Scale.Min = 0
                    End If

                    sender.GraphPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    sender.GraphPane.YAxis.Scale.MinorStep = 10
                    sender.GraphPane.YAxis.MinorTic.IsAllTics = True
                    sender.GraphPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    sender.GraphPane.YAxis.Scale.MajorStep = 1
                    sender.GraphPane.YAxis.MajorTic.IsAllTics = True
                Case Else
                    'Scale 5 seconds
                    Dim dtmax As DateTime = New DateTime(New XDate(yMaxValue).DateTime.Ticks).Add(New TimeSpan(0, 0, 5))
                    Dim rndSeconds As Int32 = CInt(Math.Round(CInt(dtmax.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                    If rndSeconds < CInt(dtmax.TimeOfDay.TotalSeconds) Then
                        'Add 5 seconds
                        rndSeconds += 5
                    Else
                        'add 10 seconds
                        rndSeconds += 0
                    End If
                    sender.GraphPane.YAxis.Scale.Max = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    If dtmax.Hour > 0 Then
                        sender.GraphPane.YAxis.Scale.Format = "hh:mm:ss"
                        bHour = True
                    End If
                    If New XDate(yMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                        Dim dtMin As DateTime = New DateTime(New XDate(yMinValue).DateTime.Ticks).Subtract(New TimeSpan(0, 0, 5))
                        rndSeconds = CInt(Math.Round(CInt(dtMin.TimeOfDay.TotalSeconds) / 5, 0, MidpointRounding.AwayFromZero) * 5)
                        If rndSeconds > CInt(dtMin.TimeOfDay.TotalSeconds) Then
                            'substract 5
                            rndSeconds -= 5
                        Else
                            rndSeconds -= 0
                        End If
                        sender.GraphPane.YAxis.Scale.Min = New XDate(New DateTime(0).AddSeconds(rndSeconds)).DateTime.TimeOfDay.TotalDays
                    Else
                        ' set to 0
                        sender.GraphPane.YAxis.Scale.Min = 0
                    End If
                    sender.GraphPane.YAxis.Scale.MinorUnit = DateUnit.Second
                    sender.GraphPane.YAxis.Scale.MinorStep = 1
                    sender.GraphPane.YAxis.MinorTic.IsAllTics = True
                    sender.GraphPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                    sender.GraphPane.YAxis.Scale.MajorStep = 1
                    sender.GraphPane.YAxis.MajorTic.IsAllTics = True
            End Select

            sender.GraphPane.AxisChange()

        Catch ex As Exception

        End Try
    End Sub
End Class