Imports HandyControl.Controls
Imports SalesMonitoringSystem.sgsmsdb
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
        Try
            _sqlCommand = New SqlCommand("EXEC InsertTransactionProcedure @product_id, @user_id, @invoice_number, @quantity, @total_amount;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            _sqlCommand.Parameters.AddWithValue("@invoice_number", _data.Item("invoice_number"))
            _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
            _sqlCommand.Parameters.AddWithValue("@total_amount", _data.Item("total_amount"))
            If _sqlCommand.ExecuteNonQuery() < 0 Then
                Growl.Error("An error occured!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Search all the transaction base on the invoice number
    ''' </summary>
    ''' <param name="invoice_no">The invoce number to be searched</param>
    ''' <returns>A datatable containing all the result search for invoice number</returns>
    Public Shared Function FillByProductTransaction(invoice_no As String) As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FetchTransactionProductsProcedure @invoice_no", conn)
            cmd.Parameters.AddWithValue("@invoice_no", invoice_no)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    ''' <summary>
    ''' Gets the total sales of the current date.
    ''' </summary>
    ''' <returns>An integer sum of the sales in the current date.</returns>
    Public Shared Function ScalarSales() As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT CASE WHEN SUM(total_amount) IS NULL THEN 0 ELSE SUM(total_amount) END AS SALES_TODAY FROM tbltransactions WHERE date_added = CAST(GETDATE() AS DATE)", conn)

            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Gets the total rows of the transactions
    ''' </summary>
    ''' <returns>An integer of the total rows of transaction table</returns>
    Public Shared Function ScalarTransactions() As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtbltransactions", conn)
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Search query for the transactions.
    ''' </summary>
    ''' <param name="query">A string query for invoice.</param>
    ''' <returns>A list of the result from the query.</returns>
    Public Shared Function Search(query As String) As sgsmsdb.viewtbltransactionsDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtbltransactions WHERE INVOICE_NO LIKE CONCAT('%', @query, '%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New sgsmsdb.viewtbltransactionsDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New sgsmsdb.viewtbltransactionsDataTable
        End Try
    End Function

    ''' <summary>
    ''' Fetches the fifteen latest transactions in the transactions table.
    ''' </summary>
    ''' <returns>Returns a list of transactions.</returns>
    Public Shared Function FetchLatestTransactions() As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FetchLatestTransctionsProcedure", conn)
            Dim _dataTable As New DataTable
            Dim _adapter As New SqlDataAdapter(cmd)
            _adapter.Fill(_dataTable)
            Return _dataTable
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    ''' <summary>
    ''' Filter the transactions data by dates
    ''' </summary>
    ''' <param name="fd">A starting date for the query</param>
    ''' <param name="sd">An end date for the querys</param>
    ''' <returns>A table of the result of search</returns>
    Public Shared Function FilterTransactionsByDate(fd As Date, sd As Date) As viewtbltransactionsDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("FilterTransactionDateProcedure", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@fd", fd).DbType = DbType.Date
            cmd.Parameters.AddWithValue("@sd", sd).DbType = DbType.Date
            Dim _dataTable As New viewtbltransactionsDataTable
            Dim _adapter As New SqlDataAdapter(cmd)
            _adapter.Fill(_dataTable)
            Return _dataTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtbltransactionsDataTable
        End Try
    End Function

    ''' <summary>
    '''  A function for filtering all the reports of the transactions.
    ''' </summary>
    ''' <param name="sdate">Start date of the query.</param>
    ''' <param name="edate">End date of the query.</param>
    ''' <param name="haystack">Haystack of the result whether it is weekly, monthly, or yearly</param>
    ''' <returns>Returns a number of records and revenue between two dates.</returns>
    Public Shared Function FetchReportransactions(sdate As Date, edate As Date, Optional haystack As Integer = 1) As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SalesReportProcedure", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@start_date", sdate).DbType = DbType.Date
            cmd.Parameters.AddWithValue("@end_date", edate).DbType = DbType.Date
            cmd.Parameters.AddWithValue("@haystack", haystack)

            Dim _dataTable As New DataTable
            Dim _adapter As New SqlDataAdapter(cmd)
            _adapter.Fill(_dataTable)
            Return _dataTable
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function
End Class
