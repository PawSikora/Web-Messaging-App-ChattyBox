using System.ComponentModel.DataAnnotations;

namespace Web.Models.UserDtos 
{
    public class LoginUserDTO
    {
        [EmailAddress(ErrorMessage = "Niepoprawny format")]
        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
