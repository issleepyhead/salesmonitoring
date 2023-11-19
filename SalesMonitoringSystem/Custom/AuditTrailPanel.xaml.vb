Public Class AuditTrailPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtbllogsTableAdapter
    Private _dataTable As New sgsmsdb.viewtbllogsDataTable
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
        TransactionsDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        If _dataTable.Count <= MAX_PAGE_COUNT Then
            Pagination.Visibility = Visibility.Collapsed
            Return
        End If

        If MAX_PAGE_COUNT / _dataTable.Count < 0 Then
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT + 1
        Else
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT
        End If
    End Sub

    Private Sub Pagination_PageUpdated(sender As Object, e As HandyControl.Data.FunctionEventArgs(Of Integer)) Handles Pagination.PageUpdated
        TransactionsDataGridView.ItemsSource = _dataTable.Skip((e.Info - 1) * MAX_PAGE_COUNT).Take(MAX_PAGE_COUNT)
    End Sub
End Class
