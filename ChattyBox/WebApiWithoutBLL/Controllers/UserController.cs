using AutoMapper;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Models.UserDtos;

namespace WebApiWithoutBLL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            var user = _unitOfWork.Users.GetUser(id);

            if (user == null)
            {
                return View("Error");
            }

            return View(_mapper.Map<UserDTO>(user));
        }

        [HttpGet("Chats/{id}/{pageNumber}")]
        public ActionResult<ICollection<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber)
        {
            var chat = _unitOfWork.Users.GetChats(id, pageNumber).ToList();

            if (chat == null)
            {
                return View("Error");
            }

            return View(_mapper.Map<ICollection<GetUserChatDTO>>(chat));
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] CreateUserDTO registerUser)
        {
            _unitOfWork.Users.RegisterUser(registerUser.Email, registerUser.Name, registerUser.Password);
            _unitOfWork.Save();
            return View();
        }

        [HttpPost("login")]
        public ActionResult<UserDTO> Login([FromBody] LoginUserDTO loginUser)
        {
            var user = _unitOfWork.Users.LoginUser(loginUser.Email, loginUser.Password);

            if (user == null)
            {
                return View("Error");
            }

            _unitOfWork.Save();

            return View(_mapper.Map<UserDTO>(user));
        }
    }
}
