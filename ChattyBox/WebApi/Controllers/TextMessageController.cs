using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.TextMessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public ActionResult Create([FromForm] CreateTextMessageDTO messageDTO)
        {
            if (!ModelState.IsValid)
                BadRequest("Blad tworzenia wiadomosci tekstowej!");

            _textMessageService.CreateTextMessage(messageDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _textMessageService.DeleteTextMessage(id);
            return Ok();
        }

        [HttpGet("GetNewest/{id}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int id)
        {
            return Ok(_textMessageService.GetLastTextMessage(id));
        }
    }
}
