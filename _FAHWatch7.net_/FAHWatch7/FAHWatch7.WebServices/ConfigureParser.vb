Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports mojoPortal.Web
Imports mojoPortal.Web.framework
Imports mojoPortal.Web.UI
Imports log4net
Imports mojoPortal.Business
Public Class ConfigureParser
    Inherits SiteModuleControl
    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)
        AddHandler Me.Load, AddressOf Page_Load
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        loadsettings()
        populatelabels()
        populatecontrols()
    End Sub
    Private Sub LoadSettings()

    End Sub
    Private Sub PopulateControls()

    End Sub
    Private Sub PopulateLabels()

    End Sub
    
End Class
