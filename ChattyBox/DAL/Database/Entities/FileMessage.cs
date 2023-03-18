using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Entities
{
    public class FileMessage : Message
    {
        [Required(ErrorMessage = "Pole jest wymagane")]
        [MaxLength(100, ErrorMessage = "Sciezka jest za długa")]
        [MinLength(1, ErrorMessage = "Niepoprawne dane")]
        public string Path { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }

    }
}
