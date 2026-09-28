using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // A Review is left by a Guest after checkout, for one specific Booking.
    // Each Booking has at most one Review.
    public class Review
    {
        public int Id { get; set; }

        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;

        // Which stay the review is about.
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // 1 is terrible, 5 is excellent. 
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
