Imports HandyControl.Controls
Imports HandyControl.Data

Public Class DeliveryPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtbldeliveriesTableAdapter
    Private _dataTable As New sgsmsdb.viewtbldeliveriesDataTable
    Private _subject As IObservablePanel
    Private Const MAX_PAGE_COUNT As Integer = 30

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
        _tableAdapter.Fill(_dataTable)
        DeliveryDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig(_dataTable, [Pagination])
    End Sub

    Private Sub DeliveryAddDeliveryButton_Click(sender As Object, e As RoutedEventArgs) Handles DeliveryAddDeliveryButton.Click
        Dialog.Show(New DeliveryCartDialog(subject:=_subject))
    End Sub

    Private Sub DeliveryDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DeliveryDataGridView.SelectionChanged
        If DeliveryDataGridView.SelectedItems.Count > 0 Then
            Dialog.Show(New DeliveryCartDialog(DeliveryDataGridView.SelectedItems(0), _subject))
            DeliveryDataGridView.SelectedIndex = -1
        End If
    End Sub


    Private Sub SearchDelivery_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchDelivery.SearchStarted
        _dataTable = BaseDeliveryCart.Search(SearchDelivery.Text)
        DeliveryDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        PaginationConfig(_dataTable, [Pagination])
    End Sub

End Class
