namespace vanchik21Car.Models
{
    public interface IVehicleRepository
    {
        IQueryable<Vehicle> Vehicles { get; }
    }
}
