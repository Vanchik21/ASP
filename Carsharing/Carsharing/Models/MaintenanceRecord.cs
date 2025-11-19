using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Carsharing.Models
{
    public class MaintenanceRecord
    {
        [Key]
        public long MaintenanceRecordID { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please select a vehicle")]
        public long VehicleID { get; set; }

        [ForeignKey(nameof(VehicleID))]
        [ValidateNever]
        public Vehicle? Vehicle { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [MaxLength(200, ErrorMessage = "Description is too long")]
        public string Description { get; set; } = string.Empty;

        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 1000000, ErrorMessage = "Please enter a valid cost")]
        public decimal Cost { get; set; }

        [MaxLength(100)]
        public string? PerformedBy { get; set; }
    }
}
