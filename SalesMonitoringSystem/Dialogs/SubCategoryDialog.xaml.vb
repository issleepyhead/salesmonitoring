Imports System.Data
Imports HandyControl.Controls

Public Class SubCategoryDialog

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

    Private Sub SubCategoryDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim parent_categories As DataTable = BaseCategory.FillByParentCategory()
        ParentCategoryCheckCombobox.ItemsSource = parent_categories.DefaultView
        ParentCategoryCheckCombobox.DisplayMemberPath = "category_name"
        ParentCategoryCheckCombobox.SelectedValuePath = "id"

        If _data IsNot Nothing Then
            SubCategoryNameTextBox.Text = _data.Item("category_name")

            Dim data As DataTable = BaseCategory.GetParent(_data?.Item("id"))
            For Each item As DataRowView In ParentCategoryCheckCombobox.Items
                For Each dataitem As DataRow In data.Rows
                    If Not DBNull.Value.Equals(item(0)) OrElse item(0) = dataitem(0) Then
                        ParentCategoryCheckCombobox.SelectedItems.Add(item)
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub SaveCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click
        Dim controls As Object() = {SubCategoryNameTextBox}
        Dim types As DataInput() = {DataInput.STRING_STRING}

        Dim result As New List(Of Object())
        For i = 0 To controls.Count - 1
            result.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        Dim invoker As ICommandInvoker
        If Not result.Any(Function(item As Object()) Not item(0)) Then
            If ParentCategoryCheckCombobox.SelectedItems.Count = 0 Then
                Growl.Info("Please select a parent category.")
                Return
            End If

            Dim parent_ids As String = ""
            For Each item In ParentCategoryCheckCombobox.SelectedItems
                parent_ids &= item.Item(0).ToString & ","
            Next
            parent_ids = parent_ids.TrimEnd(",")
            Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("id")},
                    {"category_name", result(0)(1)},
                    {"category_description", If(String.IsNullOrEmpty(SubCategoryDescriptionTextBox.Text), "", SubCategoryDescriptionTextBox.Text)},
                    {"parent_id", parent_ids}
            }
            Dim baseCommand As ICommandPanel = New BaseCategory(data)
            If BaseCategory.Exists(result(0)(1)) = 0 AndAlso _data Is Nothing Then
                invoker = New AddCommand(baseCommand)
            ElseIf _data IsNot Nothing Then
                invoker = New UpdateCommand(baseCommand)
            Else
                Growl.Info("Category exists!")
                Return
            End If
            invoker?.Execute()
            _subject?.NotifyObserver()
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
