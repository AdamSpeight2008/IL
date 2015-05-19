Option Infer On
Option Explicit On

Imports IL.Rules
Imports System.Runtime.CompilerServices
Imports IL
Imports IL.Parser

Module Module1

  Sub Main()
    Dim BNFRules = BNF.Create()
    Dim re = BNFRules.PrettyPrint

    'Dim p = New SourceReader(New SourceText(  ControlChars.Quote & "symbol" & ControlChars.Quote ))
    'Dim r = BNF("quoted_symbol")                          .Parse(p, 0)
    Console.WriteLine(re)
    Console.WriteLine($"Length {re.Length}")
    Console.WriteLine()
    Dim sr = New SourceReader(New SourceText(re))
    Dim pr = BNFRules.GetParser
    Dim sw As New Diagnostics.Stopwatch
    sw.Start()
    Dim res = pr.Parse(sr).Result
    sw.Stop()
    Console.WriteLine($"Parsed [{res}]{res.Valid.Tick} in {sw.ElapsedMilliseconds}ms")
    Dim s = sr.SubText(res.BeginsAt, res.EndsAt).Result
    Console.WriteLine()
    Console.WriteLine(s)
    Console.ReadKey()
  End Sub

  Sub IL_Grammar()
        Dim xs = Rules.Create

        xs.AddRule("DIGIT", "0"c.To("9"c)).
           AddRule("HEX_l", "a"c.To("f"c)).
           AddRule("HEX_u", "A"c.To("F"c)).
           AddRule("HEX_Char", Alt(xs("DIGIT"), xs("HEX_l"), xs("HEX_u"))).
           AddRule("HEX_4", xs("HEX_Char").Repeat(4)).
           AddRule("IL_Header", "IL_").
           AddRule("IL_Label", Group(xs("IL_Header"), xs("HEX_4"), ":  "))

        Console.Write(xs.PrettyPrint)
        Console.WriteLine()
        Console.WriteLine()
        Dim ILAsm = Rules.Create

        ILAsm.
           AddRule("/* EMPTY */", Nothing).
           AddRule("ID", Nothing).
           AddRule("DOTTEDNAME", Nothing).
           AddRule("QSTRING", Nothing).
           AddRule("SQSTRING", Nothing).
           AddRule("INT64", Nothing).
           AddRule("FLOAT64", Nothing).
           AddRule("HEXBYTE", Nothing).
           AddRule("INTSTR_*", Nothing)
        ' Data Type Nonterminals
        ILAsm.
           AddRule("compQstring", Alt(ILAsm("QSTRING"), Group(ILAsm("compQstring"), ILAsm("QSTRING")))).
           AddRule("int32", ILAsm("INT64")).
           AddRule("int64", ILAsm("INT64")).
           AddRule("float64", Alt(ILAsm("FLOAT64"),
                                   Group("float32(", ILAsm("int32"), CommonRules.RBracket),
                                   Group("float64(", ILAsm("int64"), CommonRules.RBracket)
                                   )).
           AddRule("bytes", Alt(ILAsm("/* EMPTY */"), ILAsm("hexbytes"))).
           AddRule("hexbytes", Alt(ILAsm("HEXBYTE"), Group(ILAsm("hexbytes"), ILAsm("HEXBYTE")))).
           AddRule("trueFalse", Alt("true", "false"))
        ' Identifier Nonterminals
        ILAsm.
            AddRule("id", Alt(ILAsm("ID"), ILAsm("SQSTRING"))).
            AddRule("compName", Alt(ILAsm("id"), ILAsm("DOTTEDNAME"), Group(ILAsm("compName"), CommonRules.DOT, ILAsm("compName"))))
        ' Module Level Declarations
        ILAsm.
            AddRule("PROGRAM", ILAsm("decls")).
            AddRule("decls", Alt(ILAsm("/* EMPTY */"), Group(ILAsm("decls"), ILAsm("decl")))).
            AddRule("decl",
              Alt(
                Group(ILAsm("classHead"), CommonRules.LBrace, ILAsm("classDecls"), CommonRules.RBrace),
                Group(ILAsm("nameSpaceHead"), CommonRules.LBrace, ILAsm("decls"), CommonRules.RBrace),
                ILAsm("fieldDecl"),
                ILAsm("dataDecl"),
                ILAsm("vtfixupDecl"),
                ILAsm("fileDecl"),
                Group(ILAsm("assemblyHead"), CommonRules.LBrace, ILAsm("AssemblyDecls"), CommonRules.RBrace),
                Group(ILAsm("assemblyRefHead"), CommonRules.LBrace, ILAsm("assemblyRefDecls"), CommonRules.RBrace),
                Group(ILAsm("expTypeHead"), CommonRules.LBrace, ILAsm("expTypeDecls"), CommonRules.RBrace),
                Group(ILAsm("manifestResHead"), CommonRules.LBrace, ILAsm("manifestResDEcls"), CommonRules.RBrace),
                ILAsm("moduleHead"),
                ILAsm("secDecl"),
                ILAsm("custAttrDecl"),
                Group(".subsystem", ILAsm("int32")),
                Group(".corflags", ILAsm("int32")),
                Group(".file", "alignment", ILAsm("int32")),
                Group(".imagebase", ILAsm("int32")),
                ILAsm("extSourceSpec"),
                ILAsm("languageDecl")
                 )
            )
        ' External Source Declarations
        ILAsm.
           AddRule("extSourceSpec",
                   Alt(Group(".line", ILAsm("int32"), ILAsm("SQSTRING")),
                       Group(".line", ILAsm("int32")),
                       Group(".line", ILAsm("int32"), CommonRules.Colon, ILAsm("int32"), ILAsm("SQSTRING")),
                       Group(".line", ILAsm("int32"), CommonRules.Colon, ILAsm("int32"))
                      )).
           AddRule("languageDecl",
                   Alt(Group(".language", ILAsm("SQSTRING")),
                       Group(".language", ILAsm("SQSTRING"), CommonRules.DOT, ILAsm("SQSTRING")),
                       Group(".language", ILAsm("SQSTRING"), CommonRules.DOT, ILAsm("SQSTRING"), CommonRules.DOT, ILAsm("SQSTRING"))
                       )
                   )
        ' V-Table Fixup Declaration
        ILAsm.
            AddRule(".vtfixup",
                    Group(ILAsm("int32").Optional, ILAsm("vtfixupAttr"), "at", ILAsm("id"))).
            AddRule("vtfixupAttr",
                    Alt(ILAsm("/* EMPTY */"),
                        Group(ILAsm("vtfixupAttr"), "int32"),
                        Group(ILAsm("vtfixupAttr"), "int64"),
                        Group(ILAsm("vtfixupAttr"), "fromunmanaged"),
                        Group(ILAsm("vtfixupAttr"), "callmostderived")
                       )
                   )


        Console.Write(ILAsm.PrettyPrint)


    End Sub

End Module



<DebuggerStepThrough()>
Public Class SourceText

    Dim Text As String = ""

    Public Sub New(text As String)
        Me.Text = text
    End Sub

    Public ReadOnly Property Length As Integer
        Get
            Return Text.Length
        End Get
    End Property

    Public Function CanGet(index As Integer) As Boolean 
        Return (0 <= index) AndAlso (index < Text.Length)
    End Function

    Public Async Function Chars(Index As Integer) As Task(Of Char?)
        If CanGet(Index) Then Return New Char?(Text(Index))
        Return New Char?()
    End Function

    Public Async Function SubText(si As Integer, ei As Integer) As Task(of String)
        If ei <= 0 Then Return ""
    Dim len = (ei - si) - 1
    Return Text.Substring(si, len)
    End Function
End Class


Public Module RuleDef_Exts
    <Extension>
    Public Function [Optional](rd As RuleDefinition) As RuleDefinition.Optionally
        Return New RuleDefinition.Optionally(rd)
    End Function
    <Extension>
    Public Function ZeroOrMore(rd As RuleDefinition) As RuleDefinition.ZeroOrMore
        Return New RuleDefinition.ZeroOrMore(rd)
    End Function
    <Extension>
    Public Function OneOrMore(rd As RuleDefinition) As RuleDefinition.OneOrMore
        Return New RuleDefinition.OneOrMore(rd)
    End Function
    <Extension>
    Public Function Repeat(rd As RuleDefinition, times As Integer) As RuleDefinition.Repeats
        Return New RuleDefinition.Repeats(rd, times)
    End Function
    <Extension>
    Public Function Repeat(rd As RuleDefinition, lb As Integer, ub As Integer) As RuleDefinition.RepeatBound
        Return New RuleDefinition.RepeatBound(rd, lb, ub)
    End Function
    <Extension>
    Public Function [To](lc As Char, uc As Char) As RuleDefinition.Range
        Return New RuleDefinition.Range(lc, uc)
    End Function
    '<Extension>
    'Public Function AsText(s As String) As RuleDefinition.TextDef
    '    Return New RuleDefinition.TextDef(s)
    'End Function
    '<Extension>
    'Public Function Q(c As Char) As RuleDefinition.TextDef
    '    Return New RuleDefinition.TextDef(c)
    'End Function
    '<Extension>
    'Public Function AsGroup(Of T As RuleDefinition)(g() As T) As RuleDefinition.Group
    '    Return New RuleDefinition.Group(g)
    'End Function
    Public Function Group(ParamArray g() As RuleDefinition) As RuleDefinition.Group
        Return New RuleDefinition.Group(g)
    End Function
    '<Extension>
    'Public Function AsAlt(Of T As RuleDefinition)(a() As T) As RuleDefinition.Alternatives
    '    Return New RuleDefinition.Alternatives(a)
    'End Function

    Public Function Alt(ParamArray a() As RuleDefinition) As RuleDefinition.Alternatives
        Return New RuleDefinition.Alternatives(a)
    End Function

    <Extension>
    Public Function Tick(b As Boolean) As Char
        If b Then Return "Y"c Else Return "X"c
    End Function
End Module

