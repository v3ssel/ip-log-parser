
using System.Net;
using System.Net.Sockets;
using IpLogParser.Reader;
using IpLogParser.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using IpLogParser.Writer;
using System.Globalization;

namespace IpLogParser;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var parser = new ConfigurationArgsParser(args, "config.json");
            var log_reader = new IpLogReader();
            var writer = new FileIpLogWriter();

            var options = parser.Parse();
            var result = log_reader.Read(options);
            await writer.WriteAsync(options.FileOutput!, result);

            if (result.Errors?.Count() > 0)
            {
                Console.WriteLine($"While reading '{options.FileLog}' file there was {result.Errors.Count()} non critical errors occured.");
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err.Message);
                }
            }

            Console.WriteLine($"Result successfully exported to '{options.FileOutput}' ({result.AddressToRequestCount!.Count} lines total).");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Critical error occured: {e.Message}\nApplication cannot continue to work.\nPlease review your input arguments or log file.");
        }

        // --file-log=
        // --file-output=
        // --address-start=
        // --address-mask=
        // --time-start=
        // --time-end=
    }
}
