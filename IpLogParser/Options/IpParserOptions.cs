using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IpLogParser.Options;

public class IpLogParserOptions
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "File log are required parameter.")]
    public string? FileLog { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Output file path are required parameter.")]
    public string? FileOutput { get; set; }
    
    public IPAddress? AddressStart { get; set; }
    
    [Range(1, 32, ErrorMessage = "Address mask should be between 1 and 32.")]
    public int? AddressMask { get; set; }
    
    public DateTime TimeStart { get; set; } = DateTime.MinValue;
    
    public DateTime TimeEnd { get; set; }  = DateTime.MaxValue;
}
