Public Class CancelCommand
    Implements ICommandInvoker
    Private _base As IDeliveryCommandPanel

    Public Sub New(base As IDeliveryCommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Cancel()
    End Sub
End Class
