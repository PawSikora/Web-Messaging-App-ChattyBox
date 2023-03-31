using System.ComponentModel.DataAnnotations;

namespace BLL.DataTransferObjects.ChatDtos   
{
    public class CreateChatDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(32, ErrorMessage = "Nazwa jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }
    }
}
