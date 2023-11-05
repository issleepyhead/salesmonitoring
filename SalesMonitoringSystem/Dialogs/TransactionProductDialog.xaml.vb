Imports System.Data
Imports SalesMonitoringSystem.sgsmsdbTableAdapters
Public Class TransactionProductDialog
    Private _tableAdapter As New viewtblproductsTableAdapter
    Private _data As DataRowView
    Private ID_NOT_SET As Integer = -1
    Private _parent As TransactionDialog
    Public Sub New(
        Optional parent As TransactionDialog = Nothing,
        Optional data As DataRowView = Nothing
    )
        InitializeComponent()
        _data = data
        _parent = parent
        If _data IsNot Nothing Then
            DataContext = _data
            SaveButton.Content = "ITEM INFO"
            SaveButton.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub TransactionProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ProductNameComboBox.ItemsSource = _tableAdapter.GetData().DefaultView
        ProductNameComboBox.SelectedValuePath = "ID"
        ProductNameComboBox.DisplayMemberPath = "PRODUCT_NAME"

        If _data IsNot Nothing Then
            ProductNameComboBox.SelectedValue = _data.Item("PRODUCT_ID")
        Else
            ProductNameComboBox.SelectedIndex = 0
            ProductNameComboBox_SelectionChanged(ProductNameComboBox, Nothing)
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        If InputValidation.ValidateInputString(QuantityTextBox, DataInput.STRING_INTEGER)(0) Then
            _parent._itemSource.Rows.Add({
                 ID_NOT_SET, ProductNameComboBox.SelectedValue,
                 ProductNameComboBox.Text, SellingPriceTextBox.Text, QuantityTextBox.Text,
                 CInt(SellingPriceTextBox.Text) * QuantityTextBox.Text
            })
            _parent.UpdateVisual()
            CloseDialog(Closebtn)
        End If


    End Sub

    Private Sub ProductNameComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductNameComboBox.SelectionChanged
        SellingPriceTextBox.Text = CStr(ScalarPrice(ProductNameComboBox.SelectedValue))
    End Sub
End Class
