using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // Booking is a reservation for one Guest, one Room, for a date range.
    public class Booking
    {
        public int Id { get; set; }

        public int GuestId { get; set; }

        public Guest Guest { get; set; } = null!;

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        // Nullable because Front Desk only assigns room number before check-in 
        public int? AssignedRoomId { get; set; }
        public Room? AssignedRoom { get; set; }

        // First night of the stay.
        public DateTime CheckIn { get; set; }

        // Last night of the stay (checkout morning).
        public DateTime CheckOut { get; set; }

        public decimal BasePriceAtBooking { get; set; }

        // The final calculated price after applying all dynamic pricing factors.
        public decimal FinalPrice { get; set; }

        // Once true, the FinalPrice is frozen and cannot be changed by later factor updates.
        public bool PriceLocked { get; set; } = false;

        // Current status of the booking: pending, confirmed, checkIn, checkedOut, cancelled
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // one Booking can have many Payments (deposit, balance, refunds)
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // one Booking can have many AddOns (early arrival, late checkout, pet fee).
        public ICollection<BookingAddOn> BookingAddOns { get; set; } = new List<BookingAddOn>();

        // one Booking can have at most one Review.
        public Review? Review { get; set; }
    }
}
