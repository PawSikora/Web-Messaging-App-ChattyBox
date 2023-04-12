using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

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

        [HttpPost("TextMessage/Delete/{chatId}/{messageId}/{senderId}")]
        [TypeFilter(typeof(RolesAuthorization), Arguments = new object[] { "Admin" })]
		public ActionResult Delete([FromRoute] int chatId, [FromRoute] int messageId, [FromRoute] int senderId)
        {
            _textMessageService.DeleteTextMessage(messageId);
            return RedirectToAction("Get", "Chat", new { userId = senderId, chatId = chatId, pageNumber = 1 });
        }

        [HttpGet("txtMsg/GetNewest/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return View(_textMessageService.GetLastTextMessage(idChat));
        }
    }
}
