Imports BCrypt.Net
Imports HandyControl.Controls
Public Class Login
    Private _loginModule As New LoginModule
    Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs) Handles LoginButton.Click
        If String.IsNullOrEmpty(PasswordTextBox.Password) Then
            PasswordTextBox.BorderBrush = Brushes.Red
            Return
        Else
            PasswordTextBox.BorderBrush = New BrushConverter().ConvertFromString("#FFE0E0E0")
        End If


        If String.IsNullOrEmpty(UsernameTextBox.Text) Then
            UsernameTextBox.BorderBrush = Brushes.Red
            Return
        Else
            UsernameTextBox.BorderBrush = New BrushConverter().ConvertFromString("#FFE0E0E0")
        End If

        Dim res As Object() = _loginModule.LoginAccount(UsernameTextBox.Text, PasswordTextBox.Password)
        If res(0) Then
            Dim dash As New Dashboard
            dash.Show()
            Close()
        Else
            MessageBox.Info(res(1), "Login Failed!")
        End If
    End Sub
End Class
