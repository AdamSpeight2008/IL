Partial Public Class Parser
    Partial Public Class RuleDefinition
        Public Class RuleRef
            Inherits TextDef_base
            Public ReadOnly Property Rules As Rules
            Public Sub New(Rules As Rules, RuleName As RuleName)
                MyBase.New(RuleName.Name.ToArray)
                Me.Rules = Rules
            End Sub

            Public Overrides Function ToString() As String
                Dim rn =Me.MyText
                Return $"<{rn}>"
            End Function

      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Dim RuleName = MyText()
        Trace.WriteLine($"<{RuleName}>")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim RValue As ParseResult
        Dim d = Rules.__(RuleName).Def
        RValue = Await d.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result: {RValue}")
        Return RValue
      End Function

    End Class

    End Class
End Class