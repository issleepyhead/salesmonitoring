Imports System.Data

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
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        Dim baseCommand As New BaseProduct(New Dictionary(Of String, String) From {{"id", _data.Item("ID")}})
        Dim invoker As New DeleteCommand(baseCommand)

        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub
End Class
