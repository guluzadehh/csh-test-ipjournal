using IpJournal.Core.Models;

namespace IpJournal.Core.Filter;

public class RequestLogsFilter : IRequestLogsFilter
{
    public List<FilterCb> _filterCbs = [];

    public RequestLogsFilter(params FilterCb[] filterCbs)
    {
        _filterCbs.AddRange(filterCbs);
    }

    public RequestLogsFilter()
    {
    }

    public IReadOnlyList<FilterCb> FilterCbs { get { return _filterCbs; } }

    public IEnumerable<RequestLog> Filter(IEnumerable<RequestLog> requestLogs, FilterOptions filterOptions)
    {
        ArgumentNullException.ThrowIfNull(requestLogs);
        ArgumentNullException.ThrowIfNull(filterOptions);

        return requestLogs.Where(requestLog => Passes(requestLog, filterOptions));
    }

    protected bool Passes(RequestLog requestLog, FilterOptions filterOptions)
    {
        foreach (var filterCb in _filterCbs)
        {
            if (!filterCb(requestLog, filterOptions))
            {
                return false;
            }
        }

        return true;
    }

    public void Register(FilterCb cb)
    {
        _filterCbs.Add(cb);
    }

    public void Register(IEnumerable<FilterCb> cbs)
    {
        _filterCbs.AddRange(cbs);
    }
}
