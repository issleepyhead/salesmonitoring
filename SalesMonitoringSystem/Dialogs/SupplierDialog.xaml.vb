Imports HandyControl.Controls

Public Class SupplierDialog
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
        Dim controls As Object() = {SupplierNameTextBox, SupplierAddressTextBox, SupplierContactTextBox}
        Dim types As DataInput() = {DataInput.STRING_NAME, DataInput.STRING_STRING, DataInput.STRING_PHONE}
        Dim res As New List(Of Object())

        For idx = 0 To controls.Count - 1
            res.Add(InputValidation.ValidateInputString(controls(idx), types(idx)))
        Next

        If Not res.Any(Function(item As Object()) Not item(0)) Then
            If BaseSupplier.Exists(res(0)(1), res(1)(1)) <= 0 Then
                Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("id")},
                    {"supplier_name", SupplierNameTextBox.Text},
                    {"supplier_contact", SupplierContactTextBox.Text},
                    {"supplier_address", SupplierAddressTextBox.Text}
                }
                Dim baseCommand As New BaseSupplier(data)
                Dim invoker As ICommandInvoker
                If _data Is Nothing Then
                    invoker = New AddCommand(baseCommand)
                Else
                    invoker = New UpdateCommand(baseCommand)
                End If

                invoker.Execute()
                _subject.NotifyObserver()
                CloseDialog(Closebtn)
            Else
                Growl.Info("Supplier exists!")
            End If
        End If
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        Dim baseCommand As New BaseSupplier(_data)
        Dim invoker As New DeleteCommand(baseCommand)

        invoker.Execute()
        _subject.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub SupplierDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If _data IsNot Nothing Then
            SupplierNameTextBox.Text = _data.Item("supplier_name")
            SupplierAddressTextBox.Text = _data.Item("supplier_address")
            SupplierContactTextBox.Text = _data.Item("supplier_contact")
        End If
    End Sub
End Class
