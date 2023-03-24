using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.TextMessageDTO
{
    public class CreateTextMessageDTO
    {
        [Required(ErrorMessage = "Tekst jest wymagany")]
        [MaxLength(500, ErrorMessage = "Tekst jest zbyt dlugi")]
        [MinLength(1, ErrorMessage = "Nie wpisanu tekstu")]
        public string Content { get; set; }

        public int ChatId { get; set; }

        public int SenderId { get; set; }
    }
}
