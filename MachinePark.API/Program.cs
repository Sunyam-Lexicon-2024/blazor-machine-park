using MachinePark.API.Extensions;
using MachinePark.API.Filters;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseDefaultExceptionHandler()
           .UseFastEndpoints(config =>
           {
               config.Endpoints.RoutePrefix = "api";
               config.Endpoints.Configurator = ep =>
               {
                   ep.AllowAnonymous(); // no auth for now
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

    app.Run();
    Log.Information("Application exited cleanly.");
}
catch (Exception ex)
 when (ex is not HostAbortedException &&
    ex.Source is not "Microsoft.EntityFrameworkCore.Design")
{
    Log.Fatal("{Message}", new { Message = "Encountered error in boot stage", Error = ex });
}
finally
{
    Log.CloseAndFlush();
}
