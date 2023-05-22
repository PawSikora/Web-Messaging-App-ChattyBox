using DAL.Database.Entities;

namespace DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        int GetUserChatsCount(int id);
        User? GetById(int id);
        User? GetUserByEmail(string email);
        bool IsEmailTaken(string email);
        void Save();
        void Dispose();
    }
    
}
