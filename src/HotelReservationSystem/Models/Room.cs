using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    // Each row is one room in the hotel, for example, Room 204.
    public class Room
    {
        public int Id { get; set; }

        // Stored as text, such as a label, not quantity 
        [Required, MaxLength(10)]
        public string RoomNumber { get; set; } = string.Empty;

        // Tells us which category this room belongs to.
        public int RoomTypeId { get; set; }

        public RoomType RoomType { get; set; } = null!;


        // Current status of the room: available, occupied, dirty, outOfService
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Available";
    }
}
