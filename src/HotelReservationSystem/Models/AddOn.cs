using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // An AddOn is an extra service a guest can purchase with a booking:
    // Early Arrival ($20), Late Checkout ($15), Pet Fee ($25).
    // AddOns are managed by the Administrator and have a current price.
    public class AddOn
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // CURRENT price of this add-on. If the Administrator changes this later,
        // existing bookings keep their own locked price, stored in BookingAddOn.Price.
        public decimal Price { get; set; }
    }
}