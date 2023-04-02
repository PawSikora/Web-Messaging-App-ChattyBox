using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<ICollection<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {
            return View("ChatBrowser",_userService.GetChats(id, pageNumber));
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
        public ActionResult<UserDTO> Login( LoginUserDTO loginUser)
        {
            return View("UserMenu",_userService.LoginUser(loginUser));
        }

       
        [HttpGet("user/createChat")]
        public ActionResult CreateChat(int id)
        {
            return RedirectToAction("Create", "Chat", new { id });
        }

    }
}
