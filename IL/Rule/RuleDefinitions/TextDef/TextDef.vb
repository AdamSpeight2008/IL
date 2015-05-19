Imports Microsoft.VisualBasic
Partial Public Class Parser
  Partial Public MustInherit Class RuleDefinition
    Public Class TextDef
      Inherits TextDef_base
      Public Sub New(cs As Char())
        MyBase.New(cs)
      End Sub
      Public Sub New(cs As Char)
        Me.New({cs})
      End Sub
      Public Sub New(cs As String)
        Me.New(cs.ToArray)
      End Sub
      Public Overrides Function ToString() As String
        Return ControlChars.Quote + $"{Me.MyText }" + ControlChars.Quote
      End Function


      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Return Await MyBase.Parse(sr, index)
      End Function
    End Class
  End Class
End Class

