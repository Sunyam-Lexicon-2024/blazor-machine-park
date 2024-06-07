namespace Machines.Stats.GetMachineStats;

public sealed class Endpoint : EndpointWithoutRequest<MachineStatsModel, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/api/machines/stats/get-machine-stats");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var machines = await MachineService.GetAllMachinesAsync();
        var model = Map.FromEntity(machines);
        Response = model;
    }
}