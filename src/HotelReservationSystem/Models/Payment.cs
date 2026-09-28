using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // A single Booking can have multiple Payments
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // Amount paid (positive) or refunded (positive amount and RefundStatus).
        public decimal Amount { get; set; }

        //payment method is only set to card

        // Current state of the payment: pending, completed, refunded, failed
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        // When the payment was completed 
        public DateTime? PaidAt { get; set; }

        // Amount refunded if exists.
        public decimal RefundAmount { get; set; } = 0;

        // Refund status: requested, apprived, rejected
        public string? RefundStatus { get; set; }
    }
}
