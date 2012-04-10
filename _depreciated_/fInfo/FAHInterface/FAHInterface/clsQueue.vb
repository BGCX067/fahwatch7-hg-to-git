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
Imports System.Runtime.Serialization

Namespace Client
    <Serializable()>
    Public Class Queue
        Inherits Dictionary(Of String, Object)
        Implements ISerializable
        Implements IEquatable(Of Queue)
        Public ReadOnly Property ID As Short
            Get
                Return CShort(Me("id").ToString.Replace(Chr(34), "").Replace("id:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property State As String
            Get
                Return Me("state").ToString.Replace(Chr(34), "").Replace("state:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Project As String
            Get
                Return Me("project").ToString.Replace(Chr(34), "").Replace("project:", "").Trim
            End Get
        End Property
        Public ReadOnly Property run As String
            Get
                Return Me("run").ToString.Replace(Chr(34), "").Replace("run:", "").Trim
            End Get
        End Property
        Public ReadOnly Property clone As String
            Get
                Return Me("clone").ToString.Replace(Chr(34), "").Replace("clone:", "").Trim
            End Get
        End Property
        Public ReadOnly Property gen As String
            Get
                Return Me("gen").ToString.Replace(Chr(34), "").Replace("gen:", "").Trim
            End Get
        End Property
        Public ReadOnly Property core As String
            Get
                Return Me("core").ToString.Replace(Chr(34), "").Replace("core:", "").Trim
            End Get
        End Property
        Public ReadOnly Property unit As String
            Get
                Return Me("unit").ToString.Replace(Chr(34), "").Replace("unit:", "").Trim
            End Get
        End Property
        Public ReadOnly Property percentdone As Short
            Get
                Return CShort(Me("percentdone").ToString.Replace(Chr(34), "").Replace("percentdone:", "").Trim.Replace("%", ""))
            End Get
        End Property
        Public ReadOnly Property totalframes As Int32
            Get
                Return CInt(Me("totalframes").ToString.Replace(Chr(34), "").Replace("totalframes:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property framesdone As Int32
            Get
                Return CInt(Me("framesdone").ToString.Replace(Chr(34), "").Replace("framesdone:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property strAssigned As String
            Get
                Return Me("assigned").ToString.Replace(Chr(34), "").Replace("assigned:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Assigned As DateTime
            Get
                'dk timezone info, no conversion yet but will change no doubt ;)
                'Find first space
                Dim dtRet2 As New DateTime
                If DateTime.TryParse(Me("assigned").ToString.Replace(Chr(34), "").Replace("assigned:", "").Trim.Replace("-", Chr(32)), dtRet2) Then
                    Return dtRet2
                Else
                    Return DateTime.MinValue
                End If
            End Get
        End Property
        Public ReadOnly Property strTimeout As String
            Get
                Dim dtRet2 As New DateTime
                If DateTime.TryParse(Me("timeout").ToString.Replace(Chr(34), "").Replace("timeout:", "").Trim.Replace("-", Chr(32)), dtRet2) Then
                    Return dtRet2
                Else
                    Return DateTime.MinValue
                End If
            End Get
        End Property
        Public ReadOnly Property Timeout As DateTime
            Get
                Dim dtRet2 As New DateTime
                If DateTime.TryParse(Me("timeout").ToString.Replace(Chr(34), "").Replace("timeout:", "").Trim.Replace("-", Chr(32)), dtRet2) Then
                    Return dtRet2
                Else
                    Return DateTime.MinValue
                End If
            End Get
        End Property
        Public ReadOnly Property strdeadline As String
            Get
                Return Me("deadline").ToString.Replace(Chr(34), "").Replace("deadline:", "").Trim
            End Get
        End Property
        Public ReadOnly Property Deadline As DateTime
            Get
                Dim dtRet2 As New DateTime
                If DateTime.TryParse(Me("deadline").ToString.Replace(Chr(34), "").Replace("deadline:", "").Trim.Replace("-", Chr(32)), dtRet2) Then
                    Return dtRet2
                Else
                    Return DateTime.MinValue
                End If
            End Get
        End Property
        Public ReadOnly Property workserver As String
            Get
                Return Me("workserver").ToString.Replace(Chr(34), "").Replace("workserver:", "").Trim
            End Get
        End Property
        Public ReadOnly Property collectionserver As String
            Get
                Return Me("collectionserver").ToString.Replace(Chr(34), "").Replace("collectionserver:", "").Trim
            End Get
        End Property
        Public ReadOnly Property waitingon As String
            Get
                Return Me("waitingon").ToString.Replace(Chr(34), "").Replace("waitingon:", "").Trim
            End Get
        End Property
        Public ReadOnly Property attempts As Short
            Get
                Return CShort(Me("attempts").ToString.Replace(Chr(34), "").Replace("attempts:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property nextattempt As String
            Get
                Return Me("nextattempt").ToString.Replace(Chr(34), "").Replace("nextattempt:", "").Trim
            End Get
        End Property
        Public ReadOnly Property slot As Short
            Get
                Return CShort(Me("slot").ToString.Replace(Chr(34), "").Replace("slot:", "").Trim)
            End Get
        End Property
        Public ReadOnly Property eta As String
            Get
                Return Me("eta").ToString.Replace(Chr(34), "").Replace("eta:", "").Trim
            End Get
        End Property
        Public ReadOnly Property ppd As String
            Get
                Return Me("ppd").ToString.Replace(Chr(34), "").Replace("ppd:", "").Trim
            End Get
        End Property
        Public ReadOnly Property tpf As TimeSpan
            Get
                Dim tsRet As New TimeSpan
                If TimeSpan.TryParse(Me("tpf").ToString.Replace(Chr(34), "").Replace("tpf:", "").Trim, tsRet) Then
                    Return tsRet
                Else
                    Return tsRet ' Empty 
                End If
            End Get
        End Property
        Public ReadOnly Property strtpf As String
            Get
                Return Me("tpf").ToString.Replace(Chr(34), "").Replace("tpf:", "").Trim
            End Get
        End Property
        Public ReadOnly Property basecredit As String ' uint or long/double, idk 
            Get
                Return Me("basecredit").ToString.Replace(Chr(34), "").Replace("basecredit:", "").Trim
            End Get
        End Property
        Public ReadOnly Property creditestimate As String ' see above
            Get
                Return Me("creditestimate").ToString.Replace(Chr(34), "").Replace("creditestimate:", "").Trim
            End Get
        End Property
        Public Shared Function Parse(value As String) As Queue
            Try
                Dim o = JObject.Parse(value)
                Dim queue = New Queue()
                For Each prop As Object In o.Properties()
                    queue.Add(prop.Name, GetValue(prop))
                Next
                Return queue
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return New Queue
            End Try
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
        Public Function Equals1(other As Queue) As Boolean Implements System.IEquatable(Of Queue).Equals
            Return Me.Project = other.Project And Me.run = other.run And Me.clone = other.clone And Me.gen = other.gen And Me.Assigned = other.Assigned And Me.State = other.State And Me.percentdone = other.percentdone And Me.waitingon = other.waitingon And Me.tpf = other.tpf
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
