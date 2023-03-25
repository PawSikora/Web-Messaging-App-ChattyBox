using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Web.Models.UserDTOs;
using WebApi.Models.ChatDTO;
using WebApi.Models.UserDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public UserDTO Get(int id)
        {
            var user = _unitOfWork.Users.GetUser(id);
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Created = user.Created,
            };
            return userDTO;
            //return _unitOfWork.Users.GetUser(id);
        }
        [HttpGet("Chats/{id}")]
        public ICollection<GetUserChatDTO> GetChats(int id)
        {
           
            var chat = _unitOfWork.Users.GetChats(id)
                .Select(c => new GetUserChatDTO()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return chat;

        }

        // POST api/<UserController>
        [HttpPost("register")]
        public void Register([FromBody] CreateUserDTO registerUser)
        {
            _unitOfWork.Users.RegisterUser(registerUser.Email, registerUser.Name, registerUser.Password);
            _unitOfWork.Save();
        }

        [HttpPost("login")]

        public UserDTO Login([FromBody] LoginUserDTO loginUser)
        {
            var user = _unitOfWork.Users.LoginUser(loginUser.Email, loginUser.Password);
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Created = user.Created,
            };
            return userDTO;
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
