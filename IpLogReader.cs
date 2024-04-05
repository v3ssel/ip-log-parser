namespace IpLogReader;

public class IpLogReader
{
    private readonly IpParser OptionsParser;

    public IpLogReader(IpParser options_parser)
    {
        OptionsParser = options_parser;
    }

    public void Read()
    {
        var options = OptionsParser.Parse();

        
    }
}
