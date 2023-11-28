Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class BaseDeliveryCart
    Inherits SqlBaseConnection
    Implements IDeliveryCommandPanel, ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub
    Public Sub Receive() Implements IDeliveryCommandPanel.Receive
        Try
            _sqlCommand = New SqlCommand("EXEC RecieveDeliveryProcedure @refno, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@refno", _data.Item("reference_number"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Delivery has been recieved!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Cancel() Implements IDeliveryCommandPanel.Cancel
        Try
            _sqlCommand = New SqlCommand("EXEC CancelDeliveryProcedure @refno, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@refno", _data.Item("reference_number"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Delivery has been cancelled!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Serves as remove to the delivery cart.
    ''' </summary>
    Public Sub Delete() Implements ICommandPanel.Delete
        Try
            _sqlCommand = New SqlCommand("EXEC DeleteDeliveryCartProcedure @delivery_id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@delivery_id", _data.Item("delivery_id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been removed from delivery cart!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Try
            _sqlCommand = New SqlCommand("EXEC UpdateDeliveryProcedure @id, @supplier_id, @product_id, @product_cost, @quantity, @delivery_date, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@supplier_id", _data.Item("supplier_id"))
            _sqlCommand.Parameters.AddWithValue("@product_id", _data.Item("product_id"))
            _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
            _sqlCommand.Parameters.AddWithValue("@quantity", _data.Item("quantity"))
            _sqlCommand.Parameters.AddWithValue("@delivery_date", CDate(_data.Item("delivery_date")))
            _sqlCommand.Parameters.AddWithValue("id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() = 0 Then
                Growl.Error("Failed updating the product!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        Try
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
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function FillByProductDelivery(refNo As String, delivery_date As String) As DataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("EXEC FetchProductDeliveryProcedure @ref_no, @delivery_date", conn)
        cmd.Parameters.AddWithValue("@ref_no", refNo)
        cmd.Parameters.AddWithValue("@delivery_date", delivery_date)
        Dim dTable As New DataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function

    Public Shared Function Search(query As String) As sgsmsdb.viewtbldeliveriesDataTable
        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
        Dim cmd As New SqlCommand("SELECT * FROM viewtbldeliveries WHERE REFERENCE_NUMBER LIKE CONCAT('%', @query, '%') OR SUPPLIER LIKE CONCAT('%', @query, '%')", conn)
        cmd.Parameters.AddWithValue("@query", query)
        Dim dTable As New sgsmsdb.viewtbldeliveriesDataTable
        Dim adapter As New SqlDataAdapter(cmd)
        adapter.Fill(dTable)
        Return dTable
    End Function
End Class
