using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.ViewModels;

namespace UnitTests.ControllersMVC
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();

        [Fact]
        public void GetUser_ReturnsUserView()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            var userId = 1;
            var userDto = new UserDTO { Id = userId, Username = "TestUser" };
            _mockUserService.Setup(service => service.GetUser(userId)).Returns(userDto);

            // Act
            var result = controller.Get(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<UserDTO>(viewResult.Model);
            Assert.Equal(userDto.Id, model.Id);
            Assert.Equal(userDto.Username, model.Username);

            _mockUserService.Verify(service => service.GetUser(userId), Times.Once);
        }

        [Fact]
        public void GetChats_ReturnsChatBrowserView()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            var userId = 1;
            var pageNumber = 1;
            var chatsPerPage = 5;
            var count = 10;
            var chatList = new List<GetUserChatDTO> { new GetUserChatDTO { Id = 1, Name = "Chat1" }, new GetUserChatDTO { Id = 2, Name = "Chat2" } };

            _mockUserService.Setup(service => service.GetUserChatsCount(userId)).Returns(count);
            _mockUserService.Setup(service => service.GetChats(userId, pageNumber, chatsPerPage)).Returns(chatList);

            // Act
            var result = controller.GetChats(userId, pageNumber);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ChatsAndCount>>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<ChatsAndCount>(viewResult.Model);
            Assert.Equal(count, model.Count);
            Assert.Equal(chatsPerPage, model.ChatsPerPage);
            Assert.Equal(chatList, model.Chats);
            Assert.Equal(userId, model.UserId);

            _mockUserService.Verify(service => service.GetUserChatsCount(userId), Times.Once);
            _mockUserService.Verify(service => service.GetChats(userId, pageNumber, chatsPerPage), Times.Once);
        }

        [Fact]
        public void Register_ReturnsRegisterView_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            var registerUser = new CreateUserDTO();
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = controller.Register(registerUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Register", viewResult.ViewName);

            _mockUserService.Verify(service => service.RegisterUser(It.IsAny<CreateUserDTO>()), Times.Never);
        }

        [Fact]
        public void Register_RedirectsToLoginView_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            var registerUser = new CreateUserDTO { Email = "test@example.com", Password = "123456" };

            // Act
            var result = controller.Register(registerUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Login", viewResult.ViewName);

            _mockUserService.Verify(service => service.RegisterUser(registerUser), Times.Once);
        }

        [Fact]
        public void Login_ReturnsUserMenuView()
        {
            // Arrange
            var controller = new UserController(_mockUserService.Object);
            var loginUser = new LoginUserDTO { Email = "test@example.com", Password = "123456" };
            var userDto = new UserDTO { Id = 1, Username = "TestUser" };
            _mockUserService.Setup(service => service.LoginUser(loginUser)).Returns(userDto);

            // Act
            var result = controller.Login(loginUser);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<UserDTO>(viewResult.Model);
            Assert.Equal(userDto.Id, model.Id);
            Assert.Equal(userDto.Username, model.Username);

            _mockUserService.Verify(service => service.LoginUser(loginUser), Times.Once);
        }
    }
}
