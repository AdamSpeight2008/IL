<DebuggerStepThrough()>
Public Class SourceReader
    Public ReadOnly Property Source As SourceText

    Public Sub New(source As SourceText)
        Me.Source = source
    End Sub

    Default Public ReadOnly Property Peek(index As Integer) As Char?
        Get
            If (0 <= index) AndAlso (index < Source.Length) Then Return Source.Chars(index)
            Return Nothing
        End Get
    End Property
    Default Public ReadOnly Property Peek(index As Integer, length As Integer) As Boolean
        Get
            If (0 <= index) AndAlso (index < Source.Length) Then
                Dim newindex = (index + length) - 1
                Return (0 <= newindex) AndAlso (newindex < Source.Length)
            End If
            Return False
        End Get
    End Property

    Public Function SubText(si As Integer, ei As Integer) As String
        Return Source.SubText(si, ei)
    End Function

End Class

