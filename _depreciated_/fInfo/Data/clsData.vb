'/*
' * fInfo DATA Copyright Marvin Westmaas ( mtm ) 
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
Imports System.IO
Imports System.Data
Imports System.Data.SQLite
'Imports System.Runtime.Serialization
Imports FAHInterface.FAHInterface
Imports FAHInterface.Client
Imports OpenHardwareMonitor.Hardware
Imports OpenHardwareMonitor
Imports gpuInfo.gpuInfo
Imports HWInfo
Imports ProjectInfo

'Imports System.Runtime.Serialization.Formatters.Binary

Public Class Data
    Public Event Exception(Ex As Exception)
    Private strCon As String = ""
    Private sqLiteCon As New SQLiteConnection
    Private dbName As String = ""
    Private _Ex As Exception
    Private Const strDTFormat As String = "dd/MM/yyyy-H:mm:ss:ff(zzz)"
    Public ReadOnly Property LastException As Exception
        Get
            Return _Ex
        End Get
    End Property

    ' Public Function Init(ByVal DataRoot As String, HWinf As clsHWInfo.cHWInfo, Client As FAHInterface.Client.ClientAccess, ProjectInfo As clsProjectInfo) As Boolean
    Public Function Init(DataRoot As String) As Boolean
        ' TODO Replace entire init function with only db creation, let external run the updates manually
        Try
            If Not My.Computer.FileSystem.DirectoryExists(DataRoot) Then My.Computer.FileSystem.CreateDirectory(DataRoot)

            Dim Cmd As New SQLiteCommand
            'Hardware and environment

            Dim dtNow As DateTime = DateTime.Now
            dbName = DataRoot & My.Application.Info.AssemblyName & ".db"

            If Not My.Computer.FileSystem.FileExists(dbName) Then
                strCon = "Data source=" & dbName & ";New=True;Compress=True;Synchronous=off"
                sqLiteCon = New SQLiteConnection(strCon)
                Try
                    sqLiteCon.Open()
                    Cmd = sqLiteCon.CreateCommand
                    Cmd.CommandText = "CREATE TABLE TextReport (TS date,OhmReport TEXT, oclReport TEXT, CUDAReport TEXT, CALReport TEXT, NVapiReport TEXT, ADLReport TEXT)"
                    Cmd.ExecuteNonQuery()
                    'Create table Client Info
                    Cmd.CommandText = "CREATE TABLE Client_Info (TS date, JsON TEXT)"
                    Cmd.ExecuteNonQuery()
                    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                    Cmd = sqLiteCon.CreateCommand
                    Cmd.CommandText = "CREATE TABLE Client_Options (TS date, JsON TEXT)"
                    Cmd.ExecuteNonQuery()
                    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                    Cmd = sqLiteCon.CreateCommand
                    Cmd.CommandText = "CREATE TABLE Client_Queue (TS date, JsON TEXT)"
                    Cmd.ExecuteNonQuery()
                    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                    Cmd = sqLiteCon.CreateCommand
                    Cmd.CommandText = "CREATE TABLE Client_Slots (TS date, JsON TEXT)"
                    Cmd.ExecuteNonQuery()
                    Return sqLiteCon.State = ConnectionState.Open
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                    Return False
                End Try
            Else
                strCon = "Data source=" & dbName & ";Compress=True"
                sqLiteCon = New SQLiteConnection(strCon)
                Try
                    sqLiteCon.Open()
                Catch ex As Exception
                    LogWindow.WriteError(ex.Message, Err)
                    Return False
                End Try

                Return sqLiteCon.State = ConnectionState.Open
            End If
         





            '    Cmd.CommandText = "INSERT INTO TextReport (TS, OhmReport, oclReport, CUDAReport, CALReport, NVapiReport, ADLReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.OHMReport & "','" & HWinf.gpuInf.openCLReport & "','" & HWinf.gpuInf.cudaReport & "','" & HWinf.gpuInf.calReport & "','" & HWinf.ohmInterface.NVAPI.AdapterReport(New Short) & "','" & HWinf.ohmInterface.ADL.AdapterReport(New Short) & "')"
            '    Cmd.ExecuteNonQuery()




            '    Try
            '        If Not Update_ClientInfo(Client) Then

            '        End If
            '    Catch ex As Exception

            '    End Try



            '    Try
            '        If Not Update_ClientOptions(Client) Then

            '        End If
            '    Catch ex As Exception

            '    End Try



            '    Try
            '        If Not Update_ClientQueue(Client) Then

            '        End If

            '    Catch ex As Exception

            '    End Try


            '    Try
            '        If Not Update_ClientSlots(Client) Then

            '        End If
            '    Catch ex As Exception

            '    End Try

            '    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            '    Cmd = sqLiteCon.CreateCommand
            '    If Not IsNothing(HWinf.ohmInterface.Ati(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.ATICount - 1
            '            Cmd.CommandText = "CREATE TABLE Hardware_ATI" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorList_ATI" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorEventData_ATI" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '            Cmd.ExecuteNonQuery()

            '            Cmd.CommandText = "INSERT INTO Hardware_ATI" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.Ati(xInt).Identifier & "','" & HWinf.ohmInterface.Ati(xInt).Name & "','" & HWinf.ohmInterface.Ati(xInt).Driver & "','" & HWinf.ohmInterface.Ati(xInt).DeviceID & "','" & HWinf.ohmInterface.Ati(xInt).Sensors.Count.ToString & "')"
            '            Cmd.ExecuteNonQuery()

            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.Ati(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_ATI" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_ATI" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next

            '        Next
            '    End If

            '    If Not IsNothing(HWinf.ohmInterface.Nvidia(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.NVIDIACount - 1
            '            Cmd.CommandText = "CREATE TABLE Hardware_NVIDIA" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorList_NVIDIA" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorEventData_NVIDIA" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '            Cmd.ExecuteNonQuery()

            '            Cmd.CommandText = "INSERT INTO Hardware_NVIDIA" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.Nvidia(xInt).Identifier & "','" & HWinf.ohmInterface.Nvidia(xInt).Name & "','" & HWinf.ohmInterface.Nvidia(xInt).Driver & "','" & HWinf.ohmInterface.Nvidia(xInt).DeviceID & "','" & HWinf.ohmInterface.Nvidia(xInt).Sensors.Count.ToString & "')"
            '            Cmd.ExecuteNonQuery()

            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.Nvidia(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_NVIDIA" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_NVIDIA" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next

            '        Next
            '    End If

            '    If Not IsNothing(HWinf.ohmInterface.CPU(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.CpuCount - 1
            '            Cmd.CommandText = "CREATE TABLE Hardware_CPU" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Cores INTEGER, ThreadsPerCore Integer, MicroArchitecture TEXT, SensorCount INTEGER, Report TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "INSERT INTO Hardware_CPU" & xInt.ToString & " (TS, Identifier, Name, Cores, ThreadsPerCore, MicroArchitecture, SensorCount, Report)  VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.CPU(xInt).Identifier & "','" & HWinf.ohmInterface.CPU(xInt).CpuName & "','" & HWinf.ohmInterface.CPU(xInt).CoreCount & "','" & HWinf.ohmInterface.CPU(xInt).ThreadsPerCore & "','" & HWinf.ohmInterface.CPU(xInt).MicroArchitecture & "','" & HWinf.ohmInterface.CPU(xInt).Sensors.Count & "','" & HWinf.ohmInterface.CPU(xInt).Report & "')"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorList_CPU" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            Cmd.CommandText = "CREATE TABLE SensorEventData_CPU" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '            Cmd.ExecuteNonQuery()
            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.CPU(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_CPU" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_CPU" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next
            '        Next
            '    End If
            '    Cmd.Dispose()
            'Else
            '    strCon = "Data source=" & Root & fName & ";Synchonous=off"
            '    sqLiteCon = New SQLiteConnection(strCon)
            '    Try
            '        sqLiteCon.Open()
            '        Cmd = sqLiteCon.CreateCommand
            '    Catch ex As Exception
            '        Return False
            '    End Try
            '    Cmd.CommandText = "INSERT INTO TextReport (TS, OhmReport, oclReport, CUDAReport, CALReport, NVapiReport, ADLReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.OHMReport & "','" & HWinf.gpuInf.openCLReport & "','" & HWinf.gpuInf.cudaReport & "','" & HWinf.gpuInf.calReport & "','" & HWinf.ohmInterface.NVAPI.AdapterReport(New Short) & "','" & HWinf.ohmInterface.ADL.AdapterReport(New Short) & "')"
            '    Cmd.ExecuteNonQuery()
            '    Update_ClientInfo(Client)
            '    Update_ClientOptions(Client)
            '    Update_ClientQueue(Client)
            '    Update_ClientSlots(Client)

            '    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            '    Cmd = sqLiteCon.CreateCommand
            '    If Not IsNothing(HWinf.ohmInterface.Ati(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.ATICount - 1
            '            'Check if table exists
            '            Dim bDoCreate As Boolean = False
            '            Try
            '                Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name='" & Convert.ToString("Hardware_ATI" & xInt.ToString) & "'"
            '                Dim rdr As SQLiteDataReader = Cmd.ExecuteReader()
            '                bDoCreate = rdr.HasRows
            '            Catch ex As Exception

            '            End Try
            '            If bDoCreate Then
            '                Cmd.CommandText = "CREATE TABLE Hardware_ATI" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorList_ATI" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorEventData_ATI" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '                Cmd.ExecuteNonQuery()
            '            End If

            '            Cmd.CommandText = "INSERT INTO Hardware_ATI" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.Ati(xInt).Identifier & "','" & HWinf.ohmInterface.Ati(xInt).Name & "','" & HWinf.ohmInterface.Ati(xInt).Driver & "','" & HWinf.ohmInterface.Ati(xInt).DeviceID & "','" & HWinf.ohmInterface.Ati(xInt).Sensors.Count.ToString & "')"
            '            Cmd.ExecuteNonQuery()

            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.Ati(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_ATI" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_ATI" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next

            '        Next
            '    End If

            '    If Not IsNothing(HWinf.ohmInterface.Nvidia(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.NVIDIACount - 1
            '            Dim bDoCreate As Boolean = False
            '            Try
            '                Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name ='" & "Hardware_ATI" & xInt.ToString & "'"
            '                Dim rdr As SQLiteDataReader = Cmd.ExecuteReader
            '                bDoCreate = rdr.HasRows
            '                rdr = Nothing
            '            Catch ex As Exception

            '            End Try
            '            If bDoCreate Then
            '                Cmd.CommandText = "CREATE TABLE Hardware_NVIDIA" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorList_NVIDIA" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorEventData_NVIDIA" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '                Cmd.ExecuteNonQuery()
            '            End If

            '            Cmd.CommandText = "INSERT INTO Hardware_NVIDIA" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.Nvidia(xInt).Identifier & "','" & HWinf.ohmInterface.Nvidia(xInt).Name & "','" & HWinf.ohmInterface.Nvidia(xInt).Driver & "','" & HWinf.ohmInterface.Nvidia(xInt).DeviceID & "','" & HWinf.ohmInterface.Nvidia(xInt).Sensors.Count.ToString & "')"
            '            Cmd.ExecuteNonQuery()

            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.Nvidia(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_NVIDIA" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_NVIDIA" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next

            '        Next
            '    End If

            '    If Not IsNothing(HWinf.ohmInterface.CPU(New Short)) Then
            '        For xInt As Int16 = 0 To HWinf.ohmInterface.CpuCount - 1
            '            Dim bDoCreate As Boolean = False
            '            Try
            '                Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name ='Hardware_CPU" & xInt.ToString & "'"
            '                Dim rdr As SQLiteDataReader = Cmd.ExecuteReader
            '                bDoCreate = rdr.HasRows
            '            Catch ex As Exception

            '            End Try
            '            If bDoCreate Then
            '                Cmd.CommandText = "CREATE TABLE Hardware_CPU" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Cores INTEGER, ThreadsPerCore Integer, MicroArchitecture TEXT, SensorCount INTEGER, Report TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorList_CPU" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "CREATE TABLE SensorEventData_CPU" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
            '                Cmd.ExecuteNonQuery()
            '            End If

            '            Cmd.CommandText = "INSERT INTO Hardware_CPU" & xInt.ToString & " (TS, Identifier, Name, Cores, ThreadsPerCore, MicroArchitecture, SensorCount, Report)  VALUES ('" & dtNow.ToString(strDTFormat) & "','" & HWinf.ohmInterface.CPU(xInt).Identifier & "','" & HWinf.ohmInterface.CPU(xInt).CpuName & "','" & HWinf.ohmInterface.CPU(xInt).CoreCount & "','" & HWinf.ohmInterface.CPU(xInt).ThreadsPerCore & "','" & HWinf.ohmInterface.CPU(xInt).MicroArchitecture & "','" & HWinf.ohmInterface.CPU(xInt).Sensors.Count & "','" & HWinf.ohmInterface.CPU(xInt).Report & "')"
            '            Cmd.ExecuteNonQuery()

            '            For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In HWinf.ohmInterface.CPU(xInt).Sensors
            '                Cmd.CommandText = "INSERT INTO SensorList_CPU" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
            '                Cmd.ExecuteNonQuery()
            '                Cmd.CommandText = "INSERT INTO SensorEventData_CPU" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
            '                Cmd.ExecuteNonQuery()
            '            Next
            '        Next
            '    End If
            '    Cmd.Dispose()
            'End If

            'If Not Update_ProjectInfo(ProjectInfo) Then

            'End If

            'Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Overloads Function UpdateHardware(hwInf As clsHWInfo.cHWInfo) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim Cmd As SQLiteCommand = sqLiteCon.CreateCommand
            If Not IsNothing(hwInf.ohmInterface.Ati(New Short)) Then
                For xInt As Int16 = 0 To hwInf.ohmInterface.ATICount - 1
                    'Check if table exists
                    Dim bDoCreate As Boolean = False
                    Try
                        Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name='Hardware_ATI" & xInt.ToString & "'"
                        Dim rdr As SQLiteDataReader = Cmd.ExecuteReader()
                        bDoCreate = Not rdr.HasRows
                        rdr.Close()
                    Catch ex As Exception

                    End Try
                    If bDoCreate Then
                        Cmd.CommandText = "CREATE TABLE Hardware_ATI" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorList_ATI" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorEventData_ATI" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
                        Cmd.ExecuteNonQuery()
                    End If

                    Cmd.CommandText = "INSERT INTO Hardware_ATI" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwInf.ohmInterface.Ati(xInt).Identifier & "','" & hwInf.ohmInterface.Ati(xInt).Name & "','" & hwInf.ohmInterface.Ati(xInt).Driver & "','" & hwInf.ohmInterface.Ati(xInt).DeviceID & "','" & hwInf.ohmInterface.Ati(xInt).Sensors.Count.ToString & "')"
                    Cmd.ExecuteNonQuery()

                    For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In hwInf.ohmInterface.Ati(xInt).Sensors
                        Cmd.CommandText = "INSERT INTO SensorList_ATI" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "INSERT INTO SensorEventData_ATI" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
                        Cmd.ExecuteNonQuery()
                    Next

                Next
            End If

            If Not IsNothing(hwInf.ohmInterface.Nvidia(New Short)) Then
                For xInt As Int16 = 0 To hwInf.ohmInterface.NVIDIACount - 1
                    Dim bDoCreate As Boolean = True
                    Try
                        Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name ='" & "Hardware_NVIDIA" & xInt.ToString & "'"
                        Dim rdr As SQLiteDataReader = Cmd.ExecuteReader
                        bDoCreate = Not rdr.HasRows
                        rdr.Close()
                    Catch ex As Exception

                    End Try
                    If bDoCreate Then
                        Cmd.CommandText = "CREATE TABLE Hardware_NVIDIA" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Driver TEXT, DeviceID TEXT, SensorCount TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorList_NVIDIA" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorEventData_NVIDIA" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
                        Cmd.ExecuteNonQuery()
                    End If

                    Cmd.CommandText = "INSERT INTO Hardware_NVIDIA" & xInt.ToString & " (TS, Identifier, Name, Driver, DeviceID, SensorCount) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwInf.ohmInterface.Nvidia(xInt).Identifier & "','" & hwInf.ohmInterface.Nvidia(xInt).Name & "','" & hwInf.ohmInterface.Nvidia(xInt).Driver & "','" & hwInf.ohmInterface.Nvidia(xInt).DeviceID & "','" & hwInf.ohmInterface.Nvidia(xInt).Sensors.Count.ToString & "')"
                    Cmd.ExecuteNonQuery()

                    For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In hwInf.ohmInterface.Nvidia(xInt).Sensors
                        Cmd.CommandText = "INSERT INTO SensorList_NVIDIA" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "INSERT INTO SensorEventData_NVIDIA" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
                        Cmd.ExecuteNonQuery()
                    Next

                Next
            End If

            If Not IsNothing(hwInf.ohmInterface.CPU(New Short)) Then
                For xInt As Int16 = 0 To hwInf.ohmInterface.CpuCount - 1
                    Dim bDoCreate As Boolean = True
                    Try
                        Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name ='Hardware_CPU" & xInt.ToString & "'"
                        Dim rdr As SQLiteDataReader = Cmd.ExecuteReader
                        bDoCreate = Not rdr.HasRows
                        rdr.Close()
                    Catch ex As Exception

                    End Try
                    If bDoCreate Then
                        Cmd.CommandText = "CREATE TABLE Hardware_CPU" & xInt.ToString & " (TS date, Identifier TEXT, Name TEXT, Cores INTEGER, ThreadsPerCore Integer, MicroArchitecture TEXT, SensorCount INTEGER, Report TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorList_CPU" & xInt.ToString & " (Identifier TEXT, Name TEXT)"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "CREATE TABLE SensorEventData_CPU" & xInt.ToString & " (Identifier TEXT, TS date, Value TEXT)"
                        Cmd.ExecuteNonQuery()
                    End If

                    Cmd.CommandText = "INSERT INTO Hardware_CPU" & xInt.ToString & " (TS, Identifier, Name, Cores, ThreadsPerCore, MicroArchitecture, SensorCount, Report)  VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwInf.ohmInterface.CPU(xInt).Identifier & "','" & hwInf.ohmInterface.CPU(xInt).CpuName & "','" & hwInf.ohmInterface.CPU(xInt).CoreCount & "','" & hwInf.ohmInterface.CPU(xInt).ThreadsPerCore & "','" & hwInf.ohmInterface.CPU(xInt).MicroArchitecture & "','" & hwInf.ohmInterface.CPU(xInt).Sensors.Count & "','" & hwInf.ohmInterface.CPU(xInt).Report & "')"
                    Cmd.ExecuteNonQuery()

                    For Each oSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors In hwInf.ohmInterface.CPU(xInt).Sensors
                        Cmd.CommandText = "INSERT INTO SensorList_CPU" & xInt.ToString & " (Identifier, Name) VALUES('" & oSensor.Identifier & "','" & oSensor.Name & "')"
                        Cmd.ExecuteNonQuery()
                        Cmd.CommandText = "INSERT INTO SensorEventData_CPU" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & oSensor.Identifier & "','" & oSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & oSensor.CurrentValue.ToString & "')"
                        Cmd.ExecuteNonQuery()
                    Next
                Next
            End If
            Cmd.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Overloads Function Update(OhmSensor As clsHWInfo.cHWInfo.cOHMInterface.ohmSensors) As Boolean
        Try
            If OhmSensor.Sensor.Hardware.HardwareType = Hardware.HardwareType.GpuAti Then
                Dim xInt As Int16 = CInt(OhmSensor.Identifier.Substring(OhmSensor.Identifier.IndexOf("/", 1) + 1, 1))
                Dim cmd As New SQLiteCommand
                If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                cmd = sqLiteCon.CreateCommand
                cmd.CommandText = "INSERT INTO SensorEventData_ATI" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & OhmSensor.Identifier & "','" & OhmSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & OhmSensor.CurrentValue.ToString & "')"
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            ElseIf OhmSensor.Sensor.Hardware.HardwareType = Hardware.HardwareType.GpuNvidia Then
                Dim xInt As Int16 = CInt(OhmSensor.Identifier.Substring(OhmSensor.Identifier.IndexOf("/", 1) + 1, 1))
                Dim cmd As New SQLiteCommand
                If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                cmd = sqLiteCon.CreateCommand
                cmd.CommandText = "INSERT INTO SensorEventData_NVIDIA" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & OhmSensor.Identifier & "','" & OhmSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & OhmSensor.CurrentValue.ToString & "')"
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            ElseIf OhmSensor.Sensor.Hardware.HardwareType = Hardware.HardwareType.CPU Then
                Dim xInt As Int16 = CInt(OhmSensor.Identifier.Substring(OhmSensor.Identifier.IndexOf("/", 1) + 1, 1))
                Dim cmd As New SQLiteCommand
                If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                cmd = sqLiteCon.CreateCommand
                cmd.CommandText = "INSERT INTO SensorEventData_CPU" & xInt.ToString & " (Identifier, TS, Value) VALUES ('" & OhmSensor.Identifier & "','" & OhmSensor.EventTime.ToString("dd/MM/yyyy/hh:mm:ss-tt") & "','" & OhmSensor.CurrentValue.ToString & "')"
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            End If
            Debug.WriteLine(OhmSensor.Name & ":-" & OhmSensor.CurrentValue)
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_CUDA(hwinf As clsHWInfo.cHWInfo) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO TextReport (TS, CUDAReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwinf.gpuInf.cudaReport & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_OHM(hwInf As clsHWInfo.cHWInfo) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO TextReport (TS, OhmReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwInf.ohmInterface.OHMReport & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_CAL(hwinf As clsHWInfo.cHWInfo) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO TextReport (TS, CALReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwinf.gpuInf.calReport & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_openCL(hwinf As clsHWInfo.cHWInfo) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO TextReport (TS, oclReport) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & hwinf.gpuInf.openCLReport & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_ClientInfo(Client As FAHInterface.Client.ClientAccess) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO Client_Info (TS, JsON) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & Client.JsON_Info & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try

    End Function
    Public Function Update_ClientOptions(Client As FAHInterface.Client.ClientAccess) As Boolean

        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO Client_Options (TS, JsON) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & Client.JsON_Options & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public Function Update_ClientQueue(Client As FAHInterface.Client.ClientAccess) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO Client_Queue (TS, JsOn) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & Client.JsON_Queue & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False

        End Try
    End Function
    Public Function Update_ClientSlots(Client As FAHInterface.Client.ClientAccess) As Boolean
        Try
            Dim dtNow As DateTime = DateTime.Now
            Dim mySelectQuery As String = "INSERT INTO Client_Slots (TS, JsON) VALUES ('" & dtNow.ToString(strDTFormat) & "','" & Client.JsON_Slots & "')"
            Dim sqCommand As New SQLiteCommand(mySelectQuery, sqLiteCon)
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            sqCommand.ExecuteNonQuery()
            sqCommand.Dispose()
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            RaiseEvent Exception(ex)
            Err.Clear()
            Return False
        End Try
    End Function
    ' TODO I'm loosing +- 200 known projects between parsing, storing and retrieving... :S ---> db has 500+ wu's, retrieval stops at 200 or so must be sql syntax issue or reader mis use.
    Public Function Update_ProjectInfo(ProjectInfo As clsProjectInfo) As Boolean
        Try
            'Check if table exists
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim Cmd As SQLite.SQLiteCommand = sqLiteCon.CreateCommand
            Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name='ProjectInfo'"
            Dim rdr As SQLiteDataReader = Cmd.ExecuteReader()
            If Not rdr.HasRows Then
                ' Return False <- Table doesn't exist, create!
                Cmd = sqLiteCon.CreateCommand
                Cmd.CommandText = "CREATE TABLE ProjectInfo (ProjectNumber TEXT,ServerIP TEXT, WuName TEXT, NumberOfAtoms TEXT, PrefferedDays TEXT, FinalDeadline TEXT, Credit TEXT, Frames TEXT, Code TEXT, Contact TEXT, kFactor TEXT, UnitType TEXT, Description TEXT)"
                Cmd.ExecuteNonQuery()
            End If

            For xIndex As Int16 = 0 To ProjectInfo.Projects.ProjectCount
                With ProjectInfo.Projects.Project(CShort(xIndex + 1))
                    Try
                        If IsNothing(.ProjectNumber) Then Exit For
                    Catch ex As Exception
                        Exit For
                    End Try
                    If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                    Cmd = sqLiteCon.CreateCommand
                    Cmd.CommandText = "SELECT * FROM ProjectInfo WHERE ProjectNumber='" & .ProjectNumber & "' AND UnitType='" & .ProjectType.ToString & "'"
                    rdr = Cmd.ExecuteReader()
                    If Not rdr.HasRows Then
                        'Project is not known, check for description 
                        If .Description.ToUpper.Contains("HTTP://") Then
                            'Check fahclient
                            Dim strCon = "Data source=" & Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\FAHControl.db;Synchronous=off"
                            Dim sqLcon2 As SQLiteConnection = New SQLiteConnection(strCon)
                            Try
                                sqLcon2.Open()
                                Dim cmd2 As SQLiteCommand = sqLcon2.CreateCommand
                                cmd2.CommandText = "SELECT description FROM projects WHERE id='" & .ProjectNumber & "'"
                                Dim sr2 As SQLiteDataReader = cmd2.ExecuteReader
                                If sr2.HasRows Then
                                    .Description = sr2.Item("description")
                                End If
                            Catch ex As Exception

                            End Try
                        End If


                        If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
                        Cmd = sqLiteCon.CreateCommand
                        Cmd.CommandText = "INSERT INTO ProjectInfo (ProjectNumber, ServerIP, WuName, NumberOfAtoms, PrefferedDays, FinalDeadline, Credit, Frames, Code, Contact, kFactor, UnitType, Description) VALUES ('"
                        Cmd.CommandText &= .ProjectNumber & "','" & .ServerIP & "','" & .WUName & "','" & .NumberOfAtoms & "','" & .PreferredDays & "','" & .FinalDeadline & "','" & .Credit & "','" & .Frames & "','" & .Code & "','" & .Contact & "','" & .kFactor & "','" & .ProjectType.ToString & "','" & .Description & "')"
                        Cmd.ExecuteNonQuery()
                    End If
                End With
            Next
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Public ReadOnly Property HasProjectInfo As Boolean
        Get
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim Cmd As SQLite.SQLiteCommand = sqLiteCon.CreateCommand
            Cmd.CommandText = "SELECT name FROM sqlite_master WHERE name='ProjectInfo'"
            Dim rdr As SQLiteDataReader = Cmd.ExecuteReader()
            Return rdr.HasRows
        End Get
    End Property
    Public Function Read_Projects() As clsProjectInfo.sProject
        Dim nProjects As New clsProjectInfo.sProject
        Try
            If Not sqLiteCon.State = ConnectionState.Open Then sqLiteCon.Open()
            Dim Cmd As SQLiteCommand = sqLiteCon.CreateCommand
            Cmd.CommandText = "SELECT * FROM ProjectInfo"
            Dim rdr As SQLiteDataReader = Cmd.ExecuteReader()
            Dim nPr As New clsProjectInfo.sProject.clsProject
            While rdr.Read
                nPr = New clsProjectInfo.sProject.clsProject
                With nPr
                    .Code = rdr.Item("Code").ToString '0 
                    .Contact = rdr.Item("Contact").ToString '1
                    .Credit = rdr.Item("Credit").ToString '2
                    .Description = rdr.Item("Description").ToString '3
                    .FinalDeadline = rdr.Item("FinalDeadline").ToString '4
                    .Frames = rdr.Item("Frames").ToString '5
                    .kFactor = rdr.Item("kFactor").ToString '6
                    .NumberOfAtoms = rdr.Item("NumberOfAtoms").ToString '7
                    .PreferredDays = rdr.Item("PrefferedDays").ToString
                    .ProjectNumber = rdr.Item("ProjectNumber").ToString
                    If .Description.ToUpper.Substring(0, 7) = "HTTP://" Then
                        'Check fahclient
                        Dim strCon = "Data source=" & Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\FAHClient\FAHControl.db;Synchronous=off"
                        Dim sqLcon2 As SQLiteConnection = New SQLiteConnection(strCon)
                        Try
                            sqLcon2.Open()
                            Dim cmd2 As SQLiteCommand = sqLcon2.CreateCommand
                            cmd2.CommandText = "SELECT description FROM projects WHERE id='" & .ProjectNumber & "'"
                            Dim sr2 As SQLiteDataReader = cmd2.ExecuteReader
                            If sr2.HasRows Then
                                .Description = sr2.Item("description")
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                    If rdr.Item("UnitType").ToString = "Beta" Then
                        .ProjectType = clsProjectInfo.sProject.eProjectType.Beta
                    ElseIf rdr.Item("UnitType").ToString = "Advanced" Then
                        .ProjectType = clsProjectInfo.sProject.eProjectType.Advanced
                    ElseIf rdr.Item("UnitType").ToString = "Regular" Then
                        .ProjectType = clsProjectInfo.sProject.eProjectType.Regular
                    End If
                    .ServerIP = rdr.Item("ServerIP").ToString
                    .WUName = rdr.Item("WuName").ToString
                End With
                nProjects.AddProject(nPr)
                If Not rdr.Read Then Exit While
            End While
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
        Return nProjects ' will be empty if function fails 
    End Function
    Public Sub Close()
        sqLiteCon.Close()
    End Sub

    Public Class cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public Shared WithEvents LogWindow As New cLW
    Public Shared Event Log(ByVal Message As String)
    Public Shared Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
    Private Shared Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Shared Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub

    Function ReadProjects() As clsProjectInfo.sProject
        Throw New NotImplementedException
    End Function

End Class


