using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using DAL.Exceptions;

namespace DAL.Repositories.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DbChattyBox _context;
        public ChatRepository(DbChattyBox context)
        {
            _context = context;
        }

        public void AddUserById(int userId, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono chatu");
            Role role = _context.Roles.SingleOrDefault(r => r.Id == 2) ?? throw new NotFoundException("Nie znaleziono roli");

            if (_context.UserChats.Any(u => u.User == user && u.Chat == chat))
            {
                throw new IllegalOperationException("Uzytkownik jest juz w czacie");
            }

            var userChat = new UserChat { User = user, Chat = chat, Role = role};
            _context.UserChats.Add(userChat);
            chat.Updated = DateTime.Now;
        }

        public void DeleteUserById(int userId, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono chatu");
            var userChat = _context.UserChats.FirstOrDefault(u => u.User == user && u.Chat == chat) ?? throw new NotFoundException("Nie znaleziono powiazanych rekordow");
            _context.UserChats.Remove(userChat);
            chat.Updated = DateTime.Now;
        }

        public IEnumerable<User> GetUsersInChat(int chatId)
        {
            var chat = _context.Chats
                .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .FirstOrDefault(c => c.Id == chatId) ?? throw new NotUniqueElementException("Czat o podanej nazwie nie istnieje");
            var users = chat.UserChats.Select(uc => uc.User);
            return users;
        }

        public void CreateChat(string name, int userId)
        {
            if (_context.Chats.Any(c => c.Name == name))
            {
                throw new Exception("Chat o takiej nazwie juz istnieje");
            }

            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("Nie znaleziono uzytkownika");

            Role role = _context.Roles.SingleOrDefault(r => r.Id == 1) ?? throw new NotFoundException("Nie znaleziono roli");

            Chat chat = new Chat
            {
                Name = name,
                Created = DateTime.Now
            };

            UserChat userChat = new UserChat
            {
                Chat = chat,
                User = user,
                Role = role,
            };
            _context.Chats.Add(chat);
            _context.UserChats.Add(userChat);
        }

        public void DeleteChat(int chatId)
        {
            var userChats = _context.UserChats.Where(uc => uc.Chat.Id == chatId);

            if (userChats != null)
            {
                _context.UserChats.RemoveRange(userChats);
            }

            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono czatu");

            _context.Chats.Remove(chat);

        }

        public Chat GetChat(int chatId, int pageNumber,int messagesPerPage)
        {
            if (pageNumber < 1)
            {
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");
            }

            var messageCount = _context.Chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.Messages).Count();

            int maxPageNumber = (int)Math.Ceiling((double)messageCount / messagesPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chat = _context.Chats
                .Where(c => c.Id == chatId)
                .Include(c => c.Messages.OrderByDescending(m => m.TimeStamp).Skip((pageNumber - 1) * messagesPerPage).Take(messagesPerPage))
                .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .SingleOrDefault() ?? throw new NotFoundException("Nie znaleziono czatu");

            return chat;
        }

        public int GetChatMessagesCount(int chatId)
        {
            return _context.Chats.Where(c => c.Id == chatId)
                .SelectMany(c => c.Messages).Count();
        }

        public void AssignRole(int userId, int chatId, int roleId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("Nie znaleziono uzytkownika");

            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono czatu");

            Role role = _context.Roles.SingleOrDefault(r => r.Id == roleId) ?? throw new NotFoundException("Nie znaleziono roli");

            if (_context.UserChats.Any(u => u.User == user && u.Role == role && u.Chat == chat))
            {
                throw new IllegalOperationException("Uzytkownik ma juz ta role");
            }

            UserChat userChat = _context.UserChats.SingleOrDefault(uc => uc.User == user && uc.Chat == chat) ?? throw new NotFoundException("Nie znaleziono relacji");

            userChat.Role = role;
            userChat.RoleId = role.Id;

            _context.Attach(userChat);
            _context.Entry(userChat).State = EntityState.Modified;
        }

        public void RevokeRole(int userId, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new NotFoundException("Nie znaleziono uzytkownika");

            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono czatu");
            Role role = _context.Roles.SingleOrDefault(r => r.Id == 2) ?? throw new NotFoundException("Nie znaleziono roli");

            if (_context.UserChats.Any(u => u.User == user && u.Chat == chat))
            {
                throw new IllegalOperationException("Uzytkownik nie ma juz tej roli");
            }

            UserChat userChat = _context.UserChats.SingleOrDefault(uc => uc.User == user && uc.Chat == chat) ?? throw new NotFoundException("Nie znaleziono relacji");

            userChat.Role = role;
            userChat.RoleId = role.Id;

            _context.Attach(userChat);
            _context.Entry(userChat).State = EntityState.Modified;
        }

        public Role GetUserRole(int userId, int chatId)
        {
	        var role = _context.UserChats
		        .Where(uc => uc.UserId == userId && uc.ChatId == chatId)
		        .Select(uc => uc.Role)
		        .SingleOrDefault() ?? throw new NotFoundException("Nie znaleziono roli użytkownika");
	        return role;
        }

	}

}
