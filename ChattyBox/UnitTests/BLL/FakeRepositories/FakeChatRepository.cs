using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Database.Entities;
using DAL.Repositories.ChatRepository;

namespace UnitTests.BLL.FakeRepositories
{
    public class FakeChatRepository : IChatRepository
    {
        private List<Chat> _chats = new List<Chat>();
        private List<UserChat> _userChats = new List<UserChat>();
        public Chat? GetById(int id)
        {
            return _chats.SingleOrDefault(c => c.Id == id);
        }

        public bool IsUserInChat(int userId, int chatId)
        {
            return _chats.Any(c => c.Id == chatId && c.UserChats.Any(uc => uc.UserId == userId));
        }

        public bool IsChatNameTaken(string name)
        {
            return _chats.Any(c => c.Name == name);
        }

        public IEnumerable<UserChat>? GetChatUsersById(int chatId)
        {
           // return _chats.SingleOrDefault(c => c.Id == chatId)?.UserChats;
           throw new NotImplementedException();
        }

        public void RemoveUsersFromChat(IEnumerable<UserChat> chatUsers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User>? GetUsersInChat(int chatId)
        {
            throw new NotImplementedException();
        }

        public UserChat? GetUserChatById(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public void AddUserToChat(UserChat userChat)
        {
            _userChats.Add(userChat);
        }

        public void RemoveUserFromChat(UserChat userChat)
        {
            _userChats.Remove(userChat);
        }

        public void CreateChat(Chat chat, UserChat userChat)
        {
            _chats.Add(chat);
            _userChats.Add(userChat);
        }

        public void DeleteChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public Chat? GetChat(int id, int pageNumber, int messagesPerPage)
        {
            throw new NotImplementedException();
        }

        public int GetChatMessagesCount(int chatId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserRole(int userId, int chatId, int roleId)
        {
            throw new NotImplementedException();
        }

        public Role? GetUserRole(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Chat> GetChatsForUser(int userId, int pageNumber, int chatsPerPage)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
