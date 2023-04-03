using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TextMessageController : Controller
    {
        private readonly ITextMessageService _textMessageService;

        public TextMessageController(ITextMessageService textMessageService)
        {
            _textMessageService = textMessageService;
        }

        [HttpGet("txtMsg/{id}")]
        public ActionResult<TextMessageDTO> Get([FromRoute] int id)
        {
            return View(_textMessageService.GetTextMessage(id));
        }

        [HttpPost]
        public ActionResult Create( CreateTextMessageDTO messageDTO)
        {
            if (!ModelState.IsValid) return View("Create",messageDTO);
            _textMessageService.CreateTextMessage(messageDTO);
            return RedirectToAction("Get", "Chat", new { userId=messageDTO.SenderId,chatId = messageDTO.ChatId, pageNumber = 1 });
        }
        [HttpGet]
        public ActionResult Create(int id,int senderId)
        {
            ViewBag.ChatId = id;
            ViewBag.SenderId = senderId;
            
            return View();
           

        }

        [HttpDelete("txtMsg/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _textMessageService.DeleteTextMessage(id);
            return View();
        }

        [HttpGet("txtMsg/GetNewest/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return View(_textMessageService.GetLastTextMessage(idChat));
        }
    }
}
