using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace IpLogReader;

public class IpParserConfiguration : IpParser
{
    private readonly string[] InputArgs;

    public IpParserConfiguration(string[] args)
    {
        InputArgs = args;
    }

    public IpParserOptions Parse()
    {
        var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: false)
                .AddEnvironmentVariables()
                .AddCommandLine(InputArgs)
                .Build();

        var options = new IpParserOptions()
        {
            FileLog = configuration["file-log"],
            FileOutput = configuration["file-output"],
        };

        var time_start = configuration["time-start"];
        if (time_start is not null)
        {
            options.TimeStart = DateTime.ParseExact(time_start, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        var time_end = configuration["time-end"];
        if (time_end is not null)
        {
            options.TimeEnd = DateTime.ParseExact(time_end, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        var address_start = configuration["address-start"];
        if (address_start is not null)
        {
            options.AddressStart = IPAddress.Parse(address_start);

            var mask = configuration["address-mask"];
            options.AddressMask = mask is null ? null : int.Parse(mask);

            // if (options.AddressMask is not null && (options.AddressMask < 1 || options.AddressMask > 32))
            // {
            //     throw new ArgumentException("Address mask should be between 1 and 32.");
            // }
        }

        Validator.ValidateObject(options, new ValidationContext(options), true);

        return options;
    }
}
