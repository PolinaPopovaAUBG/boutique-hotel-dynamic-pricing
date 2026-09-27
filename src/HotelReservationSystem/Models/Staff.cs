using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class Staff
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        // Whether this staff account is still active. If employee is not working - status is not inactive.
        public bool Active { get; set; } = true;
    }
}
