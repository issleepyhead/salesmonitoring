Imports System.Data
Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters

Public Class InventoryPanel
    Implements IObserverPanel
    Private _tableAdapterTransactions As New viewtblinventoryTableAdapter
    Private _dataTableTransactions As New sgsmsdb.viewtblinventoryDataTable
    Private _tableAdapterTransactionsRecords As New viewtblinventoryrecordsTableAdapter
    Private _dataTableTransactionsRecords As New sgsmsdb.viewtblinventoryrecordsDataTable
    Private _subject As IObservablePanel
    Private Const MAX_PAGE_COUNT = 30

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapterTransactions.Fill(_dataTableTransactions)
        _tableAdapterTransactionsRecords.Fill(_dataTableTransactionsRecords)
        InventoryTransactionsDataGrid.ItemsSource = _dataTableTransactions.DefaultView
        InventoryRecordsDataGrid.ItemsSource = _dataTableTransactionsRecords
    End Sub

    ''' <summary>
    ''' To configure the paginations pages
    ''' </summary>
    Private Sub PaginationConfig()
        If _dataTableTransactions.Count <= MAX_PAGE_COUNT Then
            PaginationTransactions.Visibility = Visibility.Collapsed
            Return
        Else
            PaginationTransactions.Visibility = Visibility.Visible
        End If

        If MAX_PAGE_COUNT / _dataTableTransactions.Count < 0 Then
            PaginationTransactions.MaxPageCount = _dataTableTransactions.Count / MAX_PAGE_COUNT + 1
        Else
            PaginationTransactions.MaxPageCount = _dataTableTransactions.Count / MAX_PAGE_COUNT
        End If
    End Sub

    Private Sub InventoryRecordsDataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles InventoryTransactionsDataGrid.SelectionChanged
        If InventoryTransactionsDataGrid.SelectedItems.Count > 0 Then
            Dialog.Show(New InventoryDialog(data:=InventoryTransactionsDataGrid.SelectedItems(0)))
            InventoryTransactionsDataGrid.SelectedIndex = -1
        End If
    End Sub
End Class
