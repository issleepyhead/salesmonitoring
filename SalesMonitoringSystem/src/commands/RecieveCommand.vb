Public Class RecieveCommand
    Implements ICommandInvoker
    Private _base As IDeliveryCommandPanel

    Public Sub New(base As IDeliveryCommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Recieve()
    End Sub
End Class
