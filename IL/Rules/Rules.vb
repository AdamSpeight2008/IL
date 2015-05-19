
Imports IL.Parser

Public Class Rules

    Private _Def As Rule = Nothing
    Public ReadOnly Property DEF As Rule
        Get
            Return _Def
        End Get
    End Property
    Public Shared ReadOnly Property Common As CommonRules = IL.CommonRules.Create

    Friend Sub New()

    End Sub


    Friend __ As New Dictionary(Of RuleName, Rule)


    Shared Function Create() As Rules
        Return New Rules()
    End Function


    Public Function AddRule(Name As String, RuleDef As RuleDefinition, Optional def As Boolean = False) As Rules
        Dim rn As New RuleName(Name)
        Dim ru As New Rule(rn, RuleDef)
        __.Add(rn, ru)
        If def Then _Def = ru

        Return Me
    End Function

    Default Public ReadOnly Property Index(name As String) As RuleDefinition.RuleRef
        Get
            Dim rn As New RuleName(name)
            Dim ru As Rule = Nothing
            If __.TryGetValue(rn, ru) Then Return New RuleDefinition.RuleRef(Me, rn)
            If Common.__.TryGetValue(rn, ru) Then Return New RuleDefinition.RuleRef(Common, rn)
            ''            Throw New KeyNotFoundException($"<{name}> not found")
            Return New RuleDefinition.RuleRef(Me, rn)
        End Get
    End Property

    Default Public ReadOnly Property Index(ruleRef As RuleDefinition.RuleRef) As RuleDefinition.RuleRef
        Get
            Dim rn As RuleName = ruleRef.cs

            Dim ru As Rule = Nothing
            If ruleRef.Rules.__.TryGetValue(rn, ru) Then Return ruleRef
            ' If Rules.Common.__.TryGetValue(rn,ru) Then Return 
            '   Throw New KeyNotFoundException($"<{rn}> not found")
            Return Nothing
        End Get
    End Property


    Public Function PrettyPrint() As String
        Dim wide = Aggregate k In __.Keys Select k.Name.Length Into Max

        Dim sb As New System.Text.StringBuilder()
        For Each r As KeyValuePair(Of RuleName, Rule) In __.AsEnumerable
            sb.Append($"{r.Key.Name.PadRight(wide + 1)} ::= ")
            If r.Value.Def IsNot Nothing Then sb.Append($"{r.Value.Def.ToString}")
            sb.AppendLine()
        Next
        Return sb.ToString
    End Function

    Public Function GetParser() As Parser
        Return New Parser(Me)
    End Function


End Class


