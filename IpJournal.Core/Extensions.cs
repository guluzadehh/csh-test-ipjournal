using System.Net;

namespace IpJournal.Core;

public static class Extensions
{
    public static int Compare(this IPAddress ipAddress1, IPAddress ipAddress2)
    {
        if (ipAddress1 == null && ipAddress2 == null)
            return 0;

        if (ipAddress1 == null)
            return -1;

        if (ipAddress2 == null)
            return 1;

        byte[] addressBytes1 = ipAddress1.GetAddressBytes();
        byte[] addressBytes2 = ipAddress2.GetAddressBytes();

        for (int i = 0; i < addressBytes1.Length; i++)
        {
            if (addressBytes1[i] < addressBytes2[i])
                return -1;

            else if (addressBytes1[i] > addressBytes2[i])
                return 1;
        }

        return 0;
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
