using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        // [Required] and [EmailAddress] indicate tehse cannot be empty and must be in email format 
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        // We store a BCrypt hash, so even if the DB leaks, nobody can read the original password.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
