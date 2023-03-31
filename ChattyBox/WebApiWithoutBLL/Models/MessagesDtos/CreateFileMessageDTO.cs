using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace WebApiWithoutBLL.Models.MessagesDtos
{
    public class CreateFileMessageDTO :MessageDTO
    {
        [JsonIgnore] 
        public int Id { get; set; }


        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(100, ErrorMessage = "Sciezka jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Path { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(100, ErrorMessage = "Nazwa pliku jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }

        public override string MessageType => "createFile";


    }
}
