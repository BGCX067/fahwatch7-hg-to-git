Imports log4net
Imports MySql.Data.MySqlClient


Public Class Statistics
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
#Region "Initialization"
    Public Shared Function InitDB() As Boolean
        Try
            If mIsInitialized Then Return True
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "SELECT * FROM fw7_Statistics"
                Dim bHas As Boolean = False
                Using mCmd As New MySqlCommand(sql, mCon)
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
                    sql = "CREATE TABLE fw7_Statistics (ClientID BINARY(16), Submission DATETIME, HardwareID BINARY(16), Project INT, RCG VARCHAR(100), TpfMin INT, TpfAvg INT, TpfMax INT)"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        mCmd.ExecuteNonQuery()
                    End Using
                End If
            End Using
            mIsInitialized = True
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
    Public Shared Function SubmitStatistics(ClientID As String, HardwareID As String, Submission As DateTime, Project As Integer, RCG As String, TpfMin As Integer, TpfAvg As Integer, TpfMax As Integer) As Boolean
        Try
            If Not IsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "INSERT INTO fw7_Statistics (ClientID, Submission, HardwareID, Project, RCG, TpfMin, TpfAvg, TpfMax) VALUES(@ClientID, @Submission, @HardwareID, @Project, @RCG, @TpfMin, @TpfAvg, @TpfMax)"
                Using mCmd As New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@ClientID", Guid.Parse(ClientID).ToByteArray)
                    mCmd.Parameters.AddWithValue("@Submission", Submission)
                    mCmd.Parameters.AddWithValue("@HardwareID", Guid.Parse(HardwareID).ToByteArray)
                    mCmd.Parameters.AddWithValue("@Project", Project)
                    mCmd.Parameters.AddWithValue("@RCG", RCG)
                    mCmd.Parameters.AddWithValue("@TpfMin", TpfMin)
                    mCmd.Parameters.AddWithValue("@TpfAvg", TpfAvg)
                    mCmd.Parameters.AddWithValue("@TpfMax", TpfMax)
                    Return mCmd.ExecuteNonQuery() <> 0
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared Function GetStatisticsByClient(ClientID As String) As List(Of Dictionary(Of String, String))
        Try

        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return New List(Of Dictionary(Of String, String))
        End Try
    End Function
End Class



