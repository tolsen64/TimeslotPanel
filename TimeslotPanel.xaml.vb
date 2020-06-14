Imports System.Data

Public Class TimeSlotPanel

    Public Event TimeslotPanelDateChanged(sender As Object, e As TimeslotPanelDateChangedEventArgs)
    Private EventEnabled As Boolean = False

    Public Event TimeslotPanelCreateEntry(sender As Object, e As TimeslotPanelCreateEntryEventArgs)
    Public Event TimeSlotPanelSchedSVCPDT(sender As Object, e As TimeslotPanelCreateEntryEventArgs)
    Public Event TimeslotPanelDeleteEntry(sender As Object, e As TimeslotPanelDeleteEventArgs)
    Public Event TimeslotPanelEditEntry(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeslotPanelDropFromGrid(sender As Object, e As TimeslotPanelDropFromGridEventArgs)
    Public Event TimeslotPanelDropFromCal(sender As Object, e As TimeslotPanelDropFromCalEventArgs)
    Public Event TimeslotPanelCutEntry(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeslotPanelCopyEntry(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeslotPanelPasteEntry(sender As Object, e As TimeSlotPanelPasteEventArgs)
    Public Event TimeslotPanelTimeMilesNotes(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)
    Public Event TimeslotPanelItemResizeEvent(sender As Object, e As TimeSlotPanelItemResizeEventArgs)
    Public Event TimeslotPanelTimeShiftEvent(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeslotPanelCompletedRemotelyChanged(sender As Object, e As TimeSlotPanelCompletedRemotelyChangedEventArgs)
    Public Event TimeslotPanelViewSO(sender As Object, e As TimeSlotPanelViewSOEventArgs)
    Public Event TimeslotPanelMultiDeleteEntry(sender As Object, e As List(Of TimeslotPanelDeleteEventArgs))
    Public Event TimeslotPanelMultiCutEntry(sender As Object, e As List(Of TimeslotPanelCutCopyEditEventArgs))
    Public Event TimeslotPanelMultiCopyEntry(sender As Object, e As List(Of TimeslotPanelCutCopyEditEventArgs))
    Public Event TimeslotPanelMultiTimeShiftEvent(sender As Object, e As List(Of TimeslotPanelCutCopyEditEventArgs))
    Public Event TimeslotPanelVacationDay(sender As Object, e As List(Of TimeslotPanelCutCopyEditEventArgs))
    Public Event TimeslotPanelDiscretionaryDay(sender As Object, e As Object)
    Public Event TimeslotPanelReload(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelTransferRoutineTask(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
    Public Event TimeslotPanelSchedItemDoubleClick(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)

    Public Event TimeslotPanelNeedTechInfo(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelSendSchedToSupervisor(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelApproveTechSchedule(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelRejectTechSchedule(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelViewScheduleNotifications(sender As Object, e As RoutedEventArgs)
    Public Event TimeslotPanelViewRoutineTaskNotifications(sender As Object, e As RoutedEventArgs)

    Public Event TimeslotPanelToggleTravel(sender As Object, e As TimeslotPanelToggleTravelEventArgs)
    Public Event TimeslotPanelUnassignTask(sender As Object, e As TimeslotPanelUnassignTaskEventArgs)

    'Private lstSchedItems As New List(Of SchedItem)
    Private lstSchedItems As New List(Of ContentControl)

    Public Property StartTime As Date = CDate("08:00")
    Public Property EndTime As Date = CDate("17:00").AddMinutes(-15)
    Public Property drTech As DataRow = Nothing

    Public ReadOnly Property ScheduledItems As SchedItem()
        Get
            Dim si As New List(Of SchedItem)
            For Each cc As ContentControl In lstSchedItems
                si.Add(CType(cc.Content, SchedItem))
            Next
            Return si.ToArray
        End Get
    End Property

    Private IsNew As Boolean = True

    Public Sub SetNotificationIcon(IsVisible As Boolean)
        SetVisible(btnSchedApprovalStatus, IsVisible)
    End Sub

    Public Sub SetRoutineTaskNotificationIcon(IsVisible As Boolean)
        SetVisible(btnRoutineTaskApprovalStatus, IsVisible)
    End Sub

    Public Sub SetScheduleIcons(IsSchedSaved As Boolean, IsSubmitted As Boolean, IsApproved As Boolean, IsRejected As Boolean, IsSupervisor As Boolean)
        SetVisible(btnApproveTechSchedule, False)
        SetVisible(btnRejectTechSchedule, False)
        SetVisible(btnSendSchedToSupervisor, False)
        SetVisible(imgLocked, False)
        SetVisible(imgSubmitted, False)
        SetVisible(imgApproved, IsApproved)

        'MsgBox("IsSchedSaved: " & IsSchedSaved & vbCrLf &
        '       "IsSubmitted: " & IsSubmitted & vbCrLf &
        '       "IsApproved: " & IsApproved & vbCrLf &
        '       "IsRejected: " & IsRejected & vbCrLf &
        '       "IsSupervisor: " & IsSupervisor)

        If dpPickDate.SelectedDate.Value.Date > DateTime.Now.Date Then
            Debug.WriteLine("In1")
            If IsSupervisor Then
                Debug.WriteLine("In2")
                SetVisible(btnApproveTechSchedule, True)
                SetVisible(btnRejectTechSchedule, True)
                If IsRejected Then
                    Debug.WriteLine("In3")
                    'SetVisible(btnApproveTechSchedule, True)
                ElseIf IsApproved Then
                    Debug.WriteLine("In4")
                    'SetVisible(btnRejectTechSchedule, True)
                ElseIf IsSubmitted Then
                    Debug.WriteLine("In5")
                    SetVisible(imgSubmitted, True)
                    'SetVisible(btnApproveTechSchedule, Not IsApproved)
                    'SetVisible(btnRejectTechSchedule, Not IsRejected)
                End If
            Else
                Debug.WriteLine("In6")
                If IsApproved Then
                    SetVisible(imgApproved, True)
                ElseIf IsSubmitted Or IsSchedSaved Then
                    Debug.WriteLine("In7")
                Else
                    Debug.WriteLine("In8")
                    SetVisible(btnSendSchedToSupervisor, True)
                End If
            End If
        End If
    End Sub

    Private Sub SetVisible(ctl As Object, value As Boolean)
        ctl.GetType().GetProperty("Visibility").SetValue(ctl, If(value, Visibility.Visible, Visibility.Collapsed))
        'ctl.Visibility = If(value, Visibility.Visible, Visibility.Collapsed)
    End Sub

    Public Sub New()
        InitializeComponent()
        Debug.WriteLine("TimeslotPanel.New()")
        dpPickDate.SelectedDate = DateTime.Now.Date
        EventEnabled = True
        ThrowEvent()
    End Sub

    Private TimeClicked As String = ""

    Public Sub InitSchedView()
        Dim tm As Date = New Date(1, 1, 1, 0, 0, 0)
        lstSchedItems.Clear()
        gridItems.Children.Clear()
        If drTech Is Nothing Then RaiseEvent TimeslotPanelNeedTechInfo(Me, New RoutedEventArgs)
        Dim dow As String = SelectedDate.DayOfWeek.ToString.Substring(0, 3)
        Dim IsWorkDay As Boolean = If(drTech Is Nothing OrElse Convert.IsDBNull(drTech(dow)), False, CBool(drTech(dow)))
        Dim oddBrush As Brush = Brushes.LightYellow
        Dim evenBrush As Brush = Brushes.Linen
        Dim currentBrush As Brush = oddBrush

        For i As Integer = 0 To 95
            If tm.Minute = 0 Then
                If currentBrush Is oddBrush Then
                    currentBrush = evenBrush
                Else
                    currentBrush = oddBrush
                End If
            End If

            'Military time = "H:mm"
            '12 hour time = "t"

            Dim tb As New TextBlock() With {.Background = Brushes.Gainsboro, .Height = 17, .Padding = New Thickness(0, 0, 2, 0), .TextAlignment = TextAlignment.Right, .Text = tm.ToString("t"), .Name = "tb" & tm.ToString("Hmm")}
            Grid.SetColumn(tb, 0)
            Grid.SetRow(tb, i)
            gridItems.Children.Add(tb)

            Dim tbTime As Date = CDate(tb.Text)

            Dim rect As New Rectangle With {.Stroke = Brushes.LightGray, .Fill = Brushes.Transparent}

            rect.AllowDrop = True
            AddHandler rect.DragOver, AddressOf rect_DragOver
            AddHandler rect.Drop, AddressOf rect_Drop
            rect.Tag = tb.Text
            Grid.SetColumn(rect, 0)
            Grid.SetRow(rect, i)
            gridItems.Children.Add(rect)

            rect = New Rectangle With {.Stroke = Brushes.LightGray, .Fill = Brushes.Gainsboro, .Tag = tm.ToString("H:mm")}
            'If dpPickDate.SelectedDate.Value.Date >= DateTime.Now.Date Then rect.ContextMenu = CType(Me.Resources.Item("mnuCreateEntry"), ContextMenu)
            'AddHandler rect.PreviewMouseRightButtonDown, Sub(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)
            '                                                 CType(Me.Resources.Item("mnuCreateEntry"), ContextMenu).Tag = CType(sender, Rectangle).Tag
            '                                             End Sub
            rect.AllowDrop = True
            AddHandler rect.DragOver, AddressOf rect_DragOver
            AddHandler rect.Drop, AddressOf rect_Drop
            rect.Tag = tb.Text

            Grid.SetColumn(rect, 1)
            Grid.SetRow(rect, i)
            gridItems.Children.Add(rect)

            If IsWorkDay Then
                If StartTime.TimeOfDay < EndTime.TimeOfDay Then
                    If tbTime.TimeOfDay >= StartTime.TimeOfDay AndAlso tbTime.TimeOfDay <= EndTime.TimeOfDay Then
                        rect.Fill = currentBrush
                        tb.Background = currentBrush
                    End If
                Else
                    If tbTime.TimeOfDay <= EndTime.TimeOfDay OrElse tbTime.TimeOfDay >= StartTime.TimeOfDay Then
                        rect.Fill = currentBrush
                        tb.Background = currentBrush
                    End If
                End If
            End If

            tm = tm.AddMinutes(15)
        Next

    End Sub

    Public Sub ScrollToStartTime()
        Dim tm As Date = New Date(1, 1, 1, 0, 0, 0)
        Dim scrollTo As Double = 0
        For i As Integer = 0 To 95
            If StartTime.Hour = tm.Hour AndAlso StartTime.Minute = tm.Minute Then
                scrollTo = i
                Exit For
            End If
            tm = tm.AddMinutes(15)
        Next
        scrollView.ScrollToVerticalOffset((scrollView.ExtentHeight / 96) * scrollTo)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        EventEnabled = False
        Dim txt As String = CType(sender, Button).Content.ToString
        Select Case txt
            Case "<" : dpPickDate.SelectedDate = dpPickDate.SelectedDate.Value.AddDays(-1) : InitSchedView()
            Case ">" : dpPickDate.SelectedDate = dpPickDate.SelectedDate.Value.AddDays(1) : InitSchedView()
        End Select
        EventEnabled = True
        ThrowEvent()
    End Sub

    Private Sub dpPickDate_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        InitSchedView()
        ThrowEvent()
    End Sub

    Private Sub ThrowEvent()
        Debug.WriteLine($"EventEnabled={EventEnabled}")
        If EventEnabled Then
            Debug.WriteLine("Throwing Event!")
            RaiseEvent TimeslotPanelDateChanged(Me, New TimeslotPanelDateChangedEventArgs With {.tspDate = CDate(dpPickDate.SelectedDate)})
        End If
    End Sub

    Public Sub AddScheduledItem(SysId As String, TaskNum As String, WorkType As String, Status As String, StartTime As DateTime, Duration As TimeSpan, BLDG As String, Summary As String,
                                CI As String, Application As String, Location As String, Description As String, Notes As String, SLA As String, HasTravel As Boolean, Message As String)
        If Duration.TotalMinutes = 0 Then
            MsgBox("This task has a zero duration and cannot be displayed." & vbCrLf & vbCrLf &
                   "SysId: " & SysId & vbCrLf &
                   "TaskNum: " & TaskNum & vbCrLf &
                   "BLDG: " & BLDG & vbCrLf &
                   "Summary: " & Summary & vbCrLf &
                   "Description: " & Description & vbCrLf &
                   "StartTime: " & StartTime.ToString & vbCrLf &
                   "Duration: " & Duration.ToString & vbCrLf & vbCrLf &
                   "Please take a screenshot of this message and email it to ustsg@ups.com", MsgBoxStyle.OkOnly, "Active Schedule"
                   )
            Exit Sub
        End If

        Debug.WriteLine($"TaskNum: {TaskNum}, HasTravel: {HasTravel}")

        Dim si As New SchedItem(SysId, TaskNum, WorkType, Status, StartTime, Duration, BLDG, Summary, CI, Application, Location, Description, Notes, SLA, HasTravel, Message)

        AddHandler si.Delete, AddressOf SchedItemDeleteEventHandler
        AddHandler si.Edit, AddressOf SchedItemEditEventHandler
        AddHandler si.Cut, AddressOf SchedItemCutEventHandler
        AddHandler si.Copy, AddressOf SchedItemCopyEventHandler
        AddHandler si.TimeMilesNotes, AddressOf SchedItemTimeMilesNoteEventHandler
        AddHandler si.TimeShift, AddressOf TimeShiftEventHandler
        AddHandler si.SchedItemDrop, AddressOf rect_Drop
        AddHandler si.SchedItemPaste, AddressOf MenuItemPaste_Click
        AddHandler si.SchedItemDoubleClick, AddressOf SchedItemDoubleClickEventHandler
        AddHandler si.SchedItemCompletedRemotelyChanged, AddressOf SchedItemCompletedRemotelyChangedHandler
        AddHandler si.SchedItemViewSO, AddressOf SchedItemViewSOEventHandler
        AddHandler si.TransferRoutineTask, AddressOf TransferRoutineTaskEventHandler
        AddHandler si.ToggleTravel, AddressOf ToggleTravelEventHandler
        AddHandler si.UnassignTask, AddressOf UnassignTaskEventHandler

        'AddHandler si.MouseMove, AddressOf si_MouseMove

        Dim cc As ContentControl = New ContentControl
        cc.Tag = $"{SysId},{Duration.TotalMinutes},{WorkType}"  'using the tag to send info to the events for calculations.
        cc.Content = si
        cc.Template = CType(FindResource("DesignerItemTemplate"), ControlTemplate) 'This allows the user to expand/contract the scheduled item using the drag bar at the bottom of the scheduled item.
        Grid.SetColumn(cc, 1)
        Grid.SetRow(cc, GetStartRow(StartTime))
        Grid.SetRowSpan(cc, si.GetRowSpan(Duration))

        lstSchedItems.Add(cc)
        gridItems.Children.Add(cc)
    End Sub

    Private Function GetStartRow(StartTime As Date) As Integer
        Return CInt(StartTime.TimeOfDay.TotalMinutes / 15)
    End Function

    Private Function GetRowSpan(Duration As Integer) As Integer
        Return CInt(Duration / 15)
    End Function

    Private Sub TransferRoutineTaskEventHandler(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
        RaiseEvent TimeslotPanelTransferRoutineTask(Me, e)
    End Sub

    Private Sub ToggleTravelEventHandler(sender As Object, e As TimeslotPanelToggleTravelEventArgs)
        RaiseEvent TimeslotPanelToggleTravel(Me, e)
    End Sub

    Private Sub UnassignTaskEventHandler(sender As Object, e As TimeslotPanelUnassignTaskEventArgs)
        RaiseEvent TimeslotPanelUnassignTask(Me, e)
    End Sub

    Private Sub SchedItemDeleteEventHandler(sender As Object, e As TimeslotPanelDeleteEventArgs)
        'If Not CType(sender, SchedItem).IsChecked Then
        '    If MessageBox.Show("Are you sure you want to unschedule " & CType(sender, SchedItem).SO & "?", Nothing, MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
        '        Dim cc As ContentControl = CType(CType(sender, SchedItem).Parent, ContentControl)
        '        For i As Integer = lstSchedItems.Count - 1 To 0 Step -1
        '            If lstSchedItems(i) Is cc Then
        '                lstSchedItems(i) = Nothing
        '                lstSchedItems.RemoveAt(i)
        '            End If
        '        Next
        '        gridItems.Children.Remove(cc)
        '        RaiseEvent TimeslotPanelDeleteEntry(Me, e)
        '    End If
        'Else
        '    Dim sis As List(Of TimeslotPanelCutCopyEditEventArgs) = GetCheckedSchedItemEventArgs("Delete")
        '    If MessageBox.Show("Do you want to unschedule these " & sis.Count.ToString & " events?", Nothing, MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
        '        Dim lst As New List(Of TimeslotPanelDeleteEventArgs)
        '        For Each ee As TimeslotPanelCutCopyEditEventArgs In sis
        '            Dim si As SchedItem = GetSchedItemById(ee.id)
        '            Dim eee As New TimeslotPanelDeleteEventArgs With {.id = si.id, .SO = si.SO}
        '            Dim cc As ContentControl = CType(si.Parent, ContentControl)
        '            For i As Integer = lstSchedItems.Count - 1 To 0 Step -1
        '                If lstSchedItems(i) Is cc Then
        '                    lstSchedItems(i) = Nothing
        '                    lstSchedItems.RemoveAt(i)
        '                End If
        '            Next
        '            gridItems.Children.Remove(cc)
        '            lst.Add(eee)
        '        Next
        '        RaiseEvent TimeslotPanelMultiDeleteEntry(Me, lst)
        '    End If
        'End If
    End Sub

    Private Sub SchedItemCompletedRemotelyChangedHandler(sender As Object, e As TimeSlotPanelCompletedRemotelyChangedEventArgs)
        Debug.WriteLine("SchedItemCompletedRemotelyChangedHandler(" & e.SO & ")")
        RaiseEvent TimeslotPanelCompletedRemotelyChanged(Me, e)
    End Sub

    Private Sub SchedItemDoubleClickEventHandler(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)
        RaiseEvent TimeslotPanelSchedItemDoubleClick(Me, e)
    End Sub

    Private Sub SchedItemEditEventHandler(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
        RaiseEvent TimeslotPanelEditEntry(Me, e)
    End Sub

    Private Sub TimeShiftEventHandler(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
        'If Not CType(sender, SchedItem).IsChecked Then
        '    RaiseEvent TimeslotPanelTimeShiftEvent(Me, e)
        'Else
        '    RaiseEvent TimeslotPanelMultiTimeShiftEvent(Me, GetCheckedSchedItemEventArgs("TimeShift"))
        'End If
    End Sub

    Private Sub SchedItemViewSOEventHandler(sender As Object, e As TimeSlotPanelViewSOEventArgs)
        RaiseEvent TimeslotPanelViewSO(Me, e)
    End Sub

    Private Sub SchedItemCutEventHandler(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
        'If Not CType(sender, SchedItem).IsChecked Then
        '    RaiseEvent TimeslotPanelCutEntry(Me, e)
        'Else
        '    RaiseEvent TimeslotPanelMultiCutEntry(Me, GetCheckedSchedItemEventArgs(e.Action))
        'End If
    End Sub

    Private Sub SchedItemCopyEventHandler(sender As Object, e As TimeslotPanelCutCopyEditEventArgs)
        'If Not CType(sender, SchedItem).IsChecked Then
        '    RaiseEvent TimeslotPanelCopyEntry(Me, e)
        'Else
        '    RaiseEvent TimeslotPanelMultiCopyEntry(Me, GetCheckedSchedItemEventArgs(e.Action))
        'End If
    End Sub

    Private Sub SchedItemTimeMilesNoteEventHandler(sender As Object, e As TimeSlotPanelTimeMilesNoteEventArgs)
        RaiseEvent TimeslotPanelTimeMilesNotes(Me, e)
    End Sub

    Private Function GetCheckedSchedItemEventArgs(action As String, Optional OnlyChecked As Boolean = True) As List(Of TimeslotPanelCutCopyEditEventArgs)
        GetCheckedSchedItemEventArgs = New List(Of TimeslotPanelCutCopyEditEventArgs)
        GetCheckedSchedItemEventArgs2(Me, action, OnlyChecked, GetCheckedSchedItemEventArgs)
    End Function

    Private Sub GetCheckedSchedItemEventArgs2(dep As DependencyObject, action As String, OnlyChecked As Boolean, ByRef lstArgs As List(Of TimeslotPanelCutCopyEditEventArgs))
        'For Each o As Object In LogicalTreeHelper.GetChildren(dep)
        '    Dim si As SchedItem = TryCast(o, SchedItem)
        '    If si IsNot Nothing Then
        '        If si.IsChecked Or Not OnlyChecked Then
        '            Dim e As New TimeslotPanelCutCopyEditEventArgs
        '            e.Action = action
        '            e.BLDG = si.BLDG
        '            e.Duration = si.Duration
        '            e.id = si.id
        '            e.SchedDateTime = si.SchedDateTime
        '            e.IntExt = si.IntExt
        '            e.Shipper = si.Shipper
        '            e.SO = si.SO
        '            e.SODetails = si.SODetails
        '            e.Title = si.Title
        '            e.UnitOfWork = si.UnitOfWork
        '            e.Miles = si.Miles
        '            e.IsLocked = si.IsLocked
        '            lstArgs.Add(e)
        '        End If
        '    ElseIf TypeOf o Is DependencyObject Then
        '        GetCheckedSchedItemEventArgs2(CType(o, DependencyObject), action, OnlyChecked, lstArgs)
        '    End If
        'Next
    End Sub

    Private Function GetSchedItemById(SysId As String, IsTravelTask As Boolean) As SchedItem
        If IsTravelTask Then
            Return GetSchedItems(gridItems).FirstOrDefault(Function(si As SchedItem) si.SysId = SysId And si.WorkType = "Travel")
        Else
            Return GetSchedItems(gridItems).FirstOrDefault(Function(si As SchedItem) si.SysId = SysId And si.WorkType <> "Travel")
        End If
    End Function

    Private Iterator Function GetSchedItems(dep As DependencyObject) As IEnumerable(Of SchedItem)
        For Each o As Object In LogicalTreeHelper.GetChildren(dep)
            Dim si As SchedItem = TryCast(o, SchedItem)
            If si IsNot Nothing Then
                Yield si
            ElseIf TypeOf o Is DependencyObject Then
                For Each si In GetSchedItems(CType(o, DependencyObject))
                    Yield si
                Next
            End If
        Next
    End Function

    'Public Sub FlashScheduledItem(id As Integer)
    '    Dim si As SchedItem = GetSchedItemById(id)
    '    If si IsNot Nothing Then si.Flash()
    'End Sub

#Region "Properties"

    Public Property SelectedDate As Date
        Get
            Return CDate(dpPickDate.SelectedDate)
        End Get
        Set(value As Date)
            dpPickDate.SelectedDate = value
        End Set
    End Property

    Public ReadOnly Property TotalWorkTime As Double
        Get
            Dim ts As New TimeSpan()
            For Each cc As ContentControl In lstSchedItems
                Dim si As SchedItem = CType(cc.Content, SchedItem)
                ts = ts.Add(si.Duration)
            Next
            Return ts.TotalHours
        End Get
    End Property

    Public ReadOnly Property TotalNonWorkTime As Double = 0
    'Get
    '    Dim ts As New TimeSpan()
    '    For Each cc As ContentControl In lstSchedItems
    '        Dim si As SchedItem = CType(cc.Content, SchedItem)
    '        If si.SO = "NW" Then
    '            ts = ts.Add(si.Duration)
    '        End If
    '    Next
    '    Return ts.TotalHours
    'End Get
    'End Property

    Public ReadOnly Property TotalTravelTime As Double = 0
    'Get
    '    Dim ts As New TimeSpan()
    '    For Each cc As ContentControl In lstSchedItems
    '        Dim si As SchedItem = CType(cc.Content, SchedItem)
    '        If si.SO.StartsWith("TSGT") Then
    '            ts = ts.Add(si.Duration)
    '        End If
    '    Next
    '    Return ts.TotalHours
    'End Get
    'End Property

#End Region

    Private Sub TimeslotPanel_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles Me.PreviewMouseDoubleClick
        If dpPickDate.SelectedDate.Value.Date >= DateTime.Now.Date Then
            If TypeOf Mouse.DirectlyOver Is Rectangle AndAlso CType(Mouse.DirectlyOver, Rectangle).Tag IsNot Nothing Then   'Bug reported by Scott Haskell (crashed when double-clicking on scrollbar)
                RaiseEvent TimeslotPanelCreateEntry(Me, New TimeslotPanelCreateEntryEventArgs With {.StartDate = CDate(dpPickDate.SelectedDate), .TimeStart = CType(Mouse.DirectlyOver, Rectangle).Tag.ToString})
            End If
        End If
    End Sub

    Private Sub MenuItemCreateEntry_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelCreateEntry(Me, New TimeslotPanelCreateEntryEventArgs With {.StartDate = CDate(dpPickDate.SelectedDate), .TimeStart = CType(CType(sender, MenuItem).Parent, ContextMenu).Tag.ToString})
    End Sub

    Private Sub MenuItemScheduleSVCPDT_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeSlotPanelSchedSVCPDT(Me, New TimeslotPanelCreateEntryEventArgs With {.StartDate = CDate(dpPickDate.SelectedDate), .TimeStart = CType(CType(sender, MenuItem).Parent, ContextMenu).Tag.ToString})
    End Sub

    Private Sub MenuItemPaste_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelPasteEntry(Me, New TimeSlotPanelPasteEventArgs With {.TimeStart = CType(CType(sender, MenuItem).Parent, ContextMenu).Tag.ToString, .SchedDate = CDate(dpPickDate.SelectedDate)})
    End Sub

    Private Sub MenuItemVacationDay_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelVacationDay(Me, GetCheckedSchedItemEventArgs("VacationDay", False))
    End Sub

    Private Sub MenuItemDiscretionaryDay_Click(sender As Object, e As RoutedEventArgs)
        If TypeOf sender Is MenuItem Then
            Dim mi As MenuItem = CType(sender, MenuItem)
            If mi.Header.ToString = "Full" Then
                RaiseEvent TimeslotPanelDiscretionaryDay(mi.Header, GetCheckedSchedItemEventArgs("DiscretionaryDay", False))
            Else
                Dim cm As ContextMenu = CType(CType(mi.Parent, MenuItem).Parent, ContextMenu)
                RaiseEvent TimeslotPanelDiscretionaryDay(mi.Header, New TimeslotPanelCreateEntryEventArgs With {.StartDate = CDate(dpPickDate.SelectedDate), .TimeStart = cm.Tag.ToString})
            End If
        End If
    End Sub

    Private Sub rect_DragOver(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(GetType(System.Data.DataRowView)) OrElse e.Data.GetDataPresent(GetType(SchedItem)) Then
            e.Effects = DragDropEffects.Move
        Else
            e.Effects = DragDropEffects.None
            e.Handled = True
        End If
    End Sub

    Private Sub rect_Drop(sender As Object, e As DragEventArgs)
        If e.Data.GetDataPresent(GetType(System.Data.DataRowView)) Then
            Dim schedTime As DateTime
            If TypeOf sender Is RichTextBox Then
                schedTime = CDate(CType(sender, RichTextBox).Tag)
            Else
                schedTime = CDate(CType(sender, Rectangle).Tag)
            End If
            Dim drv As System.Data.DataRowView
            drv = CType(e.Data.GetData(GetType(System.Data.DataRowView)), DataRowView)
            RaiseEvent TimeslotPanelDropFromGrid(Me, New TimeslotPanelDropFromGridEventArgs With {
                                                .WorkOrder = $"{drv.Item("WorkOrder")}",
                                                 .Summary = $"{drv.Item("Summary")}",
                                                 .WindowEnd = If(drv.Item("WindowEnd") Is DBNull.Value, Nothing, drv.Item("WindowEnd")),
                                                 .CI = $"{drv.Item("CI")}",
                                                 .SLA = If(drv.Item("SLA") Is DBNull.Value OrElse drv.Item("SLA") = "UNKNOWN", Nothing, drv.Item("SLA")),
                                                 .Domicile = $"{drv.Item("Domicile")}",
                                                 .RowDetails = $"{drv.Item("RowDetails")}",
                                                 .WindowStart = If(drv.Item("WindowStart") Is DBNull.Value, Nothing, drv.Item("WindowStart")),
                                                 .Description = $"{drv.Item("Description")}",
                                                 .Vendor = $"{drv.Item("Vendor")}",
                                                 .CalDuration = If(drv.Item("CalDuration") Is DBNull.Value, New TimeSpan(), drv.Item("CalDuration")),
                                                 .TravelDur = If(drv.Item("TravelDur") Is DBNull.Value, New TimeSpan(), drv.Item("TravelDur")),
                                                 .TravelStart = If(drv.Item("TravelStart") Is DBNull.Value, Nothing, drv.Item("TravelStart")),
                                                 .BLDG = $"{drv.Item("BLDG")}",
                                                 .WorkDur = If(drv.Item("WorkDur") Is DBNull.Value, New TimeSpan(), drv.Item("WorkDur")),
                                                 .WorkEnd = If(drv.Item("WorkEnd") Is DBNull.Value, Nothing, drv.Item("WorkEnd")),
                                                 .WorkStart = If(drv.Item("WorkStart") Is DBNull.Value, Nothing, drv.Item("WorkStart")),
                                                 .TaskNum = $"{drv.Item("TaskNum")}",
                                                 .SysId = $"{drv.Item("SysId")}",
                                                 .Application = $"{drv.Item("Application")}",
                                                 .Location = $"{drv.Item("Location")}",
                                                 .Status = $"{drv.Item("Status")}",
                                                 .Notes = $"{drv.Item("Notes")}",
                                                 .AssignedTo = $"{drv.Item("AssignedTo")}",
                                                 .WorkType = $"{drv.Item("WorkType")}",
                                                 .LastUpdated = If(drv.Item("LastUpdated") Is DBNull.Value, Nothing, drv.Item("LastUpdated")),
                                                 .SchedTime = New TimeSpan(schedTime.Hour, schedTime.Minute, 0)
                                                 })
        ElseIf e.Data.GetDataPresent(GetType(SchedItem)) Then
            Dim si As SchedItem = CType(e.Data.GetData(GetType(SchedItem)), SchedItem)
            si.Visibility = Visibility.Visible
            Dim StartDateTime As Date = Nothing
            If TypeOf sender Is RichTextBox Then
                StartDateTime = CDate(CType(sender, RichTextBox).Tag)
            Else
                StartDateTime = CDate(si.StartTime.ToShortDateString & " " & CType(sender, Rectangle).Tag.ToString)
            End If
            RaiseEvent TimeslotPanelDropFromCal(Me, New TimeslotPanelDropFromCalEventArgs With {.SysId = si.SysId, .StartDateTime = StartDateTime, .Duration = si.Duration, .IsTravelTask = si.WorkType = "Travel"})
        End If
    End Sub

    Private Sub ResizeThumb_ItemResizeCompleted(sender As Object, e As TimeSlotPanelItemResizeEventArgs)
        floatingTip.IsOpen = False
        If Not txtMsg.Text.StartsWith("Error!") Then
            e.StartDateTime = GetSchedItemById(e.sys_id, e.IsTravelTask).StartTime
            Dim hours As Integer = CInt(txtMsg.Text.Split(":"c)(0))
            Dim mins As Integer = CInt(txtMsg.Text.Split(":"c)(1))
            e.Duration = New TimeSpan(hours, mins, 0)
            Debug.WriteLine($"SysId: {e.sys_id}; StartDateTime: {e.StartDateTime}; Duration: {e.Duration}")
            RaiseEvent TimeslotPanelItemResizeEvent(Me, e)
        End If
    End Sub

    Private Sub ResizeThumb_ItemResizeDragDelta(sender As Object, e As TimeSlotPanelItemResizeDragDeltaEventArgs)
        Debug.WriteLine("ResizeThumb_ItemResizeDragDelta.VerticalChange=" & e.VerticalChange.ToString)

        If Not floatingTip.IsOpen Then floatingTip.IsOpen = True

        Dim changed As Integer = CInt(e.VerticalChange)
        changed = CInt(changed / 20)    'each timeslot is 20 pixels high
        changed = changed * 15          'each timeslot is 15 minutes

        Dim ts As New TimeSpan(0, e.CurrentMinutes + changed, 0)
        Dim tsStr As String = ts.Hours.ToString & ":" & Right("0" & ts.Minutes.ToString, 2)

        Dim txt As TextBlock = CType(floatingTip.Child, TextBlock)
        txt.Text = If(e.CurrentMinutes + changed > 0, tsStr, "Error!")
        txt.Background = If(e.CurrentMinutes + changed > 0, Brushes.Bisque, Brushes.Red)

        Dim curWidth As Double = SystemParameters.CursorWidth
        Dim curHeight As Double = SystemParameters.CursorHeight

        Dim p As Point = Mouse.GetPosition(gridItems)
        floatingTip.HorizontalOffset = p.X + curWidth + If(txt.Text.StartsWith("Error!"), 13, 5)
        floatingTip.VerticalOffset = p.Y - 8

        '  (      catches everything prior to yesterday      )        (                 catches yesterday if after 1AM today for overnight techs                  )
        If dpPickDate.SelectedDate < Now.Date.AddDays(-1).Date OrElse (dpPickDate.SelectedDate = Now.Date.AddDays(-1).Date AndAlso Now.TimeOfDay.TotalMinutes > 60) Then
            txt.Text = "Error! Cannot modify schedule in the past."
            txt.Background = Brushes.Red
        End If

    End Sub

    Private Sub btnReload_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelReload(Me, e)
    End Sub

    Private Sub btnToday_Click(sender As Object, e As RoutedEventArgs)
        dpPickDate.SelectedDate = Date.Today
    End Sub

    Private Sub btnSendSchedToSupervisor_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelSendSchedToSupervisor(Me, New RoutedEventArgs)
    End Sub

    Private Sub btnApproveTechSchedule_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelApproveTechSchedule(Me, New RoutedEventArgs)
    End Sub

    Private Sub btnRejectTechSchedule_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelRejectTechSchedule(Me, New RoutedEventArgs)
    End Sub

    Private Sub btnSchedApprovalStatus_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelViewScheduleNotifications(Me, e)
    End Sub

    Private Sub btnRoutineTaskApprovalStatus_Click(sender As Object, e As RoutedEventArgs)
        RaiseEvent TimeslotPanelViewRoutineTaskNotifications(Me, e)
    End Sub

End Class