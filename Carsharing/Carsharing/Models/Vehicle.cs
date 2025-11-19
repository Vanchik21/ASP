using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Carsharing.Models
{
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

        [Required(ErrorMessage = "Please enter a make")]
        [MaxLength(50)]
        public string Make { get; set; } = String.Empty;

        [Required(ErrorMessage = "Please enter a model")]
        [MaxLength(50)]
        public string Model { get; set; } = String.Empty;

        [Range(1900, 2100, ErrorMessage = "Please enter a valid year")]
        public int Year { get; set; }

        [MaxLength(17, ErrorMessage = "VIN must be 17 characters or less")]
        public string VIN { get; set; } = String.Empty;

        [MaxLength(20, ErrorMessage = "License plate is too long")]
        public string LicensePlate { get; set; } = String.Empty;

        [Required(ErrorMessage = "Please specify a category")]
        [MaxLength(50)]
        public string Category { get; set; } = String.Empty;

        private decimal _dailyRate;

        [Column(TypeName = "decimal(8,2)")]
        [Range(0.01, 100000, ErrorMessage = "Please enter a positive daily rate")]
        public decimal DailyRate
        {
            get => _dailyRate;
            set => _dailyRate = value;
        }

        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        [NotMapped]
        public bool IsAvailable => Status == VehicleStatus.Available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    }
}