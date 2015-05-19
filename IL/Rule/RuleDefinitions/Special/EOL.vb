Imports Microsoft.VisualBasic

Partial Public Class Parser
  Partial Public MustInherit Class RuleDefinition
    Public Class EOL
      Inherits Special
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Dim ch = Await sr.Peek(index)
        If ch = ControlChars.Lf Then Return ParseResult.Yes(index, index + 1)
        If ch <> ControlChars.Cr Then Return ParseResult.No(index, index)
        ch = Await sr.Peek(index + 1)
        If ch = ControlChars.Lf Then Return ParseResult.Yes(index, index + 2)
        Return ParseResult.Yes(index, index + 1)
      End Function

      Public Shared Function Create() As EOL
        Return New EOL
      End Function

    End Class
  End Class
End Class