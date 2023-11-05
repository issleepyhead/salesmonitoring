Public Class DeleteCommand
    Implements ICommandInvoker

    Private _base As ICommandPanel
    Public Sub New(base As ICommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Delete()
    End Sub
End Class