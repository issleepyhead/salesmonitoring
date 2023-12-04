Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls
Imports SalesMonitoringSystem.sgsmsdb

Public Class BaseCategory
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))

        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        Try
            _sqlCommand = New SqlCommand("EXEC DeleteCategoryProcedure @id, @user_id, @is_subcategory;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            _sqlCommand.Parameters.AddWithValue("@is_subcategory", If(String.IsNullOrEmpty(_data.Item("is_subcategory")), DBNull.Value, 1))
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Category has been deleted successfully!")
            Else
                Growl.Error("Failed deleting the category!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Try
            _sqlCommand = New SqlCommand("EXEC UpdateCategoryProcedure @id, @category_name, @category_description, @parent_id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@category_name", _data.Item("category_name"))
            _sqlCommand.Parameters.AddWithValue("@category_description", If(String.IsNullOrEmpty(_data.Item("category_description")), DBNull.Value, _data.Item("category_description")))
            _sqlCommand.Parameters.AddWithValue("@parent_id", If(String.IsNullOrEmpty(_data.Item("parent_id")), DBNull.Value, _data.Item("parent_id")))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Category has been updated successfully!")
            Else
                Growl.Error("Failed updating the category!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        Try
            _sqlCommand = New SqlCommand("EXEC InsertCategoryProcedure @category_name, @category_description, @parent_id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@category_name", _data.Item("category_name"))
            _sqlCommand.Parameters.AddWithValue("@category_description", If(String.IsNullOrEmpty(_data.Item("category_description")), DBNull.Value, _data.Item("category_description")))
            _sqlCommand.Parameters.AddWithValue("@parent_id", If(String.IsNullOrEmpty(_data.Item("parent_id")), DBNull.Value, _data.Item("parent_id")))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Category has been added successfully!")
            Else
                Growl.Error("Failed adding the category!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function FillByParent() As viewtblcategoriesDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FillParentCategoriesProcedure", conn)
            Dim dTable As New viewtblcategoriesDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtblcategoriesDataTable
        End Try
    End Function

    Public Shared Function FillByFilterParent(id As String) As viewtblcategoriesDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("EXEC FilterCategoryProcedure @parent_id", conn)
            cmd.Parameters.AddWithValue("@parent_id", id)
            Dim dTable As New viewtblcategoriesDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtblcategoriesDataTable
        End Try
    End Function

    Public Shared Function IsParent(id As String) As Boolean
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT CASE WHEN COUNT(*) IS NULL THEN 0 ELSE COUNT(*) END AS s FROM tblcategories WHERE id = @id AND id NOT IN (SELECT category_id FROM tblsubcategories)", conn)
            cmd.Parameters.AddWithValue("@id", id)
            If cmd.ExecuteScalar() > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function GetParent(id As String) As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT parent_id FROM tblsubcategories WHERE category_id = @id", conn)
            cmd.Parameters.AddWithValue("@id", id)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    Public Shared Function Exists(data As String) As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM tblcategories WHERE LOWER(category_name) = @data", conn)
            cmd.Parameters.AddWithValue("@data", data.Trim.ToLower)

            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function FillByParentCategory() As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT id, category_name FROM tblcategories WHERE id NOT IN (SELECT category_id FROM tblsubcategories)", conn)

            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    Public Shared Function Search(query As String) As viewtblcategoriesDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtblcategories WHERE CATEGORY_NAME LIKE CONCAT('%', @query, '%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New viewtblcategoriesDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New viewtblcategoriesDataTable
        End Try
    End Function
End Class