using MachinePark.Data.Seeds;

namespace MachinePark.API.Extensions;

public static class WebAppExtensions
{

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFastEndpoints()
            .SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "Machine Park API";
                    s.Version = "v1";
                };
            })
            .AddSerilog();

        services.AddDbContext<MachineParkDbContext>(opt =>
             opt.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IMachineService, MachineService>();

        return services;
    }

    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = app.ApplicationServices.GetService<ILogger<Program>>() ?? throw new ArgumentException("Could not acquire Logger from Service Collection");
        var serviceProvider = scope.ServiceProvider;
        var machineParkDbContext = serviceProvider.GetRequiredService<MachineParkDbContext>();

        MachineSeeds machineSeeds = new(machineParkDbContext);

        await machineParkDbContext.Database.EnsureDeletedAsync();
        await machineParkDbContext.Database.MigrateAsync();

        try
        {
            await machineSeeds.InitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("{Message}", new { ex.Message, ex.StackTrace });
            throw;
        }
    }
}