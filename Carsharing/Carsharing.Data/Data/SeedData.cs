using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Carsharing.Data.Models;

namespace Carsharing.Data.Data;

public static class SeedData
{
    public static void EnsurePopulated(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (!context.Vehicles.Any())
        {
            context.Vehicles.AddRange(
            new Vehicle { Make = "Toyota", Model = "Camry", Year =2021, VIN = "JT123456789012345", LicensePlate = "ABC-1234", Category = "Sedan", DailyRate =49.99m },
            new Vehicle { Make = "Honda", Model = "Civic", Year =2020, VIN = "HN123456789012345", LicensePlate = "CIV-2020", Category = "Sedan", DailyRate =45.50m }
            );
            context.SaveChanges();
        }
    }
}