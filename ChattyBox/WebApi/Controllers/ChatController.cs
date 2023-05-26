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


        [HttpGet("{chatId}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int chatId, [FromRoute] int pageNumber)
        {
            var messagesPerPage = 5;
            return Ok(_chatService.GetChat(chatId, pageNumber, messagesPerPage));
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateChatDTO chat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Błąd tworzenia chatu!");
            }

            _chatService.CreateChat(chat);
            return Ok();
        }

        [HttpPost("{chatId}/{userId}")]
        public ActionResult AddUser([FromRoute] int chatId, [FromRoute] int userId)
        {
            _chatService.AddUserById(userId, chatId);
            return Ok();
        }

        [HttpGet("FindUser")]

        public ActionResult<UserDTO> FindUser([FromBody] UserDTO user)
        {
            var foundUser = _chatService.GetUserByEmail(user.Email);
            if (foundUser is null)
                return NotFound();

            return Ok(foundUser);
        }

        [HttpDelete("{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id,[FromRoute] int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return Ok();
        }

        [HttpDelete("{chatId}")]
        public ActionResult DeleteChat([FromRoute] int chatId)
        {
            _chatService.DeleteChat(chatId);
            return Ok();
        }

        [HttpGet("getUsers/{chatId}")]
        public ActionResult<ICollection<UserDTO>> GetUsersInChat([FromRoute] int chatId)
        {
            var users = _chatService.GetUsersInChat(chatId);
            if (users is null)
                return NotFound();

            return Ok(users);
        }

    }
}
