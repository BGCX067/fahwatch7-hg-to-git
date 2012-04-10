'/*
' * HWInfo interop module Copyright Marvin Westmaas ( mtm )
' *
' * This program is free software; you can redistribute it and/or
' * modify it under the terms of the GNU General Public License
' * as published by the Free Software Foundation; version 2
' * of the License. See the included file GPLv2.TXT.
' * 
' * This program is distributed in the hope that it will be useful,
' * but WITHOUT ANY WARRANTY; without even the implied warranty of
' * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' * GNU General Public License for more details.
' * 
' * You should have received a copy of the GNU General Public License
' * along with this program; if not, write to the Free Software
' * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
' */
'/*
Imports System.Runtime.InteropServices
Module modMain
    <DllImport("kernel32.dll")> _
    Public Function GetCurrentProcess() As IntPtr
    End Function
    <DllImport("advapi32.dll", SetLastError:=True)> _
    Public Function OpenProcessToken(ByVal ProcessHandle As IntPtr, ByVal DesiredAccess As UInt32, ByRef TokenHandle As IntPtr) As Boolean
    End Function
    <DllImport("advapi32.dll", SetLastError:=True)> _
    Public Function GetTokenInformation(ByVal TokenHandle As IntPtr, ByVal TokenInformationClass As TOKEN_INFO_CLASS, ByVal TokenInformation As IntPtr, ByVal TokenInformationLength As Integer, ByRef ReturnLength As UInteger) As Boolean
    End Function
    <DllImport("advapi32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Function DuplicateToken(ByVal ExistingTokenHandle As IntPtr, ByVal ImpersonationLevel As Security.Principal.TokenImpersonationLevel, <Out()> ByRef DuplicateTokenHandle As IntPtr) As Boolean
    End Function
    Public Enum TOKEN_ELEVATION_TYPE
        TokenElevationTypeDefault = 1
        TokenElevationTypeFull
        TokenElevationTypeLimited
    End Enum
    Public Enum TOKEN_INFO_CLASS
        TokenUser = 1
        TokenGroups
        TokenPrivileges
        TokenOwner
        TokenPrimaryGroup
        TokenDefaultDacl
        TokenSource
        TokenType
        TokenImpersonationLevel
        TokenStatistics
        TokenRestrictedSids
        TokenSessionId
        TokenGroupsAndPrivileges
        TokenSessionReference
        TokenSandBoxInert
        TokenAuditPolicy
        TokenOrigin
        TokenElevationType
        TokenLinkedToken
        TokenElevation
        TokenHasRestrictions
        TokenAccessInformation
        TokenVirtualizationAllowed
        TokenVirtualizationEnabled
        TokenIntegrityLevel
        TokenUIAccess
        TokenMandatoryPolicy
        TokenLogonSid
        MaxTokenInfoClass
        ' MaxTokenInfoClass should always be the last enum
    End Enum
    Public Const TOKEN_QUERY As UInt32 = &H8
    Public Const INT_SIZE As Integer = 4
End Module
