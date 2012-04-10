'   FAHWatch7 ListViewColumnSorter
'
'   Copyright (c) 2011 Marvin Westmaas ( MtM / Marvin_The_Martian )
'
'   This program is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.
'
'   This program is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.
'
'   You should have received a copy of the GNU General Public License
'   along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
'   This class is based on the msdn example -> http://support.microsoft.com/kb/319401
'   mixed with the numerical and datetime sorting from -> http://www.vb-helper.com/howto_net_listview_sort_clicked_column.html

Imports System.Collections
Imports System.Windows.Forms
''' <summary>
''' This class is an implementation of the 'IComparer' interface.
''' </summary>
Public Class ListViewColumnSorter
    Implements IComparer
    ''' <summary>
    ''' Specifies the column to be sorted
    ''' </summary>
    Private ColumnToSort As Integer
    ''' <summary>
    ''' Specifies the order in which to sort (i.e. 'Ascending').
    ''' </summary>
    Private OrderOfSort As SortOrder
    ''' <summary>
    ''' Case insensitive comparer object
    ''' </summary>
    'Private ObjectCompare As CaseInsensitiveComparer
    ''' <summary>
    ''' Class constructor.  Initializes various elements
    ''' </summary>
    Public Sub New()
        ' Initialize the column to '0'
        ColumnToSort = 0

        ' Initialize the sort order to 'none'
        OrderOfSort = SortOrder.None

        ' Initialize the CaseInsensitiveComparer object
        'ObjectCompare = New CaseInsensitiveComparer()
    End Sub

    ''' <summary>
    ''' This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    ''' </summary>
    ''' <param name="x">First object to be compared</param>
    ''' <param name="y">Second object to be compared</param>
    ''' <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        Dim item_x As ListViewItem = DirectCast(x, ListViewItem)
        Dim item_y As ListViewItem = DirectCast(y, ListViewItem)

        ' Get the sub-item values.
        Dim string_x As String
        If item_x.SubItems.Count <= ColumnToSort Then
            string_x = ""
        Else
            string_x = item_x.SubItems(ColumnToSort).Text
        End If

        Dim string_y As String
        If item_y.SubItems.Count <= ColumnToSort Then
            string_y = ""
        Else
            string_y = item_y.SubItems(ColumnToSort).Text
        End If

        ' Compare them.
        If OrderOfSort = SortOrder.Ascending Then
            If IsNumeric(string_x) And IsNumeric(string_y) Then
                Return Val(string_x).CompareTo(Val(string_y))
            ElseIf IsDate(string_x) And IsDate(string_y) Then
                Return DateTime.Parse(string_x).CompareTo(DateTime.Parse(string_y))
            Else
                Return String.Compare(string_x, string_y)
            End If
        Else
            If IsNumeric(string_x) And IsNumeric(string_y) Then
                Return Val(string_y).CompareTo(Val(string_x))
            ElseIf IsDate(string_x) And IsDate(string_y) Then
                Return DateTime.Parse(string_y).CompareTo(DateTime.Parse(string_x))
            Else
                Return String.Compare(string_y, string_x)
            End If
        End If
    End Function

    ''' <summary>
    ''' Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
    ''' </summary>
    Public Property SortColumn() As Integer
        Get
            Return ColumnToSort
        End Get
        Set(value As Integer)
            ColumnToSort = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
    ''' </summary>
    Public Property Order() As SortOrder
        Get
            Return OrderOfSort
        End Get
        Set(value As SortOrder)
            OrderOfSort = value
        End Set
    End Property

End Class
