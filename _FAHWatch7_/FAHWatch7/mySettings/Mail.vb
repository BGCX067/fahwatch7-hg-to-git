'   FAHWatch7 MAIL 
'
'   Copyright (c) 2011-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
'
'   This class uses one import from ExceptionSender.dll to facilitate sending exception
'   email to me, due to potential (certain) abuse I don't feel secure providing this file
'   in plain source including the smtp details of the account used. Therefore this dll is pre
'   compiled and obfuscated. To satisfy the need for open source disclosure 
'   as well as showing no other information then the exception itself, the obfuscated source 
'   function which is imported is shown below. Anyone is free to check 
'   this with tools suchs as .NET Reflector ( which has a 30 day free trail ).
'

'''Public Shared Function SendException(ByVal Message As String) As Boolean
'''    Dim flag As Boolean
'''    Try
'''        Dim flag2 As Boolean = False
'''        Dim client As New SmtpClient
'''        Dim message As New MailMessage
'''        client.Credentials = New NetworkCredential(.(-1390612948), .(-1390612790))
'''        client.Port = &H24B
'''        client.Host = .(-1390612774)
'''        client.EnableSsl = True
'''        message = New MailMessage With { _
'''            .From = New MailAddress(.(-1390612948)) _
'''        }
'''        message.To.Add(.(-1390612746))
'''        message.Subject = .(-1390612854)
'''        message.Body = Message
'''        Try
'''            client.Send(message)
'''            flag2 = True
'''        Catch exception1 As SmtpException
'''            ProjectData.SetProjectError(exception1)
'''            Dim exception As SmtpException = exception1
'''            SMTP. = exception
'''            ProjectData.ClearProjectError()
'''        End Try
'''        flag = flag2
'''    Catch exception3 As Exception
'''        ProjectData.SetProjectError(exception3)
'''        Dim exception2 As Exception = exception3
'''        SMTP. = New SmtpException(String.Concat(New String() { .(-1390612823), exception2.Message, Environment.NewLine, .(-1390612665), exception2.StackTrace }))
'''        flag = False
'''        ProjectData.ClearProjectError()
'''    End Try
'''    Return flag
'''End Function

Imports System.Net.Mail
Imports FAHWatch7.Exceptions
Imports ExceptionSender
Public Class Mail
    Inherits Dictionary(Of String, EmailProvider)
    Public Structure EmailProvider
#Region "Declarations"
        Friend Property SMTP As String
        Friend Property Port As Integer
        Friend Property UseSSL As Boolean
        Private mAccountName As String
        Private mAccountPassword As String
        Private mSmtpException As SmtpException
        Friend ReadOnly Property SMTPStatus As String
            Get
                If Not IsNothing(mSmtpException) Then
                    Return mSmtpException.StatusCode.ToString
                Else
                    Return String.Empty
                End If
            End Get
        End Property
        Friend ReadOnly Property AccountName As String
            Get
                Return mAccountName
            End Get
        End Property
        Friend ReadOnly Property AccountPassword As String
            Get
                Return mAccountPassword
            End Get
        End Property
        Private Property Recipients As Dictionary(Of String, EmailSchedule)
#End Region
        Sub New(Server As String, Port As Integer, UseSSL As Boolean, Optional Username As String = "", Optional Password As String = "")
            Me.SMTP = Server
            Me.Port = Port
            Me.UseSSL = UseSSL
            If Not Username = "" Then Me.mAccountName = Username
            If Not Password = "" Then Me.mAccountPassword = Password
        End Sub
        Friend Sub TestVerification()
            Try
                SendMail("FAHWatch7 mail verification", "This is a test")
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Friend Sub SendAllert(Message As String)
            Try
                For Each Reciepent In Recipients
                    If Reciepent.Value.Always Then
                        GoTo SendAllert
                    ElseIf Reciepent.Value.Weekdays AndAlso DateTime.Now.DayOfWeek <> DayOfWeek.Saturday AndAlso DateTime.Now.DayOfWeek <> DayOfWeek.Sunday Then
                        GoTo SendAllert
                    ElseIf Reciepent.Value.Weekends AndAlso (DateTime.Now.DayOfWeek = DayOfWeek.Saturday Or DateTime.Now.DayOfWeek = DayOfWeek.Sunday) Then
                        GoTo SendAllert
                    ElseIf Reciepent.Value.Between AndAlso Reciepent.Value.FallsInBetween(DateTime.Now) Then
                        GoTo SendAllert
                    Else
                        WriteLog("Allert message falls out of all scheduled options, not sending email to " & AccountName)
                        GoTo Skip
                    End If
SendAllert:
                    SendMail("FAHWatch7 alert", Message, Reciepent.Key)
Skip:
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Overloads Sub SendMail(Subject As String, Message As String, Destinations As List(Of String))
            Try
                For Each Destination As String In Destinations
                    Dim SmtpServer As New SmtpClient()
                    Dim mail As New MailMessage()
                    SmtpServer.Credentials = New Net.NetworkCredential(AccountName, AccountPassword)
                    SmtpServer.Port = Port
                    SmtpServer.Host = SMTP
                    mail = New MailMessage()
                    mail.From = New MailAddress(AccountName)
                    mail.To.Add(Destination)
                    mail.Subject = Subject
                    mail.Body = Message
                    Try
                        SmtpServer.Send(mail)
                    Catch exSmtp As SmtpException
                        mSmtpException = exSmtp
                        If Not IsNothing(exSmtp.StatusCode) Then
                            Select Case exSmtp.StatusCode

                                Case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery

                                Case SmtpStatusCode.ClientNotPermitted

                                Case SmtpStatusCode.GeneralFailure

                                Case SmtpStatusCode.Ok



                            End Select
                        End If
                    End Try
                Next
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
        Private Overloads Sub SendMail(Subject As String, Message As String, Optional Destination As String = Nothing, Optional From As String = Nothing)
            Try
                Dim SmtpServer As New SmtpClient()
                Dim mail As New MailMessage()
                SmtpServer.Credentials = New Net.NetworkCredential(AccountName, AccountPassword)
                SmtpServer.Port = Port
                SmtpServer.Host = SMTP
                mail = New MailMessage()
                mail.From = New MailAddress(AccountName)
                mail.To.Add(AccountName)
                mail.Subject = Subject
                mail.Body = Message
                Try
                    SmtpServer.Send(mail)
                Catch exSmtp As SmtpException
                    mSmtpException = exSmtp
                End Try
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Sub
    End Structure
    Public Structure EmailSchedule
        Property Always As Boolean
        Property Weekdays As Boolean
        Property Weekends As Boolean
        Property Between As Boolean
        Property After As DateTime
        Property Before As DateTime
        Public ReadOnly Property FallsInBetween(EventDateTime As DateTime) As Boolean
            Get
                Return EventDateTime.TimeOfDay > After.TimeOfDay AndAlso EventDateTime.TimeOfDay < Before.TimeOfDay
            End Get
        End Property
    End Structure
    Private Shared Providers As Mail
    Private Shared mSmtpException As SmtpException
    Private Shared mFrmMail As frmMail
    Friend Shared Sub ShowMailSettings()
        Try
            If IsNothing(mFrmMail) OrElse mFrmMail.IsDisposed OrElse mFrmMail.Disposing Then
                mFrmMail = New frmMail
            End If
            Select Case mFrmMail.ShowDialog
                Case DialogResult.None Or DialogResult.Cancel
                    'don't save, reset from saved 
                Case DialogResult.OK
                    'save settings

            End Select
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            mFrmMail.Dispose()
        End Try
    End Sub
    Friend Shared ReadOnly Property SMTPStatus As String
        Get
            If Not IsNothing(mSmtpException) Then
                Return mSmtpException.StatusCode.ToString
            Else
                Return String.Empty
            End If
        End Get
    End Property
    Friend Shared ReadOnly Property MailProviders As List(Of String)
        Get
            If IsNothing(Providers) Then Providers = New Mail
            Try
                Dim rVal As New List(Of String)
                For Each Provider As String In Providers.Keys
                    If Not Provider = "crash" Then rVal.Add(Provider)
                Next
                Return rVal
            Catch ex As Exception
                WriteError(ex.Message, Err)
                Return New List(Of String)
            End Try
        End Get
    End Property
    Friend Shared ReadOnly Property Provider(Name As String) As EmailProvider
        Get
            If IsNothing(Providers) Then Providers = New Mail
            If Providers.ContainsKey(Name) Then
                Return Providers(Name)
            Else
                Return New EmailProvider("", 587, True, "", "")
            End If
        End Get
    End Property
    Friend Shared Function SendCrashReport(Message As String) As Boolean
        If Not ExceptionSender.SMTP.SendException(Message) Then
            mSmtpException = ExceptionSender.SMTP.StatusMessage
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub New()
        MyBase.New()
        Me.Add("Yahoo", New EmailProvider("smtp.mail.yahoo.com", 995, True))
        Me.Add("Gmail", New EmailProvider("smtp.gmail.com", 587, True))
        Me.Add("Hotmail", New EmailProvider("smtp.live.com", 587, True))
        Providers = Me
    End Sub
End Class

