using IpJournal.UI.Contracts;

namespace IpJournal.Core.Filter;

public interface IFilterOptionsFactory
{
    FilterOptions Create(FilterOptionsDTO from);
}
