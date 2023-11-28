Imports System.Data.SqlClient

Public Class SqlConnectionSingleton
    Private Shared ReadOnly _sqlConnection As New SqlConnection(My.Settings.sgsmsdbConnectionString)

    ''' <summary>
    ''' Sets the constructor as private to ensure that the instance will not be
    ''' instantiated again.
    ''' </summary>
    Private Sub New()

    End Sub

    ''' <summary>
    ''' A single instance of the connection for all the operations
    ''' </summary>
    ''' <returns>An open instancne of the connection.</returns>
    Public Shared Function GetInstance() As SqlConnection
        If _sqlConnection.State = System.Data.ConnectionState.Closed Then
            _sqlConnection.Open()
        End If
        Return _sqlConnection
    End Function
End Class
