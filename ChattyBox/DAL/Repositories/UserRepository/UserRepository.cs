using DAL.Database;
using DAL.Database.Entities;
using DAL.Exceptions;
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
            var user = _context.Users.SingleOrDefault(x => x.Email == email) ?? throw new LoginFailedException("Niepoprawny login lub hasło");

            if (password == null ||
           !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new LoginFailedException("Niepoprawny login lub hasło");

            user.LastLog = DateTime.Now;
            
            return user;
        }

        public void RegisterUser(string email, string username, string password)
        {
            if (_context.Users.Any(x => x.Email == email))
                throw new EmailAlreadyUsedException("Użytkownik o podanym adresie email już istnieje");

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
                .SingleOrDefault(i => i.Id == id) ?? throw new NotFoundException("Nie znaleziono uzytkownika");
            return user;
        }

        public ICollection<Chat> GetChats(int id, int pageNumber,int chatsPerPage)
        {
            if (pageNumber < 1)
            {
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");
            }
            
            
            
            var chatCount = _context.UserChats
                .Where(x => x.UserId == id)
                .Select(x => x.Chat)
                .Count();

            if (chatCount == 0)
            {
                throw new NotFoundException("Nie znaleziono czatów");
            }

            int maxPageNumber = (int)Math.Ceiling((double)chatCount / chatsPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chatList = _context.UserChats
                .Where(uc => uc.UserId == id)
                .Select(uc => uc.Chat)
                .OrderByDescending(c => c.Updated)
                .Skip((pageNumber - 1) * chatsPerPage)
                .Take(chatsPerPage)
                .ToList() ?? throw new NotFoundException("Nie znaleziono czatów");

            return chatList;
        }

        public int GetUserChatsCount(int id)
        {
            var chatCount = _context.UserChats
                .Where(x => x.UserId == id)
                .Select(x => x.Chat)
                .Count();
            return chatCount;
        }
    }
}
