using System.Net;
using IpJournal.Core.Models;
using IpJournal.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IpJournal.DataAccess;

public class RequestLogsFileRepository : IRequestLogsRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<RequestLogsFileRepository> _logger;

    public RequestLogsFileRepository(IConfiguration config, ILogger<RequestLogsFileRepository> logger)
    {
        _config = config;
        _logger = logger;
    }

    private string FileName
    {
        get
        {
            return _config.GetValue<string>("FileLog") ?? string.Empty;
        }
    }
    private string Delimiter
    {
        get
        {
            return _config.GetValue<string>("Delimiter") ?? ":";
        }
    }
    private int IpAddressPos
    {
        get
        {
            return _config.GetValue<int>("IpAddrPos");
        }
    }
    private int DatePos
    {
        get
        {
            return _config.GetValue<int>("DatePos");
        }
    }
    private string DateFormat
    {
        get
        {
            return _config.GetValue<string>("DateFormat") ?? "yyyy-MM-dd HH:mm:ss";
        }
    }

    public IEnumerable<RequestLog> GetAll()
    {
        using StreamReader sr = new(FileName);

        string? line;

        while ((line = sr.ReadLine()) != null)
        {
            RequestLog requestLog;

            try
            {
                requestLog = Parse(line);
            }
            catch (Exception exc)
            {
                _logger.LogWarning($"Error parsing line '{line}': {exc.Message}\n");
                continue;
            }

            yield return requestLog;
        }
    }

    protected RequestLog Parse(string line)
    {
        string[] split = line.Split(Delimiter, 2);

        IPAddress ipAddress = IPAddress.Parse(split[IpAddressPos]);
        DateTimeOffset date = DateTimeOffset.ParseExact(split[DatePos], DateFormat, null);

        return new RequestLog(ipAddress, date);
    }
}
