using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace IpLogParser.Options;

public class ArgsParserConfiguration : IArgsParser
{
    private readonly string[] InputArgs;
    private const string DatesFormat = "dd.MM.yyyy";
    
    public string ConfigPath { get; set; }

    public ArgsParserConfiguration(string[] args, string config_path = "config.json")
    {
        InputArgs = args;
        ConfigPath = config_path;
    }

    public IpLogParserOptions Parse()
    {
        var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(ConfigPath, optional: true)
                .AddCommandLine(InputArgs)
                .Build();

        var options = new IpLogParserOptions()
        {
            FileLog = configuration["file-log"],
            FileOutput = configuration["file-output"],
        };

        var time_start = configuration["time-start"];
        if (time_start is not null)
        {
            options.TimeStart = DateTime.ParseExact(time_start, DatesFormat, CultureInfo.InvariantCulture);
        }

        var time_end = configuration["time-end"];
        if (time_end is not null)
        {
            options.TimeEnd = DateTime.ParseExact(time_end, DatesFormat, CultureInfo.InvariantCulture);
        }

        var address_start = configuration["address-start"];
        if (address_start is not null)
        {
            options.AddressStart = IPAddress.Parse(address_start);

            var mask = configuration["address-mask"];
            options.AddressMask = mask is null ? null : int.Parse(mask);
        }

        Validator.ValidateObject(options, new ValidationContext(options), true);

        return options;
    }
}
