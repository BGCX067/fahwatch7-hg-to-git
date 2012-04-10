'/*

'  ucHWM Dialog for gauge settings class
'  Version: MPL 1.1/GPL 2.0/LGPL 2.1

'  The contents of this file are subject to the Mozilla Public License Version
'  1.1 (the "License"); you may not use this file except in compliance with
'  the License. You may obtain a copy of the License at

'  http://www.mozilla.org/MPL/

'  Software distributed under the License is distributed on an "AS IS" basis,
'  WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
'  for the specific language governing rights and limitations under the License.

'  The Original Code is the cftUnity.nl source code.

'  The Initial Developer of the Original Code is 
'   Marvin Westmaas/ Marvin_The_Martian / MtM ( webmaster@cftunity.nl )
'  Portions created by the Initial Developer are Copyright (C) 2010-2011
'  the Initial Developer. All Rights Reserved.

'  The repository for cftUnity.nl is hosted at
'  http://code.google.com/p/cftunity/

'  Contributor(s):

'  Alternatively, the contents of this file may be used under the terms of
'  either the GNU General Public License Version 2 or later (the "GPL"), or
'  the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
'  in which case the provisions of the GPL or the LGPL are applicable instead
'  of those above. If you wish to allow use of your version of this file only
'  under the terms of either the GPL or the LGPL, and not to allow others to
'  use your version of this file under the terms of the MPL, indicate your
'  decision by deleting the provisions above and replace them with the notice
'  and other provisions required by the GPL or the LGPL. If you do not delete
'  the provisions above, a recipient may use your version of this file under
'  the terms of any one of the MPL, the GPL or the LGPL.

'*/
Imports System.Windows.Forms


Public Class DiagGaugeSettings
    Private MySettings As clsSettings
    Public Function Assign_Settings(ByVal Location As String, Optional ByVal SetDefaults As Boolean = False, Optional ByVal Settings As clsSettings.sSettings = Nothing) As Boolean
        '(location, false,nothing)
        Try
            If Location.Contains("\config.dat") Then
                Location = Location.Replace("\config.dat", "")
            End If
            Gauge.Assign_Settings(Location, SetDefaults, Settings)
            MySettings = New clsSettings(Location)
            AddHandler MySettings.Log, AddressOf LogWindow_Log
            AddHandler MySettings.LogError, AddressOf LogWindow_LogError
            If Not IsNothing(Settings) Then
                MySettings.MySettings = Settings
            ElseIf SetDefaults Then
                MySettings.SetDefaults()
            End If
            Return True
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
            Return False
        End Try
    End Function
#Region "Log extender"
    Public Class cLW
        Public Event Log(ByVal Message As String)
        Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
        Public Sub WriteError(ByVal Message As String, ByVal EObj As ErrObject)
            RaiseEvent LogError(Message, EObj)
        End Sub
        Public Sub WriteLog(ByVal Message As String)
            RaiseEvent Log(Message)
        End Sub
    End Class
    Public WithEvents LogWindow As New cLW
    Public Event Log(ByVal Message As String)
    Public Event LogError(ByVal Message As String, ByVal EObj As ErrObject)
    Private Sub LogWindow_Log(ByVal Message As String) Handles LogWindow.Log
        RaiseEvent Log(Message)
    End Sub
    Private Sub LogWindow_LogError(ByVal Message As String, ByVal EObj As Microsoft.VisualBasic.ErrObject) Handles LogWindow.LogError
        RaiseEvent LogError(Message, EObj)
    End Sub
#End Region
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Dim sSettings As New clsSettings.sSettings.sGaugeSettings
            With sSettings
                .Identifier = Me.Text
                .backColor = Gauge.colorBack
                .borderColor = Gauge.borderColor
                .iBorder = Gauge.Border
                .iCorner = Gauge.iCorner
                .minmaxColor = Gauge.colorMinMax
                .normalMIN = Gauge.normalMin
                .rangeColor = Gauge.colorRange
                .stepLarge = Gauge.StepIntervalLarge
                .stepSmall = Gauge.StepIntervalSmall
                .valueColor = Gauge.colorCurrent
                .valueMAX = Gauge.maxValue
                .valueMIN = Gauge.minValue
                .warningColor = Gauge.colorWarning
                .warningStart = Gauge.warningStart
                .vSize = Gauge.vSize
            End With
            MySettings.MySettings.SaveGaugeSettings(Me.Text, sSettings)
            MySettings.SaveSettings()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            LogWindow.WriteError(ex.Message, Err)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public WriteOnly Property SetGauge As mtmGauge
        Set(ByVal value As mtmGauge)
            Try
                Gauge.colorBack = value.colorBack
                Gauge.Border = value.Border
                Gauge.borderColor = value.borderColor
                Gauge.iCorner = value.iCorner
                Gauge.StepIntervalLarge = value.StepIntervalLarge
                Gauge.StepIntervalSmall = value.StepIntervalSmall
                Gauge.colorCurrent = value.colorCurrent
                Gauge.colorMinMax = value.colorMinMax
                Gauge.colorRange = value.colorRange
                Gauge.colorWarning = value.colorWarning
                Gauge.minValue = value.minValue
                Gauge.maxValue = value.maxValue
                Gauge.normalMin = value.normalMin
                Gauge.warningStart = value.warningStart
                Gauge.vSize = value.vSize
                Gauge.Assign_Settings(MySettings.File, False, MySettings.MySettings)
                Gauge.AttachSensor(value.mySensor)
                'Gauge = value
                Gauge.Invalidate()
                Me.Text = value.mySensor.Identifier.ToString
            Catch ex As Exception

            End Try
        End Set
    End Property

    Private Sub DiagGaugeSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Timer1.Enabled = False

        Catch ex As Exception

        End Try
    End Sub


    Private Sub DiagGaugeSettings_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            lblRange.Text = Gauge.colorRange.ToString
            lblRange.BackColor = Gauge.colorRange
            lblEarning.Text = Gauge.colorWarning.ToString
            lblEarning.BackColor = Gauge.colorWarning
            lblBack.Text = Gauge.colorBack.ToString
            lblBack.BackColor = Gauge.colorBack
            lblValue.Text = Gauge.colorCurrent.ToString
            lblValue.BackColor = Gauge.colorCurrent
            lblMinMax.Text = Gauge.colorMinMax.ToString
            lblMinMax.BackColor = Gauge.colorMinMax
            nudMax.Value = Gauge.maxValue
            nudMin.Value = Gauge.minValue
            nudNormal.Maximum = Int32.MaxValue
            nudNormal.Value = Gauge.normalMin
            nudWarning.Maximum = Int32.MaxValue
            nudWarning.Value = Gauge.warningStart
            nudValue.Value = Gauge.CurrentValue
            nudStepLarge.Value = Gauge.StepIntervalLarge
            nudStepSmall.Value = Gauge.StepIntervalSmall
            nudVsize.Value = Gauge.vSize
            nudCorner.Value = Gauge.iCorner
            nudBorder.Value = Gauge.Border
            cmdBorder.Enabled = nudBorder.Value <> 0
            lblBorder.Text = Gauge.borderColor.ToString
            Gauge.Invalidate()
            Call Timer1_Tick(Me, Nothing)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudStepSmall_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudStepSmall.ValueChanged
        Try
            Gauge.StepIntervalSmall = nudStepSmall.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cmdRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRange.Click
        Try
            ColorDialog1.Color = lblRange.BackColor
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                lblRange.Text = ColorDialog1.Color.ToString
                lblRange.BackColor = ColorDialog1.Color
                Gauge.colorRange = ColorDialog1.Color
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdWarning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWarning.Click
        Try
            ColorDialog1.Color = lblEarning.BackColor
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                lblEarning.Text = ColorDialog1.Color.ToString
                lblEarning.BackColor = ColorDialog1.Color
                Gauge.colorWarning = ColorDialog1.Color
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdValue.Click
        Try
            ColorDialog1.Color = lblValue.BackColor
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                Gauge.colorCurrent = ColorDialog1.Color
                lblValue.Text = ColorDialog1.Color.ToString
                lblValue.BackColor = ColorDialog1.Color
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudMin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMin.ValueChanged
        Try
            Gauge.minValue = nudMin.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudMax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMax.ValueChanged
        Try
            Gauge.maxValue = nudMax.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
        'nudWarning.Maximum = nudMax.Value
        'nudNormal.Maximum = nudMax.Value
    End Sub

    Private Sub nudWarning_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWarning.ValueChanged
        Try
            Gauge.warningStart = nudWarning.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub
    Sub p() Handles Gauge.AutoSizeChanged

    End Sub
    Private Sub nudValue_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudValue.ValueChanged
        Try
            Gauge.Value = nudValue.Value
            Call Timer1_Tick(Me, Nothing)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudNormal_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNormal.ValueChanged
        Try
            Gauge.normalMin = nudNormal.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            TextBox1.Text = "Max:" & Gauge.valueMax.ToString
            TextBox2.Text = "Curr:" & Gauge.CurrentValue.ToString
            TextBox3.Text = "Min:" & Gauge.valueMin.ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        Gauge.ResetMaxMin()
    End Sub

    Private Sub nudStep_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudStepLarge.ValueChanged
        Try
            Gauge.StepIntervalLarge = nudStepLarge.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Try
            ColorDialog1.Color = CType(lblBack.BackColor, Color)
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                lblBack.Text = ColorDialog1.Color.ToString
                lblBack.BackColor = ColorDialog1.Color
                Gauge.colorBack = ColorDialog1.Color
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub cmdMinMax_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMinMax.Click
        Try
            ColorDialog1.Color = lblMinMax.BackColor
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                Gauge.colorMinMax = ColorDialog1.Color
                lblMinMax.Text = ColorDialog1.Color.ToString
                lblMinMax.BackColor = ColorDialog1.Color
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudVsize_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudVsize.ValueChanged
        Try
            Gauge.vSize = nudVsize.Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdBorder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBorder.Click
        Try
            ColorDialog1.Color = CType(lblBorder.BackColor, Color)
            If ColorDialog1.ShowDialog = DialogResult.OK Then
                lblBorder.Text = ColorDialog1.Color.ToString
                lblBorder.BackColor = ColorDialog1.Color
                Gauge.borderColor = ColorDialog1.Color
                Gauge.Border = True
                Gauge.Invalidate()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudCorner_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCorner.ValueChanged
        Try
            Gauge.iCorner = nudCorner.Value
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub nudBorder_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBorder.ValueChanged
        Try
            If nudBorder.Value <> 0 Then
                cmdBorder.Enabled = True
                Gauge.Border = nudBorder.Value
                If lblBorder.Text = "" Then lblBorder.Text = Gauge.borderColor.ToString
                lblBorder.BackColor = Color.FromName(lblBorder.Text.Replace("Color [", "").Replace("]", ""))
                Gauge.borderColor = lblBorder.BackColor
            Else
                cmdBorder.Enabled = False
                Gauge.Border = 0
            End If
            Gauge.Invalidate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub p(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Gauge.AutoSizeChanged

    End Sub
End Class
