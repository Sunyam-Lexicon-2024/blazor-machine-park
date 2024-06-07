using MachinePark.Core.Entities;
using MachinePark.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MachinePark.Data.Seeds;

public class MachineSeeds(MachineParkDbContext context)
{

    private readonly MachineParkDbContext _context = context;
    private readonly Random _rnd = new();

    private List<Machine> _machines = [];

    public async Task InitAsync()
    {
        try
        {
            GenerateMachines(50);
            foreach (var m in _machines)
            {
                await _context.Machines.AddAsync(m);
            }
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }


    private void GenerateMachines(int count)
    {
        for (var i = 0; i < count; i++)
        {
            Machine machineToAdd = new()
            {
                Name = $"Machine-{i + 1}",
                Online = true,
                Section = _rnd.Next(1, 20),
                Wattage = RandomWattage()
            };
            _machines.Add(machineToAdd);
        }
    }

    private double RandomWattage()
    {
        int precision = 100000000;
        return _rnd.Next(0, precision + 1) / ((double)precision) * 100;
    }
}

