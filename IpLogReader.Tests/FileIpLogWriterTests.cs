using System.Net;
using IpLogParser.Reader;
using IpLogParser.Writer;
using Xunit;

namespace IpLogParser.Tests;

public class FileIpLogWriterTests
{
    private static IpLogReaderResult GenerateResult(int size)
    {
        var result_dict = new Dictionary<IPAddress, long>();

        var rand = new Random();
        for (int i = 0; i < size; i++)
        {
            result_dict.Add(IPAddress.Parse($"{rand.NextInt64(0, 255)}.{rand.NextInt64(0, 255)}.{rand.NextInt64(0, 255)}.{rand.NextInt64(0, 255)}"), rand.NextInt64(1, 99));
        }

        return new IpLogReaderResult()
        {
            AddressToRequestCount = result_dict,
            Errors = []
        };
    }

    [Fact]
    public void WriteSmallData()
    {
        var results_size = 10;
        var random_results = GenerateResult(results_size);

        var log_output = "output.log";

        var writer = new FileIpLogWriter();
        writer.Write(log_output, random_results);

        Assert.True(File.Exists(log_output));

        var lines = File.ReadLines(log_output);
        Assert.Equal(results_size, lines.Count());
    }

    [Fact]
    public async Task WriteAsyncSmallData()
    {
        var results_size = 10;
        var random_results = GenerateResult(results_size);

        var log_output = "output.log";

        var writer = new FileIpLogWriter();
        await writer.WriteAsync(log_output, random_results);

        Assert.True(File.Exists(log_output));

        var lines = File.ReadLines(log_output);
        Assert.Equal(results_size, lines.Count());
    }
    
    [Fact]
    public async Task WriteAsyncBigData()
    {
        var results_size = 10000;
        var random_results = GenerateResult(results_size);

        var log_output = "output_big.log";

        var writer = new FileIpLogWriter();
        await writer.WriteAsync(log_output, random_results);

        Assert.True(File.Exists(log_output));

        var lines = File.ReadLines(log_output);
        Assert.Equal(results_size, lines.Count());
    }
}
