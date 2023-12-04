Public Interface IFileParser
    Function Exists(data As Object) As Boolean
    Function Insert(data As Object) As Boolean
    Function Update(data As Object) As Boolean
    Function Fetch(data As Object) As Integer
End Interface
