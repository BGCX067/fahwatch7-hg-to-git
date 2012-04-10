Imports System.Text
Imports System.IO
Imports System.Globalization
Imports log4net
Imports System.Net
Imports FAHWatch7.Data
Imports MySql.Data.MySqlClient
Imports FAHWatch7.Core.Definitions

Partial Public Class ProjectInfo
#Region "Declarations"
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(ProjectInfo))
    Public Structure struct_pSummary
        Public LastUpdated As DateTime
        Public Requested As DateTime
        Public pSummaries As List(Of pSummary)
        Public Count As Int32
    End Structure
    Private Shared dPSummary As New SortedDictionary(Of Integer, pSummary)
    Private Shared dPSummaryDepreciated As New SortedDictionary(Of Integer, SortedDictionary(Of DateTime, pSummary))
    Private Shared mLastParsed As DateTime = DateTime.MinValue
#End Region
#Region "Loading/saving"
    ''' <summary>
    ''' 'Clears the pSummary dicationary and fills it with the stored information
    ''' </summary>
    ''' <returns>Boolean indicating success</returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function LoadProjects() As Boolean
        Try
            ClearSummary()
            Dim lDPR As List(Of Dictionary(Of String, String)) = FAHWatch7.Data.dbProjectInfo.fw7_ReadProjectInfo
            SyncLock dPSummary
                For Each dpr As Dictionary(Of String, String) In lDPR
                    Dim np As New pSummary
                    np.ProjectNumber = dpr("ProjectNumber")
                    np.ServerIP = dpr("ServerIP")
                    np.WUName = dpr("WUName")
                    np.NumberOfAtoms = dpr("NumberOfAtoms")
                    np.PreferredDays = dpr("PreferredDays")
                    np.FinalDeadline = dpr("FinalDeadline")
                    np.Credit = dpr("Credit")
                    np.Frames = dpr("Frames")
                    np.Code = dpr("Code")
                    np.Description = dpr("Description")
                    np.Contact = dpr("Contact")
                    np.kFactor = dpr("kFactor")
                    np.dtSummary = CDate(dpr("dtSummary"))
                    If Not String.IsNullOrEmpty(dpr("dtUpdated")) Then
                        np.dtUpdated = CDate(dpr("dtUpdated"))
                    Else
                        np.dtUpdated = DateTime.MinValue
                    End If
                    AddProject(np)
                Next
            End SyncLock
            log.Infoformat("Loaded pSummary information")
            Return CBool(SummaryCount > 0)
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 'Clears the depreciated pSummary dictionary and fills it with the stored information
    ''' </summary>
    ''' <returns>Boolean indicating succes</returns>
    ''' <remarks></remarks>
    Public Shared Function LoadDepreciated() As Boolean
        Try
            SyncLock dPSummaryDepreciated
                ClearDepreciated()
                Dim dS As SortedDictionary(Of Integer, SortedDictionary(Of DateTime, Dictionary(Of String, String))) = FAHWatch7.Data.dbProjectInfo.fw7_ReadDepreciatedProjects
                For Each pInt As Integer In dS.Keys
                    dPSummaryDepreciated.Add(pInt, New SortedDictionary(Of DateTime, pSummary))
                    For Each sP As Dictionary(Of String, String) In dS(pInt).Values
                        Dim np As New pSummary
                        np.ProjectNumber = sP("ProjectNumber")
                        np.ServerIP = sP("ServerIP")
                        np.WUName = sP("WUName")
                        np.NumberOfAtoms = sP("NumberOfAtoms")
                        np.PreferredDays = sP("PreferredDays")
                        np.FinalDeadline = sP("FinalDeadline")
                        np.Credit = sP("Credit")
                        np.Frames = sP("Frames")
                        np.Code = sP("Code")
                        np.Description = sP("Description")
                        np.Contact = sP("Contact")
                        np.kFactor = sP("kFactor")
                        np.dtSummary = CDate(sP("dtSummary"))
                        np.dtUpdated = CDate(sP("dtUpdated"))
                        If Not dPSummaryDepreciated(pInt).ContainsKey(CDate(np.dtUpdated)) Then
                            dPSummaryDepreciated(pInt).Add(CDate(np.dtUpdated), np)
                        Else
                            log.Debug("Attempt to add a project summary to the depreciated collection with a duplicate identifier, add aborted")
                        End If
                    Next
                Next
            End SyncLock
            log.Infoformat("Loaded depreciated pSummary information")
            Return True
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function

#End Region
#Region "Exporting"
    Public Shared ReadOnly Property SummaryCount As Int32
        Get
            Return dPSummary.Values.Count
        End Get
    End Property
    Public Shared ReadOnly Property DepreciatedCount As Int32
        Get
            Dim iRet As Int32 = 0
            For Each iPnumber As Int32 In dPSummaryDepreciated.Keys
                iRet += dPSummaryDepreciated(iPnumber).Values.Count
            Next
            Return iRet
        End Get
    End Property
#Region "raw"
    ''' <summary>
    ''' 'Returns a list with pSummary objects
    ''' </summary>
    ''' <value></value>
    ''' <returns>list(of pSummary)</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Projects As List(Of pSummary)
        Get
            Return dPSummary.Values.ToList
        End Get
    End Property
    ''' <summary>
    ''' 'Return the pSummary collection as array of pSummary
    ''' </summary>
    ''' <returns>list of pSummary objects</returns>
    ''' <remarks></remarks>
    Public Shared Function SummaryToArray() As Array
        Return dPSummary.Values.ToArray
    End Function
    Public Shared ReadOnly Property SummaryProjects As List(Of String)
        Get
            Try
                Dim rVal As New List(Of String)
                For Each P As pSummary In dPSummary.Values
                    rVal.Add(P.ProjectNumber)
                Next
                Return rVal
            Catch ex As Exception
                log.Errorformat(ex.message)
                Return New List(Of String)
            End Try
        End Get
    End Property
    ''' <summary>
    ''' 'Returns a sorted dictionary with pSummary information, sorted on projectnumber ( as integer ), sorted dictionary of datetime of when it was replaced by updated information, pSummary object
    ''' </summary>
    ''' <value></value>
    ''' <returns>Depreciated pSummary information</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DepreciatedProjects As SortedDictionary(Of Int32, SortedDictionary(Of DateTime, pSummary))
        Get
            Return dPSummaryDepreciated
        End Get
    End Property
    ''' <summary>
    ''' 'Returns a flat list of depreciated summaries
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DepreciatedProjectsList As List(Of pSummary)
        Get
            Dim rVal As New List(Of pSummary)
            For Each iPnumber As Int32 In dPSummaryDepreciated.Keys
                For Each pS As pSummary In dPSummaryDepreciated(iPnumber).Values
                    rVal.Add(pS)
                Next
            Next
            Return rVal
        End Get
    End Property
#End Region
#Region "String dictionaries"
    ''' <summary>
    ''' 'Return the pSummary collection as a list of string dictionary's
    ''' </summary>
    ''' <returns>list of pSummary objects convert to string, string dictionary</returns>
    ''' <remarks></remarks>
    Public Shared Function SummaryToDictionary() As List(Of Dictionary(Of String, String))
        Dim rVal As New List(Of Dictionary(Of String, String))
        For Each pr As pSummary In dPSummary.Values
            rVal.Add(pr.toDictionary)
        Next
        Return rVal
    End Function
    ''' <summary>
    ''' 'Returns the depreciated pSummary collection as list of string dictionary's 
    ''' </summary>
    ''' <returns>list of pSummary object</returns>
    ''' <remarks></remarks>
    Public Shared Function DepreciatedToDictionary() As List(Of Dictionary(Of String, String))
        Dim rVal As New List(Of Dictionary(Of String, String))
        For Each pInt As Integer In dPSummaryDepreciated.Keys
            For Each pr As pSummary In dPSummaryDepreciated(pInt).Values
                rVal.Add(pr.toDictionary)
            Next
        Next
        Return rVal
    End Function
#End Region
#Region "Xml serialization"
    ''' <summary>
    ''' 'Returns a string containing the serialized pSummary collection in xml format
    ''' </summary>
    ''' <returns>xml serialized pSummary collection</returns>
    ''' <remarks></remarks>
    Public Shared Function XmlSerializedpSummary() As String
        Dim pExp As New struct_pSummary
        pExp.pSummaries = dPSummary.Values.ToList
        pExp.Count = pExp.pSummaries.Count
        pExp.LastUpdated = LastParse
        pExp.Requested = DateTime.UtcNow
        Dim xSerializer As New Xml.Serialization.XmlSerializer(pExp.GetType)
        Dim sWriter As New StringWriter
        xSerializer.Serialize(sWriter, pExp)
        Return sWriter.ToString
    End Function
    ''' <summary>
    ''' 'Returns a string containing the serialized depreciated pSummary collection in xml format
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function xmlSerializedDepreciated() As String
        Dim pExp As New struct_pSummary
        pExp.pSummaries = DepreciatedProjectsList
        pExp.Count = DepreciatedCount
        pExp.LastUpdated = LastParse
        pExp.Requested = DateTime.UtcNow
        Dim xSerializer As New Xml.Serialization.XmlSerializer(pExp.GetType)
        Dim sWriter As New StringWriter
        xSerializer.Serialize(sWriter, pExp)
        Return sWriter.ToString
    End Function
#End Region
#End Region

#Region "external properties"
    Public Shared Property LastParse As DateTime
        Get
            Return mLastParsed
        End Get
        Set(value As DateTime)
            mLastParsed = value
        End Set
    End Property
    ''' <summary>
    ''' 'Returns a boolean indicating the presence of a project with the given projectnumber in the current pSummary dicationary
    ''' </summary>
    ''' <param name="ProjectNumber">The project number to queury</param>
    ''' <value></value>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property KnownProject(ByVal ProjectNumber As String) As Boolean
        Get
            Return dPSummary.ContainsKey(CInt(ProjectNumber))
        End Get
    End Property
    ''' <summary>
    ''' 'Return a boolean indicting the presense of the projectnumber in the depreciated dictionary
    ''' </summary>
    ''' <param name="ProjectNumber">The project number to query</param>
    ''' <value></value>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property KnownDepreciatedProject(ProjectNumber As String) As Boolean
        Get
            Return dPSummaryDepreciated.ContainsKey(CInt(ProjectNumber))
        End Get
    End Property
    ''' <summary>
    ''' 'Return a pSummary object for the given project number
    ''' </summary>
    ''' <param name="ProjectNumber">Number of the project to return</param>
    ''' <value></value>
    ''' <returns>pSummary object, empty is project isn't present</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Project(ProjectNumber As String) As pSummary
        Get
            If dPSummary.ContainsKey(CInt(ProjectNumber)) Then
                Return dPSummary(CInt(ProjectNumber))
            Else
                Return New pSummary
            End If
        End Get
    End Property
    ''' <summary>
    ''' 'Return the collection of updates to one project sorted by date
    ''' </summary>
    ''' <param name="ProjectNumber">The project to return the updates for</param>
    ''' <value></value>
    ''' <returns>sorted dictionary of datetime, psummary</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DepreciatedProjects(ProjectNumber As String) As SortedDictionary(Of DateTime, pSummary)
        Get
            If dPSummaryDepreciated.ContainsKey(CInt(ProjectNumber)) Then
                Return dPSummaryDepreciated(CInt(ProjectNumber))
            Else
                Return New SortedDictionary(Of DateTime, pSummary)
            End If
        End Get
    End Property
#End Region
#Region "Add/remove"
    ''' <summary>
    ''' 'Add's a project to the dictionary and checks wether to replace an existing one, when supplying preserve the
    ''' old summary is backed up and the dtUpdated will be dbNull
    ''' </summary>
    ''' <param name="ProjectToAdd">The project to add or update</param>
    ''' <param name="Silent">To indicate if this should be a silent edit or not</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function AddProject(ByVal ProjectToAdd As pSummary, Optional Silent As Boolean = False) As Boolean
        Try
            log.Debug("Attempt to add project nr: " & ProjectToAdd.ProjectNumber)
            If KnownProject(ProjectToAdd.ProjectNumber) Then
                log.Debug("Project number is a duplicate, comparing parse result with previously stored project")
                If Not Project(ProjectToAdd.ProjectNumber).Equals1(ProjectToAdd) Then
                    If Silent Then
                        log.Debug("Project is not equal, Silent flag supplied. Changing project properties")
                        Project(ProjectToAdd.ProjectNumber).MakeEqual(ProjectToAdd)
                        Return True
                    Else
                        log.Debug("Project is different, updating current project listing and backing up the old project")
                        'Should use a dropped projects table...
                        dPSummary(CInt(ProjectToAdd.ProjectNumber)).dtUpdated = DateTime.UtcNow
                        If dPSummaryDepreciated.ContainsKey(CInt(ProjectToAdd.ProjectNumber)) Then
                            dPSummaryDepreciated(CInt(ProjectToAdd.ProjectNumber)).Add(dPSummary(CInt(ProjectToAdd.ProjectNumber)).dtUpdated, dPSummary(CInt(ProjectToAdd.ProjectNumber)))
                        Else
                            dPSummaryDepreciated.Add(CInt(ProjectToAdd.ProjectNumber), New SortedDictionary(Of DateTime, pSummary))
                            dPSummaryDepreciated(CInt(ProjectToAdd.ProjectNumber)).Add(dPSummary(CInt(ProjectToAdd.ProjectNumber)).dtUpdated, dPSummary(CInt(ProjectToAdd.ProjectNumber)))
                        End If
                        dPSummary.Remove(CInt(ProjectToAdd.ProjectNumber))
                        dPSummary.Add(CInt(ProjectToAdd.ProjectNumber), ProjectToAdd)
                        Return True
                    End If
                Else
                    log.Debug("The projects are identical, discarding")
                    Return False
                End If
            Else
                log.Debug("Project is new, adding to collection")
                dPSummary.Add(CInt(ProjectToAdd.ProjectNumber), ProjectToAdd)
                Return True
            End If
        Catch ex As Exception
            log.Errorformat(ex.message)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 'Deletes a project from the depreciated projects collection, for 'when if'
    ''' </summary>
    ''' <param name="Project">Project to remove</param>
    ''' <returns></returns>
    ''' <remarks>This is useless for now, since the actual database is not updated to reflect the collection it's only updated incrementally</remarks>
    Friend Shared Function DeleteDepreciatedProject(Project As pSummary) As Boolean
        If Not KnownDepreciatedProject(Project.ProjectNumber) Then
            Return False
        Else
            If Not dPSummaryDepreciated(CInt(Project.ProjectNumber)).ContainsValue(Project) Then
                Return False
            Else
                dPSummaryDepreciated(CInt(Project.ProjectNumber)).Remove(Project.dtUpdated)
                Return True
            End If
        End If
    End Function
    ''' <summary>
    ''' 'Deletes a project from the actual psummary collection if found, for 'when if'
    ''' </summary>
    ''' <param name="ProjectToRemove">The project to remove</param>
    ''' <returns>boolean</returns>
    ''' <remarks></remarks>
    Friend Shared Function DeleteProject(ProjectToRemove As pSummary) As Boolean
        If Not KnownProject(ProjectToRemove.ProjectNumber) Then
            Return False
        Else
            If Not Project(ProjectToRemove.ProjectNumber).Equals1(ProjectToRemove) Then
                log.WarnFormat("Attempt to remove a project with mismatched properties, aborted")
                Return False
            Else
                dPSummary.Remove(CInt(ProjectToRemove.ProjectNumber))
                Return True
            End If
        End If
    End Function
    ''' <summary>
    ''' 'Clears the pSummary dictionary
    ''' </summary>
    ''' <param name="ByWho">Object which calls for clearing, logged when passed</param>
    ''' <remarks></remarks>
    Public Shared Sub ClearSummary(Optional ByWho As Object = Nothing)
        SyncLock dPSummary
            If Not IsNothing(ByWho) Then
                log.Infoformat("Clearing the psummary dictionary, called by " & CStr(ByWho))
            Else
                log.Infoformat("Clearing the psummary dictionary, no caller id")
            End If
            dPSummary.Clear()
        End SyncLock
    End Sub
    ''' <summary>
    ''' Clears the depreciated pSummary dictionary
    ''' </summary>
    ''' <param name="ByWho">Object which calls for clearing, logged when passed</param>
    ''' <remarks></remarks>
    Public Shared Sub ClearDepreciated(Optional ByWho As Object = Nothing)
        SyncLock dPSummaryDepreciated
            If Not IsNothing(ByWho) Then
                log.Infoformat("Clearing the depreciated summary collection, called by " & CStr(ByWho))
            Else
                log.Infoformat("Clearing the depreciated summary collection, no caller id")
            End If
            dPSummaryDepreciated.Clear()
        End SyncLock
    End Sub
#End Region
#Region "Effective PPD"
    Public Structure ProjectPPD
        Friend Credit As String
        Friend PPD As String
        Friend NoBonusCredit As String
        Friend NoBonusPPD As String
    End Structure
    ''' <summary>
    ''' Returns the ppd/credit for a project based on the timespan provided, both with and without bonus
    ''' </summary>
    ''' <param name="ProjectTimeSpan">Timespan to use for calculation</param>
    ''' <param name="ProjectNumber">The project number used to reference against pSummary information</param>
    ''' <returns>ProjectPPD structure</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetEffectivePPD_sqrt(ProjectTimeSpan As TimeSpan, ProjectNumber As String) As ProjectPPD
        Try
            Dim rV As New ProjectPPD
            If ProjectTimeSpan.TotalMilliseconds < 0 Then
                log.Infoformat("The timespan provided is negative, returning an empty ProjectPPD")
                Return rV
            ElseIf ProjectTimeSpan = TimeSpan.Zero Then
                log.Infoformat("The timespan provided is zero, returning an empty ProjectPPD")
                Return rV
            End If
            Dim iKfactor As Double = Double.Parse(Project(ProjectNumber).kFactor, CultureInfo.InvariantCulture)
            Dim iPworth As Double = Double.Parse(Project(ProjectNumber).Credit, CultureInfo.InvariantCulture)
            rV.NoBonusCredit = FAHWatch7.Business.Formatting.FormatPPD(iPworth.ToString(CultureInfo.InvariantCulture))
            Dim iNoBonus As Double = iPworth
            Dim BeginTime As DateTime = #1/1/2000#
            Dim EndTime As DateTime = BeginTime.Add(ProjectTimeSpan)
            If iKfactor > 0 Then
                If EndTime < BeginTime.AddDays(Double.Parse(Project(ProjectNumber).PreferredDays, CultureInfo.InvariantCulture)) Then
                    Dim bMulti As Double = Math.Sqrt((Double.Parse(Project(ProjectNumber).FinalDeadline, CultureInfo.InvariantCulture) * iKfactor) / ProjectTimeSpan.TotalDays)
                    iPworth = Math.Round(iPworth * bMulti)
                End If
            End If
            rV.Credit = FAHWatch7.Business.Formatting.FormatPPD(iPworth.ToString(CultureInfo.InvariantCulture))
            'How many frames per 24/h
            Dim iPPD As Double = 0, iNoBonusPPD As Double = 0
            Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
            Do
                Try
                    If tsDay.Subtract(ProjectTimeSpan).TotalSeconds >= 0 Then
                        iPPD += iPworth
                        iNoBonusPPD += iNoBonus
                        tsDay = tsDay.Subtract(ProjectTimeSpan)
                    Else
                        Exit Do
                    End If
                Catch ex As Exception
                    log.Errorformat(ex.message) 'maybe disable the error loggin, replace with warning?
                    rV.PPD = "-1"
                    Return rV
                End Try
            Loop
            'get fraction of _tsFrame to be done in remaining seconds
            Dim iRfraction As Double
            If tsDay.TotalSeconds > 0 Then
                iRfraction = tsDay.TotalSeconds / ProjectTimeSpan.TotalSeconds
                iPPD += iRfraction * iPworth
                iNoBonusPPD += iRfraction * iPworth
            End If
            rV.PPD = FAHWatch7.Business.Formatting.FormatPPD(CStr(Math.Round(iPPD, 2)))
            rV.NoBonusPPD = FAHWatch7.Business.Formatting.FormatPPD(CStr(Math.Round(iNoBonusPPD, 2)))
            Return rV
        Catch ex As Exception
            log.Errorformat(ex.message)
            Try
                Dim rv As New ProjectPPD
                rv.PPD = "-1"
                Return rv
            Finally : End Try
        End Try
    End Function

    ''' <summary>
    ''' Returns the ppd/credit for a project based on end and begin time, both with and without bonus
    ''' </summary>
    ''' <param name="BeginTime">Begin time for the project, usually the time the project was assigned/start of the download</param>
    ''' <param name="EndTime">End time for the project, usually the time when the project was submitted</param>
    ''' <param name="ProjectNumber">The projectNumber for pSummary reference</param>
    ''' <returns>ProjectPPD structure</returns>
    ''' <remarks></remarks>
    Public Overloads Function GetEffectivePPD_sqrt(ByVal BeginTime As DateTime, ByVal EndTime As DateTime, ByVal ProjectNumber As String) As ProjectPPD
        Try
            Dim rV As New ProjectPPD
            If EndTime = #1/1/2000# Then
                log.Infoformat("End time not set, returning 0 credit and 0 ppd while no db comparison method has been implemented")
                Return rV
            ElseIf BeginTime < EndTime Then
                log.Infoformat("End time is before BeginTime, return empty ProjectPPD")
                Return rV
            End If
            Dim iKfactor As Double = Double.Parse(Project(ProjectNumber).kFactor, CultureInfo.InvariantCulture)
            Dim iPworth As Double = Double.Parse(Project(ProjectNumber).Credit, CultureInfo.InvariantCulture)
            Dim pCompletiontime As TimeSpan = EndTime.Subtract(BeginTime)
            If pCompletiontime.TotalSeconds = 0 Then
                rV.PPD = "0" : rV.Credit = "0"
                Return rV
            End If
            rV.NoBonusCredit = FAHWatch7.Business.Formatting.FormatPPD(iPworth.ToString(CultureInfo.InvariantCulture))
            Dim iNoBonus As Double = iPworth
            If iKfactor > 0 Then
                If EndTime < BeginTime.AddDays(Double.Parse(Project(ProjectNumber).PreferredDays, CultureInfo.InvariantCulture)) Then
                    Dim bMulti As Double = Math.Sqrt((Double.Parse(Project(ProjectNumber).FinalDeadline, CultureInfo.InvariantCulture) * iKfactor) / pCompletiontime.TotalDays)
                    iPworth = Math.Round(iPworth * bMulti)
                End If
            End If
            rV.Credit = FAHWatch7.Business.Formatting.FormatPPD(iPworth.ToString(CultureInfo.InvariantCulture))
            'How many frames per 24/h
            Dim iPPD As Double = 0, iNoBonusPPD As Double = 0
            Dim tsDay As TimeSpan = TimeSpan.FromDays(1)
            Do
                Try
                    If tsDay.Subtract(pCompletiontime).TotalSeconds >= 0 Then
                        iPPD += iPworth
                        iNoBonusPPD += iNoBonus
                        tsDay = tsDay.Subtract(pCompletiontime)
                    Else
                        Exit Do
                    End If
                Catch ex As Exception
                    log.Errorformat(ex.message) 'maybe disable the error loggin, replace with warning?
                    rV.PPD = "-1"
                    Return rV
                End Try
            Loop
            'get fraction of _tsFrame to be done in remaining seconds
            Dim iRfraction As Double
            If tsDay.TotalSeconds > 0 Then
                iRfraction = tsDay.TotalSeconds / pCompletiontime.TotalSeconds
                iPPD += iRfraction * iPworth
                iNoBonusPPD += iRfraction * iPworth
            End If
            rV.PPD = FAHWatch7.Business.Formatting.FormatPPD(CStr(Math.Round(iPPD, 2)))
            rV.NoBonusPPD = FAHWatch7.Business.Formatting.FormatPPD(CStr(Math.Round(iNoBonusPPD, 2)))
            Return rV
        Catch ex As Exception
            log.Errorformat(ex.message)
            Try
                Dim rv As New ProjectPPD
                rv.PPD = "-1"
                Return rv
            Finally : End Try
        End Try
    End Function
#End Region
End Class





