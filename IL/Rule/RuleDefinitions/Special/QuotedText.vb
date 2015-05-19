Imports IL
Imports Microsoft.VisualBasic

Partial Public Class Parser
  Partial Public Class Common
    Public Class QuotedText
      Inherits Special

      Public Shared ReadOnly Property Create As New QuotedText 
      Public Overrides Function Parse(sr As SourceReader, index As Integer) As ParseResult
        Dim i = index
        Const DQ As Char = ControlChars.Quote
        If sr(index) <> DQ Then Return ParseResult.No(index, index)
        If sr(i, 3) AndAlso sr(i + 3) = DQ Then
          If sr(i + 1) = DQ Then
            If sr(i + 3) = DQ AndAlso sr(i, 4) AndAlso sr(i + 4) = "c"c Then
              Return ParseResult.Yes(i, i + 5)
            End If
          ElseIf sr(i + 3) = "c"c Then
            Return ParseResult.Yes(i, i + 4)
          End If
        End If

        If sr(i, 2) AndAlso sr(i + 1) = DQ AndAlso sr(i + 2) = "c"c Then Return ParseResult.No(i, i)

        i = 1
        Dim ch As Char?
        While sr(index, i)
          ch = sr(index + i)
          If ch = DQ Then
            If sr(index, i + 1) Then
              ch = sr.Peek(index + i + 1)
              If ch = DQ Then
                i += 2
                Continue While
              Else
                If ch = "c"c Then
                  Return ParseResult.No(index, index)
                End If
              End If

            End If
            'Elseif 
            '    i += 1
          End If
          i += 1

        End While
        Return ParseResult.Yes(index, i - 1)

      End Function

    End Class
  End Class
End Class