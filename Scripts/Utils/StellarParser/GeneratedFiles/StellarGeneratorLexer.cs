﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\programs\Godot\GloryOfRitiria\Parsers\StellarSystemParser\StellarGenerator.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace StellarSystemParser {

    using System.Globalization;
    using GloryOfRitiria.Scripts.Utils;

using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public partial class StellarGeneratorLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, INT=33, FLOAT=34, ID=35, WORD=36, TEXT=37, WHITESPACE=38, COMMENT=39, 
		NEWLINE=40;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "T__28", "T__29", "T__30", "T__31", "UNDERSCORE", 
		"DASH", "LOWERCASE", "UPPERCASE", "NUMBER", "INT", "FLOAT", "ID", "WORD", 
		"TEXT", "WHITESPACE", "COMMENT", "NEWLINE"
	};


	public StellarGeneratorLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'stellar_system'", "'{'", "'}'", "'star'", "'celestial_body'", 
		"'satellites'", "'shipyards'", "'shipyard'", "'ships'", "'ship'", "'modules'", 
		"'module'", "'durability'", "':'", "'name'", "'icon'", "'type'", "'size'", 
		"'distance_from_detnura'", "'map_angle'", "'gravitational_pull'", "'distance'", 
		"'science'", "'building_progress'", "'star_class'", "'orange_dwarf'", 
		"'red_dwarf'", "'yellow_dwarf'", "'discovery_status'", "'explored'", "'existence_known'", 
		"'undiscovered'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, "INT", "FLOAT", 
		"ID", "WORD", "TEXT", "WHITESPACE", "COMMENT", "NEWLINE"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete("Use IRecognizer.Vocabulary instead.")]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "StellarGenerator.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2*\x1EA\b\x1\x4\x2"+
		"\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b\x4"+
		"\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4\x10"+
		"\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15\t\x15"+
		"\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A\x4\x1B"+
		"\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x4 \t \x4!"+
		"\t!\x4\"\t\"\x4#\t#\x4$\t$\x4%\t%\x4&\t&\x4\'\t\'\x4(\t(\x4)\t)\x4*\t"+
		"*\x4+\t+\x4,\t,\x4-\t-\x4.\t.\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2"+
		"\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x3\x3\x3\x3\x4\x3"+
		"\x4\x3\x5\x3\x5\x3\x5\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6"+
		"\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\x6\x3\a\x3\a\x3\a"+
		"\x3\a\x3\a\x3\a\x3\a\x3\a\x3\a\x3\a\x3\a\x3\b\x3\b\x3\b\x3\b\x3\b\x3\b"+
		"\x3\b\x3\b\x3\b\x3\b\x3\t\x3\t\x3\t\x3\t\x3\t\x3\t\x3\t\x3\t\x3\t\x3\n"+
		"\x3\n\x3\n\x3\n\x3\n\x3\n\x3\v\x3\v\x3\v\x3\v\x3\v\x3\f\x3\f\x3\f\x3\f"+
		"\x3\f\x3\f\x3\f\x3\f\x3\r\x3\r\x3\r\x3\r\x3\r\x3\r\x3\r\x3\xE\x3\xE\x3"+
		"\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xE\x3\xF\x3\xF\x3\x10"+
		"\x3\x10\x3\x10\x3\x10\x3\x10\x3\x11\x3\x11\x3\x11\x3\x11\x3\x11\x3\x12"+
		"\x3\x12\x3\x12\x3\x12\x3\x12\x3\x13\x3\x13\x3\x13\x3\x13\x3\x13\x3\x14"+
		"\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14"+
		"\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14\x3\x14"+
		"\x3\x14\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15\x3\x15"+
		"\x3\x15\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16"+
		"\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16\x3\x16"+
		"\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x17\x3\x18"+
		"\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x18\x3\x19\x3\x19\x3\x19"+
		"\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19"+
		"\x3\x19\x3\x19\x3\x19\x3\x19\x3\x19\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A"+
		"\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1B\x3\x1B"+
		"\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1B\x3\x1C"+
		"\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1C\x3\x1D"+
		"\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D\x3\x1D"+
		"\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E"+
		"\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1E\x3\x1F"+
		"\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3\x1F\x3 \x3 \x3 \x3"+
		" \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3 \x3!\x3!\x3!\x3!\x3!"+
		"\x3!\x3!\x3!\x3!\x3!\x3!\x3!\x3!\x3\"\x3\"\x3#\x3#\x3$\x3$\x3%\x3%\x3"+
		"&\x3&\x3\'\x6\'\x1A5\n\'\r\'\xE\'\x1A6\x3(\x6(\x1AA\n(\r(\xE(\x1AB\x3"+
		"(\x3(\x6(\x1B0\n(\r(\xE(\x1B1\x6(\x1B4\n(\r(\xE(\x1B5\x5(\x1B8\n(\x3)"+
		"\x3)\a)\x1BC\n)\f)\xE)\x1BF\v)\x3*\x3*\x3*\x3*\x6*\x1C5\n*\r*\xE*\x1C6"+
		"\x3+\x3+\a+\x1CB\n+\f+\xE+\x1CE\v+\x3+\x3+\x3,\x6,\x1D3\n,\r,\xE,\x1D4"+
		"\x3,\x3,\x3-\x3-\a-\x1DB\n-\f-\xE-\x1DE\v-\x3-\x3-\x3.\x5.\x1E3\n.\x3"+
		".\x3.\x6.\x1E7\n.\r.\xE.\x1E8\x3\x1CC\x2\x2/\x3\x2\x3\x5\x2\x4\a\x2\x5"+
		"\t\x2\x6\v\x2\a\r\x2\b\xF\x2\t\x11\x2\n\x13\x2\v\x15\x2\f\x17\x2\r\x19"+
		"\x2\xE\x1B\x2\xF\x1D\x2\x10\x1F\x2\x11!\x2\x12#\x2\x13%\x2\x14\'\x2\x15"+
		")\x2\x16+\x2\x17-\x2\x18/\x2\x19\x31\x2\x1A\x33\x2\x1B\x35\x2\x1C\x37"+
		"\x2\x1D\x39\x2\x1E;\x2\x1F=\x2 ?\x2!\x41\x2\"\x43\x2\x2\x45\x2\x2G\x2"+
		"\x2I\x2\x2K\x2\x2M\x2#O\x2$Q\x2%S\x2&U\x2\'W\x2(Y\x2)[\x2*\x3\x2\b\x3"+
		"\x2\x63|\x3\x2\x43\\\x3\x2\x32;\x5\x2\x32;\x61\x61\x63|\x5\x2\v\f\xF\xF"+
		"\"\"\x4\x2\f\f\xF\xF\x1F4\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3"+
		"\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3"+
		"\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2\x2\x2\x2"+
		"\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D\x3\x2\x2"+
		"\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3\x2\x2\x2"+
		"\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2\x2\x2/"+
		"\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33\x3\x2\x2\x2\x2\x35\x3\x2\x2\x2"+
		"\x2\x37\x3\x2\x2\x2\x2\x39\x3\x2\x2\x2\x2;\x3\x2\x2\x2\x2=\x3\x2\x2\x2"+
		"\x2?\x3\x2\x2\x2\x2\x41\x3\x2\x2\x2\x2M\x3\x2\x2\x2\x2O\x3\x2\x2\x2\x2"+
		"Q\x3\x2\x2\x2\x2S\x3\x2\x2\x2\x2U\x3\x2\x2\x2\x2W\x3\x2\x2\x2\x2Y\x3\x2"+
		"\x2\x2\x2[\x3\x2\x2\x2\x3]\x3\x2\x2\x2\x5l\x3\x2\x2\x2\an\x3\x2\x2\x2"+
		"\tp\x3\x2\x2\x2\vu\x3\x2\x2\x2\r\x84\x3\x2\x2\x2\xF\x8F\x3\x2\x2\x2\x11"+
		"\x99\x3\x2\x2\x2\x13\xA2\x3\x2\x2\x2\x15\xA8\x3\x2\x2\x2\x17\xAD\x3\x2"+
		"\x2\x2\x19\xB5\x3\x2\x2\x2\x1B\xBC\x3\x2\x2\x2\x1D\xC7\x3\x2\x2\x2\x1F"+
		"\xC9\x3\x2\x2\x2!\xCE\x3\x2\x2\x2#\xD3\x3\x2\x2\x2%\xD8\x3\x2\x2\x2\'"+
		"\xDD\x3\x2\x2\x2)\xF3\x3\x2\x2\x2+\xFD\x3\x2\x2\x2-\x110\x3\x2\x2\x2/"+
		"\x119\x3\x2\x2\x2\x31\x121\x3\x2\x2\x2\x33\x133\x3\x2\x2\x2\x35\x13E\x3"+
		"\x2\x2\x2\x37\x14B\x3\x2\x2\x2\x39\x155\x3\x2\x2\x2;\x162\x3\x2\x2\x2"+
		"=\x173\x3\x2\x2\x2?\x17C\x3\x2\x2\x2\x41\x18C\x3\x2\x2\x2\x43\x199\x3"+
		"\x2\x2\x2\x45\x19B\x3\x2\x2\x2G\x19D\x3\x2\x2\x2I\x19F\x3\x2\x2\x2K\x1A1"+
		"\x3\x2\x2\x2M\x1A4\x3\x2\x2\x2O\x1A9\x3\x2\x2\x2Q\x1B9\x3\x2\x2\x2S\x1C4"+
		"\x3\x2\x2\x2U\x1C8\x3\x2\x2\x2W\x1D2\x3\x2\x2\x2Y\x1D8\x3\x2\x2\x2[\x1E6"+
		"\x3\x2\x2\x2]^\au\x2\x2^_\av\x2\x2_`\ag\x2\x2`\x61\an\x2\x2\x61\x62\a"+
		"n\x2\x2\x62\x63\a\x63\x2\x2\x63\x64\at\x2\x2\x64\x65\a\x61\x2\x2\x65\x66"+
		"\au\x2\x2\x66g\a{\x2\x2gh\au\x2\x2hi\av\x2\x2ij\ag\x2\x2jk\ao\x2\x2k\x4"+
		"\x3\x2\x2\x2lm\a}\x2\x2m\x6\x3\x2\x2\x2no\a\x7F\x2\x2o\b\x3\x2\x2\x2p"+
		"q\au\x2\x2qr\av\x2\x2rs\a\x63\x2\x2st\at\x2\x2t\n\x3\x2\x2\x2uv\a\x65"+
		"\x2\x2vw\ag\x2\x2wx\an\x2\x2xy\ag\x2\x2yz\au\x2\x2z{\av\x2\x2{|\ak\x2"+
		"\x2|}\a\x63\x2\x2}~\an\x2\x2~\x7F\a\x61\x2\x2\x7F\x80\a\x64\x2\x2\x80"+
		"\x81\aq\x2\x2\x81\x82\a\x66\x2\x2\x82\x83\a{\x2\x2\x83\f\x3\x2\x2\x2\x84"+
		"\x85\au\x2\x2\x85\x86\a\x63\x2\x2\x86\x87\av\x2\x2\x87\x88\ag\x2\x2\x88"+
		"\x89\an\x2\x2\x89\x8A\an\x2\x2\x8A\x8B\ak\x2\x2\x8B\x8C\av\x2\x2\x8C\x8D"+
		"\ag\x2\x2\x8D\x8E\au\x2\x2\x8E\xE\x3\x2\x2\x2\x8F\x90\au\x2\x2\x90\x91"+
		"\aj\x2\x2\x91\x92\ak\x2\x2\x92\x93\ar\x2\x2\x93\x94\a{\x2\x2\x94\x95\a"+
		"\x63\x2\x2\x95\x96\at\x2\x2\x96\x97\a\x66\x2\x2\x97\x98\au\x2\x2\x98\x10"+
		"\x3\x2\x2\x2\x99\x9A\au\x2\x2\x9A\x9B\aj\x2\x2\x9B\x9C\ak\x2\x2\x9C\x9D"+
		"\ar\x2\x2\x9D\x9E\a{\x2\x2\x9E\x9F\a\x63\x2\x2\x9F\xA0\at\x2\x2\xA0\xA1"+
		"\a\x66\x2\x2\xA1\x12\x3\x2\x2\x2\xA2\xA3\au\x2\x2\xA3\xA4\aj\x2\x2\xA4"+
		"\xA5\ak\x2\x2\xA5\xA6\ar\x2\x2\xA6\xA7\au\x2\x2\xA7\x14\x3\x2\x2\x2\xA8"+
		"\xA9\au\x2\x2\xA9\xAA\aj\x2\x2\xAA\xAB\ak\x2\x2\xAB\xAC\ar\x2\x2\xAC\x16"+
		"\x3\x2\x2\x2\xAD\xAE\ao\x2\x2\xAE\xAF\aq\x2\x2\xAF\xB0\a\x66\x2\x2\xB0"+
		"\xB1\aw\x2\x2\xB1\xB2\an\x2\x2\xB2\xB3\ag\x2\x2\xB3\xB4\au\x2\x2\xB4\x18"+
		"\x3\x2\x2\x2\xB5\xB6\ao\x2\x2\xB6\xB7\aq\x2\x2\xB7\xB8\a\x66\x2\x2\xB8"+
		"\xB9\aw\x2\x2\xB9\xBA\an\x2\x2\xBA\xBB\ag\x2\x2\xBB\x1A\x3\x2\x2\x2\xBC"+
		"\xBD\a\x66\x2\x2\xBD\xBE\aw\x2\x2\xBE\xBF\at\x2\x2\xBF\xC0\a\x63\x2\x2"+
		"\xC0\xC1\a\x64\x2\x2\xC1\xC2\ak\x2\x2\xC2\xC3\an\x2\x2\xC3\xC4\ak\x2\x2"+
		"\xC4\xC5\av\x2\x2\xC5\xC6\a{\x2\x2\xC6\x1C\x3\x2\x2\x2\xC7\xC8\a<\x2\x2"+
		"\xC8\x1E\x3\x2\x2\x2\xC9\xCA\ap\x2\x2\xCA\xCB\a\x63\x2\x2\xCB\xCC\ao\x2"+
		"\x2\xCC\xCD\ag\x2\x2\xCD \x3\x2\x2\x2\xCE\xCF\ak\x2\x2\xCF\xD0\a\x65\x2"+
		"\x2\xD0\xD1\aq\x2\x2\xD1\xD2\ap\x2\x2\xD2\"\x3\x2\x2\x2\xD3\xD4\av\x2"+
		"\x2\xD4\xD5\a{\x2\x2\xD5\xD6\ar\x2\x2\xD6\xD7\ag\x2\x2\xD7$\x3\x2\x2\x2"+
		"\xD8\xD9\au\x2\x2\xD9\xDA\ak\x2\x2\xDA\xDB\a|\x2\x2\xDB\xDC\ag\x2\x2\xDC"+
		"&\x3\x2\x2\x2\xDD\xDE\a\x66\x2\x2\xDE\xDF\ak\x2\x2\xDF\xE0\au\x2\x2\xE0"+
		"\xE1\av\x2\x2\xE1\xE2\a\x63\x2\x2\xE2\xE3\ap\x2\x2\xE3\xE4\a\x65\x2\x2"+
		"\xE4\xE5\ag\x2\x2\xE5\xE6\a\x61\x2\x2\xE6\xE7\ah\x2\x2\xE7\xE8\at\x2\x2"+
		"\xE8\xE9\aq\x2\x2\xE9\xEA\ao\x2\x2\xEA\xEB\a\x61\x2\x2\xEB\xEC\a\x66\x2"+
		"\x2\xEC\xED\ag\x2\x2\xED\xEE\av\x2\x2\xEE\xEF\ap\x2\x2\xEF\xF0\aw\x2\x2"+
		"\xF0\xF1\at\x2\x2\xF1\xF2\a\x63\x2\x2\xF2(\x3\x2\x2\x2\xF3\xF4\ao\x2\x2"+
		"\xF4\xF5\a\x63\x2\x2\xF5\xF6\ar\x2\x2\xF6\xF7\a\x61\x2\x2\xF7\xF8\a\x63"+
		"\x2\x2\xF8\xF9\ap\x2\x2\xF9\xFA\ai\x2\x2\xFA\xFB\an\x2\x2\xFB\xFC\ag\x2"+
		"\x2\xFC*\x3\x2\x2\x2\xFD\xFE\ai\x2\x2\xFE\xFF\at\x2\x2\xFF\x100\a\x63"+
		"\x2\x2\x100\x101\ax\x2\x2\x101\x102\ak\x2\x2\x102\x103\av\x2\x2\x103\x104"+
		"\a\x63\x2\x2\x104\x105\av\x2\x2\x105\x106\ak\x2\x2\x106\x107\aq\x2\x2"+
		"\x107\x108\ap\x2\x2\x108\x109\a\x63\x2\x2\x109\x10A\an\x2\x2\x10A\x10B"+
		"\a\x61\x2\x2\x10B\x10C\ar\x2\x2\x10C\x10D\aw\x2\x2\x10D\x10E\an\x2\x2"+
		"\x10E\x10F\an\x2\x2\x10F,\x3\x2\x2\x2\x110\x111\a\x66\x2\x2\x111\x112"+
		"\ak\x2\x2\x112\x113\au\x2\x2\x113\x114\av\x2\x2\x114\x115\a\x63\x2\x2"+
		"\x115\x116\ap\x2\x2\x116\x117\a\x65\x2\x2\x117\x118\ag\x2\x2\x118.\x3"+
		"\x2\x2\x2\x119\x11A\au\x2\x2\x11A\x11B\a\x65\x2\x2\x11B\x11C\ak\x2\x2"+
		"\x11C\x11D\ag\x2\x2\x11D\x11E\ap\x2\x2\x11E\x11F\a\x65\x2\x2\x11F\x120"+
		"\ag\x2\x2\x120\x30\x3\x2\x2\x2\x121\x122\a\x64\x2\x2\x122\x123\aw\x2\x2"+
		"\x123\x124\ak\x2\x2\x124\x125\an\x2\x2\x125\x126\a\x66\x2\x2\x126\x127"+
		"\ak\x2\x2\x127\x128\ap\x2\x2\x128\x129\ai\x2\x2\x129\x12A\a\x61\x2\x2"+
		"\x12A\x12B\ar\x2\x2\x12B\x12C\at\x2\x2\x12C\x12D\aq\x2\x2\x12D\x12E\a"+
		"i\x2\x2\x12E\x12F\at\x2\x2\x12F\x130\ag\x2\x2\x130\x131\au\x2\x2\x131"+
		"\x132\au\x2\x2\x132\x32\x3\x2\x2\x2\x133\x134\au\x2\x2\x134\x135\av\x2"+
		"\x2\x135\x136\a\x63\x2\x2\x136\x137\at\x2\x2\x137\x138\a\x61\x2\x2\x138"+
		"\x139\a\x65\x2\x2\x139\x13A\an\x2\x2\x13A\x13B\a\x63\x2\x2\x13B\x13C\a"+
		"u\x2\x2\x13C\x13D\au\x2\x2\x13D\x34\x3\x2\x2\x2\x13E\x13F\aq\x2\x2\x13F"+
		"\x140\at\x2\x2\x140\x141\a\x63\x2\x2\x141\x142\ap\x2\x2\x142\x143\ai\x2"+
		"\x2\x143\x144\ag\x2\x2\x144\x145\a\x61\x2\x2\x145\x146\a\x66\x2\x2\x146"+
		"\x147\ay\x2\x2\x147\x148\a\x63\x2\x2\x148\x149\at\x2\x2\x149\x14A\ah\x2"+
		"\x2\x14A\x36\x3\x2\x2\x2\x14B\x14C\at\x2\x2\x14C\x14D\ag\x2\x2\x14D\x14E"+
		"\a\x66\x2\x2\x14E\x14F\a\x61\x2\x2\x14F\x150\a\x66\x2\x2\x150\x151\ay"+
		"\x2\x2\x151\x152\a\x63\x2\x2\x152\x153\at\x2\x2\x153\x154\ah\x2\x2\x154"+
		"\x38\x3\x2\x2\x2\x155\x156\a{\x2\x2\x156\x157\ag\x2\x2\x157\x158\an\x2"+
		"\x2\x158\x159\an\x2\x2\x159\x15A\aq\x2\x2\x15A\x15B\ay\x2\x2\x15B\x15C"+
		"\a\x61\x2\x2\x15C\x15D\a\x66\x2\x2\x15D\x15E\ay\x2\x2\x15E\x15F\a\x63"+
		"\x2\x2\x15F\x160\at\x2\x2\x160\x161\ah\x2\x2\x161:\x3\x2\x2\x2\x162\x163"+
		"\a\x66\x2\x2\x163\x164\ak\x2\x2\x164\x165\au\x2\x2\x165\x166\a\x65\x2"+
		"\x2\x166\x167\aq\x2\x2\x167\x168\ax\x2\x2\x168\x169\ag\x2\x2\x169\x16A"+
		"\at\x2\x2\x16A\x16B\a{\x2\x2\x16B\x16C\a\x61\x2\x2\x16C\x16D\au\x2\x2"+
		"\x16D\x16E\av\x2\x2\x16E\x16F\a\x63\x2\x2\x16F\x170\av\x2\x2\x170\x171"+
		"\aw\x2\x2\x171\x172\au\x2\x2\x172<\x3\x2\x2\x2\x173\x174\ag\x2\x2\x174"+
		"\x175\az\x2\x2\x175\x176\ar\x2\x2\x176\x177\an\x2\x2\x177\x178\aq\x2\x2"+
		"\x178\x179\at\x2\x2\x179\x17A\ag\x2\x2\x17A\x17B\a\x66\x2\x2\x17B>\x3"+
		"\x2\x2\x2\x17C\x17D\ag\x2\x2\x17D\x17E\az\x2\x2\x17E\x17F\ak\x2\x2\x17F"+
		"\x180\au\x2\x2\x180\x181\av\x2\x2\x181\x182\ag\x2\x2\x182\x183\ap\x2\x2"+
		"\x183\x184\a\x65\x2\x2\x184\x185\ag\x2\x2\x185\x186\a\x61\x2\x2\x186\x187"+
		"\am\x2\x2\x187\x188\ap\x2\x2\x188\x189\aq\x2\x2\x189\x18A\ay\x2\x2\x18A"+
		"\x18B\ap\x2\x2\x18B@\x3\x2\x2\x2\x18C\x18D\aw\x2\x2\x18D\x18E\ap\x2\x2"+
		"\x18E\x18F\a\x66\x2\x2\x18F\x190\ak\x2\x2\x190\x191\au\x2\x2\x191\x192"+
		"\a\x65\x2\x2\x192\x193\aq\x2\x2\x193\x194\ax\x2\x2\x194\x195\ag\x2\x2"+
		"\x195\x196\at\x2\x2\x196\x197\ag\x2\x2\x197\x198\a\x66\x2\x2\x198\x42"+
		"\x3\x2\x2\x2\x199\x19A\a\x61\x2\x2\x19A\x44\x3\x2\x2\x2\x19B\x19C\a/\x2"+
		"\x2\x19C\x46\x3\x2\x2\x2\x19D\x19E\t\x2\x2\x2\x19EH\x3\x2\x2\x2\x19F\x1A0"+
		"\t\x3\x2\x2\x1A0J\x3\x2\x2\x2\x1A1\x1A2\t\x4\x2\x2\x1A2L\x3\x2\x2\x2\x1A3"+
		"\x1A5\x5K&\x2\x1A4\x1A3\x3\x2\x2\x2\x1A5\x1A6\x3\x2\x2\x2\x1A6\x1A4\x3"+
		"\x2\x2\x2\x1A6\x1A7\x3\x2\x2\x2\x1A7N\x3\x2\x2\x2\x1A8\x1AA\x5K&\x2\x1A9"+
		"\x1A8\x3\x2\x2\x2\x1AA\x1AB\x3\x2\x2\x2\x1AB\x1A9\x3\x2\x2\x2\x1AB\x1AC"+
		"\x3\x2\x2\x2\x1AC\x1B7\x3\x2\x2\x2\x1AD\x1B3\a\x30\x2\x2\x1AE\x1B0\x5"+
		"K&\x2\x1AF\x1AE\x3\x2\x2\x2\x1B0\x1B1\x3\x2\x2\x2\x1B1\x1AF\x3\x2\x2\x2"+
		"\x1B1\x1B2\x3\x2\x2\x2\x1B2\x1B4\x3\x2\x2\x2\x1B3\x1AF\x3\x2\x2\x2\x1B4"+
		"\x1B5\x3\x2\x2\x2\x1B5\x1B3\x3\x2\x2\x2\x1B5\x1B6\x3\x2\x2\x2\x1B6\x1B8"+
		"\x3\x2\x2\x2\x1B7\x1AD\x3\x2\x2\x2\x1B7\x1B8\x3\x2\x2\x2\x1B8P\x3\x2\x2"+
		"\x2\x1B9\x1BD\t\x2\x2\x2\x1BA\x1BC\t\x5\x2\x2\x1BB\x1BA\x3\x2\x2\x2\x1BC"+
		"\x1BF\x3\x2\x2\x2\x1BD\x1BB\x3\x2\x2\x2\x1BD\x1BE\x3\x2\x2\x2\x1BER\x3"+
		"\x2\x2\x2\x1BF\x1BD\x3\x2\x2\x2\x1C0\x1C5\x5G$\x2\x1C1\x1C5\x5I%\x2\x1C2"+
		"\x1C5\x5\x43\"\x2\x1C3\x1C5\x5\x45#\x2\x1C4\x1C0\x3\x2\x2\x2\x1C4\x1C1"+
		"\x3\x2\x2\x2\x1C4\x1C2\x3\x2\x2\x2\x1C4\x1C3\x3\x2\x2\x2\x1C5\x1C6\x3"+
		"\x2\x2\x2\x1C6\x1C4\x3\x2\x2\x2\x1C6\x1C7\x3\x2\x2\x2\x1C7T\x3\x2\x2\x2"+
		"\x1C8\x1CC\a$\x2\x2\x1C9\x1CB\v\x2\x2\x2\x1CA\x1C9\x3\x2\x2\x2\x1CB\x1CE"+
		"\x3\x2\x2\x2\x1CC\x1CD\x3\x2\x2\x2\x1CC\x1CA\x3\x2\x2\x2\x1CD\x1CF\x3"+
		"\x2\x2\x2\x1CE\x1CC\x3\x2\x2\x2\x1CF\x1D0\a$\x2\x2\x1D0V\x3\x2\x2\x2\x1D1"+
		"\x1D3\t\x6\x2\x2\x1D2\x1D1\x3\x2\x2\x2\x1D3\x1D4\x3\x2\x2\x2\x1D4\x1D2"+
		"\x3\x2\x2\x2\x1D4\x1D5\x3\x2\x2\x2\x1D5\x1D6\x3\x2\x2\x2\x1D6\x1D7\b,"+
		"\x2\x2\x1D7X\x3\x2\x2\x2\x1D8\x1DC\a%\x2\x2\x1D9\x1DB\n\a\x2\x2\x1DA\x1D9"+
		"\x3\x2\x2\x2\x1DB\x1DE\x3\x2\x2\x2\x1DC\x1DA\x3\x2\x2\x2\x1DC\x1DD\x3"+
		"\x2\x2\x2\x1DD\x1DF\x3\x2\x2\x2\x1DE\x1DC\x3\x2\x2\x2\x1DF\x1E0\b-\x2"+
		"\x2\x1E0Z\x3\x2\x2\x2\x1E1\x1E3\a\xF\x2\x2\x1E2\x1E1\x3\x2\x2\x2\x1E2"+
		"\x1E3\x3\x2\x2\x2\x1E3\x1E4\x3\x2\x2\x2\x1E4\x1E7\a\f\x2\x2\x1E5\x1E7"+
		"\a\xF\x2\x2\x1E6\x1E2\x3\x2\x2\x2\x1E6\x1E5\x3\x2\x2\x2\x1E7\x1E8\x3\x2"+
		"\x2\x2\x1E8\x1E6\x3\x2\x2\x2\x1E8\x1E9\x3\x2\x2\x2\x1E9\\\x3\x2\x2\x2"+
		"\x12\x2\x1A6\x1AB\x1B1\x1B5\x1B7\x1BB\x1BD\x1C4\x1C6\x1CC\x1D4\x1DC\x1E2"+
		"\x1E6\x1E8\x3\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace StellarSystemParser
