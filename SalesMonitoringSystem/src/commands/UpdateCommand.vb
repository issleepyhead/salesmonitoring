Public Class UpdateCommand
    Implements ICommandInvoker

    Private _base As ICommandPanel
    Public Sub New(base As ICommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Update()
    End Sub
End Class
