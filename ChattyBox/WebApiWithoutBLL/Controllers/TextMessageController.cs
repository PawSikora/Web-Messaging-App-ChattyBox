using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Exceptions;
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
            var message = _unitOfWork.TextMessages.GetById(id);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return View(_mapper.Map<TextMessageDTO>(message));
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateTextMessageDTO dto)
        {
            var sender = _unitOfWork.Users.GetById(dto.SenderId);

            if (sender is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            var chat = _unitOfWork.Chats.GetById(dto.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            //TextMessage message = new TextMessage
            //{
            //    Content = dto.Content,
            //    SenderId = dto.SenderId,
            //    ChatId = dto.ChatId,
            //    TimeStamp = DateTime.Now
            //};

            var message = _mapper.Map<TextMessage>(dto);

            _unitOfWork.TextMessages.CreateTextMessage(message);
            _unitOfWork.Save();

            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var message = _unitOfWork.TextMessages.GetById(id);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            _unitOfWork.TextMessages.DeleteTextMessage(message);
            _unitOfWork.Save();

            return View();
        }

        [HttpGet("GetNewestTextMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var message = _unitOfWork.TextMessages.GetLastTextMessage(chatId);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return View(_mapper.Map<GetNewestMessageDTO>(message));
        }
    }
}
    
