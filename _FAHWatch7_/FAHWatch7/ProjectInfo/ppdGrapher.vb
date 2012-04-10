Public Class ppdGrapher
    'project select, scale xaxis from 0 to final deadline, create pointpair for every hour
    'on zoom, disable zooming out further then 0-final and if zoomed in get min and max xaxis values visible, adjust pointpair list to minutes 
    'if the zoom level is sufficient
    'create two pointpair lists, one with bonus and one without
    Private Sub cmdToggleList_Click(sender As System.Object, e As System.EventArgs) Handles cmdToggleList.Click
        SplitContainer1.Panel1Collapsed = Not SplitContainer1.Panel1Collapsed
    End Sub

    Private Sub cmdToggleConfigure_Click(sender As System.Object, e As System.EventArgs) Handles cmdToggleConfigure.Click
        If SplitContainer2.Panel2Collapsed Then
            'going into detail mode, allow multi selections

        Else

        End If
        SplitContainer2.Panel2Collapsed = Not SplitContainer2.Panel2Collapsed
    End Sub

    Private Sub ppdGrapher_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try


        Catch ex As Exception

        End Try
    End Sub
End Class