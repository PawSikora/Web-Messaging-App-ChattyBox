using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.MessagesDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileMessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileMessageController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        // GET: api/<FileMessageController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FileMessageController>/5
        [HttpGet("{id}")]
        public FileMessageDTO Get(int id)
        {
            var file = _unitOfWork.FileMessages.GetFileMessage(id);
            return new FileMessageDTO
            {
                Id = file.Id,
                ChatId = file.ChatId,
                SenderId = file.SenderId,
                TimeStamp = file.TimeStamp,
                Name = file.Name,
                Path = file.Path,
                
            };

        }

        // POST api/<FileMessageController>
        [HttpPost("create")]
        public void Post([FromBody] CreateFileMessageDTO createFile)
        {

            _unitOfWork.FileMessages.CreateFileMessage(createFile.SenderId, createFile.Path, createFile.ChatId);
            _unitOfWork.Save();
            

        }

        //// PUT api/<FileMessageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<FileMessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.FileMessages.DeleteFileMessage(id);
            _unitOfWork.Save();
            
        }
        [HttpGet("GetNewest/{idChat}")]
        public GetNewestMessage GetNewestMessage(int idChat)
        {
            var message = _unitOfWork.FileMessages.GetLastTextMessage(idChat);
            return new GetNewestMessage()
            {
                Content = message.Name,
                SenderId = message.SenderId,
                SenderName = message.Sender.Username,

            };



        }
    }
}
