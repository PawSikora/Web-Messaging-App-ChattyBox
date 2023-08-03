using DAL.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Database
{
    public class DbChattyBox : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<FileMessage> FileMessages { get; set; }

        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<Role> Roles { get; set; }

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

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<Chat>()
                .ToTable("Chats");

            modelBuilder.Entity<Chat>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .ToTable("Roles");

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<UserChat>()
                .HasKey(uc => new { uc.UserId, uc.ChatId });

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UserChats)
                .HasForeignKey(uc => uc.ChatId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Role)
                .WithMany(r => r.UserChats)
                .HasForeignKey(uc => uc.RoleId);

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
                .ToTable("FileMessage", tb => tb.Property(e => e.Id)
                    .UseIdentityColumn(2, 2));

            modelBuilder.Entity<TextMessage>()
                .ToTable("TextMessages", tb => tb.Property(e => e.Id)
                    .UseIdentityColumn(1, 2));


            var user1 = createPasswordHash("123");
            var user2 = createPasswordHash("1234");

            List<UserChat> initUserChats = new List<UserChat>()
            {
                new UserChat { UserId = 1, ChatId = 1, RoleId = 1},
                new UserChat { UserId = 2, ChatId = 1, RoleId = 2}
            };

            modelBuilder.Entity<UserChat>().HasData(initUserChats);

            List<Role> initRoles = new List<Role>()
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            };

            modelBuilder.Entity<Role>().HasData(initRoles);

            List<User> initUsers = new List<User>()
            {
                new User { Id=1, Email = "marcinq@gmail.com", Username="MarIwin", Created = DateTime.Now, PasswordHash = user1.passwordHash, PasswordSalt = user1.passwordSalt, TokenCreated = DateTime.Now, TokenExpires = DateTime.Now.AddDays(1), RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))},
                new User { Id = 2, Email = "tymonq@gmail.com", Username = "TymonSme", Created = DateTime.Now, PasswordHash = user2.passwordHash, PasswordSalt = user2.passwordSalt, TokenCreated = DateTime.Now, TokenExpires = DateTime.Now.AddDays(1), RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)) }
            };

            modelBuilder.Entity<User>().HasData(initUsers.ToArray());

            List<TextMessage> initTextMessages = new List<TextMessage>()
            {
                 new TextMessage {Id = 1,TimeStamp = new DateTime(2019, 1, 1), SenderId = 1, ChatId = 1, Content = "Hello1" },
                 new TextMessage { Id = 3, TimeStamp = new DateTime(2021, 1, 1), SenderId = 2, ChatId = 1, Content = "Hello2" }
            };

            modelBuilder.Entity<TextMessage>().HasData(initTextMessages);

            List<FileMessage> initFileMessages = new List<FileMessage>()
            {
                new FileMessage { Id = 2, TimeStamp = new DateTime(2020, 1, 1), SenderId = 1, ChatId = 1, Name = "stockImage.jpg", Path = "files\\Chat1\\stockImage.jpg", Size = 0.09969329833984375},
                new FileMessage { Id = 4, TimeStamp = new DateTime(2022, 1, 1), SenderId = 2, ChatId = 1, Name = "stockGif.gif", Path = "files\\Chat1\\stockGif.gif", Size = 5.467991828918457},
            };

            modelBuilder.Entity<FileMessage>().HasData(initFileMessages);

            List<Chat> initChats = new List<Chat>()
            {
                new Chat { Created = DateTime.Now, Name = "Chat1", Id = 1 }
            };
          
            modelBuilder.Entity<Chat>().HasData(initChats);

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            string relativePath;
            string fullPath;

            foreach (var chat in initChats)
            {
                relativePath = Path.Combine("files", chat.Name);
                fullPath = Path.Combine(wwwrootPath, relativePath);

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
            }
        }

    }
}
