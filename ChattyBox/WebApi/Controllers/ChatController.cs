using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using BLL.Services.FileMessageService;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }


        [HttpGet]
        public ActionResult<GetChatDTO> Get([FromQuery] int chatId, [FromQuery] int pageNumber)
        {
            var messagesPerPage = 5;
            return Ok(_chatService.GetChat(chatId, pageNumber, messagesPerPage));
        }

        [HttpPost("Create")]
        public ActionResult Create([FromForm] CreateChatDTO chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Błąd tworzenia chatu!");
            }

            _chatService.CreateChat(chat);
            return Ok();
        }

        [HttpPut("AddUser")]
        public ActionResult AddUser([FromBody] ChatUserUpdateDTO chatUser)
        {
            _chatService.AddUserById(chatUser.UserId, chatUser.ChatId);
            return Ok();
        }

        [HttpGet("FindUser")]
        public ActionResult<UserDTO> FindUser([FromQuery] string email)
        {
            var foundUser = _chatService.GetUserByEmail(email);
            if (foundUser is null)
                return NotFound();

            return Ok(foundUser);
        }

        [HttpPut("DeleteUser")]
        public ActionResult DeleteUser([FromBody] ChatUserUpdateDTO chatUser)
        {
            _chatService.DeleteUserById(chatUser.UserId, chatUser.ChatId);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteChat([FromRoute] int id)
        {
            _chatService.DeleteChat(id);
            return Ok();
        }

        [HttpGet("GetUsers")]
        //public ActionResult<ICollection<UserDTO>> GetUsersInChat([FromRoute] int id, [FromRoute] int pageNumber)
        public ActionResult<ICollection<UserDTO>> GetUsersInChat([FromQuery] int id, [FromQuery] int pageNumber)
        {
            var usersPerPage = 5;
            var users = _chatService.GetUsersInChat(id, pageNumber, usersPerPage);
            if (users is null)
                return NotFound();

            return Ok(users);
        }

        [HttpGet("GetUserRole")]
        public ActionResult<string> GetUserRole([FromQuery] int chatId, [FromQuery] int userId)
        {
            var role = _chatService.GetUserRole(userId, chatId);
            if (role is null)
                return NotFound();
            return Ok(role);
        }

        [HttpGet("GetMessagesCount")]
        public ActionResult<int> GetMessagesCount([FromQuery] int id)
        {
            var count = _chatService.GetChatMessagesCount(id);
            return Ok(count);
        }
        
    }
}
