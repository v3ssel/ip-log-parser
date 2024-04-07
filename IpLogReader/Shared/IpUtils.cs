using System.Net;

namespace IpLogParser.Shared;

public static class IpUtils
{
    public static IPAddress MaskToAddress(int mask)
    {
        return new IPAddress(BitConverter.GetBytes(uint.MaxValue << (32 - mask)).Reverse().ToArray());
    }

    public static IPAddress GetLastUsableAddress(IPAddress address, IPAddress mask)
    {
        var address_long = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray());
        var mask_long = BitConverter.ToUInt32(mask.GetAddressBytes().Reverse().ToArray());

        uint network = address_long & mask_long;
        uint last_usable = network | ~mask_long;
        
        return new IPAddress(BitConverter.GetBytes(last_usable).Reverse().ToArray());
    }

    public static (IPAddress?, IPAddress?) GetAddressBounds(IPAddress? address_start, int? mask)
    {
        IPAddress? lower_address_bytes = null;
        IPAddress? upper_address_bytes = null;

        if (address_start is not null)
        {
            lower_address_bytes = address_start;

            if (mask is not null)
            {
                var mask_address = MaskToAddress((int)mask);
                upper_address_bytes = GetLastUsableAddress(address_start, mask_address);
            }
        }

        return (lower_address_bytes, upper_address_bytes);
    }

    public static bool AddressMatch(IPAddress address, IPAddress? lower_bound, IPAddress? upper_bound)
    {
        if (lower_bound is null && upper_bound is null)
            return true;

        var address_bytes = address.GetAddressBytes();
        var lower_bound_bytes = lower_bound?.GetAddressBytes();
        var upper_bound_bytes = upper_bound?.GetAddressBytes();

        for (var i = 0; i < address_bytes.Length; i++)
        {
            if ((lower_bound_bytes is not null && address_bytes[i] < lower_bound_bytes[i]) ||
                (upper_bound_bytes is not null && address_bytes[i] > upper_bound_bytes[i]))
            {
                return false;
            }
        }
      
        return true;
    }
}
