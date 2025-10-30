using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace vanchik21Car.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            VehicleDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<VehicleDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Vehicles.Any())
            {
                context.Vehicles.AddRange(
                new Vehicle
                {
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2021,
                    VIN = "JT123456789012345",
                    LicensePlate = "ABC-1234",
                    Category = "Sedan",
                    DailyRate = 49.99m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2020,
                    VIN = "HN123456789012345",
                    LicensePlate = "CIV-2020",
                    Category = "Sedan",
                    DailyRate = 45.50m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "Ford",
                    Model = "Explorer",
                    Year = 2019,
                    VIN = "FD123456789012345",
                    LicensePlate = "SUV-7890",
                    Category = "SUV",
                    DailyRate = 69.00m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "Tesla",
                    Model = "Model3",
                    Year = 2022,
                    VIN = "TS123456789012345",
                    LicensePlate = "EV-3333",
                    Category = "Electric",
                    DailyRate = 99.95m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "BMW",
                    Model = "X5",
                    Year = 2018,
                    VIN = "BM123456789012345",
                    LicensePlate = "BMW-X5",
                    Category = "SUV",
                    DailyRate = 89.99m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "Chevrolet",
                    Model = "Malibu",
                    Year = 2017,
                    VIN = "CH123456789012345",
                    LicensePlate = "MLB-2017",
                    Category = "Sedan",
                    DailyRate = 39.99m,
                    Status = VehicleStatus.Available
                },
                new Vehicle
                {
                    Make = "Nissan",
                    Model = "Rogue",
                    Year = 2019,
                    VIN = "NS123456789012345",
                    LicensePlate = "ROG-2019",
                    Category = "SUV",
                    DailyRate = 59.99m,
                    Status = VehicleStatus.Available
                }
                );
                context.SaveChanges();
            }
        }
    }
}
