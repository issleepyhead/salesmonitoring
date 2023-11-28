Imports System.Data
Imports SalesMonitoringSystem.sgsmsdb

Public Class InventoryDialog
    Private _data As viewtblinventoryRow
    Private _itemSource As DataTable
    Public Sub New(Optional data As viewtblinventoryRow = Nothing)
        InitializeComponent()
        _data = data
        DataContext = _data
    End Sub

    Private Sub InventoryDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        _itemSource = BaseInventory.FillByInventoryProduct(ReferenceNumberLabel.Text)
        UpdateVisual()
    End Sub

    Private Sub UpdateVisual()
        ProductsDataGridView.ItemsSource = _itemSource.DefaultView
        Dim total As Integer = 0
        For Each data As DataRow In _itemSource.Rows
            total += data.Item("TOTAL")
        Next
        TotalCost.Text = total
    End Sub
End Class
