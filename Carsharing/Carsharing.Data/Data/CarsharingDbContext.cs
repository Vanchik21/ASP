using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Carsharing.Data.Models;

namespace Carsharing.Data.Data;

public class CarsharingDbContext : IdentityDbContext<ApplicationUser>
{
    public CarsharingDbContext(DbContextOptions<CarsharingDbContext> options) : base(options) {}

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();
}
