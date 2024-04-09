using System.Net;

namespace IpJournal.Core.Filter;

public sealed record FilterOptions(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    IPAddress? StartIpAddress = null,
    int? Mask = null
);
