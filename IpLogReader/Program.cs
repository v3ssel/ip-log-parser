
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace IpLogReader;

internal class Program
{
    private static void Main(string[] args)
    {
        var parser = new IpParserConfiguration(args, "config.json");
        var options = parser.Parse();

        // --file-log=
        // --file-output=
        // --address-start=
        // --address-mask=
        // --time-start=
        // --time-end=

        Console.WriteLine($"--file-log={options.FileLog}\n--file-output={options.FileOutput}\n--address-start={options.AddressStart}\n--address-mask={options.AddressMask}\n--time-start={options.TimeStart}\n--time-end={options.TimeEnd}");
    }
}
