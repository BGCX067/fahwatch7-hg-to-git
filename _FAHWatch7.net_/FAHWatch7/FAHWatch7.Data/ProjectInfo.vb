Imports log4net
Imports System.Web
Imports System.Text
Imports System.Globalization
Imports System.Configuration
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports System.Data.SqlServerCe
Imports FAHWatch7.Core.Formatting
Imports FAHWatch7.Core.Definitions

Public Class dbProjectInfo
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(FAHWatch7.Data.dbProjectInfo))
    Private Shared mIsInitialized As Boolean = False
#Region "Exception reporting"
    Private Shared mHasError As Boolean = False, mException As Exception = Nothing
    Public Shared ReadOnly Property HasError As Boolean
        Get
            Return mHasError
        End Get
    End Property
    Public Shared ReadOnly Property LastException As Exception
        Get
            Return mException
        End Get
    End Property
    Public Shared Sub ClearError()
        mHasError = False
        If Not IsNothing(mException) Then
            mException = Nothing
        End If
    End Sub
#End Region
#Region "Initialize tables"
    Public Shared ReadOnly Property InitDB As Boolean
        Get
            Try
                Dim bChanges As Boolean = False, bProjects As Boolean = False
                Using mCon As New MySqlClient.MySqlConnection(ConnectionStrings.GetConnectionString)
                    mCon.Open()
                    If mCon.State <> ConnectionState.Open Then
                        log.Warn("Can't open MySQL connection using connectionstring: " & mCon.ConnectionString)
                        mException = New Exception("Failed to open MySQL connection")
                        Return False
                    End If
                    Using mCmd As MySqlCommand = New MySqlCommand("select * from fw7_ProjectInfo", mCon)
                        Try
                            Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                                bProjects = True
                            End Using
                        Catch ex As MySqlException
                            If ex.Number = 1146 Then
                                bProjects = False
                            Else
                                Throw New Exception("Exception while querying table", ex)
                            End If
                        End Try
                    End Using
                    Using mCmd As MySqlCommand = New MySqlCommand("select * from fw7_ProjectChanges", mCon)
                        Try
                            Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                                bChanges = True
                            End Using
                        Catch ex As MySqlException
                            If ex.Number = 1146 Then
                                bChanges = False
                            Else
                                Throw New Exception("Exception while querying table", ex)
                            End If
                        End Try
                    End Using
                    If Not bProjects Then
                        log.Infoformat("projectinfo table does not exist, creating now")
                        Try
                            Dim sql As String = "CREATE TABLE fw7_ProjectInfo (Project INT, ServerIP NVARCHAR(128), WUName NVARCHAR(100), NumberOfAtoms INT, PreferredDays NVARCHAR(6), FinalDeadline NVARCHAR(6), Credit NVARCHAR(10), Frames TINYINT, Code NVARCHAR(100), Description TEXT, kfactor VARCHAR(6), Contact TEXT, dtSummary DATETIME, dtUpdated DATETIME)"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.ExecuteNonQuery()
                            End Using
                            bProjects = True
                        Catch ex As Exception
                            Log.FatalFormat(ex.message)
                        End Try
                    End If
                    If Not bChanges Then
                        log.Infoformat("projectchanges table does not exist, creating now")
                        Try
                            Dim sql As String = "CREATE TABLE fw7_ProjectChanges (Project INT, ServerIP NVARCHAR(128), WUName NVARCHAR(100), NumberOfAtoms INT, PreferredDays NVARCHAR(6), FinalDeadline NVARCHAR(6), Credit NVARCHAR(10), Frames TINYINT, Code NVARCHAR(100), Description TEXT, kfactor VARCHAR(6), Contact TEXT, dtSummary DATETIME, dtUpdated DATETIME)"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.ExecuteNonQuery()
                            End Using
                            bChanges = True
                        Catch ex As Exception
                            Log.FatalFormat(ex.message)
                        End Try
                    End If
                End Using
                mIsInitialized = bChanges AndAlso bProjects
                Return mIsInitialized
            Catch ex As Exception
                Log.FatalFormat(ex.message)
                mHasError = True
                mException = ex
                Return False
            End Try
        End Get
    End Property
#End Region
#Region "ProjectInfo"
    ''' <summary>
    ''' 'Return a list of pSummary objects
    ''' </summary>
    ''' <returns>List of pSummary</returns>
    ''' <remarks></remarks>
    Public Shared Function fw7_ReadProjectInfo2() As List(Of pSummary)
        Try
            Dim rVal As New List(Of pSummary)
            If Not mIsInitialized Then
                If Not InitDB Then Return rVal
            End If
            Dim dtStart As DateTime = DateTime.UtcNow
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ProjectInfo", mCon)
                Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                    While mRDR.Read
                        Dim dNP As New Dictionary(Of String, String)
                        dNP.Add("ProjectNumber", CStr(mRDR.Item("Project")))
                        dNP.Add("ServerIP", CStr(mRDR.Item("ServerIP")))
                        dNP.Add("WUName", CStr(mRDR.Item("WUName")))
                        dNP.Add("NumberOfAtoms", CStr(mRDR.Item("NumberOfAtoms")))
                        dNP.Add("PreferredDays", CStr(mRDR.Item("PreferredDays")))
                        dNP.Add("FinalDeadline", CStr(mRDR.Item("FinalDeadline")))
                        dNP.Add("Credit", CStr(mRDR.Item("Credit")))
                        dNP.Add("Frames", CStr(mRDR.Item("Frames")))
                        dNP.Add("Code", CStr(mRDR.Item("Code")))
                        dNP.Add("Description", CStr(mRDR.Item("Description")))
                        dNP.Add("Contact", CStr(mRDR.Item("Contact")))
                        dNP.Add("kFactor", CStr(mRDR.Item("kFactor")))
                        dNP.Add("dtSummary", CStr(mRDR.Item("dtSummary")))
                        If IsDBNull(mRDR.Item("dtUpdated")) Then
                            dNP.Add("dtUpdated", String.Empty)
                        Else
                            dNP.Add("dtUpdated", CStr(mRDR.Item("dtUpdated")))
                        End If
                        Dim NP As New pSummary
                        NP.Populate(dNP)
                        rVal.Add(NP)
                    End While
                End Using
            End Using
            log.Infoformat("Reading projects took " & Format_ts(DateTime.UtcNow.Subtract(dtStart)))
            log.Infoformat("Known project count: " & rVal.Count.ToString)
            Return rVal
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return New List(Of pSummary)
        End Try
    End Function
    ''' <summary>
    ''' 'Return a list of pSummary objects in dictionary form
    ''' </summary>
    ''' <returns>List of pSummary in dictionary form</returns>
    ''' <remarks></remarks>
    Public Shared Function fw7_ReadProjectInfo() As List(Of Dictionary(Of String, String))
        Try
            Dim rVal As New List(Of Dictionary(Of String, String))
            If Not mIsInitialized Then
                If Not InitDB Then Return rVal
            End If
            Dim dtStart As DateTime = DateTime.UtcNow
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ProjectInfo", mCon)
                Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                    While mRDR.Read
                        Dim dNP As New Dictionary(Of String, String)
                        dNP.Add("ProjectNumber", CStr(mRDR.Item("Project")))
                        dNP.Add("ServerIP", CStr(mRDR.Item("ServerIP")))
                        dNP.Add("WUName", CStr(mRDR.Item("WUName")))
                        dNP.Add("NumberOfAtoms", CStr(mRDR.Item("NumberOfAtoms")))
                        dNP.Add("PreferredDays", CStr(mRDR.Item("PreferredDays")))
                        dNP.Add("FinalDeadline", CStr(mRDR.Item("FinalDeadline")))
                        dNP.Add("Credit", CStr(mRDR.Item("Credit")))
                        dNP.Add("Frames", CStr(mRDR.Item("Frames")))
                        dNP.Add("Code", CStr(mRDR.Item("Code")))
                        dNP.Add("Description", CStr(mRDR.Item("Description")))
                        dNP.Add("Contact", CStr(mRDR.Item("Contact")))
                        dNP.Add("kFactor", CStr(mRDR.Item("kFactor")))
                        dNP.Add("dtSummary", CStr(mRDR.Item("dtSummary")))
                        If IsDBNull(mRDR.Item("dtUpdated")) Then
                            dNP.Add("dtUpdated", String.Empty)
                        Else
                            dNP.Add("dtUpdated", CStr(mRDR.Item("dtUpdated")))
                        End If
                        rVal.Add(dNP)
                    End While
                End Using
            End Using
            log.Infoformat("Reading projects took " & Format_ts(DateTime.UtcNow.Subtract(dtStart)))
            log.Infoformat("Known project count: " & rVal.Count.ToString)
            Return rVal
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return New List(Of Dictionary(Of String, String))
        End Try
    End Function
    Public Shared Function fw7_UpdateProjectInfo2(pInfo As List(Of pSummary)) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB Then Return False
            End If
            Dim dtStart As DateTime = DateTime.Now
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                For Each Project As pSummary In pInfo
                    Dim bUpdate As Boolean = False, bInsert As Boolean = False

                    Dim sql As String = "SELECT PreferredDays, FinalDeadline, Credit, kFactor, Description FROM fw7_ProjectInfo WHERE Project = '" & CInt(Project.ProjectNumber) & "'"
                    Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                        Using mRDR As MySqlDataReader = mCmd.ExecuteReader
                            If mRDR.Read Then
                                bInsert = False
                                If Not Project.PreferredDays = CStr(mRDR.Item("PreferredDays")) OrElse Not Project.FinalDeadline = CStr(mRDR.Item("FinalDeadline")) OrElse Not Project.Credit = CStr(mRDR.Item("Credit")) OrElse Not Project.kFactor = CStr(mRDR.Item("kFactor")) OrElse Not Project.Description = CStr(mRDR.Item("Description")) Then
                                    bUpdate = True
                                End If
                            Else
                                bInsert = True
                            End If
                        End Using
                    End Using
                    If bInsert Or bUpdate Then
                        If bInsert Then
                            sql = "INSERT INTO fw7_ProjectInfo (Project, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary, dtUpdated) values(@projectnumber,@serverip, @wuname, @numberofatoms, @preferreddays, @finaldeadline, @credit, @frames, @code, @description, @contact, @kfactor, @dtsummary, @dtupdated)"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.Parameters.AddWithValue("@projectnumber", CInt(Project.ProjectNumber))
                                mCmd.Parameters.AddWithValue("@serverip", Project.ServerIP)
                                mCmd.Parameters.AddWithValue("@wuname", Project.WUName)
                                mCmd.Parameters.AddWithValue("@numberofatoms", CInt(Project.NumberOfAtoms))
                                mCmd.Parameters.AddWithValue("@preferreddays", Project.PreferredDays)
                                mCmd.Parameters.AddWithValue("@finaldeadline", Project.FinalDeadline)
                                mCmd.Parameters.AddWithValue("@credit", CInt(Project.Credit))
                                mCmd.Parameters.AddWithValue("@frames", CInt(Project.Frames))
                                mCmd.Parameters.AddWithValue("@code", Project.Code)
                                mCmd.Parameters.AddWithValue("@description", Project.Description)
                                mCmd.Parameters.AddWithValue("@contact", Project.Contact)
                                mCmd.Parameters.AddWithValue("@kfactor", Decimal.Parse(Project.kFactor))
                                If IsNothing(Project.dtSummary) OrElse Project.dtSummary = DateTime.MinValue Then
                                    mCmd.Parameters.AddWithValue("@dtsummary", DBNull.Value)
                                Else
                                    mCmd.Parameters.AddWithValue("@dtsummary", Project.dtSummary)
                                End If
                                If IsNothing(Project.dtUpdated) OrElse Project.dtUpdated = DateTime.MinValue Then
                                    mCmd.Parameters.AddWithValue("@dtupdated", DBNull.Value)
                                Else
                                    mCmd.Parameters.AddWithValue("@dtupdated", Project.dtUpdated)
                                End If
                                mCmd.ExecuteNonQuery()
                            End Using
                        Else
                            sql = "UPDATE fw7_ProjectInfo SET PreferredDays = '" & Project.PreferredDays & "', FinalDeadline = '" & Project.FinalDeadline & "', Credit = '" & CInt(Project.Credit) & "', kFactor = '" & Decimal.Parse(Project.kFactor) & "', dtSummary = '" & Project.dtSummary & "', dtUpdated = '" & Project.dtUpdated & "' WHERE Project = '" & CInt(Project.ProjectNumber) & "'"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.ExecuteNonQuery()
                            End Using
                        End If
                    End If
                Next
            End Using
            log.Infoformat("Updating pSummary database took: " & Format_ts(DateTime.Now.Subtract(dtStart)))
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared Function fw7_UpdateProjectInfo(pInfo As List(Of Dictionary(Of String, String))) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB Then Return False
            End If
            Dim dtStart As DateTime = DateTime.Now
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                For Each Project As Dictionary(Of String, String) In pInfo
                    Dim bUpdate As Boolean = False, bInsert As Boolean = False

                    Dim sql As String = "SELECT PreferredDays, FinalDeadline, Credit, kFactor, Description FROM fw7_ProjectInfo WHERE Project = '" & CInt(Project("ProjectNumber")) & "'"
                    Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                        Using mRDR As MySqlDataReader = mCmd.ExecuteReader
                            If mRDR.Read Then
                                bInsert = False
                                If Not Project("PreferredDays") = CStr(mRDR.Item("PreferredDays")) OrElse Not Project("FinalDeadline") = CStr(mRDR.Item("FinalDeadline")) OrElse Not Project("Credit") = CStr(mRDR.Item("Credit")) OrElse Not Project("kFactor") = CStr(mRDR.Item("kFactor")) OrElse Not Project("Description") = CStr(mRDR.Item("Description")) Then
                                    bUpdate = True
                                End If
                            Else
                                bInsert = True
                            End If
                        End Using
                    End Using
                    If bInsert Or bUpdate Then
                        If bInsert Then
                            sql = "INSERT INTO fw7_ProjectInfo (Project, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary, dtUpdated) values(@projectnumber,@serverip, @wuname, @numberofatoms, @preferreddays, @finaldeadline, @credit, @frames, @code, @description, @contact, @kfactor, @dtsummary, @dtupdated)"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.Parameters.AddWithValue("@projectnumber", CInt(Project("ProjectNumber")))
                                mCmd.Parameters.AddWithValue("@serverip", Project("ServerIP"))
                                mCmd.Parameters.AddWithValue("@wuname", Project("WUName"))
                                mCmd.Parameters.AddWithValue("@numberofatoms", CInt(Project("NumberOfAtoms")))
                                mCmd.Parameters.AddWithValue("@preferreddays", Project("PreferredDays"))
                                mCmd.Parameters.AddWithValue("@finaldeadline", Project("FinalDeadline"))
                                mCmd.Parameters.AddWithValue("@credit", CInt(Project("Credit")))
                                mCmd.Parameters.AddWithValue("@frames", CInt(Project("Frames")))
                                mCmd.Parameters.AddWithValue("@code", Project("Code"))
                                mCmd.Parameters.AddWithValue("@description", Project("Description"))
                                mCmd.Parameters.AddWithValue("@contact", Project("Contact"))
                                mCmd.Parameters.AddWithValue("@kfactor", Decimal.Parse(Project("kFactor")))
                                If String.IsNullOrEmpty(Project("dtSummary")) OrElse Project("dtSummary") = "" Then
                                    mCmd.Parameters.AddWithValue("@dtsummary", DBNull.Value)
                                Else
                                    mCmd.Parameters.AddWithValue("@dtsummary", Project("dtSummary"))
                                End If

                                If String.IsNullOrEmpty(Project("dtUpdated")) OrElse Project("dtUpdated") = "" Then
                                    mCmd.Parameters.AddWithValue("@dtupdated", DBNull.Value)
                                Else
                                    mCmd.Parameters.AddWithValue("@dtupdated", Project("dtUpdated"))
                                End If
                                mCmd.ExecuteNonQuery()
                            End Using
                        Else
                            sql = "UPDATE fw7_ProjectInfo SET PreferredDays = '" & Project("PreferredDays") & "', FinalDeadline = '" & Project("FinalDeadline") & "', Credit = '" & CInt(Project("Credit")) & "', kFactor = '" & Decimal.Parse(Project("kFactor")) & "', dtSummary = '" & CDate(Project("dtSummary")) & "', dtUpdated = '" & CDate(Project("dtUpdated")) & "' WHERE ProjectNumber = '" & CInt(Project("ProjectNumber")) & "'"
                            Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                                mCmd.ExecuteNonQuery()
                            End Using
                        End If
                    End If
                Next
            End Using
            log.Infoformat("Updating pSummary database took: " & Format_ts(DateTime.Now.Subtract(dtStart)))
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
#End Region
#Region "Depreciated info"
    Public Shared Function fw7_ReadDepreciatedProjects2() As SortedDictionary(Of Integer, SortedDictionary(Of DateTime, pSummary))
        Try
            Dim rVal As New SortedDictionary(Of Integer, SortedDictionary(Of DateTime, pSummary))
            If Not mIsInitialized Then
                If Not InitDB Then Return rVal
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ProjectChanges", mCon)
                Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                    While mRDR.Read
                        Dim dNP As New Dictionary(Of String, String)
                        dNP.Add("ProjectNumber", CStr(mRDR.Item("ProjectNumber")))
                        dNP.Add("ServerIP", CStr(mRDR.Item("ServerIP")))
                        dNP.Add("WUName", CStr(mRDR.Item("WUName")))
                        dNP.Add("NumberOfAtoms", CStr(mRDR.Item("NumberOfAtoms")))
                        dNP.Add("PreferredDays", CStr(mRDR.Item("PreferredDays")))
                        dNP.Add("FinalDeadline", CStr(mRDR.Item("FinalDeadline")))
                        dNP.Add("Credit", CStr(mRDR.Item("Credit")))
                        dNP.Add("Frames", CStr(mRDR.Item("Frames")))
                        dNP.Add("Code", CStr(mRDR.Item("Code")))
                        dNP.Add("Description", CStr(mRDR.Item("Description")))
                        dNP.Add("Contact", CStr(mRDR.Item("Contact")))
                        dNP.Add("kFactor", CStr(mRDR.Item("kFactor")))
                        dNP.Add("dtSummary", CStr(mRDR.Item("dtSummary")))
                        If IsDBNull(mRDR.Item("dtUpdated")) Then
                            dNP.Add("dtUpdated", String.Empty)
                        Else
                            dNP.Add("dtUpdated", CStr(mRDR.Item("dtUpdated")))
                        End If
                        Dim NP As New pSummary
                        NP.Populate(dNP)
                        'check project number
                        If rVal.ContainsKey(CInt(dNP("ProjectNumber"))) Then
                            If rVal(CInt(dNP("ProjectNumber"))).ContainsKey(CDate(dNP("dtUpdated"))) Then
                                If CBool(ConfigurationManager.AppSettings("Debug")) Then
                                    log.Debug("Attempt to add a depreciated project with a duplicate datetime field, aborted add")
                                End If
                            Else
                                rVal(CInt(dNP("ProjectNumber"))).Add(CDate(dNP("dtUpdated")), NP)
                            End If
                        Else
                            rVal.Add(CInt(dNP("ProjectNumber")), New SortedDictionary(Of DateTime, pSummary))
                            rVal(CInt(dNP("ProjectNumber"))).Add(CDate(dNP("dtUpdated")), NP)
                        End If
                    End While
                End Using
            End Using
            Return rVal
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return New SortedDictionary(Of Integer, SortedDictionary(Of DateTime, pSummary))
        End Try
    End Function
    Public Shared Function fw7_ReadDepreciatedProjects() As SortedDictionary(Of Integer, SortedDictionary(Of DateTime, Dictionary(Of String, String)))
        Try
            Dim rVal As New SortedDictionary(Of Integer, SortedDictionary(Of DateTime, Dictionary(Of String, String)))
            If Not mIsInitialized Then
                If Not InitDB Then Return rVal
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ProjectChanges", mCon)
                Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                    While mRDR.Read
                        Dim dNP As New Dictionary(Of String, String)
                        dNP.Add("ProjectNumber", CStr(mRDR.Item("ProjectNumber")))
                        dNP.Add("ServerIP", CStr(mRDR.Item("ServerIP")))
                        dNP.Add("WUName", CStr(mRDR.Item("WUName")))
                        dNP.Add("NumberOfAtoms", CStr(mRDR.Item("NumberOfAtoms")))
                        dNP.Add("PreferredDays", CStr(mRDR.Item("PreferredDays")))
                        dNP.Add("FinalDeadline", CStr(mRDR.Item("FinalDeadline")))
                        dNP.Add("Credit", CStr(mRDR.Item("Credit")))
                        dNP.Add("Frames", CStr(mRDR.Item("Frames")))
                        dNP.Add("Code", CStr(mRDR.Item("Code")))
                        dNP.Add("Description", CStr(mRDR.Item("Description")))
                        dNP.Add("Contact", CStr(mRDR.Item("Contact")))
                        dNP.Add("kFactor", CStr(mRDR.Item("kFactor")))
                        dNP.Add("dtSummary", CStr(mRDR.Item("dtSummary")))
                        If IsDBNull(mRDR.Item("dtUpdated")) Then
                            dNP.Add("dtUpdated", String.Empty)
                        Else
                            dNP.Add("dtUpdated", CStr(mRDR.Item("dtUpdated")))
                        End If
                        'check project number
                        If rVal.ContainsKey(CInt(dNP("ProjectNumber"))) Then
                            If rVal(CInt(dNP("ProjectNumber"))).ContainsKey(CDate(dNP("dtUpdated"))) Then
                                If CBool(ConfigurationManager.AppSettings("Debug")) Then
                                    log.Debug("Attempt to add a depreciated project with a duplicate datetime field, aborted add")
                                End If
                            Else
                                rVal(CInt(dNP("ProjectNumber"))).Add(CDate(dNP("dtUpdated")), dNP)
                            End If
                        Else
                            rVal.Add(CInt(dNP("ProjectNumber")), New SortedDictionary(Of DateTime, Dictionary(Of String, String)))
                            rVal(CInt(dNP("ProjectNumber"))).Add(CDate(dNP("dtUpdated")), dNP)
                        End If
                    End While
                End Using
            End Using
            Return rVal
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return New SortedDictionary(Of Integer, SortedDictionary(Of DateTime, Dictionary(Of String, String)))
        End Try
    End Function
    Public Shared Function fw7_UpdateDepreciatedProjects2(dProj As SortedDictionary(Of Integer, SortedDictionary(Of DateTime, pSummary))) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB Then Return False
            End If
            Dim dtStart As DateTime = DateTime.Now
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand
                mCMD.Connection = mCon
                mCMD.CommandType = CommandType.Text
                For Each iProj As Integer In dProj.Keys
                    For Each Project As pSummary In dProj(iProj).Values
                        Dim sql As String = "SELECT PreferredDays, FinalDeadline, Credit, kFactor, Description FROM fw7_ProjectChanges WHERE ProjectNumber = '" & CInt(Project.ProjectNumber) & "'"
                        mCMD.CommandText = sql
                        Dim bUpdate As Boolean = False, bInsert As Boolean = False
                        Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                            If mRDR.Read Then
                                bInsert = False
                                If Not Project.PreferredDays = CStr(mRDR.Item("PreferredDays")) OrElse Not Project.FinalDeadline = CStr(mRDR.Item("FinalDeadline")) OrElse Not Project.Credit = CStr(mRDR.Item("Credit")) OrElse Not Project.kFactor = CStr(mRDR.Item("kFactor")) OrElse Not Project.Description = CStr(mRDR.Item("Description")) Then
                                    bUpdate = True
                                End If
                            Else
                                bInsert = True
                            End If
                        End Using
                        If bInsert Or bUpdate Then
                            mCMD = New MySqlCommand
                            mCMD.Connection = mCon
                            mCMD.CommandType = CommandType.Text
                            If bInsert Then
                                sql = "INSERT INTO fw7_ProjectChanges (ProjectNumber, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary, dtUpdates) values(@projectnumber,@serverip, @wuname, @numberofatoms, @preferreddays, @finaldeadline, @credit, @frames, @code, @description, @contact, @kfactor, @dtsummary, @dtupdated)"
                                mCMD.CommandText = sql
                                mCMD.Parameters.AddWithValue("@projectnumber", CInt(Project.ProjectNumber))
                                mCMD.Parameters.AddWithValue("@serverip", Project.ServerIP)
                                mCMD.Parameters.AddWithValue("@wuname", Project.WUName)
                                mCMD.Parameters.AddWithValue("@numberofatoms", CInt(Project.NumberOfAtoms))
                                mCMD.Parameters.AddWithValue("@preferreddays", Project.PreferredDays)
                                mCMD.Parameters.AddWithValue("@finaldeadline", Project.FinalDeadline)
                                mCMD.Parameters.AddWithValue("@credit", CInt(Project.Credit))
                                mCMD.Parameters.AddWithValue("@frames", CInt(Project.Frames))
                                mCMD.Parameters.AddWithValue("@code", Project.Code)
                                mCMD.Parameters.AddWithValue("@description", Project.Description)
                                mCMD.Parameters.AddWithValue("@contact", Project.Contact)
                                mCMD.Parameters.AddWithValue("@kfactor", Project.kFactor)
                                mCMD.Parameters.AddWithValue("@dtsummary", Project.dtSummary)
                                If IsNothing(Project.dtUpdated) OrElse Project.dtUpdated = DateTime.MinValue Then
                                    mCMD.Parameters.AddWithValue("@dtupdated", DBNull.Value)
                                Else
                                    mCMD.Parameters.AddWithValue("@dtupdated", Project.dtUpdated)
                                End If
                                mCMD.ExecuteNonQuery()
                            ElseIf bUpdate Then
                                sql = "UPDATE fw7_ProjectChanges SET PreferredDays = '" & Project.PreferredDays & "', FinalDeadline = '" & Project.FinalDeadline & "', Credit = '" & CInt(Project.Credit) & "', kFactor = '" & Project.kFactor & "', dtSummary = '" & Project.dtSummary & "', dtUpdated = '" & Project.dtUpdated & "' WHERE ProjectNumber = '" & CInt(Project.ProjectNumber) & "'"
                                mCMD.CommandText = sql
                                mCMD.ExecuteNonQuery()
                            End If
                        End If
                    Next
                Next
            End Using
            log.Infoformat("Updating depreciated pSummary information took: " & Format_ts(DateTime.Now.Subtract(dtStart)))
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared Function fw7_UpdateDepreciatedProjects(dProj As SortedDictionary(Of Integer, SortedDictionary(Of DateTime, Dictionary(Of String, String)))) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB Then Return False
            End If
            Dim dtStart As DateTime = DateTime.Now
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim mCMD As MySqlCommand = New MySqlCommand
                mCMD.Connection = mCon
                mCMD.CommandType = CommandType.Text
                For Each iProj As Integer In dProj.Keys
                    For Each Project As Dictionary(Of String, String) In dProj(iProj).Values
                        Dim sql As String = "SELECT PreferredDays, FinalDeadline, Credit, kFactor, Description from fw7_ProjectChanges WHERE ProjectNumber = '" & CInt(Project("ProjectNumber")) & "'"
                        mCMD.CommandText = sql
                        Dim bUpdate As Boolean = False, bInsert As Boolean = False
                        Using mRDR As MySqlDataReader = mCMD.ExecuteReader
                            If mRDR.Read Then
                                bInsert = False
                                If Not Project("PreferredDays") = CStr(mRDR.Item("PreferredDays")) OrElse Not Project("FinalDeadline") = CStr(mRDR.Item("FinalDeadline")) OrElse Not Project("Credit") = CStr(mRDR.Item("Credit")) OrElse Not Project("kFactor") = CStr(mRDR.Item("kFactor")) OrElse Not Project("Description") = CStr(mRDR.Item("Description")) Then
                                    bUpdate = True
                                End If
                            Else
                                bInsert = True
                            End If
                        End Using
                        If bInsert Or bUpdate Then
                            mCMD = New MySqlCommand
                            mCMD.Connection = mCon
                            mCMD.CommandType = CommandType.Text
                            If bInsert Then
                                sql = "INSERT INTO fw7_ProjectChanges (ProjectNumber, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary, dtUpdates) values(@projectnumber,@serverip, @wuname, @numberofatoms, @preferreddays, @finaldeadline, @credit, @frames, @code, @description, @contact, @kfactor, @dtsummary, @dtupdated)"
                                mCMD.CommandText = sql
                                mCMD.Parameters.AddWithValue("@projectnumber", CInt(Project("ProjectNumber")))
                                mCMD.Parameters.AddWithValue("@serverip", Project("ServerIP"))
                                mCMD.Parameters.AddWithValue("@wuname", Project("WUName"))
                                mCMD.Parameters.AddWithValue("@numberofatoms", CInt(Project("NumberOfAtoms")))
                                mCMD.Parameters.AddWithValue("@preferreddays", Project("PreferredDays"))
                                mCMD.Parameters.AddWithValue("@finaldeadline", Project("FinalDeadline"))
                                mCMD.Parameters.AddWithValue("@credit", CInt(Project("Credit")))
                                mCMD.Parameters.AddWithValue("@frames", CInt(Project("Frames")))
                                mCMD.Parameters.AddWithValue("@code", Project("Code"))
                                mCMD.Parameters.AddWithValue("@description", Project("Description"))
                                mCMD.Parameters.AddWithValue("@contact", Project("Contact"))
                                mCMD.Parameters.AddWithValue("@kfactor", Project("kFactor"))
                                mCMD.Parameters.AddWithValue("@dtsummary", Project("dtSummary"))
                                If String.IsNullOrEmpty(Project("dtUpdated")) OrElse Project("dtUpdated") = "" Then
                                    mCMD.Parameters.AddWithValue("@dtupdated", DBNull.Value)
                                Else
                                    mCMD.Parameters.AddWithValue("@dtupdated", Project("dtUpdated"))
                                End If
                                mCMD.ExecuteNonQuery()
                            ElseIf bUpdate Then
                                sql = "UPDATE fw7_ProjectChanges SET PreferredDays = '" & Project("PreferredDays") & "', FinalDeadline = '" & Project("FinalDeadline") & "', Credit = '" & CInt(Project("Credit")) & "', kFactor = '" & Decimal.Parse(Project("kFactor")) & "', dtSummary = '" & CDate(Project("dtSummary")) & "', dtUpdated = '" & CDate(Project("dtUpdated")) & "' WHERE ProjectNumber = '" & CInt(Project("ProjectNumber")) & "'"
                                mCMD.CommandText = sql
                                mCMD.ExecuteNonQuery()
                            End If
                        End If
                    Next
                Next
            End Using
            log.Infoformat("Updating depreciated pSummary information took: " & Format_ts(DateTime.Now.Subtract(dtStart)))
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
#End Region
#Region "Importing"
    Public Shared Function ImportTabbed(fileTAB As String) As Boolean
        Try
            Dim txtTAB As String = My.Computer.FileSystem.ReadAllText(fileTAB)
            Dim pStr() As String = txtTAB.Split(CChar(Environment.NewLine))
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                For Each p As String In pStr
                    If Not p.Contains(vbTab) Then Exit For
                    Dim sql As String = "INSERT INTO fw7_ProjectInfo (Project, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary) values(@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12)"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        mCmd.CommandText = sql
                        For i = 0 To 12
                            Dim c As String = ""
                            If i < 11 Then
                                c = p.Substring(0, p.IndexOf(vbTab))
                            Else
                                c = p.Replace(Environment.NewLine, "").Trim
                            End If
                            If i = 0 Then
                                mCmd.Parameters.AddWithValue("@0", CInt(c))
                            ElseIf i = 12 Then
                                mCmd.Parameters.AddWithValue("@12", DateTime.UtcNow)
                            Else
                                mCmd.Parameters.AddWithValue(("@" & CStr(i) & Chr(32)).Trim, c.Trim)
                            End If

                            If i < 11 Then p = p.Substring(p.IndexOf(vbTab) + 1)
                        Next
                        Using mCHK As New MySqlCommand("select Project from Projects where Project like '" & CInt(mCmd.Parameters("@0").Value) & "'", mCon)
                            Using mRDR As MySqlDataReader = mCHK.ExecuteReader
                                If Not mRDR.Read Then
                                    mCmd.ExecuteNonQuery()
                                End If
                            End Using
                        End Using
                    End Using
                Next
            End Using
            Return True
        Catch ex As Exception
            log.FatalFormat(ex.Message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared Function ImportSQLCE() As Boolean
        Try
            log.Infoformat("Starting ProjectInfo import from SQLCE database to MySQL database")
            Dim dtStart As DateTime = DateTime.Now
            Dim lPD As New List(Of Dictionary(Of String, String))
            Using mCon As SqlCeConnection = New SqlCeConnection("Data Source=" & System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/fw7_ProjectSummary.sdf") & ";Persist Security Info=False;")
                mCon.Open()
                Dim mCMD As SqlCeCommand = New SqlCeCommand("SELECT * FROM Projects", mCon)
                Using mRDR As SqlCeDataReader = mCMD.ExecuteReader
                    While mRDR.Read
                        Dim nP As New Dictionary(Of String, String)
                        nP.Add("ProjectNumber", CStr(mRDR.Item("ProjectNumber")))
                        nP.Add("ServerIP", CStr(mRDR.Item("ServerIP")))
                        nP.Add("WUName", CStr(mRDR.Item("WUName")))
                        nP.Add("NumberOfAtoms", CStr(mRDR.Item("NumberOfAtoms")))
                        nP.Add("PreferredDays", CStr(mRDR.Item("PreferredDays")))
                        nP.Add("FinalDeadline", CStr(mRDR.Item("FinalDeadline")))
                        nP.Add("Credit", CStr(mRDR.Item("Credit")))
                        nP.Add("Frames", CStr(mRDR.Item("Frames")))
                        nP.Add("Code", CStr(mRDR.Item("Code")))
                        nP.Add("Description", CStr(mRDR.Item("Description")))
                        nP.Add("Contact", CStr(mRDR.Item("Contact")))
                        nP.Add("kFactor", CStr(mRDR.Item("kFactor")))
                        If Not IsDBNull(mRDR.Item("dtSummary")) AndAlso Not CStr(mRDR.Item("dtSummary")) = "" Then
                            nP.Add("dtSummary", CDate(mRDR.Item("dtSummary")).ToString("s"))
                        End If
                        lPD.Add(nP)
                    End While
                End Using
            End Using
            Dim iImported As Int32 = 0
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                For Each NP As Dictionary(Of String, String) In lPD
                    Dim sql As String = "SELECT Project FROM fw7_ProjectInfo WHERE Project = '" & CInt(NP("ProjectNumber")) & "'"
                    Dim bHas As Boolean = True
                    Using mCmd As New MySqlCommand(sql, mCon)
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            bHas = mRdr.HasRows
                        End Using
                    End Using
                    If Not bHas Then
                        sql = "INSERT INTO fw7_ProjectInfo (Project, ServerIP, WUName, NumberOfAtoms, PreferredDays, FinalDeadline, Credit, Frames, Code, Description, Contact, kFactor, dtSummary) values(@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12)"
                        Using mCmd As New MySqlCommand(sql, mCon)
                            mCmd.Parameters.AddWithValue("@0", CInt(NP("ProjectNumber")))
                            mCmd.Parameters.AddWithValue("@1", NP("ServerIP"))
                            mCmd.Parameters.AddWithValue("@2", NP("WUName"))
                            mCmd.Parameters.AddWithValue("@3", CInt(NP("NumberOfAtoms")))
                            mCmd.Parameters.AddWithValue("@4", NP("PreferredDays"))
                            mCmd.Parameters.AddWithValue("@5", NP("FinalDeadline"))
                            mCmd.Parameters.AddWithValue("@6", NP("Credit"))
                            mCmd.Parameters.AddWithValue("@7", CInt("Frames"))
                            mCmd.Parameters.AddWithValue("@8", NP("Code"))
                            mCmd.Parameters.AddWithValue("@9", NP("Description"))
                            mCmd.Parameters.AddWithValue("@10", NP("Contact"))
                            mCmd.Parameters.AddWithValue("@11", NP("kFactor"))
                            mCmd.Parameters.AddWithValue("@12", CDate(NP("dtSummary")))
                            If mCmd.ExecuteNonQuery() <> 0 Then
                                iImported += 1
                            End If
                        End Using
                    End If
                Next
            End Using
            log.Infoformat("Reading projects took " & Format_ts(DateTime.Now.Subtract(dtStart)) & ", imported " & iImported.ToString & " projects")
            Return True
        Catch ex As Exception
            log.Error(ex)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
#End Region
End Class





