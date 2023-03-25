using AutoMapper;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.MessagesDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileMessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileMessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        // GET: api/<FileMessageController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<FileMessageController>/5
        [HttpGet("{id}")]
        public ActionResult<FileMessageDTO> Get(int id)
        {
            var file = _unitOfWork.FileMessages.GetFileMessage(id);
           
            if (file == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<FileMessageDTO>(file));

        }

        // POST api/<FileMessageController>
        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateFileMessageDTO createFile)
        {
            _unitOfWork.FileMessages.CreateFileMessage(createFile.SenderId, createFile.Path, createFile.ChatId);
            _unitOfWork.Save();
            return Ok();
        }

        //// PUT api/<FileMessageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<FileMessageController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _unitOfWork.FileMessages.DeleteFileMessage(id);
            _unitOfWork.Save();
            return Ok();
        }
        
        [HttpGet("GetNewestFileMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage(int idChat)
        {
            var message = _unitOfWork.FileMessages.GetLastFileMessage(idChat);
           
            if (message == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<GetNewestMessageDTO>(message));


        }
    }
}
