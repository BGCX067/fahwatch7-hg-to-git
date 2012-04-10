'   cftUnity Project info class
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
Imports System.Xml
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports System.Windows.Forms

Public Class clsProjectInfo
    'GetIsDone to be used to notify completion
    Public Event GetIsDone()
#Region "Entry point"
    Public Sub New()
        ' TODO Fix storage
     
    End Sub
#End Region
#Region "Declares"
    Private urlPsummary As String = "http://fah-web.stanford.edu/psummary.html"
    Private fileDATA As String = ""
    Private fileDATABackup As String = ""
    'Parsing constants
    Private Const parseHeader As String = "<TD>Credit</TD><TD>Frames</TD><TD>Code</TD><TD>Description</TD>"
    Private Const parseEnd As String = "</TABLE>"
    Public Projects As New sProject
#End Region
#Region "Project info"
    'Structure declare
    <Serializable()>
    Public Class sProject
        Public Enum eProjectType
            Beta
            Advanced
            Regular
        End Enum
        Private Projects As New Collection
        Public cProject As clsProject
        <Serializable()>
        Public Class clsProject
            Public ProjectNumber As String, ServerIP As String, WUName As String, NumberOfAtoms As String, PreferredDays As String, FinalDeadline As String
            Public Credit As String, Frames As String, Code As String, Description As String, Contact As String, kFactor As String, ProjectType As eProjectType
        End Class
#Region "Properties"
        Public ReadOnly Property IsEmpty() As Boolean
            Get
                Try
                    If Projects.Count = 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Catch ex As Exception
                    Projects = New Collection
                    Return 0
                End Try
            End Get
        End Property
        'Used to enumerate collection
        Public ReadOnly Property ProjectNumber(ByVal Ind As Int16) As String
            Get
                Try
                    If Ind > Projects.Count Then Return vbNullString
                    Return CType(Projects(Ind), clsProject).ProjectNumber
                Catch ex As Exception
                    Return vbNullString
                End Try
            End Get
        End Property
        Public ReadOnly Property KnownProject(ByVal ProjectNumber As String) As Boolean
            Get
                Try
                    Return Projects.Contains(ProjectNumber)
                Catch ex As Exception
                    Return False
                End Try
            End Get
        End Property
        Public ReadOnly Property ProjectCount() As Integer
            Get
                Try
                    Return Projects.Count
                Catch ex As Exception
                    Projects = New Collection
                    Return 0
                End Try
            End Get
        End Property
        Public Overloads ReadOnly Property Project(ProjectNumber As String) As clsProject
            Get
                If Projects.Contains(ProjectNumber) Then
                    Return Projects(ProjectNumber)
                Else
                    Return New clsProject
                End If
            End Get
        End Property
        Public Overloads ReadOnly Property Project(ByVal ProjectNumber As Int16) As clsProject
            Get
                Try
                    Return Projects(ProjectNumber)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property
#End Region
#Region "Sub and functions"
        Public Sub Clear()
            Try
                ' Cleared to prevent wrong parsed info, but haven't seen it happen and 
                Projects.Clear()
            Catch ex As Exception

            End Try
        End Sub
        Public Function AddProject(ByVal Project As clsProjectInfo.sProject.clsProject) As Boolean
            Try
                If Projects.Contains(Project.ProjectNumber) Then
                    Dim cProject As clsProject = Projects(Project.ProjectNumber)
                    If cProject.Equals(Project) Then
                        ' Store the changes units go through to allow proper historic point tracking at some time. so allow 
                        Return False
                    End If
                Else
                    Projects.Add(Project, Project.ProjectNumber)
                    Return True
                End If
            Catch ex As Exception

                Return False
            End Try
        End Function
        Public Function RemoveProject(ByVal ProjectNumber As String) As Boolean
            Try
                If Projects.Contains(ProjectNumber) Then
                    Projects.Remove(ProjectNumber)
                    ' TODO Fix storage
                    ' ProjectInfo.DoSerialize()
                    ' ProjectInfo.DeSerialize()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception

                Return False
            End Try
        End Function
        Public Function UpdateProject(ByVal uProject As clsProject) As Boolean
            Try
                If KnownProject(uProject.ProjectNumber) Then
                    RemoveProject(uProject.ProjectNumber)
                    Return AddProject(uProject)
                Else
                    Return False
                End If
            Catch ex As Exception

                Return False
            End Try
        End Function
#End Region
    End Class
#End Region
#Region "Project download and parsing"
    Public Function GetProjectdescription(ProjectNumber As String) As Boolean  ' Mimic FAHControl and download descriptions of active projects only
        Try
            If Projects.KnownProject(ProjectNumber) Then
                If Projects.Project(ProjectNumber).Description.ToUpper.Contains("HTTP://") Then

                End If
            End If
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function GetProjects(Optional ByVal Url As String = "", Optional ByVal ShowUI As Boolean = False) As Boolean
        Dim dStart As DateTime = DateTime.Now
        Try
            'check for network
            Dim allText As String = vbNullString
            If ShowUI Then
                Dim fPB As New frmPBStatus
                Dim Result As Boolean = True
                AddHandler fPB.Log, AddressOf LogWindow_Log
                AddHandler fPB.LogError, AddressOf LogWindow_LogError
                With fPB
                    .StartPosition = FormStartPosition.CenterScreen
                    .SetMessage("Getting project info list")
                    Application.DoEvents()
                    .ShowInTaskbar = False
                    .TopMost = True
                    .SetPBMax(100)
                    .Show()
                    If Url = "" Then
                        'Do extensive looping
                        .SetMessage("Parsing list of regular work units")
                        'Regular
                        Url = "http://fah-web.stanford.edu/psummary.html"
                        Dim uri As Uri = New Uri(Url)
                        Dim wb As WebBrowser = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "Currently Running Projects") = 0 Then
                            Result = False
                            GoTo HandleResult
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Regular, allText, fPB) Then
                            Result = False
                            GoTo HandleResult
                        End If

                        'Advanced
                        .SetMessage("Parsing comprehensive list of work units")
                        Url = "http://fah-web.stanford.edu/psummaryC.html"
                        uri = New Uri(Url)
                        wb = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "All Currently Running Projects") = 0 Then
                            Result = False
                            GoTo HandleResult
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Advanced, allText, fPB) Then
                            Result = False
                            GoTo HandleResult
                        End If
                        'Beta
                        .SetMessage("Parsing beta work units")
                        Url = "http://fah-web.stanford.edu/psummaryB.html"
                        uri = New Uri(Url)
                        wb = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "Currently Running Beta Projects") = 0 Then
                            Result = False
                            GoTo HandleResult
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Beta, allText, fPB) Then
                            Result = False
                            GoTo HandleResult
                        End If
                    Else
                        .SetMessage("Opening url: " & Url)
                        Dim uri As Uri = New Uri(Url)
                        Dim wb As WebBrowser = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "<th>Kfactor</th>") = 0 Then
                            Result = False
                            GoTo HandleResult
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Regular, allText, fPB) Then
                            Result = False
                        End If
                    End If
HandleResult:
                    .Close()
                End With
                If Result = False Then
                    Projects.Clear()
                    Return False
                Else
                    RaiseEvent GetIsDone()
                    Return True
                End If
            Else
                Dim Result As Boolean = False
                Try
                    If Url = "" Then
                        'Do extensive looping
                        'Regular
                        Url = "http://fah-web.stanford.edu/psummary.html"
                        Dim uri As Uri = New Uri(Url)
                        Dim wb As WebBrowser = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "Currently Running Projects") = 0 Then
                            Result = False
                            GoTo HandleResult2
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Regular, allText) Then
                            Result = False
                            GoTo HandleResult2
                        End If
                        'Advanced
                        Url = "http://fah-web.stanford.edu/psummaryC.html"
                        uri = New Uri(Url)
                        wb = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "All Currently Running Projects") = 0 Then
                            Result = False
                            GoTo HandleResult2
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Advanced, allText) Then
                            Result = False
                            GoTo HandleResult2
                        End If
                        'Beta
                        Url = "http://fah-web.stanford.edu/psummaryB.html"
                        uri = New Uri(Url)
                        wb = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "Currently Running Beta Projects") = 0 Then
                            Result = False
                            GoTo HandleResult2
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Beta, allText) Then
                            Result = False
                            GoTo HandleResult2
                        End If
HandleResult2:
                        If Result = False Then
                            Projects.Clear()
                            Return False
                        Else
                            RaiseEvent GetIsDone()
                            Return True
                        End If
                    Else
                        Dim uri As Uri = New Uri(Url)
                        Dim wb As WebBrowser = New WebBrowser
                        wb.Navigate(uri)
                        wb.Update()
                        While wb.IsBusy Or wb.ReadyState <> WebBrowserReadyState.Complete
                            Application.DoEvents()
                        End While
                        allText = wb.DocumentText
                        If InStr(allText, "<th>Kfactor</th>") = 0 Then
                            Return False
                        Else
                            allText = Mid(allText, InStr(allText, "<th>Kfactor</th>") + Len("<th>Kfactor</th>"))
                        End If
                        If Not ParseProjects(sProject.eProjectType.Regular, allText) Then
                            Projects.Clear()
                            Return False
                        Else
                            RaiseEvent GetIsDone()
                            Return True
                        End If

                    End If


                Catch ex As WebException

                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception
            LogWindow.WriteError("GetProjects", Err)
            Return False
        Finally
            LogWindow.WriteLog("GetProjects took " & DateTime.Now.Subtract(dStart).TotalMilliseconds & "ms.")
        End Try
    End Function
    Public Function ParseProjects(ByVal Ptype As clsProjectInfo.sProject.eProjectType, ByVal allText As String, Optional ByVal UIform As frmPBStatus = Nothing) As Boolean
        Try
            Dim UseUI As Boolean = False
            Dim Occurrences As Integer
            Dim iLoop As Int16 = 0
            Try
                If Not IsNothing(UIform) And UIform.Visible Then UseUI = True
                Dim StringToFind As String = "<td>"
                Dim exp As New Regex(StringToFind, RegexOptions.IgnoreCase)
                Occurrences = exp.Matches(allText).Count / 12
                UIform.SetPBMax(Occurrences + 1)
                Application.DoEvents()
            Catch ex As Exception

            End Try
            Dim colKey As String ' every collection key is the projectnumber
            Dim strTmp As String ' needed because parsing in one time is sh1t

            Do
                'Cut to after <TD>
                Try
                    If UseUI Then
                        iLoop += 1
                        UIform.SetMessage("Starting parse " & iLoop & " of " & Occurrences)
                        Application.DoEvents()
                        UIform.pBar.Value = iLoop
                    End If
                    allText = Mid(allText, InStr(allText, "<td>") + 4)
                    colKey = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                    If Not Projects.KnownProject(colKey) Then
                        Dim nProject As New sProject.clsProject
                        nProject.ProjectNumber = colKey
                        nProject.ProjectType = Ptype
                        'set blank
                        nProject.WUName = "" : nProject.ServerIP = "" : nProject.PreferredDays = "" : nProject.NumberOfAtoms = "" : nProject.Frames = "" : nProject.FinalDeadline = "" : nProject.Code = "" : nProject.Contact = "" : nProject.Credit = "" : nProject.Description = "" : nProject.kFactor = ""
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
                        Projects.AddProject(nProject)
                    Else
                        'cut alltext and override project type, parse regular, comprehensive then beta 
                        Select Case Projects.Project(colKey).ProjectType
                            Case Is = sProject.eProjectType.Regular
                                If Ptype = sProject.eProjectType.Advanced Then
                                    ' Project is both in regular and comprehensive, keep regular
                                ElseIf Ptype = sProject.eProjectType.Advanced Then
                                    ' Keep in advanced
                                ElseIf Ptype = sProject.eProjectType.Beta Then
                                    ' Project is in regular and in beta ( should not happen ), change to beta
                                    Projects.Project(colKey).ProjectType = sProject.eProjectType.Beta
                                End If
                            Case Is = sProject.eProjectType.Advanced
                                If Ptype = sProject.eProjectType.Regular Then
                                    ' Both in comphrehensive as in regular, make regular 
                                    Projects.Project(colKey).ProjectType = sProject.eProjectType.Regular
                                ElseIf Ptype = sProject.eProjectType.Advanced Then
                                    ' Keep in advanced
                                ElseIf Ptype = sProject.eProjectType.Beta Then
                                    ' Project is in comprehensive and in beta, change to beta
                                    Projects.Project(colKey).ProjectType = sProject.eProjectType.Beta
                                End If
                            Case Is = sProject.eProjectType.Beta
                                If Ptype = sProject.eProjectType.Regular Then
                                    ' Project is both in regular and beta, make beta
                                    ' ( Should never happen )
                                    Projects.Project(colKey).ProjectType = sProject.eProjectType.Beta
                                ElseIf Ptype = sProject.eProjectType.Advanced Then
                                    ' Project in comphrehensive as well as beta, make beta
                                    Projects.Project(colKey).ProjectType = sProject.eProjectType.Beta
                                ElseIf Ptype = sProject.eProjectType.Beta Then
                                    ' Keep in beta
                                End If
                        End Select
                        allText = Mid(allText, InStr(allText, "</tr>") + 3)
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    If allText.IndexOf("</tr>") <> -1 Then
                        allText = Mid(allText, allText.IndexOf("</tr>") + 5)
                    Else
                        Exit Do
                    End If
                    'hehe no doubles :D
                End Try
                If allText = "" Then Exit Do
                If allText.IndexOf("<td>") = -1 Then Exit Do
            Loop
            If UseUI Then
                UIform.SetMessage("Ended parse")
                Application.DoEvents()
            End If
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Effective PPD"
    Public Function GetEffectivePPD(ByVal BeginTime As DateTime, ByVal EndTime As DateTime, ByVal Project As String) As Double
        Try
            Dim iKfactor As Double = CDbl(Projects.Project(Project).kFactor.Replace(".", ","))
            Dim iPworth As Double = CDbl(Projects.Project(Project).Credit.Replace(".", ","))
            Dim pCompletiontime As TimeSpan = EndTime.Subtract(BeginTime)
            If iKfactor > 0 Then
                'check if eta is before preferred
                If EndTime < BeginTime.AddDays(Projects.Project(Project).PreferredDays.Replace(".", ",")) Then
                    Dim bMulti As Double = Math.Sqrt((Projects.Project(Project).PreferredDays.Replace(".", ",") * iKfactor) / pCompletiontime.TotalDays)
                    iPworth = Math.Round(iPworth * bMulti)
                End If
            End If
            'iPworth = iPworth / 100.0F
            'How many frames per 24/h
            Dim iPPD As Double = 0
            Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
            Do
                If tsDay.Subtract(pCompletiontime).TotalSeconds >= 0 Then
                    iPPD += iPworth
                    tsDay = tsDay.Subtract(pCompletiontime)
                Else
                    Exit Do
                End If
            Loop
            'get fraction of _tsFrame to be done in remaining seconds
            Dim iRfraction As Double
            If tsDay.TotalSeconds > 0 Then
                iRfraction = tsDay.TotalSeconds / pCompletiontime.TotalSeconds
                iPPD += iRfraction * iPworth
            End If
            Return (Math.Round(iPPD, 2))
        Catch ex As Exception
            Return 0
        End Try
    End Function
#End Region
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Function RemoveProject(ByVal ProjectNumber As String) As Boolean
        Try
            If Projects.KnownProject(ProjectNumber) Then
                Return Projects.RemoveProject(ProjectNumber)
            Else
                Return False
            End If
        Catch ex As Exception

            Return False
        End Try
    End Function
    Function UpdateProject(ByVal uProject As clsProjectInfo.sProject.clsProject) As Boolean
        Try
            If Projects.KnownProject(uProject.ProjectNumber) Then
                RemoveProject(uProject.ProjectNumber)
                Return AddProject(uProject)
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Function AddProject(ByVal nProject As sProject.clsProject) As Boolean
        Try
            If Projects.KnownProject(nProject.ProjectNumber) Then Return False
            Return Projects.AddProject(nProject)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Function Purge() As Boolean
        Try
            My.Computer.FileSystem.CopyFile(fileDATA, fileDATABackup)
            My.Computer.FileSystem.DeleteFile(fileDATA, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            Projects.Clear()
            If GetProjects() Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

            Return False
        End Try
    End Function
    Public ReadOnly Property Project(ByVal ProjectNumber As String) As sProject.clsProject
        Get
            Try
                If Projects.KnownProject(ProjectNumber) Then
                    Return Projects.Project(ProjectNumber)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                LogWindow.WriteError("clsProjectInfo_property project", Err)
                Return Nothing
            End Try
        End Get
    End Property
#Region "Log Extension"
    Public Class cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public WithEvents LogWindow As New cLW
    Public Event Log(ByVal Message As String)
    Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
    Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub
#End Region
   
End Class
