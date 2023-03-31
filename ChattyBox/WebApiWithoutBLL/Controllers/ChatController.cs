using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Models.ChatDtos;
using WebApiWithoutBLL.Models.UserDtos;

namespace WebApiWithoutBLL.Controllers
{
    public class ChatController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int id, [FromRoute] int pageNumber)
        {

            Chat chat = _unitOfWork.Chats.GetChat(id, pageNumber);

            if (chat == null)
            {
                return View("Error");
            }

            var chatDto = _mapper.Map<GetChatDTO>(chat);

            return View(chatDto);

        }
        
        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateChatDTO chat)
        {
            _unitOfWork.Chats.CreateChat(chat.Name, chat.UserId);
            _unitOfWork.Save();
            return View();
        }

        [HttpPut("{id}/addUser/{userId}")]
        public ActionResult AddUser([FromRoute] int id, [FromRoute] int userId)
        {
            _unitOfWork.Chats.AddUserById(userId, id);
            _unitOfWork.Save();
            return View();
        }

        [HttpPut("{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id, [FromRoute] int userId)
        {
            _unitOfWork.Chats.DeleteUserById(userId, id);
            _unitOfWork.Save();
            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _unitOfWork.Chats.DeleteChat(id);
            _unitOfWork.Save();
            return View();
        }

        [HttpGet("getUsers/{id}")]
        public ActionResult<IEnumerable<UserDTO>> GetUsersInChat(int id)
        {
            var users = _unitOfWork.Chats.GetUsersInChat(id);

            if (users == null)
            {
                return View("Error");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return View(userDtos);
        }
    }
}
