using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IpJournal.UI.Contracts;
using IpJournal.Core;
using IpJournal.Core.Filter;

namespace IpJournal.UI;

public class ConsoleApp
{
    private readonly IConfiguration _config;
    private readonly ILogger<ConsoleApp> _logger;
    private readonly IFilterOptionsFactory _filterOptionsFactory;
    private readonly IpJournalApp _app;

    public ConsoleApp(
        IConfiguration config,
        ILogger<ConsoleApp> logger,
        IFilterOptionsFactory filterOptionsFactory,
        IpJournalApp app)
    {
        _config = config;
        _logger = logger;
        _filterOptionsFactory = filterOptionsFactory;
        _app = app;
    }

    public void Run()
    {
        try
        {
            _app.Run(_filterOptionsFactory.Create(CreateFilterOptionsDTO()));
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, $"{exc.Message}\n");
        }
    }

    protected FilterOptionsDTO CreateFilterOptionsDTO()
    {
        string? addressStart = _config.GetValue<string>("AddressStart");
        int? addressMask = _config.GetValue<int?>("AddressMask");

        if (addressStart is null && addressMask is not null)
        {
            _logger.LogWarning("You can't use address mask filter option without address start.");
            addressMask = null;
        }

        string? timeStart = _config.GetValue<string>("TimeStart");
        string? timeEnd = _config.GetValue<string>("TimeEnd");

        if (timeStart is null || timeEnd is null)
        {
            throw new NullReferenceException("Both time start and time end arguments must be set.");
        }

        return new FilterOptionsDTO(
            timeStart,
            timeEnd,
            addressStart,
            addressMask
        );
    }
}
