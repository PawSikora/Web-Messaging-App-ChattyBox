using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using BLL.Services.UserService;
using DAL.Database.Entities;
using DAL.Repositories.ChatRepository;
using DAL.Repositories.FileMessageRepository;
using DAL.Repositories.RoleRepository;
using DAL.Repositories.TextMessageRepository;
using DAL.Repositories.UserRepository;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using UnitTests.BLL.FakeRepositories;
//private readonly Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
//private readonly Mock<IChatRepository> _mockChatRepo = new Mock<IChatRepository>();
//private readonly Mock<ITextMessageRepository> _mockTextRepo = new Mock<ITextMessageRepository>();
//private readonly Mock<IFileMessageRepository> _mockFileRepo = new Mock<IFileMessageRepository>();
//private readonly Mock<IRoleRepository> _mockRoleRepo = new Mock<IRoleRepository>();
//private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
//private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
//private readonly Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
//private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
namespace UnitTests.BLL
{
    public class UserServiceTest
    {



        [Fact]
        public void GetUserChatsCountFake_ShouldReturnCount_WhenUserHasChats()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            var userService = new UserService(unitOfWork, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            var user = new User { Id = 1, Email = "User1", UserChats = new List<UserChat>() };
            var chat = new Chat { Id = 1, Name = "Chat1" };
            var chat2 = new Chat { Id = 2, Name = "Chat2" };

            var userChat = new UserChat { Chat = chat, User = user };
            var userChat2 = new UserChat { Chat = chat2, User = user };

            userRepo.CreateUser(user);
            user.UserChats.Add(userChat);
            user.UserChats.Add(userChat2);

            // Act
            var count = userService.GetUserChatsCount(user.Id);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void GetUserFake_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            FakeUserRepository userRepo = new FakeUserRepository();
            FakeChatRepository chatRepo = new FakeChatRepository();
            FakeTextMessageRepository textRepo = new FakeTextMessageRepository();
            FakeFileMessageRepository fileRepo = new FakeFileMessageRepository();
            FakeRoleRepository roleRepo = new FakeRoleRepository();

            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });

            var user = new User { Id = 1, Email = "User1", UserChats = new List<UserChat>() };
            var chat = new Chat { Id = 1, Name = "Chat1" };
            var userChat = new UserChat { Chat = chat, User = user };
            userRepo.CreateUser(user);
            user.UserChats.Add(userChat);

            var unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);

            var userService = new UserService(unitOfWork, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act
            var userDto = userService.GetUser(1);

            // Assert
            Assert.Equal("User1", userDto.Email);
        }

        [Fact]
        public void GetUserMoq_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });
            _mockUnitOfWork.Setup(uow => uow.Users.GetById(1)).Returns(new User { Id = 1, Email = "User1" });
            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Equal("User1", userService.GetUser(1).Email);
        }

        [Fact]
        public void GetUser_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var userId = 1;
            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);
            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => userService.GetUser(userId));
        }

        [Fact]
        public void GetUserChatsCountMoq_ShouldReturnCount_WhenUserHasChats()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserChatsCount(1)).Returns(2);
            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Equal(2, userService.GetUserChatsCount(1));
        }

        [Fact]
        public void TGetUser_ShouldReturnUserDTOWithChatsCount_WhenUserExists()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);
            var user = new User { Id = 1, Username = "Test User" };
            var userDTO = new UserDTO { Id = 1, Username = "Test User", ChatsCount = 2 };

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(user.Id)).Returns(user);
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserChatsCount(user.Id)).Returns(2);
            _mockMapper.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);

            // Act
            var result = _userService.GetUser(user.Id);

            // Assert
            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Username, result.Username);
            Assert.Equal(userDTO.ChatsCount, result.ChatsCount);

            _mockUnitOfWork.Verify(uow => uow.Users.GetById(user.Id), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Users.GetUserChatsCount(user.Id), Times.Once);
        }

        [Fact]
        public void TestGetUser_ShouldThrowNotFoundException_WhenUserDoesNotExist_()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            var userId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => _userService.GetUser(userId));

            _mockUnitOfWork.Verify(uow => uow.Users.GetById(userId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Users.GetUserChatsCount(userId), Times.Never);
        }

        [Fact]
        public void GetChats_ShouldReturnChatList_WhenChatListIsNotNull()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var userId = 1;
            var pageNumber = 1;
            var chatsPerPage = 5;
            var chatCount = 5;

            _mockUnitOfWork.Setup(u => u.Users.GetUserChatsCount(userId)).Returns(chatCount);

            var chatList = new List<Chat> { new Chat { Id = 1, Name = "Chat1" }, new Chat { Id = 2, Name = "Chat2" } };
            _mockUnitOfWork.Setup(uow => uow.Chats.GetChatsForUser(userId, pageNumber, chatsPerPage)).Returns(chatList);
            var expectedDtoList = new List<GetUserChatDTO>
                { new GetUserChatDTO { Id = 1, Name = "Chat1" }, new GetUserChatDTO { Id = 2, Name = "Chat2" } };
            _mockMapper.Setup(m => m.Map<IEnumerable<GetUserChatDTO>>(chatList)).Returns(expectedDtoList);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act
            var result = userService.GetChats(userId, pageNumber, chatsPerPage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDtoList, result);
        }

        [Fact]
        public void GetChats_Throws_IllegalOperationException_WhenPageNumberIsLessThanOne()
        {
            //Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);
            var userId = 1;
            var pageNumber = 0;
            var chatsPerPage = 5;

            //Act + Assert
            Assert.Throws<IllegalOperationException>(() => userService.GetChats(userId, pageNumber, chatsPerPage));
        }

        [Fact]
        public void GetChats_Throws_NotFoundException_WhenUserHasNoChats()
        {
            //Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var userId = 1;
            var pageNumber = 1;
            var chatsPerPage = 5;
            var chatCount = 0;
            _mockUnitOfWork.Setup(u => u.Users.GetUserChatsCount(userId)).Returns(chatCount);

            var _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            //Act + Assert
            Assert.Throws<NotFoundException>(() => _userService.GetChats(userId, pageNumber, chatsPerPage));
        }

        [Fact]
        public void LoginUser_ShouldReturnToken_WhenLoginSuccessful()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var expectedPassword = "1234";
            byte[] passwordHash;
            byte[] passwordHashSalt;
            using var hmac = new HMACSHA512();
            passwordHashSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(expectedPassword));
            var loginUser = new LoginUserDTO { Email = "test@test.com", Password = "1234" };
            var user = new User { Email = "test@test.com", PasswordHash = passwordHash, PasswordSalt = passwordHashSalt, LastLog = DateTime.Now};

            _mockConfiguration.SetupGet(x => x["TokenSettings:SecurityKey"]).Returns("super secret key");
            _mockConfiguration.SetupGet(c => c["TokenSettings:Issuer"]).Returns("localhost");
            _mockConfiguration.SetupGet(c => c["TokenSettings:Audience"]).Returns("localhost");

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginUser.Email)).Returns(user);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                               _mockHttpContextAccessor.Object);

            // Act
            var result = userService.LoginUser(loginUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TokenToReturn>(result);
        }
        
        [Fact]
        public void LoginUser_Throws_NotFoundException_WhenUserNotFound()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var loginUser = new LoginUserDTO { Email = "test@test.com", Password = "1234" };
            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginUser.Email)).Returns((User)null);
            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => userService.LoginUser(loginUser));
        }

        [Fact]
        public void LoginUser_Throws_LoginFailedException_WhenPasswordIncorrect()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var expectedPassword = "1234";
            byte[] passwordHash;
            byte[] passwordHashSalt;

            using var hmac = new HMACSHA512();
            passwordHashSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(expectedPassword));

            var loginUser = new LoginUserDTO { Email = "test@test.com", Password = "1111" };
            var user = new User { Email = "test@test.com", PasswordHash = passwordHash, PasswordSalt = passwordHashSalt};

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmail(loginUser.Email)).Returns(user);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                               _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Throws<LoginFailedException>(() => userService.LoginUser(loginUser));
        }

        [Fact]
        public void RegisterUser_Throws_EmailAlreadyUsedException_WhenEmailAlreadyTaken()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var createUser = new CreateUserDTO { Email = "test@test.com", Password = "1234" };

            _mockUnitOfWork.Setup(uow => uow.Users.IsEmailTaken(createUser.Email)).Returns(true);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                                              _mockHttpContextAccessor.Object);

            // Act + Assert
            Assert.Throws<EmailAlreadyUsedException>(() => userService.RegisterUser(createUser));
        }

        [Fact]
        public void RegisterUser_ShouldCreateUser_WhenEmailNotTaken()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var createUser = new CreateUserDTO { Email = "new@test.com", Password = "1234", Name = "test"};

            _mockUnitOfWork.Setup(uow => uow.Users.IsEmailTaken(createUser.Email)).Returns(false);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object, _mockHttpContextAccessor.Object);

            // Act
            userService.RegisterUser(createUser);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Users.CreateUser(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Fact]
        public void GetUserChatsCount_ReturnUserChatsCount()
        {
            // Arrange
            Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            Mock<IMapper> _mockMapper = new Mock<IMapper>();
            Mock<IHttpContextAccessor> _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
            var userId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetUserChatsCount(userId)).Returns(2);

            var userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object, _mockConfiguration.Object,
                                                             _mockHttpContextAccessor.Object);

            // Act
            var result = userService.GetUserChatsCount(userId);

            // Assert
            Assert.Equal(2, result);
        }
    }
}