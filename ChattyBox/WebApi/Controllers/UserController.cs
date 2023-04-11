using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet("user/get/{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            return View(_userService.GetUser(id));
        }

        [HttpGet("user/getChats/{id}/{pageNumber}")]
        public ActionResult<ICollection<ChatsAndCount>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {

            var chatsPerPage = 5;
            var chatList = _userService.GetChats(id, pageNumber, chatsPerPage);
            var count = _userService.GetUserChatsCount(id);
            var chats = new ChatsAndCount()
            {
                Count = count,
                ChatsPerPage = chatsPerPage,
                Chats = chatList,
                UserId = id,

            };
            return View("ChatBrowser", chats);
        }

        [HttpGet("user/register")]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(CreateUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return View("Register", registerUser);

            _userService.RegisterUser(registerUser);
            return View("Login");
        }


        [HttpPost]
        public ActionResult<UserDTO> Login(LoginUserDTO loginUser)
        {
            return View("UserMenu", _userService.LoginUser(loginUser));
        }


        [HttpGet("user/createChat")]
        public ActionResult CreateChat(int id)
        {
            return RedirectToAction("Create", "Chat", new { id });
        }

        public ActionResult Unauthorized()
        {
			return View("AuthorizeFailed");
        }
    }
}
