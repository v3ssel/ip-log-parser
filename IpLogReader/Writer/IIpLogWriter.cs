using IpLogParser.Reader;

namespace IpLogParser.Writer;

public interface IIpLogWriter
{
    void Write(string path, IpLogReaderResult data);
    Task WriteAsync(string path, IpLogReaderResult data);
}
