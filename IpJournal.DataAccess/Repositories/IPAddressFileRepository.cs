using System.Net;
using IpJournal.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace IpJournal.DataAccess;

public class IPAdressFileRepository : IIPAddressRepository
{
    private readonly IConfiguration _config;
    private string FileName
    {
        get
        {
            return _config.GetValue<string>("FileOutput") ?? string.Empty;
        }
    }

    public IPAdressFileRepository(IConfiguration config)
    {
        _config = config;
    }

    public void Save(List<IPAddress> ipAddresses)
    {
        if (ipAddresses.Count == 0) return;

        using StreamWriter sw = File.AppendText(FileName);

        sw.WriteLine($"{DateTimeOffset.Now}\n");

        foreach (IPAddress ip in ipAddresses)
        {
            sw.WriteLine(ip.ToString());
        }

        sw.WriteLine();
    }
}
