Imports System.Data
Imports System.Data.SqlClient

Public Class SqlTableCreator

#Region "Instance Variables"

    Private _connection As SqlConnection

    Public Property Connection() As SqlConnection

        Get
            Return _connection
        End Get
        Set(ByVal value As SqlConnection)
            Connection = value
        End Set
    End Property

    Private _transaction As SqlTransaction
    Public Property Transaction() As SqlTransaction

        Get
            Return _transaction
        End Get
        Set(ByVal value As SqlTransaction)
            _transaction = value
        End Set
    End Property
    Private _tableName As String
    Public Property DestinationTableName() As String
        Get
            Return _tableName
        End Get
        Set(ByVal value As String)
            _tableName = value
        End Set

    End Property

#End Region

#Region "Constructor"
    Public Sub New()

    End Sub
    Public Sub New(ByVal connection As SqlConnection)
        Me.New(connection, Nothing)
    End Sub
    Public Sub New(ByVal connection As SqlConnection, ByVal transaction As SqlTransaction)
        _connection = connection
        _transaction = transaction

    End Sub
#End Region

#Region "Instance Methods"
    Public Function Create(ByVal schema As DataTable) As Object
        Return Create(schema, Nothing)
    End Function

    Public Shared Function GetCreateSQL(ByVal tableName As String, ByVal schema As DataTable, ByVal primaryKeys As Integer()) As String
        Dim sql As String = "CREATE TABLE [" & tableName & "] (" & vbLf
        ' columns
        For Each column As DataRow In schema.Rows
            If Not (schema.Columns.Contains("IsHidden") AndAlso CBool(column("IsHidden"))) Then
                sql += vbTab & "[" & column("ColumnName").ToString() & "] " & SQLGetType(column)
                If schema.Columns.Contains("AllowDBNull") AndAlso CBool(column("AllowDBNull")) = False Then
                    sql += " NOT NULL"
                End If
                sql += "," & vbLf
            End If
        Next
        sql = sql.TrimEnd(New Char() {","c, ControlChars.Lf}) & vbLf
        ' primary keys
        Dim pk As String = ", CONSTRAINT PK_" & tableName & " PRIMARY KEY CLUSTERED ("
        Dim hasKeys As Boolean = (primaryKeys IsNot Nothing AndAlso primaryKeys.Length > 0)
        If hasKeys Then
            ' user defined keys
            For Each key As Integer In primaryKeys
                pk += schema.Rows(key)("ColumnName").ToString() & ", "
            Next
        Else
            ' check schema for keys
            Dim keys As String = String.Join(", ", GetPrimaryKeys(schema))
            pk += keys
            hasKeys = keys.Length > 0
        End If
        pk = pk.TrimEnd(New Char() {","c, " "c, ControlChars.Lf}) & ")" & vbLf
        If hasKeys Then
            sql += pk
        End If
        sql += ")"
        Return sql
    End Function

    Public Function Create(ByVal schema As DataTable, ByVal primaryKeys As Integer()) As Object
        Dim sql As String = GetCreateSQL(_tableName, schema, primaryKeys)
        Dim cmd As SqlCommand
        If _transaction IsNot Nothing AndAlso _transaction.Connection IsNot Nothing Then

            cmd = New SqlCommand(sql, _connection, _transaction)
        Else
            cmd = New SqlCommand(sql, _connection)
        End If
        Return cmd.ExecuteNonQuery()
    End Function
    Public Function CreateFromDataTable(ByVal table As DataTable) As Object
        Dim sql As String = GetCreateFromDataTableSQL(_tableName, table)

        Dim cmd As SqlCommand
        If _transaction IsNot Nothing AndAlso _transaction.Connection IsNot Nothing Then

            cmd = New SqlCommand(sql, _connection, _transaction)
        Else
            cmd = New SqlCommand(sql, _connection)
        End If
        Return cmd.ExecuteNonQuery()
    End Function

#End Region

#Region "Static Methods"

    Public Shared Function GetCreateFromDataTableSQL(ByVal tableName As String, ByVal table As DataTable) As String
        Try
            Dim sql As String = "CREATE TABLE [" & tableName & "] (" & vbLf

            ' columns
            For Each column As DataColumn In table.Columns
                sql += "[" + column.ColumnName & "] " & SQLGetType(column) & "," & vbLf
            Next

            sql = sql.TrimEnd(New Char() {","c, ControlChars.Lf}) & vbLf
            'primary keys
            If table.PrimaryKey.Length > 0 Then

                sql += "CONSTRAINT [PK_" & tableName & "] PRIMARY KEY CLUSTERED ("

                For Each column As DataColumn In table.PrimaryKey

                    sql += "[" + column.ColumnName & "],"

                Next
                sql = sql.TrimEnd(New Char() {","c}) & "))" & vbLf
            End If
            If Not Mid(sql, sql.Length - 1) = ")" Then sql &= ")"

            Return sql
        Catch ex As Exception
        End Try
    End Function
    Public Shared Function GetPrimaryKeys(ByVal schema As DataTable) As String()
        Dim keys As New List(Of String)()
        For Each column As DataRow In schema.Rows
            If schema.Columns.Contains("IsKey") AndAlso CBool(column("IsKey")) Then
                keys.Add(column("ColumnName").ToString())
            End If

        Next
        Return keys.ToArray()

    End Function
    ' Return T-SQL data type definition, based on schema definition for a column

    Public Shared Function SQLGetType(ByVal type As Object, ByVal columnSize As Integer, ByVal numericPrecision As Integer, ByVal numericScale As Integer) As String

        Try
            Select Case type.ToString()
                Case "System.String"
                    Return "NCHAR(" & (If((columnSize = -1), 255, columnSize)) & ")"

                Case "System.Decimal"

                    If numericScale > 0 Then
                        Return "REAL"
                    ElseIf numericPrecision > 10 Then
                        Return "BIGINT"
                    Else
                        Return "INT"
                    End If
                Case "System.Double", "System.Single"
                    Return "REAL"
                Case "System.Int64"
                    Return "BIGINT"
                Case "System.Int16", "System.Int32"

                    Return "INT"
                Case "System.DateTime"
                    Return "DATETIME"
                Case "System.Guid"
                    Return "GUID"
                Case "System.Byte"
                    Return "TINYINT"
                Case Else

                    Throw New Exception(type.ToString() & " not implemented.")
            End Select
        Catch ex As Exception
        End Try

    End Function
    ' Overload based on row from schema table
    Public Shared Function SQLGetType(ByVal schemaRow As DataRow) As String
        Return SQLGetType(schemaRow("DataType"), Integer.Parse(schemaRow("ColumnSize").ToString()), Integer.Parse(schemaRow("NumericPrecision").ToString()), Integer.Parse(schemaRow("NumericScale").ToString()))
    End Function

    ' Overload based on DataColumn from DataTable type
    Public Shared Function SQLGetType(ByVal column As DataColumn) As String
        Try
            Return SQLGetType(column.DataType, column.MaxLength, 10, 2)
        Catch ex As Exception
            LogWindow.WriteError("SQLGetType", Err)
            Return ""
        End Try
    End Function

#End Region

End Class
