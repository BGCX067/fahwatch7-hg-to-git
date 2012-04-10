Imports System.Net.Mail
Public Class SMTP
    Private Shared mSMTPException As SmtpException
    Public Shared ReadOnly Property StatusMessage As SmtpException
        Get
            Return mSMTPException
        End Get
    End Property
    Public Shared Function SendException(Message As String) As Boolean
        Try
            Dim bSucces As Boolean = False
            Dim SmtpServer As New SmtpClient()
            Dim mail As New MailMessage()
            SmtpServer.Credentials = New Net.NetworkCredential(CStr("fahwatch7-crash@hotmail.com"), CStr("f4hw4tch7"))
            SmtpServer.Port = CInt(587)
            SmtpServer.Host = CStr("smtp.live.com")
            SmtpServer.EnableSsl = True
            mail = New MailMessage()
            mail.From = New MailAddress(CStr("fahwatch7-crash@hotmail.com"))
            mail.To.Add(CStr("FAHWatch7@hotmail.com"))
            mail.Subject = CStr("FAHWatch7 Exception report")
            mail.Body = Message
            Try
                SmtpServer.Send(mail)
                bSucces = True
            Catch exSmtp As SmtpException
                mSMTPException = exSmtp
            End Try
            Return bSucces
        Catch ex As Exception
            mSMTPException = New SmtpException("Exception: " & ex.Message & Environment.NewLine & "Stacktrace: " & ex.StackTrace)
            Return False
        End Try
    End Function
End Class
