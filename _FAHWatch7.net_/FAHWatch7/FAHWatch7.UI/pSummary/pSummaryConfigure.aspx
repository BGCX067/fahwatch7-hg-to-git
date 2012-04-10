<%@ Page Title="" Language="vb" MasterPageFile="~/App_MasterPages/pSummary.Master"  %>

<%@ Import Namespace="FAHWatch7.Business" %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.UI" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Import Namespace ="log4net" %>

<script runat= "server" >

    Private ReadOnly log As ILog = LogManager.GetLogger(GetType(psummary_psummaryconfigure_aspx))
    Private SiteUser As SiteUser = SiteUtils.GetCurrentSiteUser
    Sub Page_Load()
        Try
         
            If Not mojoPortal.Business.WebHelpers.WebUser.IsAdmin Then
                log.Warn("Attempt to access the pSummary configuration from " & Request.UserHostAddress & "/" & Request.UserHostName & " redirected to AccessDenied")
                SiteUtils.RedirectToAccessDeniedPage()
            Else
                lblSummaryCount.Text = ProjectInfo.SummaryCount
                lblChangeCount.Text = ProjectInfo.DepreciatedCount
                lblLastParse.Text = ProjectInfo.LastParse.ToString
                'do more..
                
            End If
        Catch ex As Exception
            log.Error(ex)
            'SiteUtils.RedirectToAccessDeniedPage()
        End Try
    End Sub
    
       
    
    Protected Sub cmdSet_Click(sender As Object, e As System.EventArgs)
        Dim SP As New FAHWatch7.Business.pSummaryParser
        SP.StartTask()
    End Sub
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <asp:Label ID="Label2" runat="server" BorderStyle="Inset" Text="Last parse event" width="236px" />
    <asp:Label ID="lblLastParse" runat="server" BorderStyle="Inset" width="160px" />
    <br />
    <asp:Label ID="Label3" runat="server" BorderStyle="Inset" Text="Next parse due time" width="236px" />
    <asp:Label ID="lblNextParse" runat="server" BorderStyle="Inset" width="160px" />
    <br />
    <asp:Label ID="Label4" runat="server" BorderStyle="Inset" Text="Known projects" width="236px" />
    <asp:Label ID="lblSummaryCount" runat="server" BorderStyle="Inset" width="160px" />
    <br />
    <asp:Label ID="Label5" runat="server" BorderStyle="Inset" Text="Known changes" width="236px" />
    <asp:Label ID="lblChangeCount" runat="server" BorderStyle="Inset" width="160px" />
    <br />
    <hr align="left" style="width: 379px" />
    <br />
    <asp:Label ID="Label1" runat="server" BorderStyle="Inset" Text="automated pSummary parser enabled"          width="236px" />
    <asp:CheckBox ID="chkAutoEnabled" runat="server" />
    <br />
    <asp:Label ID="Label6" runat="server" BorderStyle="Inset" Text="Automated pSummary parser interval" width="236px" />
    <asp:DropDownList ID="ddlInterval" runat="server">
        <asp:ListItem Selected="True" Text="Once every hour" />
        <asp:ListItem Text="Once every two hours" />
        <asp:ListItem Text="Once every three hours" />
        <asp:ListItem Text="Once every four hours" />
    </asp:DropDownList>
    <br />
    <asp:Label ID="Label7" runat="server" BorderStyle="Inset" Text="Minutes after the hour to run update" width="236px" />
    <asp:DropDownList ID="ddlMinutes" runat="server" width="160px">
        <asp:ListItem Selected="True" Text="Five minutes" />
        <asp:ListItem Text ="</asp:ListItem>" />
        <asp:ListItem Text="Fiftheen minutes" />
    </asp:DropDownList>
    <br />
    <asp:Button ID="cmdSet" runat="server" Text="Store settings" Width="396px" onclick="cmdSet_Click" />
    <br />

</asp:Content>
