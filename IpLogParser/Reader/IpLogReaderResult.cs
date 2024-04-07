using System.Net;

namespace IpLogParser.Reader;

public class IpLogReaderResult
{
    public IDictionary<IPAddress, long> AddressToRequestCount { get; set; }  = new Dictionary<IPAddress, long>();
    public IEnumerable<Exception> Errors { get; set; } = new List<Exception>();
}
