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
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new Exception("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono chatu");
            var userChat = new UserChat { User = user, Chat = chat };
            _context.UserChats.Add(userChat);
            chat.Updated = DateTime.Now;
        }

        public void DeleteUserById(int userId, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new Exception("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono chatu");
            var userChat = _context.UserChats.FirstOrDefault(u => u.User == user && u.Chat == chat) ?? throw new Exception("Nie znaleziono powiazanych rekordow");
            _context.UserChats.Remove(userChat);
            chat.Updated = DateTime.Now;
        }


        public IEnumerable<User> GetUsersInChat(int chatId)
        {
            var chat = _context.Chats
                .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .FirstOrDefault(c => c.Id == chatId) ?? throw new Exception("Czat o podanej nazwie nie istnieje");
            var users = chat.UserChats.Select(uc => uc.User);
            return users;
        }

        public void CreateChat(string name, int userId)
        {
            if (_context.Chats.Any(c => c.Name == name))
            {
                throw new Exception("Chat o takiej nazwie juz istnieje");
            }

            User user = _context.Users.SingleOrDefault(u => u.Id == userId) ?? throw new Exception("Nie znaleziono uzytkownika");

            Chat chat = new Chat
            {
                Name = name,
                Created = DateTime.Now
            };

            UserChat userChat = new UserChat
            {
                Chat = chat,
                User = user
            };
            _context.Chats.Add(chat);
            _context.UserChats.Add(userChat);
        }

        public void DeleteChat(int chatId)
        {
            var userChats = _context.UserChats.Where(uc => uc.Chat.Id == chatId);

            if(userChats != null)
            {
                _context.UserChats.RemoveRange(userChats);
            }
           
            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono czatu");

            _context.Chats.Remove(chat);
           
        }

        public Chat GetChat(int chatId)
        {
            var chat = _context.Chats
                .Include(c => c.Messages)
                .Include(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .FirstOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono czatu");
 
            return chat;
        }

    }
   
}
