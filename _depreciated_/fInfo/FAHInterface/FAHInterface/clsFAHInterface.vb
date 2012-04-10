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

Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Net
Imports System.Runtime.Serialization.XmlObjectSerializer
Imports System.ServiceModel.Web
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Data
Imports Newtonsoft.Json


'Namespace fahInterface should hold expose all public functions

Namespace FAHInterface

    Public Class clsFAHInterface
        'Each interface should run on it's own thread.
        Public Class clsInterface
            Private _Identifier As String
            Private Shared ConnectDone As New ManualResetEvent(False)
            Private Shared SendDone As New ManualResetEvent(False)
            Private Shared receiveDone As New ManualResetEvent(False)
            Private Shared response As String
            Public Shared Event ResponceRecieved(ByVal Response As String)
            'Private WithEvents bgwClient As New System.ComponentModel.BackgroundWorker
            Private Client As Socket
            Private _Interface As clsInterface = Me


            Public Property Identifier As String
                Get
                    Return _Identifier
                End Get
                Set(ByVal value As String)
                    _Identifier = value
                End Set
            End Property
            Public ReadOnly Property IsConnected As Boolean
                Get
                    Try
                        Return Client.Connected
                    Catch ex As Exception
                        Return False
                    End Try
                End Get
            End Property
            Public Function ConnectClient(ByVal IP As String, ByVal Port As String) As Boolean
                Return ConnectClient(IP, CInt(Port))
            End Function
            Public Function ConnectClient(ByVal IP As String, ByVal Port As Integer) As Boolean
                Try
                    ' For this example use local machine.

                    Dim ipHostInfo As IPHostEntry : ipHostInfo = Dns.GetHostEntry(IP)
                    Dim ipAddress As IPAddress : ipAddress = ipHostInfo.AddressList(1)
                    Dim remoteEP As IPEndPoint : remoteEP = New IPEndPoint(ipAddress, Port)

                    ' Create a TCP/IP socket.
                    Client = New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)


                    ' Connect to the remote endpoint.
                    Client.BeginConnect(remoteEP, New AsyncCallback(AddressOf ConnectCallback), Client)
                    ' Wait for connect.
                    If ConnectDone.WaitOne() Then
                        Receive(Client)
                    End If
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End Function
            Public Function Disconnect() As Boolean
                Try
                    If Client.Connected Then
                        Client.Close()
                        Return Client.Connected
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End Function
            Public Function SendCommand(ByVal Command As String) As Boolean
                ' Client string should be host or ip
                Try
                    With Client
                        If Command.Length > 0 Then
                            If Not Command.Chars(Command.Length - 1) = vbLf Then Command &= vbLf
                        End If
                        Dim b() As Byte = StrToByteArray(Command)

                        .Send(b)

                        Receive(Client)
                        receiveDone.WaitOne()
                    End With

                Catch ex As Exception

                End Try
            End Function
            Public Shared Function StrToByteArray(ByVal str As String) As Byte()
                Dim encoding As New System.Text.UTF8Encoding()
                Return encoding.GetBytes(str)
            End Function 'StrToByteArray
            Public Shared Function ByteToString(ByVal Value() As Byte) As String
                Try
                    Dim rString As String = ""
                    For Each b As Byte In Value
                        If b = 0 Then Exit For
                        rString &= ChrW(b)
                    Next
                    Return rString
                Catch ex As Exception
                    Logwindow.writeerror("ByteToString", Err)
                    Return vbNullString
                End Try
            End Function
            Public Class StateObject
                ' Client socket.
                Public workSocket As Socket = Nothing
                ' Size of receive buffer.
                Public BufferSize As Integer = 256
                ' Receive buffer.
                Public buffer(256) As Byte
                ' Received data string.
                Public sb As New StringBuilder()
            End Class 'StateObject
#Region "Connection"
            Private Shared Sub Connect(ByVal remoteEP As EndPoint, ByVal client As Socket)
                client.BeginConnect(remoteEP, AddressOf ConnectCallback, client)
                ConnectDone.WaitOne()
            End Sub 'Connect
            Private Shared Sub ConnectCallback(ByVal ar As IAsyncResult)
                Try
                    ' Retrieve the socket from the state object.
                    Dim client As Socket = CType(ar.AsyncState, Socket)

                    ' Complete the connection.
                    client.EndConnect(ar)

                    Console.WriteLine("Socket connected to {0}", _
                        client.RemoteEndPoint.ToString())

                    ' Signal that the connection has been made.
                    ConnectDone.Set()
                Catch e As Exception
                    Console.WriteLine(e.ToString())
                End Try
            End Sub
#End Region
#Region "Send"
            Private Shared Sub Send(ByVal client As Socket, ByVal data As [String])
                ' Convert the string data to byte data using ASCII encoding.
                Dim byteData As Byte() = Encoding.ASCII.GetBytes(data)
                ' Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, AddressOf SendCallback, client)
            End Sub 'Send
            Private Shared Sub SendCallback(ByVal ar As IAsyncResult)
                Try
                    ' Retrieve the socket from the state object.
                    Dim client As Socket = CType(ar.AsyncState, Socket)

                    ' Complete sending the data to the remote device.
                    Dim bytesSent As Integer = client.EndSend(ar)
                    Console.WriteLine("Sent {0} bytes to server.", bytesSent)

                    ' Signal that all bytes have been sent.
                    SendDone.Set()
                Catch e As Exception
                    Console.WriteLine(e.ToString())
                End Try
            End Sub 'SendCallback
#End Region
#Region "Receive"
            Private Shared Sub Receive(ByVal client As Socket)
                Try
                    ' Create the state object.
                    Dim state As New StateObject()
                    state.workSocket = client
                    ' Begin receiving the data from the remote device.
                    client.BeginReceive(state.buffer, 0, state.BufferSize, 0, AddressOf ReceiveCallback, state)
                Catch e As Exception
                    Console.WriteLine(e.ToString())
                End Try
            End Sub
            Private Shared IsBussy As Boolean = False
            Private _lastCommand As String = ""
            Private Shared Sub ReceiveCallback(ByVal ar As IAsyncResult)
                Try
                    ' Retrieve the state object and the client socket 
                    ' from the asynchronous state object.
                    While IsBussy
                        Threading.Thread.SpinWait(500)
                        Exit While
                    End While
                    IsBussy = True
                    Dim state As StateObject = CType(ar.AsyncState, StateObject)
                    Dim client As Socket = state.workSocket

                    ' Read data from the remote device.
                    Dim bytesRead As Integer = client.EndReceive(ar)


                    If state.sb.ToString.Length > 0 Then
                        ' New responce
                        If ByteToString(state.buffer).Contains("PyON") Or state.sb.ToString.Contains("PyON") Then
                            ' Pyon header
                            If Not ByteToString(state.buffer).Contains("]" & vbLf & "---" & vbLf & "> ") Then
                                ' Pyon info not complete 
                                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))
                                '  Get the rest of the data.
                                client.BeginReceive(state.buffer, 0, state.BufferSize, 0, AddressOf ReceiveCallback, state)
                            Else
                                ' Info complete
                                If state.sb.Length = 0 Then
                                    response = ByteToString(state.buffer)
                                Else
                                    state.sb.Append(ByteToString(state.buffer))
                                    response = state.sb.ToString()
                                End If
                                ' Signal that all bytes have been received.
                                Console.WriteLine(response)
                                receiveDone.Set()

                                RaiseEvent ResponceRecieved(response)
                                'Dim ms As New System.IO.MemoryStream(state.buffer)
                                'Dim nQueue As New QueueInfo
                                'Dim djson As New DataContractJsonSerializer(GetType(QueueInfo))
                                'nQueue = djson.ReadObject(ms)
                            End If
                        Else
                            ' Text input
                            If Not ByteToString(state.buffer).Contains(Chr(10) & Chr(62) & Chr(32)) Then
                                ' There is more data, so store the data received so far.
                                state.sb.Append(ByteToString(state.buffer))
                                client.BeginReceive(state.buffer, 0, state.BufferSize, 0, AddressOf ReceiveCallback, state)
                            Else
                                ' All the data has arrived; put it in response.
                                If state.sb.Length = 0 Then
                                    response = ByteToString(state.buffer)
                                Else
                                    state.sb.Append(ByteToString(state.buffer))
                                    response = state.sb.ToString()
                                End If
                                ' Signal that all bytes have been received.
                                ' response = response.Replace("PyON 1 units", "") : response = Mid(response, 2, response.IndexOf(vbLf & "---" & vbLf)) : Dim ms As New System.IO.MemoryStream(StrToByteArray(response))
                                receiveDone.Set()
                                RaiseEvent ResponceRecieved(response)
                                'Dim nQueue As New QueueInfo
                                'Dim b(0 To ms.Length) As Byte
                                'ms.Read(b, 0, ms.Length)
                                'ms.Position = 0
                                'Dim djson As New DataContractJsonSerializer(GetType(QueueInfo))
                                'Dim js As New JavaScriptSerializer
                                'nQueue = js.Deserialize(response, GetType(QueueInfo))

                                'nQueue = djson.ReadObject(ms)
                                'Console.WriteLine(response)


                            End If
                        End If
                    Else
                        If ByteToString(state.buffer).Contains("PyON") Then
                            ' Pyon header
                            If Not ByteToString(state.buffer).Contains(vbLf & "---" & vbLf & "> ") Then
                                ' Pyon info not complete 
                                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead))
                                '  Get the rest of the data.
                                client.BeginReceive(state.buffer, 0, state.BufferSize, 0, AddressOf ReceiveCallback, state)
                            Else
                                ' Info complete
                                If ByteToString(state.buffer).Contains("PyON") Then
                                    response = ByteToString(state.buffer)
                                Else
                                    state.sb.Append(ByteToString(state.buffer))
                                    response = state.sb.ToString()
                                End If
                                ' Signal that all bytes have been received.
                                Console.WriteLine(response)
                                receiveDone.Set()
                                RaiseEvent ResponceRecieved(response)
                                'Dim ms As New System.IO.MemoryStream(state.buffer)
                                'Dim nQueue As New QueueInfo
                                'Dim djson As New DataContractJsonSerializer(GetType(QueueInfo))
                                'nQueue = djson.ReadObject(ms)
                            End If
                        Else
                            ' Text input
                            If Not ByteToString(state.buffer).Contains(Chr(10) & Chr(62) & Chr(32)) Then
                                ' There is more data, so store the data received so far.
                                state.sb.Append(ByteToString(state.buffer))
                                client.BeginReceive(state.buffer, 0, state.BufferSize, 0, AddressOf ReceiveCallback, state)
                            Else
                                ' All the data has arrived; put it in response.
                                If state.sb.Length = 0 Then
                                    response = ByteToString(state.buffer)
                                Else
                                    state.sb.Append(ByteToString(state.buffer))
                                    response = state.sb.ToString()
                                End If
                                ' Signal that all bytes have been received.

                                Console.WriteLine(response)
                                receiveDone.Set()

                                RaiseEvent ResponceRecieved(response)
                            End If
                        End If
                    End If

                    IsBussy = False
                Catch e As Exception
                    Console.WriteLine(e.ToString())
                End Try
            End Sub
#End Region
        End Class
        Private WithEvents _Clients As New List(Of clsInterface)
        Public Event ResponseRecieved(ByVal Response As String)
        Public Sub New()
            'Anything which needs initializing starts here.

        End Sub
        Public ReadOnly Property Clients As List(Of clsInterface)
            Get
                Return _Clients
            End Get
        End Property
        Public Sub RRecieved(ByVal Response As String)
            RaiseEvent ResponseRecieved(Response)
        End Sub
        Public Function ConnectToClient(ByVal HOST As String, ByVal PORT As String, Optional ByVal Password As String = "") As Boolean
            Try
                'Create a client object, issue a connect with callback on seperate thread
                Dim nClient As New clsInterface
                AddHandler nClient.ResponceRecieved, AddressOf RRecieved

                If nClient.ConnectClient(HOST, PORT) Then
                    If Password <> "" Then
                        ' Authenticate needs work!
                        nClient.SendCommand("Auth " & Password)
                    End If

                    Clients.Add(nClient)
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                ' Error handling 

                Return False
            End Try
        End Function

        ' temporary!!
        Public Sub SendCommand(ByVal Command As String, ByVal Client As clsInterface)
            Clients(0).SendCommand(Command)
        End Sub
        Public Sub Disconnect(ByVal Client As clsInterface)
            Clients(0).Disconnect()
        End Sub
        Public ReadOnly Property IsConnected(ByVal Client As clsInterface) As Boolean
            Get
                Return Clients(0).IsConnected
            End Get
        End Property

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

