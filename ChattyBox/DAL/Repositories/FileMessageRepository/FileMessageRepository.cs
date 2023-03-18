using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.FileMessageRepository
{
    public class FileMessageRepository : IFileMessageRepository
    {
        private readonly DbChattyBox _context;
        public FileMessageRepository(DbChattyBox context) 
        {
            _context = context;
        }
        

        
    }
}
