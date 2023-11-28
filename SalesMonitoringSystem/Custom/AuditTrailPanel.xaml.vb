Imports HandyControl.Data

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
            PaginationLog.Visibility = Visibility.Collapsed
            Return
        End If

        PaginationConfig(_dataTable, PaginationLog)
    End Sub

    Private Sub Pagination_PageUpdated(sender As Object, e As HandyControl.Data.FunctionEventArgs(Of Integer)) Handles PaginationLog.PageUpdated
        TransactionsDataGridView.ItemsSource = _dataTable.Skip((e.Info - 1) * MAX_PAGE_COUNT).Take(MAX_PAGE_COUNT)
    End Sub

    Private Sub LogSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles LogSearch.SearchStarted
        _dataTable = BaseAudit.Search(LogSearch.Text)
        TransactionsDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig(_dataTable, PaginationLog)
    End Sub
End Class
