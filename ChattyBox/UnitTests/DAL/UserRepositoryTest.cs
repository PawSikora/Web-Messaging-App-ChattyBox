using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL.Database;
using DAL.Database.Entities;
using DAL.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.DAL
{
    public class UserRepositoryTest
    {
        [Fact]
        public void GetUserByEmail_ReturnsUser_WhenUserExists()
        {
            var options = new DbContextOptionsBuilder<DbChattyBox>()
                .UseInMemoryDatabase(databaseName: "TestChattyBox")
                .Options;

            var chattyBoxContext = new DbChattyBox(options);
            UserRepository userRepository = new UserRepository(chattyBoxContext);

            Assert.Null(userRepository.GetById(1));

            var user = new User() { Id = 1, Email = "User1", Username = "User"};

            using (var hmac = new HMACSHA512())
            { user.PasswordSalt = hmac.Key; user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("password123"));  }

            userRepository.CreateUser(user);

            userRepository.Save();

            Assert.NotNull(userRepository.GetById(1));
        }

    }
}
