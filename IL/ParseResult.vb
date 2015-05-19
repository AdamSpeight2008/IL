Imports IL

<DebuggerStepThrough ()>
Public Class ParseResult
    Public ReadOnly Property Valid As Boolean
    Public ReadOnly Property BeginsAt As Integer
    Public ReadOnly Property EndsAt As Integer

    Private Sub New(Valid As Boolean, BeginsAt As Integer, EndsAt As Integer)
        Me.Valid = Valid
        Me.BeginsAt = BeginsAt
        Me.EndsAt = EndsAt
    End Sub

    Public Shared Function Yes(BeginsAt As Integer, EndsAt As Integer) As ParseResult
        Return New ParseResult(True, BeginsAt, EndsAt)
   End Function
    Public Shared Function No(BeginsAt As Integer, EndsAt As Integer) As ParseResult
        Return New ParseResult(False, BeginsAt, EndsAt)
    End Function

    Public Function Count() As Integer
        Return (EndsAt - BeginsAt)
    End Function


    Public Overrides Function ToString() As String
    Return $"{Valid.Tick,-2} ({BeginsAt} : {EndsAt}) = {Count()}"
  End Function
End Class
