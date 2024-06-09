
namespace Machines.AddMachine;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, BadRequest>, Mapper>
{
    public IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Post("/machines/add-machine");
        Description(d => d
            .Accepts<Request>("application/json")
            .Produces<Response>(200, "application/json")
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, BadRequest>> HandleAsync(Request req, CancellationToken ct)
    {
        var machineToCreate = Map.ToEntity(req);
        var createdMachine = await MachineService.AddAsync(machineToCreate);
        await MachineService.SaveChangesAsync();

        return TypedResults.Ok(Map.FromEntity(createdMachine!));
    }
}