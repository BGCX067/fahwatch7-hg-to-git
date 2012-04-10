Module Module1
    Sub Main()
        If IntPtr.Size = 8 Then
            Console.WriteLine("X64")
        Else
            Console.WriteLine("X86")
        End If
    End Sub

End Module
