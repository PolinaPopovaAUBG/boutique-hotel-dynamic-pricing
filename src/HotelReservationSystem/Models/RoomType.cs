using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // A RoomType is a category of room defined by its view and bed type.
    // Example: "Standard King", "Harbor View King", "Breakwater Two Doubles".
    // The Administrator sets the BasePrice here.
    public class RoomType
    {
        public int Id { get; set; }

        // Room name shown to guests, such as "Harbor View King".
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        // Type of view: Standard, Cape Tip, Harbor, Breakwater.
        // Used for filtering and for a cleaner E-R diagram.
        [Required, MaxLength(30)]
        public string View { get; set; } = string.Empty;

        // Bed type: King, Two Doubles, Two Queens.
        [Required, MaxLength(30)]
        public string BedType { get; set; } = string.Empty;

        // King = 2, Two Doubles = 4.
        [Range(1, 10)]
        public int Capacity { get; set; }

        // Base price before dynamic pricing factors apply.
        [Range(0, 10000)]
        public decimal BasePrice { get; set; }

    }
}
