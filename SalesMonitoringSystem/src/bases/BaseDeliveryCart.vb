Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class BaseDeliveryCart
    Inherits SqlBaseConnection
    Implements IDeliveryCommandPanel, ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub
    Public Sub Recieve() Implements IDeliveryCommandPanel.Recieve
        _sqlCommand = New SqlCommand("EXEC RecieveDeliveryProcedure @refno, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@refno", _data.Item("reference_number"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Delivery has been recieved!")
        End If
    End Sub

    Public Sub Cancel() Implements IDeliveryCommandPanel.Cancel
        _sqlCommand = New SqlCommand("EXEC CancelDeliveryProcedure @refno, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@refno", _data.Item("reference_number"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Delivery has been cancelled!")
        End If
    End Sub

    ' Serves as delete to product cart
    Public Sub Delete() Implements ICommandPanel.Delete
        _sqlCommand = New SqlCommand("EXEC DeleteDeliveryCartProcedure @delivery_id, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@delivery_id", _data.Item("delivery_id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() > 0 Then
            Growl.Info("Product has been deleted from delivery cart!")
        End If
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        _sqlCommand = New SqlCommand("EXEC UpdateDeliveryProcedure @id, @supplier_id, @product_id, @product_cost, @quantity, @delivery_date, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@supplier_id", _data.Item("supplier_id"))
        _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
        _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
        _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
        _sqlCommand.Parameters.AddWithValue("@delivery_date", CDate(_data.Item("delivery_date")))
        _sqlCommand.Parameters.AddWithValue("id", _data.Item("id"))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() = 0 Then
            Growl.Info("Failed updating the product!")
        End If
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        _sqlCommand = New SqlCommand("EXEC InsertDeliveryProcedure @supplier_id, @product_id, @reference_number, @product_cost, @quantity, @delivery_date, @user_id;", _sqlConnection)
        _sqlCommand.Parameters.AddWithValue("@supplier_id", _data.Item("supplier_id"))
        _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
        _sqlCommand.Parameters.AddWithValue("@reference_number", _data.Item("reference_number"))
        _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
        _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
        _sqlCommand.Parameters.AddWithValue("@delivery_date", CDate(_data.Item("delivery_date")))
        _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
        If _sqlCommand.ExecuteNonQuery() = 0 Then
            Growl.Info("Failed adding the product!")
        End If
    End Sub
End Class
