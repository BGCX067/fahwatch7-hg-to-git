'   FAHWatch7   
'
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports ZedGraph
Friend Class frmZGRAPH
    Implements IDisposable
    Friend bAllowclose As Boolean = False
    Friend myWU As New clsWU
    Friend mProjectStats As New clsStatistics.clsProjectStatistics.clsProject
    Private Enum eMode
        WU
        History
        Performance
        Projects
        Statistics
    End Enum
    Private mMode As eMode = eMode.WU
    Friend Sub ShowWUFrames(WU As clsWU)
        Try
            myWU = WU
            mMode = eMode.WU
            Dim ppProject As New ZedGraph.PointPairList
            Dim bHour As Boolean = False, mMinValue As Double = Double.MaxValue, mMaxValue As Double = Double.MinValue
            For xInt As Int32 = 0 To myWU.Frames.Count - 2
                Dim tsFrame As TimeSpan = myWU.Frames(xInt + 1).FrameDT - myWU.Frames(xInt).FrameDT
                If tsFrame.TotalDays > mMaxValue Then mMaxValue = tsFrame.TotalDays
                If tsFrame.TotalDays < mMinValue Then mMinValue = tsFrame.TotalDays
                If tsFrame.Duration.TotalHours > 1 Then
                    bHour = True
                End If
                ppProject.Add(New XDate(myWU.Frames(xInt).FrameDT), tsFrame.TotalDays)
            Next
            zgProject.MasterPane.PaneList.Clear()
            Dim mPane As New GraphPane
            Me.Text = myWU.HW & Chr(32) & myWU.ClientName & myWU.Slot & Chr(32) & myWU.PRCG
            mPane.XAxis.Title.Text = "Checkpoints occurance"
            mPane.XAxis.Title.FontSpec.Size = 10
            mPane.YAxis.Title.Text = "Time per frame ( discarding idle time )"
            mPane.YAxis.Title.FontSpec.Size = 10
            mPane.XAxis.MinorTic.IsOpposite = False
            mPane.XAxis.MajorTic.IsOpposite = False
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
            ' Console.WriteLine(myWU.PRCG & " " & tsSpan.ToString)
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
                mPane.YAxis.Scale.MinorStep = 60
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


            mPane.AddCurve("", ppProject, Color.Blue)
            mPane.Rect = Me.ClientRectangle
            zgProject.GraphPane = mPane
            zgProject.AxisChange()
            If bHour Then mPane.YAxis.ResetAutoScale(zgProject.GraphPane, zgProject.CreateGraphics)
            zgProject.Refresh()
            zgProject.IsShowPointValues = True
            'Me.Invalidate()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub ShowProjectStats(ProjectStats As clsStatistics.clsProjectStatistics.clsProject, Optional HistoryForm As frmHistory = Nothing)
        Try
            zgProject.MasterPane.PaneList.Clear()
            mMode = eMode.Projects
            mProjectStats = ProjectStats
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
            ppdPane.XAxis.Scale.Align = AlignP.Outside
            ppdPane.XAxis.Scale.AlignH = AlignH.Center
            ppdPane.XAxis.Scale.LabelGap = 0.1
            ppdPane.XAxis.Scale.IsPreventLabelOverlap = True

            Dim strLabel(0 To ProjectStats.Projects.Count + ProjectStats.HW_Names.Count) As String
            Dim lPPD As New List(Of Double)
            Dim lTPF As New List(Of Double)
            Try
                lPPD.Add(CDbl(ProjectStats.AvgPPD))
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
                    lTPF.Add(TimeSpan.Parse(ProjectStats.AvgTPF(Name)).TotalDays)
                    lPPD.Add(CDbl(ProjectStats.AvgPPD(Name)))
                    iInd += 1
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            Next
            iInd = ProjectStats.HW_Names.Count + 1
            Dim bHours As Boolean = False, bThousands As Boolean = True
            For Each Project As clsWU In ProjectStats.Projects
                If (Not Project.tpfDB = "" And Not Project.tpfDB = "00:00:00") And (Not Project.PPD = "" And Not Project.PPD = "0") Then
                    strLabel(iInd) = Project.RCG_Short & Environment.NewLine & Project.HW.ToString
                    iInd += 1
                    lPPD.Add(CDbl(Project.PPD))
                    If CDbl(Project.PPD) < 1000 Then bThousands = False
                    Dim tsTPF As TimeSpan = TimeSpan.Parse(Project.tpfDB)
                    If tsTPF.TotalHours > 1 Then bHours = True
                    lTPF.Add(tsTPF.TotalDays)
                Else
                    ReDim Preserve strLabel(0 To strLabel.GetUpperBound(0) - 1)
                End If
            Next

            Dim biPPD As BarItem = ppdPane.AddBar("PPD", Nothing, lPPD.ToArray, Color.Green)
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
            If bThousands Then
                ppdPane.YAxis.Scale.MajorStep = 1000
                ppdPane.YAxis.Scale.MinorStep = 250
            Else
                ppdPane.YAxis.Scale.MajorStep = 100
                ppdPane.YAxis.Scale.MinorStep = 10
            End If


            'ppdPane.YAxis.Scale.FormatAuto = True
            ppdPane.Rect = Me.ClientRectangle
            zgProject.MasterPane.Add(ppdPane)
            zgProject.MasterPane.Rect = Me.ClientRectangle
            zgProject.GraphPane.AxisChange()
            If bHours Then ppdPane.Y2Axis.ResetAutoScale(zgProject.GraphPane, zgProject.CreateGraphics)
            zgProject.Refresh()
            'ppdPane.Y2Axis.ResetAutoScale(ppdPane, zgProject.CreateGraphics)
            zgProject.IsShowPointValues = True
        Catch ex As Exception

        End Try
    End Sub
    Private Sub zgProject_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles zgProject.MouseDoubleClick
        Try
            Me.TopMost = Not Me.TopMost
            Me.Text = myWU.PRCG & " " & myWU.dtDownloaded.ToString & vbTab & " topmost=" & Me.TopMost.ToString
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Public Sub CloseForm()
        Try
            bAllowclose = True
            Me.Close()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmZGRAPH_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Not bAllowclose Then
                e.Cancel = True
                delegateFactory.HideFade(Me, 100)
                'Me.Hide()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmZGRAPH_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Try
            If mMode = eMode.WU Then
                'Allow going through the wu list with left and right
                If e.KeyData = Keys.Right Or e.KeyData = Keys.Left Then
                    If e.KeyData = Keys.Left And History.lvWU.SelectedIndices(0) > 1 Then
                        History.lvWU.Items(History.lvWU.SelectedIndices(0) - 1).Selected = True
                    ElseIf e.KeyData = Keys.Right And History.lvWU.SelectedIndices(0) < History.lvWU.Items.Count - 1 Then
                        History.lvWU.Items(History.lvWU.SelectedIndices(0) + 1).Selected = True
                    End If
                End If
            ElseIf mMode = eMode.Projects Then
                If e.KeyCode = Keys.Right Or e.KeyCode = Keys.Left Then
                    If e.KeyCode = Keys.Left And History.tsProjects_cmbProjects.SelectedIndex < History.tsProjects_cmbProjects.Items.Count - 1 Then
                        History.tsProjects_cmbProjects.SelectedIndex += 1
                    ElseIf e.KeyCode = Keys.Right And History.tsProjects_cmbProjects.SelectedIndex > 1 Then
                        History.tsProjects_cmbProjects.SelectedIndex -= 1
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmZGRAPH_PreviewKeyDown(sender As Object, e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles Me.PreviewKeyDown
        Try
            If e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Then e.IsInputKey = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
   
    Private Function zgProject_PointValueEvent(sender As ZedGraph.ZedGraphControl, pane As ZedGraph.GraphPane, curve As ZedGraph.CurveItem, iPt As Integer) As String Handles zgProject.PointValueEvent
        Try
            If mMode = eMode.Projects Then
                With curve.Points(iPt)
                    Dim s As String = "Project " & mProjectStats.Number & Chr(32) & pane.XAxis.Scale.TextLabels(CInt(.X) - 1) '.Replace("R", "run:").Replace("C", " clone:").Replace("G", " gen:")
                    If curve.Label.Text.ToLowerInvariant = "tpf" Then
                        Dim xD As New XDate(.Y)
                        Return s & Chr(32) & "tpf: " & FormatTimeSpan(xD.DateTime.TimeOfDay)
                    Else
                        Return s & Chr(32) & "ppd: " & FormatPPD(.Y.ToString)
                    End If
                End With
            ElseIf mMode = eMode.WU Then
                With curve.Points(iPt)
                    Dim dtC As XDate = New XDate(.X)
                    Dim tsF As XDate = New XDate(.Y)
                    If tsF.DateTime.TimeOfDay.TotalMilliseconds > 0 Then
                        Return dtC.ToString & Chr(32) & "completed " & iPt + 2 & "% tpf: " & FormatTimeSpan(tsF.DateTime.TimeOfDay)
                    Else
                        Return dtC.ToString & Chr(32) & "completed " & iPt + 2 & "%"
                    End If
                End With
            Else
                Return ""
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return String.Empty
        End Try
    End Function
End Class