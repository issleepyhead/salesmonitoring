Imports System.Data
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
        Try
            _sqlCommand = New SqlCommand("EXEC DeleteAccountProcedure @id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() <= 0 Then
                Growl.Error("An error occured!")
            Else
                Growl.Success("Account has been deleted successfully!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Try
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
                Growl.Success("Account has been updated successfully!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        Try
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
                Growl.Success("Account has been added successfully!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function ScalarRoleName(rolename As String) As Integer
        Try

            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT id FROM tblroles WHERE role_name = @role_name", conn)
            cmd.Parameters.AddWithValue("@role_name", rolename)

            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function Exists(data As String) As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblusers WHERE USERNAME = @data", conn)
            cmd.Parameters.AddWithValue("@data", data.Trim.ToLower)

            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function FillByRoles() As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT id, role_name FROM tblroles WHERE id <> 1", conn)

            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    Public Shared Function Search(query As String) As sgsmsdb.viewtblusersDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtblusers WHERE id <> 1 AND FULL_NAME LIKE CONCAT('%', @query, '%') OR USERNAME LIKE CONCAT('%', @query, '%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New sgsmsdb.viewtblusersDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New sgsmsdb.viewtblusersDataTable
        End Try
    End Function


End Class