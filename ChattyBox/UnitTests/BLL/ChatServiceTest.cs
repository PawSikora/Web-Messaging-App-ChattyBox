using AutoMapper;
using DAL.UnitOfWork;
using Moq;
using UnitTests.BLL.FakeRepositories;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using BLL.Services.ChatService;
using DAL.Database.Entities;

namespace UnitTests.BLL
{
    public class ChatServiceTest
    {

        [Fact]
        public void AddUserById_ShouldReturnTrue_WhenUserIsInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>(); 
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
            Assert.Equal(1,unitOfWork.Chats.GetUsersInChat(1, 1, 5).Count());
        }

        [Fact]
        public void AddUserById_Throws_NotFoundException_WhenNoUserFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;
            int userId = 1;

            _mockUnitOfWork.Setup(x => x.Users.GetById(userId)).Returns((User)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            //Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AddUserById(userId, chatId));
        }

        [Fact]
        public void AddUserById_Throws_NotFoundException_WhenNoChatFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;
            int userId = 1;

            _mockUnitOfWork.Setup(x => x.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(x => x.Chats.GetById(chatId)).Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            //Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AddUserById(userId, chatId));
        }

        [Fact]
        public void AddUserById_Throws_NotFoundException_WhenNoRoleFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;
            int userId = 1;

            _mockUnitOfWork.Setup(x => x.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(x => x.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(x => x.Roles.GetByName("User")).Returns((Role)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            //Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AddUserById(userId, chatId));
        }

        [Fact]
        public void AddUserById_Throws_IllegalOperationException_WhenUserIsInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;
            int userId = 1;

            _mockUnitOfWork.Setup(x => x.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(x => x.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(x => x.Roles.GetByName("User")).Returns(new Role());
            _mockUnitOfWork.Setup(x => x.Chats.IsUserInChat(userId, chatId)).Returns(true);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            //Act + Assert
            Assert.Throws<IllegalOperationException>(() => chatService.AddUserById(userId, chatId));
        }

        [Fact]
        public void CreateChat_ShouldReturnTrue_WhenChatExist()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            _mockMapper.Setup(x => x.Map<Chat>(It.IsAny<CreateChatDTO>()))
                .Returns((CreateChatDTO src) => new Chat { Created = DateTime.Now, Name = "Chat1", Id = 1 });
            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

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
        public void CreateChat_Throws_NotUniqueElementException_WhenChatNameTaken()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatDTO = new CreateChatDTO { Name = "Chat1"};

            _mockUnitOfWork.Setup(uow => uow.Chats.IsChatNameTaken(chatDTO.Name)).Returns(true);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            //Act + Assert
            Assert.Throws<NotUniqueElementException>(() => chatService.CreateChat(chatDTO));
        }

        [Fact]
        public void CreateChat_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatDTO = new CreateChatDTO { Name = "Chat1", UserId = 1 };

            _mockUnitOfWork.Setup(uow => uow.Chats.IsChatNameTaken(chatDTO.Name)).Returns(false);
            _mockUnitOfWork.Setup(uow => uow.Users.GetById(chatDTO.UserId)).Returns((User)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.CreateChat(chatDTO));
        }

        [Fact]
        public void CreateChat_Throws_NotFoundException_WhenRoleNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatDTO = new CreateChatDTO { Name = "Chat1", UserId = 1 };

            _mockUnitOfWork.Setup(uow => uow.Chats.IsChatNameTaken(chatDTO.Name)).Returns(false);
            _mockUnitOfWork.Setup(uow => uow.Users.GetById(chatDTO.UserId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetByName("Admin")).Returns((Role)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.CreateChat(chatDTO));
        }

        [Fact]
        public void DeleteChat_ShouldReturnTrue_WhenChatDoesntExist()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relativePath = Path.Combine("files", chat.Name);
            var fullPath = Path.Combine(wwwrootPath, relativePath);
            Directory.CreateDirectory(fullPath);

            // Act
            chatService.DeleteChat(1);

            // Assert
            Assert.Null(unitOfWork.Chats.GetById(1));
        }

        [Fact]
        public void DeleteChat_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(1)).Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);
            
            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.DeleteChat(1));
        }

        [Fact]
        public void DeleteChat_ShouldDeleteUsers_WhenChatUsersNotNull()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var chat = new Chat { Id = chatId, Name = "Chat1" };
            var chatUser = new User { Id = 1, Email = "User1" };
            var chatUsers = new List<UserChat> { new UserChat { User = chatUser, Chat = chat, ChatId = chatId, UserId = chatUser.Id} };

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relativePath = Path.Combine("files", chat.Name);
            var fullPath = Path.Combine(wwwrootPath, relativePath);
            Directory.CreateDirectory(fullPath);

            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(chat);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetChatUsersById(chatId)).Returns(chatUsers);
            _mockUnitOfWork.Setup(uow => uow.Chats.RemoveUsersFromChat(chatUsers));

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            chatService.DeleteChat(chatId);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Chats.RemoveUsersFromChat(chatUsers), Times.Once);
        }

        [Fact]
        public void DeleteUserById_ShouldReturnTrue_WhenUserIsNotInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            // Act
            chatService.DeleteUserById(user.Id, chat.Id);
            
            // Assert
            Assert.False(unitOfWork.Chats.IsUserInChat(1,1));
        }

        [Fact]
        public void DeleteUserById_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var userId = 1;
            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.DeleteUserById(userId, chatId));
        }

        [Fact]
        public void DeleteUserById_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var userId = 1;
            var user = new User { Id = userId, Email = "User1" };

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(user);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.DeleteUserById(userId, chatId));
        }

        [Fact]
        public void DeleteUserById_Throws_NotFoundException_WhenUserNotInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var userId = 1;
            var user = new User { Id = userId, Email = "User1" };
            var chat = new Chat { Id = chatId, Name = "Chat1" };

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(user);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(chat);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns((UserChat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.DeleteUserById(userId, chatId));
        }

        [Fact]
        public void GetChat_Throws_IllegalOperationException_WhenPageNumberIsLessThanOne()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var chatId = 1;
            var pageNumber = 0;
            var messagesPerPage = 10;

            var chat = new Chat();
            _mockUnitOfWork.Setup(uow => uow.Chats.GetChat(chatId, pageNumber, messagesPerPage))
                .Returns(chat);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act, Assert
            Assert.Throws<IllegalOperationException>(() => chatService.GetChat(chatId, pageNumber, messagesPerPage));
        }

        [Fact]
        public void GetChat_Throws_NotFoundException_WhenChatIsNull()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var chatId = 1;
            var pageNumber = 1;
            var messagesPerPage = 10;

            _mockUnitOfWork.Setup(uow => uow.Chats.GetChat(chatId, pageNumber, messagesPerPage))
                .Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act, Assert
            Assert.Throws<NotFoundException>(() => chatService.GetChat(chatId, pageNumber, messagesPerPage));
        }

        [Fact]
        public void GetChat_ReturnsGetChatDto_WhenChatIsFound()
        {
            // Arrange
            var _mapperMock = new Mock<IMapper>();
            var _unitOfWorkMock = new Mock<IUnitOfWork>();
            var chatId = 1;
            var pageNumber = 1;
            var messagesPerPage = 10;
            var chat = new Chat();
            var expectedDto = new GetChatDTO();

            _unitOfWorkMock.Setup(uow => uow.Chats.GetChatMessagesCount(chatId)).Returns(messagesPerPage);
            _unitOfWorkMock.Setup(uow => uow.Chats.GetChat(chatId, pageNumber, messagesPerPage))
                .Returns(chat);
            _mapperMock.Setup(mapper => mapper.Map<GetChatDTO>(chat)).Returns(expectedDto);

            var chatService = new ChatService(_unitOfWorkMock.Object, _mapperMock.Object);

            // Act
            var actualDto = chatService.GetChat(chatId, pageNumber, messagesPerPage);

            // Assert
            Assert.Equal(expectedDto, actualDto);
        }

        [Fact]
        public void GetChatMessagesCount_ShouldReturnTrue_WhenMessagesCountIsCorrect()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            // Act
            var count = chatService.GetChatMessagesCount(chat.Id);

            // Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void GetChatMessagesCount_ShouldReturnFalse_WhenChatDoesNotExist()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var chat = new Chat { Id = 1, Name = "Chat1" };
            chatRepo.CreateChat(chat);

            // Act
            var count = chatService.GetChatMessagesCount(2);

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void GetChatUsersCount_ShouldReturnTrue_WhenUsersCountIsCorrect()
        { 
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();
    
            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { new UserChat { User = user } } };
            chatRepo.CreateChat(chat);
    
            // Act
            var count = chatService.GetChatUsersCount(chat.Id);
    
            // Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void GetChatUsersCount_ShouldReturnFalse_WhenChatDoesNotExist()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var chatService = new ChatService(unitOfWork, _mockMapper.Object);

            var user = new User { Id = 1, Username = "User1" };
            userRepo.CreateUser(user);
            var chat = new Chat { Id = 1, Name = "Chat1", UserChats = new List<UserChat> { new UserChat { User = user } } };
            chatRepo.CreateChat(chat);

            // Act
            var count = chatService.GetChatUsersCount(2);

            // Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void GetUsersInChat_ShouldReturnTrue_WhenChatHasUsers()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            // Act
             var users = chatService.GetUsersInChat(chat.Id, 1, 5);
            
            // Assert
             Assert.NotNull(users);
        }

        [Fact]
        public void GetUsersInChat_Throws_IllegalOperationException_WhenPageNumberIsLessThanOne()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var pageNumber = 0;
            var usersPerPage = 10;

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<IllegalOperationException>(() => chatService.GetUsersInChat(chatId, pageNumber, usersPerPage));
        }

        [Fact]
        public void GetUsersInChat_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var chatId = 1;
            var pageNumber = 1;
            var usersPerPage = 10;

            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUsersInChat(chatId, pageNumber, usersPerPage));
        }

        [Fact]
        public void GetUsersInChat_Throws_NotFoundException_WhenUsersNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var chatId = 1;
            var pageNumber = 1;
            var usersPerPage = 10;
            var userCount = 0;
            var chat = new Chat();

            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(chat);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetChatUsersCount(chatId)).Returns(userCount);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUsersInChat(chatId, pageNumber, usersPerPage)).Returns((IEnumerable<User>)null);
            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUsersInChat(chatId, pageNumber, usersPerPage));
        }

        [Fact]
        public void AssignRole_ShouldReturnTrue_WhenUserHasRole()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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
            
            // Act
            chatService.AssignRole(userChat.UserId,userChat.ChatId,roleAdmin.Id);

            // Assert
            Assert.True(unitOfWork.Chats.IsUserRole(userChat.UserId, userChat.ChatId, roleAdmin.Id));
        }

        [Fact]
        public void AssignRole_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AssignRole(userId, chatId, roleId));
        }

        [Fact]
        public void AssignRole_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns((Chat)null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AssignRole(userId, chatId, roleId));
        }

        [Fact]
        public void AssignRole_Throws_NotFoundException_WhenRoleNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AssignRole(userId, chatId, roleId));
        }

        [Fact]
        public void AssignRole_Throws_NotFoundException_WhenUserNotFoundInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(new Role());
            _mockUnitOfWork.Setup(uow => uow.Chats.IsUserRole(userId, chatId, roleId)).Returns(false);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.AssignRole(userId, chatId, roleId));
        }

        [Fact]
        public void AssignRole_Throws_IllegalOperationException_WhenUserAlreadyHasRole()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(new Role());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(new UserChat());
            _mockUnitOfWork.Setup(uow => uow.Chats.IsUserRole(userId, chatId, roleId)).Returns(true);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<IllegalOperationException>(() => chatService.AssignRole(userId, chatId, roleId));
        }

        [Fact]
        public void RevokeRole_ShouldReturnTrue_WhenUserHasNoRole()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            // Act
            chatService.RevokeRole(user.Id,chat.Id);

            // Assert
            Assert.True(unitOfWork.Chats.IsUserRole(user.Id,chat.Id,2));
        }

        [Fact]
        public void RevokeRole_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.RevokeRole(userId, chatId));
        }

        [Fact]
        public void RevokeRole_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();    
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.RevokeRole(userId, chatId));
        }

        [Fact]
        public void RevokeRole_Throws_NotFoundException_WhenRoleNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.RevokeRole(userId, chatId));
        }

        [Fact]
        public void RevokeRole_Throws_NotFoundException_WhenUserNotFoundInChat()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetById(roleId)).Returns(new Role());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.RevokeRole(userId, chatId));
        }

        [Fact]
        public void RevokeRole_Throws_IllegalOperationException_WhenUserAlreadyHasRole()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;
            var roleId = 2;
            var userRole = new Role { Id = 2, Name = "User" };

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Roles.GetByName("User")).Returns(userRole);
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(new UserChat());
            _mockUnitOfWork.Setup(uow => uow.Chats.IsUserRole(userId, chatId, roleId)).Returns(true);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<IllegalOperationException>(() => chatService.RevokeRole(userId, chatId));
        }

        [Fact]
        public void GetUserRole_ShouldReturnTrue_WhenRoleNameIsCorrect()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
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

            // Act
            var role =chatService.GetUserRole(user.Id, chat.Id);

            // Assert
            Assert.Equal("Admin",role);
        }

        [Fact]
        public void GetUserRole_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUserRole(userId, chatId));
        }

        [Fact]
        public void GetUserRole_Throws_NotFoundException_WhenChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(() => null);
            
            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUserRole(userId, chatId));
        }

        [Fact]
        public void GetUserRole_Throws_NotFoundException_WhenUserChatNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUserRole(userId, chatId));
        }

        [Fact]
        public void GetUserRole_Throws_NotFoundException_WhenRoleNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var userId = 1;
            var chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns(new User());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetById(chatId)).Returns(new Chat());
            _mockUnitOfWork.Setup(uow => uow.Chats.GetUserChatById(userId, chatId)).Returns(new UserChat());
            _mockUnitOfWork.Setup(uow => uow.Chats.IsUserRole(userId, chatId, 1)).Returns(false);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUserRole(userId, chatId));
        }

        [Fact]
        public void GetAllUsers_ShouldReturnTrue_WhenUsersFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;
            var usersList = new List<User>
            {
                new User { Id = 1, Username = "User1" },
                new User { Id = 2, Username = "User2" },
                new User { Id = 3, Username = "User3" }
            };

            var expectedDtoList = new List<UserDTO>
            {
                new UserDTO { Id = 1, Username = "User1" },
                new UserDTO { Id = 2, Username = "User2" },
                new UserDTO { Id = 3, Username = "User3" }
            };

            _mockUnitOfWork.Setup(uow => uow.Chats.GetAllUsers(chatId)).Returns(usersList);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(usersList)).Returns(expectedDtoList);
            
            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = chatService.GetAllUsers(chatId);

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Equal(expectedDtoList, result);
        }

        [Fact]
        public void GetAllUsers_Throws_NotFoundException_WhenUsersNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            int chatId = 1;

            _mockUnitOfWork.Setup(uow => uow.Chats.GetAllUsers(chatId)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetAllUsers(chatId));
        }

        [Fact]
        public void GetUserByEmail_ShouldReturnTrue_WhenUserEmailIsCorrect()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });
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

            // Act
            var userByEmail = chatService.GetUserByEmail("DummyEmail");

            // Assert
            Assert.Equal("DummyEmail", userByEmail.Email);
        }

        [Fact]
        public void GetUserByEmail_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            var email = "DummyEmail";

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(email)).Returns(() => null);

            var chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => chatService.GetUserByEmail(email));
        }

    }
}
