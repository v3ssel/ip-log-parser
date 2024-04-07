using System.Globalization;
using System.Net;
using IpLogParser.Options;
using IpLogParser.Shared;

namespace IpLogParser.Reader;

public class IpLogReader
{
    public virtual IpLogReaderResult Read(IpLogParserOptions options)
    {
        var bounds = IpUtils.GetAddressBounds(options.AddressStart, options.AddressMask);
        var err_list = new List<Exception>();
        var result_dict = new Dictionary<IPAddress, long>();
        
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

                    if (IpUtils.AddressMatch(ip_address, bounds.Item1, bounds.Item2))
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
}
