Partial Public Class Parser
    Partial Public MustInherit Class RuleDefinition
        Public Class TextDef_base
            Inherits RuleDefinition

            Public ReadOnly Property cs As Char()
            Public Sub New(cs As Char())
                MyBase.New()
                Me.cs = cs
            End Sub

            Public Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
                If sr(index, cs.Length) = False Then Return ParseResult.No(index, index)
                Dim si As Integer = index
                Dim i = 0
                While i < cs.Length
                    Dim c = sr.Peek(si + i)
                    If c <> cs(i) Then Return ParseResult.No(si, si) 
                    i += 1
                End While
                Return ParseResult.Yes(si, si + i)
            End Function

            Public Overrides Function ToString() As String
                Return String.Join("  ", Me.cs.Select(Function(c) $"'{c}'"))
            End Function

            Friend Function MyText() As String
                Dim txt = ""
                If Me.cs IsNot Nothing Then txt = String.Join("", cs)
                Return txt
            End Function

        End Class

    End Class

End Class