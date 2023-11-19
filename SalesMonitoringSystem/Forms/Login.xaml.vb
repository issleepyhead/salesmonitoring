Imports BCrypt.Net
Imports HandyControl.Controls
Public Class Login
    Private _loginModule As New LoginModule

    Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs) Handles LoginButton.Click
        Dim res As Object() = Nothing
        Dim controls As Object() = {UsernameTextBox, PasswordTextBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_STRING}

        UsernameTextBox.BorderBrush = Brushes.Red

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
