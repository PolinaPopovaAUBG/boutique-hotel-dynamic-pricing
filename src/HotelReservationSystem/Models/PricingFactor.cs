using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // The class stores the lookup tables used by the dynamic pricing algorithm:
    // occupancy, lead time, and length of stay.
    // Seasonal multipliers are outlined in the SeasonalRates table.
    //
    // Example rows:
    // FactorType=Occupancy, MinValue=0.85, MaxValue=0.95, Multiplier=1.30
    // FactorType=LeadTime, MinValue=7, MaxValue=29, Multiplier=1.00
    // FactorType=LengthOfStay, MinValue=4, MaxValue=6, Multiplier=0.90
    public class PricingFactor
    {
        public int Id { get; set; }

        // Allowed values: Occupancy, LeadTime, LengthOfStay.
        [Required, MaxLength(30)]
        public string FactorType { get; set; } = string.Empty;

        // Lower bound of the range is inclusive.
        // For Occupancy: a percentage as a decimal (for example, 85%).
        // For LeadTime: number of days.
        // For LengthOfStay: number of nights.
        public decimal MinValue { get; set; }

        // Upper bound of the range is exclusive.
        // Same units as MinValue.
        public decimal MaxValue { get; set; }

        // Multiplier applied when the input falls in this range.
        public decimal Multiplier { get; set; }
    }
}
