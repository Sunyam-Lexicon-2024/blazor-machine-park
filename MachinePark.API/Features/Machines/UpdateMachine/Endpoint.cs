
namespace Machines.UpdateMachine;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>, Mapper>
{
    public IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Post("/machines/update-machine");
        Description(d => d
            .Accepts<Request>("application/json")
            .Produces<Response>(200, "application/json")
            .Produces(404)
            .Produces(400)
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
        try
        {
            var machineToUpdate = Map.ToEntity(req);
            var updatedMachine = await MachineService.UpdateAsync(machineToUpdate);
            await MachineService.SaveChangesAsync();

            return TypedResults.Ok(Map.FromEntity(updatedMachine!));
        }
        catch (Exception ex)
        {
            Log.Error("{Message}", new { Message = "Internal error while updating machine model", Error = ex });
            return TypedResults.BadRequest();
        }
    }
}