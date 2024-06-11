
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
            .Produces<Response>(400, "application/problem+json")
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        bool machineExists = await MachineService.AnyAsync(m => m.Name == req.Name);

        if (machineExists)
        {
            AddError(r => r.Name, "A machine with the given name already exists");
        }

        ThrowIfAnyErrors();

        var machineToCreate = Map.ToEntity(req);

        var createdMachine = await MachineService.AddAsync(machineToCreate);
        await MachineService.SaveChangesAsync();

        Response response = Map.FromEntity(createdMachine!);
        return TypedResults.Ok(response);
    }
}