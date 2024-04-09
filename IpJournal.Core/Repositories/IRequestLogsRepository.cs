using IpJournal.Core.Models;

namespace IpJournal.Core.Repositories;

public interface IRequestLogsRepository
{
    IEnumerable<RequestLog> GetAll();
}
