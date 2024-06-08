
using Microsoft.AspNetCore.Mvc;

namespace Machines.DeleteMachine;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound, BadRequest>>
{
    IMachineService MachineService { get; set; }
    public override void Configure()
    {
        Delete("/api/machines/{MachineId}");
        AllowAnonymous();
        Description(d => d
            .Accepts<Request>("application/json")
            .Produces<Response>(200, "application/json")
            .Produces(404)
            .ProducesProblemFE<InternalErrorResponse>(500),
               clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, NotFound, BadRequest>> ExecuteAsync([FromRoute] Request req, CancellationToken ct)
    {
        Log.Information("MACHINE ID: " + req.MachineId);

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
                return TypedResults.Ok<Response>(new() { MachineId = deletedMachine.Id });
            }
        }
        else
        {
            return TypedResults.NotFound();
        }
    }
}