using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("chat/user/{userId}/Get/{chatId}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute]int userId,[FromRoute] int chatId, [FromRoute] int pageNumber)
        {
            var messagesPerPage = 5;
            var count = _chatService.GetChatMessagesCount(chatId);
            var chat = new MessagesAndCount()
            {
                Chat = _chatService.GetChat(chatId, pageNumber,messagesPerPage),
                Count = count,
                MessagesPerPage = messagesPerPage,
                UserId = userId,
                UserRole = _chatService.GetUserRole(userId, chatId),
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
            {
                return View("CreateChat", chat);
            }
            
            _chatService.CreateChat(chat);
            return RedirectToAction("GetChats", "User", new { id = chat.UserId, pageNumber = 1 });
        }

        [HttpPost("chat/{chatId}/addUser/{userId}/senderId/{senderId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
        public ActionResult AddUser([FromRoute] int chatId, [FromRoute] int userId, [FromRoute] int senderId)
        {

            _chatService.AddUserById(userId, chatId);
            return RedirectToAction("GetUsersInChat","Chat",new {chatId= chatId, userId=senderId,pageNumber=1});
        }
        [HttpGet("chat/{chatId}/GetAddUserToChat/adminId/{userId}")]
        public ActionResult GetAddUserToChat([FromRoute]int chatId,[FromRoute] int userId)
        {
            ViewBag.chatId = chatId;
            ViewBag.userId = userId;

            return View("ChatAddUser");
        }
        [HttpGet]
        public ActionResult FindUser(AddUserToChat user)
        {
            if (!ModelState.IsValid)
            {
                return View("ChatAddUser", user);
            }
            user.userDto =_chatService.GetUserByEmail(user.Email);
            return View("ChatAddUser",user);
        }


        [HttpPost("chat/{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id, [FromRoute] int userId)
        {
            _chatService.DeleteUserById(userId, id);
            return RedirectToAction("GetChats", "User", new { id = userId, pageNumber = 1 });

        }

        [HttpPost("chat/{chatId}-{senderId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
        public ActionResult DeleteChat([FromRoute] int chatId, [FromRoute] int senderId)
        {
            _chatService.DeleteChat(chatId);
            return RedirectToAction("GetChats", "User", new { id = senderId, pageNumber = 1 });
        }

        [HttpGet("chat/getUsers/{chatId}/{pageNumber}/{userId}")]
        public ActionResult<ICollection<ChatAndUsers>> GetUsersInChat([FromRoute]int chatId, [FromRoute] int userId, [FromRoute] int pageNumber)
        {
            var usersPerPage = 5;
            var users = _chatService.GetUsersInChat(chatId);
            var role = _chatService.GetUserRole(userId,chatId);
            var chatsAndUsers = new ChatAndUsers
            {
                ChatId = chatId,
                Users = users.ToList(),
                Count = users.Count(),
                UsersPerPage = usersPerPage,
                PageNumber = pageNumber,
                UserRole = role,
                UserId = userId

            };

            return View("ChatGetUsers",chatsAndUsers);
        }

    }
}
