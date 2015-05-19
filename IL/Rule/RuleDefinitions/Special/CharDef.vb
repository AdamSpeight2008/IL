Partial Public Class Parser
  Partial Public Class RuleDefinition
    <DebuggerDisplay("[{c}]")>
    Public Class CharDef
      Inherits Special
      Public ReadOnly Property C As Char
      Public Sub New(c As Char)
        Me.C = c
      End Sub
      Public Overrides Function ToString() As String
        Return $"'{C}'"
      End Function

      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Dim ch As Char? = Await sr.Peek(index)
        Trace.Write($"Does ({ch}) = ({C}) ? ")
        Dim RValue As ParseResult
        If Await sr.Peek(index, 1) AndAlso ch = Me.C Then
          RValue = ParseResult.Yes(index, index + 1)
        Else
          RValue = ParseResult.No(index, index)
        End If
        Trace.WriteLine(RValue)
        Return RValue
      End Function

    End Class
  End Class
End Class