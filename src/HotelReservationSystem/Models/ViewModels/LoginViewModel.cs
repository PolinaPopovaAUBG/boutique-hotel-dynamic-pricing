
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.ViewModels
{
    //demonstrates what the login form submits
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;


    }
}
