Imports HandyControl.Controls

Public Class CategoryDialog
    Private _data As Dictionary(Of String, String)
    Private _subject As IObservablePanel
    Public Sub New(Optional data As Dictionary(Of String, String) = Nothing, Optional subject As IObservablePanel = Nothing)
        InitializeComponent()
        _data = data
        _subject = subject
        If data Is Nothing Then
            DeleteCategoryButton.Visibility = Visibility.Collapsed
        Else
            SaveCategoryButton.Content = "UPDATE"
        End If
    End Sub

    Private Sub CategoryDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CategoryParentComboBox.ItemsSource = BaseCategory.FillByParentCategory().DefaultView
        CategoryParentComboBox.DisplayMemberPath = "category_name"
        CategoryParentComboBox.SelectedValuePath = "id"

        If _data IsNot Nothing Then
            CategoryNameTextBox.Text = _data.Item("category_name")
            CategoryDescriptionTextBox.Text = _data.Item("category_description")
            CategoryParentComboBox.SelectedValue = _data.Item("parent_id")
        Else

        End If
    End Sub


    Private Sub SaveCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click
        Dim controls As Object() = {CategoryDescriptionTextBox, CategoryNameTextBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_STRING}

        Dim result As New List(Of Object())
        For i = 0 To controls.Count - 1
            result.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not result.Any(Function(item As Object()) Not item(0)) Then
            If BaseCategory.Exists(result(1)(1)) = 0 Then
                Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("id")},
                    {"parent_id", If(CategoryParentComboBox.SelectedIndex = -1, "NULL", CategoryParentComboBox.SelectedValue)},
                    {"category_name", result(1)(1)},
                    {"category_description", result(0)(1)}
                }

                Dim baseCommand As New BaseCategory(data)
                Dim invoker As ICommandInvoker
                If _data Is Nothing Then
                    invoker = New AddCommand(baseCommand)
                Else
                    invoker = New UpdateCommand(baseCommand)
                End If

                invoker.Execute()
                _subject.NotifyObserver()
                CloseDialog(Closebtn)
            Else
                Growl.Info("Category exists!")
            End If

        End If
    End Sub

    Private Sub DeleteCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteCategoryButton.Click
        Dim baseCommand As New BaseCategory(_data)
        Dim deleteCommand As New DeleteCommand(baseCommand)
        deleteCommand.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub
End Class
