
namespace Machines.UpdateMachine;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>, Mapper>
{
    public IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Put("/machines/update-machine");
        Description(d => d
            .Accepts<Request>("application/json")
            .Produces<Response>(200, "application/json")
            .Produces<Response>(400, "application/json+problem")
            .Produces(404)
            .ProducesProblemFE<InternalErrorResponse>(500),
        clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
    {
        bool machineExists = await MachineService.AnyAsync(m => m.Id == req.Id);
        if (!machineExists)
        {
            return TypedResults.NotFound();
        }
        else
        {
            var machineToUpdate = Map.ToEntity(req);
            var updatedMachine = await MachineService.UpdateAsync(machineToUpdate);
            await MachineService.SaveChangesAsync();

            new MachineDataUpdated
            {
                Id = 3,
                Description = $"Machine data updated with updated machine [{updatedMachine.Id}]"
            }
            .Broadcast(ct: ct);

            return TypedResults.Ok(Map.FromEntity(updatedMachine!));
        }
    }
}