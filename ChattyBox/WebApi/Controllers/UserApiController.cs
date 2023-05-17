using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }
       
        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            return Ok(_userService.GetUser(id));
        }

        [HttpGet("getChats/{id}/{pageNumber}")]
        public ActionResult<IEnumerable<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {
            var chatsPerPage = 5;
            return Ok(_userService.GetChats(id, pageNumber, chatsPerPage));
        }

        [HttpPost("Register")]
        public ActionResult Register([FromBody] CreateUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest("Błąd tworzenia uzytkownika!");

            _userService.RegisterUser(registerUser);
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult<UserDTO> Login([FromBody] LoginUserDTO loginUser)
        {
            return Ok(_userService.LoginUser(loginUser));
        }

    }
}
