
Imports HandyControl.Controls
Imports HandyControl.Data
Imports HandyControl.Tools.Extension

Public Class CategoriesPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _dataTable As New sgsmsdb.viewtblcategoriesDataTable
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
        _tableAdapter.Fill(_dataTable)
        CategoriesDataGridView.ItemsSource = _dataTable.DefaultView
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

    Private Sub CategorySearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles CategorySearch.SearchStarted
        CategoriesDataGridView.ItemsSource = BaseCategory.Search(CategorySearch.Text).DefaultView
    End Sub
End Class
