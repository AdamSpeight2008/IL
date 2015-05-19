Imports IL

Partial Public Class Parser
  Partial Public Class RuleDefinition

    Public MustInherit Class Repetitions
      Inherits RuleDefinition
      Public ReadOnly Property Min As Integer
      Public ReadOnly Property Max As Integer

      Public ReadOnly Property What As RuleDefinition

      Friend Sub New(what As RuleDefinition, min As Integer, max As Integer)
        MyBase.New()

        Me.What = what
        Me.Min = min
        Me.Max = max
      End Sub

      Friend Async Function CountReps(sr As SourceReader, R As RuleDefinition, index As Integer) As Task(Of Tuple(Of Integer, Integer, Integer))
        Dim si = index
        Dim Count = 0
        Dim Ok = True
        Dim res As ParseResult
        While Ok
          res = Await R.Parse(sr, index)
          If res.Valid = False Then
            Ok = False
          Else
            Count += 1
            index = res.EndsAt  ' index + res.Count
          End If
        End While
        Return New Tuple(Of Integer, Integer, Integer)(Count, si, index)
      End Function


      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        'Trace.WriteLine($"Repeats {Min} to {Max} times: ")
        'Trace.Indent()
        Dim si = index
        Dim res = Await CountReps(sr, What, si)
        Dim count = res.Item1
        If (Me.Min <= count) AndAlso (count <= Me.Max) Then
          Return ParseResult.Yes(si, res.Item3)
        Else
          Return ParseResult.No(si, res.Item3)
        End If
        'Trace.Unindent 
        'Trace.WriteLine($"Result{{ Count: {res.Item1} }}")
        'Trace.WriteLine(parse)
      End Function

    End Class

    Public Class Optionally
      Inherits Repetitions

      Public Sub New(What As RuleDefinition)
        MyBase.New(What, 0, 1)
      End Sub

      Public Overrides Function ToString() As String
        Return $"[ {What} ]"
      End Function

      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Trace.WriteLine("Optional")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim rvalue = Await MyBase.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result:= {rvalue}")
        Return rvalue
      End Function


    End Class

    Public Class ZeroOrMore
      Inherits Repetitions

      Public Sub New(What As RuleDefinition)
        MyBase.New(What, 0, Integer.MaxValue)
      End Sub
      Public Overrides Function ToString() As String
        Return $"{{ {What} }}"

      End Function
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Trace.WriteLine("ZeroOrMore")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim rvalue = Await MyBase.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result:= {rvalue}")
        Return rvalue
      End Function
    End Class

    Public Class OneOrMore
      Inherits Repetitions

      Public Sub New(What As RuleDefinition)
        MyBase.New(What, 1, Integer.MaxValue)
      End Sub
      Public Overrides Function ToString() As String
        Return $"{What}+"
      End Function
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Trace.WriteLine("OneOrMore")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim RValue = Await MyBase.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result:= {RValue}")
        Return RValue
      End Function
    End Class

    Public Class Repeats
      Inherits Repetitions

      Public Sub New(What As RuleDefinition, times As Integer)
        MyBase.New(What, times, times)
      End Sub
      Public Overrides Function ToString() As String
        Return $"{What}{{{Min}}}"
      End Function
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Trace.WriteLine($"Repeat {Min} times")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim RValue = Await MyBase.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result:= {RValue}")
        Return RValue
      End Function
    End Class

    Public Class RepeatBound
      Inherits Repetitions

      Public Sub New(What As RuleDefinition, min As Integer, max As Integer)
        MyBase.New(What, min, max)
      End Sub
      Public Overrides Async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
        Trace.WriteLine($"Repeats {Min} To {Max}")
        Trace.WriteLine("{")
        Trace.Indent()
        Dim RValue = Await MyBase.Parse(sr, index)
        Trace.Unindent()
        Trace.WriteLine($"}} Result:= {RValue}")
        Return RValue
      End Function
    End Class



  End Class
End Class