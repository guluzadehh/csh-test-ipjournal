using IpJournal.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


var host = Host.CreateApplicationBuilder(args)
    .BuildAppServices()
    .ConfigureCommandLine(args)
    .Build();

var app = host.Services.GetRequiredService<ConsoleApp>();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start the application");
}
finally
{
    Log.CloseAndFlush();
}