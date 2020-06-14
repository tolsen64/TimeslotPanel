Imports System.Text.RegularExpressions

Public Class SchedItem

    Public Event Edit(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event Delete(sender As Object, e As TimeslotPanelDeleteEventArgs)
    Public Event Cut(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event Copy(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeMilesNotes(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)
    Public Event TimeShift(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event SchedItemDrop(sender As Object, e As DragEventArgs)
    Public Event SchedItemPaste(sender As Object, e As RoutedEventArgs)
    Public Event SchedItemDoubleClick(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)
    Public Event SchedItemCompletedRemotelyChanged(sender As Object, e As TimeSlotPanelCompletedRemotelyChangedEventArgs)
    Public Event SchedItemViewSO(sender As Object, e As TimeSlotPanelViewSOEventArgs)
    Public Event TransferRoutineTask(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event ToggleTravel(sender As Object, e As TimeslotPanelToggleTravelEventArgs)
    Public Event UnassignTask(sender As Object, e As TimeslotPanelUnassignTaskEventArgs)

    Public Property SysId As String
    Public Property TaskNum As String
    Public Property WorkType As String
    Public Property Status As String
    Public Property StartTime As DateTime
    Public Property Duration As TimeSpan
    Public Property BLDG As String
    Public Property Summary As String
    Public Property CI As String
    Public Property Application As String
    Public Property Location As String
    Public Property Description As String
    Public Property Notes As String
    Public Property SLA As String
    Public Property HasTravel As Boolean
    Public ReadOnly Property IsTravel As Boolean = WorkType = "Travel"
    Public Property Message As String

    ' These are not needed but keeping them just in case
#Region "Deprecated properties"
    Public Property DisableInteraction As Boolean = False
#End Region

    Public Sub New(SysId As String, TaskNum As String, WorkType As String, Status As String, StartTime As DateTime, Duration As TimeSpan, BLDG As String, Summary As String,
                    CI As String, Application As String, Location As String, Description As String, Notes As String, SLA As String, HasTravel As Boolean, Message As String)
        InitializeComponent()
        Me.SysId = SysId
        Me.TaskNum = TaskNum
        Me.WorkType = WorkType
        Me.Status = Status
        Me.StartTime = StartTime
        Me.Duration = Duration
        Me.BLDG = BLDG
        Me.Summary = Summary
        Me.CI = CI
        Me.Application = Application
        Me.Location = Location
        Me.Description = Description
        Me.Notes = Notes
        Me.SLA = SLA
        Me.HasTravel = HasTravel
        mnuToggleTravel.Header = If(HasTravel, "Delete Travel", "Add Travel")
        Me.Message = Message

        SetRtbTextDocument()

        Select Case WorkType
            Case "Travel" : Background = Brushes.PaleGreen
            Case "Self Generated" : Background = Brushes.SeaGreen
            Case "Break/Fix" : Background = Brushes.SandyBrown
            Case "Project" : Background = Brushes.MediumOrchid
            Case Else : Background = Brushes.AntiqueWhite
        End Select

        Dim ContrastColor As Brush = If(DirectCast(Background, SolidColorBrush) Is Brushes.White, Brushes.Blue, Brushes.White)
        'Dim cm As ContextMenu = Nothing
        Dim itm As MenuItem = Nothing

        AddHandler Me.MouseMove, AddressOf SchedItem_MouseMove
        AddHandler Me.MouseDoubleClick, AddressOf SchedItem_MouseDoubleClick
        rtbText.AllowDrop = True
        AddHandler rtbText.DragOver, AddressOf rtbText_DragOver
        AddHandler rtbText.PreviewDragOver, Sub(sender As Object, e As DragEventArgs) e.Handled = True
        AddHandler rtbText.Drop, Sub(sender As Object, e As DragEventArgs) RaiseEvent SchedItemDrop(sender, e)

        rtbText.Tag = StartTime.ToString
        'cm = CType(Me.Resources.Item("rtbContextMenu"), ContextMenu)
        'cm.Tag = StartTime.ToString("H:mm")
    End Sub

    Private Sub SetRtbTextDocument()
        Dim p As New Paragraph
        p.Inlines.Add(New Bold(New Run(TaskNum)))
        If WorkType IsNot Nothing AndAlso WorkType <> "" Then p.Inlines.Add(New Bold(New Run($" | {WorkType}")))
        If Message IsNot Nothing AndAlso Message <> "" Then p.Inlines.Add(New Run($" | {Message}"))
        If WorkType <> "Travel" Then
            If IsDate(SLA) Then p.Inlines.Add(New Bold(New Run($" | SLA:{SLA}")))
            If BLDG IsNot Nothing AndAlso BLDG <> "" Then p.Inlines.Add(New Bold(New Run($" | {BLDG}")))
            If Location IsNot Nothing AndAlso Location <> "" Then p.Inlines.Add(New Bold(New Run($" | {Location}")))
            If CI IsNot Nothing AndAlso CI <> "" Then p.Inlines.Add(New Bold(New Run($" | {CI}")))
            If Application IsNot Nothing AndAlso Application <> "" Then p.Inlines.Add(New Bold(New Run($" | {Application}")))
            If Summary IsNot Nothing AndAlso Summary <> "" Then p.Inlines.Add(New Bold(New Run($" | {Summary}")))

            If Description IsNot Nothing AndAlso Description <> "" Then p.Inlines.Add(New Run($"{vbCrLf}{vbCrLf}{Description}"))
            If Notes IsNot Nothing AndAlso Notes <> "" Then p.Inlines.Add(New Run($"{vbCrLf}{vbCrLf}{Notes}"))
        End If

        rtbText.Document = New FlowDocument(p) With {.PagePadding = New Thickness(0.0)}

    End Sub

    Public Sub SchedItem_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        'Dim ee As New TimeSlotPanelTimeMilesNoteEventArgs
        'ee.id = id
        'ee.SO = SO
        'ee.Status = Status
        'RaiseEvent SchedItemDoubleClick(Me, ee)
    End Sub

    Dim IsDragging As Boolean = False
    Private Sub SchedItem_MouseMove(sender As Object, e As MouseEventArgs)   'Handles Me.MouseMove
        If IsDragging OrElse DisableInteraction Then Exit Sub
        If StartTime.Date >= Now.Date Then
            If e.LeftButton = MouseButtonState.Pressed Then
                Dim timer1 As New Timers.Timer With {.Interval = 500, .AutoReset = False}   ' wait 1/2 second before initiating a drag.
                AddHandler timer1.Elapsed, AddressOf Timer1_Elapsed
                timer1.Start()
            End If
        End If
    End Sub

    Private Sub Timer1_Elapsed(sender As Object, e As EventArgs)
        Dim timer1 As Timers.Timer = CType(sender, Timers.Timer)
        timer1.Stop()
        timer1.Dispose()
        Dispatcher.InvokeAsync(Sub()
                                   If Mouse.LeftButton = MouseButtonState.Pressed Then
                                       Me.Visibility = Visibility.Collapsed
                                       IsDragging = True
                                       Try
                                           DragDrop.DoDragDrop(Me, Me, DragDropEffects.Move)
                                       Catch ex As Exception
                                           'Error HRESULT E_FAIL has been returned from a call to a COM component.
                                           'at BoycoT.SchedItem._Lambda$__162-0() in E:\LaptopSymlinks\Documents\Visual Studio 2017\Projects\BPG\Scheduling Analysis and Management\TimeslotPanel\TimeslotPanel\SchedItem.xaml.vb:line 211
                                           Debug.WriteLine("SchedItem::Timer1_Elapsed::" & ex.Message)
                                       End Try
                                       IsDragging = False
                                   End If
                               End Sub)
    End Sub

    Private Sub BuildContextMenu()
        'Dim cm As ContextMenu = CType(Resources("rtbContextMenu"), ContextMenu)
        'With New Regex("http(s*?)://.*?\s", RegexOptions.IgnoreCase)
        '    If .IsMatch(SODetails) Then
        '        cm.Items.Add(New Separator)
        '        Dim i As Integer = 1
        '        For Each m As Match In .Matches(SODetails)
        '            Dim mi As New MenuItem With {.Header = "Browse Link " & i.ToString, .Tag = m.Value, .ToolTip = m.Value}
        '            AddHandler mi.Click, AddressOf LinkClickedEventHandler
        '            cm.Items.Add(mi)
        '            i += 1
        '        Next
        '    End If
        'End With
    End Sub

    Private Sub LinkClickedEventHandler(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = CType(sender, MenuItem)
        Process.Start(mi.Tag.ToString)
    End Sub

    'Private Sub SchedItem_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseDoubleClick
    '    Debug.WriteLine("PreviewMouseDoubleClick")
    '    itmEditTask_Click(Nothing, Nothing)
    '    e.Handled = True
    'End Sub

    'Private mPos As Point
    'Private Sub SchedItem_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
    '    mPos = Mouse.GetPosition(Nothing)
    'End Sub

    'Private Function IsReallyMouseMove() As Boolean
    '    Dim pPos As Point = Mouse.GetPosition(Nothing)
    '    Debug.WriteLine(Math.Abs(mPos.X - pPos.X) & ", " & Math.Abs(mPos.Y - pPos.Y))
    '    IsReallyMouseMove = Math.Abs(mPos.X - pPos.X) > 10 OrElse Math.Abs(mPos.Y - pPos.Y) > 10
    'End Function

    Private Sub itmViewTaskInSMC_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent SchedItemViewSO(Me, New TimeSlotPanelViewSOEventArgs With {.TaskNum = TaskNum})
    End Sub

    Private Sub itmCopy_Click(sender As Object, e As RoutedEventArgs)
        'Dim args As New TimeslotPanelCutCopyEditEventArgs
        'With args
        '    .id = id
        '    .UnitOfWork = UnitOfWork
        '    .Duration = Duration
        '    .SchedDateTime = SchedDateTime
        '    .SO = SO
        '    .DueDate = If(IsDate(DueDate), CDate(DueDate), Nothing)
        '    .SODetails = SODetails
        '    .IntExt = IntExt
        '    .Shipper = Shipper
        '    .Title = Title
        '    .BLDG = BLDG
        '    .Miles = Miles
        '    .IsLocked = IsLocked
        '    .Action = "Copy"
        'End With
        'RaiseEvent Copy(Me, args)
    End Sub

    Private Sub itmPaste_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent SchedItemPaste(sender, e)
    End Sub

    Private Sub itmCut_Click(sender As Object, e As RoutedEventArgs)
        'Dim args As New TimeslotPanelCutCopyEditEventArgs
        'With args
        '    .id = id
        '    .UnitOfWork = UnitOfWork
        '    .Duration = Duration
        '    .SchedDateTime = SchedDateTime
        '    .DueDate = If(IsDate(DueDate), CDate(DueDate), Nothing)
        '    .SO = SO
        '    .SODetails = SODetails
        '    .IntExt = IntExt
        '    .Shipper = Shipper
        '    .Title = Title
        '    .BLDG = BLDG
        '    .Miles = Miles
        '    .IsLocked = IsLocked
        '    .Action = "Cut"
        'End With
        'RaiseEvent Cut(Me, args)
    End Sub

    Private Sub itmCopyTaskNumber_Click(sender As Object, e As RoutedEventArgs)
        Try
            Clipboard.SetText(TaskNum)
        Catch ex As Exception
            MsgBox("Unable to copy the Task Number to the clipboard.  Try running the following command from a command prompt and try again:" & vbCrLf & vbCrLf & "C:\Windows\System32\cmd.exe /c ""echo off | clip""", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub itmUnassignTask_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent UnassignTask(Me, New TimeslotPanelUnassignTaskEventArgs With {.SysId = SysId})
    End Sub

    Private Sub itmToggleTravel_Click(sender As Object, e As RoutedEventArgs)
        Dim mi As MenuItem = CType(sender, MenuItem)
        RaiseEvent ToggleTravel(Me, New TimeslotPanelToggleTravelEventArgs With {.SysId = SysId, .ToggleAction = mi.Header.ToString.Replace("Travel", "").Trim})
    End Sub

    Private Sub itmDeleteTask_Click(sender As Object, e As RoutedEventArgs)
        'RaiseEvent Delete(Me, New TimeslotPanelDeleteEventArgs With {.id = id, .SO = SO})
    End Sub

    Private Sub itmTimeMilesNote_Click(sender As Object, e As RoutedEventArgs)
        'RaiseEvent TimeMilesNotes(Me, New TimeSlotPanelTimeMilesNoteEventArgs With {.id = id, .SO = SO, .Status = Status})
    End Sub

    Public Function GetStartRow(SchedDateTime As Date) As Integer
        Return CInt(SchedDateTime.TimeOfDay.TotalMinutes / 15)
        'Row = CInt(SchedDateTime.TimeOfDay.TotalMinutes / 15)
        'Return Row
    End Function

    Public Function GetRowSpan(Duration As TimeSpan) As Integer
        Return CInt(Duration.TotalMinutes / 15)
    End Function

    Private Sub Image_MouseUp(sender As Object, e As MouseButtonEventArgs)
        itmViewTaskInSMC_Click(Nothing, Nothing)
    End Sub

    Private Sub imgUpArrow_MouseDown(sender As Object, e As MouseButtonEventArgs)
        rtbText.LineUp()
    End Sub

    Private Sub imgDownArrow_MouseDown(sender As Object, e As MouseButtonEventArgs)
        rtbText.LineDown()
    End Sub

    Private Sub chkSelect_Click(sender As Object, e As RoutedEventArgs)
        'IsChecked = CBool(chkSelect.IsChecked)
    End Sub

    Private Sub rtbText_DragOver(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(GetType(System.Data.DataRowView)) OrElse e.Data.GetDataPresent(GetType(SchedItem)) Then
            e.Effects = DragDropEffects.Move
        Else
            e.Effects = DragDropEffects.None
            e.Handled = True
        End If
    End Sub

    Public Async Sub Flash()
        Await Task.Run(Sub()
                           For i As Integer = 1 To 4
                               Dispatcher.BeginInvoke(Sub() Visibility = Visibility.Hidden)
                               Threading.Thread.Sleep(250)
                               Dispatcher.BeginInvoke(Sub() Visibility = Visibility.Visible)
                               If i < 4 Then Threading.Thread.Sleep(250)
                           Next
                       End Sub)
    End Sub

    Private Sub itmCopy_Click_1(sender As Object, e As RoutedEventArgs)
        rtbText.Copy()
    End Sub


    Private Sub itmSelectAll_Click(sender As Object, e As RoutedEventArgs)
        rtbText.SelectAll()
    End Sub

    Private Sub itmViewSO_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent SchedItemViewSO(Me, New TimeSlotPanelViewSOEventArgs With {.TaskNum = TaskNum})
    End Sub

    Private Sub itmTransferRoutineTask_Click(sender As Object, e As RoutedEventArgs)
        'RaiseEvent TransferRoutineTask(Me, New TimeslotPanelCutCopyEditEventArgs With {.id = id, .SchedDateTime = SchedDateTime, .SO = SO, .Title = Title})
    End Sub

    Private Sub ItmSetCompletedRemotelyStatus_Click(sender As Object, e As RoutedEventArgs)
        ' Debug.WriteLine("ItmSetCompletedRemotelyStatus_Click(" & SO & ")")
        ' RaiseEvent SchedItemCompletedRemotelyChanged(Me, New TimeSlotPanelCompletedRemotelyChangedEventArgs With {.SO = SO})
    End Sub

    Private Sub itmOnsiteVisitRequest_Click(sender As Object, e As RoutedEventArgs)
        'Process.Start($"https://thor.inside.ups.com/THOR2/RemoteSR.aspx?SO={SO}")
    End Sub

End Class
