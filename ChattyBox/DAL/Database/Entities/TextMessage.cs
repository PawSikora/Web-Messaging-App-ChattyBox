using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
