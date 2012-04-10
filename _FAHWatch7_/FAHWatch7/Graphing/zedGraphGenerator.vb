Imports ZedGraph
Partial Friend Class zedGraphGenerator
#Region "Frame graphs"
    Friend Overloads Shared Sub GenerateFrameGraph(WorkUnit As clsWU, zg As ZedGraph.ZedGraphControl)
        Try
            Dim ppProject As New ZedGraph.PointPairList
            Dim bHour As Boolean = False, mMinValue As Double = Double.MaxValue, mMaxValue As Double = Double.MinValue
            For xInt As Int32 = 0 To WorkUnit.Frames.Count - 2
                Dim tsFrame As TimeSpan = WorkUnit.Frames(xInt + 1).FrameDT - WorkUnit.Frames(xInt).FrameDT
                If tsFrame.TotalDays > mMaxValue Then mMaxValue = tsFrame.TotalDays
                If tsFrame.TotalDays < mMinValue Then mMinValue = tsFrame.TotalDays
                If tsFrame.Duration.TotalHours > 1 Then
                    bHour = True
                End If
                ppProject.Add(New XDate(WorkUnit.Frames(xInt).FrameDT), tsFrame.TotalDays)
            Next
            zg.MasterPane.PaneList.Clear()
            Dim mPane As New GraphPane
            mPane.Title.Text = WorkUnit.HW & Chr(32) & WorkUnit.ClientName & WorkUnit.Slot & Chr(32) & WorkUnit.PRCG
            mPane.XAxis.Title.Text = "Checkpoints occurance"
            mPane.XAxis.Title.FontSpec.Size = 10
            mPane.XAxis.MinorTic.IsOpposite = False
            mPane.XAxis.MajorTic.IsOpposite = False
            mPane.XAxis.Scale.MinGrace = 0.01
            mPane.XAxis.Scale.MaxGrace = 0.01
            mPane.YAxis.MinorTic.IsOpposite = False
            mPane.YAxis.MajorTic.IsOpposite = False
            mPane.YAxis.MinorGrid.IsVisible = True
            mPane.YAxis.MajorGrid.IsVisible = True
            mPane.YAxis.MajorGrid.IsZeroLine = True
            mPane.YAxis.MajorGrid.PenWidth = 0.1
            mPane.YAxis.MinorGrid.PenWidth = 0.1

            mPane.YAxis.Scale.IsPreventLabelOverlap = True

            mPane.XAxis.Type = AxisType.Date
            mPane.YAxis.Type = AxisType.Date

            mPane.YAxis.Scale.MagAuto = False
            mPane.YAxis.Scale.FormatAuto = False

            If bHour Then
                mPane.YAxis.Scale.Format = "hh:mm:ss"
            Else
                mPane.YAxis.Scale.Format = "mm:ss"
            End If

            Dim tsSpan As TimeSpan = New XDate(mMaxValue).DateTime.Subtract(New XDate(mMinValue))
            Console.WriteLine(WorkUnit.PRCG & " " & tsSpan.ToString)
            If tsSpan.TotalDays > 1 Then
                'Scale 1h
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(1, 0, 0)).TotalDays > 0 Then
                    ' scale to that
                    mPane.YAxis.Scale.Min = New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(1, 0, 0)).TotalDays
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                mPane.YAxis.Scale.Max = New XDate(mMaxValue).DateTime.TimeOfDay.Add(New TimeSpan(1, 0, 0)).TotalDays
                mPane.YAxis.Scale.Format = "hh:mm:ss"
                mPane.YAxis.Scale.MinorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.MinorTic.IsAllTics = False
                mPane.YAxis.Scale.MajorUnit = DateUnit.Hour
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalHours > 1 Then
                'scale 15 minutes 
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 15, 0)).TotalDays > 0 Then
                    ' scale to that
                    mPane.YAxis.Scale.Min = New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 15, 0)).TotalDays
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                mPane.YAxis.Scale.Max = New XDate(mMaxValue).DateTime.TimeOfDay.Add(New TimeSpan(0, 15, 0)).TotalDays
                mPane.YAxis.Scale.Format = "hh:mm:ss"
                mPane.YAxis.Scale.MinorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MinorStep = 1
                mPane.YAxis.MinorTic.IsAllTics = False
                mPane.YAxis.Scale.MajorUnit = DateUnit.Hour
                'prevent majorstep?
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 30 Then
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
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 60
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 15 Then
                'scale 5 minutes 
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 5, 0)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 5, 0))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 5, 0))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 60
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 10 Then
                'scale 1 minute
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 1, 0)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 1, 0))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 1, 0))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 5 Then
                'scale 30 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 30)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 30))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 30))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 3 Then
                'Scale 15 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 15)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 15))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 15))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 2 Then
                'Scale 15 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 15)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 15))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 15))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 1 Then
                'Scale 10 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 10)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 10))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 10))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalSeconds > 30 Then
                'Scale 5 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 5))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 5))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 5
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            Else
                'Scale 5 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 5))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 5))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 1
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            End If
            mPane.YAxis.Title.Text = "Time per frame ( discarding idle time )"
            mPane.YAxis.Title.FontSpec.Size = 10
            mPane.AddCurve("", ppProject, Color.Blue)
            mPane.Rect = zg.ClientRectangle
            zg.GraphPane = mPane
            zg.AxisChange()
            If bHour Then mPane.YAxis.ResetAutoScale(zg.GraphPane, zg.CreateGraphics)
            zg.Refresh()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Overloads Shared Sub GenerateFrameGraph(WorkUnits As List(Of clsWU), zg As ZedGraph.ZedGraphControl)
        Try
            Dim lppProject As New Dictionary(Of clsWU, ZedGraph.PointPairList)
            Dim bHour As Boolean = False, mMinValue As Double = Double.MaxValue, mMaxValue As Double = Double.MinValue
            Dim dtLow As DateTime = DateTime.MaxValue, dtHigh As DateTime = DateTime.MinValue, bGo As Boolean = False
            For Each WorkUnit As clsWU In WorkUnits
                Dim ppProject As New PointPairList, maxFrames As Int32 = 0
                If WorkUnit.Frames.Count = 0 Then
                    GoTo skip
                End If
                For xInt As Int32 = 0 To (WorkUnit.Frames.Count - 1) Step 1
                    bGo = True
                    Dim tsFrame As TimeSpan
                    If xInt = 0 Then
                        tsFrame = WorkUnit.Frames(0).FrameDT - WorkUnit.utcStarted
                    Else
                        tsFrame = WorkUnit.Frames(xInt).FrameDT - WorkUnit.Frames(xInt - 1).FrameDT
                    End If

                    If tsFrame.TotalDays > mMaxValue Then mMaxValue = tsFrame.TotalDays
                    If tsFrame.TotalDays < mMinValue Then mMinValue = tsFrame.TotalDays
                    If tsFrame.Duration.TotalHours > 1 Then
                        bHour = True
                    End If
                    If WorkUnit.Frames(xInt).FrameDT < dtLow Then dtLow = WorkUnit.Frames(xInt).FrameDT
                    If WorkUnit.Frames(xInt).FrameDT > dtHigh Then dtHigh = WorkUnit.Frames(xInt).FrameDT
                    ppProject.Add(New XDate(WorkUnit.Frames(xInt).FrameDT), tsFrame.TotalDays)
                Next
                lppProject.Add(WorkUnit, ppProject)
skip:
            Next


            If lppProject.Count = 0 Or Not bGo Then
                'just clear
                zg.MasterPane.PaneList.Clear()
                zg.GraphPane = New GraphPane
                zg.GraphPane.Rect = zg.ClientRectangle
                zg.AxisChange()
                zg.Invalidate()
                Exit Sub
            End If

            Dim lColors As List(Of Color) = GenerateColorList(lppProject.Count - 1)

            zg.MasterPane.PaneList.Clear()
            Dim mPane As New GraphPane
            'mPane.Title.Text = mWorkUnit.HW & Chr(32) & mWorkUnit.ClientName & mWorkUnit.Slot & Chr(32) & mWorkUnit.PRCG
            mPane.XAxis.Title.Text = "Checkpoints occurance"
            mPane.XAxis.Title.FontSpec.Size = 10
            mPane.XAxis.Scale.FontSpec.Angle = 45
            mPane.XAxis.MinorTic.IsOpposite = False
            mPane.XAxis.MajorTic.IsOpposite = False
            mPane.XAxis.Scale.MinGrace = 0.01
            mPane.XAxis.Scale.MaxGrace = 0.01
            mPane.YAxis.MinorTic.IsOpposite = False
            mPane.YAxis.MajorTic.IsOpposite = False
            mPane.YAxis.MinorGrid.IsVisible = True
            mPane.YAxis.MajorGrid.IsVisible = True
            mPane.YAxis.MajorGrid.IsZeroLine = True
            mPane.YAxis.MajorGrid.PenWidth = 0.1
            mPane.YAxis.MinorGrid.PenWidth = 0.1
            mPane.XAxis.MinorTic.IsAllTics = True
            mPane.YAxis.Scale.IsPreventLabelOverlap = True
            mPane.Legend.FontSpec.Size = 8
            mPane.XAxis.Type = AxisType.Date
            mPane.YAxis.Type = AxisType.Date

            mPane.YAxis.Scale.MagAuto = False
            mPane.YAxis.Scale.FormatAuto = False

            If bHour Then
                mPane.YAxis.Scale.Format = "hh:mm:ss"
            Else
                mPane.YAxis.Scale.Format = "mm:ss"
            End If

            Dim tsSpan As TimeSpan = New XDate(mMaxValue).DateTime.Subtract(New XDate(mMinValue))
            If tsSpan.TotalDays > 1 Then
                'Scale 1h
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(1, 0, 0)).TotalDays > 0 Then
                    ' scale to that
                    mPane.YAxis.Scale.Min = New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(1, 0, 0)).TotalDays
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                mPane.YAxis.Scale.Max = New XDate(mMaxValue).DateTime.TimeOfDay.Add(New TimeSpan(1, 0, 0)).TotalDays
                mPane.YAxis.Scale.Format = "hh:mm:ss"
                mPane.YAxis.Scale.MinorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.MinorTic.IsAllTics = False
                mPane.YAxis.Scale.MajorUnit = DateUnit.Hour
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalHours > 1 Then
                'scale 15 minutes 
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 15, 0)).TotalDays > 0 Then
                    ' scale to that
                    mPane.YAxis.Scale.Min = New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 15, 0)).TotalDays
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                mPane.YAxis.Scale.Max = New XDate(mMaxValue).DateTime.TimeOfDay.Add(New TimeSpan(0, 15, 0)).TotalDays
                mPane.YAxis.Scale.Format = "hh:mm:ss"
                mPane.YAxis.Scale.MinorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MinorStep = 1
                mPane.YAxis.MinorTic.IsAllTics = False
                mPane.YAxis.Scale.MajorUnit = DateUnit.Hour
                'prevent majorstep?
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 30 Then
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
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 60
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 15 Then
                'scale 5 minutes 
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 5, 0)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 5, 0))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 5, 0))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 60
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 10 Then
                'scale 1 minute
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 1, 0)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 1, 0))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 1, 0))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 5
            ElseIf tsSpan.TotalMinutes > 5 Then
                'scale 30 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 30)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 30))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 30))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 3 Then
                'Scale 15 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 15)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 15))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 15))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 30
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 2 Then
                'Scale 15 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 15)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 15))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 15))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalMinutes > 1 Then
                'Scale 10 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 10)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 10))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 10))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 15
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            ElseIf tsSpan.TotalSeconds > 30 Then
                'Scale 5 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 5))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 5))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 5
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            Else
                'Scale 5 seconds
                If New XDate(mMinValue).DateTime.TimeOfDay.Subtract(New TimeSpan(0, 0, 5)).TotalDays > 0 Then
                    ' scale to that
                    Dim dtMin As DateTime = New XDate(mMinValue).DateTime.Subtract(New TimeSpan(0, 0, 5))
                    mPane.YAxis.Scale.Min = New XDate(dtMin)
                Else
                    ' set to 0
                    mPane.YAxis.Scale.Min = 0
                End If
                Dim dtMax As DateTime = New XDate(mMaxValue).DateTime.Add(New TimeSpan(0, 0, 5))
                mPane.YAxis.Scale.Max = New XDate(dtMax)
                mPane.YAxis.Scale.MinorUnit = DateUnit.Second
                mPane.YAxis.Scale.MinorStep = 1
                mPane.YAxis.Scale.MajorUnit = DateUnit.Minute
                mPane.YAxis.Scale.MajorStep = 1
            End If
            mPane.YAxis.Title.Text = "Time per frame ( discarding idle time )"
            mPane.YAxis.Title.FontSpec.Size = 10

            Dim iColor As Int32 = 0
            For Each DictionaryEntry In lppProject
                mPane.AddCurve(DictionaryEntry.Key.PRCG, DictionaryEntry.Value, lColors(iColor))
                iColor += 1
            Next


            mPane.Rect = zg.ClientRectangle
            zg.GraphPane = mPane
            zg.AxisChange()
            If bHour Then mPane.YAxis.ResetAutoScale(zg.GraphPane, zg.CreateGraphics)
            zg.Refresh()

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "TPF/PPD graphs"
    Friend Overloads Shared Sub GenerateTPFAndPPDGraph(WorkUnit As clsWU, zg As ZedGraph.ZedGraphControl)

    End Sub
    Friend Overloads Shared Sub GenerateTPFAndPPDGraph(WorkUnits As List(Of clsWU), zg As ZedGraph.ZedGraphControl)

    End Sub
    Friend Overloads Shared Sub GenerateTPFAndPPDGraph(ProjectStats As clsStatistics.clsProjectStatistics.clsProject, zg As ZedGraph.ZedGraphControl)
        Try
            zg.MasterPane.PaneList.Clear()
            Dim ppdPane As New GraphPane
            ppdPane.Title.Text = "Project: " & ProjectStats.Number & Chr(32) & " ppd: " & FormatPPD(ProjectStats.AvgPPD) & " tpf: " & ProjectStats.AvgTPF & " succes rate: " & ProjectStats.SuccesRate
            ppdPane.YAxis.Title.Text = "PPD"
            ppdPane.Y2Axis.Title.Text = "TPF"
            ppdPane.Y2Axis.Title.IsVisible = True
            ppdPane.YAxis.MajorTic.IsOpposite = False
            ppdPane.YAxis.MinorTic.IsOpposite = False
            ppdPane.YAxis.Scale.Min = 0
            ppdPane.Y2Axis.MajorTic.IsOpposite = False
            ppdPane.Y2Axis.MinorTic.IsOpposite = False
            'ppdPane.YAxis.Scale.Align = AlignP.Inside
            ppdPane.Y2Axis.IsVisible = True
            ppdPane.Y2Axis.Type = AxisType.Date
            ppdPane.Y2Axis.Scale.Min = 0
            ppdPane.Y2Axis.MajorGrid.IsZeroLine = True
            ppdPane.Y2Axis.Scale.FormatAuto = True
            ppdPane.XAxis.MajorTic.IsBetweenLabels = True
            ppdPane.XAxis.Type = AxisType.Text
            ppdPane.XAxis.Scale.FontSpec.Angle = 90
            ppdPane.XAxis.Scale.FontSpec.Size = 8
            ppdPane.XAxis.MinorTic.IsOpposite = False
            ppdPane.XAxis.MajorTic.IsOpposite = False
            ppdPane.XAxis.Scale.Align = AlignP.Outside
            ppdPane.XAxis.Scale.AlignH = AlignH.Center
            ppdPane.XAxis.Scale.LabelGap = 0.1
            ppdPane.XAxis.Scale.IsPreventLabelOverlap = True
            Dim dblMax As Double = Double.MinValue, dblMin As Double = Double.MaxValue
            Dim strLabel(0 To (ProjectStats.Projects.Count + ProjectStats.HW_Names.Count)) As String
            Dim lPPDMin As New List(Of Double)
            Dim lPPDAvg As New List(Of Double)
            Dim lPPDMax As New List(Of Double)
            Dim lTPF As New List(Of Double)
            Try
                lPPDAvg.Add(CDbl(ProjectStats.AvgPPD))
                If CDbl(ProjectStats.AvgPPD) < dblMin Then dblMin = CDbl(ProjectStats.AvgPPD)
                If CDbl(ProjectStats.AvgPPD) > dblMax Then dblMax = CDbl(ProjectStats.AvgPPD)
                Dim tsTmp As New TimeSpan
                TimeSpan.TryParse(ProjectStats.AvgTPF, tsTmp)
                lTPF.Add(tsTmp.TotalDays)
                strLabel(0) = "Project " & ProjectStats.Number & Environment.NewLine & "Averages"
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try

            Dim hwNames As List(Of String) = ProjectStats.HW_Names
            Dim iInd As Int32 = 1

            For Each Name As String In hwNames
                Try
                    strLabel(iInd) = "Project " & ProjectStats.Number & Environment.NewLine & Name & " averages"
                    Try ' Should use timespan.tryparse but this is quicker
                        lTPF.Add(TimeSpan.Parse(ProjectStats.AvgTPF(Name)).TotalDays)
                    Catch ex As Exception : End Try
                    lPPDAvg.Add(CDbl(ProjectStats.AvgPPD(Name)))
                    iInd += 1
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            Next
            iInd = ProjectStats.HW_Names.Count + 1
            Dim bHours As Boolean = False, bThousands As Boolean = False
            For Each Project As clsWU In ProjectStats.Projects
                If (Not Project.tpfDB = "" And Not Project.tpfDB = "00:00:00") And (Not Project.PPD = "" And Not Project.PPD = "0") Then
                    strLabel(iInd) = Project.RCG_Short & Environment.NewLine & Project.HW.ToString
                    iInd += 1
                    lPPDAvg.Add(CDbl(Project.PPD))
                    If CDbl(Project.PPD) > 1000 Then bThousands = True
                    Dim tsTPF As TimeSpan = TimeSpan.Parse(Project.tpfDB)
                    If tsTPF.TotalHours > 1 Then bHours = True
                    lTPF.Add(tsTPF.TotalDays)
                Else
                    ReDim Preserve strLabel(0 To strLabel.GetUpperBound(0) - 1)
                End If
            Next

            Dim biPPD As BarItem = ppdPane.AddBar("PPD", Nothing, lPPDAvg.ToArray, Color.Green)
            Dim biTPF As BarItem = ppdPane.AddBar("TPF", Nothing, lTPF.ToArray, Color.Yellow)
            biTPF.IsY2Axis = True

            ppdPane.XAxis.Scale.TextLabels = strLabel

            ppdPane.Y2Axis.Scale.MagAuto = False

            If bHours Then
                ppdPane.Y2Axis.Scale.Format = "hh:mm:ss"
            Else
                ppdPane.Y2Axis.Scale.Format = "mm:ss"
            End If


            ppdPane.YAxis.Scale.MagAuto = False
            Select Case dblMax - dblMin
                ' set scales based on range not on raw values! 



            End Select

            If bThousands Then
                ppdPane.YAxis.Scale.MajorStep = 1000
                ppdPane.YAxis.Scale.MinorStep = 250
            Else
                ppdPane.YAxis.Scale.MajorStep = 100
                ppdPane.YAxis.Scale.MinorStep = 10
            End If


            'ppdPane.YAxis.Scale.FormatAuto = True

            zg.GraphPane = ppdPane
            If bHours Then ppdPane.Y2Axis.ResetAutoScale(zg.GraphPane, zg.CreateGraphics)
            zg.Refresh()
            'ppdPane.Y2Axis.ResetAutoScale(ppdPane, zg.CreateGraphics)
            zg.IsShowPointValues = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Performance Graphs"

#End Region
End Class
