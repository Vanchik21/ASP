using Carsharing.Data.Data;
using Carsharing.Data.Models;

namespace Carsharing.Data.Repositories;

public class EFVehicleRepository : IVehicleRepository
{
     private readonly CarsharingDbContext _context;
     public EFVehicleRepository(CarsharingDbContext context) => _context = context;
     public IQueryable<Vehicle> Vehicles => _context.Vehicles;
}