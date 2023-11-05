Public Class AddCommand
    Implements ICommandInvoker

    Private _base As ICommandPanel
    Public Sub New(base As ICommandPanel)
        _base = base
    End Sub

    Public Sub Execute() Implements ICommandInvoker.Execute
        _base.Add()
    End Sub
End Class