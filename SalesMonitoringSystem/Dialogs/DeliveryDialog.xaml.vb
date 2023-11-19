Imports System.Data
Imports System.Reflection
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
        Else
            ProductNameComboBox.SelectedIndex = 0
            ProductNameComboBox_SelectionChanged(ProductNameComboBox, Nothing)
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim controls As Object() = {CostPriceTextBox, QuantityTextBox}
        Dim types As DataInput() = {DataInput.STRING_INTEGER, DataInput.STRING_INTEGER}

        ' Validate the inputs
        Dim res As New List(Of Object())
        For idx = 0 To controls.Count - 1
            res.Add(InputValidation.ValidateInputString(controls(idx), types(idx)))
        Next

        If Not res.Any(Function(item As Object()) Not item(0)) Then
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
                        ProductNameComboBox.Text, res(1)(1),
                        SellingPriceTextBox.Text, res(0)(1),
                        CInt(res(0)(1)) * res(1)(1)
                    })
                End If
            Else
                ' Update the data value
                _data.Row.Item("PRODUCT_ID") = ProductNameComboBox.SelectedValue
                _data.Item("PRODUCT_NAME") = ProductNameComboBox.Text
                _data.Item("PRICE") = SellingPriceTextBox.Text
                _data.Item("TOTAL") = CInt(res(0)(1)) * res(1)(1)
                _data.Item("QUANTITY") = CInt(res(1)(1))
            End If
            _parent.UpdateVisual()
            CloseDialog(Closebtn)
        End If

    End Sub

    Private Sub ProductNameComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductNameComboBox.SelectionChanged
        SellingPriceTextBox.Text = CStr(BaseProduct.ScalarPrice(ProductNameComboBox.SelectedValue))
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
