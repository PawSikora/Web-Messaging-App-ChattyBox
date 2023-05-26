using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.FileMessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class FileMessageController : Controller
    {
        private readonly IFileMessageService _fileMessageService;

        public FileMessageController(IFileMessageService fileMessageService)
        {
            _fileMessageService = fileMessageService;
        }

        [HttpGet("fMsg/{id}")]
        public ActionResult<FileMessageDTO> Get([FromRoute] int id)
        {
            return View(_fileMessageService.GetFileMessage(id));
        }

        [HttpPost]
        public ActionResult Create(CreateFileMessageDTO createFile)
        {
            if (ModelState.IsValid)
            {
                createFile.Name = createFile.File.FileName;
                _fileMessageService.CreateFileMessage(createFile);
            }

            return RedirectToAction("Get", "Chat", new { chatId = createFile.ChatId, pageNumber = 1 });
        }

		[HttpPost("FileMessage/Delete/{chatId}/{messageId}")]
		[TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
		public ActionResult Delete([FromRoute] int chatId, [FromRoute] int messageId)
		{
            _fileMessageService.DeleteFileMessage(messageId);
            return RedirectToAction("Get", "Chat", new { chatId, pageNumber = 1 });
        }

		[HttpGet("fMsg/GetNewest/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return View(_fileMessageService.GetLastFileMessage(idChat));
        }
    }
}
