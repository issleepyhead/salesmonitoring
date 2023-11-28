Public Class StackNotificationControl
    Private Sub StackNotificationControl_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        MsgBox("clicked me" & LabelStackHeading.Text)
    End Sub
End Class
