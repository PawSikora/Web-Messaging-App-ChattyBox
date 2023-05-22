using System.ComponentModel.DataAnnotations;

namespace DAL.Database.Entities
{
    public class TextMessage : Message
    {
        [Required(ErrorMessage = "Tekst jest wymagany")]
        [MaxLength(500, ErrorMessage = "Tekst jest zbyt dlugi")]
        [MinLength(1, ErrorMessage = "Nie wpisanu tekstu")]
        public string Content { get; set; }
    }
}
