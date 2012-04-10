Imports System.Security.Permissions
Module modLinkDemand
    <EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted:=True)> _
    Public Function EnvironmentSetting(environmentVariable As String) As String
        Dim envPermission As New EnvironmentPermission(EnvironmentPermissionAccess.Read, environmentVariable)
        envPermission.Assert()
        Return Environment.GetEnvironmentVariable(environmentVariable)
    End Function
End Module
