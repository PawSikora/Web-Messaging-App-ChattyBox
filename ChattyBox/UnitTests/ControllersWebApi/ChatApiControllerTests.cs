using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTests.BLL.MockServices;
using WebApi.Controllers;

namespace UnitTests.ControllersWebApi
{
    public class ChatApiControllerTests
    {
 
        [Fact]
        public void Get_ReturnsOkResultWithGetChatDTO()
        {
            // Arrange
            int chatId = 1;
            int pageNumber = 1;
            var messagesPerPage = 5;

            var chatMessages = new List<MessageDTO>
            {
                new TextMessageDTO { Id = 1, Content = "Message 1" },
                new TextMessageDTO { Id = 2, Content = "Message 2" },
                new TextMessageDTO { Id = 3, Content = "Message 3" }
            };
            var chat = new GetChatDTO
            {
                ChatId = chatId, 
                Name = "Chat 1", 
                AllMessages = chatMessages
            };

            var mockChatService = new Mock<IChatService>();
           
            mockChatService.Setup(s => s.GetChat(chatId, pageNumber, messagesPerPage))
                .Returns(chat);
           
            var controller = new ChatController(mockChatService.Object);

            // Act
            var result = controller.Get(chatId, pageNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var chatDto = Assert.IsAssignableFrom<GetChatDTO>(okResult.Value);
            Assert.Equal(chat.AllMessages, chatDto.AllMessages);
            Assert.Equal(chat.ChatId, chatDto.ChatId);
            Assert.Equal(chat.Name, chatDto.Name);
            mockChatService.Verify(s => s.GetChat(chatId, pageNumber, messagesPerPage), Times.Once);
        }

        [Fact]
        public void Create_ReturnsOkResult_WhenModelStateValid()
        {
            // Arrange
            var mockChatService = new Mock<IChatService>();
         
            var controller = new ChatController(mockChatService.Object);
            var chat = new CreateChatDTO
            {
                Name = "TestChat"
            };

            // Act
            var result = controller.Create(chat) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            mockChatService.Verify(service => service.CreateChat(chat), Times.Once);
        }

        [Fact]
        public void Create_ReturnsBadRequest_WhenInvalidModelState()
        {
            //Arrange
            var mockChatService = new Mock<IChatService>();
            var mockFormFile = new Mock<IFormFile>();

            var chat = new CreateChatDTO { Name = "TestChat" };
            var _chatController = new ChatController(mockChatService.Object);
            _chatController.ModelState.AddModelError("Error", "Error message");

            //Act
            var result = _chatController.Create(chat) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal("Błąd tworzenia chatu!", result.Value);
        }

        [Fact]
        public void AddUser_ReturnsOkResult()
        {
            // Arrange
            var mockChatService = new Mock<IChatService>();
          
            var controller = new ChatController(mockChatService.Object);
            var chatId = 1;
            var userId = 1;
            var user = new User { Id = userId, Username = "Test user" };

            // Act
            var result = controller.AddUser(new ChatUserUpdateDTO { ChatId = chatId, UserId = userId }) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            mockChatService.Verify(service => service.AddUserById(userId, chatId), Times.Once);
        }

        [Fact]
        public void FindUser_ReturnsOkResultWithUser()
        {
            // Arrange
            var expectedUser = new UserDTO { Id = 1, Username = "Mock1", Email = "testUser@mail1.com" };
       
            var mockChatService = new ChatServiceMock();

            var controller = new ChatController(mockChatService);

            // Act
            var result = controller.FindUser(expectedUser.Email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var foundUser = Assert.IsAssignableFrom<UserDTO>(okResult.Value);
            Assert.Equal(expectedUser.Id, foundUser.Id);
            Assert.Equal(expectedUser.Username, foundUser.Username);
            Assert.Equal(expectedUser.Email, foundUser.Email);
        }

        [Fact]
        public void FindUser_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            Mock<IChatService> _mockChatService = new Mock<IChatService>();

            var email = "test@email.com";

            _mockChatService.Setup(s => s.GetUserByEmail(email)).Returns(() => null);

            var chatController = new ChatController(_mockChatService.Object);

            // Act
            var result = chatController.FindUser(email);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteUser_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            int userId = 3;

            var mockChatService = new Mock<IChatService>();
            
            var controller = new ChatController(mockChatService.Object);

            // Act
            var result = controller.DeleteUser(new ChatUserUpdateDTO { ChatId = id, UserId = userId });

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockChatService.Verify(service => service.DeleteUserById(userId, id), Times.Once);
        }

        [Fact]
        public void DeleteChat_ReturnsOkResult()
        {
            // Arrange
            int chatId = 1;

            var mockChatService = new Mock<IChatService>();
          
            var controller = new ChatController(mockChatService.Object);

            // Act
            var result = controller.DeleteChat(chatId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockChatService.Verify(service => service.DeleteChat(chatId), Times.Once);
        }

        [Fact]
        public void GetUsersInChat_ReturnsOkResultWithUsers()
        {
            // Arrange
            int chatId = 1;
           
            var expectedUsers = new List<UserDTO>
            {
                new UserDTO { Id = 1, Username = "User1" },
                new UserDTO { Id = 2, Username = "User2" },
                new UserDTO { Id = 3, Username = "User3" }
            };

            var mockChatService = new Mock<IChatService>();
            mockChatService.Setup(c => c.GetUsersInChat(chatId, 1, 5))
                .Returns(expectedUsers);
          
            var controller = new ChatController(mockChatService.Object);

            // Act
            var result = controller.GetUsersInChat(chatId, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<ICollection<UserDTO>>(okResult.Value);
            Assert.Equal(expectedUsers.Count, users.Count);
            mockChatService.Verify(service => service.GetUsersInChat(chatId, 1, 5), Times.Once);
        }

        [Fact]
        public void GetUsersInChat_ReturnsNotFound_WhenNoUsersInChat()
        {
            // Arrange
            Mock<IChatService> _mockChatService = new Mock<IChatService>();

            var chatId = 1;
            var pageNumber = 1;
            int usersPerPage = 5;

            _mockChatService.Setup(s => s.GetUsersInChat(chatId, pageNumber, usersPerPage)).Returns(() => null);

            var userController = new ChatController(_mockChatService.Object);

            // Act
            var result = userController.GetUsersInChat(chatId, pageNumber);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetUserRole_ReturnsOkResultWithRole()
        {
            // Arrange
            Mock<IChatService> _mockChatService = new Mock<IChatService>();

            var chatId = 1;
            var userId = 1;
            var expectedRole = "Admin";

            _mockChatService.Setup(s => s.GetUserRole(userId, chatId)).Returns(expectedRole);

            var chatController = new ChatController(_mockChatService.Object);

            // Act
            var result = chatController.GetUserRole(userId, chatId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var role = Assert.IsAssignableFrom<string>(okResult.Value);
            Assert.Equal(expectedRole, role);
        }

        [Fact]
        public void GetUserRole_ReturnsNotFound_WhenRoleNotFound()
        {
            // Arrange
            Mock<IChatService> _mockChatService = new Mock<IChatService>();

            var chatId = 1;
            var userId = 1;

            _mockChatService.Setup(s => s.GetUserRole(userId, chatId)).Returns(() => null);

            var chatController = new ChatController(_mockChatService.Object);

            // Act
            var result = chatController.GetUserRole(userId, chatId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetMessagesCount_ReturnsOkResultWithMessagesCount()
        {
            // Arrange
            Mock<IChatService> _mockChatService = new Mock<IChatService>();

            var chatId = 1;
            var expectedCount = 10;

            _mockChatService.Setup(s => s.GetChatMessagesCount(chatId)).Returns(expectedCount);

            var chatController = new ChatController(_mockChatService.Object);

            // Act
            var result = chatController.GetMessagesCount(chatId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var count = Assert.IsAssignableFrom<int>(okResult.Value);

            Assert.Equal(expectedCount, count);
        }

    }
}
