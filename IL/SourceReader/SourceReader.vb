<DebuggerStepThrough()>
Public Class SourceReader
  Public ReadOnly Property Source As SourceText

  Public Sub New(source As SourceText)
    Me.Source = source
  End Sub

  Public Async Function Peek(index As Integer) As Task(Of Char?)
    If (0 <= index) AndAlso (index < Source.Length) Then Return Await Source.Chars(index)
    Return Nothing
  End Function
  Public Async Function Peek(index As Integer, length As Integer) As Task(Of Boolean)
    If (0 <= index) AndAlso (index < Source.Length) Then
      Dim newindex = (index + length) - 1
      Return (0 <= newindex) AndAlso (newindex < Source.Length)
    End If
    Return False
  End Function

  Public Async Function SubText(si As Integer, ei As Integer) As Task(Of String)
    Return Await Source.SubText(si, ei)
  End Function

End Class

