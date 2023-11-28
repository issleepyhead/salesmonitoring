Imports System.Data
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports HandyControl.Controls

''' <summary>
''' Collection of functions that will be generally used in forms.
''' </summary>
Module Helpers

    ''' <summary>
    ''' An invoice generator function that returns a string with a format of SGMS-{DATE}-{8_DIGIT_SINCE_FIRST_EPOCH_UNIX_TIME}
    ''' </summary>
    ''' <returns>String containing the generated invoice number</returns>
    Public Function GenInvoiceNumber(type As InvoiceType) As String
        Randomize()
        Dim unixTimeString As String = CStr(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds)
        Dim timeString As String = unixTimeString.Substring(5, unixTimeString.Count - 6)
        Dim dateString As String = String.Join("", DateTime.Now.ToShortDateString.Split("/"))
        Select Case type
            Case InvoiceType.Transaction
                Return String.Format("SGMST-{0}-{1}", dateString, timeString)
            Case InvoiceType.Delivery
                Return String.Format("SGMSD-{0}-{1}", dateString, timeString)
        End Select
        Return Nothing
    End Function

    ''' <summary>
    ''' Searches for button within a dialog then invoke the click command in it.
    ''' </summary>
    ''' <param name="btn">A button control.</param>
    Public Sub CloseDialog(btn As Button)
        Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(btn), ButtonAutomationPeer)
        ' If the peer variable has found the button
        If peer IsNot Nothing Then
            ' We invoke the click so that it the dialog will close
            Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
            provider?.Invoke()
        End If
    End Sub

    Public Sub PaginationConfig(_dataTable As Object, _pagination As Pagination, Optional MAX_PAGE_COUNT As Integer = 30)
        If _dataTable.Count Mod MAX_PAGE_COUNT = 0 Then
            _pagination.MaxPageCount = Math.Truncate(_dataTable.Count / MAX_PAGE_COUNT)
        Else
            _pagination.MaxPageCount = Math.Truncate(_dataTable.Count / MAX_PAGE_COUNT) + 1
        End If
    End Sub

    Enum InvoiceType
        Transaction
        Delivery
    End Enum
End Module
