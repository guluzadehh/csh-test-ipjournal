using System.Net;
using IpJournal.Core;

namespace IpJournal.Tests;
public class IpExtensionsTests
{
    [Fact]
    public void Compare_BothAddressesNull_ReturnsZero()
    {
        int result = Extensions.Compare(null, null);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_FirstAddressNull_ReturnsNegativeOne()
    {
        var ipAddress2 = IPAddress.Parse("192.168.1.1");

        int result = Extensions.Compare(null, ipAddress2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Compare_SecondAddressNull_ReturnsOne()
    {
        var ipAddress1 = IPAddress.Parse("192.168.1.1");

        int result = Extensions.Compare(ipAddress1, null);

        Assert.Equal(1, result);
    }

    [Fact]
    public void Compare_SameAddresses_ReturnsZero()
    {
        var ipAddress1 = IPAddress.Parse("192.168.1.1");
        var ipAddress2 = IPAddress.Parse("192.168.1.1");

        int result = ipAddress1.Compare(ipAddress2);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Compare_FirstAddressBeforeSecond_ReturnsNegativeOne()
    {
        var ipAddress1 = IPAddress.Parse("192.168.1.1");
        var ipAddress2 = IPAddress.Parse("192.168.1.2");

        int result = ipAddress1.Compare(ipAddress2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Compare_FirstAddressAfterSecond_ReturnsOne()
    {
        var ipAddress1 = IPAddress.Parse("192.168.1.2");
        var ipAddress2 = IPAddress.Parse("192.168.1.1");

        int result = ipAddress1.Compare(ipAddress2);

        Assert.Equal(1, result);
    }

    [Fact]
    public void IsInSubnet_IPAddressInSubnet_ReturnsTrue()
    {
        var ipAddress = IPAddress.Parse("192.168.1.1");
        var subnetAddress = IPAddress.Parse("192.168.1.0");
        int subnetMaskLength = 24;

        bool result = ipAddress.IsInSubnet(subnetAddress, subnetMaskLength);

        Assert.True(result);
    }

    [Fact]
    public void IsInSubnet_IPAddressNotInSubnet_ReturnsFalse()
    {
        var ipAddress = IPAddress.Parse("192.168.1.1");
        var subnetAddress = IPAddress.Parse("10.0.0.0");
        int subnetMaskLength = 8;

        bool result = ipAddress.IsInSubnet(subnetAddress, subnetMaskLength);

        Assert.False(result);
    }

    [Fact]
    public void IsInSubnet_IPAddressInSubnetWithMaskLessThan8_ReturnsTrue()
    {
        var ipAddress = IPAddress.Parse("192.168.1.1");
        var subnetAddress = IPAddress.Parse("192.168.1.0");
        int subnetMaskLength = 24;

        bool result = ipAddress.IsInSubnet(subnetAddress, subnetMaskLength);

        Assert.True(result);
    }
}