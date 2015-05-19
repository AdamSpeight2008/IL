Public MustInherit Class Special
    Inherits Parser.RuleDefinition

    Public MustOverride Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
    Public Overrides Function ToString() As String
        Return ""
    End Function
End Class
