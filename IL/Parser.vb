Public Class Parser
    Public ReadOnly Property Rules As Rules

    Public Sub New(rules As Rules)
        Me.Rules = rules
    End Sub
    Public Function Parse(sr As SourceReader) As ParseResult
        Return Rules.DEF.Def.Parse(sr, 0)
    End Function

End Class