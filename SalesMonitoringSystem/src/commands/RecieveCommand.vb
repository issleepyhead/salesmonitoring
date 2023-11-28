Public Class RecieveCommand
    Implements ICommandInvoker
    Private ReadOnly _base As IDeliveryCommandPanel

    Public Sub New(base As IDeliveryCommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Receive()
    End Sub
End Class
