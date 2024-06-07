namespace Machines.GetAllMachines;

public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<MachineModel>, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/api/machines/get-all-machines");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var machines = await MachineService.GetAllMachinesAsync();
        var models = machines.Select(m => Map.FromEntity(m));
        Response = models;
    }
}