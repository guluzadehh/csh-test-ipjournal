using IpJournal.Core.Filter;
using Microsoft.Extensions.DependencyInjection;

namespace IpJournal.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IRequestLogsFilter, RequestLogsFilter>();
        services.AddSingleton<IpJournalApp>();

        return services;
    }
}
