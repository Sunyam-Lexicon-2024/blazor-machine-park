namespace Machines.GetAllMachines;

public sealed class Endpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<MachineModel>>,
                                           NoContent>, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/machines/get-all-machines");
        Description(d => d
        .Produces<IEnumerable<MachineModel>>(200, "application/json+custom")
        .Produces(204)
        .ProducesProblemFE<InternalErrorResponse>(500),
    clearDefaults: true);
    }

    public override async Task<Results<Ok<IEnumerable<MachineModel>>, NoContent>>
    ExecuteAsync(CancellationToken ct)
    {
        var machines = await MachineService.GetAllMachinesAsync();
        var models = machines.Select(Map.FromEntity);

        if (!models.Any())
        {
            return TypedResults.NoContent();
        }
        else
        {
            return TypedResults.Ok(models);
        }
    }
}