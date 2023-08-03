using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            return Ok(_userService.GetUser(id));
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<GetUserChatDTO>> GetChats([FromQuery] int id, [FromQuery] int pageNumber)
        {
            var chatsPerPage = 5;
            return Ok(_userService.GetChats(id, pageNumber, chatsPerPage));
        }

        [HttpPost("Register")]
        public ActionResult Register([FromBody] CreateUserDTO registerUser)
        {
            _userService.RegisterUser(registerUser);
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult<TokenToReturn> Login([FromBody] LoginUserDTO loginUser)
        {
            return Ok(_userService.LoginUser(loginUser));
        }
        [HttpPost("RefreshToken")]
        public ActionResult<TokenToReturn> RefreshToken([FromQuery] int userId)
        {
            var userDTO= _userService.GetUser(userId);
           var user= _userService.GetUser(userDTO.Email);
           var refreshToken = Request.Cookies["refreshToken"];

           if(!user.RefreshToken.Equals(refreshToken)) 
               return Unauthorized("Niepoprawny token");

           if(user.TokenExpires<DateTime.Now)
               return Unauthorized("Token wygasł");

           return Ok(new TokenToReturn(_userService.GenerateNewToken(user)));
        }

    }
}
