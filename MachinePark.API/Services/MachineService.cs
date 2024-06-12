using System.Linq.Expressions;

namespace MachinePark.API.Services;

public interface IQueryParams
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int? SetSize { get; set; }
    public SortProp? SortProp { get; set; }
    public SortDirection? SortDirection { get; set; }
}

public class QueryParams : IQueryParams
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int? SetSize { get; set; }
    public SortProp? SortProp { get; set; }
    public SortDirection? SortDirection { get; set; }
}

public enum SortProp
{
    Id,
    Name,
    Online,
    Section,
    CreatedAt,
    UpdatedAt,
}

public enum SortDirection {
    Ascending,
    Descending
}

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<IEnumerable<Machine>> GetAllMachinesAsync(QueryParams queryParams);
    Task<Machine?> GetByIdAsync(int machineId);
    Task<Machine?> AddAsync(Machine machine);
    Task<Machine?> UpdateAsync(Machine machine);
    Task<Machine?> DeleteAsync(int machineId);
    int GetDataSetCount(int? dataSetSizeLimit);

    Task<bool> AnyAsync(Expression<Func<Machine, bool>> expression);
    Task SaveChangesAsync();
}

sealed class MachineService(MachineParkDbContext machineParkDbContext) : IMachineService
{
    private readonly MachineParkDbContext _machineParkDbContext = machineParkDbContext;

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _machineParkDbContext.Machines.ToListAsync();
    }

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync(QueryParams queryParams)
    {
        var machines = _machineParkDbContext.Machines.AsQueryable();

        if (queryParams.SortProp is not null)
        {
            machines = queryParams.SortDirection == SortDirection.Descending ? 
                machines.OrderByDescending(m => EF.Property<string>(m, queryParams.SortProp.ToString()!)) 
                :
                machines.OrderBy(m => EF.Property<string>(m, queryParams.SortProp.ToString()!));


        }

        if (queryParams.Page is not null && queryParams.PageSize is not null)
        {
            machines = machines
                        .Skip((queryParams.Page * queryParams.PageSize) ?? 0)
                        .Take(queryParams.SetSize ?? machines.Count());
        }

        return await machines.ToListAsync();
    }

    public async Task<Machine?> GetByIdAsync(int machineId)
    {
        return await _machineParkDbContext.Machines.FirstOrDefaultAsync(m => m.Id == machineId);
    }

    public async Task<Machine?> AddAsync(Machine machine)
    {
        var createdMachine = (await _machineParkDbContext.Machines.AddAsync(machine)).Entity;
        return createdMachine;
    }
    public async Task<Machine?> UpdateAsync(Machine machine)
    {
        var updatedMachine = await Task.Run(() => _machineParkDbContext.Machines.Update(machine).Entity);
        return updatedMachine;
    }

    public async Task<Machine?> DeleteAsync(int machineId)
    {
        var machineToDelete = await GetByIdAsync(machineId);
        if (machineToDelete is null)
        {
            return null;
        }
        else
        {
            var deletedMachine = _machineParkDbContext.Machines.Remove(machineToDelete);
            return deletedMachine.Entity;
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<Machine, bool>> expression)
    {
        return await _machineParkDbContext.Machines.AnyAsync(expression);
    }

    public int GetDataSetCount(int? dataSetSizeLimit)
    {
        int setCount;
        if (dataSetSizeLimit is not null)
        {
            setCount = _machineParkDbContext.Machines.Take((int)dataSetSizeLimit).Count();
        }
        else
        {
            setCount = _machineParkDbContext.Machines.Count();
        }
        return setCount;
    }

    public async Task SaveChangesAsync()
    {
        await _machineParkDbContext.SaveChangesAsync();
    }
}