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
End Class
