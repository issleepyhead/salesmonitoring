Imports System.Data
Imports System.Data.SqlClient

Public Class BaseInventory
    Inherits SqlBaseConnection

    Public Shared Function FillByInventoryProduct(refNo As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchProductInventoryProcedure @ref_no", conn)
        cmd.Parameters.AddWithValue("@ref_no", refNo)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Shared Function ScalarStocks(product_id As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchProductStocksProcedure @id", conn)
        cmd.Parameters.AddWithValue("@id", product_id)
        Return cmd.ExecuteScalar()
    End Function
End Class
