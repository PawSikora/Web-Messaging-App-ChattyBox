using DAL.Repositories.ChatRepository;
using DAL.Repositories.FileMessageRepository;
using DAL.Repositories.TextMessageRepository;
using DAL.Repositories.UserRepository;
using DAL.Repositories.RoleRepository;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IChatRepository Chats { get; }

        IFileMessageRepository FileMessages { get; }

        ITextMessageRepository TextMessages { get; }

        IUserRepository Users { get; }

        IRoleRepository Roles { get; }

        void Save();

    }
}
