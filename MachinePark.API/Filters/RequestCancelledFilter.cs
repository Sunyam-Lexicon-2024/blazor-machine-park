namespace MachinePark.API.Filters;

sealed class OperationCancelledFilter(ILogger<OperationCancelledFilter> logger) : IEndpointFilter
{
    private readonly ILogger<OperationCancelledFilter> _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Request was cancelled");
            return Results.StatusCode(499);
        }
    }
}