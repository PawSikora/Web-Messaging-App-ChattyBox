using AutoMapper;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.MessagesDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextMessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TextMessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<TextMessageController>
        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET api/<TextMessageController>/5
        [HttpGet("{id}")]
        public ActionResult<TextMessageDTO> Get(int id)
        {
            var message = _unitOfWork.TextMessages.GetTextMessage(id);
         
            if (message == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<TextMessageDTO>(message));
        }

        // POST api/<TextMessageController>
        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateTextMessageDTO messageDTO)
        {
            _unitOfWork.TextMessages.CreateTextMessage(messageDTO.SenderId, messageDTO.Content, messageDTO.ChatId);
            _unitOfWork.Save();
            return Ok();
        }

        // PUT api/<TextMessageController>/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }*/

        // DELETE api/<TextMessageController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _unitOfWork.TextMessages.DeleteTextMessage(id);
            _unitOfWork.Save();
            return Ok();
        }
        
        [HttpGet("GetNewestTextMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage(int idChat)
        {
            var message = _unitOfWork.TextMessages.GetLastTextMessage(idChat);
           
            if (message == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<GetNewestMessageDTO>(message));
        }
    }
}
