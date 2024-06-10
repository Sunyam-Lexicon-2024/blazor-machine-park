using MachinePark.UI;
using MachinePark.UI.Components;
using MachinePark.UI.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode();

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
