Imports System.Web
Imports System.Web.Services
Imports log4net
Imports FAHWatch7.Business
Public Class downloadSummary
    Implements System.Web.IHttpHandler
    Shared log As log4net.ILog = log4net.LogManager.GetLogger(GetType(FAHWatch7.WebServices.downloadSummary))
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Try
            If context.Request.QueryString IsNot Nothing Then
                If String.Equals(context.Request.QueryString.ToString, "current", StringComparison.InvariantCultureIgnoreCase) OrElse String.Equals(context.Request.QueryString.ToString, "changelog", StringComparison.InvariantCultureIgnoreCase) Then
                    context.Response.AddHeader("Content-Disposition", "attachement; filename=Summary.xml")
                    Dim xmlString As String

                    If String.Equals(context.Request.QueryString.ToString, "changelog", StringComparison.InvariantCultureIgnoreCase) Then
                        xmlString = ProjectInfo.xmlSerializedDepreciated
                    Else
                        xmlString = ProjectInfo.XmlSerializedpSummary
                    End If
                    context.Response.AddHeader("Content-Length", CStr(xmlString.Length))
                    context.Response.AddHeader("Connection", "close")
                    context.Response.ContentType = "text/xml"
                    context.Response.Write(xmlString)
                    context.Response.Flush()
                End If
            End If
        Catch ex As Exception
            log.Error(ex)
            context.Response.Clear()
            context.Response.Redirect("~/errorpage.aspx")
        End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

End Class