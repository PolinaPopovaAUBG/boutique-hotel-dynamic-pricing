namespace HotelReservationSystem.Models
{
    // It is a junction table linking Bookings to AddOns.
    public class BookingAddOn
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int AddOnId { get; set; }
        public AddOn AddOn { get; set; } = null!;
        public int Quantity { get; set; } = 1;

        // The add-on's price at the time of booking - changes future prices only when/if admin changes the add on prices. 
        public decimal PriceAtBooking { get; set; }
    }
}
