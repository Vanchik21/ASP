using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.Data.Models;

public enum VehicleStatus
{
     Available,
     Rented,
     Maintenance,
     Reserved,
     Decommissioned
}

public class Vehicle
{
     [Key]
     public long VehicleID { get; set; }

     [Required, MaxLength(50)]
     public string Make { get; set; } = string.Empty;

     [Required, MaxLength(50)]
     public string Model { get; set; } = string.Empty;

     [Range(1900,2100)]
     public int Year { get; set; }

     [MaxLength(17)]
     public string VIN { get; set; } = string.Empty;

     [MaxLength(20)]
     public string LicensePlate { get; set; } = string.Empty;

     [Required, MaxLength(50)]
     public string Category { get; set; } = string.Empty;

     [Column(TypeName="decimal(8,2)")]
     [Range(0.01,100000)]
     public decimal DailyRate { get; set; }

     public VehicleStatus Status { get; set; } = VehicleStatus.Available;

     [NotMapped]
     public bool IsAvailable => Status == VehicleStatus.Available;

     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     public DateTime? UpdatedAt { get; set; }

     public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
}