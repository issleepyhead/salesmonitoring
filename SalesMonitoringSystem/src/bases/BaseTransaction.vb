Imports HandyControl.Controls
Imports System.Data.SqlClient

Public Class BaseTransaction
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Sub Delete() Implements ICommandPanel.Delete
        Throw New NotImplementedException()
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Throw New NotImplementedException()
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertTransactionProcedure @product_id, @user_id, @invoice_number, @quantity, @total_amount;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        _sqlCommand.Parameters.AddWithValue("@invoice_number", _data.Item("invoice_number"))
        _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
        _sqlCommand.Parameters.AddWithValue("@total_amount", _data.Item("total_amount"))
        If _sqlCommand.ExecuteNonQuery() < 0 Then
            Growl.Error("An error occured!")
        End If
    End Sub
End Class
