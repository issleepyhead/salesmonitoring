Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class LoginModule
    Private ReadOnly _sqlConnection As SqlConnection = SqlConnectionSingleton.GetInstance
    Private _sqlCommand As SqlCommand = Nothing
    Private _dataSet As DataTable
    Private _sqlAdapter As SqlDataAdapter

    Public Function LoginAccount(username As String, password As String) As Object()
        _sqlCommand = New SqlCommand("SELECT id, password, role_id FROM tblusers WHERE username = @username", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@username", username)
        _sqlAdapter = New SqlDataAdapter(_sqlCommand)
        _dataSet = New DataTable
        _sqlAdapter.Fill(_dataSet)

        If _dataSet.Rows.Count > 0 Then
            Dim pwd As String = _dataSet.Rows(0).Item("password")
            If BCrypt.Net.BCrypt.Verify(password, pwd) Then
                My.Settings.userID = _dataSet.Rows(0).Item("id")
                My.Settings.userRole = _dataSet.Rows(0).Item("role_id")
                My.Settings.Save()
                Return {True, "Login Success!"}
            Else
                Return {False, "Incorrect username or password!"}
            End If
        Else
            Return {False, "Incorrect username or password!"}
        End If

        Return {False, "Unknown error please try again."}
    End Function
End Class
