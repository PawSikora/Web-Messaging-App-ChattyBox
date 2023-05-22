using System.ComponentModel.DataAnnotations;
using MVCWebAppWithoutBLL.Models.MessagesDtos;
using MVCWebAppWithoutBLL.Models.UserDtos;

namespace MVCWebAppWithoutBLL.Models.ChatDtos
{
    public class GetChatDTO
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(32, ErrorMessage = "Nazwa jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }

        public int ChatId { get; set; }

        public virtual ICollection<UserDTO> Users { get; set; }

        public virtual ICollection<MessageDTO>? AllMessages { get; set; }


    }
}
