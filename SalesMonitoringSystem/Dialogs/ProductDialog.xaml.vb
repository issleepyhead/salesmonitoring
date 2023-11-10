Imports System.Data
Imports HandyControl.Controls

Public Class ProductDialog
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _subject As IObservablePanel
    Private _data As DataRowView
    Public Sub New(
        Optional subject As IObservablePanel = Nothing,
        Optional data As DataRowView = Nothing
    )
        InitializeComponent()

        _data = data
        _subject = subject
        DataContext = _data
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
        Dim controls As Object() = {ProductNameTextBox, ProductDescriptionTextBox, ProductPriceTextBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_STRING, DataInput.STRING_INTEGER}

        Dim res As New List(Of Object())
        For i = 0 To controls.Count - 1
            res.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not res.Any(Function(item As Object()) Not item(0)) Then
            If BaseProduct.Exists(res(0)(1), res(2)(1), CategoryComboBox.SelectedValue) <= 0 Then
                Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("ID")},
                    {"category_id", CategoryComboBox.SelectedValue},
                    {"product_name", ProductNameTextBox.Text},
                    {"product_description", ProductDescriptionTextBox.Text},
                    {"product_price", ProductPriceTextBox.Text}
                }

                Dim baseCommand As New BaseProduct(data)
                Dim invoker As ICommandInvoker
                If _data Is Nothing Then
                    invoker = New AddCommand(baseCommand)
                Else
                    invoker = New UpdateCommand(baseCommand)
                End If

                invoker.Execute()
            Else
                Growl.Info("Product exists!")

            End If
            _subject.NotifyObserver()
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
End Class
