Imports System.Data.SqlClient

Public Class BaseAccount
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        _sqlCommand = New SqlCommand("EXEC DeleteAccountProcedure @id, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        _sqlCommand = New SqlCommand("EXEC UpdateAccountProcedure @id, @role_id, @first_name, @last_name, @address, @contact, @username, @password, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@role_id", _data.Item("role_id"))
        _sqlCommand.Parameters.AddWithValue("@first_name", _data.Item("first_name"))
        _sqlCommand.Parameters.AddWithValue("@last_name", _data.Item("last_name"))
        _sqlCommand.Parameters.AddWithValue("@address", _data.Item("address"))
        _sqlCommand.Parameters.AddWithValue("@contact", _data.Item("contact"))
        _sqlCommand.Parameters.AddWithValue("@username", _data.Item("username"))
        _sqlCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(_data.Item("password")))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertAccountProcedure @role_id, @first_name, @last_name, @address, @contact, @username, @password, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@role_id", _data.Item("role_id"))
        _sqlCommand.Parameters.AddWithValue("@first_name", _data.Item("first_name"))
        _sqlCommand.Parameters.AddWithValue("@last_name", _data.Item("last_name"))
        _sqlCommand.Parameters.AddWithValue("@address", _data.Item("address"))
        _sqlCommand.Parameters.AddWithValue("@contact", _data.Item("contact"))
        _sqlCommand.Parameters.AddWithValue("@username", _data.Item("username"))
        _sqlCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(_data.Item("password")))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then

        End If
    End Sub
End Class