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

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public int GetUserChatsCount(int id)
        {
            return _context.UserChats
                .Where(x => x.UserId == id)
                .Select(x => x.Chat)
                .Count();
        }

        public User? GetById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }

        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(x => x.Email == email);
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
