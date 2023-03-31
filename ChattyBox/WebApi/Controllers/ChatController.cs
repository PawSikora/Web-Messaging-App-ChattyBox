using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("{id}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int id, [FromRoute] int pageNumber)
        {
            return View(_chatService.GetChat(id, pageNumber));
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateChatDTO chat)
        {
            _chatService.CreateChat(chat);
            return View();
        }

        [HttpPut("{id}/addUser/{userId}")]
        public ActionResult AddUser([FromRoute] int id, [FromRoute] int userId)
        {
            _chatService.AddUserById(userId, id);
            return View();
        }

        [HttpPut("{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id, [FromRoute] int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _chatService.DeleteChat(id);
            return View();
        }

        [HttpGet("getUsers/{id}")]
        public ActionResult<IEnumerable<UserDTO>> GetUsersInChat(int id)
        {
            return View(_chatService.GetUsersInChat(id));
        }

    }
}
