using System.Net;

namespace IpJournal.Core.Repositories;

public interface IIPAddressRepository
{
    void Save(List<IPAddress> ipAddresses);
}
