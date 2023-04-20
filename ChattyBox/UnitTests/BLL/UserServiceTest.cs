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
        private FakeUserRepository userRepo;
        private FakeChatRepository chatRepo;
        private FakeTextMessageRepository textRepo;
        private FakeFileMessageRepository fileRepo;
        private FakeRoleRepository roleRepo;
        private UnitOfWork unitOfWork;
        private Mock<IMapper> mockMapper;

        private Mock<IUserRepository> mockUserRepo;
        private Mock<IChatRepository> mockChatRepo;
        private Mock<ITextMessageRepository> mockTextRepo;
        private Mock<IFileMessageRepository> mockFileRepo;
        private Mock<IRoleRepository> mockRoleRepo;
        private Mock<IUnitOfWork> mockUnitOfWork;
        public UserServiceTest()
        {
            userRepo = new FakeUserRepository();
            chatRepo = new FakeChatRepository();
            textRepo = new FakeTextMessageRepository();
            fileRepo = new FakeFileMessageRepository();
            roleRepo = new FakeRoleRepository();
            unitOfWork = new UnitOfWork(chatRepo, fileRepo, textRepo, userRepo, roleRepo);
            mockMapper = new Mock<IMapper>();

            mockUserRepo = new Mock<IUserRepository>();
            mockChatRepo = new Mock<IChatRepository>();
            mockTextRepo = new Mock<ITextMessageRepository>();
            mockFileRepo = new Mock<IFileMessageRepository>();
            mockRoleRepo = new Mock<IRoleRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public void GetUserChatsCountFake_ShouldReturnCount_WhenUserHasChats()
        {
            var userService = new UserService(unitOfWork, mockMapper.Object);

            var user = new User() { Id = 1, Email = "User1", UserChats = new List<UserChat>()};
            var chat = new Chat() { Id = 1, Name = "Chat1" };
            var chat2 = new Chat() { Id = 2, Name = "Chat2" };

            var userChat = new UserChat() { Chat = chat, User = user };
            var userChat2 = new UserChat() { Chat = chat2, User = user };

            userRepo.CreateUser(user);
            user.UserChats.Add(userChat);
            user.UserChats.Add(userChat2);

            var count = userService.GetUserChatsCount(user.Id);

            Assert.Equal(2, count);
        }

        [Fact]
        public void GetUserFake_ShouldReturnUser_WhenUserExists()
        {
            mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });

            var user = new User() { Id = 1, Email = "User1", UserChats = new List<UserChat>() };
            var chat = new Chat() { Id = 1, Name = "Chat1" };
            var userChat = new UserChat() { Chat = chat, User = user };
            userRepo.CreateUser(user);
            user.UserChats.Add(userChat);

            var userService = new UserService(unitOfWork, mockMapper.Object);

            var userDto = userService.GetUser(1);

            Assert.Equal("User1", userDto.Email);
        }

        [Fact]
        public void GetUserMoq_ShouldReturnUser_WhenUserExists()
        {
            mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<User>()))
                .Returns<User>(u => new UserDTO { Id = u.Id, Email = u.Email });
            mockUserRepo.Setup(repo => repo.GetById(1)).Returns(new User() { Id = 1, Email = "User1" });

            var unitOfWork = new UnitOfWork(mockChatRepo.Object, mockFileRepo.Object, mockTextRepo.Object, mockUserRepo.Object, mockRoleRepo.Object);
            var userService = new UserService(unitOfWork, mockMapper.Object);

            Assert.Equal("User1", userService.GetUser(1).Email);
        }

        [Fact]
        public void GetUserChatsCountMoq_ShouldReturnCount_WhenUserHasChats()
        {
            mockUserRepo.Setup(repo => repo.GetUserChatsCount(1)).Returns(2);
            var unitOfWork = new UnitOfWork(mockChatRepo.Object, mockFileRepo.Object, mockTextRepo.Object, mockUserRepo.Object, mockRoleRepo.Object);
            var userService = new UserService(unitOfWork, mockMapper.Object);

            Assert.Equal(2, userService.GetUserChatsCount(1));

        }


        [Fact]
        public void TGetUser_ShouldReturnUserDTOWithChatsCount_WhenUserExists()
        {
            var _userService = new UserService(mockUnitOfWork.Object, mockMapper.Object);

            // Arrange
            var user = new User { Id = 1, Username = "Test User" };
            var userDTO = new UserDTO { Id = 1, Username = "Test User", ChatsCount = 2 };

            mockUnitOfWork.Setup(uow => uow.Users.GetById(user.Id)).Returns(user);
            mockUnitOfWork.Setup(uow => uow.Users.GetUserChatsCount(user.Id)).Returns(2);
            mockMapper.Setup(m => m.Map<UserDTO>(user)).Returns(userDTO);

            // Act
            var result = _userService.GetUser(user.Id);

            // Assert
            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Username, result.Username);
            Assert.Equal(userDTO.ChatsCount, result.ChatsCount);

            mockUnitOfWork.Verify(uow => uow.Users.GetById(user.Id), Times.Once);
            mockUnitOfWork.Verify(uow => uow.Users.GetUserChatsCount(user.Id), Times.Once);
        }

        [Fact]
        public void TestGetUser_ShouldThrowNotFoundException_WhenUserDoesNotExist_()
        {
            var _userService = new UserService(mockUnitOfWork.Object, mockMapper.Object);

            // Arrange
            var userId = 1;

            mockUnitOfWork.Setup(uow => uow.Users.GetById(userId)).Returns((User)null);

            // Act + Assert
            Assert.Throws<NotFoundException>(() => _userService.GetUser(userId));

            mockUnitOfWork.Verify(uow => uow.Users.GetById(userId), Times.Once);
            mockUnitOfWork.Verify(uow => uow.Users.GetUserChatsCount(userId), Times.Never);
        }

    }
}
