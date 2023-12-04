
Imports System.Data
Imports HandyControl.Controls
Imports HandyControl.Data
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdb

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
            ComboboxCategoryFilter.Items.Add("Category")
            ComboboxCategoryFilter.Items.Add("Sub Category")
            ComboboxCategoryFilter.Items.Add("All")
            ComboboxCategoryFilter.SelectedIndex = 2
        Catch ex As Exception
            MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update

        ComboboxCategoryFilter.SelectedIndex = 2
        ComboboxParentFilter.SelectedValue = -2
        If ComboboxCategoryFilter.SelectedIndex = 2 AndAlso ComboboxParentFilter.SelectedValue = -2 Then
            _tableAdapter.Fill(_dataTable)
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
            Return
        End If

        Dim data As DataTable = BaseCategory.FillByParentCategory()
        data.Rows.Add({-2, "None"})
        ComboboxParentFilter.ItemsSource = data.DefaultView
        ComboboxParentFilter.DisplayMemberPath = "category_name"
        ComboboxParentFilter.SelectedValuePath = "id"
        ComboboxParentFilter.SelectedValue = -2
        PaginationConfig(_dataTable, Pagination)
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click, AddSubButton.Click
        Select Case True
            Case sender.Equals(AddButton)
                Dialog.Show(New CategoryDialog(subject:=_subject))
            Case sender.Equals(AddSubButton)
                Dialog.Show(New SubCategoryDialog(subject:=_subject))
        End Select
    End Sub

    Private Sub CategoriesDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles CategoriesDataGridView.SelectionChanged
        If CategoriesDataGridView.SelectedItems.Count > 0 Then
            Dim cols As viewtblcategoriesRow = CategoriesDataGridView.SelectedItems(0)
            Dim data As New Dictionary(Of String, String) From {
                {"id", cols.ID},
                {"category_name", cols.CATEGORY_NAME},
                {"category_description", If(cols.DESCRIPTION = "NULL", "", cols.DESCRIPTION)}
            }

            If BaseCategory.IsParent(cols.ID) Then
                data.Add("is_subcategory", "")
                Dialog.Show(New CategoryDialog(data, _subject))
            Else
                data.Add("is_subcategory", "1")
                Dialog.Show(New SubCategoryDialog(data, _subject))
            End If


            CategoriesDataGridView.SelectedIndex = -1
        End If
    End Sub

    '''' <summary>
    '''' To configure the paginations pages
    '''' </summary>
    'Private Sub PaginationConfig()
    '    If _dataTable.Count <= MAX_PAGE_COUNT Then
    '        Pagination.Visibility = Visibility.Collapsed
    '        Return
    '    Else
    '        Pagination.Visibility = Visibility.Visible
    '    End If

    '    If MAX_PAGE_COUNT / _dataTable.Count > 0 Then
    '        Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT + 1
    '    Else
    '        Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT
    '    End If
    'End Sub

    Private Sub CategorySearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles CategorySearch.SearchStarted
        _dataTable = BaseCategory.Search(CategorySearch.Text)
        CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        PaginationConfig(_dataTable, Pagination)
    End Sub

    Private Sub Pagination_PageUpdated(sender As Object, e As FunctionEventArgs(Of Integer)) Handles Pagination.PageUpdated
        CategoriesDataGridView.ItemsSource = _dataTable.Skip((e.Info - 1) * MAX_PAGE_COUNT).Take(MAX_PAGE_COUNT)
    End Sub

    Private Sub ComboboxCategoryFilter_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboboxCategoryFilter.SelectionChanged
        If ComboboxCategoryFilter.SelectedIndex = 0 AndAlso ComboboxParentFilter.IsEnabled Then
            ComboboxParentFilter.IsEnabled = False
            _dataTable = BaseCategory.FillByParent()
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        ElseIf ComboboxCategoryFilter.SelectedIndex = 2 Then
            _tableAdapter.Fill(_dataTable)
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        Else
            If ComboboxParentFilter.Items.Count > 0 Then
                ComboboxParentFilter.IsEnabled = True
                If ComboboxParentFilter.SelectedValue = -2 Then
                    _tableAdapter.Fill(_dataTable)
                    CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
                Else
                    _dataTable = BaseCategory.FillByFilterParent(ComboboxParentFilter.SelectedValue)
                    CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
                End If
            End If
        End If
    End Sub

    Private Sub ComboboxParentFilter_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboboxParentFilter.SelectionChanged
        If ComboboxCategoryFilter.SelectedIndex = 0 AndAlso Not ComboboxParentFilter.IsEnabled Then
            _dataTable = BaseCategory.FillByParent()
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
            Return
        End If

        If ComboboxParentFilter.SelectedValue = -2 AndAlso ComboboxParentFilter.IsEnabled Then
            _tableAdapter.Fill(_dataTable)
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        ElseIf ComboboxParentFilter.SelectedIndex <> -1 Then
            _dataTable = BaseCategory.FillByFilterParent(ComboboxParentFilter.SelectedValue)
            CategoriesDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        End If
    End Sub
End Class
