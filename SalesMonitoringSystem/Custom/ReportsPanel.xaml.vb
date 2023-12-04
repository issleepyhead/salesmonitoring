Imports System.Data
Imports System.Windows.Forms

Public Class ReportsPanel
    Implements IObserverPanel
    Private _dataTable As DataTable
    Private _subject As IObservablePanel

    Public Sub New()
        InitializeComponent()
        Try
            Dim sdate As Date = New Date(Year(Date.Now), Month(Date.Now), 1)
            DatePickerFirstDate.SelectedDate = sdate
            DatePickerSecondDate.SelectedDate = sdate.AddDays(1)
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _dataTable = BaseTransaction.FetchReportransactions(DatePickerFirstDate.SelectedDate, DatePickerSecondDate.SelectedDate, ComboBoxFilter.SelectedIndex)
        ReportsDataGridView.ItemsSource = _dataTable?.DefaultView
    End Sub

    Private Sub ReportsPanel_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ComboBoxFilter.Items.Add("Weekly")
        ComboBoxFilter.Items.Add("Monthly")
        ComboBoxFilter.Items.Add("Yearly")
    End Sub

    Private Sub ComboBoxFilter_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ComboBoxFilter.SelectionChanged
        If ComboBoxFilter.SelectedIndex <> -1 Then
            _dataTable = BaseTransaction.FetchReportransactions(DatePickerFirstDate.SelectedDate, DatePickerSecondDate.SelectedDate, ComboBoxFilter.SelectedIndex + 1)
            ReportsDataGridView.ItemsSource = _dataTable?.DefaultView
        End If
    End Sub
End Class
