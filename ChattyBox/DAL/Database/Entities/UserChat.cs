using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Entities
{
    public class UserChat
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
