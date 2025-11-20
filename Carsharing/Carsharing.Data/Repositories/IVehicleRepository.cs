using Carsharing.Data.Models;

namespace Carsharing.Data.Repositories;

public interface IVehicleRepository
{
    IQueryable<Vehicle> Vehicles { get; }
}