using System.Net;
using IpLogParser.Shared;
using Xunit;

namespace IpLogParser.Tests;

public class IpUtilsTests
{
    [Fact]
    public void MaskToAddressTest_WhereMaskIs24()
    {
        int mask = 24;

        var expected = IPAddress.Parse("255.255.255.0");
        var actual = IpUtils.MaskToAddress(mask);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MaskToAddressTest_WhereMaskIs1()
    {
        int mask = 1;

        var expected = IPAddress.Parse("128.0.0.0");
        var actual = IpUtils.MaskToAddress(mask);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MaskToAddressTest_WhereMaskIs32()
    {
        int mask = 32;

        var expected = IPAddress.Parse("255.255.255.255");
        var actual = IpUtils.MaskToAddress(mask);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void MaskToAddressTest_WhereMaskIs0()
    {
        int mask = 0;
        
        Assert.Throws<ArgumentException>(() => IpUtils.MaskToAddress(mask));
    }

    [Fact]
    public void MaskToAddressTest_WhereMaskIs33()
    {
        int mask = 33;

        Assert.Throws<ArgumentException>(() => IpUtils.MaskToAddress(mask));
    }
    
    [Fact]
    public void GetLastUsableAddress_WhereIpIsPublicTest()
    {
        var address = IPAddress.Parse("92.53.71.234");
        var address_mask = 24;

        var expected = IPAddress.Parse("92.53.71.255");
        var actual = IpUtils.GetLastUsableAddress(address, address_mask);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetLastUsableAddress_WhereIpIsPrivateTest()
    {
        var address = IPAddress.Parse("172.16.23.45");
        var address_mask = 12;

        var expected = IPAddress.Parse("172.31.255.255");
        var actual = IpUtils.GetLastUsableAddress(address, address_mask);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetAddressBounds_WhereIpIsPublicTest()
    {
        var address = IPAddress.Parse("92.53.71.234");
        var address_mask = 18;

        var lower_expected = address;
        var upper_expected = IPAddress.Parse("92.53.127.255");

        var actual = IpUtils.GetAddressBounds(address, address_mask);

        Assert.Equal(lower_expected, actual.Item1);
        Assert.Equal(upper_expected, actual.Item2);
    }

    [Fact]
    public void GetAddressBounds_WhereIpIsPublicWithoutMaskTest()
    {
        var address = IPAddress.Parse("92.53.71.234");

        var lower_expected = address;

        var actual = IpUtils.GetAddressBounds(address, null);

        Assert.Equal(lower_expected, actual.Item1);
        Assert.Null(actual.Item2);
    }

    [Fact]
    public void GetAddressBounds_WhereWithoutIpAndMaskTest()
    {
        var actual = IpUtils.GetAddressBounds(null, null);

        Assert.Null(actual.Item1);
        Assert.Null(actual.Item2);
    }

    [Fact]
    public void AddressMatch_WhereWithoutRangeTest()
    {
        var address = IPAddress.Parse("128.0.0.1");
        var actual = IpUtils.AddressMatch(address, null, null);

        Assert.True(actual);
    }

    [Fact]
    public void AddressMatch_WithLowerBound_ExpectTrue()
    {
        var address = IPAddress.Parse("128.0.0.1");
        var lower_bound = IPAddress.Parse("1.0.0.1");

        var actual = IpUtils.AddressMatch(address, lower_bound, null);

        Assert.True(actual);
    }

    [Fact]
    public void AddressMatch_WithLowerBound_ExpectFalse()
    {
        var address = IPAddress.Parse("128.0.0.1");
        var lower_bound = IPAddress.Parse("192.0.0.1");

        var actual = IpUtils.AddressMatch(address, lower_bound, null);

        Assert.False(actual);
    }

    [Fact]
    public void AddressMatch_WithBounds_ExpectTrue()
    {
        var address = IPAddress.Parse("142.150.184.143");
        var lower_bound = IPAddress.Parse("100.100.100.100");
        var upper_bound = IPAddress.Parse("200.200.200.200");

        var actual = IpUtils.AddressMatch(address, lower_bound, upper_bound);

        Assert.True(actual);
    }

    [Fact]
    public void AddressMatch_WithBounds_ExpectFalse()
    {
        var address = IPAddress.Parse("152.186.205.124");
        var lower_bound = IPAddress.Parse("100.100.100.100");
        var upper_bound = IPAddress.Parse("200.200.200.200");

        var actual = IpUtils.AddressMatch(address, lower_bound, upper_bound);

        Assert.False(actual);
    }

    [Fact]
    public void AddressMatch_WithUpperBound_ExpectTrue()
    {
        var address = IPAddress.Parse("1.1.1.1");
        var upper_bound = IPAddress.Parse("200.200.200.200");

        var actual = IpUtils.AddressMatch(address, null, upper_bound);

        Assert.True(actual);
    }

    [Fact]
    public void AddressMatch_WithUpperBound_ExpectFalse()
    {
        var address = IPAddress.Parse("130.248.212.205");
        var upper_bound = IPAddress.Parse("200.200.200.200");

        var actual = IpUtils.AddressMatch(address, null, upper_bound);

        Assert.False(actual);
    }
}
