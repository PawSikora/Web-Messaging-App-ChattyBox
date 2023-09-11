using System.Security.Cryptography.X509Certificates;
using DAL.Database;
using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DbChattyBox _context;
        public ChatRepository(DbChattyBox context)
        {
            _context = context;
        }

        public void AddUserToChat(UserChat userChat)
        {
            _context.UserChats.Add(userChat);
        }

        public void RemoveUserFromChat(UserChat userChat)
        {
            _context.UserChats.Remove(userChat);
        }

        public IEnumerable<User>? GetUsersInChat(int chatId, int pageNumber, int usersPerPage)
        {
            return _context.Chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.UserChats)
                .Select(uc => uc.User)
                .OrderByDescending(uc => uc.Username)
                .Skip((pageNumber - 1) * usersPerPage)
                .Take(usersPerPage).ToList();
        }

        public void CreateChat(Chat chat, UserChat userChat)
        {
            _context.Chats.Add(chat);
            _context.UserChats.Add(userChat);
        }

        public void DeleteChat(Chat chat)
        {
            _context.Chats.Remove(chat);
        }

        public Chat? GetChat(int chatId, int pageNumber,int messagesPerPage)
        {
            var chat = _context.Chats
                .Where(c => c.Id == chatId)
                .Include(c => c.Messages.OrderByDescending(m => m.TimeStamp).Skip((pageNumber - 1) * messagesPerPage).Take(messagesPerPage))
                .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .FirstOrDefault();

            return chat;
        }

        public int GetChatMessagesCount(int chatId)
        {
            return _context.Chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.Messages).Count();
        }

        public int GetChatUsersCount(int chatId)
        {
            return _context.Chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.UserChats).Count();
        }

        public Role? GetUserRole(int userId, int chatId)
        {
            return _context.UserChats
                .Where(uc => uc.UserId == userId && uc.ChatId == chatId)
                .Select(uc => uc.Role)
                .FirstOrDefault();
        }
       
        public Chat? GetById(int id)
        {
           return _context.Chats.FirstOrDefault(c => c.Id == id);
        }

        public bool IsUserInChat(int userId, int chatId)
        {
            return _context.UserChats.Any(uc => uc.UserId == userId && uc.ChatId == chatId);
        }

        public bool IsChatNameTaken(string name)
        {
           return _context.Chats.Any(c => c.Name == name);
        }

        public IEnumerable<UserChat>? GetChatUsersById(int chatId)
        {
           return _context.UserChats.Where(uc => uc.ChatId == chatId);
        }

        public void RemoveUsersFromChat(IEnumerable<UserChat> chatUsers)
        {
            _context.UserChats.RemoveRange(chatUsers);
        }

        public UserChat? GetUserChatById(int userId, int chatId)
        {
            return _context.UserChats.FirstOrDefault(uc => uc.UserId == userId && uc.ChatId == chatId);
        }

        public bool IsUserRole(int userId, int chatId, int roleId)
        {
           return _context.UserChats.Any(uc => uc.UserId == userId && uc.ChatId == chatId && uc.RoleId == roleId);
        }

        public IEnumerable<Chat> GetChatsForUser(int userId, int pageNumber, int chatsPerPage)
        {
            return _context.UserChats
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Chat)
                .OrderByDescending(c => c.Updated)
                .Skip((pageNumber - 1) * chatsPerPage)
                .Take(chatsPerPage);
        }

        public IEnumerable<User>? GetAllUsers(int chatId)
        {
	        return _context.Users.Where(x => x.UserChats.Any(uc => uc.ChatId == chatId)).ToList();

        }

		public void Save()
        {
           _context.SaveChanges();
        }

        public void Dispose()
        {
           _context.Dispose();
        }

    }

}
