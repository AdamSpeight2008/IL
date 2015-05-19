
Partial Public Class Parser
  Partial Public Class RuleDefinition

    Public Class Group
      Inherits RuleDefinition
      Friend _A As New List(Of RuleDefinition)
      Private _S As String = " "
      Friend Sub New(s As String, a As IEnumerable(Of RuleDefinition))
        MyBase.New()
        _S = s
        _A = New List(Of RuleDefinition)(a)
      End Sub
      Public Sub New(a As IEnumerable(Of RuleDefinition))
        Me.New(" ", a)
      End Sub
      Public Overrides Function ToString() As String
        Return $"{String.Join(_S, _A)}"
      End Function
      
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Dim curr = index
        Dim res As ParseResult
        For Each g In _A
          res = Await g.Parse(sr, curr)
          If Not res.Valid Then Return ParseResult.No(index, res.EndsAt)
          curr = res.EndsAt
        Next
        Return ParseResult.Yes(index, res.EndsAt)
      End Function

    End Class
  End Class
End Class