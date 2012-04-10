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
Imports System.Diagnostics
Imports System.Net.Sockets
Imports System.Text
Namespace Client
    Public Class ClientAccess
        Implements IDisposable
        ' TO DO: Add the new or changed objects to the event notification, enabling updating ui without parsing all objects
        Public Event WrongPassword()
        Public Event SlotsChanged()
        Public Event QueueChanged()
        Public Event OptionsChanged() ' not implemented yet
        Public Event Disconnected()


        Private Const InternalBufferSize As Integer = 1024
        Private Const SocketBufferSize As Integer = 8196
        Private Const DefaultTimeoutLength As Integer = 2000

        Private WithEvents _tcpClient As TcpClient
        Private _stream As NetworkStream
        Private ReadOnly _readBuffer As StringBuilder
        Private WithEvents tSQ As New Timers.Timer
        Private WithEvents tOpt As New Timers.Timer
        Public ReadOnly Property IsSQUpdating As Boolean
            Get
                Return tSQ.Enabled
            End Get
        End Property
        Public ReadOnly Property IsOptUpdating As Boolean
            Get
                Return tOpt.Enabled
            End Get
        End Property
        Public WriteOnly Property ClientInterval_SQ As Double
            Set(value As Double)
                tSQ.Interval = value
            End Set
        End Property
        Public WriteOnly Property ClientInterval_Opt As Double
            Set(value As Double)
                tOpt.Interval = value
            End Set
        End Property
        Public Sub StartUpdates(Optional SQInterval As Double = Double.MinValue, Optional OptInterval As Double = Double.MinValue)
            Try
                LogWindow.WriteLog("Starting client interface updates - slot/queue=" & SQInterval.ToString & "ms options=" & OptInterval.ToString & "ms.")
                If Not SQInterval = Double.MinValue Then tSQ.Interval = SQInterval
                If Not OptInterval = Double.MinValue Then tOpt.Interval = OptInterval
                tSQ.AutoReset = True
                tOpt.AutoReset = True
                tSQ.Enabled = True
                tOpt.Enabled = True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
        End Sub
        Public Sub StopUpdating()
            tSQ.Enabled = False
            tOpt.Enabled = False
        End Sub
        Private Sub tOpt_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles tOpt.Elapsed
            Debug.WriteLine("Client options - " & DateTime.Now.ToString("hh:mm:ss-tt"))
            If Not Connected Then
                tOpt.Enabled = False
                Exit Sub
            End If
            SendCommand("options -d")
            Update()
        End Sub
        Private Sub tSQ_Elapsed(Sender As Object, e As System.Timers.ElapsedEventArgs) Handles tSQ.Elapsed
            Debug.WriteLine("Client SQ - " & DateTime.Now.ToString("hh:mm:ss-tt"))
            If Not Connected Then
                tSQ.Enabled = False
                Exit Sub
            End If
            SendCommand("queue-info")
            Update()
            SendCommand("slot-info")
            Update()
        End Sub
#Region "TcpClient Properties"
        Private _bWrongPassword As Boolean = True ' requires password to be used even for local clients
        Private _PassWord As String = ""
        Private _HostName As String = ""
        Private _LastResponce As String = ""
        Private _Proxy As String = ""
        Public ReadOnly Property LastResponce As String
            Get
                Return _LastResponce
            End Get
        End Property
        Public Property HostName As String
            Get
                Return _HostName
            End Get
            Set(value As String)
                _HostName = value
            End Set
        End Property
        Private _iPort As Integer
        Public Property Port As Integer
            Get
                Return _iPort
            End Get
            Set(value As Integer)
                _iPort = value
            End Set
        End Property
        Public ReadOnly Property Password As String
            Get
                Return _PassWord
            End Get
        End Property
        Public ReadOnly Property ValidPassword As Boolean
            Get
                Return (_bWrongPassword = False)
            End Get
        End Property
        Public ReadOnly Property Connected() As Boolean
            Get
                Return If(_tcpClient.Client Is Nothing, False, _tcpClient.Connected)
            End Get
        End Property
        Public Property ConnectTimeout() As Integer
            Get
                Return m_ConnectTimeout
            End Get
            Set(value As Integer)
                m_ConnectTimeout = value
            End Set
        End Property
        Private m_ConnectTimeout As Integer
        Public Property SendTimeout() As Integer
            Get
                Return m_SendTimeout
            End Get
            Set(value As Integer)
                m_SendTimeout = value
            End Set
        End Property
        Private m_SendTimeout As Integer
        Public Property SendBufferSize() As Integer
            Get
                Return m_SendBufferSize
            End Get
            Set(value As Integer)
                m_SendBufferSize = value
            End Set
        End Property
        Private m_SendBufferSize As Integer

        Public Property ReceiveTimeout() As Integer
            Get
                Return m_ReceiveTimeout
            End Get
            Set(value As Integer)
                m_ReceiveTimeout = value
            End Set
        End Property
        Private m_ReceiveTimeout As Integer

        Public Property ReceiveBufferSize() As Integer
            Get
                Return m_ReceiveBufferSize
            End Get
            Set(value As Integer)
                m_ReceiveBufferSize = value
            End Set
        End Property
        Private m_ReceiveBufferSize As Integer
#End Region
#Region "Data Properties"
        Public Property Info() As Info
            Get
                Return m_Info
            End Get
            Set(value As Info)
                m_Info = value
            End Set
        End Property
        Private m_Info As Info
        Public Overloads Property Slots() As Slots
            Get
                Return m_Slots
            End Get
            Private Set(value As Slots)
                m_Slots = value
            End Set
        End Property
        Public ReadOnly Property SlotCount As Integer
            Get
                Return _lslots.Count
            End Get
        End Property
        Public Overloads Property Slots(Index) As Slots
            Get
                If _lslots.Count = 0 Then Return Nothing
                Return _lslots(Index)
            End Get
            Set(value As Slots)
                If _lslots.Count <= Index Then
                    _lslots(Index) = value
                Else
                    _lslots.Add(value)
                End If
            End Set
        End Property
        Public ReadOnly Property lSlots As List(Of Slots)
            Get
                Return _lslots
            End Get
        End Property
        Private m_Slots As Slots
        Public Property Options() As Options
            Get
                Return m_Options
            End Get
            Private Set(value As Options)
                m_Options = value
            End Set
        End Property
        Private m_Options As Options

        Public Overloads Property Queue() As Queue
            Get
                Return m_Queue
            End Get
            Private Set(value As Queue)
                m_Queue = value
            End Set
        End Property
        Public ReadOnly Property QueueCount As Integer
            Get
                Return _lQueue.Count
            End Get
        End Property
        Public Overloads Property Queue(Index As Short) As Queue
            Get
                Return _lQueue(Index)
            End Get
            Set(value As Queue)
                If _lQueue.Count <= Index Then
                    _lQueue(Index) = value
                Else
                    _lQueue.Add(value)
                End If
            End Set
        End Property
        Public ReadOnly Property lQueue() As List(Of Queue)
            Get
                Return _lQueue
            End Get
        End Property
        Private m_Queue As Queue

        Public Property LogFile() As String
            Get
                Return m_LogFile
            End Get
            Set(value As String)
                m_LogFile = value
            End Set
        End Property
        Private m_LogFile As String
#End Region
        Public Sub New()
            ConnectTimeout = DefaultTimeoutLength
            SendTimeout = DefaultTimeoutLength
            SendBufferSize = SocketBufferSize
            ReceiveTimeout = DefaultTimeoutLength
            ReceiveBufferSize = SocketBufferSize
            _tcpClient = CreateClient()
            _readBuffer = New StringBuilder()
        End Sub
        Private Function CreateClient() As TcpClient
            Return New TcpClient() With {.SendTimeout = SendTimeout, .SendBufferSize = SendBufferSize, .ReceiveTimeout = ReceiveTimeout, .ReceiveBufferSize = ReceiveBufferSize}

        End Function
        Public Sub Disconnect()
            Try
                _tcpClient.Client.Close()
                RaiseEvent Disconnected()
            Catch ex As Exception

            End Try
        End Sub
        Public Sub Connect(hostname As String, port As Integer)
            Try

                _tcpClient = CreateClient()

                _HostName = hostname : _iPort = port
                Dim ar As IAsyncResult = _tcpClient.BeginConnect(hostname, port, Nothing, Nothing)
                Dim wh As System.Threading.WaitHandle = ar.AsyncWaitHandle
                Try
                    If Not ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(ConnectTimeout), False) Then
                        _tcpClient.Close()
                        Throw New TimeoutException("Client connection has timed out.")
                    End If
                    Try
                        _tcpClient.EndConnect(ar)
                    Catch ex As Exception
                        RaiseEvent Disconnected()
                    End Try
                Finally
                    wh.Close()
                End Try
            Catch ex As SocketException
                RaiseEvent Disconnected()
            End Try
        End Sub
        Public Enum eCallBack
            Password
        End Enum
        Public CallBack As eCallBack = eCallBack.Password
        Public Sub SendCommand(command As String)
            If Not Connected Then Exit Sub
            Try
                Try
                    If Not command.EndsWith(vbLf) Then
                        command += vbLf
                    End If
                    If command.Contains("auth ") And command.IndexOf(" ", 5) = -1 Then
                        _PassWord = command.Replace("auth ", "").Trim(Chr(34)).TrimEnd
                    End If
                    If _stream Is Nothing Then
                        _stream = _tcpClient.GetStream()
                    End If
                    Dim buffer = Encoding.ASCII.GetBytes(command)
                    _stream.Write(buffer, 0, buffer.Length)
                Catch ex As Exception
                    RaiseEvent Disconnected()
                End Try
            Catch ex As Exception

            End Try
        End Sub
        Public Sub Update()
            Try
                If Not Connected Then
                    'Throw New InvalidOperationException("Client is not connected.")
                    Exit Sub
                End If

                If _stream Is Nothing Then
                    _stream = _tcpClient.GetStream()
                End If
                Dim buffer = New Byte(InternalBufferSize - 1) {}

                'int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                'while (bytesRead != 0)
                '{
                '   _readBuffer.Append(Encoding.ASCII.GetString(buffer));
                '   bytesRead = _stream.Read(buffer, 0, buffer.Length);
                '}

                Try
                    While _stream.DataAvailable
                        _stream.Read(buffer, 0, buffer.Length)
                        _readBuffer.Append(Encoding.ASCII.GetString(buffer))
                    End While
                Catch ex As IO.IOException
                    RaiseEvent Disconnected()
                    ' Supplying wrong password will cause a disconnect at some time, catch here!
                End Try
                ProcessBuffer()
            Catch ex As Exception

            End Try
        End Sub
        Public ReadOnly Property ReadBuffer As String
            Get
                Return _readBuffer.ToString
            End Get
        End Property
        Private Sub ProcessBuffer()
            Try
                Dim value As String = _readBuffer.ToString()
                If CallBack = eCallBack.Password Then
                    If value.ToUpper.Contains("INVALID PASSWORD") Then
                        _bWrongPassword = True
                        RaiseEvent WrongPassword()
                        Exit Sub
                    Else
                        CallBack = -1
                        _bWrongPassword = False
                    End If
                End If
                _LastResponce = value
                If value.Contains("PyON") AndAlso value.Contains("---" & vbLf & ">") Then
                    Dim json As JsonMessage
                    While (InlineAssignHelper(json, GetNextJsonValue(value))) IsNot Nothing
                        ProcessJsonMessage(json)
                        value = If(json.NextStartIndex < value.Length, value.Substring(json.NextStartIndex), [String].Empty)
                    End While

                End If
                _readBuffer.Remove(0, _readBuffer.Length)
                _readBuffer.Append(value)
            Catch ex As Exception

            End Try
        End Sub
        Private _lQueue As New List(Of Queue)
        Private _lslots As New List(Of Slots)
        Private Sub ParseSlots(Text As String)
            _lslots = New List(Of Slots)
            Do
                Dim iPos As Int16 = InStr(Text.IndexOf("{") + 2, Text, "}")
                iPos = InStr(iPos + 1, Text, "}")
                Dim tmpStr As String = Text.Substring(0, iPos + 1)
                Text = Text.Replace(tmpStr, "")
                Try
                    _lslots.Add(Slots.Parse(tmpStr))
                Catch ex As Exception

                End Try
            Loop While Text.IndexOf("{") <> -1
        End Sub
        Private Sub ParseQueue(Text As String)
            _lQueue = New List(Of Queue)
            Do
                Dim tmpStr As String = Text.Substring(Text.IndexOf("{"), (Text.IndexOf("}") - Text.IndexOf("{")) + 1)
                Try
                    _lQueue.Add(Queue.Parse(tmpStr))
                Catch ex As Exception
                End Try
                Text = Text.Replace(tmpStr, "")
            Loop While Text.IndexOf("{") <> -1
        End Sub
        Private _jsonOPTIONS As String = "", _jsonQUEUE As String = "", _jsonINFO As String = "", _jsonSLOTS As String = ""
        Public ReadOnly Property JsON_Queue As String
            Get
                Return _jsonQUEUE
            End Get
        End Property
        Public ReadOnly Property JsON_Slots As String
            Get
                Return _jsonSLOTS
            End Get
        End Property
        Public ReadOnly Property JsON_Info As String
            Get
                Return _jsonINFO
            End Get
        End Property
        Public ReadOnly Property JsON_Options As String
            Get
                Return _jsonOPTIONS
            End Get
        End Property
        Private Sub ProcessJsonMessage(json As JsonMessage)
            'Check for changes between updates, enable raising events for change notifications
            'Seems to work without taxing cpu much with 1s queue/slot and 2500ms options, 500ms hw measurements taken.
            Try
                Select Case json.Name
                    Case "slots"
                        Dim Text As String = json.Value
                        If _jsonSLOTS = "" Then
                            _jsonSLOTS = Text
                            ParseSlots(Text)
                            RaiseEvent SlotsChanged()
                            Exit Sub
                        Else
                            If Text <> _jsonSLOTS Then
                                _jsonSLOTS = Text
                                ParseSlots(Text)
                                RaiseEvent SlotsChanged()
                                Exit Sub
                            End If
                        End If
                        Exit Select
                        ' TODO Remove if needed
                        'Dim Text As String = json.Value
                        'Dim nSlots As New List(Of Slots)
                        'Do
                        '    Dim iPos As Int16 = InStr(Text.IndexOf("{") + 2, Text, "}")
                        '    iPos = InStr(iPos + 1, Text, "}")
                        '    Dim tmpStr As String = Text.Substring(0, iPos + 1)
                        '    Text = Text.Replace(tmpStr, "")
                        '    Try
                        '        nSlots.Add(Slots.Parse(tmpStr))
                        '    Catch ex As Exception

                        '    End Try
                        'Loop While Text.IndexOf("{") <> -1
                        'If nSlots.Count <> _lslots.Count Then
                        '    'notify of change!
                        '    _lslots = nSlots
                        '    RaiseEvent SlotsChanged()
                        '    Exit Select
                        'Else
                        '    For xInt As Int16 = 0 To nSlots.Count - 1
                        '        If Not nSlots(xInt).Equals1(_lslots(xInt)) Then
                        '            'notify of change!
                        '            _lslots = nSlots
                        '            RaiseEvent SlotsChanged()
                        '            Exit Select
                        '        End If

                        '    Next
                        'End If
                        'Exit Select
                    Case "options"
                        ' TODO Check if string comparison isn't quicker then object comparison
                        ' TODO Check values for object arrays
                        If IsNothing(Options) Then
                            Options = New Options
                            Options = Client.Options.Parse(json.Value)
                            _jsonOPTIONS = json.Value
                            RaiseEvent OptionsChanged()
                            Exit Sub
                        ElseIf _jsonOPTIONS = "" Then
                            _jsonOPTIONS = json.Value
                            Options = Client.Options.Parse(json.Value)
                            RaiseEvent OptionsChanged()
                            Exit Sub
                        Else
                            If _jsonOPTIONS <> json.Value Then
                                Options = Client.Options.Parse(json.Value)
                                _jsonOPTIONS = json.Value
                                RaiseEvent OptionsChanged()
                                Exit Sub
                            End If
                        End If
                        ' TODO Remove if needed
                        'Dim nOptions As New Options
                        'nOptions = Client.Options.Parse(json.Value)
                        'If Options.Count <> nOptions.Count Then
                        '    Options = nOptions
                        '    RaiseEvent OptionsChanged()
                        'Else
                        '    If Not Options.Equals1(nOptions) Then
                        '        Options = nOptions
                        '        RaiseEvent OptionsChanged()
                        '    End If
                        'End If
                        Exit Select

                    Case "units"
                        If _jsonQUEUE = "" Then
                            _jsonQUEUE = json.Value
                            ParseQueue(_jsonQUEUE)
                            RaiseEvent QueueChanged()
                            Exit Sub
                        Else
                            ' TODO Check this
                            Dim Text As String = json.Value
                            Dim nQueue = New List(Of Queue)
                            Do
                                Dim tmpStr As String = Text.Substring(Text.IndexOf("{"), (Text.IndexOf("}") - Text.IndexOf("{")) + 1)
                                Try
                                    nQueue.Add(Queue.Parse(tmpStr))
                                Catch ex As Exception
                                End Try
                                Text = Text.Replace(tmpStr, "")
                            Loop While Text.IndexOf("{") <> -1
                            If _lQueue.Count <> nQueue.Count Then
                                _lQueue = nQueue
                                _jsonQUEUE = json.Value
                                RaiseEvent QueueChanged()
                                Exit Sub
                            Else
                                Dim iIndex As Int16 = 0
                                For Each oldQueue As Queue In _lQueue
                                    If Not oldQueue.Equals1(nQueue(iIndex)) Then
                                        _lQueue = nQueue
                                        _jsonQUEUE = json.Value
                                        RaiseEvent QueueChanged()
                                        Exit Sub
                                    End If
                                    iIndex += 1
                                Next
                            End If
                        End If
                        Exit Select
                        'If nQueue.Count <> _lQueue.Count Then
                        '    'notify change!
                        '    _lQueue = nQueue
                        '    RaiseEvent QueueChanged()
                        '    Exit Select
                        'Else
                        '    For xInt As Int16 = 0 To nQueue.Count - 1
                        '        If Not nQueue(xInt).Equals1(_lQueue(xInt)) Then
                        '            'notify change!
                        '            _lQueue = nQueue
                        '            RaiseEvent QueueChanged()
                        '            Exit Select
                        '        End If
                        '    Next
                        'End If
                    Case "info"
                        ' Parse each section seperate
                        _jsonINFO = json.Value
                        Info = Info.Parse(json.Value)
                        Exit Sub
                    Case "log-restart"
                        ' set the platform specific new line character(s)
                        Dim logFile__1 As String = json.Value.Replace("\" & "n", Environment.NewLine)
                        LogFile = logFile__1
                        Exit Sub
                End Select
            Catch ex As Exception

            End Try
        End Sub
        Private Shared Function GetNextJsonValue(value As String) As JsonMessage
            Debug.Assert(value IsNot Nothing)

            ' find the header
            Dim messageIndex As Integer = value.IndexOf("PyON ")
            If messageIndex < 0 Then
                Return Nothing
            End If
            ' "PyON " plus version number and another space - i.e. "PyON 1 "
            messageIndex += 7

            Dim startIndex As Integer = value.IndexOf(ControlChars.Lf, messageIndex)
            If startIndex < 0 Then
                Return Nothing
            End If

            ' find the footer
            Dim endIndex As Integer = value.IndexOf("---" & vbLf, startIndex)
            If endIndex < 0 Then
                Return Nothing
            End If

            Dim jsonMessage = New JsonMessage()
            ' get the PyON message name
            jsonMessage.Name = value.Substring(messageIndex, startIndex - messageIndex)

            ' get the PyON message
            Dim pyon As String = value.Substring(startIndex, endIndex - startIndex)
            ' replace PyON values with JSON values
            jsonMessage.Value = pyon.Replace("[" & vbLf, [String].Empty).Replace("]" & vbLf, [String].Empty).Replace(": None", ": null")
            ' set the index so we know where to trim the string
            jsonMessage.NextStartIndex = endIndex + 4
            Return jsonMessage
        End Function
#Region "IDisposable Members"

        Private _disposed As Boolean

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Private Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    _tcpClient.Close()
                End If
            End If

            _disposed = True
        End Sub

        Protected Overrides Sub Finalize()
            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try
        End Sub
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
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

#End Region


    End Class
    Public Class JsonMessage
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = value
            End Set
        End Property
        Private m_Name As String

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Set(value As String)
                m_Value = value
            End Set
        End Property
        Private m_Value As String

        Public Property NextStartIndex() As Integer
            Get
                Return m_NextStartIndex
            End Get
            Set(value As Integer)
                m_NextStartIndex = value
            End Set
        End Property
        Private m_NextStartIndex As Integer
    End Class
End Namespace