Public Class SuppliersPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblsuppliersTableAdapter
    Private _dataTable As New sgsmsdb.viewtblsuppliersDataTable
    Private _subject As IObservablePanel

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.Cast(Of Dashboard).FirstOrDefault
            If _subject IsNot Nothing Then
                _subject.RegisterObserver(Me)
                _subject.NotifyObserver()
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapter.Fill(_dataTable)
        SuppliersDataGridView.ItemsSource = _dataTable.DefaultView
    End Sub
End Class
