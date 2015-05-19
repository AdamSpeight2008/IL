Imports IL.Parser

<DebuggerStepThrough()>
Public Class Rule

  Public ReadOnly Property Name As RuleName
  Public ReadOnly Property Def As RuleDefinition

  Public Sub New(Name As RuleName, RuleDef As RuleDefinition)
    Me.Name = Name : Me.Def = RuleDef
  End Sub

  Public Overrides Function ToString() As String
    Return $"<{Name}> ::= {Def}"
  End Function

  Public Shared Narrowing Operator CType(r As Rule) As RuleDefinition
    Return r.Def
  End Operator

End Class
