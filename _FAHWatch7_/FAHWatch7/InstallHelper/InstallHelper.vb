' I can't get this to work... Error 1001 stateobject not found oid
'
'
'Imports System
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Configuration.Install
'<RunInstaller(True)> _
'Public Class InstallHelper
'    Inherits Installer

'    Public Sub New()
'        MyBase.New()
'        ' Attach the 'Committed' event.
'        AddHandler Me.Committed, AddressOf MyInstaller_Committed
'        ' Attach the 'Committing' event.
'        AddHandler Me.Committing, AddressOf MyInstaller_Committing
'    End Sub 'New

'    ' Event handler for 'Committing' event.
'    Private Sub MyInstaller_Committing(ByVal sender As Object, ByVal e As InstallEventArgs)
'        Console.WriteLine("")
'        Console.WriteLine("Committing Event occured.")
'        Console.WriteLine("")
'    End Sub 'MyInstaller_Committing

'    ' Event handler for 'Committed' event.
'    Private Sub MyInstaller_Committed(ByVal sender As Object, ByVal e As InstallEventArgs)
'        Console.WriteLine("")
'        Console.WriteLine("Committed Event occured.")
'        Console.WriteLine("")
'    End Sub 'MyInstaller_Committed

'    ' Override the 'Install' method.
'    Public Overrides Sub Install(ByVal savedState As IDictionary)
'        MyBase.Install(savedState)
'    End Sub 'Install

'    ' Override the 'Commit' method.
'    Public Overrides Sub Commit(ByVal savedState As IDictionary)
'        MyBase.Commit(savedState)
'    End Sub 'Commit

'    ' Override the 'Rollback' method.
'    Public Overrides Sub Rollback(ByVal savedState As IDictionary)
'        MyBase.Rollback(savedState)
'    End Sub 'Rollback
'    Public Shared Sub Main()
'        Console.WriteLine("Usage : installutil.exe Installer.exe ")
'    End Sub 'Main
'    Protected Overrides Sub OnAfterUninstall(savedState As System.Collections.IDictionary)
'        MyBase.OnAfterUninstall(savedState)
'    End Sub
'    Public Overrides Sub Uninstall(savedState As System.Collections.IDictionary)
'        MyBase.Uninstall(savedState)
'    End Sub
'    Protected Overrides Sub OnBeforeInstall(savedState As System.Collections.IDictionary)
'        MyBase.OnBeforeInstall(savedState)
'    End Sub
'    Protected Overrides Sub OnCommitted(savedState As System.Collections.IDictionary)
'        MyBase.OnCommitted(savedState)
'    End Sub
'    Protected Overrides Sub OnCommitting(savedState As System.Collections.IDictionary)
'        MyBase.OnCommitting(savedState)
'    End Sub

'End Class
