using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
    public class DbChattyBox : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<FileMessage> FileMessages { get; set; }

        public DbChattyBox(DbContextOptions<DbChattyBox> options) : base(options)
        {
           
        }

        private (byte[] passwordSalt, byte[] passwordHash) createPasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            return (hmac.Key, hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("Users");


            modelBuilder.Entity<Chat>()
                .ToTable("Chats");
            
      
            modelBuilder.Entity<Chat>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Chats)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("UserChat"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);


            modelBuilder.Entity<Message>()
                .UseTpcMappingStrategy();

            
            modelBuilder.Entity<FileMessage>()
               .ToTable("FileMessages");
            
            modelBuilder.Entity<TextMessage>()
                .ToTable("TextMessages");

            var user1 = createPasswordHash("123");
            var user2 = createPasswordHash("1234");
            
            List<User> initUsers = new List<User>() 
            { new User { Id=1, Email = "marcinq@gmail.com", Username="MarIwin", Created = DateTime.Now, PasswordHash = user1.passwordHash, PasswordSalt = user1.passwordSalt },
              new User { Id = 2, Email = "tymonq@gmail.com", Username = "TymonSme", Created = DateTime.Now, PasswordHash = user2.passwordHash, PasswordSalt = user2.passwordSalt } 
            };

            modelBuilder.Entity<User>().HasData(

             initUsers[0],
             initUsers[1]
            );

            List<TextMessage> initTextMessages = new List<TextMessage>()
            {
                 new TextMessage {Id=1,TimeStamp = new DateTime(2020, 1, 1), SenderId = 1, ChatId = 1, Content = "Hello1" },
                 new TextMessage { Id = 2, TimeStamp = new DateTime(2020, 1, 1), SenderId = 2, ChatId = 1, Content = "Hello2" }
            };

            modelBuilder.Entity<TextMessage>().HasData(
                initTextMessages[0],
                initTextMessages[1]
            );


            modelBuilder.Entity<Chat>().HasData(
                 new Chat { Created = DateTime.Now, Name = "Chat1", Id = 1 }
            );

        }
               
    }
}
