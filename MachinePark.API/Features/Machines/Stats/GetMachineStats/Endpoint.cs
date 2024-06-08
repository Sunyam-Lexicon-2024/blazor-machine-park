namespace Machines.Stats.GetMachineStats;

public sealed class Endpoint : EndpointWithoutRequest<Results<Ok<MachineStatsModel>, NoContent>, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/machines/stats/get-machine-stats");
        Description(d => d
            .Produces<IEnumerable<MachineModel>>(200, "application/json+custom")
            .Produces(204)
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
    }

    public override async Task<Results<Ok<MachineStatsModel>, NoContent>>
    ExecuteAsync(CancellationToken ct)
    {
        var machines = await MachineService.GetAllMachinesAsync();
        if (!machines.Any())
        {
            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.Ok(Map.FromEntity(machines));
        }
    }
}