Imports log4net
Imports MySql.Data.MySqlClient
Imports MySql.Data
Public Class ExceptionReports
    Private Shared log As log4net.ILog = LogManager.GetLogger(GetType(FAHWatch7.Data.ExceptionReports))
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
#Region "Initialization"
    Public Shared Function InitDB() As Boolean
        Try
            If mIsInitialized Then Return True
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                If Not mCon.State = ConnectionState.Open Then
                    log.Fatal("Can't open MySQL connection")
                    mHasError = True
                    mException = New Exception("Can't open MySQL connection, connection string: " & ConnectionStrings.GetConnectionString)
                    Return False
                End If
                Dim bHas As Boolean = False
                Using mCmd As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ExceptionReports", mCon)
                    Try
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            bHas = True
                        End Using
                    Catch ex As MySqlException
                        If ex.Number = 1146 Then
                            bHas = False
                        Else
                            Throw New Exception("Exception while querying table", ex)
                        End If
                    End Try
                End Using
                If Not bHas Then
                    log.InfoFormat("Table ExceptionReports doesn't excist, creating now")
                    Dim sql As String = "CREATE TABLE fw7_ExceptionReports (ExSource VARCHAR(100), ErrNumber INT, ExMessage VARCHAR(100), StackTrace TEXT, ExTimeStamp DATETIME, ClientID CHAR(36), ClientVersion VARCHAR(10))"
                    Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                        mCmd.ExecuteNonQuery()
                    End Using
                End If
                mIsInitialized = True
            End Using
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared ReadOnly Property IsInitialized As Boolean
        Get
            Return mIsInitialized
        End Get
    End Property
#End Region
#Region "Exception reports"
    Public Shared Function ReportException(ExSource As String, ErrNumber As Integer, ExMessage As String, StackTrace As String, ExTimeStamp As DateTime, ClientVersion As String, ClientID As String) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "select * from fw7_ExceptionReports where  ClientID like '" & ClientID & "' and ClientVersion like '" & ClientVersion & "' and ExSource like '" & ExSource & "' and ErrNumber like '" & ErrNumber & "' and ExMessage like '" & ExMessage & "' and StrackTrace like '" & StackTrace & "' and ExTimeStamp like '" & ExTimeStamp & "'"
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                        If mRdr.Read Then
                            Return True
                        End If
                    End Using
                End Using
                sql = "INSERT INTO fw7_ExceptionReports (ExSource, ErrNumber, ExMessage, StackTrace, ExTimeStamp, ClientID, ClientVersion) VALUES(@ExSource, @ErrNumber, @ExMessage, @StackTrace, @ExTimeStamp, @ClientID, @ClientVersion)"
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@ExSource", ExSource)
                    mCmd.Parameters.AddWithValue("@ErrNumber", ErrNumber)
                    mCmd.Parameters.AddWithValue("@ExMessage", ExMessage)
                    mCmd.Parameters.AddWithValue("@StackTrace", StackTrace)
                    mCmd.Parameters.AddWithValue("@ExTimeStamp", ExTimeStamp)
                    mCmd.Parameters.AddWithValue("@ClientID", ClientID)
                    mCmd.Parameters.AddWithValue("@ClientVersion", ClientVersion)
                    mCmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared Function GetAllExceptions() As List(Of Dictionary(Of String, String))
        Dim rVal As New List(Of Dictionary(Of String, String))
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return rVal
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Using mCmd As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_ExceptionReports", mCon)
                    Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                        While mRdr.Read
                            Dim nDic As New Dictionary(Of String, String)
                            nDic.Add("ExSource", mRdr.GetString("ExSource"))
                            nDic.Add("ErrNumber", CStr(mRdr.GetInt32("ErrNumber")))
                            nDic.Add("ExMessage", mRdr.GetString("ExMessage"))
                            nDic.Add("StackTrace", mRdr.GetString("StackTrace"))
                            nDic.Add("ExTimeStamp", CDate(mRdr.GetDateTime("ExTimeStamp")).ToString("s"))
                            nDic.Add("ClientID", mRdr.GetString("ClientID"))
                            nDic.Add("ClientVersion", mRdr.GetString("ClientVersion"))
                            rVal.Add(nDic)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
        End Try
        Return rVal
    End Function
    Public Shared Function GetExceptionReports(Optional ExSource As String = Nothing, Optional ErrNumber As Integer = Nothing, Optional ExMessage As String = Nothing, Optional ClientID As String = Nothing, Optional ClientVersion As String = Nothing) As List(Of Dictionary(Of String, String))
        Dim rVal As New List(Of Dictionary(Of String, String))
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return rVal
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "SELECT * FROM fw7_ExceptionReports"
                If Not IsNothing(ExSource) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ExSource LIKE '" & ExSource & "'"
                End If
                If Not IsNothing(ErrNumber) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ErrNumber LIKE '" & ErrNumber & "'"
                End If
                If Not IsNothing(ExMessage) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ExMessage LIKE '" & ExMessage & "'"
                End If
                If Not IsNothing(ClientID) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ClientID LIKE '" & ClientID & "'"
                End If
                If Not IsNothing(ClientVersion) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ClientVersion LIKE '" & ClientVersion & "'"
                End If
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                        While mRdr.Read
                            Dim nDic As New Dictionary(Of String, String)
                            nDic.Add("ExSource", mRdr.GetString("ExSource"))
                            nDic.Add("ErrNumber", CStr(mRdr.GetInt32("ErrNumber")))
                            nDic.Add("ExMessage", mRdr.GetString("ExMessage"))
                            nDic.Add("StackTrace", mRdr.GetString("StackTrace"))
                            nDic.Add("ExTimeStamp", CDate(mRdr.GetDateTime("ExTimeStamp")).ToString("s"))
                            nDic.Add("ClientID", mRdr.GetString("ClientID"))
                            nDic.Add("ClientVersion", mRdr.GetString("ClientVersion"))
                            rVal.Add(nDic)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
        End Try
        Return rVal
    End Function
    Public Shared Sub ClearExceptionReports(Optional ExSource As String = Nothing, Optional ErrNumber As Integer = Nothing, Optional ExMessage As String = Nothing, Optional ClientID As String = Nothing, Optional ClientVersion As String = Nothing)
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "DELETE FROM fw7_ExceptionReports"
                If Not IsNothing(ExSource) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ExSource LIKE '" & ExSource & "'"
                End If
                If Not IsNothing(ErrNumber) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ErrNumber LIKE '" & ErrNumber & "'"
                End If
                If Not IsNothing(ExMessage) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ExMessage LIKE '" & ExMessage & "'"
                End If
                If Not IsNothing(ClientID) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ClientID LIKE '" & ClientID & "'"
                End If
                If Not IsNothing(ClientVersion) Then
                    If Not sql.EndsWith(" ") Then sql &= " "
                    sql &= "WHERE ClientVersion LIKE '" & ClientVersion & "'"
                End If
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    mCmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
        End Try
    End Sub
#End Region
End Class


