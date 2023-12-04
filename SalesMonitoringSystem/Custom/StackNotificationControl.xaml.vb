Imports HandyControl.Controls

Public Class StackNotificationControl
    Private Sub StackNotificationControl_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        Dim transaction_dialog As New TransactionDialog()
        transaction_dialog.ReferenceNumberLabel.Text = LabelStackHeading.Text
        transaction_dialog._is_from_notif = True
        Dialog.Show(transaction_dialog)
    End Sub
End Class
