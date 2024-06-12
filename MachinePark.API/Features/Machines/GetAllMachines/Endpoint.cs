namespace Machines.GetAllMachines;

public sealed class Endpoint : Endpoint<Request, Results<Ok<Response>,
                                           NoContent>, Mapper>
{

    public IMachineService MachineService { get; set; }

    public override void Configure()
    {
        Get("/machines/get-all-machines");
        Description(d => d
        .Produces<IEnumerable<MachineModel>>(200, "application/json+custom")
        .Produces(204)
        .ProducesProblemFE<InternalErrorResponse>(500), clearDefaults: true);
    }

    public override async Task<Results<Ok<Response>, NoContent>>
    ExecuteAsync(Request? req, CancellationToken ct)
    {
        IEnumerable<Machine> machines;

        if (req == null)
        {
            machines = await MachineService.GetAllMachinesAsync();
        }
        else
        {
            QueryParams queryParams = new()
            {
                SearchTerm = req.Search,
                Page = req.Page,
                PageSize = req.PageSize,
                SortProp = !string.IsNullOrWhiteSpace(req.SortBy) ? Enum.Parse<SortProp>(req.SortBy) : null,
                SortDirection = !string.IsNullOrWhiteSpace(req.SortDirection) ? Enum.Parse<SortDirection>(req.SortDirection) : null
            };

            machines = await MachineService.GetAllMachinesAsync(queryParams);
        }

        if (!machines.Any())
        {
            return TypedResults.NoContent();
        }

        else
        {
            Response response = new()
            {
                Machines = machines.Select(Map.FromEntity),
                SetSize = MachineService.GetDataSetCount(req.SetSize)
            };

            return TypedResults.Ok(response);
        }
    }
}