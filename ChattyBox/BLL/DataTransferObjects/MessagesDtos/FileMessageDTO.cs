using System.ComponentModel.DataAnnotations;

namespace BLL.DataTransferObjects.MessageDtos
{
    public class FileMessageDTO : MessageDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tekst jest wymagany")]
        [MaxLength(500, ErrorMessage = "Tekst jest zbyt dlugi")]
        [MinLength(1, ErrorMessage = "Nie wpisanu tekstu")]


        public override string MessageType => "file";


        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(100, ErrorMessage = "Sciezka jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Path { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(100, ErrorMessage = "Nazwa pliku jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }

    }
}
