using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            chat.Users.Add(user);

        }

        public void AddMessage(Message message,string chatName)
        {

            Chat chat = _context.Chats.SingleOrDefault(c => c.Name == chatName) ?? throw new Exception("Nie znaleziono chatu");
            chat.Messages.Add(message); // nie wiem po czym to filtrowac 

        }

        public List<User> SortByName(string chatName)
        {
            List<User> sortedUsers = _context.Chats.SingleOrDefault(c => c.Name == chatName).Users.OrderBy(u => u.Username).ToList() ?? throw new Exception("Nie znaleziono chatu");
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

            }
    }
   
}
