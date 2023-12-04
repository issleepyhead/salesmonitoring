Imports System.Data
Imports System.Data.SqlClient

Public Class CategoryParser
    Implements IFileParser

    Private ReadOnly _conn As SqlConnection
    Private _command As SqlCommand

    Public Sub New(conn As SqlConnection)
        _conn = conn
    End Sub

    Public Function Exists(data As Object) As Boolean Implements IFileParser.Exists
        Return BaseCategory.Exists(data.CATEGORY_NAME) > 0
    End Function

    Public Function Insert(data As Object) As Boolean Implements IFileParser.Insert
        _command = New SqlCommand("InsertCategoryProcedure", _conn)
        _command.CommandType = CommandType.StoredProcedure
        _command.Parameters.AddWithValue("@category_name", data.CATEGORY_NAME)
        _command.Parameters.AddWithValue("@category_description", data.CATEGORY_DESCRIPTION)
        Return _command.ExecuteNonQuery() > 0
    End Function

    Public Function Update(data As Object) As Boolean Implements IFileParser.Update
        Throw New NotImplementedException()
    End Function

    Public Function Fetch(data As Object) As Integer Implements IFileParser.Fetch
        Throw New NotImplementedException()
    End Function
End Class
