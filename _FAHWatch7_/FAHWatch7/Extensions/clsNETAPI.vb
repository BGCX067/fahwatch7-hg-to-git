' written by richard_deeming and posted -> http://forums.devx.com/archive/index.php/t-145593.html
Imports System
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public NotInheritable Class NetworkInformation
    Friend Enum JoinStatus
        Unknown = 0
        UnJoined = 1
        Workgroup = 2
        Domain = 3
    End Enum
    Private Shared _local As New NetworkInformation()
    Private _computerName As String
    Private _domainName As String
    Private _status As JoinStatus = JoinStatus.Unknown
    Friend Sub New(ByVal computerName As String)
        If String.IsNullOrEmpty(computerName) Then
            Throw New ArgumentNullException("computerName")
        End If
        _computerName = computerName
        LoadInformation()
    End Sub

    Private Sub New()
        LoadInformation()
    End Sub

    Friend Shared ReadOnly Property LocalComputer As NetworkInformation
        Get
            Return _local
        End Get
    End Property

    Friend ReadOnly Property ComputerName As String
        Get
            If _computerName Is Nothing Then Return "(local)"
            Return _computerName
        End Get
    End Property

    Friend ReadOnly Property DomainName As String
        Get
            Return _domainName
        End Get
    End Property

    Friend ReadOnly Property Status As JoinStatus
        Get
            Return _status
        End Get
    End Property

    Private Sub LoadInformation()
        Dim pBuffer As IntPtr = IntPtr.Zero
        Dim status As JoinStatus
        Try
            Dim result As Integer = NativeMethods.NetGetJoinInformation(_computerName, pBuffer, status)
            If 0 <> result Then Throw New Win32Exception()
            _status = status
            _domainName = Marshal.PtrToStringUni(pBuffer)
        Finally
            If Not IntPtr.Zero.Equals(pBuffer) Then
                NativeMethods.NetApiBufferFree(pBuffer)
            End If
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Select Case _status
            Case JoinStatus.Domain
                Return ComputerName & " is a member of the domain " & DomainName
            Case JoinStatus.Workgroup
                Return ComputerName & " is a member of the workgroup " & DomainName
            Case JoinStatus.UnJoined
                Return ComputerName & " is a standalone computer"
            Case Else
                Return "Unable to determine the network status of " & ComputerName
        End Select
    End Function
End Class