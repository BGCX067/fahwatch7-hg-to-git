'   fTray statistics class
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
Imports System.Data
Imports System.Data.SQLite
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Public Class clsStatistics
    Private _bError As Boolean = False, _Err As ErrObject
    Private strCon As String = ""
    Private sqLiteCon As New SQLiteConnection
    Public ReadOnly Property HasError(ByVal ErrObj As ErrObject) As Boolean
        Get
            Try
                If _bError Then
                    ErrObj = _Err
                    _Err = Nothing
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                LogWindow.WriteError("clsStatistics_HasError", Err)
                _bError = True
                _Err = Err()
                Err.Clear()
                Return True
            End Try
        End Get
    End Property
    Public Sub New(ByVal Location As String)
        Try
            If Not My.Computer.FileSystem.FileExists(Location & "\dbStats.db") Then
                LogWindow.WriteLog("Client statistics database not found, creating new one")
                'strCon = "Data source=" & Location & "\dbStats.db;Password=" & MySettings.MySettings._WTF & ";New=True;Compress=True;Synchronous=Off"
                strCon = "Data source=" & Location & "\dbStats.db;New=True;Compress=True;Synchronous=Off"
                sqLiteCon = New SQLiteConnection(strCon)
                sqLiteCon.Open()
                LogWindow.WriteLog("Creating client table")
                Dim Cmd As New SQLiteCommand
                Cmd = sqLiteCon.CreateCommand
                Cmd.CommandText = "CREATE TABLE Client (ClientID NVARCHAR(100) primary key, DATA blob)"
                Cmd.ExecuteNonQuery()
                LogWindow.WriteLog("Creating Projects table")
                Cmd.CommandText = "CREATE TABLE Projects (ProjectID NVARCHAR(100) primary key, ClientID NVARCHAR(100), DATA blob)"
                Cmd.ExecuteNonQuery()
                LogWindow.WriteLog("Creating Frame table")
                Cmd.CommandText = "CREATE TABLE Frames (ProjectID NVARCHAR(100) primary key, StartTime datetime, EndTime datetime)"
                Cmd.ExecuteNonQuery()
                LogWindow.WriteLog("Creating EOC table")
                Cmd.CommandText = "CREATE TABLE EXTREMEOVERCLOCKING (UNIXTS integer primary key, DATA blob)"
                Cmd.ExecuteNonQuery()
                LogWindow.WriteLog("Creating signature table")
                Cmd.CommandText = "CREATE TABLE SIGNATURE (UNIXTS integer primary key, DATA blob)"
                Cmd.ExecuteNonQuery()
                Cmd.Dispose()
                LogWindow.WriteLog("Finished creating table's")
                sqLiteCon.Close()
            Else
                LogWindow.WriteLog("Client statistics database found, opening...")
                'strCon = "Data source=" & Location & "\dbStats.db;Password=" & MySettings.MySettings._WTF & ";Compress=True;Synchronous=Off"
                strCon = "Data source=" & Location & "\dbStats.db;Compress=True;Synchronous=Off"
                sqLiteCon = New SQLiteConnection(strCon)
                sqLiteCon.Open()
                LogWindow.WriteLog("Client statistics database opend succesfully")
                sqLiteCon.Close()
            End If
        Catch ex As Exception
            LogWindow.WriteError("clsStatistics_New(" & Location & ")", Err)
            _bError = True
            _Err = Err()
            Err.Clear()
        End Try
    End Sub
#Region "Client functions"
    Public Function GetClient(ByVal Executable As String) As clsClientControl.sClient
        Dim rClient As New clsClientControl.sClient
        Try
            Dim mySelectQuery As String = "SELECT * FROM Client WHERE ClientID = '" & Executable & "'"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Try
                Dim sqReader As SQLiteDataReader = sqCommand.ExecuteReader()
                ' Always call Read before accessing data.
                While sqReader.Read()
                    'If it reads something, it has the record, need to convert it class 
                    Dim bf As New BinaryFormatter
                    Dim mStream As New System.IO.MemoryStream
                    Dim pData() As Byte = DirectCast(sqReader(1), Byte())
                    mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
                    mStream.Position = 0
                    rClient = CType(bf.Deserialize(mStream), clsClientControl.sClient)
                End While
                sqReader.Close()
            Catch ex As SQLiteException
                'Catch these!
            Catch ex As Exception

            Finally
                sqLiteCon.Close()
            End Try
        Catch ex As Exception
            LogWindow.WriteError("Statistics_GetClient", Err)
        End Try
        Return rClient
    End Function
    Public Function AddClient(ByVal Client As clsClientControl.sClient) As Boolean
        Try
            Dim mySelectQuery As String = "INSERT INTO Client (ClientID, DATA) VALUES ('" & Client.ClientEXE & "',@DATA)"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim bf As New BinaryFormatter()
            Dim ms As New MemoryStream()
            bf.Serialize(ms, Client)
            Dim DATA() As Byte = ms.ToArray
            Dim SQLparm As New SQLiteParameter("@DATA", DATA)
            SQLparm.DbType = DbType.Binary
            SQLparm.Value = DATA
            sqCommand.Parameters.Add(SQLparm)
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            sqLiteCon.Close()
            Return True
        Catch ex As Exception
            LogWindow.WriteError("ClientControl_AddClient", Err)
            Return False
        End Try
    End Function
    Public Function UpdateClient(ByVal Client As clsClientControl.sClient) As Boolean
        Try
            '"UPDATE foo SET image = @image WHERE id = '1'"
            Dim mySelectQuery As String = "UPDATE Client SET DATA = @DATA WHERE ClientID = '" & Client.ClientEXE & "'"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim bf As New BinaryFormatter()
            Dim ms As New MemoryStream()
            bf.Serialize(ms, Client)
            Dim DATA() As Byte = ms.ToArray
            Dim SQLparm As New SQLiteParameter("@DATA", DATA)
            SQLparm.DbType = DbType.Binary
            SQLparm.Value = DATA
            sqCommand.Parameters.Add(SQLparm)
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            sqLiteCon.Close()
            Return True
            Return True
        Catch ex As Exception
            LogWindow.WriteError("Statistics_UpdateClient", Err)
            Return False
        End Try
    End Function
#End Region
#Region "Project functions"
    Public ReadOnly Property KnownProject(ByVal Project As clsClientControl.sProject) As Boolean
        Get
            Try

                Return (True)
            Catch ex As Exception
                LogWindow.WriteError("Statistics_KnownProject", Err)
                Return False
            End Try
        End Get
    End Property
    Public Function AddProject(ByVal qSlot As clsClientControl.sProject) As Boolean
        Try

            Return True
        Catch ex As Exception
            LogWindow.WriteError("Statistics_AddProject", Err)
            Return False
        End Try
    End Function
    Public Function GetProject(ByVal Project As clsClientControl.sProject) As clsClientControl.sProject
        Dim rProject As New clsClientControl.sProject

        Try
            Dim mySelectQuery As String = "SELECT * FROM Projects WHERE ProjectID = '" & Project.ProjectID & "'"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Try
                Dim sqReader As SQLiteDataReader = sqCommand.ExecuteReader()
                ' Always call Read before accessing data.
                While sqReader.Read()
                    'If it reads something, it has the record, need to convert it class 
                    'TABLE Projects (ProjectID NVARCHAR(100) primary key, ClientID NVARCHAR(100), DATA blob)"
                    Dim bf As New BinaryFormatter
                    Dim mStream As New System.IO.MemoryStream
                    Dim pData() As Byte = DirectCast(sqReader(1), Byte())
                    mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
                    mStream.Position = 0
                    
                    bf = Nothing
                    mStream = Nothing
                    pData = Nothing
                End While
                sqReader.Close()
            Catch ex As SQLiteException
                'Catch these!
            Catch ex As Exception

            Finally
                sqLiteCon.Close()
            End Try
        Catch ex As Exception
            LogWindow.WriteError("GetProjects", Err)
            Return rProject
        End Try
    End Function
    Public Function GetClientProjects(ByVal ClientID As Int16) As ArrayList
        Try
            Dim alProjects As New ArrayList
            'Add each project for this client to arraylist

            Return alProjects
        Catch ex As Exception
            LogWindow.WriteError("GetclientProjects", Err, "ClientID=" & ClientID)
            Dim alEmpty As New ArrayList
            Dim Empty As Boolean = True
            alEmpty.Add(Empty)
            Return (alEmpty)
        End Try
    End Function
#End Region
#Region "EOC XML functions"
    'If returns empty then no update pressent 
    Public ReadOnly Property GetXMLUpdate(ByVal TimeStamp As Int64) As clsEOC.clsUpdates
        Get
            Dim xmlUpdate As New clsEOC.clsUpdates
            Try
                Dim mySelectQuery As String = "SELECT * FROM EXTREMEOVERCLOCKING WHERE UNIXTS = '" & TimeStamp & "'"
                Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
                If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                Try
                    Dim sqReader As SQLiteDataReader = sqCommand.ExecuteReader()
                    ' Always call Read before accessing data.
                    While sqReader.Read()
                        'If it reads something, it has the record, need to convert it class 
                        Dim bf As New BinaryFormatter
                        Dim mStream As New System.IO.MemoryStream
                        Dim pData() As Byte = DirectCast(sqReader(1), Byte())
                        mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
                        mStream.Position = 0
                        xmlUpdate = CType(bf.Deserialize(mStream), clsEOC.clsUpdates)
                        xmlUpdate.IsEmpty = False
                        bf = Nothing
                        mStream = Nothing
                        pData = Nothing
                    End While
                    sqReader.Close()
                Catch ex As SQLiteException
                    'Catch these!
                Catch ex As Exception

                Finally
                    sqLiteCon.Close()
                End Try
                Return xmlUpdate
            Catch ex As Exception
                LogWindow.WriteError("clsStatistics_HasUpdate", Err, "EOC XML Functions")
                Return xmlUpdate
            End Try
        End Get
    End Property
    Public Function GetXMLUpdateTimeStamps() As Array
        Dim TS() As Long
        Try
            ReDim TS(0 To 0)
            Dim mySelectQuery As String = "SELECT UNIXTS FROM EXTREMEOVERCLOCKING"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Try
                Dim sqReader As SQLiteDataReader = sqCommand.ExecuteReader()
                ' Always call Read before accessing data.
                While sqReader.Read()
                    If TS.GetUpperBound(0) = 0 Then
                        TS(0) = sqReader(0)
                    Else
                        ReDim Preserve TS(0 To TS.GetUpperBound(0) + 1)
                        TS(TS.GetUpperBound(0)) = sqReader(0)
                    End If
                End While
                sqReader = Nothing
            Catch ex As Exception
                LogWindow.WriteError("GetXMLTimeStamps", Err, "Enumerating")
            End Try
        Catch
            LogWindow.WriteError("GetXMLTimeStamps", Err, )
        End Try
        Return TS
    End Function
    Public Function InsertXMLUpdate(ByVal NewUpdate As clsEOC.clsUpdates) As Boolean
        Try 'Works!
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim cmd As New SQLiteCommand
            cmd.Connection = sqLiteCon
            Dim bf As New BinaryFormatter()
            Dim ms As New MemoryStream()
            bf.Serialize(ms, NewUpdate)
            'INSERT INTO EXTREMEOVERCLOCKING (UNIXTS, DATA) VALUES ('" & CInt(NewUpdate.Update.UpdateStatus.strUnixTimeStamp.ToString) & "','" &  ms.ToArray() & "')"
            cmd.CommandText = "INSERT INTO EXTREMEOVERCLOCKING (UNIXTS, DATA) VALUES ('" & CInt(NewUpdate.Update.UpdateStatus.strUnixTimeStamp.ToString) & "',@xmldata)"
            Dim XML() As Byte = ms.ToArray
            Dim SQLparm As New SQLiteParameter("@xmldata", XML)
            SQLparm.DbType = DbType.Binary
            SQLparm.Value = XML
            cmd.Parameters.Add(SQLparm)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            sqLiteCon.Close()
            Return True
        Catch ex As Exception
            LogWindow.WriteError("clsStatistics_InsertXMLUpdate", Err)
            Return False
        End Try
    End Function
    Public Function GetAllXMLUpdates() As ArrayList
        Try
            Dim ts() As Long = GetXMLUpdateTimeStamps()
            Dim alUpdates As New ArrayList
            For xInt As Int16 = 0 To ts.Count - 1
                alUpdates.Add(GetXMLUpdate(ts(xInt)))
            Next
            Return alUpdates
        Catch ex As Exception
            LogWindow.WriteError("GetAllXMLUpdates", Err)
            Return Nothing
        End Try
    End Function
#Region "SignatureImage"
    Private Function BlobToImage(ByVal blob)
        Try
            Dim mStream As New System.IO.MemoryStream
            Dim pData() As Byte = DirectCast(blob, Byte())
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length))
            Dim bm As Bitmap = New Bitmap(mStream, False)
            mStream.Dispose()
            Return bm
        Catch ex As Exception
            LogWindow.WriteError("BlobToImage", Err)
            Return Nothing
        End Try
    End Function
    Public Function GetSIGImage() As Image
        Try
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim cmd As New SQLiteCommand
            cmd.Connection = sqLiteCon
            cmd.CommandText = "SELECT * FROM SIGNATURE"
            Dim SQLreader As SQLiteDataReader = cmd.ExecuteReader()
            SQLreader.Read()
            Dim tImg As Image= BlobToImage(SQLreader(1))
            SQLreader.Close()
            cmd.Dispose()
            sqLiteCon.Close()
            Return tImg
        Catch ex As Exception
            LogWindow.WriteError("clsStatistics_GetSIGImage", Err)
            Return Nothing
        End Try
    End Function
    Public Function SaveSIGImage(ByVal UNIXTS As String, ByVal bArr() As Byte) As Boolean
        Try
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim cmd As SQLiteCommand = sqLiteCon.CreateCommand
            cmd.CommandText = "INSERT INTO SIGNATURE (UNIXTS, DATA) VALUES ('" & UNIXTS & "',@image)"
            Dim SQLparm As New SQLiteParameter("@image", bArr)
            SQLparm.DbType = DbType.Binary
            SQLparm.Value = bArr
            cmd.Parameters.Add(SQLparm)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            sqLiteCon.Close()
            Return True
        Catch ex As Exception
            LogWindow.WriteError("SaveSigImage", Err)
            Return False
        End Try
    End Function
#End Region

#End Region




End Class
