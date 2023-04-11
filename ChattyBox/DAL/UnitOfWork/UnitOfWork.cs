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
using DAL.Repositories.RoleRepository;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbChattyBox _context;

        public UnitOfWork(DbChattyBox context, IChatRepository chatRepository, IFileMessageRepository fileMessageRepository, 
            ITextMessageRepository textMessageRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _context = context;
            Chats = chatRepository;
            FileMessages = fileMessageRepository;
            TextMessages = textMessageRepository;
            Users = userRepository;
            Roles = roleRepository;
        }
        
        public IChatRepository Chats { get; private set; }

        public IFileMessageRepository FileMessages { get; private set; }

        public ITextMessageRepository TextMessages { get; private set; }

        public IUserRepository Users { get; private set; }

        public IRoleRepository Roles { get; private set; }

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
