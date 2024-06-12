using System.Linq.Expressions;

namespace MachinePark.API.Services;

public interface IQueryParams
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public class QueryParams : IQueryParams
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<IEnumerable<Machine>> GetAllMachinesAsync(QueryParams queryParams);
    Task<Machine?> GetByIdAsync(int machineId);
    Task<Machine?> AddAsync(Machine machine);
    Task<Machine?> UpdateAsync(Machine machine);
    Task<Machine?> DeleteAsync(int machineId);

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

        if (queryParams.Page is not null && queryParams.PageSize is not null)
        {
            machines = machines
                        .Skip((int)(queryParams.Page * queryParams.PageSize))
                        .Take((int)queryParams.PageSize);
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

    public async Task SaveChangesAsync()
    {
        await _machineParkDbContext.SaveChangesAsync();
    }
}