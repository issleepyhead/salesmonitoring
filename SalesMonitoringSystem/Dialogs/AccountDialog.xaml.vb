Imports System.Text.RegularExpressions
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
        ' All the logic inside the save button is repeated so there must be a function to handle this all at once.
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
            ' To get rid of this indexes you should use a ViewModel next time.
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
            Dim invoker As ICommandInvoker = Nothing
            If BaseAccount.Exists(result(5)(1)) = 0 AndAlso _data Is Nothing Then
                invoker = New AddCommand(baseCommand)
            ElseIf _data IsNot Nothing Then
                invoker = New UpdateCommand(baseCommand)
            Else
                Growl.Info("Username exists!")
                Return
            End If
            invoker?.Execute()
            _subject.NotifyObserver()
            CloseDialog(Closebtn)
        Else
            MessageBox.Info("Please fill out the empty fields or provide a valid input.")
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        ' This constant should not be declared here but for the functionality purpose let's allow it.
        Const SUPER_ADMIN As Integer = &H1

        If _data.Item("role_id") <> SUPER_ADMIN Then
            If _data.Item("id") <> My.Settings.userID Then
                Dim baseCommand As New BaseAccount(_data)
                Dim invoker As New DeleteCommand(baseCommand)

                invoker.Execute()
                _subject.NotifyObserver()
                CloseDialog(Closebtn)
            Else
                MessageBox.Info("You can't delete your account.")
            End If
        Else
            MessageBox.Info("Action can't be performed.")
        End If
    End Sub



    Private Sub AccountDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        RoleComboBox.ItemsSource = BaseAccount.FillByRoles().DefaultView
        RoleComboBox.DisplayMemberPath = "role_name"
        RoleComboBox.SelectedValuePath = "id"

        If _data Is Nothing Then
            RoleComboBox.SelectedIndex = 1
        Else
            ' Would be a lot easier if I use a ViewModel here but yeah lesson learned.
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
