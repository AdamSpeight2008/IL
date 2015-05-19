Imports IL.CommonRules

Public Class BNF
  Private Shared ReadOnly BNF As Grammar = Grammar.Create

  Shared Sub New()
    BNF.
        AddRule("identifier", {BNF!letter, ZeroOrMore(Alt(BNF!letter, BNF!digit, BNF!symbol))}).
        AddRule("syntax", ZeroOrMore(BNF!rule), True).
        AddRule("QChar", {"'"c, BNF!any_character, "'"c}).
        AddRule("quoted_symbol", Common.AnyChar.Create).
        AddRule("symbol", "_"c).
        AddRule("any_character", Common.AnyChar.Create).
        AddRule("rule", {WS, BNF!identifier, WS, "::=", WS, BNF!definition, BNF!EOL}).
        AddRule("definition", {BNF!expression, WS}).
        AddRule("expression", {BNF!term, WS, ZeroOrMore({"|"c, WS, BNF!term})})
    ' terms
    BNF.
        AddRule("term", {BNF!factor, ZeroOrMore(BNF!factor)}).
        AddRule("factor", {WS, Alt(BNF!ref,
                                        BNF!QChar,
                                        BNF!quoted_symbol,
                                        BNF!grouping,
                                        BNF!optional,
                                        BNF!ZeroOrMore), WS}).
        AddRule("ref", {LAngle, BNF!identifier, RAngle}).
        AddRule("grouping", {LBracket, WS, BNF!expression, RBracket, WS}).
        AddRule("ZeroOrMore", {LBrace, WS, BNF!expression, RBrace, WS}).
        AddRule("optional", {LSquare, WS, BNF!expression, RSquare, WS})
  End Sub

  Public Shared Function Create() As Grammar
    Return BNF
  End Function
End Class
