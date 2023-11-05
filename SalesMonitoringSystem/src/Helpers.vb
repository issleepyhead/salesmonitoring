Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider

Module Helpers
    Public Function FillByParentCategory() As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT id, category_name FROM tblcategories WHERE parent_category IS NULL", conn)

        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Function ScalarCategoryParentID(id As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT CASE WHEN parent_category IS NULL THEN -1 ELSE parent_category END AS parent_category FROM tblcategories WHERE id = @id", conn)
        cmd.Parameters.AddWithValue("@id", id)

        Return cmd.ExecuteScalar()
    End Function

    Public Function FillByRoles() As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT id, role_name FROM tblroles WHERE id <> 1", conn)

        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Function FillByProductDelivery(refNo As String, delivery_date As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchProductDeliveryProcedure @ref_no, @delivery_date", conn)
        cmd.Parameters.AddWithValue("@ref_no", refNo)
        cmd.Parameters.AddWithValue("@delivery_date", delivery_date)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Function ScalarRoleName(rolename As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT id FROM tblroles WHERE role_name = @role_name", conn)
        cmd.Parameters.AddWithValue("@role_name", rolename)

        Return cmd.ExecuteScalar()
    End Function

    Public Function ScalarPrice(id As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT product_price FROM tblproducts WHERE id = @id", conn)
        cmd.Parameters.AddWithValue("@id", id)

        Return cmd.ExecuteScalar()
    End Function

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
