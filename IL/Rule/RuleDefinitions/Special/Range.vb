Imports IL

Partial Public Class Parser
    Public MustInherit Class RuleDefinition

        Public Class Range
            Inherits Special
            Public ReadOnly Property Min As Char
            Public ReadOnly Property Max As Char

            Public Sub New(Min As Char, Max As Char)
                Me.Min = Min : Me.Max = Max
            End Sub
            Public Overrides Function ToString() As String
                Return $"'{Min}'-'{Max}'"
            End Function

            Public Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
                Dim c = sr.Source.Chars(index)
                If (Min <= c) AndAlso (c <= Max) Then Return ParseResult.Yes(index, index + 1)
                Return ParseResult.No(index, index)
            End Function

        End Class

    End Class

End Class