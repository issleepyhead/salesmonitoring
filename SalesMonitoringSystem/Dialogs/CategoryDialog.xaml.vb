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
        If _data IsNot Nothing Then
            CategoryNameTextBox.Text = _data.Item("category_name")
            CategoryDescriptionTextBox.Text = _data.Item("category_description")
        End If
    End Sub


    Private Sub SaveCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click
        Dim controls As Object() = {CategoryNameTextBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_STRING}

        Dim result As New List(Of Object())
        For i = 0 To controls.Count - 1
            result.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        Dim baseCommand As BaseCategory = Nothing
        Dim invoker As ICommandInvoker = Nothing
        If Not result.Any(Function(item As Object()) Not item(0)) Then
            Dim data As New Dictionary(Of String, String) From {
                {"id", _data?.Item("id")},
                {"category_name", result(0)(1)},
                {"category_description", If(String.IsNullOrEmpty(CategoryDescriptionTextBox.Text), "", CategoryDescriptionTextBox.Text)},
                {"parent_id", ""}
            }

            If BaseCategory.Exists(result(0)(1)) = 0 AndAlso _data Is Nothing Then
                baseCommand = New BaseCategory(data)
                invoker = New AddCommand(baseCommand)
            ElseIf _data IsNot Nothing Then
                baseCommand = New BaseCategory(Data)
                invoker = New UpdateCommand(baseCommand)
            Else
                Growl.Info("Category exists!")
            End If

            invoker?.Execute()
            _subject.NotifyObserver()
            CloseDialog(Closebtn)
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
