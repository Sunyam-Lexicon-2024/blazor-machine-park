namespace MachinePark.API.Extensions;

public static class ServiceExtensions
{

    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFastEndpoints()
            .AddResponseCaching()
            .SwaggerDocument(options =>
            {
                options.DocumentSettings = document =>
                {
                    document.Title = "Machine Park API";
                    document.Version = "v1";
                };
            })
            .AddSerilog();

        services.AddDbContext<MachineParkDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<IMachineService, MachineService>();

        return services;
    }
}