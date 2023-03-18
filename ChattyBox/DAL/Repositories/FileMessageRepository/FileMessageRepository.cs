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

        public FileMessage CreateFileMessage(string path, string name)
        {
            FileMessage message = new FileMessage
            {
                Path = path,
                Name = name,
                TimeStamp = DateTime.Now
            };
            return message;
        }

        public void DeleteFileMessage(string path)
        {
            FileMessage fileMessage = _context.Files.FirstOrDefault(f => f.Path == path) ??
                                      throw new Exception("Nie ma takiego pliku ");
            _context.Files.Remove(fileMessage);
        }
    }
}
