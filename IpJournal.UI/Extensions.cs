using IpJournal.Core;
using IpJournal.Core.Filter;
using IpJournal.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace IpJournal.UI;

public static class Extensions
{
    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        services.AddSingleton<IFilterOptionsFactory, FilterOptionsFactory>();
        services.AddSingleton<ConsoleApp>();

        return services;
    }

    public static HostApplicationBuilder BuildAppServices(this HostApplicationBuilder builder)
    {
        builder.Services.AddSerilog(
            new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger()
        );
        builder.Services.AddDatatAccess();
        builder.Services.AddCore();
        builder.Services.AddUI();

        return builder;
    }

    public static HostApplicationBuilder ConfigureCommandLine(this HostApplicationBuilder builder, string[] args)
    {
        builder.Configuration.AddCommandLine(args, new Dictionary<string, string>
        {
            { "--file-log", "FileLog" },
            { "--file-output", "FileOutput" },
            { "--address-start", "AddressStart" },
            { "--address-mask", "AddressMask" },
            {"--time-start", "TimeStart"},
            {"--time-end", "TimeEnd"}
        });
        return builder;
    }
}
