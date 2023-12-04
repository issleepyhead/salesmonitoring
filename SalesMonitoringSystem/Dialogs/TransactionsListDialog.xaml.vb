Imports System.Data
Imports HandyControl.Controls

Public Class TransactionsListDialog
    Private _data As DataRowView
    Public Sub New(Optional data As DataRowView = Nothing)

        ' This call is required by the designer.
        InitializeComponent()
        _data = data
        DataContext = data
    End Sub

    Private Sub TransactionsListDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        TransactionsDataGridView.ItemsSource = BaseTransaction.FilterTransactionsByDate(Date.Parse(_data.Item("START_DATE")), Date.Parse(_data.Item("END_DATE")))
    End Sub

    Private Sub TransactionsDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles TransactionsDataGridView.SelectionChanged
        If TransactionsDataGridView.SelectedItems.Count > 0 Then
            Dialog.Show(New TransactionDialog(TransactionsDataGridView.SelectedItem))
            TransactionsDataGridView.SelectedIndex = -1
        End If
    End Sub
End Class
