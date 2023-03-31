using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.FileMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class FileMessageController : Controller
    {
        private readonly IFileMessageService _fileMessageService;

        public FileMessageController(IFileMessageService fileMessageService)
        {
            _fileMessageService = fileMessageService;
        }

        [HttpGet("{id}")]
        public ActionResult<FileMessageDTO> Get([FromRoute] int id)
        {
            return View(_fileMessageService.GetFileMessage(id));
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateFileMessageDTO createFile)
        {
            _fileMessageService.CreateFileMessage(createFile);
            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _fileMessageService.DeleteFileMessage(id);
            return View();
        }

        [HttpGet("GetNewestFileMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return View(_fileMessageService.GetLastFileMessage(idChat));
        }
    }
}
