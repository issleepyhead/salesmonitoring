Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' A base class for the commands to inherit.
''' </summary>
Public Class SqlBaseConnection
    Protected _sqlCommand As SqlCommand
    Protected _sqlConnection As SqlConnection = SqlConnectionSingleton.GetInstance
    Protected _sqlDataSet As DataSet
    Protected _sqlAdapter As SqlDataAdapter

End Class
