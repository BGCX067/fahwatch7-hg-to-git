'   FAHWatch7 EventLog parser 
'
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports Microsoft.Win32
Imports System.IO
Imports System.Management
Imports System.Text
Imports System.Globalization

Friend Class clsEventLog
    Private lEntries As New List(Of EventLogEntry)
    Friend ReadOnly Property LogEntries As List(Of EventLogEntry)
        Get
            Return lEntries
        End Get
    End Property
    Friend ReadOnly Property Report As String
        Get
            Dim sb As New StringBuilder
            Try
                sb.AppendLine("Reporting " & lEntries.Count & " event log entries.")
                sb.AppendLine()
                Dim iCount As Int64 = 0
                For Each eEntry As EventLogEntry In lEntries
                    iCount += 1
                    sb.AppendLine("Log entry-" & iCount.ToString)
                    If Not IsNothing(eEntry.TimeGenerated) AndAlso Not IsNothing(eEntry.Message) Then sb.AppendLine(eEntry.TimeGenerated & " : " & eEntry.Message)
                    If Not IsNothing(eEntry.UserName) Then sb.AppendLine("User : " & eEntry.UserName)
                    If Not IsNothing(eEntry.Category) Then sb.AppendLine("Category : " & eEntry.Category)
                    If Not IsNothing(eEntry.InstanceId) Then sb.AppendLine("InstanceID : " & eEntry.InstanceId)
                    If Not IsNothing(eEntry.Data) AndAlso Not eEntry.Data.Length = 0 Then
                        sb.AppendLine("<Data>")
                        sb.AppendLine()
                        For Each b As Byte In eEntry.Data
                            sb.Append(b.ToString)
                        Next
                        sb.AppendLine()
                        sb.AppendLine("</Data>")
                    End If
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
            Return sb.ToString
        End Get
    End Property
    Friend Function QueryEventLog() As Boolean
        Try
            Dim eventLogs As EventLog() = EventLog.GetEventLogs()
            Dim aLog As EventLog = Nothing, sLog As EventLog = Nothing
            For Each eLog As EventLog In eventLogs
                If eLog.Log = "Application" Then aLog = eLog
                If eLog.Log = "System" Then sLog = eLog
            Next
            If Not IsNothing(aLog) Then
                For Each lEntry As EventLogEntry In aLog.Entries
                    If lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Or lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCONTROL") Or lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCORE_") Or lEntry.Data.ToString.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Or lEntry.Data.ToString.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCORE") Or lEntry.Data.ToString.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCONTROL") Or lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Or lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCONTROL") Or lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCORE") Then
                        If lEntry.EntryType = EventLogEntryType.Error Or lEntry.EntryType = EventLogEntryType.Warning Then lEntries.Add(lEntry)
                    End If
                Next
            End If
            If Not IsNothing(sLog) Then
                For Each lEntry As EventLogEntry In sLog.Entries
                    If lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Then
                        MsgBox("Yes")
                    End If
                    If lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Or lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCONTROL") Or lEntry.Source.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCORE_") Or lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCLIENT") Or lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCONTROL") Or lEntry.Message.ToUpper(CultureInfo.InvariantCulture).Contains("FAHCORE") Then
                        If lEntry.EntryType = EventLogEntryType.Error Or lEntry.EntryType = EventLogEntryType.Warning Then lEntries.Add(lEntry)
                    End If
                    Application.DoEvents()
                Next
            End If
            Return lEntries.Count > 0
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
End Class
