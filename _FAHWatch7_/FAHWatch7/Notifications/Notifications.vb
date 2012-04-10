'/*
' * FAHWatch7 Notifications handler 
' *
' *  Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Imports System.Globalization
Public Class Notifications
    Friend Structure Notification
        Friend Property Source As String
        Friend Property TimeStamp As String
        Friend Property Reason As String
        Friend Property Info As String
        Friend ReadOnly Property ID As String
            Get
                Return Source & "#" & TimeStamp & "#" & Reason
            End Get
        End Property
        Sub New(Source As String, TimeStamp As String, Reason As String, Info As String)
            Me.Source = Source
            Me.TimeStamp = TimeStamp
            Me.Reason = Reason
            Me.Info = Info
        End Sub
    End Structure
    Private dActiveNotifications As New Dictionary(Of String, Notification)
    Private Shared nRules As List(Of modMySettings.clsNFilter)
    Friend Shared Sub Init()
        nRules = modMySettings.nFilters
    End Sub
    Friend Shared Sub CheckNotifyEUE(EUE As clsWU)
        Try
            If modMySettings.DisableNotifications Then Exit Sub
            'update filters 
            Init()
            Dim lNotifications As New List(Of Notification)
            If modMySettings.clsNotifyOptions.EUE_always Then
                'notify
                lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of EUE", EUE.ActiveLogfile))
            ElseIf modMySettings.clsNotifyOptions.EUE_by_Rate Then
                'check current eue
                If clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRateDBL <= modMySettings.clsNotifyOptions.NotifyRATE Then
                    'notify 
                    lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(modMySettings.clsNotifyOptions.NotifyRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRate, EUE.ActiveLogfile))
                End If
            ElseIf modMySettings.Notify_UseRules Then
                'Check client rule
                For Each Rule As modMySettings.clsNFilter In nRules
                    If Rule.fEnabled Then
                        If Rule.ClientName = EUE.ClientName Then
                            If Rule.ucFilter.rbEUE_Always.Checked Then
                                'notify
                                lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of EUE", EUE.ActiveLogfile))
                                Exit For
                            ElseIf Rule.ucFilter.rbEUE_ratio.Checked Then
                                If Rule.iRATE <= clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).SuccesRateDBL Then
                                    'notify
                                    lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(Rule.iRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).SuccesRate, EUE.ActiveLogfile))
                                    Exit For
                                End If
                            End If
                        ElseIf Rule.ClientName = EUE.ClientName AndAlso Rule.SlotID = EUE.Slot Then
                            If Rule.ucFilter.rbEUE_Always.Checked Then
                                'notify
                                lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of EUE", EUE.ActiveLogfile))
                                Exit For
                            ElseIf Rule.ucFilter.rbEUE_ratio.Checked Then
                                If Rule.iRATE <= clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).Slots(EUE.Slot).SuccesRateDBL Then
                                    'notify
                                    lNotifications.Add(New Notification(EUE.ClientName & ":" & strID(EUE), DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(Rule.iRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).Slots(EUE.Slot).SuccesRate, EUE.ActiveLogfile))
                                    Exit For
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            If lNotifications.Count > 0 Then
                For Each Notification In lNotifications
                    If modMySettings.NotifyOption = modMySettings.clsNotifyOptions.eNotifyOption.PopUpForm Then
                        Dim dNotify As New Notify
                        AddHandler dNotify.FormClosed, AddressOf HandleClose
                        dNotify.Notification = Notification
                        dNotify.ActivateNotification()
                    Else
                        Dim dTray As New TrayNotification
                        AddHandler dTray.NotificationClosed, AddressOf HandleClose
                        dTray.Notification = Notification
                        dTray.ActivateTrayNotification()
                    End If
                Next
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub CheckNotifySlotFailure(Client As Client, SlotID As String)
        Try
            If modMySettings.DisableNotifications Then Exit Sub
            'update filters 
            Init()
            Dim lNotifications As New List(Of Notification)
            If modMySettings.Notify_Failure Then
                lNotifications.Add(New Notification(Client.ClientName & ":SLOT FAILURE", DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of Failure", ""))
            ElseIf modMySettings.Notify_UseRules Then
                'Check client rule
                For Each Rule As modMySettings.clsNFilter In nRules
                    If Rule.fEnabled Then
                        If Rule.ClientName = Client.ClientName And Rule.SlotID = "-1" Then
                            If Rule.ucFilter.rbSlotFail.Checked Then
                                'notify
                                lNotifications.Add(New Notification(Client.ClientName & ":" & SlotID & ":SLOT FAILURE", DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of Failure", ""))
                                Exit For
                            End If
                        ElseIf Rule.ClientName = Client.ClientName AndAlso Rule.SlotID = SlotID Then
                            If Rule.ucFilter.rbSlotFail.Checked Then
                                'notify
                                lNotifications.Add(New Notification(Client.ClientName & ":" & SlotID & ":SLOT FAILURE", DateTime.Now.ToString(CultureInfo.CurrentCulture), "Always notify of Failure", ""))
                                Exit For
                            End If
                        End If
                    End If
                Next
            End If
            If lNotifications.Count > 0 Then

            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Friend Shared Sub CheckNotifyStatistics()
        Try
            If modMySettings.DisableNotifications Then Exit Sub
            'update filters
            Init()
            Dim lNotifications As New List(Of Notification)
            If modMySettings.clsNotifyOptions.EUE_by_Rate Then
                'check current eue
                If clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRateDBL <= modMySettings.clsNotifyOptions.NotifyRATE Then
                    'notify 
                    lNotifications.Add(New Notification("Overall performance statistics", DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(modMySettings.clsNotifyOptions.NotifyRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.SuccesRate, clsStatistics.clsPerformanceStatistics.CurrentStatistics.Report))
                End If
            ElseIf modMySettings.Notify_UseRules Then
                For Each Rule As modMySettings.clsNFilter In nRules
                    If Rule.fEnabled Then
                        If Rule.SlotID = "-1" Then
                            'client wide rule
                            If clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).SuccesRateDBL <= Rule.iRATE Then
                                lNotifications.Add(New Notification(Rule.ClientName & " performance statistics", DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(Rule.iRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).SuccesRate, clsStatistics.clsPerformanceStatistics.CurrentStatistics.Report(Rule.ClientName)))
                                'Shouldn't exit sub, what if multiple notifications should be given?
                                'Exit Sub
                            End If
                        Else
                            If clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).Slots(Rule.SlotID).SuccesRateDBL <= Rule.iRATE Then
                                lNotifications.Add(New Notification(Rule.ClientName & ":" & Rule.SlotID & " performance statistics", DateTime.Now.ToString(CultureInfo.CurrentCulture), "EUE by rate, limit: " & CStr(Rule.iRATE) & " Current: " & clsStatistics.clsPerformanceStatistics.CurrentStatistics.Clients(Rule.ClientName).Slots(Rule.SlotID).SuccesRate, clsStatistics.clsPerformanceStatistics.CurrentStatistics.Report(Rule.ClientName, Rule.SlotID)))
                            End If
                        End If
                    End If
                Next
            End If
            If lNotifications.Count > 0 Then

            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
    Private Shared Sub HandleClose(sender As Object, e As EventArgs)
        Try
            If TypeOf sender Is Form Then
                CType(sender, Form).Dispose()
            ElseIf TypeOf sender Is TrayNotification Then
                CType(sender, TrayNotification).Dispose()
            End If
        Catch ex As Exception
            WriteError(ex.Message, Err)
        End Try
    End Sub
End Class
