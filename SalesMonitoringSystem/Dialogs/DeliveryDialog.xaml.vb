Imports System.Data
Imports System.Diagnostics.Tracing
Imports System.Reflection
Imports HandyControl.Controls
Imports SalesMonitoringSystem.sgsmsdbTableAdapters

Public Class DeliveryDialog
    Private _parent As DeliveryCartDialog
    Private _data As DataRowView
    Private _tableAdapter As New viewtblproductsTableAdapter
    Private ID_NOT_SET = -1
    Public Sub New(
        Optional parent As DeliveryCartDialog = Nothing,
        Optional data As DataRowView = Nothing
    )
        InitializeComponent()
        _data = data
        _parent = parent
        If _data Is Nothing Then
            DeleteButton.Visibility = Visibility.Collapsed
        Else
            DataContext = _data
            SaveButton.Content = "UPDATE"
        End If
    End Sub


    ''' <summary>
    ''' Initialize all the necessary data of the dialog.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DeliveryDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ProductNameComboBox.ItemsSource = _tableAdapter.GetData().DefaultView
        ProductNameComboBox.DisplayMemberPath = "PRODUCT_NAME"
        ProductNameComboBox.SelectedValuePath = "ID"

        If _data IsNot Nothing Then
            ProductNameComboBox.SelectedValue = _data.Item("PRODUCT_ID")
            ProductNameComboBox.IsEnabled = False
            CostPriceTextBox.IsEnabled = False
            SellingPriceTextBox.IsEnabled = False
            DeleteButton.Visibility = Visibility.Collapsed
            SaveButton.Visibility = Visibility.Collapsed
            QuantityTextBox.IsEnabled = False
        Else
            ProductNameComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        'Dim controls As Object() = {QuantityTextBox}
        'Dim types As DataInput() = {DataInput.STRING_INTEGER, DataInput.STRING_INTEGER}

        '' Validate the inputs
        'Dim res As New List(Of Object())
        'For idx = 0 To controls.Count - 1
        '    res.Add(InputValidation.ValidateInputString(controls(idx), types(idx)))
        'Next
        Dim res As Object() = InputValidation.ValidateInputString(QuantityTextBox, DataInput.STRING_INTEGER)
        If res(0) Then
            If ProductNameComboBox.SelectedIndex = -1 Then
                Growl.Info("Please select a product first.")
                Return
            End If
            If _data Is Nothing Then
                ' Temporarily adding the data to the datagrid
                Dim is_existing As Boolean = False
                For Each item As DataRow In _parent._itemSource.Rows
                    If item.Item("PRODUCT_NAME") = ProductNameComboBox.Text AndAlso item.Item("COST") = CostPriceTextBox.Text Then
                        item.Item("QUANTITY") = CStr(CInt(QuantityTextBox.Text) + item.Item("QUANTITY"))
                        item.Item("TOTAL") += CInt(CostPriceTextBox.Text) * QuantityTextBox.Text
                        is_existing = True
                        Exit For
                    End If
                Next

                ' If the product is not yet existing then add it to the cart
                If Not is_existing Then
                    _parent._itemSource.Rows.Add({
                        ID_NOT_SET, ProductNameComboBox.SelectedValue,
                        _parent.SupplierNameComboBox.SelectedValue,
                        ProductNameComboBox.Text, QuantityTextBox.Text,
                        SellingPriceTextBox.Text, CostPriceTextBox.Text,
                        CInt(CostPriceTextBox.Text) * QuantityTextBox.Text
                    })
                End If
            Else
                ' Update the data value
                _data.Row.Item("PRODUCT_ID") = ProductNameComboBox.SelectedValue
                _data.Item("PRODUCT_NAME") = ProductNameComboBox.Text
                _data.Item("PRICE") = SellingPriceTextBox.Text
                _data.Item("TOTAL") = CInt(CostPriceTextBox.Text) * QuantityTextBox.Text
                _data.Item("QUANTITY") = CInt(QuantityTextBox.Text)
            End If
            _parent.UpdateVisual()
            CloseDialog(Closebtn)
        End If

    End Sub

    Private Sub ProductNameComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductNameComboBox.SelectionChanged
        If ProductNameComboBox.SelectedIndex <> -1 Then
            Dim product_info As DataTable = BaseProduct.ProductInfo(ProductNameComboBox.SelectedValue)
            SellingPriceTextBox.Text = product_info.Rows.Item(0).Item("PRICE").ToString
            CostPriceTextBox.Text = product_info.Rows.Item(0).Item("COST_PRICE").ToString
        Else
            SellingPriceTextBox.Text = "None"
            CostPriceTextBox.Text = "None"
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        If _data.Item("ID") <> ID_NOT_SET Then
            Dim baseCommand As New BaseDeliveryCart(New Dictionary(Of String, String) From {
                {"delivery_id", _data.Item("ID")}
            })
            Dim invoker As ICommandInvoker = New DeleteCommand(baseCommand)
            invoker.Execute()
        End If
        _parent._itemSource.Rows.Remove(_data.Row)
        _parent.UpdateVisual()
        CloseDialog(Closebtn)
    End Sub
End Class
