using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Models.ChatDTO;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Web.Models.UserDTOs;
using System.Linq;
using WebApi.Models.MessagesDTO;

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

            var usersDto = chat.UserChats.Select(uc => new UserDTO
            {
                Id = uc.User.Id,
                Email = uc.User.Email,
                Username = uc.User.Username,
                LastLog = uc.User.LastLog,
                Created = uc.User.Created,
            }).ToList();

            var textMessagesDto = chat.Messages
               .OfType<TextMessage>()
               .Select(m => new TextMessageDTO
               {
                   Id = m.Id,
                   ChatId = m.ChatId,
                   Content = m.Content,
                   SenderId = m.SenderId,
               })
               .ToList<MessageDTO>();

            var chatDto = new GetChatDTO()
            {
                ChatId = chat.Id,
                Name = chat.Name,
                Users = usersDto,
                AllMessages = textMessagesDto
            };

            return chatDto;
        }

        // POST api/<ChatController>
        [HttpPost("create")]
        public void Create([FromBody] CreateChatDTO value)
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
