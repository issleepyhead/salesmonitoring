Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class BaseProduct
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        Try
            _sqlCommand = New SqlCommand("EXEC DeleteProductProcedure @id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been deleted successfully!")
            Else
                Growl.Error("Failed deleting the product!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Try
            _sqlCommand = New SqlCommand("EXEC UpdateProductProcedure @id, @category_id, @product_name, @product_description, @product_price, @product_cost, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
            _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
            _sqlCommand.Parameters.AddWithValue("@product_description", If(String.IsNullOrEmpty(_data.Item("product_description")), DBNull.Value, _data.Item("product_description")))
            _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
            _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been updated successfully!")
            Else
                Growl.Error("Failed updating the product!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        Try
            _sqlCommand = New SqlCommand("EXEC InsertProductProcedure @category_id, @product_name, @product_description, @product_price, @product_cost, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
            _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
            _sqlCommand.Parameters.AddWithValue("@product_description", If(String.IsNullOrEmpty(_data.Item("product_description")), DBNull.Value, _data.Item("product_description")))
            _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
            _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been added successfully!")
            Else
                Growl.Error("Failed adding the product")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function ProductInfo(id As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT PRICE, COST_PRICE FROM viewtblproducts WHERE id = @id", conn)
        cmd.Parameters.AddWithValue("@id", id)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Shared Function ScalarProducts() As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM tblproducts", conn)
        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function Exists(name As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblproducts WHERE LOWER(PRODUCT_NAME) = LOWER(@name)", conn)
        cmd.Parameters.AddWithValue("@name", name.Trim.ToLower)
        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function Search(query As String) As sgsmsdb.viewtblproductsDataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtblproducts WHERE PRODUCT_NAME LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New sgsmsdb.viewtblproductsDataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function
End Class
