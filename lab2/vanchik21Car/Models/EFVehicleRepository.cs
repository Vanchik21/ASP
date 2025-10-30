namespace vanchik21Car.Models
{
    public class EFVehicleRepository : IVehicleRepository
    {
        private VehicleDbContext context;
        public EFVehicleRepository(VehicleDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Vehicle> Vehicles => context.Vehicles;
    }
}
