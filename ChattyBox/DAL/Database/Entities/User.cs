using System.ComponentModel.DataAnnotations;

namespace DAL.Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Pole jest wymagane")]
        [EmailAddress(ErrorMessage = "Niepoprawne dane")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(32, ErrorMessage = "Nazwa jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
        
        public byte[] PasswordSalt { get; set; }

        public DateTime? LastLog { get; set; }
        
        public DateTime Created { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }

        public virtual ICollection<UserChat>? UserChats { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }
    }
}
