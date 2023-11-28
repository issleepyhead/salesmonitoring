Imports HandyControl.Controls
Imports System.Data
Imports System.Data.SqlClient

Public Class BaseTransaction
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        Throw New NotImplementedException()
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Throw New NotImplementedException()
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertTransactionProcedure @product_id, @user_id, @invoice_number, @quantity, @total_amount;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        _sqlCommand.Parameters.AddWithValue("@invoice_number", _data.Item("invoice_number"))
        _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
        _sqlCommand.Parameters.AddWithValue("@total_amount", _data.Item("total_amount"))
        If _sqlCommand.ExecuteNonQuery() < 0 Then
            Growl.Error("An error occured!")
        End If
    End Sub

    Public Shared Function FillByProductTransaction(invoice_no As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchTransactionProductsProcedure @invoice_no", conn)
        cmd.Parameters.AddWithValue("@invoice_no", invoice_no)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    ''' <summary>
    ''' Gets the total sales of the current date.
    ''' </summary>
    ''' <returns>An integer sum of the sales in the current date.</returns>
    Public Shared Function ScalarSales() As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT CASE WHEN SUM(total_amount) IS NULL THEN 0 ELSE SUM(total_amount) END AS SALES_TODAY FROM tbltransactions WHERE date_added = CAST(GETDATE() AS DATE)", conn)

        Return cmd.ExecuteScalar()
    End Function

    ''' <summary>
    ''' Gets the total rows of the transactions
    ''' </summary>
    ''' <returns>An integer of the total rows of transaction table</returns>
    Public Shared Function ScalarTransactions() As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM (SELECT DISTINCT invoice_number FROM tbltransactions) AS count_table", conn)
        Return cmd.ExecuteScalar()
    End Function

    ''' <summary>
    ''' Search query for the transactions.
    ''' </summary>
    ''' <param name="query">A string query for invoice.</param>
    ''' <returns>A list of the result from the query.</returns>
    Public Shared Function Search(query As String) As sgsmsdb.viewtbltransactionsDataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtbltransactions WHERE INVOICE_NO LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New sgsmsdb.viewtbltransactionsDataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    ''' <summary>
    ''' Fetches the fifteen latest transactions in the transactions table.
    ''' </summary>
    ''' <returns>Returns a list of transactions.</returns>
    Public Shared Function FetchLatestTransactions() As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchLatestTransctionsProcedure", conn)
        Dim _dataTable As New DataTable
        Dim _adapter As New SqlDataAdapter(cmd)
        _adapter.Fill(_dataTable)
        Return _dataTable
    End Function
End Class
