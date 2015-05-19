Imports Microsoft.VisualBasic
Partial Public Class Parser
  Partial Public MustInherit Class RuleDefinition
    Public Class ControlChar
      Inherits TextDef_base
      Public ReadOnly Property Display As String

      Private Sub New(Display As String, Chars As String)
        MyBase.New(Chars.ToArray)
        Me.Display = Display
      End Sub

      Public Shared Function CR() As ControlChar
        Return New ControlChar("\cr", ControlChars.Cr)
      End Function

      Public Shared Function LF() As ControlChar
        Return New ControlChar("\lf", ControlChars.Lf)
      End Function

      Public Shared Function NL() As ControlChar
        Return New ControlChar("\nl", ControlChars.CrLf)
      End Function


      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Return Await MyBase.Parse(sr, index)
      End Function
    End Class

  End Class
End Class
