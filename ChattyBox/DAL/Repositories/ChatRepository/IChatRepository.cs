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
        IEnumerable<User> GetUsersInChat(int chatId);
        void AddUserByEmail(string email, int chatId);
        void DeleteUserByEmail(string email, int chatId);
        Chat CreateChat(string name,User user);
        void DeleteChat(int chatId);
        Chat GetChat(int id);
    }
}
