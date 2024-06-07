namespace MachinePark.Core.Models;

public class MachineStatsModel {
    public int MachineCount { get; set; }
    public int OnlineCount { get; set; }
    public int OfflineCount { get; set; }
    public double TotalWattage { get; set; }
    public MachineModel LastUpdated { get; set; }
}