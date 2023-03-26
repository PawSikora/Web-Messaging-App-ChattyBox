using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Web.Models.UserDtos;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            var user = _unitOfWork.Users.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<UserDTO>(user));
        }
        
        [HttpGet("Chats/{id}/{pageNumber}")]
        public ActionResult<ICollection<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {
            var chat = _unitOfWork.Users.GetChats(id,pageNumber).ToList();

            if (chat == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ICollection<GetUserChatDTO>>(chat));

        }

        // POST api/<UserController>
        [HttpPost("register")]
        public ActionResult Register([FromBody] CreateUserDTO registerUser)
        {
            _unitOfWork.Users.RegisterUser(registerUser.Email, registerUser.Name, registerUser.Password);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<UserDTO> Login([FromBody] LoginUserDTO loginUser)
        {
            var user = _unitOfWork.Users.LoginUser(loginUser.Email, loginUser.Password);
            
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.Save();

            return Ok(_mapper.Map<UserDTO>(user));
        }

    }
}
