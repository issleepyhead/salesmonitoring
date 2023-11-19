
Imports HandyControl.Controls
Imports HandyControl.Data
Imports HandyControl.Tools.Extension

Public Class CategoriesPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _dataTable As New sgsmsdb.viewtblcategoriesDataTable
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
        CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New CategoryDialog(subject:=_subject))
    End Sub

    Private Sub CategoriesDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CategoriesDataGridView.SelectionChanged
        If CategoriesDataGridView.SelectedItems.Count > 0 Then
            Dim cols As IList(Of DataGridCellInfo) = CategoriesDataGridView.SelectedCells()
            Dim data As New Dictionary(Of String, String) From {
                {"id", cols.Item(0).Item(0).ToString},
                {"parent_id", BaseCategory.ScalarCategoryParentID(cols.Item(0).Item(0).ToString)},
                {"category_name", cols.Item(0).Item(1).ToString},
                {"category_description", cols.Item(0).Item(2).ToString}
            }

            Dialog.Show(New CategoryDialog(data, _subject))
            CategoriesDataGridView.SelectedIndex = -1
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

    Private Sub CategorySearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles CategorySearch.SearchStarted
        _dataTable = BaseCategory.Search(CategorySearch.Text)
        CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        PaginationConfig()
    End Sub

    Private Sub Pagination_PageUpdated(sender As Object, e As FunctionEventArgs(Of Integer)) Handles Pagination.PageUpdated
        CategoriesDataGridView.ItemsSource = _dataTable.Skip((e.Info - 1) * MAX_PAGE_COUNT).Take(MAX_PAGE_COUNT)
    End Sub
End Class
