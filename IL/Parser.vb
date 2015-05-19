Public Class Parser
  Public ReadOnly Property Rules As Grammar

  Public Sub New(rules As Grammar)
    Me.Rules = rules
  End Sub
  Public Async Function Parse(sr As SourceReader) As Task(Of ParseResult)
    Return Await Task.Run(Function() Rules.DEF.Def.Parse(sr, 0))
  End Function

End Class