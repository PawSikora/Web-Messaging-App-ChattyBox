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

        [HttpGet("chat/Get/{id}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int id, [FromRoute] int pageNumber)
        {
            return View("ChatMenu",_chatService.GetChat(id, pageNumber));
        }

        [HttpGet("chat/Create")]
        public ActionResult Create(int id)
        {
            ViewBag.UserId = id;
            return View("CreateChat");
        }

        [HttpPost]
        public ActionResult Create(CreateChatDTO chat)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateChat", chat);
            }
            
            _chatService.CreateChat(chat);
            return RedirectToAction("GetChats", "User", new { id = chat.UserId, pageNumber = 1 });
        }

        [HttpPut("chat/{id}/addUser/{userId}")]
        public ActionResult AddUser([FromRoute] int id, [FromRoute] int userId)
        {
            _chatService.AddUserById(userId, id);
            return View();
        }

        [HttpPut("chat/{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id, [FromRoute] int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return View();
        }

        [HttpDelete("chat/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _chatService.DeleteChat(id);
            return View();
        }

        [HttpGet("chat/getUsers/{id}")]
        public ActionResult<IEnumerable<UserDTO>> GetUsersInChat(int id)
        {
            return View(_chatService.GetUsersInChat(id));
        }

    }
}
