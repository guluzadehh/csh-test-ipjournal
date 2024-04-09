using System.Net;
using IpJournal.Core.Filter;
using IpJournal.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IpJournal.Core;

public class IpJournalApp
{
    private readonly ILogger<IpJournalApp> _logger;
    private readonly IRequestLogsRepository _requestLogsRepository;
    private readonly IIPAddressRepository _ipAddressRepository;
    private readonly IRequestLogsFilter _filter;

    public IpJournalApp(
        ILogger<IpJournalApp> logger,
        IRequestLogsRepository requestLogsRepository,
        IIPAddressRepository ipAddressRepository,
        IRequestLogsFilter filter)
    {
        _logger = logger;
        _requestLogsRepository = requestLogsRepository;
        _ipAddressRepository = ipAddressRepository;
        _filter = filter;
        _filter.Register([
            (requestLog, options) => requestLog.Date.CompareTo(options.StartDate) > 0 && requestLog.Date.CompareTo(options.EndDate) < 0,
            (requestLog, options) => {
                bool output = true;

                if (options.StartIpAddress is null) return output;

                output = requestLog.IpAddress.Compare(options.StartIpAddress) >= 0;
                if (!options.Mask.HasValue) return output;

                return output && requestLog.IpAddress.IsInSubnet(options.StartIpAddress, options.Mask.Value);
            }
        ]);
    }

    public void Run(FilterOptions filterOptions)
    {
        var requestLogs = _requestLogsRepository.GetAll().ToList();

        var ipAddresses = _filter.Filter(requestLogs, filterOptions)
            .Select(requestLog => requestLog.IpAddress)
            .ToList();

        _ipAddressRepository.Save(ipAddresses);
    }
}
