'/*
' * FAHWatch7 DATA Copyright Marvin Westmaas ( mtm ) 
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
'

'Still not working well IF I nest datareaders in the same try catch.. :(
Imports System.IO
Imports System.Data
Imports System.Data.SQLite
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing
Imports System.Text
Imports System.Threading
Imports System.Globalization
Imports FAHWatch7.modMySettings
Imports FAHWatch7.Exceptions
Friend Class Data
#Region "Declarations"
    Implements IDisposable
    Private strCon As String = ""
    Private mDataRoot As String = ""
    Friend ReadOnly Property dbFile As String
        Get
            Return mDataRoot & My.Application.Info.AssemblyName & ".db"
        End Get
    End Property
    Friend ReadOnly Property dbBackup As String
        Get
            Return mDataRoot & My.Application.Info.AssemblyName & "-backup.db"
        End Get
    End Property
    Private Const strDTFormat As String = "dd/MM/yyyy-H:mm:ss"
    Private sib As String = "','"
    Private sqlErr As SQLiteErrorCode
#End Region
#Region "Master database access"
    Friend Class clsDB
        Implements IDisposable
#Region "Declarations"
        Private dInstances As New Dictionary(Of Int32, clsDBInstance)
        Private WithEvents Master As clsDBInstance
        Private connectionString As String = String.Empty, dbFile As String = String.Empty, bIsNew As Boolean = False, TimeOut As TimeSpan = TimeSpan.FromSeconds(5)
#End Region
#Region "Properties"
        Friend ReadOnly Property IsNew As Boolean
            Get
                Return bIsNew
            End Get
        End Property
        Friend Property TimeOutInterval As TimeSpan
            Get
                Return TimeOut
            End Get
            Set(ByVal value As TimeSpan)
                TimeOut = value
                'maybe don't use it globally
                For Each dbI As clsDBInstance In dInstances.Values.ToList
                    dbI.TimeOut = value
                Next
            End Set
        End Property
        Friend ReadOnly Property IsClosed As Boolean
            Get
                Return CBool(dInstances.Count = 0)
            End Get
        End Property
#End Region
#Region "Master database creation and destruction"
        Friend Sub Init(Optional ByVal dbName As String = Nothing, Optional ByVal Pragma As String = Nothing, Optional ByVal tsTimeOut As TimeSpan = Nothing)
            Try
                If Not IsNothing(dbName) Then
                    dbFile = dbName
                Else
                    dbFile = Application.StartupPath & "\" & My.Application.Info.AssemblyName & ".db"
                End If
                If Not IsNothing(tsTimeOut) Then TimeOut = tsTimeOut
                If Not dbFile.IndexOf("\") = -1 Then
                    dbFile = dbFile.Replace("\", "/")
                End If
                If Not My.Computer.FileSystem.FileExists(dbName) Then
                    bIsNew = True
                    connectionString = "Data source=" & dbFile & ";New=True"
                    If IsNothing(Pragma) Then
                        connectionString &= ";Compress=True;Synchronous=on"
                    Else
                        connectionString &= ";" & Pragma
                    End If
                Else
                    connectionString = "Data source=" & dbFile
                    If IsNothing(Pragma) Then
                        connectionString &= ";Compress=True;Synchronous=on"
                    Else
                        connectionString &= ";" & Pragma
                    End If
                End If
                Master = New clsDBInstance(0)
                Master.AllowDispose = False
                'Master.con = New SQLiteConnection(connectionString)
                Master.strConnection = connectionString
                Try
                    Master.TryOpenCon()
                Catch ex As Exception
                    WriteLog("Can't initialize database access", eSeverity.Important)
                    WriteError(ex.Message, Err)
                    Exit Try
                End Try
                AddHandler Master.conDisposed, AddressOf _conDisposed
                AddHandler Master.cmdDisposed, AddressOf _cmdDisposed
                AddHandler Master.ConnectionStateChange, AddressOf _ConnectionStateChange
                AddHandler Master.IsDisposed, AddressOf FreeInstance
                dInstances.Add(0, Master)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Sub close(Optional ByVal deleteDB As Boolean = False)
            Try
                If dInstances.Count > 0 Then
                    'Allow close master
                    dbMaster.AllowDispose = True
                    Dim nL(0 To dInstances.Count - 1) As clsDBInstance
                    dInstances.Values.CopyTo(nL, 0)
                    For Each dInstance As clsDBInstance In nL
                        'dInstance = Nothing
                        dInstance.TryCloseCon()
                        Application.DoEvents()
                        dInstance.Dispose()
                    Next
                    Dim dtNow As DateTime = DateTime.Now
                    Do
                        Application.DoEvents()
                    Loop Until DateTime.Now.Subtract(dtNow).TotalSeconds >= 2 Or dInstances.Count = 0
                    If dInstances.Count > 0 Then
                        WriteLog("Couldn't close all database connections", eSeverity.Important)
                    End If
                End If
                Try
                    If My.Computer.FileSystem.FileExists(sqdata.dbBackup) Then
                        My.Computer.FileSystem.DeleteFile(sqdata.dbBackup)
                    End If
                Catch ex As Exception : End Try
                Try
                    If deleteDB Then
                        My.Computer.FileSystem.DeleteFile(sqdata.dbFile)
                    End If
                Catch ex As Exception : End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    For Each DictionaryEntry In dInstances
                        DictionaryEntry.Value.Dispose()
                    Next
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
#End Region
#Region "Database primary query instance"
        Friend ReadOnly Property dbMaster As clsDBInstance
            Get
                Return Master
            End Get
        End Property
#End Region
#Region "Database query instances"
        Friend Class clsDBInstance
            Implements IDisposable
#Region "Events"
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")> Friend Event ConnectionStateChange(ByVal ID As Int32, ByVal OldState As ConnectionState, ByVal NewState As ConnectionState)
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")> Friend Event HandleExc(ByVal RaisedExeption As Exception, ByVal TheErr As ErrObject, ByVal ID As Int32, ByVal exInfo As String)
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")> Friend Event cmdDisposed(ByVal ID As Int32)
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")> Friend Event conDisposed(ByVal ID As Int32)
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")> Friend Event IsDisposed(ByVal ID As Int32)
#End Region
#Region "Declares"
            Private dtNow As DateTime = #1/1/2000#
            Private b_cmdDisposed As Boolean, b_conDisposed As Boolean
            Private i_id As Int32 = -1
            Private bAllowDispose As Boolean = True
            Private WithEvents con As SQLiteConnection
            Private trans As SQLiteTransaction
            Private WithEvents cmd As New SQLiteCommand
            Private WithEvents rdr As SQLiteDataReader
            Private sqlException As SQLiteErrorCode = SQLiteErrorCode.Ok
            Property TimeOut As TimeSpan = TimeSpan.FromSeconds(5)
            Property strConnection As String = String.Empty
#End Region
#Region "Properties"
            Friend Property AllowDispose As Boolean
                Get
                    Return bAllowDispose
                End Get
                Set(ByVal value As Boolean)
                    bAllowDispose = value
                End Set
            End Property
            Friend ReadOnly Property ID As Int32
                Get
                    Return i_id
                End Get
            End Property
            Friend ReadOnly Property conState As ConnectionState
                Get
                    Return con.State
                End Get
            End Property
            Friend ReadOnly Property LastException As SQLiteErrorCode
                Get
                    Return sqlException
                End Get
            End Property
            Friend ReadOnly Property GetSQLCommand As SQLiteCommand
                Get
                    Try
                        cmd = con.CreateCommand
                        Return CType(cmd.Clone, SQLiteCommand)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        If TypeOf (ex) Is SQLiteException Then
                            sqlException = CType(ex, SQLiteException).ErrorCode
                        End If
                        Return Nothing
                    End Try
                End Get
            End Property
            Friend ReadOnly Property GetTableNames() As List(Of String)
                Get
                    Try
                        Dim rVal As New List(Of String)
                        If con.State = ConnectionState.Open Then
                            Dim SchemaTable = con.GetSchema(SQLiteMetaDataCollectionNames.Tables)
                            For int As Integer = 0 To SchemaTable.Rows.Count - 1
                                If SchemaTable.Rows(int)!TABLE_TYPE.ToString = "table" Then
                                    rVal.Add(SchemaTable.Rows(int)!TABLE_NAME.ToString())
                                End If
                            Next
                        End If
                        Return rVal
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return New List(Of String)
                    End Try
                End Get
            End Property
            Friend ReadOnly Property GetTableFields(Table As String) As List(Of String)
                Get
                    Try
                        Dim rVal As New List(Of String)
                        If con.State = ConnectionState.Open Then
                            Dim SchemaTable = con.GetSchema("Columns", {Nothing, Nothing, Table, Nothing})
                            For int As Integer = 0 To SchemaTable.Rows.Count - 1
                                rVal.Add(SchemaTable.Rows(int)!COLUMN_NAME.ToString())
                            Next
                        End If
                        Return rVal
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return New List(Of String)
                    End Try
                End Get
            End Property
            <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")> Friend ReadOnly Property ExecuteNonQuery(ByVal Sql As String, Optional ByVal sqlType As System.Data.CommandType = CommandType.Text) As SQLiteErrorCode
                Get
                    Try
                        If con.State = ConnectionState.Open Then
                            Try
                                cmd = con.CreateCommand
                            Catch Sqlex As Exception
                                'new instance, or try to run on the same instance?
                                Select Case cmd.Transaction.IsolationLevel
                                    Case IsolationLevel.ReadCommitted

                                    Case IsolationLevel.Chaos

                                    Case IsolationLevel.ReadUncommitted

                                    Case IsolationLevel.RepeatableRead

                                    Case IsolationLevel.Serializable

                                    Case IsolationLevel.Snapshot

                                    Case IsolationLevel.Unspecified

                                End Select
                            End Try
                            cmd.CommandType = sqlType
                            cmd.CommandText = Sql
                            Dim iRes As Integer = cmd.ExecuteNonQuery
                            Return CType(iRes, SQLiteErrorCode)
                        Else
                            WriteLog("Can't execute sql against a closed connection", eSeverity.Important)
                            Return SQLiteErrorCode.CantOpen
                        End If
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return SQLiteErrorCode.Error
                    End Try
                End Get
            End Property
            Friend ReadOnly Property ExecuteCommand(ByVal Command As SQLiteCommand) As SQLiteErrorCode
                Get
                    Command.Connection = con
                    Return CType(cmd.ExecuteNonQuery, SQLiteErrorCode)
                End Get
            End Property
            Friend ReadOnly Property dataReader(ByVal Sql As String) As SQLiteDataReader
                Get
                    Try
                        If Not IsNothing(cmd.Transaction) Then
                            WriteDebug("check")
                        ElseIf con.State = ConnectionState.Closed Then
                            If con.ConnectionString <> "" Then
                                Try
                                    con.Open()
                                    GoTo IsOpen
                                Catch sqlEx As Exception
                                    'New instance?
                                End Try
                            End If
                        ElseIf con.State = ConnectionState.Open Then
IsOpen:
                            Try
                                cmd = con.CreateCommand
                                GoTo SetCMD
                            Catch sqlEx As Exception
                                'New instance?
                            End Try
                        End If
SetCMD:
                        cmd.CommandText = Sql
                        Try
                            Return cmd.ExecuteReader
                        Catch sqlEx As SQLiteException
                            sqlException = sqlEx.ErrorCode
                            Return Nothing
                        End Try
                    Catch Ex As Exception
                        Return Nothing
                    End Try
                End Get
            End Property
#End Region
#Region "connection state handling"
            Private Sub con_Commit(ByVal sender As Object, ByVal e As System.Data.SQLite.CommitEventArgs) Handles con.Commit
                WriteDebug("sqliteconnection " & ID.ToString & " commited a transaction - " & IsNothing(cmd.Transaction).ToString)
            End Sub
            Private Sub con_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles con.Disposed
                WriteDebug("sqliteconnection " & ID.ToString & " has been disposed")
                b_conDisposed = True
            End Sub
            Private Sub con_RollBack(ByVal sender As Object, ByVal e As System.EventArgs) Handles con.RollBack
                WriteDebug("sqliteconnection " & ID.ToString & " rolled back a transaction")
            End Sub
            Private Sub con_Update(ByVal sender As Object, ByVal e As System.Data.SQLite.UpdateEventArgs) Handles con.Update
                WriteDebug("sqliteconnection " & ID.ToString & " performed " & e.Event.ToString & " " & e.Table.ToString & " row " & e.RowId.ToString)
            End Sub
            Private Sub con_StateChange(ByVal sender As Object, ByVal e As System.Data.StateChangeEventArgs) Handles con.StateChange
                WriteDebug("sqliteconnection " & ID.ToString & " state changed from " & e.OriginalState.ToString & " to " & e.CurrentState.ToString)
                RaiseEvent ConnectionStateChange(ID, e.OriginalState, e.CurrentState)
            End Sub
            Friend Sub TryOpenCon(Optional ByVal tsWait As TimeSpan = Nothing, Optional ByVal _con As SQLiteConnection = Nothing)
                Try
                    If Not IsNothing(con) Then
                        WriteLog("Passed on  a cloned sql connection " & con.ConnectionString & " " & con.State.ToString)
                        'con = _con
                    Else
                        con = New SQLiteConnection
                    End If
                    Dim tmpTS As TimeSpan = TimeOut
                    If IsNothing(tsWait) Then
                        WriteDebug("tryopencon " & ID.ToString & " called, dtNow=" & dtNow.ToString("s") & " TimeOut=" & TimeOut.ToString)
                    Else
                        WriteDebug("tryopencon " & ID.ToString & " called, dtNow=" & dtNow.ToString("s") & " TimeOut=" & TimeOut.ToString & " tsWait=" & tsWait.ToString)
                    End If
                    If b_conDisposed Then
                        WriteLog("Con id:" & ID.ToString & " was disposed, creating new one")
                        CreateCon()
                        GoTo TryOpen
                    ElseIf con.State = ConnectionState.Open Then
                        WriteLog("sqliteconnection id:" & ID.ToString & " is already open", eSeverity.Debug)
                        Exit Sub
                    ElseIf con.State = ConnectionState.Broken Then
                        TryCloseCon()
                        If con.State = ConnectionState.Broken Then
                            'try to dispose and recreate
                            dtNow = DateTime.Now
                            If Not IsNothing(tsWait) Then
                                TimeOut = tsWait
                            End If
                            If ID = 0 And Not bAllowDispose Then bAllowDispose = True
                            con.Dispose()

                            While Not DateTime.Now.Subtract(dtNow) > TimeOut
                                Application.DoEvents()
                                If IsNothing(con) Or b_conDisposed Then
                                    WriteDebug("Con disposed")
                                    Exit While
                                End If
                            End While
                            dtNow = #1/1/2000#
                            If Not IsNothing(tsWait) Then
                                TimeOut = tmpTS
                            End If
                            con = New SQLiteConnection(strConnection)
                            GoTo TryOpen
                        ElseIf con.State = ConnectionState.Closed Then
                            GoTo TryOpen
                        End If
                    Else
TryOpen:

                        dtNow = DateTime.Now
                        If Not IsNothing(tsWait) Then
                            TimeOut = tsWait
                        End If
                        con.ConnectionString = strConnection
                        con.Open()
                        While Not DateTime.Now.Subtract(dtNow) > TimeOut
                            Application.DoEvents()
                            If con.State = ConnectionState.Open Then
                                WriteDebug("Con opened")
                                Exit While
                            End If
                        End While
                        dtNow = #1/1/2000#
                        If Not IsNothing(tsWait) Then
                            TimeOut = tmpTS
                        End If
                        If con.State = ConnectionState.Open Then
                            WriteLog("Created sqlitecommand for instance " & ID.ToString)
                            cmd = con.CreateCommand
                        Else
                            WriteLog("Can't open the connection for id:" & ID.ToString & " " & con.State.ToString, eSeverity.Debug)
                        End If
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    RaiseEvent HandleExc(ex, Err, ID, "tryopencon-" & con.ConnectionString & "-" & con.State.ToString)
                End Try
            End Sub
            Friend Sub TryCloseCon(Optional ByVal tsWait As TimeSpan = Nothing)
                Try
                    If IsNothing(tsWait) Then
                        WriteDebug("tryclosecon " & ID.ToString & " called, dtNow=" & dtNow.ToString("s") & " TimeOut=" & TimeOut.ToString & " " & con.State.ToString)
                    Else
                        WriteDebug("tryclosecon " & ID.ToString & " called, dtNow=" & dtNow.ToString("s") & " TimeOut=" & TimeOut.ToString & " tsWait=" & tsWait.ToString & " " & con.State.ToString)
                    End If
                    If con.State <> ConnectionState.Open Then
                        WriteLog("connection was not open, aborting close")
                        Exit Sub
                    End If
                    con.Close()
                    dtNow = DateTime.Now
                    Dim tmpTS As TimeSpan = TimeOut
                    If Not IsNothing(tsWait) Then
                        TimeOut = tsWait
                    End If
                    While Not DateTime.Now.Subtract(dtNow) > TimeOut
                        Application.DoEvents()
                        If con.State = ConnectionState.Closed Then
                            WriteDebug("Connection closed")
                            Exit While
                        End If
                    End While
                    dtNow = #1/1/2000#
                    If Not IsNothing(tsWait) Then
                        TimeOut = tmpTS
                    End If
                    If Not con.State = ConnectionState.Closed Then
                        WriteLog("Can't close an sqliteconnection: " & strConnection & " - " & con.State.ToString, eSeverity.Critical)
                    End If
                    tmpTS = Nothing
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    RaiseEvent HandleExc(ex, Err, ID, "tryclosecon-" & con.ConnectionString & "-" & con.State.ToString)
                    WriteError(ex.Message, Err)
                End Try
            End Sub
            Private Sub CreateCon()
                Try
                    WriteDebug("Creating connection for " & ID.ToString & " - " & strConnection)
                    con = New SQLiteConnection(strConnection)
                    b_conDisposed = False
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            End Sub
#End Region
#Region "Transaction handling"
            Private bTransAction As Boolean = False
            Friend ReadOnly Property TransactionInProgress As Boolean
                Get
                    Try
                        If IsNothing(trans) Then Return False
                        Return Not IsNothing(trans.Connection)
                    Catch ex As Exception
                        Return False
                    End Try
                End Get
            End Property
            Friend Sub BeginTransaction()
                Try
                    If TransactionInProgress Then
                        WriteDebug("transaction " & ID.ToString & " already running")
                    Else
                        WriteDebug("createtransaction:" & ID.ToString)
                        trans = con.BeginTransaction()
                        bTransAction = True
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            End Sub
            Friend Sub EndTransaction()
                Try
                    WriteDebug("endtransaction:" & ID.ToString)
                    If IsNothing(trans.Connection) Then
                        WriteDebug("transaction " & ID.ToString & " doesn't exist")
                    ElseIf Not bTransAction Then
                        WriteDebug("transaction " & ID.ToString & " doesn't exist")
                    Else
                        Try
                            trans.Commit()
                            bTransAction = Not IsNothing(trans.Connection)
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            Logwindow.ShowDebugWindow(My.Resources.iWarning, True)
                        End Try
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
            End Sub
#End Region
#Region "sqlitecommand handling"
            Private Sub cmd_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmd.Disposed
                b_cmdDisposed = True
                RaiseEvent cmdDisposed(ID)
            End Sub
#End Region
#Region "Instance handling"
            ' sqlitecom property for external access
            Friend Function CreateInstance(ByVal ID As Int32, Optional ByVal strCon As String = Nothing, Optional ByVal tsTimeOut As TimeSpan = Nothing) As Boolean
                Dim bRet As Boolean = False
                Try
                    i_id = ID
                    If Not IsNothing(tsTimeOut) AndAlso tsTimeOut.TotalMilliseconds > 0 Then TimeOut = tsTimeOut
                    If Not IsNothing(strCon) Then strConnection = strCon
                    If IsNothing(strConnection) Or strConnection = String.Empty Then
                        strConnection = Data.dbPool.connectionString
                    End If
                    'CreateCon()
                    WriteDebug("Creating connection for " & ID.ToString & " - " & strConnection)
                    con = New SQLiteConnection(strConnection)
                    bRet = True
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                End Try
                Return bRet
            End Function
            Friend Sub ClearError()
                sqlException = 0
            End Sub
            Friend Sub New(Optional ByVal ID As Int32 = -1)
                i_id = ID
            End Sub
            Public Sub Dispose() Implements System.IDisposable.Dispose
                If ID = 0 And Not bAllowDispose Then
                    WriteDebug("Preventing dispose of dbMaster")
                    Exit Sub
                End If
                WriteDebug("dbInstance:" & ID.ToString & " disposing")
                Me.Finalize()
            End Sub
            Protected Overrides Sub Finalize()
                RaiseEvent IsDisposed(ID)
                WriteDebug("dbInstance:" & ID.ToString & " finalizing")
                MyBase.Finalize()
            End Sub
#End Region
        End Class
#End Region
#Region "Query instance creation"
        Friend Function GetFreeID() As Int32
            Dim iRet As Int32 = -1
            Try
                For xInt As Int32 = 0 To Int32.MaxValue
                    If Not dInstances.ContainsKey(xInt) Then
                        iRet = xInt
                        Exit For
                    End If
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return iRet
        End Function
        Friend ReadOnly Property dbInstance(Optional ByVal ID As Int32 = 0, Optional ByVal conString As String = Nothing, Optional ByVal tsTimeOut As TimeSpan = Nothing) As clsDBInstance
            Get
                Try
                    If ID = 0 Then
                        If Not Master.TransactionInProgress Then
                            Return Master
                        Else
                            WriteLog("Main database instance called which was busy with a transaction")
                            ID = -1
                            GoTo ForceNew
                        End If
                        Return Master
                    ElseIf ID = -1 Then
ForceNew:
                        Dim nInstance As New clsDBInstance()
                        AddHandler nInstance.IsDisposed, AddressOf FreeInstance
                        If IsNothing(conString) Then conString = connectionString
                        If IsNothing(tsTimeOut) Then TimeOut = tsTimeOut
                        If nInstance.CreateInstance(GetFreeID, conString, tsTimeOut) Then
                            dInstances.Add(nInstance.ID, nInstance)
                            WriteDebug("Created dbInstance " & ID.ToString)
                            Return nInstance
                        Else
                            WriteDebug("Instance failed to initialize")
                            Return Nothing
                        End If
                    Else
                        If dInstances.ContainsKey(ID) Then
                            Return dInstances(ID)
                        Else
                            WriteDebug("error")
                            Return Nothing
                        End If
                    End If
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    Return Nothing
                End Try
            End Get
        End Property
#End Region
#Region "Instance event handlers"
        Private Sub _cmdDisposed(ByVal ID As Object)
            WriteDebug("dbInstance " & ID.ToString & " disposed the sqlitecommand")
        End Sub
        Private Sub _conDisposed(ByVal ID As Object)
            WriteDebug("dbInstance " & ID.ToString & " disposed the sqliteconnection")
        End Sub
        Private Sub _ConnectionStateChange(ByVal ID As Integer, ByVal OldState As System.Data.ConnectionState, ByVal NewState As System.Data.ConnectionState)
            WriteDebug("dbInstance " & ID.ToString & " changed state from " & OldState.ToString & " to " & NewState.ToString)
        End Sub
        Private Sub _HandleExc(ByVal RaisedExeption As System.Exception, ByVal TheErr As Microsoft.VisualBasic.ErrObject, ByVal ID As Integer, ByVal exInfo As String)
            WriteError("dbInstance " & ID.ToString & " :" & RaisedExeption.Message, Err)
        End Sub
        Private Sub FreeInstance(ByVal ID As Int32)
            Try
                WriteDebug("dbInstance:" & ID.ToString & " has been disposed, removing from dictionary")
                dInstances.Remove(ID)
                WriteDebug("-" & dInstances.Count & " - " & dInstances.ContainsKey(ID))
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
#End Region
    End Class
    Friend Shared dbPool As New clsDB
#End Region
#Region "Initialize database access / convert older databases"
    Friend Function Init(ByVal DataRoot As String) As Boolean
        Try
            If DataRoot = "" Then
                DataRoot = Application.StartupPath & "\Database\"
            Else
                If Not DataRoot.EndsWith("\") Then
                    mDataRoot = DataRoot & "\"
                Else
                    mDataRoot = DataRoot
                End If
            End If
            'Check for backup db, application exit should remove it, offer chanche of restoring
            If My.Computer.FileSystem.FileExists(dbBackup) Then
                Dim rVal As MsgBoxResult = MsgBox("A backup database file is present, do you want to restore it?", CType(MsgBoxStyle.OkCancel + MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxSetForeground, MsgBoxStyle), "Restore backup?")
                If rVal = MsgBoxResult.Ok Then
                    Try
                        My.Computer.FileSystem.DeleteFile(dbFile)
                    Catch ex As Exception : End Try
                    Try
                        My.Computer.FileSystem.CopyFile(dbBackup, dbFile)
                    Catch ex As Exception : End Try
                End If
            Else
                If My.Computer.FileSystem.FileExists(dbFile) Then
                    Try
                        My.Computer.FileSystem.CopyFile(dbFile, dbBackup)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                    End Try
                End If
            End If
            dbPool.Init(dbFile, "Compress=True;Synchronous=off;locking_mode=EXCLUSIVE", New TimeSpan(0, 0, 5))
            Dim bRes As Boolean
            Try
                Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        bRes = True
                    Else
                        WriteLog("Can't open the database specified, " & dbFile & " - " & strCon & " - " & CStr(.LastException), eSeverity.Critical)
                        bRes = False
                    End If
                End With
                If bRes Then
                    Return CheckAndConvert()
                Else
                    Return bRes
                End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        Catch ex As Exception
            WriteDebug("data.init - failed to init dbPool")
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Function CheckAndConvert() As Boolean
        Try
            'Just TPFmin/TPFmax for now... :(
            Dim bAddMinMaxTPF As Boolean = False, bConvertTPF As Boolean = False
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHasTable As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='WorkUnits'")
                            If IsNothing(rdr) Then
                                Return False
                            Else
                                bHasTable = rdr.HasRows
                            End If
                        End Using
                        If bHasTable Then
                            Dim tableFields As List(Of String) = .GetTableFields("WorkUnits")
                            If Not tableFields.Contains("TPFmin") OrElse Not tableFields.Contains("TPFmax") Then
                                WriteLog("Need to convert the database!", eSeverity.Critical)
                                bAddMinMaxTPF = True
                            End If
                            If tableFields.Contains("TPF") Then
                                Dim strField As String = ""
                                Using rdr As SQLiteDataReader = .dataReader("SELECT MAX(TPF) FROM WorkUnits")
                                    If Not IsNothing(rdr) Then
                                        If rdr.HasRows Then strField = CStr(rdr.Item(0))
                                    End If
                                End Using
                                If strField <> "" Then
                                    If Not IsNumeric(strField) Then
                                        bConvertTPF = True
                                    End If
                                End If
                            End If
                        End If
                    Else
                        WriteLog("The database can't be accessed", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
            If bAddMinMaxTPF Then
                Return ConvertWorkUnitTable()
            Else
                Return True
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Function ConvertWorkUnitTable() As Boolean
        Try
            'Load wu's in list
            WriteLog("Reading work unit history prior to conversion", eSeverity.Important)
            BussyBox.ShowForm("Converting WorkUnit table" & Environment.NewLine & Environment.NewLine & "Loading data..", True, Nothing, False)
            Dim lWU As List(Of clsWU) = WorkUnits("", False, True)
            WriteLog("Removing old table", eSeverity.Important)
            BussyBox.SetMessage("Converting WorkUnit table" & Environment.NewLine & Environment.NewLine & "Creating new table and saving")
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        sqlErr = .ExecuteNonQuery("DROP TABLE WorkUnits", CommandType.Text)
                    Else
                        WriteLog("The database can't be accessed", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
            WriteLog("Saving history in new table", eSeverity.Important)
            SaveWorkUnit(lWU, True, True)
            Return dbPool.dbInstance.GetTableFields("WorkUnits").Contains("TPFmin")
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        Finally
            If BussyBox.IsFormVisible Then BussyBox.CloseForm()
        End Try
    End Function
    Friend Sub PurgeDB()
        Try
            Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
            With dbInst
                If .conState = ConnectionState.Open Then
                    .BeginTransaction()
                    WriteDebug("Purging database leaving only project information")
                    Dim lNames As New List(Of String)
                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE NOT name like '%ProjectInfo%'")
                        While rdr.Read
                            lNames.Add(CStr(rdr.Item(0)))
                        End While
                    End Using
                    For Each Table In lNames
                        'cmd = sqlitecon.createcommand
                        sqlErr = .ExecuteNonQuery("DROP TABLE " & Table)
                    Next
                    'VACUUM DB
                    .EndTransaction()
                    If .TransactionInProgress Then
                        WriteDebug("Database vacuum")
                        sqlErr = .ExecuteNonQuery("VACUUM")
                    End If
                Else
                    WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                End If
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Project info"
    Friend ReadOnly Property HasProjectInfo As Boolean
        Get
            Dim bRet As Boolean = False
            Try
                Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ProjectInfo'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            'close cmd/sqlitecon
                            Exit Try
                        End If
                        Try
                            Using rdr As SQLiteDataReader = .dataReader("SELECT COUNT(1) FROM 'ProjectInfo'")
                                If rdr.Read Then
                                    'Ignore for now
                                    bRet = CInt(rdr.Item(0)) > 0
                                End If
                            End Using
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            bRet = False
                        End Try
                    Else
                        WriteLog("Can't access the database table, there should be more errors")
                    End If
                End With
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return bRet
        End Get
    End Property

    Friend Sub Update_ProjectInfo()
        Try
            Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
            With dbInst
                Dim bHas As Boolean
                Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ProjectInfo'")
                    bHas = rdr.HasRows
                End Using
                If Not bHas Then
                    WriteLog("Creating projectinfo table")
                    Try
                        sqlErr = .ExecuteNonQuery("CREATE TABLE 'ProjectInfo' (ProjectNumber INT, ServerIP TEXT, WuName TEXT, NumberOfAtoms TEXT, PrefferedDays TEXT, FinalDeadline TEXT, Credit TEXT, Frames TEXT, Code TEXT, Contact TEXT, kFactor TEXT, Description TEXT, dtSummary DATETIME)")
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        Return
                    End Try
                End If
                For Each Project As pSummary In FAHWatch7.ProjectInfo.pSummaryList
                    Dim dtComp As DateTime = DateTime.MinValue
                    Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ProjectInfo' WHERE ProjectNumber='" & Project.ProjectNumber & "'")
                        If Not IsNothing(rdr) Then
                            bHas = rdr.HasRows
                            Try
                                If Not IsDBNull(rdr.Item("dtSummary")) Then
                                    dtComp = CDate(rdr.Item("dtSummary"))
                                End If
                            Catch ex As Exception
                                dtComp = DateTime.Now
                            End Try
                        Else
                            dtComp = DateTime.Now
                            bHas = False
                        End If
                    End Using
                    'Check if new entries overwrite older one's
                    If bHas And dtComp < Project.dtSummary Then
                        WriteDebug("Updating project information for #" & Project.ProjectNumber)
                        sqlErr = .ExecuteNonQuery("UPDATE 'ProjectInfo' SET ServerIP='" & Project.ServerIP & "', WuName='" & Project.WUName & "', NumberOfAtoms='" & Project.NumberOfAtoms & "', PrefferedDays='" & Project.PreferredDays & "', FinalDeadline='" & Project.FinalDeadline & "', Credit='" & Project.Credit & "', Frames='" & Project.Frames & "', Code='" & Project.Code & "', Contact='" & Project.Contact & "', kFactor='" & Project.kFactor & "', Description='" & Project.Description & "', dtSummary='" & Project.dtSummary.ToString("s") & "' WHERE ProjectNumber='" & CInt(Project.ProjectNumber) & "'")
                    ElseIf Not bHas Then
                        WriteDebug("Adding project information for #" & Project.ProjectNumber)
                        sqlErr = .ExecuteNonQuery("INSERT INTO 'ProjectInfo' (ProjectNumber, ServerIP, WuName, NumberOfAtoms, PrefferedDays, FinalDeadline, Credit, Frames, Code, Contact, kFactor, Description, dtSummary) VALUES ('" & CInt(Project.ProjectNumber) & "','" & Project.ServerIP & "','" & Project.WUName & "','" & Project.NumberOfAtoms & "','" & Project.PreferredDays & "','" & Project.FinalDeadline & "','" & Project.Credit & "','" & Project.Frames & "','" & Project.Code & "','" & Project.Contact & "','" & Project.kFactor & "','" & Project.Description & "','" & Project.dtSummary.ToString("s") & "')")
                    End If
                Next
            End With
            WriteDebug("Update_ProjectInfo finished")
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Function ReadProjectInfo() As Boolean
        Dim bRes As Boolean = False
        Try
            If Not HasProjectInfo Then Exit Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ProjectInfo'")
                            If IsNothing(rdr) Then Exit Try
                            While rdr.Read
                                Dim nPr As New pSummary
                                With nPr
                                    .ProjectNumber = CStr(rdr.Item("ProjectNumber"))
                                    Try
                                        .dtSummary = CDate(rdr.Item("dtSummary"))
                                    Catch ex As Exception
                                        .dtSummary = DateTime.Now
                                    End Try
                                    .Code = rdr.Item("Code").ToString '0 
                                    .Contact = rdr.Item("Contact").ToString '1
                                    .Credit = rdr.Item("Credit").ToString '2
                                    .Description = rdr.Item("Description").ToString '3
                                    If .Description.ToUpperInvariant.Substring(0, 4) = "HTTP" Then
                                        'WriteDebug("Checking with FAHControl.db for project #" & .ProjectNumber & " description: " & GetFAHControlProjectDescription(.ProjectNumber).ToString)
                                    End If
                                    .FinalDeadline = rdr.Item("FinalDeadline").ToString '4
                                    .Frames = rdr.Item("Frames").ToString '5
                                    .kFactor = rdr.Item("kFactor").ToString '6
                                    .NumberOfAtoms = rdr.Item("NumberOfAtoms").ToString '7
                                    .PreferredDays = rdr.Item("PrefferedDays").ToString
                                    .ServerIP = rdr.Item("ServerIP").ToString
                                    .WUName = rdr.Item("WuName").ToString
                                End With
                                FAHWatch7.ProjectInfo.AddProject(nPr)
                            End While
                            WriteDebug("Read " & ProjectInfo.pSummaryList.Count & " project summaries from the database")
                        End Using
                        bRes = True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return bRes
    End Function
#Region "FAHControl.db project description retrieval"
    Friend Function GetFAHControlProjectDescriptions(Optional Location As String = vbNullString) As Boolean
        Dim bRet As Boolean = True, bSucces As Boolean = False
        Try
            'Should use transaction but this should work
Retry:
            Dim fCopy As String = modLinkDemand.EnvironmentSetting("TMP") & "\fw7_"
            If IsNothing(fCopy) Then
                WriteLog("The application was not able to access environment settings", eSeverity.Important)
                Return False
            ElseIf fCopy = String.Empty Then
                WriteLog("The application was not able to access environment settings", eSeverity.Important)
                Return False
            End If
            fCopy &= System.IO.Path.GetRandomFileName & ".db"
            If My.Computer.FileSystem.FileExists(fCopy) Then
                Try
                    My.Computer.FileSystem.DeleteFile(fCopy, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                Catch ex As Exception
                    GoTo Retry
                End Try
            End If
            If IsNothing(Location) Then
                If Not My.Computer.FileSystem.FileExists(ClientConfig.Configuration.DataLocation & "\FAHControl.db") Then Return False
                My.Computer.FileSystem.CopyFile(ClientConfig.Configuration.DataLocation & "\FAHControl.db", fCopy)
            Else
                If Location.EndsWith("\") Then Location = Location.Substring(0, Location.Length - 1)
                If Not My.Computer.FileSystem.FileExists(Location & "\FAHControl.db") Then Return False
                My.Computer.FileSystem.CopyFile(Location & "\FAHControl.db", fCopy)
            End If
            lFilesToRemove.Add(fCopy)
            Dim dicDescriptions As New Dictionary(Of String, String)
            Using dbInstFC As clsDB.clsDBInstance = dbPool.dbInstance(-1, "Data source=" & fCopy & ";mode=ro")
                With dbInstFC
                    .TryOpenCon()
                    Application.DoEvents()
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM projects")
                            If IsNothing(rdr) Then
                                WriteLog("You're running a FAHClient version which has no 'projects' table, import failed", eSeverity.Informative)
                                Return False
                            Else
                                While rdr.Read
                                    If Not CStr(rdr.Item(0)) = "0" Then dicDescriptions.Add(CStr(rdr.Item(0)), CStr(rdr.Item(1)))
                                End While
                            End If
                        End Using
                        If dicDescriptions.Count = 0 Then Exit Try
                    Else
                        WriteLog("Can't access the FAHControl database, there could be more errors", eSeverity.Debug)
                        Exit Try
                    End If
                End With
            End Using
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        For Each DictionaryEntry In dicDescriptions
                            Using rdr As SQLiteDataReader = .dataReader("SELECT Description FROM ProjectInfo WHERE ProjectNumber='" & DictionaryEntry.Key & "' AND NOT Description='" & FormatSQLString(DictionaryEntry.Value, True) & "'")
                                If Not IsNothing(rdr) Then
                                    If rdr.HasRows Then
                                        Try
                                            sqlErr = .ExecuteNonQuery("UPDATE ProjectInfo SET Description='" & FormatSQLString(DictionaryEntry.Value) & "' WHERE ProjectNumber='" & DictionaryEntry.Key & "'")
                                            FAHWatch7.ProjectInfo.Project(DictionaryEntry.Key).Description = DictionaryEntry.Value
                                            bSucces = True
                                        Catch exSql As SQLiteException
                                            WriteLog("Updating project info description failed with the following error:", eSeverity.Important)
                                            WriteError(exSql.Message, Err)
                                            bSucces = False
                                        End Try
                                    End If
                                End If
                            End Using
                        Next
                    Else
                        WriteLog("Database writing failed", eSeverity.Debug)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet AndAlso bSucces
    End Function
    Friend Function GetFAHControlProjectDescription(ByVal ProjectNumber As String) As Boolean
        Dim bRet As Boolean = False
        Try
Retry:
            Dim fCopy As String = modLinkDemand.EnvironmentSetting("TMP") & "\fw7_"
            fCopy &= System.IO.Path.GetRandomFileName & ".db"
            If My.Computer.FileSystem.FileExists(fCopy) Then
                Try
                    My.Computer.FileSystem.DeleteFile(fCopy, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
                Catch ex As Exception
                    GoTo Retry
                End Try
            End If
            My.Computer.FileSystem.CopyFile(ClientConfig.Configuration.ClientLocation & "\FAHControl.db", fCopy)
            If Not My.Computer.FileSystem.FileExists(fCopy) Then Exit Try
            Dim Description As String = String.Empty
            Using dbInstFC As clsDB.clsDBInstance = dbPool.dbInstance(-1, "Data source=" & fCopy & ";mode=ro")
                With dbInstFC
                    .TryOpenCon()
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sql_master WHERE name=projects")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            WriteDebug("FAHControl has no record of this project")
                            Exit Try
                        End If
                        Using rdr As SQLiteDataReader = .dataReader("SELECT description FROM projects WHERE id='" & ProjectNumber & "'")
                            If rdr.HasRows Then
                                Description = CStr(rdr.Item(0))
                            End If
                        End Using
                    Else
                        WriteLog("Sqlite connection failed for FAHControl.db", eSeverity.Debug)
                    End If
                End With
            End Using

            If Not Description = String.Empty Then
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Try
                                sqlErr = .ExecuteNonQuery("UPDATE ProjectInfo SET Description='" & Description & "' WHERE ProjectNumber='" & CInt(ProjectNumber) & "'")
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                            End Try
                            bRet = True
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Debug)
                        End If
                    End With
                End Using
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return bRet
    End Function
#End Region
#End Region
#Region "Unknown projects"
    Friend ReadOnly Property UnknownProject(ProjectNumber As String) As Boolean
        Get
            Try
                Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='UnknownProjects'")
                            If Not IsNothing(rdr) Then
                                bHas = rdr.HasRows
                            Else
                                bHas = False
                            End If
                        End Using
                        If Not bHas Then
                            WriteLog("Creating projectinfo table")
                            Try
                                sqlErr = .ExecuteNonQuery("CREATE TABLE 'UnknownProjects' (ProjectNumber TEXT)")
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                            End Try
                            Return False
                        Else
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'UnknownProjects' WHERE ProjectNumber ='" & ProjectNumber & "'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    bHas = False
                                End If
                            End Using
                            Return bHas
                        End If
                    Else
                        WriteLog("Can't access the database table, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Function InsertUnknownProject(ProjectNumber As String) As Boolean
        If UnknownProject(ProjectNumber) Then Return True
        Try
            Dim bHas As Boolean
            Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
            With dbInst
                If .conState = ConnectionState.Open Then
                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='UnknownProjects'")
                        If Not IsNothing(rdr) Then
                            bHas = rdr.HasRows
                        Else
                            bHas = False
                        End If
                    End Using
                    If Not bHas Then
                        WriteLog("Creating projectinfo table")
                        Try
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'UnknownProjects' (ProjectNumber TEXT)")
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                        Return False
                    Else
                        Try
                            Dim sqlStr As String = "INSERT INTO 'UnknownProjects' (ProjectNumber) VALUES('" & ProjectNumber & "')"
                            sqlErr = .ExecuteNonQuery(sqlStr)
                            Return True
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            Return False
                        End Try
                    End If
                Else
                    WriteLog("Can't access the database table, there should be more errors", eSeverity.Critical)
                    Return False
                End If
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Friend ReadOnly Property UnknownProjects As List(Of String)
        Get
            Dim rVal As New List(Of String)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name = 'UnknownProjects'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    bHas = False
                                    WriteLog("Can't query against sqlite_master, failure is imminent!", eSeverity.Critical)
                                End If
                            End Using
                            If Not bHas Then
                                WriteDebug("No table is created for UnknownProjects yet, returning empty list")
                                Exit Try
                            End If
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'UnknownProjects'")
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        rVal.Add(CStr(rdr.Item(0)))
                                    End While
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Exit Try
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                            Exit Try
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
    Friend Function RemoveUnknownProject(ProjectNumber As String) As Boolean
        If Not UnknownProject(ProjectNumber) Then Return True
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Try
                            Dim sqlStr As String = "DELETE FROM 'UnknownProjects' WHERE ProjectNumber='" & ProjectNumber & "'"
                            sqlErr = .ExecuteNonQuery(sqlStr)
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            Return False
                        End Try
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'UnkownProjects' WHERE ProjectNumber='" & ProjectNumber & "'")
                            If Not IsNothing(rdr) Then
                                Return Not rdr.HasRows
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Return False
                            End If
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "SQL queries"
    Friend ReadOnly Property InitSQLFilters As Boolean
        Get
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bhas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Filters'")
                                bhas = rdr.HasRows
                            End Using
                            If Not bhas Then
                                sqlErr = .ExecuteNonQuery("CREATE TABLE Filters (name TEXT, sql TEXT)")
                                sqlErr = .ExecuteNonQuery("INSERT INTO Filters (name, sql) VALUES('EUE','" & FormatSQLString(" WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND NOT CoreStatus LIKE '%dumped%' ORDER BY Downloaded DESC") & "')")
                                sqlErr = .ExecuteNonQuery("INSERT INTO Filters (name, sql) VALUES('dumped','" & FormatSQLString(" WHERE CoreStatus LIKE '%dumped%' ORDER BY Downloaded DESC") & "')")
                                sqlErr = .ExecuteNonQuery("INSERT INTO Filters (name, sql) VALUES('Not submitted','" & FormatSQLString(" WHERE CoreStatus LIKE '%FINISHED_UNIT%' AND ServerResponse='' ORDER BY Completed DESC") & "')")
                                sqlErr = .ExecuteNonQuery("INSERT INTO Filters (name, sql) VALUES('Submitted','" & FormatSQLString(" WHERE NOT ServerResponse = '' ORDER BY Completed ASC") & "')")
                                GoTo ReadFilters
                            Else
ReadFilters:
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM Filters")
                                    While rdr.Read
                                        sqlFilters.AddFilter(CStr(rdr.Item("name")), FormatSQLString(CStr(rdr.Item("sql")), True))
                                    End While
                                End Using
                            End If
                            If sqlFilters.FilterNames.Contains("EUE") And sqlFilters.FilterNames.Contains("dumped") Then
                                Return True
                            Else
                                Return False
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                            Return False
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Function StoreFilters() As Boolean
        Dim bRet As Boolean = False
        Try
            'Check
            Dim dbInst As clsDB.clsDBInstance = dbPool.dbInstance
            With dbInst
                If .conState = ConnectionState.Open Then
                    Dim bhas As Boolean
                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Filters'")
                        bhas = rdr.HasRows
                    End Using
                    If Not bhas Then
                        sqlErr = .ExecuteNonQuery("CREATE TABLE Filters (name TEXT, sql TEXT)")
                    End If
                    For Each sqlFilter In sqlFilters.Filters
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM Filters WHERE name='" & sqlFilter.Key & "'")
                            bhas = rdr.HasRows
                        End Using
                        If bhas Then
                            sqlErr = .ExecuteNonQuery("UPDATE Filters SET sql='" & FormatSQLString(sqlFilter.Value) & "' WHERE name='" & sqlFilter.Key & "'")
                        Else
                            sqlErr = .ExecuteNonQuery("INSERT INTO Filters (name, sql) VALUES('" & sqlFilter.Key & "','" & FormatSQLString(sqlFilter.Value) & "')")
                        End If
                    Next
                    bRet = True
                Else
                    WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                End If
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return bRet
    End Function
#End Region
#Region "Statistics"
    Friend Function PerformanceStatistics(Optional ByVal startDate As DateTime = #1/1/2000#, Optional ByVal EndDate As DateTime = #1/1/2000#, Optional ByVal Limit As Boolean = False) As clsStatistics.clsPerformanceStatistics
        Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
        WriteLog("Starting Hardware statistics generation")
        Dim rVal As New clsStatistics.clsPerformanceStatistics
        'SELECT client, slot, hw, TOTAL(credit) as 'total_credit', TOTAL(1) AS 'total_workunits', AVG(ppd) as 'Avg_ppd' FROM 'WorkUnits' GROUP BY client, slot, hw 
        Try
            For Each Client As Client In Clients.Clients
                Dim nClient As New clsStatistics.clsPerformanceStatistics.sClient
                nClient.Name = Client.ClientName
                rVal.Clients.Add(nClient.Name, nClient)
                Dim iWuCompleted As Int32 = 0
                Dim iWuEUE As Int32 = 0
                Dim iTotalCredit As Int32 = 0
                Dim tsComputation As New TimeSpan(0)
                'Do slots, combine for client
                If Client.Slots.Count > 0 Then
                    For Each Slot As Client.clsSlot In Client.Slots
                        Dim sSlot As New clsStatistics.clsPerformanceStatistics.sClient.sSlot
                        sSlot.ID = Slot.Index
                        If Slot.Type = "gpu" Then
                            sSlot.Hardware = Slot.Hardware
                        Else
                            sSlot.Hardware = Slot.Type
                        End If
                        rVal.Clients(Client.ClientName).Slots.Add(sSlot.ID, sSlot)
                        Try
                            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                With dbInst
                                    If .conState = ConnectionState.Open Then
                                        Dim sqlQuery As String = ""
                                        If startDate = #1/1/2000# Then
                                            sqlQuery = "SELECT Slot, TOTAL(credit) as 'total_credit', COUNT(1) AS 'workunits', AVG(ppd) as 'avg_ppd', HW FROM 'WorkUnits' WHERE Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' GROUP BY Slot"
                                        Else
                                            sqlQuery = "SELECT Slot, TOTAL(credit) as 'total_credit', COUNT(1) AS 'workunits', AVG(ppd) as 'avg_ppd', HW FROM 'WorkUnits' WHERE Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Downloaded > '" & startDate.ToString("s") & "' AND Submitted < '" & EndDate.ToString("s") & "' GROUP BY Slot"
                                        End If
                                        Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                            If Not IsNothing(rdr) Then
                                                While rdr.Read
                                                    rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed = CStr(rdr.Item("workunits"))
                                                    rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit = CStr(rdr.Item("total_credit"))
                                                    'rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD = FormatPPD(CStr(Math.Round(CDbl(rdr.Item("avg_ppd")), 2)))
                                                End While
                                            Else
                                                WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                bFailure = True
                                                GoTo Skip
                                            End If
                                        End Using
                                        If rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed <> "" Then iWuCompleted += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed)
                                        If rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit <> "" Then iTotalCredit += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit)
                                        'If rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD <> "" Then
                                        '    If dblAveragePPD = 0.0 Then
                                        '        dblAveragePPD = CDbl(rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD)
                                        '    Else
                                        '        dblAveragePPD = dblAveragePPD + CDbl(rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD)
                                        '    End If
                                        'End If
                                    Else
                                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End With
                            End Using
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            bFailure = True
                            Exit Try
                        End Try
                        'Get eue
                        Try
                            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                With dbInst
                                    If .conState = ConnectionState.Open Then
                                        Dim sqlQuery As String = ""
                                        If startDate = #1/1/2000# Then
                                            sqlQuery = "SELECT Slot, COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' GROUP BY Slot"
                                        Else
                                            sqlQuery = "SELECT Slot, COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Downloaded > '" & startDate.ToString("s") & "' AND Submitted < '" & EndDate.ToString("s") & "' GROUP BY Slot"
                                        End If
                                        Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                            If Not IsNothing(rdr) Then
                                                If rdr.Read Then
                                                    rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE = CStr(rdr.Item("eue"))
                                                End If
                                            Else
                                                WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                bFailure = True
                                                GoTo Skip
                                            End If
                                        End Using
                                        If rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE <> "" Then
                                            iWuEUE += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE)
                                        End If
                                    Else
                                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End With
                            End Using
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            bFailure = True
                            Exit Try
                        End Try
                        'Get computation time?
                        Try
                            If Limit Then Exit Try 'Don't include computation time 
                            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                With dbInst
                                    If .conState = ConnectionState.Open Then
                                        Dim tsTotal As New TimeSpan
                                        Dim sqlQuery As String = ""
                                        If startDate = #1/1/2000# Then
                                            sqlQuery = "SELECT StartDownload, Submitted, Started, Completed FROM 'WorkUnits' WHERE client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%'"
                                        Else
                                            sqlQuery = "SELECT Started, Completed FROM 'WorkUnits' WHERE client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Downloaded > '" & startDate.ToString("s") & "' AND Submitted < '" & EndDate.ToString("s") & "'"
                                        End If
                                        Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                            If Not IsNothing(rdr) Then
                                                While rdr.Read
                                                    If CDate(rdr.Item("Submitted")) > rVal.Clients(Client.ClientName).dtUpper Then rVal.Clients(Client.ClientName).dtUpper = CDate(rdr.Item("Submitted"))
                                                    If CDate(rdr.Item("StartDownload")) < rVal.Clients(Client.ClientName).dtLower Then rVal.Clients(Client.ClientName).dtLower = CDate(rdr.Item("StartDownload"))
                                                    If CDate(rdr.Item("Submitted")) > rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtUpper Then rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtUpper = CDate(rdr.Item("Submitted"))
                                                    If CDate(rdr.Item("StartDownload")) < rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtLower Then rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtLower = CDate(rdr.Item("StartDownload"))
                                                    Dim tsAdd As New TimeSpan
                                                    tsAdd = CDate(rdr.Item("Completed")).Subtract(CDate(rdr.Item("Started")))
                                                    tsTotal += tsAdd
                                                End While
                                            Else
                                                WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                bFailure = True
                                                GoTo Skip
                                            End If
                                            rVal.Clients(Client.ClientName).Slots(sSlot.ID).ComputationTime = tsTotal.ToString
                                        End Using
                                        If tsTotal > TimeSpan.FromTicks(0) Then
                                            tsComputation = tsComputation.Add(tsTotal)
                                        End If
                                    Else
                                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End With
                            End Using
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            bFailure = True
                            Exit Try
                        End Try
                    Next
                Else
                    If Not IsNothing(Client.ClientConfig.Configuration.slots) Then
                        For xInt As Int32 = 0 To Client.ClientConfig.Configuration.slots.Count - 1
                            Dim cSlot As clsClientConfig.clsConfiguration.sSlot = Client.ClientConfig.Configuration.slots(xInt)
                            Dim sSlot As New clsStatistics.clsPerformanceStatistics.sClient.sSlot
                            If cSlot.id.Length = 1 Then
                                sSlot.ID = "0" & cSlot.id
                            Else
                                sSlot.ID = cSlot.id
                            End If
                            If cSlot.type = "GPU" Then
                                If cSlot.mArguments.Keys.Contains("gpu-index") Then
                                    sSlot.Hardware = Client.ClientInfo.Info.GPU(CInt(cSlot.mArguments("gpu-index"))).DeviceName
                                Else
                                    sSlot.Hardware = Client.ClientInfo.Info.GPU(0).DeviceName
                                End If
                            Else
                                sSlot.Hardware = cSlot.type
                            End If
                            rVal.Clients(Client.ClientName).Slots.Add(sSlot.ID, sSlot)
                            Try
                                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                    With dbInst
                                        If .conState = ConnectionState.Open Then
                                            Dim sqlQuery As String = ""
                                            If startDate = #1/1/2000# Then
                                                sqlQuery = "SELECT Slot, TOTAL(credit) as 'total_credit', COUNT(1) AS 'workunits', AVG(ppd) as 'avg_ppd', HW FROM 'WorkUnits' WHERE Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' GROUP BY Slot"
                                            Else
                                                sqlQuery = "SELECT Slot, TOTAL(credit) as 'total_credit', COUNT(1) AS 'workunits', AVG(ppd) as 'avg_ppd', HW FROM 'WorkUnits' WHERE Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Completed > '" & startDate.ToString("s") & "' GROUP BY Slot"
                                            End If
                                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                                If Not IsNothing(rdr) Then
                                                    While rdr.Read
                                                        rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed = CStr(rdr.Item("workunits"))
                                                        rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit = CStr(rdr.Item("total_credit"))
                                                        'rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD = FormatPPD(CStr(Math.Round(CDbl(rdr.Item("avg_ppd")), 2)))
                                                    End While

                                                Else
                                                    WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                    bFailure = True
                                                    GoTo Skip
                                                End If
                                            End Using
                                            If rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed <> "" Then iWuCompleted += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_Completed)
                                            If rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit <> "" Then iTotalCredit += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).TotalCredit)
                                            'If rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD <> "" Then
                                            '    If dblAveragePPD = 0.0 Then
                                            '        dblAveragePPD = CDbl(rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD)
                                            '    Else
                                            '        dblAveragePPD = dblAveragePPD + CDbl(rVal.Clients(Client.ClientName).Slots(sSlot.ID).AveragePPD)
                                            '    End If
                                            'End If
                                        Else
                                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End With
                                End Using
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                bFailure = True
                                Exit Try
                            End Try
                            'Get eue
                            Try
                                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                    With dbInst
                                        If .conState = ConnectionState.Open Then
                                            Dim sqlQuery As String = ""
                                            If startDate = #1/1/2000# Then
                                                sqlQuery = "SELECT Slot, COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' GROUP BY Slot"
                                            Else
                                                sqlQuery = "SELECT Slot, COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND Client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Completed > '" & startDate.ToString("s") & "' GROUP BY Slot"
                                            End If
                                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                                If Not IsNothing(rdr) Then
                                                    If rdr.Read Then
                                                        rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE = CStr(rdr.Item("eue"))
                                                    End If
                                                Else
                                                    WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                    bFailure = True
                                                    GoTo Skip
                                                End If
                                            End Using
                                            If rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE <> "" Then
                                                iWuEUE += CInt(rVal.Clients(Client.ClientName).Slots(sSlot.ID).Wu_EUE)
                                            End If
                                        Else
                                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End With
                                End Using
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                bFailure = True
                                Exit Try
                            End Try
                            'Get computation time?
                            Try
                                If Limit Then Exit Try 'Don't include computation time 
                                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                                    With dbInst
                                        If .conState = ConnectionState.Open Then
                                            Dim tsTotal As New TimeSpan
                                            Dim sqlQuery As String = ""
                                            If startDate = #1/1/2000# Then
                                                sqlQuery = "SELECT StartDownload, Submitted, Started, Completed FROM 'WorkUnits' WHERE client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%'"
                                            Else
                                                sqlQuery = "SELECT Started, Completed FROM 'WorkUnits' WHERE client='" & FormatSQLString(Client.ClientName, False, True) & "' AND Slot='" & sSlot.ID & "' AND HW LIKE '" & sSlot.Hardware & "%' AND Completed > '" & startDate.ToString("s") & "'"
                                            End If
                                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                                If Not IsNothing(rdr) Then
                                                    While rdr.Read
                                                        If CDate(rdr.Item("Submitted")) > rVal.Clients(Client.ClientName).dtUpper Then rVal.Clients(Client.ClientName).dtUpper = CDate(rdr.Item("Submitted"))
                                                        If CDate(rdr.Item("StartDownload")) < rVal.Clients(Client.ClientName).dtLower Then rVal.Clients(Client.ClientName).dtLower = CDate(rdr.Item("StartDownload"))
                                                        If CDate(rdr.Item("Submitted")) > rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtUpper Then rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtUpper = CDate(rdr.Item("Submitted"))
                                                        If CDate(rdr.Item("StartDownload")) < rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtLower Then rVal.Clients(Client.ClientName).Slots(sSlot.ID).dtLower = CDate(rdr.Item("StartDownload"))
                                                        Dim tsAdd As New TimeSpan
                                                        tsAdd = CDate(rdr.Item("Completed")).Subtract(CDate(rdr.Item("Started")))
                                                        tsTotal += tsAdd
                                                    End While
                                                Else
                                                    WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                                    bFailure = True
                                                    GoTo Skip
                                                End If
                                                rVal.Clients(Client.ClientName).Slots(sSlot.ID).ComputationTime = tsTotal.ToString
                                            End Using
                                            If tsTotal > TimeSpan.FromTicks(0) Then
                                                tsComputation = tsComputation.Add(tsTotal)
                                            End If
                                        Else
                                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End With
                                End Using
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                bFailure = True
                                Exit Try
                            End Try
                        Next
                    End If
                End If


                'Combine slots
                'rVal.Clients(Client.ClientName).AveragePPD = FormatPPD(CStr(Math.Round(dblAveragePPD, 2)))
                rVal.Clients(Client.ClientName).ComputationTime = tsComputation.ToString
                rVal.Clients(Client.ClientName).TotalCredit = CStr(iTotalCredit)
                rVal.Clients(Client.ClientName).Wu_Completed = CStr(iWuCompleted)
                rVal.Clients(Client.ClientName).Wu_EUE = CStr(iWuEUE)
            Next

            For Each C As clsStatistics.clsPerformanceStatistics.sClient In rVal.Clients.Values.ToList
                If C.dtLower < rVal.dtLower Then rVal.dtLower = C.dtLower
                If C.dtUpper > rVal.dtUpper Then rVal.dtUpper = C.dtUpper
                If Not C.Wu_Completed = "" Then
                    If rVal.Wu_Completed <> "" Then
                        rVal.Wu_Completed = CStr(CInt(rVal.Wu_Completed) + CInt(C.Wu_Completed))
                    Else
                        rVal.Wu_Completed = C.Wu_Completed
                    End If
                End If
                If Not C.Wu_EUE = "" Then
                    If rVal.Wu_EUE <> "" Then
                        rVal.Wu_EUE = CStr(CInt(rVal.Wu_EUE) + CInt(C.Wu_EUE))
                    Else
                        rVal.Wu_EUE = C.Wu_EUE
                    End If
                End If

                If Not C.ComputationTime = "" Then
                    If Not rVal.ComputationTime = "" Then
                        rVal.ComputationTime = TimeSpan.Parse(rVal.ComputationTime).Add(TimeSpan.Parse(C.ComputationTime)).ToString
                    Else
                        rVal.ComputationTime = C.ComputationTime
                    End If
                End If

                If Not C.TotalCredit = "" Then
                    If Not rVal.TotalCredit = "" Then
                        rVal.TotalCredit = CStr(CInt(rVal.TotalCredit) + CInt(C.TotalCredit))
                    Else
                        rVal.TotalCredit = C.TotalCredit
                    End If
                End If
                'Average ppd is not calculated from the total points and total timespan, it's accurate :)
                'If Not C.AveragePPD = "" Then
                '    If Not rVal.AveragePPD = "" Then
                '        rVal.AveragePPD = FormatPPD(CStr(Math.Round(CDbl(rVal.AveragePPD) + CDbl(C.AveragePPD), 2)))
                '    Else
                '        rVal.AveragePPD = FormatPPD(C.AveragePPD)
                '    End If
                'End If
            Next
            rVal.dtStatistics = DateTime.Now
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bFailure = True
        End Try
Skip:
        If Not bFailure Then WriteLog("-generation took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
        Return rVal
        'SELECT client, slot, hw, TOTAL(credit) as 'total_credit', TOTAL(1) AS 'total_workunits', AVG(ppd) as 'Avg_ppd' FROM 'WorkUnits' GROUP BY client, slot, hw 
        'SELECT client, slot, hw, TOTAL(credit) as 'credit', COUNT(1) AS 'workunits' FROM 'WorkUnits', COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT 'CoreStatus' LIKE '%FINISHED_UNIT%' GROUP BY client, slot, hw
    End Function
    Friend Function HardwareStatistics() As clsStatistics.clsHardwareStatistics
        Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
        WriteDebug("Starting Hardware statistics generation")
        Dim rVal As New clsStatistics.clsHardwareStatistics
        clsStatistics.clsHardwareStatistics.mHardware.Clear()
        Try

            Dim hwNames As Dictionary(Of String, Int32) = HardwareNamesCount()
            For Each hwName As String In hwNames.Keys
                Dim nHW As New clsStatistics.clsHardwareStatistics.clsHardware
                nHW.Hardware = hwName
                nHW.WU_Completed = CStr(hwNames(hwName))
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        If dbInst.conState = ConnectionState.Open Then
                            'SELECT  COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND HW LIKE 'G92 [GeForce 9600 GSO]'
                            Dim strQuery As String = "SELECT  COUNT(1) AS 'eue' FROM 'WorkUnits' WHERE NOT CoreStatus LIKE '%FINISHED_UNIT%' AND HW = '" & nHW.Hardware & "'"
                            With dbInst
                                Using rdr As SQLiteDataReader = .dataReader(strQuery)
                                    If IsNothing(rdr) Then
                                        WriteLog("Sql reader disposed - " & strQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        nHW.WU_EUE = CStr(rdr.Item("eue"))
                                    End If
                                End Using
                            End With
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'SELECT AVG(ppd) AS 'ppd' FROM 'WorkUnits' WHERE 
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        If dbInst.conState = ConnectionState.Open Then
                            Dim strQuery As String = "SELECT AVG(ppd) AS 'ppd' FROM 'WorkUnits' WHERE HW = '" & nHW.Hardware & "'"
                            With dbInst
                                Using rdr As SQLiteDataReader = .dataReader(strQuery)
                                    If IsNothing(rdr) Then
                                        WriteLog("Sql reader disposed - " & strQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        If Not CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = "." Then
                                            nHW.AveragePPD = FormatPPD(CStr(Math.Round(CDbl(CStr(rdr.Item("ppd")).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)), 2)))
                                        Else
                                            nHW.AveragePPD = FormatPPD(CStr(Math.Round(CDbl(rdr.Item("ppd")), 2)))
                                        End If
                                    End If
                                End Using
                            End With
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'Total credit 
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        If dbInst.conState = ConnectionState.Open Then
                            Dim strQuery As String = "SELECT TOTAL(Credit) AS 'Credit' FROM 'WorkUnits' WHERE HW = '" & nHW.Hardware & "'"
                            With dbInst
                                Using rdr As SQLiteDataReader = .dataReader(strQuery)
                                    If IsNothing(rdr) Then
                                        WriteLog("Sql reader disposed - " & strQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        nHW.TotalCredit = CStr(rdr.Item("Credit"))
                                    End If
                                End Using
                            End With
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'Computation time
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        If dbInst.conState = ConnectionState.Open Then
                            'Dim strQuery As String = "SELECT Downloaded, Completed  FROM 'WorkUnits' WHERE HW = '" & nHW.Hardware & "'"
                            Dim strQuery As String = "SELECT min(Downloaded) as 'Start', max(Completed) as 'End' FROM 'WorkUnits' WHERE HW = '" & nHW.Hardware & "'"
                            With dbInst
                                Using rdr As SQLiteDataReader = .dataReader(strQuery)
                                    If IsNothing(rdr) Then
                                        WriteLog("Sql reader disposed - " & strQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        nHW.ComputationTime = CDate(rdr.Item("End")).Subtract(CDate(rdr.Item("Start"))).ToString
                                        'While rdr.Read
                                        '    If nHW.ComputationTime = "" Then
                                        '        nHW.ComputationTime = CDate(rdr.Item("Completed")).Subtract(CDate(rdr.Item("Downloaded"))).ToString
                                        '    Else
                                        '        nHW.ComputationTime = TimeSpan.Parse(nHW.ComputationTime).Add(CDate(rdr.Item("Completed")).Subtract(CDate(rdr.Item("Downloaded")))).ToString
                                        '    End If
                                        'End While
                                    End If
                                End Using
                            End With
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'Add work units
                Try
                    nHW.AddWUS(WorkUnits(sqlFilters.GetSqlHardwareLimit(nHW.Hardware), True))
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    Exit Try
                End Try
                clsStatistics.clsHardwareStatistics.mHardware.Add(hwName, nHW)
            Next
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bFailure = True
        End Try
Skip:
        If Not bFailure Then WriteDebug("-Generation took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
        Return rVal
    End Function
    'Friend ReadOnly Property Hardware
    Friend Function HardwareNamesCount() As Dictionary(Of String, Int32)
        Dim rVal As New Dictionary(Of String, Int32)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim sqlQuery As String = "select distinct HW from WorkUnits ORDER BY HW DESC"
                        Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                            If Not IsNothing(rdr) Then
                                While rdr.Read
                                    rVal.Add(CStr(rdr.Item("HW")), 0)
                                End While
                            Else
                                WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                Exit Try
                            End If
                        End Using
                        For xint As Int32 = 0 To rVal.Count - 1
                            sqlQuery = "Select COUNT(1) from WorkUnits where HW='" & rVal.Keys(xint) & "'"
                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                If Not IsNothing(rdr) Then
                                    If rdr.Read Then
                                        rVal(rVal.Keys(xint)) = CInt(rdr.Item(0))
                                    End If
                                Else
                                    WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                    Exit Try
                                End If
                            End Using
                        Next
                    Else
                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                        Exit Try
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
    Friend ReadOnly Property ProjectNames_Count(Optional ByVal Limit As String = Nothing) As Dictionary(Of Int32, Int32)
        Get
            Dim rVal As New Dictionary(Of Int32, Int32)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim sqlQuery As String = "select distinct Project from WorkUnits ORDER BY Project ASC"
                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        rVal.Add(CInt(rdr.Item("Project")), 0)
                                    End While
                                Else
                                    WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                    Exit Try
                                End If
                            End Using

                            For xint As Int32 = 0 To rVal.Count - 1
                                sqlQuery = "Select COUNT(1) from WorkUnits where Project='" & CInt(rVal.Keys(xint)) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        If rdr.Read Then
                                            rVal(rVal.Keys(xint)) = CInt(rdr.Item(0))
                                        End If
                                    Else
                                        WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                        Exit Try
                                    End If
                                End Using
                            Next
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            Exit Try
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try

            Return rVal
        End Get
    End Property
    Friend ReadOnly Property ProjectRanges_Count As Dictionary(Of String, Int32)
        Get
            Dim dPR As New Dictionary(Of String, Int32)
            Try
                'I would like to get proper ranges of simular projects but psummary isn't conclusive 
                Dim dNC As Dictionary(Of Int32, Int32) = ProjectNames_Count
                Dim iTotal As Int32 = 0, iPCount As Int32 = 0
                For Each DictionaryEntry In dNC
                    'Console.WriteLine(CStr(DictionaryEntry.Key) & " (" & CStr(DictionaryEntry.Value) & ")")
                    iPCount += 1
                    iTotal += DictionaryEntry.Value
                Next
                'Console.WriteLine("pCount: " & CStr(iPCount) & " iTotal:" & CStr(iTotal))
                'For Each pInfo In ProjectInfo.Projects.lProjects
                '  writedebug("psummary - " & pInfo.ProjectNumber)
                'Next
                Dim iTotal2 As Int32 = 0
                Dim xInt As Int32 = 0
                Do
                    If dNC.ContainsKey((dNC.Keys(xInt) + 1)) Then
                        'Is a range, find upper limit
                        Dim iLower As Int32 = xInt, iCount As Int32 = dNC(dNC.Keys(xInt))
                        'Console.WriteLine("current index: " & xInt)
                        'Console.WriteLine("-project: " & dNC.Keys(xInt) & " (" & dNC(dNC.Keys(xInt)) & ")")
                        Do
                            iCount += dNC(dNC.Keys(xInt) + 1)
                            'Console.WriteLine("-- added " & dNC.Keys(xInt + 1) & " total: " & iCount)
                            xInt += 1
                            If Not dNC.ContainsKey(dNC.Keys(xInt) + 1) Then
                                'Check with pSummary
                                Exit Do
                            Else
                                'Check with pSummary if projects have the same worth? code? core?
                                'Exit Do
                            End If
                        Loop
                        Dim rName As String = CStr(dNC.Keys(iLower)) & " to " & CStr(dNC.Keys(xInt))
                        dPR.Add(rName, iCount)
                        'Console.WriteLine(" - " & rName & " (" & iCount & ")")
                        iTotal2 += iCount
                        xInt += 1
                    Else
                        'Not a range, add single project 
                        dPR.Add(CStr(dNC.Keys(xInt)), dNC(dNC.Keys(xInt)))
                        iTotal2 += dNC(dNC.Keys(xInt))
                        'Console.WriteLine(" - " & dNC.Keys(xInt) & " (" & dNC(dNC.Keys(xInt)).ToString & ")")
                        xInt += 1
                    End If
                    'Console.WriteLine("loop -" & xInt & " current: " & CStr(dNC.Keys(xInt)))
                    'Console.WriteLine(" iTotal2: " & CStr(iTotal2))
                Loop While xInt < dNC.Count - 2
                'Console.WriteLine("iTotal:" & CStr(iTotal) & " iTotal2:" & CStr(iTotal2))
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return dPR
        End Get
    End Property
    Friend ReadOnly Property ProjectStatistics(Optional ByVal ProjectNumber As String = Nothing, Optional ByVal HW As String = Nothing) As Dictionary(Of String, clsStatistics.clsProjectStatistics.clsProject)
        Get
            Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
            WriteDebug("Generating individual project statistics")
            Dim rVal As New Dictionary(Of String, clsStatistics.clsProjectStatistics.clsProject)
            Dim dNames As Dictionary(Of Int32, Int32) = ProjectNames_Count
            If IsNothing(ProjectNumber) Then
                For Each DictionaryEntry In dNames
                    Dim nProject As New clsStatistics.clsProjectStatistics.clsProject
                    nProject.Number = CStr(DictionaryEntry.Key)
                    nProject.Count = DictionaryEntry.Value
                    'Get failed
                    Try
                        Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInst
                                If .conState = ConnectionState.Open Then
                                    Dim sqlQuery As String = "SELECT COUNT(1) AS 'EUE' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "' AND NOT CoreStatus LIKE '%FINISHED_UNIT%'"
                                    Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                        If Not IsNothing(rdr) Then
                                            If rdr.Read Then
                                                nProject.Failed = CInt(rdr.Item(0))
                                            End If
                                        Else
                                            WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    'Get AvgPPD
                    Try
                        Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInst
                                If .conState = ConnectionState.Open Then
                                    Dim sqlQuery As String = "SELECT AVG(ppd) AS 'ppd' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                    Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                        If Not IsNothing(rdr) Then
                                            If rdr.Read Then
                                                nProject.AvgPPDDB = CStr(rdr.Item(0))
                                            End If
                                        Else
                                            WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    'Get avg tpf
                    Try
                        Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInst
                                If .conState = ConnectionState.Open Then
                                    Dim tsTotal As New TimeSpan(0), iCount As Int32 = 0
                                    'Dim sqlQuery As String = "SELECT TPF FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                    Dim sqlQuery As String = "SELECT AVG(TPF) AS 'TPF' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "' AND NOT 'TPF' ='" & 0 & "'"
                                    Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                        If Not IsNothing(rdr) Then
                                            While rdr.Read
                                                tsTotal = tsTotal.Add(New TimeSpan(0, 0, CInt(rdr.Item(0))))
                                                iCount += 1
                                            End While
                                            tsTotal = New TimeSpan(CLng(tsTotal.Ticks / iCount))
                                        Else
                                            WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    'Get total credit 
                    Try
                        Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInst
                                If .conState = ConnectionState.Open Then
                                    Dim sqlQuery As String = "SELECT TOTAL(Credit) AS 'Credit' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                    Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                        If Not IsNothing(rdr) Then
                                            If rdr.Read Then
                                                nProject.TotalCredit = CStr(rdr.Item(0))
                                            End If
                                        Else
                                            WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    'Get total computation time 
                    Try
                        Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInst
                                If .conState = ConnectionState.Open Then
                                    Dim sqlQuery As String = "SELECT min(Started) as 'Start', max(Completed) as 'End' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                    Dim tsTotal As New TimeSpan(0)
                                    Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                        If Not IsNothing(rdr) Then
                                            tsTotal = CDate(rdr.Item("End")).Subtract(CDate(rdr.Item("Start")))
                                        Else
                                            WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                    If tsTotal > TimeSpan.FromTicks(0) Then
                                        nProject.tsComputationTime = tsTotal
                                    End If
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    'Add al projects 
                    Try
                        Dim lWU As List(Of clsWU) = WorkUnits(" WHERE Project='" & CInt(nProject.Number) & "'")
                        nProject.AddWU(lWU)
                        'Dim aTPF As TimeSpan = New TimeSpan(0), iCount As Int32 = 0
                        'For Each wu As clsWU In lWU
                        '    If Not wu.tpfDB = "" Then
                        '        Try
                        '            aTPF = aTPF.Add(TimeSpan.Parse(wu.tpfDB))
                        '        Catch ex As Exception

                        '        End Try
                        '        iCount += 1
                        '    End If
                        'Next
                        'aTPF = New TimeSpan(CLng(aTPF.Ticks / iCount))
                        'nProject.AvgTPF = FormatTimeSpan(aTPF)
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                    rVal.Add(nProject.Number, nProject)
                Next
            Else
                Dim nProject As New clsStatistics.clsProjectStatistics.clsProject
                nProject.Number = ProjectNumber
                nProject.Count = dNames(CInt(ProjectNumber))
                'Get failed
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInst
                            If .conState = ConnectionState.Open Then
                                Dim sqlQuery As String = "SELECT COUNT(1) AS 'EUE' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        If rdr.Read Then
                                            nProject.Failed = CInt(rdr.Item(0))
                                        End If
                                    Else
                                        WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    WriteLog("Project: " & ProjectNumber, eSeverity.Critical)
                    bFailure = True
                    GoTo Skip
                End Try
                'Get AvgPPD
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInst
                            If .conState = ConnectionState.Open Then
                                Dim sqlQuery As String = "SELECT AVG(ppd) AS 'ppd' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        If rdr.Read Then
                                            nProject.AvgPPDDB = CStr(rdr.Item(0))
                                        End If
                                    Else
                                        WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    WriteLog("Project: " & ProjectNumber, eSeverity.Critical)
                    bFailure = True
                    GoTo Skip
                End Try
                'Get avg tpf
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInst
                            If .conState = ConnectionState.Open Then
                                Dim tsTotal As New TimeSpan(0), iCount As Int32 = 0
                                Dim sqlQuery As String = "SELECT TPF FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        While rdr.Read
                                            tsTotal = tsTotal.Add(TimeSpan.Parse(CStr(rdr.Item(0))))
                                            iCount += 1
                                        End While
                                        tsTotal = New TimeSpan(CLng(tsTotal.Ticks / iCount))
                                    Else
                                        WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    WriteLog("Project: " & ProjectNumber, eSeverity.Critical)
                    bFailure = True
                    GoTo Skip
                End Try
                'Get total credit 
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInst
                            If .conState = ConnectionState.Open Then
                                Dim sqlQuery As String = "SELECT TOTAL(Credit) AS 'Credit' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        If rdr.Read Then
                                            nProject.TotalCredit = CStr(rdr.Item(0))
                                        End If
                                    Else
                                        WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    WriteLog("Project: " & ProjectNumber, eSeverity.Critical)
                    bFailure = True
                    GoTo Skip
                End Try
                'Get total computation time 
                Try
                    Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInst
                            If .conState = ConnectionState.Open Then
                                Dim sqlQuery As String = "SELECT min(Started) as 'Start', max(Completed) as 'End' FROM 'WorkUnits' WHERE Project='" & CInt(nProject.Number) & "'"
                                Dim tsTotal As New TimeSpan(0)
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        tsTotal = CDate(rdr.Item("End")).Subtract(CDate(rdr.Item("Start")))
                                    Else
                                        WriteLog("The datareader got disposed - " & sqlQuery, eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                                If tsTotal > TimeSpan.FromTicks(0) Then
                                    nProject.tsComputationTime = tsTotal
                                End If
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    WriteLog("Project: " & ProjectNumber, eSeverity.Critical)
                    bFailure = True
                    GoTo Skip
                End Try
                rVal.Add(nProject.Number, nProject)
            End If
Skip:
            If Not bFailure Then WriteDebug("-Generation took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property ClientNames_Count As Dictionary(Of String, Int32)
        Get
            Dim rVal As New Dictionary(Of String, Int32)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim sqlQuery As String = "select distinct Client from WorkUnits ORDER BY Client DESC"
                            Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        rVal.Add(CStr(rdr.Item("Client")), 0)
                                    End While
                                Else
                                    WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                    Exit Try
                                End If
                            End Using
                            For xint As Int32 = 0 To rVal.Count - 1
                                sqlQuery = "Select COUNT(1) from WorkUnits where Client='" & rVal.Keys(xint) & "'"
                                Using rdr As SQLiteDataReader = .dataReader(sqlQuery)
                                    If Not IsNothing(rdr) Then
                                        If rdr.Read Then
                                            rVal(rVal.Keys(xint)) = CInt(rdr.Item(0))
                                        End If
                                    Else
                                        WriteLog("The data reader got invalidated while trying to order project information", eSeverity.Critical)
                                        Exit Try
                                    End If
                                End Using
                            Next
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            Exit Try
                        End If

                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
#End Region
#Region "Diagnostics"
    Public ReadOnly Property IsDiagnosticsStored(ByVal ClientName As String, ByVal dtDiagnostics As String) As Boolean
        Get
            Dim bRet As Boolean = False
            Try
                'Not used/finished
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Diagnostics'")
                            Dim bHas As Boolean = rdr.HasRows
                            rdr.Close()
                            If Not bHas Then Exit Try

                            bRet = True
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return bRet
        End Get
    End Property

    Public Sub StoreDiagnostics(ByVal ClientName As String, ByVal dtDiagnostics As String, ByVal Diagnostics As String)
        Try
            Throw New NotImplementedException
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#End Region
#Region "Should this log be checked?"
    Friend ReadOnly Property IsLogStored(ByVal ClientName As String, ByVal FileName As String) As Boolean
        Get
            Dim rVal As Boolean = False
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FinishedLogs'")
                                bHas = rdr.HasRows
                            End Using
                            If Not bHas Then Exit Try
                            Dim sQuery As String = "SELECT FileName FROM 'FinishedLogs' WHERE ClientName='" & FormatSQLString(ClientName, False, True) & "' AND FileName='" & FormatSQLString(FileName.Replace(".txt", ""), False, True) & "'"
                            Using rdr As SQLiteDataReader = .dataReader(sQuery)
                                rVal = rdr.HasRows
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                rVal = False
            End Try
            Return rVal
        End Get
    End Property
    Friend Sub SetLogStored(ByVal ClientName As String, ByVal FileName As String, ByVal LogString As String, ByVal LineCount As Int32)
        Try
            If IsLogStored(ClientName, FileName) Then Exit Sub
            WriteDebug("Setting log stored for " & ClientName & " - " & FileName)
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        'Dim bDoCommit As Boolean = Not .TransactionInProgress
                        'If Not .TransactionInProgress Then
                        '    .BeginTransaction()
                        'End If
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FinishedLogs'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            'CreateTable
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'FinishedLogs' (ClientName TEXT, FileName TEXT, LogString TEXT, LineCount TEXT)")
                        End If
                        sqlErr = .ExecuteNonQuery("INSERT INTO 'FinishedLogs' (ClientName, FileName, LogString, LineCount) VALUES('" & FormatSQLString(ClientName, False, True) & sib & FormatSQLString(FileName, False, True) & sib & FormatSQLString(LogString) & sib & LineCount.ToString & "')")
                        'If bDoCommit Then .EndTransaction()
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Function StoredLog(ByVal ClientName As String, ByVal FileName As String) As clsLogFile
        Dim lSection As New clsLogFile(ClientName)
        Try
            If Not IsLogStored(ClientName, FileName) Then Return lSection
            lSection.File = FileName
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'Finishedlogs' WHERE ClientName='" & FormatSQLString(ClientName, False, True) & "' AND FileName='" & FormatSQLString(FileName, False, True) & "'")
                        While rdr.Read
                            lSection.Log = FormatSQLString(CStr(rdr.Item("LogString")), True)
                            lSection.LineCount = CStr(rdr.Item("LineCount"))
                        End While
                        rdr.Close()
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return lSection
    End Function
#End Region
#Region "FAHCLient INFO"
    Friend Function HasClientInfo(ByVal ClientName As String, ByVal LogDate As DateTime) As Boolean
        Dim rVal As Boolean = False
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientInfo'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then Exit Try
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ClientInfo' WHERE Client='" & ClientName & "' AND LogDate='" & LogDate.ToString("s") & "'")
                            rVal = rdr.HasRows
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
    Friend Overloads Sub SaveFAHClientInfo(ByVal ClientName As String, lClientInfo As List(Of clsClientInfo))
        Try
            Dim lCI As New List(Of clsClientInfo)
            For Each ClientInfo As clsClientInfo In lClientInfo
                If Not HasClientInfo(ClientName, ClientInfo.Info.dtDate) Then lCI.Add(ClientInfo)
            Next
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientInfo'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'ClientInfo' (Client TEXT, LogDate DATETIME, Info TEXT)")
                        End If
                        Dim bCommit As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then .BeginTransaction()
                        For Each ClientInfo As clsClientInfo In lCI
                            WriteDebug("Storing client info for " & ClientName & " - " & ClientInfo.Info.dtDate.ToString("s"))
                            Dim sbValues As New StringBuilder
                            For xInt As Int32 = 0 To ClientInfo.Info.Keys.Count - 1
                                sbValues.Append(ClientInfo.Info.Keys(xInt) & "==" & ClientInfo.Info(ClientInfo.Info.Keys(xInt)) & ",,,")
                            Next
                            sbValues.Remove(sbValues.Length - 3, 3)
                            sqlErr = .ExecuteNonQuery("INSERT INTO 'ClientInfo' (Client, LogDate, Info) VALUES('" & ClientName & "','" & ClientInfo.Info.dtDate.ToString("s") & "','" & FormatSQLString(sbValues.ToString) & "')")
                        Next
                        If bCommit Then .EndTransaction()
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Overloads Sub SaveFAHClientInfo(ByVal ClientName As String, ByVal LogDate As DateTime, ByVal ClientInf As clsClientInfo.FAHClientInfo)
        Try
            If HasClientInfo(ClientName, LogDate) Then
                Exit Sub
            End If
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        WriteDebug("Storing client info for " & ClientName & " - " & LogDate.ToString("s"))
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientInfo'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'ClientInfo' (Client TEXT, LogDate DATETIME, Info TEXT)")
                        End If
                        Dim sbValues As New StringBuilder
                        For xInt As Int32 = 0 To ClientInf.Keys.Count - 1
                            sbValues.Append(ClientInf.Keys(xInt) & "==" & ClientInf(ClientInf.Keys(xInt)) & ",,,")
                        Next
                        sbValues.Remove(sbValues.Length - 3, 3)
                        sqlErr = .ExecuteNonQuery("INSERT INTO 'ClientInfo' (Client, LogDate, Info) VALUES('" & ClientName & "','" & LogDate.ToString("s") & "','" & FormatSQLString(sbValues.ToString) & "')")
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Function ClientInfoBeforeDT(ByVal ClientName As String, ByVal LogDate As DateTime) As clsClientInfo.FAHClientInfo
        'Use this to find ClientInfo for workunits
        'split(sbvalues.ToString ,",,,",,CompareMethod.Text )
        Dim rCI As New clsClientInfo.FAHClientInfo
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ClientInfo' WHERE Client='" & ClientName & "' AND LogDate <'" & LogDate.ToString("s") & "' ORDER BY LogDate DESC")
                            If Not IsNothing(rdr) Then
                                If rdr.HasRows Then
                                    rCI.Clear()
                                    rCI.dtDB = CDate(rdr.Item("LogDate"))
                                    Dim iInf As String = CStr(rdr.Item("Info"))
                                    Dim iArr As Array = Split(iInf, ",,,", , CompareMethod.Text)
                                    For Each iStr As String In iArr
                                        rCI.Add(iStr.Substring(0, iStr.IndexOf("==")), iStr.Substring(iStr.IndexOf("==") + 2))
                                    Next
                                    Exit Try
                                Else
                                    Exit Try
                                End If
                            Else
                                WriteLog("The datareader got disposed, there should be more errors", eSeverity.Critical)
                            End If
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rCI
    End Function
#End Region
#Region "FAHClient CONFIG"
    Friend Function HasClientConfig(ByVal ClientName As String, ByVal Config As String, ByVal dtConfig As DateTime) As Boolean
        Dim rVal As Boolean = False
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientConfig'")
                            rVal = rdr.HasRows
                        End Using
                        If Not rVal Then Exit Try
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ClientConfig' WHERE Client='" & ClientName & "' AND ConfigString='" & FormatSQLString(Config) & "' AND LogDate='" & dtConfig.ToString("s") & "'")
                            rVal = rdr.HasRows
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            rVal = False
        End Try
        Return rVal
    End Function
    Public Overloads Sub SaveFAHClientConfig(ByVal ClientName As String, lClientConfig As List(Of clsClientConfig))
        Try
            Dim lCC As New List(Of clsClientConfig)
            For Each ClientConfig As clsClientConfig In lClientConfig
                If Not HasClientConfig(ClientName, ClientConfig.Configuration.ConfigString, ClientConfig.Configuration.ConfigurationDT) Then lCC.Add(ClientConfig)
            Next
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientConfig'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'ClientConfig' (Client TEXT, LogDate DATETIME, ConfigString TEXT)")
                        End If
                        Dim bCommit As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then .BeginTransaction()
                        For Each ClientConfig As clsClientConfig In lClientConfig
                            WriteDebug("Saving fahclient config for " & ClientName & " - " & ClientConfig.Configuration.ConfigurationDT.ToString("s"))
                            sqlErr = .ExecuteNonQuery("INSERT INTO 'ClientConfig' (Client, LogDate, ConfigString) VALUES ('" & ClientName & sib & ClientConfig.Configuration.ConfigurationDT.ToString("s") & sib & FormatSQLString(ClientConfig.Configuration.ConfigString) & "')")
                        Next
                        If bCommit Then .EndTransaction()
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Public Overloads Sub SaveFAHClientConfig(ByVal ClientName As String, ByVal LogDate As DateTime, ByVal ClientCFG As clsClientConfig.clsConfiguration)
        Try
            If HasClientConfig(ClientName, ClientCFG.ConfigString, LogDate) Then
                Exit Sub
            End If
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        WriteDebug("Saving fahclient config for " & ClientName & " - " & LogDate.ToString)
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='ClientConfig'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'ClientConfig' (Client TEXT, LogDate DATETIME, ConfigString TEXT)")
                        End If
                        sqlErr = .ExecuteNonQuery("INSERT INTO 'ClientConfig' (Client, LogDate, ConfigString) VALUES ('" & ClientName & sib & LogDate.ToString("s") & sib & FormatSQLString(ClientCFG.ConfigString) & "')")
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Function ClientConfigBeforeDT(ByVal ClientName As String, ByVal LogDate As DateTime) As clsClientConfig.clsConfiguration
        'Use this to find clientconfig per work unit 
        Dim rCC As New clsClientConfig.clsConfiguration
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'ClientConfig' WHERE Client='" & ClientName & "' AND LogDate <'" & LogDate.ToString("s") & "' ORDER BY LogDate DESC")
                            If Not IsNothing(rdr) Then
                                If rdr.HasRows Then
                                    rCC.ReadString(FormatSQLString(CStr(rdr.Item("ConfigString")), True))
                                    rCC.ConfigurationDT = CDate(rdr.Item("LogDate"))
                                    Exit Try
                                Else
                                    Exit Try
                                End If
                            Else
                                WriteLog("The datareader got invalidated", eSeverity.Critical)
                                Exit Try
                            End If
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rCC
    End Function
#End Region
#Region "Work unit"
#Region "Last sql query"
    Private _LastSQL As String = String.Empty
    Friend ReadOnly Property LastSQL As String
        Get
            Return _LastSQL
        End Get
    End Property
#End Region
#Region "Queued WorkUnits"
    Friend Overloads ReadOnly Property HasQueuedWorkUnit(WU As clsWU) As Boolean
        Get
            Return HasQueuedWorkUnit(WU.ClientName, WU.Slot, WU.unit, WU.utcDownloaded)
        End Get
    End Property
    Friend Overloads ReadOnly Property HasQueuedWorkUnit(ClientName As String, SlotID As String, unit As String, Downloaded As DateTime) As Boolean
        Get
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='QueuedWorkUnits'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Return False
                                End If
                            End Using
                            If Not bHas Then Return False
                            If SlotID.Length = 1 Then SlotID = "0" & SlotID
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'QueuedWorkUnits' WHERE unit='" & unit & "' AND Slot='" & SlotID & "' AND Client='" & FormatSQLString(ClientName, False, True) & "' AND Downloaded='" & Downloaded.ToString("s") & "'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Return False
                                End If
                            End Using
                            Return bHas
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                            Return False
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Sub RemoveWorkUnitFromQueue(WU As clsWU)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='QueuedWorkUnits'")
                            If Not IsNothing(rdr) Then
                                bHas = rdr.HasRows
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Exit Sub
                            End If
                        End Using
                        If Not bHas Then Exit Sub
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'QueuedWorkUnits' WHERE unit='" & WU.unit & "' AND Slot='" & WU.Slot & "' AND Client='" & FormatSQLString(WU.ClientName, False, True) & "' AND Downloaded='" & WU.utcDownloaded.ToString("s") & "'")
                            If Not IsNothing(rdr) Then
                                bHas = rdr.HasRows
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Exit Sub
                            End If
                        End Using
                        If bHas Then sqlErr = .ExecuteNonQuery("DELETE FROM 'QueuedWorkUnits' WHERE unit='" & WU.unit & "' AND Slot='" & WU.Slot & "' AND Client='" & FormatSQLString(WU.ClientName, False, True) & "' AND Downloaded='" & WU.utcDownloaded.ToString("s") & "'")
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Exit Sub
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Overloads Sub SaveQueuedWU(WorkUnit As clsWU)
        Try
            Dim nList As New List(Of clsWU)
            nList.Add(WorkUnit)
            SaveQueuedWU(nList)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Overloads Sub SaveQueuedWU(WorkUnits As List(Of clsWU), Optional ShowBussy As Boolean = False)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='QueuedWorkUnits'")
                            If Not IsNothing(rdr) Then
                                bHas = rdr.HasRows
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Exit Sub
                            End If
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'QueuedWorkUnits' (Client TEXT, Slot TEXT, HW TEXT, Project INT, RCG TEXT, unit TEXT, CS TEXT, WS TEXT, CoreStatus TEXT, ServerResponse TEXT, CoreSnippet TEXT, CoreVersion TEXT, CoreCompiler TEXT, Core TEXT, BoardType TEXT, Downloaded DATETIME, Started DATETIME, Completed DATETIME, Submitted DATETIME, Credit REAL, PPD REAL,UploadSize TEXT, dblUploadSize REAL, StartUpload DATETIME, TPF INTEGER,DownloadSize TEXT, dblDownloadSize REAL, StartDownload DATETIME, TPFmin INTEGER, TPFmax INTEGER, LogFile TEXT, LastLine TEXT, LineIndex INTEGER, LineDT DATETIME,Progress INTEGER)")
                        End If
                        Dim bTransaction As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then
                            .BeginTransaction()
                        End If
                        Dim iWU As Int32 = 0
                        If ShowBussy Then
                            delegateFactory.BussyBox.SetNewProgressBoundries(0, WorkUnits.Count, 0, ProgressBarStyle.Continuous)
                        End If
                        For Each WU As clsWU In WorkUnits
                            iWU += 1
                            If ShowBussy Then
                                BussyBox.SetMessage("Storing queued work unit for " & WU.ClientName & Environment.NewLine & "Downloaded: " & WU.dtDownloaded.ToString & Environment.NewLine & WU.PRCG)
                            End If
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'QueuedWorkUnits' WHERE unit='" & WU.unit & "' AND Slot='" & WU.Slot & "' AND Client='" & FormatSQLString(WU.ClientName, False, True) & "' AND Downloaded='" & WU.utcDownloaded.ToString("s") & "'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Exit Sub
                                End If
                            End Using
                            If bHas Then
                                sqlErr = .ExecuteNonQuery("DELETE FROM 'QueuedWorkUnits' WHERE Client='" & FormatSQLString(WU.ClientName, False, True) & "' AND Downloaded='" & WU.utcDownloaded.ToString("s") & "' AND unit='" & WU.unit & "' AND Slot='" & WU.Slot & "'")
                            End If
                            Try
                                Dim qStr As String = "INSERT INTO 'QueuedWorkUnits' (Client, Slot, HW, Project, RCG, unit, CS, WS, CoreStatus, ServerResponse, CoreSnippet, CoreVersion, CoreCompiler, Core, BoardType, Downloaded, Started, Completed, Submitted, Credit, PPD, UploadSize, StartUpload, TPF, dblUploadSize, DownloadSize, dblDownloadSize, StartDownload, TPFmin, TPFmax, LogFile, LastLine, LineIndex, LineDT, Progress) VALUES('" & FormatSQLString(WU.ClientName, False, True) & sib & WU.Slot & sib & WU.HW & sib & CInt(WU.Project) & sib & WU.RCG & sib & WU.unit & sib & WU.CS & sib & WU.WS & sib & WU.CoreStatus & sib & WU.ServerResponce & sib & FormatSQLString(WU.CoreSnippet) & sib & WU.CoreVersion & sib & WU.CoreCompiler & sib & WU.Core & sib & WU.BoardType & sib & WU.utcDownloaded.ToString("s") & sib & WU.utcStarted.ToString("s") & sib & WU.utcCompleted.ToString("s") & sib & WU.utcSubmitted.ToString("s") & sib & WU.Credit & sib & WU.PPD & sib & WU.UploadSize & sib & WU.utcStartUpload.ToString("s") & sib & CInt(WU.tsTPF.TotalSeconds) & sib & WU.iUploadSize & sib & WU.DownloadSize & sib & WU.iDownloadSize & sib & WU.utcStartDownload.ToString("s") & sib & WU.tsTPF_Min.TotalSeconds & sib & WU.tsTPF_Max.TotalSeconds & sib & WU.Log & sib & WU.Line & sib & WU.lineIndex & sib & WU.LineDT.ToString("s") & sib & CInt(WU.Percentage) & "')"
                                sqlErr = .ExecuteNonQuery(qStr)
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                Exit Sub
                            End Try


                            'Save frames
                            If WU.Frames.Count > 0 Then
                                Try
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FrameInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "'")
                                        bHas = rdr.HasRows
                                    End Using
                                    If bHas Then
                                        WriteDebug("Existing frame data for " & WU.PRCG & "-" & WU.unit & " will be removed")
                                        sqlErr = .ExecuteNonQuery("DELETE FROM 'FrameInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "'")
                                    Else
                                        sqlErr = .ExecuteNonQuery("CREATE TABLE 'FrameInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "' (dtPercentage DATETIME, Percentage INTEGER)")
                                    End If
                                    'transactions moved to whole parsing loop

                                    For Each fInfo In WU.Frames
                                        'delegatefactory.bussybox.SetMessage("Storing parse results for " & wu.ClientName & Environment.NewLine & "Downloaded: " & wu.dtDownloaded.ToString & Environment.NewLine & wu.PRCG & Environment.NewLine & "Frame " & fInfo.strPercentage & " - " & fInfo.FrameDT.ToString)
                                        sqlErr = .ExecuteNonQuery("INSERT INTO 'FrameInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "' (dtPercentage, Percentage) VALUES('" & fInfo.utcFrame.ToString("s") & "','" & fInfo.strPercentage & "')")
                                    Next
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                    Exit Sub
                                End Try
                            End If

                            If WU.bHasRestarted AndAlso WU.restartInfo.RestartInfo.Count > 0 Then
                                Try
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='RestartInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "'")
                                        bHas = rdr.HasRows
                                    End Using
                                    'mhm delete table.. then recreate might be quicker then deleting all records?
                                    If Not bHas Then
                                        WriteDebug("Creating table for restart info for work unit: " & WU.unit)
                                        sqlErr = .ExecuteNonQuery("CREATE TABLE 'RestartInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "' (dtFrame DATETIME, Frame TEXT, CoreStatus TEXT)")
                                    Else
                                        WriteDebug("Cleaning existing restart information table for unit: " & WU.unit)
                                        sqlErr = .ExecuteNonQuery("DELETE FROM 'RestartInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "'")
                                    End If

                                    'transactions moved to whole parsing loop
                                    For Each rInfo In WU.restartInfo.RestartInfo
                                        'delegatefactory.bussybox.SetMessage("Storing parse results for " & wu.ClientName & Environment.NewLine & "Downloaded: " & wu.dtDownloaded.ToString & " - " & wu.PRCG & "Restart info " & rInfo.CoreStatus & " frame " & rInfo.LastFrame.strPercentage)
                                        sqlErr = .ExecuteNonQuery("INSERT INTO 'RestartInfo_" & FormatSQLString(WU.ClientName, False, True) & "_" & WU.Slot & "_" & WU.unit & "' (dtFrame, Frame, CoreStatus) VALUES('" & rInfo.LastFrame.utcFrame.ToString("s") & sib & rInfo.LastFrame.strPercentage & sib & rInfo.CoreStatus & "')")
                                    Next
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                    Exit Sub
                                End Try
                            End If
SkipWU:
                        Next
                        If bTransaction Then .EndTransaction()
                    Else
                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                        Exit Sub
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Exit Sub
        End Try
    End Sub
    Friend Function QueuedWorkUnit(ClientName As String, SlotID As String, unit As String, Downloaded As DateTime, Optional ConvertToActiveLog As Boolean = True, Optional SkipDetails As Boolean = True) As clsWU
        Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
        WriteDebug("Reading work unit from database, client: " & ClientName & " slot: " & SlotID & " unit: " & unit)
        Dim nWU As New clsWU
        Try
            nWU.ClientName = ClientName
            nWU.Slot = SlotID
            nWU.unit = unit
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'QueuedWorkUnits' WHERE Client='" & FormatSQLString(ClientName, False, True) & "' AND Slot='" & SlotID & "' AND unit='" & unit & "' AND Downloaded ='" & Downloaded.ToString("s") & "'")
                            If Not IsNothing(rdr) Then
                                If rdr.Read Then
                                    nWU.unit = CStr(rdr.Item("unit"))
                                    nWU.Slot = CStr(rdr.Item("Slot"))
                                    nWU.ClientName = FormatSQLString(CStr(rdr.Item("Client")), True, True)
                                    nWU.PRCG = "Project:" & CStr(rdr.Item("Project")) & " " & CStr(rdr.Item("RCG"))
                                    nWU.BoardType = CStr(rdr.Item("BoardType"))
                                    nWU.Core = CStr(rdr.Item("core"))
                                    nWU.CoreCompiler = CStr(rdr.Item("CoreCompiler"))
                                    nWU.CoreSnippet = FormatSQLString(CStr(rdr.Item("CoreSnippet")), True)
                                    nWU.CoreStatus = CStr(rdr.Item("CoreStatus"))
                                    nWU.CoreVersion = CStr(rdr.Item("CoreVersion"))
                                    nWU.Credit = CStr(rdr.Item("Credit"))
                                    nWU.CS = CStr(rdr.Item("CS"))
                                    nWU.WS = CStr(rdr.Item("WS"))
                                    nWU.dtCompleted = CDate(rdr.Item("Completed"))
                                    nWU.dtDownloaded = CDate(rdr.Item("Downloaded"))
                                    nWU.dtStartDownload = CDate(rdr.Item("StartDownload"))
                                    nWU.DownloadSize = CStr(rdr.Item("DownloadSize"))
                                    nWU.iDownloadSize = CDbl(rdr.Item("dblDownloadSize"))
                                    nWU.dtStarted = CDate(rdr.Item("Started"))
                                    nWU.dtSubmitted = CDate(rdr.Item("Submitted"))
                                    nWU.HW = CStr(rdr.Item("HW"))
                                    nWU.ServerResponce = CStr(rdr.Item("ServerResponse"))
                                    nWU.PPD = CStr(rdr.Item("PPD"))
                                    nWU.Credit = CStr(rdr.Item("Credit"))
                                    nWU.UploadSize = CStr(rdr.Item("UploadSize"))
                                    nWU.dtStartUpload = CDate(rdr.Item("StartUpload"))
                                    'nWU.tpfDB = FormatTimeSpan(TimeSpan.Parse(CStr(rdr.Item("TPF"))))
                                    nWU.tpfDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPF"))))
                                    Try
                                        nWU.tpfMaxDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmax"))))
                                        nWU.tpfMinDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmin"))))
                                    Catch ex As Exception
                                        WriteLog("The database needs converting!", eSeverity.Critical)
                                    End Try
                                    nWU.Log = CStr(rdr.Item("Logfile"))
                                    nWU.Line = CStr(rdr.Item("LastLine"))
                                    nWU.lineIndex = CInt(rdr.Item("LineIndex"))
                                    nWU.LineDT = CDate(rdr.Item("LineDT"))
                                    nWU.Percentage = CStr(rdr.Item("Progress"))
                                    If ConvertToActiveLog Then
                                        WriteLog(WorkUnitLogHeader(nWU) & "Converting core snippet to active log: " & nWU.ConvertSnippetToActiveLog.ToString)
                                    End If
                                Else
                                    Return nWU
                                End If
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                        bFailure = True
                        GoTo Skip
                    End If
                End With
            End Using

            Try
                If SkipDetails Then Exit Try
                'Add frames
                Using dbInstFrames As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInstFrames
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                If IsNothing(rdr) Then
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                Else
                                    bHas = rdr.HasRows
                                End If
                            End Using
                            If bHas Then
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                    If IsNothing(rdr) Then
                                        WriteLog("The datareader got disposed", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        While rdr.Read
                                            nWU.AddFrame("THIS IS A TEST (" & rdr.Item("Percentage").ToString & "%)", CDate(rdr.Item("dtPercentage")))
                                            Application.DoEvents()
                                        End While
                                    End If
                                End Using
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bFailure = True
                GoTo Skip
            End Try
            'Add restart info
            Try
                If SkipDetails Then Exit Try
                Using dbInstRestart As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInstRestart
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean = False
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End Using
                            If bHas Then
                                nWU.bHasRestarted = True
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                    If Not IsNothing(rdr) Then
                                        While rdr.Read
                                            'THIS IS A TEST IS NEEDED FOR ACCURATE FRAME ADDING 
                                            nWU.restartInfo.AddRestart(CStr(rdr.Item("CoreStatus")), New clsWU.clsFrame("THIS IS A TEST (" & CStr(rdr.Item("Frame")) & "%)", CDate(rdr.Item("dtFrame"))))
                                        End While
                                    Else
                                        WriteLog("The datareader got disposed", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If

                                End Using
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bFailure = True
                GoTo Skip
            End Try
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bFailure = True
            GoTo Skip
        End Try
Skip:
        If Not bFailure Then WriteDebug("-process took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
        Return nWU
    End Function
    Friend Function QueuedWorkUnits(Optional ClientName As String = Nothing, Optional ConvertSnippetToActiveLog As Boolean = True) As List(Of clsWU)
        Try
            Dim rVal As New List(Of clsWU)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='QueuedWorkUnits'")
                                bHas = rdr.HasRows
                            End Using
                            If Not bHas Then GoTo Skip
                            Dim sqlSelect As String
                            If IsNothing(ClientName) Then
                                sqlSelect = "SELECT * FROM 'QueuedWorkUnits'"
                            Else
                                sqlSelect = "SELECT * FROM 'QueuedWorkUnits' WHERE Client='" & FormatSQLString(ClientName, False, True) & "'"
                            End If
                            Using rdr As SQLiteDataReader = .dataReader(sqlSelect)
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        Dim nWU As New clsWU
                                        nWU.unit = CStr(rdr.Item("unit"))
                                        nWU.Slot = CStr(rdr.Item("Slot"))
                                        nWU.ClientName = FormatSQLString(CStr(rdr.Item("Client")), True, True)
                                        nWU.PRCG = "Project:" & CStr(rdr.Item("Project")) & " " & CStr(rdr.Item("RCG"))
                                        nWU.BoardType = CStr(rdr.Item("BoardType"))
                                        nWU.Core = CStr(rdr.Item("core"))
                                        nWU.CoreCompiler = CStr(rdr.Item("CoreCompiler"))
                                        nWU.CoreSnippet = FormatSQLString(CStr(rdr.Item("CoreSnippet")), True)
                                        nWU.CoreStatus = CStr(rdr.Item("CoreStatus"))
                                        nWU.CoreVersion = CStr(rdr.Item("CoreVersion"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.CS = CStr(rdr.Item("CS"))
                                        nWU.WS = CStr(rdr.Item("WS"))
                                        nWU.dtCompleted = CDate(rdr.Item("Completed"))
                                        nWU.dtDownloaded = CDate(rdr.Item("Downloaded"))
                                        nWU.dtStarted = CDate(rdr.Item("Started"))
                                        nWU.dtSubmitted = CDate(rdr.Item("Submitted"))
                                        nWU.HW = CStr(rdr.Item("HW"))
                                        nWU.ServerResponce = CStr(rdr.Item("ServerResponse"))
                                        nWU.PPD = CStr(rdr.Item("PPD"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.UploadSize = CStr(rdr.Item("UploadSize"))
                                        nWU.dtStartUpload = CDate(rdr.Item("StartUpload"))
                                        nWU.dtStartDownload = CDate(rdr.Item("StartDownload"))
                                        nWU.DownloadSize = CStr(rdr.Item("DownloadSize"))
                                        Try
                                            Dim strTPFdb As String = CStr(rdr.Item("TPF"))
                                            If IsNumeric(strTPFdb) Then
                                                nWU.tpfDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(strTPFdb)))
                                            Else
                                                Dim tsTmp As TimeSpan
                                                If TimeSpan.TryParse(strTPFdb, tsTmp) Then
                                                    nWU.tpfDB = FormatTimeSpan(tsTmp)
                                                End If
                                            End If
                                            If Not IsNothing(rdr.Item("TPFmax")) AndAlso Not IsDBNull(rdr.Item("TPFmax")) Then nWU.tpfMaxDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmax"))))
                                            If Not IsNothing(rdr.Item("TPFmin")) AndAlso Not IsDBNull(rdr.Item("TPFmin")) Then nWU.tpfMinDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmin"))))
                                        Catch ex As Exception
                                            WriteError(ex.Message, Err)
                                            GoTo Skip
                                        End Try
                                        nWU.Log = CStr(rdr.Item("Logfile"))
                                        nWU.Line = CStr(rdr.Item("LastLine"))
                                        nWU.lineIndex = CInt(rdr.Item("LineIndex"))
                                        nWU.LineDT = CDate(rdr.Item("LineDT"))
                                        nWU.Percentage = CStr(rdr.Item("Progress"))
                                        If ConvertSnippetToActiveLog Then
                                            WriteLog(WorkUnitLogHeader(nWU) & "Converting core snippet to active log: " & nWU.ConvertSnippetToActiveLog.ToString)
                                        End If
                                        rVal.Add(nWU)
                                    End While
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    GoTo Skip
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                            GoTo Skip
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                GoTo Skip
            End Try
Skip:
            Return rVal
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New List(Of clsWU)
        End Try
    End Function
#End Region
#Region "Finished WorkUnits"
    Friend Overloads ReadOnly Property HasWorkUnit(WU As clsWU) As Boolean
        Get
            Return HasWorkUnit(WU.ClientName, WU.Slot, WU.unit, WU.utcDownloaded)
        End Get
    End Property
    Friend Overloads ReadOnly Property HasWorkUnit(ByVal ClientName As String, ByVal SlotID As String, ByVal unit As String, ByVal Downloaded As DateTime) As Boolean
        Get
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='WorkUnits'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Return False
                                End If
                            End Using
                            If Not bHas Then Return False
                            If SlotID.Length = 1 Then SlotID = "0" & SlotID
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'WorkUnits' WHERE unit='" & unit & "' AND Slot='" & SlotID & "' AND Client='" & FormatSQLString(ClientName, False, True) & "' AND Downloaded='" & Downloaded.ToString("s") & "'")
                                If Not IsNothing(rdr) Then
                                    bHas = rdr.HasRows
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Return False
                                End If
                            End Using
                            Return bHas
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                            Return False
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
    Friend Overloads Sub SaveWorkUnit(ByVal TheList As List(Of clsWU), Optional ByVal ShowBussy As Boolean = False, Optional UpdateAll As Boolean = False)
        Dim sWU As New List(Of clsWU)
        If Not UpdateAll Then
            For Each WU In TheList
                If Not HasWorkUnit(WU) Then
                    sWU.Add(WU)
                End If
            Next
        Else
            sWU.AddRange(TheList)
        End If
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='WorkUnits'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'WorkUnits' (Client TEXT, Slot TEXT, HW TEXT, Project INT, RCG TEXT, unit TEXT, CS TEXT, WS TEXT, CoreStatus TEXT, ServerResponse TEXT, CoreSnippet TEXT, CoreVersion TEXT, CoreCompiler TEXT, Core TEXT, BoardType TEXT, Downloaded DATETIME, Started DATETIME, Completed DATETIME, Submitted DATETIME, Credit REAL, PPD REAL,UploadSize TEXT, dblUploadSize REAL, StartUpload DATETIME, TPF INTEGER,DownloadSize TEXT, dblDownloadSize REAL, StartDownload DATETIME, TPFmin INTEGER, TPFmax INTEGER)")
                        End If
                        Dim bTransaction As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then
                            .BeginTransaction()
                        End If
                        Dim iWu As Int32 = 0
                        If UpdateAll Then BussyBox.SetNewProgressBoundries(0, sWU.Count, 0, ProgressBarStyle.Continuous)
                        For Each wu As clsWU In sWU
                            iWu += 1
                            If ShowBussy Then
                                If Not UpdateAll Then
                                    BussyBox.SetMessage("Storing parse results for " & wu.ClientName & Environment.NewLine & "Downloaded: " & wu.dtDownloaded.ToString & Environment.NewLine & wu.PRCG)
                                Else
                                    BussyBox.SetMessage("Storing converted data " & " (" & CStr(iWu) & " of " & sWU.Count & ")" & Environment.NewLine & Environment.NewLine & WorkUnitLogHeader(wu, True))
                                    BussyBox.SetProgress(iWu)
                                End If
                            End If
                            Try
                                Dim qStr As String = "INSERT INTO 'WorkUnits' (Client, Slot, HW, Project, RCG, unit, CS, WS, CoreStatus, ServerResponse, CoreSnippet, CoreVersion, CoreCompiler, Core, BoardType, Downloaded, Started, Completed, Submitted, Credit, PPD, UploadSize, StartUpload, TPF, dblUploadSize, DownloadSize, dblDownloadSize, StartDownload, TPFmin, TPFmax) VALUES('" & FormatSQLString(wu.ClientName, False, True) & sib & wu.Slot & sib & wu.HW & sib & CInt(wu.Project) & sib & wu.RCG & sib & wu.unit & sib & wu.CS & sib & wu.WS & sib & wu.CoreStatus & sib & wu.ServerResponce & sib & FormatSQLString(wu.CoreSnippet) & sib & wu.CoreVersion & sib & wu.CoreCompiler & sib & wu.Core & sib & wu.BoardType & sib & wu.utcDownloaded.ToString("s") & sib & wu.utcStarted.ToString("s") & sib & wu.utcCompleted.ToString("s") & sib & wu.utcSubmitted.ToString("s") & sib & wu.Credit & sib & wu.PPD & sib & wu.UploadSize & sib & wu.utcStartUpload.ToString("s") & sib & CInt(wu.tsTPF.TotalSeconds) & sib & wu.iUploadSize & sib & wu.DownloadSize & sib & wu.iDownloadSize & sib & wu.utcStartDownload.ToString("s") & sib & wu.tsTPF_Min.TotalSeconds & sib & wu.tsTPF_Max.TotalSeconds & "')"
                                sqlErr = .ExecuteNonQuery(qStr)
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                            End Try
                            'Save frames
                            If wu.Frames.Count > 0 Then
                                Try
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FrameInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "'")
                                        bHas = rdr.HasRows
                                    End Using
                                    If bHas Then
                                        WriteDebug("Existing frame data for " & wu.PRCG & "-" & wu.unit & " will be removed")
                                        sqlErr = .ExecuteNonQuery("DELETE FROM 'FrameInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "'")
                                    Else
                                        sqlErr = .ExecuteNonQuery("CREATE TABLE 'FrameInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "' (dtPercentage DATETIME, Percentage INTEGER)")
                                    End If
                                    'transactions moved to whole parsing loop

                                    For Each fInfo In wu.Frames
                                        'delegatefactory.bussybox.SetMessage("Storing parse results for " & wu.ClientName & Environment.NewLine & "Downloaded: " & wu.dtDownloaded.ToString & Environment.NewLine & wu.PRCG & Environment.NewLine & "Frame " & fInfo.strPercentage & " - " & fInfo.FrameDT.ToString)
                                        sqlErr = .ExecuteNonQuery("INSERT INTO 'FrameInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "' (dtPercentage, Percentage) VALUES('" & fInfo.utcFrame.ToString("s") & "','" & fInfo.strPercentage & "')")
                                    Next
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                End Try
                            End If

                            If wu.bHasRestarted AndAlso wu.restartInfo.RestartInfo.Count > 0 Then
                                Try
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='RestartInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "'")
                                        bHas = rdr.HasRows
                                    End Using
                                    'mhm delete table.. then recreate might be quicker then deleting all records?
                                    If Not bHas Then
                                        WriteDebug("Creating table for restart info for work unit: " & wu.unit)
                                        sqlErr = .ExecuteNonQuery("CREATE TABLE 'RestartInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "' (dtFrame DATETIME, Frame TEXT, CoreStatus TEXT)")
                                    Else
                                        WriteDebug("Cleaning existing restart information table for unit: " & wu.unit)
                                        sqlErr = .ExecuteNonQuery("DELETE FROM 'RestartInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "'")
                                    End If

                                    'transactions moved to whole parsing loop
                                    For Each rInfo In wu.restartInfo.RestartInfo
                                        'delegatefactory.bussybox.SetMessage("Storing parse results for " & wu.ClientName & Environment.NewLine & "Downloaded: " & wu.dtDownloaded.ToString & " - " & wu.PRCG & "Restart info " & rInfo.CoreStatus & " frame " & rInfo.LastFrame.strPercentage)
                                        sqlErr = .ExecuteNonQuery("INSERT INTO 'RestartInfo_" & FormatSQLString(wu.ClientName, False, True) & "_" & wu.Slot & "_" & wu.unit & "' (dtFrame, Frame, CoreStatus) VALUES('" & rInfo.LastFrame.utcFrame.ToString("s") & sib & rInfo.LastFrame.strPercentage & sib & rInfo.CoreStatus & "')")
                                    Next
                                Catch ex As Exception
                                    WriteError(ex.Message, Err)
                                End Try
                            End If
SkipWU:
                        Next
                        If bTransaction Then .EndTransaction()
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property WorkUnits(ByVal sqlSelectString As String, Optional ByVal SkipDetails As Boolean = True, Optional ShowProgress As Boolean = False, Optional ParentForm As IntPtr = Nothing) As List(Of clsWU)
        Get
            Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False, bConversionLogged As Boolean = False
            WriteDebug("Reading work units from database, query: " & sqlSelectString & " skipdetails: " & SkipDetails.ToString)
            Dim lstWU As New List(Of clsWU)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='WorkUnits'")
                                bHas = rdr.HasRows
                            End Using
                            If Not bHas Then Return lstWU
                            Dim iCount As Int32 = 0
                            If ShowProgress Then
                                Using rdr As SQLiteDataReader = .dataReader("SELECT count(TPF) FROM 'WorkUnits'")
                                    If Not IsNothing(rdr) Then
                                        If rdr.HasRows Then iCount = CInt(rdr.Item(0))
                                    End If
                                End Using
                                If BussyBox.IsFormVisible Then
                                    BussyBox.SetNewProgressBoundries(0, iCount, 0, ProgressBarStyle.Continuous)
                                    BussyBox.SetMessage("Loading work unit history")
                                Else
                                    BussyBox.ShowForm("Loading work unit history", False, Nothing, False, 0, iCount)
                                End If
                            End If
                            'Prepare query string
                            Dim qStr As New StringBuilder
                            qStr.Append("SELECT * FROM 'WorkUnits'")
                            Try
                                If Not sqlSelectString = "" Then
                                    If sqlSelectString.Substring(0, 1) = " " Then
                                        qStr.Append(sqlSelectString)
                                    Else
                                        qStr.Append(" " & sqlSelectString)
                                    End If
                                End If
                            Catch ex As Exception
                                WriteError(ex.Message, Err)
                                bFailure = True
                                GoTo Skip
                            End Try
                            If sqlSelectString = "" Then qStr.Append(" ORDER BY Downloaded DESC")
                            Dim iIndex As Int32 = 0
                            Using rdr As SQLiteDataReader = .dataReader(qStr.ToString)
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        Dim nWU As New clsWU
                                        nWU.unit = CStr(rdr.Item("unit"))
                                        nWU.Slot = CStr(rdr.Item("Slot"))
                                        nWU.ClientName = FormatSQLString(CStr(rdr.Item("Client")), True, True)
                                        nWU.PRCG = "Project:" & CStr(rdr.Item("Project")) & " " & CStr(rdr.Item("RCG"))
                                        nWU.BoardType = CStr(rdr.Item("BoardType"))
                                        nWU.Core = CStr(rdr.Item("core"))
                                        nWU.CoreCompiler = CStr(rdr.Item("CoreCompiler"))
                                        nWU.CoreSnippet = FormatSQLString(CStr(rdr.Item("CoreSnippet")), True)
                                        nWU.CoreStatus = CStr(rdr.Item("CoreStatus"))
                                        nWU.CoreVersion = CStr(rdr.Item("CoreVersion"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.CS = CStr(rdr.Item("CS"))
                                        nWU.WS = CStr(rdr.Item("WS"))
                                        nWU.dtCompleted = CDate(rdr.Item("Completed"))
                                        nWU.dtDownloaded = CDate(rdr.Item("Downloaded"))
                                        nWU.dtStarted = CDate(rdr.Item("Started"))
                                        nWU.dtSubmitted = CDate(rdr.Item("Submitted"))
                                        nWU.HW = CStr(rdr.Item("HW"))
                                        nWU.ServerResponce = CStr(rdr.Item("ServerResponse"))
                                        nWU.PPD = CStr(rdr.Item("PPD"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.UploadSize = CStr(rdr.Item("UploadSize"))
                                        nWU.dtStartUpload = CDate(rdr.Item("StartUpload"))
                                        nWU.dtStartDownload = CDate(rdr.Item("StartDownload"))
                                        nWU.DownloadSize = CStr(rdr.Item("DownloadSize"))
                                        Try
                                            Dim strTPFdb As String = CStr(rdr.Item("TPF"))
                                            If IsNumeric(strTPFdb) Then
                                                nWU.tpfDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(strTPFdb)))
                                            Else
                                                Dim tsTmp As TimeSpan
                                                If TimeSpan.TryParse(strTPFdb, tsTmp) Then
                                                    nWU.tpfDB = FormatTimeSpan(tsTmp)
                                                End If
                                            End If
                                            If Not IsNothing(rdr.Item("TPFmax")) Then
                                                nWU.tpfMaxDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmax"))))
                                            Else
                                                If Not bConversionLogged Then
                                                    WriteLog("Database format is old, needs converting!", eSeverity.Critical)
                                                    bConversionLogged = True
                                                End If
                                            End If
                                            If Not IsNothing(rdr.Item("TPFmin")) Then
                                                nWU.tpfMinDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmin"))))
                                            Else
                                                If Not bConversionLogged Then
                                                    WriteLog("Database format is old, needs converting!", eSeverity.Critical)
                                                    bConversionLogged = True
                                                End If
                                            End If
                                        Catch ex As Exception
                                            If Not bConversionLogged Then
                                                WriteLog("Database format is old, needs converting!", eSeverity.Critical)
                                                bConversionLogged = True
                                            End If
                                        End Try
                                        iIndex += 1
                                        If ShowProgress Then
                                            BussyBox.SetProgress(iIndex)
                                        End If
                                        lstWU.Add(nWU)
                                    End While
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End With
                End Using
                If Not SkipDetails Then
                    'Add frames
                    Try
                        Using dbInstFrames As clsDB.clsDBInstance = dbPool.dbInstance
                            With dbInstFrames
                                If .conState = ConnectionState.Open Then
                                    If ShowProgress Then
                                        BussyBox.SetMessage("Reading back frame data")
                                        BussyBox.SetProgress(0)
                                    End If
                                    Dim iIndex As Int32 = 0
                                    For Each nWU In lstWU
                                        Dim bHas As Boolean = False
                                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                            If Not IsNothing(rdr) Then
                                                bHas = rdr.HasRows
                                            Else
                                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                                bFailure = True
                                                GoTo Skip
                                            End If
                                        End Using
                                        If bHas Then
                                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                                If Not IsNothing(rdr) Then
                                                    While rdr.Read
                                                        nWU.AddFrame("THIS IS A TEST (" & rdr.Item("Percentage").ToString & "%)", CDate(rdr.Item("dtPercentage")))
                                                    End While
                                                Else
                                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                                    bFailure = True
                                                    GoTo Skip
                                                End If
                                            End Using
                                        End If
                                        If ShowProgress Then
                                            iIndex += 1
                                            BussyBox.SetProgress(iIndex)
                                        End If
                                    Next
                                Else
                                    WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End With
                        End Using
                    Catch ex As Exception
                        WriteError(ex.Message, Err)
                        bFailure = True
                        GoTo Skip
                    End Try
                End If
                'Skipdetails shouldn't leave out restart info 
                'If Not SkipDetails Then
                'Add restart info
                Try
                    Using dbInstRestart As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInstRestart
                            If .conState = ConnectionState.Open Then
                                If ShowProgress Then
                                    BussyBox.SetMessage("Reading back restart data")
                                    BussyBox.SetProgress(0)
                                End If
                                Dim iIndex As Int32 = 0

                                For Each nWU In lstWU
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                        If Not IsNothing(rdr) Then
                                            nWU.bHasRestarted = rdr.HasRows
                                        Else
                                            WriteLog("The datareader got disposed", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If
                                    End Using
                                    If nWU.bHasRestarted Then
                                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                            If Not IsNothing(rdr) Then
                                                While rdr.Read
                                                    'THIS IS A TEST IS NEEDED FOR ACCURATE FRAME ADDING 
                                                    nWU.restartInfo.AddRestart(CStr(rdr.Item("CoreStatus")), New clsWU.clsFrame("THIS IS A TEST (" & CStr(rdr.Item("Frame")) & "%)", CDate(rdr.Item("dtFrame"))))
                                                End While
                                            Else
                                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                                bFailure = True
                                                GoTo Skip
                                            End If
                                        End Using
                                    End If
                                    If ShowProgress Then
                                        iIndex += 1
                                        BussyBox.SetProgress(iIndex)
                                    End If
                                Next
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'End If
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bFailure = True
            End Try
Skip:
            If Not bFailure Then WriteDebug("-process took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return lstWU
        End Get
    End Property
    Public ReadOnly Property WorkUnit(ByVal ClientName As String, ByVal SlotID As String, ByVal unit As String, Downloaded As DateTime, Optional SkipDetails As Boolean = False) As clsWU
        Get
            Dim dtNow As DateTime = DateTime.Now, bFailure As Boolean = False
            WriteDebug("Reading work unit from database, client: " & ClientName & " slot: " & SlotID & " unit: " & unit)
            Dim nWU As New clsWU
            Try
                nWU.ClientName = ClientName
                nWU.Slot = SlotID
                nWU.unit = unit
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'WorkUnits' WHERE Client='" & FormatSQLString(ClientName, False, True) & "' AND Slot='" & SlotID & "' AND unit='" & unit & "' AND Downloaded ='" & Downloaded.ToString("s") & "'")
                                If Not IsNothing(rdr) Then
                                    If rdr.Read Then
                                        nWU.unit = CStr(rdr.Item("unit"))
                                        nWU.Slot = CStr(rdr.Item("Slot"))
                                        nWU.ClientName = FormatSQLString(CStr(rdr.Item("Client")), True, True)
                                        nWU.PRCG = "Project:" & CStr(rdr.Item("Project")) & " " & CStr(rdr.Item("RCG"))
                                        nWU.BoardType = CStr(rdr.Item("BoardType"))
                                        nWU.Core = CStr(rdr.Item("core"))
                                        nWU.CoreCompiler = CStr(rdr.Item("CoreCompiler"))
                                        nWU.CoreSnippet = FormatSQLString(CStr(rdr.Item("CoreSnippet")), True)
                                        nWU.CoreStatus = CStr(rdr.Item("CoreStatus"))
                                        nWU.CoreVersion = CStr(rdr.Item("CoreVersion"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.CS = CStr(rdr.Item("CS"))
                                        nWU.WS = CStr(rdr.Item("WS"))
                                        nWU.dtCompleted = CDate(rdr.Item("Completed"))
                                        nWU.dtDownloaded = CDate(rdr.Item("Downloaded"))
                                        nWU.dtStartDownload = CDate(rdr.Item("StartDownload"))
                                        nWU.DownloadSize = CStr(rdr.Item("DownloadSize"))
                                        nWU.iDownloadSize = CDbl(rdr.Item("dblDownloadSize"))
                                        nWU.dtStarted = CDate(rdr.Item("Started"))
                                        nWU.dtSubmitted = CDate(rdr.Item("Submitted"))
                                        nWU.HW = CStr(rdr.Item("HW"))
                                        nWU.ServerResponce = CStr(rdr.Item("ServerResponse"))
                                        nWU.PPD = CStr(rdr.Item("PPD"))
                                        nWU.Credit = CStr(rdr.Item("Credit"))
                                        nWU.UploadSize = CStr(rdr.Item("UploadSize"))
                                        nWU.dtStartUpload = CDate(rdr.Item("StartUpload"))
                                        'nWU.tpfDB = FormatTimeSpan(TimeSpan.Parse(CStr(rdr.Item("TPF"))))
                                        nWU.tpfDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPF"))))
                                        Try
                                            nWU.tpfMaxDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmax"))))
                                            nWU.tpfMinDB = FormatTimeSpan(New TimeSpan(0, 0, CInt(rdr.Item("TPFmin"))))
                                        Catch ex As Exception
                                            WriteLog("The database needs converting!", eSeverity.Critical)
                                        End Try
                                    Else
                                        Return nWU
                                    End If
                                Else
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    bFailure = True
                                    GoTo Skip
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                            bFailure = True
                            GoTo Skip
                        End If
                    End With
                End Using

                Try
                    If SkipDetails Then Exit Try
                    'Add frames
                    Using dbInstFrames As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInstFrames
                            If .conState = ConnectionState.Open Then
                                Dim bHas As Boolean
                                Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                    If IsNothing(rdr) Then
                                        WriteLog("The datareader got disposed", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    Else
                                        bHas = rdr.HasRows
                                    End If
                                End Using
                                If bHas Then
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'FrameInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                        If IsNothing(rdr) Then
                                            WriteLog("The datareader got disposed", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        Else
                                            While rdr.Read
                                                nWU.AddFrame("THIS IS A TEST (" & rdr.Item("Percentage").ToString & "%)", CDate(rdr.Item("dtPercentage")))
                                                Application.DoEvents()
                                            End While
                                        End If
                                    End Using
                                End If
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
                'Add restart info
                Try
                    If SkipDetails Then Exit Try
                    Using dbInstRestart As clsDB.clsDBInstance = dbPool.dbInstance
                        With dbInstRestart
                            If .conState = ConnectionState.Open Then
                                Dim bHas As Boolean = False
                                Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                    If Not IsNothing(rdr) Then
                                        bHas = rdr.HasRows
                                    Else
                                        WriteLog("The datareader got disposed", eSeverity.Critical)
                                        bFailure = True
                                        GoTo Skip
                                    End If
                                End Using
                                If bHas Then
                                    nWU.bHasRestarted = True
                                    Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'RestartInfo_" & FormatSQLString(nWU.ClientName, False, True) & "_" & nWU.Slot & "_" & nWU.unit & "'")
                                        If Not IsNothing(rdr) Then
                                            While rdr.Read
                                                'THIS IS A TEST IS NEEDED FOR ACCURATE FRAME ADDING 
                                                nWU.restartInfo.AddRestart(CStr(rdr.Item("CoreStatus")), New clsWU.clsFrame("THIS IS A TEST (" & CStr(rdr.Item("Frame")) & "%)", CDate(rdr.Item("dtFrame"))))
                                            End While
                                        Else
                                            WriteLog("The datareader got disposed", eSeverity.Critical)
                                            bFailure = True
                                            GoTo Skip
                                        End If

                                    End Using
                                End If
                            Else
                                WriteLog("Can't access the database, there should be more errors in the log", eSeverity.Critical)
                                bFailure = True
                                GoTo Skip
                            End If
                        End With
                    End Using
                Catch ex As Exception
                    WriteError(ex.Message, Err)
                    bFailure = True
                    GoTo Skip
                End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bFailure = True
                GoTo Skip
            End Try
Skip:
            If Not bFailure Then WriteDebug("-process took " & FormatTimeSpan(DateTime.Now.Subtract(dtNow), True))
            Return nWU
        End Get
    End Property
#End Region
#Region "Recredit WorkUnits"
    Private mWuRecredited As Boolean = False
    Friend Property SuccessfullRecredit As Boolean
        Get
            Return mWuRecredited
        End Get
        Set(ByVal value As Boolean)
            mWuRecredited = value
        End Set
    End Property
    Friend ReadOnly Property RecreditAllWorkUnits As Boolean
        Get
            Try
                Dim rVal As Boolean = False
                Dim lWU As List(Of clsWU) = WorkUnits(" WHERE Credit='' AND ServerResponse LIKE '%WORK_ACK%' AND CoreStatus LIKE '%FINISHED_UNIT%'")
                Dim lRecredited As New List(Of clsWU)
                For Each wu In lWU
                    'wu = AccreditWorkunit(wu)
                    If AccreditWorkunit(wu) Then
                        If wu.Credit <> "" AndAlso wu.Credit <> "0" Then
                            rVal = True
                            lRecredited.Add(wu)
                        End If
                    Else

                    End If
                Next
                Using dbInstRecredit As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInstRecredit
                        If .conState = ConnectionState.Open Then
                            Dim bCommit As Boolean = Not .TransactionInProgress
                            If Not .TransactionInProgress Then
                                .BeginTransaction()
                            End If
                            For Each wu In lRecredited
                                'UPDATE WorkUnits SET Credit='787', PPD='3398.31' WHERE Client='MARVIN-PC' AND Slot='02' AND Downloaded='2011-12-12T01:00:16' AND unit='0x5a7ed2154ee5522100030141000616a3'
                                sqlErr = .ExecuteNonQuery("UPDATE WorkUnits SET Credit='" & wu.Credit & "' WHERE Client='" & wu.ClientName & "' AND Slot='" & wu.Slot & "' AND Downloaded='" & wu.utcDownloaded.ToString("s") & "' AND unit='" & wu.unit & "'")
                                sqlErr = .ExecuteNonQuery("UPDATE WorkUnits SET PPD='" & wu.PPD & "' WHERE Client='" & wu.ClientName & "' AND Slot='" & wu.Slot & "' AND Downloaded='" & wu.utcDownloaded.ToString("s") & "' AND unit='" & wu.unit & "'")
                            Next
                            If bCommit Then .EndTransaction()
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
                mWuRecredited = rVal
                Return rVal
            Catch ex As Exception
                mWuRecredited = False
                WriteError(ex.Message, Err)
                Return False
            End Try
        End Get
    End Property
#End Region
#End Region
#Region "EOC XML functions"
    Friend Function InitEOC() As Boolean
        Dim bRet As Boolean = True
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='eoc_accounts'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE eoc_accounts (username TEXT, team TEXT, eocid TEXT, signature TEXT, enabled TEXT)")
                        End If
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='eocupdates'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'eocupdates' (dtupdate DATETIME, username TEXT, teamid TEXT, teamname TEXT, teamrank TEXT, teamactiveusers TEXT, teamusers TEXT, teamchangerankday TEXT, teamchangerankweek TEXT, teampointsdayavg TEXT, teampointsupdate TEXT, teampointstoday TEXT, teampointsweek TEXT, teampoints TEXT, teamwus TEXT, userid TEXT, userteamrank TEXT, useroverallrank TEXT, userchangerankday TEXT, userchangerankweek TEXT, userpointsdayaverage TEXT, userpointsupdate TEXT, userpointstoday TEXT, userpointsweek TEXT, userpoints TEXT, userwus TEXT, strlastupdate TEXT, strunixtimestamp TEXT, updatestatus TEXT)")
                        End If
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='signatureimage'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'signatureimage' (dtUPDATE DATETIME, username TEXT, team TEXT, DATA blob)")
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        bRet = False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    '"CREATE TABLE 'ExtremeOverclocking' (name TEXT, team TEXT, eocid TEXT, signature TEXT, enabled TEXT)"
    Friend Function EOCAccounts() As List(Of EOCInfo.sEOCAccount)
        Dim bRet As New List(Of EOCInfo.sEOCAccount)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='eoc_accounts'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            Return bRet
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return bRet
                    End If
                End With
            End Using
        Catch ex As Exception
            'WriteError(ex.Message, Err)
            Return bRet
        End Try
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'eoc_accounts'")
                            If IsNothing(rdr) Then Exit Try
                            While rdr.Read
                                Dim nA As New EOCInfo.sEOCAccount
                                nA.Username = CStr(rdr.Item("username"))
                                nA.Teamnumber = CStr(rdr.Item("team"))
                                nA.ID = CStr(rdr.Item("eocid"))
                                nA.customIMG = CStr(rdr.Item("signature"))
                                nA.Enabled = CBool(CStr(rdr.Item("enabled")))
                                bRet.Add(nA)
                            End While
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return bRet
    End Function
    Friend ReadOnly Property HasAccount(ByVal UserName As String, ByVal TeamNumber As String) As Boolean
        Get
            Dim bRet As Boolean = False
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'eoc_accounts' WHERE username='" & UserName & "' AND team='" & TeamNumber & "'")
                                bRet = rdr.HasRows
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return bRet
        End Get
    End Property
    Public Function SaveEOCAccounts(ByVal Accounts As List(Of EOCInfo.sEOCAccount)) As Boolean
        Dim bRet As Boolean = True
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        sqlErr = .ExecuteNonQuery("DELETE FROM 'eoc_accounts'")
                        Dim bCommit As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then .BeginTransaction()
                        For Each Account As EOCInfo.sEOCAccount In Accounts
                            sqlErr = .ExecuteNonQuery("INSERT INTO 'eoc_accounts' (username, team, eocid, signature, enabled) VALUES('" & Account.Username & sib & Account.Teamnumber & sib & Account.ID & sib & Account.customIMG & sib & CStr(Account.Enabled) & "')")
                        Next
                        If bCommit Then .EndTransaction()
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        bRet = False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
#Region "XML updates"
    '"CREATE TABLE 'eocupdates' (dtUPDATE DATETIME, user TEXT, team TEXT, DATA blob)"
    Friend Function GetXMLUpdate(ByVal dtUpdate As DateTime, ByVal UserName As String, ByVal TeamNumber As String) As clsEOC.clsUpdates
        Dim xmlUpdate As New clsEOC.clsUpdates
        Try
            Dim mySelectQuery As String = "SELECT * FROM 'eocupdates' WHERE dtupdate='" & dtUpdate.ToString("s") & "' AND username='" & UserName & "' AND teamid='" & TeamNumber & "'"
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Try
                            WriteDebug("Retrieving stored EOC update for " & UserName & "(" & TeamNumber & ") - " & dtUpdate.ToString("s"))
                            Using rdr As SQLiteDataReader = .dataReader(mySelectQuery)
                                ' Always call Read before accessing data.
                                While rdr.Read()
                                    '"CREATE TABLE 'eocupdates' (dtupdate DATETIME, username TEXT, teamid TEXT, teamname TEXT, teamrank TEXT, teamactiveusers TEXT, teamusers TEXT, teamchangerankday TEXT, teamchangerankweek TEXT, teampointsdayavg TEXT, teampointsupdate TEXT, teampointstoday TEXT, teampointsweek TEXT, teampoints TEXT, teamwus TEXT, userid TEXT, userteamrank TEXT, useroverallrank TEXT, userchangerankday TEXT, userchangerankweek TEXT, userpointsdayaverage TEXT, userpointsupdate TEXT, userpointstoday TEXT, userpointsweek TEXT, userpoints TEXT, userwus TEXT, strlastupdate TEXT, strunixtimestamp TEXT, updatestatus TEXT)"
                                    xmlUpdate.Update.User.User_Name = UserName
                                    xmlUpdate.Update.Team.TeamID = TeamNumber
                                    xmlUpdate.Update.User.Change_Rank_24h = CStr(rdr.Item("userchangerankday"))
                                    xmlUpdate.Update.User.Change_Rank_7days = CStr(rdr.Item("userchangerankweek"))
                                    xmlUpdate.Update.User.Overall_Rank = CStr(rdr.Item("useroverallrank"))
                                    xmlUpdate.Update.User.Points = CStr(rdr.Item("userpoints"))
                                    xmlUpdate.Update.User.Points_24h_Avg = CStr(rdr.Item("userpointsdayaverage"))
                                    xmlUpdate.Update.User.Points_Today = CStr(rdr.Item("userpointstoday"))
                                    xmlUpdate.Update.User.Points_Update = CStr(rdr.Item("userpointsupdate"))
                                    xmlUpdate.Update.User.Points_Week = CStr(rdr.Item("userpointsweek"))
                                    xmlUpdate.Update.User.Team_Rank = CStr(rdr.Item("userteamrank"))
                                    xmlUpdate.Update.User.UserID = CStr(rdr.Item("userid"))
                                    xmlUpdate.Update.User.WUs = CStr(rdr.Item("userwus"))
                                    xmlUpdate.Update.Team.Change_Rank_24h = CStr(rdr.Item("teamchangerankday"))
                                    xmlUpdate.Update.Team.Change_Rank_7days = CStr(rdr.Item("teamchangerankweek"))
                                    xmlUpdate.Update.Team.Points = CStr(rdr.Item("teampoints"))
                                    xmlUpdate.Update.Team.TeamName = CStr(rdr.Item("teamname"))
                                    xmlUpdate.Update.Team.Points_24h_Avg = CStr(rdr.Item("teampointsdayavg"))
                                    xmlUpdate.Update.Team.Points_Today = CStr(rdr.Item("teampointstoday"))
                                    xmlUpdate.Update.Team.Points_Update = CStr(rdr.Item("teampointsupdate"))
                                    xmlUpdate.Update.Team.Points_Week = CStr(rdr.Item("teampointsweek"))
                                    xmlUpdate.Update.Team.Rank = CStr(rdr.Item("teamrank"))
                                    xmlUpdate.Update.Team.Users = CStr(rdr.Item("teamusers"))
                                    xmlUpdate.Update.Team.Users_Active = CStr(rdr.Item("teamactiveusers"))
                                    xmlUpdate.Update.Team.WUs = CStr(rdr.Item("teamwus"))
                                    xmlUpdate.Update.UpdateStatus.dtUpdate = CDate(rdr.Item("dtupdate"))
                                    xmlUpdate.Update.UpdateStatus.strLastUpdate = CStr(rdr.Item("strlastupdate"))
                                    xmlUpdate.Update.UpdateStatus.strUnixTimeStamp = CStr(rdr.Item("strunixtimestamp"))
                                    xmlUpdate.Update.UpdateStatus.Update_Status = CStr(rdr.Item("updatestatus"))
                                    xmlUpdate.IsEmpty = False
                                End While
                            End Using
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                        End Try
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return xmlUpdate
    End Function
    Friend Function lasteocupdate(ByVal UserName As String, ByVal Teamnumber As String) As clsEOC.clsUpdates
        Dim xmlUpdate As New clsEOC.clsUpdates
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Try
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'eocupdates' WHERE username='" & UserName & "' AND teamid='" & Teamnumber & "' ORDER BY dtupdate DESC")
                                ' Always call Read before accessing data.
                                If IsNothing(rdr) Then Exit Try
                                If Not rdr.HasRows Then Exit Try
                                If rdr.Read Then
                                    xmlUpdate.Update.User.User_Name = UserName
                                    xmlUpdate.Update.Team.TeamID = Teamnumber
                                    xmlUpdate.Update.User.Change_Rank_24h = CStr(rdr.Item("userchangerankday"))
                                    xmlUpdate.Update.User.Change_Rank_7days = CStr(rdr.Item("userchangerankweek"))
                                    xmlUpdate.Update.User.Overall_Rank = CStr(rdr.Item("useroverallrank"))
                                    xmlUpdate.Update.User.Points = CStr(rdr.Item("userpoints"))
                                    xmlUpdate.Update.User.Points_24h_Avg = CStr(rdr.Item("userpointsdayaverage"))
                                    xmlUpdate.Update.User.Points_Today = CStr(rdr.Item("userpointstoday"))
                                    xmlUpdate.Update.User.Points_Update = CStr(rdr.Item("userpointsupdate"))
                                    xmlUpdate.Update.User.Points_Week = CStr(rdr.Item("userpointsweek"))
                                    xmlUpdate.Update.User.Team_Rank = CStr(rdr.Item("userteamrank"))
                                    xmlUpdate.Update.User.UserID = CStr(rdr.Item("userid"))
                                    xmlUpdate.Update.User.WUs = CStr(rdr.Item("userwus"))
                                    xmlUpdate.Update.Team.Change_Rank_24h = CStr(rdr.Item("teamchangerankday"))
                                    xmlUpdate.Update.Team.Change_Rank_7days = CStr(rdr.Item("teamchangerankweek"))
                                    xmlUpdate.Update.Team.Points = CStr(rdr.Item("teampoints"))
                                    xmlUpdate.Update.Team.TeamName = CStr(rdr.Item("teamname"))
                                    xmlUpdate.Update.Team.Points_24h_Avg = CStr(rdr.Item("teampointsdayavg"))
                                    xmlUpdate.Update.Team.Points_Today = CStr(rdr.Item("teampointstoday"))
                                    xmlUpdate.Update.Team.Points_Update = CStr(rdr.Item("teampointsupdate"))
                                    xmlUpdate.Update.Team.Points_Week = CStr(rdr.Item("teampointsweek"))
                                    xmlUpdate.Update.Team.Rank = CStr(rdr.Item("teamrank"))
                                    xmlUpdate.Update.Team.Users = CStr(rdr.Item("teamusers"))
                                    xmlUpdate.Update.Team.Users_Active = CStr(rdr.Item("teamactiveusers"))
                                    xmlUpdate.Update.Team.WUs = CStr(rdr.Item("teamwus"))
                                    xmlUpdate.Update.UpdateStatus.dtUpdate = CDate(rdr.Item("dtupdate"))
                                    xmlUpdate.Update.UpdateStatus.strLastUpdate = CStr(rdr.Item("strlastupdate"))
                                    xmlUpdate.Update.UpdateStatus.strUnixTimeStamp = CStr(rdr.Item("strunixtimestamp"))
                                    xmlUpdate.Update.UpdateStatus.Update_Status = CStr(rdr.Item("updatestatus"))
                                    xmlUpdate.IsEmpty = False
                                End If
                            End Using
                        Catch ex As Exception
                            'WriteError(ex.Message, Err)
                        End Try
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return xmlUpdate
    End Function
    Friend ReadOnly Property EOC_dtLastUpdate(ByVal UserName As String, ByVal Teamnumber As String) As DateTime
        Get
            Dim dtRet As DateTime = #1/1/2000#
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst

                        If .conState = ConnectionState.Open Then
                            Using sqReader As SQLiteDataReader = .dataReader("SELECT dtupdate FROM 'eocupdates' WHERE username='" & UserName & "' AND teamid='" & Teamnumber & "' ORDER BY sigDate DESC")
                                ' Always call Read before accessing data.
                                If Not sqReader.HasRows Then Exit Try
                                sqReader.Read()
                                dtRet = CDate(sqReader.Item("dtupdate"))
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return dtRet
        End Get
    End Property
    Friend Function GetXMLUpdateTimeStamps(ByVal UserName As String, ByVal TeamNumber As String) As List(Of DateTime)
        Dim lSigDate As New List(Of DateTime)
        Try
            Dim mySelectQuery As String = "SELECT dtupdate FROM 'eocupdates' WHERE username='" & UserName & "' AND teamid='" & TeamNumber & "' ORDER BY dtupdate DESC"
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst

                    If .conState = ConnectionState.Open Then
                        Using sqReader As SQLiteDataReader = .dataReader(mySelectQuery)
                            ' Always call Read before accessing data.
                            While sqReader.Read()
                                lSigDate.Add(CDate(sqReader.Item("dtupdate")))
                            End While
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return lSigDate
    End Function
    Friend Function InsertXMLUpdate(ByVal NewUpdate As clsEOC.clsUpdates) As Boolean
        Dim bRet As Boolean = False
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        '"CREATE TABLE 'eocupdates' (dtupdate DATETIME, username TEXT, teamid TEXT, teamname TEXT, teamrank TEXT, teamactiveusers TEXT, teamusers TEXT, teamchangerankday TEXT, teamchangerankweek TEXT, teampointsdayavg TEXT, teampointsupdate TEXT, teampointstoday TEXT, teampointsweek TEXT, teampoints TEXT, teamwus TEXT, userid TEXT, userteamrank TEXT, useroverallrank TEXT, userchangerankday TEXT, userchangerankweek TEXT, userpointsdayaverage TEXT, userpointsupdate TEXT, userpointstoday TEXT, userpointsweek TEXT, userpoints TEXT, userwus TEXT, strlastupdate TEXT, strunixtimestamp TEXT, updatestatus TEXT)"
                        Dim sQuery As String = "INSERT INTO 'eocupdates' (dtupdate, username, teamid, teamname, teamrank, teamactiveusers, teamusers, teamchangerankday, teamchangerankweek, teampointsdayavg, teampointsupdate, teampointstoday, teampointsweek, teampoints, teamwus, userid, userteamrank, useroverallrank, userchangerankday, userchangerankweek, userpointsdayaverage, userpointsupdate, userpointstoday, userpointsweek, userpoints, userwus, strlastupdate, strunixtimestamp, updatestatus) VALUES('" & NewUpdate.Last_Update_LocalTime.ToString("s") & sib & NewUpdate.Update.User.User_Name & sib & NewUpdate.Update.Team.TeamID & sib & NewUpdate.Update.Team.TeamName & sib & NewUpdate.Update.Team.Rank & sib & NewUpdate.Update.Team.Users_Active & sib & NewUpdate.Update.Team.Users & sib & NewUpdate.Update.Team.Change_Rank_24h & sib & NewUpdate.Update.Team.Change_Rank_7days & sib & NewUpdate.Update.Team.Points_24h_Avg & sib & NewUpdate.Update.Team.Points_Update & sib & NewUpdate.Update.Team.Points_Today & sib & NewUpdate.Update.Team.Points_Week & sib & NewUpdate.Update.Team.Points & sib & NewUpdate.Update.Team.WUs & sib & NewUpdate.Update.User.UserID & sib & NewUpdate.Update.User.Team_Rank & sib & NewUpdate.Update.User.Overall_Rank & sib & NewUpdate.Update.User.Change_Rank_24h & sib & NewUpdate.Update.User.Change_Rank_7days & sib & NewUpdate.Update.User.Points_24h_Avg & sib & NewUpdate.Update.User.Points_Update & sib & NewUpdate.Update.User.Points_Today & sib & NewUpdate.Update.User.Points_Week & sib & NewUpdate.Update.User.Points & sib & NewUpdate.Update.User.WUs & sib & NewUpdate.Update.UpdateStatus.strLastUpdate & sib & NewUpdate.Update.UpdateStatus.strUnixTimeStamp & sib & NewUpdate.Update.UpdateStatus.Update_Status & "')"
                        sqlErr = SQLiteErrorCode.Ok
                        sqlErr = .ExecuteNonQuery(sQuery)
                        WriteLog("Stored EOC update " & NewUpdate.Last_Update_LocalTime.ToString("s") & " - " & NewUpdate.Update.User.User_Name & "(" & NewUpdate.Update.Team.TeamName & ")")
                        bRet = True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError("clsStatistics_InsertXMLUpdate", Err)
        End Try
        Return bRet
    End Function
    Friend Function GetAllXMLUpdates(ByVal UserName As String, ByVal TeamNumber As String) As List(Of clsEOC.clsUpdates)
        Dim alUpdates As New List(Of clsEOC.clsUpdates)
        Dim ts As New List(Of DateTime)
        Try
            ts = GetXMLUpdateTimeStamps(UserName, TeamNumber)
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Try
            For xInt As Int32 = 0 To ts.Count - 1
                alUpdates.Add(GetXMLUpdate(ts(xInt), UserName, TeamNumber))
            Next
            Return alUpdates
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return alUpdates
    End Function
#End Region
#Region "SignatureImage"
    Private Function BlobToImage(ByVal blob As Object) As Bitmap
        Try
            Dim mStream As System.IO.Stream = Nothing
            Dim bm As Bitmap = Nothing
            Try
                mStream = New System.IO.MemoryStream
                Dim pData() As Byte = DirectCast(blob, Byte())
                mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
                bm = New Bitmap(mStream, False)
            Finally
                If mStream IsNot Nothing Then mStream = Nothing
            End Try
            Return bm
        Catch ex As Exception
            WriteError("BlobToImage", Err)
            Return Nothing
        End Try
    End Function
    '"CREATE TABLE 'signatureimage' (dtUPDATE DATETIME, user TEXT, team, TEXT, DATA blob)"
    Friend ReadOnly Property signatureDTs(ByVal UserName As String, ByVal TeamNumber As String) As List(Of DateTime)
        Get
            Dim rVal As New List(Of DateTime)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using SQLreader As SQLiteDataReader = .dataReader("SELECT dtUPDATE FROM 'signatureimage' WHERE username='" & UserName & "' AND team='" & TeamNumber & "' ORDER BY dtUPDATE DESC")
                                If Not IsNothing(SQLreader) Then
                                    While SQLreader.Read
                                        If Not IsDBNull(SQLreader.Item("dtUPDATE")) And Not IsNothing(SQLreader.Item("dtUPDATE")) Then
                                            rVal.Add(CDate(CStr(SQLreader.Item("dtUPDATE"))))
                                        End If
                                    End While
                                Else
                                    WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
    Friend ReadOnly Property signatureImages(ByVal UserName As String, ByVal TeamNumber As String) As List(Of Image)
        Get
            Dim rVal As New List(Of Image)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using SQLreader As SQLiteDataReader = .dataReader("SELECT DATA FROM 'signatureimage' WHERE username='" & UserName & "' AND team='" & TeamNumber & "' ORDER BY dtUPDATE DESC")
                                SQLreader.Read()
                                rVal.Add(BlobToImage(SQLreader("DATA")))
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
    Friend Function GetSIGImage(ByVal UserName As String, ByVal TeamNumber As String, Optional ByVal dtSignature As DateTime = #1/1/2000#) As Image
        Dim tImg As Image = Nothing
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        If dtSignature = #1/1/2000# Then
                            Using SQLreader As SQLiteDataReader = .dataReader("SELECT * FROM 'signatureimage' WHERE username='" & UserName & "' AND team='" & TeamNumber & "' ORDER BY dtUPDATE DESC")
                                If Not IsDBNull(SQLreader.Item("DATA")) Then
                                    tImg = BlobToImage(SQLreader("DATA"))
                                Else
                                    Exit Try
                                End If
                            End Using
                        Else
                            Using SQLreader As SQLiteDataReader = .dataReader("SELECT * FROM 'signatureimage' WHERE username='" & UserName & "' AND team='" & TeamNumber & "' AND dtUPDATE='" & dtSignature.ToString("s") & "'")
                                If Not IsDBNull(SQLreader.Item("DATA")) Then
                                    tImg = BlobToImage(SQLreader("DATA"))
                                Else
                                    Exit Try
                                End If
                            End Using
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return tImg
    End Function
    Friend Function SaveSIGImage(ByVal SigDate As DateTime, ByVal UserName As String, ByVal TeamNumber As String, ByVal bArr() As Byte) As Boolean
        Dim bRet As Boolean = False
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim EOCcmd As SQLiteCommand = .GetSQLCommand
                        EOCcmd.CommandText = "INSERT INTO 'signatureimage' (dtUPDATE, username, team, DATA) VALUES ('" & SigDate.ToString("s") & "','" & UserName & "','" & TeamNumber & "',@image)"
                        Dim SQLparm As New SQLiteParameter("@image")
                        SQLparm.DbType = DbType.Binary
                        SQLparm.Value = bArr
                        EOCcmd.Parameters.Add(SQLparm)
                        Try
                            EOCcmd.ExecuteNonQuery()
                            bRet = True
                        Catch ex As Exception
                            WriteError(ex.Message, Err)
                            bRet = False
                        End Try
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
        Return bRet
    End Function
#End Region
#End Region
#Region "Settings"
    'Store options in db
    Friend ReadOnly Property IsUpgrading() As Boolean
        Get
            If Not HasSettings Then Return True
            Dim bRet As Boolean = False
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using rdr As SQLiteDataReader = .dataReader("SELECT Version FROM 'Settings'")
                                If Not IsNothing(rdr) Then
                                    Return CBool(My.Application.Info.Version.ToString <> CStr(rdr.Item(0)))
                                Else
                                    If My.Application.Info.Version.ToString = "0.1.0.7" Then
                                        Return True
                                    Else
                                        WriteLog("The datareader got disposed", eSeverity.Critical)
                                        Return True 'return true, show advanced settings!
                                    End If
                                End If
                                bRet = rdr.HasRows
                            End Using
                            If bRet Then
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'Settings'")
                                    bRet = rdr.HasRows
                                End Using
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bRet = False
            End Try
            Return bRet
        End Get
    End Property
    Friend Function StoreSettings(ByVal Settings As Dictionary(Of String, String)) As Boolean
        Dim bRet As Boolean = False
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bR As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Settings'")
                            bR = rdr.HasRows
                        End Using
                        If Not bR Then
                            'Create table
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'Settings' (Setting TEXT, Value TEXT)")
                        Else
                            'Check for stored settings
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM Settings")
                                bR = rdr.HasRows
                            End Using
                            If bR Then
                                'Clear table
                                sqlErr = .ExecuteNonQuery("DELETE FROM Settings")
                            End If
                        End If
                        Dim bCommit As Boolean = Not .TransactionInProgress
                        If Not .TransactionInProgress Then
                            .BeginTransaction()
                        End If
                        For xInt As Int32 = 0 To Settings.Count - 1
                            sqlErr = .ExecuteNonQuery("INSERT INTO Settings (Setting, Value) VALUES ('" & Settings.Keys(xInt).ToString & "','" & Settings.Values(xInt).ToString & "')")
                        Next
                        sqlErr = .ExecuteNonQuery("INSERT INTO Settings (Setting, Value) VALUES ('Version','" & My.Application.Info.Version.ToString & "')")
                        If bCommit Then .EndTransaction()
                        bRet = True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    Friend Function ReadSettings() As Dictionary(Of String, String)
        Dim rVal As New Dictionary(Of String, String)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bR As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Settings'")
                            bR = rdr.HasRows
                        End Using
                        If Not bR Then
                            'Create table
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'Settings' (Setting TEXT, Value TEXT)")
                        Else
                            'Read table
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'Settings'")
                                While rdr.Read
                                    If Not rVal.ContainsKey(CStr(rdr.Item(0))) Then
                                        rVal.Add(CStr(rdr.Item(0)), CStr(rdr.Item(1)))
                                    End If
                                End While
                            End Using
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
    Friend ReadOnly Property HasSettings() As Boolean
        Get
            Dim bRet As Boolean = False
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst

                        If .conState = ConnectionState.Open Then
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Settings'")
                                bRet = rdr.HasRows
                            End Using
                            If bRet Then
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'Settings'")
                                    bRet = rdr.HasRows
                                End Using
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bRet = False
            End Try
            Return bRet
        End Get
    End Property
    Friend Sub ClearSettings()
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='Settings'")
                            bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DROP TABLE 'Settings'")
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
#Region "ColumnSettings"
    Friend Sub ClearColumnSettings()
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim lColumns As New List(Of String)
                        '_columns_master : _columns_info
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name like '%_columns_%'")
                            If Not IsNothing(rdr) Then
                                While rdr.Read
                                    lColumns.Add(CStr(rdr.Item(0)))
                                End While
                            End If
                        End Using
                        For Each Table As String In lColumns
                            sqlErr = .ExecuteNonQuery("DROP TABLE '" & Table & "'")
                        Next
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property lvMasterTable(listview As ListView) As String
        Get
            Return (listview.FindForm.Name & "_" & listview.Name & "_columns_master").ToLowerInvariant
        End Get
    End Property
    Friend ReadOnly Property lvColumnTable(listview As ListView) As String
        Get
            Return (listview.FindForm.Name & "_" & listview.Name & "_columns_info").ToLowerInvariant
        End Get
    End Property
    Protected Friend Function UpdateColumnVisible(ListView As ListView, Header As String, Visible As Boolean) As Boolean
        Try
            Dim strId As String = lvMasterTable(ListView)
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bOld As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT Visible FROM '" & strId & "' WHERE Header='" & Header & "'")
                            If Not IsNothing(rdr) AndAlso rdr.HasRows Then
                                bOld = CBool(CStr(rdr.Item(0)))
                            Else
                                If IsNothing(rdr) Then
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                Else
                                    WriteLog("Table " & strId & " has no value for " & Header, eSeverity.Critical)
                                End If
                                Return False
                            End If
                        End Using
                        sqlErr = .ExecuteNonQuery("UPDATE '" & strId & "' SET Visible='" & Visible & "' WHERE Header='" & Header & "'")
                        Return True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Protected Friend Function CreateColumnMaster(Listview As ListView) As Boolean
        Try
            Dim Identifier As String = lvMasterTable(Listview)
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='" & Identifier & "'")
                            If Not IsNothing(rdr) Then bHas = rdr.HasRows
                        End Using
                        If bHas Then Return True
                        sqlErr = .ExecuteNonQuery("CREATE TABLE '" & Identifier & "' (ColumnIndex INTEGER, DisplayIndex INTEGER, Header TEXT, Width INTEGER, Visible TEXT)")
                        For Each CHinfo As ColumnHeader In Listview.Columns
                            sqlErr = .ExecuteNonQuery("INSERT INTO '" & Identifier & "' (ColumnIndex, DisplayIndex, Header, Width, Visible) VALUES('" & CHinfo.Index & sib & CHinfo.DisplayIndex & sib & CHinfo.Text & sib & CHinfo.Width & sib & Boolean.TrueString & "')")
                        Next
                        Return True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Protected Friend Function GetColumnMasters() As Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Dim rVal As New Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim Names As New List(Of String)
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name LIKE '%_columns_master'")
                            If Not IsNothing(rdr) Then
                                If Not rdr.HasRows Then
                                    WriteLog("No stored master column info")
                                    Exit Try
                                End If
                                While rdr.Read
                                    Names.Add(CStr(rdr.Item(0)))
                                End While
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Exit Try
                            End If
                        End Using
                        For Each table As String In Names
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM '" & table & "'")
                                If IsNothing(rdr) Then
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Exit Try
                                Else
                                    If rdr.HasRows Then
                                        Dim lCHInfos As New SortedList(Of Int32, sColumnInfo)
                                        While rdr.Read
                                            Dim CHInfo As New sColumnInfo
                                            CHInfo.Index = CInt(rdr.Item("ColumnIndex"))
                                            CHInfo.DisplayIndex = CInt(rdr.Item("DisplayIndex"))
                                            CHInfo.Header = CStr(rdr.Item("Header"))
                                            CHInfo.Width = CInt(rdr.Item("Width"))
                                            CHInfo.Visible = CBool(CStr(rdr.Item("Visible")))
                                            lCHInfos.Add(CHInfo.Index, CHInfo)
                                        End While
                                        rVal.Add(table, lCHInfos)
                                    End If
                                End If
                            End Using
                        Next
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Exit Try
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
    Protected Friend Function GetColumnSettings() As Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Dim rVal As New Dictionary(Of String, SortedList(Of Int32, sColumnInfo))
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim Names As New List(Of String)
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name LIKE '%_columns_info'")
                            If Not IsNothing(rdr) Then
                                If Not rdr.HasRows Then
                                    WriteLog("No stored column info")
                                    Exit Try
                                End If
                                While rdr.Read
                                    Names.Add(CStr(rdr.Item(0)))
                                End While
                            Else
                                WriteLog("The datareader got disposed", eSeverity.Critical)
                                Exit Try
                            End If
                        End Using
                        For Each table As String In Names
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM '" & table & "'")
                                If IsNothing(rdr) Then
                                    WriteLog("The datareader got disposed", eSeverity.Critical)
                                    Exit Try
                                Else
                                    If rdr.HasRows Then
                                        Dim lCHInfos As New SortedList(Of Int32, sColumnInfo)
                                        Dim lNewVisible As New List(Of sColumnInfo)
                                        While rdr.Read
                                            Dim CHInfo As New sColumnInfo
                                            CHInfo.Index = CInt(rdr.Item("ColumnIndex"))
                                            CHInfo.DisplayIndex = CInt(rdr.Item("DisplayIndex"))
                                            CHInfo.Header = CStr(rdr.Item("Header"))
                                            CHInfo.Width = CInt(rdr.Item("Width"))
                                            CHInfo.Visible = CBool(CStr(rdr.Item("Visible")))
                                            If Not lCHInfos.ContainsKey(CHInfo.DisplayIndex) Then
                                                lCHInfos.Add(CHInfo.DisplayIndex, CHInfo)
                                            Else
                                                lNewVisible.Add(CHInfo)
                                            End If
                                        End While
                                        If lNewVisible.Count > 0 Then
                                            Dim iMax As Int32 = lCHInfos.Keys.Max
                                            For Each nVisible As sColumnInfo In lNewVisible
                                                iMax += 1
                                                nVisible.DisplayIndex = iMax
                                                lCHInfos.Add(iMax, nVisible)
                                            Next
                                        End If
                                        rVal.Add(table, lCHInfos)
                                    End If
                                End If
                            End Using
                        Next
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Exit Try
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
    Protected Friend Function UpdateColumnSettings(ListView As ListView) As Boolean
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False, Identifier As String = lvColumnTable(ListView)
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='" & Identifier & "'")
                            If Not IsNothing(rdr) Then bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DROP TABLE '" & Identifier & "'")
                        End If
                        sqlErr = .ExecuteNonQuery("CREATE TABLE '" & Identifier & "' (ColumnIndex INTEGER, DisplayIndex INTEGER, Header TEXT, Width INTEGER, Visible TEXT)")
                        For Each CH As ColumnHeader In ListView.Columns
                            sqlErr = .ExecuteNonQuery("INSERT INTO '" & Identifier & "' (ColumnIndex, DisplayIndex, Header, Width, Visible) VALUES('" & CH.Index & sib & CH.DisplayIndex & sib & CH.Text & sib & CH.Width & sib & Boolean.TrueString & "')")
                        Next
                        Return True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        Return False
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#End Region
#Region "Legacy NonFatal CoreStatusMessages"
    Friend Sub ClearNonFatal()
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='LegacyNonFatal'")
                            bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DROP TABLE 'LegacyNonFatal'")
                            sqlErr = .ExecuteNonQuery("VACUUM")
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub UpdateLegacyNonFatal(NonFatal As List(Of String))
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='LegacyNonFatal'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'LegacyNonFatal' (Code TEXT)")
                        Else
                            sqlErr = .ExecuteNonQuery("DELETE FROM 'LegacyNonFatal'")
                        End If
                        For Each Code As String In NonFatal
                            sqlErr = .ExecuteNonQuery("INSERT INTO 'LegacyNonFatal' (Code) VALUES('" & Code & "')")
                        Next
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property LegacyNonFatal As List(Of String)
        Get
            Dim rVal As New List(Of String)
            rVal.AddRange({"6E", "62", "64"})
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean = False
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='LegacyNonFatal'")
                                bHas = rdr.HasRows
                            End Using
                            If Not bHas Then Exit Try
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'LegacyNonFatal'")
                                If IsNothing(rdr) Then
                                    WriteLog("LegacyNonFatal::The datareader got disposed", eSeverity.Critical)
                                    Exit Try
                                Else
                                    While rdr.Read
                                        rVal.Add(CStr(rdr.Item(0)))
                                    End While
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return rVal
        End Get
    End Property
#End Region
#Region "Affinity/Priority settings"
    Friend Sub ClearAffinitySettings()

    End Sub

#End Region
#Region "mail settings"
    Friend Sub ClearMailSettings()

    End Sub
    Friend Function GetMailSettings() As Dictionary(Of String, Mail.EmailProvider)
        Dim rVal As New Dictionary(Of String, Mail.EmailProvider)
        Try

        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return rVal
    End Function
#End Region
#End Region
#Region "Stored Exceptions"
    Friend Sub ClearExceptions()
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='StoredExceptions'")
                            bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DROP TABLE 'StoredExceptions'")
                            sqlErr = .ExecuteNonQuery("VACUUM")
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    ' TODO formatsqlstring
    Friend Sub AddOrUpdateException(ExceptionToStore As StoredExceptions.Exceptions)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='StoredExceptions'")
                            bHas = rdr.HasRows
                        End Using
                        If Not bHas Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'StoredExceptions' (ExcSource TEXT, ExcMessage TEXT, ErrNumber TEXT, ErrText TEXT, Reported TEXT)")
                        End If
                        'check for update
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'StoredExceptions' WHERE ExcSource='" & FormatSQLString(ExceptionToStore.Source, False) & "' AND ErrText='" & FormatSQLString(ExceptionToStore.ErrText, False) & "')")
                            If Not IsNothing(rdr) AndAlso rdr.HasRows Then
                                sqlErr = .ExecuteNonQuery("UPDATE 'StoredException' SET Reported='" & ExceptionToStore.IsReported.ToString & "' WHERE ExcSource='" & FormatSQLString(ExceptionToStore.Source, False) & "' AND ErrText='" & FormatSQLString(ExceptionToStore.ErrText, False) & "'")
                            Else
                                sqlErr = .ExecuteNonQuery("INSERT INTO 'StoredExceptions' (ExcSource, ExcMessage, ErrNumber, ErrText, Reported) VALUES ('" & FormatSQLString(ExceptionToStore.Source, False) & sib & FormatSQLString(ExceptionToStore.ExceptionMessage, False) & sib & FormatSQLString(ExceptionToStore.ErrorNumber, False) & sib & FormatSQLString(ExceptionToStore.ErrText, False) & sib & ExceptionToStore.IsReported.ToString & "')")
                            End If
                        End Using
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Sub RemoveException(StoredException As StoredExceptions.Exceptions)
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='StoredExceptions'")
                            bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DELETE FROM 'StoredExceptions' WHERE ExcSource='" & FormatSQLString(StoredException.Source, False, True) & "' AND ErrText='" & FormatSQLString(StoredException.ErrText, False, True) & "'")
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'StoredExceptions' WHERE ExcSource='" & StoredException.Source & "' AND ErrText='" & StoredException.ErrText & "'")
                                If IsNothing(rdr) Then
                                    WriteLog("The data reader got disposed trying to verify removal of an stored exception", eSeverity.Critical)
                                Else
                                    WriteDebug("Removed stored exception " & StoredException.Source & "#" & StoredException.ExceptionMessage & ": " & rdr.HasRows.ToString)
                                    'WriteDebug("Removed stored exception " & Exceptions.StoredExceptions.StoredExceptionIdentifier(StoredException) & ": " & rdr.HasRows.ToString)
                                End If
                            End Using
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub

    Friend Function GetStoredExceptions() As Dictionary(Of String, StoredExceptions.Exceptions)
        Try
            Dim dRet As New Dictionary(Of String, StoredExceptions.Exceptions)
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Dim bHas As Boolean = False
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='StoredExceptions'")
                                bHas = rdr.HasRows
                            End Using
                            If Not bHas Then Exit Try
                            Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'StoredExceptions'")
                                If Not IsNothing(rdr) Then
                                    While rdr.Read
                                        Dim TheException As New StoredExceptions.Exceptions(FormatSQLString(CStr(rdr.Item(0)), True), FormatSQLString(CStr(rdr.Item(1)), True), FormatSQLString(CStr(rdr.Item(2)), True), FormatSQLString(CStr(rdr.Item(3)), True), CBool(CStr(rdr.Item(4))))
                                        If Not dRet.ContainsKey(Exceptions.StoredExceptions.StoredExceptionIdentifier(TheException)) Then
                                            dRet.Add(Exceptions.StoredExceptions.StoredExceptionIdentifier(TheException), TheException)
                                        Else
                                            WriteLog("This exception was already added", eSeverity.Critical)
                                        End If
                                    End While
                                Else
                                    WriteLog("The datareader got disposed, there could be more errors", eSeverity.Critical)
                                    Exit Try
                                End If
                            End Using
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try

            Return dRet
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return New Dictionary(Of String, StoredExceptions.Exceptions)
        End Try
    End Function
#End Region
#Region "Remote clients"
    Friend Sub ClearRemoteClients()
        Try
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst
                    If .conState = ConnectionState.Open Then
                        Dim bHas As Boolean = False
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='RemoteClients'")
                            bHas = rdr.HasRows
                        End Using
                        If bHas Then
                            sqlErr = .ExecuteNonQuery("DROP TABLE 'RemoteClients'")
                            sqlErr = .ExecuteNonQuery("VACUUM")
                        End If
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend ReadOnly Property HasRemoteClients() As Boolean
        Get
            Dim bRet As Boolean = False
            Try
                Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                    With dbInst
                        If .conState = ConnectionState.Open Then
                            Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='RemoteClients'")
                                bRet = rdr.HasRows
                            End Using
                            If bRet Then
                                Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'RemoteClients'")
                                    bRet = rdr.HasRows
                                End Using
                            End If
                        Else
                            WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                        End If
                    End With
                End Using
            Catch ex As Exception
                WriteError(ex.Message, Err)
                bRet = False
            End Try
            Return bRet
        End Get
    End Property
    Friend Function SaveRemoteClients() As Boolean
        Dim bRet As Boolean = False
        Try
            If IsNothing(Clients.Clients) Then
                WriteLog("Attempt to save remote clients when none are specified.", eSeverity.Debug)
                Exit Try
            End If
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst

                    If .conState = ConnectionState.Open Then
                        '.BeginTransaction()
                        Dim b As Boolean
                        Using rdr As SQLiteDataReader = .dataReader("SELECT name from sqlite_master WHERE name='RemoteClients'")
                            b = rdr.HasRows
                        End Using
                        If Not b Then
                            sqlErr = .ExecuteNonQuery("CREATE TABLE 'RemoteClients' (ClientName TEXT, ClientLocation TEXT, FCPort TEXT, PWD TEXT, FWPort TEXT, Enabled TEXT)")
                        Else
                            sqlErr = .ExecuteNonQuery("DELETE FROM RemoteClients")
                        End If
                        If Clients.Clients.Count <= 1 Then Return True
                        For xInt As Int32 = 1 To Clients.Clients.Count - 1
                            sqlErr = .ExecuteNonQuery("INSERT INTO 'RemoteClients' (ClientName, ClientLocation, FCPort, PWD, FWPort, Enabled) VALUES ('" & Clients.Clients(xInt).ClientName & "','" & Clients.Clients(xInt).ClientLocation & "','" & Clients.Clients(xInt).FCPort & "','" & Clients.Clients(xInt).PWD & "','" & Clients.Clients(xInt).FWPort & "','" & Clients.Clients(xInt).Enabled.ToString & "')")
                            If sqlErr <> SQLiteErrorCode.Ok Then
                                'WriteLog("Sqlite threw an error - " & sqlErr.ToString, eSeverity.Critical)
                            End If
                        Next
                        '.EndTransaction()
                        bRet = True
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
            bRet = False
        End Try
        Return bRet
    End Function
    Friend Function ReadRemoteClients() As Boolean
        'Should only be called AFTER initializing local client!
        If Not HasRemoteClients() Then Return False
        Dim bRet As Boolean = False
        Try
            If Clients.Clients.Count = 0 Then
                WriteLog("Attempt to read remote clients before initializing the local client", eSeverity.Debug)
                Exit Try
            End If
            Using dbInst As clsDB.clsDBInstance = dbPool.dbInstance
                With dbInst

                    If .conState = ConnectionState.Open Then
                        Dim iOld As Int32 = Clients.Clients.Count
                        Using rdr As SQLiteDataReader = .dataReader("SELECT * FROM 'RemoteClients'")
                            While rdr.Read
                                Clients.AddClient(CStr(rdr.Item(0)), CStr(rdr.Item(1)), CStr(rdr.Item(2)), CStr(rdr.Item(3)), CStr(rdr.Item(4)), CBool(rdr.Item(5)))
                            End While
                        End Using
                        bRet = Clients.Clients.Count > iOld
                    Else
                        WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    End If
                End With
            End Using
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
        Return bRet
    End Function
#End Region
#Region "Internal functions for future use"
    Private Function HasTable(dbInst As clsDB.clsDBInstance, TableName As String) As ReturnCheck
        Dim rCheck As New ReturnCheck
        Try
            With dbInst
                If .conState = ConnectionState.Open Then
                    Using rdr As SQLiteDataReader = .dataReader("SELECT name FROM sqlite_master WHERE name='" & TableName & "'")
                        If Not IsNothing(rdr) Then
                            rCheck.ReturnValue = rdr.HasRows
                        Else
                            rCheck.ReturnValue = False
                        End If
                    End Using
                Else
                    WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    rCheck.HasError = True
                    rCheck.Exception = New Exception(ReturnCheck.ClosedConnection)
                End If
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
            rCheck.HasError = True
            rCheck.Exception = ex
            rCheck.ErrObj = Err()
        End Try
        Return rCheck
    End Function
    Private Function CreateTable(dbInst As clsDB.clsDBInstance, SqlString As String) As ReturnCheck
        Dim rCheck As New ReturnCheck
        Try
            With dbInst
                If .conState = ConnectionState.Open Then
                    sqlErr = .ExecuteNonQuery(SqlString)
                Else
                    WriteLog("Can't access the database, there should be more errors", eSeverity.Critical)
                    rCheck.HasError = True
                    rCheck.Exception = New Exception(ReturnCheck.ClosedConnection)
                End If
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
            rCheck.HasError = True
            rCheck.Exception = ex
            rCheck.ErrObj = Err()
        End Try
        Return rCheck
    End Function
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                dbPool.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Friend Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class