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
        public void AddUserById(int userId, int chatId);
        public void DeleteUserById(int userId, int chatId);
        void CreateChat(string name, int userId);
        void DeleteChat(int chatId);
        Chat GetChat(int id, int pageNumber);
        int GetChatMessagesCount(int chatId);
    }
}
