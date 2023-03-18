using System.ComponentModel.DataAnnotations;

namespace Web.Models.UserDTOs
{
    public class LoginUserDTO
    {
        [EmailAddress(ErrorMessage = "Niepoprawny format")]
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
