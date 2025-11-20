using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.Data.Models;

public class MaintenanceRecord
{
     [Key]
     public long MaintenanceRecordID { get; set; }

     [Required]
     [Range(1,long.MaxValue)]
     public long VehicleID { get; set; }

     [ForeignKey(nameof(VehicleID))]
     public Vehicle? Vehicle { get; set; }

     [Required, MaxLength(200)]
     public string Description { get; set; } = string.Empty;

     public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

     [Column(TypeName="decimal(10,2)")]
     [Range(0,1000000)]
     public decimal Cost { get; set; }

     [MaxLength(100)]
     public string? PerformedBy { get; set; }
}