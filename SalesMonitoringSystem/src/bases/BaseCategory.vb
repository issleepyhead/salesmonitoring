Imports System.Data.SqlClient

Public Class BaseCategory
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))

        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        _sqlCommand = New SqlCommand("EXEC DeleteCategoryProcedure @id, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            HandyControl.Controls.MessageBox.Info("DELETED SUCCESSFULLY!")
        End If
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        _sqlCommand = New SqlCommand("EXEC UpdateCategoryProcedure @id, @parent_id, @category_name, @category_description, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@parent_id", If(_data.Item("parent_id") = "NULL", DBNull.Value, _data.Item("parent_id")))
        _sqlCommand.Parameters.AddWithValue("@category_name", _data.Item("category_name"))
        _sqlCommand.Parameters.AddWithValue("@category_description", _data.Item("category_description"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            HandyControl.Controls.MessageBox.Info("UPDATED SUCCESSFULLY!")
        End If
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertCategoryProcedure @parent_id, @category_name, @category_description, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@parent_id", If(_data.Item("parent_id") = "NULL", DBNull.Value, _data.Item("parent_id")))
        _sqlCommand.Parameters.AddWithValue("@category_name", _data.Item("category_name"))
        _sqlCommand.Parameters.AddWithValue("@category_description", _data.Item("category_description"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            HandyControl.Controls.MessageBox.Info("INSERTED SUCCESSFULLY!")
        End If
    End Sub
End Class