using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models.MessagesDTO
{
    public class CreateTextMessageDTO : MessageDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tekst jest wymagany")]
        [MaxLength(500, ErrorMessage = "Tekst jest zbyt dlugi")]
        [MinLength(1, ErrorMessage = "Nie wpisanu tekstu")]
        public string Content { get; set; }

        public override string MessageType => "createText";
    }
}
