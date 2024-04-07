using System.Net;

namespace IpLogParser.Reader;

public class IpLogReaderResult
{
    public IDictionary<IPAddress, long>? AddressToRequestCount { get; set; }
    public IEnumerable<Exception>? Errors { get; set; }
}
