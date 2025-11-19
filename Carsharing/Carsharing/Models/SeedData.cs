using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Models
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

            if (!context.MaintenanceRecords.Any())
            {
                var vehicles = context.Vehicles
                .OrderBy(v => v.VehicleID)
                .Take(3)
                .Select(v => new { v.VehicleID, v.Make, v.Model })
                .ToList();

                if (vehicles.Any())
                {
                    var now = DateTime.UtcNow;
                    context.MaintenanceRecords.AddRange(
                    new MaintenanceRecord
                    {
                        VehicleID = vehicles[0].VehicleID,
                        Description = $"Oil change for {vehicles[0].Make} {vehicles[0].Model}",
                        PerformedAt = now.AddDays(-14),
                        Cost = 39.99m,
                        PerformedBy = "QuickLube"
                    },
                    new MaintenanceRecord
                    {
                        VehicleID = vehicles[0].VehicleID,
                        Description = "Tire rotation",
                        PerformedAt = now.AddDays(-7),
                        Cost = 25.00m,
                        PerformedBy = "AutoCare"
                    },
                    new MaintenanceRecord
                    {
                        VehicleID = vehicles[Math.Min(1, vehicles.Count - 1)].VehicleID,
                        Description = "Brake inspection",
                        PerformedAt = now.AddDays(-30),
                        Cost = 59.00m,
                        PerformedBy = "BrakeMasters"
                    },
                    new MaintenanceRecord
                    {
                        VehicleID = vehicles[Math.Min(2, vehicles.Count - 1)].VehicleID,
                        Description = "Battery check",
                        PerformedAt = now.AddDays(-3),
                        Cost = 0.00m,
                        PerformedBy = "ServiceCenter"
                    }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
