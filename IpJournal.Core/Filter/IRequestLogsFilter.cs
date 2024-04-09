using IpJournal.Core.Models;

namespace IpJournal.Core.Filter;

public delegate bool FilterCb(RequestLog requestLog, FilterOptions options);

public interface IRequestLogsFilter
{
    IEnumerable<RequestLog> Filter(IEnumerable<RequestLog> requestLogs, FilterOptions options);
    void Register(FilterCb cb);
    void Register(IEnumerable<FilterCb> cbs);
}
