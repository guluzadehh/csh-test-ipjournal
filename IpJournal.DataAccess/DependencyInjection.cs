using IpJournal.Core;
using IpJournal.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IpJournal.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDatatAccess(this IServiceCollection services)
    {
        services.AddSingleton<IRequestLogsRepository, RequestLogsFileRepository>();
        services.AddSingleton<IIPAddressRepository, IPAddressFileRepository>();

        return services;
    }
}
