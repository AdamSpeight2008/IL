Public MustInherit Class Special
  Inherits Parser.RuleDefinition

  Public MustOverride Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
  Public Overrides Function ToString() As String
    Return ""
  End Function
End Class
