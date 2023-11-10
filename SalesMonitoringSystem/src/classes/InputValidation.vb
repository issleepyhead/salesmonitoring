Imports System.CodeDom
Imports System.Text.RegularExpressions
Imports HandyControl.Tools.Extension

Public Class InputValidation
    Public Shared Function ValidateInputString(control As Object, type As DataInput) As Object()
        Dim stringInput As String = Nothing

        ' The have different properties so we have distinguish them
        Select Case True
            Case TypeOf control Is TextBox
                stringInput = TryCast(control, TextBox).Text
            Case TypeOf control Is HandyControl.Controls.PasswordBox
                stringInput = TryCast(control, HandyControl.Controls.PasswordBox).Password
            Case TypeOf control Is ComboBox
                stringInput = TryCast(control, ComboBox).Text
        End Select

        stringInput = stringInput.Trim()
        If String.IsNullOrEmpty(stringInput) OrElse String.IsNullOrWhiteSpace(stringInput) Then
            control.BorderBrush = Brushes.Red
            Return {False, ""}
        End If

        control.BorderBrush = New BrushConverter().ConvertFromString("#FFE0E0E0")
        Select Case type
            Case DataInput.STRING_STRING
                If Not String.IsNullOrEmpty(stringInput) AndAlso Not String.IsNullOrWhiteSpace(stringInput) Then
                    Return {True, stringInput}
                End If
            Case DataInput.STRING_NAME
                If stringInput.Count > 1 Then
                    Dim nameString As String() = stringInput.Split(" ")
                    For i = 0 To nameString.Count - 1
                        Dim charArr As Char() = nameString(i).ToArray()
                        charArr(0) = CStr(charArr(0)).ToUpper
                        nameString(i) = String.Join("", charArr)
                    Next
                    MsgBox(String.Join(" ", nameString))
                    Return {True, String.Join(" ", nameString)}
                End If
            Case DataInput.STRING_PASSWORD
                If stringInput.Count > 6 Then
                    Return {True, stringInput}
                End If
            Case DataInput.STRING_PHONE
                If Not Regex.IsMatch(stringInput, ".*[A-Za-z\p{P}\p{S}].*") Then
                    Return {True, stringInput}
                End If
            Case DataInput.STRING_USERNAME
                If stringInput.Count > 6 Then
                    Return {True, stringInput}
                End If
            Case DataInput.STRING_INTEGER
                If Not Regex.IsMatch(stringInput, ".*[A-Za-z\p{P}\p{S}].*") OrElse Not stringInput = "0" Then
                    Return {True, stringInput}
                End If
        End Select

        control.BorderBrush = Brushes.Red
        Return {False, ""}
    End Function
End Class


Public Enum DataInput
    STRING_STRING
    STRING_NAME
    STRING_PASSWORD
    STRING_USERNAME
    STRING_PHONE
    STRING_INTEGER
End Enum