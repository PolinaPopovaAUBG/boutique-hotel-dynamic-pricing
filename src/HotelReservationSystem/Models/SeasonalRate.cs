using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // Each row defines a date range and a multiplier used by the dynamic pricing algorithm.
    // Example: High Season from July 1 to August 31 with multiplier 1.25.
    // The Administrator can edit these rows.
    public class SeasonalRate
    {
        public int Id { get; set; }

        // High Season or Low Season.
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // First date of the season (inclusive).
        public DateTime StartDate { get; set; }

        // Last date of the season (inclusive).
        public DateTime EndDate { get; set; }

        // Multiplier applied to the base price during the season.
        public decimal Multiplier { get; set; }
    }
}
