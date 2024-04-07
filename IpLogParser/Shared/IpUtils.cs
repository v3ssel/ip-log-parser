using System.Net;

namespace IpLogParser.Shared;

public static class IpUtils
{
    public static IPAddress MaskToAddress(int mask)
    {
        if (mask < 1 || mask > 32)
            throw new ArgumentException("Address mask must be between 1 and 32");

        return new IPAddress(BitConverter.GetBytes(uint.MaxValue << (32 - mask)).Reverse().ToArray());
    }

    public static IPAddress GetLastUsableAddress(IPAddress address, int mask)
    {
        var address_long = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray());
        var mask_long = BitConverter.ToUInt32(MaskToAddress(mask).GetAddressBytes().Reverse().ToArray());

        uint network = address_long & mask_long;
        uint last_usable = network | ~mask_long;
        
        return new IPAddress(BitConverter.GetBytes(last_usable).Reverse().ToArray());
    }

    public static (IPAddress?, IPAddress?) GetAddressBounds(IPAddress? address_start, int? mask)
    {
        if (address_start is null)
            return (null, null);

        var lower_address_bytes = address_start;

        if (mask is not null)
        {
            var upper_address_bytes = GetLastUsableAddress(address_start, (int)mask);
            return (lower_address_bytes, upper_address_bytes);
        }

        return (lower_address_bytes, null);
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
