using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Exceptions;
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
        public ActionResult<GetChatDTO> Get([FromRoute] int id, [FromRoute] int pageNumber, [FromRoute] int messagesPerPage)
        {
            if (pageNumber < 1)
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");

            var messageCount = _unitOfWork.Chats.GetChatMessagesCount(id);

            int maxPageNumber = (int)Math.Ceiling((double)messageCount / messagesPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chat = _unitOfWork.Chats.GetChat(id, pageNumber, messagesPerPage);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            return _mapper.Map<GetChatDTO>(chat);

        }
        
        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateChatDTO dto)
        {
            if (_unitOfWork.Chats.IsChatNameTaken(dto.Name))
                throw new NotUniqueElementException("Nazwa czatu jest już zajęta");

            var user = _unitOfWork.Users.GetById(dto.UserId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var role = _unitOfWork.Roles.GetByName("Admin");

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            var chat = _mapper.Map<Chat>(dto);

            UserChat userChat = new UserChat
            {
                Chat = chat,
                UserId = dto.UserId,
                RoleId = role.Id
            };

            _unitOfWork.Chats.CreateChat(chat, userChat);
            _unitOfWork.Save();

            return View();
        }

        [HttpPut("{id}/addUser/{userId}")]
        public ActionResult AddUser([FromRoute] int chatId, [FromRoute] int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var role = _unitOfWork.Roles.GetByName("User");

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            if (_unitOfWork.Chats.IsUserInChat(userId, chatId))
                throw new IllegalOperationException("Użytkownik jest już w czacie");

            var userChat = new UserChat { UserId = userId, ChatId = chatId, RoleId = role.Id };

            _unitOfWork.Chats.AddUserToChat(userChat);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();

            return View();
        }

        [HttpPut("{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int chatId, [FromRoute] int userId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);
            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var userChat = _unitOfWork.Chats.GetUserChatById(userId, chatId);

            if (userChat is null)
                throw new NotFoundException("Nie znaleziono użytkownika w czacie");

            _unitOfWork.Chats.RemoveUserFromChat(userChat);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();

            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var chatUsers = _unitOfWork.Chats.GetChatUsersById(chatId);

            if (chatUsers is not null)
                _unitOfWork.Chats.RemoveUsersFromChat(chatUsers);

            _unitOfWork.Chats.DeleteChat(chat);
            _unitOfWork.Save();

            return View();
        }

        [HttpGet("getUsers/{id}")]
        public ActionResult<IEnumerable<UserDTO>> GetUsersInChat(int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var users = _unitOfWork.Chats.GetUsersInChat(chatId);

            if (users is null)
                throw new NotFoundException("Nie znaleziono użytkowników");

            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return View(userDtos);
        }
    }
}
