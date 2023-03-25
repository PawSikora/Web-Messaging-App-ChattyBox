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

        // GET: api/<UserController>
        //[HttpGet]
        /*public ICollection<User> Get()
        {
            //var user = _unitOfWork.Users.GetUser(1);
            throw new NotImplementedException();
        }*/

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get(int id)
        {
            var user = _unitOfWork.Users.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<UserDTO>(user));
        }
        
        [HttpGet("Chats/{id}")]
        public ActionResult<ICollection<GetUserChatDTO>> GetChats(int id)
        {
            var chat = _unitOfWork.Users.GetChats(id).ToList();

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
            
            return Ok(_mapper.Map<UserDTO>(user));
        }


        // PUT api/<UserController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }*/

        // DELETE api/<UserController>/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {

        }*/
    }
}
