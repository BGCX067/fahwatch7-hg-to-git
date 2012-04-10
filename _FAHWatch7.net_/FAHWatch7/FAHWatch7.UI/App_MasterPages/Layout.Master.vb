Imports System
Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports mojoPortal.Business
Imports mojoPortal.Web.UI

Namespace FAHWatch7.UI
    ''' <summary>
    ''' Author:             Joe Audette
    ''' Created:            2/9/2007
    ''' Last Modified:      2/9/2007
    ''' </summary>
    Partial Public Class layout
        Inherits System.Web.UI.MasterPage
        'protected int leftModuleCount = 0;
        'protected int centerModuleCount = 0;
        'protected int rightModuleCount = 0;
        'protected SiteSettings siteSettings;
        'protected PageSettings currentPage;
        'protected String protocol = "http";


        Protected Sub Page_Load(sender As Object, e As EventArgs)
            'if (SiteUtils.IsSecureRequest())
            '{
            '    protocol = "https";
            '}

            'siteSettings = CacheHelper.GetCurrentSiteSettings();
            'currentPage = CacheHelper.GetCurrentPage();

            'if ((siteSettings != null) && (currentPage != null))
            '{
            '    SetupLayout();
            '}

        End Sub

        'private void SetupLayout()
        '{
        '    if (!SiteUtils.IsNoMenuPage())
        '    {
        '        if (
        '            (SiteMenu1 != null)
        '            && (SiteMenu1.Visible)
        '            )
        '        {
        '            // printable view skin doesn't have a menu so it is null there

        '            if (SiteMenu1.Parent.ID == "divLeft")
        '            {
        '                leftModuleCount += 1;
        '            }
        '            if (SiteMenu1.Parent.ID == "divRight")
        '            {
        '                rightModuleCount += 1;
        '            }
        '        }

        '        Control c = this.FindControl("PageMenu1");
        '        if (c != null)
        '        {
        '            if (SiteUtils.TopPageHasChildren())
        '            {
        '                if (c.Parent.ID == "divLeft")
        '                {
        '                    leftModuleCount += 1;
        '                }
        '                if (c.Parent.ID == "divRight")
        '                {
        '                    rightModuleCount += 1;
        '                }
        '            }
        '        }


        '    }

        '    if (IsContentSystemPage())
        '    {
        '        foreach (Module module in currentPage.Modules)
        '        {
        '            switch (module.PaneName.ToLower())
        '            {
        '                case "leftpane":
        '                    leftModuleCount += 1;
        '                    break;
        '                case "rightpane":
        '                    rightModuleCount += 1;
        '                    break;

        '                case "contentpane":
        '                default:
        '                    centerModuleCount += 1;

        '                    break;
        '            }
        '        }

        '    }

        '    if ((Request.Url.ToString().Contains("MyPage.aspx"))
        '                || (Request.Url.ToString().Contains("ChooseContent.aspx"))
        '                || (Request.Url.ToString().Contains("SiteMail"))
        '                )
        '    {
        '        this.divLeft.CssClass = "left-mypage";
        '        this.divCenter.CssClass = "center-mypage";
        '        this.divRight.CssClass = "right-mypage";

        '    }
        '    else
        '    {
        '        if (leftModuleCount > 0)
        '        {
        '            if (rightModuleCount > 0)
        '            {
        '                this.divRight.Visible = true;
        '                this.divCenter.CssClass = "center-rightandleftmargins";

        '            }
        '            else
        '            {
        '                this.divCenter.CssClass = "center-leftmargin";
        '                this.divRight.Visible = false;
        '            }
        '        }
        '        else
        '        {
        '            this.divLeft.Visible = false;

        '            if (rightModuleCount > 0)
        '            {
        '                this.divRight.Visible = true;
        '                this.divCenter.CssClass = "center-rightmargin";
        '            }
        '            else
        '            {
        '                this.divRight.Visible = false;
        '                divCenter.CssClass = "center-nomargins";

        '            }
        '        }
        '    }
        '}

        'private bool IsContentSystemPage()
        '{
        '    bool result = false;
        '    if (
        '            (
        '            (Request.Url.ToString().ToLower().Contains("default.aspx"))
        '            || (Request.Url.ToString() == siteSettings.SiteRoot)
        '            )
        '            && (!Request.RawUrl.Contains("SiteMail"))
        '            )
        '    {

        '        result = true;
        '    }

        '    return result;
        '}

    End Class
End Namespace
