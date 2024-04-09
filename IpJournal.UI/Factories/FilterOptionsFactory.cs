using System.Net;
using IpJournal.UI.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IpJournal.Core.Filter;

public class FilterOptionsFactory : IFilterOptionsFactory
{
    private readonly IConfiguration _config;
    private readonly ILogger<FilterOptionsFactory> _logger;

    public FilterOptionsFactory(IConfiguration config, ILogger<FilterOptionsFactory> logger)
    {
        _config = config;
        _logger = logger;
    }

    private string TimeFormat
    {
        get
        {
            return _config.GetValue<string>("ParameterDateFormat") ?? "dd.MM.yyyy";
        }
    }

    public FilterOptions Create(FilterOptionsDTO from)
    {
        DateTimeOffset timeStart = ParseTime(from.TimeStart);
        DateTimeOffset timeEnd = ParseTime(from.TimeEnd);

        IPAddress? addressStart = ParseIPAddress(from.AddressStart);
        int? addressMask = addressStart is not null ? ParseMask(from.AddressMask) : null;

        return new FilterOptions(
            timeStart,
            timeEnd,
            addressStart,
            addressMask
        );
    }

    protected int? ParseMask(int? mask)
    {
        if (!mask.HasValue) return null;

        if (mask < 0 || mask > 32)
        {
            _logger.LogWarning("Mask value must be between 0 and 32.");
            return null;
        }

        return mask;
    }

    protected DateTimeOffset ParseTime(string time)
    {
        try
        {
            return DateTimeOffset.ParseExact(time, TimeFormat, null);
        }
        catch
        {
            throw new Exception($"Error while parsing time filter option `{time}` with format `{TimeFormat}`\n");
        }
    }

    protected IPAddress? ParseIPAddress(string? ipAddress)
    {
        if (ipAddress is null) return null;

        try
        {
            return IPAddress.Parse(ipAddress);
        }
        catch (Exception exc)
        {
            _logger.LogWarning($"Error while parsing ip address filter option: {ipAddress}\n", exc);
        }

        return null;
    }
}
