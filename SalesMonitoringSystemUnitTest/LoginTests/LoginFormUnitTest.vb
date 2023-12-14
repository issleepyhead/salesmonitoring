Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SalesMonitoringSystem
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient

Namespace SalesMonitoringSystemUnitTest
    <TestClass>
    Public Class LoginFormUnitTest
        Inherits SqlBaseConnection
        Private _dataSet As DataTable
        <TestMethod>
        Sub LoginTestMethod()
            Dim login_module As New LoginModule
            Dim test_cases As New Dictionary(Of String, Object()) From {
                {"TestCase 1", {False, "sample", "sample"}},
                {"TestCase 2", {True, "super", "super"}},
                {"TestCase 3", {False, "", ""}}
            }

            For Each key As String In test_cases.Keys
                Dim data As Object() = test_cases.Item(key)
                Dim message As String = $"{0}: username : {data(1)}{vbTab} passwrod: {data(2)}"
                Assert.AreEqual(data(0), login_module.LoginAccount(data(1), data(2))(0), message)
            Next
        End Sub
    End Class
End Namespace

