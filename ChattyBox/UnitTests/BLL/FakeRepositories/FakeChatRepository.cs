using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Database.Entities;
using DAL.Repositories.ChatRepository;
using Microsoft.EntityFrameworkCore;

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
            return _userChats.Where(c=>c.ChatId==chatId);

        }

        public void RemoveUsersFromChat(IEnumerable<UserChat> chatUsers)
        {

            foreach (var userChat in chatUsers)
            {
                var userChatToRemove = _userChats.FirstOrDefault(x => x.UserId == userChat.UserId && x.ChatId == userChat.ChatId);
                if (userChatToRemove != null)
                {
                    _userChats.Remove(userChatToRemove);
                }
            }
        }

        public IEnumerable<User>? GetUsersInChat(int chatId)
        {
            var users = _chats
                .Where(c => c.Id == chatId)
                .SelectMany(c => c.UserChats)
                .Select(uc => uc.User).ToList();

            return users;
        }

        public UserChat? GetUserChatById(int userId, int chatId)
        {
            return _userChats.FirstOrDefault(uc => uc.UserId == userId && uc.ChatId == chatId);
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
        public void CreateChat(Chat chat)
        {
            _chats.Add(chat);
        }

        public void DeleteChat(Chat chat)
        {
            _chats.Remove(chat);
        }

        public Chat? GetChat(int id, int pageNumber, int messagesPerPage)
        {
            throw new NotImplementedException();
        }

        public int GetChatMessagesCount(int chatId)
        {
            return _chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.Messages).Count();
        }

        public bool IsUserRole(int userId, int chatId, int roleId)
        {
            return _userChats.Any(uc => uc.UserId == userId && uc.ChatId == chatId && uc.RoleId == roleId);

        }

        public Role? GetUserRole(int userId, int chatId)
        {
            return _userChats
                .Where(uc => uc.UserId == userId && uc.ChatId == chatId)
                .Select(uc => uc.Role)
                .FirstOrDefault();
        }

        public IEnumerable<Chat> GetChatsForUser(int userId, int pageNumber, int chatsPerPage)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }


    }
}
