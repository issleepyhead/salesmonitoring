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
End Class
