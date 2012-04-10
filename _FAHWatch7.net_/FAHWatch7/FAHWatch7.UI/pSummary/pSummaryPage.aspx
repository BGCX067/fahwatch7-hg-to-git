<%@ Page Title="Historical Folding@Home project summary" Language="vb" MasterPageFile="~/App_MasterPages/pSummary.Master"  %>

<%@ Import Namespace="FAHWatch7.Core.Definitions" %>
<%@ Import Namespace="FAHWatch7.Business"  %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.UI" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Import Namespace ="log4net" %>


<script runat="server">
    
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(psummary_psummarypage_aspx))
 
    Sub Page_Load()
        Try
            If Context.Request.QueryString IsNot Nothing Then
                If String.Equals(Context.Request.QueryString.ToString, "current", StringComparison.InvariantCultureIgnoreCase) Then
                    PopulateSummary()
                ElseIf String.Equals(Context.Request.QueryString.ToString, "changelog", StringComparison.InvariantCultureIgnoreCase) Then
                    populateChanges()
                ElseIf String.IsNullOrEmpty(Context.Request.QueryString.ToString) Then
                    PopulateSummary()
                End If
            End If
        Catch ex As Exception
            log.Error(ex)
            Context.Response.Redirect("~/NeatUpload/Error413.aspx")
        End Try
    End Sub

    Sub populateChanges()
     Try
            If ProjectInfo.Projects.Count = 0 Then
                If Not ProjectInfo.LoadProjects() Then
                    label1.Text = "No projects stored, sorry. Check back later!"
                    Return
                Else
                    If ProjectInfo.DepreciatedCount > 0 Then
                        label1.Text = "Project summary change log, last collected at: " & ProjectInfo.LastParse.ToString(CultureInfo.CurrentCulture) & " known projects " & ProjectInfo.SummaryCount & " (" & ProjectInfo.DepreciatedCount & ")"
                        rptSummary.DataSource = ProjectInfo.DepreciatedProjectsList
                        'ddlproject.items.add("")
                        rptSummary.DataBind()
                    Else
                        label1.Text = "Oops.. something went wrong, sorry."
                    End If
                End If
            Else
                label1.Text = "Project summary change log, last collected at: " & ProjectInfo.LastParse.ToString(CultureInfo.CurrentCulture) & " known projects " & ProjectInfo.SummaryCount & " (" & ProjectInfo.DepreciatedCount & ")"
                rptSummary.DataSource = ProjectInfo.DepreciatedProjectsList
                rptSummary.DataBind()
            End If
        Catch ex As Exception
            log.Error(ex)
            Context.Response.Redirect("~/NeatUpload/Error413.aspx")
        End Try
    End Sub



    Sub PopulateSummary()
      Try
            If ProjectInfo.Projects.Count = 0 Then
                If Not ProjectInfo.LoadProjects() Then
                    label1.Text = "No projects stored, sorry. Check back later!"
                    Return
                Else
                    If ProjectInfo.Projects.Count > 0 Then
                        label1.Text = "Project summary, last collected at: " & ProjectInfo.LastParse.ToString(CultureInfo.CurrentCulture) & " known projects " & ProjectInfo.SummaryCount & " (" & ProjectInfo.DepreciatedCount & ")"
                        rptSummary.DataSource = ProjectInfo.Projects
                                              
                        rptSummary.DataBind()
                    Else
                        label1.Text = "Oops.. something went wrong, sorry."
                    End If
                End If
            Else
                label1.Text = "Project summary, last collected at: " & ProjectInfo.LastParse.ToString(CultureInfo.CurrentCulture) & " known projects " & ProjectInfo.SummaryCount & " (" & ProjectInfo.DepreciatedCount & ")"
                rptSummary.DataSource = ProjectInfo.Projects
                rptSummary.DataBind()
            End If
        Catch ex As Exception
            log.Error(ex)
            Context.Response.Clear()
            Context.Response.Redirect("~/ErrorPage.aspx")
        End Try

    End Sub

    Sub onClick(sender As Object, e As EventArgs)
        Dim Item As RepeaterItem = DirectCast(sender, RepeaterItem)
        
    End Sub
    
    Sub onMouseUp(sender As Object, e As EventArgs)
        Dim Item As RepeaterItem = DirectCast(sender, RepeaterItem)
        
    End Sub
    
    Sub onMouseOver(sender As Object, e As EventArgs)
        Dim Item As RepeaterItem = DirectCast(sender, RepeaterItem)
        
    End Sub
    
    Sub onMouseOut(Sender As Object, e As EventArgs)
        Dim Item As RepeaterItem = DirectCast(Sender, RepeaterItem)
        
    End Sub

    Protected Sub rptSummary_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs)

    End Sub
</script>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <portal:mojolabel ID ="label1" runat="server" Font-Size="Medium" /> 
    <asp:Repeater ID="rptSummary" runat = "server" 
        onitemcommand="rptSummary_ItemCommand" >
    <HeaderTemplate>
       <table id="Table1" border="1" style="width: auto; white-space: nowrap; text-align: center; border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">
            <tr bgcolor="#4444F3" id="tblHeader" onmouseover="hoverHeader" runat ="server"  style =" border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black " >
            <th runat="server" onclick ="onClick" id="Th1" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Project Number</th>
            <th runat="server" onclick ="onClick" id="Th2" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Credit</th>
            <th runat="server" onclick ="onClick" id="Th3" style ="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Kfactor</th>
            <th runat="server" onclick ="onClick" id="Th4" style ="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Preferred (days)</th>
            <th runat="server" onclick ="onClick" id="Th5" style ="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Final deadline (days)</th>
            <th runat="server" onclick ="onClick" id="Th6" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black " >Work Unit Name</th>
            <th runat="server" onclick ="onClick" id="Th7" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Number of Atoms</th>
            <th runat="server" onclick ="onClick" id="Th8" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Frames</th>
            <th runat="server" onclick ="onClick" id="Th9" style ="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Code</th>
            <th runat="server" onclick ="onClick" id="Th10" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Server IP</th>
            <th runat="server" onclick ="onClick" id="Th11" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black " >Contact</th>
            <th runat="server" onclick ="onClick" id="Th12" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black " >Description</th>
            <th runat="server" onclick ="onClick" id="Th13" style="border-bottom-color: Black ; border-bottom-style: solid ; border-bottom-width: thin ; border-left-style: solid ; border-left-color: Black; border-left-width : thin ; border-right-style: solid; border-right-width : thin ; border-right-color: Black ; border-top-style : solid ; border-top-width:thin ; border-top-color: Black ">Date added</th>
      </tr>
</HeaderTemplate>
<ItemTemplate>
    <tr id="Tr1" runat="server" bgcolor="#dfdfdf" onmouseover="this.style.backgroundColor='#F30B0F'" onmouseout="this.style.backgroundColor=''" onclick="onClick" onmouseup="OnMouseUp" >
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).ProjectNumber%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Credit%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).kFactor%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).PreferredDays%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).FinalDeadline%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).WUName%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).NumberOfAtoms%></td>       
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Frames%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Code%></td>       
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).ServerIP%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Contact%></td>
        <td><a target="_blank" href="<%# CType(DataBinder.GetDataItem(Container), pSummary).Description%>">Description</td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).dtSummary%></td>
    </tr>
</ItemTemplate>
<AlternatingItemTemplate >
    <tr id="Tr2" runat="server" bgcolor="#ffffff"  onmouseover="this.style.backgroundColor='#F30B0F'" onmouseout="this.style.backgroundColor=''" onclick="onClick" onmouseup="OnMouseUp" >
         <td><%# CType(DataBinder.GetDataItem(Container), pSummary).ProjectNumber%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Credit%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).kFactor%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).PreferredDays%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).FinalDeadline%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).WUName%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).NumberOfAtoms%></td>       
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Frames%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Code%></td>       
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).ServerIP%></td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).Contact%></td>
        <td><a target="_blank" href="<%# CType(DataBinder.GetDataItem(Container), pSummary).Description%>">Description</td>
        <td><%# CType(DataBinder.GetDataItem(Container), pSummary).dtSummary%></td>
    </tr>
</AlternatingItemTemplate>
<FooterTemplate>
    </table>
</FooterTemplate>
</asp:Repeater>
</asp:Content>


