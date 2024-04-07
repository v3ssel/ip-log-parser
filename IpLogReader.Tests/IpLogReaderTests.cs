using System.Net;
using IpLogParser.Options;
using IpLogParser.Reader;
using Xunit;

namespace IpLogParser.Tests;

public class IpLogReaderTests
{
    [Fact]
    public void ReadLogWithAllBounds_ExpectNonEmptyOutput()
    {
        var options = new IpLogParserOptions()
        {
            FileLog = "test_input/input1.log",
            FileOutput = "test_input/output1.log",
            AddressStart = IPAddress.Parse("1.0.10.100"),
            AddressMask = 2,
            TimeStart = new DateTime(2023, 1, 1),
            TimeEnd = new DateTime(2024, 1, 1)
        };

        var expected_dict = new Dictionary<IPAddress, long>()
        {
            {IPAddress.Parse("5.58.183.222"), 1},
            {IPAddress.Parse("29.40.207.207"), 2},
            {IPAddress.Parse("44.44.167.154"), 1},
        };

        var reader = new IpLogReader();
        var actual = reader.Read(options);

        Assert.Empty(actual.Errors!);
        Assert.NotEmpty(actual.AddressToRequestCount!);

        Assert.Equal(expected_dict, actual.AddressToRequestCount);
    }

    [Fact]
    public void ReadLogWithAllBounds_ExpectEmptyOutput()
    {
        var options = new IpLogParserOptions()
        {
            FileLog = "test_input/input1.log",
            FileOutput = "test_input/output1.log",
            AddressStart = IPAddress.Parse("100.0.0.100"),
            AddressMask = 8,
            TimeStart = new DateTime(2023, 1, 1),
            TimeEnd = new DateTime(2024, 1, 1)
        };;

        var reader = new IpLogReader();
        var actual = reader.Read(options);

        Assert.Empty(actual.Errors!);
        Assert.Empty(actual.AddressToRequestCount!);
    }

    [Fact]
    public void ReadLogWithTimeBounds_ExpectNonEmptyOutput()
    {
        var options = new IpLogParserOptions()
        {
            FileLog = "test_input/input2.log",
            FileOutput = "test_input/output2.log",
            TimeStart = new DateTime(2023, 11, 6),
            TimeEnd = new DateTime(2023, 11, 15)
        };

        var expected_dict = new Dictionary<IPAddress, long>()
        {
            {IPAddress.Parse("142.250.184.238"), 3},
            {IPAddress.Parse("52.86.205.118"), 3},
            {IPAddress.Parse("192.0.2.1"), 3},
            {IPAddress.Parse("203.0.113.15"), 4},
            {IPAddress.Parse("198.51.100.42"), 2},
        };

        var reader = new IpLogReader();
        var actual = reader.Read(options);

        Assert.Empty(actual.Errors!);
        Assert.NotEmpty(actual.AddressToRequestCount!);

        Assert.Equal(expected_dict, actual.AddressToRequestCount);
    }

    [Fact]
    public void ReadLogWithoutBounds_ExpectNonEmptyOutput()
    {
        var options = new IpLogParserOptions()
        {
            FileLog = "test_input/input2.log",
            FileOutput = "test_input/output2.log",
        };;

        var reader = new IpLogReader();
        var actual = reader.Read(options);

        Assert.Empty(actual.Errors!);
        Assert.NotEmpty(actual.AddressToRequestCount!);

        var expected_dict_size = 40;
        Assert.Equal(expected_dict_size, actual.AddressToRequestCount!.Count);
    }
}
