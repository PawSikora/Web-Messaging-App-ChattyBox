using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using BLL.Services.FileMessageService;
using BLL.Services.TextMessageService;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTests.BLL.MockServices;
using WebApi.Controllers;
using WebApi.ViewModels;

namespace UnitTests.ControllersWebApi
{
    public class ChatApiControllerTests
    {
 
        [Fact]
        public void Get_ReturnsOkResultWithGetChatDTO()
        {
            // Arrange
            int userId = 1;
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
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            mockChatService.Setup(s => s.GetChat(chatId, pageNumber, messagesPerPage))
                .Returns(chat);
            mockChatService.Setup(s => s.GetUsersInChat(chatId))
                .Returns(new List<UserDTO>());

            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object, mockTextMessageService.Object);

            // Act
            var result = controller.Get(userId, chatId, pageNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var chatDto = Assert.IsAssignableFrom<GetChatDTO>(okResult.Value);
            Assert.Equal(chat.AllMessages, chatDto.AllMessages);
            Assert.Equal(chat.ChatId, chatDto.ChatId);
            Assert.Equal(chat.Name, chatDto.Name);
            mockChatService.Verify(s => s.GetChat(chatId, pageNumber, messagesPerPage), Times.Once);
            mockChatService.Verify(s => s.GetUsersInChat(chatId), Times.Once);
        }

        [Fact]
        public void Create_ReturnsOkResult()
        {
            // Arrange
            var mockChatService = new Mock<IChatService>();
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object,
                               mockTextMessageService.Object);
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
        public void AddUser_ReturnsOkResult()
        {
            // Arrange
            var mockChatService = new Mock<IChatService>();
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object, mockTextMessageService.Object);
            var chatId = 1;
            var userId = 1;
            var senderId = 1;
            var user = new User { Id = userId, Username = "Test user" };

            // Act
            var result = controller.AddUser(chatId, userId, senderId) as OkResult;

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
            var user = new AddUserToChatTest { Email = "testUser@mail1.com" };
            var mockChatService = new ChatServiceMock();

            var controller = new ChatApiController(mockChatService, null, null);

            // Act
            var result = controller.FindUser(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var foundUser = Assert.IsAssignableFrom<UserDTO>(okResult.Value);
            Assert.Equal(expectedUser.Id, foundUser.Id);
            Assert.Equal(expectedUser.Username, foundUser.Username);
            Assert.Equal(expectedUser.Email, foundUser.Email);
        }

        [Fact]
        public void DeleteUser_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            int senderId = 2;
            int userId = 3;

            var mockChatService = new Mock<IChatService>();
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object, mockTextMessageService.Object);

            // Act
            var result = controller.DeleteUser(id, senderId, userId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockChatService.Verify(service => service.DeleteUserById(userId, id), Times.Once);
        }

        [Fact]
        public void DeleteChat_ReturnsOkResult()
        {
            // Arrange
            int chatId = 1;
            int senderId = 1;

            var mockChatService = new Mock<IChatService>();
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object, mockTextMessageService.Object);

            // Act
            var result = controller.DeleteChat(chatId, senderId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            mockChatService.Verify(service => service.DeleteChat(chatId), Times.Once);
        }

        [Fact]
        public void GetUsersInChat_ReturnsOkResultWithUsers()
        {
            // Arrange
            int chatId = 1;
            int userId = 2;
            int pageNumber = 1;
            var expectedUsers = new List<UserDTO>
            {
                new UserDTO { Id = 1, Username = "User1" },
                new UserDTO { Id = 2, Username = "User2" },
                new UserDTO { Id = 3, Username = "User3" }
            };

            var mockChatService = new Mock<IChatService>();
            mockChatService.Setup(c => c.GetUsersInChat(chatId))
                .Returns(expectedUsers);
            var mockFileMessageService = new Mock<IFileMessageService>();
            var mockTextMessageService = new Mock<ITextMessageService>();
            var controller = new ChatApiController(mockChatService.Object, mockFileMessageService.Object, mockTextMessageService.Object);

            // Act
            var result = controller.GetUsersInChat(chatId, userId, pageNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<ICollection<UserDTO>>(okResult.Value);
            Assert.Equal(expectedUsers.Count, users.Count);
            mockChatService.Verify(service => service.GetUsersInChat(chatId), Times.Once);
        }

    }
}
