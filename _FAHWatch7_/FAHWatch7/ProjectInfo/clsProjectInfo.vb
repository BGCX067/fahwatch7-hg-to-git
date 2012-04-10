'   FAHWatch7   
'
'   Copyright (c) 2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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

Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Text
Imports System.Globalization
Friend Class ProjectInfo
#Region "Async downloader"
    Friend Class AsyncDownloader
        Friend Function GetText(Optional url As String = "") As String
            Dim allText As String = vbNullString
            If url = "" Then url = modMySettings.DefaultSummary
            Try
                Dim request As HttpWebRequest = CType(HttpWebRequest.Create(url), HttpWebRequest)
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    Dim rStream As IO.Stream = response.GetResponseStream
                    Dim rReader As StreamReader = New StreamReader(rStream)
                    allText = rReader.ReadToEnd
                End If
            Catch exW As System.Net.WebException
                WriteLog("WebException while trying to access the default psummary url ( " & url & " )", eSeverity.Critical)
                If Not IsNothing(CType(exW.Response, HttpWebResponse)) Then
                    WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusCode, eSeverity.Important)
                    WriteLog(" - " & CType(exW.Response, HttpWebResponse).StatusDescription, eSeverity.Important)
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return allText
        End Function
    End Class
    Friend Delegate Function AsyncDownload(url As String) As String
    'GetIsDone to be used to notify completion
    Friend Shared Event DownloadSucces(ByVal sender As Object, ByVal e As EventArgs)
    Friend Shared Event DownloadFailed(ByVal sender As Object, ByVal e As EventArgs)
#End Region
#Region "Projectinfo parsing declares"
    Private Const urlPsummary As String = "http://fah-web.stanford.edu/psummary.html"
    Private Const parseHeader As String = "<TD>Credit</TD><TD>Frames</TD><TD>Code</TD><TD>Description</TD>"
    Private Const parseEnd As String = "</TABLE>"
    Private Shared dPSummary As New Dictionary(Of String, pSummary)
    Private Shared dtLastAttempt As DateTime = #1/1/2000#
#End Region
#Region "ProjectInfo properties and functions"
    Friend Shared ReadOnly Property LastAttempt As DateTime
        Get
            Return dtLastAttempt
        End Get
    End Property
    Friend Shared ReadOnly Property IsEmpty As Boolean
        Get
            Return dPSummary.Count = 0
        End Get
    End Property
    Friend Shared ReadOnly Property KnownProject(ByVal ProjectNumber As String) As Boolean
        Get
            Return dPSummary.ContainsKey(ProjectNumber)
        End Get
    End Property
    Friend ReadOnly Property ProjectCount() As Integer
        Get
            Return dPSummary.Count
        End Get
    End Property
    Friend Overloads Shared ReadOnly Property Project(ByVal ProjectNumber As Integer) As pSummary
        Get
            If dPSummary.ContainsKey(CStr(ProjectNumber)) Then
                Return dPSummary(CStr(ProjectNumber))
            Else
                Return New pSummary
            End If
        End Get
    End Property
    Friend Overloads Shared ReadOnly Property Project(ByVal ProjectNumber As String) As pSummary
        Get
            If dPSummary.ContainsKey(ProjectNumber) Then
                Return dPSummary(ProjectNumber)
            Else
                Return New pSummary
            End If
        End Get
    End Property
    Friend Shared ReadOnly Property pSummaryList As List(Of pSummary)
        Get
            Dim dTmp As New SortedDictionary(Of Integer, pSummary)
            For Each DictionaryEntry In dPSummary
                dTmp.Add(CInt(DictionaryEntry.Key), DictionaryEntry.Value)
            Next
            Return dTmp.Values.ToList
        End Get
    End Property
    Friend Shared Sub Clear()
        Try
            ' Cleared to prevent wrong parsed info, but haven't seen it happen and 
            dPSummary.Clear()
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Function AddProject(ByVal Project As pSummary) As Boolean
        Try
            If KnownProject(Project.ProjectNumber) Then
                Return False
            Else
                dPSummary.Add(Project.ProjectNumber, Project)
                Return True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Shared Function RemoveProject(ByVal ProjectNumber As String) As Boolean
        Try
            If KnownProject(ProjectNumber) Then
                dPSummary.Remove(ProjectNumber)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend Function UpdateProject(ByVal uProject As pSummary) As Boolean
        Try
            If KnownProject(uProject.ProjectNumber) Then
                RemoveProject(uProject.ProjectNumber)
                Return AddProject(uProject)
            Else
                Return False
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Project download and parsing"
    Friend Shared Function GetProjects(Optional ByVal Url As String = Nothing, Optional ByVal ShowUI As Boolean = False) As Boolean
        If Not My.Computer.Network.IsAvailable Then Return False
        If IsNothing(Url) Then Url = modMySettings.DefaultSummary
        Dim dStart As DateTime = DateTime.Now
        Dim Result As Boolean = True
        Try
            If ShowUI Then
                delegateFactory.BussyBox.ShowForm("Connecting to " & Url, True, Nothing, False)
            Else
                delegateFactory.SetMessage("Connecting to " & Url)
            End If
            WriteLog("Attempting to download new project information from " & Url)
            Dim aP As New AsyncDownloader
            Dim caller As New AsyncDownload(AddressOf aP.GetText)
            Dim aResult As IAsyncResult = caller.BeginInvoke(Url, Nothing, Nothing)
            While Not aResult.IsCompleted
                Application.DoEvents()
            End While
            Dim allText As String = caller.EndInvoke(aResult)
            If InStr(allText, "Currently Running Projects") = 0 Then
                WriteLog("Content not as expected, parsing aborted", eSeverity.Important)
                Result = False
                GoTo HandleResult
            Else
                allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
            End If
            If Not ParseProjects(allText, ShowUI) Then
                WriteLog("Content could not be parsed", eSeverity.Important)
                Result = False
                GoTo HandleResult
            Else
                WriteLog("New project info downloaded, updating database")
                sqdata.Update_ProjectInfo()
                dtLastAttempt = DateTime.Now
                Dim lUP As List(Of String) = sqdata.UnknownProjects
                If lUP.Count > 0 Then
                    WriteLog("Checking unknown project list")
                    Dim bTry As Boolean = False
                    For Each strProject As String In lUP
                        If KnownProject(strProject) Then
                            bTry = True
                            WriteLog("Removing P" & strProject & " from the unknown project list: " & sqdata.RemoveUnknownProject(strProject).ToString)
                        End If
                    Next
                    If bTry Then
                        WriteLog("Runnig crediting function: " & sqdata.RecreditAllWorkUnits.ToString)
                    End If
                End If
            End If
HandleResult:
            If Result = False Then
                RaiseEvent DownloadFailed(modMAIN.ProjectInfo, MyEventArgs.Empty)
                WriteLog("Failed to download project summary", eSeverity.Important)
            Else
                RaiseEvent DownloadSucces(modMAIN.ProjectInfo, MyEventArgs.Empty)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Result = False
        Finally
            If ShowUI And delegateFactory.BussyBox.IsFormVisible Then delegateFactory.BussyBox.CloseForm()
            delegateFactory.SetMessage("GetProjects took " & DateTime.Now.Subtract(dStart).TotalMilliseconds & "ms.")
        End Try
        Return Result
    End Function
    Friend Shared Function ParseProjects(ByVal allText As String, Optional ShowUI As Boolean = False) As Boolean
        Try
            Dim colKey As String ' every collection key is the projectnumber
            Dim strTmp As String ' needed because parsing in one time is sh1t
            If ShowUI Then
                delegateFactory.BussyBox.SetMessage("Parsing project summary information")
            Else
                delegateFactory.SetMessage("Parsing project summary information")
            End If
            Dim dtNow As DateTime = DateTime.Now
            Do
                'Cut to after <TD>
                Try
                    allText = Mid(allText, InStr(allText, "<td>") + 4)
                    colKey = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                    If Not KnownProject(colKey) Then
                        Dim nProject As New pSummary
                        nProject.ProjectNumber = colKey
                        'set blank
                        nProject.WUName = "" : nProject.ServerIP = "" : nProject.PreferredDays = "" : nProject.NumberOfAtoms = "" : nProject.Frames = "" : nProject.FinalDeadline = "" : nProject.Code = "" : nProject.Contact = "" : nProject.Credit = "" : nProject.Description = "" : nProject.kFactor = "" : nProject.dtSummary = #1/1/2000#
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.ServerIP = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.WUName = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.NumberOfAtoms = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.PreferredDays = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.FinalDeadline = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Credit = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Frames = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Code = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'Looks like "<a href=http://fah-web.stanford.edu/cgi-bin/fahproject?p=772>Description</a>"
                        If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then nProject.Description = Mid(strTmp, InStr(strTmp, "http"), InStr(strTmp, ">Des") - 11)
                        'Cut to after <td>
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        'looks like "<font size=-1>vvishal</font>"
                        strTmp = Mid(strTmp, InStr(strTmp, ">") + 1)
                        nProject.Contact = Mid(strTmp, 1, Len(strTmp) - 7)
                        allText = Mid(allText, InStr(allText, "<td>") + 4)
                        'looks like asson</font></td><td>26.40</td></tr>
                        nProject.kFactor = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        nProject.dtSummary = dtNow
                        AddProject(nProject)
                    Else
                        'cut alltext and override project type, parse regular, comprehensive then beta 
                        allText = Mid(allText, InStr(allText, "</tr>") + 3)
                    End If
                Catch ex As Exception
                    If allText.IndexOf("</tr>") <> -1 Then
                        allText = Mid(allText, allText.IndexOf("</tr>") + 5)
                    Else
                        Exit Do
                    End If
                End Try
                If allText = "" Then Exit Do
                If allText.IndexOf("<td>") = -1 Then Exit Do
                'If ShowUI Then Application.DoEvents() ' Allow marquee animation to be drawn 
            Loop
            If ShowUI Then
                delegateFactory.BussyBox.SetMessage("Finished")
            Else
                delegateFactory.SetMessage("Finished parsing project summary information")
            End If
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Effective PPD"
    Friend Structure sProjectPPD
        Friend Credit As String
        Friend PPD As String
    End Structure
    Friend Shared Function GetEffectivePPD_sqrt(ByVal BeginTime As DateTime, ByVal EndTime As DateTime, ByVal ProjectNumber As String) As sProjectPPD
        Dim rV As New sProjectPPD
        Try
            WriteDebug("GetPPD - " & ProjectNumber & " - begin: " & BeginTime.ToString("s") & " - end: " & EndTime.ToString("s"))
            If EndTime = #1/1/2000# Then
                WriteDebug("End time not set, returning 0 credit and 0 ppd while no db comparison method has been implemented")
                Return rV
            End If
            Dim iKfactor As Double = Double.Parse(Project(ProjectNumber).kFactor, CultureInfo.InvariantCulture)
            Dim iPworth As Double = Double.Parse(Project(ProjectNumber).Credit, CultureInfo.InvariantCulture)
            'If CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator <> "." Then
            '    'iKfactor = CDbl(Projects.Project(Project).kFactor.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            '    'Dim iPworth As Double = CDbl(Projects.Project(Project).Credit.Replace(".", ","))
            '    iPworth = CInt(Projects.Project(Project).Credit.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            'Else
            '    iKfactor = CDbl(Projects.Project(Project).kFactor)
            '    'Dim iPworth As Double = CDbl(Projects.Project(Project).Credit.Replace(".", ","))
            '    iPworth = CInt(Projects.Project(Project).Credit)
            'End If
            Dim pCompletiontime As TimeSpan = EndTime.Subtract(BeginTime)
            If pCompletiontime.TotalSeconds = 0 Then
                rV.PPD = "0" : rV.Credit = "0"
                Return rV
            End If
            If iKfactor > 0 Then
                ''check if eta is before preferred
                'If EndTime < BeginTime.AddDays(Projects.Project(Project).PreferredDays.Replace(".", ",")) Then
                '    Dim bMulti As Double = Math.Sqrt((Projects.Project(Project).FinalDeadline * iKfactor) / pCompletiontime.TotalDays)
                '    iPworth = Math.Round(iPworth * bMulti)
                'End If
                'check if eta is before preferred
                If EndTime < BeginTime.AddDays(Double.Parse(Project(ProjectNumber).PreferredDays, CultureInfo.InvariantCulture)) Then
                    Dim bMulti As Double = Math.Sqrt((Double.Parse(Project(ProjectNumber).FinalDeadline, CultureInfo.InvariantCulture) * iKfactor) / pCompletiontime.TotalDays)
                    iPworth = Math.Round(iPworth * bMulti)
                    'iPworth = CInt(Math.Round(iPworth * bMulti))
                End If
            End If
            rV.Credit = FormatPPD(iPworth.ToString(CultureInfo.InvariantCulture))

            'How many frames per 24/h
            Dim iPPD As Double = 0
            Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
            Do
                Try
                    If tsDay.Subtract(pCompletiontime).TotalSeconds >= 0 Then
                        iPPD += iPworth
                        tsDay = tsDay.Subtract(pCompletiontime)
                    Else
                        Exit Do
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    rV.PPD = "-1"
                    Return rV
                End Try
            Loop
            'get fraction of _tsFrame to be done in remaining seconds
            Dim iRfraction As Double
            If tsDay.TotalSeconds > 0 Then
                iRfraction = tsDay.TotalSeconds / pCompletiontime.TotalSeconds
                iPPD += iRfraction * iPworth
            End If
            rV.PPD = FormatPPD(CStr(Math.Round(iPPD, 2)))
            Return rV
        Catch ex As Exception
            WriteError(ex.Message, Err)
            rV.PPD = "-1"
            Return rV
        End Try
    End Function
#End Region
#Region "Form handling"
    Private Shared mFrmList As frmPBList
    Private Shared mFrmGraph As ppdGrapher
    Friend Shared Sub ShowList(Optional parent As Form = Nothing)
        Try
            If IsNothing(mFrmList) OrElse mFrmList.IsDisposed OrElse mFrmList.Disposing Then
                mFrmList = New frmPBList
            End If
            mFrmList.FillView()
            If IsNothing(parent) Then
                mFrmList.ShowDialog()
            Else
                mFrmList.ShowDialog(parent)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            mFrmList.Dispose()
        End Try
    End Sub
    Friend Shared Sub ShowGraph(Optional parent As Form = Nothing)
        Try
            If IsNothing(mFrmGraph) OrElse mFrmGraph.IsDisposed OrElse mFrmGraph.Disposing Then
                mFrmGraph = New ppdGrapher
            End If
            If IsNothing(parent) Then
                mFrmGraph.ShowDialog()
            Else
                mFrmGraph.ShowDialog(parent)
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            mFrmGraph.Dispose()
        End Try
    End Sub
#End Region
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


