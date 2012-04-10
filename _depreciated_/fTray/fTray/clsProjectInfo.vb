'   fTray Project info class
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

Public Class clsProjectInfo
    'GetIsDone to be used to notify completion
    Public Event GetIsDone()
#Region "Entry point"
    Public Sub New(ByVal DataLocation As String)
        DataLocation = DataLocation.TrimEnd("\")
        fileDATA = DataLocation & "\Projects.dat"
        fileDATABackup = DataLocation & "\Projects.old"
        If Not My.Computer.FileSystem.FileExists(fileDATA) And My.Computer.FileSystem.FileExists(fileDATABackup) Then My.Computer.FileSystem.RenameFile(fileDATABackup, "Projects.dat")
        If My.Computer.FileSystem.FileExists(fileDATA) Then DeSerialize()
    End Sub
#End Region
#Region "Declares"
    Private urlPsummary As String = "http://fah-web.stanford.edu/psummary.html"
    Private fileDATA As String = ""
    Private fileDATABackup As String = ""
    'Parsing constants
    Private Const parseHeader As String = "<th>Code</th><th>Description</th><th>Contact</th><th>Kfactor</th>"
    Private Const parseEnd As String = "</TABLE>"
    Public Projects As New sProject
#End Region
#Region "Project info"
    'Structure declare
    <Serializable()>
    Public Class sProject
        Private Projects As New Collection
        Public cProject As clsProject
        <Serializable()>
        Public Class clsProject
            Public ProjectNumber As String, ServerIP As String, WUName As String, NumberOfAtoms As String, PreferredDays As String, FinalDeadline As String
            Public Credit As String, Frames As String, Code As String, Description As String, Contact As String, kFactor As String
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
        Public ReadOnly Property Project(ByVal ProjectNumber) As clsProject
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
                Projects.Clear()
            Catch ex As Exception

            End Try
        End Sub
        Public Function AddProject(ByVal Project As clsProjectInfo.sProject.clsProject) As Boolean
            Try
                If Projects.Contains(Project.ProjectNumber) Then
                    Return False
                Else
                    Projects.Add(Project, Project.ProjectNumber)
                    ProjectInfo.DoSerialize()
                    ProjectInfo.DeSerialize()
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
                    ProjectInfo.DoSerialize()
                    ProjectInfo.DeSerialize()
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
    Public Function GetProjects(Optional ByVal Url As String = "", Optional ByVal ShowUI As Boolean = False) As Boolean
        Dim dStart As DateTime = DateTime.Now
        Try
            'check for network
            Dim allText As String = vbNullString
            If ShowUI Then
                Dim fPB As New frmPBStatus
                With fPB
                    .StartPosition = FormStartPosition.CenterScreen
                    .SetMessage("Downloading projects")
                    Application.DoEvents()
                    .ShowInTaskbar = False
                    .TopMost = True
                    .SetPBMax(100)
                    .Show()
                    Try
                        Dim mC As WebClient = New WebClient
                        .SetPBValue(-2)
                        allText = mC.DownloadString(Url)
                    Catch ex As Exception
                        LogWindow.WriteError("DownloadString in GetProjects failed", Err)
                        .Close()
                        Return False
                    End Try
                    If InStr(allText, "Currently Running Projects") = 0 Then
                        LogWindow.WriteLog("Unexpected results from downloading project summary")
                        .Close()
                        Return False
                    Else
                        allText = Mid(allText, InStr(allText, parseHeader) + Len(parseHeader))
                        .SetMessage("Preparing parse")
                        Application.DoEvents()
                    End If
                    If Not ParseProjects(allText, fPB) Then
                        .Close()
                        Projects.Clear()
                        DeSerialize()
                        Return False
                    Else
                        .Close()
                        DoSerialize()
                        RaiseEvent GetIsDone()
                        Return True
                    End If
                End With
            Else
                Try
                    Dim mC As WebClient = New WebClient
                    allText = mC.DownloadString(Url)
                Catch ex As Exception
                    Return False
                End Try
                If InStr(allText, "All Currently Running Projects") = 0 Then
                    Return False
                Else
                    allText = Mid(allText, InStr(allText, parseHeader) + Len(parseHeader))
                End If
                If Not ParseProjects(allText) Then
                    Projects.Clear()
                    DeSerialize()
                    Return False
                End If
                DoSerialize()
                RaiseEvent GetIsDone()
                Return True
            End If
        Catch ex As Exception
            LogWindow.WriteError("GetProjects", Err)
            Return False
        Finally
            LogWindow.WriteLog("GetProjects took " & DateTime.Now.Subtract(dStart).TotalMilliseconds & "ms.")
        End Try
    End Function
    Private Function ParseProjects(ByVal allText As String, Optional ByVal UIform As frmPBStatus = Nothing) As Boolean
        Try
            Dim UseUI As Boolean = False
            Dim Occurrences As Integer
            Dim iLoop As Int16 = 0
            Try
                If UIform.Visible Then UseUI = True
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
                        With nProject
                            .ProjectNumber = colKey
                            'set blank
                            .WUName = "" : .ServerIP = "" : .PreferredDays = "" : .NumberOfAtoms = "" : .Frames = "" : .FinalDeadline = "" : .Code = "" : .Contact = "" : .Credit = "" : .Description = "" : .kFactor = ""
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .ServerIP = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .WUName = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .NumberOfAtoms = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .PreferredDays = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .FinalDeadline = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .Credit = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .Frames = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .Code = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'Looks like "<a href=http://fah-web.stanford.edu/cgi-bin/fahproject?p=772>Description</a>"
                            If Mid(allText, 1, InStr(allText, "</td>") - 1).Trim <> "" Then .Description = Mid(strTmp, InStr(strTmp, "http"), InStr(strTmp, ">Des") - 11)
                            'Cut to after <td>
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            strTmp = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                            'looks like "<font size=-1>vvishal</font>"
                            strTmp = Mid(strTmp, InStr(strTmp, ">") + 1)
                            .Contact = Mid(strTmp, 1, Len(strTmp) - 7)
                            allText = Mid(allText, InStr(allText, "<td>") + 4)
                            'looks like asson</font></td><td>26.40</td></tr>
                            .kFactor = Mid(allText, 1, InStr(allText, "</td>") - 1).Trim
                        End With
                        Projects.AddProject(nProject)
                    Else
                        'cut alltext
                        For tentimes As Int16 = 1 To 11 '11 fields after project number
                            allText = Mid(allText, InStr(allText, "<TD>") + 4)
                        Next
                    End If
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    'hehe no doubles :D
                End Try
                If InStr(allText, "<td>") = 0 Then Exit Do
            Loop
            If UseUI Then
                UIform.SetMessage("Ended parse")
                Application.DoEvents()
            End If
            Return True
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return False
        End Try
    End Function
#End Region
#Region "DAT read/write"
    Private Function DoSerialize() As Boolean
        Try
            Dim Serializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileDATA, FileMode.Create, FileAccess.Write, FileShare.None)
            Serializer.Serialize(DataFile, Projects)
            DataFile.Close()
            Serializer = Nothing
            DataFile = Nothing
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function
    Private Function DeSerialize() As Boolean
        Try
            Dim Deserializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileDATA, FileMode.Open, FileAccess.Read, FileShare.None)
            Projects = CType(Deserializer.Deserialize(DataFile), sProject)
            DataFile.Close()
            Deserializer = Nothing
            DataFile = Nothing
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function
#End Region
#Region "Effective PPD"
    Public Function GetEffectivePPD(ByVal BeginTime As DateTime, ByVal EndTime As DateTime, ByVal Project As String) As Double
        Try
            Dim iKfactor As Double = CDbl(ProjectInfo.Projects.Project(Project).kFactor.Replace(".", ","))
            Dim iPworth As Double = CDbl(ProjectInfo.Projects.Project(Project).Credit.Replace(".", ","))
            Dim pCompletiontime As TimeSpan = EndTime.Subtract(BeginTime)
            If iKfactor > 0 Then
                'check if eta is before preferred
                If EndTime < BeginTime.AddDays(ProjectInfo.Projects.Project(Project).PreferredDays.Replace(".", ",")) Then
                    Dim bMulti As Double = Math.Sqrt((ProjectInfo.Projects.Project(Project).PreferredDays.Replace(".", ",") * iKfactor) / pCompletiontime.TotalDays)
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
        Throw New NotImplementedException
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
            My.Computer.FileSystem.CopyFile(fileDATA, fileDataBackup)
            My.Computer.FileSystem.DeleteFile(fileDATA, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            Projects.Clear()
            If GetProjects() Then
                Return True
            Else
                If My.Computer.FileSystem.FileExists(fileDATABackup) Then My.Computer.FileSystem.RenameFile(fileDATABackup, "Projects.dat")
                DeSerialize()
                Return False
            End If
        Catch ex As Exception

            Return False
        End Try
    End Function

End Class
