namespace Machines.GetAllMachines.Paginated;

public class Endpoint : Endpoint<PaginatedRequest,
    Results<Ok<IEnumerable<MachineModel>>, NoContent>, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/machines/get-all-machines?Page={Page}&PageSize={PageSize}");
        ResponseCache(60);
        Options(r => r.CacheOutput(o => o.Expire(TimeSpan.FromSeconds(60))));
        Description(d => d
        .Produces<IEnumerable<MachineModel>>(200, "application/json+custom")
        .Produces(204)
        .ProducesProblemFE<InternalErrorResponse>(500), clearDefaults: true);
    }

    public override async Task<Results<Ok<IEnumerable<MachineModel>>, NoContent>>
        ExecuteAsync(PaginatedRequest req, CancellationToken ct)
    {
        var machines = await MachineService.GetAllMachinesAsync(new QueryParams() { Page = req.Page, PageSize = req.PageSize });
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