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
        public ActionResult Create([FromBody] CreateChatDTO chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Błąd tworzenia chatu!");
            }

            _chatService.CreateChat(chat);
            return Ok();
        }

        [HttpPost("AddUser")]
        public ActionResult AddUser([FromQuery] int chatId, [FromQuery] int userId)
        {
            _chatService.AddUserById(userId, chatId);
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

        [HttpDelete]
        public ActionResult DeleteUser([FromQuery] int id,[FromQuery] int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteChat([FromRoute] int id)
        {
            _chatService.DeleteChat(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<ICollection<UserDTO>> GetUsersInChat([FromRoute] int id)
        {
            var users = _chatService.GetUsersInChat(id);
            if (users is null)
                return NotFound();

            return Ok(users);
        }

    }
}
