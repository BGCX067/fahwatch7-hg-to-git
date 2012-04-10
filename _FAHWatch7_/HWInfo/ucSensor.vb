Imports System.Drawing.Drawing2D
Imports OpenHardwareMonitor.Hardware
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Drawing
Public Class ucSensor
    Inherits UserControl
#Region "sensor"
    Private mSensor As ISensor
    Private WithEvents mTimer As New Timers.Timer
    Private Visitor As New SensorVisitor(New SensorEventHandler(AddressOf SensorEvent))
    Private Sub SensorEvent(Sensor As ISensor)
        If Not IsNothing(Sensor.Value) Then
            If mValueMin = 0 Then
                mValueMin = CInt(Sensor.Value)
            Else
                If CInt(Sensor.Value) < mValueMin Then
                    mValueMin = CInt(Sensor.Value)
                End If
            End If
            If mValueMax = 0 Then
                mValueMax = CInt(Sensor.Value)
            Else
                If CInt(Sensor.Value) > mValueMax Then
                    mValueMax = CInt(Sensor.Value)
                End If
            End If
            Console.WriteLine(Sensor.Identifier.ToString & " :" & Sensor.Value.ToString)
            If Not IsNothing(Sensor.Value) Then
                If mCurrentValue <> Sensor.Value Then
                    Me.Invalidate()
                End If
            End If
        End If
    End Sub
    Private Sub mTimer_Elapsed(sender As Object, e As System.Timers.ElapsedEventArgs) Handles mTimer.Elapsed
        If IsNothing(mSensor) Then
            mTimer.Enabled = False
            Return
        End If
        mSensor.Hardware.Update()
        mSensor.Accept(Visitor)
    End Sub
    Public Sub enableTimer(Interval As Integer)
        mTimer = New Timers.Timer
        mTimer.Interval = Interval
        mTimer.AutoReset = True
        mTimer.Enabled = True
    End Sub
    Public ReadOnly Property timerEnabled As Boolean
        Get
            If IsNothing(mTimer) Then
                Return False
            Else
                Return mTimer.Enabled
            End If

        End Get
    End Property
    Public Sub stopTimer()
        mTimer.Enabled = False
    End Sub
    Public ReadOnly Property IsEmpty As Boolean
        Get
            If IsNothing(mSensor) Then
                Return True
            Else
                Return Not IsNothing(mSensor.Value)
            End If
        End Get
    End Property
    Public ReadOnly Property Identifier As String
        Get
            If IsNothing(mSensor) Then
                Return String.Empty
            Else
                Return mSensor.Identifier.ToString
            End If
        End Get
    End Property
    Public ReadOnly Property hardwareIdentifier As String
        Get
            If IsNothing(mSensor) Then
                Return String.Empty
            Else
                Return mSensor.Hardware.Identifier.ToString
            End If
        End Get
    End Property
    Public ReadOnly Property hardwareType As HardwareType
        Get
            If IsNothing(mSensor) Then
                Return CType(-1, OpenHardwareMonitor.Hardware.HardwareType)
            Else
                Return mSensor.Hardware.HardwareType
            End If
        End Get
    End Property
    Public WriteOnly Property Sensor As ISensor
        Set(value As ISensor)
            mSensor = value
        End Set
    End Property
#End Region
#Region "colors"
    Private mbackColor As Color = Color.FromKnownColor(KnownColor.Control)
    Public Shadows Property backColor As Color
        Get
            Return mbackColor
        End Get
        Set(value As Color)
            mbackColor = value
            Me.Invalidate()
        End Set
    End Property
    Private mNormalColor As Color = Color.Yellow
    Public Property normalColor As Color
        Get
            Return mNormalColor
        End Get
        Set(value As Color)
            mNormalColor = value
            Me.Invalidate()
        End Set
    End Property
    Private mWarningColor As Color = Color.Red
    Public Property warningColor As Color
        Get
            Return mWarningColor
        End Get
        Set(value As Color)
            mWarningColor = value
            Me.Invalidate()
        End Set
    End Property
#End Region
#Region "range & values"
    Private mCurrentValue As Single
    Public ReadOnly Property CurrentValue As Single
        Get
            Return mCurrentValue
        End Get
    End Property
    Public Enum eTemperatureUnit
        Celsius
        Fahrenheit
    End Enum
    Public Property TemperatureUnit As eTemperatureUnit = eTemperatureUnit.Celsius
    Private iMin As Integer = 0
    Public Property minValue As Integer
        Get
            Return iMin
        End Get
        Set(value As Integer)
            iMin = value
            Me.Invalidate()
        End Set
    End Property
    Private iMax As Integer = 100
    Public Property maxValue As Integer
        Get
            Return iMax
        End Get
        Set(value As Integer)
            iMax = value
            Me.Invalidate()
        End Set
    End Property
    Private iWarning As Integer = 80
    Public Property warningStart As Integer
        Get
            Return iWarning
        End Get
        Set(value As Integer)
            iWarning = value
        End Set
    End Property
    Private iNormal As Integer = 50
    Public Property normalStart As Integer
        Get
            Return iNormal
        End Get
        Set(value As Integer)
            iNormal = value
        End Set
    End Property
    Private mValueMin As Single
    Public ReadOnly Property valueMin As Single
        Get
            Return mValueMin
        End Get
    End Property
    Private mValueMax As Single
    Private ReadOnly Property valueMax As Single
        Get
            Return mValueMax
        End Get
    End Property
    Private mSmallStep As Integer = 5
    Public Property SmallStep As Integer
        Get
            Return mSmallStep
        End Get
        Set(value As Integer)
            mSmallStep = value
        End Set
    End Property
    Private mLargeStep As Integer = 10
    Public Property LargeStep As Integer
        Get
            Return mLargeStep
        End Get
        Set(value As Integer)
            mLargeStep = value
        End Set
    End Property
#End Region
#Region "orientation"
    Public Enum eOrientation
        Horizontal = 1
        Vertical = 2
    End Enum
    Public Orientation As eOrientation = eOrientation.Horizontal
#End Region
#Region "Painting"
    Private bmpGradient As Bitmap, gradientGraphics As Graphics
    Private rectUC As Rectangle, rectBN As Rectangle, rectNW As Rectangle
    Private bnBrush As LinearGradientBrush, nwBrush As LinearGradientBrush
    Private Sub ucTest_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        CreateGradient()
        e.Graphics.DrawImage(bmpGradient, 0, 0, Me.ClientRectangle.Width, Me.ClientRectangle.Height)
        If Not IsNothing(mSensor) Then
            tTip.SetToolTip(Me, mSensor.Identifier.ToString & Chr(32) & "min: " & mSensor.Min.ToString & " max: " & mSensor.Max.ToString & " current: " & mSensor.Value.ToString)
            If Not IsNothing(mSensor.Value) Then
                mCurrentValue = CSng(mSensor.Value)
                Dim sFormat As New StringFormat
                sFormat.Alignment = StringAlignment.Center
                sFormat.LineAlignment = StringAlignment.Center
                Dim sFont As New Font(ucSensor.DefaultFont, FontStyle.Bold)
                Dim vString As String = mSensor.Name.Replace("GPU", "").Replace("CPU", "").Replace("#", "") & Chr(32)
                If mSensor.SensorType = SensorType.Temperature Then
                    If TemperatureUnit = eTemperatureUnit.Celsius Then
                        vString &= CStr(CInt(mSensor.Value)) & "c"
                    Else
                        vString &= CStr(CInt(mSensor.Value) * 1.8 + 32) & "f"
                    End If
                ElseIf mSensor.SensorType = SensorType.Control Or mSensor.SensorType = SensorType.Fan Or mSensor.SensorType = SensorType.Load Then
                    vString &= CStr(CInt(mSensor.Value)) & "%"
                ElseIf mSensor.SensorType = SensorType.Voltage Then
                    vString &= CStr(mSensor.Value) & "v"
                End If
                e.Graphics.DrawString(vString, sFont, Brushes.Black, Me.ClientRectangle, sFormat)
            End If
        End If
    End Sub
    Public Sub CreateGradient()
        If Orientation = eOrientation.Horizontal Then
            'rectBN = New Rectangle(0, 0, iNormal, Me.ClientRectangle.Height)
            rectBN = New Rectangle(0, 0, iNormal, 100)
            bnBrush = New LinearGradientBrush(rectBN, mbackColor, mNormalColor, 0)
            'rectNW = New Rectangle(iNormal, 0, iMax - iNormal, Me.ClientRectangle.Height)
            rectNW = New Rectangle(iNormal, 0, iMax - iNormal, 100)
            nwBrush = New LinearGradientBrush(rectNW, mNormalColor, mWarningColor, 0)
            bmpGradient = New Bitmap(iMax - iMin, 100)
        Else
            rectBN = New Rectangle(0, 0, Me.ClientRectangle.Width, iNormal)
            bnBrush = New LinearGradientBrush(rectBN, mbackColor, mNormalColor, 90)
            rectNW = New Rectangle(0, iNormal, Me.ClientRectangle.Width, iWarning)
            nwBrush = New LinearGradientBrush(rectNW, mNormalColor, mWarningColor, 90)
            bmpGradient = New Bitmap(Me.ClientRectangle.Width, iMax - iMin)
        End If
        gradientGraphics = Graphics.FromImage(bmpGradient)
        gradientGraphics.FillRectangle(New SolidBrush(mbackColor), Me.ClientRectangle)
        gradientGraphics.FillRectangle(bnBrush, rectBN)
        gradientGraphics.FillRectangle(nwBrush, rectNW)
        If Orientation = eOrientation.Vertical Then bmpGradient.RotateFlip(RotateFlipType.Rotate180FlipNone)
        gradientGraphics.Flush(FlushIntention.Flush)
        If Orientation = eOrientation.Horizontal Then
            If mSmallStep > 0 AndAlso mLargeStep > 0 Then
                Dim dHeight As Integer = 10
                For xInt As Integer = (maxValue - minValue) To 5 Step -mSmallStep
                    Dim rectVisual As Rectangle = New Rectangle
                    rectVisual.Height = dHeight
                    rectVisual.Width = 1
                    rectVisual.X = xInt
                    gradientGraphics.FillRectangle(New SolidBrush(Color.Gray), rectVisual)
                    rectVisual.Y = 100 - dHeight
                    gradientGraphics.FillRectangle(New SolidBrush(Color.Gray), rectVisual)
                Next
                dHeight = 20
                For xInt As Integer = (maxValue - minValue) To 10 Step -mLargeStep
                    Dim rectVisual As Rectangle = New Rectangle
                    rectVisual.Height = dHeight
                    rectVisual.Width = 1
                    rectVisual.X = xInt
                    gradientGraphics.FillRectangle(New SolidBrush(Color.Gray), rectVisual)
                    rectVisual.Y = 100 - dHeight
                    gradientGraphics.FillRectangle(New SolidBrush(Color.Gray), rectVisual)
                Next
            End If
        End If
        If mTimer.Enabled = False Then Return
        If Not IsNothing(mSensor) AndAlso Not IsNothing(mSensor.Value) Then
            If Orientation = eOrientation.Horizontal Then
                Dim recValue As New Rectangle(CInt(mSensor.Value), 2, 1, Rectangle.Union(rectBN, rectNW).Height - 4)
                'Dim recTest As New Rectangle( CInt(mSensor.Value),1,1,
                gradientGraphics.FillRectangle(New SolidBrush(Color.DarkBlue), recValue)
            Else
                'Dim recValue As New Rectangle(2, CInt(mSensor.Value), 2, Me.ClientRectangle.Width - 2)
                'gradientGraphics.FillRectangle(New SolidBrush(Color.DarkBlue), recValue)
            End If
        End If
        'Me.BackgroundImage = bmpGradient
        'Me.BackgroundImageLayout = ImageLayout.Stretch
    End Sub
    Private Sub ucTest_SizeChanged(sender As Object, e As System.EventArgs) Handles Me.SizeChanged
        CreateGradient()
    End Sub
#End Region
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        CreateGradient()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(minValue As Integer, maxValue As Integer, normalValue As Integer, warningValue As Integer, Sensor As ISensor)
        InitializeComponent()
        iMin = minValue
        iMax = maxValue
        iNormal = normalValue
        iWarning = warningValue
        mSensor = Sensor
        CreateGradient()
    End Sub
End Class
