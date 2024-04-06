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
}
