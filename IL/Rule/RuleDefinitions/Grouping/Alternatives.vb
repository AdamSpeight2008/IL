Partial Public Class Parser
  Partial Public Class RuleDefinition
    Public Class Alternatives
      Inherits Group
      Public Sub New(a As IEnumerable(Of RuleDefinition))
        MyBase.New(" | ", a)
      End Sub

      Public Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
        Trace.WriteLine("Alts")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim si = index
        Dim curr = si
        Dim res As ParseResult
        For Each g In _A
          res = g.Parse(sr, curr)
          If res.Valid Then Parse = ParseResult.Yes(si, res.EndsAt) : GoTo [exit]
          curr = si
        Next
        Parse = ParseResult.No(si, si)
[exit]:
        Trace.Unindent()
        Trace.Write("}  ")
        Trace.WriteLine(Parse)

      End Function

    End Class

  End Class
End Class