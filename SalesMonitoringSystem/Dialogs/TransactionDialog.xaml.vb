Imports HandyControl.Controls
Imports System.Data

Public Class TransactionDialog
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblsuppliersTableAdapter
    Private _data As DataRowView
    Private _subject As IObservablePanel
    Private Const ID_NOT_SET As Integer = -1
    Public _itemSource As DataTable
    Public _is_from_notif As Boolean = False
    Public Sub New(
        Optional data As DataRowView = Nothing,
        Optional subject As IObservablePanel = Nothing
    )
        InitializeComponent()
        _data = data
        _subject = subject
        DataContext = _data

        If _data Is Nothing Then
            ' Create a data table to populate the item source
            _itemSource = New DataTable
            _itemSource.Columns.Add("ID")
            _itemSource.Columns.Add("PRODUCT_ID")
            _itemSource.Columns.Add("PRODUCT_NAME")
            _itemSource.Columns.Add("PRICE")
            _itemSource.Columns.Add("QUANTITY")
            _itemSource.Columns.Add("TOTAL")
        Else
            SaveButton.Visibility = Visibility.Collapsed
            AddItemButton.Visibility = Visibility.Collapsed
            ParentPanel.RowDefinitions.Item(2).Height = New GridLength(0)
            ParentPanel.RowDefinitions.Item(4).Height = New GridLength(0)
        End If
    End Sub

    Private Sub AddItemButton_Click(sender As Object, e As RoutedEventArgs) Handles AddItemButton.Click
        Dialog.Show(New TransactionProductDialog(parent:=Me))
    End Sub

    ''' <summary>
    ''' Loaded is only used here to initialize all the necessary data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DeliveryCartDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        If _data IsNot Nothing OrElse _is_from_notif Then
            SaveButton.Visibility = Visibility.Collapsed
            AddItemButton.Visibility = Visibility.Collapsed
            ParentPanel.RowDefinitions.Item(2).Height = New GridLength(0)
            ParentPanel.RowDefinitions.Item(ParentPanel.RowDefinitions.Count - 1).Height = New GridLength(0)
            _itemSource = BaseTransaction.FillByProductTransaction(ReferenceNumberLabel.Text)
            UpdateVisualData()
        Else
            ReferenceNumberLabel.Text = GenInvoiceNumber(InvoiceType.Transaction)
        End If
    End Sub

    Public Sub UpdateVisualData()
        ItemsDataGridView.ItemsSource = _itemSource?.DefaultView
        Dim total As Integer = 0
        For i = 0 To _itemSource?.Rows.Count - 1
            total += _itemSource.Rows(i).Item("TOTAL")
        Next
        TotalPrice.Text = total
    End Sub

    Private Sub ItemsDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ItemsDataGridView.SelectionChanged
        If _data Is Nothing AndAlso Not _is_from_notif Then
            If ItemsDataGridView.SelectedItems.Count > 0 Then
                Dim data As DataRowView = ItemsDataGridView.SelectedItems(0)
                Dialog.Show(New TransactionProductDialog(Me, data))
                ItemsDataGridView.SelectedIndex = -1
            End If
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        If _itemSource.Rows.Count <> 0 Then
            For Each row As DataRow In _itemSource.Rows
                Dim invoker As ICommandInvoker
                Dim data = New Dictionary(Of String, String) From {
                        {"id", row.Item("ID")},
                        {"product_id", row.Item("PRODUCT_ID")},
                        {"invoice_number", ReferenceNumberLabel.Text},
                        {"quantity", row.Item("QUANTITY")},
                        {"total_amount", row.Item("TOTAL")}
                }
                Dim baseCommand As New BaseTransaction(data)
                invoker = New AddCommand(baseCommand)
                invoker.Execute()
            Next
        Else
            Growl.Info("Please add an item to the cart first.")
            Return
        End If

        Growl.Info("Transaction has been added successfully!")
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub
End Class
