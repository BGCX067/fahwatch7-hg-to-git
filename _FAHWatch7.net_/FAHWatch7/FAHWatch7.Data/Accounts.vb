Imports log4net
Imports MySql.Data.MySqlClient
Imports MySql.Data
Partial Public Class Accounts
    Private Shared Log As log4net.ILog = log4net.LogManager.GetLogger(GetType(FAHWatch7.Data.Accounts))
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
                    Log.Fatal("Can't open MySQL connection")
                    mHasError = True
                    mException = New Exception("Can't open MySQL connection, connection string: " & ConnectionStrings.GetConnectionString)
                    Return False
                End If
                Dim bHas As Boolean = False
                Using mCmd As MySqlCommand = New MySqlCommand("SELECT * FROM fw7_Accounts", mCon)
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
                    Log.InfoFormat("Table ExceptionReports doesn't excist, creating now")
                    Dim sql As String = "CREATE TABLE fw7_Accounts (ClientID BINARY(16), UserName NVARCHAR(30), IPAddress TEXT, LastConnected DATETIME)"
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
#Region "Accounts"
    Public Shared ReadOnly Property GetFreeID As String
        Get
            Try
                If Not IsInitialized Then
                    If Not InitDB() Then Return String.Empty
                End If
                Dim mGUID As Guid
                Do
                    mGUID = Guid.NewGuid
                    Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                        mCon.Open()
                        Dim sql As String = "SELECT ClientID FROM fw7_Accounts WHERE ClientID LIKE '" & mGUID.ToString & "'"
                        Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                            Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                                If Not mRdr.Read Then Exit Do
                            End Using
                        End Using
                    End Using
                Loop
                Return mGUID.ToString
            Catch ex As Exception
                Log.FatalFormat(ex.message)
                mHasError = True
                mException = ex
                Return String.Empty
            End Try
        End Get
    End Property
    Public Shared Function CreateAccount(ClientID As String, ClientIP As String, UserName As String) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "INSERT INTO fw7_Accounts (ClientID, UserName, IPAddress, LastConnected) VALUES(@ClientID, @UserName, @IPAddress, @LastConnected)"
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@ClientID", Guid.Parse(ClientID).ToByteArray)
                    mCmd.Parameters.AddWithValue("@UserName", UserName)
                    mCmd.Parameters.AddWithValue("@IPAddress", ClientIP)
                    mCmd.Parameters.AddWithValue("@LastConnected", DateTime.UtcNow)
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
    Public Shared Function UpdateAccountIPs(ClientID As String, ClientIPs As String) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                Dim sql As String = "UPDATE fw7_Accounts SET IPAddress = '" & ClientIPs & "' WHERE ClientID = @ClientID "
                Using mCmd As New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@ClientID", Guid.Parse(ClientID).ToByteArray)
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
    Public Shared Function AddIP(ClientID As String, ClientIP As String, UserName As String) As Boolean
        Try
            If Not mIsInitialized Then
                If Not InitDB() Then Return False
            End If
            Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                mCon.Open()
                Dim sql As String = "SELECT IPAddress FROM fw7_Accounts WHERE ClientID = @ClientID AND UserName = @UserName"
                Dim strIP As String = String.Empty
                Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                    mCmd.Parameters.AddWithValue("@ClientID", Guid.Parse(ClientID).ToByteArray)
                    mCmd.Parameters.AddWithValue("@UserName", UserName)
                    Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                        If mRdr.Read Then
                            strIP = mRdr.GetString("IPAddress")
                        End If
                    End Using
                End Using
                If strIP = String.Empty Then
                    'should never be empty
                    Return False
                ElseIf strIP.Contains(ClientIP) Then
                    'can't add, don't return true as query can be made to get valid ip adresses for this account 
                    Return False
                Else
                    If Not strIP.EndsWith("//") Then strIP &= "//"
                    strIP &= ClientIP
                    sql = "UPDATE 'fw7_accounts' SET IPAddress = '" & strIP & "' WHERE ClientID = '" & ClientID & "' AND UserName = '" & UserName & "'"
                    Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                        Return mCmd.ExecuteNonQuery() <> 0
                    End Using
                End If
            End Using
        Catch ex As Exception
            Log.FatalFormat(ex.message)
            mHasError = True
            mException = ex
            Return False
        End Try
    End Function
    Public Shared ReadOnly Property AccountsDS As DataSet
        Get
            Try
                If Not IsInitialized Then
                    If Not InitDB() Then
                        Return New DataSet
                    End If
                End If
                Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                    mCon.Open()
                    Dim sql As String = "SELECT * FROM fw7_Accounts"
                    Using mDA As MySqlDataAdapter = New MySqlDataAdapter(sql, mCon)
                        Dim DS As New DataSet
                        mDA.Fill(DS)
                        Return DS
                    End Using
                End Using
            Catch ex As Exception
                Log.FatalFormat(ex.message)
                mHasError = True
                mException = ex
                Return New DataSet
            End Try
        End Get
    End Property
    Public Shared ReadOnly Property Accounts As List(Of Dictionary(Of String, String))
        Get
            Try
                Dim rVal As New List(Of Dictionary(Of String, String))
                If Not IsInitialized Then
                    If Not InitDB() Then
                        Return rVal
                    End If
                End If
                Using mCon As MySqlConnection = New MySqlConnection(ConnectionStrings.GetConnectionString)
                    mCon.Open()
                    Dim sql As String = "SELECT * FROM fw7_Accounts"
                    Using mCmd As MySqlCommand = New MySqlCommand(sql, mCon)
                        Using mRdr As MySqlDataReader = mCmd.ExecuteReader
                            While mRdr.Read
                                Dim nDic As New Dictionary(Of String, String)
                                nDic.Add("ClientID", Guid.Parse(CStr(mRdr.Item("ClientID"))).ToString)
                                nDic.Add("ClientID", mRdr.GetString("ClientID"))
                                nDic.Add("UserName", mRdr.GetString("UserName"))
                                nDic.Add("IPAddress", mRdr.GetString("IPAddress"))
                                nDic.Add("LastConnected", mRdr.GetDateTime("LastConnected").ToString("s"))
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
#End Region
End Class




