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

      Public Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
        Dim ch As Char? = sr(index)
        Trace.Write($"Does ({ch}) = ({C}) ? ")
        If sr(index, 1) AndAlso ch = Me.C Then
          Parse = ParseResult.Yes(index, index + 1)
        Else
          Parse = ParseResult.No(index, index)
        End If
        Trace.WriteLine(Parse)

      End Function

    End Class
  End Class
End Class