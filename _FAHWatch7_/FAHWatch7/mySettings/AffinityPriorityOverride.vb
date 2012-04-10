'/*
' * FAHWatch7 Affinity/Priority  Copyright Marvin Westmaas ( mtm )
' *
' * Copyright (c) 2010-2012 Marvin Westmaas ( MtM / Marvin_The_Martian )
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
Public Class AffinityPriorityOverride
    Private Shared mFrmAffinity As frmAffinity
    Friend Shared Sub ShowForm()
        Try
            If IsNothing(mFrmAffinity) OrElse mFrmAffinity.IsDisposed OrElse mFrmAffinity.Disposing Then
                mFrmAffinity = New frmAffinity
            End If
            Select Case mFrmAffinity.ShowDialog
                Case DialogResult.Cancel Or DialogResult.None

                Case DialogResult.OK

            End Select
        Catch ex As Exception
            WriteError(ex.Message, Err)
        Finally
            mFrmAffinity.Dispose()
        End Try
    End Sub
End Class
