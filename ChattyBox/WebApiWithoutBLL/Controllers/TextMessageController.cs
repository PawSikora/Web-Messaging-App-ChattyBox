using AutoMapper;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Models.MessagesDtos;

namespace WebApiWithoutBLL.Controllers
{
    public class TextMessageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TextMessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<TextMessageDTO> Get([FromRoute] int id)
        {
            var message = _unitOfWork.TextMessages.GetTextMessage(id);

            if (message == null)
            {
                return View("Error");
            }

            return View(_mapper.Map<TextMessageDTO>(message));
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateTextMessageDTO messageDTO)
        {
            _unitOfWork.TextMessages.CreateTextMessage(messageDTO.SenderId, messageDTO.Content, messageDTO.ChatId);
            _unitOfWork.Save();
            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _unitOfWork.TextMessages.DeleteTextMessage(id);
            _unitOfWork.Save();
            return View();
        }

        [HttpGet("GetNewestTextMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            var message = _unitOfWork.TextMessages.GetLastTextMessage(idChat);

            if (message == null)
            {
                return View("Error");
            }

            return View(_mapper.Map<GetNewestMessageDTO>(message));
        }
    }
}
    
