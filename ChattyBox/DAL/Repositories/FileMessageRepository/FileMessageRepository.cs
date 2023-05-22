using DAL.Database;
using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.FileMessageRepository
{
    public class FileMessageRepository : IFileMessageRepository
    {
        private readonly DbChattyBox _context;
        public FileMessageRepository(DbChattyBox context) 
        {
            _context = context;
        }

        public void CreateFileMessage(FileMessage message)
        {
            _context.FileMessages.Add(message);
        }

        public void DeleteFileMessage(FileMessage fileMessage)
        {
            _context.FileMessages.Remove(fileMessage);
        }

        public bool IsFileNameTaken(string fileName)
        {
            return _context.FileMessages.Any(f => f.Name == fileName);
        }

        public FileMessage? GetById(int chatid)
        {
            return _context.FileMessages.FirstOrDefault(f => f.Id == chatid);
        }

        public FileMessage? GetLastFileMessage(int chatid)
        {
            return _context.FileMessages
                 .Include(m => m.Sender)
                 .Where(m => m.ChatId == chatid)
                 .OrderByDescending(m => m.TimeStamp)
                 .FirstOrDefault();
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
