using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Linq;
using WebApi.Models.ChatDtos;
using Web.Models.UserDtos;
using WebApi.Models.MessagesDtos;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ChatController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET api/<ChatController>/5
        [HttpGet("{id}/{pageNumber}")]
        public ActionResult<GetChatDTO> Get([FromRoute] int id, [FromRoute] int pageNumber)
        {

            Chat chat = _unitOfWork.Chats.GetChat(id,pageNumber);

            if (chat == null)
            {
                return NotFound();
            }

            var chatDto = _mapper.Map<GetChatDTO>(chat);

            return Ok(chatDto);
          
        }


        // POST api/<ChatController>
        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateChatDTO chat)
        {
            _unitOfWork.Chats.CreateChat(chat.Name, chat.UserId);
            _unitOfWork.Save();
            return Ok();
        }
        
        [HttpPut("{id}/addUser/{userId}")]
        public ActionResult AddUser([FromRoute] int id, [FromRoute] int userId)
        {
            _unitOfWork.Chats.AddUserById(userId, id);
            _unitOfWork.Save();
            return Ok();
        }

        // PUT api/<ChatController>/5
        [HttpPut("{id}/deleteUser/{userId}")]
        public ActionResult DeleteUser([FromRoute] int id, [FromRoute] int userId)
        {
            _unitOfWork.Chats.DeleteUserById(userId, id);
            _unitOfWork.Save();
            return Ok();
        }

        // DELETE api/<ChatController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _unitOfWork.Chats.DeleteChat(id);
            _unitOfWork.Save();
            return Ok();
        }

    }
}
