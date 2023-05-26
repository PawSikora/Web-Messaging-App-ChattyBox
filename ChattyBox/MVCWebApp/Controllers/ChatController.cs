using BLL.DataTransferObjects.ChatDtos;
using BLL.Services.ChatService;
using Microsoft.AspNetCore.Mvc;
using MVCWebApp.ViewModels;
using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.FileMessageService;
using BLL.Services.TextMessageService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IFileMessageService _fileMessageService;
        private readonly ITextMessageService _textMessageService;

        public ChatController(IChatService chatService, IFileMessageService fileMessageService, ITextMessageService textMessageService)
        {
            _chatService = chatService;
            _fileMessageService = fileMessageService;
            _textMessageService = textMessageService;
        }

        [HttpPost]
        public ActionResult SendMessage([FromForm] CreateFileMessageDTO fileMessage, [FromForm] CreateTextMessageDTO textMessage)
        {
            if (!textMessage.Content.IsNullOrEmpty())
            {
                _textMessageService.CreateTextMessage(textMessage);
            }

            if (fileMessage.File is not null)
            {
                fileMessage.Name = fileMessage.File.FileName;
                _fileMessageService.CreateFileMessage(fileMessage);
            }

            return RedirectToAction("Get", "Chat", new { chatId = textMessage.ChatId, pageNumber = 1 });
        }

        [HttpGet("chat/Get/{chatId}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int chatId, [FromRoute] int pageNumber)
        {
            var senderId = int.Parse(User.FindFirst("userId")?.Value);

            var messagesPerPage = 5;
            var count = _chatService.GetChatMessagesCount(chatId);
            var chat = new MessagesAndCount()
            {
                Chat = _chatService.GetChat(chatId, pageNumber,messagesPerPage),
                Count = count,
                MessagesPerPage = messagesPerPage,
                UserId = senderId,
                UserRole = _chatService.GetUserRole(senderId, chatId),
            };
            return View("ChatMenu",chat);
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
                return View("CreateChat", chat);
            
            _chatService.CreateChat(chat);
            return RedirectToAction("GetChats", "User", new { id = chat.UserId, pageNumber = 1 });
        }

        [HttpPost("chat/{chatId}/addUser/{userId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
		public ActionResult AddUser([FromRoute] int chatId, [FromRoute] int userId)
        {
            _chatService.AddUserById(userId, chatId);
            return RedirectToAction("GetUsersInChat", "Chat",new { chatId, pageNumber = 1});
        }

        [HttpGet("chat/{chatId}/GetAddUserToChat")]
        public ActionResult GetAddUserToChat([FromRoute]int chatId)
        {
            var senderId = int.Parse(User.FindFirst("userId")?.Value);

            ViewBag.chatId = chatId;
            ViewBag.userId = senderId;

            return View("ChatAddUser");
        }

        [HttpGet]
        public ActionResult FindUser(AddUserToChat user)
        {
            if (!ModelState.IsValid)
                return View("ChatAddUser", user);
            
            user.userDto =_chatService.GetUserByEmail(user.Email);
            return View("ChatAddUser",user);
        }

        [HttpPost("chat/{chatId}/deleteUser/{userId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
        public ActionResult DeleteUser([FromRoute] int chatId, [FromRoute] int userId)
        {
            _chatService.DeleteUserById(userId, chatId);
            return RedirectToAction("GetUsersInChat", "Chat", new { chatId, pageNumber = 1 });
        }

        [HttpPost("chat/{chatId}-{senderId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
        public ActionResult DeleteChat([FromRoute] int chatId, [FromRoute] int senderId)
        {
            _chatService.DeleteChat(chatId);
            return RedirectToAction("GetChats", "User", new { id = senderId, pageNumber = 1 });
        }

        [HttpGet("chat/getUsers/{chatId}/{pageNumber}")]
        public ActionResult<ICollection<ChatAndUsers>> GetUsersInChat([FromRoute] int chatId, [FromRoute] int pageNumber)
        {
            var senderId = int.Parse(User.FindFirst("userId")?.Value);

            var usersPerPage = 5;
            var users = _chatService.GetUsersInChat(chatId);
            var role = _chatService.GetUserRole(senderId,chatId);
            var chatsAndUsers = new ChatAndUsers
            {
                ChatId = chatId,
                Users = users.ToList(),
                Count = users.Count(),
                UsersPerPage = usersPerPage,
                PageNumber = pageNumber,
                UserRole = role,
                UserId = senderId
            };
            return View("ChatGetUsers",chatsAndUsers);
        }

    }
}
