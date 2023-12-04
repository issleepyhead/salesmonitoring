Imports HandyControl.Controls
Imports HandyControl.Data

Public Class TransactionsPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtbltransactionsTableAdapter
    Private _dataTable As New sgsmsdb.viewtbltransactionsDataTable
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
        TransactionsDataGridView.ItemsSource = _dataTable.DefaultView
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New TransactionDialog(subject:=_subject))
    End Sub

    Private Sub TransactionsDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles TransactionsDataGridView.SelectionChanged
        If TransactionsDataGridView.SelectedItems.Count > 0 Then
            Dialog.Show(New TransactionDialog(TransactionsDataGridView.SelectedItems(0), _subject))
            TransactionsDataGridView.SelectedIndex = -1
        End If
    End Sub

    Private Sub TransactionSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles TransactionSearch.SearchStarted
        TransactionsDataGridView.ItemsSource = BaseTransaction.Search(TransactionSearch.Text).DefaultView
    End Sub

    Private Sub ButtonFilterDate_Click(sender As Object, e As RoutedEventArgs) Handles ButtonFilterDate.Click
        If DatePickerFirstDate.SelectedDate < DatePickerSecondDate.SelectedDate Then
            Dim fd As Date = DatePickerFirstDate.SelectedDate, sd As Date = DatePickerSecondDate.SelectedDate
            _dataTable = BaseTransaction.FilterTransactionsByDate(fd, sd)
            TransactionsDataGridView.ItemsSource = _dataTable
        Else
            Growl.Info("The first date should be less than the first.")
        End If
    End Sub
End Class
