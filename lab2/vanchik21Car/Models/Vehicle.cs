using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vanchik21Car.Models
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

        [Required]
        public string Make { get; set; } = String.Empty;

        [Required]
        public string Model { get; set; } = String.Empty;

        public int Year { get; set; }

        public string VIN { get; set; } = String.Empty;

        public string LicensePlate { get; set; } = String.Empty;

        public string Category { get; set; } = String.Empty;

        private decimal _dailyRate;

        [Column(TypeName = "decimal(8,2)")]
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
    }
}