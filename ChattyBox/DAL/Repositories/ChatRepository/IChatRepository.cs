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
        Chat? GetById(int id);
        bool IsUserInChat(int userId, int chatId);
        bool IsChatNameTaken(string name);
        IEnumerable<UserChat>? GetChatUsersById(int chatId);
        void RemoveUsersFromChat(IEnumerable<UserChat> chatUsers);
        IEnumerable<User>? GetUsersInChat(int chatId);
        public UserChat? GetUserChatById(int userId, int chatId);
        void AddUserToChat(UserChat userChat);
        public void RemoveUserFromChat(UserChat userChat);
        void CreateChat(Chat chat, UserChat userChat);
        void DeleteChat(Chat chat);
        Chat? GetChat(int id, int pageNumber,int messagesPerPage);
        int GetChatMessagesCount(int chatId);
        bool IsUserRole(int userId, int chatId, int roleId);
        Role? GetUserRole(int userId, int chatId);
        IEnumerable<Chat> GetChatsForUser(int userId, int pageNumber, int chatsPerPage);
        void Save();
        void Dispose();
    }
}
