Imports IL.Parser
Imports IL.Parser.RuleDefinition
Imports Microsoft.VisualBasic
Public Class CommonRules
  Inherits Rules
  Private Shared _CommonDefs As New List(Of RuleDefinition)(10)
  Public Shared ReadOnly Property DOT As New RuleDefinition.CharDef("."c)
  Public Shared ReadOnly Property LBracket As New RuleDefinition.CharDef("("c)
  Public Shared ReadOnly Property RBracket As New RuleDefinition.CharDef(")"c)
  Public Shared ReadOnly Property LBrace As New RuleDefinition.CharDef("{"c)
  Public Shared ReadOnly Property RBrace As New RuleDefinition.CharDef("}"c)
  Public Shared ReadOnly Property LSquare As New RuleDefinition.CharDef("["c)
  Public Shared ReadOnly Property RSquare As New RuleDefinition.CharDef("]"c)
  Public Shared ReadOnly Property LAngle As New RuleDefinition.CharDef("<"c)
  Public Shared ReadOnly Property RAngle As New RuleDefinition.CharDef(">"c)
  Public Shared ReadOnly Property DQ As New RuleDefinition.CharDef(ControlChars.Quote)
  Public Shared ReadOnly Property Colon As New RuleDefinition.CharDef(":"c)
  Public Shared ReadOnly Property AnyChar As Common.AnyChar = New Common.AnyChar
  Public Shared ReadOnly Property WS As Common.WS = New Common.WS
  Public Shared ReadOnly Property EOL As EOL = RuleDefinition.EOL.Create
  Public Shared ReadOnly Property QuotedText As Parser.Common.QuotedText = Parser.Common.QuotedText.create

  Public Shared ReadOnly Property Digit As Common.Digit = New Common.Digit
  Public Shared ReadOnly Property Letter As Common.Letter = New Common.Letter

  Shared Sub New()
    With _CommonDefs
      .Add(Digit)
      .Add(OneOrMore(_CommonDefs(0)))
      .Add("a"c.To("z"c))
      .Add(OneOrMore(_CommonDefs(2)))
      .Add("A"c.To("Z"c))
      .Add(OneOrMore(_CommonDefs(4)))
      .Add(Letter)
      .Add(OneOrMore(_CommonDefs(6)))
      .Add(" ")
      .Add(OneOrMore(_CommonDefs(8)))
      .Add(WS)
    End With
  End Sub

  Private Sub New()
    MyBase.New()
    AddRule("digit", _CommonDefs(0))
    AddRule("digits", _CommonDefs(1))
    AddRule("lowerCaseLetter", _CommonDefs(2))
    AddRule("lowerCaseLetters", _CommonDefs(3))
    AddRule("upperCaseLetter", _CommonDefs(4))
    AddRule("upperCaseLetters", _CommonDefs(5))
    AddRule("letter", _CommonDefs(6))
    AddRule("letters", _CommonDefs(7))
    AddRule("SPACE", _CommonDefs(8))
    AddRule("SPACES", _CommonDefs(9))
    AddRule("WS", _CommonDefs(10)).
    AddRule("EOL", EOL)
  End Sub



  Private Shadows Function AddRule(name As String, rule As RuleDefinition) As Rules
    Return MyBase.AddRule(name, rule)
  End Function

  Public Shared Shadows Function Create() As CommonRules
    Return New CommonRules()
  End Function




End Class

Partial Public Class Common
  Public Class SPACE
    Inherits Special
    Private Sub New()

    End Sub
    Public Overrides async Function Parse(sr As SourceReader, index As Integer) As Task(of ParseResult)
      If await sr.Peek(index) <> " "c Then Return ParseResult.Yes(index, index + 1)
      Return ParseResult.No(index, index)
    End Function
    Private Shared ReadOnly _SPC As New SPACE
    Public Shared Function Create() As SPACE
      Return _SPC
    End Function

  End Class
  Public Class WS
    Inherits Special
    Friend Sub New()

    End Sub
    Public Overrides async Function Parse(sr As SourceReader, index As Integer) As Task(of ParseResult)
      Dim i = 0
      Dim ch As Char?
      While True
        ch = Await sr.Peek(index + i)
        If ch.HasValue = False OrElse ch <> " "c Then Exit While
        i += 1
      End While
      Return ParseResult.Yes(index, index + i)
    End Function
    Public Shared Function Create() As WS
      Return New WS
    End Function

  End Class

  Public Class Letter
    Inherits Special
    Friend Sub New()

    End Sub
    Public Overrides async Function Parse(sr As SourceReader, index As Integer) As Task(Of ParseResult)
      Dim ch = Await sr.Peek(index)
      Trace.Write($"({ch}).IsLetter? ")
      Dim RValue As ParseResult
      If ch.HasValue = False Then
          RValue = ParseResult.No(index, index)
      ElseIf ("A"c <= ch) AndAlso (ch <= "Z"c) Then
        RValue = ParseResult.Yes(index, index + 1)
      Else If ("a"c <= ch) AndAlso (ch <= "z"c) Then
        RValue = ParseResult.Yes(index, index + 1)
        Else
        RValue = ParseResult.No(index, index)
        End If
      Trace.Writeline(RValue)
      Return RValue
    End Function
    Private Shared ReadOnly __ As New Letter
    Public Shared Function Create() As Letter
      Return __
    End Function
  End Class

  Public Class AnyChar
    Inherits Special

     Public Overrides async Function Parse(sr As SourceReader, index As Integer) As Task(of ParseResult)
      If Not (Await sr.Peek(index)).HasValue Then Return ParseResult.No(index, index)
      Return ParseResult.Yes(index, index + 1)
    End Function
    Private Shared ReadOnly __ As New AnyChar
    Public Shared Function Create() As AnyChar
      Return __
    End Function
  End Class

  Public Class Digit
    Inherits Special
    Friend Sub New()

    End Sub
     Public Overrides async Function Parse(sr As SourceReader, index As Integer) As Task(of ParseResult)

      Dim ch = Await sr.Peek(index)
      Trace.Write($"({ch}).IsDigit? ")
      Dim RValue As ParseResult
      If ch.HasValue AndAlso ("0"c <= ch) AndAlso (ch <= "9"c) Then
          RValue = ParseResult.Yes(index, index + 1)
      Else
        RValue = ParseResult.No(index, index)
        End If
      Trace.WriteLine(RValue)
      Return RValue
    End Function
    Public Shared Function Create() As Digit
      Return New Digit
    End Function
  End Class
End Class