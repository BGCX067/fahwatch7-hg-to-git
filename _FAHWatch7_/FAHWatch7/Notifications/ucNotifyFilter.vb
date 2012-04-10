'   FAHWatch7 UCNotifyFilter
'
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
Friend Class ucNotifyFilter
    Friend Class eEventArgs
        Inherits EventArgs
        Friend IsEnabled As Boolean
        Friend Client As String = ""
        Friend slotID As String = ""
    End Class
    Friend bManual As Boolean = False
    Friend Shadows Event EnabledChanged(sender As Object, e As eEventArgs)
    Friend Event bBoth(CheckBoth As Boolean)
    Friend Event bEUE(CheckEUE As Boolean)
    Friend Event bRATE(CheckRate As Boolean)
    Friend Event RATE(Rate As Int32)
    Friend Event EUE(EUE As Int32)
    Private myFilter As New modMySettings.clsNFilter
    Friend Property Settings As modMySettings.clsNFilter
        Get
            Try
                Return Update()
            Catch ex As Exception
                WriteError(ex.Message, Err)
                myFilter.bError = True
            End Try
            Return myFilter
        End Get
        Set(value As modMySettings.clsNFilter)
            Try
                Init(value)
            Catch ex As Exception
                WriteError(ex.Message, Err)
            End Try
        End Set
    End Property
    Friend Shadows Function Update() As modMySettings.clsNFilter
        Try
            With myFilter
                .fEnabled = chkRuleName.Checked
                .bRate = rbEUE_ratio.Checked
                .bEUE = rbEUE_Always.Checked
                .iRATE = CInt(nudRatio_Warning.Value)
            End With
        Catch ex As Exception
            WriteError(ex.Message, Err)
            myFilter.bError = True
        End Try
        Return myFilter
    End Function
    Friend Function Init(Optional nFilter As modMySettings.clsNFilter = Nothing) As Boolean
        Try
            If Not IsNothing(nFilter) Then myFilter = nFilter
            With myFilter
                bManual = True
                chkRuleName.Text = .fName
                chkRuleName.Checked = .fEnabled
                pnlOptions.Enabled = .fEnabled
                rbSlotFail.Checked = .bFail
                rbEUE.Checked = .bEUE
                rbEUE_ratio.Checked = .bRate
                rbEUE_Always.Checked = Not .bRate
                nudRatio_Warning.Value = CDec(.iRATE)
                txtRatio_Actual.Text = .currentRate
                pnlEUE_1.Enabled = rbEUE.Checked
                If rbEUE.Checked Then
                    pnlEUE_2.Enabled = rbEUE_ratio.Checked
                Else
                    pnlEUE_2.Enabled = False
                End If
                bManual = False
            End With
            Return True
        Catch ex As Exception
            WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
    Private Sub chkRuleName_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkRuleName.CheckedChanged
        If bManual Then Exit Sub
        Try
            bManual = True
            Dim nEv As New eEventArgs
            Dim cName As String = ""
            If InStr(myFilter.fName, "_") = 0 Then
                cName = myFilter.fName
            Else
                cName = myFilter.fName.Substring(8, myFilter.fName.IndexOf("_", 8) - 8)
            End If
            If myFilter.fName.IndexOf("_") = -1 Then
                nEv.Client = cName
                nEv.slotID = "-1"
                nEv.IsEnabled = chkRuleName.Checked
            Else
                Dim sIndex As String = myFilter.fName.Substring(myFilter.fName.LastIndexOf("_") + 1, myFilter.fName.IndexOf(":") - myFilter.fName.LastIndexOf("_") - 1)
                If sIndex.Length = 1 Then sIndex = "0" & sIndex
                nEv.Client = cName
                nEv.slotID = sIndex
                nEv.IsEnabled = chkRuleName.Checked
            End If
            pnlOptions.Enabled = chkRuleName.Checked
            If rbEUE_ratio.Checked And rbEUE_ratio.Enabled Then
                nudRatio_Warning.Enabled = True
            Else
                nudRatio_Warning.Enabled = False
            End If
            RaiseEvent EnabledChanged(Me, nEv)
            nEv = Nothing
            cName = Nothing
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            bManual = False
            GC.Collect()
        End Try
    End Sub
    Private Sub rbEUE_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbEUE.CheckedChanged
        If bManual Then Exit Sub
        bManual = True
        pnlEUE_1.Enabled = rbEUE.Checked
        pnlEUE_2.Enabled = rbEUE_ratio.Checked
        If rbEUE_ratio.Checked And rbEUE_ratio.Enabled Then
            nudRatio_Warning.Enabled = True
        Else
            nudRatio_Warning.Enabled = False
        End If
        bManual = False
    End Sub
    Private Sub rbEUE_Always_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbEUE_Always.CheckedChanged
        If bManual Then Exit Sub
        bManual = True
        pnlEUE_2.Enabled = rbEUE_ratio.Checked
        If rbEUE_ratio.Checked And rbEUE_ratio.Enabled Then
            nudRatio_Warning.Enabled = True
        Else
            nudRatio_Warning.Enabled = False
        End If
        bManual = False
    End Sub
End Class
