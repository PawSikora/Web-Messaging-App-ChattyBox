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

namespace DAL.Repositories.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly DbChattyBox _context;
        public ChatRepository(DbChattyBox context)
        {
            _context = context;
        }

        public void AddUserByEmail(string email,string chatName) 
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == email) ?? throw new Exception("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.Include(c=>c.Users).SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            if (chat.Users.Contains(user))
            {
                throw new Exception("Uzytkownik jest juz w czacie");
            }
            chat.Users.Add(user);
            user.Chats.Add(chat);
            chat.Updated = DateTime.Now;
        }

        public void DeleteUserByEmail(string email, string chatName)
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == email) ?? throw new Exception("Nie znaleziono uzytkownika");
            Chat chat = _context.Chats.Include(c => c.Users).SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            if (!chat.Users.Contains(user))
            {
                throw new Exception("Uzytkownik nie jest w czacie");
            }
            chat.Users.Remove(user);
            user.Chats.Remove(chat);
            chat.Updated = DateTime.Now;
        }


        public IEnumerable<User> SortByName(string chatName)
        {
            Chat chat = _context.Chats.Include(c=>c.Users).SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            return chat.Users.OrderBy(u => u.Username);
        }

        public Chat CreateChat(string name, User user)
        {
            if (_context.Chats.Any(c => c.Name == name))
            {
                throw new Exception("Chat o takiej nazwie juz istnieje");
            }

            if (user == null)
            {
                throw new Exception("Nie znaleziono uzytkownika");
            }

            Chat chat = new Chat
            {
                Name = name,
                Created = DateTime.Now
            };
            chat.Users.Add(user);
            user.Chats.Add(chat);
            _context.Chats.Add(chat);
            return chat;
        }

        public void DeleteChat(string name)
        {
            Chat chat = _context.Chats.Include(c=>c.Users).SingleOrDefault(c => c.Name == name) ?? throw new Exception("Blad usuwania czatu");
            
            foreach(var user in chat.Users)
            {
                user.Chats.Remove(chat);
            }
            
            _context.Chats.Remove(chat);
        }
    }
   
}
