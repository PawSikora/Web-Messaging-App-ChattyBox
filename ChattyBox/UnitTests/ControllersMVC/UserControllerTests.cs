using System.Security.Claims;
using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTests.BLL.MockServices;
using MVCWebApp.Controllers;
using MVCWebApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace UnitTests.ControllersMVC
{
    public class UserControllerTests
    {
        [Fact]
        public void Login_ReturnsViewWithLoginForm_WhenUserClaimIsNull()
        {
            // Arrange         
            Mock<IUserService> _mockUserService = new Mock<IUserService>();

            var expectedViewName = "LoginForm";

            var userController = new UserController(_mockUserService.Object);

            userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity())
                }
            };

            // Act
            var result = userController.CheckUserSession() as ViewResult;

            // Assert
            Assert.Equal(expectedViewName, result.ViewName);
        }

        [Fact]
        public void Login_ReturnsRedirectToUserMenu_WhenUserClaimIsNotNull()
        {
            // Arrange
            Mock<IUserService> _mockUserService = new Mock<IUserService>();

            var expectedActionName = "UserMenu";

            var userController = new UserController(_mockUserService.Object);

            userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    }))
                }
            };

            // Act
            var result = userController.CheckUserSession() as RedirectToActionResult;

            // Assert
            Assert.Equal(expectedActionName, result.ActionName);
        }

        [Fact]
        public void Login_ReturnsRedirectToUserMenu()
        {
            // Arrange
            Mock<IUserService> _userServiceMock = new Mock<IUserService>();

            var loginUser = new LoginUserDTO()
            {
                Email = "test@email.com",
                Password = "TestPassword"
            };
            var tokenToReturn = new TokenToReturn("test-token");

            _userServiceMock.Setup(x => x.LoginUser(loginUser)).Returns(tokenToReturn);

            var _userController = new UserController(_userServiceMock.Object);
            _userController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _userController.Login(loginUser) as RedirectToActionResult;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserMenu", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }

        [Fact]
        public void Login_CreatesTokenCookie()
        {
            // Arrange
            Mock<IUserService> _userServiceMock = new Mock<IUserService>();
            Mock<IResponseCookies> _cookieCollectionMock = new Mock<IResponseCookies>();

            var loginUser = new LoginUserDTO()
            {
                Email = "test@email.com",
                Password = "TestPassword"
            };
            var tokenToReturn = new TokenToReturn("test-token");

            _userServiceMock.Setup(x => x.LoginUser(loginUser)).Returns(tokenToReturn);
            _cookieCollectionMock.Setup(x => x.Append("userToken", "test-token", It.IsAny<CookieOptions>()));

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Response.Cookies).Returns(_cookieCollectionMock.Object);

            var _userController = new UserController(_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext.Object
                }
            };

            // Act
            var result = _userController.Login(loginUser) as RedirectToActionResult;

            // Assert
            _cookieCollectionMock.Verify(x => x.Append("userToken", "test-token", It.IsAny<CookieOptions>()), Times.Once);
            _userServiceMock.Verify(x => x.LoginUser(loginUser), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("UserMenu", result.ActionName);
        }

        [Fact]
        public void Logout_ReturnsLoginViewAndDeletesToken_WhenTokenValid()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();
            var _mockHttpContext = new Mock<HttpContext>();
            var _mockHttpResponse = new Mock<HttpResponse>();

            var userController = new UserController(_mockUserService.Object);

            var token = "validToken";
            var expectedViewName = "LoginForm";

            _mockHttpResponse.SetupGet(x => x.Cookies).Returns(new Mock<IResponseCookies>().Object);
            _mockHttpContext.SetupGet(s => s.Request.Cookies["userToken"]).Returns(token);
            _mockHttpContext.SetupGet(s => s.Response).Returns(_mockHttpResponse.Object);

            userController.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            // Act
            var result = userController.Logout() as ViewResult;

            // Assert
            Assert.Equal(expectedViewName, result.ViewName);
            _mockHttpResponse.Verify(r => r.Cookies.Delete("userToken"), Times.Once);
        }

        [Fact]
        public void Logout_ReturnsLoginForm_WhenNoValidToken()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();
            var _mockHttpContext = new Mock<HttpContext>();
            var _mockHttpResponse = new Mock<HttpResponse>();

            var userController = new UserController(_mockUserService.Object);

            var expectedViewName = "LoginForm";

            _mockHttpContext.SetupGet(s => s.Request.Cookies["userToken"]).Returns(() => null);
            _mockHttpContext.SetupGet(s => s.Response).Returns(_mockHttpResponse.Object);

            userController.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };

            // Act
            var result = userController.Logout() as ViewResult;

            // Assert
            Assert.Equal(expectedViewName, result.ViewName);
            _mockHttpResponse.Verify(r => r.Cookies.Delete("userToken"), Times.Never);
        }

        [Fact]
        public void UserMenu_ReturnsViewWithUser_WhenAuthorized()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();
            var _mockHttpContext = new Mock<HttpContext>();

            var userDto = new UserDTO { Id = 1, Username = "Test"};
            var userIdClaim = new ClaimsIdentity();


            userIdClaim.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            _mockHttpContext.SetupGet(s => s.User.Identity).Returns(userIdClaim);
            _mockUserService.Setup(s => s.GetUser(1)).Returns(userDto);

            var userController = new UserController(_mockUserService.Object);
            userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    }))
                }
            };

            // Act
            var result = userController.UserMenu();

            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<UserDTO>(viewResult.Model);

            //Assert
            Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.Equal(userDto, model);
        }

        [Fact]
        public void CreateChat_ReturnsRedirectToCreate()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();

            var id = 1;
            var expectedActionName = "Create";

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.CreateChat(id) as RedirectToActionResult;

            // Assert
            Assert.Equal(expectedActionName, result.ActionName);
            Assert.Equal(result.ControllerName, "Chat");
            Assert.Equal(result.RouteValues["id"], id);
        }

        [Fact]
        public void Get_ReturnsViewWithUser_WhenAuthorized()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();

            var userId = 1;
            var userDto = new UserDTO { Id = 1, Username = "Test" };

            _mockUserService.Setup(s => s.GetUser(userId)).Returns(userDto);

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.Get(userId);

            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<UserDTO>(viewResult.Model);

            //Assert
            Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.Equal(userDto, model);
        }

        [Fact]
        public void GetUser_ReturnsUserView()
        {
            // Arrange
            var userBllMock = new UserServiceBllMock();
            var controller = new UserController(userBllMock);
            var userId = 1;
            var userDto = new UserDTO { Id = userId, Username = "Mock1" };

            // Act
            var result = controller.Get(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<UserDTO>(viewResult.Model);
            Assert.Equal(userDto.Id, model.Id);
            Assert.Equal(userDto.Username, model.Username);
        }

        [Fact]
        public void GetChats_ReturnsViewChatBrowser_WhenUserHasChats()
        {
            // Arrange
            Mock<IUserService> _mockUserService = new Mock<IUserService>();

            var userId = 1;
            var pageNumber = 1;
            var chatsPerPage = 5;
            var count = 10;
            var chatList = new List<GetUserChatDTO> { new GetUserChatDTO { Id = 1, Name = "Chat1" }, new GetUserChatDTO { Id = 2, Name = "Chat2" } };

            _mockUserService.Setup(service => service.GetUserChatsCount(userId)).Returns(count);
            _mockUserService.Setup(service => service.GetChats(userId, pageNumber, chatsPerPage)).Returns(chatList);

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.GetChats(userId, pageNumber);

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
        public void GetChats_ReturnsViewUserMenuWithUser_WhenUserHasNoChats()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();

            var userId = 1;
            var pageNumber = 1;
            var userChatCount = 0;
            var expectedView = "UserMenu";
            var userDto = new UserDTO { Id = 1, Username = "Test" };

            _mockUserService.Setup(s => s.GetUserChatsCount(userId)).Returns(userChatCount);
            _mockUserService.Setup(s => s.GetUser(userId)).Returns(userDto);

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.GetChats(userId, pageNumber);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<ChatsAndCount>>>(result);
            var viewResult = Assert.IsType<ViewResult>(actionResult.Result);

            //Assert
            Assert.Equal(expectedView, viewResult.ViewName);
            Assert.Equal(userDto, viewResult.Model);
        }

        [Fact]
        public void ShowRegistrationForm_ReturnsViewResult()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();

            var expectedView = "Register";

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.ShowRegistrationForm();
            var viewResult = Assert.IsType<ViewResult>(result);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedView, viewResult.ViewName);
        }

        [Fact]
        public void Register_ReturnsRegisterView_WhenModelStateIsInvalid()
        {
            // Arrange
            Mock<IUserService> _mockUserService = new Mock<IUserService>();
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
            Mock<IUserService> _mockUserService = new Mock<IUserService>();
            var controller = new UserController(_mockUserService.Object);
            var registerUser = new CreateUserDTO { Email = "test@example.com", Password = "123456" };

            // Act
            var result = controller.Register(registerUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("LoginForm", viewResult.ViewName);

            _mockUserService.Verify(service => service.RegisterUser(registerUser), Times.Once);
        }

        [Fact]
        public void Unauthorized_ReturnsViewResult_WhenUnauthorized()
        {
            // Arrange
            var _mockUserService = new Mock<IUserService>();

            var expectedView = "AuthorizeFailed";

            var userController = new UserController(_mockUserService.Object);

            // Act
            var result = userController.Unauthorized();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedView, viewResult.ViewName);
        }
    }
}
