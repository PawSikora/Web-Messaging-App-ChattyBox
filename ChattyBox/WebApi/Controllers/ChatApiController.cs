using System.Web.WebPages;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using BLL.Services.FileMessageService;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatApiController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IFileMessageService _fileMessageService;
        private readonly ITextMessageService _textMessageService;

        public ChatApiController(IChatService chatService, IFileMessageService fileMessageService,
            ITextMessageService textMessageService)
        {
            _chatService = chatService;
            _fileMessageService = fileMessageService;
            _textMessageService = textMessageService;
        }


        [HttpGet("{userId}/{chatId}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get(int userId, int chatId, int pageNumber)
        {
            var messagesPerPage = 5;
            var chat = _chatService.GetChat(chatId, pageNumber, messagesPerPage);
            if(chat is null)
                return NotFound();

            var chatDto = new GetChatDTO()
            {
                AllMessages = chat.AllMessages, 
                ChatId = chat.ChatId, 
                Name = chat.Name,
                Users = (ICollection<UserDTO>)_chatService.GetUsersInChat(chatId) 
            };
            return Ok(chatDto);
        }

        [HttpPost("Create")]
        public ActionResult Create(CreateChatDTO chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Błąd tworzenia chatu!");
            }

            _chatService.CreateChat(chat);
            return Ok();
        }

        [HttpPost("{chatId}/{userId}/{senderId}")]
        public ActionResult AddUser(int chatId, int userId, int senderId)
        {
            _chatService.AddUserById(userId, chatId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<UserDTO> FindUser(AddUserToChatTest user)
        {
            var foundUser = _chatService.GetUserByEmail(user.Email);
            if (foundUser is null)
                return NotFound();
            return Ok(foundUser);
        }

        [HttpDelete("chat/{id}-{senderId}/deleteUser/{userId}")]
        public ActionResult DeleteUser(int id, int senderId, int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return Ok();
        }

        [HttpDelete("{chatId}-{senderId}")]
        public ActionResult DeleteChat(int chatId, int senderId)
        {
            _chatService.DeleteChat(chatId);
            return Ok();
        }

        [HttpGet("chat/getUsers/{chatId}/{pageNumber}/{userId}")]
        public ActionResult<ICollection<UserDTO>> GetUsersInChat(int chatId, int userId, int pageNumber)
        {
            var usersPerPage = 5;
            var users = _chatService.GetUsersInChat(chatId);
            if (users is null)
                return NotFound();

            return Ok(users);
        }

    }
}
