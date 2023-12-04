Imports System.Data
Imports HandyControl.Controls

Public Class StackNotificationControl
    Public Property _parent As IObservablePanel
    Private Sub StackNotificationControl_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        Dim transaction_dialog As New TransactionDialog()
        transaction_dialog.ReferenceNumberLabel.Text = LabelStackHeading.Text
        transaction_dialog._is_from_notif = True
        BaseTransaction.UpdateStatus(LabelStackHeading.Text)
        Dialog.Show(transaction_dialog)
        _parent?.NotifyObserver()
    End Sub
End Class
