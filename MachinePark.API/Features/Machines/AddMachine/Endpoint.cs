
namespace Machines.AddMachine;

public class Endpoint : Endpoint<Request, Response, Mapper>
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

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        Log.Information(req.Name);
        var machineToCreate = Map.ToEntity(req);
        var createdMachine = await MachineService.AddAsync(machineToCreate);
        await MachineService.SaveChangesAsync();

        Response = Map.FromEntity(createdMachine!);
    }
}