using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            chat.Updated = DateTime.Now;
            _context.Users.Add(user);

        }

        public IEnumerable<User> SortByName(string chatName)
        {
            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            return chat.Users.OrderBy(u => u.Username);
        }

        public Chat CreateChat(string name, User user)
        {
            Chat chat = new Chat
            {
                Name = name,
                Created = DateTime.Now
            };

            _context.Chats.Add(chat);
            return chat;

        }

        public void DeleteChat(string name)
        {
            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == name) ?? throw new Exception("Blad usuwania czatu");
            _context.Chats.Remove(chat);
        }
    }
   
}
