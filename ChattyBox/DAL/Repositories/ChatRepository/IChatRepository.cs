using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.ChatRepository
{
    public interface IChatRepository
    {
        IEnumerable<User> GetUsersInChat(string chatName);
        void AddUserByEmail(string email, string chatName);
        void DeleteUserByEmail(string email, string chatName);
        Chat CreateChat(string name,User user);
        void DeleteChat(string name);
    }
}
