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

        PaginationConfig()
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

    ''' <summary>
    ''' To configure the paginations pages
    ''' </summary>
    Private Sub PaginationConfig()
        If _dataTable.Count <= MAX_PAGE_COUNT Then
            Pagination.Visibility = Visibility.Collapsed
            Return
        Else
            Pagination.Visibility = Visibility.Visible
        End If

        If MAX_PAGE_COUNT / _dataTable.Count < 0 Then
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT + 1
        Else
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT
        End If
    End Sub

    Private Sub SearchDelivery_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SearchDelivery.SearchStarted
        _dataTable = BaseDeliveryCart.Search(SearchDelivery.Text)
        DeliveryDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        PaginationConfig()
    End Sub
End Class
