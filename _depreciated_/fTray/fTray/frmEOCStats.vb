Imports ZedGraph
Public Class frmEOCXML
    Private MyXML() As clsEOC.clsUpdates
    Private uPane As GraphPane, tPane As GraphPane
    Private WithEvents myEOC As clsEOC = ClientControl.EOC.EOCStats
    Private Sub HandleUpdate() Handles myEOC.UpdateRecieved
        Try

        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmStats_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            uPane = zUser.GraphPane
            uPane.Title.Text = ClientControl.MyClient.UserName
            tPane = zTeam.GraphPane
            tPane.Title.Text = ClientControl.MyClient.TeamNumber


            Dim alUpdates As ArrayList = ClientControl.Statistics.GetAllXMLUpdates
            ReDim MyXML(0 To 0)
            For Each sUpdate In alUpdates
                MyXML(MyXML.GetUpperBound(0)) = CType(sUpdate, clsEOC.clsUpdates)
                ReDim Preserve MyXML(0 To MyXML.GetUpperBound(0) + 1)
            Next
            ReDim Preserve MyXML(0 To MyXML.GetUpperBound(0) - 1)
            For Each XMLUpdate As clsEOC.clsUpdates In MyXML
                With XMLUpdate.Update.User


                End With
                With XMLUpdate.Update.Team

                End With

            Next
        Catch ex As Exception
            LogWindow.WriteError("EOCXML_Load", Err)
        End Try
    End Sub
End Class