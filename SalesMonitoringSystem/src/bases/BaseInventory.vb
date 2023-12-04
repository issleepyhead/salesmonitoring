Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports SalesMonitoringSystem.sgsmsdb

Public Class BaseInventory
    Inherits SqlBaseConnection

    Public Shared Function FillByInventoryProduct(refNo As String) As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FetchProductInventoryProcedure @ref_no", conn)
            cmd.Parameters.AddWithValue("@ref_no", refNo)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    Public Shared Function ScalarStocks(product_id As String) As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FetchProductStocksProcedure @id", conn)
            cmd.Parameters.AddWithValue("@id", product_id)
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function SearchInventoryRecords(query As String) As viewtblinventoryrecordsDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtblinventoryrecords WHERE [PRODUCT_NAME] LIKE CONCAT('%',@query,'%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New viewtblinventoryrecordsDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtblinventoryrecordsDataTable
        End Try
    End Function

    Public Shared Function SearchInventoryTransactions(query As String) As viewtblinventoryDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtblinventory WHERE [REF_NO] LIKE CONCAT('%',@query,'%') OR [SUPPLIER_NAME] LIKE CONCAT('%',@query,'%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New viewtblinventoryDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtblinventoryDataTable
        End Try
    End Function
End Class
