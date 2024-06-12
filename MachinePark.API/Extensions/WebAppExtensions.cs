using MachinePark.API.Filters;
using MachinePark.Data.Seeds;

namespace MachinePark.API.Extensions;

public static class WebAppExtensions
{

    public static async Task<WebApplication> ConfigureWebApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage()
               .UseDefaultExceptionHandler()
               .UseResponseCaching()
               .UseFastEndpoints(config =>
               {
                   config.Endpoints.RoutePrefix = "api";
                   config.Endpoints.Configurator = ep =>
                   {
                       ep.AllowAnonymous(); // no auth for now
                       ep.Options(o => o.AddEndpointFilter<OperationCancelledFilter>());
                   };
               })
               .UseSwaggerGen();

            if (Environment.GetEnvironmentVariable("SEED_DATA") == "1")
            {
                await app.SeedDataAsync();
            }
        }
        else
        {
            app.UseDefaultExceptionHandler()
               .UseResponseCaching()
               .UseFastEndpoints(config => config.Endpoints.Configurator = ep =>
               {
                   ep.Options(o => o.AddEndpointFilter<OperationCancelledFilter>());
               });
        }
        return app;
    }

    private static async Task SeedDataAsync(this IApplicationBuilder app)
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