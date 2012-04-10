'/*
' * FAHWatch7  Copyright Marvin Westmaas ( mtm )
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
'/*


' InitHistoryColumns()
' UpdateColumnSettings()
Imports ZedGraph
Imports System.Globalization
Imports FAHWatch7.ucZedGraphContainer
Imports FAHWatch7.modMySettings

Public Class frmHistory
#Region "Declarations"
    Private mCHInfos As New SortedList(Of Int32, sColumnInfo)
    Private lvSorter As ListViewColumnSorter
    Private IsShown As Boolean = False
    Private bManual As Boolean = True
    Private bResetColumns As Boolean = False
    Private mLastFill As String = String.Empty
    Private m_SelectedWU As clsWU = Nothing
    Private mSilentClose As Boolean = False
    Private bStatisticsChanging As Boolean = False
    Private bResized As Boolean = False
    Private m_PerformanceStatistics As New clsStatistics.clsPerformanceStatistics.sClient ' Used for statistics based on filters ( eg status bar stats )
    Private mHistoryStatistics As clsStatistics.clsPerformanceStatistics.sClient = Nothing, iLastCount As Int32 = 0
#End Region
#Region "Enable/Disable controls"
    Private ReadOnly Property AreControlsDisabled As Boolean
        Get
            Return scMain.Enabled
        End Get
    End Property
    Private Sub EnableControls()
        Try
            scMain.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub DisableControls()
        Try
            scMain.Enabled = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Listview events"
    Private Sub lvWU_ColumnClick(sender As Object, e As System.Windows.Forms.ColumnClickEventArgs) Handles lvWU.ColumnClick
        Try
            If IsNothing(lvWU.ListViewItemSorter) Then lvWU.ListViewItemSorter = lvSorter
            If e.Column = lvSorter.SortColumn Then
                ' Reverse the current sort direction for this column.
                If lvSorter.Order = SortOrder.Ascending Then
                    lvSorter.Order = SortOrder.Descending
                Else
                    lvSorter.Order = SortOrder.Ascending
                End If
            Else
                ' Set the column number that is to be sorted; default to ascending.
                lvSorter.SortColumn = e.Column
                lvSorter.Order = SortOrder.Ascending
            End If
            ResetColumnHeader()
            lvWU.Sort()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tsCmdClearSort_Click(sender As System.Object, e As System.EventArgs) Handles tsCmdClearSort.Click
        lvWU.ListViewItemSorter = Nothing
        Fill(mLastFill)
    End Sub
    Private Sub ResetColumnHeader()
        Try
            tsSepClear.Visible = False
            tsCmdClearSort.Visible = False
            For Each column As ColumnHeader In lvWU.Columns
                column.Text = column.Text.Replace("*", "").Replace("<", "").Replace(">", "")
                If Not IsNothing(lvWU.ListViewItemSorter) Then
                    If lvSorter.SortColumn = column.Index Then
                        If lvSorter.Order = SortOrder.Ascending Then
                            column.Text = "*" & column.Text & ">"
                            tsCmdClearSort.Visible = True
                            tsSepClear.Visible = True
                        ElseIf lvSorter.Order = SortOrder.Descending Then
                            column.Text = "*" & column.Text & "<"
                            tsCmdClearSort.Visible = True
                            tsSepClear.Visible = True
                        Else
                            'Ignore sortorder.none 
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvWU_ItemSelectionChanged(sender As Object, e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvWU.ItemSelectionChanged
        Try
            'Throw New Exception("This is a test", New Exception("Inner exception"))
            If Not e.IsSelected Then
                WriteDebug("Clearing data")
                m_SelectedWU = Nothing
                If Not scMain.Panel2Collapsed Then
                    rtWU.Clear()
                    zgProject.GraphPane = New GraphPane
                    zgProject.GraphPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.AxisChange()
                    zgProject.Refresh()
                    Exit Sub
                End If
            Else
                If modMySettings.history_ViewDetails Then
                    Dim sTmp As String = CStr(e.Item.Tag)
                    Dim strArr() As String = sTmp.Split({"##"}, StringSplitOptions.RemoveEmptyEntries)
                    Dim tClient As String = strArr(0)
                    Dim tSlot As String = strArr(1)
                    Dim tUnit As String = strArr(2)
                    Dim tDownload As DateTime = DateTime.Parse(strArr(3))
                    WriteDebug("Loading data for selected wu")
                    m_SelectedWU = sqdata.WorkUnit(tClient, tSlot, tUnit, tDownload)
                    Select Case modMySettings.history_detail
                        Case modMySettings.eDetail.Hardware
                            Detail_Hardware()
                        Case modMySettings.eDetail.Performance
                            Detail_Performance()
                        Case modMySettings.eDetail.Projects
                            Detail_Projects()
                        Case modMySettings.eDetail.WU
                            Detail_WU()
                    End Select
                End If
                lvWU.EnsureVisible(lvWU.SelectedIndices(0))
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvWU_DoubleClick(sender As Object, e As System.EventArgs) Handles lvWU.MouseDoubleClick
        Try
            'Allow a 'detailed form' displaying both frame info and wu details?
            lvWU.SelectedItems.Clear()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Procedures and functions"
#Region "Listview functions"
    Private Function lvItemForHistory(ByVal WU As clsWU) As ListViewItem
        Try
            Dim nI As New ListViewItem
            nI.BackColor = Color.LightBlue
            Dim bItem As Boolean = True
            For Each ch As ColumnHeader In lvWU.Columns
                Select Case ch.Text
                    Case Is = "Client"
                        If bItem Then
                            nI.Text = WU.ClientName
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.ClientName)
                        End If
                    Case Is = "Slot"
                        If bItem Then
                            nI.Text = WU.Slot
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.Slot)
                        End If
                    Case Is = "Hardware"
                        If bItem Then
                            nI.Text = WU.HW
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.HW)
                        End If
                    Case Is = "PRCG"
                        If bItem Then
                            nI.Text = WU.PRCG
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.PRCG)
                        End If
                    Case Is = "Downloaded"
                        If bItem Then
                            nI.Text = WU.dtDownloaded.ToString(CultureInfo.CurrentCulture)
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.dtDownloaded.ToString(CultureInfo.CurrentCulture))
                        End If
                    Case Is = "Submitted"
                        If bItem Then
                            nI.Text = WU.dtSubmitted.ToString(CultureInfo.CurrentCulture)
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.dtSubmitted.ToString(CultureInfo.CurrentCulture))
                        End If
                    Case Is = "WorkServer"
                        If bItem Then
                            nI.Text = WU.WS
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.WS)
                        End If
                    Case Is = "FahCore"
                        If bItem Then
                            nI.Text = WU.Core
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.Core)
                        End If
                    Case Is = "CoreStatus"
                        If bItem Then
                            nI.Text = WU.CoreStatus
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.CoreStatus)
                        End If
                    Case Is = "FahCoreVersion"
                        If bItem Then
                            nI.Text = WU.CoreVersion
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.CoreVersion)
                        End If
                    Case Is = "CoreStatus"
                        If bItem Then
                            nI.Text = WU.CoreStatus
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.CoreStatus)
                        End If
                    Case Is = "ServerResponse"
                        If bItem Then
                            nI.Text = WU.ServerResponce
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.ServerResponce)
                        End If
                    Case Is = "Credit"
                        If bItem Then
                            nI.Text = WU.Credit
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.Credit)
                        End If
                    Case Is = "PPD"
                        If bItem Then
                            nI.Text = WU.PPD
                            bItem = False
                        Else
                            nI.SubItems.Add(FormatPPD(WU.PPD))
                        End If
                    Case Is = "TPF"
                        If bItem Then
                            nI.Text = WU.tpfDB
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.tpfDB)
                        End If
                    Case Is = "ResultSize"
                        If bItem Then
                            nI.Text = WU.UploadSize
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.UploadSize)
                        End If
                    Case Is = "UploadSpeed"
                        If bItem Then
                            nI.Text = WU.UploadSpeed
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.UploadSpeed)
                        End If
                    Case Is = "DownloadSize"
                        If bItem Then
                            nI.Text = WU.DownloadSize
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.DownloadSize)
                        End If
                    Case Is = "DownloadSpeed"
                        If bItem Then
                            nI.Text = WU.DownloadSpeed
                            bItem = False
                        Else
                            nI.SubItems.Add(WU.DownloadSpeed)
                        End If
                End Select
            Next
            If WU.CoreStatus <> "" AndAlso (Not WU.CoreStatus.Contains("(100") Or Not WU.CoreStatus.Contains("FINISHED_UNIT")) Then
                nI.BackColor = Color.Red
            End If
            If Not WU.ServerResponce.Contains("WORK_ACK") And Not nI.BackColor = Color.Red Then
                nI.BackColor = Color.LightGray
            End If
            If WU.bHasRestarted Then
                If Not nI.BackColor = Color.Red Then nI.BackColor = Color.Yellow
            End If
            nI.Tag = WU.unit
            Return nI
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New ListViewItem
        End Try
    End Function
#Region "List view quick filters"
    Private Class ListViewQuickfilter
        'Should really use an array of listviewitems for memory efficiency
        Private Shared lvItems As New List(Of ListViewItem)
        Friend Shared WriteOnly Property SetCollection As ListView.ListViewItemCollection
            Set(value As ListView.ListViewItemCollection)
                Try
                    ClearCombos()
                    lvItems.Clear()
                    For Each lItem As ListViewItem In value
                        lvItems.Add(lItem)
                        If Not ItemClient(lItem) = "" AndAlso Not lClientNames.Contains(ItemClient(lItem)) Then lClientNames.Add(ItemClient(lItem))
                        If Not ItemHardware(lItem) = "" AndAlso Not lHardware.Contains(ItemHardware(lItem)) Then lHardware.Add(ItemHardware(lItem))
                        If Not ItemProject(lItem) = "" AndAlso Not lProjects.Contains(ItemProject(lItem)) Then lProjects.Add(ItemProject(lItem))
                    Next
                    'lvItems.AddRange(value)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            End Set
        End Property
        Friend Shared ReadOnly Property ClearedCollection As List(Of ListViewItem)
            Get
                Return lvItems
            End Get
        End Property
        Friend Shared Sub Clear()
            lvItems.Clear()
            ClearCombos()
        End Sub
        Private Shared lClientNames As New List(Of String), lHardware As New List(Of String), lProjects As New List(Of String)
        Friend Shared Sub ClearCombos()
            lClientNames.Clear() : lHardware.Clear() : lProjects.Clear()
            lClientNames.Add("-all-") : lHardware.Add("-all-") : lProjects.Add("-all-")
        End Sub
        Friend Shared ReadOnly Property ClientNames As List(Of String)
            Get
                Return lClientNames
            End Get
        End Property
        Friend Shared ReadOnly Property ProjectNames As List(Of String)
            Get
                Try
                    Dim lInt As New List(Of Integer)
                    For Each pName As String In lProjects
                        Dim nInt As Integer
                        If Integer.TryParse(pName, nInt) Then
                            lInt.Add(nInt)
                        End If
                    Next
                    Dim rVal As New List(Of String)
                    lInt.Sort()
                    rVal.Add("-all-")
                    For Each pInt As Integer In lInt
                        rVal.Add(CStr(pInt))
                    Next
                    Return rVal
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return New List(Of String)
                End Try
            End Get
        End Property
        Friend Shared ReadOnly Property HardwareNames As List(Of String)
            Get
                Return lHardware
            End Get
        End Property
        Private Shared Function ItemMatch(Item As ListViewItem, Client As String, Hardware As String, Project As String) As Boolean
            Try
                If Client = "-all-" And Hardware = "-all-" And Project = "-all-" Then Return True
                If Client <> "-all-" And Client <> ItemClient(Item) Then Return False
                If Hardware <> "-all-" And Hardware <> ItemHardware(Item) Then Return False
                If Project <> "-all-" Then
                    If Project <> ItemProject(Item) Then
                        Return False
                    End If
                End If
                Return True
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Private Shared ReadOnly Property ItemClient(lItem As ListViewItem) As String
            Get
                If History.lvWU.Columns.IndexOfKey("Client") > -1 Then
                    If History.lvWU.Columns.IndexOfKey("Client") = 0 Then
                        Return lItem.Text
                    Else
                        Return lItem.SubItems(History.lvWU.Columns.IndexOfKey("Client")).Text
                    End If
                Else
                    For Each ch As ColumnHeader In History.lvWU.Columns
                        If ch.Text = "Client" Then
                            If ch.Index = 0 Then
                                Return lItem.Text
                            Else
                                Return lItem.SubItems(ch.Index).Text
                            End If
                        End If
                    Next
                    Return ""
                End If
            End Get
        End Property
        Private Shared ReadOnly Property ItemHardware(lItem As ListViewItem) As String
            Get
                If History.lvWU.Columns.IndexOfKey("Hardware") > -1 Then
                    If History.lvWU.Columns.IndexOfKey("Hardware") = 0 Then
                        Return lItem.Text
                    Else
                        Return lItem.SubItems(History.lvWU.Columns.IndexOfKey("Hardware")).Text
                    End If
                Else
                    For Each ch As ColumnHeader In History.lvWU.Columns
                        If ch.Text = "Hardware" Then
                            If ch.Index = 0 Then
                                Return lItem.Text
                            Else
                                Return lItem.SubItems(ch.Index).Text
                            End If
                        End If
                    Next
                    Return ""
                End If
            End Get
        End Property
        Private Shared ReadOnly Property ItemProject(lItem As ListViewItem) As String
            Get
                If History.lvWU.Columns.IndexOfKey("PRCG") > -1 Then
                    If History.lvWU.Columns.IndexOfKey("PRCG") = 0 Then
                        Return lItem.Text
                    Else
                        Return lItem.SubItems(History.lvWU.Columns.IndexOfKey("PRCG")).Text
                    End If
                Else
                    For Each ch As ColumnHeader In History.lvWU.Columns
                        If ch.Text = "PRCG" Then
                            If ch.Index = 0 Then
                                Return lItem.Text
                            Else
                                Return lItem.SubItems(ch.Index).Text.Replace("Project:", "").Substring(0, lItem.SubItems(ch.Index).Text.Replace("Project:", "").IndexOf(Chr(32)))
                            End If
                        End If
                    Next
                    Return ""
                End If
            End Get
        End Property
        Friend Shared ReadOnly Property ListViewItems(Client As String, Hardware As String, Project As String) As ListView.ListViewItemCollection
            Get
                Try
                    Dim rVal As ListView.ListViewItemCollection = New ListView.ListViewItemCollection(New ListView)
                    ClearCombos()
                    For Each lItem As ListViewItem In lvItems
                        If ItemMatch(lItem, Client, Hardware, Project) Then
                            If Not ItemClient(lItem) = "" AndAlso Not lClientNames.Contains(ItemClient(lItem)) Then lClientNames.Add(ItemClient(lItem))
                            If Not ItemHardware(lItem) = "" AndAlso Not lHardware.Contains(ItemHardware(lItem)) Then lHardware.Add(ItemHardware(lItem))
                            If Not ItemProject(lItem) = "" AndAlso Not lProjects.Contains(ItemProject(lItem)) Then lProjects.Add(ItemProject(lItem))
                            rVal.Add(lItem)
                        End If
                    Next
                    Return rVal
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return Nothing
                End Try
            End Get
        End Property
    End Class
    Private Sub lvQuickFilterUpdate()
        Try
            Dim bIsManual As Boolean = bManual
            Try
                If Not bIsManual Then bManual = True
                Try
                    Dim selItem As String = String.Empty
                    If tsHQFClient.SelectedIndex > -1 Then selItem = tsHQFClient.SelectedItem.ToString()
                    tsHQFClient.Items.Clear()
                    tsHQFClient.Items.AddRange(ListViewQuickfilter.ClientNames.ToArray)
                    If Not selItem = String.Empty Then
                        tsHQFClient.SelectedIndex = tsHQFClient.Items.IndexOf(selItem)
                    Else
                        tsHQFClient.SelectedIndex = 0
                    End If
                    tsHQFClient.Enabled = CBool(tsHQFClient.Items.Count > 1)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Try
                    Dim selItem As String = String.Empty
                    If tsHQFHardware.SelectedIndex > -1 Then selItem = tsHQFHardware.SelectedItem.ToString()
                    tsHQFHardware.Items.Clear()
                    tsHQFHardware.Items.AddRange(ListViewQuickfilter.HardwareNames.ToArray)
                    If Not selItem = String.Empty Then
                        tsHQFHardware.SelectedIndex = tsHQFHardware.Items.IndexOf(selItem)
                    Else
                        tsHQFHardware.SelectedIndex = 0
                    End If
                    tsHQFHardware.Enabled = CBool(tsHQFHardware.Items.Count > 1)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Try
                    Dim selItem As String = String.Empty
                    If tsHQFProject.SelectedIndex > -1 Then selItem = tsHQFProject.SelectedItem.ToString()
                    tsHQFProject.Items.Clear()
                    tsHQFProject.Items.AddRange(ListViewQuickfilter.ProjectNames.ToArray)
                    If Not selItem = String.Empty Then
                        tsHQFProject.SelectedIndex = tsHQFProject.Items.IndexOf(selItem)
                    Else
                        tsHQFProject.SelectedIndex = 0
                    End If
                    tsHQFProject.Enabled = CBool(tsHQFProject.Items.Count > 1)
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
            Finally
                bManual = bIsManual
            End Try
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvQuickStatistics()
        Try
            Dim iTotal As Double = lvWU.Items.Count
            Dim iEUE As Double = 0, dblPPD As Double = 0
            For Each lItem As ListViewItem In lvWU.Items
                If TypeOf lItem.Tag Is clsWU Then
                    With CType(lItem.Tag, clsWU)
                        If .CoreStatus.IndexOf("FINISHED_UNIT", StringComparison.InvariantCultureIgnoreCase) <> -1 Then iEUE += 1
                        If Not .PPD = "" Then dblPPD += CDbl(.PPD)
                    End With
                ElseIf TypeOf lItem.Tag Is String Then
                    Try
                        Dim strArr() As String = CStr(lItem.Tag).Split({"##"}, StringSplitOptions.RemoveEmptyEntries)
                        Dim tClient As String = strArr(0)
                        Dim tSlot As String = strArr(1)
                        Dim tUnit As String = strArr(2)
                        Dim tDownload As DateTime = DateTime.Parse(strArr(3))
                        Dim dtNow As DateTime = DateTime.Now
                        Dim itemWU As clsWU = sqdata.WorkUnit(tClient, tSlot, tUnit, tDownload, True)
                        WriteDebug("Loading work unit data for quickfilter statistics took: " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
                        If itemWU.CoreStatus.IndexOf("FINISHED_UNIT", StringComparison.InvariantCultureIgnoreCase) <> -1 Then iEUE += 1
                        If Not itemWU.PPD = "" Then dblPPD += CDbl(itemWU.PPD)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Exit Try
                    End Try
                End If
            Next
            tsStatistics.Text = "Total WUs: " & lvWU.Items.Count & " Failed: " & CStr(iEUE)
            If lvWU.Items.Count = 0 Or iEUE = 0 Then
                tsStatistics.Text &= " Successrate: 100% "
            Else
                tsStatistics.Text &= " Successrate: " & CStr(Math.Round(100 - iEUE / (iTotal / 100), 2).ToString & "% ")
            End If
            dblPPD = dblPPD / iTotal
            tsStatistics.Text &= "Avg ppd: " & Math.Round(dblPPD, 2).ToString
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub tsHQFProject_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles tsHQFProject.SelectedIndexChanged, tsHQFHardware.SelectedIndexChanged, tsHQFClient.SelectedIndexChanged
        If bManual Then Exit Sub
#If CONFIG = "Debug" Then
        Console.WriteLine("Starting quickStatistics: " & DateTime.Now.ToLongTimeString)
        Dim dtNow As DateTime = DateTime.Now
#End If
        Try
            Dim bIsBmanual As Boolean = bManual
            Dim bIsWaitCursor As Boolean = Me.UseWaitCursor
            Dim bShouldEnable As Boolean = Not AreControlsDisabled
            Try
                Me.UseWaitCursor = True
                If Not AreControlsDisabled Then DisableControls()
                If Not bIsBmanual Then bManual = True

                lvWU.BeginUpdate()
                lvWU.Items.Clear()

                lvWU.ListViewItemSorter = Nothing
                lvSorter.Order = SortOrder.None

                ResetColumnHeader()


                For Each lItem As ListViewItem In ListViewQuickfilter.ListViewItems(tsHQFClient.SelectedItem.ToString, tsHQFHardware.SelectedItem.ToString, tsHQFProject.SelectedItem.ToString)
                    lItem.Remove()
                    lvWU.Items.Add(lItem)
                Next
                lvQuickFilterUpdate()

            Catch ex As Exception
                WriteError(ex.Message, Err)
            Finally
                lvWU.EndUpdate()
                lvQuickStatistics()
                If Not bIsBmanual And bManual Then bManual = False
                If Not bIsWaitCursor Then Me.UseWaitCursor = False
                If bShouldEnable Then EnableControls()
                If lvWU.Items.Count > 0 Then lvWU.Items(0).Selected = True
            End Try
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
#If CONFIG = "Debug" Then
        Console.WriteLine("--Took: " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
        dtNow = Nothing
#End If
    End Sub
    Private Sub tsHQFReset_Click(sender As System.Object, e As System.EventArgs)
        If bManual Then Exit Sub
        Try
            Dim bShouldEnable As Boolean = Not AreControlsDisabled
            Dim bUseWaitCursor As Boolean = Me.UseWaitCursor
            Try
                lvWU.BeginUpdate()
                lvWU.Items.Clear()
                lvWU.SelectedItems.Clear()
                If Not bUseWaitCursor Then Me.UseWaitCursor = True
                If bShouldEnable Then DisableControls()
                For Each nItem As ListViewItem In ListViewQuickfilter.ClearedCollection
                    lvWU.Items.Add(nItem)
                Next
                lvWU.EndUpdate()
            Catch ex As Exception
                WriteError(ex.Message, Err)
            Finally
                If bShouldEnable Then EnableControls()
                If Not bUseWaitCursor Then Me.UseWaitCursor = False
                If lvWU.Items.Count > 0 Then lvWU.Items(0).Selected = True
            End Try
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
    Public Function Fill(Optional sqlQuery As String = "") As Boolean
        Try
            Me.UseWaitCursor = True
            If lvWU.SelectedItems.Count > 0 Then lvWU.SelectedItems(0).Selected = False
            bManual = True
            DisableControls()

            mLastFill = sqlQuery

            lvWU.BeginUpdate()

            lvWU.Items.Clear()
            lvWU.ListViewItemSorter = Nothing
            lvSorter.Order = SortOrder.None

            If bResetColumns Then
                mCHInfos = modMySettings.ColumnSettings.ColumnSettings(lvWU)
                bResetColumns = False
            End If
            ResetColumnHeader()
            ListViewQuickfilter.Clear()
            m_SelectedWU = Nothing
            If sqlQuery = "" Then
                Me.Text = My.Application.Info.AssemblyName & Chr(32) & My.Application.Info.Version.ToString
                Dim lWUS As List(Of clsWU) = sqdata.WorkUnits("")
                For Each wu As clsWU In lWUS
                    Dim nI As ListViewItem = lvItemForHistory(wu)
                    nI.Tag = wu.ClientName & "##" & wu.Slot & "##" & wu.unit & "##" & wu.utcDownloaded.ToString("s")
                    lvWU.Items.Add(nI)
                Next
                If modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current Then
                    tsStatistics.Text = "Total WUs: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Wu_Completed & " Total credit: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.TotalCredit & " WU's failed: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Wu_EUE & " Succesrate: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRate & " Total computation time: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.ComputationTime & " Avg PPD: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.AvgPPD
                Else
                    tsStatistics.Text = "Total WUs: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_Completed & " Total credit: " & clsStatistics.clsHistoricalStatistics.Statistics.TotalCredit & " WU's Failed: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_EUE & " Succesrate: " & clsStatistics.clsHistoricalStatistics.Statistics.SuccesRate & " Total computation time: " & clsStatistics.clsHistoricalStatistics.Statistics.ComputationTime & " Avg PPD: " & clsStatistics.clsHistoricalStatistics.Statistics.AvgPPD
                End If
            Else
                Me.Text = My.Application.Info.AssemblyName & Chr(32) & My.Application.Info.Version.ToString & New String(Chr(32), 10) & sqlQuery
                Dim lWUS As List(Of clsWU) = sqdata.WorkUnits(sqlQuery)
                For Each wu In sqdata.WorkUnits(sqlQuery)
                    Dim nI As ListViewItem = lvItemForHistory(wu)
                    nI.Tag = wu.ClientName & "##" & wu.Slot & "##" & wu.unit & "##" & wu.utcDownloaded.ToString("s")
                    lvWU.Items.Add(nI)
                Next
                m_PerformanceStatistics = clsStatistics.clsPerformanceStatistics.Statistics(lWUS)
                tsStatistics.Text = "Total WUs: " & m_PerformanceStatistics.Wu_Completed & " Failed: " & m_PerformanceStatistics.Wu_EUE & " Succesrate: " & m_PerformanceStatistics.SuccesRate & " Total computation time: " & m_PerformanceStatistics.ComputationTime & " Avg PPD: " & m_PerformanceStatistics.AvgPPD
            End If

            lvWU.EndUpdate()

            ListViewQuickfilter.SetCollection = lvWU.Items
            lvQuickFilterUpdate()
            If (modMySettings.FirstRun AndAlso Not bResized) OrElse Not modMySettings.NoAutoSizeColumns Then
                bResized = True
                lvWU.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                For xInt As Int32 = 0 To lvWU.Columns.Count - 1
                    lvWU.Columns(xInt).Tag = lvWU.Columns(xInt).Width
                Next
                lvWU.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                For xint As Int32 = 0 To lvWU.Columns.Count - 1
                    If lvWU.Columns(xint).Width < CInt(lvWU.Columns(xint).Tag) Then lvWU.Columns(xint).Width = CInt(lvWU.Columns(xint).Tag)
                Next
            End If
            tsHQFClient.SelectedIndex = 0
            tsHQFHardware.SelectedIndex = 0
            tsHQFProject.SelectedIndex = 0
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            lvWU.EndUpdate()
            Me.UseWaitCursor = False
            EnableControls()
            bManual = False
            If lvWU.Items.Count > 0 Then lvWU.Items(0).Selected = True
        End Try
        Return CBool(lvWU.Items.Count > 0)
    End Function
#End Region
    Private Sub RefreshFilterMenu()
        Try
            'remove old event handlers
            RemoveHandler tsiFilters.DropDownItemClicked, AddressOf HandleFilterItemClicked
            tsiFilters.DropDownItems.Clear()
            'Add new one's
            For Each fName In sqlFilters.FilterNames
                Dim nItem As New ToolStripMenuItem(fName)
                tsiFilters.DropDownItems.Add(nItem)
            Next
            AddHandler tsiFilters.DropDownItemClicked, AddressOf HandleFilterItemClicked
            RemoveHandler ClientsToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
            ClientsToolStripMenuItem.DropDownItems.Clear()
            For Each DictionaryEntry In sqdata.ClientNames_Count
                Dim nItem As New ToolStripMenuItem(DictionaryEntry.Key & " (" & CStr(DictionaryEntry.Value) & ")")
                nItem.Tag = "Clients"
                For Each Slot As Client.clsSlot In Clients.Client(FormatSQLString(DictionaryEntry.Key, True, True)).Slots
                    ' Need to look at this later....!!
                    If Slot.Type = "UNIPROCESSOR" Then
                        Dim n2Item As New ToolStripMenuItem("UNIPROCESSOR")
                        For Each fName In sqlFilters.FilterNames
                            Dim nItem3 As New ToolStripMenuItem(fName)
                            n2Item.DropDownItems.Add(nItem3)
                        Next
                        nItem.DropDownItems.Add(n2Item)
                    Else
                        Try
                            If Not IsNothing(Slot.WorkUnit) Then
                                Dim n2Item As New ToolStripMenuItem(Slot.WorkUnit.HW)
                                n2Item.Tag = FormatSQLString(DictionaryEntry.Key, True, True)
                                For Each fName In sqlFilters.FilterNames
                                    Dim nItem3 As New ToolStripMenuItem(fName)
                                    n2Item.DropDownItems.Add(nItem3)
                                Next
                                nItem.DropDownItems.Add(n2Item)
                            Else
                                If Slot.Type = "SMP" Then
                                    Dim n2Item As New ToolStripMenuItem("SMP")
                                    n2Item.Tag = FormatSQLString(DictionaryEntry.Key, True, True)
                                    For Each fName In sqlFilters.FilterNames
                                        Dim nItem3 As New ToolStripMenuItem(fName)
                                        n2Item.DropDownItems.Add(nItem3)
                                    Next
                                    nItem.DropDownItems.Add(n2Item)
                                Else
                                    Dim n2Item As New ToolStripMenuItem(Slot.Hardware)
                                    n2Item.Tag = FormatSQLString(DictionaryEntry.Key, True, True)
                                    For Each fName In sqlFilters.FilterNames
                                        Dim nItem3 As New ToolStripMenuItem(fName)
                                        n2Item.DropDownItems.Add(nItem3)
                                    Next
                                    nItem.DropDownItems.Add(n2Item)
                                End If

                            End If
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                    End If
                Next
                AddHandler nItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
                ClientsToolStripMenuItem.DropDownItems.Add(nItem)
            Next
            AddHandler ClientsToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
            RemoveHandler HardwareToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
            HardwareToolStripMenuItem.DropDownItems.Clear()
            For Each DictionaryEntry In sqdata.HardwareNamesCount
                Dim nItem As New ToolStripMenuItem(DictionaryEntry.Key & " (" & CStr(DictionaryEntry.Value) & ")")
                HardwareToolStripMenuItem.DropDownItems.Add(nItem)
            Next
            AddHandler HardwareToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
            RemoveHandler ProjectsToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
            ProjectsToolStripMenuItem.DropDownItems.Clear()
            'Dim lProjects As Dictionary(Of String, Int32) = sqdata.ProjectNames_Count
            Dim lProjects As Dictionary(Of String, Int32) = sqdata.ProjectRanges_Count
            Dim lItems As New List(Of ToolStripMenuItem)
            For Each DictionaryEntry In lProjects
                Dim nI As New ToolStripMenuItem(DictionaryEntry.Key & " (" & CStr(DictionaryEntry.Value) & ")")
                lItems.Add(nI)
            Next
            ProjectsToolStripMenuItem.DropDownItems.AddRange(lItems.ToArray)
            AddHandler ProjectsToolStripMenuItem.DropDownItemClicked, AddressOf HandleFilterItemClicked
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Toolstrip items"
    Private Sub GraphOptionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles GraphOptionsToolStripMenuItem.Click
        modMySettings.clsGraphSettings.ShowOptions(Me)
    End Sub
    Private Sub SignatureToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SignatureToolStripMenuItem.Click
        EOCInfo.showSigs()
    End Sub
    Private Sub WhatsTheDifferenceToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles WhatsTheDifferenceToolStripMenuItem.Click
        MsgBox("Overal is based on all stored work units, calculating ppd from the first known unit to the the last one." & Environment.NewLine & "Current statistics are those which match your current client setup ( local and remote clients and slot configuration )." & Environment.NewLine & Environment.NewLine & "* note that when using a filter the statistics will be based on the filtered results", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle))
    End Sub
    Private Sub StartPageToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles StartPageToolStripMenuItem.Click
        Process.Start("http://folding.extremeoverclocking.com/")
    End Sub
    Private Sub TeamStatisticsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TeamStatisticsToolStripMenuItem.Click
        If EOCInfo.primaryAccount.LastUpdate.IsEmpty Then
            SetMessage("The teamID isn't known, can't open team statistics")
        Else
            Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&t=" & EOCInfo.primaryAccount.LastUpdate.Update.Team.TeamID)
        End If
    End Sub
    Private Sub UserStatisticsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UserStatisticsToolStripMenuItem.Click
        If EOCInfo.primaryAccount.LastUpdate.IsEmpty Then
            SetMessage("The userID isn't known, can't open team statistics")
        Else
            Process.Start("http://folding.extremeoverclocking.com/team_summary.php?s=&u=" & EOCInfo.primaryAccount.LastUpdate.Update.User.UserID)
        End If
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
            modMySettings.SaveSettings()
            delegateFactory.RaiseEOCViewUserChanged(ViewUserStatusToolStripMenuItem.Checked, Me)
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
            modMySettings.SaveSettings()
            delegateFactory.RaiseEOCViewTeamChanged(ViewTeamStatusToolStripMenuItem.Checked, Me)
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
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bStatisticsChanging = False
        End Try
    End Sub
    Private Sub OverallToolStripMenuItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OverallToolStripMenuItem.CheckedChanged
        Try
            If bManual Or bStatisticsChanging Then Exit Sub
            bStatisticsChanging = True
            CurrentToolStripMenuItem.Checked = Not OverallToolStripMenuItem.Checked
            'UpdateStatisticsStrip()
            If CurrentToolStripMenuItem.Checked Then
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current
            Else
                modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Overall
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bStatisticsChanging = False
        End Try
    End Sub
    Public Sub HandleFilterItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        Try
            tsiFilters.DropDown.Hide()
            Application.DoEvents()
            'Add limiters to performance/project/hardware statistics, and generate charts for them
            If Not IsNothing(e.ClickedItem.Tag) Then
                If e.ClickedItem.Tag.ToString = "Clients" Then
                    Dim strClient As String = FormatSQLString(e.ClickedItem.Text.Substring(0, e.ClickedItem.Text.IndexOf(Chr(32))), True, True)
                    Dim strQuery As String = sqlFilters.GetSqlClientLimit(strClient).Trim
                    If Not Fill(strQuery) Then
                        If sqdata.LastSQL <> String.Empty Then
                            MsgBox("This filter seems to return an SQL exception. Check the syntax of " & strQuery, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                        End If
                    End If
                End If
            End If
            If e.ClickedItem.OwnerItem.OwnerItem.Text = "Clients" Then
                'Get client name
                Dim strClient As String = FormatSQLString(e.ClickedItem.OwnerItem.Text.Substring(0, e.ClickedItem.OwnerItem.Text.IndexOf(Chr(32))), True, True)
                Dim strQuery As String = sqlFilters.GetSqlClientLimit(strClient).Trim & " AND HW like '%" & e.ClickedItem.Text & "%'"
                If Not Fill(strQuery) Then
                    If sqdata.LastSQL <> String.Empty Then
                        MsgBox("This filter seems to return an SQL exception. Check the syntax of " & strQuery, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                    End If
                End If
            ElseIf e.ClickedItem.OwnerItem.Text = "Projects" Then
                'Get range or number
                If e.ClickedItem.Text.ToUpper(CultureInfo.InvariantCulture).Contains(" TO ") Then
                    Dim iLower As Integer = CInt(e.ClickedItem.Text.Substring(0, e.ClickedItem.Text.IndexOf(Chr(32))))
                    Dim strQuery As String = e.ClickedItem.Text.Substring(e.ClickedItem.Text.ToUpper(CultureInfo.InvariantCulture).IndexOf(" TO ") + 4)
                    Dim iUpper As Integer = CInt(strQuery.Substring(0, strQuery.IndexOf("(")).Trim)
                    strQuery = sqlFilters.GetSqlProjectRangeLimit(iLower, iUpper)
                    If Not Fill(strQuery) Then
                        If sqdata.LastSQL <> String.Empty Then
                            MsgBox("This filter seems to return an SQL exception. Check the syntax of " & strQuery, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                        End If
                    End If
                Else
                    Dim strProject As String = e.ClickedItem.Text.Substring(0, e.ClickedItem.Text.IndexOf(Chr(32)))
                    Dim strQuery As String = sqlFilters.GetSqlProjectLimit(strProject).Trim
                    If Not Fill(strQuery) Then
                        If sqdata.LastSQL <> String.Empty Then
                            MsgBox("This filter seems to return an SQL exception. Check the syntax of " & strQuery, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                        End If
                    End If
                End If
            ElseIf e.ClickedItem.OwnerItem.Text = "Hardware" Then
                'Get hardware
                Dim sHW As String = e.ClickedItem.Text.Substring(0, e.ClickedItem.Text.LastIndexOf(Chr(32)))
                Dim strQuery As String = sqlFilters.GetSqlHardwareLimit(sHW).Trim
                If Not Fill(strQuery) Then
                    If sqdata.LastSQL <> String.Empty Then
                        MsgBox("This filter seems to return an SQL exception. Check the syntax of " & strQuery, CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                    End If
                End If
            ElseIf e.ClickedItem.OwnerItem.Text = "Stored filters" Then
                Dim strQuery As String = sqlFilters.sql(e.ClickedItem.Text)
                If Not Fill(strQuery) Then
                    If sqdata.LastSQL <> String.Empty Then
                        MsgBox("This filter seems to return an SQL exception. Check the syntax of " & CType(sender, ToolStripMenuItem).Text & "!", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "SQL exception")
                    End If
                End If
            Else
                WriteDebug("Unknown items?")
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub EditFiltersToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EditFiltersToolStripMenuItem.Click
        Try
            sqlFilters.ShowForm(Me)
            RefreshFilterMenu()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ClearToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ClearToolStripMenuItem.Click
        Try
            Fill()
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
                    Dim cb As New AsyncCallback(AddressOf ParserFinished)
                    If Clients.ParseLogs(False, Me, False) Then
                        WriteDebug("Logparser finished, took " & FormatTimeSpan(DateTime.Now.Subtract(dtStart), True))
                    End If
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub AdvancedToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AdvancedToolStripMenuItem.Click
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
        If modMySettings.MainForm = eMainForm.History Then
            Me.Close()
        Else
            Live.Close()
        End If
        'modMySettings.ColumnSettings.UpdateColumnSettings(lvWU)
        'appContext.ExitThread()
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        about.ShowAbout(Me)
    End Sub
    Private Sub LicenseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LicenseToolStripMenuItem.Click
        license.ShowLicense(Me)
    End Sub
    Private Sub ProjectInfoListToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProjectInfoListToolStripMenuItem.Click
        Try
            ProjectInfo.ShowList(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ProjectInfoCalculatorToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ProjectInfoCalculatorToolStripMenuItem.Click
        Try
            ProjectInfo.ShowGraph(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub ViewToolStripMenuItem_DropDownOpening(sender As Object, e As System.EventArgs) Handles ViewToolStripMenuItem.DropDownOpening
        Try
            SelectColumnsToolStripMenuItem.Enabled = True
            bManual = True
            If modMySettings.history_ViewDetails Then
                DetailedToolStripMenuItem.Checked = True
                DefaultToolStripMenuItem.Checked = False
            Else
                DetailedToolStripMenuItem.Checked = False
                DefaultToolStripMenuItem.Checked = True
            End If
            LargeGraphToolStripMenuItem.Checked = zGraph.IsFormVisible
            RemoveHandler SelectColumnsToolStripMenuItem.DropDownItemClicked, AddressOf ColumnCheckChanged
            With SelectColumnsToolStripMenuItem
                .DropDownItems.Clear()
                Try
                    Dim nItem As New ToolStripMenuItem("Reset all", My.Resources.iWarning1)
                    nItem.CheckOnClick = False
                    Dim bFont As Font = New Font(nItem.Font.FontFamily.Name, nItem.Font.Size, FontStyle.Bold)
                    nItem.Font = bFont
                    AddHandler nItem.Click, AddressOf ColumnCheckChanged
                    .DropDownItems.Add(nItem)
                    .DropDownItems.Add("-")
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                For Each cInfo As sColumnInfo In modMySettings.ColumnSettings.MasterSettings(lvWU).Values
                    Dim nItem As New ToolStripMenuItem(cInfo.Header)
                    nItem.CheckOnClick = True : nItem.Checked = cInfo.Visible
                    nItem.Checked = cInfo.Visible
                    AddHandler nItem.CheckedChanged, AddressOf ColumnCheckChanged
                    .DropDown.Items.Add(nItem)
                Next
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
        End Try
    End Sub
    Private Sub ColumnCheckChanged(sender As Object, e As System.EventArgs)
        Try
            If TypeOf (sender) Is ToolStripMenuItem AndAlso CType(sender, ToolStripMenuItem).Text = "Reset all" Then
                lvWU.BeginUpdate()
                lvWU.Clear()
                lvWU.EndUpdate()
                FillColumns(True)
            ElseIf TypeOf (sender) Is ToolStripMenuItem Then
                With CType(sender, ToolStripMenuItem)
                    Dim header As String = .Text.Replace("*", "").Replace("<", "").Replace(">", "")
                    If .Checked Then
                        'Add it, reset, fill
                        modMySettings.ColumnSettings.UpdateColumnVisible(lvWU, header, True)
                        Dim ch As New ColumnHeader
                        ch.Text = header
                        ch.Name = header
                        Try
                            Dim sList As SortedList(Of Int32, sColumnInfo) = modMySettings.ColumnSettings.MasterSettings(lvWU)
                            For xInt As Int32 = 0 To sList.Count - 1
                                If sList.Values(xInt).Header = header Then
                                    ch.DisplayIndex = xInt
                                    Exit For
                                End If
                            Next
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        lvWU.Columns.Add(ch)
                    Else
                        'remove it, reset, fill
                        Try
                            Dim cToRemove As ColumnHeader = Nothing
                            For Each ch As ColumnHeader In lvWU.Columns
                                If ch.Text = header Then
                                    cToRemove = ch
                                    Exit For
                                End If
                            Next
                            If Not IsNothing(cToRemove) Then
                                lvWU.Columns.Remove(cToRemove)
                                modMySettings.ColumnSettings.UpdateColumnVisible(lvWU, cToRemove.Text, False)
                            End If
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                    End If
                End With
                'modMySettings.columnVisible(lvWU.Name, CType(sender, ToolStripMenuItem).Text.Replace("*", "").Replace("<", "").Replace(">", "")) = CType(sender, ToolStripMenuItem).Checked
                modMySettings.ColumnSettings.UpdateColumnSettings(lvWU)
                lvWU.BeginUpdate()
                lvWU.Clear()
                lvWU.EndUpdate()
                FillColumns()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
        End Try
    End Sub
    Private Sub DetailedToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles DetailedToolStripMenuItem.Click
        Try
            modMySettings.history_ViewDetails = True
            modMySettings.SaveSettings()
            scMain.Panel2Collapsed = False
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub DefaultToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles DefaultToolStripMenuItem.Click
        Try
            modMySettings.history_ViewDetails = False
            modMySettings.SaveSettings()
            scMain.Panel2Collapsed = True
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
    Private Sub IconToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles IconToolStripMenuItem.Click
        Try
            EOCInfo.IconVisible = Not IconToolStripMenuItem.Checked
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub UpdateToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles UpdateToolStripMenuItem.Click
        Try
            Me.UseWaitCursor = True
            DisableControls()
            If Clients.ParseLogs(False, Me, False) Then
                Fill()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            Me.UseWaitCursor = False
            EnableControls()
        End Try
    End Sub
    Private Sub LogMessagesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LogMessagesToolStripMenuItem.Click
        Try
            Logwindow.ShowDebugWindow()
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
    Private Sub LargeGraphToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LargeGraphToolStripMenuItem.Click
        Try
            If bManual Then Exit Sub
            If zGraph.IsFormVisible Then
                zGraph.HideForm()
            Else
                Select Case modMySettings.history_detail
                    Case modMySettings.eDetail.Hardware
                        zGraph.ShowHardwareStatistics(Nothing)
                    Case modMySettings.eDetail.Performance
                        zGraph.ShowPerformanceStatistics(Nothing)
                    Case modMySettings.eDetail.Projects
                        zGraph.ShowProjectStatistics(ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.Text))
                    Case modMySettings.eDetail.WU
                        zGraph.ShowWUFrames(m_SelectedWU)
                End Select
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub LiveMonitoringToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LiveMonitoringToolStripMenuItem.Click
        If Not delegateFactory.IsFormVisible(Live) Then
            Live.ShowForm()
        Else
            Live.HideForm()
        End If
    End Sub
    Private Sub MinimizeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MinimizeToolStripMenuItem.Click
        If modMySettings.MinimizeToTray Then
            HideForm()
            Me.WindowState = FormWindowState.Minimized
        Else
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub
    Private Sub HowDoIEnableTheseToolsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles HowDoIEnableTheseToolsToolStripMenuItem.Click
        Try
            Tools.ShowDownload(Me)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub FileToolStripMenuItem_DropDownOpening(sender As System.Object, e As System.EventArgs) Handles FileToolStripMenuItem.DropDownOpening
        Try
            If delegateFactory.IsFormVisible(Live) Then
                LiveMonitoringToolStripMenuItem.Text = "Hide Live"
            Else
                LiveMonitoringToolStripMenuItem.Text = "Show Live"
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Splitters"
    Private Sub scMain_SplitterMoved(sender As System.Object, e As System.Windows.Forms.SplitterEventArgs) Handles scMain.SplitterMoved
        Try
            If Not Me.Created Then Exit Sub
            WriteDebug("History main splitter moved, value: " & scMain.SplitterDistance.ToString)
            modMySettings.history_tcMain_splitterdistance = scMain.SplitterDistance
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub scDetail_SplitterMoved(sender As System.Object, e As System.Windows.Forms.SplitterEventArgs) Handles scDetail.SplitterMoved
        Try
            If Not Me.Created Then Exit Sub
            WriteDebug("History detail splitter moved, value: " & scDetail.SplitterDistance.ToString)
            modMySettings.history_tcdetails_splitterdistance = scDetail.SplitterDistance
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Form handling"
    'Protected Overrides Sub WndProc(ByRef m As Message)
    '    Try
    '        If m.Msg = NativeMethods.WM_SYSCOMMAND Then
    '            Select Case m.WParam.ToInt32()
    '                Case NativeMethods.WM_SYSCOMMANDS.SC_MAXIMIZE
    '                    WriteDebug(Me.Name & " gets maximized")
    '                Case NativeMethods.WM_SYSCOMMANDS.SC_MINIMIZE
    '                    WriteDebug(Me.Name & " gets minimized")
    '                    If mySettings.MinimizeToTray Then
    '                        WriteDebug(Me.Name & " playing hideFade animation")
    '                        delegateFactory.HideFade(Me)
    '                        WriteDebug("showing tray icon")
    '                        Tray.IconShow()
    '                        Return
    '                    End If
    '                Case NativeMethods.WM_SYSCOMMANDS.SC_RESTORE
    '                    WriteDebug(Me.Name & " gets restored.")
    '                Case NativeMethods.WM_SYSCOMMANDS.SC_CLOSE
    '                    WriteDebug(Me.Name & " gets closed.")
    '                Case Else
    '                    WriteDebug(Me.Name & " recieved WM_SYSCOMMAND: " & m.WParam.ToInt32)
    '            End Select
    '        End If
    '        MyBase.WndProc(m)
    '    Catch ex As Exception
    '        WriteLog("Message: " & m.WParam.ToInt32, eSeverity.Critical)
    '        WriteError(ex.Message, Err)
    '    End Try
    'End Sub
    Friend Property SilentClose As Boolean
        Get
            Return mSilentClose
        End Get
        Set(value As Boolean)
            mSilentClose = value
        End Set
    End Property
    Friend Sub ShowForm()
        ShowFade(Me)
        ActivateForm(Me)
    End Sub
    Friend Sub HideForm()
        HideFade(Me)
        Me.Visible = False
    End Sub

    Private Sub frmHistory_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        Me.Invalidate(True)
        'InvokePaint(Me, New System.Windows.Forms.PaintEventArgs(Me.CreateGraphics, Me.DisplayRectangle))
    End Sub
    Private Sub frmHistory_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
       
    End Sub
    Private Sub frmHistory_LocationChanged(sender As Object, e As System.EventArgs) Handles Me.LocationChanged
        Try
            If Not Me.Created Or bManual Or Me.WindowState = FormWindowState.Minimized Then Exit Sub
            modMySettings.history_formlocation = Me.Location
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmHistory_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        Try
            If Not Me.Created Or bManual Then Exit Sub
            If Not Me.WindowState = FormWindowState.Minimized Then
                modMySettings.history_formlocation = Me.Location
                modMySettings.history_windowstate = Me.WindowState
                If Not Me.WindowState = FormWindowState.Maximized Then modMySettings.history_formsize = Me.Size
            Else
                WriteDebug(Me.Name & " gets minimized")
                If modMySettings.MinimizeToTray Then
                    WriteDebug(Me.Name & " playing hideFade animation")
                    delegateFactory.HideFade(Me)
                    WriteDebug("showing tray icon")
                    modIcon.ShowIcon()
                    Return
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmHistory_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            modMySettings.ColumnSettings.CreateMaster(lvWU)
            AddHandler delegateFactory.HideInactiveMessageStripChanged, AddressOf HideInactiveMessageStripChanged
            AddHandler delegateFactory.DefaultStatisticsChanged, AddressOf DefaultStatisticsChanged
            AddHandler delegateFactory.EOC_EnabledChanged, AddressOf EOC_EnabledChanged
            AddHandler delegateFactory.EOC_ViewTeamChanged, AddressOf EOC_ViewTeamChanged
            AddHandler delegateFactory.EOC_ViewUserChanged, AddressOf EOC_ViewUserChanged
            bManual = True
            lvWU.ListViewItemSorter = lvSorter
            Cursor.Current = Cursors.AppStarting
            sStripMessage.Visible = Not modMySettings.HideInactiveMessageStrip
            sStripEOC.Visible = Not modMySettings.DisableEOC : sStripEoc2.Visible = Not modMySettings.DisableEOC
            scMain.Panel2Collapsed = Not modMySettings.history_ViewDetails
            tscHistory.TopToolStripPanel.Visible = True
            If Not modMySettings.history_formlocation.Equals(New Point(0, 0)) Then
                Me.Location = modMySettings.history_formlocation
            End If
            Me.WindowState = modMySettings.history_windowstate
            If Not modMySettings.history_windowstate = FormWindowState.Maximized Then
                If Not modMySettings.history_formsize.Equals(New Size(0, 0)) Then
                    Me.Size = modMySettings.history_formsize
                End If
            End If
#If CONFIG = "Debug" Then
            Console.WriteLine("::History.Shown")
#End If
            If modMySettings.history_ViewDetails Then
                Select Case modMySettings.history_detail
                    Case modMySettings.eDetail.Hardware
                        tcDetail.SelectedIndex = 3
                    Case modMySettings.eDetail.Performance
                        tcDetail.SelectedIndex = 1
                    Case modMySettings.eDetail.Projects
                        tcDetail.SelectedIndex = 2
                    Case modMySettings.eDetail.WU
                        tcDetail.SelectedIndex = 0
                End Select
            End If
            scMain.SplitterDistance = modMySettings.history_tcMain_splitterdistance
            scDetail.SplitterDistance = modMySettings.history_tcdetails_splitterdistance
            tShow.Enabled = True
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub frmHistory_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            If modMySettings.MinimizeToTray Then
                modIcon.ShowIcon()
                delegateFactory.HideFade(Me)
            End If
        ElseIf Not Me.WindowState = FormWindowState.Maximized Then
            modMySettings.history_formsize = Me.Size
        End If
    End Sub
    Private Sub tShow_Tick(sender As System.Object, e As System.EventArgs) Handles tShow.Tick
        Try
            tShow.Enabled = False
            Me.Activate()
            IsShown = True
            If Not modMySettings.HideInactiveMessageStrip Then sStripMessage.Visible = True
            tsHQFTimeLimit.SelectedIndex = 0
            RefreshFilterMenu()

            sStripEOC.Visible = Not modMySettings.DisableEOC : sStripEoc2.Visible = Not modMySettings.DisableEOC
            If Not modMySettings.DisableEOC Then
                AddHandler delegateFactory.EOC_UpdateRecieved, AddressOf EocUpdateHandler
                Call EocUpdateHandler(Me, New MyEventArgs.EocUpdateArgs(EOCInfo.primaryAccount))
                bSurpress = False
            End If
            ViewTeamStatusToolStripMenuItem.Checked = modMySettings.viewEocTeam
            ViewUserStatusToolStripMenuItem.Checked = modMySettings.viewEocUser

            FillColumns()
           
            If modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current Then
                CurrentToolStripMenuItem.Checked = True
            Else
                OverallToolStripMenuItem.Checked = True
            End If
            'Detail_zGraph()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            Me.UseWaitCursor = False
        End Try
    End Sub
    Public ReadOnly Property HasBeenShown As Boolean
        Get
            Return IsShown
        End Get
    End Property
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        lvSorter = New ListViewColumnSorter
        lvWU.ListViewItemSorter = lvSorter
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region
#Region "Async parser finishing"
    Private Sub ParserFinished(aR As System.IAsyncResult)
        ' not used right now
    End Sub
#End Region
#Region "Column stuff"
    Private Sub FillColumns(Optional Reset As Boolean = False)
        Try
            Me.UseWaitCursor = True
            bManual = True
            DisableControls()
            lvWU.BeginUpdate()
            Dim iIndexOffset As Int32 = 0
            lvWU.Clear()
            If Reset Then
                For Each cInfo In modMySettings.ColumnSettings.MasterSettings(lvWU).Values
                    modMySettings.ColumnSettings.UpdateColumnVisible(lvWU, cInfo.Header, True)
                    Dim ch As New ColumnHeader
                    ch.Text = cInfo.Header
                    ch.Width = cInfo.Width
                    lvWU.Columns.Add(ch)
                Next
                modMySettings.ColumnSettings.InitMasters()
            Else
                For Each cInfo In modMySettings.ColumnSettings.ColumnSettings(lvWU).Values
                    Dim ch As New ColumnHeader
                    ch.Text = cInfo.Header.Replace("*", "").Replace("<", "").Replace(">", "")
                    ch.Width = cInfo.Width
                    lvWU.Columns.Add(ch)
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            'lvWU.EndUpdate()
            'Me.UseWaitCursor = False
            'bManual = False
            Fill(mLastFill)
        End Try
    End Sub
    Private Sub lvWU_ColumnReordered(sender As System.Object, e As System.Windows.Forms.ColumnReorderedEventArgs) Handles lvWU.ColumnReordered
        Try
            If bManual Then Exit Sub
            If Not Me.Created Then Exit Sub
#If CONFIG = "Debug" Then
            Console.WriteLine("::ColumnReordered: " & e.Header.Text & "displayindex - old: " & e.OldDisplayIndex & " new: " & e.NewDisplayIndex)
#End If

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvWU_ColumnWidthChanging(sender As System.Object, e As System.Windows.Forms.ColumnWidthChangingEventArgs) Handles lvWU.ColumnWidthChanging
        Try
            If Not modMySettings.NoAutoSizeColumns Then
                e.Cancel = True
                delegateFactory.SetMessage("Autoresize columns is enabled, disable this first")
                Return
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub lvWU_ColumnWidthChanged(sender As System.Object, e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvWU.ColumnWidthChanged
        Try
            If bManual Then Exit Sub
            If Not Me.Created Then Exit Sub
            If Not modMySettings.NoAutoSizeColumns Then
                For Each chinfo In modMySettings.ColumnSettings.ColumnSettings(lvWU)
                    If chinfo.Key = e.ColumnIndex Then
                        bManual = True
                        lvWU.Columns(e.ColumnIndex).Width = chinfo.Value.Width
                        bManual = False
                        Exit For
                    End If
                Next
            End If
            modMySettings.ColumnSettings.UpdateColumnSettings(lvWU)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "zedGraph"
    Private Sub Detail_zGraph()
        Try
            If Not Me.Created Or bManual Then Exit Sub
            Select Case modMySettings.history_detail
                Case modMySettings.eDetail.Hardware
                    zgProject.MasterPane.PaneList.Clear()
                    Dim nPane As New GraphPane
                    nPane.Title.Text = "There will be some charts here for your hardware... soon :)"
                    nPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.GraphPane = nPane
                    zgProject.Refresh()
                    If zGraph.IsFormVisible Then zGraph.ShowHardwareStatistics()
                Case modMySettings.eDetail.Performance
                    zgProject.MasterPane.PaneList.Clear()
                    Dim nPane As New GraphPane
                    nPane.Title.Text = "There will be some charts here for your performance ... soon :)"
                    nPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.GraphPane = nPane
                    zgProject.Refresh()
                    If zGraph.IsFormVisible Then zGraph.ShowPerformanceStatistics()
                Case modMySettings.eDetail.Projects
                    Static oldProject As String = ""
                    'If oldProject = tsProjects_cmbProjects.Text Then Return
                    oldProject = tsProjects_cmbProjects.Text
                    Dim ProjectStats As clsStatistics.clsProjectStatistics.clsProject = ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.Text)
                    zgProject.MasterPane.PaneList.Clear()


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
                    Dim strLabel(0 To ProjectStats.Projects.Count + ProjectStats.HW_Names.Count) As String
                    Dim lPPD As New List(Of Double)
                    Dim lTPF As New List(Of Double)
                    Try
                        lPPD.Add(CDbl(ProjectStats.AvgPPD))
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
                            lPPD.Add(CDbl(ProjectStats.AvgPPD(Name)))
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
                            lPPD.Add(CDbl(Project.PPD))
                            If CDbl(Project.PPD) > 1000 Then bThousands = True
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
                    ppdPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.MasterPane.Add(ppdPane)
                    zgProject.MasterPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.GraphPane.AxisChange()
                    If bHours Then ppdPane.Y2Axis.ResetAutoScale(zgProject.GraphPane, zgProject.CreateGraphics)
                    zgProject.Refresh()
                    'ppdPane.Y2Axis.ResetAutoScale(ppdPane, zgProject.CreateGraphics)
                    zgProject.IsShowPointValues = True
                    If zGraph.IsFormVisible Then zGraph.ShowProjectStatistics(ProjectStats)

                    ''Add panes for tpf/ppd, pie chart for succes
                    ''PPD/TPF pane
                    'Dim ps As clsStatistics.clsProjectStatistics.clsProject = ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.Text)
                    'Dim ppdPane As New GraphPane
                    'ppdPane.Rect = New Rectangle(New Point(0, 0), New Size(250, scDetail.Panel2.ClientRectangle.Height))
                    ''Dim dblPPD_Low As Double = Double.MaxValue, dblPPD_High As Double = Double.MinValue
                    ''Dim dblTPF_Low As Double = Double.MaxValue, dblTPF_High As Double = Double.MinValue
                    ''ppdPane.Title.Text = "Project: " & pS.Number & Chr(32) & " ppd: " & FormatPPD(pS.AvgPPD) & " tpf: " & pS.AvgTPF & " succes rate: " & pS.SuccesRate
                    'ppdPane.YAxis.Title.Text = "PPD"
                    'ppdPane.Y2Axis.Title.Text = "TPF"
                    'ppdPane.Y2Axis.Title.IsVisible = True
                    'ppdPane.YAxis.MajorTic.IsOpposite = False
                    'ppdPane.YAxis.MinorTic.IsOpposite = False
                    'ppdPane.YAxis.Scale.Min = 0
                    'ppdPane.Y2Axis.MajorTic.IsOpposite = False
                    'ppdPane.Y2Axis.MinorTic.IsOpposite = False
                    ''ppdPane.YAxis.Scale.Align = AlignP.Inside
                    'ppdPane.Y2Axis.IsVisible = True
                    'ppdPane.Y2Axis.Type = AxisType.Date
                    'ppdPane.Y2Axis.Scale.Format = "mm:ss"
                    'ppdPane.Y2Axis.Scale.Min = 0
                    'ppdPane.Y2Axis.MajorGrid.IsZeroLine = False
                    'ppdPane.XAxis.MajorTic.IsBetweenLabels = True
                    'ppdPane.XAxis.Type = AxisType.Text
                    'ppdPane.XAxis.Scale.FontSpec.Angle = 0
                    'ppdPane.XAxis.Scale.FontSpec.Size = 18
                    'ppdPane.XAxis.Scale.FontSpec.IsBold = True
                    'ppdPane.XAxis.Scale.Align = AlignP.Outside
                    'ppdPane.XAxis.Scale.AlignH = AlignH.Center
                    'ppdPane.XAxis.Scale.LabelGap = 0.1
                    'ppdPane.Chart.Border.IsVisible = False
                    ''ppdPane.XAxis.Scale.TextLabels = strLabel
                    'Dim clonePane As GraphPane = ppdPane.Clone
                    'ppdPane.XAxis.Scale.TextLabels = {ps.Number & " average"}

                    ''Dim biPPD As BarItem = ppdPane.AddBar("PPD", Nothing, lPPD.ToArray, Color.Green)
                    ''Dim biTPF As BarItem = ppdPane.AddBar("TPF", Nothing, lTPF.ToArray, Color.Yellow)

                    'Dim biPPD As BarItem = ppdPane.AddBar("PPD", Nothing, {CDbl(pS.AvgPPD)}, Color.Green)
                    'Dim biTPF2 As BarItem = ppdPane.AddBar("TPF", Nothing, {TimeSpan.Parse(pS.AvgTPF).TotalDays}, Color.Yellow)
                    'biTPF2.IsY2Axis = True

                    ''ppdPane.YAxis.Scale.FormatAuto = True
                    ''ppdPane.Rect = scDetail.Panel2.ClientRectangle
                    ''ppdPane.AxisChange()
                    'zgProject.MasterPane.Add(ppdPane)
                    'zgProject.MasterPane.Rect = ppdPane.Rect

                    ''Dim strLabel(0 To pS.Projects.Count - 1) As String
                    ''Dim lPPD As New List(Of Double)
                    ' ''Dim lTPF As New List(Of Double)
                    ''Dim lTPF As New List(Of Double)
                    ''Dim iInd As Int32 = 0
                    'For Each Project As clsWU In pS.Projects
                    '    If (Not IsNothing(Project.tpfDB) And Not IsNothing(Project.PPD)) And (Not Project.tpfDB = "" And Not Project.tpfDB = "00:00:00") And (Not Project.PPD = "" And Not Project.PPD = "0") Then
                    '        Dim nPane As GraphPane = clonePane.Clone
                    '        nPane.AddBar("PPD", Nothing, {CDbl(Project.PPD)}, Color.Green)
                    '        Dim biTPF As BarItem = nPane.AddBar("TPF", Nothing, {TimeSpan.Parse(Project.tpfDB).TotalDays}, Color.Yellow)
                    '        biTPF.IsY2Axis = True
                    '        nPane.XAxis.Scale.TextLabels = {Project.RCG_Short & Environment.NewLine & Project.HW.ToString}
                    '        'nPane.Tag = {Project.RCG_Short & Environment.NewLine & Project.HW.ToString}
                    '        'zgProject.MasterPane.Rect.Inflate(nPane.Rect.Width, 0)
                    '        'zgProject.MasterPane.Rect = New Rectangle(0, 0, CInt(zgProject.MasterPane.Rect.Width + nPane.Rect.Width), CInt(nPane.Rect.Height))

                    '        zgProject.MasterPane.Add(nPane)

                    '        'strLabel(iInd) = Project.RCG_Short
                    '        'iInd += 1
                    '        'lPPD.Add(CDbl(Project.PPD))
                    '        ''If CDbl(Project.PPD) < dblPPD_Low Then dblPPD_Low = CDbl(Project.PPD)
                    '        ''If CDbl(Project.PPD) > dblPPD_High Then dblPPD_High = CDbl(Project.PPD)
                    '        'Dim tsTPF As TimeSpan = TimeSpan.Parse(Project.tpfDB)
                    '        'lTPF.Add(tsTPF.TotalDays)
                    '        'If tsTPF.TotalDays < dblTPF_Low Then dblTPF_Low = tsTPF.TotalDays
                    '        'If tsTPF.TotalDays > dblTPF_High Then dblTPF_High = tsTPF.TotalDays
                    '    Else
                    '        'ReDim Preserve strLabel(0 To strLabel.GetUpperBound(0) - 1)
                    '    End If
                    'Next


                    ''piePane
                    ''Dim piePane As New GraphPane()
                    ''With piePane
                    ''    Dim piSucces As PieItem = piePane.AddPieSlice(pS.DBL_succesRate, Color.Green, Color.Yellow, 45.0F, 0, "FINISHED_UNIT")
                    ''    Dim piFailed As PieItem = piePane.AddPieSlice(100 - pS.DBL_succesRate, Color.Red, Color.Purple, 45.0F, 0.2, "EUE")
                    ''    .AxisChange()
                    ''End With
                    ''zgProject.MasterPane.Add(piePane)
                    ''zgProject.MasterPane.Rect = scDetail.Panel2.ClientRectangle
                    'Using g As Graphics = zgProject.CreateGraphics
                    '    zgProject.MasterPane.SetLayout(g, PaneLayout.SingleRow)
                    'End Using
                    'zgProject.ScrollMaxX = zgProject.MasterPane.PaneList.Count
                    'zgProject.MasterPane.AxisChange()
                    'zgProject.Refresh()
                    'If zgProject.MasterPane.Rect.Width > scDetail.Panel2.ClientRectangle.Width Then
                    '    zgProject.IsShowHScrollBar = True
                    'Else
                    '    zgProject.IsShowHScrollBar = False
                    'End If
                    'If zGraph.IsFormVisible Then zGraph.ShowProjectStatistics(ps, Me)
                Case modMySettings.eDetail.WU
                    Dim ppProject As New ZedGraph.PointPairList
                    Dim bHour As Boolean = False, mMinValue As Double = Double.MaxValue, mMaxValue As Double = Double.MinValue
                    If m_SelectedWU.Frames.Count > 0 Then
                        For xInt As Int32 = 0 To m_SelectedWU.Frames.Count - 1
                            Dim tsFrame As New TimeSpan(0)
                            If xInt = 0 Then
                                tsFrame = m_SelectedWU.Frames(0).FrameDT - m_SelectedWU.dtStarted
                            Else
                                tsFrame = m_SelectedWU.Frames(xInt).FrameDT - m_SelectedWU.Frames(xInt - 1).FrameDT
                            End If
                            If tsFrame.TotalDays > mMaxValue Then mMaxValue = tsFrame.TotalDays
                            If tsFrame.TotalDays < mMinValue Then mMinValue = tsFrame.TotalDays
                            If tsFrame.Duration.TotalHours > 1 Then
                                bHour = True
                            End If
                            If xInt = 0 Then
                                ppProject.Add(New XDate(m_SelectedWU.dtStarted), tsFrame.TotalDays)
                            Else
                                ppProject.Add(New XDate(m_SelectedWU.Frames(xInt).FrameDT), tsFrame.TotalDays)
                            End If
                        Next
                    End If
                    zgProject.MasterPane.PaneList.Clear()
                    Dim mPane As New GraphPane
                    mPane.Title.Text = m_SelectedWU.HW & Chr(32) & m_SelectedWU.ClientName & m_SelectedWU.Slot & Chr(32) & m_SelectedWU.PRCG
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

                    Dim tsSpan As New TimeSpan
                    Try
                        tsSpan = New XDate(mMaxValue).DateTime.Subtract(New XDate(mMinValue))
                    Catch ex As Exception
                        WriteLog("Can't graph frame interval's for a work unit with no completed frames", eSeverity.Important)
                        GoTo skipFrames
                    End Try

                    Console.WriteLine(m_SelectedWU.PRCG & " " & tsSpan.ToString)
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
skipFrames:
                    mPane.YAxis.Title.Text = "Time per frame ( discarding idle time )"
                    mPane.YAxis.Title.FontSpec.Size = 10
                    zgProject.IsShowPointValues = True
                    mPane.AddCurve("", ppProject, Color.Blue)
                    mPane.Rect = scDetail.Panel2.ClientRectangle
                    zgProject.GraphPane = mPane
                    zgProject.AxisChange()
                    If bHour Then mPane.YAxis.ResetAutoScale(zgProject.GraphPane, zgProject.CreateGraphics)
                    zgProject.Refresh()
                    If zGraph.IsFormVisible Then zGraph.ShowWUFrames(m_SelectedWU)
            End Select
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Function zgProject_PointValueEvent(sender As ZedGraph.ZedGraphControl, pane As ZedGraph.GraphPane, curve As ZedGraph.CurveItem, iPt As Integer) As String Handles zgProject.PointValueEvent
        Try
            If tcDetail.SelectedIndex = 2 Then
                Dim s As String = "Project " & tsProjects_cmbProjects.Text & Chr(32)
                s &= pane.XAxis.Scale.TextLabels(CInt(curve.Points(iPt).X) - 1).Substring(0, pane.XAxis.Scale.TextLabels(CInt(curve.Points(iPt).X) - 1).IndexOf(Environment.NewLine)).Replace("R", "run:").Replace("C", " clone:").Replace("G", " gen:") & pane.XAxis.Scale.TextLabels(CInt(curve.Points(iPt).X) - 1).Substring(pane.XAxis.Scale.TextLabels(CInt(curve.Points(iPt).X) - 1).IndexOf(Environment.NewLine))
                If curve.Label.Text.ToLowerInvariant = "tpf" Then
                    Dim xD As New XDate(curve.Points(iPt).Y)
                    Return s & Chr(32) & "tpf: " & FormatTimeSpan(xD.DateTime.TimeOfDay)
                Else
                    Return s & Chr(32) & "ppd: " & FormatPPD(curve.Points(iPt).Y.ToString)
                End If
            ElseIf tcDetail.SelectedIndex = 0 Then
                With curve.Points(iPt)
                    Dim dtC As XDate = New XDate(.X)
                    Dim tsF As XDate = New XDate(.Y)
                    If tsF.DateTime.TimeOfDay.TotalMilliseconds > 0 Then
                        Return dtC.ToString & Chr(32) & "completed " & iPt + 1 & "% tpf: " & FormatTimeSpan(tsF.DateTime.TimeOfDay)
                    Else
                        Return dtC.ToString & Chr(32) & "completed " & iPt + 1 & "%"
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
    Private Sub zgProject_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        If zGraph.IsFormVisible Then
            zGraph.HideForm()
        Else
            If tcDetail.SelectedIndex = 0 Then
                zGraph.ShowWUFrames(m_SelectedWU)
            ElseIf tcDetail.SelectedIndex = 2 Then
                Dim pS As clsStatistics.clsProjectStatistics.clsProject = ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.Text)
                zGraph.ShowProjectStatistics(pS)
            End If
        End If
    End Sub
    'Private Sub zgProject_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles zgProject.MouseClick
    '    If e.Button = Windows.Forms.MouseButtons.Middle Then
    '        Dim cPane As ZedGraph.GraphPane = zgProject.MasterPane.FindPane(e.Location)
    '        Dim cItem As Object = Nothing, iIndex As Int32 = 0
    '        If zgProject.MasterPane.FindNearestPaneObject(e.Location, zgProject.CreateGraphics, cPane, cItem, iIndex) Then
    '            If TypeOf (cItem) Is BarItem Then
    '                With CType(cItem, BarItem)
    '                    Dim strCoord As String = ""
    '                    If .GetCoords(cPane, iIndex, strCoord) Then
    '                        zgProject.ZoomPane(cPane, 1, e.Location, True)
    '                    Else


    '                    End If
    '                End With
    '            ElseIf TypeOf (cItem) Is PieItem Then


    '            End If
    '        Else

    '        End If

    '    End If
    'End Sub
#End Region
#Region "tpDetails"
    Private Sub tcDetail_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tcDetail.SelectedIndexChanged
        If Not Me.Created Then Exit Sub
        If bManual Then Exit Sub
        Try
            modMySettings.history_details_index = CStr(tcDetail.SelectedIndex)
            Select Case modMySettings.history_detail
                Case modMySettings.eDetail.Hardware
                    Detail_Hardware()
                Case modMySettings.eDetail.Performance
                    Detail_Performance()
                Case modMySettings.eDetail.Projects
                    Detail_Projects()
                    Detail_zGraph()
                Case modMySettings.eDetail.WU
                    Detail_WU()
            End Select
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Detail_Hardware()
        rtHW.Clear()
        rtHW.Text = clsStatistics.clsHardwareStatistics.Report
        Detail_zGraph()
    End Sub
    Private Sub Detail_Performance()
        Try
            rtPerformance.Clear()
            rtPerformance.Text = clsStatistics.clsPerformanceStatistics.CurrentStatistics.Report
            Detail_zGraph()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Detail_WU()
        'Don't update on tab chancge is wu hasn't changed 
        Try
            Static oldWU As clsWU = Nothing
            If Not IsNothing(oldWU) Then
                If oldWU.Equals(m_SelectedWU) Then
                    ' Do nothing
                Else
                    rtWU.Clear()
                    rtWU.Text = m_SelectedWU.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.CoreSnippet & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientConfig.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientInfo.Report
                    Detail_zGraph()
                End If
            Else
                rtWU.Clear()
                rtWU.Text = m_SelectedWU.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.CoreSnippet & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientConfig.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientInfo.Report
                Detail_zGraph()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub Detail_Projects()
        Try
            Static oldWu As clsWU = Nothing
            If Not IsNothing(oldWu) Then
                If oldWu.Equals(m_SelectedWU) Then
                    'do nothing 
                Else
                    oldWu = m_SelectedWU
                    If tsProjects_cmbProjects.Items.Contains(m_SelectedWU.Project) Then
                        tsProjects_cmbProjects.SelectedIndex = tsProjects_cmbProjects.Items.IndexOf(m_SelectedWU.Project)
                    End If
                    'Detail_zGraph()
                End If
            Else
                tsProjects_cmbProjects.AutoCompleteCustomSource.Clear()
                tsProjects_cmbProjects.AutoCompleteCustomSource.AddRange(ProjectStatistics.lProjects.ToArray)
                tsProjects_cmbProjects.Items.Clear()
                tsProjects_cmbProjects.Items.AddRange(ProjectStatistics.lProjects.ToArray)
                rtProjects.Clear()
                rtProjects.Text = clsStatistics.clsProjectStatistics.Report
                If tsProjects_cmbProjects.Items.Count > 0 Then tsProjects_cmbProjects.SelectedIndex = 0
                Detail_zGraph()
                oldWu = m_SelectedWU
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#Region "Project details toolstrip"
    Private Sub tsProjects_cmbProjects_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tsProjects_cmbProjects.SelectedIndexChanged
        If Not Me.Created Or bManual Then Exit Sub
        '"Project number:" & vbTab & .Number
        If rtProjects.Text.Contains("Project number:" & vbTab & tsProjects_cmbProjects.Text) Then
            rtProjects.SelectionStart = rtProjects.Text.IndexOf("Project number:" & vbTab & tsProjects_cmbProjects.Text)
            rtProjects.ScrollToCaret()
            Dim pS As clsStatistics.clsProjectStatistics.clsProject = ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.Text)
            tsProjects_cmbRCG.Items.Clear()
            tsProjects_cmbRCG.Items.Add("-All-")
            For Each Project As clsWU In pS.Projects
                tsProjects_cmbRCG.Items.Add(Project.RCG & "::" & Project.HW)
            Next
            If tsProjects_cmbRCG.Items.Count > 0 Then tsProjects_cmbRCG.SelectedIndex = 0
            Detail_zGraph()
        End If
    End Sub
    Private Sub tsProjects_cmbRCG_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tsProjects_cmbRCG.SelectedIndexChanged
        Try
            If tsProjects_cmbRCG.SelectedIndex = -1 Then Exit Sub
            If tsProjects_cmbRCG.SelectedIndex = 0 Then
                If rtProjects.Text.Contains("Project number:" & vbTab & tsProjects_cmbProjects.Text) Then
                    rtProjects.SelectionStart = rtProjects.Text.IndexOf("Project number:" & vbTab & tsProjects_cmbProjects.Text)
                    rtProjects.ScrollToCaret()
                End If
                Exit Sub
            End If
            If tcDetail.SelectedIndex = 2 And rtProjects.Text.Contains("PRCG: " & vbTab & vbTab & "Project:" & tsProjects_cmbProjects.SelectedItem.ToString & " " & tsProjects_cmbRCG.SelectedItem.ToString.Substring(0, tsProjects_cmbRCG.SelectedItem.ToString.IndexOf("::"))) Then
                rtProjects.SelectionStart = rtProjects.Text.IndexOf("PRCG: " & vbTab & vbTab & "Project:" & tsProjects_cmbProjects.SelectedItem.ToString & " " & tsProjects_cmbRCG.SelectedItem.ToString.Substring(0, tsProjects_cmbRCG.SelectedItem.ToString.IndexOf("::")))
                rtProjects.ScrollToCaret()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#End Region
#Region "EocStrip updates"
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
    Private bSurpress As Boolean = True
    Friend Sub EocUpdateHandler(sender As Object, e As MyEventArgs.EocUpdateArgs)
        If Not e.EOCAccount.Equals(EOCInfo.primaryAccount) Then
            WriteLog("EOC Update recieved for non primary Eoc account, not updating status")
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
#Region "Context menus"
    Private Sub cmDetailWU_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles cmDetailWU.Opening
        If IsNothing(m_SelectedWU) Then
            e.Cancel = True
        Else
            cmDetailWU.Items.Clear()
            cmDetailWU.Items.Add("Copy details to clipboard")
            cmDetailWU.Items.Add("-")
            cmDetailWU.Items.Add("Copy wu report to clipboard")
            cmDetailWU.Items.Add("Copy wu core snippet to clipboard")
            cmDetailWU.Items.Add("Copy wu's client config to clipboard")
            cmDetailWU.Items.Add("Copy wu's client info to clipboard")
            cmDetailWU.Items.Add("-")
            cmDetailWU.Items.Add("Open the 'Issues with a specific WU' section at foldingforum.org")
        End If
    End Sub
    Private Sub cmDetailWU_ItemClicked(sender As System.Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmDetailWU.ItemClicked
        Try
            If e.ClickedItem.Text = "Copy details to clipboard" Then
                Clipboard.SetText(m_SelectedWU.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.CoreSnippet & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientConfig.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientInfo.Report)
            ElseIf e.ClickedItem.Text = "Copy wu report to clipboard" Then
                Clipboard.SetText(m_SelectedWU.Report)
            ElseIf e.ClickedItem.Text = "Copy wu core snippet to clipboard" Then
                Clipboard.SetText(m_SelectedWU.CoreSnippet)
            ElseIf e.ClickedItem.Text = "Copy wu's client config to clipboard" Then
                Clipboard.SetText(m_SelectedWU.ClientConfig.Report)
            ElseIf e.ClickedItem.Text = "Copy wu's client info to clipboard" Then
                Clipboard.SetText(m_SelectedWU.ClientInfo.Report)
            ElseIf e.ClickedItem.Text = "Open the 'Issues with a specific WU' section at foldingforum.org" Then
                Process.Start(urlIssuesWithWU)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmDetails_ItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles cmDetails.ItemClicked
        Try
            If tcDetail.SelectedIndex = 0 Then
                If e.ClickedItem.Text = "Open log" Then

                ElseIf e.ClickedItem.Text = "Copy selected text to clipboard" Then
                    Clipboard.SetText(rtWU.SelectedText)
                ElseIf e.ClickedItem.Text = "Copy details to clipboard" Then
                    Clipboard.SetText(m_SelectedWU.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.CoreSnippet & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientConfig.Report & Environment.NewLine & Environment.NewLine & m_SelectedWU.ClientInfo.Report)
                ElseIf e.ClickedItem.Text = "Copy wu report to clipboard" Then
                    Clipboard.SetText(m_SelectedWU.Report)
                ElseIf e.ClickedItem.Text = "Copy wu core snippet to clipboard" Then
                    Clipboard.SetText(m_SelectedWU.CoreSnippet)
                ElseIf e.ClickedItem.Text = "Copy wu's client config to clipboard" Then
                    Clipboard.SetText(m_SelectedWU.ClientConfig.Report)
                ElseIf e.ClickedItem.Text = "Copy wu's client info to clipboard" Then
                    Clipboard.SetText(m_SelectedWU.ClientInfo.Report)
                ElseIf e.ClickedItem.Text = "Open FoldingForum.org's Isseus with a specific WU section" Then
                    Process.Start(urlIssuesWithWU)
                End If
            ElseIf tcDetail.SelectedIndex = 1 Then
                If e.ClickedItem.Text = "Copy selected text to clipboard" Then
                    Clipboard.SetText(rtPerformance.SelectedText)
                ElseIf e.ClickedItem.Text = "Copy all text to clipboard" Then
                    Clipboard.SetText(rtPerformance.Text)
                Else
                    For Each Client As Client In Clients.Clients
                        If e.ClickedItem.Text = "Copy data for " & Client.ClientName & " to clipboard" Then
                            Clipboard.SetText(clsStatistics.clsPerformanceStatistics.CurrentStatistics.Report(Client.ClientName))
                            Exit Sub
                        End If
                    Next
                End If
            ElseIf tcDetail.SelectedIndex = 2 Then
                If e.ClickedItem.Text = "Copy selected text to clipboard" Then
                    Clipboard.SetText(rtProjects.SelectedText)
                ElseIf e.ClickedItem.Text = "Copy all text to clipboard" Then
                    Clipboard.SetText(rtProjects.Text)
                ElseIf e.ClickedItem.Text = "Copy all text for project: " & tsProjects_cmbProjects.SelectedItem.ToString & " to clipboard" Then
                    Clipboard.SetText(ProjectStatistics.ProjectStatistics(tsProjects_cmbProjects.SelectedItem.ToString).Report)
                Else
                    Try
                        Dim bFail As Boolean = True, iEnd As Int32 = 0
                        If rtProjects.Text.Contains("PRCG: " & vbTab & vbTab & "Project:" & tsProjects_cmbProjects.SelectedItem.ToString & Chr(32) & e.ClickedItem.Text.Replace("Copy data for ", "").Substring(0, e.ClickedItem.ToString.Replace("Copy data for ", "").IndexOf("::"))) Then
                            rtProjects.SelectionStart = rtProjects.Text.IndexOf("PRCG: " & vbTab & vbTab & "Project:" & tsProjects_cmbProjects.SelectedItem.ToString & Chr(32) & e.ClickedItem.Text.Replace("Copy data for ", "").Substring(0, e.ClickedItem.ToString.Replace("Copy data for ", "").IndexOf("::")))
                            rtProjects.ScrollToCaret()
                            iEnd = rtProjects.Text.IndexOf(New String(CChar(vbLf), 2), rtProjects.SelectionStart)
                            If Not iEnd = -1 Then
                                rtProjects.SelectionLength = iEnd - rtProjects.SelectionStart
                                Clipboard.SetText(rtProjects.SelectedText)
                                bFail = False
                            End If
                        End If
                        If bFail Then
                            WriteLog("Failed to place selected information on the clipboard", eSeverity.Critical)
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                End If
            ElseIf tcDetail.SelectedIndex = 3 Then
                If e.ClickedItem.Text = "Copy selected text to clipboard" Then
                    Clipboard.SetText(rtHW.SelectedText)
                ElseIf e.ClickedItem.Text = "Copy all text to clipboard" Then
                    Clipboard.SetText(rtHW.Text)
                Else
                    Try
                        Dim hwName As String = e.ClickedItem.Text.Replace("Copy data for ", "").Replace(" to clipboard", "")
                        Clipboard.SetText(clsStatistics.clsHardwareStatistics.mHardware(hwName).Report)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                End If
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Sub cmDetails_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles cmDetails.Opening
        Try
            cmDetails.Items.Clear()
            If tcDetail.SelectedIndex = 0 Then
                If rtWU.SelectedText <> "" Then
                    cmDetails.Items.Add("Copy selected text to clipboard")
                End If
                cmDetails.Items.Add("Open log")
                cmDetails.Items.Add("-")
                cmDetails.Items.Add("Copy details to clipboard")
                If Not IsNothing(m_SelectedWU) Then
                    cmDetails.Items.Add("-")
                    cmDetails.Items.Add("Copy wu report to clipboard")
                    cmDetails.Items.Add("Copy wu core snippet to clipboard")
                    cmDetails.Items.Add("Copy wu's client config to clipboard")
                    cmDetails.Items.Add("Copy wu's client info to clipboard")
                    cmDetails.Items.Add("-")
                    cmDetails.Items.Add("Open FoldingForum.org's Isseus with a specific WU section")
                End If
            ElseIf tcDetail.SelectedIndex = 1 Then
                If rtPerformance.SelectedText <> "" Then
                    cmDetails.Items.Add("Copy selected text to clipboard")
                End If
                cmDetails.Items.Add("Copy all text to clipboard")
                cmDetails.Items.Add("-")
                For Each Client As Client In Clients.Clients
                    cmDetails.Items.Add("Copy data for " & Client.ClientName & " to clipboard")
                Next
            ElseIf tcDetail.SelectedIndex = 2 Then
                If rtProjects.SelectedText <> "" Then
                    cmDetails.Items.Add("Copy selected text to clipboard")
                End If
                cmDetails.Items.Add("Copy all text to clipboard")
                cmDetails.Items.Add("-")
                cmDetails.Items.Add("Copy all text for project: " & tsProjects_cmbProjects.SelectedItem.ToString & " to clipboard")
                cmDetails.Items.Add("-")
                For Each strProject As String In tsProjects_cmbRCG.Items
                    If Not strProject = "-All-" Then
                        cmDetails.Items.Add("Copy data for " & strProject & " to clipboard")
                    End If
                Next
            ElseIf tcDetail.SelectedIndex = 3 Then
                If rtHW.SelectedText <> "" Then
                    cmDetails.Items.Add("Copy selected text to clipboard")
                End If
                cmDetails.Items.Add("Copy all text to clipboard")
                cmDetails.Items.Add("-")
                For Each hwName As String In clsStatistics.clsHardwareStatistics.mHardware.Keys
                    cmDetails.Items.Add("Copy data for " & hwName & " to clipboard")
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Update statistics"
    Private Delegate Sub UpdateStatisticsStripDelegate()
    Private Sub dUpdateStatisticsStrip()
        Try
            If bManual Then Exit Sub
            If OverallToolStripMenuItem.Checked Then
                'modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Overall
                tsStatistics.Text = "Total WUs: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_Completed & " Total credit: " & clsStatistics.clsHistoricalStatistics.Statistics.TotalCredit & " WU's Failed: " & clsStatistics.clsHistoricalStatistics.Statistics.Wu_EUE & " Succesrate: " & clsStatistics.clsHistoricalStatistics.Statistics.SuccesRate & " Total computation time: " & clsStatistics.clsHistoricalStatistics.Statistics.ComputationTime & " Avg PPD: " & clsStatistics.clsHistoricalStatistics.Statistics.AvgPPD
            Else
                'modMySettings.defaultStatistics = modMySettings.defaultStatisticsEnum.Current
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
#Region "delegatefactory event handlers"
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

    Private Sub tsHQFTimeLimit_Click(sender As System.Object, e As System.EventArgs) Handles tsHQFTimeLimit.Click

    End Sub

   
   
End Class