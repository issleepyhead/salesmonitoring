Imports System.Data
Imports HandyControl.Controls
Imports SalesMonitoringSystem.sgsmsdb

Public Class ProductDialog
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _subject As IObservablePanel
    Private _data As viewtblproductsRow = Nothing
    Public Sub New(
        Optional subject As IObservablePanel = Nothing,
        Optional data As viewtblproductsRow = Nothing
    )
        InitializeComponent()

        _data = data
        _subject = subject
        DataContext = data
        If _data IsNot Nothing Then
            SaveButton.Content = "UPDATE"
        Else
            DeleteButton.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Sub ProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CategoryComboBox.ItemsSource = _tableAdapter.GetData().DefaultView
        CategoryComboBox.DisplayMemberPath = "CATEGORY_NAME"
        CategoryComboBox.SelectedValuePath = "ID"

        If _data Is Nothing Then
            CategoryComboBox.SelectedIndex = 0
        Else
            CategoryComboBox.SelectedValue = _data.Item("CATEGORY_ID")
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim controls As Object() = {ProductNameTextBox, ProductPriceTextBox, ProductCostTextBox, CategoryComboBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_INTEGER, DataInput.STRING_INTEGER, DataInput.STRING_STRING}

        Dim res As New List(Of Object())
        For i = 0 To controls.Count - 1
            res.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        Dim baseCommand As ICommandPanel = Nothing
        Dim invoker As ICommandInvoker = Nothing
        If Not res.Any(Function(item As Object()) Not item(0)) Then
            Dim data As New Dictionary(Of String, String) From {
                {"id", _data?.Item("ID")},
                {"category_id", CategoryComboBox.SelectedValue},
                {"product_name", res(0)(1)},
                {"product_description", If(String.IsNullOrEmpty(ProductDescriptionTextBox.Text), "", ProductDescriptionTextBox.Text)},
                {"product_price", res(1)(1)},
                {"product_cost", res(2)(1)}
            }
            baseCommand = New BaseProduct(data)
            If BaseProduct.Exists(res(0)(1)) <= 0 AndAlso _data Is Nothing Then
                invoker = New AddCommand(baseCommand)
            ElseIf _data IsNot Nothing Then
                invoker = New UpdateCommand(baseCommand)
            Else
                Growl.Info("Product exists!")
            End If

            invoker?.Execute()
            _subject?.NotifyObserver()
            CloseDialog(Closebtn)
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        Dim baseCommand As New BaseProduct(New Dictionary(Of String, String) From {{"id", _data.Item("ID")}})
        Dim invoker As New DeleteCommand(baseCommand)

        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub Closebtn_Click(sender As Object, e As RoutedEventArgs) Handles Closebtn.Click
        _subject.NotifyObserver()
    End Sub
End Class
