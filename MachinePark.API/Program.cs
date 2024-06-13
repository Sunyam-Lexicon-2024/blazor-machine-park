using Microsoft.AspNetCore.Server.Kestrel.Core;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(k =>
    {
        k.ListenLocalhost(6000, c => c.Protocols = HttpProtocols.Http2);
        k.ListenLocalhost(5000, c => c.Protocols = HttpProtocols.Http1AndHttp2);
    });

    builder.AddHandlerServer();

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    app.MapHandlers(h =>
    {
        h.RegisterEventHub<MachineDataUpdated>();
    });

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
