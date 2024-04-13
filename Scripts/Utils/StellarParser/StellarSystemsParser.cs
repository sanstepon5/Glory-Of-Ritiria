using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using GloryOfRitiria.Scripts.ShipRelated;
using StellarSystemParser;

namespace GloryOfRitiria.Scripts.Utils;

public class StellarSystemsParser
{
    private StellarGeneratorParser _parser;
    // public List<StarSystemInfo> Systems;
    // public List<Ship> Ships;
    // public List<CelestialBody> BodiesWithShipyards;
    // public StarSystemInfo Detnura;
    
    public StellarSystemsParser(StringReader fileStream)
    {
        var input = new AntlrInputStream(fileStream);
		
        var lexer = new StellarGeneratorLexer(input);
        var token_stream = new CommonTokenStream(lexer);
        _parser = new StellarGeneratorParser(token_stream);
    }

    public StelSysData Parse()
    {
        _parser.file();
        return StelSysGen.UnloadData();
    }
}