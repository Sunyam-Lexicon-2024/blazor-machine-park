namespace Machines.Stats.GetMachineStats;

public class Mapper : ResponseMapper<MachineStatsModel, IEnumerable<Machine>>
{
    public override MachineStatsModel FromEntity(IEnumerable<Machine> machines)
    {
        var lastUpdatedMachine = machines.Where(m => m.UpdatedAt is not null).OrderBy(m => m.UpdatedAt).FirstOrDefault();

        lastUpdatedMachine ??= machines.FirstOrDefault();

        return new()
        {
            MachineCount = machines.Count(),
            OnlineCount = machines.Select(m => m.Online).Count(),
            OfflineCount = machines.Select(m => !m.Online).Count(),
            TotalWattage = machines.Sum(m => m.Wattage),
            LastUpdated = new()
            {
                Id = lastUpdatedMachine.Id,
                Name = lastUpdatedMachine.Name,
                Online = lastUpdatedMachine.Online,
                Wattage = lastUpdatedMachine.Wattage,
                Section = lastUpdatedMachine.Section,
                CreatedAt = lastUpdatedMachine.CreatedAt,
                UpdatedAt = lastUpdatedMachine.UpdatedAt
            }
        };
    }
}