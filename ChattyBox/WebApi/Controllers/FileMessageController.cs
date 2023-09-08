using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.FileMessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileMessageController : ControllerBase
    {
        private readonly IFileMessageService _fileMessageService;

        public FileMessageController(IFileMessageService fileMessageService)
        {
            _fileMessageService = fileMessageService;
        }

        [HttpGet("{id}")]
        public ActionResult<FileMessageDTO> Get([FromRoute] int id)
        {
            return Ok(_fileMessageService.GetFileMessage(id));
        }

        [HttpPost]
        public ActionResult Create([FromForm] CreateFileMessageDTO createFile)
        {
            if (!ModelState.IsValid)
                return BadRequest("Błąd dodawania pliku!");

            createFile.Name = createFile.File.FileName;
            _fileMessageService.CreateFileMessage(createFile);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _fileMessageService.DeleteFileMessage(id);
           return Ok();
        }

        [HttpGet("GetNewest/{id}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int id)
        {
            return Ok(_fileMessageService.GetLastFileMessage(id));
        }
    }
}
