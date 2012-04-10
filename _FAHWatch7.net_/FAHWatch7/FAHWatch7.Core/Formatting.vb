Imports System.Text
Imports System.Globalization
Imports log4net
Partial Public Class Formatting
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(FAHWatch7.Core.Formatting))
#Region "ppd formatting"
    Public Shared ReadOnly Property FormatPPD(ByVal PPD As String) As String
        Get
            Dim sSep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            If PPD.Contains(sSep) Then
                If PPD.Length - (PPD.IndexOf(sSep) + 1) < 2 Then
                    Return CStr(PPD & "0")
                Else
                    Return Math.Round(CDbl(PPD), 2).ToString
                End If
            Else
                Return CStr(PPD & ".00")
            End If
        End Get
    End Property
#End Region
#Region "Timespan formatting"
    Public Shared Function Format_ts(ts As TimeSpan, Optional ms As Boolean = False) As String
        Try
            Dim sb As New StringBuilder
            If ts.TotalHours > 0 Then
                If ts.Hours > 9 Then
                    sb.Append(ts.Hours & ":")
                Else
                    sb.Append("0" & ts.Hours.ToString & ":")
                End If
            End If
            If ts.Minutes > 0 Then
                If ts.Minutes > 9 Then
                    sb.Append(ts.Minutes.ToString & ":")
                Else
                    sb.Append("0" & ts.Minutes.ToString & ":")
                End If
            Else
                sb.Append("00:")
            End If
            If ts.Seconds > 0 Then
                If ts.Seconds > 9 Then
                    sb.Append(ts.Seconds.ToString & ":")
                Else
                    sb.Append("0" & ts.Seconds.ToString & ":")
                End If
            Else
                sb.Append("00:")
            End If
            If ms Then sb.Append(ts.Milliseconds.ToString)
            Return sb.ToString
        Catch ex As Exception
            log.ErrorFormat(ex.Message)
            Return "error"
        End Try
    End Function
#End Region
End Class





