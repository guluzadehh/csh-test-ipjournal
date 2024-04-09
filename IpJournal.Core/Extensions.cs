using System.Net;

namespace IpJournal.Core;

public static class Extensions
{
    public static uint ToInteger(this IPAddress ipAddress)
    {
        byte[] bytes = ipAddress.GetAddressBytes();

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        return BitConverter.ToUInt32(bytes, 0);
    }

    public static uint Compare(this IPAddress ipAddress1, IPAddress ipAddress2)
    {
        uint ip1 = ipAddress1.ToInteger();
        uint ip2 = ipAddress2.ToInteger();
        return ((ip1 - ip2) >> 0x1F) | (uint)(-(ip1 - ip2)) >> 0x1F;
    }

    public static bool IsInSubnet(this IPAddress ipAddress, IPAddress subnetAddress, int subnetMaskLength)
    {
        byte[] ipAddressBytes = ipAddress.GetAddressBytes();
        byte[] subnetAddressBytes = subnetAddress.GetAddressBytes();

        byte[] subnetMaskBytes = new byte[ipAddressBytes.Length];
        for (int i = 0; i < subnetMaskLength / 8; i++)
        {
            subnetMaskBytes[i] = 0xFF; // 255 in decimal
        }
        if (subnetMaskLength % 8 != 0)
        {
            subnetMaskBytes[subnetMaskLength / 8] = (byte)(0xFF << (8 - subnetMaskLength % 8));
        }

        // Check if the IP address and subnet IP address share the same subnet
        for (int i = 0; i < ipAddressBytes.Length; i++)
        {
            if ((ipAddressBytes[i] & subnetMaskBytes[i]) != (subnetAddressBytes[i] & subnetMaskBytes[i]))
            {
                return false;
            }
        }

        return true;
    }
}
