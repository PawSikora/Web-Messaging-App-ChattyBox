using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
