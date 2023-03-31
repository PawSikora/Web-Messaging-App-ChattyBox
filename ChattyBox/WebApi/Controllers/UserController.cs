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

        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            return View(_userService.GetUser(id));
        }

        [HttpGet("Chats/{id}/{pageNumber}")]
        public ActionResult<ICollection<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {
            return View(_userService.GetChats(id, pageNumber));
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] CreateUserDTO registerUser)
        {
            _userService.RegisterUser(registerUser);
            return View();
        }

        [HttpPost("login")]
        public ActionResult<UserDTO> Login([FromBody] LoginUserDTO loginUser)
        {
            return View(_userService.LoginUser(loginUser));
        }

    }
}
