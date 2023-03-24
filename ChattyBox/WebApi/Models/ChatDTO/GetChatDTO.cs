using DAL.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.ChatDTO
{
    public class GetChatDTO
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(32, ErrorMessage = "Nazwa jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }

        public int ChatId { get; set; }

        /*public virtual ICollection<User> Users { get; set; }*/
    }
}
