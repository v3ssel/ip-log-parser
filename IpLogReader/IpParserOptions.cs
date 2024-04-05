using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace IpLogReader;

public class IpParserOptions
{
    [Required]
    public string? FileLog { get; set; }
    
    [Required]
    public string? FileOutput { get; set; }
    
    public IPAddress? AddressStart { get; set; }
    
    [Range(1, 32)]
    public int? AddressMask { get; set; }
    
    public DateTime TimeStart { get; set; } = DateTime.MinValue;
    
    public DateTime TimeEnd { get; set; }  = DateTime.MaxValue;
}
