Imports System.Data
Imports System.Data.SqlClient

Public Class BaseAudit
    Public Shared Function Search(query As String) As sgsmsdb.viewtbllogsDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As SqlCommand
            If String.IsNullOrEmpty(query) Then
                cmd = New SqlCommand("SELECT * FROM viewtbllogs", conn)
            Else
                cmd = New SqlCommand("SELECT * FROM viewtbllogs WHERE FULL_NAME = @query OR ACTION = @query OR CATEGORY = @query", conn)
                cmd.Parameters.AddWithValue("@query", query)
            End If
            Dim dTable As New sgsmsdb.viewtbllogsDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function
End Class
