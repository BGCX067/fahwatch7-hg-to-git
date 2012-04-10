'   fTray MySettings class
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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Public Class clsSettings
    <Serializable()>
    Public Class Settings
        Public Enum eStartMethod
            Registry
            RegistryMinimized
        End Enum
        Public Enum eEocLimit
            None = 0
            Minimal = 1
            OneDay = 2
            OneWeek = 3
            OneMonth = 4
        End Enum
        Public _WTF As String
        Public EocLimit As eEocLimit
        Public EOCid As String
        Public StartWithWindows As Boolean
        Public StartMethod As eStartMethod
        Public AutoStartClient As Boolean
        Public LastEOCUpdate As DateTime
        Public LastSummaryUpdate As DateTime
        Public NonFatal As New Collection
        Public URLsummary As String
        Public UseEOC As Boolean
        Public EOCNotify As Boolean
        Public StartMinimized As Boolean
        Public SafeMode As Boolean
    End Class
    Private _Settings As New Settings
    Public Property MySettings As Settings = _Settings
    Private fileSettings As String = ""
    Private _Empty As Boolean
    Public ReadOnly Property IsEmpty As Boolean
        Get
            Try
                Return _Empty
            Catch ex As Exception
                Return True
            End Try
        End Get
    End Property
    Public Sub New(ByVal Datalocation As String)
        Try
            fileSettings = Datalocation & "\config.dat"
            If My.Computer.FileSystem.FileExists(fileSettings) Then
                ReadSettings()
            Else
                _Empty = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function SaveSettings() As Boolean
        Try
            Dim Serializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileSettings, FileMode.Create, FileAccess.Write, FileShare.None)
            Serializer.Serialize(DataFile, MySettings)
            DataFile.Close()
            Serializer = Nothing
            Return True
        Catch ex As Exception

            Return False
        End Try
    End Function
    Public Function ReadSettings() As Boolean
        Try
            Dim Deserializer As New BinaryFormatter
            Dim DataFile As New FileStream(fileSettings, FileMode.Open, FileAccess.Read, FileShare.None)
            MySettings = CType(Deserializer.Deserialize(DataFile), Settings)
            DataFile.Close()
            Deserializer = Nothing
            DataFile = Nothing
            _Empty = False
            Return True
        Catch ex As Exception
            _Empty = True
            Return False
        End Try
        
    End Function
    Public Sub SetDefaults()
        Try
            With _Settings
                .URLsummary = "http://fah-web.stanford.edu/psummary.html"
                .StartWithWindows = False
                .StartMethod = Settings.eStartMethod.RegistryMinimized
                .LastSummaryUpdate = DateTime.MinValue
                .LastEOCUpdate = DateTime.MinValue
                .EocLimit = Settings.eEocLimit.OneWeek
                .AutoStartClient = False
                ._WTF = GetPassword()
                With .NonFatal
                    .Add("6E", "6E")
                    .Add("62", "62")
                    .Add("64", "64")
                End With
                .UseEOC = False
                .EOCNotify = True
                .StartMinimized = False
                .SafeMode = True
            End With
        Catch ex As Exception

        End Try
    End Sub
End Class
