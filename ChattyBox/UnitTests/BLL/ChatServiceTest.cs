using AutoMapper;
using DAL.Repositories.ChatRepository;
using DAL.Repositories.FileMessageRepository;
using DAL.Repositories.RoleRepository;
using DAL.Repositories.TextMessageRepository;
using DAL.Repositories.UserRepository;
using DAL.UnitOfWork;
using Moq;
using UnitTests.BLL.FakeRepositories;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using DAL.Database.Entities;

namespace UnitTests.BLL
{
    public class ChatServiceTest
    {

        private readonly Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
        private readonly Mock<IChatRepository> _mockChatRepo = new Mock<IChatRepository>();
        private readonly Mock<ITextMessageRepository> _mockTextRepo = new Mock<ITextMessageRepository>();
        private readonly Mock<IFileMessageRepository> _mockFileRepo = new Mock<IFileMessageRepository>();
        private readonly Mock<IRoleRepository> _mockRoleRepo = new Mock<IRoleRepository>();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        [Fact]
        public void AddUserById_ShouldReturnTrue_WhenUserIsInChat()
        {

            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();
            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Email = "User1" };

            var chat = new Chat { Id = 1, Name = "Chat1" ,UserChats = new List<UserChat>{new UserChat{User = chatCreator}} };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Email = "User1" };
            userRepo.CreateUser(user);
            var role = new Role{Id = 1,Name = "User" };
            roleRepo.CreateRole(role);


            //Act
            chatService.AddUserById(user.Id, chat.Id);

            //Assert
            Assert.Equal(1,unitOfWork.Chats.GetUsersInChat(1).Count());
        }

        [Fact]
        public void CrateChat_ShouldReturnTrue_WhenChatExist()
        {
            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);
            _mockMapper.Setup(x => x.Map<Chat>(It.IsAny<CreateChatDTO>()))
                .Returns((CreateChatDTO src) => new Chat { Created = DateTime.Now ,Name = "Chat1",Id = 1});
            
            var user = new User { Id = 1, Email = "User1" };
            userRepo.CreateUser(user);
            var role = new Role { Id = 1, Name = "Admin" };
            roleRepo.CreateRole(role);
            var createChatDTO = new CreateChatDTO { Name = "Chat1", UserId = user.Id };

        

            //Act
                    chatService.CreateChat(createChatDTO);

            //Assert
            Assert.NotNull(unitOfWork.Chats.GetById(1));
        }
        [Fact]
        public void DeleteChat_ShouldReturnTrue_WhenChatDoesentExist()
        {           
            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);
            var user = new User { Id = 1, Email = "User1" };
            userRepo.CreateUser(user);
            var chat= new Chat { Name = "Chat1", Id = 1 };
            var role = new Role { Id = 1, Name = "Admin" };
            roleRepo.CreateRole(role);
            chatRepo.CreateChat(chat);
            //Act
            chatService.DeleteChat(1);

            //Assert
            Assert.Null(unitOfWork.Chats.GetById(1));

        }

        [Fact]
        public void DeleteUserById_ShouldReturnTrue_WhenUserIsNotInChat()
        {
            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Email = "User1" };

            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { new UserChat { User = chatCreator } } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Email = "User1" };
            userRepo.CreateUser(user);
            var role = new Role { Id = 1, Name = "User" };
            roleRepo.CreateRole(role);
            chatService.AddUserById(user.Id, chat.Id);


            //Act
            chatService.DeleteUserById(user.Id, chat.Id);
            
            //Assert
            Assert.False(unitOfWork.Chats.IsUserInChat(1,1));



        }

        [Fact]
        public void GetChatMessagesCount_ShouldReturnTrue_WhenCountIsCorrect()
        {

            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);
            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var chat = new Chat { Id = 1, Name = "Chat1",Messages = new List<Message>{new TextMessage{Content = "DummyText"}}};
            chatRepo.CreateChat(chat);

            //Act
            var count = chatService.GetChatMessagesCount(chat.Id);

            //Assert
            Assert.Equal(1, count);

        }

        [Fact]
        public void GetUsersInChat_ShouldReturnTrue_WhenChatHasUsers()
        {

            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Email = "User1" };

            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { new UserChat { User = chatCreator } } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Email = "User1" };
            userRepo.CreateUser(user);
            var role = new Role { Id = 1, Name = "User" };
            roleRepo.CreateRole(role);
            chatService.AddUserById(user.Id, chat.Id);


            //Act
             var users = chatService.GetUsersInChat(chat.Id);
            
            //Assert
             Assert.NotNull(users);

        }

        [Fact]
        public void AssingRole_ShouldReturnTrue_WhenUserHasRole()
        {

            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1,Username = "User1" };
            
           
            var userChat = new UserChat
            {
                User = chatCreator,
                UserId =chatCreator.Id,
                Role = new Role { Id = 2, Name = "User" },
                ChatId = 1,

            };
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { userChat } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var roleAdmin = new Role { Id = 1, Name = "Admin" };
            roleRepo.CreateRole(roleAdmin);
            var roleUser = new Role { Id = 2, Name = "User" };
            roleRepo.CreateRole(roleUser);
            chatRepo.AddUserToChat(userChat);
            

            //Act
            chatService.AssignRole(userChat.UserId,userChat.ChatId,roleAdmin.Id);

            //Assert
            Assert.True(unitOfWork.Chats.IsUserRole(userChat.UserId, userChat.ChatId, roleAdmin.Id));

        }

        [Fact]
        public void RevokeRole_ShouldReturnTrue_WhenUserHasNoRole()
        {
            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Username = "User1" };


            var userChat = new UserChat
            {
                User = chatCreator,
                UserId = chatCreator.Id,
                Role = new Role { Id = 1, Name = "Admin" },
                RoleId = 1,
                ChatId = 1,

            };
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { userChat } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var roleUser = new Role { Id = 2, Name = "User" };
            roleRepo.CreateRole(roleUser);
            var roleAdmin = new Role { Id = 1, Name = "Admin" };
            roleRepo.CreateRole(roleAdmin);
            chatRepo.AddUserToChat(userChat);


            //Act
            chatService.RevokeRole(user.Id,chat.Id);

            //Assert
            Assert.True(unitOfWork.Chats.IsUserRole(user.Id,chat.Id,2));
        }

        [Fact]
        public void GetUserRole_ShouldReturnTrue_WhenRoleNameIsCorrect()
        {

            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Username = "User1" };


            var userChat = new UserChat
            {
                User = chatCreator,
                UserId = chatCreator.Id,
                Role = new Role { Id = 1, Name = "Admin" },
                RoleId = 1,
                ChatId = 1,

            };
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { userChat } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var roleUser = new Role { Id = 2, Name = "User" };
            roleRepo.CreateRole(roleUser);
            var roleAdmin = new Role { Id = 1, Name = "Admin" };
            roleRepo.CreateRole(roleAdmin);
            chatRepo.AddUserToChat(userChat);


            //Act
            var role =chatService.GetUserRole(user.Id, chat.Id);

            //Assert
            Assert.Equal("Admin",role);

        }

        [Fact]
        public void GetUserRole_ShouldReturnTrue_WhenUserEmailIsCorrect()
        {
            // Arrange
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var chatCreator = new User { Id = 1, Username = "User1",Email = "DummyEmail"};


            var userChat = new UserChat
            {
                User = chatCreator,
                UserId = chatCreator.Id,
                Role = new Role { Id = 1, Name = "Admin" },
                RoleId = 1,
                ChatId = 1,

            };
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { userChat } };
            chatRepo.CreateChat(chat);
            var user = new User { Id = 1, Username = "User1",Email = "DummyEmail" };
            userRepo.CreateUser(user);
            chatRepo.AddUserToChat(userChat);
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });



            //Act
            var userByEmail = chatService.GetUserByEmail("DummyEmail");

            //Assert
            Assert.Equal("DummyEmail", userByEmail.Email);


        }

    }
}
