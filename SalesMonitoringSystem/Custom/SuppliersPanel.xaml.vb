Imports HandyControl.Controls
Imports HandyControl.Data

Public Class SuppliersPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblsuppliersTableAdapter
    Private _dataTable As New sgsmsdb.viewtblsuppliersDataTable
    Private _subject As IObservablePanel
    Private Const MAX_PAGE_COUNT As Integer = 30

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
        _tableAdapter.Fill(_dataTable)
        SuppliersDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New SupplierDialog(subject:=_subject))
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

    Private Sub SuppliersDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SuppliersDataGridView.SelectionChanged
        If SuppliersDataGridView.SelectedItems.Count > 0 Then
            Dim cols As IList(Of DataGridCellInfo) = SuppliersDataGridView.SelectedCells()
            Dim data As New Dictionary(Of String, String) From {
                {"id", cols.Item(0).Item(0)},
                {"supplier_name", cols.Item(0).Item(1)},
                {"supplier_address", cols.Item(0).Item(2)},
                {"supplier_contact", cols.Item(0).Item(3)}
            }
            Dialog.Show(New SupplierDialog(data, _subject))
            SuppliersDataGridView.SelectedIndex = -1
        End If
    End Sub

    Private Sub SupplierSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles SupplierSearch.SearchStarted
        _dataTable = BaseSupplier.Search(SupplierSearch.Text)
        SuppliersDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig()
    End Sub
End Class
