Partial Public Class Parser
  Partial Public MustInherit Class RuleDefinition
    Friend Sub New()

    End Sub

    Public MustOverride Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)

    Public Shared Widening Operator CType(ByVal c As Char) As RuleDefinition
      Return New CharDef(c)
    End Operator
    Public Shared Widening Operator CType(ByVal c As String) As RuleDefinition
      Return New TextDef(c)
    End Operator

    Public Shared Widening Operator CType(ByVal rd As RuleDefinition()) As RuleDefinition
      Return New Group(rd)
    End Operator

  End Class

End Class