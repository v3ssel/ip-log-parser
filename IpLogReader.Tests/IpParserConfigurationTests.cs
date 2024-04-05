using Xunit;
using IpLogReader;
using System.Net;
namespace IpLogReader.Tests;

public class IpParserConfigurationTests
{
    [Fact]
    public void ParseOptionsWithCommandLineArgs()
    {
        var args = new string[]
        {
            "--file-log=input.log",
            "--file-output=output.log",
            "--address-start=192.168.0.0",
            "--address-mask=8",
            "--time-start=01.01.2024",
            "--time-end=01.04.2024"
        };

        var parser = new IpParserConfiguration(args);
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Equal(IPAddress.Parse("192.168.0.0"), options.AddressStart);   
        Assert.Equal(8, options.AddressMask);
        Assert.Equal(new DateTime(2024, 1, 1), options.TimeStart);
        Assert.Equal(new DateTime(2024, 4, 1), options.TimeEnd);
    }
}