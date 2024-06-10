using Serilog;
using MudBlazor.Services;

namespace MachinePark.UI.Extensions;

public static class WebAppExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMudServices();

        services.AddRazorComponents()
                .AddInteractiveServerComponents();

        services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(configuration["API:BaseURI"]),
                Timeout = TimeSpan.FromSeconds(30)
            });

        services.AddSerilog((services, loggerConfiguration) =>
            loggerConfiguration.ReadFrom.Configuration(configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());

        return services;
    }
}