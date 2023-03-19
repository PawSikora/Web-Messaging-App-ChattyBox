using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Entities
{
    public class Chat
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(32, ErrorMessage = "Nazwa jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public virtual ICollection<UserChat> UserChats { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }

        
    }
}
