Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider

Module Helpers

    ''' <summary>
    ''' An invoice generator function that returns a string with a format of SGMS-{DATE}-{8_DIGIT_SINCE_FIRST_EPOCH_UNIX_TIME}
    ''' </summary>
    ''' <returns>String containing the generated invoice number</returns>
    Public Function GenInvoiceNumber() As String
        Randomize()
        Dim unixTimeString As String = CStr(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds)
        Dim timeString As String = unixTimeString.Substring(5, unixTimeString.Count - 6)
        Dim dateString As String = String.Join("", DateTime.Now.ToShortDateString.Split("/"))
        Return String.Format("SGMS-{0}-{1}", dateString, timeString)
    End Function

    ''' <summary>
    ''' Searches for button within a dialog then invoke the click command in it.
    ''' </summary>
    ''' <param name="btn">A control inherited to button.</param>
    Public Sub CloseDialog(btn As Button)
        Dim peer As ButtonAutomationPeer = TryCast(UIElementAutomationPeer.CreatePeerForElement(btn), ButtonAutomationPeer)
        ' If the peer variable has found the button
        If peer IsNot Nothing Then
            ' We invoke the click so that it the dialog will close
            Dim provider As IInvokeProvider = TryCast(peer.GetPattern(PatternInterface.Invoke), IInvokeProvider)
            provider?.Invoke()
        End If
    End Sub
End Module
