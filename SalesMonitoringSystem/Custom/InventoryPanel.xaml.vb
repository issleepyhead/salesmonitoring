Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters

Public Class InventoryPanel
    Implements IObserverPanel
    Private _tableAdapterTransactions As New sgsmsdbTableAdapters.viewtblinventoryTableAdapter
    Private _dataTableTransactions As New sgsmsdb.viewtblinventoryDataTable
    Private _tableAdapterTransactionsRecords As New viewtblinventoryrecordsTableAdapter
    Private _dataTableTransactionsRecords As New sgsmsdb.viewtblinventoryrecordsDataTable
    Private _subject As IObservablePanel

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapterTransactions.Fill(_dataTableTransactions)
        _tableAdapterTransactionsRecords.Fill(_dataTableTransactionsRecords)
        InventoryTransactionsDataGrid.ItemsSource = _dataTableTransactions.DefaultView
        InventoryRecordsDataGrid.ItemsSource = _dataTableTransactionsRecords
    End Sub

    Private Sub InventoryRecordsDataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles InventoryTransactionsDataGrid.SelectionChanged
        If InventoryTransactionsDataGrid.SelectedItems.Count > 0 Then
            Dialog.Show(New InventoryDialog(data:=InventoryTransactionsDataGrid.SelectedItems(0)))
            InventoryTransactionsDataGrid.SelectedIndex = -1
        End If
    End Sub
End Class
