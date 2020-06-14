Imports System

Public Class TimeslotPanelDateChangedEventArgs
    Inherits EventArgs
    Public tspDate As Date
End Class

Public Class TimeslotPanelDeleteEventArgs
    Inherits EventArgs
    Public id As Integer
    Public SO As String
End Class

Public Class TimeslotPanelCreateEntryEventArgs
    Inherits EventArgs
    Public StartDate As Date
    Public TimeStart As String
End Class

Public Class TimeslotPanelUnassignTaskEventArgs
    Inherits EventArgs
    Public SysId As String
End Class

Public Class TimeslotPanelToggleTravelEventArgs
    Inherits EventArgs
    Public SysId As String
    Public ToggleAction As String
End Class

Public Class TimeslotPanelDropFromGridEventArgs
    Inherits EventArgs
    Public WorkOrder As String
    Public Summary As String
    Public WindowEnd As DateTime
    Public CI As String
    Public SLA As DateTime
    Public Domicile As String
    Public RowDetails As String
    Public WindowStart As DateTime
    Public Description As String
    Public Vendor As String
    Public CalDuration As TimeSpan
    Public TravelDur As TimeSpan
    Public TravelStart As DateTime
    Public BLDG As String
    Public WorkDur As TimeSpan
    Public WorkStart As DateTime
    Public WorkEnd As DateTime
    Public TaskNum As String
    Public SysId As String
    Public Application As String
    Public Location As String
    Public Status As String
    Public Notes As String
    Public AssignedTo As String
    Public WorkType As String
    Public LastUpdated As DateTime
    Public SchedTime As TimeSpan
End Class

Public Class TimeslotPanelDropFromCalEventArgs
    Inherits EventArgs
    Public SysId As String
    Public StartDateTime As Date
    Public Duration As TimeSpan
    Public IsTravelTask As Boolean
End Class

Public Class TimeslotPanelCutCopyEditEventArgs
    Inherits EventArgs
    Public id As Integer
    Public UnitOfWork As String
    Public Duration As TimeSpan
    Public SchedDateTime As Date
    Public DueDate As Date
    Public IsLocked As Boolean
    Public SO As String
    Public SODetails As String
    Public Shipper As String
    Public Title As String
    Public BLDG As String
    Public Action As String
    Public Miles As Decimal
    Public IntExt As String
    Public Tech As String
End Class

Public Class TimeSlotPanelPasteEventArgs
    Inherits EventArgs
    Public TimeStart As String
    Public SchedDate As Date
    Public IsLocked As Boolean
End Class

Public Class TimeSlotPanelTimeMilesNoteEventArgs
    Inherits EventArgs
    Public id As Integer
    Public SO As String
    Public Status As String
End Class

Public Class TimeSlotPanelItemResizeEventArgs
    Inherits EventArgs
    Public sys_id As String
    Public StartDateTime As DateTime
    Public Duration As TimeSpan
    Public IsTravelTask As Boolean
End Class

Public Class TimeSlotPanelItemResizeDragDeltaEventArgs
    Inherits EventArgs
    Public CurrentMinutes As Integer
    Public VerticalChange As Integer
End Class

Public Class TimeSlotPanelCompletedRemotelyChangedEventArgs
    Inherits EventArgs
    Public SO As String
    Public IsCompletedRemotely As Boolean
End Class

Public Class TimeSlotPanelViewSOEventArgs
    Inherits EventArgs
    Public TaskNum As String
End Class