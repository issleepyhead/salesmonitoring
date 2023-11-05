Imports System.Windows.Interop
Imports HandyControl.Controls
Imports SalesMonitoringSystem.My

Public Class AccountsPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblusersTableAdapter
    Private _dataTable As New sgsmsdb.viewtblusersDataTable
    Private _subject As IObservablePanel

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapter.Fill(_dataTable)
        AccountsDataGridView.ItemsSource = _dataTable.DefaultView
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New AccountDialog(subject:=_subject))
    End Sub

    Private Sub AccountsDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles AccountsDataGridView.SelectionChanged
        If AccountsDataGridView.SelectedItems.Count > 0 Then
            Dim cols As IList(Of DataGridCellInfo) = AccountsDataGridView.SelectedCells()
            Dim full_name As String() = cols.Item(0).Item(1).ToString.Split(" ")
            Dim data As New Dictionary(Of String, String) From {
                {"id", cols.Item(0).Item(0)},
                {"role_id", ScalarRoleName(cols.Item(0).Item(2))},
                {"first_name", String.Join(" ", full_name.Take(full_name.Count - 1))},
                {"last_name", full_name.Last},
                {"address", cols.Item(0).Item(3)},
                {"contact", cols.Item(0).Item(4)},
                {"username", cols.Item(0).Item(5)},
                {"password", cols.Item(0).Item(6)}
            }

            Dialog.Show(New AccountDialog(data, _subject))
            AccountsDataGridView.SelectedIndex = -1
        End If
    End Sub

End Class
