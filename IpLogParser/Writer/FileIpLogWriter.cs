using IpLogParser.Reader;

namespace IpLogParser.Writer;

public class FileIpLogWriter : IIpLogWriter
{
    public void Write(string path, IpLogReaderResult data)
    {
        var task = Task.Run(() => WriteAsync(path, data));
        task.Wait();
    }

    public async Task WriteAsync(string path, IpLogReaderResult data)
    {
        var file = File.Open(path, FileMode.Create);

        using (var writer = new StreamWriter(file))
        {
            foreach (var result in data.AddressToRequestCount)
            {
                await writer.WriteLineAsync($"{result.Key}:{result.Value}");
            }

            await writer.FlushAsync();
        }
    }
}
