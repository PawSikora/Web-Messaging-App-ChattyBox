using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
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
using Moq;
using UnitTests.BLL.FakeRepositories;

namespace UnitTests.BLL
{
    public class UserServiceTest
    {
        private readonly FakeUserRepository _userRepo = new FakeUserRepository();
        private readonly FakeChatRepository _chatRepo = new FakeChatRepository();
        private readonly FakeTextMessageRepository _textRepo = new FakeTextMessageRepository();
        private readonly FakeFileMessageRepository _fileRepo = new FakeFileMessageRepository();
        private readonly FakeRoleRepository _roleRepo = new FakeRoleRepository();
      
        private readonly Mock<IUserRepository> _mockUserRepo = new Mock<IUserRepository>();
        private readonly Mock<IChatRepository> _mockChatRepo = new Mock<IChatRepository>();
        private readonly Mock<ITextMessageRepository> _mockTextRepo = new Mock<ITextMessageRepository>();
        private readonly Mock<IFileMessageRepository> _mockFileRepo = new Mock<IFileMessageRepository>();
        private readonly Mock<IRoleRepository> _mockRoleRepo = new Mock<IRoleRepository>();
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        [Fact]
        public void GetUserChatsCountFake_ShouldReturnCount_WhenUserHasChats()
        {
            // Arrange
            var unitOfWork = new UnitOfWork(_chatRepo, _fileRepo, _textRepo, _userRepo, _roleRepo);
            var userService = new UserService(unitOfWork, _mockMapper.Object);

            var user = new User { Id = 1, Email = "User1", UserChats = new List<UserChat>()};
            var chat = new Chat { Id = 1, Name = "Chat1" };
            var chat2 = new Chat { Id = 2, Name = "Chat2" };

            var userChat = new UserChat { Chat = chat, User = user };
            var userChat2 = new UserChat { Chat = chat2, User = user };

            _userRepo.CreateUser(user);
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
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });

            var user = new User { Id = 1, Email = "User1", UserChats = new List<UserChat>() };
            var chat = new Chat { Id = 1, Name = "Chat1" };
            var userChat = new UserChat { Chat = chat, User = user };
            _userRepo.CreateUser(user);
            user.UserChats.Add(userChat);

            var unitOfWork = new UnitOfWork(_chatRepo, _fileRepo, _textRepo, _userRepo, _roleRepo);

            var userService = new UserService(unitOfWork, _mockMapper.Object);

            // Act
            var userDto = userService.GetUser(1);

            // Assert
            Assert.Equal("User1", userDto.Email);
        }

        [Fact]
        public void GetUserMoq_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });
            _mockUserRepo.Setup(repo => repo.GetById(1)).Returns(new User { Id = 1, Email = "User1" });

            var unitOfWork = new UnitOfWork(_mockChatRepo.Object, _mockFileRepo.Object, _mockTextRepo.Object, _mockUserRepo.Object, _mockRoleRepo.Object);
            var userService = new UserService(unitOfWork, _mockMapper.Object);

            // Act + Assert
            Assert.Equal("User1", userService.GetUser(1).Email);
        }

        [Fact]
        public void GetUserChatsCountMoq_ShouldReturnCount_WhenUserHasChats()
        {
            // Arrange
            _mockUserRepo.Setup(repo => repo.GetUserChatsCount(1)).Returns(2);
            var unitOfWork = new UnitOfWork(_mockChatRepo.Object, _mockFileRepo.Object, _mockTextRepo.Object, _mockUserRepo.Object, _mockRoleRepo.Object);
            var userService = new UserService(unitOfWork, _mockMapper.Object);

            // Act + Assert
            Assert.Equal(2, userService.GetUserChatsCount(1));
        }


        [Fact]
        public void TGetUser_ShouldReturnUserDTOWithChatsCount_WhenUserExists()
        {
            // Arrange
            var _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object);
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
            var _userService = new UserService(_mockUnitOfWork.Object, _mockMapper.Object);

            var userId = 1;

            _mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => _userService.GetUser(userId));

            _mockUnitOfWork.Verify(uow => uow.Users.GetById(userId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Users.GetUserChatsCount(userId), Times.Never);
        }

    }
}
