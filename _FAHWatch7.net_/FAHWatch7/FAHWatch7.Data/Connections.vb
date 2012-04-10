Imports log4net
Imports MySql.Data.MySqlClient
Public Class Connections
    Private Shared log As log4net.ILog = LogManager.GetLogger(GetType(FAHWatch7.Data.Connections))
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
                Dim bInvalid As Boolean = False, bBlacklisted As Boolean = False
                Dim sql As String = "SELECT * FROM fw7_InvalidConnections"
                Using mCmd As New MySqlCommand(sql, mCon)
                    Try
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            bInvalid = True
                        End Using
                    Catch ex As MySqlException
                        If ex.Number = 1146 Then
                            bInvalid = False
                        Else
                            Throw New Exception("Exception while querying table", ex)
                        End If
                    End Try
                End Using
                sql = "SELECT * FROM fw7_Blacklisted"
                Using mCmd As New MySqlCommand(sql, mCon)
                    Try
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            bBlacklisted = True
                        End Using
                    Catch ex As MySqlException
                        If ex.Number = 1146 Then
                            bBlacklisted = False
                        Else
                            Throw New Exception("Exception while querying table", ex)
                        End If
                    End Try
                End Using
                If Not bBlacklisted Then
                    sql = "CREATE TABLE fw7_Blacklisted (IPAddress BIGINT UNSIGNED, BeginTime DATETIME, EndTime DATETIME)"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        mCmd.ExecuteNonQuery()
                    End Using
                End If
                If Not bInvalid Then
                    sql = "CREATE TABLE fw7_InvalidConnections (IPAddress BIGINT UNSIGNED, ClientID BINARY(16), UserName VARCHAR(30), TimeStamp DATETIME)"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        mCmd.ExecuteNonQuery()
                    End Using
                End If
                mIsInitialized = True
                Return True
            End Using
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
#Region "InvalidConnections"
    Public Shared Function AddInvalidConnection(IPAddress As String, ClientID As String, UserName As String, TimeStamp As DateTime) As Boolean
        Try
            If Not IsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "INSERT INTO fw7_InvalidConnections (IPAddress, ClientID, UserName, TimeStamp) VALUES (@IPAddress, @ClientID, @UserName, @TimeStamp)"
                Using mCmd As New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@IPAddress", BitConverter.ToUInt64(System.Net.IPAddress.Parse(IPAddress).GetAddressBytes(), 0))
                    mCmd.Parameters.AddWithValue("@ClientID", Guid.Parse(ClientID).ToByteArray)
                    mCmd.Parameters.AddWithValue("@UserName", UserName)
                    mCmd.Parameters.AddWithValue("@TimeStamp", TimeStamp)
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
    Public Shared Function PurgeInvalidConnections(Optional FromWhen As DateTime = Nothing) As Boolean
        Try
            If Not IsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "DELETE FROM fw7_InvalidConnections"
                If Not IsNothing(FromWhen) Then sql &= " WHERE TimeStamp >= @FromWhen"
                Using mCmd As New MySqlCommand(sql, mCon)
                    If Not IsNothing(FromWhen) Then mCmd.Parameters.AddWithValue("@FromWhen", FromWhen)
                    Return mCmd.ExecuteNonQuery <> 0
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
#End Region
#Region "Blacklist"
    Public Shared Function AddBlacklisted(IPAddress As String, BeginTime As DateTime, EndTime As DateTime) As Boolean
        Try
            If Not IsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "INSERT INTO fw7_Blacklisted (IPAddress, Begintime, EndTime) VALUES (@IPAddress, @BeginTime, @EndTime)"
                Using mCmd As New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@IPAddress", BitConverter.ToUInt64(System.Net.IPAddress.Parse(IPAddress).GetAddressBytes(), 0))
                    mCmd.Parameters.AddWithValue("@BeginTime", BeginTime)
                    mCmd.Parameters.AddWithValue("@EndTime", EndTime)
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
    Public Shared ReadOnly Property BlacklistedIPs As List(Of String)
        Get
            Try
                Dim rVal As New List(Of String)
                If Not IsInitialized Then
                    If Not InitDB() Then Return rVal
                End If
                Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                    mCon.Open()
                    Dim sql As String = "SELECT IPAddress FROM fw7_Blacklisted"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            While mRdr.Read
                                rVal.Add(New System.Net.IPAddress(BitConverter.GetBytes(mRdr.GetUInt64("IPAddress"))).ToString)
                            End While
                        End Using
                    End Using
                End Using
                Return rVal
            Catch ex As Exception
                Log.FatalFormat(ex.message)
                mHasError = True
                mException = ex
                Return New List(Of String)
            End Try
        End Get
    End Property
    Public ReadOnly Property BlackListedData As List(Of Dictionary(Of String, String))
        Get
            Try
                Dim rVal As New List(Of Dictionary(Of String, String))
                If Not IsInitialized Then
                    If Not InitDB() Then Return rVal
                End If
                Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                    mCon.Open()
                    Dim sql As String = "SELECT * FROM fw7_Blacklisted"
                    Using mCmd As New MySqlCommand(sql, mCon)
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            While mRdr.Read
                                Dim nDic As New Dictionary(Of String, String)
                                nDic.Add("IPAddress", New System.Net.IPAddress(BitConverter.GetBytes(mRdr.GetUInt64("IPAddress"))).ToString)
                                nDic.Add("BeginTime", mRdr.GetDateTime("BeginTime").ToString("s"))
                                nDic.Add("EndTime", mRdr.GetDateTime("EndTime").ToString("s"))
                                rVal.Add(nDic)
                            End While
                        End Using
                    End Using
                End Using
                Return rVal
            Catch ex As Exception
                Log.FatalFormat(ex.message)
                mHasError = True
                mException = ex
                Return New List(Of Dictionary(Of String, String))
            End Try
        End Get
    End Property
    Public Shared Function ExpireBlacklist(Optional IPAddress As String = Nothing) As Boolean
        Try
            If Not IsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "UPDATE fw7_Blacklisted SET EndTime = '" & DateTime.UtcNow & "'"
                If Not IsNothing(IPAddress) Then sql &= " WHERE IPAddress = '" & BitConverter.ToUInt64(System.Net.IPAddress.Parse(IPAddress).GetAddressBytes(), 0) & "'"
                Using mCmd As New MySqlCommand(sql, mCon)
                    Return mCmd.ExecuteNonQuery <> 0
                End Using
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
#End Region
End Class



