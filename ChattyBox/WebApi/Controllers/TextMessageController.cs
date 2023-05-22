using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextMessageController : ControllerBase
    {
        private readonly ITextMessageService _textMessageService;

        public TextMessageController(ITextMessageService textMessageService)
        {
            _textMessageService = textMessageService;
        }

        [HttpGet("{id}")]
        public ActionResult<TextMessageDTO> Get([FromRoute] int id)
        {
            return Ok(_textMessageService.GetTextMessage(id));
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateTextMessageDTO messageDTO)
        {
            if (!ModelState.IsValid)
                BadRequest("Blad tworzenia wiadomosci tekstowej!");

            _textMessageService.CreateTextMessage(messageDTO);
            return Ok();
        }

        [HttpDelete("{messageId}")]
        public ActionResult Delete([FromRoute] int messageId)
        {
            _textMessageService.DeleteTextMessage(messageId);
            return Ok();
        }

        [HttpGet("GetNewest/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return Ok(_textMessageService.GetLastTextMessage(idChat));
        }
    }
}
