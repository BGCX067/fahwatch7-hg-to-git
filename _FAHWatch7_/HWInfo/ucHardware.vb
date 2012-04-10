Imports OpenHardwareMonitor.Hardware
Imports HWInfo.clsHWInfo.cHWInfo.cOHMInterface
Public Class ucHardware
    Private dSensors As New Dictionary(Of String, ucSensor)
    Private mHW As IHardware
    Public ReadOnly Property HardwareIdentifier As String
        Get
            If IsNothing(mHW) Then
                Return String.Empty
            Else
                Return mHW.Identifier.ToString
            End If
        End Get
    End Property
    Public Sub Attach(HW As IHardware)
        mHW = HW
        mHW.Update()
        If mHW.HardwareType = HardwareType.CPU Then
            For Each Sensor As ISensor In mHW.Sensors
                If Sensor.SensorType = SensorType.Load And Not Sensor.Name.ToLower.Contains("total") Then
                    Me.Height += 22
                    Dim nUC As New ucSensor
                    nUC.Sensor = Sensor
                    dSensors.Add(nUC.Identifier, nUC)
                    tlpSensors.Controls.Add(nUC, 0, tlpSensors.RowCount)
                    Dim sString As String = Sensor.Identifier.ToString.Replace("load", "temperature")
                    For Each tSensor As ISensor In mHW.Sensors
                        If tSensor.Identifier.ToString = sString Then
                            nUC = New ucSensor
                            nUC.Sensor = tSensor
                            dSensors.Add(nUC.Identifier, nUC)
                            tlpSensors.Controls.Add(nUC, 1, tlpSensors.RowCount)
                            Exit For
                        End If
                    Next
                    tlpSensors.RowCount += 1
                End If
            Next
        End If

    End Sub
    Public Sub AttachATI(AtiGPU As ohmATI)

    End Sub
    Public Sub AttachNvidia(NvidiaGpu As ohmNvidia)

    End Sub
    Public Sub AttachCPU(Cpu As ohmCPU)

    End Sub
    Private Sub ucHardware_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
    Public Sub AutoUpdate(Interval As Integer)

    End Sub
    Public Sub HandleSensor(Sensor As HWInfo.clsHWInfo.cHWInfo.cOHMInterface.ohmSensors)

    End Sub
End Class
