Imports System.Data
Imports HandyControl.Controls
Imports SalesMonitoringSystem.sgsmsdb

Public Class DeliveryCartDialog
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblsuppliersTableAdapter
    Private _data As viewtbldeliveriesRow
    Private _subject As IObservablePanel
    Private Const ID_NOT_SET As Integer = -1
    Public _itemSource As DataTable
    Public Sub New(
        Optional data As viewtbldeliveriesRow = Nothing,
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
            _itemSource.Columns.Add("SUPPLIER_ID")
            _itemSource.Columns.Add("PRODUCT_NAME")
            _itemSource.Columns.Add("QUANTITY")
            _itemSource.Columns.Add("PRICE")
            _itemSource.Columns.Add("COST")
            _itemSource.Columns.Add("TOTAL")

            DeliveryDate.SelectedDate = Date.Now
            RecievedButton.Visibility = Visibility.Collapsed
            CancelButton.Visibility = Visibility.Collapsed
        Else
            Dim values As String() = _data.Item("DELIVERY_DATE").Split("/")
            DeliveryDate.SelectedDate = New Date(values(2), values(0), values(1))
            DeliveryDate.IsEnabled = False
            SupplierNameComboBox.IsEnabled = False
            SaveButton.Visibility = Visibility.Collapsed
            RecievedButton.Margin = New Thickness(0, 0, 30, 0)
            AddItemButton.Visibility = Visibility.Collapsed

        End If
    End Sub

    Private Sub AddItemButton_Click(sender As Object, e As RoutedEventArgs) Handles AddItemButton.Click
        Dialog.Show(New DeliveryDialog(parent:=Me))
    End Sub

    ''' <summary>
    ''' Loaded is only used here to initialize all the controls and data.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DeliveryCartDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim supplier_data As viewtblsuppliersDataTable = _tableAdapter.GetData()
        supplier_data.Rows.Add({-1, "None", "", "", ""})
        SupplierNameComboBox.ItemsSource = supplier_data
        SupplierNameComboBox.DisplayMemberPath = "SUPPLIER_NAME"
        SupplierNameComboBox.SelectedValuePath = "ID"

        If _data IsNot Nothing Then
            _itemSource = BaseDeliveryCart.FillByProductDelivery(_data.Item("REFERENCE_NUMBER"))
            ItemsDataGridView.ItemsSource = _itemSource.DefaultView
            If _itemSource.Rows.Count > 0 Then
                SupplierNameComboBox.SelectedValue = _itemSource.Rows(0).Item("SUPPLIER_ID")
            End If

            If _data.Item("STATUS") <> "Pending" Then
                RecievedButton.Visibility = Visibility.Collapsed
                CancelButton.Visibility = Visibility.Collapsed
            End If
            UpdateVisual()
        Else
            ReferenceNumberLabel.Text = GenInvoiceNumber(InvoiceType.Delivery)
            SupplierNameComboBox.SelectedValue = -1
        End If
    End Sub

    ''' <summary>
    ''' To update the total and datagrid data
    ''' </summary>
    Public Sub UpdateVisual()
        ItemsDataGridView.ItemsSource = _itemSource.DefaultView
        Dim total As Integer = 0
        For i = 0 To _itemSource.Rows.Count - 1
            total += _itemSource.Rows(i).Item("TOTAL")
        Next
        CostPrice.Text = total
    End Sub

    Private Sub ItemsDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ItemsDataGridView.SelectionChanged
        If _data IsNot Nothing Then
            If ItemsDataGridView.SelectedItems.Count > 0 Then
                Dim ddialog As New DeliveryDialog(Me, ItemsDataGridView.SelectedItems(0))
                ddialog._is_from_delivery = False
                Dialog.Show(ddialog)
                ItemsDataGridView.SelectedIndex = -1
            End If
        Else
            Dialog.Show(New DeliveryDialog(Me, ItemsDataGridView.SelectedItems(0)))
        End If
        ItemsDataGridView.SelectedIndex = -1
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click


        If _itemSource.Rows.Count <> 0 Then
            ' Check if the created delivery date is valid
            If CDate(DeliveryDate.SelectedDate.Value) < Date.Now.AddDays(-1) Then
                Growl.Info("Invalid delivery date.")
                Return
            End If

            If SupplierNameComboBox.SelectedValue = -1 OrElse SupplierNameComboBox.SelectedIndex = -1 Then
                Growl.Info("Please select a supplier")
                Return
            End If

            For Each row As DataRow In _itemSource.Rows
                row.Item("SUPPLIER_ID") = SupplierNameComboBox.SelectedValue
                Dim data = New Dictionary(Of String, String) From {
                        {"id", row.Item("ID")},
                        {"product_id", row.Item("PRODUCT_ID")},
                        {"product_cost", row.Item("COST")},
                        {"quantity", row.Item("QUANTITY")},
                        {"reference_number", ReferenceNumberLabel.Text},
                        {"delivery_date", DeliveryDate.SelectedDate.Value},
                        {"supplier_id", row.Item("SUPPLIER_ID")}
                }
                Dim baseCommand = New BaseDeliveryCart(data)
                Dim invoker As ICommandInvoker
                If row.Item("ID") = ID_NOT_SET Then
                    invoker = New AddCommand(baseCommand)
                Else
                    invoker = New UpdateCommand(baseCommand)
                End If
                invoker.Execute()
            Next
        Else
            Growl.Info("Please add an item to the cart first.")
            Return
        End If

        If _data Is Nothing Then
            Growl.Success("Delivery has been added successfully!")
        Else
            Growl.Success("Delivery has been updated successfully!")
        End If
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub



    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs) Handles CancelButton.Click
        Dim baseCommand As New BaseDeliveryCart(New Dictionary(Of String, String) From {
            {"reference_number", ReferenceNumberLabel.Text}
        })
        Dim invoker As New CancelCommand(baseCommand)
        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub RecievedButton_Click(sender As Object, e As RoutedEventArgs) Handles RecievedButton.Click
        Dim baseCommand As New BaseDeliveryCart(New Dictionary(Of String, String) From {
            {"reference_number", ReferenceNumberLabel.Text}
        })
        Dim invoker As New RecieveCommand(baseCommand)
        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub
End Class
