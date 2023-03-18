using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.TextMessageRepository
{
    public class TextMessageRepository : ITextMessageRepository
    {
        private readonly DbChattyBox _context;
        public TextMessageRepository(DbChattyBox context) 
        {
            _context = context;
        }
    }
}
