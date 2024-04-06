using System.Globalization;
using System.Net;
using IpLogParser.Options;
using IpLogParser.Shared;

namespace IpLogParser.Reader;

public class IpLogReader
{
    private readonly IArgsParser OptionsParser;

    public IpLogReader(IArgsParser options_parser)
    {
        OptionsParser = options_parser;
    }

    public IpLogReaderResult Read()
    {
        var options = OptionsParser.Parse();
 
        var (lower_address_bytes, upper_address_bytes) =  GetAddressBounds(options);
        var err_list = new List<Exception>();
        var result_dict = new Dictionary<IPAddress, long>();
        
        // int i = 0;
        foreach (var line in File.ReadLines(options.FileLog!))
        {
            try
            {
                if (string.IsNullOrEmpty(line)) continue;

                var separator = line.IndexOf(':');
                var time = DateTime.ParseExact(line[(separator + 1)..], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                if (time >= options.TimeStart && time <= options.TimeEnd)
                {
                    var ip_address = IPAddress.Parse(line[..separator]);

                    // var up = string.Join(".", upper_address_bytes ?? ""u8.ToArray());
                    // Console.WriteLine($"{string.Format("{0,4:D}", ++i)}. {options.AddressStart} {up} {string.Format("{0,20}", time)} {ip_address}");
                    if (AddressMatch(ip_address.GetAddressBytes(), lower_address_bytes, upper_address_bytes))
                    {
                        if (!result_dict.TryAdd(ip_address, 1))
                            result_dict[ip_address]++;
                    }
                }
            }
            catch(Exception ex)
            {
                err_list.Add(ex);
            }
        }

        return new IpLogReaderResult
        {
            Errors = err_list,
            AddressToRequestCount = result_dict
        };
    }

    public static bool AddressMatch(byte[] address, byte[]? lower_bound, byte[]? upper_bound)
    {
        if (lower_bound is null && upper_bound is null)
            return true;

        for (var i = 0; i < address.Length; i++)
        {
            if ((lower_bound is not null && address[i] < lower_bound[i]) || (upper_bound is not null && address[i] > upper_bound[i]))
                return false;
        }
      
        return true;
    }

    private (byte[]?, byte[]?) GetAddressBounds(IpLogParserOptions options)
    {
        byte[]? upper_address_bytes = null;
        byte[]? lower_address_bytes = null;

        if (options.AddressStart is not null)
        {
            lower_address_bytes = options.AddressStart.GetAddressBytes();

            if (options.AddressMask is not null)
            {
                var mask_address = IpUtils.MaskToAddress((int)options.AddressMask);
                upper_address_bytes = IpUtils.GetLastUsableAddress(options.AddressStart, mask_address).GetAddressBytes();
            }
        }

        return (lower_address_bytes, upper_address_bytes);
    }
}
