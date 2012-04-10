Partial Public Class Definitions
    <Serializable()> _
    Public Class pSummary
        Implements IEquatable(Of pSummary)
        Public ProjectNumber As String, ServerIP As String, WUName As String, NumberOfAtoms As String, PreferredDays As String, FinalDeadline As String, Credit As String, Frames As String, Code As String, Description As String, Contact As String, kFactor As String, dtSummary As DateTime = DateTime.MinValue, dtUpdated As DateTime = DateTime.MinValue
        Public Function Equals1(other As pSummary) As Boolean Implements System.IEquatable(Of pSummary).Equals
            If Credit <> other.Credit OrElse kFactor <> other.kFactor OrElse PreferredDays <> other.PreferredDays OrElse FinalDeadline <> other.FinalDeadline OrElse Frames <> other.Frames OrElse Code <> other.Code OrElse Description <> other.Description OrElse Contact <> other.Contact Then
                Return False
            Else
                Return True
            End If
        End Function
        ''' <summary>
        ''' 'Update the pSummary object to match another
        ''' </summary>
        ''' <param name="Other">The object to match</param>
        ''' <remarks></remarks>
        Public Sub MakeEqual(Other As pSummary)
            With Other
                'ProjectNumber = .ProjectNumber  not needed
                ServerIP = .ServerIP
                WUName = .WUName
                NumberOfAtoms = .NumberOfAtoms
                PreferredDays = .PreferredDays
                FinalDeadline = .FinalDeadline
                Credit = .Credit
                Frames = .Frames
                Code = .Code
                Description = .Description
                Contact = .Contact
                kFactor = .kFactor
                'dtSummary = .dtSummary
                'dtUpdated = .dtUpdated
            End With
        End Sub
        ''' <summary>
        ''' 'Populate pSummary object based on dictionary
        ''' </summary>
        ''' <param name="dict">Dictionary with pSummary definitions</param>
        ''' <returns>Boolean indicating success</returns>
        ''' <remarks></remarks>
        Public Function Populate(dict As Dictionary(Of String, String)) As Boolean
            Try
                ProjectNumber = dict("ProjectNumber")
                ServerIP = dict("ServerIP")
                WUName = dict("WUName")
                NumberOfAtoms = dict("NumberOfAtoms")
                PreferredDays = dict("PreferredDays")
                FinalDeadline = dict("FinalDeadline")
                Credit = dict("Credit")
                Frames = dict("Frames")
                Code = dict("Code")
                Description = dict("Description")
                Contact = dict("Contact")
                kFactor = dict("kFactor")
                If String.IsNullOrEmpty(dict("dtSummary")) Then
                    dtSummary = DateTime.MinValue
                Else
                    dtSummary = CDate(dict("dtSummary"))
                End If
                If String.IsNullOrEmpty(dict("dtUpdate")) Then
                    dtUpdated = DateTime.MinValue
                Else
                    dtUpdated = CDate(dict("dtUpdate"))
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
        ''' <summary>
        ''' Converts the pSummary object to dictionary
        ''' </summary>
        ''' <returns>String dictionary with psummary values</returns>
        ''' <remarks></remarks>
        Public Function toDictionary() As Dictionary(Of String, String)
            Dim rVal As New Dictionary(Of String, String)
            rVal.Add("ProjectNumber", ProjectNumber)
            rVal.Add("WUName", WUName)
            rVal.Add("ServerIP", ServerIP)
            rVal.Add("NumberOfAtoms", NumberOfAtoms)
            rVal.Add("PreferredDays", PreferredDays)
            rVal.Add("FinalDeadline", FinalDeadline)
            rVal.Add("Credit", Credit)
            rVal.Add("Frames", Frames)
            rVal.Add("Code", Code)
            rVal.Add("Description", Description)
            rVal.Add("Contact", Contact)
            rVal.Add("kFactor", kFactor)
            rVal.Add("dtSummary", dtSummary.ToString("s"))
            If dtUpdated <> DateTime.MinValue Then
                rVal.Add("dtUpdated", dtUpdated.ToString("s"))
            Else
                rVal.Add("dtUpdated", String.Empty)
            End If
            Return rVal
        End Function
    End Class
End Class

