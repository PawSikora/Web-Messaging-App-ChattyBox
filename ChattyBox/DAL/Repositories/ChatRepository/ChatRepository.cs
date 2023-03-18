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
            chat.Users.Add(user);

        }

        public void AddMessage(Message message,string chatName) 
        {
            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            chat.Updated = DateTime.Now;
            chat.Messages.Add(message);

        }

        public IEnumerable<User> SortByName(string chatName)
        {
            IEnumerable<User> sortedUsers = _context.Chats.SingleOrDefault(c => c.Name == chatName).Users.OrderBy(u => u.Username).ToList() ?? throw new Exception("Nie znaleziono chatu");
            return sortedUsers;

        }

        public void RemoveUser(string email, string chatName) 
        {
            try
            {
                _context.Chats.SingleOrDefault(c => c.Name == chatName).Users
                    .Remove(_context.Users.SingleOrDefault(u => u.Email == email));

            }
            catch
            {
                throw new Exception("Blad usuwania uzytkownia z czatu ");
            }
            _context.Chats.SingleOrDefault(c => c.Name == chatName).Updated=DateTime.Now;
            

        }

        public void RemoveMessage(Message message, string chatName) 
        {
            try
            {
                _context.Chats.SingleOrDefault(c => c.Name == chatName).Messages.Remove(message);
            }
            catch
            {
                throw new Exception("Blad usuwania wiadomosci z czatu ");
            }
            _context.Chats.SingleOrDefault(c => c.Name == chatName).Updated = DateTime.Now;
        }

        public Chat CreateChat(string name, User user)
        {
            Chat chat = new Chat
            {
                Name = name,
                Users = new List<User>{user},
                Updated = DateTime.Now


            };
            return chat;

        }

        public void DeleteChat(string name)
        {
            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == name)?? throw new Exception("blad usuwania czatu");
            _context.Chats.Remove(chat);
        }
    }
   
}
