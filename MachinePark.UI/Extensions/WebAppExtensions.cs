using Serilog;
using MudBlazor;
using MudBlazor.Services;

namespace MachinePark.UI.Extensions;

public static class WebAppExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMudServices(config => {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        services.AddRazorComponents()
                .AddInteractiveServerComponents();

        services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(configuration["API:BaseURI"]!)
                ?? throw new ArgumentException("Could not get API URI from configuration"),
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