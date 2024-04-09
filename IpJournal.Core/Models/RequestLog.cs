using System.Net;

namespace IpJournal.Core.Models;

public sealed class RequestLog
{
    public RequestLog(IPAddress ipAddress, DateTimeOffset date)
    {
        IpAddress = ipAddress;
        Date = date;
    }

    public IPAddress IpAddress { get; }
    public DateTimeOffset Date { get; }
}