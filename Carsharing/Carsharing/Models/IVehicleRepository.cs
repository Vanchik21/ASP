namespace Carsharing.Models
{
    public interface IVehicleRepository
    {
        IQueryable<Vehicle> Vehicles { get; }
    }
}
