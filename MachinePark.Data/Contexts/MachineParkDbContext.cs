using MachinePark.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MachinePark.Data.Contexts;

public class MachineParkDbContext(DbContextOptions options) : DbContext(options) {

    public DbSet<Machine> Machines => Set<Machine>();
}