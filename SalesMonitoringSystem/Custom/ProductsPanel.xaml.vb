Imports HandyControl.Controls
Imports HandyControl.Data

Public Class ProductsPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblproductsTableAdapter
    Private _dataTable As New sgsmsdb.viewtblproductsDataTable
    Private _subject As IObservablePanel

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ic As InvalidCastException
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapter.Fill(_dataTable)
        ProductDataGridView.ItemsSource = _dataTable.DefaultView
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New ProductDialog(subject:=_subject))
    End Sub

    Private Sub ProductDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductDataGridView.SelectionChanged
        If ProductDataGridView.SelectedItems.Count > 0 Then
            Dialog.Show(New ProductDialog(_subject, ProductDataGridView.SelectedItems(0)))
            ProductDataGridView.SelectedIndex = -1
        End If
    End Sub

    Private Sub ProductSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles ProductSearch.SearchStarted
        ProductDataGridView.ItemsSource = BaseProduct.Search(ProductSearch.Text).DefaultView
    End Sub
End Class
