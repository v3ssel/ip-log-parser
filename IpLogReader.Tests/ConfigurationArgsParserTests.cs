using Xunit;
using IpLogParser.Options;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace IpLogParser.Tests;

public class ConfigurationArgsParserTests
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

        var parser = new ConfigurationArgsParser(args);
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Equal(IPAddress.Parse("192.168.0.0"), options.AddressStart);   
        Assert.Equal(8, options.AddressMask);
        Assert.Equal(new DateTime(2024, 1, 1), options.TimeStart);
        Assert.Equal(new DateTime(2024, 4, 1), options.TimeEnd);
    }

    [Fact]
    public void ParseOptionsWithJsonConfig()
    {
        var args = Array.Empty<string>();
        var parser = new ConfigurationArgsParser(args, "full_config.json");
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Equal(IPAddress.Parse("192.168.0.0"), options.AddressStart);   
        Assert.Equal(16, options.AddressMask);
        Assert.Equal(new DateTime(2024, 4, 1), options.TimeStart);
        Assert.Equal(new DateTime(2024, 4, 30), options.TimeEnd);
    }

    [Fact]
    public void ParseOptionsWithEnvVars()
    {
        System.Environment.SetEnvironmentVariable("file-log", "input.log");
        System.Environment.SetEnvironmentVariable("file-output", "output.log");
        System.Environment.SetEnvironmentVariable("address-start", "172.16.0.0");
        System.Environment.SetEnvironmentVariable("address-mask", "4");
        System.Environment.SetEnvironmentVariable("time-start", "15.01.2024");
        System.Environment.SetEnvironmentVariable("time-end", "29.02.2024");
        
        var args = Array.Empty<string>();
        var parser = new ConfigurationArgsParser(args);
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Equal(IPAddress.Parse("172.16.0.0"), options.AddressStart);   
        Assert.Equal(4, options.AddressMask);
        Assert.Equal(new DateTime(2024, 1, 15), options.TimeStart);
        Assert.Equal(new DateTime(2024, 2, 29), options.TimeEnd);
        
        System.Environment.SetEnvironmentVariable("file-log", null);
        System.Environment.SetEnvironmentVariable("file-output", null);
        System.Environment.SetEnvironmentVariable("address-start", null);
        System.Environment.SetEnvironmentVariable("address-mask", null);
        System.Environment.SetEnvironmentVariable("time-start", null);
        System.Environment.SetEnvironmentVariable("time-end", null);
    }

    [Fact]
    public void ParseOptionsWithAllProviders()
    {
        System.Environment.SetEnvironmentVariable("time-start", "01.01.2024");
        System.Environment.SetEnvironmentVariable("time-end", "01.05.2024");
        
        var args = new string[]
        {
            "--file-log=input.log",
            "--file-output=output.log"
        };

        var parser = new ConfigurationArgsParser(args, "only_address.json");
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Equal(IPAddress.Parse("192.0.0.0"), options.AddressStart);   
        Assert.Equal(24, options.AddressMask);
        Assert.Equal(new DateTime(2024, 1, 1), options.TimeStart);
        Assert.Equal(new DateTime(2024, 5, 1), options.TimeEnd);

        System.Environment.SetEnvironmentVariable("time-start", null);
        System.Environment.SetEnvironmentVariable("time-end", null);
    }

    [Fact]
    public void ParseOptionsWithOnlyRequiredArgs()
    {
        var args = new string[]
        {
            "--file-log=input.log",
            "--file-output=output.log",
        };

        var parser = new ConfigurationArgsParser(args);
        var options = parser.Parse();

        Assert.Equal("input.log", options.FileLog);
        Assert.Equal("output.log", options.FileOutput);
        Assert.Null(options.AddressStart);   
        Assert.Null(options.AddressMask);
        Assert.Equal(DateTime.MinValue, options.TimeStart);
        Assert.Equal(DateTime.MaxValue, options.TimeEnd);
    }

    [Fact]
    public void ParseOptionsWithInvalidArgs()
    {
        var args = new string[6]
        {
            "",
            "",
            "",
            "",
            "",
            ""
        };

        var parser = new ConfigurationArgsParser(args);

        Assert.Throws<ValidationException>(parser.Parse);
        args[0] = "--file-log=input.log";
        
        Assert.Throws<ValidationException>(parser.Parse);
        args[1] = "--file-output=output.log";

        args[2] = "--address-start=999.333.222";
        Assert.Throws<FormatException>(parser.Parse);
        args[2] = "--address-start=255.255.255.255";

        args[3] = "--address-mask=89";
        Assert.Throws<ValidationException>(parser.Parse);
        args[3] = "--address-mask=32";

        args[4] = "--time-start=33.02.2015";
        Assert.Throws<FormatException>(parser.Parse);
        args[4] = "--time-start=10.02.2015";

        args[5] = "--time-end=01.04";
        Assert.Throws<FormatException>(parser.Parse);
    }
}
