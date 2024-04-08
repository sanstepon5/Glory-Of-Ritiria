using System;
using System.IO;
using Antlr4.Runtime;
using StellarSystemParser;

public class MainProgram
{
	public static void Main()
	{
		Console.WriteLine("Lol");
		var fileName = Console.ReadLine();

		StreamReader fileStream = new StreamReader(fileName);
		AntlrInputStream input = new AntlrInputStream(fileStream);
		
		StellarGeneratorLexer lexer = new StellarGeneratorLexer(input);
		CommonTokenStream token_stream = new CommonTokenStream(lexer);
		StellarGeneratorParser parser = new StellarGeneratorParser(token_stream);
		
		//parser.ErrorHandler = new BailErrorStrategy(); // Stop parsing on syntax errors

		parser.file();

	}

}
