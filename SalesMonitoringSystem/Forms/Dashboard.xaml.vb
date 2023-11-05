Imports HandyControl.Controls

Class Dashboard
    Implements IObservablePanel
    Private _observables As New List(Of IObserverPanel)

    Public Sub RegisterObserver(o As IObserverPanel) Implements IObservablePanel.RegisterObserver
        _observables.Add(o)
    End Sub

    Public Sub NotifyObserver() Implements IObservablePanel.NotifyObserver
        For Each o As IObserverPanel In _observables
            o.Update()
        Next
    End Sub

#Region "SwithPanelEvents"
    ' Use for panel switching
    Public Sub SwitchPanelEvents(sender As Object, e As EventArgs) Handles BottomContainerDashboardButton.Click,
        BottomContainerProductsButton.Click, BottomContainerTransactionsButton.Click, BottomContainerLogoutButton.Click,
        BottomContainerMaintenaceButton.Click, BottomContainerInventoryButton.Click, BottomContainerLogsButton.Click

        Dim panels As Object() = {
            DashboardPanel, ProductsPanel, TransactionsPanel, MaintenancePanel, InventoryPanel,
            AuditTrailPanel
        }
        Dim buttons As Object() = {
            BottomContainerDashboardButton, BottomContainerProductsButton,
            BottomContainerTransactionsButton, BottomContainerMaintenaceButton,
            BottomContainerInventoryButton, BottomContainerLogsButton
        }

        ' Collapse all the panels first before opening the desired panel
        For Each panel In panels
            panel.Visibility = Visibility.Collapsed
        Next

        ' Set the background back to it's original color
        For Each button In buttons
            button.Background = Brushes.Transparent
        Next

        ' Make the panel visible and change the button's background
        For i = 0 To buttons.Count - 1
            If sender.Equals(buttons(i)) Then
                buttons(i).Background = New BrushConverter().ConvertFromString("#FF76AFD2")
                panels(i).Visibility = Visibility.Visible
            End If
        Next


    End Sub
#End Region

#Region "TitleBarClickEvents"
    ' Use for the title bar click events, minimize, maximize, and close
    Private Sub TitleBarClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, RestoreButton.Click,
        MinimizeButton.Click
        If sender.Equals(CloseButton) Then
            Me.Close()                                                                          ' Close the window
        ElseIf sender.Equals(RestoreButton) Then
            Dim rectArea As Rect = SystemParameters.WorkArea
            ' Is the window in maximized state?
            If Me.Width = rectArea.Width AndAlso Me.Height = rectArea.Height Then
                ' If so then go back to the initial height and width
                Me.Width = Me.MinWidth
                Me.Height = Me.MinHeight
                RestoreIcon.Source = TryCast(FindResource("ic_maximizebit"), ImageSource)      ' Change the icon of restore down
            Else
                ' Position the window at the leftmost and topmost
                Me.Left = 0
                Me.Top = 0
                Me.Width = rectArea.Width
                Me.Height = rectArea.Height
                RestoreIcon.Source = TryCast(FindResource("ic_restoredownbit"), ImageSource)   ' Change the icon of the restore down

            End If
        ElseIf sender.Equals(MinimizeButton) Then
            Me.WindowState = WindowState.Minimized                                              ' Minimize the window
        End If
    End Sub
#End Region

    Private Sub SettingsButton_Click(sender As Object, e As RoutedEventArgs) Handles SettingsButton.Click
        If Not SettingsMenu.IsOpen Then
            SettingsMenu.IsOpen = True

        End If
    End Sub

    Private Sub AboutButtonItem_Click(sender As Object, e As RoutedEventArgs) Handles AboutButtonItem.Click
        Dialog.Show(New AboutDialog)
    End Sub
End Class
