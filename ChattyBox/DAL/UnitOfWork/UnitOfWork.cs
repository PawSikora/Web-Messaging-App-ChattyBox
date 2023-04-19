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
        public UnitOfWork(IChatRepository chatRepository, IFileMessageRepository fileMessageRepository, 
            ITextMessageRepository textMessageRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
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
            Chats.Dispose();
            FileMessages.Dispose();
            TextMessages.Dispose();
            Users.Dispose();
            Roles.Dispose();
        }

        public void Save()
        {
           Chats.Save();
           FileMessages.Save();
           TextMessages.Save();
           Users.Save();
           Roles.Save();
        }
    }
}
