Imports System.Data
Imports System.Data.SqlClient

Public Class BaseProduct
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        _sqlCommand = New SqlCommand("EXEC DeleteProductProcedure @id, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        _sqlCommand = New SqlCommand("EXEC UpdateProductProcedure @id, @category_id, @product_name, @product_description, @product_price, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
        _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
        _sqlCommand.Parameters.AddWithValue("@product_description", _data.Item("product_description"))
        _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertProductProcedure @category_id, @product_name, @product_description, @product_price, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
        _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
        _sqlCommand.Parameters.AddWithValue("@product_description", _data.Item("product_description"))
        _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub

    Public Shared Function ScalarPrice(id As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT product_price FROM tblproducts WHERE id = @id", conn)
        cmd.Parameters.AddWithValue("@id", id)

        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function ScalarProducts() As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM tblproducts", conn)
        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function Exists(name As String, price As String, category As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblproducts WHERE LOWER(PRODUCT_NAME) = LOWER(@name) AND PRODUCT_PRICE = @price AND CATEGORY_ID = @category", conn)
        cmd.Parameters.AddWithValue("@name", name.Trim.ToLower)
        cmd.Parameters.AddWithValue("@price", price.Trim)
        cmd.Parameters.AddWithValue("@category", category.Trim)

        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function Search(query As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtblproducts WHERE PRODUCT_NAME LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function
End Class
