using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCWebApp.Controllers
{
    [Authorize]
    public class TextMessageController : Controller
    {
        private readonly ITextMessageService _textMessageService;

        public TextMessageController(ITextMessageService textMessageService)
        {
            _textMessageService = textMessageService;
        }

        [HttpGet("txtMsg/{id}")]
        public ActionResult<TextMessageDTO> Get(int id)
        {
            return View(_textMessageService.GetTextMessage(id));
        }


        [HttpPost]
        public ActionResult Create(CreateTextMessageDTO messageDTO)
        {
            if (ModelState.IsValid)
                _textMessageService.CreateTextMessage(messageDTO);
             
            return RedirectToAction("Get", "Chat", new { chatId = messageDTO.ChatId, pageNumber = 1 });
        }

        [HttpPost]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
		public ActionResult Delete(int chatId, int messageId)
        {
            _textMessageService.DeleteTextMessage(messageId);
            return RedirectToAction("Get", "Chat", new { chatId, pageNumber = 1 });
        }

        [HttpGet]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage(int idChat)
        {
            return View(_textMessageService.GetLastTextMessage(idChat));
        }
    }
}
