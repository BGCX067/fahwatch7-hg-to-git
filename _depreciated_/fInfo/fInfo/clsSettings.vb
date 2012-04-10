'/*
' * fInfo Settings class Copyright Marvin Westmaas ( mtm ) 
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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Namespace fInfo
    Public Class clsSettings
        <Serializable()>
        Public Class sSettings
            Implements ISerializable
            Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
                With info
                    .AddValue("HistoryLimit", HistoryLimit)
                    .AddValue("StartWithWindows", StartWithWindows)
                    .AddValue("AutoStartClient", AutoStartClient)
                    .AddValue("LastEOCUpdate", LastEOCUpdate)
                    .AddValue("NonFatal", NonFatal)
                    .AddValue("LastSummaryUpdate", LastSummaryUpdate)
                    .AddValue("URLsummary", URLsummary)
                    .AddValue("UseEOC", UseEOC)
                    .AddValue("EOCNotify", EOCNotify)
                    .AddValue("StartMinimized", StartMinimized)
                    .AddValue("lSettings", lSettings)
                    .AddValue("mHWSettings", mHWsettings)
                    .AddValue("Pwd", Pwd)
                    .AddValue("Port", Port)
                    .AddValue("lsPCI", lsPCI)
                    .AddValue("Automation", Automation)
                    .AddValue("intOHM", intOHM)
                    .AddValue("intSQ", intSQ)
                    .AddValue("intOPT", intOPT)
                    .AddValue("ConfirmExit", ConfirmExit)
                    .AddValue("EOCConfirmDelete", EOCConfirmDelete)
                    .AddValue("EOCIcon", EOCIcon)
                End With
            End Sub
            Public Enum eStartMethod
                Registry = 0
                RegistryMinimized = 1
            End Enum
            Public Enum eHistoryLimit
                None = 0
                Minimal = 1
                OneDay = 2
                OneWeek = 3
                OneMonth = 4
            End Enum
            Public HistoryLimit As eHistoryLimit = eHistoryLimit.None
            Public StartWithWindows As Boolean = True
            Public AutoStartClient As Boolean = True
            Public LastEOCUpdate As DateTime = DateTime.MinValue
            Public LastSummaryUpdate As DateTime = DateTime.MinValue
            Public NonFatal As New List(Of String)
            Public TrackEUE As Boolean = True
            Public URLsummary As String = ""
            Public UseEOC As Boolean = True
            Public EOCIcon As Boolean = True
            Public EOCNotify As Boolean = True
            Public StartMinimized As Boolean = True
            Public Pwd As String = ""
            Public Port As String = ""
            Public lsPCI As Boolean = True
            Public Automation As Boolean = True
            Public intOHM As Double = 1000
            Public intSQ As Double = 5000
            Public intOPT As Double = 5000
            Public ConfirmExit As Boolean = True
            Public EOCConfirmDelete As Boolean = True
            <Serializable()>
            Public Class sHWsettings
                Implements ISerializable
                Public Sub New()

                End Sub
                Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
                    Try
                        If IsNothing(info) Then
                            Throw New System.ArgumentNullException("info")
                        End If
                        Identifier = info.GetString("Identifier")
                        ClocksAsGauges = info.GetBoolean("ClocksAsGauges")
                        HideClocks = info.GetBoolean("HideClocks")
                    Catch ex As Exception
                    End Try
                End Sub

                Public Identifier As String
                Public ClocksAsGauges As Boolean
                Public HideClocks As Boolean
                Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
                    info.AddValue("Identifier", Identifier)
                    info.AddValue("ClocksAsGauges", ClocksAsGauges)
                    info.AddValue("HideClocks", HideClocks)
                End Sub
            End Class
            Private mHWsettings As New List(Of sHWsettings)
            Public ReadOnly Property HasHWSettings(ByVal Identifier As String) As Boolean
                Get
                    Try
                        For Each gs As sHWsettings In mHWsettings
                            If gs.Identifier = Identifier Then Return True
                        Next
                        Return False
                    Catch ex As Exception
                        Return False
                    End Try
                End Get
            End Property
            Public ReadOnly Property HWSettings(ByVal Identifier As String) As sHWsettings
                Get
                    Try
                        For Each hwSetting As sHWsettings In mHWsettings
                            If hwSetting.Identifier = Identifier Then Return hwSetting
                        Next
                        Return Nothing
                    Catch ex As Exception
                        LogWindow.WriteError("clsSettings_GaugeSettings", Err)
                        Return Nothing
                    End Try
                End Get
            End Property
            Public Function SaveHWSettings(ByVal Identifier As String, ByVal Settings As clsSettings.sSettings.sHWsettings) As Boolean
                Try
                    For Each Setting As sHWsettings In mHWsettings
                        If Setting.Identifier = Identifier Then
                            Setting = Settings
                            Return True
                        End If
                    Next
                    mHWsettings.Add(Settings)
                    Return mHWsettings.Contains(Settings)
                Catch ex As Exception
                    LogWindow.WriteError("clsSettings_SaveSettings", Err)
                    Return False
                End Try
            End Function
            <Serializable()>
            Public Class sGaugeSettings
                Implements ISerializable
                Public Sub New()

                End Sub
                Protected Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
                    Try
                        If Not IsNothing(info) Then
                            Identifier = info.GetString("Identifier")
                            backColor = Color.FromName(info.GetString("backColor"))
                            valueColor = Color.FromName(info.GetString("valueColor"))
                            rangeColor = Color.FromName(info.GetString("rangeColor"))
                            minmaxColor = Color.FromName(info.GetString("minmaxColor"))
                            warningColor = Color.FromName(info.GetString("warningColor"))
                            warningStart = info.GetInt32("warningStart")
                            normalMIN = info.GetInt32("normalMIN")
                            valueMAX = info.GetInt32("valueMAX")
                            valueMIN = info.GetInt32("valueMIN")
                            stepSmall = info.GetInt32("stepSmall")
                            stepLarge = info.GetInt32("stepLarge")
                            vSize = info.GetInt32("vSize")
                            Try
                                AlternateImage = info.GetString("AlternateImage")
                            Catch ex As Exception

                            End Try
                            Try
                                Hide = info.GetBoolean("Hide")
                            Catch ex As Exception

                            End Try
                            Try
                                iBorder = info.GetInt32("border")
                            Catch ex As Exception

                            End Try
                            Try
                                borderColor = Color.FromName(info.GetString("borderColor"))
                            Catch ex As Exception

                            End Try
                            Try
                                iCorner = info.GetInt32("iCorner")
                            Catch ex As Exception

                            End Try
                        End If
                    Catch ex As Exception

                    End Try
                End Sub
                Public Hide As Boolean
                Public Identifier As String
                Public backColor As Color = Color.Yellow
                Public valueColor As Color
                Public rangeColor As Color
                Public minmaxColor As Color
                Public warningColor As Color
                Public valueMAX As Integer
                Public valueMIN As Integer
                Public warningStart As Integer
                Public normalMIN As Integer
                Public stepSmall As Integer
                Public stepLarge As Integer
                Public vSize As Integer = 3
                Public iCorner As Int32 = 10
                Public iBorder As Int32 = 0
                Public borderColor As Color = Color.Black
                Public AlternateImage As String
                Public wSound As String
                Public wIcon As Boolean
                Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
                    Try
                        With info
                            .AddValue("Hide", Hide)
                            .AddValue("iCorner", iCorner)
                            .AddValue("border", iBorder)
                            .AddValue("borderColor", borderColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("Identifier", Identifier.ToString)
                            .AddValue("backColor", backColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("rangeColor", rangeColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("minmaxColor", minmaxColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("valueColor", valueColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("warningColor", warningColor.ToString.Replace("Color ", "").Replace("[", "").Replace("]", ""))
                            .AddValue("valueMAX", valueMAX)
                            .AddValue("valueMIN", valueMIN)
                            .AddValue("normalMIN", normalMIN)
                            .AddValue("warningStart", warningStart)
                            .AddValue("stepSmall", stepSmall)
                            .AddValue("stepLarge", stepLarge)
                            .AddValue("vSize", vSize)
                            If Not IsNothing(AlternateImage) Then .AddValue("AlternateImage", AlternateImage)
                        End With
                    Catch ex As Exception

                    End Try

                End Sub
            End Class
            Private lSettings As New List(Of sGaugeSettings)
            Private lCSounds As New List(Of String)
            Public ReadOnly Property HasGaugeSettings(ByVal Identifier As String) As Boolean
                Get
                    Try
                        For Each gs As sGaugeSettings In lSettings
                            If gs.Identifier = Identifier Then Return True
                        Next
                        Return False
                    Catch ex As Exception
                        Return False
                    End Try
                End Get
            End Property
            Public ReadOnly Property GaugeSettings(ByVal Identifier As String) As sGaugeSettings
                Get
                    Try
                        For xInt As Int16 = 0 To lSettings.Count - 1
                            If lSettings(xInt).Identifier = Identifier Then
                                Return lSettings(xInt)
                            End If
                        Next
                        Return Nothing
                    Catch ex As Exception
                        LogWindow.WriteError("clsSettings_GaugeSettings", Err)
                        Return Nothing
                    End Try
                End Get
            End Property
            Public Function SaveGaugeSettings(ByVal Identifier As String, ByVal GSettings As clsSettings.sSettings.sGaugeSettings) As Boolean
                Try
                    Dim iIndex As Int32 = 0
                    For xInt As Int32 = 0 To lSettings.Count - 1
                        If lSettings(xInt).Identifier = Identifier Then
                            lSettings.RemoveAt(xInt)
                            lSettings.Insert(xInt, GSettings)
                            Return lSettings.Contains(GSettings)
                        End If
                    Next
                    lSettings.Add(GSettings)
                    Return True
                Catch ex As Exception
                    LogWindow.WriteError("clsSettings_SaveSettings", Err)
                    Return False
                End Try
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
            Public WithEvents LogWindow As New cLW
            Public Event Log(ByVal Message As String)
            Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
            Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
                RaiseEvent Log(Message)
            End Sub
            Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
                RaiseEvent LogError(Message, EObj)
            End Sub
#End Region
            Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
                Try
                    If IsNothing(info) Then
                        Throw New System.ArgumentNullException("info")
                    End If
                    HistoryLimit = info.GetInt32("HistoryLimit")
                    StartWithWindows = info.GetBoolean("StartWithWindows")
                    AutoStartClient = info.GetBoolean("AutoStartClient")
                    LastEOCUpdate = info.GetDateTime("LastEOCUpdate")
                    lSettings = info.GetValue("lSettings", lSettings.GetType)
                    mHWsettings = info.GetValue("mHWSettings", mHWsettings.GetType)
                    NonFatal = info.GetValue("NonFatal", NonFatal.GetType)
                    LastSummaryUpdate = info.GetDateTime("LastSummaryUpdate")
                    URLsummary = info.GetString("URLsummary")
                    UseEOC = info.GetBoolean("UseEOC")
                    EOCIcon = info.GetBoolean("EOCIcon")
                    EOCNotify = info.GetBoolean("EOCNotify")
                    StartMinimized = info.GetBoolean("StartMinimized")
                    Pwd = info.GetString("Pwd")
                    Port = info.GetString("Port")
                    lsPCI = info.GetBoolean("lsPCI")
                    Automation = info.GetBoolean("Automation")
                    intOHM = info.GetDouble("intOHM")
                    intSQ = info.GetDouble("intSQ")
                    intOPT = info.GetDouble("intOPT")
                    ConfirmExit = info.GetBoolean("ConfirmExit")
                    EOCConfirmDelete = info.GetBoolean("EOCConfirmDelete")
                Catch ex As Exception

                End Try
            End Sub
            Public Sub New()

            End Sub
        End Class
        Private WithEvents _Settings As New sSettings
        Public Property MySettings As sSettings = _Settings
        Private fileSettings As String = ""
        Private _Empty As Boolean
        Public ReadOnly Property File As String
            Get
                Return fileSettings
            End Get
        End Property
        Public ReadOnly Property IsFile As Boolean
            Get
                Return fileSettings <> ""
            End Get
        End Property
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
                fileSettings = Datalocation & "\" & My.Application.Info.AssemblyName & "_config.dat"
                If My.Computer.FileSystem.FileExists(fileSettings) Then
                    ReadSettings()
                Else
                    _Empty = True
                End If
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
        End Sub

        Public Function SaveSettings() As Boolean
            Try
                Dim Serializer As New BinaryFormatter
                Dim DataFile As New FileStream(fileSettings, FileMode.Create, FileAccess.Write, FileShare.Inheritable)
                Serializer.Serialize(DataFile, MySettings)
                DataFile.Close()
                DataFile = Nothing
                Serializer = Nothing
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Public Function ReadSettings() As Boolean
            Try
                Dim Deserializer As New BinaryFormatter
                Dim DataFile As New FileStream(fileSettings, FileMode.Open, FileAccess.Read, FileShare.Inheritable)
                MySettings = CType(Deserializer.Deserialize(DataFile), sSettings)
                DataFile.Close()
                Deserializer = Nothing
                DataFile = Nothing
                _Empty = False
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                _Empty = True
                Return False
            End Try

        End Function
        Public Sub SetDefaults()
            Try
                With _Settings
                    .URLsummary = "http://fah-web.stanford.edu/psummaryC.html"
                    .StartWithWindows = True
                    .LastSummaryUpdate = DateTime.MinValue
                    .LastEOCUpdate = DateTime.MinValue
                    .HistoryLimit = sSettings.eHistoryLimit.None
                    .AutoStartClient = True
                    With .NonFatal
                        .Add("6E")
                        .Add("62")
                        .Add("64")
                    End With
                    .UseEOC = True
                    .EOCIcon = True
                    .EOCNotify = True
                    .StartMinimized = False
                    .intOHM = 1000
                    .intOPT = 5000
                    .intSQ = 5000
                    .lsPCI = True
                    .TrackEUE = True
                    .Automation = True
                    .ConfirmExit = True
                End With
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
            End Try
        End Sub
      
        Private _Form As New frmSettings
        Public ReadOnly Property IsFormActive As Boolean
            Get
                Return _Form.Visible
            End Get
        End Property
        Public Sub ShowSettings()
            _Form.Show()
        End Sub
        Public ReadOnly Property InitialForm As Form
            Get
                _Form.cmdCancel.Visible = False
                _Form.Show()
                Return _Form
            End Get
        End Property
        Public ReadOnly Property form As Form
            Get
                _Form.Show()
                Return _Form
            End Get
        End Property
        Public Sub HideSettings()
            _Form.WindowState = FormWindowState.Minimized
            _Form.Visible = False
        End Sub
        Public Sub CloseSettings()
            _Form.CloseForm()
        End Sub

        Public Sub New()
            With _Form
                .chkPCI.Checked = MySettings.lsPCI
                .chkAUTORUN.Checked = MySettings.StartWithWindows
                .chkEUE.Checked = MySettings.TrackEUE
                For Each NonFatal As String In MySettings.NonFatal
                    .lbEUE.Items.Add(NonFatal)
                Next
                If CDec(MySettings.intOPT) < .nudO.Minimum Then
                    .nudO.Value = .nudO.Minimum
                Else
                    If CDec(MySettings.intOPT) > .nudO.Maximum Then
                        .nudO.Value = .nudO.Maximum
                    Else
                        .nudO.Value = CDec(MySettings.intOPT)
                    End If
                End If
                If CDec(MySettings.intOHM) < .nudOHM.Minimum Then
                    .nudOHM.Value = .nudOHM.Minimum
                Else
                    If CDec(MySettings.intOHM) > .nudOHM.Maximum Then
                        .nudOHM.Value = .nudOHM.Maximum
                    Else
                        .nudOHM.Value = CDec(MySettings.intOHM)
                    End If
                End If
                If CDec(MySettings.intSQ) < .nudSQ.Minimum Then
                    .nudSQ.Value = .nudSQ.Minimum
                Else
                    If CDec(MySettings.intSQ) > .nudSQ.Maximum Then
                        .nudSQ.Value = .nudSQ.Maximum
                    Else
                        .nudSQ.Value = CDec(MySettings.intSQ)
                    End If
                End If

                .chkConfirmExit.Checked = MySettings.ConfirmExit
                .chkStartClient.Checked = MySettings.AutoStartClient
            End With

        End Sub
#Region "Log extender"
        Private Sub DoLog(ByVal Message As String) Handles _Settings.Log
            LogWindow_Log(Message)
        End Sub
        Private Sub DoError(ByVal Message As String, ByVal EObj As ErrObject) Handles _Settings.LogError
            LogWindow_LogError(Message, EObj)
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
        Public WithEvents LogWindow As New cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
            RaiseEvent Log(Message)
        End Sub
        Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
            RaiseEvent LogError(Message, EObj)
        End Sub
#End Region
    End Class
End Namespace

