using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.TextMessageDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextMessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TextMessageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<TextMessageController>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET api/<TextMessageController>/5
        [HttpGet("{id}")]
        public CreateTextMessageDTO Get(int id)
        {
            var message = _unitOfWork.TextMessages.GetTextMessage(id);
            var messageDTO = new CreateTextMessageDTO
            {
                ChatId = message.ChatId,
                Content = message.Content,
                SenderId = message.SenderId,
            };
            return messageDTO;
        }

        // POST api/<TextMessageController>
        [HttpPost]
        public void Post([FromBody] CreateTextMessageDTO messageDTO)
        {
            _unitOfWork.TextMessages.CreateTextMessage(messageDTO.SenderId, messageDTO.Content, messageDTO.ChatId);
            _unitOfWork.Save();
        }

        // PUT api/<TextMessageController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<TextMessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.TextMessages.DeleteTextMessage(id);
            _unitOfWork.Save();
        }
    }
}
