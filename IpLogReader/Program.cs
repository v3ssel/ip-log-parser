
using System.Net;
using System.Net.Sockets;
using IpLogParser.Reader;
using IpLogParser.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using IpLogParser.Writer;

namespace IpLogParser;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // var options = parser.Parse();
        
        // var mask = 26;
        // var real_ip = IPAddress.Parse("180.103.163.25");
        // var bytes = real_ip.GetAddressBytes();

        // var start_ip = IPAddress.Parse("50.100.150.0");
        // var end_ip = IpUtils.GetLastUsableAddress(start_ip, IpUtils.MaskToAddress(mask));

        // Console.WriteLine($"{string.Join('.', bytes)} | {start_ip} | {end_ip}");
        // var m = IpLogReader.AddressMatch(bytes, start_ip.GetAddressBytes(), end_ip.GetAddressBytes());
        // Console.WriteLine($"{m}");


        // foreach (var r in result.AddressToRequestCount)
        // {
        //     Console.WriteLine($"{r.Key} {r.Value}");
        // }

        try
        {
            var parser = new ArgsParserConfiguration(args, "config.json");
            var options = parser.Parse();

            var log_reader = new IpLogReader();
            var result = log_reader.Read(options);

            var writer = new FileIpLogWriter();
            await writer.WriteAsync(options.FileOutput!, result);

        }
        catch (Exception e)
        {
            Console.WriteLine($"Critical error occured: {e.Message}.\nPlease review your input arguments or log file.");
        }

        // --file-log=
        // --file-output=
        // --address-start=
        // --address-mask=
        // --time-start=
        // --time-end=
    }
}
