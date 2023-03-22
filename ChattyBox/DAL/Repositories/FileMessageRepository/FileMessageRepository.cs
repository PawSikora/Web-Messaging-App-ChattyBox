using DAL.Database;
using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        public FileMessage CreateFileMessage(string userEmail, string path, int chatId)
        {
            if (!File.Exists(path)) throw new Exception("Nie znaleziono pliku");
            if (_context.FileMessages.Any(f=>f.Path == path)) throw new Exception("Plik juz istnieje w tym miejscu");

            User user = _context.Users.SingleOrDefault(u => u.Email == userEmail) ?? throw new Exception("Nie znaleziono uzytkownika");
            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono czatu");

            FileInfo file = new FileInfo(path);
            double fileSizeOnMB = (double)file.Length / (1024 * 1024);
            
            FileMessage message = new FileMessage
            {
                Path = path,
                Name = file.Name,
                Sender = user,
                Chat = chat,
                Size = fileSizeOnMB,
                TimeStamp = DateTime.Now
            };

            _context.FileMessages.Add(message);
            return message;
        }

        public void DeleteFileMessage(int id)
        {
            var fileMessage = _context.FileMessages.SingleOrDefault(f => f.Id == id) ?? throw new Exception("Plik nie istnieje");
            _context.FileMessages.Remove(fileMessage);
        }

        public FileMessage GetFileMessage(int id)
        {
            var fileMessage = _context.FileMessages.SingleOrDefault(f => f.Id == id) ?? throw new Exception("Nie znaleziono wiadomosci");
            return fileMessage;
        }
    }
}
