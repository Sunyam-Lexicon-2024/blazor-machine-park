using MachinePark.API.Extensions;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    await app.ConfigureWebApplication();

    await app.RunAsync();

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
