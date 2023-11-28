Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        If My.Settings.userID <> -1 Then
            Me.StartupUri = New System.Uri("Forms/Dashboard.xaml", System.UriKind.Relative)
        End If
    End Sub

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

End Class
