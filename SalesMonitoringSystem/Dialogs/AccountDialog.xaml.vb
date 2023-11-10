Imports HandyControl.Controls

Public Class AccountDialog
    Private _data As Dictionary(Of String, String)
    Private _subject As IObservablePanel
    Public Sub New(
        Optional data As Dictionary(Of String, String) = Nothing,
        Optional subject As IObservablePanel = Nothing
    )
        InitializeComponent()
        _subject = subject
        _data = data

        If _data Is Nothing Then
            DeleteButton.Visibility = Visibility.Collapsed
        Else
            SaveButton.Content = "UPDATE"
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim controls As Object() = {
            RoleComboBox, FirstNameTextBox, LastNameTextBox, AddressTextBox, ContactTextBox,
            UsernameTextBox, PasswordTextBox
        }
        Dim types As DataInput() = {
            DataInput.STRING_STRING, DataInput.STRING_NAME, DataInput.STRING_NAME, DataInput.STRING_STRING,
            DataInput.STRING_PHONE, DataInput.STRING_USERNAME, DataInput.STRING_PASSWORD
        }

        Dim result As New List(Of Object())
        For i = 0 To controls.Count - 1
            result.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not result.Any(Function(item As Object()) Not item(0)) Then
            If BaseAccount.Exists(result(5)(1)) = 0 Then
                Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("id")},
                    {"role_id", RoleComboBox.SelectedValue},
                    {"first_name", result(1)(1)},
                    {"last_name", result(2)(1)},
                    {"address", result(3)(1)},
                    {"contact", result(4)(1)},
                    {"username", result(5)(1)},
                    {"password", result(6)(1)}
                }
                Dim baseCommand As New BaseAccount(data)
                Dim invoker As ICommandInvoker
                If _data Is Nothing Then
                    invoker = New AddCommand(baseCommand)
                Else
                    invoker = New UpdateCommand(baseCommand)
                End If
                invoker.Execute()
            Else
                Growl.Info("Username exists!")
            End If
            _subject.NotifyObserver()
            CloseDialog(Closebtn)
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        Dim baseCommand As New BaseAccount(_data)
        Dim invoker As New DeleteCommand(baseCommand)

        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub AccountDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        RoleComboBox.ItemsSource = BaseAccount.FillByRoles().DefaultView
        RoleComboBox.DisplayMemberPath = "role_name"
        RoleComboBox.SelectedValuePath = "id"

        If _data Is Nothing Then
            RoleComboBox.SelectedIndex = 0
        Else
            FirstNameTextBox.Text = _data.Item("first_name")
            LastNameTextBox.Text = _data.Item("last_name")
            RoleComboBox.SelectedValue = _data.Item("role_id")
            AddressTextBox.Text = _data.Item("address")
            UsernameTextBox.Text = _data.Item("username")
            ContactTextBox.Text = _data.Item("contact")
            PasswordTextBox.ShowEyeButton = False
        End If
    End Sub
End Class
