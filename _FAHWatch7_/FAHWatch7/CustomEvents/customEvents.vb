'/*
' * FAHWatch7 Copyright Marvin Westmaas ( mtm )
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
Friend Class MyEventArgs
    Friend Shared ReadOnly Empty As EventArgs
    Friend Class BooleanArgs
        Inherits EventArgs
        Property Value As Boolean
        Sub New(Value As Boolean)
            Me.Value = Value
        End Sub
    End Class
    Friend Class ParseOnIntervalValueArgs
        Inherits EventArgs
        Property Interval As TimeSpan
        Sub New(ParseInterval As TimeSpan)
            Interval = ParseInterval
        End Sub
    End Class
    Friend Class ParseOnIntervalArgs
        Inherits EventArgs
        Property Enabled As Boolean
        Sub New(IsEnabled As Boolean)
            Enabled = IsEnabled
        End Sub
    End Class
    Friend Class ConvertFromUtcArgs
        Inherits EventArgs
        Property Convert As Boolean
        Sub New(ShouldConvert As Boolean)
            Convert = ShouldConvert
        End Sub
    End Class
    Friend Class HideInactiveMessageStripArgs
        Inherits EventArgs
        Property Hide As Boolean
        Sub New(ShouldHide As Boolean)
            Hide = ShouldHide
        End Sub
    End Class
    Friend Class EOCParseOnUpdateArgs
        Inherits EventArgs
        Property Enabled As Boolean
        Sub New(IsEnabled As Boolean)
            Enabled = IsEnabled
        End Sub
    End Class
    Friend Class EtaStyleArgs
        Inherits EventArgs
        Property Style As modMySettings.eEtaStyle
        Sub New(EtaStyle As modMySettings.eEtaStyle)
            Style = EtaStyle
        End Sub
    End Class
    Friend Class MessageStripEnabledArgs
        Inherits EventArgs
        Property Enabled As Boolean
        Property HideInactive As Boolean
        Sub New(IsEnabled As Boolean, HideWhenInactive As Boolean)
            Enabled = IsEnabled
            HideInactive = HideWhenInactive
        End Sub
    End Class
    Friend Class DefaultStatisticsArgs
        Inherits EventArgs
        Property DefaultStatistics As modMySettings.defaultStatisticsEnum
        Sub New(Statistics As modMySettings.defaultStatisticsEnum)
            DefaultStatistics = Statistics
        End Sub
    End Class
    Friend Class EOCEnabledChangedArgs
        Inherits EventArgs
        Property Enabled As Boolean
        Sub New(EnabledState As Boolean)
            Enabled = EnabledState
        End Sub
    End Class
    Friend Class MessageTimerElapsedArgs
        Inherits EventArgs
        Property MessageStrip As StatusStrip
        Sub New(sStrip As StatusStrip)
            MessageStrip = sStrip
        End Sub
    End Class
    Friend Class EocUpdateArgs
        Inherits EventArgs
        Property EOCAccount As clsEOC
        Sub New(Account As clsEOC)
            EOCAccount = Account
        End Sub
    End Class
    Friend Class EmptyArgs
        Inherits EventArgs
        Sub New()
        End Sub
    End Class
    Friend Class FailedEventArgs
        Inherits EventArgs
        Property Exception As Exception
        Property Message As String
        Sub New(Exc As Exception, Optional Message As String = Nothing)
            If IsNothing(Message) Then Message = Exception.Message
            Exc = Exception
        End Sub
    End Class
    Friend Class ParserFailedEventArgs
        Inherits EventArgs
        Property Exception As Exception
        Property IsNowDisabled As Boolean
        Property Clientname As String
    End Class
    Friend Class ParserCompletedEventArgs
        Inherits EventArgs
        Property Clientname As String
        Property HasActiveWU As Boolean
    End Class
    Friend Class PBrowserClosedArgs
        Inherits EventArgs
        Property IsRecreditted As Boolean
    End Class
    Friend Class PInfoUpdateComplete
        Inherits EventArgs
        Property HasNewContent As Boolean
        Property HasSpecificContent As Boolean
        Property SpecificProjectNumber As String
    End Class
    Friend Class ErrorEventArgs
        Inherits EventArgs
        Property Message As String
        Property Err As ErrObject
    End Class
    Friend Class ClearWarningEventArgs
        Inherits EventArgs
        Property Message As String
        Property IconRemoved As Boolean
    End Class
    Friend Class WarningEventArgs
        Inherits EventArgs
        Property Message As String
        Property ShowIcon As Boolean
        Property source As Object
        Property Exception As Exception
        Property Err As ErrObject
    End Class
End Class


