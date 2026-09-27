using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class Guest
    {
       
        public int Id { get; set; }

        public int UserId { get; set; }

        // User is not null at runtime.
        public User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

    }
}
