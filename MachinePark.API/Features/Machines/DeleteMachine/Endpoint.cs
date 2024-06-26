
namespace Machines.DeleteMachine;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    public IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Delete("/machines/{MachineId}");
        Description(d => d
            .Produces<Response>(200, "application/json")
            .Produces(404)
            .ProducesProblemFE<InternalErrorResponse>(500),
               clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        bool machineExists = await MachineService.AnyAsync(m => m.Id == req.MachineId);

        if (machineExists)
        {
            Machine? deletedMachine = await MachineService.DeleteAsync(req.MachineId);
            if (deletedMachine is null)
            {
                return TypedResults.BadRequest();
            }
            else
            {
                new MachineDataUpdated
                {
                    Id = 2,
                    Description = $"Machine data updated with deleted machine [{deletedMachine.Id}]"
                }
                .Broadcast(ct: ct);

                await MachineService.SaveChangesAsync();
                return TypedResults.Ok<Response>(new() { MachineId = deletedMachine.Id });
            }
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}