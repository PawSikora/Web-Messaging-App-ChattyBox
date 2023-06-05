using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.ViewModels;


namespace MVCWebApp.Controllers
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
            var userId = User.FindFirst("userId")?.Value;

            if (!string.IsNullOrEmpty(userId))
                return RedirectToAction("UserMenu");
            
            return View("LoginForm");
        }

        public IActionResult Logout()
        {
            var token = Request.Cookies["userToken"];
            if (!string.IsNullOrEmpty(token))
            {
                Response.Cookies.Delete("userToken");
            }
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult<UserDTO> UserMenu()
        {
            int userId = int.Parse(User.FindFirst("userId")?.Value);

            return View(_userService.GetUser(userId));
        }

        [Authorize]
        [HttpGet]
        public ActionResult<UserDTO> Get(int id)
        {
            return View(_userService.GetUser(id));
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ChatsAndCount>> GetChats(int id, int pageNumber)
        {
            var count = _userService.GetUserChatsCount(id);

            if (count == 0)
                return View("UserMenu", _userService.GetUser(id));

            var chatsPerPage = 5;

            var chatList = _userService.GetChats(id, pageNumber, chatsPerPage);
            
            var chats = new ChatsAndCount()
            {
                Count = count,
                ChatsPerPage = chatsPerPage,
                Chats = chatList,
                UserId = id,

            };
            return View("ChatBrowser", chats);
        }

        [HttpGet]
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
        public ActionResult Login(LoginUserDTO loginUser)
        {
            var tokenToReturn = _userService.LoginUser(loginUser);

            Response.Cookies.Append("userToken", tokenToReturn.TokenContent, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(1)
            }); ; 

            return RedirectToAction("UserMenu","User");
        }

        [Authorize]
        [HttpGet]
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
