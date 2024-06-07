
namespace Machines.AddMachine;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Post("/api/machines/add-machine");
        AllowAnonymous();
        Description(d => d
            .Accepts<Request>("application/json+custom")
            .Produces<Response>(200, "application/json+custom")
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var machineToCreate = Map.ToEntity(req);
        var createdMachine = await MachineService.AddAsync(machineToCreate);
        await MachineService.SaveChangesAsync();

        Response = Map.FromEntity(createdMachine!);
    }
}