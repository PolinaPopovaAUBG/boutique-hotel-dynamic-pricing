using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // This class records sensitive actions performed in the system to be able to
    // trace back the employee who performed it, such as price override, refund approval, or staff change.
    //
    // Example rows:
    // User=5 (Admin), Action="RefundApproved", Entity="Payment", EntityId=17
    public class AuditLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Examples: "PriceOverride", "RefundRequested", "RefundApproved", "StaffDeactivated", "RoomStatusChanged".
        [Required, MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        // Which table or entity the action affected.
        // Examples: "Booking", "Payment", "Staff", "Room".
        [MaxLength(50)]
        public string? Entity { get; set; }

        public int? EntityId { get; set; }

        // Previous value as text, if relevant.
        public string? OldValue { get; set; }

        // New value as text, if relevant.
        public string? NewValue { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
