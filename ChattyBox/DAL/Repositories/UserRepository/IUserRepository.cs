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
        User LoginUser(string email, string password);
        void RegisterUser(string email, string username, string password);
        User GetUser(int id);
        ICollection<Chat> GetChats(int id, int pageNumber, int chatsPerPage);
        int GetUserChatsCount(int id);

    }
    
}
