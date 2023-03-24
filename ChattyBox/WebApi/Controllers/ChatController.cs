using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Models.ChatDTO;
using WebApi.Models.TextMessageDTO;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<ChatController>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET api/<ChatController>/5
        [HttpGet("{id}")]
        public GetChatDTO Get(int id)
        {
            var chat = _unitOfWork.Chats.GetChat(id);
            /*ICollection<User> users = new List<User>();

            foreach (var user in chat.UserChats)
            {
                users.Add(user.User);
            }*/

            var chatDto = new GetChatDTO()
            {
                ChatId = chat.Id,
                Name = chat.Name
            };
            return chatDto;
        }

        /*[HttpGet("{chatId}")]
        public IActionResult GetChatData(int chatId)
        {
            var chatData = _unitOfWork.Chats.GetChat(chatId);

            if (chatData == null)
            {
                return NotFound();
            }

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            //return Json(chatData, jsonSettings);
        }*/

        // POST api/<ChatController>
        [HttpPost]
        public void Post([FromBody] CreateChatDTO value)
        {
            _unitOfWork.Chats.CreateChat(value.Name, value.UserId);
            _unitOfWork.Save();
        }

        // PUT api/<ChatController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<ChatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.Chats.DeleteChat(id);
            _unitOfWork.Save();
        }
    }
}
