Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class BaseSupplier
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        _sqlCommand = New SqlCommand("EXEC DeleteSupplierProcedure @id, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Supplier has been deleted successfully!")
        End If
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        _sqlCommand = New SqlCommand("EXEC UpdateSupplierProcedure @id, @supplier_name, @supplier_address, @supplier_contact, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@supplier_name", _data.Item("supplier_name"))
        _sqlCommand.Parameters.AddWithValue("@supplier_address", _data.Item("supplier_address"))
        _sqlCommand.Parameters.AddWithValue("@supplier_contact", _data.Item("supplier_contact"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Supplier has been updated successfully!")
        End If
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertSupplierProcedure @supplier_name, @supplier_address, @supplier_contact, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@supplier_name", _data.Item("supplier_name"))
        _sqlCommand.Parameters.AddWithValue("@supplier_address", _data.Item("supplier_address"))
        _sqlCommand.Parameters.AddWithValue("@supplier_contact", _data.Item("supplier_contact"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Supplier has been added successfully!")
        End If
    End Sub

    Public Shared Function Search(query As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtblsuppliers WHERE SUPPLIER_NAME LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Shared Function Exists(name As String, address As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblsuppliers WHERE LOWER(SUPPLIER_NAME) = LOWER(@name) AND LOWER(ADDRESS) = LOWER(@address)", conn)
        cmd.Parameters.AddWithValue("@name", name.Trim.ToLower)
        cmd.Parameters.AddWithValue("@address", address.Trim.ToLower)

        Return cmd.ExecuteScalar()
    End Function
End Class
