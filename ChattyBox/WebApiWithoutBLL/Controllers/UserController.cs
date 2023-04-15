using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApiWithoutBLL.Exceptions;
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

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> Get([FromRoute] int id)
        {
            var user = _unitOfWork.Users.GetById(id);

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            return View(_mapper.Map<UserDTO>(user));
        }

        [HttpGet("Chats/{id}/{pageNumber}")]
        public ActionResult<IEnumerable<GetUserChatDTO>> GetChats([FromRoute] int id, [FromRoute] int pageNumber, [FromRoute] int chatsPerPage)
        {
            if (pageNumber < 1)
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");

            var chatCount = _unitOfWork.Users.GetUserChatsCount(id);

            if (chatCount == 0)
                throw new NotFoundException("Nie znaleziono czatów");

            int maxPageNumber = (int)Math.Ceiling((double)chatCount / chatsPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chatList = _unitOfWork.Chats.GetChatsForUser(id, pageNumber, chatsPerPage);

            if (chatList is null)
                throw new NotFoundException("Nie znaleziono czatu");

            return View(_mapper.Map<IEnumerable<GetUserChatDTO>>(chatList));
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] CreateUserDTO dto)
        {
            if (_unitOfWork.Users.IsEmailTaken(dto.Email))
                throw new EmailAlreadyUsedException("Email jest już zajęty");

            createPasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = dto.Email,
                Username = dto.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Created = DateTime.Now
            };

            _unitOfWork.Users.CreateUser(user);
            _unitOfWork.Save();

            return View();
        }

        [HttpPost("login")]
        public ActionResult<UserDTO> Login([FromBody] LoginUserDTO dto)
        {
            var user = _unitOfWork.Users.GetUserByEmail(dto.Email);

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new LoginFailedException("Niepoprawny login lub hasło");

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            user.LastLog = DateTime.Now;

            _unitOfWork.Save();

            return View(_mapper.Map<UserDTO>(user));
        }
    }
}
