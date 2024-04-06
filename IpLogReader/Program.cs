
using System.Net;
using System.Net.Sockets;
using IpLogParser.Reader;
using IpLogParser.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace IpLogParser;

internal class Program
{
    private static void Main(string[] args)
    {
        var parser = new ArgsParserConfiguration(args, "config.json");
        // var options = parser.Parse();
        
        // var mask = 26;
        // var real_ip = IPAddress.Parse("180.103.163.25");
        // var bytes = real_ip.GetAddressBytes();

        // var start_ip = IPAddress.Parse("50.100.150.0");
        // var end_ip = IpUtils.GetLastUsableAddress(start_ip, IpUtils.MaskToAddress(mask));

        // Console.WriteLine($"{string.Join('.', bytes)} | {start_ip} | {end_ip}");
        // var m = IpLogReader.AddressMatch(bytes, start_ip.GetAddressBytes(), end_ip.GetAddressBytes());
        // Console.WriteLine($"{m}");

        var log_reader = new IpLogReader(parser);
        var result = log_reader.Read();

        foreach (var r in result.AddressToRequestCount)
        {
            Console.WriteLine($"{r.Key} {r.Value}");
        }

        // 180.103.163.25




        // --file-log=
        // --file-output=
        // --address-start=
        // --address-mask=
        // --time-start=
        // --time-end=
    }
}
