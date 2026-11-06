
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.ViewModels
{
    //what the register form submits
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]

        [DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
