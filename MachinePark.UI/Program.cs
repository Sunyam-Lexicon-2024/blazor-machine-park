
using MachinePark.UI.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterServices(builder.Configuration);

    var app = builder.Build();

    app.ConfigureApplication();
  
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
