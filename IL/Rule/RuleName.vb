<DebuggerStepThrough()>
Public Class RuleName
  Implements IEquatable(Of RuleName)
  Public ReadOnly Property Name As String
  Public Sub New(Name As String)
    Me.Name = Name
  End Sub
  Public Overrides Function ToString() As String
    Return Name
  End Function
  Public Overrides Function GetHashCode() As Integer
    Return Me.Name.GetHashCode
  End Function
  Public Overrides Function Equals(obj As Object) As Boolean
    Return Me.Equals(CType(obj, RuleName))
  End Function

  Public Overloads Function Equals(other As RuleName) As Boolean Implements IEquatable(Of RuleName).Equals
    Return Me.Name.Equals(other.Name)
  End Function

  Public Shared Widening Operator CType(s As String) As RuleName
    Return New RuleName(s)
  End Operator
End Class