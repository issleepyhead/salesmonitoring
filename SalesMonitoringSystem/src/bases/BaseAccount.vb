﻿Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

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
        If _sqlCommand.ExecuteNonQuery() <= 0 Then
            Growl.Error("An error occured!")
        Else
            Growl.Info("Account has been deleted successfully!")
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
        If _sqlCommand.ExecuteNonQuery() <= 0 Then
            Growl.Error("An error occured!")
        Else
            Growl.Info("Account has been updated successfully!")
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
        If _sqlCommand.ExecuteNonQuery() <= 0 Then
            Growl.Error("An error occured")
        Else
            Growl.Info("Account has been added successfully!")
        End If
    End Sub

    Public Shared Function ScalarRoleName(rolename As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT id FROM tblroles WHERE role_name = @role_name", conn)
        cmd.Parameters.AddWithValue("@role_name", rolename)

        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function Exists(data As String) As Integer
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblusers WHERE USERNAME = @data", conn)
        cmd.Parameters.AddWithValue("@data", data.Trim.ToLower)

        Return cmd.ExecuteScalar()
    End Function

    Public Shared Function FillByRoles() As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT id, role_name FROM tblroles WHERE id <> 1", conn)

        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Shared Function Search(query As String) As sgsmsdb.viewtblusersDataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtblusers WHERE id <> 1 AND FULL_NAME LIKE CONCAT('%', @query, '%') OR USERNAME LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New sgsmsdb.viewtblusersDataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function


End Class