using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;
using System.ComponentModel.DataAnnotations;


namespace BLL.DataTransferObjects.ChatDtos
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
