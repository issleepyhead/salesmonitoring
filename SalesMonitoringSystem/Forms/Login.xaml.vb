
Imports HandyControl.Controls
Public Class Login
    Private _loginModule As New LoginModule

    Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs) Handles LoginButton.Click
        Dim res As Object() = Nothing
        Dim controls As Object() = {UsernameTextBox, PasswordTextBox}
        Dim types As DataInput() = {DataInput.STRING_USERNAME, DataInput.STRING_PASSWORD}

        If BaseAccount.CountUser() = 0 Then
            Dim res1 As MessageBoxResult = MessageBox.Ask("This account will be created as a default user of this system.", "Create this account as default?")
            If res1 = MessageBoxResult.OK Then
                Dim pwd As String = PasswordTextBox.Password
                Dim data As New Dictionary(Of String, String) From {
                        {"id", Nothing},
                        {"role_id", 1},
                        {"first_name", "John"},
                        {"last_name", "Doe"},
                        {"address", "Taguig City"},
                        {"contact", "09123456789"},
                        {"username", UsernameTextBox.Text},
                        {"password", pwd}
                    }
                Dim baseCommand As New BaseAccount(data)
                Dim invoker As New AddCommand(baseCommand)
                invoker.Execute()
                MessageBox.Info("Please log in your account.")
            Else
                MessageBox.Info("Please create a default account.")
 
            End If
            UsernameTextBox.Text = ""
            PasswordTextBox.Password = ""
            Return
        End If

        Dim vres As New List(Of Object())
        For i = 0 To controls.Count - 1
            vres.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not vres.Any(Function(item As Object()) Not item(0)) Then
            res = _loginModule.LoginAccount(UsernameTextBox.Text, PasswordTextBox.Password)

            If res?(0) Then
                Dim dash As New Dashboard
                If My.Settings.userRole <> 1 Then
                    dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                    dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                    Dim tabs As ItemCollection = dash.MaintainanceContainer.TabControlContainer.Items()
                    tabs.Remove(dash.MaintainanceContainer.AccountTab)
                    tabs.Remove(dash.MaintainanceContainer.CategoryTab)
                    tabs.Remove(dash.MaintainanceContainer.SupplierTab)
                End If
                dash.Show()
                Close()
            Else
                MessageBox.Info(res(1), "Login Failed!")
            End If
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
        Close()
    End Sub
End Class
