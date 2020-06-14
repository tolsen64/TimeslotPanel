Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives

Public Class ResizeThumb
    Inherits Thumb

    Public Event ItemResizeCompleted(sender As Object, e As TimeSlotPanelItemResizeEventArgs)
    Public Event ItemResizeDragDelta(sender As Object, e As TimeSlotPanelItemResizeDragDeltaEventArgs)

    Private LastDelta As Integer = 0

    ' for the two methods below, the grid's Tag contains: <id of scheduled item>,<TotalMinutes of scheduled item>

    Private Sub ResizeThumb_DragDelta(sender As Object, e As DragDeltaEventArgs) Handles Me.DragDelta
        Dim rs As ResizeThumb = CType(sender, ResizeThumb)
        Dim gd As Grid = CType(rs.Parent, Grid)
        Dim mins As Integer = CInt(gd.Tag.split(","c)(1))
        LastDelta = Convert.ToInt32(e.VerticalChange)    'To compensate for differences between DragDelta and DragCompleted.

        RaiseEvent ItemResizeDragDelta(sender, New TimeSlotPanelItemResizeDragDeltaEventArgs With {.CurrentMinutes = mins, .VerticalChange = LastDelta})
        e.Handled = True
    End Sub

    Private Sub ItemResized(sender As Object, e As DragCompletedEventArgs) Handles Me.DragCompleted
        Dim rs As ResizeThumb = CType(sender, ResizeThumb)
        Dim gd As Grid = CType(rs.Parent, Grid)
        Dim tag As String() = gd.Tag.ToString.Split(","c)
        'RaiseEvent ItemResizeCompleted(Me, New TimeSlotPanelItemResizeEventArgs With {.sys_id = CStr(tag(0)), .CurrentMinutes = CInt(tag(1)), .VerticalChange = LastDelta})
        RaiseEvent ItemResizeCompleted(Me, New TimeSlotPanelItemResizeEventArgs With {.sys_id = tag(0), .IsTravelTask = tag(2) = "Travel"})
        e.Handled = True
    End Sub
End Class