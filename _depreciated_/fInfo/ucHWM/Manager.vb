'/*

'  ucHWM Manager class 
'  Version: MPL 1.1/GPL 2.0/LGPL 2.1

'  The contents of this file are subject to the Mozilla Public License Version
'  1.1 (the "License"); you may not use this file except in compliance with
'  the License. You may obtain a copy of the License at

'  http://www.mozilla.org/MPL/

'  Software distributed under the License is distributed on an "AS IS" basis,
'  WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
'  for the specific language governing rights and limitations under the License.

'  The Original Code is the cftUnity.nl source code.

'  The Initial Developer of the Original Code is 
'   Marvin Westmaas/ Marvin_The_Martian / MtM ( webmaster@cftunity.nl )
'  Portions created by the Initial Developer are Copyright (C) 2010-2011
'  the Initial Developer. All Rights Reserved.

'  The repository for cftUnity.nl is hosted at
'  http://code.google.com/p/cftunity/

'  Contributor(s):

'  Alternatively, the contents of this file may be used under the terms of
'  either the GNU General Public License Version 2 or later (the "GPL"), or
'  the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
'  in which case the provisions of the GPL or the LGPL are applicable instead
'  of those above. If you wish to allow use of your version of this file only
'  under the terms of either the GPL or the LGPL, and not to allow others to
'  use your version of this file under the terms of the MPL, indicate your
'  decision by deleting the provisions above and replace them with the notice
'  and other provisions required by the GPL or the LGPL. If you do not delete
'  the provisions above, a recipient may use your version of this file under
'  the terms of any one of the MPL, the GPL or the LGPL.

'*/
Imports OpenHardwareMonitor.Hardware
Imports OpenHardwareMonitor
Imports HWInfo


Public Class Manager
    Private Class HWManager
        Private HW As IHardware
        Private WithEvents ucHM As ucHWM
        Private WithEvents FloatForm As frmFloat
        Private WithEvents mySettings As clsSettings
        Private lFloat As New List(Of frmFloat)
        Public Function ShowFloat(ByVal Identifier As String) As Boolean
            Try
                If Not IsNothing(FloatForm) Then
                    FloatForm.BringToFront()
                    Return False
                End If
                FloatForm = New frmFloat
                AddHandler FloatForm.FormClosing, AddressOf CloseHandler
                FloatForm.Controls.Add(ui(FloatForm))
                FloatForm.Show()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
        Public ReadOnly Property HWName As String
            Get
                Return HW.Name
            End Get
        End Property
        Public ReadOnly Property Identifier As String
            Get
                Return HW.Identifier.ToString
            End Get
        End Property
        Public Function CloseFloat() As Boolean
            Try
                If IsNothing(FloatForm) Then Return True
                FloatForm.Close()
                FloatForm = Nothing
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return False
            End Try
        End Function
        Private Structure s_ucHW
            Public ParentForm As Form
            Public ucHWM As ucHWM
        End Structure
        Private lucHM As New List(Of s_ucHW)
        Private Property myManager As Manager
        Public ReadOnly Property ui(ByVal ParentForm As Form, Optional ByVal AutoUpdate As Boolean = True, Optional ByVal UpdateInterval As Int32 = 2000) As ucHWM
            Get
                Try
                    For Each s_ucHW As s_ucHW In lucHM
                        If ReferenceEquals(ParentForm, s_ucHW.ParentForm) Then
                            Return s_ucHW.ucHWM
                        End If
                    Next
                    Dim ns_UC As New s_ucHW, nHWM As New ucHWM
                    AddHandler nHWM.Log, AddressOf LogWindow_Log
                    AddHandler nHWM.LogError, AddressOf LogWindow_LogError
                    AddHandler nHWM.ParentClosed, AddressOf CloseHandler
                    nHWM.Init(mySettings.File, False, mySettings.MySettings, HW, myManager)
                    ns_UC.ucHWM = nHWM
                    ns_UC.ParentForm = ParentForm
                    lucHM.Add(ns_UC)
                    If AutoUpdate Then nHWM.AutoUpdate(UpdateInterval)
                    Return nHWM
                Catch ex As Exception

                    Return Nothing
                End Try
            End Get
        End Property
        Private Sub CloseHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs)
            Try
                Dim thesucm As s_ucHW = Nothing
                For Each sucm As s_ucHW In lucHM
                    If ReferenceEquals(sucm.ParentForm, sender) Then
                        thesucm = sucm
                        Exit For
                    End If
                Next
                If Not IsNothing(thesucm) Then lucHM.Remove(thesucm)
            Catch ex As Exception

            End Try
        End Sub
        Public Overloads Function Init(ByVal Location As String, ByVal SetDefaults As Boolean, ByVal TheHardware As IHardware, Optional ByVal TheucHWM As ucHWM = Nothing, Optional ByVal TheSettings As clsSettings.sSettings = Nothing, Optional ByVal formFloat As frmFloat = Nothing, Optional ByVal RefreshInterval As Int32 = Nothing, Optional ByVal TheManager As Manager = Nothing) As Boolean
            Try
                If Location.Contains("\config.dat") Then
                    Location = Location.Replace("\config.dat", "")
                End If
                If IsNothing(TheSettings) Then
                    mySettings.SetDefaults()
                Else
                    mySettings.MySettings = TheSettings
                End If

                AddHandler mySettings.Log, AddressOf LogWindow_Log
                AddHandler mySettings.LogError, AddressOf LogWindow_LogError
                
                HW = TheHardware
                If IsNothing(TheucHWM) Then
                    ' new
                    ucHM = New ucHWM
                    AddHandler ucHM.LogError, AddressOf LogWindow_LogError
                    AddHandler ucHM.Log, AddressOf LogWindow_Log
                    ucHM.Init(mySettings.File, False, mySettings.MySettings, HW)
                    'ucHM.AttachHW(HW)
                Else
                    ucHM = TheucHWM
                End If
                If Not IsNothing(RefreshInterval) Then ucHM.AutoUpdate(RefreshInterval)
                If Not IsNothing(TheManager) Then myManager = TheManager
                If Not IsNothing(formFloat) Then FloatForm = formFloat
                Return True
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
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


    End Class
    Private lHW As New List(Of HWManager)
    Sub New()
        ' TODO: Complete member initialization 

    End Sub
    Public ReadOnly Property Count
        Get
            Return lHW.Count
        End Get
    End Property
    Public ReadOnly Property HWName(ByVal Index As Int32) As String
        Get
            Return lHW(Index).HWName
        End Get
    End Property
    Public ReadOnly Property userControl(ByVal Identifier As String, Optional ByVal ParentForm As Form = Nothing) As ucHWM
        Get
            Try
                For Each hwm As HWManager In lHW
                    If Not IsNothing(hwm.Identifier) And hwm.Identifier.ToString = Identifier Then
                        Return hwm.ui(ParentForm)
                    End If
                Next
                Return Nothing
            Catch ex As Exception
                LogWindow.WriteError(ex.Message, Err)
                Return Nothing
            End Try
        End Get
    End Property
    Public ReadOnly Property Identifier(ByVal Index As Int32) As String
        Get
            Return lHW(Index).Identifier.ToString
        End Get
    End Property
    Public Sub New(ByVal hwInf As clsHWInfo.cHWInfo, ByVal Settings As clsSettings)
        Try
            For xInt As Short = 0 To hwInf.ohmInterface.CpuCount - 1
                Dim nM As New HWManager()
                AddHandler nM.Log, AddressOf LogWindow_Log
                AddHandler nM.LogError, AddressOf LogWindow_LogError
                nM.Init(Settings.File, False, hwInf.ohmInterface.CPU(xInt), Nothing, Settings.MySettings, Nothing, Nothing, Me)
                lHW.Add(nM)
            Next
            For xInt As Int32 = 0 To hwInf.ohmInterface.NVIDIACount - 1
                Dim nM As New HWManager()
                AddHandler nM.Log, AddressOf LogWindow_Log
                AddHandler nM.LogError, AddressOf LogWindow_LogError
                nM.Init(Settings.File, False, hwInf.ohmInterface.NvidiaGpu(xInt), Nothing, Settings.MySettings, Nothing, Nothing, Me)
                lHW.Add(nM)
            Next
            For xInt As Int32 = 0 To hwInf.ohmInterface.ATICount - 1
                Dim nM As New HWManager()
                AddHandler nM.Log, AddressOf LogWindow_Log
                AddHandler nM.LogError, AddressOf LogWindow_LogError
                nM.Init(Settings.File, False, hwInf.ohmInterface.AtiGpu(xInt), Nothing, Settings.MySettings, Nothing, Nothing, Me)
                lHW.Add(nM)
            Next
        Catch ex As Exception

        End Try
    End Sub
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
End Class
