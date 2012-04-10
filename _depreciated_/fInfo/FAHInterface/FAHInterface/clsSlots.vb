'/*
' * Code converted to vb.net from Harlam357 code hosted on http://code.google.com/p/hfm-net/source/browse/#svn%2Ftrunk%2Fsrc%2FHFM.Client 
' * Edits made mtm
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
Imports System.Collections.Generic
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.Runtime.Serialization
Namespace Client
    <Serializable()>
    Public Class Slots
        Inherits Dictionary(Of String, String)
        Implements ISerializable
        Implements IEquatable(Of Slots)
        Public ReadOnly Property ID As String
            Get
                Return Me("id").ToString.Replace(Chr(34), "").Replace("id:", "").Trim
            End Get
        End Property
        Public ReadOnly Property status As String
            Get
                Return Me("status").ToString.Replace(Chr(34), "").Replace("status:", "").Trim
            End Get
        End Property
        Public ReadOnly Property description As String
            Get
                Return Me("description").ToString.Replace(Chr(34), "").Replace("description:", "").Trim
            End Get
        End Property
        Public Property Options() As Options
            Get
                Return m_Options
            End Get
            Private Set(value As Options)
                m_Options = value
            End Set
        End Property
        Private m_Options As Options

        Public Shared Function Parse(value As String) As Slots
            Dim o = JObject.Parse(value)
            Dim slots = New Slots()
            For Each prop As Object In o.Properties()
                If prop.Name.Equals("options") Then
                    Dim optionsValue = prop.ToString()
                    ' have to strip off "options" portion of the JSON
                    slots.Options = Options.Parse(optionsValue.Substring(optionsValue.IndexOf("{"c)))
                Else
                    slots.Add(prop.Name, GetValue(prop))
                End If
            Next
            Return slots
        End Function
        Private Shared Function GetValue(prop As JProperty) As Object
            If prop.Value.Type.Equals(JTokenType.[String]) Then
                '  Return DirectCast(prop, String)
                Return DirectCast(prop.ToString, String)
            End If
            If prop.Value.Type.Equals(JTokenType.[Integer]) Then
                Return CInt(prop)
            End If
            Return [String].Empty
        End Function

        Public Function Equals1(other As Slots) As Boolean Implements System.IEquatable(Of Slots).Equals
            Return Me.ID = other.ID And Me.description = other.description And Me.status = other.status
        End Function
#Region "Log extender"
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
#End Region
    End Class
End Namespace
