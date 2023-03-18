using DAL.Database;
using DAL.Repositories.ChatRepository;
using DAL.Repositories.FileMessageRepository;
using DAL.Repositories.TextMessageRepository;
using DAL.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbChattyBox _context;

        public UnitOfWork(DbChattyBox context)
        {
            _context = context;
            Chats = new ChatRepository(_context);
            FileMessages = new FileMessageRepository(_context);
            TextMessages = new TextMessageRepository(_context);
            Users = new UserRepository(_context);
        }

        public IChatRepository Chats { get; private set; }

        public IFileMessageRepository FileMessages { get; private set; }

        public ITextMessageRepository TextMessages { get; private set; }

        public IUserRepository Users { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
