using DAL.Database;
using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.UserRepository
{
    public class UserRepository :IUserRepository
    {
        private readonly DbChattyBox _context;
        public UserRepository(DbChattyBox context) 
        {
            _context = context;
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }


        public User LoginUser(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email) ?? throw new Exception("Nie znaleziono uzytkownika");

            if (password == null ||
           !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Niepoprawny login lub hasło");

            return user;
        }

        public void RegisterUser(string email, string username, string password)
        {
            if (_context.Users.Any(x => x.Email == email))
                throw new Exception("Użytkownik o podanym adresie email już istnieje");

            createPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Created = DateTime.Now
            };

            _context.Users.Add(user);
        }

        public User GetUser(int id)
        {
            var user = _context.Users
                .SingleOrDefault(i => i.Id == id) ?? throw new Exception("Nie znaleziono uzytkownika");
            return user;
        }

        public ICollection<Chat> GetChats(int id)
        {
            //var user = _context.Users
            //    .Where(i => i.Id == id)
            //    .Include(u => u.UserChats)
            //    .ThenInclude(uc => uc.Chat)
            //    .SingleOrDefault() ?? throw new Exception(" Nie znaleziono uzytkownika");
            //var chats = user.UserChats.ToList() ?? throw new Exception("uzytkownik nie ma czatu");


            //List<Chat> chatList = new List<Chat>();
            //foreach (Chat chat in chats)
            //{
            //    chatList.Add(chat);
            //}

            //return chatList;






            var user = _context.Users
                .Include(u => u.UserChats)
                .ThenInclude(uc => uc.Chat)
                .SingleOrDefault(i => i.Id == id) ?? throw new Exception("Brak użytkownika");

            var chatList = user.UserChats
                .Select(uc => uc.Chat)
                .ToList();

            return chatList;


        }
    }
}
